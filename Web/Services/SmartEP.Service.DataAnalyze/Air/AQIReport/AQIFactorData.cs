namespace SmartEP.Service.DataAnalyze.AQIReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class AQIFactorData
    {
        #region Fields

        private DateTime time;
        private string _factorName;
        private decimal? _factorValue;
        private int _portId;
        private List<AQIDataType> _qQIDataType;
        private Enums.ReportType _reportType;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 因子参与计算的种类，如：O3在AQI日报中有最大1小时平均和最大8小时平均
        /// </summary>
        public List<AQIDataType> AQIDataTypes
        {
            get { return _qQIDataType; }
            set { _qQIDataType = value; }
        }

        public string FactorName
        {
            get { return _factorName; }
            set { _factorName = value; }
        }

        public decimal? FactorValue
        {
            get { return _factorValue; }
            set { _factorValue = value; }
        }

        public int PortId
        {
            get { return _portId; }
            set { _portId = value; }
        }

        /// <summary>
        /// 数据源的用途：小时报或日报
        /// </summary>
        public Enums.ReportType ReportType
        {
            get { return _reportType; }
            set { _reportType = value; }
        }

        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        #endregion Properties
    }
}