using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class PrimaryPollutantAnalyze : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 气系统标识
        /// </summary>
        private string AirUid = SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Core.Enums.ApplicationType.Air);
        /// <summary>
        /// 站点日AQI数据接口
        /// </summary>
        DayAQIService g_DayAQIService = new DayAQIService();
        /// <summary>
        /// 授权测点接口
        /// </summary>
        PersonalizedSetService g_PersonalizedSetService = new PersonalizedSetService();
        /// <summary>
        /// 气系统测点接口
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAirService = new MonitoringPointAirService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        #region 初始化控件
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //初始化授权测点
            IList<MonitoringPointEntity> PointList = GetPointList();
            ddlPoint.DataTextField = "MonitoringPointName";
            ddlPoint.DataValueField = "PointId";
            ddlPoint.DataSource = PointList;
            ddlPoint.DataBind();

            //初始化周数据
            RadMonthYearPicker1.SelectedDate = DateTime.Now;
            BindWeekList();

            //初始化年数据
            BindYearList();

            //初始化日期
            txtStartDate.SelectedDate = DateTime.Now.AddDays(-1);
            txtEndDate.SelectedDate = DateTime.Now;

            //初始化控件显示
            IniDatePicker();

            //初始化HightCharts.series
            string returnValue = InitTable();
            RadScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "<script>GetHighchartsPie(escape('" + returnValue + "'));</script>", false);
        }
        #endregion

        #region 获取授权测点数据
        public IList<MonitoringPointEntity> GetPointList()
        {
            DataTable dt = g_PersonalizedSetService.GetPersonalizedSetByUserGuid(SessionHelper.Get("UserGuid"), AirUid);
            List<string> PointUids = new List<string>() { };
            foreach (DataRow dr in dt.Rows)
            {
                PointUids.Add(Convert.ToString(dr["ParameterUid"]));
            }
            IQueryable<MonitoringPointEntity> AirPointList = g_MonitoringPointAirService.RetrieveAirMPList();
            return AirPointList.Where(x => PointUids.Contains(x.MonitoringPointUid)).OrderByDescending(x => x.OrderByNum).ToList<MonitoringPointEntity>();
        }
        #endregion

        #region 选择类型变更事件
        /// <summary>
        /// 选择类型变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTypeList.SelectedValue == "0")//选择测点
            {
                divPoint.Visible = true;
                ddlPoint.Visible = true;
            }
            else//选择区域
            {
                divPoint.Visible = false;
                ddlPoint.Visible = false;
            }
        }
        #endregion

        #region 选择年月变更事件
        /// <summary>
        /// 选择年月变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadMonthYearPicker1_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekList();
        }
        #endregion

        #region 绑定周数据
        private void BindWeekList()
        {
            if (RadMonthYearPicker1.SelectedDate > System.DateTime.Now)
            {
                Alert("选择时间必须小于等于当前时间！");
                RadMonthYearPicker1.SelectedDate = DateTime.Now;
                return;
            }
            System.DateTime cuMonth = RadMonthYearPicker1.SelectedDate ?? System.DateTime.Now;
            ddlWeek.DataValueField = "value";
            ddlWeek.DataTextField = "text";
            ddlWeek.DataSource = DateTimeExtensions.GetWeekOfMonth(cuMonth);
            ddlWeek.DataBind();
            SetLiteral();
        }
        #endregion

        #region 周变更事件
        /// <summary>
        /// 周变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetLiteral();
        }
        #endregion

        #region 计算一年中的第几周
        /// <summary>
        /// 计算一年中的第几周
        /// </summary>
        private void SetLiteral()
        {
            System.DateTime endDate = System.DateTime.ParseExact(ddlWeek.SelectedValue, "yyyy-MM-dd", null).AddDays(6);
            if (endDate > System.DateTime.Now)
            {
                endDate = System.DateTime.Now;
            }
            Literal1.Text = string.Format("时间：从{0:yyyy-MM-dd}到{1:yyyy-MM-dd}；全年第{2}周", ddlWeek.SelectedValue, endDate, DateTimeExtensions.WeekOfYear(DateTime.ParseExact(ddlWeek.SelectedValue, "yyyy-MM-dd", null).AddDays(6)));
        }
        #endregion

        #region 绑定年数据
        public void BindYearList()
        {
            int year = DateTime.Now.Year > 2022 ? DateTime.Now.Year : 2022;
            for (int i = 2012; i <= year; i++)
            {
                ddlYear.Items.Insert(i - 2012, new ListItem(i.ToString() + "年", i.ToString()));
                ddlYear.SelectedValue = Convert.ToString(DateTime.Now.Year);
            }
        }
        #endregion

        #region 获取季度首日
        /// <summary>
        /// 获取季度首日
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="season">季</param>
        /// <returns></returns>
        private string GetSeasonFirstDay(int year, int season)
        {
            string result = "";
            switch (season)
            {
                case 1:
                    result = year.ToString() + "-01-01";
                    break;
                case 2:
                    result = year.ToString() + "-04-01";
                    break;
                case 3:
                    result = year.ToString() + "-07-01";
                    break;
                case 4:
                    result = year.ToString() + "-10-01";
                    break;
            }
            return result;
        }
        #endregion

        #region 获取季度末日
        /// <summary>
        /// 获取季度末日
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="season">季</param>
        /// <returns></returns>
        private string GetSeasonLastDay(int year, int season)
        {
            string result = "";
            switch (int.Parse(season.ToString()))
            {
                case 1:
                    result = year.ToString() + "-03-31";
                    break;
                case 2:
                    result = year.ToString() + "-06-30";
                    break;
                case 3:
                    result = year.ToString() + "-09-30";
                    break;
                case 4:
                    result = year.ToString() + "-12-31";
                    break;
            }
            return result;
        }
        #endregion

        #region 报表类型变更事件
        /// <summary>
        /// 报表类型变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlReportTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniDatePicker();
        }
        #endregion

        #region 初始化各种时间控件
        /// <summary>
        /// 初始化各种时间控件
        /// </summary>
        private void IniDatePicker()
        {
            switch (ddlReportTypeList.SelectedValue)
            {
                case "Week":
                    Tab1.Visible = true;
                    ddlWeek.Visible = true;
                    Literal1.Visible = true;
                    Tab2.Visible = false;
                    Tab3.Visible = false;
                    break;
                case "Month":
                    Tab1.Visible = true;
                    ddlWeek.Visible = false;
                    Literal1.Visible = false;
                    Tab2.Visible = false;
                    Tab3.Visible = false;
                    break;
                case "Season":
                    Tab1.Visible = false;
                    Tab2.Visible = true;
                    ddlSeason.Visible = true;
                    Tab3.Visible = false;
                    break;
                case "Year":
                    Tab1.Visible = false;
                    Tab2.Visible = true;
                    ddlSeason.Visible = false;
                    Tab3.Visible = false;
                    break;
                case "Self":
                    Tab1.Visible = false;
                    Tab2.Visible = false;
                    Tab3.Visible = true;
                    break;
            }
        }
        #endregion

        #region 查询按钮事件
        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            //重载HightCharts.series
            string returnValue = InitTable();
            RadScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "<script>GetHighchartsPie(escape('" + returnValue + "'));</script>", false);

            RadGrid1.DataSource = GetDataSource();
            RadGrid1.Rebind();
        }
        #endregion

        #region Guid数据绑定
        /// <summary>
        /// Guid数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = GetDataSource();
        }
        #endregion

        #region 组装数据源
        /// <summary>
        /// 组装数据源
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataSource()
        {
            int Pointid = int.Parse(ddlPoint.SelectedValue);

            DataTable dt = new DataTable();
            dt.Columns.Add("统计范围", typeof(String));
            dt.Columns.Add("SO<sub>2</sub>", typeof(String));
            dt.Columns.Add("NO<sub>2</sub>", typeof(String));
            dt.Columns.Add("PM<sub>10</sub>", typeof(String));
            dt.Columns.Add("CO", typeof(String));
            dt.Columns.Add("O<sub>3</sub> 1-hr max", typeof(String));
            dt.Columns.Add("O<sub>3</sub> 8-hr max", typeof(String));
            dt.Columns.Add("PM<sub>2.5</sub>", typeof(String));
            dt.Columns.Add("无首要污染物", typeof(String));
            dt.Columns.Add("SUM", typeof(String));

            DataRow newRow = dt.NewRow();
            string tstamp = "", tstamp2 = "";
            DateTime startDay, endDay;
            DateTime startDay2 = DateTime.Now, endDay2 = DateTime.Now;
            switch (ddlReportTypeList.SelectedValue)
            {
                case "Week":
                    startDay = DateTime.ParseExact(ddlWeek.SelectedValue, "yyyy-MM-dd", null);
                    endDay = startDay.AddDays(6);
                    DateTime firstDay = Convert.ToDateTime(startDay.ToString("yyyy-01-01"));
                    int dayOfWeek = DateTimeExtensions.WeekOfYear(startDay);
                    tstamp = string.Format("{0}年第{1}周", Convert.ToDateTime(endDay).Year, DateTimeExtensions.WeekOfYear(endDay));
                    if (dayOfWeek > 1)
                    {
                        startDay2 = firstDay.AddDays(-(int)firstDay.DayOfWeek + 1);
                        endDay2 = startDay.AddDays(-1);
                        tstamp2 = string.Format("{0}年第{1}周~{2}周", Convert.ToDateTime(startDay).Year, "1", (dayOfWeek - 1).ToString());
                    }
                    break;
                case "Month":
                    startDay = Convert.ToDateTime(Convert.ToDateTime(RadMonthYearPicker1.SelectedDate).ToString("yyyy-MM-01"));
                    endDay = startDay.AddMonths(1).AddDays(-1);
                    tstamp = string.Format("{0}年{1}月", startDay.Year, startDay.Month);
                    if (Convert.ToDateTime(RadMonthYearPicker1.SelectedDate).Month != 1)
                    {
                        startDay2 = Convert.ToDateTime(startDay.Year + "-01-01");
                        endDay2 = startDay.AddDays(-1);
                        tstamp2 = string.Format("{0}年1月~{1}月", startDay.Year, startDay.AddMonths(-1).Month);
                    }
                    break;
                case "Season":
                    startDay = Convert.ToDateTime(GetSeasonFirstDay(int.Parse(ddlYear.SelectedValue), int.Parse(ddlSeason.SelectedValue)));
                    endDay = Convert.ToDateTime(GetSeasonLastDay(int.Parse(ddlYear.SelectedValue), int.Parse(ddlSeason.SelectedValue)));
                    tstamp = string.Format("第{0}季度", ddlSeason.SelectedValue);
                    if (ddlSeason.SelectedValue != "1")
                    {
                        startDay2 = Convert.ToDateTime(startDay.Year + "-01-01");
                        endDay2 = startDay.AddDays(-1);
                        tstamp2 = string.Format("第1季度~第{0}季度", (int.Parse(ddlSeason.SelectedValue) - 1).ToString());
                    }
                    break;
                case "Year":
                    startDay = Convert.ToDateTime(ddlYear.SelectedValue + "-01-01");
                    endDay = Convert.ToDateTime(ddlYear.SelectedValue + "-12-31");
                    tstamp = string.Format("{0}年", ddlYear.SelectedValue);
                    startDay2 = DateTime.Now;
                    endDay2 = DateTime.Now;
                    break;
                case "Self":
                    startDay = Convert.ToDateTime(txtStartDate.SelectedDate);
                    endDay = Convert.ToDateTime(txtEndDate.SelectedDate);
                    tstamp = string.Format("{0}--{1}", startDay.ToString("yyyy-MM-dd"), endDay.ToString("yyyy-MM-dd"));
                    startDay2 = DateTime.Now;
                    endDay2 = DateTime.Now;
                    break;
                default:
                    startDay = Convert.ToDateTime(txtStartDate.SelectedDate);
                    endDay = Convert.ToDateTime(txtEndDate.SelectedDate);
                    tstamp = string.Format("{0}--{1}", startDay.ToString("yyyy-MM-dd"), endDay.ToString("yyyy-MM-dd"));
                    startDay2 = DateTime.Now;
                    endDay2 = DateTime.Now;
                    break;

            }

            DataView dv = new DataView();
            switch (ddlTypeList.SelectedValue)
            {
                case "0"://测点
                    dv = g_DayAQIService.GetDataByPoint(Pointid, startDay, endDay);
                    break;
                case "1"://区域
                    dv = null;
                    break;
            }

            newRow[0] = tstamp;
            if (dv.Table.Rows.Count > 0)
            {
                //SO2
                dv.RowFilter = " PrimaryPollutant like '%SO2%'";
                newRow[1] = dv.ToTable().Rows.Count;
                //NO2
                dv.RowFilter = " PrimaryPollutant like '%NO2%'";
                newRow[2] = dv.ToTable().Rows.Count;
                //PM10
                dv.RowFilter = " PrimaryPollutant like '%PM10%'";
                newRow[3] = dv.ToTable().Rows.Count;
                //CO
                dv.RowFilter = " PrimaryPollutant like '%CO%'";
                newRow[4] = dv.ToTable().Rows.Count;
                //O3 1-hr max
                dv.RowFilter = " PrimaryPollutant like '%O3-1%'";
                newRow[5] = dv.ToTable().Rows.Count;
                //O3 8-hr max
                dv.RowFilter = " PrimaryPollutant like '%O3-8%'";
                newRow[6] = dv.ToTable().Rows.Count;
                //PM2.5
                dv.RowFilter = " PrimaryPollutant like '%PM2.5%'";
                newRow[7] = dv.ToTable().Rows.Count;
                //无首要污染物
                dv.RowFilter = " PrimaryPollutant is null";
                newRow[8] = dv.ToTable().Rows.Count;
            }
            else
            {
                newRow[1] = "0";
                newRow[2] = "0";
                newRow[3] = "0";
                newRow[4] = "0";
                newRow[5] = "0";
                newRow[6] = "0";
                newRow[7] = "0";
                newRow[8] = "0";
            }
            //SUM
            newRow[9] = dv.Table.Rows.Count;

            dt.Rows.Add(newRow);

            //第二行
            if (tstamp2 != "")
            {
                DataRow newRow2 = dt.NewRow();
                switch (ddlTypeList.SelectedValue)
                {
                    case "0"://测点
                        dv = g_DayAQIService.GetDataByPoint(Pointid, startDay2, endDay2);
                        break;
                    case "1"://区域
                        dv = null;
                        break;
                }
                //时间
                newRow2[0] = tstamp2;
                if (dv.Table.Rows.Count > 0)
                {
                    //SO2
                    dv.RowFilter = " PrimaryPollutant like '%SO2%'";
                    newRow2[1] = dv.ToTable().Rows.Count;
                    //NO2
                    dv.RowFilter = " PrimaryPollutant like '%NO2%'";
                    newRow2[2] = dv.ToTable().Rows.Count;
                    //PM10
                    dv.RowFilter = " PrimaryPollutant like '%PM10%'";
                    newRow2[3] = dv.ToTable().Rows.Count;
                    //CO
                    dv.RowFilter = " PrimaryPollutant like '%CO%'";
                    newRow2[4] = dv.ToTable().Rows.Count;
                    //O3 1-hr max
                    dv.RowFilter = " PrimaryPollutant like '%O3-1%'";
                    newRow2[5] = dv.ToTable().Rows.Count;
                    //O3 8-hr max
                    dv.RowFilter = " PrimaryPollutant like '%O3-8%'";
                    newRow2[6] = dv.ToTable().Rows.Count;
                    //PM2.5
                    dv.RowFilter = " PrimaryPollutant like '%PM2.5%'";
                    newRow2[7] = dv.ToTable().Rows.Count;
                    //无首要污染物
                    dv.RowFilter = " PrimaryPollutant is null";
                    newRow2[8] = dv.ToTable().Rows.Count;
                }
                else
                {
                    newRow2[1] = "0";
                    newRow2[2] = "0";
                    newRow2[3] = "0";
                    newRow2[4] = "0";
                    newRow2[5] = "0";
                    newRow2[6] = "0";
                    newRow2[7] = "0";
                    newRow2[8] = "0";
                }
                //SUM
                newRow2[9] = dv.Table.Rows.Count;

                dt.Rows.Add(newRow2);
            }
            return dt;
        }
        #endregion

        #region 初始化HightCharts.series
        /// <summary>
        /// 初始化HightCharts.series
        /// </summary>
        public string InitTable()
        {
            DataTable dt = GetDataSource();
            string namevalue1 = "", yvalue1 = "";
            string namevalue2 = "", yvalue2 = "";
            string title1 = "", title2 = "";
            if (dt.Rows.Count > 0)
            {
                DataRow dr1 = dt.Rows[0];
                title1 = dr1["统计范围"].ToString();
                int sum = Convert.ToInt32(dr1["SUM"]);
                if (sum > 0)
                {
                    //if (Convert.ToInt32(dr1["SO<sub>2</sub>"]) > 0)
                    //{
                        namevalue1 += "SO<sub>2</sub>";
                        namevalue1 += "&";
                        yvalue1 += dr1["SO<sub>2</sub>"].ToString();
                        yvalue1 += "&";
                    //}
                    //if (Convert.ToInt32(dr1["NO<sub>2</sub>"]) > 0)
                    //{
                        namevalue1 += "NO<sub>2</sub>";
                        namevalue1 += "&";
                        yvalue1 += dr1["NO<sub>2</sub>"].ToString();
                        yvalue1 += "&";
                    //}
                    //if (Convert.ToInt32(dr1["PM<sub>10</sub>"]) > 0)
                    //{
                        namevalue1 += "PM<sub>10</sub>";
                        namevalue1 += "&";
                        yvalue1 += dr1["PM<sub>10</sub>"].ToString();
                        yvalue1 += "&";
                    //}
                    //if (Convert.ToInt32(dr1["CO"]) > 0)
                    //{
                        namevalue1 += "CO";
                        namevalue1 += "&";
                        yvalue1 += dr1["CO"].ToString();
                        yvalue1 += "&";
                    //}
                    //if (Convert.ToInt32(dr1["O<sub>3</sub> 1-hr max"]) > 0)
                    //{
                        namevalue1 += "O<sub>3</sub> 1-hr max";
                        namevalue1 += "&";
                        yvalue1 += dr1["O<sub>3</sub> 1-hr max"].ToString();
                        yvalue1 += "&";
                    //}
                    //if (Convert.ToInt32(dr1["O<sub>3</sub> 8-hr max"]) > 0)
                    //{
                        namevalue1 += "O<sub>3</sub> 8-hr max";
                        namevalue1 += "&";
                        yvalue1 += dr1["O<sub>3</sub> 8-hr max"].ToString();
                        yvalue1 += "&";
                    //}
                    //if (Convert.ToInt32(dr1["PM<sub>2.5</sub>"]) > 0)
                    //{
                        namevalue1 += "PM<sub>2.5</sub>";
                        //namevalue1 += "&";
                        yvalue1 += dr1["PM<sub>2.5</sub>"].ToString();
                    //    yvalue1 += "&";
                    //}
                }

                DataRow dr2 = dt.Rows[1];
                title2 = dr2["统计范围"].ToString();
                sum = Convert.ToInt32(dr2["SUM"]);
                if (sum > 0)
                {
                    //if (Convert.ToInt32(dr2["SO<sub>2</sub>"]) > 0)
                    //{
                        namevalue2 += "SO<sub>2</sub>";
                        namevalue2 += "&";
                        yvalue2 += dr2["SO<sub>2</sub>"].ToString();
                        yvalue2 += "&";
                    //}
                    //if (Convert.ToInt32(dr2["NO<sub>2</sub>"]) > 0)
                    //{
                        namevalue2 += "NO<sub>2</sub>";
                        namevalue2 += "&";
                        yvalue2 += dr2["NO<sub>2</sub>"].ToString();
                        yvalue2 += "&";
                    //}
                    //if (Convert.ToInt32(dr2["PM<sub>10</sub>"]) > 0)
                    //{
                        namevalue2 += "PM<sub>10</sub>";
                        namevalue2 += "&";
                        yvalue2 += dr2["PM<sub>10</sub>"].ToString();
                        yvalue2 += "&";
                    //}
                    //if (Convert.ToInt32(dr2["CO"]) > 0)
                    //{
                        namevalue2 += "CO";
                        namevalue2 += "&";
                        yvalue2 += dr2["CO"].ToString();
                        yvalue2 += "&";
                    //}
                    //if (Convert.ToInt32(dr2["O<sub>3</sub> 1-hr max"]) > 0)
                    //{
                        namevalue2 += "O<sub>3</sub> 1-hr max";
                        namevalue2 += "&";
                        yvalue2 += dr2["O<sub>3</sub> 1-hr max"].ToString();
                        yvalue2 += "&";
                    //}
                    //if (Convert.ToInt32(dr2["O<sub>3</sub> 8-hr max"]) > 0)
                    //{
                        namevalue2 += "O<sub>3</sub> 8-hr max";
                        namevalue2 += "&";
                        yvalue2 += dr2["O<sub>3</sub> 8-hr max"].ToString();
                        yvalue2 += "&";
                    //}
                    //if (Convert.ToInt32(dr2["PM<sub>2.5</sub>"]) > 0)
                    //{
                        namevalue2 += "PM<sub>2.5</sub>";
                        //namevalue2 += "&";
                        yvalue2 += dr2["PM<sub>2.5</sub>"].ToString();
                        //yvalue2 += "&";
                    //}
                }
            }
            //if (namevalue1.Length > 0 && yvalue1.Length > 0)
            //{
            //    namevalue1 = namevalue1.Substring(0, namevalue1.Length - 1);
            //    yvalue1 = yvalue1.Substring(0, yvalue1.Length - 1);
            //}
            //if (namevalue2.Length > 0 && yvalue2.Length > 0)
            //{
            //    namevalue2 = namevalue2.Substring(0, namevalue2.Length - 1);
            //    yvalue2 = yvalue2.Substring(0, yvalue2.Length - 1);
            //}
            return title1 + "☆" + namevalue1 + "☆" + yvalue1 + "☆" + title2 + "☆" + namevalue2 + "☆" + yvalue2;
        }
        #endregion
    }
}