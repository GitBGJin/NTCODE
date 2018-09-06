using Aspose.Words;
using Aspose.Words.Tables;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Service.Frame;
using SmartEP.Service.ReportLibrary.Air;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    /// <summary>
    /// 名称：AirQualityWeekReport.aspx.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-01-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：苏州是市环境空气质量周报
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AirQualityWeekReport : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DictionaryService dicService = Singleton<DictionaryService>.GetInstance();
        DayAQIService m_DayAQIService = new DayAQIService();
        ReportLogService ReportLogService = new ReportLogService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ViewState["DisplayPerson"] = PageHelper.GetQueryString("DisplayPerson");
                IntiControl();
                LoadingReport();
            }
        }
        #region 初始化控件
        private void IntiControl()
        {
            DateTime firstDate = new DateTime(DateTime.Now.Year, 1, 1);
            dtpBegin.SelectedDate = firstDate;//初始化时间
            if (firstDate == DateTime.Now.Date)
            {
                dtpEnd.SelectedDate = DateTime.Now.Date;
            }
            else
            {
                dtpEnd.SelectedDate = DateTime.Now.Date.AddDays(-1);//初始化时间   
            }
        }
        #endregion
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadingReport();
        }
        #region 加载报表数据
        /// <summary>
        /// 加载报表数据
        /// </summary>
        private void LoadingReport()
        {
            string[] regionGuids = dicService.RetrieveCityList().Where(t => t.ItemText == "苏州市区").Select(t => t.ItemGuid).ToArray();
            DateTime dtBegin = new DateTime(dtpBegin.SelectedDate.Value.Year, dtpBegin.SelectedDate.Value.Month, dtpBegin.SelectedDate.Value.Day);
            DateTime dtEnd = new DateTime(dtpEnd.SelectedDate.Value.Year, dtpEnd.SelectedDate.Value.Month, dtpEnd.SelectedDate.Value.Day).AddDays(+1).AddSeconds(-1);
            TimeSpan ts = dtEnd - new DateTime(dtEnd.Year, 1, 1);
            int intcycle = 0;
            if ((ts.Days + 1) % 7 == 0)
            {
                intcycle = (int)((ts.Days + 1) / 7);
            }
            else
            {
                intcycle = (int)((ts.Days + 1) / 7) + 1;
            }
            DataTable dt = m_DayAQIService.GetRegionsTable(regionGuids, dtBegin, dtEnd);
            string thisValue = dt.Rows[0][dtEnd.Year.ToString()] != DBNull.Value ? (dt.Rows[0][dtEnd.Year.ToString()].ToString()) : "--";
            string lastValue = dt.Rows[0][dtEnd.AddYears(-1).Year.ToString()] != DBNull.Value ? (dt.Rows[0][dtEnd.AddYears(-1).Year.ToString()].ToString()) : "--";
            string yanzhongValue = dt.Rows[0]["YanzhongDays"] != DBNull.Value ? (dt.Rows[0]["YanzhongDays"].ToString()) : "--";
            string lastyanzValue = dt.Rows[0]["lastYanzDays"] != DBNull.Value ? (dt.Rows[0]["lastYanzDays"].ToString()) : "--";
            string lastText = "";
            string yanzhongText = "";
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
            if (yanzhongValue != "--" && lastyanzValue != "--")
            {
                if (decimal.Parse(yanzhongValue) > decimal.Parse(lastyanzValue))
                {
                    yanzhongText = "苏州市区出现重度污染以上天数" + yanzhongValue + "天，与去年同期相比上升" + Math.Round((decimal.Parse(yanzhongValue) - decimal.Parse(lastyanzValue)), 0).ToString() + "天。";
                }
                else if (decimal.Parse(yanzhongValue) < decimal.Parse(lastyanzValue))
                {
                    yanzhongText = "苏州市区出现重度污染以上天数" + yanzhongValue + "天，与去年同期相比下降" + Math.Round((decimal.Parse(lastyanzValue) - decimal.Parse(yanzhongValue)), 0).ToString() + "天。";
                }
                else
                {
                    yanzhongText = "苏州市区出现重度污染以上天数" + yanzhongValue + "天，与去年同期相比持平。";
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
            string lastStandRate = dt.Rows[0]["lastStandRate"] != DBNull.Value ? (dt.Rows[0]["lastStandRate"].ToString()) : "--";
            string thisStandRate = dt.Rows[0]["thisStandRate"] != DBNull.Value ? (dt.Rows[0]["thisStandRate"].ToString()) : "--";
            string text2013 = "";
            if (lastStandRate != "--" && thisStandRate != "--")
            {
                if (decimal.Parse(thisStandRate) > decimal.Parse(lastStandRate))
                {
                    text2013 = ",与去年同期（" + lastStandRate + "%)相比有所上升";
                }
                else if (decimal.Parse(thisStandRate) < decimal.Parse(lastStandRate))
                {
                    text2013 = ",与去年同期（" + lastStandRate + "%)相比有所下降";
                }
                else
                {
                    text2013 = ",与去年同期（" + lastStandRate + "%)相比持平";
                }
            }
            else
            {
                text2013 = ",去年同期达标天数比例为" + lastStandRate + "%";
            }
            string strcycle = "第" + intcycle + "期";
            string summaryText = "截止到" + dtEnd.ToString("yyyy年MM月dd日") + "，苏州市区PM2.5浓度均值为" + thisValue + "微克/立方米" + lastText + lastText2013 + "。苏州市区环境空气质量达标率为" + thisStandRate + "%" + text2013 + "。" + yanzhongText;
            string overdaystext = "截止到" + dtEnd.ToString("yyyy年MM月dd日") + "苏州市区共有" + dt.Rows[0]["invalidDays"].ToString() + "天无效天";
            InstanceReportSource instanceReportSource = new InstanceReportSource();
            AirQualityWeekReportService AirQualityWeekReport = new AirQualityWeekReportService();
            AirQualityWeekReport.ReportParameters.Add("regionGuids", Telerik.Reporting.ReportParameterType.String, string.Join(";", regionGuids));
            AirQualityWeekReport.ReportParameters.Add("beginTime", Telerik.Reporting.ReportParameterType.String, string.Join(";", dtBegin));
            AirQualityWeekReport.ReportParameters.Add("endTime", Telerik.Reporting.ReportParameterType.String, string.Join(";", dtEnd));
            AirQualityWeekReport.ReportParameters.Add("top", Telerik.Reporting.ReportParameterType.String, string.Join(";", "    " + summaryText));
            AirQualityWeekReport.ReportParameters.Add("subColumn1", Telerik.Reporting.ReportParameterType.String, string.Join(";", 2013));
            //AirQualityWeekReport.ReportParameters.Add("subColumn2", Telerik.Reporting.ReportParameterType.String, string.Join(";", dtEnd.AddYears(-2).Year));
            AirQualityWeekReport.ReportParameters.Add("subColumn3", Telerik.Reporting.ReportParameterType.String, string.Join(";", dtEnd.AddYears(-1).Year));
            AirQualityWeekReport.ReportParameters.Add("subColumn4", Telerik.Reporting.ReportParameterType.String, string.Join(";", dtEnd.Year));
            AirQualityWeekReport.ReportParameters.Add("subColumn5", Telerik.Reporting.ReportParameterType.String, string.Join(";", compare2013));
            AirQualityWeekReport.ReportParameters.Add("subColumn6", Telerik.Reporting.ReportParameterType.String, string.Join(";", "注: 根据数据有效性规定，" + overdaystext + "，不参与达标率统计。"));
            instanceReportSource.ReportDocument = AirQualityWeekReport;
            this.ReportViewer1.ReportSource = instanceReportSource;
            ReportViewer1.ShowPrintButton = false;
            ReportViewer1.ShowPrintPreviewButton = false;
            ReportViewer1.ShowExportGroup = false;
            RegisterScript("SetHeigth();");
        }
        #endregion
        public void Buildtable(DocumentBuilder builder, DataTable dt, DateTime dtStart)
        {
            int intX = 6;
            int intY = 2;

            string nullStr = "--";
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.LineWidth = 1;
            builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
            for (int y = 0; y < intY; y++)
            {
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                for (int x = 0; x < intX; x++)
                {

                    builder.InsertCell();
                    #region 表
                    if (y == 0)
                    {
                        builder.Font.ClearFormatting();
                        builder.Font.Size = 14;
                        builder.Font.Name = "宋体";
                        if (x == 0)
                        {
                            builder.Write("项目");
                        }
                        if (x >= 1 && x <= intX - 2)
                        {
                            builder.Write((dtStart.AddYears(-(intX - 2 - x)).Year) + "年");
                        }
                        if (x == intX - 1)
                        {
                            builder.Write("与2013年同期比较");
                        }
                        builder.Font.ClearFormatting();
                    }
                    if (y > 0)
                    {
                        builder.Font.ClearFormatting();
                        builder.Font.Size = 14;
                        builder.Font.Name = "宋体";
                        if (x == 0)
                        {
                            builder.Write("PM2.5");
                        }
                        if (x >= 1 && x <= intX - 2)
                        {
                            string lastStandRate = dt.Rows[0][(dtStart.AddYears(-(intX - 2 - x)).Year).ToString()] != DBNull.Value ? (dt.Rows[0][(dtStart.AddYears(-(intX - 2 - x)).Year).ToString()].ToString()) : nullStr;
                            builder.Write(lastStandRate);
                        }
                        if (x == intX - 1)
                        {
                            string thisValue2013 = dt.Rows[0][dtStart.Year.ToString()] != DBNull.Value ? (dt.Rows[0][dtStart.Year.ToString()].ToString()) : "--";
                            string lastValue2013 = dt.Rows[0]["2013"] != DBNull.Value ? (dt.Rows[0]["2013"].ToString()) : "--";
                            string lastText2013 = "";
                            if (lastValue2013 != "--" && thisValue2013 != "--")
                            {
                                lastText2013 = Math.Round((decimal.Parse(thisValue2013) - decimal.Parse(lastValue2013)) / decimal.Parse(lastValue2013) * 100, 1).ToString() + "%";
                            }
                            else
                            {
                                lastText2013 = nullStr;
                            }
                            builder.Write(lastText2013);
                        }
                    }
                    #endregion
                }
                builder.EndRow();
            }
            builder.EndTable();
        }
        protected void btnExport_Click(object sender, ImageClickEventArgs e)
        {
            string[] regionGuids = dicService.RetrieveCityList().Where(t => t.ItemText == "苏州市区").Select(t => t.ItemGuid).ToArray();
            DateTime dtBegin = new DateTime(dtpBegin.SelectedDate.Value.Year, dtpBegin.SelectedDate.Value.Month, dtpBegin.SelectedDate.Value.Day);
            DateTime dtEnd = new DateTime(dtpEnd.SelectedDate.Value.Year, dtpEnd.SelectedDate.Value.Month, dtpEnd.SelectedDate.Value.Day).AddDays(+1).AddSeconds(-1);
            TimeSpan ts = dtEnd - new DateTime(dtEnd.Year, 1, 1);
            int intcycle = 0;
            if ((ts.Days + 1) % 7 == 0)
            {
                intcycle = (int)((ts.Days + 1) / 7);
            }
            else
            {
                intcycle = (int)((ts.Days + 1) / 7) + 1;
            }
            DataTable dt = m_DayAQIService.GetRegionsTable(regionGuids, dtBegin, dtEnd);
            string thisValue = dt.Rows[0][dtEnd.Year.ToString()] != DBNull.Value ? (dt.Rows[0][dtEnd.Year.ToString()].ToString()) : "--";
            string lastValue = dt.Rows[0][dtEnd.AddYears(-1).Year.ToString()] != DBNull.Value ? (dt.Rows[0][dtEnd.AddYears(-1).Year.ToString()].ToString()) : "--";
            string yanzhongValue = dt.Rows[0]["YanzhongDays"] != DBNull.Value ? (dt.Rows[0]["YanzhongDays"].ToString()) : "--";
            string lastyanzValue = dt.Rows[0]["lastYanzDays"] != DBNull.Value ? (dt.Rows[0]["lastYanzDays"].ToString()) : "--";
            string lastText = "";
            string yanzhongText = "";
            if (lastValue != "--" && thisValue != "--")
            {
                if (decimal.Parse(thisValue) > decimal.Parse(lastValue))
                {
                    lastText = "上升了" + Math.Round((decimal.Parse(thisValue) - decimal.Parse(lastValue)) / decimal.Parse(lastValue) * 100, 1).ToString() + "%";
                }
                else if (decimal.Parse(thisValue) < decimal.Parse(lastValue))
                {
                    lastText = "下降了" + Math.Round((decimal.Parse(lastValue) - decimal.Parse(thisValue)) / decimal.Parse(lastValue) * 100, 1).ToString() + "%";
                }
                else
                {
                    lastText = "持平";
                }
            }
            if (yanzhongValue != "--" && lastyanzValue != "--")
            {
                if (decimal.Parse(yanzhongValue) > decimal.Parse(lastyanzValue))
                {
                    yanzhongText = "苏州市区出现重度污染以上天数" + yanzhongValue + "天，与去年同期相比上升" + Math.Round((decimal.Parse(yanzhongValue) - decimal.Parse(lastyanzValue)), 0).ToString() + "天。";
                }
                else if (decimal.Parse(yanzhongValue) < decimal.Parse(lastyanzValue))
                {
                    yanzhongText = "苏州市区出现重度污染以上天数" + yanzhongValue + "天，与去年同期相比下降" + Math.Round((decimal.Parse(lastyanzValue) - decimal.Parse(yanzhongValue)), 0).ToString() + "天。";
                }
                else
                {
                    yanzhongText = "苏州市区出现重度污染以上天数" + yanzhongValue + "天，与去年同期相比持平。";
                }
            }
            string thisValue2013 = dt.Rows[0][dtEnd.Year.ToString()] != DBNull.Value ? (dt.Rows[0][dtEnd.Year.ToString()].ToString()) : "--";
            string lastValue2013 = dt.Rows[0]["2013"] != DBNull.Value ? (dt.Rows[0]["2013"].ToString()) : "--";
            string lastText2013 = "";
            string compare2013 = "--";
            if (lastValue2013 != "--" && thisValue2013 != "--")
            {
                if (decimal.Parse(thisValue2013) > decimal.Parse(lastValue2013))
                {
                    lastText2013 = "上升了" + Math.Round((decimal.Parse(thisValue2013) - decimal.Parse(lastValue2013)) / decimal.Parse(lastValue2013) * 100, 1).ToString() + "%";
                    compare2013 = Math.Round((decimal.Parse(thisValue2013) - decimal.Parse(lastValue2013)) / decimal.Parse(lastValue2013) * 100, 1).ToString() + "%";
                }
                else if (decimal.Parse(thisValue2013) < decimal.Parse(lastValue2013))
                {
                    lastText2013 = "下降了" + Math.Round((decimal.Parse(lastValue2013) - decimal.Parse(thisValue2013)) / decimal.Parse(lastValue2013) * 100, 1).ToString() + "%";
                    compare2013 = Math.Round((decimal.Parse(thisValue2013) - decimal.Parse(lastValue2013)) / decimal.Parse(lastValue2013) * 100, 1).ToString() + "%";
                }
                else
                {
                    lastText2013 = "持平";
                    compare2013 = Math.Round((decimal.Parse(thisValue2013) - decimal.Parse(lastValue2013)) / decimal.Parse(lastValue2013) * 100, 1).ToString() + "%";
                }
            }
            string lastStandRate = dt.Rows[0]["lastStandRate"] != DBNull.Value ? (dt.Rows[0]["lastStandRate"].ToString()) : "--";
            string thisStandRate = dt.Rows[0]["thisStandRate"] != DBNull.Value ? (dt.Rows[0]["thisStandRate"].ToString()) : "--";
            string text2013 = "";
            if (lastStandRate != "--" && thisStandRate != "--")
            {
                if (decimal.Parse(thisStandRate) > decimal.Parse(lastStandRate))
                {
                    text2013 = "有所上升";
                }
                else if (decimal.Parse(thisStandRate) < decimal.Parse(lastStandRate))
                {
                    text2013 = "有所下降";
                }
                else
                {
                    text2013 = "相比持平";
                }
            }

            //string strcycle = intcycle.ToString();
            string strcycle = " ";
            string summaryText = "截止到" + dtEnd.ToString("yyyy年MM月dd日") + "，苏州市区PM2.5浓度均值为" + thisValue + "微克/立方米" + lastText + lastText2013 + "。苏州市区环境空气质量达标率为" + thisStandRate + "%" + text2013 + "。" + yanzhongText;
            string overdaystext = "截止到" + dtEnd.ToString("yyyy年MM月dd日") + "苏州市区共有" + dt.Rows[0]["invalidDays"].ToString() + "天无效天";
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AirQualityWeekReport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToMergeField("cycle");
            builder.Write(strcycle);

            builder.MoveToMergeField("summarytime");
            builder.Write(dtEnd.ToString("yyyy年MM月dd日"));

            builder.MoveToMergeField("summary1");
            builder.Write(thisValue);

            builder.MoveToMergeField("summary2");
            builder.Write(lastText);

            builder.MoveToMergeField("summary3");
            builder.Write(lastText2013);

            builder.MoveToMergeField("summary4");
            builder.Write(thisStandRate + "%");

            builder.MoveToMergeField("summary5");
            builder.Write(lastStandRate + "%");

            builder.MoveToMergeField("summary6");
            builder.Write(text2013);

            builder.MoveToMergeField("summary7");
            builder.Write(yanzhongText);

            builder.MoveToMergeField("time1");
            builder.Write((dtEnd.Year - 1).ToString());

            builder.MoveToMergeField("time2");
            builder.Write((dtEnd.Year).ToString());

            builder.MoveToMergeField("value1");
            builder.Write(dt.Rows[0]["2013"].ToString());

            builder.MoveToMergeField("value2");
            builder.Write(dt.Rows[0][(dtEnd.Year - 1).ToString()].ToString());

            builder.MoveToMergeField("value3");
            builder.Write(dt.Rows[0][(dtEnd.Year).ToString()].ToString());

            builder.MoveToMergeField("value4");
            builder.Write(compare2013);

            //builder.MoveToMergeField("table");
            //Buildtable(builder, dt, dtEnd);

            builder.MoveToMergeField("days");
            builder.Write(dt.Rows[0]["invalidDays"].ToString());
            string shortDate = System.DateTime.Today.ToString("yyyy/MM/dd");
            builder.MoveToMergeField("createtime");
            builder.Write(date2chinese(shortDate));
            doc.MailMerge.DeleteFields();
            string filename = "(" + DateTime.Now.ToString("yyyyMMdd") + ")" + "AirQualityWeekReport" + ".doc";
            string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/AirQualityWeekReport/" + dtEnd.Year + "/" + dtEnd.Month + "/" + filename);
            doc.Save(strTarget);
            //doc.Save(this.Response, "（周报）苏州市空气质量周报第" + intcycle + "期" + DateTime.Now.Date.ToString("yyyyMMdd") + ".doc", ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));//+ Convert.ToDateTime(ViewState["FromDate"].ToString()).ToString("yyyyMMdd") + "-" + Convert.ToDateTime(ViewState["ToDate"].ToString()).ToString("yyyyMMdd")
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointNames = "苏州市区";//测点名称
            customDatum.FactorsNames = "PM2.5";//因子名称
            customDatum.DateTimeRange = dtEnd.Year + "年第" + intcycle + "期";
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "AirQualityWeekReport";//页面ID
            customDatum.StartDateTime = dtBegin;
            customDatum.EndDateTime = dtEnd;
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/AirQualityWeekReport/" + dtEnd.Year + "/" + dtEnd.Month + "/" + filename).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            customDatum.ExportName = dtEnd.Year + "年第" + intcycle + "期市区空气质量周报";
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            if (!Directory.Exists(strTarget))
            {


                Alert("保存成功！");
                //Directory.CreateDirectory(strTarget);//创建新路径
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
        }
        private string num2chinese(String s)
        {
            string[] chinese = new string[10] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            //将单个数字转成中文.
            int slen = s.Length;
            string result = "";
            for (int i = 0; i < slen; i++)
            {
                string xxs = Convert.ToString(s[i]);
                int jj = Convert.ToInt16(xxs);
                result += chinese[jj];
            }
            return result;
        }

        private string n2c(string s)
        {
            string[] chinese = new string[10] { "○", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            string[] len = new string[1] { "十" };
            string[] ydm = new string[3] { "年", "月", "日" };
            //对特殊情况进行处理.
            if (s.Length == 2)
            {
                if (s[0].Equals('1'))
                {
                    if (s[1].Equals('0')) return len[0];

                    string xxs = Convert.ToString(s[1]);
                    int jj = Convert.ToInt16(xxs);
                    return len[0] + chinese[jj];
                }
                if (s[0].Equals('0'))
                {
                    string xxs = Convert.ToString(s[1]);
                    int jj = Convert.ToInt16(xxs);
                    return chinese[jj];
                }
                if (s[1].Equals('0')) return chinese[int.Parse(s[0].ToString())] + len[0];

                string xxs0 = Convert.ToString(s[0]);
                int jj0 = Convert.ToInt16(xxs0);

                string xxs1 = Convert.ToString(s[1]);
                int jj1 = Convert.ToInt16(xxs1);
                return chinese[jj0] + len[0] + chinese[jj1];
            }
            return num2chinese(s);
        }

        public string date2chinese(string s)
        {
            string REGEXP_IS_VALID_DATE = @"(\d{2}|\d{4})(\/|-)(\d{1,2})(\2)(\d{1,2})";
            string[] ydm = new string[3] { "年", "月", "日" };
            Match ma = Regex.Match(s, REGEXP_IS_VALID_DATE);
            System.Text.RegularExpressions.GroupCollection gc = ma.Groups;

            string ok = "";
            int count = gc.Count;

            for (int i = 1; i < count; i = i + 2)
            {
                ok += n2c(gc[i].Value) + ydm[(i - 1) / 2];
            }
            return ok;
        }
        protected void dtpEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            dtpBegin.SelectedDate = new DateTime(dtpEnd.SelectedDate.Value.Year, dtpBegin.SelectedDate.Value.Month, dtpBegin.SelectedDate.Value.Day);
        }

        protected void dtpBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            dtpEnd.SelectedDate = new DateTime(dtpBegin.SelectedDate.Value.Year, dtpEnd.SelectedDate.Value.Month, dtpEnd.SelectedDate.Value.Day);
        }
    }
}