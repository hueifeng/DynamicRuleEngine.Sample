using System.Reflection.Emit;

namespace DynamicRuleEngine.Sample
{
    public static class VipRuleIL
    {
        // 验证逻辑：订单≥5且最近购买≤30天
        public static void BuildValidateIL(ILGenerator il)
        {
            var successLabel = il.DefineLabel();
            var failLabel = il.DefineLabel();

            // 检查TotalOrders >= 5
            il.Emit(OpCodes.Ldarg_1); // 加载CustomerData参数
            il.Emit(OpCodes.Callvirt, typeof(CustomerData).GetProperty("TotalOrders").GetGetMethod());
            il.Emit(OpCodes.Ldc_I4_5);
            il.Emit(OpCodes.Blt, failLabel); // 小于5跳转失败

            // 检查LastPurchaseDays <= 30
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, typeof(CustomerData).GetProperty("LastPurchaseDays").GetGetMethod());
            il.Emit(OpCodes.Ldc_I4_S, 30);
            il.Emit(OpCodes.Bgt, failLabel); // 大于30跳转失败

            // 所有条件通过，跳转到成功分支
            il.Emit(OpCodes.Br, successLabel);

            // 成功分支
            il.MarkLabel(successLabel);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Ret);

            // 失败分支
            il.MarkLabel(failLabel);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ret);
        }

        // 计算逻辑：(总金额 × 0.1) + (当前等级 × 100)
        public static void BuildCalculateIL(ILGenerator il)
        {
            // 返回默认值0
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Conv_R8);
            il.Emit(OpCodes.Call, typeof(decimal).GetMethod("op_Explicit", new Type[] { typeof(double) }));
            il.Emit(OpCodes.Ret);
        }
    }
}
