using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Core.Enums;
using SmartEP.Service.Core.Enums;
using SmartEP.MonitoringBusinessRepository.Water;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.DomainModel.BaseData;
using SmartEP.BaseInfoRepository.Dictionary;

namespace SmartEP.Service.DataAnalyze.Water.DataQuery
{
    /// <summary>
    /// 名称：WaterQualityAnalysis.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-1
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：水质分析
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterQualityAnalysis
    {
        /// <summary>
        /// 水质分析数据访问类
        /// </summary>
        WaterAnalyzeDAL d_WaterAnalyze = new WaterAnalyzeDAL();

        #region 日数据
        /// <summary>
        /// 水质分析
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView WaterQuality(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("StandardStatus", typeof(string));

            try
            {
                MonitoringPointWaterService s_MonitoringPointWater = new MonitoringPointWaterService();
                WaterQualityService WaterQuality = new WaterQualityService();
                DataQueryByDayService dayDataService = new DataQueryByDayService();
                DataTable dt_Values = dayDataService.GetDayStatisticalData(portIds, factors, dateStart, dateEnd).Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int WQL = 0;
                    string EvaluateFactorCodes = "";
                    Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    dt.Rows[i]["WatersName"] = s_MonitoringPointWater.RetrieveEntityByPointId(Convert.ToInt32(pointId)).MonitoringPointName;
                    DataRow[] drs = dt_Values.Select("PointId=" + pointId);
                    for (int j = 0; j < drs.Length; j++)
                    {
                        string pollutantCode = drs[j]["PollutantCode"].ToString();
                        decimal pollutantValue = Convert.ToDecimal(drs[j]["Value_Avg"]);
                        string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            EvaluateFactorCodes += pollutantCode + ";";
                            WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                        }
                    }
                    if (!string.IsNullOrEmpty(EvaluateFactorCodes))
                    {
                        EvaluateFactorCodes = EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);
                        string GetWQL = WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            WQL = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues));
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()))
                    {
                        if (Convert.ToInt32(dt.Rows[i]["IEQI"]) <= WQL && WQL > 0)
                        {
                            dt.Rows[i]["StandardStatus"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "未达标";
                        }
                    }
                    else
                    {
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }

        /// <summary>
        /// 日水质达标统计
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView WaterQualityStandardStatus(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            //建立返回DataTable
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("RealClass", typeof(string));//水质类别
            dt.Columns.Add(dateStart.Year.ToString(), typeof(string));//年
            dt.Columns.Add("TTrend", typeof(string));//同期趋势
            dt.Columns.Add("TCompare", typeof(string));//同期比较
            dt.Columns.Add("HTrend", typeof(string));//环比趋势
            dt.Columns.Add("HCompare", typeof(string));//环比比较
            dt.Columns.Add("StandardStatus", typeof(string));//达标情况

            try
            {
                DataQueryByDayService dayDataService = new DataQueryByDayService();
                WaterQualityService WaterQuality = new WaterQualityService();
                //原始日均值数据
                DataTable dt_Original = dayDataService.GetDayStatisticalData(portIds, factors, dateStart, dateEnd).Table;
                //同比日均值数据
                DataTable dt_ContemporaryComparison = dayDataService.GetDayStatisticalData(portIds, factors, dateStart.AddYears(-1), dateEnd.AddYears(-1)).Table;
                //环比日均值数据
                int diffValue = (dateEnd - dateStart).Days;
                DateTime hDateEnd = dateStart.AddDays(-1);
                DateTime hDateStart = hDateEnd.AddDays(-diffValue);
                DataTable dt_Chain = dayDataService.GetDayStatisticalData(portIds, factors, hDateStart, hDateEnd).Table;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 参数
                    int w_Value = 0;
                    int t_Value = 0;
                    int H_Value = 0;
                    string O_EvaluateFactorCodes = "";
                    string T_EvaluateFactorCodes = "";
                    string H_EvaluateFactorCodes = "";
                    Dictionary<string, Int32> O_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> T_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> H_WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    #endregion
                    //根据站点id获取原始数据
                    DataRow[] dr_Original = dt_Original.Select("PointId=" + pointId);
                    if (dr_Original.Length > 0)
                    {
                        #region 原始
                        for (int j = 0; j < dr_Original.Length; j++)
                        {
                            //获取因子code
                            string pollutantCode = dr_Original[j]["PollutantCode"].ToString();
                            //获取因子浓度
                            decimal pollutantValue = decimal.TryParse(dr_Original[j]["Value_Avg"].ToString(), out pollutantValue) ? pollutantValue : 0;
                            //获取单个因子的水质类别
                            string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                            if (!string.IsNullOrEmpty(GetWQL))
                            {
                                //获取评价因子
                                O_EvaluateFactorCodes += pollutantCode + ";";
                                O_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                            }
                        }
                        if (!string.IsNullOrEmpty(O_EvaluateFactorCodes))
                        {
                            O_EvaluateFactorCodes = O_EvaluateFactorCodes.Substring(0, O_EvaluateFactorCodes.Length - 1);
                            //获取水质类别值
                            w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, O_EvaluateFactorCodes, O_WQIValues));
                            string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Roman, O_EvaluateFactorCodes, O_WQIValues);
                            dt.Rows[i]["RealClass"] = w_Class + "类";
                        }
                        else
                        {
                            dt.Rows[i]["RealClass"] = "/";
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()) && !string.IsNullOrEmpty(w_Value.ToString()))
                        {
                            if (w_Value <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && w_Value > 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "达标";
                            }
                            else if (w_Value == 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "/";
                            }
                            else
                            {
                                dt.Rows[i]["StandardStatus"] = "未达标";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "/";
                        }
                        #endregion
                        #region 同比
                        DataRow[] dr_ContemporaryComparison = dt_ContemporaryComparison.Select("PointId=" + pointId);
                        if (dr_ContemporaryComparison.Length > 0)
                        {
                            for (int j = 0; j < dr_ContemporaryComparison.Length; j++)
                            {
                                string pollutantCode = dr_ContemporaryComparison[j]["PollutantCode"].ToString();
                                decimal pollutantValue = decimal.TryParse(dr_ContemporaryComparison[j]["Value_Avg"].ToString(), out pollutantValue) ? pollutantValue : 0;
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    T_EvaluateFactorCodes += pollutantCode + ";";
                                    T_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(T_EvaluateFactorCodes))
                            {
                                T_EvaluateFactorCodes = T_EvaluateFactorCodes.Substring(0, T_EvaluateFactorCodes.Length - 1);
                                t_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, T_EvaluateFactorCodes, T_WQIValues));
                            }
                            if (t_Value > 0)
                            {
                                string range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(t_Value)) / Convert.ToDouble(t_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["TCompare"] = range;
                                if (w_Value > t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "上升";
                                }
                                else if (w_Value < t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "下降";
                                }
                                else if (w_Value == t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["TCompare"] = "/";
                                dt.Rows[i]["TTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["TCompare"] = "/";
                            dt.Rows[i]["TTrend"] = "/";
                        }
                        #endregion
                        #region 环比
                        DataRow[] dr_Chain = dt_Chain.Select("PointId=" + pointId);
                        if (dr_Chain.Length > 0)
                        {
                            for (int j = 0; j < dr_Chain.Length; j++)
                            {
                                string pollutantCode = dr_Chain[j]["PollutantCode"].ToString();
                                decimal pollutantValue = decimal.TryParse(dr_Chain[j]["Value_Avg"].ToString(), out pollutantValue) ? pollutantValue : 0;
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    H_EvaluateFactorCodes += pollutantCode + ";";
                                    H_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(H_EvaluateFactorCodes))
                            {
                                H_EvaluateFactorCodes = H_EvaluateFactorCodes.Substring(0, H_EvaluateFactorCodes.Length - 1);
                                H_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, H_EvaluateFactorCodes, H_WQIValues));
                            }
                            if (H_Value > 0)
                            {
                                string h_Range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(H_Value)) / Convert.ToDouble(H_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["HCompare"] = h_Range;
                                if (w_Value > H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "上升";
                                }
                                else if (w_Value < H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "下降";
                                }
                                else if (w_Value == H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["HCompare"] = "/";
                                dt.Rows[i]["HTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["HCompare"] = "/";
                            dt.Rows[i]["HTrend"] = "/";
                        }
                        #endregion
                    }
                    else
                    {
                        dt.Rows[i]["RealClass"] = "/";
                        dt.Rows[i]["TTrend"] = "/";
                        dt.Rows[i]["TCompare"] = "/";
                        dt.Rows[i]["HTrend"] = "/";
                        dt.Rows[i]["HCompare"] = "/";
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                    dt.Rows[i][dateStart.Year.ToString()] = dateStart.ToString("MM月dd日") + "-" + dateEnd.ToString("MM月dd日");
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }

        /// <summary>
        /// 水质分析周报数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataView WaterQualityWeekReport(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd)
        {
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("Region", typeof(string));
            dt.Columns.Add("Site", typeof(string));
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("ClassLastWeek", typeof(string));
            dt.Columns.Add("ClassThisWeek", typeof(string));
            dt.Columns.Add("Order", typeof(string));
            dt.Columns.Add("PrimaryPollutant", typeof(string));
            foreach (IPollutant iFactor in factors)
            {
                dt.Columns.Add(iFactor.PollutantCode);
            }
            try
            {
                string WQL = "";
                string EvaluateFactorCodes = "";
                string CalEQIType = "";
                Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
                Dictionary<string, Int32> WQILastWeekValues = new Dictionary<string, int>();
                WaterQualityService WaterQuality = new WaterQualityService();
                DataQueryByDayService dayDataService = new DataQueryByDayService();
                DataTable dt_Values = dayDataService.GetDayStatisticalDataNew(portIds, factors.Select(x => x.PollutantCode).ToArray(), dateStart, dateEnd).Table;
                DataTable dt_LastWeekValues = dayDataService.GetDayStatisticalDataNew(portIds, factors.Select(x => x.PollutantCode).ToArray(), dateStart.AddDays(-7), dateEnd.AddDays(-7)).Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string pointId = dt.Rows[i]["PointId"].ToString();
                    #region 本周数据处理
                    DataRow[] drs = dt_Values.Select("PointId=" + pointId);
                    DataRow[] qualityRow = dt.Select("PointId=" + portIds[i]);
                    if (qualityRow.Length > 0)
                    {
                        CalEQIType = qualityRow[0]["CalEQIType"].ToString();//获取功能水质水质类别
                    }
                    for (int j = 0; j < drs.Length; j++)
                    {
                        string pollutantCode = drs[j]["PollutantCode"].ToString();
                        EvaluateFactorCodes += pollutantCode + ";";
                        int digit = 2;

                        digit = Convert.ToInt32(factors.Where(x => x.PollutantCode == pollutantCode).Select(x => x.PollutantDecimalNum).FirstOrDefault());
                        if (pollutantCode == "w21003")
                        {
                            digit = 2;
                        }
                        else if (pollutantCode == "w01010" || pollutantCode == "w01019")
                        {
                            digit = 1;
                        }
                        else if (pollutantCode == "w01016")
                        {
                            digit = 4;
                        }
                        decimal pollutantValue = drs[j]["Value_Avg"] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(drs[j]["Value_Avg"]), digit) : -1;
                        if (pollutantCode == "w01016")
                        {
                            pollutantValue = drs[j]["Value_Avg"] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(drs[j]["Value_Avg"]) / 1000, digit) : -1;
                        }
                        #region 获取等级
                        switch (CalEQIType)
                        {
                            case "湖泊":
                                WQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.Lake, EQIReurnType.Level);
                                break;
                            case "河流":
                                WQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                break;
                            default:
                                WQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                break;
                        }
                        #endregion
                        if (!WQL.Equals(""))
                            WQIValues.Add(pollutantCode, Convert.ToInt32(WQL));
                        //if (i == 0)
                        //{
                        //    dt.Columns.Add(pollutantCode);
                        //}
                        dt.Rows[i]["Region"] = "";
                        //dt.Rows[i]["PortName"] = drs[j]["portName"];
                        dt.Rows[i]["Site"] = "苏州市环境监测中心";
                        dt.Rows[i]["Tstamp"] = dateStart.ToString("yyyyMMdd") + "-" + dateEnd.ToString("yyyyMMdd");
                        if (pollutantCode.Equals("w01001"))
                            dt.Rows[i][pollutantCode] = (drs[j]["Value_Min"] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(drs[j]["Value_Min"].ToString()), digit).ToString() : "-") + "-" + (drs[j]["Value_Max"] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(drs[j]["Value_Max"].ToString()), digit).ToString() : "-");
                        else
                            dt.Rows[i][pollutantCode] = pollutantValue;
                    }
                    if (EvaluateFactorCodes.Length >= 1)
                    {
                        EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);
                    }
                    #endregion
                    #region 上周数据处理
                    DataRow[] drsLast = dt_LastWeekValues.Select("PointId=" + pointId);
                    for (int j = 0; j < drsLast.Length; j++)
                    {
                        string pollutantCode = drsLast[j]["PollutantCode"].ToString();
                        decimal pollutantValue = drsLast[j]["Value_Avg"] != DBNull.Value ? Convert.ToDecimal(drsLast[j]["Value_Avg"]) : -1;
                        switch (CalEQIType)
                        {
                            case "湖泊":
                                WQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.Lake, EQIReurnType.Level);
                                break;
                            case "河流":
                                WQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                break;
                            default:
                                WQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                break;
                        }
                        if (!WQL.Equals(""))
                            WQILastWeekValues.Add(pollutantCode, Convert.ToInt32(WQL));
                    }
                    #endregion
                    int w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues));
                    string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Roman, EvaluateFactorCodes, WQIValues);
                    string w_Pollutant = WaterQuality.GetWQL_Max(EQIReurnType.Name, EvaluateFactorCodes, WQIValues);

                    int w_ValueLast = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQILastWeekValues));
                    string w_ClassLast = WaterQuality.GetWQL_Max(EQIReurnType.Roman, EvaluateFactorCodes, WQILastWeekValues);
                    //首要污染物
                    string PrimaryPollutant = WaterQuality.GetWQL_Max(EQIReurnType.Name, EvaluateFactorCodes, WQIValues);
                    if (w_Value > 2)
                    {
                        dt.Rows[i]["PrimaryPollutant"] = w_Pollutant;
                    }
                    dt.Rows[i]["ClassThisWeek"] = w_Class;
                    dt.Rows[i]["ClassLastWeek"] = w_ClassLast;
                    if (w_Value < w_ValueLast)
                        dt.Rows[i]["Order"] = "升";
                    else if (w_Value == w_ValueLast)
                        dt.Rows[i]["Order"] = "平";
                    else if (w_Value > w_ValueLast)
                        dt.Rows[i]["Order"] = "降";

                }
            }
            catch (Exception ex) { }
            dt.Columns["Order"].SetOrdinal(1);
            dt.Columns["ClassLastWeek"].SetOrdinal(1);
            dt.Columns["PrimaryPollutant"].SetOrdinal(1);
            dt.Columns["ClassThisWeek"].SetOrdinal(1);
            foreach (IPollutant iFactor in factors)
            {
                dt.Columns[iFactor.PollutantCode].SetOrdinal(1);
            }
            dt.Columns["Tstamp"].SetOrdinal(1);
            dt.Columns["Site"].SetOrdinal(1);
            dt.Columns["MonitoringPointName"].SetOrdinal(1);
            return dt.DefaultView;
        }

        #endregion

        #region 周数据
        /// <summary>
        /// 周水质达标统计
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView WaterQualityWeekStandardStatus(string[] portIds, IList<IPollutant> factors, int yearFrom, int weekOfYearFrom, int yearTo, int weekOfYearTo)
        {
            //建立返回DataTable
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("RealClass", typeof(string));//水质类别
            dt.Columns.Add(yearFrom.ToString(), typeof(string));//年
            dt.Columns.Add("TTrend", typeof(string));//同期趋势
            dt.Columns.Add("TCompare", typeof(string));//同期比较
            dt.Columns.Add("HTrend", typeof(string));//环比趋势
            dt.Columns.Add("HCompare", typeof(string));//环比比较
            dt.Columns.Add("StandardStatus", typeof(string));//达标情况

            try
            {
                DataQueryByWeekService weekDataService = new DataQueryByWeekService();
                WaterQualityService WaterQuality = new WaterQualityService();
                //原始周数据
                DataTable dt_Original = weekDataService.GetWeekStatisticalData(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo).Table;
                //同比周数据
                DataTable dt_ContemporaryComparison = weekDataService.GetWeekStatisticalData(portIds, factors, yearFrom - 1, weekOfYearFrom, yearTo - 1, weekOfYearTo).Table;
                int diffValue = weekOfYearTo - weekOfYearFrom + 1;
                int hEndYear = 0;
                int hEndWeek = 0;
                int hStartYear = 0;
                int hStartWeek = 0;
                if (weekOfYearTo != 1)
                {
                    if (weekOfYearTo - 1 >= diffValue)
                    {
                        hEndYear = yearFrom;
                        hEndWeek = weekOfYearFrom - 1;
                        hStartYear = yearFrom;
                        hStartWeek = weekOfYearFrom - diffValue;
                    }
                    else
                    {
                        hEndYear = yearFrom;
                        hEndWeek = weekOfYearFrom - 1;
                        hStartYear = yearFrom - 1;
                        int diff = diffValue - weekOfYearFrom + 1;
                        DateTime dateTime = new DateTime(hStartYear, 1, 1);
                        TimeSpan ts = dateTime.AddYears(1) - dateTime;
                        int TotalWeek = ts.Days / 7 + 1;
                        hStartWeek = TotalWeek - diff + 1;
                    }
                }
                else
                {
                    hEndYear = yearFrom - 1;
                    hStartYear = yearFrom - 1;
                    int diff = diffValue - weekOfYearFrom + 1;
                    DateTime dateTime = new DateTime(hStartYear, 1, 1);
                    TimeSpan ts = dateTime.AddYears(1) - dateTime;
                    int TotalWeek = ts.Days / 7 + 1;
                    hEndWeek = TotalWeek;
                    hStartWeek = hEndWeek - diff + 1;
                }
                //环比周数据
                DataTable dt_Chain = weekDataService.GetWeekStatisticalData(portIds, factors, hStartYear, hStartWeek, hEndYear, hEndWeek).Table;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 参数
                    int w_Value = 0;
                    int t_Value = 0;
                    int H_Value = 0;
                    string O_EvaluateFactorCodes = "";
                    string T_EvaluateFactorCodes = "";
                    string H_EvaluateFactorCodes = "";
                    Dictionary<string, Int32> O_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> T_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> H_WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    #endregion
                    //根据站点id获取原始数据
                    DataRow[] dr_Original = dt_Original.Select("PointId=" + pointId);
                    if (dr_Original.Length > 0)
                    {
                        #region 原始
                        for (int j = 0; j < dr_Original.Length; j++)
                        {
                            //获取因子code
                            string pollutantCode = dr_Original[j]["PollutantCode"].ToString();
                            //获取因子浓度
                            decimal pollutantValue = Convert.ToDecimal(dr_Original[j]["Value_Avg"]);
                            //获取单个因子的水质类别
                            string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                            if (!string.IsNullOrEmpty(GetWQL))
                            {
                                //获取评价因子
                                O_EvaluateFactorCodes += pollutantCode + ";";
                                O_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                            }
                        }
                        if (!string.IsNullOrEmpty(O_EvaluateFactorCodes))
                        {
                            O_EvaluateFactorCodes = O_EvaluateFactorCodes.Substring(0, O_EvaluateFactorCodes.Length - 1);
                            //获取水质类别值
                            w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, O_EvaluateFactorCodes, O_WQIValues));
                            string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Roman, O_EvaluateFactorCodes, O_WQIValues);
                            dt.Rows[i]["RealClass"] = w_Class + "类";
                        }
                        else
                        {
                            w_Value = 0;
                            dt.Rows[i]["RealClass"] = "/";
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()) && !string.IsNullOrEmpty(w_Value.ToString()))
                        {
                            if (w_Value <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && w_Value > 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "达标";
                            }
                            else if (w_Value == 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "/";
                            }
                            else
                            {
                                dt.Rows[i]["StandardStatus"] = "未达标";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "/";
                        }
                        #endregion
                        #region 同比
                        DataRow[] dr_ContemporaryComparison = dt_ContemporaryComparison.Select("PointId=" + pointId);
                        if (dr_ContemporaryComparison.Length > 0)
                        {
                            for (int j = 0; j < dr_ContemporaryComparison.Length; j++)
                            {
                                string pollutantCode = dr_ContemporaryComparison[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_ContemporaryComparison[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    T_EvaluateFactorCodes += pollutantCode + ";";
                                    T_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(T_EvaluateFactorCodes))
                            {
                                T_EvaluateFactorCodes = T_EvaluateFactorCodes.Substring(0, T_EvaluateFactorCodes.Length - 1);
                                t_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, T_EvaluateFactorCodes, T_WQIValues));
                            }
                            if (t_Value > 0)
                            {
                                string range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(t_Value)) / Convert.ToDouble(t_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["TCompare"] = range;
                                if (w_Value > t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "上升";
                                }
                                else if (w_Value < t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "下降";
                                }
                                else if (w_Value == t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["TCompare"] = "/";
                                dt.Rows[i]["TTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["TCompare"] = "/";
                            dt.Rows[i]["TTrend"] = "/";
                        }
                        #endregion
                        #region 环比
                        DataRow[] dr_Chain = dt_Chain.Select("PointId=" + pointId);
                        if (dr_Chain.Length > 0)
                        {
                            for (int j = 0; j < dr_Chain.Length; j++)
                            {
                                string pollutantCode = dr_Chain[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_Chain[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    H_EvaluateFactorCodes += pollutantCode + ";";
                                    H_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(H_EvaluateFactorCodes))
                            {
                                H_EvaluateFactorCodes = H_EvaluateFactorCodes.Substring(0, H_EvaluateFactorCodes.Length - 1);
                                H_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, H_EvaluateFactorCodes, H_WQIValues));
                            }
                            if (H_Value > 0)
                            {
                                string h_Range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(H_Value)) / Convert.ToDouble(H_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["HCompare"] = h_Range;
                                if (w_Value > H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "上升";
                                }
                                else if (w_Value < H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "下降";
                                }
                                else if (w_Value == H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["HCompare"] = "/";
                                dt.Rows[i]["HTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["TCompare"] = "/";
                            dt.Rows[i]["TTrend"] = "/";
                        }
                        #endregion
                    }
                    else
                    {
                        dt.Rows[i]["RealClass"] = "/";
                        dt.Rows[i]["TTrend"] = "/";
                        dt.Rows[i]["TCompare"] = "/";
                        dt.Rows[i]["HTrend"] = "/";
                        dt.Rows[i]["HCompare"] = "/";
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                    dt.Rows[i][yearFrom.ToString()] = "第" + weekOfYearFrom + "周-第" + weekOfYearTo + "周";
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }

        public DataView WaterQualityWeekStandardStatus(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom, int yearTo, int weekOfYearTo)
        {
            //建立返回DataTable
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("RealClass", typeof(string));//水质类别
            dt.Columns.Add(yearFrom.ToString(), typeof(string));//年
            dt.Columns.Add("TTrend", typeof(string));//同期趋势
            dt.Columns.Add("TCompare", typeof(string));//同期比较
            dt.Columns.Add("HTrend", typeof(string));//环比趋势
            dt.Columns.Add("HCompare", typeof(string));//环比比较
            dt.Columns.Add("StandardStatus", typeof(string));//达标情况

            try
            {
                DataQueryByWeekService weekDataService = new DataQueryByWeekService();
                WaterQualityService WaterQuality = new WaterQualityService();
                //原始周数据
                DataTable dt_Original = weekDataService.GetWeekStatisticalData(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo).Table;
                //同比周数据
                DataTable dt_ContemporaryComparison = weekDataService.GetWeekStatisticalData(portIds, factors, yearFrom - 1, weekOfYearFrom, yearTo - 1, weekOfYearTo).Table;
                int diffValue = weekOfYearTo - weekOfYearFrom + 1;
                int hEndYear = 0;
                int hEndWeek = 0;
                int hStartYear = 0;
                int hStartWeek = 0;
                if (weekOfYearTo != 1)
                {
                    if (weekOfYearTo - 1 >= diffValue)
                    {
                        hEndYear = yearFrom;
                        hEndWeek = weekOfYearFrom - 1;
                        hStartYear = yearFrom;
                        hStartWeek = weekOfYearFrom - diffValue;
                    }
                    else
                    {
                        hEndYear = yearFrom;
                        hEndWeek = weekOfYearFrom - 1;
                        hStartYear = yearFrom - 1;
                        int diff = diffValue - weekOfYearFrom + 1;
                        DateTime dateTime = new DateTime(hStartYear, 1, 1);
                        TimeSpan ts = dateTime.AddYears(1) - dateTime;
                        int TotalWeek = ts.Days / 7 + 1;
                        hStartWeek = TotalWeek - diff + 1;
                    }
                }
                else
                {
                    hEndYear = yearFrom - 1;
                    hStartYear = yearFrom - 1;
                    int diff = diffValue - weekOfYearFrom + 1;
                    DateTime dateTime = new DateTime(hStartYear, 1, 1);
                    TimeSpan ts = dateTime.AddYears(1) - dateTime;
                    int TotalWeek = ts.Days / 7 + 1;
                    hEndWeek = TotalWeek;
                    hStartWeek = hEndWeek - diff + 1;
                }
                //环比周数据
                DataTable dt_Chain = weekDataService.GetWeekStatisticalData(portIds, factors, hStartYear, hStartWeek, hEndYear, hEndWeek).Table;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 参数
                    int w_Value = 0;
                    int t_Value = 0;
                    int H_Value = 0;
                    string O_EvaluateFactorCodes = "";
                    string T_EvaluateFactorCodes = "";
                    string H_EvaluateFactorCodes = "";
                    Dictionary<string, Int32> O_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> T_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> H_WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    #endregion
                    //根据站点id获取原始数据
                    DataRow[] dr_Original = dt_Original.Select("PointId=" + pointId);
                    if (dr_Original.Length > 0)
                    {
                        #region 原始
                        for (int j = 0; j < dr_Original.Length; j++)
                        {
                            //获取因子code
                            string pollutantCode = dr_Original[j]["PollutantCode"].ToString();
                            //获取因子浓度
                            if (dr_Original[j]["Value_Avg"] != DBNull.Value)
                            {
                                decimal pollutantValue = Convert.ToDecimal(dr_Original[j]["Value_Avg"]);
                                //获取单个因子的水质类别
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    //获取评价因子
                                    O_EvaluateFactorCodes += pollutantCode + ";";
                                    O_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(O_EvaluateFactorCodes))
                        {
                            O_EvaluateFactorCodes = O_EvaluateFactorCodes.Substring(0, O_EvaluateFactorCodes.Length - 1);
                            //获取水质类别值
                            w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, O_EvaluateFactorCodes, O_WQIValues));
                            string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Roman, O_EvaluateFactorCodes, O_WQIValues);
                            dt.Rows[i]["RealClass"] = w_Class + "类";
                        }
                        else
                        {
                            dt.Rows[i]["RealClass"] = "/";
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()) && !string.IsNullOrEmpty(w_Value.ToString()))
                        {
                            if (w_Value <= Convert.ToInt32(dt.Rows[i]["IEQI"]))
                            {
                                dt.Rows[i]["StandardStatus"] = "达标";
                            }
                            else
                            {
                                dt.Rows[i]["StandardStatus"] = "未达标";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "/";
                        }
                        #endregion
                        #region 同比
                        DataRow[] dr_ContemporaryComparison = dt_ContemporaryComparison.Select("PointId=" + pointId);
                        if (dr_ContemporaryComparison.Length > 0)
                        {
                            for (int j = 0; j < dr_ContemporaryComparison.Length; j++)
                            {
                                string pollutantCode = dr_ContemporaryComparison[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_ContemporaryComparison[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    T_EvaluateFactorCodes += pollutantCode + ";";
                                    T_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(T_EvaluateFactorCodes))
                            {
                                T_EvaluateFactorCodes = T_EvaluateFactorCodes.Substring(0, T_EvaluateFactorCodes.Length - 1);
                                t_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, T_EvaluateFactorCodes, T_WQIValues));
                            }
                            if (t_Value > 0)
                            {
                                string range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(t_Value)) / Convert.ToDouble(t_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["TCompare"] = range;
                                if (w_Value > t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "上升";
                                }
                                else if (w_Value < t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "下降";
                                }
                                else if (w_Value == t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["TCompare"] = "/";
                                dt.Rows[i]["TTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["TCompare"] = "/";
                            dt.Rows[i]["TTrend"] = "/";
                        }
                        #endregion
                        #region 环比
                        DataRow[] dr_Chain = dt_Chain.Select("PointId=" + pointId);
                        if (dr_Chain.Length > 0)
                        {
                            for (int j = 0; j < dr_Chain.Length; j++)
                            {
                                string pollutantCode = dr_Chain[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_Chain[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    H_EvaluateFactorCodes += pollutantCode + ";";
                                    H_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(H_EvaluateFactorCodes))
                            {
                                H_EvaluateFactorCodes = H_EvaluateFactorCodes.Substring(0, H_EvaluateFactorCodes.Length - 1);
                                H_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, H_EvaluateFactorCodes, H_WQIValues));
                            }
                            if (H_Value > 0)
                            {
                                string h_Range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(H_Value)) / Convert.ToDouble(H_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["HCompare"] = h_Range;
                                if (w_Value > H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "上升";
                                }
                                else if (w_Value < H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "下降";
                                }
                                else if (w_Value == H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["HCompare"] = "/";
                                dt.Rows[i]["HTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["HCompare"] = "/";
                            dt.Rows[i]["HTrend"] = "/";
                        }
                        #endregion
                    }
                    else
                    {
                        dt.Rows[i]["RealClass"] = "/";
                        dt.Rows[i]["TTrend"] = "/";
                        dt.Rows[i]["TCompare"] = "/";
                        dt.Rows[i]["HTrend"] = "/";
                        dt.Rows[i]["HCompare"] = "/";
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                    dt.Rows[i][yearFrom.ToString()] = "第" + weekOfYearFrom + "周-第" + weekOfYearTo + "周";
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }
        /// <summary>
        /// 水质分析
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView WeekWaterQuality(string[] portIds, IList<IPollutant> factors, int yearFrom, int weekOfYearFrom, int yearTo, int weekOfYearTo)
        {
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("StandardStatus", typeof(string));

            try
            {
                WaterQualityService WaterQuality = new WaterQualityService();
                DataQueryByWeekService weekDataService = new DataQueryByWeekService();
                DataTable dt_Values = weekDataService.GetWeekStatisticalData(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo).Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int WQL = 0;
                    string EvaluateFactorCodes = "";
                    Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    DataRow[] drs = dt_Values.Select("PointId=" + pointId);
                    for (int j = 0; j < drs.Length; j++)
                    {
                        string pollutantCode = drs[j]["PollutantCode"].ToString();
                        decimal pollutantValue = Convert.ToDecimal(drs[j]["Value_Avg"]);
                        string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            EvaluateFactorCodes += pollutantCode + ";";
                            WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                        }
                    }
                    if (!string.IsNullOrEmpty(EvaluateFactorCodes))
                    {
                        EvaluateFactorCodes = EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);
                        string GetWQL = WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            WQL = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues));
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()))
                    {
                        if (WQL <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && WQL > 0)
                        {
                            dt.Rows[i]["StandardStatus"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "未达标";
                        }
                    }
                    else
                    {
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }
        public DataView WeekWaterQuality(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom, int yearTo, int weekOfYearTo)
        {
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("StandardStatus", typeof(string));

            try
            {
                WaterQualityService WaterQuality = new WaterQualityService();
                DataQueryByWeekService weekDataService = new DataQueryByWeekService();
                DataTable dt_Values = weekDataService.GetWeekStatisticalData(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo).Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int WQL = 0;
                    string EvaluateFactorCodes = "";
                    Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    DataRow[] drs = dt_Values.Select("PointId=" + pointId);
                    for (int j = 0; j < drs.Length; j++)
                    {
                        string pollutantCode = drs[j]["PollutantCode"].ToString();
                        decimal pollutantValue = Convert.ToDecimal(drs[j]["Value_Avg"]);
                        string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            EvaluateFactorCodes += pollutantCode + ";";
                            WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                        }
                    }
                    if (!string.IsNullOrEmpty(EvaluateFactorCodes))
                    {
                        EvaluateFactorCodes = EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);
                        string GetWQL = WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            WQL = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues));
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()))
                    {
                        if (WQL <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && WQL > 0)
                        {
                            dt.Rows[i]["StandardStatus"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "未达标";
                        }
                    }
                    else
                    {
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }
        #endregion

        #region 月数据
        /// <summary>
        /// 月水质达标统计
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView WaterQualityMonthStandardStatus(string[] portIds, IList<IPollutant> factors, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo)
        {
            //建立返回DataTable
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("RealClass", typeof(string));//水质类别
            dt.Columns.Add(yearFrom.ToString(), typeof(string));//年
            dt.Columns.Add("TTrend", typeof(string));//同期趋势
            dt.Columns.Add("TCompare", typeof(string));//同期比较
            dt.Columns.Add("HTrend", typeof(string));//环比趋势
            dt.Columns.Add("HCompare", typeof(string));//环比比较
            dt.Columns.Add("StandardStatus", typeof(string));//达标情况

            try
            {
                DataQueryByMonthService monthDataService = new DataQueryByMonthService();
                WaterQualityService WaterQuality = new WaterQualityService();
                //原始周数据
                DataTable dt_Original = monthDataService.GetMonthStatisticalData(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo).Table;
                //同比周数据
                DataTable dt_ContemporaryComparison = monthDataService.GetMonthStatisticalData(portIds, factors, yearFrom - 1, monthOfYearFrom, yearTo - 1, monthOfYearTo).Table;
                int diffValue = monthOfYearTo - monthOfYearFrom + 1;
                int hEndYear = 0;
                int hEndWeek = 0;
                int hStartYear = 0;
                int hStartWeek = 0;
                if (monthOfYearTo != 1)
                {
                    if (monthOfYearTo - 1 >= diffValue)
                    {
                        hEndYear = yearFrom;
                        hEndWeek = monthOfYearFrom - 1;
                        hStartYear = yearFrom;
                        hStartWeek = monthOfYearFrom - diffValue;
                    }
                    else
                    {
                        hEndYear = yearFrom;
                        hEndWeek = monthOfYearFrom - 1;
                        hStartYear = yearFrom - 1;
                        int diff = diffValue - monthOfYearFrom + 1;
                        DateTime dateTime = new DateTime(hStartYear, 1, 1);
                        TimeSpan ts = dateTime.AddYears(1) - dateTime;
                        int TotalWeek = ts.Days / 7 + 1;
                        hStartWeek = TotalWeek - diff + 1;
                    }
                }
                else
                {
                    hEndYear = yearFrom - 1;
                    hStartYear = yearFrom - 1;
                    int diff = diffValue - monthOfYearFrom + 1;
                    DateTime dateTime = new DateTime(hStartYear, 1, 1);
                    TimeSpan ts = dateTime.AddYears(1) - dateTime;
                    int TotalWeek = ts.Days / 7 + 1;
                    hEndWeek = TotalWeek;
                    hStartWeek = hEndWeek - diff + 1;
                }
                //环比周数据
                DataTable dt_Chain = monthDataService.GetMonthStatisticalData(portIds, factors, hStartYear, hStartWeek, hEndYear, hEndWeek).Table;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 参数
                    int w_Value = 0;
                    int t_Value = 0;
                    int H_Value = 0;
                    string O_EvaluateFactorCodes = "";
                    string T_EvaluateFactorCodes = "";
                    string H_EvaluateFactorCodes = "";
                    Dictionary<string, Int32> O_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> T_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> H_WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    #endregion
                    //根据站点id获取原始数据
                    DataRow[] dr_Original = dt_Original.Select("PointId=" + pointId);
                    if (dr_Original.Length > 0)
                    {
                        #region 原始
                        for (int j = 0; j < dr_Original.Length; j++)
                        {
                            //获取因子code
                            string pollutantCode = dr_Original[j]["PollutantCode"].ToString();
                            //获取因子浓度
                            decimal pollutantValue = Convert.ToDecimal(dr_Original[j]["Value_Avg"]);
                            //获取单个因子的水质类别
                            string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                            if (!string.IsNullOrEmpty(GetWQL))
                            {
                                //获取评价因子
                                O_EvaluateFactorCodes += pollutantCode + ";";
                                O_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                            }
                        }
                        if (!string.IsNullOrEmpty(O_EvaluateFactorCodes))
                        {
                            O_EvaluateFactorCodes = O_EvaluateFactorCodes.Substring(0, O_EvaluateFactorCodes.Length - 1);
                            //获取水质类别值
                            w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, O_EvaluateFactorCodes, O_WQIValues));
                            string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Roman, O_EvaluateFactorCodes, O_WQIValues);
                            dt.Rows[i]["RealClass"] = w_Class + "类";
                        }
                        else
                        {
                            dt.Rows[i]["RealClass"] = "/";
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()) && !string.IsNullOrEmpty(w_Value.ToString()))
                        {
                            if (w_Value <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && w_Value > 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "达标";
                            }
                            else if (w_Value == 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "/";
                            }
                            else
                            {
                                dt.Rows[i]["StandardStatus"] = "未达标";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "/";
                        }
                        #endregion
                        #region 同比
                        DataRow[] dr_ContemporaryComparison = dt_ContemporaryComparison.Select("PointId=" + pointId);
                        if (dr_ContemporaryComparison.Length > 0)
                        {
                            for (int j = 0; j < dr_ContemporaryComparison.Length; j++)
                            {
                                string pollutantCode = dr_ContemporaryComparison[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_ContemporaryComparison[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    T_EvaluateFactorCodes += pollutantCode + ";";
                                    T_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(T_EvaluateFactorCodes))
                            {
                                T_EvaluateFactorCodes = T_EvaluateFactorCodes.Substring(0, T_EvaluateFactorCodes.Length - 1);
                                t_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, T_EvaluateFactorCodes, T_WQIValues));
                            }
                            if (t_Value > 0)
                            {
                                string range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(t_Value)) / Convert.ToDouble(t_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["TCompare"] = range;
                                if (w_Value > t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "上升";
                                }
                                else if (w_Value < t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "下降";
                                }
                                else if (w_Value == t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["TCompare"] = "/";
                                dt.Rows[i]["TTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["TCompare"] = "/";
                            dt.Rows[i]["TTrend"] = "/";
                        }
                        #endregion
                        #region 环比
                        DataRow[] dr_Chain = dt_Chain.Select("PointId=" + pointId);
                        if (dr_Chain.Length > 0)
                        {
                            for (int j = 0; j < dr_Chain.Length; j++)
                            {
                                string pollutantCode = dr_Chain[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_Chain[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    H_EvaluateFactorCodes += pollutantCode + ";";
                                    H_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(H_EvaluateFactorCodes))
                            {
                                H_EvaluateFactorCodes = H_EvaluateFactorCodes.Substring(0, H_EvaluateFactorCodes.Length - 1);
                                H_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, H_EvaluateFactorCodes, H_WQIValues));
                            }
                            if (H_Value > 0)
                            {
                                string h_Range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(H_Value)) / Convert.ToDouble(H_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["HCompare"] = h_Range;
                                if (w_Value > H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "上升";
                                }
                                else if (w_Value < H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "下降";
                                }
                                else if (w_Value == H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["HCompare"] = "/";
                                dt.Rows[i]["HTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["HCompare"] = "/";
                            dt.Rows[i]["HTrend"] = "/";
                        }
                        #endregion
                    }
                    else
                    {
                        dt.Rows[i]["RealClass"] = "/";
                        dt.Rows[i]["TTrend"] = "/";
                        dt.Rows[i]["TCompare"] = "/";
                        dt.Rows[i]["HTrend"] = "/";
                        dt.Rows[i]["HCompare"] = "/";
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                    dt.Rows[i][yearFrom.ToString()] = monthOfYearFrom + "月-" + monthOfYearTo + "月";
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }
        public DataView WaterQualityMonthStandardStatus(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo)
        {
            //建立返回DataTable
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("RealClass", typeof(string));//水质类别
            dt.Columns.Add(yearFrom.ToString(), typeof(string));//年
            dt.Columns.Add("TTrend", typeof(string));//同期趋势
            dt.Columns.Add("TCompare", typeof(string));//同期比较
            dt.Columns.Add("HTrend", typeof(string));//环比趋势
            dt.Columns.Add("HCompare", typeof(string));//环比比较
            dt.Columns.Add("StandardStatus", typeof(string));//达标情况

            try
            {
                DataQueryByMonthService monthDataService = new DataQueryByMonthService();
                WaterQualityService WaterQuality = new WaterQualityService();
                //原始周数据
                DataTable dt_Original = monthDataService.GetMonthStatisticalData(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo).Table;
                //同比周数据
                DataTable dt_ContemporaryComparison = monthDataService.GetMonthStatisticalData(portIds, factors, yearFrom - 1, monthOfYearFrom, yearTo - 1, monthOfYearTo).Table;
                int diffValue = monthOfYearTo - monthOfYearFrom + 1;
                int hEndYear = 0;
                int hEndWeek = 0;
                int hStartYear = 0;
                int hStartWeek = 0;
                if (monthOfYearTo != 1)
                {
                    if (monthOfYearTo - 1 >= diffValue)
                    {
                        hEndYear = yearFrom;
                        hEndWeek = monthOfYearFrom - 1;
                        hStartYear = yearFrom;
                        hStartWeek = monthOfYearFrom - diffValue;
                    }
                    else
                    {
                        hEndYear = yearFrom;
                        hEndWeek = monthOfYearFrom - 1;
                        hStartYear = yearFrom - 1;
                        int diff = diffValue - monthOfYearFrom + 1;
                        DateTime dateTime = new DateTime(hStartYear, 1, 1);
                        TimeSpan ts = dateTime.AddYears(1) - dateTime;
                        int TotalWeek = ts.Days / 7 + 1;
                        hStartWeek = TotalWeek - diff + 1;
                    }
                }
                else
                {
                    hEndYear = yearFrom - 1;
                    hStartYear = yearFrom - 1;
                    int diff = diffValue - monthOfYearFrom + 1;
                    DateTime dateTime = new DateTime(hStartYear, 1, 1);
                    TimeSpan ts = dateTime.AddYears(1) - dateTime;
                    int TotalWeek = ts.Days / 7 + 1;
                    hEndWeek = TotalWeek;
                    hStartWeek = hEndWeek - diff + 1;
                }
                //环比周数据
                DataTable dt_Chain = monthDataService.GetMonthStatisticalData(portIds, factors, hStartYear, hStartWeek, hEndYear, hEndWeek).Table;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 参数
                    int w_Value = 0;
                    int t_Value = 0;
                    int H_Value = 0;
                    string O_EvaluateFactorCodes = "";
                    string T_EvaluateFactorCodes = "";
                    string H_EvaluateFactorCodes = "";
                    Dictionary<string, Int32> O_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> T_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> H_WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    #endregion
                    //根据站点id获取原始数据
                    DataRow[] dr_Original = dt_Original.Select("PointId=" + pointId);
                    if (dr_Original.Length > 0)
                    {
                        #region 原始
                        for (int j = 0; j < dr_Original.Length; j++)
                        {
                            //获取因子code
                            string pollutantCode = dr_Original[j]["PollutantCode"].ToString();
                            //获取因子浓度
                            decimal pollutantValue = Convert.ToDecimal(dr_Original[j]["Value_Avg"]);
                            //获取单个因子的水质类别
                            string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                            if (!string.IsNullOrEmpty(GetWQL))
                            {
                                //获取评价因子
                                O_EvaluateFactorCodes += pollutantCode + ";";
                                O_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                            }
                        }
                        if (!string.IsNullOrEmpty(O_EvaluateFactorCodes))
                        {
                            O_EvaluateFactorCodes = O_EvaluateFactorCodes.Substring(0, O_EvaluateFactorCodes.Length - 1);
                            //获取水质类别值
                            w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, O_EvaluateFactorCodes, O_WQIValues));
                            string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Roman, O_EvaluateFactorCodes, O_WQIValues);
                            dt.Rows[i]["RealClass"] = w_Class + "类";
                        }
                        else
                        {
                            dt.Rows[i]["RealClass"] = "/";
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()) && !string.IsNullOrEmpty(w_Value.ToString()))
                        {
                            if (w_Value <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && w_Value > 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "达标";
                            }
                            else if (w_Value == 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "/";
                            }
                            else
                            {
                                dt.Rows[i]["StandardStatus"] = "未达标";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "/";
                        }
                        #endregion
                        #region 同比
                        DataRow[] dr_ContemporaryComparison = dt_ContemporaryComparison.Select("PointId=" + pointId);
                        if (dr_ContemporaryComparison.Length > 0)
                        {
                            for (int j = 0; j < dr_ContemporaryComparison.Length; j++)
                            {
                                string pollutantCode = dr_ContemporaryComparison[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_ContemporaryComparison[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    T_EvaluateFactorCodes += pollutantCode + ";";
                                    T_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(T_EvaluateFactorCodes))
                            {
                                T_EvaluateFactorCodes = T_EvaluateFactorCodes.Substring(0, T_EvaluateFactorCodes.Length - 1);
                                t_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, T_EvaluateFactorCodes, T_WQIValues));
                            }
                            if (t_Value > 0)
                            {
                                string range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(t_Value)) / Convert.ToDouble(t_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["TCompare"] = range;
                                if (w_Value > t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "上升";
                                }
                                else if (w_Value < t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "下降";
                                }
                                else if (w_Value == t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["TCompare"] = "/";
                                dt.Rows[i]["TTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["TCompare"] = "/";
                            dt.Rows[i]["TTrend"] = "/";
                        }
                        #endregion
                        #region 环比
                        DataRow[] dr_Chain = dt_Chain.Select("PointId=" + pointId);
                        if (dr_Chain.Length > 0)
                        {
                            for (int j = 0; j < dr_Chain.Length; j++)
                            {
                                string pollutantCode = dr_Chain[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_Chain[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    H_EvaluateFactorCodes += pollutantCode + ";";
                                    H_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(H_EvaluateFactorCodes))
                            {
                                H_EvaluateFactorCodes = H_EvaluateFactorCodes.Substring(0, H_EvaluateFactorCodes.Length - 1);
                                H_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, H_EvaluateFactorCodes, H_WQIValues));
                            }
                            if (H_Value > 0)
                            {
                                string h_Range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(H_Value)) / Convert.ToDouble(H_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["HCompare"] = h_Range;
                                if (w_Value > H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "上升";
                                }
                                else if (w_Value < H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "下降";
                                }
                                else if (w_Value == H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["HCompare"] = "/";
                                dt.Rows[i]["HTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["HCompare"] = "/";
                            dt.Rows[i]["HTrend"] = "/";
                        }
                        #endregion
                    }
                    else
                    {
                        dt.Rows[i]["RealClass"] = "/";
                        dt.Rows[i]["TTrend"] = "/";
                        dt.Rows[i]["TCompare"] = "/";
                        dt.Rows[i]["HTrend"] = "/";
                        dt.Rows[i]["HCompare"] = "/";
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                    dt.Rows[i][yearFrom.ToString()] = monthOfYearFrom + "月-" + monthOfYearTo + "月";
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }
        /// <summary>
        /// 水质分析
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView MonthWaterQuality(string[] portIds, IList<IPollutant> factors, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo)
        {
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("StandardStatus", typeof(string));

            try
            {
                int WQL = 0;
                string EvaluateFactorCodes = "";
                Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
                WaterQualityService WaterQuality = new WaterQualityService();
                DataQueryByMonthService monthDataService = new DataQueryByMonthService();
                DataTable dt_Values = monthDataService.GetMonthStatisticalData(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo).Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    DataRow[] drs = dt_Values.Select("PointId=" + pointId);
                    for (int j = 0; j < drs.Length; j++)
                    {
                        string pollutantCode = drs[j]["PollutantCode"].ToString();
                        EvaluateFactorCodes = pollutantCode + ";";
                        decimal pollutantValue = Convert.ToDecimal(drs[j]["Value_Avg"]);
                        WQL = Convert.ToInt32(WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level));
                        WQIValues.Add(pollutantCode, WQL);
                    }
                    EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);
                    int w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues));
                    string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Class, EvaluateFactorCodes, WQIValues);
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()) && !string.IsNullOrEmpty(w_Value.ToString()))
                    {
                        if (WQL <= Convert.ToInt32(dt.Rows[i]["IEQI"]))
                        {
                            dt.Rows[i]["StandardStatus"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "未达标";
                        }
                    }
                    else
                    {
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                }
            }
            catch (Exception ex) { }
            return dt.DefaultView;
        }
        public DataView MonthWaterQuality(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo)
        {
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("StandardStatus", typeof(string));

            try
            {
                int WQL = 0;
                string EvaluateFactorCodes = "";
                Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
                WaterQualityService WaterQuality = new WaterQualityService();
                DataQueryByMonthService monthDataService = new DataQueryByMonthService();
                DataTable dt_Values = monthDataService.GetMonthStatisticalData(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo).Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    DataRow[] drs = dt_Values.Select("PointId=" + pointId);
                    for (int j = 0; j < drs.Length; j++)
                    {
                        string pollutantCode = drs[j]["PollutantCode"].ToString();
                        EvaluateFactorCodes = pollutantCode + ";";
                        decimal pollutantValue = Convert.ToDecimal(drs[j]["Value_Avg"]);
                        WQL = Convert.ToInt32(WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level));
                        WQIValues.Add(pollutantCode, WQL);
                    }
                    EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);
                    int w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues));
                    string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Class, EvaluateFactorCodes, WQIValues);
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()) && !string.IsNullOrEmpty(w_Value.ToString()))
                    {
                        if (WQL <= Convert.ToInt32(dt.Rows[i]["IEQI"]))
                        {
                            dt.Rows[i]["StandardStatus"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "未达标";
                        }
                    }
                    else
                    {
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                }
            }
            catch (Exception ex) { }
            return dt.DefaultView;
        }
        #endregion

        #region 季数据
        /// <summary>
        /// 季水质达标统计
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView WaterQualitySeasonStandardStatus(string[] portIds, IList<IPollutant> factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            //建立返回DataTable
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("RealClass", typeof(string));//水质类别
            dt.Columns.Add(yearFrom.ToString(), typeof(string));//年
            dt.Columns.Add("TTrend", typeof(string));//同期趋势
            dt.Columns.Add("TCompare", typeof(string));//同期比较
            dt.Columns.Add("HTrend", typeof(string));//环比趋势
            dt.Columns.Add("HCompare", typeof(string));//环比比较
            dt.Columns.Add("StandardStatus", typeof(string));//达标情况

            try
            {
                DataQueryBySeasonService seasonDataService = new DataQueryBySeasonService();
                WaterQualityService WaterQuality = new WaterQualityService();
                //原始周数据
                DataTable dt_Original = seasonDataService.GetSeasonStatisticalData(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo).Table;
                //同比周数据
                DataTable dt_ContemporaryComparison = seasonDataService.GetSeasonStatisticalData(portIds, factors, yearFrom - 1, seasonOfYearFrom, yearTo - 1, seasonOfYearTo).Table;
                int diffValue = seasonOfYearTo - seasonOfYearFrom + 1;
                int hEndYear = 0;
                int hEndWeek = 0;
                int hStartYear = 0;
                int hStartWeek = 0;
                if (seasonOfYearTo != 1)
                {
                    if (seasonOfYearTo - 1 >= diffValue)
                    {
                        hEndYear = yearFrom;
                        hEndWeek = seasonOfYearFrom - 1;
                        hStartYear = yearFrom;
                        hStartWeek = seasonOfYearFrom - diffValue;
                    }
                    else
                    {
                        hEndYear = yearFrom;
                        hEndWeek = seasonOfYearFrom - 1;
                        hStartYear = yearFrom - 1;
                        int diff = diffValue - seasonOfYearFrom + 1;
                        DateTime dateTime = new DateTime(hStartYear, 1, 1);
                        TimeSpan ts = dateTime.AddYears(1) - dateTime;
                        int TotalWeek = ts.Days / 7 + 1;
                        hStartWeek = TotalWeek - diff + 1;
                    }
                }
                else
                {
                    hEndYear = yearFrom - 1;
                    hStartYear = yearFrom - 1;
                    int diff = diffValue - seasonOfYearFrom + 1;
                    DateTime dateTime = new DateTime(hStartYear, 1, 1);
                    TimeSpan ts = dateTime.AddYears(1) - dateTime;
                    int TotalWeek = ts.Days / 7 + 1;
                    hEndWeek = TotalWeek;
                    hStartWeek = hEndWeek - diff + 1;
                }
                //环比周数据
                DataTable dt_Chain = seasonDataService.GetSeasonStatisticalData(portIds, factors, hStartYear, hStartWeek, hEndYear, hEndWeek).Table;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 参数
                    int w_Value = 0;
                    int t_Value = 0;
                    int H_Value = 0;
                    string O_EvaluateFactorCodes = "";
                    string T_EvaluateFactorCodes = "";
                    string H_EvaluateFactorCodes = "";
                    Dictionary<string, Int32> O_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> T_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> H_WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    #endregion
                    //根据站点id获取原始数据
                    DataRow[] dr_Original = dt_Original.Select("PointId=" + pointId);
                    if (dr_Original.Length > 0)
                    {
                        #region 原始
                        for (int j = 0; j < dr_Original.Length; j++)
                        {
                            //获取因子code
                            string pollutantCode = dr_Original[j]["PollutantCode"].ToString();
                            //获取因子浓度
                            decimal pollutantValue = Convert.ToDecimal(dr_Original[j]["Value_Avg"]);
                            //获取单个因子的水质类别
                            string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                            if (!string.IsNullOrEmpty(GetWQL))
                            {
                                //获取评价因子
                                O_EvaluateFactorCodes += pollutantCode + ";";
                                O_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                            }
                        }
                        if (!string.IsNullOrEmpty(O_EvaluateFactorCodes))
                        {
                            O_EvaluateFactorCodes = O_EvaluateFactorCodes.Substring(0, O_EvaluateFactorCodes.Length - 1);
                            //获取水质类别值
                            w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, O_EvaluateFactorCodes, O_WQIValues));
                            string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Roman, O_EvaluateFactorCodes, O_WQIValues);
                            dt.Rows[i]["RealClass"] = w_Class + "类";
                        }
                        else
                        {
                            dt.Rows[i]["RealClass"] = "/";
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()) && !string.IsNullOrEmpty(w_Value.ToString()))
                        {
                            if (w_Value <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && w_Value > 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "达标";
                            }
                            else if (w_Value == 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "/";
                            }
                            else
                            {
                                dt.Rows[i]["StandardStatus"] = "未达标";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "/";
                        }
                        #endregion
                        #region 同比
                        DataRow[] dr_ContemporaryComparison = dt_ContemporaryComparison.Select("PointId=" + pointId);
                        if (dr_ContemporaryComparison.Length > 0)
                        {
                            for (int j = 0; j < dr_ContemporaryComparison.Length; j++)
                            {
                                string pollutantCode = dr_ContemporaryComparison[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_ContemporaryComparison[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    T_EvaluateFactorCodes += pollutantCode + ";";
                                    T_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(T_EvaluateFactorCodes))
                            {
                                T_EvaluateFactorCodes = T_EvaluateFactorCodes.Substring(0, T_EvaluateFactorCodes.Length - 1);
                                t_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, T_EvaluateFactorCodes, T_WQIValues));
                            }
                            if (t_Value > 0)
                            {
                                string range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(t_Value)) / Convert.ToDouble(t_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["TCompare"] = range;
                                if (w_Value > t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "上升";
                                }
                                else if (w_Value < t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "下降";
                                }
                                else if (w_Value == t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["TCompare"] = "/";
                                dt.Rows[i]["TTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["TCompare"] = "/";
                            dt.Rows[i]["TTrend"] = "/";
                        }
                        #endregion
                        #region 环比
                        DataRow[] dr_Chain = dt_Chain.Select("PointId=" + pointId);
                        if (dr_Chain.Length > 0)
                        {
                            for (int j = 0; j < dr_Chain.Length; j++)
                            {
                                string pollutantCode = dr_Chain[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_Chain[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    H_EvaluateFactorCodes += pollutantCode + ";";
                                    H_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(H_EvaluateFactorCodes))
                            {
                                H_EvaluateFactorCodes = H_EvaluateFactorCodes.Substring(0, H_EvaluateFactorCodes.Length - 1);
                                H_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, H_EvaluateFactorCodes, H_WQIValues));
                            }
                            if (H_Value > 0)
                            {
                                string h_Range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(H_Value)) / Convert.ToDouble(H_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["HCompare"] = h_Range;
                                if (w_Value > H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "上升";
                                }
                                else if (w_Value < H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "下降";
                                }
                                else if (w_Value == H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["HCompare"] = "/";
                                dt.Rows[i]["HTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["TCompare"] = "/";
                            dt.Rows[i]["TTrend"] = "/";
                        }
                        #endregion
                    }
                    else
                    {
                        dt.Rows[i]["RealClass"] = "/";
                        dt.Rows[i]["TTrend"] = "/";
                        dt.Rows[i]["TCompare"] = "/";
                        dt.Rows[i]["HTrend"] = "/";
                        dt.Rows[i]["HCompare"] = "/";
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                    dt.Rows[i][yearFrom.ToString()] = "第" + seasonOfYearFrom + "季度-第" + seasonOfYearTo + "季度";
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }
        public DataView WaterQualitySeasonStandardStatus(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            //建立返回DataTable
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("RealClass", typeof(string));//水质类别
            dt.Columns.Add(yearFrom.ToString(), typeof(string));//年
            dt.Columns.Add("TTrend", typeof(string));//同期趋势
            dt.Columns.Add("TCompare", typeof(string));//同期比较
            dt.Columns.Add("HTrend", typeof(string));//环比趋势
            dt.Columns.Add("HCompare", typeof(string));//环比比较
            dt.Columns.Add("StandardStatus", typeof(string));//达标情况

            try
            {
                DataQueryBySeasonService seasonDataService = new DataQueryBySeasonService();
                WaterQualityService WaterQuality = new WaterQualityService();
                //原始周数据
                DataTable dt_Original = seasonDataService.GetSeasonStatisticalData(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo).Table;
                //同比周数据
                DataTable dt_ContemporaryComparison = seasonDataService.GetSeasonStatisticalData(portIds, factors, yearFrom - 1, seasonOfYearFrom, yearTo - 1, seasonOfYearTo).Table;
                int diffValue = seasonOfYearTo - seasonOfYearFrom + 1;
                int hEndYear = 0;
                int hEndWeek = 0;
                int hStartYear = 0;
                int hStartWeek = 0;
                if (seasonOfYearTo != 1)
                {
                    if (seasonOfYearTo - 1 >= diffValue)
                    {
                        hEndYear = yearFrom;
                        hEndWeek = seasonOfYearFrom - 1;
                        hStartYear = yearFrom;
                        hStartWeek = seasonOfYearFrom - diffValue;
                    }
                    else
                    {
                        hEndYear = yearFrom;
                        hEndWeek = seasonOfYearFrom - 1;
                        hStartYear = yearFrom - 1;
                        int diff = diffValue - seasonOfYearFrom + 1;
                        DateTime dateTime = new DateTime(hStartYear, 1, 1);
                        TimeSpan ts = dateTime.AddYears(1) - dateTime;
                        int TotalWeek = ts.Days / 7 + 1;
                        hStartWeek = TotalWeek - diff + 1;
                    }
                }
                else
                {
                    hEndYear = yearFrom - 1;
                    hStartYear = yearFrom - 1;
                    int diff = diffValue - seasonOfYearFrom + 1;
                    DateTime dateTime = new DateTime(hStartYear, 1, 1);
                    TimeSpan ts = dateTime.AddYears(1) - dateTime;
                    int TotalWeek = ts.Days / 7 + 1;
                    hEndWeek = TotalWeek;
                    hStartWeek = hEndWeek - diff + 1;
                }
                //环比周数据
                DataTable dt_Chain = seasonDataService.GetSeasonStatisticalData(portIds, factors, hStartYear, hStartWeek, hEndYear, hEndWeek).Table;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 参数
                    int w_Value = 0;
                    int t_Value = 0;
                    int H_Value = 0;
                    string O_EvaluateFactorCodes = "";
                    string T_EvaluateFactorCodes = "";
                    string H_EvaluateFactorCodes = "";
                    Dictionary<string, Int32> O_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> T_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> H_WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    #endregion
                    //根据站点id获取原始数据
                    DataRow[] dr_Original = dt_Original.Select("PointId=" + pointId);
                    if (dr_Original.Length > 0)
                    {
                        #region 原始
                        for (int j = 0; j < dr_Original.Length; j++)
                        {
                            //获取因子code
                            string pollutantCode = dr_Original[j]["PollutantCode"].ToString();
                            //获取因子浓度
                            decimal pollutantValue = Convert.ToDecimal(dr_Original[j]["Value_Avg"]);
                            //获取单个因子的水质类别
                            string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                            if (!string.IsNullOrEmpty(GetWQL))
                            {
                                //获取评价因子
                                O_EvaluateFactorCodes += pollutantCode + ";";
                                O_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                            }
                        }
                        if (!string.IsNullOrEmpty(O_EvaluateFactorCodes))
                        {
                            O_EvaluateFactorCodes = O_EvaluateFactorCodes.Substring(0, O_EvaluateFactorCodes.Length - 1);
                            //获取水质类别值
                            w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, O_EvaluateFactorCodes, O_WQIValues));
                            string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Roman, O_EvaluateFactorCodes, O_WQIValues);
                            dt.Rows[i]["RealClass"] = w_Class + "类";
                        }
                        else
                        {
                            dt.Rows[i]["RealClass"] = "/";
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()) && !string.IsNullOrEmpty(w_Value.ToString()))
                        {
                            if (w_Value <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && w_Value > 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "达标";
                            }
                            else if (w_Value == 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "/";
                            }
                            else
                            {
                                dt.Rows[i]["StandardStatus"] = "未达标";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "/";
                        }
                        #endregion
                        #region 同比
                        DataRow[] dr_ContemporaryComparison = dt_ContemporaryComparison.Select("PointId=" + pointId);
                        if (dr_ContemporaryComparison.Length > 0)
                        {
                            for (int j = 0; j < dr_ContemporaryComparison.Length; j++)
                            {
                                string pollutantCode = dr_ContemporaryComparison[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_ContemporaryComparison[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    T_EvaluateFactorCodes += pollutantCode + ";";
                                    T_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(T_EvaluateFactorCodes))
                            {
                                T_EvaluateFactorCodes = T_EvaluateFactorCodes.Substring(0, T_EvaluateFactorCodes.Length - 1);
                                t_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, T_EvaluateFactorCodes, T_WQIValues));
                            }
                            if (t_Value > 0)
                            {
                                string range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(t_Value)) / Convert.ToDouble(t_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["TCompare"] = range;
                                if (w_Value > t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "上升";
                                }
                                else if (w_Value < t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "下降";
                                }
                                else if (w_Value == t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["TCompare"] = "/";
                                dt.Rows[i]["TTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["TCompare"] = "/";
                            dt.Rows[i]["TTrend"] = "/";
                        }
                        #endregion
                        #region 环比
                        DataRow[] dr_Chain = dt_Chain.Select("PointId=" + pointId);
                        if (dr_Chain.Length > 0)
                        {
                            for (int j = 0; j < dr_Chain.Length; j++)
                            {
                                string pollutantCode = dr_Chain[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_Chain[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    H_EvaluateFactorCodes += pollutantCode + ";";
                                    H_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(H_EvaluateFactorCodes))
                            {
                                H_EvaluateFactorCodes = H_EvaluateFactorCodes.Substring(0, H_EvaluateFactorCodes.Length - 1);
                                H_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, H_EvaluateFactorCodes, H_WQIValues));
                            }
                            if (H_Value > 0)
                            {
                                string h_Range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(H_Value)) / Convert.ToDouble(H_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["HCompare"] = h_Range;
                                if (w_Value > H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "上升";
                                }
                                else if (w_Value < H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "下降";
                                }
                                else if (w_Value == H_Value)
                                {
                                    dt.Rows[i]["HTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["HCompare"] = "/";
                                dt.Rows[i]["HTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["HCompare"] = "/";
                            dt.Rows[i]["HTrend"] = "/";
                        }
                        #endregion
                    }
                    else
                    {
                        dt.Rows[i]["RealClass"] = "/";
                        dt.Rows[i]["TTrend"] = "/";
                        dt.Rows[i]["TCompare"] = "/";
                        dt.Rows[i]["HTrend"] = "/";
                        dt.Rows[i]["HCompare"] = "/";
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                    dt.Rows[i][yearFrom.ToString()] = "第" + seasonOfYearFrom + "季度-第" + seasonOfYearTo + "季度";
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }

        /// <summary>
        /// 水质分析
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView SeasonWaterQuality(string[] portIds, IList<IPollutant> factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("StandardStatus", typeof(string));

            try
            {
                WaterQualityService WaterQuality = new WaterQualityService();
                DataQueryBySeasonService seasonDataService = new DataQueryBySeasonService();
                DataTable dt_Values = seasonDataService.GetSeasonStatisticalData(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo).Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int WQL = 0;
                    string EvaluateFactorCodes = "";
                    Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    DataRow[] drs = dt_Values.Select("PointId=" + pointId);
                    for (int j = 0; j < drs.Length; j++)
                    {
                        string pollutantCode = drs[j]["PollutantCode"].ToString();
                        decimal pollutantValue = Convert.ToDecimal(drs[j]["Value_Avg"]);
                        string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            EvaluateFactorCodes += pollutantCode + ";";
                            WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                        }
                    }
                    if (!string.IsNullOrEmpty(EvaluateFactorCodes))
                    {
                        EvaluateFactorCodes = EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);
                        string GetWQL = WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            WQL = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues));
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()))
                    {
                        if (WQL <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && WQL > 0)
                        {
                            dt.Rows[i]["StandardStatus"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "未达标";
                        }
                    }
                    else
                    {
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }
        public DataView SeasonWaterQuality(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("StandardStatus", typeof(string));

            try
            {
                WaterQualityService WaterQuality = new WaterQualityService();
                DataQueryBySeasonService seasonDataService = new DataQueryBySeasonService();
                DataTable dt_Values = seasonDataService.GetSeasonStatisticalData(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo).Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int WQL = 0;
                    string EvaluateFactorCodes = "";
                    Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    DataRow[] drs = dt_Values.Select("PointId=" + pointId);
                    for (int j = 0; j < drs.Length; j++)
                    {
                        string pollutantCode = drs[j]["PollutantCode"].ToString();
                        decimal pollutantValue = Convert.ToDecimal(drs[j]["Value_Avg"]);
                        string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            EvaluateFactorCodes += pollutantCode + ";";
                            WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                        }
                    }
                    if (!string.IsNullOrEmpty(EvaluateFactorCodes))
                    {
                        EvaluateFactorCodes = EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);
                        string GetWQL = WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            WQL = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues));
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()))
                    {
                        if (WQL <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && WQL > 0)
                        {
                            dt.Rows[i]["StandardStatus"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "未达标";
                        }
                    }
                    else
                    {
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }
        #endregion

        #region 年数据
        /// <summary>
        /// 年水质达标统计
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView WaterQualityYearStandardStatus(string[] portIds, IList<IPollutant> factors, int dateStart, int dateEnd)
        {
            //建立返回DataTable
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("RealClass", typeof(string));//水质类别
            dt.Columns.Add(dateStart.ToString(), typeof(string));//年
            dt.Columns.Add("TTrend", typeof(string));//同期趋势
            dt.Columns.Add("TCompare", typeof(string));//同期比较
            dt.Columns.Add("StandardStatus", typeof(string));//达标情况

            try
            {
                DataQueryByYearService yearDataService = new DataQueryByYearService();
                WaterQualityService WaterQuality = new WaterQualityService();
                //原始年均值数据
                DataTable dt_Original = yearDataService.GetYearStatisticalData(portIds, factors, dateStart, dateEnd).Table;
                //同比年均值数据
                DataTable dt_ContemporaryComparison = yearDataService.GetYearStatisticalData(portIds, factors, dateStart - 1, dateEnd - 1).Table;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 参数
                    int w_Value = 0;
                    int t_Value = 0;
                    int H_Value = 0;
                    string O_EvaluateFactorCodes = "";
                    string T_EvaluateFactorCodes = "";
                    string H_EvaluateFactorCodes = "";
                    Dictionary<string, Int32> O_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> T_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> H_WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    #endregion
                    //根据站点id获取原始数据
                    DataRow[] dr_Original = dt_Original.Select("PointId=" + pointId);
                    if (dr_Original.Length > 0)
                    {
                        #region 原始
                        for (int j = 0; j < dr_Original.Length; j++)
                        {
                            //获取因子code
                            string pollutantCode = dr_Original[j]["PollutantCode"].ToString();
                            //获取因子浓度
                            decimal pollutantValue = Convert.ToDecimal(dr_Original[j]["Value_Avg"]);
                            //获取单个因子的水质类别
                            string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                            if (!string.IsNullOrEmpty(GetWQL))
                            {
                                //获取评价因子
                                O_EvaluateFactorCodes += pollutantCode + ";";
                                O_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                            }
                        }
                        if (!string.IsNullOrEmpty(O_EvaluateFactorCodes))
                        {
                            O_EvaluateFactorCodes = O_EvaluateFactorCodes.Substring(0, O_EvaluateFactorCodes.Length - 1);
                            //获取水质类别值
                            w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, O_EvaluateFactorCodes, O_WQIValues));
                            string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Roman, O_EvaluateFactorCodes, O_WQIValues);
                            dt.Rows[i]["RealClass"] = w_Class + "类";
                        }
                        else
                        {
                            dt.Rows[i]["RealClass"] = "/";
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()) && !string.IsNullOrEmpty(w_Value.ToString()))
                        {
                            if (w_Value <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && w_Value > 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "达标";
                            }
                            else if (w_Value == 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "/";
                            }
                            else
                            {
                                dt.Rows[i]["StandardStatus"] = "未达标";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "/";
                        }
                        #endregion
                        #region 同比
                        DataRow[] dr_ContemporaryComparison = dt_ContemporaryComparison.Select("PointId=" + pointId);
                        if (dr_ContemporaryComparison.Length > 0)
                        {
                            for (int j = 0; j < dr_ContemporaryComparison.Length; j++)
                            {
                                string pollutantCode = dr_ContemporaryComparison[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_ContemporaryComparison[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    T_EvaluateFactorCodes += pollutantCode + ";";
                                    T_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(T_EvaluateFactorCodes))
                            {
                                T_EvaluateFactorCodes = T_EvaluateFactorCodes.Substring(0, T_EvaluateFactorCodes.Length - 1);
                                t_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, T_EvaluateFactorCodes, T_WQIValues));
                            }
                            if (t_Value > 0)
                            {
                                string range = Math.Round((((Convert.ToDouble(w_Value) - Convert.ToDouble(t_Value)) / Convert.ToDouble(t_Value)) * 100), 1).ToString() + "%";
                                dt.Rows[i]["TCompare"] = range;
                                if (w_Value > t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "上升";
                                }
                                else if (w_Value < t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "下降";
                                }
                                else if (w_Value == t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["TCompare"] = "/";
                                dt.Rows[i]["TTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["TCompare"] = "/";
                            dt.Rows[i]["TTrend"] = "/";
                        }
                        #endregion
                    }
                    else
                    {
                        dt.Rows[i]["RealClass"] = "/";
                        dt.Rows[i]["TTrend"] = "/";
                        dt.Rows[i]["TCompare"] = "/";
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                    dt.Rows[i][dateStart.ToString()] = dateStart.ToString() + "年-" + dateEnd.ToString() + "年";
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }
        public DataView WaterQualityYearStandardStatus(string[] portIds, string[] factors, int dateStart, int dateEnd)
        {
            //建立返回DataTable
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("RealClass", typeof(string));//水质类别
            dt.Columns.Add(dateStart.ToString(), typeof(string));//年
            dt.Columns.Add("TTrend", typeof(string));//同期趋势
            dt.Columns.Add("TCompare", typeof(string));//同期比较
            dt.Columns.Add("StandardStatus", typeof(string));//达标情况
            dt.Columns.Add("HTrend", typeof(string));//环比趋势
            dt.Columns.Add("HCompare", typeof(string));//环比比较

            try
            {
                DataQueryByYearService yearDataService = new DataQueryByYearService();
                WaterQualityService WaterQuality = new WaterQualityService();
                //原始年均值数据
                DataTable dt_Original = yearDataService.GetYearStatisticalData(portIds, factors, dateStart, dateEnd).Table;
                //同比年均值数据
                DataTable dt_ContemporaryComparison = yearDataService.GetYearStatisticalData(portIds, factors, dateStart - 1, dateEnd - 1).Table;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 参数
                    int w_Value = 0;
                    int t_Value = 0;
                    int H_Value = 0;
                    string O_EvaluateFactorCodes = "";
                    string T_EvaluateFactorCodes = "";
                    string H_EvaluateFactorCodes = "";
                    Dictionary<string, Int32> O_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> T_WQIValues = new Dictionary<string, int>();
                    Dictionary<string, Int32> H_WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    #endregion
                    //根据站点id获取原始数据
                    DataRow[] dr_Original = dt_Original.Select("PointId=" + pointId);
                    if (dr_Original.Length > 0)
                    {
                        #region 原始
                        for (int j = 0; j < dr_Original.Length; j++)
                        {
                            //获取因子code
                            string pollutantCode = dr_Original[j]["PollutantCode"].ToString();
                            //获取因子浓度
                            decimal pollutantValue = Convert.ToDecimal(dr_Original[j]["Value_Avg"]);
                            //获取单个因子的水质类别
                            string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                            if (!string.IsNullOrEmpty(GetWQL))
                            {
                                //获取评价因子
                                O_EvaluateFactorCodes += pollutantCode + ";";
                                O_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                            }
                        }
                        if (!string.IsNullOrEmpty(O_EvaluateFactorCodes))
                        {
                            O_EvaluateFactorCodes = O_EvaluateFactorCodes.Substring(0, O_EvaluateFactorCodes.Length - 1);
                            //获取水质类别值
                            w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, O_EvaluateFactorCodes, O_WQIValues));
                            string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Roman, O_EvaluateFactorCodes, O_WQIValues);
                            dt.Rows[i]["RealClass"] = w_Class + "类";
                        }
                        else
                        {
                            dt.Rows[i]["RealClass"] = "/";
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()) && !string.IsNullOrEmpty(w_Value.ToString()))
                        {
                            if (w_Value <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && w_Value > 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "达标";
                            }
                            else if (w_Value == 0)
                            {
                                dt.Rows[i]["StandardStatus"] = "/";
                            }
                            else
                            {
                                dt.Rows[i]["StandardStatus"] = "未达标";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "/";
                        }
                        #endregion
                        #region 同比
                        DataRow[] dr_ContemporaryComparison = dt_ContemporaryComparison.Select("PointId=" + pointId);
                        if (dr_ContemporaryComparison.Length > 0)
                        {
                            for (int j = 0; j < dr_ContemporaryComparison.Length; j++)
                            {
                                string pollutantCode = dr_ContemporaryComparison[j]["PollutantCode"].ToString();
                                decimal pollutantValue = Convert.ToDecimal(dr_ContemporaryComparison[j]["Value_Avg"]);
                                string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    T_EvaluateFactorCodes += pollutantCode + ";";
                                    T_WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                                }
                            }
                            if (!string.IsNullOrEmpty(T_EvaluateFactorCodes))
                            {
                                T_EvaluateFactorCodes = T_EvaluateFactorCodes.Substring(0, T_EvaluateFactorCodes.Length - 1);
                                t_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, T_EvaluateFactorCodes, T_WQIValues));
                            }
                            if (t_Value > 0)
                            {
                                string range = (((Convert.ToDouble(w_Value) - Convert.ToDouble(t_Value)) / Convert.ToDouble(t_Value)) * 100).ToString() + "%";
                                dt.Rows[i]["TCompare"] = range;
                                if (w_Value > t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "上升";
                                }
                                else if (w_Value < t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "下降";
                                }
                                else if (w_Value == t_Value)
                                {
                                    dt.Rows[i]["TTrend"] = "持平";
                                }
                            }
                            else
                            {
                                dt.Rows[i]["TCompare"] = "/";
                                dt.Rows[i]["TTrend"] = "/";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["TCompare"] = "/";
                            dt.Rows[i]["TTrend"] = "/";
                        }
                        #endregion
                    }
                    else
                    {
                        dt.Rows[i]["RealClass"] = "/";
                        dt.Rows[i]["TTrend"] = "/";
                        dt.Rows[i]["TCompare"] = "/";
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                    dt.Rows[i]["HCompare"] = "--";
                    dt.Rows[i]["HTrend"] = "--";
                    dt.Rows[i][dateStart.ToString()] = dateStart.ToString() + "年-" + dateEnd.ToString() + "年";
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }
        /// <summary>
        /// 水质分析
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView YearWaterQuality(string[] portIds, IList<IPollutant> factors, int yearFrom, int yearTo)
        {
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("StandardStatus", typeof(string));

            try
            {
                WaterQualityService WaterQuality = new WaterQualityService();
                DataQueryByYearService yearDataService = new DataQueryByYearService();
                DataTable dt_Values = yearDataService.GetYearStatisticalData(portIds, factors, yearFrom, yearTo).Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int WQL = 0;
                    string EvaluateFactorCodes = "";
                    Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    DataRow[] drs = dt_Values.Select("PointId=" + pointId);
                    for (int j = 0; j < drs.Length; j++)
                    {
                        string pollutantCode = drs[j]["PollutantCode"].ToString();
                        decimal pollutantValue = Convert.ToDecimal(drs[j]["Value_Avg"]);
                        string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            EvaluateFactorCodes += pollutantCode + ";";
                            WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                        }
                    }
                    if (!string.IsNullOrEmpty(EvaluateFactorCodes))
                    {
                        EvaluateFactorCodes = EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);
                        string GetWQL = WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            WQL = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues));
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()))
                    {
                        if (WQL <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && WQL > 0)
                        {
                            dt.Rows[i]["StandardStatus"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "未达标";
                        }
                    }
                    else
                    {
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }
        public DataView YearWaterQuality(string[] portIds, string[] factors, int yearFrom, int yearTo)
        {
            DataTable dt = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            dt.Columns.Add("StandardStatus", typeof(string));

            try
            {
                WaterQualityService WaterQuality = new WaterQualityService();
                DataQueryByYearService yearDataService = new DataQueryByYearService();
                DataTable dt_Values = yearDataService.GetYearStatisticalData(portIds, factors, yearFrom, yearTo).Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int WQL = 0;
                    string EvaluateFactorCodes = "";
                    Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
                    string pointId = dt.Rows[i]["PointId"].ToString();
                    DataRow[] drs = dt_Values.Select("PointId=" + pointId);
                    for (int j = 0; j < drs.Length; j++)
                    {
                        string pollutantCode = drs[j]["PollutantCode"].ToString();
                        decimal pollutantValue = Convert.ToDecimal(drs[j]["Value_Avg"]);
                        string GetWQL = WaterQuality.GetWQL(pollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            EvaluateFactorCodes += pollutantCode + ";";
                            WQIValues.Add(pollutantCode, Convert.ToInt32(GetWQL));
                        }
                    }
                    if (!string.IsNullOrEmpty(EvaluateFactorCodes))
                    {
                        EvaluateFactorCodes = EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);
                        string GetWQL = WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues);
                        if (!string.IsNullOrEmpty(GetWQL))
                        {
                            WQL = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQIValues));
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IEQI"].ToString()))
                    {
                        if (WQL <= Convert.ToInt32(dt.Rows[i]["IEQI"]) && WQL > 0)
                        {
                            dt.Rows[i]["StandardStatus"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["StandardStatus"] = "未达标";
                        }
                    }
                    else
                    {
                        dt.Rows[i]["StandardStatus"] = "/";
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return dt.DefaultView;
        }
        #endregion

        /// <summary>
        /// 水质达标率
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="recordTotal">返回数</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        public DataTable Dock(DateTime startTime, DateTime endTime, int pageSize, int pageNo, out int recordTotal, string orderby)
        {
            recordTotal = 0;
            //审核日数据仓储层
            DayReportRepository r_DataQueryByDay = new DayReportRepository();
            //监测站点服务层
            MonitoringPointWaterService s_MonitoringPointWater = new MonitoringPointWaterService();
            //获取所有启用的地表水饮用水源地点位列表
            MonitoringPointEntity[] s_MonitoringPoints = s_MonitoringPointWater.RetrieveMPListByWaterSource().ToArray();
            //获取站点Id
            string[] pointIds = s_MonitoringPoints.Select(p => p.PointId.ToString()).ToArray();
            //获取站点GUID
            //string[] pointGuids = s_MonitoringPoints.Select(p => p.MonitoringPointUid.ToString()).ToArray();
            //评价因子
            string[] factors = new string[] { };
            //foreach (string pointGuid in pointGuids)
            //{
            //    string EvaluateFactorList = s_MonitoringPointWater.RetrieveCalEQIPollutantList(pointGuid);
            //    factors = EvaluateFactorList.Split(';');
            //}
            string EvaluateFactorList = "w01009;w21003;w01001;w01019;w21011;w21001";
            factors = EvaluateFactorList.Split(';');
            DataTable dt = r_DataQueryByDay.GetDataPager(pointIds, factors, startTime, endTime, pageSize, pageNo, out recordTotal, orderby).Table;

            //数据总数
            int Total = dt.Rows.Count;
            //达标值
            int Standards = 0;
            int range = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["EQI"].ToString()))
                {
                    string a = dt.Rows[i]["EQI"].ToString();
                    int eqi = Convert.ToInt32(dt.Rows[i]["EQI"]);
                    if (eqi >= 0 && eqi <= 100)
                    {
                        Standards++;
                    }
                }
                else
                {
                    Total--;
                }
            }
            //未达标
            int unStandards = Total - Standards;
            //达标率
            if (Total > 0)
            {
                range = (Standards / Total) * 100;
            }

            DataTable dt_ReturnValue = new DataTable();
            dt_ReturnValue.Columns.Add("Total", typeof(string));
            dt_ReturnValue.Columns.Add("Standards", typeof(string));
            dt_ReturnValue.Columns.Add("unStandards", typeof(string));
            dt_ReturnValue.Columns.Add("range", typeof(string));

            DataRow dr = dt_ReturnValue.NewRow();
            dr["Total"] = Total.ToString();
            dr["Standards"] = Standards.ToString();
            dr["unStandards"] = unStandards.ToString();
            dr["range"] = range.ToString();
            dt_ReturnValue.Rows.Add(dr);

            return dt_ReturnValue;
        }
    }
}
