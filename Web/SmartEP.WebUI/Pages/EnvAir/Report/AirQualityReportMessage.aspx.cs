using SmartEP.Core.Generic;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.Frame;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AirQualityReportMessage : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DictionaryService dicService = Singleton<DictionaryService>.GetInstance();
        DayAQIService m_DayAQIService = new DayAQIService();
        MonthAQIService m_MonthAQIService = new MonthAQIService();
        MessageTextService g_MessageTextService = Singleton<MessageTextService>.GetInstance();
        /// <summary>
        /// 空气污染指数
        /// </summary>
        AQIService s_AQIService = new AQIService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ViewState["DisplayPerson"] = SessionHelper.Get("DisplayName").ToString();
                if (this.ViewState["DisplayPerson"].ToString() == "")
                {
                    this.ViewState["DisplayPerson"] = "超级管理员";
                }
                dayDate.SelectedDate = DateTime.Now.AddDays(-1);
                dtpBegin.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
                dtpEnd.SelectedDate = DateTime.Now.AddDays(-1);
                seasonBegin.SelectedDate = new DateTime(DateTime.Now.AddMonths(-1).Year, 1, 1);
                seasonEnd.SelectedDate = DateTime.Now;
                dayend.SelectedDate = DateTime.Now;
                daybegin.SelectedDate = DateTime.Now.AddDays(-7);
                weekend.SelectedDate = DateTime.Now;
                weekbegin.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
                monthend.SelectedDate = DateTime.Now;
                monthbegin.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
                this.ViewState["messagetype"] = PageHelper.GetQueryString("messagetype") != "" ? PageHelper.GetQueryString("messagetype") : "day";
                insertOrUpdate();
                if (this.ViewState["messagetype"].ToString() == "day")
                {
                    dbtDay.Visible = true;
                    dbtWeek.Visible = false;
                    dbtSeason.Visible = false;
                    auditor.Visible = false;
                    Div1.Visible = true;
                    Div2.Visible = false;
                    Div3.Visible = false;
                }
                else if (this.ViewState["messagetype"].ToString() == "week")
                {
                    dbtDay.Visible = false;
                    dbtWeek.Visible = true;
                    dbtSeason.Visible = false;
                    auditor.Visible = true;
                    Div1.Visible = false;
                    Div2.Visible = true;
                    Div3.Visible = false;
                }
                else
                {
                    dbtDay.Visible = false;
                    dbtWeek.Visible = false;
                    dbtSeason.Visible = true;
                    auditor.Visible = false;
                    Div1.Visible = false;
                    Div2.Visible = false;
                    Div3.Visible = true;
                }
            }
        }

        #region 加载报表数据
        /// <summary>
        /// 加载报表数据
        /// </summary>
        private void LoadingReport(string messageType)
        {
            string[] regionGuids = dicService.RetrieveCityList().Where(t => t.ItemText == "苏州市区").Select(t => t.ItemGuid).ToArray();
            if (messageType == "day")
            {
                DateTime dtbegin = new DateTime(dayDate.SelectedDate.Value.Year, dayDate.SelectedDate.Value.Month, dayDate.SelectedDate.Value.Day);
                DateTime dtend = dtbegin.AddDays(+1).AddSeconds(-1);
                int recordTotal = 0;
                var dtday = m_DayAQIService.GetAreaDataPager(regionGuids, dtbegin, dtend, 9999, 0, out recordTotal);
                string grade = "";
                string AQIValue = "";
                string promaryPollutant = "";
                string pm25 = "";
                if (dtday.Count > 0)
                {
                    grade = dtday[0]["Class"] != DBNull.Value ? dtday[0]["Class"].ToString() : "";
                    AQIValue = dtday[0]["AQIValue"] != DBNull.Value ? dtday[0]["AQIValue"].ToString() : "";
                    promaryPollutant = dtday[0]["PrimaryPollutant"] != DBNull.Value ? dtday[0]["PrimaryPollutant"].ToString() : "--";
                    if (promaryPollutant.Contains("O3"))
                    {
                        promaryPollutant = promaryPollutant.Replace("O3", "臭氧");
                    }
                    if (promaryPollutant.Contains("NO2"))
                    {
                        promaryPollutant = promaryPollutant.Replace("NO2", "二氧化氮");
                    }
                    if (promaryPollutant.Contains("CO"))
                    {
                        promaryPollutant = promaryPollutant.Replace("CO", "一氧化碳");
                    }
                    if (promaryPollutant.Contains("SO2"))
                    {
                        promaryPollutant = promaryPollutant.Replace("SO2", "二氧化硫");
                    }
                    if (promaryPollutant.Contains("PM10"))
                    {
                        promaryPollutant = promaryPollutant.Replace("PM10", "可吸入颗粒物");
                    }
                    pm25 = dtday[0]["PM25"] != DBNull.Value ? Math.Round(decimal.Parse(dtday[0]["PM25"].ToString()) * 1000, 0).ToString() : "";
                }
                string messagetext = "【" + dtbegin.ToString("yyyy年M月d日") + "苏州市区环境空气质量日报】空气质量类别：" + grade + "；空气质量指数（AQI）：" + AQIValue
                    + "；首要污染物：" + promaryPollutant + "；PM2.5浓度：" + pm25 + "微克/立方米。\n【空气质量预报】未来24小时，空气质量以XX为主。【市环境监测中心，市气象台】";
                taMessage.InnerHtml = messagetext;
            }
            else if (messageType == "week")
            {
                DateTime dtBegin = dtpBegin.SelectedDate.Value;
                DateTime dtEnd = dtpEnd.SelectedDate.Value;
                DataTable dt = m_DayAQIService.GetRegionsTable(regionGuids, dtBegin, dtEnd);
                string thisValue = dt.Rows[0][dtEnd.Year.ToString()] != DBNull.Value ? (dt.Rows[0][dtEnd.Year.ToString()].ToString()) : "--";
                string lastValue = dt.Rows[0][dtEnd.AddYears(-1).Year.ToString()] != DBNull.Value ? (dt.Rows[0][dtEnd.AddYears(-1).Year.ToString()].ToString()) : "--";
                string lastText = "";
                if (lastValue != "--" && thisValue != "--")
                {
                    if (decimal.Parse(thisValue) > decimal.Parse(lastValue))
                    {
                        lastText = ",与" + dtEnd.AddYears(-1).Year.ToString() + "年同期相比，PM2.5浓度上升了" + Math.Round((decimal.Parse(thisValue) - decimal.Parse(lastValue)) / decimal.Parse(lastValue) * 100, 1).ToString() + "%";
                    }
                    else if (decimal.Parse(thisValue) < decimal.Parse(lastValue))
                    {
                        lastText = ",与去" + dtEnd.AddYears(-1).Year.ToString() + "年同期相比，PM2.5浓度下降了" + Math.Round((decimal.Parse(lastValue) - decimal.Parse(thisValue)) / decimal.Parse(lastValue) * 100, 1).ToString() + "%";
                    }
                    else
                    {
                        lastText = ",与" + dtEnd.AddYears(-1).Year.ToString() + "年同期相比，PM2.5浓度持平";
                    }
                }
                string thisValue2013 = dt.Rows[0][dtEnd.Year.ToString()] != DBNull.Value ? (dt.Rows[0][dtEnd.Year.ToString()].ToString()) : "--";
                string lastValue2013 = dt.Rows[0]["2013"] != DBNull.Value ? (dt.Rows[0]["2013"].ToString()) : "--";
                string lastText2013 = "";
                string compare2013 = "";
                if (lastValue2013 != "--" && thisValue2013 != "--")
                {
                    if (decimal.Parse(thisValue2013) > decimal.Parse(lastValue2013))
                    {
                        lastText2013 = "。与2013年同期相比，PM2.5浓度上升了" + Math.Round((decimal.Parse(thisValue2013) - decimal.Parse(lastValue2013)) / decimal.Parse(lastValue2013) * 100, 1).ToString() + "%";
                    }
                    else if (decimal.Parse(thisValue2013) < decimal.Parse(lastValue2013))
                    {
                        lastText2013 = "。与2013年同期相比，PM2.5浓度下降了" + Math.Round((decimal.Parse(lastValue2013) - decimal.Parse(thisValue2013)) / decimal.Parse(lastValue2013) * 100, 1).ToString() + "%";
                    }
                    else
                    {
                        lastText2013 = "。与2013年同期相比，PM2.5浓度持平";
                    }
                    compare2013 = Math.Round((decimal.Parse(thisValue2013) - decimal.Parse(lastValue2013)) / decimal.Parse(lastValue2013) * 100, 1).ToString() + "%";
                }
                else
                {
                    compare2013 = "/";
                }
                string summaryText = "【" + dtEnd.Year.ToString() + "年苏州市环境空气质量周报】截止到" + dtEnd.ToString("yyyy年M月d日") + "，苏州市PM2.5浓度均值为" + thisValue + "微克/立方米" + lastText + lastText2013 + "。编制：" + this.ViewState["DisplayPerson"].ToString() + "；审核：" + ddlAuditor.SelectedItem.Text + "。【苏州环境监测】";
                taMessage.InnerHtml = summaryText;
            }
            else if (messageType == "season")
            {
                DateTime dtbegin = new DateTime(seasonBegin.SelectedDate.Value.Year, seasonBegin.SelectedDate.Value.Month, 1);
                DateTime endtemp = new DateTime(seasonEnd.SelectedDate.Value.Year, seasonEnd.SelectedDate.Value.Month, 1);
                DateTime dtend = endtemp.AddMonths(+1).AddSeconds(-1);
                if (dtend > DateTime.Now)
                {
                }
                int recordTotal = 0;
                //var dtAQI = m_DayAQIService.GetRegionsAQI(dtbegin, dtend).Table;
                var monthAQI = m_MonthAQIService.GetDataMonthPager(dtbegin, dtend).Table;
                string AQIValue = "";

                if (monthAQI.Rows.Count > 0)
                {
                    AQIValue = monthAQI.Rows[0][6].ToString();
                }
                ////全市AQI均值
                //if (dtAQI.Rows.Count > 0)
                //{
                //    for (int j = 0; j < 6; j++)
                //    {
                //        string factorCode = dtAQI.Columns[j].ColumnName;
                //        int num = 24;
                //        if (factorCode == "a05024")
                //        {
                //            num = 8;
                //        }
                //        decimal Con = Convert.ToDecimal(dtAQI.Rows[0][j]);
                //        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, Con, num), 0);
                //        if (AQIValue < temp)
                //        {
                //            AQIValue = temp;
                //        }
                //    }
                //}
                DataTable dt = m_DayAQIService.GetRegionsTable(regionGuids, dtbegin, dtend);
                string lastStandRate = dt.Rows[0]["lastStandRate"] != DBNull.Value ? (dt.Rows[0]["lastStandRate"].ToString()) : "--";
                string thisStandRate = dt.Rows[0]["thisStandRate"] != DBNull.Value ? (dt.Rows[0]["thisStandRate"].ToString()) : "--";
                string text2013 = "";
                if (lastStandRate != "--" && thisStandRate != "--")
                {
                    if (decimal.Parse(thisStandRate) > decimal.Parse(lastStandRate))
                    {
                        text2013 = ",与去年同期相比有所上升";
                    }
                    else if (decimal.Parse(thisStandRate) < decimal.Parse(lastStandRate))
                    {
                        text2013 = ",与去年同期相比有所下降";
                    }
                    else
                    {
                        text2013 = ",与去年同期相比持平";
                    }
                }
                string thisValue = dt.Rows[0][dtend.Year.ToString()] != DBNull.Value ? (dt.Rows[0][dtend.Year.ToString()].ToString()) : "--";
                string lastValue = dt.Rows[0][dtend.AddYears(-1).Year.ToString()] != DBNull.Value ? (dt.Rows[0][dtend.AddYears(-1).Year.ToString()].ToString()) : "--";
                string lastText = "";
                if (lastValue != "--" && thisValue != "--")
                {
                    if (decimal.Parse(thisValue) > decimal.Parse(lastValue))
                    {
                        lastText = ",与去年同期相比，PM2.5浓度上升了" + Math.Round((decimal.Parse(thisValue) - decimal.Parse(lastValue)) / decimal.Parse(lastValue) * 100, 1).ToString() + "%";
                    }
                    else if (decimal.Parse(thisValue) < decimal.Parse(lastValue))
                    {
                        lastText = ",与去年同期相比，PM2.5浓度下降了" + Math.Round((decimal.Parse(lastValue) - decimal.Parse(thisValue)) / decimal.Parse(lastValue) * 100, 1).ToString() + "%";
                    }
                    else
                    {
                        lastText = ",与去年同期相比，PM2.5浓度持平";
                    }
                }
                string thisValue2013 = dt.Rows[0][dtend.Year.ToString()] != DBNull.Value ? (dt.Rows[0][dtend.Year.ToString()].ToString()) : "--";
                string lastValue2013 = dt.Rows[0]["2013"] != DBNull.Value ? (dt.Rows[0]["2013"].ToString()) : "--";
                string lastText2013 = "";
                string compare2013 = "";
                if (lastValue2013 != "--" && thisValue2013 != "--")
                {
                    if (decimal.Parse(thisValue2013) > decimal.Parse(lastValue2013))
                    {
                        lastText2013 = "。与2013年同期相比，PM2.5浓度上升了" + Math.Round((decimal.Parse(thisValue2013) - decimal.Parse(lastValue2013)) / decimal.Parse(lastValue2013) * 100, 1).ToString() + "%";
                    }
                    else if (decimal.Parse(thisValue2013) < decimal.Parse(lastValue2013))
                    {
                        lastText2013 = "。与2013年同期相比，PM2.5浓度下降了" + Math.Round((decimal.Parse(lastValue2013) - decimal.Parse(thisValue2013)) / decimal.Parse(lastValue2013) * 100, 1).ToString() + "%";
                    }
                    else
                    {
                        lastText2013 = "。与2013年同期相比，PM2.5浓度持平";
                    }
                    compare2013 = Math.Round((decimal.Parse(thisValue2013) - decimal.Parse(lastValue2013)) / decimal.Parse(lastValue2013) * 100, 1).ToString() + "%";
                }
                else
                {
                    compare2013 = "/";
                }
                string summaryText = "【" + dtend.Year.ToString() + "年" + dtbegin.Month.ToString() + "-" + dtend.Month.ToString() + "月苏州市区环境空气质量状况】空气质量指数AQI均值为" + AQIValue.ToString() + "；达标天数比例为" + thisStandRate.ToString() + "%，" + text2013 + "；PM2.5平均浓度为"
                    + thisValue + "微克/立方米" + lastText + lastText2013 + "。【苏州环境监测】";
                taMessage.InnerHtml = summaryText;
            }
        }
        #endregion

        protected void btnExport_Click(object sender, ImageClickEventArgs e)
        {
            string messagetype = this.ViewState["messagetype"].ToString();
            DateTime daydate = DateTime.Now;
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;
            string daterange = "";
            if (messagetype == "day")
            {
                daydate = new DateTime(dayDate.SelectedDate.Value.Year, dayDate.SelectedDate.Value.Month, dayDate.SelectedDate.Value.Day);
                dtbegin = daydate;
                dtend = daydate;
                daterange = daydate.ToString("yyyy年M月d日");
            }
            else if (messagetype == "week")
            {
                daydate = new DateTime(dtpEnd.SelectedDate.Value.Year, dtpEnd.SelectedDate.Value.Month, dtpEnd.SelectedDate.Value.Day);
                dtbegin = new DateTime(dtpBegin.SelectedDate.Value.Year, dtpBegin.SelectedDate.Value.Month, dtpBegin.SelectedDate.Value.Day);
                dtend = new DateTime(dtpEnd.SelectedDate.Value.Year, dtpEnd.SelectedDate.Value.Month, dtpEnd.SelectedDate.Value.Day);
                daterange = dtbegin.ToString("yyyy年M月d日") + "-" + dtend.ToString("yyyy年M月d日");
            }
            else
            {
                daydate = new DateTime(seasonEnd.SelectedDate.Value.Year, seasonEnd.SelectedDate.Value.Month, 1);
                dtbegin = new DateTime(seasonBegin.SelectedDate.Value.Year, seasonBegin.SelectedDate.Value.Month, 1);
                dtend = new DateTime(seasonEnd.SelectedDate.Value.Year, seasonEnd.SelectedDate.Value.Month, 1);
                dtend = dtend.AddMonths(+1).AddSeconds(-1);
                daterange = dtbegin.ToString("yyyy年M月") + "-" + dtend.ToString("yyyy年M月");
            }
            string messagetext = taMessage.InnerHtml;
            insert(messagetype, daydate, dtbegin, dtend, messagetext, daterange);
        }

        public void insert(string messagetype, DateTime daydate, DateTime dtbegin, DateTime dtend, string messagetext, string daterange)
        {
            if (dtbegin > dtend)
            {
                Alert("开始时间不能大于结束时间！");
                return;
            }
            if (this.ViewState["state"].ToString() == "edit")
            {
                g_MessageTextService.updatedata(messagetype, daydate, dtbegin, dtend, messagetext, this.ViewState["DisplayPerson"].ToString());
                Alert("更新成功！");
            }
            else
            {
                g_MessageTextService.insertTable(messagetype, daydate, dtbegin, dtend, messagetext, daterange, this.ViewState["DisplayPerson"].ToString());
                this.ViewState["state"] = "edit";
            }
        }
        public void insertOrUpdate()
        {
            string messagetype = this.ViewState["messagetype"].ToString();
            DateTime daydate = DateTime.Now;
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;
            DateTime daycompare = DateTime.Now;
            string daterange = "";
            if (messagetype == "day")
            {
                daydate = new DateTime(dayDate.SelectedDate.Value.Year, dayDate.SelectedDate.Value.Month, dayDate.SelectedDate.Value.Day);
                dtbegin = daydate;
                dtend = daydate;
                daterange = daydate.ToString("yyyy年M月d日");
                daycompare = DateTime.Now.Date;
            }
            else if (messagetype == "week")
            {
                daydate = new DateTime(dtpEnd.SelectedDate.Value.Year, dtpEnd.SelectedDate.Value.Month, dtpEnd.SelectedDate.Value.Day);
                dtbegin = new DateTime(dtpBegin.SelectedDate.Value.Year, dtpBegin.SelectedDate.Value.Month, dtpBegin.SelectedDate.Value.Day);
                dtend = new DateTime(dtpEnd.SelectedDate.Value.Year, dtpEnd.SelectedDate.Value.Month, dtpEnd.SelectedDate.Value.Day);
                daterange = dtbegin.ToString("yyyy年M月d日") + "-" + dtend.ToString("yyyy年M月d日");
                daycompare = DateTime.Now.Date;
            }
            else
            {
                daydate = new DateTime(seasonEnd.SelectedDate.Value.Year, seasonEnd.SelectedDate.Value.Month, 1);
                dtbegin = new DateTime(seasonBegin.SelectedDate.Value.Year, seasonBegin.SelectedDate.Value.Month, 1);
                dtend = new DateTime(seasonEnd.SelectedDate.Value.Year, seasonEnd.SelectedDate.Value.Month, 1);
                dtend = dtend.AddMonths(+1).AddSeconds(-1);
                daterange = dtbegin.ToString("yyyy年M月") + "-" + dtend.ToString("yyyy年M月");
                daycompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            DataView dv = g_MessageTextService.GetMessageText(messagetype, daydate, dtbegin, dtend);
            if (dv.Count > 0)
            {
                taMessage.InnerHtml = dv[0]["messagetext"] != DBNull.Value ? dv[0]["messagetext"].ToString() : "";
                this.ViewState["state"] = "edit";
            }
            else
            {
                LoadingReport(messagetype);
                this.ViewState["state"] = "add";
                string messagetext = taMessage.InnerHtml;
                if (daydate.Date < daycompare)
                {
                    insert(messagetype, daydate, dtbegin, dtend, messagetext, daterange);
                }
            }
        }

        protected void dayDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            insertOrUpdate();
        }

        protected void dtpBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            dtpEnd.SelectedDate = new DateTime(dtpBegin.SelectedDate.Value.Year, dtpEnd.SelectedDate.Value.Month, dtpEnd.SelectedDate.Value.Day);
            insertOrUpdate();
        }

        protected void dtpEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            dtpBegin.SelectedDate = new DateTime(dtpEnd.SelectedDate.Value.Year, dtpBegin.SelectedDate.Value.Month, dtpBegin.SelectedDate.Value.Day);
            insertOrUpdate();
        }

        protected void seasonBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            seasonEnd.SelectedDate = new DateTime(seasonBegin.SelectedDate.Value.Year, seasonEnd.SelectedDate.Value.Month, seasonEnd.SelectedDate.Value.Day);
            insertOrUpdate();
        }

        protected void seasonEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            seasonBegin.SelectedDate = new DateTime(seasonEnd.SelectedDate.Value.Year, seasonBegin.SelectedDate.Value.Month, seasonBegin.SelectedDate.Value.Day);
            insertOrUpdate();
        }

        protected void ddlAuditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ViewState["state"].ToString() == "add")
            {
                LoadingReport(this.ViewState["messagetype"].ToString());
            }
            else
            {
                string temptext = taMessage.InnerHtml;
                if (!temptext.Contains("审核：" + ddlAuditor.SelectedItem.Text))
                {
                    int leng = temptext.IndexOf("审核：");
                    temptext = temptext.Substring(0, leng);
                    taMessage.InnerHtml = temptext + "审核：" + ddlAuditor.SelectedItem.Text + "。【苏州环境监测】";
                }

            }
        }

        protected void gridMessage_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            string messagetype = this.ViewState["messagetype"].ToString();
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;
            if (messagetype == "day")
            {
                dtbegin = new DateTime(daybegin.SelectedDate.Value.Year, daybegin.SelectedDate.Value.Month, daybegin.SelectedDate.Value.Day);
                dtend = new DateTime(dayend.SelectedDate.Value.Year, dayend.SelectedDate.Value.Month, dayend.SelectedDate.Value.Day);
                dtend = dtend.AddDays(+1).AddMilliseconds(-1);
            }
            else if (messagetype == "week")
            {
                dtbegin = new DateTime(weekbegin.SelectedDate.Value.Year, weekbegin.SelectedDate.Value.Month, weekbegin.SelectedDate.Value.Day);
                dtend = new DateTime(weekend.SelectedDate.Value.Year, weekend.SelectedDate.Value.Month, weekend.SelectedDate.Value.Day);
                dtend = dtend.AddDays(+1).AddMilliseconds(-1);
            }
            else
            {
                dtbegin = new DateTime(monthbegin.SelectedDate.Value.Year, monthbegin.SelectedDate.Value.Month, 1);
                dtend = new DateTime(monthend.SelectedDate.Value.Year, monthend.SelectedDate.Value.Month, 1);
                dtend = dtend.AddMonths(+1).AddSeconds(-1);
            }
            var dvlist = g_MessageTextService.GetMessageTextList(messagetype, dtbegin, dtend);
            gridMessage.DataSource = dvlist;
            gridMessage.VirtualItemCount = dvlist.Count;
        }

        protected void gridMessage_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridMessage.Rebind();
        }

        protected void btQi_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://www.1183300.com/index.htm");
            // System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://www.1183300.com/index.htm");
            //string.Format("<a href='#' onclick='RowClick()'>{0}</a>", btQi.Text);
        }

        protected void DeleteAudit_Click(object sender, EventArgs e)
        {
            string messagetype = this.ViewState["messagetype"].ToString();
            LoadingReport(messagetype);

            DateTime daydate = DateTime.Now;
            DateTime dtbegin = DateTime.Now;
            DateTime dtend = DateTime.Now;
            string daterange = "";
            if (messagetype == "day")
            {
                daydate = new DateTime(dayDate.SelectedDate.Value.Year, dayDate.SelectedDate.Value.Month, dayDate.SelectedDate.Value.Day);
                dtbegin = daydate;
                dtend = daydate;
                daterange = daydate.ToString("yyyy年M月d日");
            }
            else if (messagetype == "week")
            {
                daydate = new DateTime(dtpEnd.SelectedDate.Value.Year, dtpEnd.SelectedDate.Value.Month, dtpEnd.SelectedDate.Value.Day);
                dtbegin = new DateTime(dtpBegin.SelectedDate.Value.Year, dtpBegin.SelectedDate.Value.Month, dtpBegin.SelectedDate.Value.Day);
                dtend = new DateTime(dtpEnd.SelectedDate.Value.Year, dtpEnd.SelectedDate.Value.Month, dtpEnd.SelectedDate.Value.Day);
                daterange = dtbegin.ToString("yyyy年M月d日") + "-" + dtend.ToString("yyyy年M月d日");
            }
            else
            {
                daydate = new DateTime(seasonEnd.SelectedDate.Value.Year, seasonEnd.SelectedDate.Value.Month, 1);
                dtbegin = new DateTime(seasonBegin.SelectedDate.Value.Year, seasonBegin.SelectedDate.Value.Month, 1);
                dtend = new DateTime(seasonEnd.SelectedDate.Value.Year, seasonEnd.SelectedDate.Value.Month, 1);
                dtend = dtend.AddMonths(+1).AddSeconds(-1);
                daterange = dtbegin.ToString("yyyy年M月") + "-" + dtend.ToString("yyyy年M月");
            }
            string messagetext = taMessage.InnerHtml;
            this.ViewState["state"] = "edit";
            insert(messagetype, daydate, dtbegin, dtend, messagetext, daterange);
        }
    }
}