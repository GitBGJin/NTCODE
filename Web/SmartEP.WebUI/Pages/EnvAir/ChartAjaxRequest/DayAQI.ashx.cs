using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Utilities.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest
{
    /// <summary>
    /// 名称：DayAQI.ashx.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-11-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：日AQI数据
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DayAQI : IHttpHandler
    {
        DayAQIService g_DayAQIService = new DayAQIService();

        public void ProcessRequest(HttpContext context)
        {
            string action = Convert.ToString(context.Request["action"]);
            switch (action)
            {
                case "Days"://最新7天AQI
                    GetAQIofDays(context);
                    break;
                default: break;
            }
        }

        #region 最新7天AQI
        /// <summary>
        /// 最新7天AQI
        /// </summary>
        /// <param name="context">json</param>
        public void GetAQIofDays(HttpContext context)
        {
            int Days = Convert.ToInt32(context.Request["days"]);
            string MonitoringRegionUid = Convert.ToString(context.Request["MonitoringRegionUid"]);
            string[] regionGuids = new string[] { MonitoringRegionUid };
            DateTime EndDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime StartDate = EndDate.AddDays(-Days);
            //获取数据源
            DataTable dt = g_DayAQIService.GetAreaExportData(regionGuids, StartDate, EndDate).ToTable();
            string jsonStr = JsonHelper.ToJson(dt);
            context.Response.ContentType = "text/plain";
            context.Response.Write(jsonStr);//返回json数据
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}