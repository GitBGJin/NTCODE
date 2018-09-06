using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.OperatingMaintenance.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest
{
    /// <summary>
    /// QualityControlDataSearchNew 的摘要说明
    /// </summary>
    public class QualityControlDataSearchNew : IHttpHandler
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        QualityControlDataSearchService m_DataSearch = Singleton<QualityControlDataSearchService>.GetInstance();
        private ExcessiveSettingService excessiveService = new ExcessiveSettingService();
        private HighChartJsonData jsonData = new HighChartJsonData();
        public string ChartType = "spline";
        public string PointFactor = "";
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //string PointID = context.Request["PointID"] != null ? context.Request["PointID"] : "";
                string[] SN = context.Request["PointID"] != null ? context.Request["PointID"].ToString().Split(';') : null;
                string[] FactorCode = context.Request["FactorCode"] != null ? context.Request["FactorCode"].ToString().Split(';') : null;
                string[] pointIds = context.Request["DtBegin"] != null ? context.Request["DtBegin"].ToString().Split(';') : null;
                int tabStrip = context.Request["radlDataType"] != null ? Convert.ToInt32(context.Request["radlDataType"].ToString()) : 0;
                string name = context.Request["pageType"] != null ? context.Request["pageType"].ToString() : "";
                string timeStr = context.Request["DtEnd"] != null ? context.Request["DtEnd"].ToString() : "";
                string pageType = context.Request["PageSize"] != null ? context.Request["PageSize"].ToString() : "";
                int PageNo = context.Request["PageNo"] != null ? Convert.ToInt32(context.Request["PageNo"].ToString()) : 0;
                ChartType = context.Request["ChartType"] != null ? context.Request["ChartType"].ToString() : "spline";
                PointFactor = context.Request["PointFactor"] != null ? context.Request["PointFactor"].ToString() : "point";

                context.Response.ContentType = "text/plain";
                context.Response.Write(GetJsonData(SN, FactorCode, pointIds, timeStr, tabStrip, name, pageType));
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
        private string GetJsonData(string[] SN, string[] FactorCode, string[] pointIds, string timeStr, int tabStrip, string name, string pageType)
        {
            string data = "";
            string XName = "";
            var formatter = "";
            DataView auditData = new DataView();
            DateTime DtBegin = DateTime.Now;
            DateTime DtEnd = DateTime.Now;
            DateTime dtFrom = DateTime.Now;
            DateTime dtTo = DateTime.Now;
            IQueryable<ExcessiveSettingInfo> excessiveList = excessiveService.RetrieveListByFactor(ApplicationValue.Air, FactorCode, SN);

            DtBegin = Convert.ToDateTime(timeStr.Split(';')[0]);
            DtEnd = Convert.ToDateTime(timeStr.Split(';')[1]);
            XName = "时间";
            formatter = "formatter:formatterDayData";
            switch (tabStrip)
            {
                case 1:
                    auditData = m_DataSearch.GetPMSharpDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartDataSearch(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, -9999);
                    break;
                case 2:
                    auditData = m_DataSearch.GetPMTeomSharpDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartDataSearch(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, -9999);
                    break;
                case 3:
                    auditData = m_DataSearch.GetStdFlowMeterDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartDataSearch(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, -9999);
                    break;
                case 4:
                    auditData = m_DataSearch.GetO3HappenDevDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartDataSearch(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, -9999);
                    break;
                case 5:
                    auditData = m_DataSearch.GetNOxDevDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartDataSearch(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, -9999);
                    break;
                case 6:
                    auditData = m_DataSearch.GetCaliDevDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartDataSearch(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, -9999);
                    break;
                case 7:
                    auditData = m_DataSearch.GetCaliDevDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartDataSearch(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, -9999);
                    break;
                case 8:
                    auditData = m_DataSearch.GetZeroGasDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartDataSearch(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, -9999);
                    break;
                case 9:
                    auditData = m_DataSearch.GetAnaDevPrecisionDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartFactorSearch(auditData, FactorCode, XName, "响应浓度", formatter, ChartType, pageType, "", null, -9999);
                    break;
                case 10:
                    auditData = m_DataSearch.GetZeroAndSpanDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartFactorSearch(auditData, FactorCode, XName, "零气调节后", formatter, ChartType, pageType, "", null, -9999);
                    break;
                case 11:
                    auditData = m_DataSearch.GetAnaDevDriftDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartDataSearch(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, -9999);
                    break;
                case 12:
                    auditData = m_DataSearch.GetAnaDevDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartDataSearch(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, -9999);
                    break;
                case 13:
                    auditData = m_DataSearch.GetMultiPointDataNewPager(SN, pointIds, DtBegin, DtEnd, name);
                    data = jsonData.GetChartDataSearch(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, -9999);
                    break;
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