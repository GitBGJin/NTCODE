using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using System.Collections;

namespace SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest
{
    /// <summary>
    /// 名称：DifferentDayCompare.ashx.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-09-29
    /// 修改人：刘长敏
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：实时小时数据【Chart】
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DifferentDayCompare : IHttpHandler
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DataCompare m_CompareData = Singleton<DataCompare>.GetInstance();
        private ExcessiveSettingService excessiveService = new ExcessiveSettingService();
        private HighChartJsonData jsonData = new HighChartJsonData();
        public string ChartType = "spline";
        public string PointFactor = "";
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string[] PointID = { };
                if (context.Request["PointID"].ToString().Contains("~"))
                {
                    PointID = context.Request["PointID"] != null ? context.Request["PointID"].ToString().Split('~') : null;
                }
                else
                {
                    PointID = context.Request["PointID"] != null ? context.Request["PointID"].ToString().Split(';') : null;
                }
                string timeBe1 = context.Request["timeBe1"] != null ? context.Request["timeBe1"] : "";
                string timeBe2 = context.Request["timeBe2"] != null ? context.Request["timeBe2"] : "";
                string[] FactorCode = context.Request["FactorCode"] != null ? context.Request["FactorCode"].ToString().Split(';') : null;
                string radlDataType = context.Request["radlDataType"] != null ? context.Request["radlDataType"].ToString() : "";
                string timeStr = context.Request["DtBegin"] != null ? context.Request["DtBegin"] : "";
                string pageType = context.Request["pageType"] != null ? context.Request["pageType"].ToString() : "";
                int tabStrip = context.Request["tabStrip"] != null ? Convert.ToInt32(context.Request["tabStrip"].ToString()) : 0;
                int PageSize = context.Request["PageSize"] != null ? Convert.ToInt32(context.Request["PageSize"].ToString()) : 100;
                int PageNo = context.Request["PageNo"] != null ? Convert.ToInt32(context.Request["PageNo"].ToString()) : 0;
                ChartType = context.Request["ChartType"] != null ? context.Request["ChartType"].ToString() : "spline";
                PointFactor = context.Request["PointFactor"] != null ? context.Request["PointFactor"].ToString() : "point";

