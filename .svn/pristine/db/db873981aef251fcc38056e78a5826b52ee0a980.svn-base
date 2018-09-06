using log4net;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.Frame;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.Calendar;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Special
{
    /// <summary>
    /// 名称：OzoneSpecial.cs
    /// 创建人：刘晋
    /// 创建日期：2017-07-16
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：臭氧专题
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class OzoneSpecial : SmartEP.WebUI.Common.BasePage
    {
        /// </summary>
        //获取一个日志记录器
        ILog log = LogManager.GetLogger("FileLogging");
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        private string[] portIds = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
                BindCharts();
            }
            
           
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {

            //时间框初始化
            DateTime dt = DateTime.Now;
            DateTime dtt = DateTime.Now.AddDays(-1);
            dt.ToLongTimeString().ToString();
            dtt.ToLongTimeString().ToString();
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            hourBegin.SelectedDate = Convert.ToDateTime(dtt.Date.ToString());
            hourEnd.SelectedDate = Convert.ToDateTime(dt.Date.ToString());
            dayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00"));
            dayEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00"));
            monthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            monthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            weekBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            weekEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));

            yearBegin.SelectedDate = DateTime.Now.AddYears(-1);
            yearEnd.SelectedDate = DateTime.Now;

            BindWeekComboBox();//绑定周
            SetLiteral();//显示周日期范围
            //数据类型
            radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
            radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
            radlDataType.Items.Add(new ListItem("周数据", PollutantDataType.Week.ToString()));
            radlDataType.Items.Add(new ListItem("月数据", PollutantDataType.Month.ToString()));
            radlDataType.Items.Add(new ListItem("季数据", PollutantDataType.Season.ToString()));
            radlDataType.Items.Add(new ListItem("年数据", PollutantDataType.Year.ToString()));
            radlDataType.SelectedValue = PollutantDataType.Day.ToString();

            RadioButtonList1.Items.Add(new ListItem("一分钟数据", PollutantDataType.Min1.ToString()));
            RadioButtonList1.Items.Add(new ListItem("五分钟数据", PollutantDataType.Min5.ToString()));
            RadioButtonList1.Items.Add(new ListItem("小时数据", PollutantDataType.Min60.ToString()));
            RadioButtonList1.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
            RadioButtonList1.Items.Add(new ListItem("月数据", PollutantDataType.Month.ToString()));
            RadioButtonList1.SelectedValue = PollutantDataType.Min60.ToString();

            dbtMonth.Visible = false;

            dbtWeek.Visible = false;
        }
        /// <summary>
        /// 绑定周
        /// </summary>
        private void BindWeekComboBox()
        {
            weekFrom.DataValueField = "value";
            weekFrom.DataTextField = "text";
            weekFrom.DataSource = ChinaDate.GetWeekOfMonth(weekBegin.SelectedDate.Value);
            weekFrom.DataBind();
            weekTo.DataValueField = "value";
            weekTo.DataTextField = "text";
            weekTo.DataSource = ChinaDate.GetWeekOfMonth(weekEnd.SelectedDate.Value);
            weekTo.DataBind();
            SetLiteral();
        }
        /// <summary>
        /// 显示所选周的日期
        /// </summary>
        private void SetLiteral()
        {
            DateTime endDate = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6);
            txtweekF.Text = string.Format("{0:yyyy-MM-dd}", weekFrom.SelectedValue);
            txtweekT.Text = string.Format("{0:yyyy-MM-dd}", endDate);
        }

        string isSuper = string.Empty;
        
        protected void pointCbxRsm_SelectedChanged()
        {

        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (tabStrip.SelectedTab.Text == "太阳辐射")
            {
                BindChart();
            }
            else if (tabStrip.SelectedTab.Text == "VOC因子占比")
            {
                BindGrid();
                BindCharts();
            }
            else if (tabStrip.SelectedTab.Text == "气象参数分析图")
            {
                BindGrid();
                BindCharts();
            }
        }
        private void BindChart1()
        {
            RegisterScript("CreatCharts();");

        }
        private void BindCharts()
        {
            RegisterScript("CreatCharts();");
            
        }
        private void BindChart()
        {
            
            RegisterScript("InitGroupChart();");
        }
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            if (!IsPostBack)
            {

            }
            try
            {
                List<string> listTypeName = new List<string>();
                listTypeName.Add("非甲烷碳氢化合物");
                listTypeName.Add("卤代烃类VOCs");
                listTypeName.Add("含氧（氮）类VOCs");
                string[] typeName = listTypeName.ToArray();
                //给饼图传值：选中的大类别
                string typeNames = string.Empty;
                foreach (string TP in typeName)
                {
                    typeNames += TP + ",";
                }
                //隐藏域传值
                hdTypes.Value = typeNames;

                portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
                //给饼图传值：站点
                string pointIds = string.Empty;
                foreach (string PI in portIds)
                {
                    pointIds += PI + ",";
                }
                //隐藏域传值
                hdPoints.Value = pointIds;
                hdPoint.Value = "204,";
                hdDataType.Value = ddlDataSource.SelectedValue;

                DateTime dtBegion = DateTime.Now;
                DateTime dtEnd = DateTime.Now;

                string[] factors = {"a51039","a04003","a05024"};
                if (portIds != null)
                {
                    if (ddlDataSource.SelectedValue == "AuditData")
                    {
                        //饼图日期类型传值
                        hdTimeType.Value = radlDataType.SelectedValue;
                        if (radlDataType.SelectedValue == "Hour")
                        {
                            string orderBy1 = "PointId asc,Tstamp desc";
                            dtBegion = hourBegin.SelectedDate.Value;
                            dtEnd = hourEnd.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                            hdBegion.Value = dtBegion.ToString("yyyy-MM-dd HH:00:00");
                            hdEnd.Value = dtEnd.ToString("yyyy-MM-dd HH:00:00");
                            hdOrderBy.Value = orderBy1;
                            dataType.Value = "Hour";
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                        }
                        else if (radlDataType.SelectedValue == "Day")
                        {
                            string orderBy1 = "PointId,DateTime";
                            dtBegion = dayBegin.SelectedDate.Value;
                            dtEnd = dayEnd.SelectedDate.Value;
                            hdBegion.Value = dtBegion.ToString("yyyy-MM-dd");
                            hdEnd.Value = dtEnd.ToString("yyyy-MM-dd");
                            hdOrderBy.Value = orderBy1;
                            dataType.Value = "Day";
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                        }
                        else if (radlDataType.SelectedValue == "Month")
                        {
                            string orderBy1 = "PointId,Year,MonthOfYear";
                            int monthB = monthBegin.SelectedDate.Value.Year;
                            int monthE = monthEnd.SelectedDate.Value.Year;
                            int monthF = monthBegin.SelectedDate.Value.Month;
                            int monthT = monthEnd.SelectedDate.Value.Month;
                            hdBegion.Value = monthB.ToString()+","+monthF.ToString();
                            hdEnd.Value = monthE.ToString() + "," + monthT.ToString();
                            hdOrderBy.Value = orderBy1;
                            dataType.Value = "Month";
                            SetHiddenData(portIds, factors, monthB + ";" + monthF + ";" + monthE + ";" + monthT);
                        }
                        else if (radlDataType.SelectedValue == "Week")
                        {
                            string orderBy1 = "PointId,Year,WeekOfYear";
                            int weekB = weekBegin.SelectedDate.Value.Year;
                            int weekE = weekEnd.SelectedDate.Value.Year;
                            int newyear = DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6).Year;
                            int nyear = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).Year;
                            int weekF = 0;
                            int weekT = 0;
                            if (newyear > weekB)
                            {
                                weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null));
                            }
                            else
                                weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6));

                            if (weekE > nyear)
                            {
                                weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6));
                            }
                            else
                            {
                                weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null));
                            }

                            hdBegion.Value = weekB.ToString() + "," + weekF.ToString();
                            hdEnd.Value = weekE.ToString() + "," + weekT.ToString();
                            hdOrderBy.Value = orderBy1;

                            SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT);
                        }
                        else if (radlDataType.SelectedValue == "Season")
                        {
                            string orderBy1 = "PointId,Year,SeasonOfYear";
                            int seasonB = seasonBegin.SelectedDate.Value.Year;
                            int seasonE = seasonEnd.SelectedDate.Value.Year;
                            int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                            int seasonT = Convert.ToInt32(seasonTo.SelectedValue);

                            hdBegion.Value = seasonB.ToString() + "," + seasonF.ToString();
                            hdEnd.Value = seasonE.ToString() + "," + seasonT.ToString();
                            hdOrderBy.Value = orderBy1;

                            SetHiddenData(portIds, factors, seasonB + ";" + seasonF + ";" + seasonE + ";" + seasonT);
                        }
                        else if (radlDataType.SelectedValue == "Year")
                        {
                            string orderBy1 = "PointId,Year";
                            int yearB = yearBegin.SelectedDate.Value.Year;
                            int yearE = yearEnd.SelectedDate.Value.Year;

                            hdBegion.Value = yearB.ToString();
                            hdEnd.Value = yearE.ToString();
                            hdOrderBy.Value = orderBy1;

                            SetHiddenData(portIds, factors, yearB + ";" + yearE);
                        }
                    }
                    else
                    {
                        hdTimeType.Value = RadioButtonList1.SelectedValue;
                        if (RadioButtonList1.SelectedValue == "Min1")
                        {
                            dtBegion = dtpBegin.SelectedDate.Value;
                            dtEnd = dtpEnd.SelectedDate.Value;
                            string orderBy1 = "PointId asc,Tstamp desc";

                            hdBegion.Value = dtBegion.ToString("yyyy-MM-dd HH:mm:ss");
                            hdEnd.Value = dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
                            hdOrderBy.Value = orderBy1;
                            dataType.Value = "Min1";
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                        }
                        else if (RadioButtonList1.SelectedValue == "Min5")
                        {
                            dtBegion = dtpBegin.SelectedDate.Value;
                            dtEnd = dtpEnd.SelectedDate.Value;
                            string orderBy1 = "PointId asc,Tstamp desc";

                            hdBegion.Value = dtBegion.ToString("yyyy-MM-dd HH:mm:ss");
                            hdEnd.Value = dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
                            hdOrderBy.Value = orderBy1;
                            dataType.Value = "Min5";
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                        }
                        else if (RadioButtonList1.SelectedValue == "Min60")
                        {
                            dtBegion = dtpBegin.SelectedDate.Value;
                            dtEnd = dtpEnd.SelectedDate.Value;
                            string orderBy1 = "PointId asc,Tstamp desc";

                            hdBegion.Value = dtBegion.ToString("yyyy-MM-dd HH:00:00");
                            hdEnd.Value = dtEnd.ToString("yyyy-MM-dd HH:00:00");
                            hdOrderBy.Value = orderBy1;
                            dataType.Value = "Min60";
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                        }
                        else if (RadioButtonList1.SelectedValue == "Day")
                        {
                            dtBegion = dtpBegin.SelectedDate.Value;
                            dtEnd = dtpEnd.SelectedDate.Value;
                            string orderBy1 = "PointId asc,DateTime desc";

                            hdBegion.Value = dtBegion.ToString("yyyy-MM-dd");
                            hdEnd.Value = dtEnd.ToString("yyyy-MM-dd");
                            hdOrderBy.Value = orderBy1;
                            dataType.Value = "OriDay";
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                        }
                        else if (RadioButtonList1.SelectedValue == "Month")
                        {
                            string orderBy1 = "PointId asc,Year desc,MonthOfYear desc";
                            dtBegion = dtpBegin.SelectedDate.Value;
                            dtEnd = dtpEnd.SelectedDate.Value;

                            hdBegion.Value = dtBegion.ToString("yyyy-MM-01 00:00:00");
                            hdEnd.Value = dtEnd.ToString("yyyy-MM-dd 23:59:59");
                            hdOrderBy.Value = orderBy1;
                            dataType.Value = "OriMonth";
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                log.Error(e.ToString());
            }
        }
        #endregion

        /// <summary>
        /// 页面隐藏域控件赋值（小时、日），将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void SetHiddenData(string[] portIds, string [] factors, DateTime dtBegin, DateTime dtEnd)
        {
            if (ddlDataSource.SelectedValue == "OriData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors)
                                 + "|" + dtBegin + "|" + dtEnd + "|" + RadioButtonList1.SelectedValue + "|Air";
                HiddenChartType.Value = ChartType.SelectedValue;
            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors)
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "|Air";
                HiddenChartType.Value = ChartType.SelectedValue;
            }
        }

        /// <summary>
        /// 页面隐藏域控件赋值,(周、月、季、年)
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="timeStr"></param>
        private void SetHiddenData(string[] portIds, string[] factors, string timeStr)
        {
            if (ddlDataSource.SelectedValue == "OriData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors)
                                 + "|" + timeStr + "|" + "|" + RadioButtonList1.SelectedValue + "|Air";
                HiddenChartType.Value = ChartType.SelectedValue;
            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors)
                                 + "|" + timeStr + "|" + "|" + radlDataType.SelectedValue + "|Air";
                HiddenChartType.Value = ChartType.SelectedValue;
            }
        }
        /// <summary>
        /// 数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //小时数据
            if (radlDataType.SelectedValue == "Hour")
            {
                dbtHour.Visible = true;
                dbtDay.Visible = false;
                dbtWeek.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
            }
            //日数据
            else if (radlDataType.SelectedValue == "Day")
            {
                dbtDay.Visible = true;

                dbtHour.Visible = false;
                dbtWeek.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;

            }
            //周数据
            else if (radlDataType.SelectedValue == "Week")
            {
                dbtWeek.Visible = true;

                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;

            }
            //月数据
            else if (radlDataType.SelectedValue == "Month")
            {
                dbtMonth.Visible = true;

                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;

            }
            //季数据
            else if (radlDataType.SelectedValue == "Season")
            {
                dbtSeason.Visible = true;

                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtWeek.Visible = false;
                dbtMonth.Visible = false;
                dbtYear.Visible = false;

            }
            //年数据
            else if (radlDataType.SelectedValue == "Year")
            {
                dbtYear.Visible = true;

                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtWeek.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;

            }
        }

        protected void radlDataTypeOri_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void weekFrom_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {

        }

        protected void weekTo_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {

        }

        protected void ddlDataSource_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            if (ddlDataSource.SelectedValue == "OriData")
            {
                radlDataType.Visible = false;
                RadioButtonList1.Visible = true;
                dtpHour.Visible = true;

                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtWeek.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                radlDataType.Visible = true;
                RadioButtonList1.Visible = false;

                dtpHour.Visible = false;

                dbtHour.Visible = false;
                dbtDay.Visible = true;
                dbtWeek.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
            }
        }

        protected void gridAudit_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

        }

        protected void gridAudit_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridAudit_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void gridRTB_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {

        }

        protected void ShowType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void weekBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {

        }

        protected void weekEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {

        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (RadioButtonList1.SelectedValue == "Min1")
            {
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm:ss";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm:ss";
            }
            else if (RadioButtonList1.SelectedValue == "Min5")
            {
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            }
            else if (RadioButtonList1.SelectedValue == "Min60")
            {
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            }
            else if (RadioButtonList1.SelectedValue == "Day")
            {
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd";
            }
            else
            {
                dtpBegin.DateInput.DateFormat = "yyyy-MM";
                dtpEnd.DateInput.DateFormat = "yyyy-MM";
            }
        }

        protected void ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenChartType.Value = ChartType.SelectedValue;
            RegisterScript("ChartTypeChanged('" + ChartType.SelectedValue + "');");
        }

        protected void PointFactor_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenPointFactor.Value = PointFactor.SelectedValue;
            RegisterScript("PointFactor('" + PointFactor.SelectedValue + "');");
        }
    }
}