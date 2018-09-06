using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Service.BaseData.BusinessRule;

namespace SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest
{
    /// <summary>
    /// 名称：PollutantAnalyze.ashx.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-09-29
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：实时小时数据【Chart】
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class PollutantAnalyze : IHttpHandler
    {
        private HourAQIService m_HourAQIService = Singleton<HourAQIService>.GetInstance();
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

            string[] FactorUnit = { "μg/m³", "mg/m³" };
            string[] FactorName = { "二氧化硫", "一氧化碳", "二氧化氮", "可吸入颗粒物", "臭氧1小时", "臭氧8小时", "细颗粒物" };
            if (!radlDataType.Equals(""))
            {
                int total = 0;
                string orderby = "";
                var analyzeDate = new DataView();
                IQueryable<ExcessiveSettingInfo> excessiveList = excessiveService.RetrieveListByFactor(ApplicationValue.Air, FactorCode, PointID);
                if (radlDataType == "Port")
                {
                    orderby = "DateTime,PointId";
                    analyzeDate = m_HourAQIService.GetPortDataPager(PointID, DtBegin, DtEnd, PageSize, PageNo, out total, orderby);
                    data = jsonData.GetChartData(analyzeDate, FactorCode, GetFactorName(FactorCode), FactorUnit, "DateTime", "formatter:formatterHourData", ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
                }
                else
                {
                    orderby = "DateTime,MonitoringRegionUid";
                    analyzeDate = m_HourAQIService.GetRegionDataPager(PointID, DtBegin, DtEnd, PageSize, PageNo, out total, orderby);
                    data = jsonData.GetChartData(analyzeDate, FactorCode, GetFactorName(FactorCode), FactorUnit, "DateTime", "formatter:formatterHourData", ChartType, pageType, FactorCode.Length == 1 ? excessiveList : null);
                }
            }
            return data;
        }

        private string[] GetFactorName(string[] FactorCode)
        {
            string[] facCodeBase = { "SO2", "CO", "NO2", "PM10", "O3", "Recent8HoursO3", "PM25" };
            string[] FactorNameBase = { "二氧化硫", "一氧化碳", "二氧化氮", "可吸入颗粒物", "臭氧1小时", "臭氧8小时", "细颗粒物" };
            List<string> FactorName = new List<string>();
            foreach (string fac in FactorCode)
            {
                FactorName.Add(FactorNameBase[Array.IndexOf(facCodeBase, fac)]);
            }
            return FactorName.ToArray();
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