namespace SmartEP.Service.DataAnalyze.PollutionIndex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PollutionIndexCaculatorContext
    {
        #region Fields

        private IPollutionIndexCaculator pollutionIndexCaculator;
        private DataAnalyze.Enums.PollutionIndexType pollutionIndexType;

        #endregion Fields

        #region Constructors

        public PollutionIndexCaculatorContext()
        {
            //默认创建AQI
            PollutionIndex_Type = DataAnalyze.Enums.PollutionIndexType.AQI;
            pollutionIndexCaculator = new DataAnalyze.PollutionIndex.PollutionIndexCaculator();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 污染指数的类型
        /// </summary>
        public DataAnalyze.Enums.PollutionIndexType PollutionIndex_Type
        {
            get
            {
                return this.pollutionIndexType;
            }
            set
            {
                this.pollutionIndexType = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 添加因子信息
        /// </summary>
        /// <param name="factorName">因子名称</param>
        /// <param name="caculatorType">因子计算的时间类型</param>
        /// <param name="factorConcentration">因子的浓度</param>
        /// <param name="pollutionIndexType">污染指数的类型</param>
        public void AddFactor(string factorName, Enums.CaculatorType caculatorType, decimal? factorConcentration, Enums.PollutionIndexType pollutionIndexType)
        {
            pollutionIndexCaculator.AddFactor(factorName, caculatorType, factorConcentration, pollutionIndexType);
        }

        /// <summary>
        /// 添加因子信息
        /// </summary>
        /// <param name="factorName">因子名称</param>
        /// <param name="caculatorType">因子计算的时间类型</param>
        /// <param name="factorConcentration">因子的浓度</param>
        public void AddFactor(string factorName, Enums.CaculatorType caculatorType, decimal? factorConcentration)
        {
            this.AddFactor(factorName, caculatorType, factorConcentration, PollutionIndex_Type);
        }

        /// <summary>
        /// 清空所有的因子信息
        /// </summary>
        public void Clear()
        {
            pollutionIndexCaculator.Clear();
        }

        /// <summary>
        /// 清空因子信息
        /// </summary>
        /// <param name="pollutionIndexType">污染指数的类型</param>
        public void Clear(Enums.PollutionIndexType pollutionIndexType)
        {
            pollutionIndexCaculator.Clear(pollutionIndexType);
        }

        /// <summary>
        /// 获得所有因子污染指数及相关信息
        /// </summary>
        /// <param name="pollutionIndexType">污染指数的类型</param>
        /// <returns></returns>
        public System.Collections.Generic.List<PollutionLevelInfo> GetALLFactorPollutionLevelInfos(Enums.PollutionIndexType pollutionIndexType)
        {
            return pollutionIndexCaculator.GetALLFactorPollutionLevelInfos(pollutionIndexType);
        }

        /// <summary>
        /// 获得所有因子污染指数及相关信息
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.List<PollutionLevelInfo> GetALLFactorPollutionLevelInfos()
        {
            return this.GetALLFactorPollutionLevelInfos(PollutionIndex_Type);
        }

        public PollutionLevelInfo GetPollutionLevelInfo(string factorName, Enums.CaculatorType caculatorType, Enums.PollutionIndexType pollutionIndexType)
        {
            return pollutionIndexCaculator.GetPollutionLevelInfo(factorName, caculatorType, pollutionIndexType);
        }

        /// <summary>
        /// 获得首要污染物的信息
        /// </summary>
        /// <param name="pollutionIndexType">污染指数的类型</param>
        /// <returns></returns>
        public System.Collections.Generic.List<PollutionLevelInfo> GetPrimaryPollutions(Enums.PollutionIndexType pollutionIndexType)
        {
            return pollutionIndexCaculator.GetPrimaryPollutions(pollutionIndexType);
        }

        /// <summary>
        /// 获得首要污染物的信息
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.List<PollutionLevelInfo> GetPrimaryPollutions()
        {
            return this.GetPrimaryPollutions(PollutionIndex_Type);
        }

        /// <summary>
        /// 删除因子信息
        /// </summary>
        /// <param name="factorName">因子名称</param>
        /// <param name="caculatorType">因子计算的时间类型</param>
        /// <param name="pollutionIndexType">污染指数的类型</param>
        public void RemoveFactor(string factorName, Enums.CaculatorType caculatorType, Enums.PollutionIndexType pollutionIndexType)
        {
            pollutionIndexCaculator.RemoveFactor(factorName, caculatorType, pollutionIndexType);
        }

        /// <summary>
        /// 删除因子信息
        /// </summary>
        /// <param name="factorName">因子名称</param>
        /// <param name="caculatorType">因子计算的时间类型</param>
        public void RemoveFactor(string factorName, Enums.CaculatorType caculatorType)
        {
            this.RemoveFactor(factorName, caculatorType, PollutionIndex_Type);
        }

        #endregion Methods
    }
}