using SmartEP.BaseInfoRepository.Channel;
using SmartEP.BaseInfoRepository.Standard;
using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.MonitoringBusinessRepository.Water;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water
{
    /// <summary>
    /// 名称：WaterCustomDataAnalysisService.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-31
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：自定义数据分析类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterCustomDataAnalysisService
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();

        #region 获取年度平均综合污染指数比较表
        /// <summary>
        /// 获取年度平均综合污染指数比较表（多选年）
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="years">年数据</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// 年均值列，如2015年均值
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetYearAvgSyntheticalPollutantData(string[] pointIds, int[] years, int pageSize, int pageNo,
            out int recordTotal, string orderBy = "PointId")
        {
            YearReportRepository yearReportRepository = Singleton<YearReportRepository>.GetInstance();
            WaterQualityRepository waterQualityRepository = Singleton<WaterQualityRepository>.GetInstance();//水质标准处理仓储层
            WaterQualityService waterQualityService = new WaterQualityService();//提供污染等级服务（如空气质量指数、水质等级）
            string[] factors = GetWaterQualityPollutantCodes();//取得参与评价的水质因子 WaterQualityService.GetWaterQualityPollutant();//取得参与评价的因子//GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            DataTable dtYearReport = yearReportRepository.GetDataPager(pointIds, factors, years.Min(), years.Max(), pageSize, pageNo, out recordTotal, orderBy).Table;//获取年报数据
            years = years.OrderBy(t => t).ToArray();
            DataTable dtNew = CreateYearAvgPollutantDataTable(years);//生成年度平均综合污染指数的表
            DataTable dtWaterAnalyzeData = waterQualityRepository.GetWaterAnalyzeData(pointIds).Table;//根据站点获取水质分析数据
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(pointIds);//根据测点Id数组获取测点Id和对应的因子Code
            pointIds = pointIds.Distinct().ToArray();

            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                DataRow drNew = dtNew.NewRow();
                DataRow[] drsYearReport = dtYearReport.Select(string.Format("PointId='{0}'", pointId));
                DataRow[] drsWaterAnalyzeData = dtWaterAnalyzeData.Select(string.Format("PointId='{0}'", pointId));
                WaterQualityClass waterQualityClass = WaterQualityClass.One;
                WaterPointCalWQType waterPointCalWQType = WaterPointCalWQType.River;//河流、湖泊
                IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                     ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                pollutantCodeList = pollutantCodeList.Distinct().ToList();
                drNew["PointId"] = pointId;

                ////如果不存在该测点的数据，则跳过
                //if (drsYearReport.Length <= 0)
                //{
                //    continue;
                //}
                if (drsWaterAnalyzeData != null && drsWaterAnalyzeData.Length > 0)
                {
                    DataRow drWaterAnalyzeData = drsWaterAnalyzeData[0];
                    string strIEQI = drWaterAnalyzeData["IEQI"].ToString().Trim();//水质类别（数字）
                    waterPointCalWQType = drWaterAnalyzeData["CalEQIType"].ToString().Trim() == "河流" ? WaterPointCalWQType.River : WaterPointCalWQType.Lake;

                    switch (strIEQI)
                    {
                        case "1":
                            waterQualityClass = WaterQualityClass.One;
                            break;
                        case "2":
                            waterQualityClass = WaterQualityClass.Two;
                            break;
                        case "3":
                            waterQualityClass = WaterQualityClass.Three;
                            break;
                        case "4":
                            waterQualityClass = WaterQualityClass.Four;
                            break;
                        case "5":
                            waterQualityClass = WaterQualityClass.Five;
                            break;
                        case "6":
                            waterQualityClass = WaterQualityClass.BadFive;
                            break;
                        default:
                            break;
                    }
                }
                foreach (DataRow drYearReport in drsYearReport)
                {
                    string year = drYearReport["Year"].ToString();
                    string evaluateFactorCodes = string.Empty;
                    Dictionary<string, decimal> WQIValuesDictionary = new Dictionary<string, decimal>();//因子指数列表（Key:PollutantCode、Value:分指数）

                    foreach (string pollutantCode in pollutantCodeList)
                    {
                        //不是参与评价的水质因子，则跳过
                        if (!factors.Contains(pollutantCode))
                        {
                            continue;
                        }

                        decimal pollutantValue = decimal.TryParse(drYearReport[pollutantCode].ToString(), out pollutantValue) ? pollutantValue : 0;
                        decimal WQI = waterQualityService.GetWQI(pollutantCode, pollutantValue, waterQualityClass, EQITimeType.One, waterPointCalWQType);//计算水质污染指数
                        evaluateFactorCodes += pollutantCode + ";";
                        WQIValuesDictionary.Add(pollutantCode, WQI);
                    }
                    evaluateFactorCodes = evaluateFactorCodes.TrimEnd(';');
                    if (evaluateFactorCodes.Length > 0 && WQIValuesDictionary.Count > 0)
                    {
                        string strWQI_Avg = waterQualityService.GetWQI_Avg(evaluateFactorCodes, WQIValuesDictionary);
                        decimal WQI_Avg;
                        if (decimal.TryParse(strWQI_Avg, out WQI_Avg))
                        {
                            drYearReport["EQI"] = WQI_Avg;
                        }
                    }
                    if (dtNew.Columns.Contains(year + "年均值"))
                    {
                        drNew[year + "年均值"] = drYearReport["EQI"];//年度平均综合污染指数
                    }
                }
                dtNew.Rows.Add(drNew);//添加新行
            }
            recordTotal = dtNew.Rows.Count;
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }

        /// <summary>
        /// 获取年度平均综合污染指数比较表（单选年）
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="year">年数据</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// 年均值列，如2015年均值
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetYearAvgSyntheticalPollutantData(string[] pointIds, int year, int pageSize, int pageNo,
            out int recordTotal, string orderBy = "PointId")
        {
            int[] years = { year - 2, year - 1, year };
            return GetYearAvgSyntheticalPollutantData(pointIds, years, pageSize, pageNo, out recordTotal, orderBy);
        }
        #endregion

        #region 数据同环比分析（无因子）
        /// <summary>
        /// 获取时均同环比比较表
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="dtmHour">时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// SiteTypeName：站点类型名称
        /// PointId：测点Id
        /// 时均值列，如2015-08-30 15:00
        /// SequentialPollutantIndex：综合污染指数
        /// PollutantLevelSorting：污染程度排序
        /// WaterQualityClassification：水质类别
        /// LinkRelativeRatio_PollutantTrend：环比污染趋势
        /// LinkRelativeRatio_RiseRate：环比上升比例（%）
        /// blankspaceColumn：空白列</returns>
        public DataView GetHourWithTheSequentialData(string[] pointIds, DateTime dtmHour, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            HourReportRepository hourReportRepository = Singleton<HourReportRepository>.GetInstance();
            string[] factors = GetWaterQualityPollutantCodes();//取得参与评价的水质因子GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            string[] strDateTimes = new string[3];//时间周期数组
            strDateTimes[0] = dtmHour.AddYears(-1).ToString("yyyy-MM-dd HH:00");//上一年同期
            strDateTimes[1] = dtmHour.AddHours(-4).ToString("yyyy-MM-dd HH:00");//上一个周期
            strDateTimes[2] = dtmHour.ToString("yyyy-MM-dd HH:00");//当前周期
            DataTable dtNew = CreateSequentialDataTableByDateTime(strDateTimes, "时");//按时间生成数据同环比分析表
            DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
            DataTable dtHourReport0 = hourReportRepository.GetDataPager(pointIds, factors, Convert.ToDateTime(dtmHour.AddYears(-1).ToString("yyyy-MM-dd HH:00")), Convert.ToDateTime(dtmHour.AddYears(-1).ToString("yyyy-MM-dd HH:59")), pageSize, pageNo, out recordTotal, orderBy).Table;//获取时报数据
            DataTable dtHourReport1 = hourReportRepository.GetDataPager(pointIds, factors, Convert.ToDateTime(dtmHour.AddHours(-4).ToString("yyyy-MM-dd HH:00")), Convert.ToDateTime(dtmHour.AddHours(-4).ToString("yyyy-MM-dd HH:59")), pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtHourReport2 = hourReportRepository.GetDataPager(pointIds, factors, Convert.ToDateTime(dtmHour.ToString("yyyy-MM-dd HH:00")), Convert.ToDateTime(dtmHour.ToString("yyyy-MM-dd HH:59")), pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtHourReport = UnionDataTable(dtHourReport0, dtHourReport1, dtHourReport2);//拼接数据表

            AddNewRowToDataTableByNoFactor(pointIds, factors, strDateTimes, "时", "Tstamp", string.Empty, dtNew, dtHourReport, dtAvgIndex);
            dvAvgIndex = dtAvgIndex.AsDataView();
            recordTotal = dtNew.Rows.Count;
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }

        /// <summary>
        /// 获取日均同环比比较表
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="dtmDay">日期</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 日均值列，如2015-08-30
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetDayWithTheSequentialData(string[] pointIds, DateTime dtmDay, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            DayReportRepository dayReportRepository = Singleton<DayReportRepository>.GetInstance();
            string[] factors = GetWaterQualityPollutantCodes();//取得参与评价的水质因子GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            string[] strDateTimes = new string[3];
            strDateTimes[0] = dtmDay.AddYears(-1).ToString("yyyy-MM-dd");
            strDateTimes[1] = dtmDay.AddDays(-1).ToString("yyyy-MM-dd");
            strDateTimes[2] = dtmDay.ToString("yyyy-MM-dd");
            DataTable dtNew = CreateSequentialDataTableByDateTime(strDateTimes, "日");//按时间生成数据同环比分析表
            DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
            DataTable dtDayReport0 = dayReportRepository.GetDataPager(pointIds, factors, dtmDay.AddYears(-1), dtmDay.AddYears(-1), pageSize, pageNo, out recordTotal, orderBy).Table;//获取日报数据
            DataTable dtDayReport1 = dayReportRepository.GetDataPager(pointIds, factors, dtmDay.AddDays(-1), dtmDay.AddDays(-1), pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtDayReport2 = dayReportRepository.GetDataPager(pointIds, factors, dtmDay, dtmDay, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtDayReport = UnionDataTable(dtDayReport0, dtDayReport1, dtDayReport2);//拼接数据表

            AddNewRowToDataTableByNoFactor(pointIds, factors, strDateTimes, "日", "DateTime", string.Empty, dtNew, dtDayReport, dtAvgIndex);
            dvAvgIndex = dtAvgIndex.AsDataView();
            recordTotal = dtNew.Rows.Count;
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }

        /// <summary>
        /// 获取周均同环比比较表
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="year">年</param>
        /// <param name="weekOfYear">周</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 周均值列，如2015-8周
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetWeekWithTheSequentialData(string[] pointIds, int year, int weekOfYear, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            WeekReportRepository weekReportRepository = Singleton<WeekReportRepository>.GetInstance();
            string[] factors = GetWaterQualityPollutantCodes();//取得参与评价的水质因子GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            string[] strDateTimes = new string[3];
            int lastYear = (weekOfYear == 1) ? year - 1 : year;//上一个周期的年
            int lastWeekOfYear = (weekOfYear == 1) ? (int)Math.Ceiling((new DateTime(year - 1, 12, 31).DayOfYear
                                                + (int)new DateTime(year - 1, 1, 1).DayOfWeek - 1
                                                + 7 - (int)new DateTime(year - 1, 12, 31).DayOfWeek) * 1M / 7) : weekOfYear - 1;//上一个周期的周
            strDateTimes[0] = (year - 1).ToString() + "-" + weekOfYear.ToString();
            strDateTimes[1] = lastYear.ToString() + "-" + lastWeekOfYear.ToString();
            strDateTimes[2] = year.ToString() + "-" + weekOfYear.ToString();
            DataTable dtNew = CreateSequentialDataTableByDateTime(strDateTimes, "周");//按时间生成数据同环比分析表
            DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
            DataTable dtWeekReport0 = weekReportRepository.GetDataPager(pointIds, factors, year - 1, weekOfYear, year - 1, weekOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;//获取周报数据
            DataTable dtWeekReport1 = weekReportRepository.GetDataPager(pointIds, factors, lastYear, lastWeekOfYear, lastYear, lastWeekOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtWeekReport2 = weekReportRepository.GetDataPager(pointIds, factors, year, weekOfYear, year, weekOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtWeekReport = UnionDataTable(dtWeekReport0, dtWeekReport1, dtWeekReport2);//拼接数据表

            AddNewRowToDataTableByNoFactor(pointIds, factors, strDateTimes, "周", "Year", "WeekOfYear", dtNew, dtWeekReport, dtAvgIndex);
            dvAvgIndex = dtAvgIndex.AsDataView();
            recordTotal = dtNew.Rows.Count;
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }

        /// <summary>
        /// 获取月均同环比比较表
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 月均值列，如2015-8月
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetMonthWithTheSequentialData(string[] pointIds, int year, int month, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            MonthReportRepository monthReportRepository = Singleton<MonthReportRepository>.GetInstance();
            string[] factors = GetWaterQualityPollutantCodes();//取得参与评价的水质因子GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            string[] strDateTimes = new string[3];
            DateTime lastYearMonth = new DateTime(year, month, 1).AddMonths(-1);//上一个周期的日期
            strDateTimes[0] = new DateTime(year, month, 1).AddYears(-1).ToString("yyyy-M");
            strDateTimes[1] = lastYearMonth.ToString("yyyy-M");
            strDateTimes[2] = new DateTime(year, month, 1).ToString("yyyy-M");
            DataTable dtNew = CreateSequentialDataTableByDateTime(strDateTimes, "月");//按时间生成数据同环比分析表
            DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
            DataTable dtMonthReport0 = monthReportRepository.GetDataPager(pointIds, factors, year - 1, month, year - 1, month, pageSize, pageNo, out recordTotal, orderBy).Table;//获取月报数据
            DataTable dtMonthReport1 = monthReportRepository.GetDataPager(pointIds, factors, lastYearMonth.Year, lastYearMonth.Month, lastYearMonth.Year, lastYearMonth.Month, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtMonthReport2 = monthReportRepository.GetDataPager(pointIds, factors, year, month, year, month, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtMonthReport = UnionDataTable(dtMonthReport0, dtMonthReport1, dtMonthReport2);//拼接数据表
            //DataTable dtStatisticalData = monthReportRepository.GetStatisticalData(pointIds, factors, year, 1, year, 1).Table;

            AddNewRowToDataTableByNoFactor(pointIds, factors, strDateTimes, "月", "Year", "MonthOfYear", dtNew, dtMonthReport, dtAvgIndex);
            dvAvgIndex = dtAvgIndex.AsDataView();
            recordTotal = dtNew.Rows.Count;
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }

        /// <summary>
        /// 获取季均同环比比较表
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="year">年</param>
        /// <param name="seasonOfYear">季</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 季均值列，如2015-4季
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetSeasonWithTheSequentialData(string[] pointIds, int year, int seasonOfYear, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            SeasonReportRepository seasonReportRepository = Singleton<SeasonReportRepository>.GetInstance();
            string[] factors = GetWaterQualityPollutantCodes();//取得参与评价的水质因子GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            string[] strDateTimes = new string[3];
            int lastYear = (seasonOfYear == 1) ? year - 1 : year;//上一个周期的年
            int lastSeasonOfYear = (seasonOfYear == 1) ? 4 : seasonOfYear - 1;//上一个周期的季
            strDateTimes[0] = (year - 1).ToString() + "-" + seasonOfYear.ToString();
            strDateTimes[1] = lastYear.ToString() + "-" + lastSeasonOfYear.ToString();
            strDateTimes[2] = year.ToString() + "-" + seasonOfYear.ToString();
            DataTable dtNew = CreateSequentialDataTableByDateTime(strDateTimes, "季");//按时间生成数据同环比分析表
            DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
            DataTable dtSeasonReport0 = seasonReportRepository.GetDataPager(pointIds, factors, year - 1, seasonOfYear, year - 1, seasonOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;//获取季报数据
            DataTable dtSeasonReport1 = seasonReportRepository.GetDataPager(pointIds, factors, lastYear, lastSeasonOfYear, year, lastSeasonOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtSeasonReport2 = seasonReportRepository.GetDataPager(pointIds, factors, year, seasonOfYear, year, seasonOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtSeasonReport = UnionDataTable(dtSeasonReport0, dtSeasonReport1, dtSeasonReport2);//拼接数据表

            AddNewRowToDataTableByNoFactor(pointIds, factors, strDateTimes, "季", "Year", "SeasonOfYear", dtNew, dtSeasonReport, dtAvgIndex);
            dvAvgIndex = dtAvgIndex.AsDataView();
            recordTotal = dtNew.Rows.Count;
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }

        /// <summary>
        /// 获取年均同环比比较表
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="year">年</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 年均值列，如2015年
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetYearWithTheSequentialData(string[] pointIds, int year, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            YearReportRepository monthReportRepository = Singleton<YearReportRepository>.GetInstance();
            string[] factors = GetWaterQualityPollutantCodes();//取得参与评价的水质因子GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            string[] strDateTimes = new string[3];
            strDateTimes[0] = (year - 2).ToString();
            strDateTimes[1] = (year - 1).ToString();
            strDateTimes[2] = year.ToString();
            DataTable dtNew = CreateSequentialDataTableByDateTime(strDateTimes, "年");//按时间生成数据同环比分析表
            DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
            DataTable dtYearReport = monthReportRepository.GetDataPager(pointIds, factors, year - 2, year, pageSize, pageNo, out recordTotal, orderBy).Table;//获取年报数据

            AddNewRowToDataTableByNoFactor(pointIds, factors, strDateTimes, "年", "Year", string.Empty, dtNew, dtYearReport, dtAvgIndex);
            dvAvgIndex = dtAvgIndex.AsDataView();
            recordTotal = dtNew.Rows.Count;
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }

        /// <summary>
        /// 为表添加数据行
        /// </summary>
        /// <param name="pointIds">测点数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="strDateTimes">时间周期数组</param>
        /// <param name="type">日期类型</param>
        /// <param name="dcName1">日期列名1</param>
        /// <param name="dcName2">日期列名2</param>
        /// <param name="dtNew">要添加数据行的表</param>
        /// <param name="dtReport">提供数据的表</param>
        /// <param name="dtAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        private void AddNewRowToDataTableByNoFactor(string[] pointIds, string[] factors, string[] strDateTimes, string type, string dcName1, string dcName2,
            DataTable dtNew, DataTable dtReport, DataTable dtAvgIndex)
        {
            WaterQualityService waterQualityService = new WaterQualityService();//提供污染等级服务（如空气质量指数、水质等级）
            WaterQualityRepository waterQualityRepository = Singleton<WaterQualityRepository>.GetInstance();//水质标准处理仓储层
            DataTable dtWaterAnalyzeData = waterQualityRepository.GetWaterAnalyzeData(pointIds).Table;//根据站点获取水质分析数据
            Dictionary<string, string> siteTypeByPointIdsList = GetSiteTypeByPointIds(pointIds);//根据站点Id数组获取站点Id和站点类型对应键值对
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(pointIds);//根据测点Id数组获取测点Id和对应的因子Code
            int pointCount = 0;//测点数量
            decimal sequentialIndexTotal = 0;//综合污染指数合计
            string strDateType = (type == "时" || type == "日") ? string.Empty : type;
            string[] waterQualityPollutantCodes = GetWaterQualityPollutantCodes();//取得参与评价的水质因子
            pointIds = pointIds.Distinct().ToArray();

            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                string WQI_Avg = string.Empty;
                DataRow drNew = dtNew.NewRow();
                DataRow[] drsReport = dtReport.Select(string.Format("PointId='{0}'", pointId));
                DataRow[] drsWaterAnalyzeData = dtWaterAnalyzeData.Select(string.Format("PointId='{0}'", pointId));
                decimal lastCycleIndex = 0;//上一周期的指数
                decimal lastYearIndex = 0;//去年同期的指数
                decimal currIndex = 0;//当前选择的月的指数
                WaterQualityClass waterQualityClass = WaterQualityClass.One;
                WaterPointCalWQType waterPointCalWQType = WaterPointCalWQType.River;
                IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                     ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                pollutantCodeList = pollutantCodeList.Distinct().ToList();
                pollutantCodeList = pollutantCodeList.Where(t => waterQualityPollutantCodes.Contains(t)).ToList();
                drNew["SiteTypeName"] = (siteTypeByPointIdsList.ContainsKey(pointId))
                      ? siteTypeByPointIdsList[pointId] : string.Empty;//站点类型名称??地表水 类型
                drNew["PointId"] = pointId;

                ////如果不存在该测点的数据，则跳过
                //if (drsMonthReport.Length <= 0)
                //{
                //    continue;
                //}    
                if (drsWaterAnalyzeData != null && drsWaterAnalyzeData.Length > 0)
                {
                    DataRow drWaterAnalyzeData = drsWaterAnalyzeData[0];
                    string strIEQI = drWaterAnalyzeData["IEQI"].ToString().Trim();//水质类别（数字）
                    waterPointCalWQType = drWaterAnalyzeData["CalEQIType"].ToString().Trim() == "河流" ? WaterPointCalWQType.River : WaterPointCalWQType.Lake;

                    switch (strIEQI)
                    {
                        case "1":
                            waterQualityClass = WaterQualityClass.One;
                            break;
                        case "2":
                            waterQualityClass = WaterQualityClass.Two;
                            break;
                        case "3":
                            waterQualityClass = WaterQualityClass.Three;
                            break;
                        case "4":
                            waterQualityClass = WaterQualityClass.Four;
                            break;
                        case "5":
                            waterQualityClass = WaterQualityClass.Five;
                            break;
                        case "6":
                            waterQualityClass = WaterQualityClass.BadFive;
                            break;
                        default:
                            break;
                    }
                }
                foreach (DataRow drReport in drsReport)
                {
                    string strYear = (dtReport.Columns.Contains(dcName1)) ? drReport[dcName1].ToString() : string.Empty;//年
                    string strCycle = (dtReport.Columns.Contains(dcName2)) ? drReport[dcName2].ToString() : string.Empty;//周期（如：周、月、季、年）
                    string strDateTime = string.Empty;
                    string evaluateFactorCodes = string.Empty;
                    Dictionary<string, decimal> WQIValuesDictionary = new Dictionary<string, decimal>();//因子指数列表（Key:PollutantCode、Value:分指数）

                    if (dtReport.Columns.Contains(dcName1))
                    {
                        if (type == "时")
                        {
                            strYear = string.Format("{0:yyyy-MM-dd HH:00}", drReport[dcName1]);
                        }
                        else if (type == "日")
                        {
                            strYear = string.Format("{0:yyyy-MM-dd}", drReport[dcName1]);
                        }
                    }
                    if (string.IsNullOrWhiteSpace(strCycle))
                    {
                        strDateTime = strYear;
                    }
                    else
                    {
                        strDateTime = strYear + "-" + strCycle;
                    }
                    foreach (string pollutantCode in pollutantCodeList)
                    {
                        if (!dtReport.Columns.Contains(pollutantCode))
                        {
                            continue;
                        }
                        decimal pollutantValue = decimal.TryParse(drReport[pollutantCode].ToString(), out pollutantValue) ? pollutantValue : 0;
                        decimal WQI = waterQualityService.GetWQI(pollutantCode, pollutantValue, waterQualityClass, EQITimeType.One, waterPointCalWQType);//计算水质污染指数
                        evaluateFactorCodes += pollutantCode + ";";
                        WQIValuesDictionary.Add(pollutantCode, WQI);
                    }
                    evaluateFactorCodes = evaluateFactorCodes.TrimEnd(';');
                    if (evaluateFactorCodes.Length > 0 && WQIValuesDictionary.Count > 0)
                    {
                        WQI_Avg = waterQualityService.GetWQI_Avg(evaluateFactorCodes, WQIValuesDictionary);
                        if(!string.IsNullOrWhiteSpace(WQI_Avg))
                            drReport["EQI"] = WQI_Avg;
                    }
                    if (dtNew.Columns.Contains(strDateTime + strDateType))
                    {
                        drNew[strDateTime + strDateType] = drReport["EQI"];//污染指数
                    }

                    //如果是去年同期的月，则记录指数
                    if (strDateTime == strDateTimes[0])
                    {
                        lastYearIndex = decimal.TryParse(drReport["EQI"].ToString(), out lastYearIndex) ? lastYearIndex : 0;
                    }

                    //如果是上一周期的月，则记录指数
                    if (strDateTime == strDateTimes[1])
                    {
                        lastCycleIndex = decimal.TryParse(drReport["EQI"].ToString(), out lastCycleIndex) ? lastCycleIndex : 0;
                    }

                    //如果是当前选择的年，则记录综合污染指数、水质类别
                    if (strDateTime == strDateTimes[2])
                    {
                        currIndex = decimal.TryParse(drReport["EQI"].ToString(), out currIndex) ? currIndex : 0;
                        drNew["SequentialPollutantIndex"] = drReport["EQI"];//综合污染指数
                        drNew["WaterQualityClassification"] = drReport["Grade"];//水质类别
                        sequentialIndexTotal += currIndex;
                    }
                }
                if (drsReport.Length > 0)
                {
                    pointCount++;
                    decimal linkRelativeRatioRiseRate = (lastCycleIndex > 0) ? Math.Round((currIndex - lastCycleIndex) * 100 / lastCycleIndex, 2) : 0; //环比上升比例（上一个周期）
                    decimal yearOnYearRiseRate = (lastYearIndex > 0) ? Math.Round((currIndex - lastYearIndex) * 100 / lastYearIndex, 2) : 0; //同比上升比例（上一年度相同周期）

                    if (linkRelativeRatioRiseRate > 0)
                    {
                        drNew["LinkRelativeRatio_PollutantTrend"] = "上升";//环比污染趋势
                    }
                    else if (linkRelativeRatioRiseRate == 0)
                    {
                        drNew["LinkRelativeRatio_PollutantTrend"] = "持平";//环比污染趋势
                    }
                    else
                    {
                        drNew["LinkRelativeRatio_PollutantTrend"] = "下降";//环比污染趋势
                    }
                    drNew["LinkRelativeRatio_RiseRate"] = linkRelativeRatioRiseRate.ToString("0.00") + "%";//环比上升比例（%）
                    if (yearOnYearRiseRate > 0)
                    {
                        drNew["YearOnYear_PollutantTrend"] = "上升";//同比污染趋势
                    }
                    else if (yearOnYearRiseRate == 0)
                    {
                        drNew["YearOnYear_PollutantTrend"] = "持平";//同比污染趋势
                    }
                    else
                    {
                        drNew["YearOnYear_PollutantTrend"] = "下降";//同比污染趋势
                    }
                    drNew["YearOnYear_RiseRate"] = yearOnYearRiseRate.ToString() + "%";//同比上升比例（%）
                }
                dtNew.Rows.Add(drNew);//添加新行
            }
            if (pointCount > 0)
            {
                decimal sequentialPollutantIndex = Math.Round(sequentialIndexTotal / pointCount, 2);
                dtAvgIndex.Rows[4]["SequentialPollutantIndex"] = sequentialPollutantIndex;//平均综合污染指数
                dtAvgIndex.Rows[1]["SequentialPollutantIndex"] = sequentialPollutantIndex;//平均污染指数的综合污染指数列
            }
            ComputePollutantIndexAvgData(strDateTimes, strDateType, dtNew, dtAvgIndex);//计算平均数据
            ComputePollutantLevelSorting(dtNew);//污染程度排序
        }

        /// <summary>
        /// 计算平均数据
        /// </summary>
        /// <param name="strDateTimes">时间周期数组</param>
        /// <param name="strDateType">时间类型字符串</param>
        /// <param name="dtNew">数据表</param>
        /// <param name="dtAvgIndex">平均数据表</param>
        private void ComputePollutantIndexAvgData(string[] strDateTimes, string strDateType, DataTable dtNew, DataTable dtAvgIndex)
        {
            foreach (string strDateTime in strDateTimes)
            {
                object objAvgValue = dtNew.Compute(string.Format("avg([{0}])", strDateTime + strDateType),
                    string.Format("isnull([{0}],-1)<>-1", strDateTime + strDateType));//平均污染指数
                if (!string.IsNullOrWhiteSpace(objAvgValue.ToString()))
                {
                    decimal avgValue = decimal.TryParse(objAvgValue.ToString(), out avgValue) ? avgValue : 0;//平均污染指数
                    dtAvgIndex.Rows[1][strDateTime + strDateType] = avgValue;//平均污染指数
                }
            }
        }

        /// <summary>
        /// 污染程度排序
        /// </summary>
        /// <param name="dt">要排序的数据表</param>
        private void ComputePollutantLevelSorting(DataTable dt)
        {
            DataTable dtSort;//排序后的表
            DataView dv = dt.DefaultView;
            int index = 1;//序号（从1开始）
            dv.Sort = "SequentialPollutantIndex Desc";
            dtSort = dv.ToTable();

            //污染程度排序
            for (int i = 0; i < dtSort.Rows.Count; i++)
            {
                //如果当前综合污染指数列的值为空，则跳到下一个
                if (string.IsNullOrWhiteSpace(dtSort.Rows[i]["SequentialPollutantIndex"].ToString()))
                {
                    continue;
                }

                //查询出dt中综合污染物指数为当前值的行
                DataRow[] drs = dt.Select(string.Format("SequentialPollutantIndex='{0}'", dtSort.Rows[i]["SequentialPollutantIndex"]));
                bool isAddIndex = false;//序号是否需要增加
                foreach (DataRow dr in drs)
                {
                    //当综合污染物指数不为空，并且污染排序程度为空时，记录排序序号
                    if (!string.IsNullOrWhiteSpace(dr["SequentialPollutantIndex"].ToString())
                        && string.IsNullOrWhiteSpace(dr["PollutantLevelSorting"].ToString()))
                    {
                        dr["PollutantLevelSorting"] = index;
                        isAddIndex = true;
                    }
                }
                index = isAddIndex ? index + 1 : index;
            }
        }
        #endregion

        #region 污染调查报告（数据同环比分析，有因子）
        /// <summary>
        /// 获取时均因子同环比比较表
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmHour">时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// SiteTypeName：站点类型名称
        /// PointId：测点Id
        /// 因子时均值列，如2015-08-30 15:00_w010010
        /// 因子时均EQI值列，如2015-08-30 15:00_w010010_EQI
        /// 因子时均Grade值列，如2015-08-30 15:00_w010010_Grade
        /// 因子时均排序值列，如2015-08-30 15:00_w010010_Sort
        /// SequentialPollutantIndex：综合污染指数
        /// PollutantLevelSorting：污染程度排序
        /// WaterQualityClassification：水质类别
        /// LinkRelativeRatio_PollutantTrend：环比污染趋势
        /// LinkRelativeRatio_RiseRate：环比上升比例（%）
        /// blankspaceColumn：空白列</returns>
        public DataView GetPollutantHourSequentialData(string[] pointIds, string[] factors, DateTime dtmHour, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            HourReportRepository hourReportRepository = Singleton<HourReportRepository>.GetInstance();
            string[] strDateTimes = new string[3];//时间周期数组
            strDateTimes[0] = dtmHour.AddYears(-1).ToString("yyyy-MM-dd HH:00");//上一年同期
            strDateTimes[1] = dtmHour.AddHours(-4).ToString("yyyy-MM-dd HH:00");//上一个周期
            strDateTimes[2] = dtmHour.ToString("yyyy-MM-dd HH:00");//当前周期
            DataTable dtNew = CreateSequentialDataTableByFactorDate(factors, strDateTimes, "时");//按时间和因子生成数据同环比分析表
            DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
            DataTable dtHourReport0 = hourReportRepository.GetDataPager(pointIds, factors, Convert.ToDateTime(dtmHour.AddYears(-1).ToString("yyyy-MM-dd HH:00")), Convert.ToDateTime(dtmHour.AddYears(-1).ToString("yyyy-MM-dd HH:59")), pageSize, pageNo, out recordTotal, orderBy).Table;//获取时报数据
            DataTable dtHourReport1 = hourReportRepository.GetDataPager(pointIds, factors, Convert.ToDateTime(dtmHour.AddHours(-4).ToString("yyyy-MM-dd HH:00")), Convert.ToDateTime(dtmHour.AddHours(-4).ToString("yyyy-MM-dd HH:59")), pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtHourReport2 = hourReportRepository.GetDataPager(pointIds, factors, Convert.ToDateTime(dtmHour.ToString("yyyy-MM-dd HH:00")), Convert.ToDateTime(dtmHour.ToString("yyyy-MM-dd HH:59")), pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtHourReport = UnionDataTable(dtHourReport0, dtHourReport1, dtHourReport2);//拼接数据表

            AddNewRowToDataTableByFactor(pointIds, factors, strDateTimes, "时", "Tstamp", string.Empty, dtNew, dtHourReport, dtAvgIndex);
            dvAvgIndex = dtAvgIndex.AsDataView();
            recordTotal = dtNew.Rows.Count;
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }
        /// <summary>
        /// 获取时均因子同环比比较表（蓝藻）
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmHour">时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// SiteTypeName：站点类型名称
        /// PointId：测点Id
        /// 因子时均值列，如2015-08-30 15:00_w010010
        /// 因子时均EQI值列，如2015-08-30 15:00_w010010_EQI
        /// 因子时均Grade值列，如2015-08-30 15:00_w010010_Grade
        /// 因子时均排序值列，如2015-08-30 15:00_w010010_Sort
        /// SequentialPollutantIndex：综合污染指数
        /// PollutantLevelSorting：污染程度排序
        /// WaterQualityClassification：水质类别
        /// LinkRelativeRatio_PollutantTrend：环比污染趋势
        /// LinkRelativeRatio_RiseRate：环比上升比例（%）
        /// blankspaceColumn：空白列</returns>
        public DataView GetLZPollutantHourSequentialData(string[] pointIds, string[] factors, DateTime dtmHour, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            try
            {
                HourReportRepository hourReportRepository = Singleton<HourReportRepository>.GetInstance();
                string[] strDateTimes = new string[3];//时间周期数组
                strDateTimes[0] = dtmHour.AddYears(-1).ToString("yyyy-MM-dd HH:00");//上一年同期
                strDateTimes[1] = dtmHour.AddHours(-4).ToString("yyyy-MM-dd HH:00");//上一个周期
                strDateTimes[2] = dtmHour.ToString("yyyy-MM-dd HH:00");//当前周期
                DataTable dtNew = CreateSequentialDataTableByFactorDate(factors, strDateTimes, "时");//按时间和因子生成数据同环比分析表
                DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
                DataTable dtHourReport0 = hourReportRepository.GetDataPager(pointIds, factors, Convert.ToDateTime(dtmHour.AddYears(-1).ToString("yyyy-MM-dd HH:00")), Convert.ToDateTime(dtmHour.AddYears(-1).ToString("yyyy-MM-dd HH:59")), pageSize, pageNo, out recordTotal, orderBy).Table;//获取时报数据
                DataTable dtHourReport1 = hourReportRepository.GetDataPager(pointIds, factors, Convert.ToDateTime(dtmHour.AddHours(-4).ToString("yyyy-MM-dd HH:00")), Convert.ToDateTime(dtmHour.AddHours(-4).ToString("yyyy-MM-dd HH:59")), pageSize, pageNo, out recordTotal, orderBy).Table;
                DataTable dtHourReport2 = hourReportRepository.GetDataPager(pointIds, factors, Convert.ToDateTime(dtmHour.ToString("yyyy-MM-dd HH:00")), Convert.ToDateTime(dtmHour.ToString("yyyy-MM-dd HH:59")), pageSize, pageNo, out recordTotal, orderBy).Table;
                DataTable dtHourReport = UnionDataTable(dtHourReport0, dtHourReport1, dtHourReport2);//拼接数据表
                AddNewRowToDataTableByLZFactor(pointIds, factors, strDateTimes, "时", "Tstamp", string.Empty, dtNew, dtHourReport, dtAvgIndex);
                dvAvgIndex = dtAvgIndex.AsDataView();
                recordTotal = dtNew.Rows.Count;
                //dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
                return dtNew.AsDataView();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 获取日均因子同环比比较表
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmDay">日期</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 因子日均值列，如2015-08-30_w010010
        /// 因子日均EQI值列，如2015-08-30_w010010_EQI
        /// 因子日均Grade值列，如2015-08-30_w010010_Grade
        /// 因子日均排序值列，如2015-08-30_w010010_Sort
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetPollutantDaySequentialData(string[] pointIds, string[] factors, DateTime dtmDay, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            try
            {
                DayReportRepository dayReportRepository = Singleton<DayReportRepository>.GetInstance();
                string[] strDateTimes = new string[3];
                strDateTimes[0] = dtmDay.AddYears(-1).ToString("yyyy-MM-dd");
                strDateTimes[1] = dtmDay.AddDays(-1).ToString("yyyy-MM-dd");
                strDateTimes[2] = dtmDay.ToString("yyyy-MM-dd");
                DataTable dtNew = CreateSequentialDataTableByFactorDate(factors, strDateTimes, "日");//按时间和因子生成数据同环比分析表
                DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
                DataTable dtDayReport0 = dayReportRepository.GetDataPager(pointIds, factors, dtmDay.AddYears(-1), dtmDay.AddYears(-1), pageSize, pageNo, out recordTotal, orderBy).Table;//获取日报数据
                DataTable dtDayReport1 = dayReportRepository.GetDataPager(pointIds, factors, dtmDay.AddDays(-1), dtmDay.AddDays(-1), pageSize, pageNo, out recordTotal, orderBy).Table;
                DataTable dtDayReport2 = dayReportRepository.GetDataPager(pointIds, factors, dtmDay, dtmDay, pageSize, pageNo, out recordTotal, orderBy).Table;
                DataTable dtDayReport = UnionDataTable(dtDayReport0, dtDayReport1, dtDayReport2);//拼接数据表
                
                //为表添加数据行
                AddNewRowToDataTableByFactor(pointIds, factors, strDateTimes, "日", "DateTime", string.Empty, dtNew, dtDayReport, dtAvgIndex);
                dvAvgIndex = dtAvgIndex.AsDataView();
                recordTotal = dtNew.Rows.Count;
                dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
                return dtNew.AsDataView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取日均因子同环比比较表(蓝藻)
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmDay">日期</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 因子日均值列，如2015-08-30_w010010
        /// 因子日均EQI值列，如2015-08-30_w010010_EQI
        /// 因子日均Grade值列，如2015-08-30_w010010_Grade
        /// 因子日均排序值列，如2015-08-30_w010010_Sort
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetLZPollutantDaySequentialData(string[] pointIds, string[] factors, DateTime dtmDay, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            try
            {
                DayReportRepository dayReportRepository = Singleton<DayReportRepository>.GetInstance();
                string[] strDateTimes = new string[3];
                strDateTimes[0] = dtmDay.AddYears(-1).ToString("yyyy-MM-dd");
                strDateTimes[1] = dtmDay.AddDays(-1).ToString("yyyy-MM-dd");
                strDateTimes[2] = dtmDay.ToString("yyyy-MM-dd");
                DataTable dtNew = CreateSequentialDataTableByFactorDate(factors, strDateTimes, "日");//按时间和因子生成数据同环比分析表
                DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
                DataTable dtDayReport0 = dayReportRepository.GetDataPager(pointIds, factors, dtmDay.AddYears(-1), dtmDay.AddYears(-1), pageSize, pageNo, out recordTotal, orderBy).Table;//获取日报数据
                DataTable dtDayReport1 = dayReportRepository.GetDataPager(pointIds, factors, dtmDay.AddDays(-1), dtmDay.AddDays(-1), pageSize, pageNo, out recordTotal, orderBy).Table;
                DataTable dtDayReport2 = dayReportRepository.GetDataPager(pointIds, factors, dtmDay, dtmDay, pageSize, pageNo, out recordTotal, orderBy).Table;
                DataTable dtDayReport = UnionDataTable(dtDayReport0, dtDayReport1, dtDayReport2);//拼接数据表

                //为表添加数据行
                AddNewRowToDataTableByLZFactor(pointIds, factors, strDateTimes, "日", "DateTime", string.Empty, dtNew, dtDayReport, dtAvgIndex);
                dvAvgIndex = dtAvgIndex.AsDataView();
                recordTotal = dtNew.Rows.Count;
                //dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
                return dtNew.AsDataView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取周均因子同环比比较表
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="year">年</param>
        /// <param name="weekOfYear">周</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 因子周均值列，如2015-8周_w010010
        /// 因子周均EQI值列，如2015-8周_w010010_EQI
        /// 因子周均Grade值列，如2015-8周_w010010_Grade
        /// 因子周均排序值列，如2015-8周_w010010_Sort
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetPollutantWeekSequentialData(string[] pointIds, string[] factors, int year, int weekOfYear, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            WeekReportRepository weekReportRepository = Singleton<WeekReportRepository>.GetInstance();
            string[] strDateTimes = new string[3];
            int lastYear = (weekOfYear == 1) ? year - 1 : year;//上一个周期的年
            int lastWeekOfYear = (weekOfYear == 1) ? (int)Math.Ceiling((new DateTime(year - 1, 12, 31).DayOfYear
                                                + (int)new DateTime(year - 1, 1, 1).DayOfWeek - 1
                                                + 7 - (int)new DateTime(year - 1, 12, 31).DayOfWeek) * 1M / 7) : weekOfYear - 1;//上一个周期的周
            strDateTimes[0] = (year - 1).ToString() + "-" + weekOfYear.ToString();
            strDateTimes[1] = lastYear.ToString() + "-" + lastWeekOfYear.ToString();
            strDateTimes[2] = year.ToString() + "-" + weekOfYear.ToString();
            DataTable dtNew = CreateSequentialDataTableByFactorDate(factors, strDateTimes, "周");//按时间和因子生成数据同环比分析表
            DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
            DataTable dtWeekReport0 = weekReportRepository.GetDataPager(pointIds, factors, year - 1, weekOfYear, year - 1, weekOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;//获取周报数据
            DataTable dtWeekReport1 = weekReportRepository.GetDataPager(pointIds, factors, lastYear, lastWeekOfYear, lastYear, lastWeekOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtWeekReport2 = weekReportRepository.GetDataPager(pointIds, factors, year, weekOfYear, year, weekOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtWeekReport = UnionDataTable(dtWeekReport0, dtWeekReport1, dtWeekReport2);//拼接数据表

            AddNewRowToDataTableByFactor(pointIds, factors, strDateTimes, "周", "Year", "WeekOfYear", dtNew, dtWeekReport, dtAvgIndex);
            dvAvgIndex = dtAvgIndex.AsDataView();
            recordTotal = dtNew.Rows.Count;
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }
        /// <summary>
        /// 获取周均因子同环比比较表（蓝藻）
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="year">年</param>
        /// <param name="weekOfYear">周</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 因子周均值列，如2015-8周_w010010
        /// 因子周均EQI值列，如2015-8周_w010010_EQI
        /// 因子周均Grade值列，如2015-8周_w010010_Grade
        /// 因子周均排序值列，如2015-8周_w010010_Sort
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetLZPollutantWeekSequentialData(string[] pointIds, string[] factors, int year, int weekOfYear, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            try
            {
                WeekReportRepository weekReportRepository = Singleton<WeekReportRepository>.GetInstance();
                string[] strDateTimes = new string[3];
                int lastYear = (weekOfYear == 1) ? year - 1 : year;//上一个周期的年
                int lastWeekOfYear = (weekOfYear == 1) ? (int)Math.Ceiling((new DateTime(year - 1, 12, 31).DayOfYear
                                                    + (int)new DateTime(year - 1, 1, 1).DayOfWeek - 1
                                                    + 7 - (int)new DateTime(year - 1, 12, 31).DayOfWeek) * 1M / 7) : weekOfYear - 1;//上一个周期的周
                strDateTimes[0] = (year - 1).ToString() + "-" + weekOfYear.ToString();
                strDateTimes[1] = lastYear.ToString() + "-" + lastWeekOfYear.ToString();
                strDateTimes[2] = year.ToString() + "-" + weekOfYear.ToString();
                DataTable dtNew = CreateSequentialDataTableByFactorDate(factors, strDateTimes, "周");//按时间和因子生成数据同环比分析表
                DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
                DataTable dtWeekReport0 = weekReportRepository.GetDataPager(pointIds, factors, year - 1, weekOfYear, year - 1, weekOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;//获取周报数据
                DataTable dtWeekReport1 = weekReportRepository.GetDataPager(pointIds, factors, lastYear, lastWeekOfYear, lastYear, lastWeekOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;
                DataTable dtWeekReport2 = weekReportRepository.GetDataPager(pointIds, factors, year, weekOfYear, year, weekOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;
                DataTable dtWeekReport = UnionDataTable(dtWeekReport0, dtWeekReport1, dtWeekReport2);//拼接数据表

                AddNewRowToDataTableByLZFactor(pointIds, factors, strDateTimes, "周", "Year", "WeekOfYear", dtNew, dtWeekReport, dtAvgIndex);
                dvAvgIndex = dtAvgIndex.AsDataView();
                recordTotal = dtNew.Rows.Count;
                //dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
                return dtNew.AsDataView();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 获取月均因子同环比比较表
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 因子月均值列，如2015-8月_w010010
        /// 因子月均EQI值列，如2015-8月_w010010_EQI
        /// 因子月均Grade值列，如2015-8月_w010010_Grade
        /// 因子月均排序值列，如2015-8月_w010010_Sort
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetPollutantMonthSequentialData(string[] pointIds, string[] factors, int year, int month, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            MonthReportRepository monthReportRepository = Singleton<MonthReportRepository>.GetInstance();
            string[] strDateTimes = new string[3];
            DateTime lastYearMonth = new DateTime(year, month, 1).AddMonths(-1);//上一个周期的日期
            strDateTimes[0] = new DateTime(year, month, 1).AddYears(-1).ToString("yyyy-M");
            strDateTimes[1] = lastYearMonth.ToString("yyyy-M");
            strDateTimes[2] = new DateTime(year, month, 1).ToString("yyyy-M");
            DataTable dtNew = CreateSequentialDataTableByFactorDate(factors, strDateTimes, "月");//按时间和因子生成数据同环比分析表
            DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
            DataTable dtMonthReport0 = monthReportRepository.GetDataPager(pointIds, factors, year - 1, month, year - 1, month, pageSize, pageNo, out recordTotal, orderBy).Table;//获取月报数据
            DataTable dtMonthReport1 = monthReportRepository.GetDataPager(pointIds, factors, lastYearMonth.Year, lastYearMonth.Month, lastYearMonth.Year, lastYearMonth.Month, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtMonthReport2 = monthReportRepository.GetDataPager(pointIds, factors, year, month, year, month, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtMonthReport = UnionDataTable(dtMonthReport0, dtMonthReport1, dtMonthReport2);//拼接数据表
            //DataTable dtStatisticalData = monthReportRepository.GetStatisticalData(pointIds, factors, year, month, year, month).Table;

            AddNewRowToDataTableByFactor(pointIds, factors, strDateTimes, "月", "Year", "MonthOfYear", dtNew, dtMonthReport, dtAvgIndex);
            dvAvgIndex = dtAvgIndex.AsDataView();
            recordTotal = dtNew.Rows.Count;
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }

        /// <summary>
        /// 获取月均因子同环比比较表（蓝藻）
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 因子月均值列，如2015-8月_w010010
        /// 因子月均EQI值列，如2015-8月_w010010_EQI
        /// 因子月均Grade值列，如2015-8月_w010010_Grade
        /// 因子月均排序值列，如2015-8月_w010010_Sort
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetLZPollutantMonthSequentialData(string[] pointIds, string[] factors, int year, int month, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            try
            {
                MonthReportRepository monthReportRepository = Singleton<MonthReportRepository>.GetInstance();
                string[] strDateTimes = new string[3];
                DateTime lastYearMonth = new DateTime(year, month, 1).AddMonths(-1);//上一个周期的日期
                strDateTimes[0] = new DateTime(year, month, 1).AddYears(-1).ToString("yyyy-M");
                strDateTimes[1] = lastYearMonth.ToString("yyyy-M");
                strDateTimes[2] = new DateTime(year, month, 1).ToString("yyyy-M");
                DataTable dtNew = CreateSequentialDataTableByFactorDate(factors, strDateTimes, "月");//按时间和因子生成数据同环比分析表
                DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
                DataTable dtMonthReport0 = monthReportRepository.GetDataPager(pointIds, factors, year - 1, month, year - 1, month, pageSize, pageNo, out recordTotal, orderBy).Table;//获取月报数据
                DataTable dtMonthReport1 = monthReportRepository.GetDataPager(pointIds, factors, lastYearMonth.Year, lastYearMonth.Month, lastYearMonth.Year, lastYearMonth.Month, pageSize, pageNo, out recordTotal, orderBy).Table;
                DataTable dtMonthReport2 = monthReportRepository.GetDataPager(pointIds, factors, year, month, year, month, pageSize, pageNo, out recordTotal, orderBy).Table;
                DataTable dtMonthReport = UnionDataTable(dtMonthReport0, dtMonthReport1, dtMonthReport2);//拼接数据表
                //DataTable dtStatisticalData = monthReportRepository.GetStatisticalData(pointIds, factors, year, month, year, month).Table;

                AddNewRowToDataTableByLZFactor(pointIds, factors, strDateTimes, "月", "Year", "MonthOfYear", dtNew, dtMonthReport, dtAvgIndex);
                dvAvgIndex = dtAvgIndex.AsDataView();
                recordTotal = dtNew.Rows.Count;
                //dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
                return dtNew.AsDataView();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        /// <summary>
        /// 获取季均因子同环比比较表
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="year">年</param>
        /// <param name="seasonOfYear">季</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 因子季均值列，如2015-8季_w010010
        /// 因子季均EQI值列，如2015-8季_w010010_EQI
        /// 因子季均Grade值列，如2015-8季_w010010_Grade
        /// 因子季均排序值列，如2015-8季_w010010_Sort
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetPollutantSeasonSequentialData(string[] pointIds, string[] factors, int year, int seasonOfYear, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            SeasonReportRepository seasonReportRepository = Singleton<SeasonReportRepository>.GetInstance();
            string[] strDateTimes = new string[3];
            int lastYear = (seasonOfYear == 1) ? year - 1 : year;//上一个周期的年
            int lastSeasonOfYear = (seasonOfYear == 1) ? 4 : seasonOfYear - 1;//上一个周期的季
            strDateTimes[0] = (year - 1).ToString() + "-" + seasonOfYear.ToString();
            strDateTimes[1] = lastYear.ToString() + "-" + lastSeasonOfYear.ToString();
            strDateTimes[2] = year.ToString() + "-" + seasonOfYear.ToString();
            DataTable dtNew = CreateSequentialDataTableByFactorDate(factors, strDateTimes, "季");//按时间和因子生成数据同环比分析表
            DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
            DataTable dtSeasonReport0 = seasonReportRepository.GetDataPager(pointIds, factors, year - 1, seasonOfYear, year - 1, seasonOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;//获取季报数据
            DataTable dtSeasonReport1 = seasonReportRepository.GetDataPager(pointIds, factors, lastYear, lastSeasonOfYear, year, lastSeasonOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtSeasonReport2 = seasonReportRepository.GetDataPager(pointIds, factors, year, seasonOfYear, year, seasonOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;
            DataTable dtSeasonReport = UnionDataTable(dtSeasonReport0, dtSeasonReport1, dtSeasonReport2);//拼接数据表

            AddNewRowToDataTableByFactor(pointIds, factors, strDateTimes, "季", "Year", "SeasonOfYear", dtNew, dtSeasonReport, dtAvgIndex);
            dvAvgIndex = dtAvgIndex.AsDataView();
            recordTotal = dtNew.Rows.Count;
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }

        /// <summary>
        /// 获取季均因子同环比比较表(蓝藻)
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="year">年</param>
        /// <param name="seasonOfYear">季</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 因子季均值列，如2015-8季_w010010
        /// 因子季均EQI值列，如2015-8季_w010010_EQI
        /// 因子季均Grade值列，如2015-8季_w010010_Grade
        /// 因子季均排序值列，如2015-8季_w010010_Sort
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetLZPollutantSeasonSequentialData(string[] pointIds, string[] factors, int year, int seasonOfYear, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            try
            {
                SeasonReportRepository seasonReportRepository = Singleton<SeasonReportRepository>.GetInstance();
                string[] strDateTimes = new string[3];
                int lastYear = (seasonOfYear == 1) ? year - 1 : year;//上一个周期的年
                int lastSeasonOfYear = (seasonOfYear == 1) ? 4 : seasonOfYear - 1;//上一个周期的季
                strDateTimes[0] = (year - 1).ToString() + "-" + seasonOfYear.ToString();
                strDateTimes[1] = lastYear.ToString() + "-" + lastSeasonOfYear.ToString();
                strDateTimes[2] = year.ToString() + "-" + seasonOfYear.ToString();
                DataTable dtNew = CreateSequentialDataTableByFactorDate(factors, strDateTimes, "季");//按时间和因子生成数据同环比分析表
                DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
                DataTable dtSeasonReport0 = seasonReportRepository.GetDataPager(pointIds, factors, year - 1, seasonOfYear, year - 1, seasonOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;//获取季报数据
                DataTable dtSeasonReport1 = seasonReportRepository.GetDataPager(pointIds, factors, lastYear, lastSeasonOfYear, year, lastSeasonOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;
                DataTable dtSeasonReport2 = seasonReportRepository.GetDataPager(pointIds, factors, year, seasonOfYear, year, seasonOfYear, pageSize, pageNo, out recordTotal, orderBy).Table;
                DataTable dtSeasonReport = UnionDataTable(dtSeasonReport0, dtSeasonReport1, dtSeasonReport2);//拼接数据表

                AddNewRowToDataTableByLZFactor(pointIds, factors, strDateTimes, "季", "Year", "SeasonOfYear", dtNew, dtSeasonReport, dtAvgIndex);
                dvAvgIndex = dtAvgIndex.AsDataView();
                recordTotal = dtNew.Rows.Count;
                //dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
                return dtNew.AsDataView();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 获取年均因子同环比比较表
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="year">年</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 因子年均值列，如2015年_w010010
        /// 因子年均EQI值列，如2015年_w010010_EQI
        /// 因子年均Grade值列，如2015年_w010010_Grade
        /// 因子年均排序值列，如2015年_w010010_Sort
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetPollutantYearSequentialData(string[] pointIds, string[] factors, int year, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            YearReportRepository monthReportRepository = Singleton<YearReportRepository>.GetInstance();
            string[] strDateTimes = new string[3];
            strDateTimes[0] = (year - 2).ToString();
            strDateTimes[1] = (year - 1).ToString();
            strDateTimes[2] = year.ToString();
            DataTable dtNew = CreateSequentialDataTableByFactorDate(factors, strDateTimes, "年");//按时间和因子生成数据同环比分析表
            DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
            DataTable dtYearReport = monthReportRepository.GetDataPager(pointIds, factors, year - 2, year, pageSize, pageNo, out recordTotal, orderBy).Table;//获取年报数据

            AddNewRowToDataTableByFactor(pointIds, factors, strDateTimes, "年", "Year", string.Empty, dtNew, dtYearReport, dtAvgIndex);
            dvAvgIndex = dtAvgIndex.AsDataView();
            recordTotal = dtNew.Rows.Count;
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }
        /// <summary>
        /// 获取年均因子同环比比较表(蓝藻)
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="year">年</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// 因子年均值列，如2015年_w010010
        /// 因子年均EQI值列，如2015年_w010010_EQI
        /// 因子年均Grade值列，如2015年_w010010_Grade
        /// 因子年均排序值列，如2015年_w010010_Sort
        /// 其它列同GetHourWithTheSequentialData
        /// </returns>
        public DataView GetLZPollutantYearSequentialData(string[] pointIds, string[] factors, int year, int pageSize, int pageNo,
            out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            YearReportRepository monthReportRepository = Singleton<YearReportRepository>.GetInstance();
            string[] strDateTimes = new string[3];
            strDateTimes[0] = (year - 2).ToString();
            strDateTimes[1] = (year - 1).ToString();
            strDateTimes[2] = year.ToString();
            DataTable dtNew = CreateSequentialDataTableByFactorDate(factors, strDateTimes, "年");//按时间和因子生成数据同环比分析表
            DataTable dtAvgIndex = CreateAvgDataTableAndAddRow(dtNew);
            DataTable dtYearReport = monthReportRepository.GetDataPager(pointIds, factors, year - 2, year, pageSize, pageNo, out recordTotal, orderBy).Table;//获取年报数据

            AddNewRowToDataTableByLZFactor(pointIds, factors, strDateTimes, "年", "Year", string.Empty, dtNew, dtYearReport, dtAvgIndex);
            dvAvgIndex = dtAvgIndex.AsDataView();
            recordTotal = dtNew.Rows.Count;
            //dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            return dtNew.AsDataView();
        }


        /// <summary>
        /// 获取因子同环比比较表（模拟数据）
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="years">年数据</param>
        /// <param name="months">月数据</param>
        /// <param name="dvAvgIndex">平均综合污染指数
        /// 返回的列名
        /// AvgIndex：平均综合污染指数
        /// </param>
        /// <returns>返回结果集
        /// 返回的列名
        /// SiteTypeName：站点类型名称
        /// PointId：测点Id
        /// 因子浓度列，如2015-8_w20001（w20001为因子Code）
        /// SequentialPollutantIndex：综合污染指数
        /// WaterQualityClassification：水质类别
        /// LinkRelativeRatio_PollutantTrend：环比污染趋势
        /// LinkRelativeRatio_RiseRate：环比上升比例（%）
        /// YearOnYear_PollutantTrend：同比污染趋势
        /// YearOnYear_RiseRate： 同比上升比例（%）
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetPollutantWithTheSequentialData(string[] pointIds, string[] pollutantCodes, int[] years, int[] months,
            int pageSize, int pageNo, out int recordTotal, out DataView dvAvgIndex, string orderBy = "PointId")
        {
            recordTotal = 0;
            DataTable dtNew = CreatePollutantWithTheSequentialDataTable(new string[] { "w01009", "w01010", "w01014" }, new int[] { 2013, 2014, 2015 }, new int[] { 1, 2, 3 });//生成因子同环比的表
            dtNew.Rows.Add("河流", "1", 3.08, 4.83, 3.37, 2.99, 4.33, 5.21, 6.21, 4.25, 3.78, 4.08, 4.53, 4.37, 3.99, 6.33, 3.21, 4.21, 6.25, 6.78, 5.08, 3.83, 4.37, 5.89, 5.32, 4.11, 5.21, 3.25, 3.78, 3.85, "Ⅱ类", "下降", -28, "上升", 9);
            dtNew.Rows.Add("河流", "14", 4.15, 3.53, 4.47, 3.67, 5.43, 4.11, 6.61, 3.25, 5.78, 6.28, 4.23, 3.37, 4.55, 7.33, 6.21, 4.26, 5.25, 6.44, 5.38, 4.83, 3.47, 4.89, 4.32, 5.11, 4.33, 5.24, 4.31, 3.05, "Ⅲ类", "上升", 18, "下降", -5);
            dtNew.Rows.Add("河流", "28", 5.23, 4.52, 2.67, 5.23, 5.33, 3.21, 5.52, 4.36, 6.48, 3.09, 5.64, 6.21, 6.88, 8.33, 5.26, 5.32, 6.25, 6.28, 6.08, 5.63, 5.39, 5.65, 6.32, 3.78, 5.43, 6.26, 5.12, 3.21, "Ⅰ类", "下降", -16, "下降", -8);
            dtNew.Rows.Add("河流", "平均浓度值", 4.23, 3.52, 2.57, 5.13, 4.23, 3.61, 4.53, 3.35, 4.37, 3.33, 4.55, 6.11, 4.78, 4.23, 3.26, 4.32, 6.15, 5.28, 3.55, 3.63, 4.21, 3.65, 4.32, 5.78, 3.43, 3.77, 4.36, "", "", "", "", "");
            dtNew.Rows.Add("河流", "评价标准（Ⅲ类标准值）", ">=5.0", "<=20", "<=6.0", "<=1.0", "<=0.2", "<=4", ">=3.0", "<=6.0", ">=7.0", ">=3.0", "<=18", "<=4.0", "<=2.0", "<=0.7", "<=3", ">=3.0", "<=5.0", ">=4.0", ">=7.0", "<=8.0", "<=4.0", "<=3.0", "<=10", "<=4.0", ">=7.0", ">=7.0", ">=8.0", "", "", "", "", "");
            dtNew.Rows.Add("河流", "平均污染指数", 0.23, 0.52, 0.67, 0.23, 0.33, 0.21, 0.52, 0.36, 0.48, 0.09, 0.64, 0.21, 0.88, 0.33, 0.26, 0.32, 0.25, 0.28, 0.08, 0.63, 0.39, 0.65, 0.32, 0.78, 0.43, 0.26, 0.12, "", "", "", "", "");
            dtNew.Rows.Add("河流", "分担率", 0.23, 0.12, 0.17, 0.23, 0.13, 0.21, 0.07, 0.05, 0.08, 0.09, 0.04, 0.12, 0.08, 0.13, 0.14, 0.12, 0.25, 0.15, 0.08, 0.13, 0.13, 0.05, 0.22, 0.07, 0.03, 0.16, 0.12, "", "", "", "", "");
            dtNew.Rows.Add("河流", "污染物排名", 5, 4, 2, 5, 3, 4, 6, 3, 4, 2, 5, 6, 4, 7, 2, 3, 6, 3, 5, 5, 4, 6, 3, 2, 3, 4, 5, "", "", "", "", "");
            DataTable dtAvgIndex = new DataTable();
            dtAvgIndex.Columns.Add("AvgIndex", typeof(string));
            dtNew.Rows.Add(3.37M);
            dvAvgIndex = dtAvgIndex.AsDataView();
            return dtNew.AsDataView();
        }

        /// <summary>
        /// 为表添加数据行
        /// </summary>
        /// <param name="pointIds">测点数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="strDateTimes">时间周期数组</param>
        /// <param name="type">日期类型</param>
        /// <param name="dcName1">日期列名1</param>
        /// <param name="dcName2">日期列名2</param>
        /// <param name="dtNew">要添加数据行的表</param>
        /// <param name="dtReport">提供数据的表</param>
        /// <param name="dtAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        private void AddNewRowToDataTableByFactor(string[] pointIds, string[] factors, string[] strDateTimes, string type, string dcName1, string dcName2,
            DataTable dtNew, DataTable dtReport, DataTable dtAvgIndex)
        {
            WaterQualityService waterQualityService = new WaterQualityService();//提供污染等级服务（如空气质量指数、水质等级）
            WaterQualityRepository waterQualityRepository = Singleton<WaterQualityRepository>.GetInstance();//水质标准处理仓储层
            DataTable dtWaterAnalyzeData = waterQualityRepository.GetWaterAnalyzeData(pointIds).Table;//根据站点获取水质分析数据
            Dictionary<string, string> siteTypeByPointIdsList = GetSiteTypeByPointIds(pointIds);//根据站点Id数组获取站点Id和站点类型对应键值对
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(pointIds);//根据测点Id数组获取测点Id和对应的因子Code
            int pointCount = 0;//测点数量
            decimal sequentialIndexTotal = 0;//综合污染指数合计
            string strDateType = (type == "时" || type == "日") ? string.Empty : type;
            //string[] waterQualityPollutantCodes = GetWaterQualityPollutantCodes();//取得参与评价的水质因子
            pointIds = pointIds.Distinct().ToArray();

            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                string WQI_Avg = string.Empty;
                DataRow drNew = dtNew.NewRow();
                DataRow[] drsReport = dtReport.Select(string.Format("PointId='{0}'", pointId));
                DataRow[] drsWaterAnalyzeData = dtWaterAnalyzeData.Select(string.Format("PointId='{0}'", pointId));
                decimal lastCycleIndex = 0;//上一周期的指数
                decimal lastYearIndex = 0;//去年同期的指数
                decimal currIndex = 0;//当前选择的月的指数
                WaterQualityClass waterQualityClass = WaterQualityClass.One;
                WaterPointCalWQType waterPointCalWQType = WaterPointCalWQType.River;
                IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                     ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                drNew["SiteTypeName"] = (siteTypeByPointIdsList.ContainsKey(pointId))
                      ? siteTypeByPointIdsList[pointId] : string.Empty;//站点类型名称??地表水 类型
                drNew["PointId"] = pointId;

                ////如果不存在该测点的数据，则跳过
                //if (drsMonthReport.Length <= 0)
                //{
                //    continue;
                //}
                if (drsWaterAnalyzeData != null && drsWaterAnalyzeData.Length > 0)
                {
                    DataRow drWaterAnalyzeData = drsWaterAnalyzeData[0];
                    string strIEQI = drWaterAnalyzeData["IEQI"].ToString().Trim();//水质类别（数字）
                    waterPointCalWQType = drWaterAnalyzeData["CalEQIType"].ToString().Trim() == "河流" ? WaterPointCalWQType.River : WaterPointCalWQType.Lake;

                    switch (strIEQI)
                    {
                        case "1":
                            waterQualityClass = WaterQualityClass.One;
                            break;
                        case "2":
                            waterQualityClass = WaterQualityClass.Two;
                            break;
                        case "3":
                            waterQualityClass = WaterQualityClass.Three;
                            break;
                        case "4":
                            waterQualityClass = WaterQualityClass.Four;
                            break;
                        case "5":
                            waterQualityClass = WaterQualityClass.Five;
                            break;
                        case "6":
                            waterQualityClass = WaterQualityClass.BadFive;
                            break;
                        default:
                            break;
                    }
                }
                foreach (DataRow drReport in drsReport)
                {
                    string strYear = (dtReport.Columns.Contains(dcName1)) ? drReport[dcName1].ToString() : string.Empty;//年
                    string strCycle = (dtReport.Columns.Contains(dcName2)) ? drReport[dcName2].ToString() : string.Empty;//周期（如：周、月、季、年）
                    string strDateTime = strDateTimes[0];
                    string evaluateFactorCodes = string.Empty;
                    Dictionary<string, decimal> WQIValuesDictionary = new Dictionary<string, decimal>();//因子指数列表（Key:PollutantCode、Value:分指数）

                    if (dtReport.Columns.Contains(dcName1))
                    {
                        if (type == "时")
                        {
                            strYear = string.Format("{0:yyyy-MM-dd HH:00}", drReport[dcName1]);
                        }
                        else if (type == "日")
                        {
                            strYear = string.Format("{0:yyyy-MM-dd}", drReport[dcName1]);
                        }
                    }
                    if (string.IsNullOrWhiteSpace(strCycle))
                    {
                        strDateTime = strYear;
                    }
                    else
                    {
                        strDateTime = strYear + "-" + strCycle;
                    }
                    foreach (string pollutantCode in factors)
                    {
                        decimal pollutantValue = decimal.TryParse(drReport[pollutantCode].ToString(), out pollutantValue) ? pollutantValue : 0;
                        if (dtNew.Columns.Contains(strDateTime + strDateType + "_" + pollutantCode))
                        {
                            drNew[strDateTime + strDateType + "_" + pollutantCode] = drReport[pollutantCode];//浓度值
                        }
                        if (dtNew.Columns.Contains(strDateTime + strDateType + "_" + pollutantCode + "_EQI"))
                        {
                            drNew[strDateTime + strDateType + "_" + pollutantCode + "_EQI"] = drReport[pollutantCode + "_EQI"];//EQI值
                        }
                        if (dtNew.Columns.Contains(strDateTime + strDateType + "_" + pollutantCode + "_Grade"))
                        {
                            drNew[strDateTime + strDateType + "_" + pollutantCode + "_Grade"] = drReport[pollutantCode + "_Grade"];//水质类别值
                        }
                        if (pollutantCodeList.Contains(pollutantCode))
                        {
                            decimal WQI = waterQualityService.GetWQI(pollutantCode, pollutantValue, waterQualityClass,
                                                EQITimeType.One, waterPointCalWQType);//计算水质污染指数
                            evaluateFactorCodes += pollutantCode + ";";
                            drNew[strDateTime + strDateType + "_" + pollutantCode + "_EQI"] = WQI;//EQI值
                            drReport[pollutantCode + "_EQI"] = WQI;
                            WQIValuesDictionary.Add(pollutantCode, WQI);//添加到WQI键值对
                        }
                    }
                    evaluateFactorCodes = evaluateFactorCodes.TrimEnd(';');
                    if (evaluateFactorCodes.Length > 0 && WQIValuesDictionary.Count > 0)
                    {
                        WQI_Avg = waterQualityService.GetWQI_Avg(evaluateFactorCodes, WQIValuesDictionary);
                        if (!string.IsNullOrWhiteSpace(WQI_Avg))
                        {
                            drReport["EQI"] = WQI_Avg;
                        }
                    }

                    //如果是去年同期的月，则记录指数
                    if (strDateTime == strDateTimes[0])
                    {
                        lastYearIndex = decimal.TryParse(drReport["EQI"].ToString(), out lastYearIndex) ? lastYearIndex : 0;
                    }

                    //如果是上一周期的月，则记录指数
                    if (strDateTime == strDateTimes[1])
                    {
                        lastCycleIndex = decimal.TryParse(drReport["EQI"].ToString(), out lastCycleIndex) ? lastCycleIndex : 0;
                    }

                    //如果是当前选择的年，则记录综合污染指数、水质类别
                    if (strDateTime == strDateTimes[2])
                    {
                        currIndex = decimal.TryParse(drReport["EQI"].ToString(), out currIndex) ? currIndex : 0;//综合污染指数
                        drNew["SequentialPollutantIndex"] = drReport["EQI"];//综合污染指数
                        drNew["WaterQualityClassification"] = drReport["Grade"];//水质类别
                        sequentialIndexTotal += currIndex;//综合污染指数合计
                    }
                }
                if (drsReport.Length > 0)
                {
                    pointCount++;
                    decimal linkRelativeRatioRiseRate = (lastCycleIndex > 0) ? Math.Round((currIndex - lastCycleIndex) * 100 / lastCycleIndex, 2) : 0; //环比上升比例（上一个周期）
                    decimal yearOnYearRiseRate = (lastYearIndex > 0) ? Math.Round((currIndex - lastYearIndex) * 100 / lastYearIndex, 2) : 0; //同比上升比例（上一年度相同周期）

                    if (linkRelativeRatioRiseRate > 0)
                    {
                        drNew["LinkRelativeRatio_PollutantTrend"] = "上升";//环比污染趋势
                    }
                    else if (linkRelativeRatioRiseRate == 0)
                    {
                        drNew["LinkRelativeRatio_PollutantTrend"] = "持平";//环比污染趋势
                    }
                    else
                    {
                        drNew["LinkRelativeRatio_PollutantTrend"] = "下降";//环比污染趋势
                    }
                    drNew["LinkRelativeRatio_RiseRate"] = linkRelativeRatioRiseRate.ToString("0.00") + "%";//环比上升比例（%）
                    if (yearOnYearRiseRate > 0)
                    {
                        drNew["YearOnYear_PollutantTrend"] = "上升";//同比污染趋势
                    }
                    else if (yearOnYearRiseRate == 0)
                    {
                        drNew["YearOnYear_PollutantTrend"] = "持平";//同比污染趋势
                    }
                    else
                    {
                        drNew["YearOnYear_PollutantTrend"] = "下降";//同比污染趋势
                    }
                    drNew["YearOnYear_RiseRate"] = yearOnYearRiseRate.ToString() + "%";//同比上升比例（%）
                }
                dtNew.Rows.Add(drNew);//添加新行
            }
            if (pointCount > 0)
            {
                decimal sequentialPollutantIndex = Math.Round(sequentialIndexTotal / pointCount, 2);
                dtAvgIndex.Rows[4]["SequentialPollutantIndex"] = sequentialPollutantIndex;//平均综合污染指数
                dtAvgIndex.Rows[1]["SequentialPollutantIndex"] = sequentialPollutantIndex;//平均污染指数的综合污染指数列
            }
            ComputeFactorPollutantIndexAvgData(factors, strDateTimes, strDateType, dtNew, dtAvgIndex);//计算平均数据
            ComputeFactorPollutantLevelSorting(factors, strDateTimes, strDateType, dtNew, dtAvgIndex);//污染程度排序
        }

        /// <summary>
        /// 为表添加数据行(不计EQI)
        /// </summary>
        /// <param name="pointIds">测点数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="strDateTimes">时间周期数组</param>
        /// <param name="type">日期类型</param>
        /// <param name="dcName1">日期列名1</param>
        /// <param name="dcName2">日期列名2</param>
        /// <param name="dtNew">要添加数据行的表</param>
        /// <param name="dtReport">提供数据的表</param>
        /// <param name="dtAvgIndex">平均综合污染指数
        /// 返回的列名同GetHourWithTheSequentialData
        /// PointId列固定值为：0行：平均浓度值、1行：平均污染指数、2行：分担率、3行：污染物排名、4行：平均综合污染指数
        /// </param>
        private void AddNewRowToDataTableByLZFactor(string[] pointIds, string[] factors, string[] strDateTimes, string type, string dcName1, string dcName2,
            DataTable dtNew, DataTable dtReport, DataTable dtAvgIndex)
        {
            WaterQualityService waterQualityService = new WaterQualityService();//提供污染等级服务（如空气质量指数、水质等级）
            WaterQualityRepository waterQualityRepository = Singleton<WaterQualityRepository>.GetInstance();//水质标准处理仓储层
            
            //DataTable dtWaterAnalyzeData = waterQualityRepository.GetWaterAnalyzeData(pointIds).Table;//根据站点获取水质分析数据
            Dictionary<string, string> siteTypeByPointIdsList = GetSiteTypeByPointIds(pointIds);//根据站点Id数组获取站点Id和站点类型对应键值对
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(pointIds);//根据测点Id数组获取测点Id和对应的因子Code
            int pointCount = 0;//测点数量
            decimal sequentialIndexTotal = 0;//综合污染指数合计
            string strDateType = (type == "时" || type == "日") ? string.Empty : type;
            //string[] waterQualityPollutantCodes = GetWaterQualityPollutantCodes();//取得参与评价的水质因子
            pointIds = pointIds.Distinct().ToArray();

            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                string WQI_Avg = string.Empty;
                DataRow drNew = dtNew.NewRow();
                DataRow[] drsReport = dtReport.Select(string.Format("PointId='{0}'", pointId));
                //DataRow[] drsWaterAnalyzeData = dtWaterAnalyzeData.Select(string.Format("PointId='{0}'", pointId));
                decimal lastCycleIndex = 0;//上一周期的指数
                decimal lastYearIndex = 0;//去年同期的指数
                decimal currIndex = 0;//当前选择的月的指数
                WaterQualityClass waterQualityClass = WaterQualityClass.One;
                WaterPointCalWQType waterPointCalWQType = WaterPointCalWQType.River;
                IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                     ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                drNew["SiteTypeName"] = (siteTypeByPointIdsList.ContainsKey(pointId))
                      ? siteTypeByPointIdsList[pointId] : string.Empty;//站点类型名称??地表水 类型
                drNew["PointId"] = pointId;

                //if (drsWaterAnalyzeData != null && drsWaterAnalyzeData.Length > 0)
                //{
                //    DataRow drWaterAnalyzeData = drsWaterAnalyzeData[0];
                //    string strIEQI = drWaterAnalyzeData["IEQI"].ToString().Trim();//水质类别（数字）
                //    waterPointCalWQType = drWaterAnalyzeData["CalEQIType"].ToString().Trim() == "河流" ? WaterPointCalWQType.River : WaterPointCalWQType.Lake;

                //    switch (strIEQI)
                //    {
                //        case "1":
                //            waterQualityClass = WaterQualityClass.One;
                //            break;
                //        case "2":
                //            waterQualityClass = WaterQualityClass.Two;
                //            break;
                //        case "3":
                //            waterQualityClass = WaterQualityClass.Three;
                //            break;
                //        case "4":
                //            waterQualityClass = WaterQualityClass.Four;
                //            break;
                //        case "5":
                //            waterQualityClass = WaterQualityClass.Five;
                //            break;
                //        case "6":
                //            waterQualityClass = WaterQualityClass.BadFive;
                //            break;
                //        default:
                //            break;
                //    }
                //}
                foreach (DataRow drReport in drsReport)
                {
                    string strYear = (dtReport.Columns.Contains(dcName1)) ? drReport[dcName1].ToString() : string.Empty;//年
                    string strCycle = (dtReport.Columns.Contains(dcName2)) ? drReport[dcName2].ToString() : string.Empty;//周期（如：周、月、季、年）
                    string strDateTime = strDateTimes[0];
                    string evaluateFactorCodes = string.Empty;
                    Dictionary<string, decimal> WQIValuesDictionary = new Dictionary<string, decimal>();//因子指数列表（Key:PollutantCode、Value:分指数）

                    if (dtReport.Columns.Contains(dcName1))
                    {
                        if (type == "时")
                        {
                            strYear = string.Format("{0:yyyy-MM-dd HH:00}", drReport[dcName1]);
                        }
                        else if (type == "日")
                        {
                            strYear = string.Format("{0:yyyy-MM-dd}", drReport[dcName1]);
                        }
                    }
                    if (string.IsNullOrWhiteSpace(strCycle))
                    {
                        strDateTime = strYear;
                    }
                    else
                    {
                        strDateTime = strYear + "-" + strCycle;
                    }
                    foreach (string pollutantCode in factors)
                    {
                        decimal pollutantValue = decimal.TryParse(drReport[pollutantCode].ToString(), out pollutantValue) ? pollutantValue : 0;
                        if (dtNew.Columns.Contains(strDateTime + strDateType + "_" + pollutantCode))
                        {
                            drNew[strDateTime + strDateType + "_" + pollutantCode] = drReport[pollutantCode];//浓度值
                        }
                        if (dtNew.Columns.Contains(strDateTime + strDateType + "_" + pollutantCode + "_EQI"))
                        {
                            drNew[strDateTime + strDateType + "_" + pollutantCode + "_EQI"] = drReport[pollutantCode + "_EQI"];//EQI值
                        }
                        if (dtNew.Columns.Contains(strDateTime + strDateType + "_" + pollutantCode + "_Grade"))
                        {
                            drNew[strDateTime + strDateType + "_" + pollutantCode + "_Grade"] = drReport[pollutantCode + "_Grade"];//水质类别值
                        }
                        if (pollutantCodeList.Contains(pollutantCode))
                        {
                            decimal WQI = waterQualityService.GetWQI(pollutantCode, pollutantValue, waterQualityClass,
                                                EQITimeType.One, waterPointCalWQType);//计算水质污染指数
                            evaluateFactorCodes += pollutantCode + ";";
                            //drNew[strDateTime + strDateType + "_" + pollutantCode + "_EQI"] = WQI;//EQI值
                            drReport[pollutantCode + "_EQI"] = WQI;
                            WQIValuesDictionary.Add(pollutantCode, WQI);//添加到WQI键值对
                        }
                    }
                    evaluateFactorCodes = evaluateFactorCodes.TrimEnd(';');
                    if (evaluateFactorCodes.Length > 0 && WQIValuesDictionary.Count > 0)
                    {
                        WQI_Avg = waterQualityService.GetWQI_Avg(evaluateFactorCodes, WQIValuesDictionary);
                        if (!string.IsNullOrWhiteSpace(WQI_Avg))
                        {
                            drReport["EQI"] = WQI_Avg;
                        }
                    }

                    //如果是去年同期，则记录指数
                    if (strDateTime == strDateTimes[0])
                    {
                        lastYearIndex = decimal.TryParse(drReport[factors[0]].ToString(), out lastYearIndex) ? lastYearIndex : 0;
                    }

                    //如果是上一周期，则记录指数
                    if (strDateTime == strDateTimes[1])
                    {
                        lastCycleIndex = decimal.TryParse(drReport[factors[0]].ToString(), out lastCycleIndex) ? lastCycleIndex : 0;
                    }

                    //如果是当前选择期，则记录综合污染指数、水质类别
                    if (strDateTime == strDateTimes[2])
                    {
                        currIndex = decimal.TryParse(drReport[factors[0]].ToString(), out currIndex) ? currIndex : 0;//综合污染指数
                        drNew["SequentialPollutantIndex"] = drReport["EQI"];//综合污染指数
                        drNew["WaterQualityClassification"] = drReport["Grade"];//水质类别
                        sequentialIndexTotal += currIndex;//综合污染指数合计
                    }
                }
                if (drsReport.Length > 0)
                {
                    pointCount++;
                    decimal linkRelativeRatioRiseRate = (lastCycleIndex > 0) ? Math.Round((currIndex - lastCycleIndex) * 100 / lastCycleIndex, 2) : 0; //环比上升比例（上一个周期）
                    decimal yearOnYearRiseRate = (lastYearIndex > 0) ? Math.Round((currIndex - lastYearIndex) * 100 / lastYearIndex, 2) : 0; //同比上升比例（上一年度相同周期）

                    if (linkRelativeRatioRiseRate > 0)
                    {
                        drNew["LinkRelativeRatio_PollutantTrend"] = "上升";//环比污染趋势
                    }
                    else if (linkRelativeRatioRiseRate == 0 && currIndex > 0 && lastCycleIndex>0)
                    {
                        drNew["LinkRelativeRatio_PollutantTrend"] = "持平";//环比污染趋势
                    }

                    else if (linkRelativeRatioRiseRate < 0 && linkRelativeRatioRiseRate > -100)
                    {
                        drNew["LinkRelativeRatio_PollutantTrend"] = "下降";//环比污染趋势
                    }
                    else
                    {
                        drNew["LinkRelativeRatio_PollutantTrend"] = DBNull.Value;
                    }
                    if (currIndex != 0 && lastCycleIndex != 0)
                    {
                        drNew["LinkRelativeRatio_RiseRate"] = linkRelativeRatioRiseRate.ToString("0.00") + "%";//环比上升比例（%）
                    }
                    
                    if (yearOnYearRiseRate > 0)
                    {
                        drNew["YearOnYear_PollutantTrend"] = "上升";//同比污染趋势
                    }
                    else if (yearOnYearRiseRate == 0 && currIndex > 0 && lastYearIndex>0)
                    {
                        drNew["YearOnYear_PollutantTrend"] = "持平";//同比污染趋势
                    }

                    else if (yearOnYearRiseRate < 0 && yearOnYearRiseRate>-100)
                    {
                        drNew["YearOnYear_PollutantTrend"] = "下降";//同比污染趋势
                    }
                    else
                    {
                        drNew["YearOnYear_PollutantTrend"] = DBNull.Value;
                    }
                    if (currIndex != 0 && lastYearIndex != 0)
                    {
                        drNew["YearOnYear_RiseRate"] = yearOnYearRiseRate.ToString() + "%";//同比上升比例（%）
                    }
                }
                dtNew.Rows.Add(drNew);//添加新行
            }
            if (pointCount > 0)
            {
                decimal sequentialPollutantIndex = Math.Round(sequentialIndexTotal / pointCount, 2);
                dtAvgIndex.Rows[4]["SequentialPollutantIndex"] = sequentialPollutantIndex;//平均综合污染指数
                dtAvgIndex.Rows[1]["SequentialPollutantIndex"] = sequentialPollutantIndex;//平均污染指数的综合污染指数列
            }
            //ComputeFactorPollutantIndexAvgData(factors, strDateTimes, strDateType, dtNew, dtAvgIndex);//计算平均数据
            //ComputeFactorPollutantLevelSorting(factors, strDateTimes, strDateType, dtNew, dtAvgIndex);//污染程度排序
        }



        /// <summary>
        /// 计算平均数据
        /// </summary>
        /// <param name="factors">因子数据</param>
        /// <param name="strDateTimes">时间周期数组</param>
        /// <param name="strDateType">时间类型字符串</param>
        /// <param name="dtNew">数据表</param>
        /// <param name="dtAvgIndex">平均数据表</param>
        private void ComputeFactorPollutantIndexAvgData(string[] factors, string[] strDateTimes, string strDateType, DataTable dtNew, DataTable dtAvgIndex)
        {
            foreach (string strDateTime in strDateTimes)
            {
                decimal avgEQITotal = 0;//当前周期的总污染指数
                for (int i = 0; i < factors.Length; i++)
                {
                    string pollutantCode = factors[i];//因子
                    string curColumnName = strDateTime + strDateType + "_" + pollutantCode;

                    //如果表中不包含该列，则跳到下一个
                    if (!dtAvgIndex.Columns.Contains(curColumnName))
                    {
                        continue;
                    }

                    //平均浓度值
                    object objAvgValue = dtNew.Compute(string.Format("avg([{0}])", curColumnName),
                        string.Format("isnull([{0}],-1)<>-1", curColumnName));
                    if (!string.IsNullOrWhiteSpace(objAvgValue.ToString()))
                    {
                        decimal avgValue = decimal.TryParse(objAvgValue.ToString(), out avgValue) ? avgValue : 0;//平均浓度值
                        dtAvgIndex.Rows[0][curColumnName] = avgValue;//平均浓度值
                    }

                    //平均污染指数
                    object objAvgEQI = dtNew.Compute(string.Format("avg([{0}])", curColumnName + "_EQI"),
                        string.Format("isnull([{0}],-1)<>-1", curColumnName + "_EQI"));
                    if (!string.IsNullOrWhiteSpace(objAvgEQI.ToString()))
                    {
                        decimal avgEQI = decimal.TryParse(objAvgEQI.ToString(), out avgEQI) ? avgEQI : 0;//平均污染指数
                        dtAvgIndex.Rows[1][curColumnName] = avgEQI;//平均污染指数（该行的污染指数存放在Code列而不是Code_EQI列）
                        avgEQITotal += avgEQI;//当前周期的总污染指数
                    }
                }

                if (avgEQITotal > 0)
                {
                    //分担率 = 单污染指数 / 总污染指数
                    for (int i = 0; i < factors.Length; i++)
                    {
                        string pollutantCode = factors[i];//因子
                        string curColumnName = strDateTime + strDateType + "_" + pollutantCode;

                        //如果平均污染指数不为空，则计算分担率
                        if (!string.IsNullOrWhiteSpace(dtAvgIndex.Rows[1][curColumnName].ToString()))
                        {
                            decimal avgEQI = decimal.TryParse(dtAvgIndex.Rows[1][curColumnName].ToString(), out avgEQI) ? avgEQI : 0;//单污染指数
                            decimal sharingRate = Math.Round(avgEQI / avgEQITotal, 2);//分担率
                            dtAvgIndex.Rows[2][curColumnName] = sharingRate;//分担率
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 污染程度排序
        /// </summary>
        /// <param name="factors"></param>
        /// <param name="strDateTimes"></param>
        /// <param name="strDateType"></param>
        /// <param name="dt"></param>
        /// <param name="dtAvgIndex"></param>
        /// <returns></returns>
        private void ComputeFactorPollutantLevelSorting(string[] factors, string[] strDateTimes, string strDateType, DataTable dt, DataTable dtAvgIndex)
        {
            DataTable dtSort;//排序后的表
            DataView dv = dt.DefaultView;//排序用的视图
            int index = 1;//排序序号（从1开始）
            dv.Sort = "SequentialPollutantIndex Desc";
            dtSort = dv.ToTable();

            //污染程度排序
            for (int i = 0; i < dtSort.Rows.Count; i++)
            {
                //查询出dt中综合污染物指数为当前值的行
                DataRow[] drs = dt.Select(string.Format("SequentialPollutantIndex='{0}'", dtSort.Rows[i]["SequentialPollutantIndex"]));
                bool isAddIndex = false;//序号是否需要增加
                foreach (DataRow dr in drs)
                {
                    //当综合污染物指数不为空，并且污染排序程度为空时，记录排序序号
                    if (!string.IsNullOrWhiteSpace(dr["SequentialPollutantIndex"].ToString())
                        && string.IsNullOrWhiteSpace(dr["PollutantLevelSorting"].ToString()))
                    {
                        dr["PollutantLevelSorting"] = index;
                        isAddIndex = true;
                    }
                }
                index = isAddIndex ? index + 1 : index;
            }

            //因子污染程度排序
            foreach (string strDateTime in strDateTimes)
            {
                foreach (string pollutantCode in factors)
                {
                    int pollutantIndex = 1;//因子排序序号（从1开始）
                    string curColumnName = strDateTime + strDateType + "_" + pollutantCode;//当前要排序的列名

                    //如果没有该污染物列，则跳到下一个
                    if (!dt.Columns.Contains(curColumnName + "_EQI"))
                    {
                        continue;
                    }
                    dv.Sort = string.Format("[{0}] Desc", curColumnName + "_EQI");//根据污染指数排序
                    dtSort = dv.ToTable();
                    for (int i = 0; i < dtSort.Rows.Count; i++)
                    {
                        //如果当前污染指数列的值为空，则跳到下一个
                        if (string.IsNullOrWhiteSpace(dtSort.Rows[i][curColumnName + "_EQI"].ToString()))
                        {
                            continue;
                        }

                        //查询出dt中当前污染物指数为当前值的行
                        DataRow[] drs = dt.Select(string.Format("[{0}]='{1}'", curColumnName + "_EQI", dtSort.Rows[i][curColumnName + "_EQI"]));
                        bool isAddIndex = false;//序号是否需要增加
                        foreach (DataRow dr in drs)
                        {
                            //当当前污染物浓度不为空，并且污染排序程度为空时，记录排序序号
                            if (!string.IsNullOrWhiteSpace(dr[curColumnName + "_EQI"].ToString())
                                && string.IsNullOrWhiteSpace(dr[curColumnName + "_Sort"].ToString()))
                            {
                                dr[curColumnName + "_Sort"] = pollutantIndex;
                                isAddIndex = true;
                            }
                        }
                        pollutantIndex = isAddIndex ? pollutantIndex + 1 : pollutantIndex;
                    }
                }
            }

            //污染物排名
            foreach (string strDateTime in strDateTimes)
            {
                Dictionary<string, decimal> pollutantIndexList = new Dictionary<string, decimal>();//污染物指数列表
                foreach (string pollutantCode in factors)
                {
                    string curColumnName = strDateTime + strDateType + "_" + pollutantCode;//污染物列名
                    if (dtAvgIndex.Columns.Contains(curColumnName)
                        && !string.IsNullOrWhiteSpace(dtAvgIndex.Rows[1][curColumnName].ToString())
                        && !pollutantIndexList.ContainsKey(curColumnName))
                    {
                        decimal avgIndex = decimal.TryParse(dtAvgIndex.Rows[1][curColumnName].ToString(), out avgIndex) ? avgIndex : 0;//平均污染指数（该行的污染指数存放在Code列而不是Code_EQI列）
                        pollutantIndexList.Add(curColumnName, avgIndex);
                    }
                }
                IList<KeyValuePair<string, decimal>> pollutantIndexSortList = pollutantIndexList.OrderByDescending(t => t.Value).ToList();//排序
                for (int i = 0; i < pollutantIndexSortList.Count; i++)
                {
                    KeyValuePair<string, decimal> keyValue = pollutantIndexSortList[i];
                    if (dtAvgIndex.Columns.Contains(keyValue.Key))
                    {
                        dtAvgIndex.Rows[3][keyValue.Key] = i + 1;//污染物排名
                    }
                }
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 拼接数据表
        /// </summary>
        /// <param name="dtArray">表数组</param>
        /// <returns></returns>
        private DataTable UnionDataTable(params DataTable[] dtArray)
        {
            DataTable dtAll = null;
            if (dtArray == null || dtArray.Length == 0)
            {
                return dtAll;
            }
            dtAll = dtArray[0];
            for (int i = 1; i < dtArray.Length; i++)
            {
                DataTable dt = dtArray[i];
                if (dtAll == null || dtAll.Rows.Count == 0)
                {
                    dtAll = dt;
                    continue;
                }
                else if (dt != null && dt.Rows.Count > 0)
                {
                    dtAll = dtAll.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                }
            }
            return dtAll;
        }

        /// <summary>
        /// 根据年数据生成年度平均综合污染指数的表
        /// </summary>
        /// <param name="years">年数据</param>
        /// <returns></returns>
        private DataTable CreateYearAvgPollutantDataTable(int[] years)
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("PointId", typeof(string));
            for (int i = 0; i < years.Length; i++)
            {
                dtNew.Columns.Add(years[i].ToString() + "年均值", typeof(decimal));
            }
            dtNew.Columns.Add("blankspaceColumn");//空白列
            return dtNew;
        }

        /// <summary>
        /// 按时间生成数据同环比分析表
        /// </summary>
        /// <param name="strDateTimes">时间字符串数组</param>
        /// <param name="type">时间的类型（时，日，周，月，季，年）</param>
        /// <returns></returns>
        private DataTable CreateSequentialDataTableByDateTime(string[] strDateTimes, string type)
        {
            DataTable dtNew = new DataTable();
            string strDateType = (type == "时" || type == "日") ? string.Empty : type;
            dtNew.Columns.Add("SiteTypeName", typeof(string));
            dtNew.Columns.Add("PointId", typeof(string));
            dtNew.Columns.Add(strDateTimes[0] + strDateType, typeof(decimal));//上一年同期
            dtNew.Columns.Add(strDateTimes[1] + strDateType, typeof(decimal));//上一周期
            dtNew.Columns.Add(strDateTimes[2] + strDateType, typeof(decimal));//当前年周期
            dtNew.Columns.Add("SequentialPollutantIndex", typeof(decimal));//综合污染指数
            dtNew.Columns.Add("PollutantLevelSorting", typeof(int));//污染程度排序
            dtNew.Columns.Add("WaterQualityClassification", typeof(string));//水质类别
            dtNew.Columns.Add("LinkRelativeRatio_PollutantTrend", typeof(string));//环比污染趋势
            dtNew.Columns.Add("LinkRelativeRatio_RiseRate", typeof(string));//环比上升比例（%）
            dtNew.Columns.Add("YearOnYear_PollutantTrend", typeof(string));//同比污染趋势
            dtNew.Columns.Add("YearOnYear_RiseRate", typeof(string));//同比上升比例（%）
            dtNew.Columns.Add("blankspaceColumn");//空白列
            return dtNew;
        }

        /// <summary>
        /// 按时间和因子生成数据同环比分析表
        /// </summary>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="strDateTimes">时间字符串数组</param>
        /// <param name="type">时间的类型（时，日，周，月，季，年）</param>
        /// <returns></returns>
        private DataTable CreateSequentialDataTableByFactorDate(string[] pollutantCodes, string[] strDateTimes, string type)
        {
            DataTable dtNew = new DataTable();
            string strDateType = (type == "时" || type == "日") ? string.Empty : type;
            dtNew.Columns.Add("SiteTypeName", typeof(string));
            dtNew.Columns.Add("PointId", typeof(string));

            //上一年同期
            for (int i = 0; i < pollutantCodes.Length; i++)
            {
                string pollutantCode = pollutantCodes[i];
                dtNew.Columns.Add(strDateTimes[0] + strDateType + "_" + pollutantCode, typeof(decimal));//浓度
                dtNew.Columns.Add(strDateTimes[0] + strDateType + "_" + pollutantCode + "_EQI", typeof(decimal));//污染指数
                dtNew.Columns.Add(strDateTimes[0] + strDateType + "_" + pollutantCode + "_Grade", typeof(decimal));//水质类别
                dtNew.Columns.Add(strDateTimes[0] + strDateType + "_" + pollutantCode + "_Sort", typeof(int));//污染程度排序
            }

            //上一周期
            for (int i = 0; i < pollutantCodes.Length; i++)
            {
                string pollutantCode = pollutantCodes[i];
                dtNew.Columns.Add(strDateTimes[1] + strDateType + "_" + pollutantCode, typeof(decimal));//浓度
                dtNew.Columns.Add(strDateTimes[1] + strDateType + "_" + pollutantCode + "_EQI", typeof(decimal));//污染指数
                dtNew.Columns.Add(strDateTimes[1] + strDateType + "_" + pollutantCode + "_Grade", typeof(decimal));//水质类别
                dtNew.Columns.Add(strDateTimes[1] + strDateType + "_" + pollutantCode + "_Sort", typeof(int));
            }

            //当前年周期
            for (int i = 0; i < pollutantCodes.Length; i++)
            {
                string pollutantCode = pollutantCodes[i];
                dtNew.Columns.Add(strDateTimes[2] + strDateType + "_" + pollutantCode, typeof(decimal));//浓度
                dtNew.Columns.Add(strDateTimes[2] + strDateType + "_" + pollutantCode + "_EQI", typeof(decimal));//污染指数
                dtNew.Columns.Add(strDateTimes[2] + strDateType + "_" + pollutantCode + "_Grade", typeof(decimal));//水质类别
                dtNew.Columns.Add(strDateTimes[2] + strDateType + "_" + pollutantCode + "_Sort", typeof(int));//污染程度排序
            }
            dtNew.Columns.Add("SequentialPollutantIndex", typeof(decimal));//综合污染指数
            dtNew.Columns.Add("PollutantLevelSorting", typeof(int));//污染程度排序
            dtNew.Columns.Add("WaterQualityClassification", typeof(string));//水质类别
            dtNew.Columns.Add("LinkRelativeRatio_PollutantTrend", typeof(string));//环比污染趋势
            dtNew.Columns.Add("LinkRelativeRatio_RiseRate", typeof(string));//环比上升比例（%）
            dtNew.Columns.Add("YearOnYear_PollutantTrend", typeof(string));//同比污染趋势
            dtNew.Columns.Add("YearOnYear_RiseRate", typeof(string));//同比上升比例（%）
            dtNew.Columns.Add("blankspaceColumn");//空白列
            return dtNew;
        }

        /// <summary>
        /// 生成月同环比的表
        /// </summary>
        /// <param name="years">年数据</param>
        /// <param name="months">月数据</param>
        /// <returns></returns>
        private DataTable CreateMonthWithTheSequentialDataTable(int[] years, int[] months)
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("SiteTypeName", typeof(string));
            dtNew.Columns.Add("PointId", typeof(string));
            for (int i = 0; i < years.Length; i++)
            {
                int year = years[i];
                for (int j = 0; j < months.Length; j++)
                {
                    int month = months[j];
                    dtNew.Columns.Add(year.ToString() + "-" + month.ToString() + "月", typeof(string));
                }
            }
            dtNew.Columns.Add("SequentialPollutantIndex", typeof(string));//综合污染指数
            dtNew.Columns.Add("WaterQualityClassification", typeof(string));//水质类别
            dtNew.Columns.Add("LinkRelativeRatio_PollutantTrend", typeof(string));//环比污染趋势
            dtNew.Columns.Add("LinkRelativeRatio_RiseRate", typeof(string));//环比上升比例（%）
            dtNew.Columns.Add("YearOnYear_PollutantTrend", typeof(string));//同比污染趋势
            dtNew.Columns.Add("YearOnYear_RiseRate", typeof(string));//同比上升比例（%）
            dtNew.Columns.Add("blankspaceColumn");//空白列
            return dtNew;
        }

        /// <summary>
        /// 生成因子同环比的表
        /// </summary>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="years">年数据</param>
        /// <param name="months">月数据</param>
        /// <returns></returns>
        private DataTable CreatePollutantWithTheSequentialDataTable(string[] pollutantCodes, int[] years, int[] months)
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("SiteTypeName", typeof(string));
            dtNew.Columns.Add("PointId", typeof(string));
            for (int i = 0; i < years.Length; i++)
            {
                int year = years[i];
                for (int j = 0; j < months.Length; j++)
                {
                    int month = months[j];
                    string yearMonth = year.ToString() + "-" + month.ToString() + "月";
                    for (int k = 0; k < pollutantCodes.Length; k++)
                    {
                        string pollutantCode = pollutantCodes[k];
                        dtNew.Columns.Add(yearMonth + "_" + pollutantCode, typeof(string));
                    }
                }
            }
            dtNew.Columns.Add("SequentialPollutantIndex", typeof(string));//综合污染指数
            dtNew.Columns.Add("WaterQualityClassification", typeof(string));//水质类别
            dtNew.Columns.Add("LinkRelativeRatio_PollutantTrend", typeof(string));//环比污染趋势
            dtNew.Columns.Add("LinkRelativeRatio_RiseRate", typeof(string));//环比上升比例（%）
            dtNew.Columns.Add("YearOnYear_PollutantTrend", typeof(string));//同比污染趋势
            dtNew.Columns.Add("YearOnYear_RiseRate", typeof(string));//同比上升比例（%）
            dtNew.Columns.Add("blankspaceColumn");//空白列
            return dtNew;
        }

        /// <summary>
        /// 根据旧表生成平均值表并添加相关行
        /// </summary>
        /// <param name="dtOld">旧的表</param>
        /// <returns></returns>
        private DataTable CreateAvgDataTableAndAddRow(DataTable dtOld)
        {
            DataTable dtNew = dtOld.Clone();
            DataRow drAvgValue = dtNew.NewRow();
            drAvgValue["PointId"] = "平均浓度值";
            dtNew.Rows.Add(drAvgValue);
            DataRow drSingleIndex = dtNew.NewRow();
            drSingleIndex["PointId"] = "平均污染指数";
            dtNew.Rows.Add(drSingleIndex);
            DataRow drRate = dtNew.NewRow();
            drRate["PointId"] = "分担率";
            dtNew.Rows.Add(drRate);
            DataRow drRanking = dtNew.NewRow();
            drRanking["PointId"] = "污染物排名";
            dtNew.Rows.Add(drRanking);
            DataRow drAvgIndex = dtNew.NewRow();
            drAvgIndex["PointId"] = "平均综合污染指数";
            dtNew.Rows.Add(drAvgIndex);
            return dtNew;
        }

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <param name="dtOld">原始表</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序方式（如：PointId,Tstamp）</param>
        /// <returns></returns>
        private DataTable GetDataPagerByPageSize(DataTable dtOld, int pageSize, int pageNo, string orderBy)
        {
            if (dtOld != null)
            {
                int startIndex = pageSize * pageNo;//要返回的数据的开始位置(从0开始)
                int endIndex = pageSize * (pageNo + 1);//要返回的数据的结束位置(数据取到小于这个值)
                DataTable dtReturn = dtOld.Clone();
                endIndex = (endIndex > dtOld.Rows.Count) ? dtOld.Rows.Count : endIndex;//如果结束位置大于最大行数，则改为最大行数的值
                orderBy = (orderBy != null) ? orderBy : string.Empty;
                string[] orderByValues = orderBy.Split(',');
                bool isOrderByContains = true;
                foreach (string orderByValue in orderByValues)
                {
                    isOrderByContains = dtOld.Columns.Contains(orderByValue);
                }
                if (isOrderByContains)
                {
                    dtOld.DefaultView.Sort = orderBy;
                    dtOld = dtOld.DefaultView.ToTable();
                }
                for (int i = startIndex; i < endIndex; i++)
                {
                    DataRow dr = dtOld.Rows[i];
                    dtReturn.Rows.Add(dr.ItemArray);
                }
                return dtReturn;
            }
            return null;
        }

        /// <summary>
        /// 根据测点Id数组获取因子列
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <returns></returns>
        private string[] GetPollutantCodesByPointIds(string[] pointIds)
        {
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointWater.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            string[] pollutantCodes = GetPollutantCodesByPointEntitys(monitorPointQueryable.ToArray());
            return pollutantCodes;
        }

        /// <summary>
        /// 根据测点数组获取因子列
        /// </summary>
        /// <param name="monitoringPointEntitys">测点数组</param>
        /// <returns></returns>
        private string[] GetPollutantCodesByPointEntitys(MonitoringPointEntity[] monitoringPointEntitys)
        {
            IList<string> pollutantList = new List<string>();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            foreach (MonitoringPointEntity monitoringPointEntity in monitoringPointEntitys)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                    instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                foreach (PollutantCodeEntity pollutantCodeEntity in pollutantCodeQueryable)
                {
                    pollutantList.Add(pollutantCodeEntity.PollutantCode);
                }
            }
            return pollutantList.ToArray();
        }

        /// <summary>
        /// 根据测点Id数组获取因子数据列
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantDataByPointIds(string[] pointIds)
        {
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointWater.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            IList<PollutantCodeEntity> pollutantCodeList = null;
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                            instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);
                if (pollutantCodeList == null)
                {
                    pollutantCodeList = pollutantCodeQueryable.ToList();
                }
                else
                {
                    pollutantCodeList = pollutantCodeList.Union(pollutantCodeQueryable).ToList();
                }
            }
            return pollutantCodeList;
        }

        /// <summary>
        /// 根据因子Code数组获取因子数据列
        /// </summary>
        /// <param name="pollutantCodes">因子Code数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantDataByPollutantCodes(string[] pollutantCodes)
        {
            PollutantCodeRepository channelRepository = new PollutantCodeRepository();
            IList<PollutantCodeEntity> pollutantCodeList = channelRepository.Retrieve(t => pollutantCodes.Contains(t.PollutantCode)).ToList();
            return pollutantCodeList;
        }

        /// <summary>
        /// 根据测点Id数组获取测点Id和对应的因子Code
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        private Dictionary<string, IList<string>> GetPointPollutantCodeByPointIds(string[] pointIds)
        {
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointWater.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            Dictionary<string, IList<string>> pointPollutantCodeList = new Dictionary<string, IList<string>>();
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                if (!pointPollutantCodeList.ContainsKey(monitoringPointEntity.PointId.ToString()))
                {
                    IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                        instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                    pointPollutantCodeList.Add(monitoringPointEntity.PointId.ToString(), pollutantCodeQueryable.Select(t => t.PollutantCode).ToList());
                }
            }
            return pointPollutantCodeList;
        }

        /// <summary>
        /// 根据站点Id数组获取站点Id和站点类型对应键值对
        /// </summary>
        /// <param name="pointIds">站点Id数组</param>
        /// <returns></returns>
        private Dictionary<string, string> GetSiteTypeByPointIds(string[] pointIds)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (pointIds == null || pointIds.Length == 0)
            {
                return dictionary;
            }
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointWater.RetrieveListByPointIds(pointIds); //根据站点ID数组获取站点
            IQueryable<V_CodeMainItemEntity> codeMainItemQueryable = dictionaryService.RetrieveList(DictionaryType.Water, "地表水站点类型");//获取城市类型
            foreach (MonitoringPointEntity monitorPointEntity in monitorPointQueryable)
            {
                if (!dictionary.ContainsKey(monitorPointEntity.PointId.ToString()))
                {
                    string siteTypeName = codeMainItemQueryable.Where(t => t.ItemGuid.Equals(monitorPointEntity.SiteTypeUid))
                            .Select(t => t.ItemText).FirstOrDefault();//站点类型名称
                    dictionary.Add(monitorPointEntity.PointId.ToString(), siteTypeName);
                }
            }
            return dictionary;
        }

        /// <summary>
        /// 根据测点Id数组增加空白数据行
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <param name="dt">表</param>
        private void AddDataRowsByPointIds(string[] pointIds, DataTable dt)
        {
            if (pointIds == null || pointIds.Length == 0 || dt == null)
            {
                return;
            }
            pointIds = pointIds.Distinct().ToArray();
            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                if (dt.Select(string.Format("PointId='{0}'", pointId)).Length == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["PointId"] = pointId;
                    dt.Rows.Add(dr);
                }
            }
        }

        /// <summary>
        /// 取得参与评价的水质因子
        /// </summary>
        /// <returns></returns>
        /// w01010		水温		Temperature
        /// w21011		总磷		Phosphorus(total)
        /// w21017		氟化物		Fluoride
        /// w20120		总铅		Lead(total)
        /// w20111		总汞		Mercury(total)
        /// w21001		总氮		Nitrogen (total)
        /// w21019	    硫化物		Sulphide 
        /// w20122		总铜		Copper(total)
        /// w20117		六价铬		Chromium (VI) Com
        /// w01009		溶解氧		Dissolved Oxygen
        /// w20115		总镉		Cadmium(total)
        /// w20123		总锌		Zinc(total)
        /// w01017	 	五日生化需氧量		Biochemical Oxygen Demand After 5 days(BOD5)
        /// w20128	    总硒		Selenium(total)
        /// w22001 	    石油类		Petroleum Oil
        /// w21003	    氨氮		Ammonnia-Nitrogen
        /// w01019	    高锰酸盐指数		Permanganate Index
        /// w02003	    粪大肠菌群		Fecal Coliform 
        /// w20119	    总砷		Arsenic(total)
        /// w21016	    氰化物		Cyanide 
        /// w23002	    挥发酚		Volatile Phenols
        /// w01018	    化学需氧量		Chemical Oxygen Demand(COD)
        /// w01001	    pH值		pH
        /// w19002	    阴离子表面活性剂		Anion Surface Active Agent 
        private string[] GetWaterQualityPollutantCodes()
        {
            return new string[] { "w01010", "w01001", "w01009", "w01019", "w21003", "w21011", "w21001" };
        }
        #endregion
    }
}
