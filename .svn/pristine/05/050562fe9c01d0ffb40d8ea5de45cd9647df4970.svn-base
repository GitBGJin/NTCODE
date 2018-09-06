namespace SmartEP.Service.DataAnalyze.AQIReport
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class AQIReport
    {
        #region Fields

        private List<AQIFactorData> _lstAQIDatas; //临时保储因子信息
        private List<FactorAQI> _lstPrimaryAQI; //首要污染物的相关信息

        #endregion Fields

        #region Constructors

        public AQIReport()
        {
        }

        /// <summary>
        /// 初使化因子数据信息列表
        /// </summary>
        /// <param name="lstAQIHourDatas">因子数据信息列表</param>
        public AQIReport(List<AQIFactorData> lstAQIHourDatas)
        {
            _lstAQIDatas = lstAQIHourDatas;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 保存用于计算的因子信息
        /// </summary>
        private List<AQIFactorData> AQIDatas
        {
            get
            {
                if (_lstAQIDatas == null)
                    _lstAQIDatas = new List<AQIFactorData>();
                return _lstAQIDatas;
            }
            set
            {
                _lstAQIDatas = value;
            }
        }

        /// <summary>
        /// 保存首要污染信息
        /// </summary>
        private List<FactorAQI> PrimaryAQI
        {
            get
            {
                if (_lstPrimaryAQI == null)
                    _lstPrimaryAQI = new List<FactorAQI>();
                return _lstPrimaryAQI;
            }
            set
            {
                _lstPrimaryAQI = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 添加因子信息
        /// </summary>
        /// <param name="strFactorName">因子名称</param>
        /// <param name="lstAQIDataTypes">因子参与计算的类型，如：O3在日报中同时参与“最大8小时滑动平均与最大1小时滑动平均两种计算”</param>
        /// <param name="reportType">数据的用途，如：小时报、日报</param>
        /// <param name="decFactorValue">因子浓度</param>
        /// <param name="dtmTime">因子信息的时间</param>
        /// <param name="intPortId">测点的ID</param>
        public void AddAQIFactorData(string strFactorName, List<AQIDataType> lstAQIDataTypes, Enums.ReportType reportType, decimal? decFactorValue, DateTime dtmTime, int intPortId)
        {
            dtmTime = DateTime.ParseExact(dtmTime.ToString("yyyy-MM-dd HH:00"), "yyyy-MM-dd HH:mm", null);
            RemoveAQIFactorData(strFactorName, dtmTime, intPortId, reportType);
            AddAQIData(new AQIFactorData() { FactorName = strFactorName, AQIDataTypes = lstAQIDataTypes, ReportType = reportType, FactorValue = decFactorValue, Time = dtmTime, PortId = intPortId });
        }

        public void AddAQIFactorData(AQIFactorData factorData)
        {
            AddAQIFactorData(factorData.FactorName, factorData.AQIDataTypes, factorData.ReportType, factorData.FactorValue, factorData.Time, factorData.PortId);
        }

        /// <summary>
        /// 获得报表AQI数据
        /// </summary>
        /// <param name="aqiType">报表类型</param>
        /// <returns></returns>
        public List<FactorAQI> GetAQI(Enums.ReportType aqiType)
        {
            List<FactorAQI> lstFactorAQI;

            if (aqiType == Enums.ReportType.HourAQI)//获小时报表AQI
            {
                lstFactorAQI = GetHourAQI();
            }
            else//获取日报AQI
            {
                lstFactorAQI = GetDayAQI();
            }
            SetHourAQIPollutionInfo(lstFactorAQI);
            return lstFactorAQI;
        }

        /// <summary>
        /// 获取因子原始数据列表
        /// </summary>
        /// <param name="reportType">获得的数据类型，如：小时报或日报</param>
        /// <returns></returns>
        public List<AQIFactorData> GetAQIFactorData(Enums.ReportType reportType)
        {
            return AQIDatas.Where(x => x.ReportType == reportType).ToList();
        }

        /// <summary>
        /// 获取首要污染物
        /// </summary>
        /// <param name="aqiType">报表类型</param>
        /// <returns></returns>
        public List<FactorAQI> GetPrimaryAQI(Enums.ReportType aqiType)
        {
            return PrimaryAQI.Where(x => x.ReportType == aqiType).ToList();
        }

        /// <summary>
        /// 删除因子信息
        /// </summary>
        /// <param name="strFactorName">因子名称</param>
        /// <param name="dtmTime">因子信息的时间</param>
        /// <param name="intPortId">测点的ID</param>
        public void RemoveAQIFactorData(string strFactorName, DateTime dtmTime, int intPortId, Enums.ReportType reportType)
        {
            List<AQIFactorData> aqiFactorDatas = GetAQIFactorData(reportType);
            AQIFactorData aqiFactorData = aqiFactorDatas.Where(x => x.FactorName == strFactorName && x.Time == dtmTime && x.PortId == intPortId && x.ReportType == reportType).FirstOrDefault();
            if (aqiFactorData != null)
            {
                RemoveAQIData(aqiFactorData);
            }
        }

        private void AddAQIData(AQIFactorData aQIFactorData)
        {
            if (_lstAQIDatas == null)
                _lstAQIDatas = new List<AQIFactorData>();
            _lstAQIDatas.Add(aQIFactorData);
        }

        /// <summary>
        /// 添加首要污染物
        /// </summary>
        /// <param name="factionAQI">AQI因子信息</param>
        private void AddPrimaryAQI(FactorAQI factionAQI)
        {
            if (_lstPrimaryAQI == null)
                _lstPrimaryAQI = new List<FactorAQI>();
            //FactorAQI tmpFactorAQI = _lstPrimaryAQI.Where(x => x.PortId == factionAQI.PortId && x.Time == factionAQI.Time && x.ReportType == factionAQI.ReportType && x.PollutionIndexDataType == factionAQI.PollutionIndexDataType).FirstOrDefault();
            //if (tmpFactorAQI != null)
            //    _lstPrimaryAQI.Remove(tmpFactorAQI);
            _lstPrimaryAQI.Add(factionAQI);
        }

        private void ClearPrimaryAQI()
        {
            if (_lstPrimaryAQI != null)
                _lstPrimaryAQI.Clear();
        }

        private FactorAQI FindAQI(List<FactorAQI> lstDayFactorAQI, int intPortId, string strFactorName, DateTime dtmTime, Enums.ReportType reportType, Enums.PollutionIndexDataType pollutionIndexDataType, Enums.CaculatorType caculatorType)
        {
            return lstDayFactorAQI.Where(x => x.PortId == intPortId && x.FactorName == strFactorName && x.Time == dtmTime && x.ReportType == reportType && x.PollutionIndexDataType == pollutionIndexDataType && x.CaculatorType == caculatorType).FirstOrDefault();
        }

        /// <summary>
        /// 求某测点的某因子在一时段时间的平均值,时间范围:[开始时间,结束)
        /// </summary>
        /// <param name="intPortId">测点Id</param>
        /// <param name="strFactorName">因子名称</param>
        /// <param name="dtmStartTime">开始时间</param>
        /// <param name="dtmEndTime">结束时间</param>
        /// <param name="reportType">报表类型</param>
        /// <returns></returns>
        private decimal? GetAvg(int intPortId, string strFactorName, DateTime dtmStartTime, DateTime dtmEndTime, Enums.ReportType reportType)
        {
            return AQIDatas.Where(x => x.Time >= dtmStartTime && x.Time < dtmEndTime && x.ReportType == reportType && x.PortId == intPortId && x.FactorName == strFactorName).Average(x => x.FactorValue);
        }

        /// <summary>
        /// 求某测点的某因子在一时段时间的平均值
        /// </summary>
        /// <param name="intPortId">测点Id</param>
        /// <param name="strFactorName">因子名称</param>
        /// <param name="dtmTime">时间点。持续时间包含time在内。</param>
        /// <param name="intHours">向前或向后持续的小时数，包含当前时间。</param>
        /// <param name="reportType">报表类型</param>
        /// <returns></returns>
        private decimal? GetAvg(int intPortId, string strFactorName, DateTime dtmTime, int intHours, Enums.ReportType reportType)
        {
            return intHours >= 0 ? GetAvg(intPortId, strFactorName, dtmTime, dtmTime.AddHours(intHours), reportType) : GetAvg(intPortId, strFactorName, dtmTime.AddHours(intHours + 1), dtmTime.AddHours(1), reportType);
        }

        private List<FactorAQI> GetDayAQI()
        {
            #region 局部变量
            List<FactorAQI> lstDayFactorAQI;//日报数据
            List<AQIFactorData> lstAQIFactorData;//用于日AQI计算的原始数据
            DateTime dtmStartTime;//开始时间，只精确到天
            DateTime dtmEndTime;//结束时间，只精确到天
            UInt16 intHours;//计算平均值的时段
            decimal? decFactorValue;//因子浓度信息
            #endregion

            #region 变量赋值
            lstDayFactorAQI = new List<FactorAQI>();
            lstAQIFactorData = GetAQIFactorData(Enums.ReportType.DayAQI);
            if (lstAQIFactorData.Count == 0) return lstDayFactorAQI;
            #endregion

            #region 获得AQI报表的数据
            foreach (AQIFactorData aqiFactorData in lstAQIFactorData)
            {
                dtmStartTime = DateTime.ParseExact(aqiFactorData.Time.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null);
                dtmEndTime = dtmStartTime.AddDays(1);//加一天，使用时间的区间变成:[开始时间,结束时间)
                foreach (AQIDataType aqiDataType in aqiFactorData.AQIDataTypes)
                {
                    //如果存在，刚找到
                    FactorAQI tmpFactorAQI = FindAQI(lstDayFactorAQI, aqiFactorData.PortId, aqiFactorData.FactorName, dtmStartTime, aqiFactorData.ReportType, aqiDataType.PollutionIndexDataType, aqiDataType.CaculatorType);
                    intHours = (UInt16)aqiDataType.CaculatorType;
                    if (aqiDataType.PollutionIndexDataType == Enums.PollutionIndexDataType.Avg)
                    {
                        decFactorValue = GetAvg(aqiFactorData.PortId, aqiFactorData.FactorName, dtmStartTime, dtmEndTime, aqiFactorData.ReportType);
                    }
                    else
                    {
                        decFactorValue = GetMaxAvg(aqiFactorData.PortId, aqiFactorData.FactorName, dtmStartTime, dtmEndTime, intHours, aqiFactorData.ReportType);
                    }
                    if (decFactorValue != null)
                        decFactorValue = Math.Round((decimal)decFactorValue, 3);
                    if (tmpFactorAQI != null)
                        tmpFactorAQI.FactorValue = decFactorValue;
                    else
                        lstDayFactorAQI.Add(new FactorAQI() { FactorName = aqiFactorData.FactorName, FactorValue = decFactorValue, PortId = aqiFactorData.PortId, Time = dtmStartTime, ReportType = aqiFactorData.ReportType, PollutionIndexDataType = aqiDataType.PollutionIndexDataType, CaculatorType = aqiDataType.CaculatorType });
                }
            }
            #endregion

            return lstDayFactorAQI;
        }

        private List<FactorAQI> GetHourAQI()
        {
            #region 局部变量
            List<FactorAQI> lstHourFactorAQI;//小时报数据
            List<AQIFactorData> lstAQIFactorData;//用于小时AQI计算的原始数据
            int intHours;//计算平均值的时段
            decimal? decFactorValue;
            #endregion

            lstHourFactorAQI = new List<FactorAQI>();
            lstAQIFactorData = GetAQIFactorData(Enums.ReportType.HourAQI);

            foreach (AQIFactorData aqiFactorData in lstAQIFactorData)
            {
                foreach (AQIDataType aqiDataType in aqiFactorData.AQIDataTypes)
                {
                    intHours = (int)aqiDataType.CaculatorType;
                    if (aqiDataType.PollutionIndexDataType == Enums.PollutionIndexDataType.Avg)
                    {
                        decFactorValue = GetAvg(aqiFactorData.PortId, aqiFactorData.FactorName, aqiFactorData.Time.AddHours(-intHours + 1), aqiFactorData.Time.AddHours(1), aqiFactorData.ReportType);
                        lstHourFactorAQI.Add(new FactorAQI() { FactorName = aqiFactorData.FactorName, CaculatorType = aqiDataType.CaculatorType, FactorValue = decFactorValue, PortId = aqiFactorData.PortId, Time = aqiFactorData.Time, ReportType = aqiFactorData.ReportType, PollutionIndexDataType = aqiDataType.PollutionIndexDataType });
                    }
                    else
                    {
                        //小时报不存在最大滑动平均
                    }
                }
            }
            return lstHourFactorAQI;
        }

        /// <summary>
        /// 找出某测点的某个因子在一段时间平均值的最大值,如XX点的SO2最大8小时平均,时间区间：[开始时间,结束时间)
        /// </summary>
        /// <param name="intPortId">测点ID</param>
        /// <param name="strFactorName">因子名称</param>
        /// <param name="dtmStartTime">开始时间</param>
        /// <param name="dtmEndTime">结束时间</param>
        /// <param name="uintHours">持续时间</param>
        /// <param name="reportType">报表类型</param>
        /// <returns>返回用银行家算法获取精确到3位小数的值</returns>
        private decimal? GetMaxAvg(int intPortId, string strFactorName, DateTime dtmStartTime, DateTime dtmEndTime, UInt16 uintHours, Enums.ReportType reportType)
        {
            decimal? decMaxValue = null;//存储最大值
            DateTime dtmTmp = dtmStartTime;
            while (dtmTmp < dtmEndTime)
            {
                decimal? decTmp;//临时变量，某个时段的平均值
                decTmp = GetAvg(intPortId, strFactorName, dtmTmp, -uintHours, reportType);
                if (decMaxValue == null)
                {
                    decMaxValue = decTmp;
                }
                else
                {
                    if (decTmp != null && decMaxValue < decTmp)//比较并存储最大值
                    {
                        decMaxValue = decTmp;
                    }
                }
                dtmTmp = dtmTmp.AddHours(1);
            }
            if (decMaxValue == null)
                return null;
            return Math.Round((decimal)decMaxValue, 3, MidpointRounding.ToEven);//用银行家算法获取精确到3位小数的值
        }

        private void RemoveAQIData(AQIFactorData aQIFactorData)
        {
            if (_lstAQIDatas == null)
                _lstAQIDatas = new List<AQIFactorData>();
            _lstAQIDatas.Remove(aQIFactorData);
        }

        /// <summary>
        /// 因子AQI浓度数据信息
        /// </summary>
        /// <param name="lstFactorAQI"></param>
        private void SetHourAQIPollutionInfo(List<FactorAQI> lstFactorAQI)
        {
            ClearPrimaryAQI();//清空首要污染物列表

            #region 变量定义
            PollutionIndex.PollutionIndexCaculatorContext aqiContext;
            #endregion

            #region 初使化变量
            aqiContext = new PollutionIndex.PollutionIndexCaculatorContext();
            #endregion

            #region 方法主体
            IEnumerable<IGrouping<int, FactorAQI>> eFactorAQIs = lstFactorAQI.GroupBy(x => x.PortId);//根据测点ID排序
            foreach (var eFactorAQI in eFactorAQIs)
            {
                IEnumerable<IGrouping<DateTime, FactorAQI>> egrps = eFactorAQI.GroupBy(x => x.Time);//再根据时间排序
                foreach (IGrouping<DateTime, FactorAQI> egrp in egrps)
                {
                    foreach (FactorAQI tmpFactorAQI in egrp)
                    {
                        #region 设置污染信息
                        aqiContext.AddFactor(tmpFactorAQI.FactorName, tmpFactorAQI.CaculatorType, tmpFactorAQI.FactorValue);
                        PollutionIndex.PollutionLevelInfo tmpPollutionLevelInfo = aqiContext.GetPollutionLevelInfo(tmpFactorAQI.FactorName, tmpFactorAQI.CaculatorType, Enums.PollutionIndexType.AQI);
                        if (tmpPollutionLevelInfo != null)
                        {
                            //lstFactorAQI.Remove(tmpFactorAQI);
                            tmpFactorAQI.AQI = tmpPollutionLevelInfo.PollutionIndex;
                            tmpFactorAQI.Color = tmpPollutionLevelInfo.Color;
                            tmpFactorAQI.ColorCode = tmpPollutionLevelInfo.ColorCode;
                            tmpFactorAQI.HealthEffect = tmpPollutionLevelInfo.HealthEffect;
                            tmpFactorAQI.IsPollutionIndexFactor = tmpPollutionLevelInfo.IsPollutionIndexFactor;
                            tmpFactorAQI.Level = tmpPollutionLevelInfo.Level;
                            tmpFactorAQI.LevelType = tmpPollutionLevelInfo.LevelType;
                            tmpFactorAQI.Roman = tmpPollutionLevelInfo.Roman;
                            tmpFactorAQI.Suggestion = tmpPollutionLevelInfo.Suggestion;
                            //lstFactorAQI.Add(tmpFactorAQI);
                        }
                        #endregion
                    }
                    #region 设置首要污染物
                    List<PollutionIndex.PollutionLevelInfo> allPLF = aqiContext.GetPrimaryPollutions();
                    if (allPLF != null)
                    {
                        foreach (PollutionIndex.PollutionLevelInfo pollutionLevelInfo in allPLF)
                        {
                            FactorAQI tempItem = egrp.Where(x => x.FactorName == pollutionLevelInfo.FactorName && x.CaculatorType == pollutionLevelInfo.CaculatorType && x.IsPollutionIndexFactor == pollutionLevelInfo.IsPollutionIndexFactor).FirstOrDefault();
                            if (tempItem != null)
                            {
                                AddPrimaryAQI(tempItem);
                            }
                        }
                    }
                    #endregion
                    aqiContext.Clear();//清空,为下次找首要污染物作准备
                }

            }
            #endregion

            //return lstFactorAQI;
        }

        #endregion Methods
    }
}