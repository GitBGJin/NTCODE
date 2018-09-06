using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.AutoMonitoring.Interfaces;

namespace SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest
{
    /// <summary>
    /// 名称：AuditData.ashx.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-09-29
    /// 维护人员：
    /// 最新维护人员：徐阳   
    /// 最新维护日期：2017-06-08
    /// 功能摘要：实时小时数据【Chart】
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditData : IHttpHandler
    {
        DataCompare m_CompareData = Singleton<DataCompare>.GetInstance();
        private IInfectantDALService g_IInfectantDALService = null;
        InfectantByDayService m_DayOriData = Singleton<InfectantByDayService>.GetInstance();
        InfectantBy60Service m_Min60Data = Singleton<InfectantBy60Service>.GetInstance();
        InfectantByMonthService m_MonthOriData = Singleton<InfectantByMonthService>.GetInstance();
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        DataQueryByDayService m_DayData = Singleton<DataQueryByDayService>.GetInstance();
        DataQueryByWeekService m_WeekData = Singleton<DataQueryByWeekService>.GetInstance();
        DataQueryByMonthService m_MonthData = Singleton<DataQueryByMonthService>.GetInstance();
        DataQueryBySeasonService m_SeasonData = Singleton<DataQueryBySeasonService>.GetInstance();
        DataQueryByYearService m_YearData = Singleton<DataQueryByYearService>.GetInstance();
        private ExcessiveSettingService excessiveService = new ExcessiveSettingService();
        private HighChartJsonData jsonData = new HighChartJsonData();
        public string ChartType = "spline";
        public string PointFactor = "";
        private string region = "";
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string name = context.Request["Name"] != null ? context.Request["Name"].ToString() : "";
                region = context.Request["Region"] != null ? context.Request["Region"].ToString() : "";
                string[] PointID = context.Request["PointID"] != null ? context.Request["PointID"].ToString().Split(';') : null;
                string[] FactorCode = context.Request["FactorCode"] != null ? context.Request["FactorCode"].ToString().Split(';') : null;
                string radlDataType = context.Request["radlDataType"] != null ? context.Request["radlDataType"].ToString() : "";
                DateTime DtBegin = DateTime.Now;
                DateTime DtEnd = DateTime.Now;
                string timeStr = "";
                if (radlDataType == "Min1" || radlDataType == "Min5" || radlDataType == "Min60")
                {
                    g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType));
                }
                if (radlDataType.Equals("Hour") || radlDataType.Equals("Hourkqy") || radlDataType.Equals("Day") || radlDataType.Equals("Min1") || radlDataType.Equals("Min5") || radlDataType.Equals("Min60") || radlDataType.Equals("Min60s") || radlDataType.Equals("Min60kqy") || radlDataType.Equals("OriDay") || radlDataType.Equals("OriMonth") || radlDataType.Equals("HourCity") || radlDataType.Equals("DayCity") || radlDataType.Equals("HourCity") || radlDataType.Equals("Min60City") || radlDataType.Equals("OriDayCity") || radlDataType.Equals("OriMonthCity"))
                {
                    DtBegin = context.Request["DtBegin"] != null ? Convert.ToDateTime(context.Request["DtBegin"]) : DateTime.Now;
                    DtEnd = context.Request["DtEnd"] != null ? Convert.ToDateTime(context.Request["DtEnd"]) : DateTime.Now;
                }
                else
                {
                    timeStr = context.Request["DtBegin"] != null ? context.Request["DtBegin"] : "";
                }
                string pageType = context.Request["pageType"] != null ? context.Request["pageType"].ToString() : "";
                int PageSize = context.Request["PageSize"] != null ? Convert.ToInt32(context.Request["PageSize"].ToString()) : 100;
                int PageNo = context.Request["PageNo"] != null ? Convert.ToInt32(context.Request["PageNo"].ToString()) : 0;
                ChartType = context.Request["ChartType"] != null ? context.Request["ChartType"].ToString() : "spline";
                PointFactor = context.Request["PointFactor"] != null ? context.Request["PointFactor"].ToString() : "point";
                context.Response.ContentType = "text/plain";
                context.Response.Write(GetJsonData(PointID, FactorCode, DtBegin, DtEnd, timeStr, radlDataType, pageType, PageSize, PageNo, name));
            }
            catch
            {

            }
        }

        /// <summary>
        /// 获取实时数据Json格式
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="FactorCode"></param>
        /// <param name="DtBegin"></param>
        /// <param name="DtEnd"></param>
        /// <param name="radlDataType"></param>
        /// <param name="pageType"></param>
        /// <returns></returns>
        private string GetJsonData(string[] PointID, string[] FactorCode, DateTime DtBegin, DateTime DtEnd, string timeStr, string radlDataType, string pageType, int PageSize, int PageNo, string name)
        {
            string data = "";
            int recordTotal = 0;
            DataView auditData = new DataView();
            string XName = "Tstamp";
            string formatter = "";
            IQueryable<ExcessiveSettingInfo> excessiveList = excessiveService.RetrieveListByFactor(ApplicationValue.Air, FactorCode, PointID);

            if (PointFactor == "point")
            {
                //原始小时数据
                if (radlDataType == "Min60" && pageType!="Air1")
                {
                    XName = "Tstamp";
                    formatter = "%m/%d %H时";
                    //auditData = g_IInfectantDALService.GetDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                    if (pageType == "lzspy")//离子色谱仪数据晚两个小时
                    {
                        auditData = m_Min60Data.GetDataPagerForO3AllTimeLZSPY(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);//原始小时数据查询

                    }
                    else
                    {
                        //除离子色谱仪之外的其他仪器数据绑定
                        auditData = m_Min60Data.GetDataPagerForO3AllTime(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);//原始小时数据查询
                    }
                    data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //原始小时数据区域
                else if (radlDataType == "Min60City" && pageType == "Air1")
                {
                   XName = "Tstamp";
                    formatter = "%m/%d %H时";
                    auditData = m_Min60Data.GetDataPagerAllTimeWithO8Region(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                    auditData.RowFilter = "PointId='" + region + "'";
                    data = jsonData.GetChartDataRegion(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, region, name);
                }
                else if (radlDataType == "Min60" && pageType == "Air1")
                {
                  XName = "Tstamp";
                  formatter = "%m/%d %H时";
                  auditData = m_Min60Data.GetDataPagerAllTimeWithO8(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //VOCs外标
                if (radlDataType == "Min60s")
                {
                    XName = "Tstamp";
                    formatter = "%m/%d %H时";
                    auditData = m_Min60Data.GetVOCWDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal, "PointId asc,Tstamp asc");//60分钟类型按 60分钟数据查询
                    data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                if (radlDataType == "Min60kqy")
                {
                    XName = "Tstamp";
                    formatter = "%m/%d %H时";
                    auditData = m_Min60Data.GetVOCsKQYDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal, "PointId asc,Tstamp asc");//60分钟类型按 60分钟数据查询
                    data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //原始五分钟数据
                else if (radlDataType == "Min5")
                {
                    XName = "Tstamp";
                    formatter = "%m/%d %H时%M分";
                    auditData = g_IInfectantDALService.GetDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                    data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //原始一分钟数据
                else if (radlDataType == "Min1")
                {
                    XName = "Tstamp";
                    formatter = "%m/%d %H时%M分";
                    auditData = g_IInfectantDALService.GetDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                    data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                else if (radlDataType == "Hour" && pageType == "Air1")
                {
                  XName = "Tstamp";
                  formatter = "%m/%d %H时";
                  auditData = m_HourData.GetNewHourDataPagerWidthO8(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //小时数据并不是原始审核页面传来的数据
                else if (radlDataType == "Hour" && pageType != "Air1")
                {
                  XName = "Tstamp";
                  formatter = "%m/%d %H时";
                  auditData = m_HourData.GetNewHourDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                else if (radlDataType == "HourCity")
                {
                  XName = "Tstamp";
                  formatter = "%m/%d %H时";
                 
                  
                  auditData = m_HourData.GetNewHourDataPagerWidthRegionO8(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  auditData.RowFilter = "PointId='" + region + "'";
                  data = jsonData.GetChartDataRegion(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, region, name);
                  //XName = "Tstamp";
                  //formatter = "%m/%d %H时";
                  //auditData = m_HourData.GetNewHourDataPagerWidthRegionO8(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  //DataTable dtRegion = m_HourData.GetRegionWithPointId(PointID);
                  //data = jsonData.GetChartDataXRegion(auditData, dtRegion, FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                else if (radlDataType == "Hourkqy")
                {
                  XName = "Tstamp";
                  formatter = "%m/%d %H时";
                  auditData = m_HourData.GetVOCsKQYHourDataPagerNew(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //日数据
                else if (radlDataType == "Day")
                {
                  XName = "DateTime";
                  formatter = "%Y/%m/%d";
                  auditData = m_DayData.GetDayData(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //日数据区域
                else if (radlDataType == "DayCity")
                {
                  XName = "DateTime";
                  formatter = "%Y/%m/%d";
                  auditData = m_DayData.GetDayDataRegionPagers(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  auditData.RowFilter = "PointId='" + region + "'";
                  data = jsonData.GetChartDataRegion(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, region, name);
                }
                //日数据
                else if (radlDataType == "OriDay")
                {
                  XName = "DateTime";
                  formatter = "%Y/%m/%d";
                  if (FactorCode.Contains("a05024") || FactorCode.Contains("a05040") || FactorCode.Contains("a05041") || FactorCode.Contains("a04003") || FactorCode.Contains("a51039"))
                  {
                    auditData = m_DayOriData.GetDataPagersWithMax(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  }
                  else
                  {
                    auditData = m_DayOriData.GetDataPagers(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  }
                  data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //日数据区域
                else if (radlDataType == "OriDayCity")
                {
                  XName = "DateTime";
                  formatter = "%Y/%m/%d";
                  if (FactorCode.Contains("a05024") || FactorCode.Contains("a05040") || FactorCode.Contains("a05041") || FactorCode.Contains("a04003") || FactorCode.Contains("a51039"))
                  {
                    auditData = m_DayOriData.GetDataPagersWithMax(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  }
                  else
                  {
                    auditData = m_DayOriData.GetDataPagerForAllTimeRegion(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                    auditData.RowFilter="PointId='"+region+"'";
                  }
                  data = jsonData.GetChartDataRegion(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, region, name);
                }
                //月数据
                else if (radlDataType == "Month")
                {
                  string[] a = { };
                  int monthB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int monthF = Convert.ToInt32(timeStr.Split(';')[1]);
                  int monthE = Convert.ToInt32(timeStr.Split(';')[2]);
                  int monthT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_MonthData.GetMonthDataPager(PointID, FactorCode, monthB, monthF, monthE, monthT, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataMonthNT(auditData, FactorCode, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //月数据区域
                else if (radlDataType == "MonthCity")
                {
                  string[] a = { };
                  int monthB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int monthF = Convert.ToInt32(timeStr.Split(';')[1]);
                  int monthE = Convert.ToInt32(timeStr.Split(';')[2]);
                  int monthT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_MonthData.GetDataPagersRegion(PointID, FactorCode, monthB, monthF, monthE, monthT, PageSize, PageNo, out recordTotal);
                  auditData.RowFilter = "PointId='" + region + "'";
                  data = jsonData.GetChartDataMonthNTRegion(auditData, FactorCode, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, region, name);
                }
                //原始月数据
                else if (radlDataType == "OriMonth")
                {
                  //string[] a = { };
                  //int monthB = Convert.ToInt32(timeStr.Split(';')[0]);
                  //int monthF = Convert.ToInt32(timeStr.Split(';')[1]);
                  //int monthE = Convert.ToInt32(timeStr.Split(';')[2]);
                  //int monthT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_MonthOriData.GetOriDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                  data = jsonData.GetChartDataMonthNT(auditData, FactorCode, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //原始月数据区域
                else if (radlDataType == "OriMonthCity")
                {
                  //string[] a = { };
                  //int monthB = Convert.ToInt32(timeStr.Split(';')[0]);
                  //int monthF = Convert.ToInt32(timeStr.Split(';')[1]);
                  //int monthE = Convert.ToInt32(timeStr.Split(';')[2]);
                  //int monthT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_MonthOriData.GetOriDataPagerRegion(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                  auditData.RowFilter = "PointId='" + region + "'";
                  data = jsonData.GetChartDataMonthNTRegion(auditData, FactorCode, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null,region, name);
                }
                //季数据
                else if (radlDataType == "Season")
                {
                  int seasonB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int seasonF = Convert.ToInt32(timeStr.Split(';')[1]);
                  int seasonE = Convert.ToInt32(timeStr.Split(';')[2]);
                  int seasonT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_SeasonData.GetSeasonDataPager(PointID, FactorCode, seasonB, seasonF, seasonE, seasonT, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataMonth(auditData, FactorCode, ("Year;SeasonOfYear").Split(';'), ("年;季").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //季数据区域
                else if (radlDataType == "SeasonCity")
                {
                  int seasonB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int seasonF = Convert.ToInt32(timeStr.Split(';')[1]);
                  int seasonE = Convert.ToInt32(timeStr.Split(';')[2]);
                  int seasonT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_SeasonData.GetSeasonDataPagersRegion(PointID, FactorCode, seasonB, seasonF, seasonE, seasonT, PageSize, PageNo, out recordTotal);
                  auditData.RowFilter = "PointId='" + region + "'";
                  data = jsonData.GetChartDataMonthRegion(auditData, FactorCode, ("Year;SeasonOfYear").Split(';'), ("年;季").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, region, name);
                }
                //年数据
                else if (radlDataType == "Year")
                {
                  int yearB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int yearE = Convert.ToInt32(timeStr.Split(';')[1]);
                  auditData = m_YearData.GetYearDataPager(PointID, FactorCode, yearB, yearE, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataMonth(auditData, FactorCode, ("Year").Split(';'), ("年").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //年数据区域
                else if (radlDataType == "YearCity")
                {
                  int yearB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int yearE = Convert.ToInt32(timeStr.Split(';')[1]);
                  auditData = m_YearData.GetYearDataPagersRegion(PointID, FactorCode, yearB, yearE, PageSize, PageNo, out recordTotal);
                  auditData.RowFilter = "PointId='" + region + "'";
                  data = jsonData.GetChartDataMonthRegion(auditData, FactorCode, ("Year").Split(';'), ("年").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, region, name);
                }
                //周数据
                else if (radlDataType == "Week")
                {
                  int weekB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int weekF = Convert.ToInt32(timeStr.Split(';')[1]);
                  int weekE = Convert.ToInt32(timeStr.Split(';')[2]);
                  int weekT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_WeekData.GetWeekDataPager(PointID, FactorCode, weekB, weekF, weekE, weekT, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataMonth(auditData, FactorCode, ("Year;WeekOfYear").Split(';'), ("年;周").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999, name);
                }
                //周数据区域
                else if (radlDataType == "WeekCity")
                {
                  int weekB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int weekF = Convert.ToInt32(timeStr.Split(';')[1]);
                  int weekE = Convert.ToInt32(timeStr.Split(';')[2]);
                  int weekT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_WeekData.GetDataPagersRegion(PointID, FactorCode, weekB, weekF, weekE, weekT, PageSize, PageNo, out recordTotal);
                  auditData.RowFilter = "PointId='" + region + "'";
                  data = jsonData.GetChartDataMonthRegion(auditData, FactorCode, ("Year;WeekOfYear").Split(';'), ("年;周").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, region, name);
                }
            }
            else if (PointFactor == "factor")
            {
                //原始小时数据
                if (radlDataType == "Min60" && pageType!="Air1")
                {
                    XName = "Tstamp";
                    formatter = "%m/%d %H时";
                    auditData = g_IInfectantDALService.GetDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                    data = jsonData.GetChartDataX(auditData, FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                else if (radlDataType == "Min60" && pageType == "Air1")
                {
                  XName = "Tstamp";
                  formatter = "%m/%d %H时";
                  auditData = m_Min60Data.GetDataPagerAllTimeWithO8(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataX(auditData, FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                else if (radlDataType == "Min60City")
                {
                  XName = "Tstamp";
                  formatter = "%m/%d %H时";
                  auditData = m_Min60Data.GetDataPagerAllTimeWithO8Region(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  DataTable dtRegion = m_HourData.GetRegionWithPointId(PointID);
                  data = jsonData.GetChartDataXRegion(auditData, dtRegion, FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //原始五分钟数据
                else if (radlDataType == "Min5")
                {
                  XName = "Tstamp";
                  formatter = "%m/%d %H时%M分";
                  auditData = g_IInfectantDALService.GetDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataX(auditData, FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //原始一分钟数据
                else if (radlDataType == "Min1")
                {
                  XName = "Tstamp";
                  formatter = "%m/%d %H时%M分";
                  auditData = g_IInfectantDALService.GetDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataX(auditData, FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                else if (radlDataType == "Hour" && pageType == "Air1")
                {
                  XName = "Tstamp";
                  formatter = "%m/%d %H时";
                  auditData = m_HourData.GetNewHourDataPagerWidthO8(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  //auditData = m_HourData.GetNewHourDataPagerWidthRegionO8(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataX(auditData, FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //小时数据
                else if (radlDataType == "Hour" && pageType != "Air1")
                {
                  XName = "Tstamp";
                  formatter = "%m/%d %H时";
                  auditData = m_HourData.GetNewHourDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataX(auditData, FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //区域小时数据
                else if (radlDataType == "HourCity")
                {
                  XName = "Tstamp";
                  formatter = "%m/%d %H时";
                  auditData = m_HourData.GetNewHourDataPagerWidthRegionO8(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  DataTable dtRegion = m_HourData.GetRegionWithPointId(PointID);
                  data = jsonData.GetChartDataXRegion(auditData, dtRegion, FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //日数据
                else if (radlDataType == "Day")
                {
                  XName = "DateTime";
                  formatter = "%Y/%m/%d";
                  auditData = m_DayData.GetDayData(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataX(auditData, FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //区域日数据
                else if (radlDataType == "DayCity")
                {
                  XName = "DateTime";
                  formatter = "%Y/%m/%d";
                  auditData = m_DayData.GetDayDataRegionPagers(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  DataTable dtRegion = m_HourData.GetRegionWithPointId(PointID);
                  data = jsonData.GetChartDataXRegion(auditData, dtRegion, FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //原始日数据
                else if (radlDataType == "OriDay")
                {
                  XName = "DateTime";
                  formatter = "%Y/%m/%d";
                  auditData = m_DayOriData.GetDataPagers(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataX(auditData, FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //原始日数据区域
                else if (radlDataType == "OriDayCity")
                {
                  XName = "DateTime";
                  formatter = "%Y/%m/%d";
                  auditData = m_DayOriData.GetDataPagerForAllTimeRegion(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                  DataTable dtRegion = m_HourData.GetRegionWithPointId(PointID);
                 
                  data = jsonData.GetChartDataXRegion(auditData,dtRegion,FactorCode, PointID, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //月数据
                else if (radlDataType == "Month")
                {
                  string[] a = { };
                  int monthB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int monthF = Convert.ToInt32(timeStr.Split(';')[1]);
                  int monthE = Convert.ToInt32(timeStr.Split(';')[2]);
                  int monthT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_MonthData.GetMonthDataPager(PointID, FactorCode, monthB, monthF, monthE, monthT, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataMonthXNT(auditData, FactorCode, PointID, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //区域月数据
                else if (radlDataType == "MonthCity")
                {
                  string[] a = { };
                  int monthB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int monthF = Convert.ToInt32(timeStr.Split(';')[1]);
                  int monthE = Convert.ToInt32(timeStr.Split(';')[2]);
                  int monthT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_MonthData.GetDataPagersRegion(PointID, FactorCode, monthB, monthF, monthE, monthT, PageSize, PageNo, out recordTotal);
                  DataTable dtRegion = m_HourData.GetRegionWithPointId(PointID);
                  data = jsonData.GetChartDataMonthXNTRegion(auditData, FactorCode, dtRegion, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //原始月数据
                else if (radlDataType == "OriMonth")
                {
                  auditData = m_MonthOriData.GetOriDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                  data = jsonData.GetChartDataMonthXNT(auditData, FactorCode, PointID, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //原始月数据区域
                else if (radlDataType == "OriMonthCity")
                {
                  auditData = m_MonthOriData.GetOriDataPagerRegion(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal, "sortNumber desc,Year asc,MonthOfYear asc");
                  DataTable dtRegion = m_HourData.GetRegionWithPointId(PointID);
                  data = jsonData.GetChartDataMonthXNTRegion(auditData, FactorCode, dtRegion, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //季数据
                else if (radlDataType == "Season")
                {
                  int seasonB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int seasonF = Convert.ToInt32(timeStr.Split(';')[1]);
                  int seasonE = Convert.ToInt32(timeStr.Split(';')[2]);
                  int seasonT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_SeasonData.GetSeasonDataPager(PointID, FactorCode, seasonB, seasonF, seasonE, seasonT, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataMonthX(auditData, FactorCode, PointID, ("Year;SeasonOfYear").Split(';'), ("年;季").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //季数据
                else if (radlDataType == "SeasonCity")
                {
                  int seasonB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int seasonF = Convert.ToInt32(timeStr.Split(';')[1]);
                  int seasonE = Convert.ToInt32(timeStr.Split(';')[2]);
                  int seasonT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_SeasonData.GetSeasonDataPagersRegion(PointID, FactorCode, seasonB, seasonF, seasonE, seasonT, PageSize, PageNo, out recordTotal);
                  DataTable dtRegion = m_HourData.GetRegionWithPointId(PointID);
                  data = jsonData.GetChartDataMonthXRegion(auditData, FactorCode, dtRegion, ("Year;SeasonOfYear").Split(';'), ("年;季").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //年数据
                else if (radlDataType == "Year")
                {
                  int yearB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int yearE = Convert.ToInt32(timeStr.Split(';')[1]);
                  auditData = m_YearData.GetYearDataPager(PointID, FactorCode, yearB, yearE, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataMonthX(auditData, FactorCode, PointID, ("Year").Split(';'), ("年").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //年数据区域
                else if (radlDataType == "YearCity")
                {
                  int yearB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int yearE = Convert.ToInt32(timeStr.Split(';')[1]);
                  auditData = m_YearData.GetYearDataPagersRegion(PointID, FactorCode, yearB, yearE, PageSize, PageNo, out recordTotal);
                  DataTable dtRegion = m_HourData.GetRegionWithPointId(PointID);
                  data = jsonData.GetChartDataMonthXRegion(auditData, FactorCode, dtRegion, ("Year").Split(';'), ("年").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //周数据
                else if (radlDataType == "Week")
                {
                  int weekB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int weekF = Convert.ToInt32(timeStr.Split(';')[1]);
                  int weekE = Convert.ToInt32(timeStr.Split(';')[2]);
                  int weekT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_WeekData.GetWeekDataPager(PointID, FactorCode, weekB, weekF, weekE, weekT, PageSize, PageNo, out recordTotal);
                  data = jsonData.GetChartDataMonthX(auditData, FactorCode, PointID, ("Year;WeekOfYear").Split(';'), ("年;周").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //周数据区域
                else if (radlDataType == "WeekCity")
                {
                  int weekB = Convert.ToInt32(timeStr.Split(';')[0]);
                  int weekF = Convert.ToInt32(timeStr.Split(';')[1]);
                  int weekE = Convert.ToInt32(timeStr.Split(';')[2]);
                  int weekT = Convert.ToInt32(timeStr.Split(';')[3]);
                  auditData = m_WeekData.GetDataPagersRegion(PointID, FactorCode, weekB, weekF, weekE, weekT, PageSize, PageNo, out recordTotal);
                  DataTable dtRegion = m_HourData.GetRegionWithPointId(PointID);
                  data = jsonData.GetChartDataMonthXRegion(auditData, FactorCode, dtRegion, ("Year;WeekOfYear").Split(';'), ("年;周").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
            }
            return data;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}