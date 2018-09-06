namespace SmartEP.Service.DataAnalyze.WaterPollutionIndex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class WQIFactorMapping
    {
        #region Fields

        private decimal? _decFactorValue;
        private DateTime _dtmTstamp;
        private string _strPortId;
        private Enums.WaterPollutionFactor _waterPollutionFactor;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 因子名称
        /// </summary>
        public Enums.WaterPollutionFactor FactorName
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