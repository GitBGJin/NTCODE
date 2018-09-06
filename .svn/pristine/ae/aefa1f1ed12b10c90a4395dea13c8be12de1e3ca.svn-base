namespace SmartEP.Service.DataAnalyze.WaterPollutionIndex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class WaterPollutionIndexLibrary
    {
        #region Fields

        private IDictionary<Enums.WaterPollutionFactor, IList<WaterPollutionLevel>> _dicWaterPollutionLevel;

        #endregion Fields

        #region Constructors

        public WaterPollutionIndexLibrary()
        {
            //初始化水的标准
            InitWaterStandard();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 获取水的等级名称
        /// </summary>
        /// <param name="waterPollutionFactor">因子的等级</param>
        /// <param name="factorValue">因子的值</param>
        /// <returns></returns>
        public string GetWaterPollutionLevel(Enums.WaterPollutionFactor waterPollutionFactor, decimal? factorValue)
        {
            if (factorValue == null)//值为NULL
            {
                return null;
            }
            IList<WaterPollutionLevel> lstWaterPollutionLevel = _dicWaterPollutionLevel[waterPollutionFactor];
            if (lstWaterPollutionLevel == null || lstWaterPollutionLevel.Count == 0)//没有找到标准
            {
                return null;
            }
            if (waterPollutionFactor == Enums.WaterPollutionFactor.PH)//特殊处理PH值
            {
                return GetSpecialPollutionLevel(waterPollutionFactor, lstWaterPollutionLevel, (decimal)factorValue);
            }
            else
            {
                return GetGenericPollutionLevel(lstWaterPollutionLevel, factorValue);
            }
        }

        /// <summary>
        /// 获取等级：处理的是等级很规则的水因子信息
        /// </summary>
        /// <param name="lstWaterPollutionLevel"></param>
        /// <param name="factorValue"></param>
        /// <returns></returns>
        private string GetGenericPollutionLevel(IList<WaterPollutionLevel> lstWaterPollutionLevel, decimal? factorValue)
        {
            //找到等级信息
            WaterPollutionLevel waterPollutionLevel;
            decimal? decLower;//最小值
            decimal? decUpper;//最大值
            decLower = lstWaterPollutionLevel.First().Max;
            decUpper = lstWaterPollutionLevel.Last().Max;
            if (decLower < decUpper)
            {
                //<=中的最小值
                waterPollutionLevel = lstWaterPollutionLevel.Where(x => factorValue <= x.Max).OrderBy(x => x.Max).FirstOrDefault();
            }
            else
            {
                //>=因子值的最大值
                waterPollutionLevel = lstWaterPollutionLevel.Where(x => factorValue >= x.Max).OrderByDescending(x => x.Max).FirstOrDefault();
            }
            if (waterPollutionLevel == null)
                return Enums.WaterPollutionLevelType.WeakFive.ToString();
            else
                return waterPollutionLevel.PollutionLevelName.ToString();
        }

        /// <summary>
        /// 获得特殊等级：处理的是等级不规则的水因子信息,如PH值
        /// </summary>
        /// <param name="waterPollutionFactor">水的污染因子</param>
        /// <param name="lstWaterPollutionLevel"></param>
        /// <param name="factorValue"></param>
        /// <returns></returns>
        private string GetSpecialPollutionLevel(Enums.WaterPollutionFactor waterPollutionFactor, IList<WaterPollutionLevel> lstWaterPollutionLevel, decimal factorValue)
        {
            if (waterPollutionFactor == Enums.WaterPollutionFactor.PH)
            {
                decimal? decLower;//最小值
                decimal? decUpper;//最大值
                WaterPollutionLevel waterPollutionLevel = lstWaterPollutionLevel.Where(x => x.PollutionLevelName == Enums.WaterPollutionLevelType.One).FirstOrDefault();
                decLower = waterPollutionLevel.Max;
                waterPollutionLevel = lstWaterPollutionLevel.Where(x => x.PollutionLevelName == Enums.WaterPollutionLevelType.Five).FirstOrDefault();
                decUpper = waterPollutionLevel.Max;
                if (factorValue < decLower || factorValue > decUpper)
                {
                    return Enums.WaterPollutionLevelType.WeakFive.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// 初始化水的标准规范
        /// </summary>
        private void InitWaterStandard()
        {
            #region 初始化水的标准
            _dicWaterPollutionLevel = new Dictionary<Enums.WaterPollutionFactor, IList<WaterPollutionLevel>>();
            //溶解氧
            _dicWaterPollutionLevel.Add(Enums.WaterPollutionFactor.DissolvedOxygen,
                new List<WaterPollutionLevel>() {
                    new WaterPollutionLevel(){ Max=7.5M, PollutionLevelName=Enums.WaterPollutionLevelType.One},
                    new WaterPollutionLevel(){ Max=6M, PollutionLevelName=Enums.WaterPollutionLevelType.Two },
                    new WaterPollutionLevel(){ Max=5M, PollutionLevelName=Enums.WaterPollutionLevelType.Three },
                    new WaterPollutionLevel(){ Max=3M, PollutionLevelName=Enums.WaterPollutionLevelType.Four },
                    new WaterPollutionLevel(){ Max=2M, PollutionLevelName=Enums.WaterPollutionLevelType.Five }
            });
            //氨氮
            _dicWaterPollutionLevel.Add(Enums.WaterPollutionFactor.AmmonniaNitrogen,
                new List<WaterPollutionLevel>() {
                    new WaterPollutionLevel(){ Max=0.15M, PollutionLevelName=Enums.WaterPollutionLevelType.One},
                    new WaterPollutionLevel(){ Max=0.5M, PollutionLevelName=Enums.WaterPollutionLevelType.Two },
                    new WaterPollutionLevel(){ Max=1.0M, PollutionLevelName=Enums.WaterPollutionLevelType.Three},
                    new WaterPollutionLevel(){ Max=1.5M, PollutionLevelName=Enums.WaterPollutionLevelType.Four },
                    new WaterPollutionLevel(){ Max=2.0M, PollutionLevelName=Enums.WaterPollutionLevelType.Five }
              });
            //高锰酸盐指数
            _dicWaterPollutionLevel.Add(Enums.WaterPollutionFactor.PermanganateIndex,
                  new List<WaterPollutionLevel>() {
                    new WaterPollutionLevel(){ Max=2M, PollutionLevelName=Enums.WaterPollutionLevelType.One},
                    new WaterPollutionLevel(){ Max=4M, PollutionLevelName=Enums.WaterPollutionLevelType.Two},
                    new WaterPollutionLevel(){ Max=6M, PollutionLevelName=Enums.WaterPollutionLevelType.Three},
                    new WaterPollutionLevel(){ Max=10M, PollutionLevelName=Enums.WaterPollutionLevelType.Four },
                    new WaterPollutionLevel(){ Max=15M, PollutionLevelName=Enums.WaterPollutionLevelType.Five }
              });
            //总磷
            _dicWaterPollutionLevel.Add(Enums.WaterPollutionFactor.PhosphorusTotal,
                new List<WaterPollutionLevel>() {
                    new WaterPollutionLevel(){ Max=0.02M, PollutionLevelName=Enums.WaterPollutionLevelType.One},
                    new WaterPollutionLevel(){ Max=0.1M, PollutionLevelName=Enums.WaterPollutionLevelType.Two },
                    new WaterPollutionLevel(){ Max=0.2M, PollutionLevelName=Enums.WaterPollutionLevelType.Three},
                    new WaterPollutionLevel(){ Max=0.3M, PollutionLevelName=Enums.WaterPollutionLevelType.Four },
                    new WaterPollutionLevel(){ Max=0.4M, PollutionLevelName=Enums.WaterPollutionLevelType.Five }
              });
            //PH值
            _dicWaterPollutionLevel.Add(Enums.WaterPollutionFactor.PH,
              new List<WaterPollutionLevel>() {
                    new WaterPollutionLevel(){ Max=6M, PollutionLevelName=Enums.WaterPollutionLevelType.One},
                    new WaterPollutionLevel(){ Max=null, PollutionLevelName=Enums.WaterPollutionLevelType.Two },
                    new WaterPollutionLevel(){ Max=null, PollutionLevelName=Enums.WaterPollutionLevelType.Three },
                    new WaterPollutionLevel(){ Max=null, PollutionLevelName=Enums.WaterPollutionLevelType.Four },
                    new WaterPollutionLevel(){ Max=9M, PollutionLevelName=Enums.WaterPollutionLevelType.Five }
              });
            #endregion
        }

        #endregion Methods
    }
}