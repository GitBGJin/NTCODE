#region Header

// File:    PollutionIndexStandard.cs
// Author:  Harry
// Created: 2012年11月30日 14:47:17
// Purpose: Definition of Class PollutionIndexStandard

#endregion Header

namespace SmartEP.Service.DataAnalyze.PollutionIndex
{
    using System;

    public class PollutionIndexStandard
    {
        #region Fields

        private Enums.CaculatorType caculatorType;
        private string factorCNName;
        private string factorName;
        private decimal maxConcentration;
        private int maxPollutionIndex;
        private decimal minConcentration;
        private int minPollutionIndex;
        private Enums.PollutionIndexType pollutionIndexType;
        private string unit;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 时间类型
        /// </summary>
        public Enums.CaculatorType CaculatorType
        {
            get
            {
                return this.caculatorType;
            }
            set
            {
                this.caculatorType = value;
            }
        }

        /// <summary>
        /// 因子中文名称
        /// </summary>
        public string FactorCNName
        {
            get
            {
                return this.factorCNName;
            }
            set
            {
                this.factorCNName = value;
            }
        }

        /// <summary>
        /// 因子名称
        /// </summary>
        public string FactorName
        {
            get
            {
                return this.factorName;
            }
            set
            {
                this.factorName = value;
            }
        }

        /// <summary>
        /// 污染浓度下限
        /// </summary>
        public decimal MaxConcentration
        {
            get
            {
                return this.maxConcentration;
            }
            set
            {
                this.maxConcentration = value;
            }
        }

        /// <summary>
        /// 空气污染指数上限
        /// </summary>
        public int MaxPollutionIndex
        {
            get
            {
                return this.maxPollutionIndex;
            }
            set
            {
                this.maxPollutionIndex = value;
            }
        }

        /// <summary>
        /// 污染浓度下限
        /// </summary>
        public decimal MinConcentration
        {
            get
            {
                return this.minConcentration;
            }
            set
            {
                this.minConcentration = value;
            }
        }

        /// <summary>
        /// 空气污染指数下限
        /// </summary>
        public int MinPollutionIndex
        {
            get
            {
                return this.minPollutionIndex;
            }
            set
            {
                this.minPollutionIndex = value;
            }
        }

        /// <summary>
        /// 污染指数的类型，如：AQI、API
        /// </summary>
        public Enums.PollutionIndexType PollutionIndexType
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

        /// <summary>
        /// 因子单位
        /// </summary>
        public string Unit
        {
            get
            {
                return this.unit;
            }
            set
            {
                this.unit = value;
            }
        }

        #endregion Properties
    }
}