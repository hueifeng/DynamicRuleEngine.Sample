namespace DynamicRuleEngine.Sample
{
    public interface IRuleEngine<T>
    {
        /// <summary>
        /// 验证是否符合升级条件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Validate(T data);
        /// <summary>
        /// 计算升级后的积分奖励
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        decimal Calculate(T data);
    }
}
