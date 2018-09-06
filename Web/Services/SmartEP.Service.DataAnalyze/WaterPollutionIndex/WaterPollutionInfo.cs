namespace SmartEP.Service.DataAnalyze.WaterPollutionIndex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class WaterPollutionInfo
    {
        #region Fields

        private decimal? _decFactorValue;
        private DateTime _dtmTstamp;
        private int _intPollutionLevelOrderNo;
        private string _strPollutionLevelName;
        private string _strPortId;
        private string _waterPollutionFactor;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 因子名称
        /// </summary>
        public string FactorName
        {
            get { return _waterPollutionFactor; }
            set { _waterPollutionFactor = value; }
        }

        /// <summary>
        /// 因子值
        /// </summary>
        public decimal? FactorValue
        {
            get { return _decFactorValue; }
            set { _decFactorValue = value; }
        }

        /// <summary>
        /// 污染等级名称
        /// </summary>
        public string PollutionLevelName
        {
            get { return _strPollutionLevelName; }
            set { _strPollutionLevelName = value; }
        }

        /// <summary>
        /// 污染等级的顺序
        /// </summary>
        public int PollutionLevelOrderNo
        {
            get { return _intPollutionLevelOrderNo; }
            set { _intPollutionLevelOrderNo = value; }
        }

        /// <summary>
        /// 站点唯一标识
        /// </summary>
        public string PortId
        {
            get { return _strPortId; }
            set { _strPortId = value; }
        }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Tstamp
        {
            get { return _dtmTstamp; }
            set { _dtmTstamp = value; }
        }

        #endregion Properties
    }
}