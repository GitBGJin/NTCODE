namespace SmartEP.Service.DataAnalyze.WaterPollutionIndex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class WQIReport
    {
        #region Fields

        private IList<WaterPollutionInfo> _lstWaterPollutionInfo;

        #endregion Fields

        #region Constructors

        public WQIReport()
        {
        }

        public WQIReport(IList<WQIFactorMapping> lstWQIFactors)
        {
            AddWQIFactor(lstWQIFactors);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 只读属性：水污染因子的污染信息
        /// </summary>
        public IList<WaterPollutionInfo> WaterPollutionInfo
        {
            get
            {
                if (_lstWaterPollutionInfo == null)
                    _lstWaterPollutionInfo = new List<WaterPollutionInfo>();
                return _lstWaterPollutionInfo;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 添加水污染物因子
        /// </summary>
        /// <param name="wqiFactorMapping">因子数据</param>
        public void AddWQIFactor(WQIFactorMapping wqiFactorMapping)
        {
            AddWaterPollutionInfo(wqiFactorMapping);
        }

        /// <summary>
        /// 添加水污染物因子列表
        /// </summary>
        /// <param name="lstWQIFactors">因子数据列表</param>
        public void AddWQIFactor(IList<WQIFactorMapping> lstWQIFactors)
        {
            foreach (WQIFactorMapping wqiFactorMapping in lstWQIFactors)
            {
                AddWQIFactor(wqiFactorMapping);
            }
        }

        /// <summary>
        /// 添加污染物等级信息
        /// </summary>
        /// <param name="wqiFactorMapping"></param>
        /// <returns></returns>
        private WaterPollutionInfo AddWaterPollutionInfo(WQIFactorMapping wqiFactorMapping)
        {
            if (_lstWaterPollutionInfo == null)
                _lstWaterPollutionInfo = new List<WaterPollutionInfo>();
            //获取污染等级
            string strPollutionLevel;
            //污染等级的显示的名称
            string strPollutionLevelName;
            //水因子显示的名称
            string strFactorCNName;
            WaterPollutionIndexLibrary waterPollutionIndexLibrary = new WaterPollutionIndexLibrary();
            strPollutionLevel = waterPollutionIndexLibrary.GetWaterPollutionLevel(wqiFactorMapping.FactorName, wqiFactorMapping.FactorValue);
            strPollutionLevelName = GetPollutionLevelName(strPollutionLevel);
            strFactorCNName = GetFactorCNName(wqiFactorMapping.FactorName.ToString());
            WaterPollutionInfo waterPollutionInfo;
            waterPollutionInfo = _lstWaterPollutionInfo.Where(x => x.Tstamp == wqiFactorMapping.Tstamp && x.PortId == wqiFactorMapping.PortId && x.FactorName == strFactorCNName).FirstOrDefault();
            if (WaterPollutionInfo != null)//存在则删除
            {
                _lstWaterPollutionInfo.Remove(waterPollutionInfo);
            }
            //添加
            waterPollutionInfo = new WaterPollutionInfo() { PortId = wqiFactorMapping.PortId, FactorName = strFactorCNName, FactorValue = wqiFactorMapping.FactorValue, Tstamp = wqiFactorMapping.Tstamp, PollutionLevelName = strPollutionLevelName, PollutionLevelOrderNo = GetPollutionLevelOrderNo(strPollutionLevel) };
            _lstWaterPollutionInfo.Add(waterPollutionInfo);
            return waterPollutionInfo;
        }

        /// <summary>
        /// 获取因子中文名称
        /// </summary>
        /// <param name="factorName"></param>
        /// <returns></returns>
        private string GetFactorCNName(string factorName)
        {
            if (factorName == null) return "--";
            IDictionary<string, string> dictionarys = new Dictionary<string, string>();
            dictionarys.Add("DissolvedOxygen", "溶解氧");
            dictionarys.Add("AmmonniaNitrogen", "氨氮");
            dictionarys.Add("PermanganateIndex", " 高锰酸盐指数");
            dictionarys.Add("PhosphorusTotal", "总磷");
            dictionarys.Add("PH", "PH值");
            return dictionarys.ContainsKey(factorName) ? dictionarys[factorName] : factorName;
        }

        /// <summary>
        /// 获取污染等级中文名称
        /// </summary>
        /// <param name="strPollutionLevel">污染等级名称</param>
        /// <returns></returns>
        private string GetPollutionLevelName(string strPollutionLevel)
        {
            if (strPollutionLevel == null) return "--";
            IDictionary<string, string> dictionarys = new Dictionary<string, string>();
            dictionarys.Add("One", "I类");
            dictionarys.Add("Two", "Ⅱ类");
            dictionarys.Add("Three", " Ⅲ类");
            dictionarys.Add("Four", "Ⅳ类");
            dictionarys.Add("Five", "Ⅴ类");
            dictionarys.Add("WeakFive", "劣Ⅴ类");
            return dictionarys.ContainsKey(strPollutionLevel) ? dictionarys[strPollutionLevel] : "--";
        }

        /// <summary>
        /// 获取污染物等级序号
        /// 原因：水因子的首要污染物是根据等级来划分的
        /// <param name="strPollutionLevelName">等级名称</param>
        /// <returns>当没有等级时，返回-1</returns>
        /// </summary>
        private int GetPollutionLevelOrderNo(string strPollutionLevelName)
        {
            try
            {
                return (int)(Enum.Parse(typeof(Enums.WaterPollutionLevelType), strPollutionLevelName));
            }
            catch
            {
                return -1;
            }
        }

        #endregion Methods
    }
}