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
    /// QualityControlDataSearch 的摘要说明
    /// </summary>
    public class QualityControlDataSearch : IHttpHandler
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
                string timeStr = context.Request["DtBegin"] != null ? context.Request["DtBegin"] : "";
                string name = context.Request["radlDataType"] != null ? context.Request["radlDataType"].ToString() : "";
                string pageType = context.Request["pageType"] != null ? context.Request["pageType"].ToString() : "";
                int tabStrip = context.Request["DtEnd"] != null ? Convert.ToInt32(context.Request["DtEnd"].ToString()) : 0;
                int PageSize = context.Request["PageSize"] != null ? Convert.ToInt32(context.Request["PageSize"].ToString()) : 100;
                int PageNo = context.Request["PageNo"] != null ? Convert.ToInt32(context.Request["PageNo"].ToString()) : 0;
                ChartType = context.Request["ChartType"] != null ? context.Request["ChartType"].ToString() : "spline";
                PointFactor = context.Request["PointFactor"] != null ? context.Request["PointFactor"].ToString() : "point";

                context.Response.ContentType = "text/plain";
                context.Response.Write(GetJsonData(SN, FactorCode, timeStr, tabStrip,name, pageType, PageSize, PageNo));
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
        private string GetJsonData(string[] SN, string[] FactorCode, string timeStr, int tabStrip,string name, string pageType, int PageSize, int PageNo)
        {
            string data = "";
            int recordTotal = 0;
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
            XName = "日期";
            formatter = "formatter:formatterDayData";
            switch (tabStrip)
            {
                case 1:
                    auditData = m_DataSearch.GetPMSharpDataPager(SN, DtBegin, DtEnd, name);
                    break;
                case 2:
                    auditData = m_DataSearch.GetPMTeomSharpDataPager(SN, DtBegin, DtEnd, name);
                    break;
                case 3:
                    auditData = m_DataSearch.GetStdFlowMeterDataPager(SN, DtBegin, DtEnd, name);
                    break;
                case 4:
                    auditData = m_DataSearch.GetO3HappenDevDataPager(SN, DtBegin, DtEnd, name);
                    break;
                case 5:
                    auditData = m_DataSearch.GetNOxDevDataPager(SN, DtBegin, DtEnd, name);
                    break;
                case 6:
                    auditData = m_DataSearch.GetCaliDevDataPager(SN, DtBegin, DtEnd, name);
                    break;
                case 7:
                    auditData = m_DataSearch.GetCaliDevDataPager(SN, DtBegin, DtEnd, name);
                    break;
                case 8:
                    auditData = m_DataSearch.GetZeroGasDataPager(SN, DtBegin, DtEnd, name);
                    break;
                case 9:
                    auditData = m_DataSearch.GetAnaDevPrecisionDataPager(SN, DtBegin, DtEnd, name);
                    break;
                case 10:
                    auditData = m_DataSearch.GetZeroAndSpanDataPager(SN, DtBegin, DtEnd, name);
                    break;
                case 11:
                    auditData = m_DataSearch.GetAnaDevDriftDataPager(SN, DtBegin, DtEnd, name);
                    break;
                case 12:
                    auditData = m_DataSearch.GetAnaDevDataPager(SN, DtBegin, DtEnd, name);
                    break;
                case 13:
                    auditData = m_DataSearch.GetMultiPointDataPager(SN, DtBegin, DtEnd, name);
                    break;
            }
            data = jsonData.GetChartDataSearch(auditData, FactorCode, XName, formatter, ChartType, pageType, "", null, -9999);
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