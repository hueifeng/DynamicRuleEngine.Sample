namespace DynamicRuleEngine.Sample
{
    internal class Program
    {
        public static void Main()
        {
            // 1. 生成VIP规则程序集
            var generator = new DynamicRuleGenerator();
            generator.BuildAndSaveRule(
                "VipUpgrade",
                VipRuleIL.BuildValidateIL,
                VipRuleIL.BuildCalculateIL
            );

            // 2. 加载规则引擎
            using var host = new RuleEngineHost();
            var engine = host.LoadRule("VipUpgrade.dll");

            // 3. 准备测试数据
            var customer = new CustomerData
            {
                TotalOrders = 8,
                LastPurchaseDays = 15,
                TotalSpent = 5000,
                CurrentLevel = 2
            };

            // 4. 执行规则
            if (engine.Validate(customer))
            {
                var reward = engine.Calculate(customer);
                Console.WriteLine($"升级成功！获得奖励积分: {reward:F0}");
            }
            else
            {
                Console.WriteLine("不符合升级条件");
            }
        }
    }
}
