namespace SmartEP.Service.DataAnalyze.PollutionIndex
{
    using System.Collections.Generic;
    using System.Linq;

    public class PollutionIndexCaculator : IPollutionIndexCaculator
    {
        #region Fields

        private System.Collections.Generic.List<PollutionLevelInfo> factors;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public PollutionIndexCaculator()
        {
            factors = new List<PollutionLevelInfo>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 添加参与污染物计算的因子信息
        /// </summary>
        /// <param name="factorName">因子的名称</param>
        /// <param name="caculatorType">计算的时间类型</param>
        /// <param name="factorConcentration">因子的浓度值</param>
        /// <param name="_override">如果已经存在,是否覆盖；默认值flase</param>
        public void AddFactor(string factorName, Enums.CaculatorType caculatorType, decimal? factorConcentration, Enums.PollutionIndexType pollutionIndexType)
        {
            PollutionLevelInfo pli;
               // if (factorConcentration == null)
            //{
              //  pli = new PollutionLevelInfo() { FactorName=factorName, Concentration=factorConcentration, PollutionIndex_Type=pollutionIndexType };
            //}
            //else {
                pli = PollutionIndexLibrary.GetPollutionLevelInfo(factorName, caculatorType, factorConcentration, pollutionIndexType);
               // }
            if (pli != null)
                factors.Add(pli);
        }

        /// <summary>
        ///清空
        /// </summary>
        public void Clear()
        {
            factors.Clear();
        }

        public void Clear(Enums.PollutionIndexType pollutionIndexType)
        {
            factors.RemoveAll(x => x.PollutionIndexType == pollutionIndexType);
        }

        /// <summary>
        /// 返回所有因子污染信息
        /// </summary>
        /// <returns></returns>
        public List<PollutionLevelInfo> GetALLFactorPollutionLevelInfos(Enums.PollutionIndexType pollutionIndexType)
        {
            return factors.Where(x => x.PollutionIndexType == pollutionIndexType).ToList();
        }

        public PollutionLevelInfo GetPollutionLevelInfo(string factorName, Enums.CaculatorType caculatorType, Enums.PollutionIndexType pollutionIndexType)
        {
            return factors.Where(x => x.FactorName == factorName && x.CaculatorType == caculatorType && x.PollutionIndexType == pollutionIndexType).LastOrDefault();
        }

        /// <summary>
        /// 获取首要污染物
        /// </summary>
        /// <returns>没有返回NULL</returns>
        public List<PollutionLevelInfo> GetPrimaryPollutions(Enums.PollutionIndexType pollutionIndexType)
        {
            var maxPollutionIndex = factors.Where(x => x.PollutionIndexType == pollutionIndexType).Max(x => x.PollutionIndex);
            if (!maxPollutionIndex.HasValue)
            {
                return null;
            }
            return factors.Where(x => x.PollutionIndexType == pollutionIndexType && x.PollutionIndex == maxPollutionIndex.GetValueOrDefault()).ToList();
        }

        /// <summary>
        /// 删除一个因子信息
        /// </summary>
        /// <param name="factorName"></param>
        /// <param name="caculatorType"></param>
        public void RemoveFactor(string factorName, Enums.CaculatorType caculatorType, Enums.PollutionIndexType pollutionIndexType)
        {
            //while (factors.Exists(x => x.FactorName == factorName && x.CaculType == caculatorType && x.PollutionIndex_Type == pollutionIndexType))
            //{
            //    factors.Remove(factors.Where(x => x.FactorName == factorName && x.CaculType == caculatorType && x.PollutionIndex_Type == pollutionIndexType).FirstOrDefault());
            //}
            factors.RemoveAll(x => x.FactorName == factorName && x.CaculatorType == caculatorType && x.PollutionIndexType == pollutionIndexType);
        }

        #endregion Methods
    }
}