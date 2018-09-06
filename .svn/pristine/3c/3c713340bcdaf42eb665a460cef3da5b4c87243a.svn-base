using SmartEP.Core.Enums;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.AutoMonitoring.Interfaces;
using SmartEP.Service.BaseData.BusinessRule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest
{
    /// <summary>
    /// _24HSinglePollutantDataAnalyzeNew 的摘要说明
    /// </summary>
    public class _24HSinglePollutantDataAnalyzeNew : IHttpHandler
    {
        private IInfectantDALService g_IInfectantDALService = null;
        private ExcessiveSettingService excessiveService = new ExcessiveSettingService();
        private HighChartJsonData jsonData = new HighChartJsonData();
        public string ChartType = "spline";
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string[] PointID = context.Request["PointID"] != null ? context.Request["PointID"].ToString().Split(';') : null;
                string[] FactorCode = context.Request["FactorCode"] != null ? context.Request["FactorCode"].ToString().Split(';') : null;
                DateTime DtBegin = context.Request["DtBegin"] != null ? Convert.ToDateTime(context.Request["DtBegin"]) : DateTime.Now;
                DateTime DtEnd = context.Request["DtEnd"] != null ? Convert.ToDateTime(context.Request["DtEnd"]) : DateTime.Now;
                string radlDataType = context.Request["radlDataType"] != null ? context.Request["radlDataType"].ToString() : "";
                string pageType = context.Request["pageType"] != null ? context.Request["pageType"].ToString() : "";
                int PageSize = context.Request["PageSize"] != null ? Convert.ToInt32(context.Request["PageSize"].ToString()) : 100;
                int PageNo = context.Request["PageNo"] != null ? Convert.ToInt32(context.Request["PageNo"].ToString()) : 0;
                ChartType = context.Request["ChartType"] != null ? context.Request["ChartType"].ToString() : "spline";

                context.Response.ContentType = "text/plain";
                context.Response.Write(GetJsonData(PointID, FactorCode, DtBegin, DtEnd, radlDataType, pageType, PageSize, PageNo));
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
        private string GetJsonData(string[] PointID, string[] FactorCode, DateTime DtBegin, DateTime DtEnd, string radlDataType, string pageType, int PageSize, int PageNo)
        {
            string data = "";
            if (!radlDataType.Equals(""))
            {
                int total = 0;
                string fomatter = "formatter:formatterHourData";
                IQueryable<ExcessiveSettingInfo> excessiveList = excessiveService.RetrieveListByFactor(ApplicationValue.Air, FactorCode, PointID);
                if (radlDataType.Equals("Min5") || radlDataType.Equals("Min1") || radlDataType.Equals("RealTime")) fomatter = "formatter:formatterMinuteData";
                g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType));
                DataView dv = g_IInfectantDALService.GetDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out total, "Tstamp");
                data = jsonData.GetChartDataSample(dv, FactorCode, PointID, "Tstamp", fomatter, ChartType, pageType, fomatter + "_XLabel", FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
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