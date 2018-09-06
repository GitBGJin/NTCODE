namespace SmartEP.Service.DataAnalyze.AQIReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AQIDataType
    {
        #region Fields

        private Enums.CaculatorType caculatorType;
        private Enums.PollutionIndexDataType pollutionIndexDataType;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 因子参与计算的种类，如：1小时，8小时，24小时
        /// </summary>
        public Enums.CaculatorType CaculatorType
        {
            get { return caculatorType; }
            set { caculatorType = value; }
        }

        /// <summary>
        /// 因子数据取值方式,如：最大值N小时平均，还是滑动平均
        /// </summary>
        public Enums.PollutionIndexDataType PollutionIndexDataType
        {
            get { return pollutionIndexDataType; }
            set { pollutionIndexDataType = value; }
        }

        #endregion Properties
    }
}