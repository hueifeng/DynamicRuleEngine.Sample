using System.Reflection;
using System.Reflection.Emit;

namespace DynamicRuleEngine.Sample
{
    public class DynamicRuleGenerator
    {
        private readonly Dictionary<string, Type> _loadedRules = new();

        /// <summary>
        /// 动态生成并持久化会员规则引擎
        /// </summary>
        /// <param name="ruleName">规则唯一标识</param>
        /// <param name="validateLogic">验证逻辑IL生成器</param>
        /// <param name="calculateLogic">计算逻辑IL生成器</param>
        public void BuildAndSaveRule(
            string ruleName,
            Action<ILGenerator> validateLogic,
            Action<ILGenerator> calculateLogic)
        {
            // 1. 创建持久化程序集构建器
            var assemblyName = new AssemblyName($"DynamicRules.{ruleName}");
            var assemblyBuilder = new PersistedAssemblyBuilder(
                assemblyName,
                typeof(object).Assembly); // 支持卸载

            // 2. 定义动态模块
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

            // 3. 创建实现IRuleEngine的类型
            var typeBuilder = moduleBuilder.DefineType(
                $"{ruleName}RuleEngine",
                TypeAttributes.Public | TypeAttributes.Class,
                null,
                new[] { typeof(IRuleEngine<CustomerData>) });

            // 4. 生成Validate方法
            BuildMethod(
                typeBuilder,
                nameof(IRuleEngine<CustomerData>.Validate),
                typeof(bool),
                validateLogic);

            // 5. 生成Calculate方法
            BuildMethod(
                typeBuilder,
                nameof(IRuleEngine<CustomerData>.Calculate),
                typeof(decimal),
                calculateLogic);

            // 6. 完成类型构建
            var dynamicType = typeBuilder.CreateType();

            // 7. 持久化到磁盘
            var dllPath = Path.Combine(AppContext.BaseDirectory, $"{ruleName}.dll");
            using var fs = new FileStream(dllPath, FileMode.Create);
            assemblyBuilder.Save(fs);

            _loadedRules[ruleName] = dynamicType;
        }

        private static void BuildMethod(
            TypeBuilder typeBuilder,
            string methodName,
            Type returnType,
            Action<ILGenerator> ilGenerator)
        {
            var methodBuilder = typeBuilder.DefineMethod(
                methodName,
                MethodAttributes.Public | MethodAttributes.Virtual,
                returnType,
                new[] { typeof(CustomerData) });

            var il = methodBuilder.GetILGenerator();
            ilGenerator(il); // 注入IL逻辑
            il.Emit(OpCodes.Ret);

            // 显式实现接口方法
            typeBuilder.DefineMethodOverride(
                methodBuilder,
                typeof(IRuleEngine<CustomerData>).GetMethod(methodName));
        }
    }
}
