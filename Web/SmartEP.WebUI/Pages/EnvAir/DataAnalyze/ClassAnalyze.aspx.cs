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
    public partial class ClassAnalyze : SmartEP.WebUI.Common.BasePage
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
        public string returnValue = "";
        public string returnValue2 = "";
        public string xAxisCategories = "";
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
            InitTable();
            ViewState["ReturnValue"] = returnValue;
            ViewState["ReturnValue2"] = returnValue2;
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
        protected void typeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (typeList.SelectedValue == "0")//选择测点
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
                yearList.Items.Insert(i - 2012, new ListItem(i.ToString() + "年", i.ToString()));
                yearList.SelectedValue = Convert.ToString(DateTime.Now.Year);
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
        protected void ReportTypeList_SelectedIndexChanged(object sender, EventArgs e)
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
            switch (ReportTypeList.SelectedValue)
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
            InitTable();
            ViewState["ReturnValue"] = returnValue;
            ViewState["ReturnValue2"] = returnValue2;
            RegisterScript("GetHighchartsPie();");
            //RadScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "<script>GetHighchartsPie(escape('" + returnValue + "'));</script>", false);
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

            //DateTime startDay = Convert.ToDateTime("2015-01-01");
            //DateTime endDay = Convert.ToDateTime("2015-12-01");
            //RadGrid1.DataSource = g_DayAQIService.GetDataByPoint(1, startDay, endDay);
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
            dt.Columns.Add("优", typeof(String));
            dt.Columns.Add("良", typeof(String));
            dt.Columns.Add("轻度污染", typeof(String));
            dt.Columns.Add("中度污染", typeof(String));
            dt.Columns.Add("重度污染", typeof(String));
            dt.Columns.Add("严重污染", typeof(String));
            dt.Columns.Add("总和", typeof(String));
            dt.Columns.Add("优良", typeof(String));
            dt.Columns.Add("超标", typeof(String));
            dt.Columns.Add("超标率(%)", typeof(String));

            DataRow newRow = dt.NewRow();
            string tstamp = "", tstamp2 = "";
            DateTime startDay, endDay;
            DateTime startDay2 = DateTime.Now, endDay2 = DateTime.Now;
            switch (ReportTypeList.SelectedValue)
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
                    startDay = Convert.ToDateTime(GetSeasonFirstDay(int.Parse(yearList.SelectedValue), int.Parse(ddlSeason.SelectedValue)));
                    endDay = Convert.ToDateTime(GetSeasonLastDay(int.Parse(yearList.SelectedValue), int.Parse(ddlSeason.SelectedValue)));
                    tstamp = string.Format("第{0}季度", ddlSeason.SelectedValue);
                    if (ddlSeason.SelectedValue != "1")
                    {
                        startDay2 = Convert.ToDateTime(startDay.Year + "-01-01");
                        endDay2 = startDay.AddDays(-1);
                        tstamp2 = string.Format("第1季度~第{0}季度", (int.Parse(ddlSeason.SelectedValue) - 1).ToString());
                    }
                    break;
                case "Year":
                    startDay = Convert.ToDateTime(yearList.SelectedValue + "-01-01");
                    endDay = Convert.ToDateTime(yearList.SelectedValue + "-12-31");
                    tstamp = string.Format("{0}年", yearList.SelectedValue);
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
            switch (typeList.SelectedValue)
            {
                case "0"://测点
                    dv = g_DayAQIService.GetDataByPoint(Pointid, startDay, endDay);
                    break;
                case "1"://区域
                    dv = null;
                    break;
            }

            newRow[0] = tstamp;
            int r1 = 0, r2 = 0, r3 = 0, r4 = 0, r5 = 0, r6 = 0;
            //总和
            newRow[7] = dv.Table.Rows.Count;
            if (dv.Table.Rows.Count > 0)
            {
                //优
                dv.RowFilter = " Class = '优'";
                newRow[1] = r1 = dv.ToTable().Rows.Count;
                //良
                dv.RowFilter = " Class = '良'";
                newRow[2] = r2 = dv.ToTable().Rows.Count;
                //轻度污染
                dv.RowFilter = " Class = '轻度污染'";
                newRow[3] = r3 = dv.ToTable().Rows.Count;
                //中度污染
                dv.RowFilter = " Class = '中度污染'";
                newRow[4] = r4 = dv.ToTable().Rows.Count;
                //重度污染
                dv.RowFilter = "Class = '重度污染'";
                newRow[5] = r5 = dv.ToTable().Rows.Count;
                //严重污染
                dv.RowFilter = "Class = '严重污染'";
                newRow[6] = r6 = dv.ToTable().Rows.Count;
                //优良
                newRow[8] = r1 + r2;
                //超标
                newRow[9] = r3 + r4 + r5 + r6;
                //超标率
                newRow[10] = (Convert.ToDecimal((r3 + r4 + r5 + r6)) / Convert.ToDecimal((r1 + r2 + r3 + r4 + r5 + r6)) * 100).ToString("0.00");
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
                newRow[9] = "0";
                newRow[10] = "0";
            }
            //SUM
            newRow[9] = dv.Table.Rows.Count;

            dt.Rows.Add(newRow);

            //第二行
            if (tstamp2 != "")
            {
                DataRow newRow2 = dt.NewRow();
                switch (typeList.SelectedValue)
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
                newRow2[7] = dv.Table.Rows.Count;
                if (dv.Table.Rows.Count > 0)
                {
                    //优
                    dv.RowFilter = " Class = '优'";
                    newRow2[1] = r1 = dv.ToTable().Rows.Count;
                    //良
                    dv.RowFilter = " Class = '良'";
                    newRow2[2] = r2 = dv.ToTable().Rows.Count;
                    //轻度污染
                    dv.RowFilter = " Class = '轻度污染'";
                    newRow2[3] = r3 = dv.ToTable().Rows.Count;
                    //中度污染
                    dv.RowFilter = " Class = '中度污染'";
                    newRow2[4] = r4 = dv.ToTable().Rows.Count;
                    //重度污染
                    dv.RowFilter = "Class = '重度污染'";
                    newRow2[5] = r5 = dv.ToTable().Rows.Count;
                    //严重污染
                    dv.RowFilter = "Class = '严重污染'";
                    newRow2[6] = r6 = dv.ToTable().Rows.Count;
                    //优良
                    newRow2[8] = r1 + r2;
                    //超标
                    newRow2[9] = r3 + r4 + r5 + r6;
                    //超标率
                    newRow2[10] = (Convert.ToDecimal((r3 + r4 + r5 + r6)) / Convert.ToDecimal((r1 + r2 + r3 + r4 + r5 + r6)) * 100).ToString("0.00");
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
                    newRow2[9] = "0";
                    newRow2[10] = "0";
                }
                //SUM
                newRow2[9] = dv.Table.Rows.Count;
                dt.Rows.Add(newRow2);
            }

            #region 年报表各月份统计
            if (ReportTypeList.SelectedValue == "Year")
            {
                DataRow RowM2 = dt.NewRow(); DataRow RowM3 = dt.NewRow(); DataRow RowM4 = dt.NewRow(); DataRow RowM5 = dt.NewRow(); DataRow RowM6 = dt.NewRow(); DataRow RowM7 = dt.NewRow(); DataRow RowM8 = dt.NewRow(); DataRow RowM9 = dt.NewRow(); DataRow RowM10 = dt.NewRow(); DataRow RowM11 = dt.NewRow(); DataRow RowM12 = dt.NewRow();
                switch (typeList.SelectedValue)
                {
                    case "0"://测点
                        dv = g_DayAQIService.GetDataByPoint(Pointid, startDay, endDay);
                        break;
                    case "1"://区域
                        dv = null;
                        break;
                }
                DataTable newDT3 = dv.ToTable();
                if (dv.Table.Rows.Count > 0)
                {
                    for (DateTime time = Convert.ToDateTime(yearList.SelectedValue + "-01-01 00:00:00"); time < Convert.ToDateTime(yearList.SelectedValue + "-01-01 00:00:00").AddYears(1); time = time.AddMonths(1))
                    {
                        #region 一月数据
                        DataRow RowM1 = dt.NewRow();
                        r1 = 0; r2 = 0; r3 = 0; r4 = 0; r5 = 0; r6 = 0;
                        dv.RowFilter = "";
                        //优
                        RowM1[0] = time.Month + "月";
                        dv.RowFilter = " Class = '优' and datetime >= '" + time + "' and datetime < '" + time.AddMonths(1) + "'";
                        RowM1[1] = r1 = dv.ToTable().Rows.Count;
                        //良
                        dv.RowFilter = "";
                        dv.RowFilter = " Class = '良' and datetime >= '" + time + "' and datetime < '" + time.AddMonths(1) + "'";
                        RowM1[2] = r2 = dv.ToTable().Rows.Count;
                        //轻度污染
                        dv.RowFilter = "";
                        dv.RowFilter = " Class = '轻度污染' and datetime >= '" + time + "' and datetime < '" + time.AddMonths(1) + "'";
                        RowM1[3] = r3 = dv.ToTable().Rows.Count;
                        //中度污染
                        dv.RowFilter = "";
                        dv.RowFilter = " Class = '中度污染' and datetime >= '" + time + "' and datetime < '" + time.AddMonths(1) + "'";
                        RowM1[4] = r4 = dv.ToTable().Rows.Count;
                        //重度污染
                        dv.RowFilter = "";
                        dv.RowFilter = "Class = '重度污染' and datetime >= '" + time + "' and datetime < '" + time.AddMonths(1) + "'";
                        RowM1[5] = r5 = dv.ToTable().Rows.Count;
                        //严重污染
                        dv.RowFilter = "";
                        dv.RowFilter = "Class = '严重污染' and datetime >= '" + time + "' and datetime < '" + time.AddMonths(1) + "'";
                        RowM1[6] = r6 = dv.ToTable().Rows.Count;
                        //SUM
                        RowM1[7] = r1 + r2 + r3 + r4 + r5 + r6;
                        //优良
                        RowM1[8] = r1 + r2;
                        //超标
                        RowM1[9] = r3 + r4 + r5 + r6;
                        //超标率
                        if ((r1 + r2 + r3 + r4 + r5 + r6) == 0)
                            RowM1[10] = 0;
                        else
                            RowM1[10] = (Convert.ToDecimal((r3 + r4 + r5 + r6)) / Convert.ToDecimal((r1 + r2 + r3 + r4 + r5 + r6))).ToString("0.00");
                        dt.Rows.Add(RowM1);
                        #endregion
                    }
                }

            }
            #endregion
            return dt;
        }
        #endregion

        #region 初始化HightCharts.series
        /// <summary>
        /// 初始化HightCharts.series
        /// </summary>
        private void InitTable()
        {
            returnValue = "";
            returnValue2 = "";
            ViewState["title1"] = "";
            ViewState["title2"] = "";
            DataTable dt = GetDataSource();
            if (dt.Rows.Count > 0)
            {
                DataRow dr1 = dt.Rows[0];
                int sum = Convert.ToInt32(dr1["总和"]);
                if (sum > 0)
                {
                    returnValue = "{name:'优:" + dr1["优"].ToString() + "天," + (Convert.ToInt32(dr1["优"]) * 100 / sum).ToString() + " %',y:" + dr1["优"].ToString() + ",color:'#2ff103'},";

                    returnValue += "{name:'良:" + dr1["良"].ToString() + "天," + (Convert.ToInt32(dr1["良"]) * 100 / sum).ToString() + " %',y:" + dr1["良"].ToString() + ",color:'#ffff03'},";

                    returnValue += "{name:'轻度污染 :" + dr1["轻度污染"].ToString() + "天," + (Convert.ToInt32(dr1["轻度污染"]) * 100 / sum).ToString() + " %',y:" + dr1["轻度污染"].ToString() + ",color:'#ff8700'},";

                    returnValue += "{name:'中度污染 :" + dr1["中度污染"].ToString() + "天," + (Convert.ToInt32(dr1["中度污染"]) * 100 / sum).ToString() + " %',y:" + dr1["中度污染"].ToString() + ",color:'#fc000e'},";

                    returnValue += "{name:'重度污染 :" + dr1["重度污染"].ToString() + "天," + (Convert.ToInt32(dr1["重度污染"]) * 100 / sum).ToString() + " %',y:" + dr1["重度污染"].ToString() + ",color:'#9c0043'},";

                    returnValue += "{name:'严重污染 :" + dr1["严重污染"].ToString() + "天," + (Convert.ToInt32(dr1["严重污染"]) * 100 / sum).ToString() + " %',y:" + dr1["严重污染"].ToString() + ",color:'#730022'}";
                    ViewState["title1"] = dr1["统计范围"].ToString();
                }
                else
                {
                    returnValue = "{name:'优:" + dr1["优"].ToString() + "天,0%',y:" + dr1["优"].ToString() + ",color:'#2ff103'},";

                    returnValue += "{name:'良:" + dr1["良"].ToString() + "天,0%',y:" + dr1["良"].ToString() + ",color:'#ffff03'},";

                    returnValue += "{name:'轻度污染 :" + dr1["轻度污染"].ToString() + "天,0%',y:" + dr1["轻度污染"].ToString() + ",color:'#ff8700'},";

                    returnValue += "{name:'中度污染 :" + dr1["中度污染"].ToString() + "天,0%',y:" + dr1["中度污染"].ToString() + ",color:'#fc000e'},";

                    returnValue += "{name:'重度污染 :" + dr1["重度污染"].ToString() + "天,0%',y:" + dr1["重度污染"].ToString() + ",color:'#9c0043'},";

                    returnValue += "{name:'严重污染 :" + dr1["严重污染"].ToString() + "天,0%',y:" + dr1["严重污染"].ToString() + ",color:'#730022'}";
                    ViewState["title1"] = dr1["统计范围"].ToString();
                }

                if (dt.Rows.Count > 1)
                {
                    DataRow dr2 = dt.Rows[1];
                    sum = Convert.ToInt32(dr2["总和"]);
                    if (sum > 0)
                    {
                        returnValue2 = "{name:'优:" + dr2["优"].ToString() + "天," + (Convert.ToInt32(dr2["优"]) * 100 / sum).ToString() + " %',y:" + dr2["优"].ToString() + ",color:'#2ff103'},";

                        returnValue2 += "{name:'良:" + dr2["良"].ToString() + "天," + (Convert.ToInt32(dr2["良"]) * 100 / sum).ToString() + " %',y:" + dr2["良"].ToString() + ",color:'#ffff03'},";

                        returnValue2 += "{name:'轻度污染 :" + dr2["轻度污染"].ToString() + "天," + (Convert.ToInt32(dr2["轻度污染"]) * 100 / sum).ToString() + " %',y:" + dr2["轻度污染"].ToString() + ",color:'#ff8700'},";

                        returnValue2 += "{name:'中度污染 :" + dr2["中度污染"].ToString() + "天," + (Convert.ToInt32(dr2["中度污染"]) * 100 / sum).ToString() + " %',y:" + dr2["中度污染"].ToString() + ",color:'#fc000e'},";

                        returnValue2 += "{name:'重度污染 :" + dr2["重度污染"].ToString() + "天," + (Convert.ToInt32(dr2["重度污染"]) * 100 / sum).ToString() + " %',y:" + dr2["重度污染"].ToString() + ",color:'#9c0043'},";

                        returnValue2 += "{name:'严重污染 :" + dr2["严重污染"].ToString() + "天," + (Convert.ToInt32(dr2["严重污染"]) * 100 / sum).ToString() + " %',y:" + dr2["严重污染"].ToString() + ",color:'#730022'}";
                        ViewState["title2"] = dr2["统计范围"].ToString();
                    }
                    else
                    {
                        returnValue2 = "{name:'优:" + dr2["优"].ToString() + "天,0%',y:" + dr2["优"].ToString() + ",color:'#2ff103'},";

                        returnValue2 += "{name:'良:" + dr2["良"].ToString() + "天,0%',y:" + dr2["良"].ToString() + ",color:'#ffff03'},";

                        returnValue2 += "{name:'轻度污染 :" + dr2["轻度污染"].ToString() + "天,0%',y:" + dr2["轻度污染"].ToString() + ",color:'#ff8700'},";

                        returnValue2 += "{name:'中度污染 :" + dr2["中度污染"].ToString() + "天,0%',y:" + dr2["中度污染"].ToString() + ",color:'#fc000e'},";

                        returnValue2 += "{name:'重度污染 :" + dr2["重度污染"].ToString() + "天,0%',y:" + dr2["重度污染"].ToString() + ",color:'#9c0043'},";

                        returnValue2 += "{name:'严重污染 :" + dr2["严重污染"].ToString() + "天,0%',y:" + dr2["严重污染"].ToString() + ",color:'#730022'}";
                        ViewState["title2"] = dr2["统计范围"].ToString();
                    }
                }
            }

        }
        #endregion
    }
}