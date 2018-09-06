using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SmartEP.Service.AutoMonitoring.Air;

namespace SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest
{
    /// <summary>
    /// InstrumentParameterSearch 的摘要说明
    /// </summary>
    public class InstrumentParameterSearch : IHttpHandler
    {
        InstrumentDataBy60Service m_InstrumentDataBy60Service = new InstrumentDataBy60Service();
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
                string instrumentCode = context.Request["radlDataType"] != null ? context.Request["radlDataType"].ToString() : "";
                string pageType = context.Request["pageType"] != null ? context.Request["pageType"].ToString() : "";
                int PageSize = context.Request["PageSize"] != null ? Convert.ToInt32(context.Request["PageSize"].ToString()) : 100;
                int PageNo = context.Request["PageNo"] != null ? Convert.ToInt32(context.Request["PageNo"].ToString()) : 0;
                ChartType = context.Request["ChartType"] != null ? context.Request["ChartType"].ToString() : "spline";

                context.Response.ContentType = "text/plain";
                context.Response.Write(GetJsonData(PointID, FactorCode, DtBegin, DtEnd, instrumentCode, pageType, PageSize, PageNo));
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
        private string GetJsonData(string[] PointID, string[] FactorCode, DateTime DtBegin, DateTime DtEnd, string instrumentCode, string pageType, int PageSize, int PageNo)
        {
            string data = "";
            int recordTotal = 0;
            string fomatter = "formatter:formatterHourData";
            DataView samplingRateData = m_InstrumentDataBy60Service.GetDataPager(PointID, instrumentCode, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out recordTotal);
            data = jsonData.GetChartData(samplingRateData, FactorCode, "Tstamp", fomatter, ChartType, pageType, fomatter + "_XLabel", null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
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