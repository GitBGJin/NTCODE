namespace SmartEP.Service.DataAnalyze.AQIReport
{
    using System.Collections.Generic;
    using SmartEP.Service;
    /// <summary>
    /// 用于AQI与数据库字段映射表
    /// </summary>
    public class AQIFactorMapping
    {
        #region Fields

        private List<DataAnalyze.AQIReport.AQIDataType> _aqiDataTypes;
        private string _factorName;
        private string _mappingColumn;
        private DataAnalyze.Enums.ReportType _reportType;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 类据类型
        /// </summary>
        public List<DataAnalyze.AQIReport.AQIDataType> AQIDataTypes
        {
            get { return _aqiDataTypes; }
            set { _aqiDataTypes = value; }
        }

        /// <summary>
        /// 因子名称
        /// </summary>
        public string FactorName
        {
            get { return _factorName; }
            set { _factorName = value; }
        }

        /// <summary>
        /// 映射的列名
        /// </summary>
        public string MappingColumn
        {
            get { return _mappingColumn; }
            set { _mappingColumn = value; }
        }

        /// <summary>
        /// 报表类型
        /// </summary>
        public DataAnalyze.Enums.ReportType ReportType
        {
            get { return _reportType; }
            set { _reportType = value; }
        }

        #endregion Properties
    }
}