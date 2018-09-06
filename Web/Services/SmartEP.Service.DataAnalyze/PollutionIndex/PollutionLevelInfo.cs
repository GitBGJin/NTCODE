#region Header

// File:    PollutionLevelInfo.cs
// Author:  Harry
// Created: 2012年11月20日 16:56:23
// Purpose: Definition of Class PollutionLevelInfo

#endregion Header

namespace SmartEP.Service.DataAnalyze.PollutionIndex
{
    using System;

    public class PollutionLevelInfo
    {
        #region Fields

        private Enums.CaculatorType caculType;
        private string color;
        private string colorCode;
        private decimal? concentration;
        private string factorName;
        private string healthEffect;
        private string id;
        private bool isPollutionIndexFactor;
        private string level;
        private string levelType;
        private int? max;
        private int? min;
        private int? pollutionIndex;
        private Enums.PollutionIndexType pollutionIndexType;
        private string range;
        private string roman;
        private string suggestion;

        #endregion Fields

        #region Properties

        public Enums.CaculatorType CaculatorType
        {
            get
            {
                return this.caculType;
            }
            set
            {
                this.caculType = value;
            }
        }

        public string Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }

        public string ColorCode
        {
            get
            {
                return this.colorCode;
            }
            set
            {
                this.colorCode = value;
            }
        }

        public decimal? Concentration
        {
            get
            {
                return this.concentration;
            }
            set
            {
                this.concentration = value;
            }
        }

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

        public string HealthEffect
        {
            get
            {
                return this.healthEffect;
            }
            set
            {
                this.healthEffect = value;
            }
        }

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// 是否找到此污染物标准
        /// </summary>
        public bool IsPollutionIndexFactor
        {
            get { return isPollutionIndexFactor; }
            set { isPollutionIndexFactor = value; }
        }

        public string Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        public string LevelType
        {
            get
            {
                return this.levelType;
            }
            set
            {
                this.levelType = value;
            }
        }

        public int? Max
        {
            get
            {
                return this.max;
            }
            set
            {
                this.max = value;
            }
        }

        public int? Min
        {
            get
            {
                return this.min;
            }
            set
            {
                this.min = value;
            }
        }

        public int? PollutionIndex
        {
            get
            {
                return this.pollutionIndex;
            }
            set
            {
                this.pollutionIndex = value;
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

        public string Range
        {
            get
            {
                return this.range;
            }
            set
            {
                this.range = value;
            }
        }

        /// <summary>
        /// 等级的罗马字符，如：I、II
        /// </summary>
        public string Roman
        {
            get
            {
                return this.roman;
            }
            set
            {
                this.roman = value;
            }
        }

        public string Suggestion
        {
            get
            {
                return this.suggestion;
            }
            set
            {
                this.suggestion = value;
            }
        }

        #endregion Properties
    }
}