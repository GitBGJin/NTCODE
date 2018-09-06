namespace SmartEP.Service.DataAnalyze.AQIReport
{
    using System;
    using System.Collections.Generic;

    public class FactorAQI
    {
        #region Fields

        private bool _blnIsPollutionIndexFactor;
        private Enums.CaculatorType _caculatorType;
        private decimal? _decFactorValue;
        private DateTime _dtmTime;
        private int? _intAQI;
        private int _intPortId;
        private Enums.PollutionIndexDataType _pollutionIndexDataType;
        private Enums.ReportType _reportType;
        private string _strColor;
        private string _strColorCode;
        private string _strFactorName;
        private string _strHealthEffect;
        private string _strLevel;
        private string _strLevelType;
        private string _strRoman;
        private string _strSuggestion;

        #endregion Fields

        #region Properties

        /// <summary>
        /// AQI值
        /// </summary>
        public int? AQI
        {
            get { return _intAQI; }
            set { _intAQI = value; }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public Enums.CaculatorType CaculatorType
        {
            get { return _caculatorType; }
            set { _caculatorType = value; }
        }

        /// <summary>
        /// 污染等级对应的颜色
        /// </summary>
        public string Color
        {
            get { return _strColor; }
            set { _strColor = value; }
        }

        /// <summary>
        /// 污染等级对应的颜色的代码
        /// </summary>
        public string ColorCode
        {
            get { return _strColorCode; }
            set { _strColorCode = value; }
        }

        /// <summary>
        /// 因子名称
        /// </summary>
        public string FactorName
        {
            get { return _strFactorName; }
            set { _strFactorName = value; }
        }

        /// <summary>
        /// 因子浓度
        /// </summary>
        public decimal? FactorValue
        {
            get { return _decFactorValue; }
            set { _decFactorValue = value; }
        }

        /// <summary>
        /// 对健康的影响
        /// </summary>
        public string HealthEffect
        {
            get { return _strHealthEffect; }
            set { _strHealthEffect = value; }
        }

        /// <summary>
        /// 是否是AQI标准中的因子
        /// </summary>
        public bool IsPollutionIndexFactor
        {
            get { return _blnIsPollutionIndexFactor; }
            set { _blnIsPollutionIndexFactor = value; }
        }

        /// <summary>
        /// 污染等级
        /// </summary>
        public string Level
        {
            get { return _strLevel; }
            set { _strLevel = value; }
        }

        /// <summary>
        /// 污染等级的名称
        /// </summary>
        public string LevelType
        {
            get { return _strLevelType; }
            set { _strLevelType = value; }
        }

        /// <summary>
        ///  因子数据取值方式,如：最大值N小时平均，还是滑动平均
        /// </summary>
        public Enums.PollutionIndexDataType PollutionIndexDataType
        {
            get { return _pollutionIndexDataType; }
            set { _pollutionIndexDataType = value; }
        }

        /// <summary>
        /// 测点ID
        /// </summary>
        public int PortId
        {
            get { return _intPortId; }
            set { _intPortId = value; }
        }

        /// <summary>
        /// 数据源的用途，如：小时报或日报
        /// </summary>
        public Enums.ReportType ReportType
        {
            get { return _reportType; }
            set { _reportType = value; }
        }

        /// <summary>
        /// 污染等级的罗马表示
        /// </summary>
        public string Roman
        {
            get { return _strRoman; }
            set { _strRoman = value; }
        }

        /// <summary>
        /// 对健康的建议
        /// </summary>
        public string Suggestion
        {
            get { return _strSuggestion; }
            set { _strSuggestion = value; }
        }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time
        {
            get { return _dtmTime; }
            set { _dtmTime = value; }
        }

        #endregion Properties
    }
}