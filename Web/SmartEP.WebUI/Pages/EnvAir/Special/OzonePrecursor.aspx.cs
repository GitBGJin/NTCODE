﻿using log4net;
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
    public partial class OzonePrecursor : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }
        #region 页面控件初始化
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
            RadioButtonList1.Items.Add(new ListItem("日数据", PollutantDataType.OriDay.ToString()));
            RadioButtonList1.Items.Add(new ListItem("月数据", PollutantDataType.OriMonth.ToString()));
            RadioButtonList1.SelectedValue = PollutantDataType.Min60.ToString();
            dbtMonth.Visible = false;

            dbtWeek.Visible = false;
            BindGrid("");
        }
        #endregion
        public void BindGrid(string type)
        {
            if (!IsPostBack)
            {

            }
            try
            {
                DateTime dtBegion = DateTime.Now;
                DateTime dtEnd = DateTime.Now;
                List<string> listTypeName = new List<string>();
                listTypeName.Add("非甲烷碳氢化合物");
                listTypeName.Add("卤代烃类");
                listTypeName.Add("含氧（氮）类");
                string[] typeName = listTypeName.ToArray();
                //给饼图传值：选中的大类别
                string typeNames = string.Empty;
                foreach (string TP in typeName)
                {
                    typeNames += TP + ",";
                }
                //隐藏域传值
                hdTypes.Value = typeNames;
                hdPoints.Value = "204,";
                hdDataType.Value = ddlDataSource.SelectedValue;
                if (ddlDataSource.SelectedValue == "AuditData")
                {
                    //饼图日期类型传值
                    hdTimeType.Value = radlDataType.SelectedValue;

                    if (radlDataType.SelectedValue == "Hour")
                    {
                        dtBegion = hourBegin.SelectedDate.Value;
                        dtEnd = hourEnd.SelectedDate.Value.AddDays(1).AddSeconds(-1);

                        hdBegion.Value = dtBegion.ToString("yyyy-MM-dd HH:mm:ss");
                        hdEnd.Value = dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
                        hdOrderBy.Value = "PointId asc,Tstamp desc";
                    }
                    else if (radlDataType.SelectedValue == "Day")
                    {
                        dtBegion = dayBegin.SelectedDate.Value;
                        dtEnd = dayEnd.SelectedDate.Value;

                        hdBegion.Value = dtBegion.ToString("yyyy-MM-dd HH:mm:ss");
                        hdEnd.Value = dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
                        hdOrderBy.Value = "PointId,DateTime";
                    }
                    else if (radlDataType.SelectedValue == "Month")
                    {
                        int monthB = monthBegin.SelectedDate.Value.Year;
                        int monthE = monthEnd.SelectedDate.Value.Year;
                        int monthF = monthBegin.SelectedDate.Value.Month;
                        int monthT = monthEnd.SelectedDate.Value.Month;

                        hdBegion.Value = monthB.ToString() + "," + monthF.ToString();
                        hdEnd.Value = monthE.ToString() + "," + monthT.ToString();
                        hdOrderBy.Value = "PointId,Year,MonthOfYear";
                    }
                    //周数据
                    else if (radlDataType.SelectedValue == "Week")
                    {
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
                        hdOrderBy.Value = "PointId,Year,WeekOfYear";
                    }
                    else if (radlDataType.SelectedValue == "Season")
                    {
                        int seasonB = seasonBegin.SelectedDate.Value.Year;
                        int seasonE = seasonEnd.SelectedDate.Value.Year;
                        int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                        int seasonT = Convert.ToInt32(seasonTo.SelectedValue);

                        hdBegion.Value = seasonB.ToString() + "," + seasonF.ToString();
                        hdEnd.Value = seasonE.ToString() + "," + seasonT.ToString();
                        hdOrderBy.Value = "PointId,Year,SeasonOfYear";
                    }
                    else if (radlDataType.SelectedValue == "Year")
                    {
                        int yearB = yearBegin.SelectedDate.Value.Year;
                        int yearE = yearEnd.SelectedDate.Value.Year;

                        hdBegion.Value = yearB.ToString();
                        hdEnd.Value = yearE.ToString();
                        hdOrderBy.Value = "PointId,Year";
                    }
                }
                else
                {
                    //饼图日期类型传值
                    hdTimeType.Value = RadioButtonList1.SelectedValue;
                    if (RadioButtonList1.SelectedValue == "Min1")
                    {
                        dtBegion = dtpBegin.SelectedDate.Value;
                        dtEnd = dtpEnd.SelectedDate.Value;
                        hdBegion.Value = dtBegion.ToString("yyyy-MM-dd HH:mm:ss");
                        hdEnd.Value = dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
                        hdOrderBy.Value = "PointId asc,Tstamp desc";
                    }
                    else if (RadioButtonList1.SelectedValue == "Min5")
                    {
                        dtBegion = dtpBegin.SelectedDate.Value;
                        dtEnd = dtpEnd.SelectedDate.Value;
                        hdBegion.Value = dtBegion.ToString("yyyy-MM-dd HH:mm:ss");
                        hdEnd.Value = dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
                        hdOrderBy.Value = "PointId asc,Tstamp desc";
                    }
                    else if (RadioButtonList1.SelectedValue == "Min60")
                    {
                        dtBegion = dtpBegin.SelectedDate.Value;
                        dtEnd = dtpEnd.SelectedDate.Value;
                        hdBegion.Value = dtBegion.ToString("yyyy-MM-dd HH:mm:ss");
                        hdEnd.Value = dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
                        hdOrderBy.Value = "PointId asc,Tstamp desc";
                    }
                    else if (RadioButtonList1.SelectedValue == "OriDay")
                    {
                        dtBegion = dtpBegin.SelectedDate.Value;
                        dtEnd = dtpEnd.SelectedDate.Value;
                        hdBegion.Value = dtBegion.ToString("yyyy-MM-dd HH:mm:ss");
                        hdEnd.Value = dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
                        hdOrderBy.Value = "PointId asc,DateTime desc";
                    }
                    else if (RadioButtonList1.SelectedValue == "OriMonth")
                    {
                        dtBegion = dtpBegin.SelectedDate.Value;
                        dtEnd = dtpEnd.SelectedDate.Value;
                        hdBegion.Value = dtBegion.ToString("yyyy-MM-dd HH:mm:ss");
                        hdEnd.Value = dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
                        hdOrderBy.Value = "PointId asc,Year desc,MonthOfYear desc";
                    }
                }
                BindCharts();
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 查找按钮事件
        /// </summary>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {

            BindGrid("图表");
        }
        private void BindCharts()
        {
            RegisterScript("CreatCharts();");
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
        #region 周数据更新日期范围
        /// <summary>
        /// 周数据更新日期范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void weekBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekComboBox();
        }
        /// <summary>
        /// 周数据更新日期范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void weekFrom_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            SetLiteral();
        }
        /// <summary>
        /// 周数据更新日期范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void weekEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekComboBox();
        }
        /// <summary>
        /// 周数据更新日期范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void weekTo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            SetLiteral();
        }

        #endregion

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

        /// <summary>
        /// 原始审核数据源切换
        /// </summary>
        protected void ddlDataSource_SelectedIndexChanged(object sender, DropDownListEventArgs e)
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
        /// <summary>
        /// 原始数据对应数据源切换
        /// </summary>
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
            else if (RadioButtonList1.SelectedValue == "OriDay")
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
    }
}