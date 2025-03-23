namespace DynamicRuleEngine.Sample
{
    public class CustomerData
    {
        /// <summary>
        /// 累计订单数
        /// </summary>
        public int TotalOrders { get; set; }
        /// <summary>
        /// 最近购买天数差
        /// </summary>
        public int LastPurchaseDays { get; set; }
        /// <summary>
        /// 累计消费金额
        /// </summary>
        public decimal TotalSpent { get; set; }
        /// <summary>
        /// 当前会员等级
        /// </summary>
        public int CurrentLevel { get; set; }  
    }
}
