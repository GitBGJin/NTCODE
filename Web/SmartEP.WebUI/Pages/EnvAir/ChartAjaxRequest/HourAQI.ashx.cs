using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Utilities.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest
{
    /// <summary>
    /// 名称：HourAQI.ashx.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-11-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：小时AQI数据
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class HourAQI : IHttpHandler
    {
        HourAQIService g_HourAQIService = new HourAQIService();

        public void ProcessRequest(HttpContext context)
        {
            string action = Convert.ToString(context.Request["action"]);
            switch (action)
            {
                case "hours"://最新24小时AQI
                    GetAQIofHours(context);
                    break;
                default: break;
            }
        }

        #region 最新24小时AQI
        /// <summary>
        /// 最新24小时AQI
        /// </summary>
        /// <param name="context">json</param>
        public void GetAQIofHours(HttpContext context)
        {
            int Hours = Convert.ToInt32(context.Request["hours"]);
            string MonitoringRegionUid = Convert.ToString(context.Request["MonitoringRegionUid"]);
            string[] regionGuids = new string[] { MonitoringRegionUid };
            DateTime EndDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));
            DateTime StartDate = EndDate.AddHours(-Hours);
            //获取数据源
            DataTable dt = g_HourAQIService.GetRegionExportData(regionGuids, StartDate, EndDate).ToTable();
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