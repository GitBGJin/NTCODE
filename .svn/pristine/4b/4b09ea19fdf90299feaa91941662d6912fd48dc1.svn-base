using Aspose.Words;
using Aspose.Words.Tables;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Service.DataAnalyze.Water;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AirQualittySeasonReport : SmartEP.WebUI.Common.BasePage
    {
        /// 数据处理服务
        /// </summary>
        private YearAQIService m_YearAQIService = new YearAQIService();
        ReportLogService ReportLogService = new ReportLogService();
        DataQueryByDayService m_DataQueryByDayService = Singleton<DataQueryByDayService>.GetInstance();
        ReportContentService m_ReportContentService = new ReportContentService();
        DateTime dt = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                this.ViewState["DisplayPerson"] = PageHelper.GetQueryString("DisplayPerson");
                InitControl();
            }
        }

        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            Year.Items.Clear();//年开始
            int yearNow = DateTime.Now.Year;
            int years = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Year"]);
            for (int i = yearNow; i >= years; i--)
            {
                //年开始
                RadComboBoxItem cmbItemYearBegin = new RadComboBoxItem();
                cmbItemYearBegin.Text = i.ToString();
                cmbItemYearBegin.Value = i.ToString();
                if (i == yearNow - 1)
                    cmbItemYearBegin.Checked = true;
                Year.Items.Add(cmbItemYearBegin);
            }
            Year.DataBind();

            Season.Items.Clear();//年开始
            int season = 0;
            decimal yu = 0;
            if (Convert.ToDecimal(Year.SelectedValue) < DateTime.Now.Year)
                season = 4;
            else if (Convert.ToDecimal(Year.SelectedValue) == DateTime.Now.Year)
            {
                season = Convert.ToInt32(DateTime.Now.Month) / 4;
                yu = Convert.ToDecimal(DateTime.Now.Month) % 4;
                if (yu > 0)
                    season += 1;
            }
            for (int i = 1; i <= season; i++)
            {
                //年开始
                RadComboBoxItem cmbItemYearBegin = new RadComboBoxItem();
                cmbItemYearBegin.Text = "第" + i.ToString() + "季";
                cmbItemYearBegin.Value = i.ToString();
                if (i == season)
                    cmbItemYearBegin.Checked = true;
                Season.Items.Add(cmbItemYearBegin);
            }
            Season.DataBind();

            YearBegin.Items.Clear();//年开始
            BindType();

            if (YearBegin.Items.Count > 0)
                YearBegin.Items[0].Checked = true;

            DateTime dtB = DateTime.Now;
            string MonthB = "";
            string MonthE = "";
            if (Season.SelectedValue == "1")
            {
                MonthB = "-01-01";
                MonthE = "-03-31";
            }
            else if (Season.SelectedValue == "2")
            {
                MonthB = "-04-01";
                MonthE = "-06-30";
            }
            else if (Season.SelectedValue == "3")
            {
                MonthB = "-07-01";
                MonthE = "-09-30";
            }
            else if (Season.SelectedValue == "4")
            {
                MonthB = "-10-01";
                MonthE = "-12-31";
            }
            dtB = Convert.ToDateTime(Year.SelectedValue + MonthB);
            System.DateTime dtE = Convert.ToDateTime(Year.SelectedValue + MonthE);
            if (dtE > System.DateTime.Now)
                dtE = System.DateTime.Now;
            txtDateF.Text = dtB.ToString("yyyy年MM月dd日");
            txtDateT.Text = dtE.ToString("yyyy年MM月dd日");
            Bind();
        }
        #endregion
        #region 绑定基数类型
        public void BindType()
        {
            DataTable dvType = m_DataQueryByDayService.GetCheckRegionDataType();
            YearBegin.DataSource = dvType;
            YearBegin.DataTextField = "DataType";
            YearBegin.DataValueField = "DataType";
            YearBegin.DataBind();
        }
        #endregion
        #region 绑定数据
        public void Bind()
        {
            DataTable dtnew = new DataTable();

            DateTime dtB = DateTime.Now;
            string MonthB = "";
            string MonthE = "";
            if (Season.SelectedValue == "1")
            {
                MonthB = "-01-01";
                MonthE = "-03-31";
            }
            else if (Season.SelectedValue == "2")
            {
                MonthB = "-04-01";
                MonthE = "-06-30";
            }
            else if (Season.SelectedValue == "3")
            {
                MonthB = "-07-01";
                MonthE = "-09-30";
            }
            else if (Season.SelectedValue == "4")
            {
                MonthB = "-10-01";
                MonthE = "-12-31";
            }
            DateTime dtBegin = Convert.ToDateTime(Year.SelectedValue + MonthB);

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(Year.SelectedValue + MonthE);
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;

            var dataViewM = new DataView();
            //string year = YearBegin.SelectedValue;
            string year = "2013";
            dataViewM = m_YearAQIService.GetRegionsSeasonAllData(dtBegin, endDate, year);
            dtnew = dataViewM.ToTable();

            string yearStr = "";
            if (Season.SelectedValue == "1")
                yearStr = dtBegin.ToString("yyyy") + "年一季度";
            else if (Season.SelectedValue == "2")
                yearStr = dtBegin.ToString("yyyy") + "年二季度";
            else if (Season.SelectedValue == "3")
                yearStr = dtBegin.ToString("yyyy") + "年三季度";
            else if (Season.SelectedValue == "4")
                yearStr = dtBegin.ToString("yyyy") + "年四季度";
            Label1.InnerText = (yearStr + "苏州市区环境空气质量分析");
            string m1 = "";
            string m2 = "";
            string m3 = "";
            string m4 = "";
            string m5 = "";
            if (dtnew.Rows.Count > 0)
            {
                if (dtnew.Rows[0]["M24"].ToString() != "")
                {
                    m1 = yearStr + "，苏州市区环境空气质量指数介于" + dtnew.Rows[0]["M1"].ToString() + dtnew.Rows[0]["M2"].ToString()
                    + dtnew.Rows[0]["M21"].ToString() + dtnew.Rows[0]["M22"].ToString() + dtnew.Rows[0]["M21"].ToString()
                    + dtnew.Rows[0]["M23"].ToString() + dtnew.Rows[0]["M21"].ToString() + dtnew.Rows[0]["M3"].ToString()
                    + dtnew.Rows[0]["M24"].ToString() + dtnew.Rows[0]["M21"].ToString() + dtnew.Rows[0]["M4"].ToString();
                }
                else
                    m1 = yearStr + "，苏州市区环境空气质量指数介于" + dtnew.Rows[0]["M1"].ToString() + dtnew.Rows[0]["M2"].ToString()
                   + dtnew.Rows[0]["M21"].ToString() + dtnew.Rows[0]["M22"].ToString() + dtnew.Rows[0]["M21"].ToString()
                   + dtnew.Rows[0]["M23"].ToString() + dtnew.Rows[0]["M21"].ToString() + dtnew.Rows[0]["M3"].ToString()
                   + dtnew.Rows[0]["M24"].ToString() + dtnew.Rows[0]["M4"].ToString();
                M1.Text = m1;

                m2 = yearStr + dtnew.Rows[0]["M5"].ToString() + dtnew.Rows[0]["M6"].ToString();
                M2.Text = m2;

                m3 = "从污染持续情况来看，" + yearStr + dtnew.Rows[0]["M7"].ToString();
                M3.Text = m3;

                m4 = yearStr + dtnew.Rows[0]["M25"].ToString() + dtnew.Rows[0]["M21"].ToString() + dtnew.Rows[0]["M8"].ToString() + dtnew.Rows[0]["M9"].ToString()
                    + "空气污染现象频发，中度/重度污染多发高发，存在典型的区域性污染特征。";
                M4.Text = m4;

                m5 = "除了本地和区域大气污染物排放强度大以外，我市受静稳气象条件影响较大，大气层结稳定，不利于污染物扩散。从长三角区域总体情况来看，我市环境空气污染发生情况与无锡、上海接近。";
                M5.Text = m5;
            }
        }
        #endregion
        #region 服务器端控件事件处理
        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dtnew = new DataTable();

            DateTime dtB = DateTime.Now;
            string MonthB = "";
            string MonthE = "";
            if (Season.SelectedValue == "1")
            {
                MonthB = "-01-01";
                MonthE = "-03-31";
            }
            else if (Season.SelectedValue == "2")
            {
                MonthB = "-04-01";
                MonthE = "-06-30";
            }
            else if (Season.SelectedValue == "3")
            {
                MonthB = "-07-01";
                MonthE = "-09-30";
            }
            else if (Season.SelectedValue == "4")
            {
                MonthB = "-10-01";
                MonthE = "-12-31";
            }

            DateTime dtBegin = Convert.ToDateTime(Year.SelectedValue + MonthB);

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(Year.SelectedValue + MonthE);
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;

            var dataViewM = new DataView();
            //string year = YearBegin.SelectedValue;
            string year = "2013";
            dataViewM = m_YearAQIService.GetRegionsSeasonAllData(dtBegin, endDate, year);
            dtnew = dataViewM.ToTable();
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AirQualittySeasonReport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);

            string yearStr = "";
            if (Season.SelectedValue == "1")
                yearStr = dtBegin.ToString("yyyy") + "年一季度";
            else if (Season.SelectedValue == "2")
                yearStr = dtBegin.ToString("yyyy") + "年二季度";
            else if (Season.SelectedValue == "3")
                yearStr = dtBegin.ToString("yyyy") + "年三季度";
            else if (Season.SelectedValue == "4")
                yearStr = dtBegin.ToString("yyyy") + "年四季度";
            builder.MoveToMergeField("Title");
            builder.Write(yearStr + "苏州市区环境空气质量分析");
            string m1 = "";
            string m2 = "";
            string m3 = "";
            string m4 = "";
            string m5 = "";
            if (dtnew.Rows.Count > 0)
            {
                m1 = yearStr + "，苏州市区环境空气质量指数介于" + dtnew.Rows[0]["M1"].ToString() + dtnew.Rows[0]["M2"].ToString();
                builder.MoveToMergeField("M1");
                builder.Write(m1);
                builder.MoveToMergeField("M12");
                builder.Write(dtnew.Rows[0]["M22"].ToString());
                builder.MoveToMergeField("M14");
                builder.Write(dtnew.Rows[0]["M23"].ToString());
                builder.MoveToMergeField("M16");
                builder.Write(dtnew.Rows[0]["M3"].ToString() + dtnew.Rows[0]["M24"].ToString());
                builder.MoveToMergeField("M18");
                builder.Write(dtnew.Rows[0]["M4"].ToString());

                builder.MoveToMergeField("M11");
                builder.Write(dtnew.Rows[0]["M21"].ToString());
                builder.MoveToMergeField("M13");
                builder.Write(dtnew.Rows[0]["M21"].ToString());
                builder.MoveToMergeField("M15");
                builder.Write(dtnew.Rows[0]["M21"].ToString());

                builder.MoveToMergeField("M17");
                if (dtnew.Rows[0]["M24"].ToString() != "")
                {
                    builder.Write(dtnew.Rows[0]["M21"].ToString());
                }
                else
                    builder.Write("");
                m2 = yearStr + dtnew.Rows[0]["M5"].ToString() + dtnew.Rows[0]["M6"].ToString();
                builder.MoveToMergeField("M2");
                builder.Write(m2);

                m3 = "从污染持续情况来看，" + yearStr + dtnew.Rows[0]["M7"].ToString();
                builder.MoveToMergeField("M3");
                builder.Write(m3);

                builder.MoveToMergeField("M4");
                builder.Write(yearStr + dtnew.Rows[0]["M25"].ToString());
                builder.MoveToMergeField("M41");
                builder.Write(dtnew.Rows[0]["M21"].ToString());
                m4 = dtnew.Rows[0]["M8"].ToString() + dtnew.Rows[0]["M9"].ToString()
                 + "空气污染现象频发，中度/重度污染多发高发，存在典型的区域性污染特征。";
                builder.MoveToMergeField("M42");
                builder.Write(m4);

                m5 = "除了本地和区域大气污染物排放强度大以外，我市受静稳气象条件影响较大，大气层结稳定，不利于污染物扩散。从长三角区域总体情况来看，我市环境空气污染发生情况与无锡、上海接近。";
                builder.MoveToMergeField("M5");
                builder.Write(m5);
                builder.MoveToMergeField("Time");
                builder.Write(DateTime.Now.ToString("yyyy年MM月dd日"));

                builder.MoveToMergeField("Table");
                builder.Font.ClearFormatting();
                DataTable dtTable = m_YearAQIService.GetContinuousDaysTable(dtBegin, endDate).Table;
                MoveToTable(builder, dtTable);

                doc.MailMerge.DeleteFields();
                string[] strPointCodes = { "" };
                string pointCodes = string.Join(";", strPointCodes);
                string[] strPointNames = { "" };
                string pointNames = string.Join(";", strPointNames);
                //添加实体类对象
                string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "AirQualittySeasonReport" + ".doc";
                ReportLogEntity customDatum = new ReportLogEntity();
                customDatum.PointIds = pointCodes;//测点Code
                customDatum.PointNames = pointNames;//测点名称
                customDatum.FactorCodes = "";//因子Code
                customDatum.FactorsNames = dtnew.Rows[0]["M10"].ToString();//因子名称
                customDatum.DateTimeRange = dtBegin.Year.ToString() + "年" + dtBegin.Month.ToString() + "月";
                customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
                customDatum.PageTypeID = "AirQualittySeasonReport";//页面ID
                customDatum.StartDateTime = dtBegin;
                customDatum.EndDateTime = endDate;
                customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
                customDatum.ExportName = yearStr + "环境空气质量分析" + string.Format("{0:yyyyMMdd}", DateTime.Now);
                customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/AirQualittySeasonReport/" + dtBegin.Year + "/" + dtBegin.Month + "/" + filename).ToString();
                customDatum.CreatDateTime = DateTime.Now;
                //添加数据
                ReportLogService.ReportLogAdd(customDatum);

                string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/AirQualittySeasonReport/" + dtBegin.Year + "/" + dtBegin.Month + "/" + filename);
                doc.Save(strTarget);
                doc.Save(this.Response, "AirQualittySeasonReport.doc", Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Docx));
                Response.End();
            }
        }
        #endregion

        /// <summary>
        /// 填入子站月报表数据
        /// </summary>
        /// <param name="dt"></param>
        private void MoveToTable(DocumentBuilder builder, DataTable dtNew)
        {
            builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Left;
            builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;

            //因子编码
            string factor = System.Configuration.ConfigurationManager.AppSettings["AirPollutantName"];
            string[] factorNames = factor.Trim(';').Split(';');
            //站点Id


            for (int i = 0; i < 1 + dtNew.Rows.Count; i++)
            {
                //string name = "";
                //if (i != 0)
                //    name = dt.Rows[i - 1]["DateTime"].ToString();


                for (int j = 0; j < 6; j++)
                {
                    builder.InsertCell();
                    builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                    builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.CellFormat.VerticalMerge = CellMerge.None;
                    // builder.CellFormat.Width = 100;

                    if (i == 0)
                    {

                        if (j == 0)
                        {
                            builder.CellFormat.VerticalMerge = CellMerge.First;
                            builder.Write("日期");
                            //  builder.CellFormat.Width = 150;
                        }
                        else if (j == 1)
                        {
                            builder.Write("持续天数");
                        }
                        else if (j == 2)
                        {
                            builder.Write("轻度污染");
                        }
                        else if (j == 3)
                        {
                            builder.Write("中度污染");
                        }
                        else if (j == 4)
                        {
                            builder.Write("重度污染");
                        }
                        else if (j == 5)
                        {
                            builder.Write("严重污染");
                        }
                    }
                    else if (i > 0)
                    {
                        if (j == 0)
                        {
                            builder.Write(dtNew.Rows[i - 1]["DateTime"].ToString());
                        }
                        else
                        {
                            string value = "";
                            value = dtNew.Rows[i - 1][j].ToString();
                            builder.Write(value == "" ? "--" : value);
                        }
                    }
                }
                builder.EndRow();
            }
            builder.EndTable();
        }
        /// <summary>
        /// 保存按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            //if (dtpBegin.SelectedDate == null)
            //{
            //    return;
            //}
            DataTable dtnew = new DataTable();

            DateTime dtB = DateTime.Now;
            string MonthB = "";
            string MonthE = "";
            if (Season.SelectedValue == "1")
            {
                MonthB = "-01-01";
                MonthE = "-03-31";
            }
            else if (Season.SelectedValue == "2")
            {
                MonthB = "-04-01";
                MonthE = "-06-30";
            }
            else if (Season.SelectedValue == "3")
            {
                MonthB = "-07-01";
                MonthE = "-09-30";
            }
            else if (Season.SelectedValue == "4")
            {
                MonthB = "-10-01";
                MonthE = "-12-31";
            }

            DateTime dtBegin = Convert.ToDateTime(Year.SelectedValue + MonthB);

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(Year.SelectedValue + MonthE);
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            //string year = YearBegin.SelectedValue;
            string year = "2013";
            var dataViewM = new DataView();
            dataViewM = m_YearAQIService.GetRegionsSeasonAllData(dtBegin, endDate, year);
            dtnew = dataViewM.ToTable();
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AirQualittySeasonReport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);

            string yearStr = "";
            if (Season.SelectedValue == "1")
                yearStr = dtBegin.ToString("yyyy") + "年一季度";
            else if (Season.SelectedValue == "2")
                yearStr = dtBegin.ToString("yyyy") + "年二季度";
            else if (Season.SelectedValue == "3")
                yearStr = dtBegin.ToString("yyyy") + "年三季度";
            else if (Season.SelectedValue == "4")
                yearStr = dtBegin.ToString("yyyy") + "年四季度";
            builder.MoveToMergeField("Title");
            builder.Write(yearStr + "苏州市区环境空气质量分析");
            string m1 = "";
            string m2 = "";
            string m3 = "";
            string m4 = "";
            string m5 = "";
            if (dtnew.Rows.Count > 0)
            {
                m1 = yearStr + "，苏州市区环境空气质量指数介于" + dtnew.Rows[0]["M1"].ToString() + dtnew.Rows[0]["M2"].ToString();
                builder.MoveToMergeField("M1");
                builder.Write(m1);
                builder.MoveToMergeField("M12");
                builder.Write(dtnew.Rows[0]["M22"].ToString());
                builder.MoveToMergeField("M14");
                builder.Write(dtnew.Rows[0]["M23"].ToString());
                builder.MoveToMergeField("M16");
                builder.Write(dtnew.Rows[0]["M3"].ToString() + dtnew.Rows[0]["M24"].ToString());
                builder.MoveToMergeField("M18");
                builder.Write(dtnew.Rows[0]["M4"].ToString());

                builder.MoveToMergeField("M11");
                builder.Write(dtnew.Rows[0]["M21"].ToString());
                builder.MoveToMergeField("M13");
                builder.Write(dtnew.Rows[0]["M21"].ToString());
                builder.MoveToMergeField("M15");
                builder.Write(dtnew.Rows[0]["M21"].ToString());
                builder.MoveToMergeField("M17");
                if (dtnew.Rows[0]["M24"].ToString() != "")
                {
                    builder.Write(dtnew.Rows[0]["M21"].ToString());
                }
                else
                    builder.Write("");

                builder.MoveToMergeField("M41");
                builder.Write(dtnew.Rows[0]["M21"].ToString());

                m2 = yearStr + dtnew.Rows[0]["M5"].ToString() + dtnew.Rows[0]["M6"].ToString();
                builder.MoveToMergeField("M2");
                builder.Write(m2);

                m3 = "从污染持续情况来看，" + yearStr + dtnew.Rows[0]["M7"].ToString();
                builder.MoveToMergeField("M3");
                builder.Write(m3);

                builder.MoveToMergeField("M4");
                builder.Write(yearStr + dtnew.Rows[0]["M25"].ToString());

                m4 = dtnew.Rows[0]["M8"].ToString() + dtnew.Rows[0]["M9"].ToString()
                 + "空气污染现象频发，中度/重度污染多发高发，存在典型的区域性污染特征。";
                builder.MoveToMergeField("M42");
                builder.Write(m4);

                m5 = "除了本地和区域大气污染物排放强度大以外，我市受静稳气象条件影响较大，大气层结稳定，不利于污染物扩散。从长三角区域总体情况来看，我市环境空气污染发生情况与无锡、上海接近。";
                builder.MoveToMergeField("M5");
                builder.Write(m5);
                builder.MoveToMergeField("Time");
                builder.Write(DateTime.Now.ToString("yyyy年MM月dd日"));

                builder.MoveToMergeField("Table");
                builder.Font.ClearFormatting();
                DataTable dtTable = m_YearAQIService.GetContinuousDaysTable(dtBegin, endDate).Table;
                MoveToTable(builder, dtTable);

                doc.MailMerge.DeleteFields();
                string[] strPointCodes = { };//站点Code
                string pointCodes = string.Join(";", strPointCodes);
                string[] strPointNames = { };//站点名称
                string pointNames = string.Join(";", strPointNames);
                string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "AirQualittySeasonReport" + ".doc";
                //添加实体类对象
                ReportLogEntity customDatum = new ReportLogEntity();
                customDatum.PointIds = "7e05b94c-bbd4-45c3-919c-42da2e63fd43;";//测点Code
                customDatum.PointNames = "苏州市区";//测点名称
                customDatum.FactorCodes = "";//因子Code
                customDatum.FactorsNames = dtnew.Rows[0]["M10"].ToString();//因子名称
                customDatum.DateTimeRange = dtBegin.Year.ToString() + "年" + dtBegin.Month.ToString() + "月";
                customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
                customDatum.PageTypeID = "AirQualittySeasonReport";//页面ID
                customDatum.StartDateTime = dtBegin;
                customDatum.EndDateTime = endDate;
                customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
                customDatum.ExportName = yearStr + "环境空气质量分析" + string.Format("{0:yyyyMMdd}", DateTime.Now);
                customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/AirQualittySeasonReport/" + dtBegin.Year + "/" + dtBegin.Month + "/" + filename).ToString();
                customDatum.CreatDateTime = DateTime.Now;
                //添加数据
                ReportLogService.ReportLogAdd(customDatum);

                string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/AirQualittySeasonReport/" + dtBegin.Year + "/" + dtBegin.Month + "/" + filename);
                doc.Save(strTarget);
                //Response.End();

                string pageid = "AirBulletin";
                Dictionary<string, string> pcontent = new Dictionary<string, string>();
                List<string> r = new List<string>();
                if (M1.Text != "")
                {
                    r.Add("1");
                    pcontent.Add("1", M1.Text);
                }
                if (M2.Text != "")
                {
                    r.Add("2");
                    pcontent.Add("2", M2.Text);
                }
                if (M3.Text != "")
                {
                    r.Add("3");
                    pcontent.Add("3", M3.Text);
                }
                if (M4.Text != "")
                {
                    r.Add("4");
                    pcontent.Add("4", M4.Text);
                }
                if (M5.Text != "")
                {
                    r.Add("5");
                    pcontent.Add("5", M5.Text);
                }

                string[] ptitle = r.ToArray();

                m_ReportContentService.insertTable(pcontent, ptitle, pageid, dtBegin, endDate);
                if (!Directory.Exists(strTarget))
                {
                    Alert("保存成功！");
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);

            }

        }

        protected void Year_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Season.Items.Clear();//年开始
            int season = 0;
            decimal yu = 0;
            if (Convert.ToDecimal(Year.SelectedValue) < DateTime.Now.Year)
                season = 4;
            else if (Convert.ToDecimal(Year.SelectedValue) == DateTime.Now.Year)
            {
                season = Convert.ToInt32(DateTime.Now.Month) / 4;
                yu = Convert.ToDecimal(DateTime.Now.Month) % 4;
                if (yu > 0)
                    season += 1;
            }
            for (int i = 1; i <= season; i++)
            {
                //年开始
                RadComboBoxItem cmbItemYearBegin = new RadComboBoxItem();
                cmbItemYearBegin.Text = "第" + i.ToString() + "季";
                cmbItemYearBegin.Value = i.ToString();
                if (i == season)
                    cmbItemYearBegin.Checked = true;
                Season.Items.Add(cmbItemYearBegin);
            }
            Season.DataBind();

            DateTime dtB = DateTime.Now;
            string MonthB = "";
            string MonthE = "";
            if (Season.SelectedValue == "1")
            {
                MonthB = "-01-01";
                MonthE = "-03-31";
            }
            else if (Season.SelectedValue == "2")
            {
                MonthB = "-04-01";
                MonthE = "-06-30";
            }
            else if (Season.SelectedValue == "3")
            {
                MonthB = "-07-01";
                MonthE = "-09-30";
            }
            else if (Season.SelectedValue == "4")
            {
                MonthB = "-10-01";
                MonthE = "-12-31";
            }
            dtB = Convert.ToDateTime(Year.SelectedValue + MonthB);
            System.DateTime dtE = Convert.ToDateTime(Year.SelectedValue + MonthE);
            if (dtE > System.DateTime.Now)
                dtE = System.DateTime.Now;
            txtDateF.Text = dtB.ToString("yyyy年MM月dd日");
            txtDateT.Text = dtE.ToString("yyyy年MM月dd日");
            Bind();
            grdSituation.Rebind();
        }
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            DateTime dtB = DateTime.Now;
            string MonthB = "";
            string MonthE = "";
            if (Season.SelectedValue == "1")
            {
                MonthB = "-01-01";
                MonthE = "-03-31";
            }
            else if (Season.SelectedValue == "2")
            {
                MonthB = "-04-01";
                MonthE = "-06-30";
            }
            else if (Season.SelectedValue == "3")
            {
                MonthB = "-07-01";
                MonthE = "-09-30";
            }
            else if (Season.SelectedValue == "4")
            {
                MonthB = "-10-01";
                MonthE = "-12-31";
            }

            DateTime dtBegin = Convert.ToDateTime(Year.SelectedValue + MonthB);
            System.DateTime endDate = Convert.ToDateTime(Year.SelectedValue + MonthE);
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;

            var analyzeDate = new DataView();

            analyzeDate = m_YearAQIService.GetContinuousDaysTable(dtBegin, endDate);

            if (analyzeDate != null)
            {
                grdSituation.DataSource = analyzeDate;//dataView;
                grdSituation.VirtualItemCount = analyzeDate.Count;
            }
            else
            {
                grdSituation.DataSource = new DataTable();
            }
        }

        #endregion
        protected void grdSituation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void grdSituation_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["LightPollution"] != null && item["LightPollution"].Text != "0")
                {
                    GridTableCell cell = (GridTableCell)item["LightPollution"];
                    cell.ToolTip = drv["LightPollutionDay"].ToString();
                }
                if (item["ModeratePollution"] != null && item["ModeratePollution"].Text != "0")
                {
                    GridTableCell cell = (GridTableCell)item["ModeratePollution"];
                    cell.ToolTip = drv["ModeratePollutionDay"].ToString();
                }
                if (item["HighPollution"] != null && item["HighPollution"].Text != "0")
                {
                    GridTableCell cell = (GridTableCell)item["HighPollution"];
                    cell.ToolTip = drv["HighPollutionDay"].ToString();
                }
                if (item["SeriousPollution"] != null && item["SeriousPollution"].Text != "0")
                {
                    GridTableCell cell = (GridTableCell)item["SeriousPollution"];
                    cell.ToolTip = drv["SeriousPollutionDay"].ToString();
                }
            }
        }

        protected void Seanson_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            DateTime dtB = DateTime.Now;
            string MonthB = "";
            string MonthE = "";
            if (Season.SelectedValue == "1")
            {
                MonthB = "-01-01";
                MonthE = "-03-31";
            }
            else if (Season.SelectedValue == "2")
            {
                MonthB = "-04-01";
                MonthE = "-06-30";
            }
            else if (Season.SelectedValue == "3")
            {
                MonthB = "-07-01";
                MonthE = "-09-30";
            }
            else if (Season.SelectedValue == "4")
            {
                MonthB = "-10-01";
                MonthE = "-12-31";
            }
            dtB = Convert.ToDateTime(Year.SelectedValue + MonthB);
            System.DateTime dtE = Convert.ToDateTime(Year.SelectedValue + MonthE);
            if (dtE > System.DateTime.Now)
                dtE = System.DateTime.Now;
            txtDateF.Text = dtB.ToString("yyyy年MM月dd日");
            txtDateT.Text = dtE.ToString("yyyy年MM月dd日");
            Bind();
            grdSituation.Rebind();
        }
    }
}