                context.Response.ContentType = "text/plain";
                context.Response.Write(GetJsonData(PointID, FactorCode, timeStr, radlDataType, pageType, tabStrip, PageSize, PageNo,timeBe1,timeBe2));
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
        private string GetJsonData(string[] PointID, string[] FactorCode, string timeStr, string radlDataType, string pageType, int tabStrip, int PageSize, int PageNo, string timeBe1, string timeBe2)
        {
            string data = "";
            int recordTotal = 0;
            string XName = "Tstamp";
            var formatter = "";
            DataView auditData = new DataView();
            DateTime DtBegin = DateTime.Now;
            DateTime DtEnd = DateTime.Now;
            DateTime dtFrom = DateTime.Now;
            DateTime dtTo = DateTime.Now;
            IQueryable<ExcessiveSettingInfo> excessiveList = excessiveService.RetrieveListByFactor(ApplicationValue.Air, FactorCode, PointID);
            if (tabStrip == 0)
            {
                int tabIndex = tabStrip;
                #region 原始审核
                if (radlDataType == "Hour")
                {
                    DtBegin = Convert.ToDateTime(timeStr.Split(';')[0]);
                    DtEnd = Convert.ToDateTime(timeStr.Split(';')[1]);
                    DateTime dtBeginD = new DateTime();
                    DateTime dtEndD = new DateTime();
                    string type = "'审核','原始'";
                    formatter = "formatter:formatterHourData";
                    auditData = m_CompareData.GetHourOtherCompareNew(PointID, FactorCode, DtBegin, DtEnd, dtBeginD, dtEndD, type, PageSize, PageNo, out recordTotal, tabIndex);
                    data = jsonData.GetChartDataHour(auditData, FactorCode, XName, formatter, ChartType, pageType, "", FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                #endregion
            }
            else if (tabStrip == 1)
            {
                int tabIndex = tabStrip;
                #region 相同时间段单测点
                if (PointFactor == "point")
                {
                    //小时数据
                    if (radlDataType == "Hour")
                    {
                        DtBegin = Convert.ToDateTime(timeStr.Split(';')[0]);
                        DtEnd = Convert.ToDateTime(timeStr.Split(';')[1]);
                        formatter = "formatter:formatterHourData";

                        auditData = m_CompareData.GetHourCompareView(PointID, FactorCode, new DateTime[,] { { DtBegin, DtEnd } }, PageSize, PageNo, out recordTotal, tabIndex);
                        data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                    }
                    //日数据
                    else if (radlDataType == "Day")
                    {
                        DtBegin = Convert.ToDateTime(timeStr.Split(';')[0]);
                        DtEnd = Convert.ToDateTime(timeStr.Split(';')[1]);
                        XName = "DateTime";
                        formatter = "formatter:formatterDayData";
                        auditData = m_CompareData.GetDayCompareView(PointID, FactorCode, new DateTime[,] { { DtBegin, DtEnd } }, PageSize, PageNo, out recordTotal, tabIndex);
                        data = jsonData.GetChartData(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                    }
                    //月数据
                    else if (radlDataType == "Month")
                    {
                        int monthB = Convert.ToInt32(timeStr.Split(';')[0]);
                        int monthF = Convert.ToInt32(timeStr.Split(';')[1]);
                        int monthE = Convert.ToInt32(timeStr.Split(';')[2]);
                        int monthT = Convert.ToInt32(timeStr.Split(';')[3]);
                        auditData = m_CompareData.GetMonthCompareView(PointID, FactorCode, new int[,] { { monthB, monthF, monthE, monthT } }, PageSize, PageNo, out recordTotal, tabIndex);
                        data = jsonData.GetChartDataMonth(auditData, FactorCode, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                    }
                    //季数据
                    else if (radlDataType == "Season")
                    {
                        int seasonB = Convert.ToInt32(timeStr.Split(';')[0]);
                        int seasonF = Convert.ToInt32(timeStr.Split(';')[1]);
                        int seasonE = Convert.ToInt32(timeStr.Split(';')[2]);
                        int seasonT = Convert.ToInt32(timeStr.Split(';')[3]);
                        auditData = m_CompareData.GetSeasonCompareView(PointID, FactorCode, new int[,] { { seasonB, seasonF, seasonE, seasonT } }, PageSize, PageNo, out recordTotal, tabIndex);
                        data = jsonData.GetChartDataMonth(auditData, FactorCode, ("Year;SeasonOfYear").Split(';'), ("年;季").Split(';'), formatter, ChartType, pageType, null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                    }
                    //年数据
                    else if (radlDataType == "Year")
                    {
                        int yearB = Convert.ToInt32(timeStr.Split(';')[0]);
                        int yearE = Convert.ToInt32(timeStr.Split(';')[1]);
                        auditData = m_CompareData.GetYearCompareView(PointID, FactorCode, new int[,] { { yearB, yearE } }, PageSize, PageNo, out recordTotal, tabIndex);
                        data = jsonData.GetChartDataMonth(auditData, FactorCode, ("Year").Split(';'), ("年").Split(';'), formatter, ChartType, pageType, null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                    }
                    //周数据
                    else if (radlDataType == "Week")
                    {
                        int weekB = Convert.ToInt32(timeStr.Split(';')[0]);
                        int weekF = Convert.ToInt32(timeStr.Split(';')[1]);
                        int weekE = Convert.ToInt32(timeStr.Split(';')[2]);
                        int weekT = Convert.ToInt32(timeStr.Split(';')[3]);
                        auditData = m_CompareData.GetWeekCompareView(PointID, FactorCode, new int[,] { { weekB, weekF, weekE, weekT } }, PageSize, PageNo, out recordTotal, tabIndex);
                        data = jsonData.GetChartDataMonth(auditData, FactorCode, ("Year;WeekOfYear").Split(';'), ("年;周").Split(';'), formatter, ChartType, pageType, null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                    }
                #endregion
                }
                #region 相同时间段单因子
                else if (PointFactor == "factor")
                {
                    //小时数据
                    if (radlDataType == "Hour")
                    {
                        DtBegin = Convert.ToDateTime(timeStr.Split(';')[0]);
                        DtEnd = Convert.ToDateTime(timeStr.Split(';')[1]);
                        formatter = "formatter:formatterHourData";

                        auditData = m_CompareData.GetHourCompareView(PointID, FactorCode, new DateTime[,] { { DtBegin, DtEnd } }, PageSize, PageNo, out recordTotal, tabIndex);
                        data = jsonData.GetChartDataX(auditData, FactorCode, PointID, XName, formatter, ChartType, pageType, "", FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                    }
                    //日数据
                    else if (radlDataType == "Day")
                    {
                        DtBegin = Convert.ToDateTime(timeStr.Split(';')[0]);
                        DtEnd = Convert.ToDateTime(timeStr.Split(';')[1]);
                        XName = "DateTime";
                        formatter = "formatter:formatterDayData";
                        auditData = m_CompareData.GetDayCompareView(PointID, FactorCode, new DateTime[,] { { DtBegin, DtEnd } }, PageSize, PageNo, out recordTotal, tabIndex);
                        data = jsonData.GetChartDataX(auditData, FactorCode, PointID, XName, formatter, ChartType, pageType, "", FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                    }
                    //月数据
                    else if (radlDataType == "Month")
                    {
                        int monthB = Convert.ToInt32(timeStr.Split(';')[0]);
                        int monthF = Convert.ToInt32(timeStr.Split(';')[1]);
                        int monthE = Convert.ToInt32(timeStr.Split(';')[2]);
                        int monthT = Convert.ToInt32(timeStr.Split(';')[3]);
                        auditData = m_CompareData.GetMonthCompareView(PointID, FactorCode, new int[,] { { monthB, monthF, monthE, monthT } }, PageSize, PageNo, out recordTotal, tabIndex);
                        auditData.Sort = "Year ASC,MonthOfYear asc";
                        data = jsonData.GetChartDataMonthX(auditData, FactorCode, PointID, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                    }
                    //季数据
                    else if (radlDataType == "Season")
                    {
                        int seasonB = Convert.ToInt32(timeStr.Split(';')[0]);
                        int seasonF = Convert.ToInt32(timeStr.Split(';')[1]);
                        int seasonE = Convert.ToInt32(timeStr.Split(';')[2]);
                        int seasonT = Convert.ToInt32(timeStr.Split(';')[3]);
                        auditData = m_CompareData.GetSeasonCompareView(PointID, FactorCode, new int[,] { { seasonB, seasonF, seasonE, seasonT } }, PageSize, PageNo, out recordTotal, tabIndex);
                        auditData.Sort = "Year ASC,SeasonOfYear asc";
                        data = jsonData.GetChartDataMonthX(auditData, FactorCode, PointID, ("Year;SeasonOfYear").Split(';'), ("年;季").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                    }
                    //年数据
                    else if (radlDataType == "Year")
                    {
                        int yearB = Convert.ToInt32(timeStr.Split(';')[0]);
                        int yearE = Convert.ToInt32(timeStr.Split(';')[1]);
                        auditData = m_CompareData.GetYearCompareView(PointID, FactorCode, new int[,] { { yearB, yearE } }, PageSize, PageNo, out recordTotal, tabIndex);
                        auditData.Sort = "Year ASC";
                        data = jsonData.GetChartDataMonthX(auditData, FactorCode, PointID, ("Year").Split(';'), ("年").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                    }
                    //周数据
                    else if (radlDataType == "Week")
                    {
                        int weekB = Convert.ToInt32(timeStr.Split(';')[0]);
                        int weekF = Convert.ToInt32(timeStr.Split(';')[1]);
                        int weekE = Convert.ToInt32(timeStr.Split(';')[2]);
                        int weekT = Convert.ToInt32(timeStr.Split(';')[3]);
                        auditData = m_CompareData.GetWeekCompareView(PointID, FactorCode, new int[,] { { weekB, weekF, weekE, weekT } }, PageSize, PageNo, out recordTotal, tabIndex);
                        //auditData.Sort = "Year ASC,WeekOfYear asc";
                        data = jsonData.GetChartDataMonthX(auditData, FactorCode, PointID, ("Year;WeekOfYear").Split(';'), ("年;周").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                    }
                #endregion
                }
            }
            else if (tabStrip == 2)
            {
                int tabIndex = tabStrip;
                #region 不同时间段
                //小时数据
                if (radlDataType == "Hour")
                {
                    formatter = "%m/%d %H时";
                    DtBegin = DateTime.TryParse(timeBe1.Split('~')[0], out DtBegin) ? DtBegin : DateTime.Now;
                    DtEnd = DateTime.TryParse(timeBe1.Split('~')[1], out DtEnd) ? DtEnd : DateTime.Now;
                    dtFrom = DateTime.TryParse(timeBe2.Split('~')[0], out dtFrom) ? dtFrom : DateTime.Now;
                    dtTo = DateTime.TryParse(timeBe2.Split('~')[1], out dtTo) ? dtTo : DateTime.Now;
                    auditData = m_CompareData.GetHourOtherCompare(PointID, FactorCode, DtBegin, DtEnd, dtFrom, dtTo, "'审核'", 99999, 0, out recordTotal, tabIndex, "PointId asc,Tstamp desc");
                    data = jsonData.GetChartDataCom(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //日数据
                else if (radlDataType == "Day")
                {
                    XName = "DateTime";
                    formatter = "%Y/%m/%d";
                    DtBegin = DateTime.TryParse(timeBe1.Split('~')[0], out DtBegin) ? DtBegin : DateTime.Now;
                    DtEnd = DateTime.TryParse(timeBe1.Split('~')[1], out DtEnd) ? DtEnd : DateTime.Now;
                    dtFrom = DateTime.TryParse(timeBe2.Split('~')[0], out dtFrom) ? dtFrom : DateTime.Now;
                    dtTo = DateTime.TryParse(timeBe2.Split('~')[1], out dtTo) ? dtTo : DateTime.Now;
                    auditData = m_CompareData.GetDayCompare(PointID, FactorCode, DtBegin, DtEnd, dtFrom, dtTo, 99999, 0, out recordTotal, tabIndex, "PointId asc,DateTime desc");
                    //auditData = m_CompareData.GetDayCompareView(PointIdJ, FactorCode, new DateTime[,] { { DtBegin, DtEnd } }, PageSize, PageNo, out recordTotal, tabIndex);
                    data = jsonData.GetChartDataCom(auditData, FactorCode, XName, formatter, ChartType, pageType, formatter, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //月数据
                else if (radlDataType == "Month")
                {
                    string[] PointIdJ = { PointID[0] };
                    int monthB = Int32.TryParse(timeBe1.Split('~')[0], out monthB) ? monthB : 0;
                    int monthF = Int32.TryParse(timeBe1.Split('~')[1], out monthF) ? monthF : 0;
                    int monthE = Int32.TryParse(timeBe1.Split('~')[2], out monthE) ? monthE : 0;
                    int monthT = Int32.TryParse(timeBe1.Split('~')[3], out monthT) ? monthT : 0;
                    int dtmonthB = Int32.TryParse(timeBe2.Split('~')[0], out dtmonthB) ? dtmonthB : 0;
                    int dtmonthF = Int32.TryParse(timeBe2.Split('~')[1], out dtmonthF) ? dtmonthF : 0;
                    int dtmonthE = Int32.TryParse(timeBe2.Split('~')[2], out dtmonthE) ? dtmonthE : 0;
                    int dtmonthT = Int32.TryParse(timeBe2.Split('~')[3], out dtmonthT) ? dtmonthT : 0;
                    auditData = m_CompareData.GetMonthCompare(PointID, FactorCode, monthB, monthF, monthE, monthT, dtmonthB, dtmonthF, dtmonthE, dtmonthT, 9999, 0, out recordTotal, tabIndex, "PointId asc,Year desc,MonthOfYear desc");
                    //auditData = m_CompareData.GetMonthCompareView(PointIdJ, FactorCode, new int[,] { { monthB, monthF, monthE, monthT } }, PageSize, PageNo, out recordTotal, tabIndex);
                    data = jsonData.GetChartDataMonthNTCom(auditData, FactorCode, ("Year;MonthOfYear").Split(';'), ("年;月").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //季数据
                else if (radlDataType == "Season")
                {
                    string[] PointIdJ = { PointID[0] };
                    int seasonB = Int32.TryParse(timeBe1.Split('~')[0], out seasonB) ? seasonB : 0;
                    int seasonF = Int32.TryParse(timeBe1.Split('~')[1], out seasonF) ? seasonF : 0;
                    int seasonE = Int32.TryParse(timeBe1.Split('~')[2], out seasonE) ? seasonE : 0;
                    int seasonT = Int32.TryParse(timeBe1.Split('~')[3], out seasonT) ? seasonT : 0;
                    int dtseasonB = Int32.TryParse(timeBe2.Split('~')[0], out dtseasonB) ? dtseasonB : 0;
                    int dtseasonF = Int32.TryParse(timeBe2.Split('~')[1], out dtseasonF) ? dtseasonF : 0;
                    int dtseasonE = Int32.TryParse(timeBe2.Split('~')[2], out dtseasonE) ? dtseasonE : 0;
                    int dtseasonT = Int32.TryParse(timeBe2.Split('~')[3], out dtseasonT) ? dtseasonT : 0;
                    auditData = m_CompareData.GetSeasonCompare(PointID, FactorCode, seasonB, seasonF, seasonE, seasonT, dtseasonB, dtseasonF, dtseasonE, dtseasonT, 9999, 0, out recordTotal, tabIndex, "PointId asc,Year desc,SeasonOfYear desc");
                    //auditData = m_CompareData.GetSeasonCompareView(PointIdJ, FactorCode, new int[,] { { seasonB, seasonF, seasonE, seasonT } }, PageSize, PageNo, out recordTotal, tabIndex);
                    data = jsonData.GetChartDataMonthNTCom(auditData, FactorCode, ("Year;SeasonOfYear").Split(';'), ("年;季").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //年数据
                else if (radlDataType == "Year")
                {
                    string[] PointIdJ = { PointID[0] };
                    int yearB = Int32.TryParse(timeBe1.Split('~')[0], out yearB) ? yearB : 0;
                    int yearE = Int32.TryParse(timeBe1.Split('~')[1], out yearE) ? yearE : 0;
                    int dtyearB = Int32.TryParse(timeBe2.Split('~')[0], out dtyearB) ? dtyearB : 0;
                    int dtyearE = Int32.TryParse(timeBe2.Split('~')[1], out dtyearE) ? dtyearE : 0;
                    auditData = m_CompareData.GetYearCompare(PointID, FactorCode, yearB, yearE, dtyearB, dtyearE, 99999, 0, out recordTotal, tabIndex, "PointId asc,Year desc");
                    //auditData = m_CompareData.GetYearCompareView(PointIdJ, FactorCode, new int[,] { { yearB, yearE } }, PageSize, PageNo, out recordTotal, tabIndex);
                    data = jsonData.GetChartDataMonthNTCom(auditData, FactorCode, ("Year").Split(';'), ("年").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                //周数据
                else if (radlDataType == "Week")
                {
                    string[] PointIdJ = { PointID[0] };
                    int weekB = Int32.TryParse(timeBe1.Split('~')[0], out weekB) ? weekB : 0;
                    int weekF = Int32.TryParse(timeBe1.Split('~')[1], out weekF) ? weekF : 0;
                    int weekE = Int32.TryParse(timeBe1.Split('~')[2], out weekE) ? weekE : 0;
                    int weekT = Int32.TryParse(timeBe1.Split('~')[3], out weekT) ? weekT : 0;
                    int dtweekB = Int32.TryParse(timeBe2.Split('~')[0], out dtweekB) ? dtweekB : 0;
                    int dtweekF = Int32.TryParse(timeBe2.Split('~')[1], out dtweekF) ? dtweekF : 0;
                    int dtweekE = Int32.TryParse(timeBe2.Split('~')[2], out dtweekE) ? dtweekE : 0;
                    int dtweekT = Int32.TryParse(timeBe2.Split('~')[3], out dtweekT) ? dtweekT : 0;
                    auditData = m_CompareData.GetWeekCompare(PointID, FactorCode, weekB, weekF, weekE, weekT, dtweekB, dtweekF, dtweekE, dtweekT, 99999, 0, out recordTotal, tabIndex, "PointId asc,Year desc,WeekOfYear desc");
                    //auditData = m_CompareData.GetWeekCompareView(PointIdJ, FactorCode, new int[,] { { weekB, weekF, weekE, weekT } }, PageSize, PageNo, out recordTotal, tabIndex);
                    data = jsonData.GetChartDataMonthNTCom(auditData, FactorCode, ("Year;WeekOfYear").Split(';'), ("年;周").Split(';'), formatter, ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                #endregion
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