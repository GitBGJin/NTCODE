using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Utilities.IO;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Dock
{
    /// <summary>
    /// 名称：AirQualityLevel.aspx
    /// 创建人：刘长敏
    /// 创建日期：2015-09-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：首页元件 时空气质量等级分布
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AirQualityLevel : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 日数据接口
        /// </summary>
        DayAQIService g_DayAQIService = new DayAQIService();
        /// <summary>
        /// 无锡市的Uid
        /// </summary>
        string CityUid = SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Service.Core.Enums.CityType.SuZhou).Split(':')[1];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            DateTime date = DateTime.Now.AddDays(-1);
            dtpEnd.SelectedDate = date;
            dtpEnd.MaxDate = date;
            dtpBegin.SelectedDate = Convert.ToDateTime(date.Year.ToString() + "-01-01");
            BindData();
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowECharts(); ", true);
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            DateTime dtmBegion = dtpBegin.SelectedDate == null ? Convert.ToDateTime("1900-01-01") : dtpBegin.SelectedDate.Value;
            DateTime dtmEnd = dtpEnd.SelectedDate == null ? Convert.ToDateTime("1900-01-01") : dtpEnd.SelectedDate.Value;
            if (dtmBegion == Convert.ToDateTime("1900-01-01") || dtmEnd == Convert.ToDateTime("1900-01-01") || dtmBegion > dtmEnd)
            {
                Alert("请选择正确的查询日期");
                return;
            }
            //绑定数据
            IQueryable<RegionDayAQIReportEntity> ItemLists = g_DayAQIService.regionRetrieve(x => x.MonitoringRegionUid == CityUid && x.StatisticalType == "CG" && x.ReportDateTime >= dtmBegion && x.ReportDateTime <= dtmEnd).OrderByDescending(x => x.ReportDateTime);

            string All = "";
            int Alldays = ItemLists.Count();
            int days = 0;

            DataTable IAQI_dt = new DataTable();
            IAQI_dt.Columns.Add("Type", typeof(string));
            IAQI_dt.Columns.Add("level", typeof(string));
            IAQI_dt.Columns.Add("Value", typeof(string));

            DataRow IAQI_dr = IAQI_dt.NewRow();
            int thisdays = ItemLists.Where(x => x.Grade == "一级").Count();
            days += thisdays;
            IAQI_dr["Type"] = "优";
            IAQI_dr["level"] = "一级";
            IAQI_dr["Value"] = thisdays.ToString();
            IAQI_dt.Rows.Add(IAQI_dr);
            if (thisdays > 0)
            {
                All += "一级" + thisdays.ToString() + "天；";
            }

            IAQI_dr = IAQI_dt.NewRow();
            thisdays = ItemLists.Where(x => x.Grade == "二级").Count();
            days += thisdays;
            IAQI_dr["Type"] = "良";
            IAQI_dr["level"] = "二级";
            IAQI_dr["Value"] = thisdays.ToString();
            IAQI_dt.Rows.Add(IAQI_dr);
            if (thisdays > 0)
            {
                All += "二级" + thisdays.ToString() + "天；";
            }

            IAQI_dr = IAQI_dt.NewRow();
            thisdays = ItemLists.Where(x => x.Grade == "三级").Count();
            IAQI_dr["Type"] = "轻度污染";
            IAQI_dr["level"] = "三级";
            IAQI_dr["Value"] = thisdays.ToString();
            IAQI_dt.Rows.Add(IAQI_dr);
            if (thisdays > 0)
            {
                All += "三级" + thisdays.ToString() + "天；";
            }

            IAQI_dr = IAQI_dt.NewRow();
            thisdays = ItemLists.Where(x => x.Grade == "四级").Count();
            IAQI_dr["Type"] = "中度污染";
            IAQI_dr["level"] = "四级";
            IAQI_dr["Value"] = thisdays.ToString();
            IAQI_dt.Rows.Add(IAQI_dr);
            if (thisdays > 0)
            {
                All += "四级" + thisdays.ToString() + "天；";
            }

            IAQI_dr = IAQI_dt.NewRow();
            thisdays = ItemLists.Where(x => x.Grade == "五级").Count();
            IAQI_dr["Type"] = "重度污染";
            IAQI_dr["level"] = "五级";
            IAQI_dr["Value"] = thisdays.ToString();
            IAQI_dt.Rows.Add(IAQI_dr);
            if (thisdays > 0)
            {
                All += "五级" + thisdays.ToString() + "天；";
            }

            IAQI_dr = IAQI_dt.NewRow();
            thisdays = ItemLists.Where(x => x.Grade == "六级").Count();
            IAQI_dr["Type"] = "严重污染";
            IAQI_dr["level"] = "六级";
            IAQI_dr["Value"] = thisdays.ToString();
            IAQI_dt.Rows.Add(IAQI_dr);
            if (thisdays > 0)
            {
                All += "六级" + thisdays.ToString() + "天；";
            }
            double exl = (Convert.ToDouble(days) / Convert.ToDouble(Alldays)) * 100;
            total.InnerText = dtmBegion.ToString("yyyy年MM月dd日") + "至" + dtmEnd.ToString("yyyy年MM月dd日") + ",市区空气质量：" + All + "优良天数累计" + days.ToString() + "天," + "占" + exl.ToString("0.00") + "%";
            hdAirQuality.Value = JsonHelper.ToJson(IAQI_dt);//有效数据
            hdregionUid.Value = CityUid;
            hdDateBegin.Value = Convert.ToDateTime(dtpBegin.SelectedDate.Value).ToString("yyyy-MM-dd");
            hdDateEnd.Value = Convert.ToDateTime(dtpEnd.SelectedDate.Value).ToString("yyyy-MM-dd");
        }
    }
}