using SmartEP.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.Calendar;
using SmartEP.Utilities.IO;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Chart
{
    public partial class GranuleChart : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 日数据接口
        /// </summary>
        GranuleSpecialService g_GranuleSpecial = new GranuleSpecialService();

        string weather = System.Configuration.ConfigurationManager.AppSettings["WeatherFactor"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowECharts(); ", true);
            }
        }
        ///// <summary>
        ///// 绑定数据
        ///// </summary>
        private void BindData()
        {
            string pointId = PageHelper.GetQueryString("pointId").TrimEnd(',');
            string[] factors = weather.TrimEnd(',').Split(',');
            string dtmBegion = PageHelper.GetQueryString("dtmBegion");
            string dtmEnd = PageHelper.GetQueryString("dtmEnd");
            string dataType = PageHelper.GetQueryString("dataType");

            DataTable dtOneMin = new DataTable();
            if (pointId != null && factors != null)
            {
                if (dataType == "OriDay")
                {
                    dtOneMin = g_GranuleSpecial.GetOriDayData(pointId, factors, Convert.ToDateTime(dtmBegion), Convert.ToDateTime(dtmEnd)).ToTable();
                }
                if (dataType == "OriMonth")
                {
                    dtOneMin = g_GranuleSpecial.GetOriMonthData(pointId, factors, Convert.ToDateTime(dtmBegion), Convert.ToDateTime(dtmEnd)).ToTable();
                }
                if (dataType == "Min1" || dataType == "Min5" || dataType == "Min60")
                {
                    dtOneMin = g_GranuleSpecial.GetOriHourData(pointId, factors, Convert.ToDateTime(dtmBegion), Convert.ToDateTime(dtmEnd), dataType).ToTable();
                }
                if (dataType == "Hour")
                {
                    dtOneMin = g_GranuleSpecial.GetAuditHourData(pointId, factors, Convert.ToDateTime(dtmBegion), Convert.ToDateTime(dtmEnd)).ToTable();

                }
                //日数据
                else if (dataType == "Day")
                {
                    dtOneMin = g_GranuleSpecial.GetAuditDayData(pointId, factors, Convert.ToDateTime(dtmBegion), Convert.ToDateTime(dtmEnd)).ToTable();
                }
                //月数据
                else if (dataType == "Month")
                {
                    dtOneMin = g_GranuleSpecial.GetAuditMonthData(pointId, factors, Convert.ToDateTime(dtmBegion), Convert.ToDateTime(dtmEnd)).ToTable();
                }

            }
            hdAirWeather.Value = JsonHelper.ToJson(dtOneMin);
        }
    }
}