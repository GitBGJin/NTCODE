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
    /// OriginalScatter 的摘要说明
    /// </summary>
    public class OriginalScatter : IHttpHandler
    {
        DataCompare m_CompareData = Singleton<DataCompare>.GetInstance();
        private IInfectantDALService g_IInfectantDALService = null;
        InfectantBy60Service m_HourOriData = Singleton<InfectantBy60Service>.GetInstance();
        InfectantByDayService m_DayOriData = Singleton<InfectantByDayService>.GetInstance();
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

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string PointID = context.Request["PointID"] != null ? context.Request["PointID"].ToString() : null;
                string[] FactorCode = context.Request["FactorCode"] != null ? context.Request["FactorCode"].ToString().Split(';') : null;
                string radlDataType = context.Request["radlDataType"] != null ? context.Request["radlDataType"].ToString() : "";
                DateTime DtBegin = DateTime.Now;
                DateTime DtEnd = DateTime.Now;
                string timeStr = "";
                if (radlDataType == "Min1" || radlDataType == "Min5" || radlDataType == "Min60")
                {
                    g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType));
                }
                if (radlDataType.Equals("Hour") || radlDataType.Equals("Day") || radlDataType.Equals("Min1") || radlDataType.Equals("Min5") || radlDataType.Equals("Min60") || radlDataType.Equals("OriDay") || radlDataType.Equals("OriMonth"))
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
                context.Response.Write(GetJsonData(PointID, FactorCode, DtBegin, DtEnd, timeStr, radlDataType, pageType, PageSize, PageNo));
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
        private string GetJsonData(string Ports, string[] FactorCode, DateTime DtBegin, DateTime DtEnd, string timeStr, string radlDataType, string pageType, int PageSize, int PageNo)
        {
            string[] PointID = Ports.Split(';');
            string data = "";
            int recordTotal = 0;
            DataView auditData = new DataView();
            string XName = "Tstamp";
            string formatter = "";
            IQueryable<ExcessiveSettingInfo> excessiveList = excessiveService.RetrieveListByFactor(ApplicationValue.Air, FactorCode, PointID);

            //原始小时数据
            if (radlDataType == "Min60")
            {
                XName = "Tstamp";
                formatter = "%m/%d %H时";
                auditData = m_HourOriData.GetHourAvgData(PointID, FactorCode, DtBegin, DtEnd,radlDataType, PageSize, PageNo, out recordTotal);
                data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID.Length > 1 ? -9999 : Convert.ToInt32(PointID[0]));
            }
            //原始五分钟数据
            else if (radlDataType == "Min5")
            {
                XName = "Tstamp";
                formatter = "%m/%d %H时%M分";
                auditData = m_HourOriData.GetHourAvgData(PointID, FactorCode, DtBegin, DtEnd,radlDataType, PageSize, PageNo, out recordTotal);
                data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID.Length > 1 ? -9999 : Convert.ToInt32(PointID[0]));
            }
            //原始一分钟数据
            else if (radlDataType == "Min1")
            {
                XName = "Tstamp";
                formatter = "%m/%d %H时%M分";
                auditData = m_HourOriData.GetHourAvgData(PointID, FactorCode, DtBegin, DtEnd,radlDataType, PageSize, PageNo, out recordTotal);
                data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID.Length > 1 ? -9999 : Convert.ToInt32(PointID[0]));
            }
            //小时数据
            else if (radlDataType == "Hour")
            {
                XName = "Tstamp";
                formatter = "%m/%d %H时";
                auditData = m_HourData.GetHourDataAvg(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID.Length > 1 ? -9999 : Convert.ToInt32(PointID[0]));
            }
            //日数据
            else if (radlDataType == "Day")
            {
                XName = "DateTime";
                formatter = "%Y/%m/%d";
                auditData = m_DayData.GetDayDataAvg(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID.Length > 1 ? -9999 : Convert.ToInt32(PointID[0]));
            }
            //日数据
            else if (radlDataType == "OriDay")
            {
                XName = "DateTime";
                formatter = "%Y/%m/%d";
                auditData = m_DayOriData.GetDayAvgData(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID.Length > 1 ? -9999 : Convert.ToInt32(PointID[0]));
            }
            //月数据
            else if (radlDataType == "Month")
            {
                string[] a = { };
                int monthB = Convert.ToInt32(timeStr.Split(';')[0]);
                int monthF = Convert.ToInt32(timeStr.Split(';')[1]);
                int monthE = Convert.ToInt32(timeStr.Split(';')[2]);
                int monthT = Convert.ToInt32(timeStr.Split(';')[3]);
                auditData = m_MonthData.GetMonthDataAvg(PointID, FactorCode, monthB, monthF, monthE, monthT, PageSize, PageNo, out recordTotal);
                data = jsonData.GetChartDataMonthNT(auditData, FactorCode, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID.Length > 1 ? -9999 : Convert.ToInt32(PointID[0]));
            }
            //原始月数据
            else if (radlDataType == "OriMonth")
            {
                //string[] a = { };
                //int monthB = Convert.ToInt32(timeStr.Split(';')[0]);
                //int monthF = Convert.ToInt32(timeStr.Split(';')[1]);
                //int monthE = Convert.ToInt32(timeStr.Split(';')[2]);
                //int monthT = Convert.ToInt32(timeStr.Split(';')[3]);
                auditData = m_MonthOriData.GetOriDataAvg(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
                data = jsonData.GetChartDataMonth(auditData, FactorCode, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID.Length > 1 ? -9999 : Convert.ToInt32(PointID[0]));
            }
            //季数据
            else if (radlDataType == "Season")
            {
                int seasonB = Convert.ToInt32(timeStr.Split(';')[0]);
                int seasonF = Convert.ToInt32(timeStr.Split(';')[1]);
                int seasonE = Convert.ToInt32(timeStr.Split(';')[2]);
                int seasonT = Convert.ToInt32(timeStr.Split(';')[3]);
                auditData = m_SeasonData.GetSeasonDataAvg(PointID, FactorCode, seasonB, seasonF, seasonE, seasonT, PageSize, PageNo, out recordTotal);
                data = jsonData.GetChartDataMonthNT(auditData, FactorCode, ("Year;SeasonOfYear").Split(';'), ("年;季").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID.Length > 1 ? -9999 : Convert.ToInt32(PointID[0]));
            }
            //年数据
            else if (radlDataType == "Year")
            {
                int yearB = Convert.ToInt32(timeStr.Split(';')[0]);
                int yearE = Convert.ToInt32(timeStr.Split(';')[1]);
                auditData = m_YearData.GetYearDataAvg(PointID, FactorCode, yearB, yearE, PageSize, PageNo, out recordTotal);
                data = jsonData.GetChartDataMonth(auditData, FactorCode, ("Year").Split(';'), ("年").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID.Length > 1 ? -9999 : Convert.ToInt32(PointID[0]));
            }
            //周数据
            else if (radlDataType == "Week")
            {
                int weekB = Convert.ToInt32(timeStr.Split(';')[0]);
                int weekF = Convert.ToInt32(timeStr.Split(';')[1]);
                int weekE = Convert.ToInt32(timeStr.Split(';')[2]);
                int weekT = Convert.ToInt32(timeStr.Split(';')[3]);
                auditData = m_WeekData.GetWeekDataAvg(PointID, FactorCode, weekB, weekF, weekE, weekT, PageSize, PageNo, out recordTotal);
                data = jsonData.GetChartDataMonth(auditData, FactorCode, ("Year;WeekOfYear").Split(';'), ("年;周").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID.Length > 1 ? -9999 : Convert.ToInt32(PointID[0]));
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