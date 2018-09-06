using Aspose.Words;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using Telerik.Web.UI;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Core.Generic;
using SmartEP.Service.DataAnalyze.Water;
using SmartEP.Utilities.Office;
using Aspose.Cells;
using System.Drawing.Imaging;
using System.Drawing;
using Aspose.Cells.Drawing;
using Aspose.Cells.Charts;
using Aspose.Words.Drawing;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AirQualittyYearReport : SmartEP.WebUI.Common.BasePage
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
        public void createimg()
        {
            string[] factors = { "a34004", "a34002", "a21026", "a21004", "a05024", "a21005", "standrate", "month" };
            string[] factorNames = { "PM2.5浓度（μg/m3）", "PM10浓度（μg/m3）", "SO2浓度（μg/m3）", "NO2浓度（μg/m3）", "O3日最大8小时平均浓度超标率(%)", "CO浓度（mg/m3）", "环境空气质量达标天数比例(%)", "环境空气质量达标天数比例(%)" };
            DataTable dtnew = new DataTable();
            DateTime dtBegin = Convert.ToDateTime(Year.SelectedValue + "-01-01");

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(Year.SelectedValue + "-12-31");
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;

            var dtNew = new DataTable();
            string year = YearBegin.SelectedValue;
            int y = 0;
            dtNew = m_YearAQIService.getRegionData(dtBegin, endDate, y);
            DataTable Newdt = m_YearAQIService.GetMonthAvgAllData(dtBegin, endDate);
            int k = 1;
            string filename = "data";

            foreach (string strfactor in factors)
            {
                if (strfactor != "month")
                {
                    DataTable dt = dtNew.Select("factor='" + strfactor + "'").CopyToDataTable();
                    if (strfactor != "standrate")
                        dt.Columns.Remove("全市");
                    dt.Columns["year"].SetOrdinal(0);
                    dt.Columns.Remove("factor");
                    string folderPath = strfactor + ".xls";
                    string mySaveFolder = System.Web.HttpContext.Current.Server.MapPath(".") + @"\Tmp\" + folderPath + @"";

                    //DataTabletoExcel(dt, mySaveFolder);
                    ExcelHelper.CreateExcel(dt, mySaveFolder, strfactor);
                    #region 数据转成折线图
                    string title = factorNames[k - 1];
                    ToLineChart(folderPath, title, strfactor, endDate);
                    #endregion
                    #region 折线图转成图片
                    LineChartToImg(strfactor);
                    #endregion
                    k++;
                }
                else
                {
                    DataTable dt = Newdt;
                    dt.Columns["year"].SetOrdinal(0);
                    string folderPath = strfactor + ".xls";
                    string mySaveFolder = System.Web.HttpContext.Current.Server.MapPath(".") + @"\Tmp\" + folderPath + @"";

                    //DataTabletoExcel(dt, mySaveFolder);
                    ExcelHelper.CreateExcel(dt, mySaveFolder, strfactor);
                    #region 数据转成折线图
                    string title = factorNames[k - 1];
                    ToLineChart(folderPath, title, strfactor, endDate);
                    #endregion
                    #region 折线图转成图片
                    LineChartToImg(strfactor);
                    #endregion
                    k++;
                }
            }


        }
        //数据转成chart图表(折线图)
        private void ToLineChart(string filename, string title, string sheetName, DateTime endDate)
        {
            try
            {
                WorkbookDesigner designer = new WorkbookDesigner();
                string path = Server.MapPath("Tmp/" + filename);
                designer.Workbook.Open(path);
                Workbook workbook = designer.Workbook;
                //创建一个chart到页面
                if (sheetName != "month")
                    CreateLineChart(workbook, title, sheetName, ChartType.Column, endDate);
                else
                    CreateLineChart(workbook, title, sheetName, ChartType.Line, endDate);
                designer.Process();
                designer.Workbook.Save(Server.MapPath("Cache/Cache.xls"));
                //Response.Flush();
                //Response.Close();
                designer = null;
                // Response.End();
            }
            catch (Exception e)
            {
                Response.Write(e.ToString());
            }
        }
        private void CreateLineChart(Workbook workbook, string title, string sheetName, ChartType ff, DateTime endDate)
        {
            try
            {
                //创建一个折线图
                Worksheet worksheet = workbook.Worksheets[0];
                worksheet.Charts.Add(ff, 1, 1, 25, 10);
                Aspose.Cells.Charts.Chart chart = workbook.Worksheets[0].Charts[0];
                chart.ChartArea.Border.IsVisible = true;//chart图标边框不显示
                chart.ChartArea.Area.ForegroundColor = Color.White;
                chart.PlotArea.Area.ForegroundColor = Color.White;
                chart.PlotArea.Border.IsVisible = false;
                chart.PlotArea.Shadow = false;
                chart.ShowDataTable = false;  //显示模拟计算表
                chart.ValueAxis.Title.Font.IsSubscript = true;
                chart.PlotAreaWithoutTickLabels.Area.BackgroundColor = Color.White;
                //折线区域竖线设置为显示颜色设置为灰色
                //chart.CategoryAxis.MajorGridLines.IsVisible = false;
                chart.CategoryAxis.MajorGridLines.Color = Color.White;
                //折线区域设置横着的网格线显示          
                //chart.MajorGridLines.IsVisible = true;
                //chart.MajorGridLines.Color = Color.Gray;

                //设置title样式
                //chart.Title.Text = title + "水质变化趋势";


                int count = Convert.ToInt32(6);
                //数字英语对照

                if (sheetName != "standrate" && sheetName != "month")
                    chart.NSeries.Add(sheetName + "!B2:G3", false);
                if (sheetName == "standrate")
                    chart.NSeries.Add(sheetName + "!B2:H3", false);
                if (sheetName == "month")
                {
                    string str = "";
                    switch (endDate.Month)
                    {
                        case 1: str = "!B2:B3";
                            break;
                        case 2: str = "!B2:C3";
                            break;
                        case 3: str = "!B2:D3";
                            break;
                        case 4: str = "!B2:E3";
                            break;
                        case 5: str = "!B2:F3";
                            break;
                        case 6: str = "!B2:G3";
                            break;
                        case 7: str = "!B2:H3";
                            break;
                        case 8: str = "!B2:I3";
                            break;
                        case 9: str = "!B2:J3";
                            break;
                        case 10: str = "!B2:K3";
                            break;
                        case 11: str = "!B2:L3";
                            break;
                        case 12: str = "!B2:M3";
                            break;
                    }
                    chart.NSeries.Add(sheetName + str, false);
                }
                //chart.NSeries.Add("Sheet1!B2:B5", false);

                //Set NSeries Category Datasource
                //chart.NSeries.CategoryData = "Sheet1!B1:" + Eng + "1";
                if (sheetName != "standrate" && sheetName != "month")
                    chart.NSeries.CategoryData = sheetName + "!B1:G1";
                if (sheetName == "standrate")
                    chart.NSeries.CategoryData = sheetName + "!B1:H1";
                if (sheetName == "month")
                {
                    string str = "";
                    switch (endDate.Month)
                    {
                        case 1: str = "!B1:B1";
                            break;
                        case 2: str = "!B1:C1";
                            break;
                        case 3: str = "!B1:D1";
                            break;
                        case 4: str = "!B1:E1";
                            break;
                        case 5: str = "!B1:F1";
                            break;
                        case 6: str = "!B1:G1";
                            break;
                        case 7: str = "!B1:H1";
                            break;
                        case 8: str = "!B1:I1";
                            break;
                        case 9: str = "!B1:J1";
                            break;
                        case 10: str = "!B1:K1";
                            break;
                        case 11: str = "!B1:L1";
                            break;
                        case 12: str = "!B1:M1";
                            break;
                    }
                    chart.NSeries.CategoryData = sheetName + str;
                }


                Cells cells = workbook.Worksheets[0].Cells;
                //chart.NSerie
                //loop over the Nseriese


                for (int i = 0; i < chart.NSeries.Count; i++)
                {
                    //设置每条折线的名称
                    chart.NSeries[i].Name = cells[i + 1, 0].Value.ToString();
                    //设置线的宽度
                    chart.NSeries[i].Line.Weight = WeightType.MediumLine;
                    chart.NSeries[i].Area.BackgroundColor = GetColor(i);
                    chart.NSeries[i].Area.ForegroundColor = GetColor(i);
                    chart.NSeries[i].Border.Color = GetColor(i);
                    //设置每个值坐标点的样式
                    chart.NSeries[i].MarkerStyle = ChartMarkerType.Automatic;
                    chart.NSeries[i].MarkerSize = 10;
                    chart.NSeries[i].MarkerBackgroundColor = GetColor(i);
                    chart.NSeries[i].MarkerForegroundColor = GetColor(i);
                    //chart.NSeries[i].Area.BackgroundColor = Color.Red;
                    //chart.NSeries[i].Area.ForegroundColor = GetColor(i);
                    //chart.NSeries[i].SeriesLines.Color = Color.Red;
                    chart.NSeries[i].DataLabels.Border.Weight = WeightType.HairLine;


                    //每个折线向显示出值
                    chart.NSeries[i].DataLabels.ShowValue = false;
                    chart.NSeries[i].DataLabels.TextFont.Color = Color.Black;
                    //chart.NSeries[i].DataLabels.ShowPercentage = true;
                    chart.NSeries[i].DataLabels.ShowCategoryName = false;
                    chart.NSeries[i].DataLabels.Position = LabelPositionType.InsideEnd;
                    //chart.NSeries[i].DataLabels.ShowValue = false;


                }
                //chart.ValueAxis.MinValue = 1;
                //设置x轴上数据的样式为灰色
                chart.CategoryAxis.TickLabels.Font.Color = Color.Black;
                if (sheetName == "month")
                {
                    chart.CategoryAxis.TickLabels.Font.Size = 12;
                }
                else
                {
                    chart.CategoryAxis.TickLabels.Font.Size = 19;
                }
                chart.CategoryAxis.TickLabelPosition = TickLabelPositionType.NextToAxis;

                //设置y轴的样式
                chart.ValueAxis.TickLabelPosition = TickLabelPositionType.Low;
                chart.ValueAxis.TickLabels.Font.Color = Color.Black;
                chart.ValueAxis.TickLabels.Font.Size = 19;
                chart.ValueAxis.TickLabels.NumberFormat = "";
                chart.ValueAxis.AxisLine.IsVisible = true;

                chart.ValueAxis.Title.Text = title;
                chart.ValueAxis.Title.Font.Size = 19;
                chart.ValueAxis.Title.Font.Name = "宋体";
                chart.ValueAxis.Title.Font.IsBold = true;

                // chart.ValueAxis.TickLabels.TextDirection = TextDirectionType.LeftToRight;
                //设置Legend位置以及样式
                chart.ShowLegend = true;
                chart.Legend.Position = LegendPositionType.Top;
                chart.Legend.TextFont.Color = Color.Black;
                chart.Legend.Border.Color = Color.Black;
                chart.Legend.Font.Name = "宋体";
                chart.Legend.Font.IsBold = true;
                chart.Legend.Font.Size = 19;
                chart.Legend.Border.IsVisible = false;
            }
            catch (Exception e)
            {
                Response.Write(e.ToString());
            }
        }
        //折线图转成图片
        private void LineChartToImg(string title)
        {
            try
            {
                Workbook workbook = new Workbook(Server.MapPath("Cache/Cache.xls"));
                Aspose.Cells.Charts.Chart chart = workbook.Worksheets[title].Charts[0];
                string imgPath = Server.MapPath("Cache/" + title + ".jpg");
                chart.ToImage(imgPath, ImageFormat.Jpeg);
                File.Delete(Server.MapPath("Cache/Cache.xls"));
            }
            catch (Exception e)
            {
                Response.Write(e.ToString());
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

            YearBegin.Items.Clear();//年开始
            BindType();

            if (YearBegin.Items.Count > 0)
                YearBegin.Items[0].Checked = true;

            DateTime dtB = DateTime.Now;
            dtB = Convert.ToDateTime(Year.SelectedValue + "-01-01");
            System.DateTime dtE = Convert.ToDateTime(Year.SelectedValue + "-12-31");
            if (dtE > System.DateTime.Now)
                dtE = System.DateTime.Now;
            txtDateF.Text = dtB.ToString("yyyy年MM月dd日");
            txtDateT.Text = dtE.ToString("yyyy年MM月dd日");
            Bind();

            createimg();
            string[] imgSrcList1 = { "Cache/standrate.jpg" };
            Repeater1.DataSource = imgSrcList1;
            Repeater1.DataBind();
            string[] imgSrcList2 = { "Cache/month.jpg" };
            Repeater2.DataSource = imgSrcList2;
            Repeater2.DataBind();
            string[] imgSrcList3 = { "Cache/a34004.jpg" };
            Repeater3.DataSource = imgSrcList3;
            Repeater3.DataBind();
            string[] imgSrcList4 = { "Cache/a34002.jpg" };
            Repeater4.DataSource = imgSrcList4;
            Repeater4.DataBind();
            string[] imgSrcList5 = { "Cache/a21004.jpg" };
            Repeater5.DataSource = imgSrcList5;
            Repeater5.DataBind();
            string[] imgSrcList6 = { "Cache/a21026.jpg" };
            Repeater6.DataSource = imgSrcList6;
            Repeater6.DataBind();
            string[] imgSrcList7 = { "Cache/a05024.jpg" };
            Repeater7.DataSource = imgSrcList7;
            Repeater7.DataBind();
            string[] imgSrcList8 = { "Cache/a21005.jpg" };
            Repeater8.DataSource = imgSrcList8;
            Repeater8.DataBind();
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
            DateTime dtBegin = Convert.ToDateTime(Year.SelectedValue + "-01-01");

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(Year.SelectedValue + "-12-31");
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;

            var dataViewM = new DataView();
            // string year = YearBegin.SelectedValue;
            string year = "2013";
            string target = "7";
            dataViewM = m_YearAQIService.GetRegionsYearAllData(dtBegin, endDate, year, target);
            dtnew = dataViewM.ToTable();
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AirQualittyYearReport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);

            string yearRange = "    " + dtBegin.ToString("yyyy") + "年，";
            string yearStr = dtBegin.ToString("yyyy") + "年";
            M100.InnerText = yearStr + "全市各地环境空气质量达标天数比例";
            M99.InnerText = yearStr + "苏州市区环境空气质量达标天数比例月度分布";
            M98.InnerText = yearStr + "全市各地细颗粒物年均浓度";
            M97.InnerText = yearStr + "全市各地可吸入颗粒物年均浓度";
            M96.InnerText = yearStr + "全市各地二氧化氮年均浓度";
            M95.InnerText = yearStr + "全市各地二氧化硫年均浓度";
            M94.InnerText = yearStr + "全市各地臭氧日最大8小时平均浓度超标率";
            M93.InnerText = yearStr + "全市各地一氧化碳年均浓度";
            M92.InnerText = dtBegin.AddYears(-1).Year.ToString() + "年和" + dtBegin.Year.ToString() + "年" + "苏州市现代化空气质量优良天数比例统计表";
            M91.InnerText = yearStr + "苏州市区空气质量排名（从优到劣）情况";
            M90.InnerText = yearStr + "全市各地酸雨发生频率";
            string a = "";
            string b = "";
            string c = "";
            string PM25str = "";
            string PM10str = "";
            string NO2str = "";
            string SO2str = "";
            string O3str = "";
            string COstr = "";
            if (dtnew.Rows.Count > 0)
            {
                //builder.MoveToMergeField("M1");
                //builder.Write(DateTime.Now.ToString("yyyy") + "年");
                a = yearRange + "全市环境空气质量达标天数比例为" + dtnew.Rows[0]["M1"].ToString() + dtnew.Rows[0]["M2"].ToString();
                M1.Text = a;

                b = yearRange + "苏州市区环境空气质量达标天数比例为" + dtnew.Rows[0]["M3"].ToString() + dtnew.Rows[0]["M4"].ToString() + "环境空气质量超标" + dtnew.Rows[0]["M5"].ToString() + dtnew.Rows[0]["M6"].ToString() + dtnew.Rows[0]["M7"].ToString() + dtnew.Rows[0]["M8"].ToString();
                M2.Text = b;

                PM25str = yearRange + "全市各地细颗粒物年均浓度分布在" + dtnew.Rows[0]["M9"].ToString() + dtnew.Rows[0]["M10"].ToString();
                M3.Text = PM25str;

                PM10str = yearRange + "全市各地可吸入颗粒物年均浓度分布在" + dtnew.Rows[0]["M11"].ToString();
                M4.Text = PM10str;

                NO2str = yearRange + "全市各地二氧化氮年均浓度分布在" + dtnew.Rows[0]["M12"].ToString();
                M5.Text = NO2str;

                SO2str = yearRange + "全市各地二氧化硫年均浓度分布在" + dtnew.Rows[0]["M13"].ToString();
                M6.Text = SO2str;

                O3str = yearRange + dtnew.Rows[0]["M14"].ToString();
                M7.Text = O3str;

                COstr = yearRange + "全市各地一氧化碳年均浓度分布在" + dtnew.Rows[0]["M15"].ToString();
                M8.Text = COstr;
                c = "根据《关于印发江苏全面建成小康社会和基本实现现代化环保指标进程监测统计报表制度及组织实施办法（试行）的通知》（苏环办[2013]360号）规定的监测评价方法，" + yearRange + "苏州市空气质量达到二级标准的天数比例为" + dtnew.Rows[0]["M3"].ToString() + dtnew.Rows[0]["M16"].ToString();
                M9.Text = c;
                M11.Text = yearRange + "【苏州市区环境空气质量在全国74个城市中排名平均为第49位，总体处于中等水平；在全省13个城市中排名平均为第8位，总体处于中等水平。】";
                M12.Text = yearRange + "【全市酸雨发生频率为42.9%，与上年相比，上升6.1个百分点。降水pH年均值为5.12，酸度同比持平。市区酸雨发生频率为55.7%，与上年相比，上升了11.0个百分点；降水pH年均值为4.96，酸度同比有所增加。】";
                M13.Text = "【（1）大气污染问题突出，全市和市区空气质量达标天数比例偏低，全市空气质量达标天数相比去年有所下降；市区二氧化氮浓度有所上升，臭氧超标率上升明显。" +
                           "（2）市区空气质量在全国、全省的排名均有下滑，市区细颗粒物与2013年同期相比下降幅度在全省排名靠后（第12名）。" +
                           "（3）市区酸雨发生频率和酸度均相比去年同期有所增加】";
                M14.Text = "【（1）全面推进生态文明建设，加大环境污染综合整治力度，努力建设国家生态文明建设示范市。" +
                           "（2）围绕大气污染防治行动计划目标，进一步切实贯彻国家“大气十条”精神，深化产业结构和能源结构调整，优化工业布局，全面实施机动车尾气污染控制，推进区域性、复合型、项目化大气污染治理。" +
                           "（3）严格控制燃煤消耗总量。摸清家底，削减全市燃煤消耗总量，提高清洁能源所占比例，综合提高能源利用效率。】";

                M30.InnerText = dtBegin.AddYears(-1).Year.ToString() + "年";
                M50.InnerText = dtBegin.Year.ToString() + "年";
                M31.Text = dtnew.Rows[0]["M31"].ToString();
                M32.Text = dtnew.Rows[0]["M32"].ToString();
                M33.Text = dtnew.Rows[0]["M33"].ToString();
                M34.Text = dtnew.Rows[0]["M34"].ToString();
                M35.Text = dtnew.Rows[0]["M35"].ToString();
                M36.Text = dtnew.Rows[0]["M36"].ToString();
                M37.Text = dtnew.Rows[0]["M37"].ToString();
                M41.Text = dtnew.Rows[0]["M41"].ToString();
                M42.Text = dtnew.Rows[0]["M42"].ToString();
                M43.Text = dtnew.Rows[0]["M43"].ToString();
                M44.Text = dtnew.Rows[0]["M44"].ToString();
                M45.Text = dtnew.Rows[0]["M45"].ToString();
                M46.Text = dtnew.Rows[0]["M46"].ToString();
                M47.Text = dtnew.Rows[0]["M47"].ToString();
                M51.Text = dtnew.Rows[0]["M51"].ToString();
                M52.Text = dtnew.Rows[0]["M52"].ToString();
                M53.Text = dtnew.Rows[0]["M53"].ToString();
                M54.Text = dtnew.Rows[0]["M54"].ToString();
                M55.Text = dtnew.Rows[0]["M55"].ToString();
                M56.Text = dtnew.Rows[0]["M56"].ToString();
                M57.Text = dtnew.Rows[0]["M57"].ToString();
                M61.Text = dtnew.Rows[0]["M61"].ToString();
                M62.Text = dtnew.Rows[0]["M62"].ToString();
                M63.Text = dtnew.Rows[0]["M63"].ToString();
                M64.Text = dtnew.Rows[0]["M64"].ToString();
                M65.Text = dtnew.Rows[0]["M65"].ToString();
                M66.Text = dtnew.Rows[0]["M66"].ToString();
                M67.Text = dtnew.Rows[0]["M67"].ToString();
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
            DateTime dtBegin = Convert.ToDateTime(Year.SelectedValue + "-01-01");

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(Year.SelectedValue + "-12-31");
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;

            var dataViewM = new DataView();
            //string year = YearBegin.SelectedValue;
            string year = "2013";
            string target = "7";
            dataViewM = m_YearAQIService.GetRegionsYearAllData(dtBegin, endDate, year, target);
            dtnew = dataViewM.ToTable();
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AirQualittyYearReport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);

            string yearRange = dtBegin.ToString("yyyy") + "年，";
            string yearStr = dtBegin.ToString("yyyy") + "年";
            builder.MoveToMergeField("M100");
            builder.Write(yearStr + "全市各地环境空气质量达标天数比例");
            builder.MoveToMergeField("M99");
            builder.Write(yearStr + "苏州市区环境空气质量达标天数比例月度分布");
            builder.MoveToMergeField("M98");
            builder.Write(yearStr + "全市各地细颗粒物年均浓度");
            builder.MoveToMergeField("M97");
            builder.Write(yearStr + "全市各地可吸入颗粒物年均浓度");
            builder.MoveToMergeField("M96");
            builder.Write(yearStr + "全市各地二氧化氮年均浓度");
            builder.MoveToMergeField("M95");
            builder.Write(yearStr + "全市各地二氧化硫年均浓度");
            builder.MoveToMergeField("M94");
            builder.Write(yearStr + "全市各地臭氧日最大8小时平均浓度超标率");
            builder.MoveToMergeField("M93");
            builder.Write(yearStr + "全市各地一氧化碳年均浓度");
            builder.MoveToMergeField("M92");
            builder.Write(dtBegin.AddYears(-1).Year.ToString() + "年和" + dtBegin.Year.ToString() + "年" + "苏州市现代化空气质量优良天数比例统计表");
            builder.MoveToMergeField("M91");
            builder.Write(yearStr + "苏州市区空气质量排名（从优到劣）情况");
            builder.MoveToMergeField("M90");
             builder.Write(yearStr + "全市各地酸雨发生频率");
            string a = "";
            string b = "";
            string c = "";
            string d = "";
            string f = "";
            string PM25str = "";
            string PM10str = "";
            string NO2str = "";
            string SO2str = "";
            string O3str = "";
            string COstr = "";
            if (dtnew.Rows.Count > 0)
            {
                //builder.MoveToMergeField("M1");
                //builder.Write(DateTime.Now.ToString("yyyy") + "年");
                a = yearRange + "全市环境空气质量达标天数比例为" + dtnew.Rows[0]["M1"].ToString() + dtnew.Rows[0]["M2"].ToString();
                builder.MoveToMergeField("M1");
                builder.Write(a);

                b = yearRange + "苏州市区环境空气质量达标天数比例为" + dtnew.Rows[0]["M3"].ToString() + dtnew.Rows[0]["M4"].ToString() + "环境空气质量超标" + dtnew.Rows[0]["M5"].ToString() + dtnew.Rows[0]["M6"].ToString() + dtnew.Rows[0]["M7"].ToString() + dtnew.Rows[0]["M8"].ToString();
                builder.MoveToMergeField("M2");
                builder.Write(b);

                PM25str = yearRange + "全市各地细颗粒物年均浓度分布在" + dtnew.Rows[0]["M9"].ToString() + dtnew.Rows[0]["M10"].ToString();
                builder.MoveToMergeField("M3");
                builder.Write(PM25str);

                PM10str = yearRange + "全市各地可吸入颗粒物年均浓度分布在" + dtnew.Rows[0]["M11"].ToString();
                builder.MoveToMergeField("M4");
                builder.Write(PM10str);

                NO2str = yearRange + "全市各地二氧化氮年均浓度分布在" + dtnew.Rows[0]["M12"].ToString();
                builder.MoveToMergeField("M5");
                builder.Write(NO2str);

                SO2str = yearRange + "全市各地二氧化硫年均浓度分布在" + dtnew.Rows[0]["M13"].ToString();
                builder.MoveToMergeField("M6");
                builder.Write(SO2str);

                O3str = yearRange + dtnew.Rows[0]["M14"].ToString();
                builder.MoveToMergeField("M7");
                builder.Write(O3str);

                COstr = yearRange + "全市各地一氧化碳年均浓度分布在" + dtnew.Rows[0]["M15"].ToString();
                builder.MoveToMergeField("M8");
                builder.Write(COstr);
                c = "根据《关于印发江苏全面建成小康社会和基本实现现代化环保指标进程监测统计报表制度及组织实施办法（试行）的通知》（苏环办[2013]360号）规定的监测评价方法，" + yearRange + "苏州市空气质量达到二级标准的天数比例为" + dtnew.Rows[0]["M3"].ToString() + dtnew.Rows[0]["M16"].ToString();
                builder.MoveToMergeField("M9");
                builder.Write(c);

                d = yearRange + "苏州市区环境空气质量在全国74个城市中排名平均为第49位，总体处于中等水平；在全省13个城市中排名平均为第8位，总体处于中等水平。";
                builder.MoveToMergeField("M10");
                builder.Write(d);

                f = yearRange + "全市酸雨发生频率为42.9%，与上年相比，上升6.1个百分点。降水pH年均值为5.12，酸度同比持平。市区酸雨发生频率为55.7%，与上年相比，上升了11.0个百分点；降水pH年均值为4.96，酸度同比有所增加。";
                builder.MoveToMergeField("M11");
                builder.Write(f);
                builder.MoveToMergeField("M30");
                builder.Write(dtBegin.AddYears(-1).Year.ToString() + "年");
                builder.MoveToMergeField("M50");
                builder.Write(dtBegin.Year.ToString() + "年");
                builder.MoveToMergeField("M31");
                builder.Write(dtnew.Rows[0]["M31"].ToString());
                builder.MoveToMergeField("M32");
                builder.Write(dtnew.Rows[0]["M32"].ToString());
                builder.MoveToMergeField("M33");
                builder.Write(dtnew.Rows[0]["M33"].ToString());
                builder.MoveToMergeField("M34");
                builder.Write(dtnew.Rows[0]["M34"].ToString());
                builder.MoveToMergeField("M35");
                builder.Write(dtnew.Rows[0]["M35"].ToString());
                builder.MoveToMergeField("M36");
                builder.Write(dtnew.Rows[0]["M36"].ToString());
                builder.MoveToMergeField("M37");
                builder.Write(dtnew.Rows[0]["M37"].ToString());
                builder.MoveToMergeField("M41");
                builder.Write(dtnew.Rows[0]["M41"].ToString());
                builder.MoveToMergeField("M42");
                builder.Write(dtnew.Rows[0]["M42"].ToString());
                builder.MoveToMergeField("M43");
                builder.Write(dtnew.Rows[0]["M43"].ToString());
                builder.MoveToMergeField("M44");
                builder.Write(dtnew.Rows[0]["M44"].ToString());
                builder.MoveToMergeField("M45");
                builder.Write(dtnew.Rows[0]["M45"].ToString());
                builder.MoveToMergeField("M46");
                builder.Write(dtnew.Rows[0]["M46"].ToString());
                builder.MoveToMergeField("M47");
                builder.Write(dtnew.Rows[0]["M47"].ToString());
                builder.MoveToMergeField("M51");
                builder.Write(dtnew.Rows[0]["M51"].ToString());
                builder.MoveToMergeField("M52");
                builder.Write(dtnew.Rows[0]["M52"].ToString());
                builder.MoveToMergeField("M53");
                builder.Write(dtnew.Rows[0]["M53"].ToString());
                builder.MoveToMergeField("M54");
                builder.Write(dtnew.Rows[0]["M54"].ToString());
                builder.MoveToMergeField("M55");
                builder.Write(dtnew.Rows[0]["M55"].ToString());
                builder.MoveToMergeField("M56");
                builder.Write(dtnew.Rows[0]["M56"].ToString());
                builder.MoveToMergeField("M57");
                builder.Write(dtnew.Rows[0]["M57"].ToString());
                builder.MoveToMergeField("M61");
                builder.Write(dtnew.Rows[0]["M61"].ToString());
                builder.MoveToMergeField("M62");
                builder.Write(dtnew.Rows[0]["M62"].ToString());
                builder.MoveToMergeField("M63");
                builder.Write(dtnew.Rows[0]["M63"].ToString());
                builder.MoveToMergeField("M64");
                builder.Write(dtnew.Rows[0]["M64"].ToString());
                builder.MoveToMergeField("M65");
                builder.Write(dtnew.Rows[0]["M65"].ToString());
                builder.MoveToMergeField("M66");
                builder.Write(dtnew.Rows[0]["M66"].ToString());
                builder.MoveToMergeField("M67");
                builder.Write(dtnew.Rows[0]["M67"].ToString());
                doc.MailMerge.DeleteFields();

                //txtDateF.Text = dtBegin.ToString("yyyy年MM月dd日");
                //txtDateT.Text = dtEnd.ToString("yyyy年MM月dd日");
                string[] strPointCodes = { "" };
                string pointCodes = string.Join(";", strPointCodes);
                string[] strPointNames = { "" };
                string pointNames = string.Join(";", strPointNames);
                //添加实体类对象
                string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "AirQualittyYearReport" + ".doc";
                ReportLogEntity customDatum = new ReportLogEntity();
                customDatum.PointIds = pointCodes;//测点Code
                customDatum.PointNames = pointNames;//测点名称
                customDatum.FactorCodes = "";//因子Code
                customDatum.FactorsNames = "";//因子名称
                customDatum.DateTimeRange = dtBegin.Year.ToString() + "年" + dtBegin.Month.ToString() + "月";
                customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
                customDatum.PageTypeID = "AirQualittyYearReport";//页面ID
                customDatum.StartDateTime = dtBegin;
                customDatum.EndDateTime = endDate;
                customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
                customDatum.ExportName =dtBegin.Year+ "年苏州市环境质量报告简本-空气环境质量(含现代化空气)";
                customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/AirQualittyYearReport/" + dtBegin.Year + "/" + dtBegin.Month + "/" + filename).ToString();
                customDatum.CreatDateTime = DateTime.Now;
                //添加数据
                ReportLogService.ReportLogAdd(customDatum);

                string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/AirQualittyYearReport/" + dtBegin.Year + "/" + dtBegin.Month + "/" + filename);
                doc.Save(strTarget);
                doc.Save(this.Response, "AirQualittyYearReport.doc", Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Docx));
                Response.End();
            }
        }
        #endregion
        /// <summary>
        /// 保存按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            string[] factors = { "a34004", "a34002", "a21026", "a21004", "a05024", "a21005", "standrate", "month" };
            //if (dtpBegin.SelectedDate == null)
            //{
            //    return;
            //}
            DataTable dtnew = new DataTable();

            //if (dtpBegin.SelectedDate == null)
            //{
            //    return;
            //}
            DateTime dtBegin = Convert.ToDateTime(Year.SelectedValue + "-01-01");

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(Year.SelectedValue + "-12-31");
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;

            //  string year = YearBegin.SelectedValue;
            string year = "2013";
            string target = "7";
            var dataViewM = new DataView();
            dataViewM = m_YearAQIService.GetRegionsYearAllData(dtBegin, endDate, year, target);
            dtnew = dataViewM.ToTable();
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AirQualittyYearReport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);
            #region 图片插入Word
            foreach (string factor in factors)
            {
                string mark = factor;
                Aspose.Words.Drawing.Shape shape2 = new Aspose.Words.Drawing.Shape(doc, ShapeType.Image);
                shape2.ImageData.SetImage(Server.MapPath("Cache/" + factor + ".jpg"));
         
                shape2.Width = 421;
                shape2.Height = 180;
                shape2.HorizontalAlignment = HorizontalAlignment.Center;
                shape2.BehindText = true;
                shape2.WrapType = WrapType.Tight;
              
                if (doc.Range.Bookmarks[mark] != null)
                {
                    builder.MoveToBookmark(mark);
                    builder.InsertNode(shape2);
                }

            }

            #endregion
            string yearRange = dtBegin.ToString("yyyy") + "年，";
            string yearStr = dtBegin.ToString("yyyy") + "年";
            builder.MoveToMergeField("M100");
            builder.Write(yearStr + "全市各地环境空气质量达标天数比例");
            builder.MoveToMergeField("M99");
            builder.Write(yearStr + "苏州市区环境空气质量达标天数比例月度分布");
            builder.MoveToMergeField("M98");
            builder.Write(yearStr + "全市各地细颗粒物年均浓度");
            builder.MoveToMergeField("M97");
            builder.Write(yearStr + "全市各地可吸入颗粒物年均浓度");
            builder.MoveToMergeField("M96");
            builder.Write(yearStr + "全市各地二氧化氮年均浓度");
            builder.MoveToMergeField("M95");
            builder.Write(yearStr + "全市各地二氧化硫年均浓度");
            builder.MoveToMergeField("M94");
            builder.Write(yearStr + "全市各地臭氧日最大8小时平均浓度超标率");
            builder.MoveToMergeField("M93");
            builder.Write(yearStr + "全市各地一氧化碳年均浓度");
            builder.MoveToMergeField("M92");
            builder.Write(dtBegin.AddYears(-1).Year.ToString() + "年和" + dtBegin.Year.ToString() + "年" + "苏州市现代化空气质量优良天数比例统计表");
            builder.MoveToMergeField("M91");
            builder.Write(yearStr + "苏州市区空气质量排名（从优到劣）情况");
            builder.MoveToMergeField("M90");
            builder.Write(yearStr + "全市各地酸雨发生频率");
            string a = "";
            string b = "";
            string c = "";
            string d = "";
            string f= "";
            string PM25str = "";
            string PM10str = "";
            string NO2str = "";
            string SO2str = "";
            string O3str = "";
            string COstr = "";
            if (dtnew.Rows.Count > 0)
            {
                a = yearRange + "全市环境空气质量达标天数比例为" + dtnew.Rows[0]["M1"].ToString() + dtnew.Rows[0]["M2"].ToString();
                builder.MoveToMergeField("M1");
                builder.Write(a);

                b = yearRange + "苏州市区环境空气质量达标天数比例为" + dtnew.Rows[0]["M3"].ToString() + dtnew.Rows[0]["M4"].ToString() + "环境空气质量超标" + dtnew.Rows[0]["M5"].ToString() + dtnew.Rows[0]["M6"].ToString() + dtnew.Rows[0]["M7"].ToString() + dtnew.Rows[0]["M8"].ToString();
                builder.MoveToMergeField("M2");
                builder.Write(b);

                PM25str = yearRange + "全市各地细颗粒物年均浓度分布在" + dtnew.Rows[0]["M9"].ToString() + dtnew.Rows[0]["M10"].ToString();
                builder.MoveToMergeField("M3");
                builder.Write(PM25str);

                PM10str = yearRange + "全市各地可吸入颗粒物年均浓度分布在" + dtnew.Rows[0]["M11"].ToString();
                builder.MoveToMergeField("M4");
                builder.Write(PM10str);

                NO2str = yearRange + "全市各地二氧化氮年均浓度分布在" + dtnew.Rows[0]["M12"].ToString();
                builder.MoveToMergeField("M5");
                builder.Write(NO2str);

                SO2str = yearRange + "全市各地二氧化硫年均浓度分布在" + dtnew.Rows[0]["M13"].ToString();
                builder.MoveToMergeField("M6");
                builder.Write(SO2str);

                O3str = yearRange + dtnew.Rows[0]["M14"].ToString();
                builder.MoveToMergeField("M7");
                builder.Write(O3str);

                COstr = yearRange + "全市各地一氧化碳年均浓度分布在" + dtnew.Rows[0]["M15"].ToString();
                builder.MoveToMergeField("M8");
                builder.Write(COstr);
                c = "根据《关于印发江苏全面建成小康社会和基本实现现代化环保指标进程监测统计报表制度及组织实施办法（试行）的通知》（苏环办[2013]360号）规定的监测评价方法，" + yearRange + "苏州市空气质量达到二级标准的天数比例为" + dtnew.Rows[0]["M3"].ToString() + dtnew.Rows[0]["M16"].ToString();
                builder.MoveToMergeField("M9");
                builder.Write(c);

                d = yearRange + "苏州市区环境空气质量在全国74个城市中排名平均为第49位，总体处于中等水平；在全省13个城市中排名平均为第8位，总体处于中等水平。";
                builder.MoveToMergeField("M10");
                builder.Write(d);

                f = yearRange + "全市酸雨发生频率为42.9%，与上年相比，上升6.1个百分点。降水pH年均值为5.12，酸度同比持平。市区酸雨发生频率为55.7%，与上年相比，上升了11.0个百分点；降水pH年均值为4.96，酸度同比有所增加。";
                builder.MoveToMergeField("M11");
                builder.Write(f);

                builder.MoveToMergeField("M30");
                builder.Write(dtBegin.AddYears(-1).Year.ToString() + "年");
                builder.MoveToMergeField("M50");
                builder.Write(dtBegin.Year.ToString() + "年");
                builder.MoveToMergeField("M31");
                builder.Write(dtnew.Rows[0]["M31"].ToString());
                builder.MoveToMergeField("M32");
                builder.Write(dtnew.Rows[0]["M32"].ToString());
                builder.MoveToMergeField("M33");
                builder.Write(dtnew.Rows[0]["M33"].ToString());
                builder.MoveToMergeField("M34");
                builder.Write(dtnew.Rows[0]["M34"].ToString());
                builder.MoveToMergeField("M35");
                builder.Write(dtnew.Rows[0]["M35"].ToString());
                builder.MoveToMergeField("M36");
                builder.Write(dtnew.Rows[0]["M36"].ToString());
                builder.MoveToMergeField("M37");
                builder.Write(dtnew.Rows[0]["M37"].ToString());
                builder.MoveToMergeField("M41");
                builder.Write(dtnew.Rows[0]["M41"].ToString());
                builder.MoveToMergeField("M42");
                builder.Write(dtnew.Rows[0]["M42"].ToString());
                builder.MoveToMergeField("M43");
                builder.Write(dtnew.Rows[0]["M43"].ToString());
                builder.MoveToMergeField("M44");
                builder.Write(dtnew.Rows[0]["M44"].ToString());
                builder.MoveToMergeField("M45");
                builder.Write(dtnew.Rows[0]["M45"].ToString());
                builder.MoveToMergeField("M46");
                builder.Write(dtnew.Rows[0]["M46"].ToString());
                builder.MoveToMergeField("M47");
                builder.Write(dtnew.Rows[0]["M47"].ToString());
                builder.MoveToMergeField("M51");
                builder.Write(dtnew.Rows[0]["M51"].ToString());
                builder.MoveToMergeField("M52");
                builder.Write(dtnew.Rows[0]["M52"].ToString());
                builder.MoveToMergeField("M53");
                builder.Write(dtnew.Rows[0]["M53"].ToString());
                builder.MoveToMergeField("M54");
                builder.Write(dtnew.Rows[0]["M54"].ToString());
                builder.MoveToMergeField("M55");
                builder.Write(dtnew.Rows[0]["M55"].ToString());
                builder.MoveToMergeField("M56");
                builder.Write(dtnew.Rows[0]["M56"].ToString());
                builder.MoveToMergeField("M57");
                builder.Write(dtnew.Rows[0]["M57"].ToString());
                builder.MoveToMergeField("M61");
                builder.Write(dtnew.Rows[0]["M61"].ToString());
                builder.MoveToMergeField("M62");
                builder.Write(dtnew.Rows[0]["M62"].ToString());
                builder.MoveToMergeField("M63");
                builder.Write(dtnew.Rows[0]["M63"].ToString());
                builder.MoveToMergeField("M64");
                builder.Write(dtnew.Rows[0]["M64"].ToString());
                builder.MoveToMergeField("M65");
                builder.Write(dtnew.Rows[0]["M65"].ToString());
                builder.MoveToMergeField("M66");
                builder.Write(dtnew.Rows[0]["M66"].ToString());
                builder.MoveToMergeField("M67");
                builder.Write(dtnew.Rows[0]["M67"].ToString());
                doc.MailMerge.DeleteFields();
                string[] strPointCodes = { };//站点Code
                string pointCodes = string.Join(";", strPointCodes);
                string[] strPointNames = { };//站点名称
                string pointNames = string.Join(";", strPointNames);
                string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "AirQualittyYearReport" + ".doc";
                //添加实体类对象
                ReportLogEntity customDatum = new ReportLogEntity();
                customDatum.PointIds = "7e05b94c-bbd4-45c3-919c-42da2e63fd43;66d2abd1-ca39-4e39-909f-da872704fbfd;d7d7a1fe-493a-4b3f-8504-b1850f6d9eff;57b196ed-5038-4ad0-a035-76faee2d7a98;2e2950cd-dbab-43b3-811d-61bd7569565a;2fea3cb2-8b95-45e6-8a71-471562c4c89c";//测点Code
                customDatum.PointNames = "市区均值;张家港;常熟市;太仓市;昆山市;吴江区";//测点名称
                customDatum.FactorCodes = "a21026;a21004;a34002;a34004;a21005;a05024";//因子Code
                customDatum.FactorsNames = "二氧化硫;二氧化氮;PM10;PM2.5;一氧化碳;臭氧";//因子名称
                customDatum.DateTimeRange = dtBegin.Year.ToString() + "年" + dtBegin.Month.ToString() + "月";
                customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
                customDatum.PageTypeID = "AirQualittyYearReport";//页面ID
                customDatum.StartDateTime = dtBegin;
                customDatum.EndDateTime = endDate;
                customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
                customDatum.ExportName = dtBegin.Year + "年苏州市环境质量报告简本-空气环境质量(含现代化空气)";
                customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/AirQualittyYearReport/" + dtBegin.Year + "/" + dtBegin.Month + "/" + filename).ToString();
                customDatum.CreatDateTime = DateTime.Now;
                //添加数据
                ReportLogService.ReportLogAdd(customDatum);

                string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/AirQualittyYearReport/" + dtBegin.Year + "/" + dtBegin.Month + "/" + filename);
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
                if (M6.Text != "")
                {
                    r.Add("6");
                    pcontent.Add("6", M6.Text);
                }
                if (M7.Text != "")
                {
                    r.Add("7");
                    pcontent.Add("7", M7.Text);
                }
                if (M8.Text != "")
                {
                    r.Add("8");
                    pcontent.Add("8", M8.Text);
                }
                if (M9.Text != "")
                {
                    r.Add("9");
                    pcontent.Add("9", M9.Text);
                }
                if (M11.Text != "")
                {
                    r.Add("11");
                    pcontent.Add("11", M11.Text);
                }
                if (M12.Text != "")
                {
                    r.Add("12");
                    pcontent.Add("12", M12.Text);
                }
                if (M13.Text != "")
                {
                    r.Add("13");
                    pcontent.Add("13", M13.Text);
                }
                if (M14.Text != "")
                {
                    r.Add("14");
                    pcontent.Add("14", M14.Text);
                }
                if (M31.Text != "")
                {
                    r.Add("31");
                    pcontent.Add("31", M31.Text);
                }
                if (M32.Text != "")
                {
                    r.Add("32");
                    pcontent.Add("32", M32.Text);
                }
                if (M33.Text != "")
                {
                    r.Add("33");
                    pcontent.Add("33", M33.Text);
                }
                if (M34.Text != "")
                {
                    r.Add("34");
                    pcontent.Add("34", M34.Text);
                }
                if (M35.Text != "")
                {
                    r.Add("35");
                    pcontent.Add("35", M35.Text);
                }
                if (M36.Text != "")
                {
                    r.Add("36");
                    pcontent.Add("36", M36.Text);
                }
                if (M37.Text != "")
                {
                    r.Add("37");
                    pcontent.Add("37", M37.Text);
                }
                if (M41.Text != "")
                {
                    r.Add("41");
                    pcontent.Add("41", M41.Text);
                }
                if (M42.Text != "")
                {
                    r.Add("42");
                    pcontent.Add("42", M42.Text);
                }
                if (M43.Text != "")
                {
                    r.Add("43");
                    pcontent.Add("43", M43.Text);
                }
                if (M44.Text != "")
                {
                    r.Add("44");
                    pcontent.Add("44", M44.Text);
                }
                if (M45.Text != "")
                {
                    r.Add("45");
                    pcontent.Add("45", M45.Text);
                }
                if (M46.Text != "")
                {
                    r.Add("46");
                    pcontent.Add("46", M46.Text);
                }
                if (M47.Text != "")
                {
                    r.Add("47");
                    pcontent.Add("47", M47.Text);
                }
                if (M51.Text != "")
                {
                    r.Add("51");
                    pcontent.Add("51", M51.Text);
                }
                if (M52.Text != "")
                {
                    r.Add("52");
                    pcontent.Add("52", M52.Text);
                }
                if (M53.Text != "")
                {
                    r.Add("53");
                    pcontent.Add("53", M53.Text);
                }
                if (M54.Text != "")
                {
                    r.Add("54");
                    pcontent.Add("54", M54.Text);
                }
                if (M55.Text != "")
                {
                    r.Add("55");
                    pcontent.Add("55", M55.Text);
                }
                if (M56.Text != "")
                {
                    r.Add("56");
                    pcontent.Add("56", M56.Text);
                }
                if (M57.Text != "")
                {
                    r.Add("57");
                    pcontent.Add("57", M57.Text);
                }
                if (M61.Text != "")
                {
                    r.Add("61");
                    pcontent.Add("61", M61.Text);
                }
                if (M62.Text != "")
                {
                    r.Add("62");
                    pcontent.Add("62", M62.Text);
                }
                if (M63.Text != "")
                {
                    r.Add("63");
                    pcontent.Add("63", M63.Text);
                }
                if (M64.Text != "")
                {
                    r.Add("64");
                    pcontent.Add("64", M64.Text);
                }
                if (M65.Text != "")
                {
                    r.Add("65");
                    pcontent.Add("65", M65.Text);
                }
                if (M66.Text != "")
                {
                    r.Add("66");
                    pcontent.Add("66", M66.Text);
                }
                if (M67.Text != "")
                {
                    r.Add("67");
                    pcontent.Add("67", M67.Text);
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
            DateTime dtB = DateTime.Now;
            dtB = Convert.ToDateTime(Year.SelectedValue + "-01-01");
            System.DateTime dtE = Convert.ToDateTime(Year.SelectedValue + "-12-31");
            if (dtE > System.DateTime.Now)
                dtE = System.DateTime.Now;
            txtDateF.Text = dtB.ToString("yyyy年MM月dd日");
            txtDateT.Text = dtE.ToString("yyyy年MM月dd日");
            Bind();
            createimg();
        }
        private Color GetColor(int i)
        {
            Color c = new Color();
            switch (i)
            {
                case 0: c = Color.YellowGreen; break;
                case 1: c = Color.RoyalBlue; break;
                case 2: c = Color.Red; break;
                case 3: c = Color.DarkViolet; break;
                case 4: c = Color.Green; break;
                case 5: c = Color.DarkOrchid; break;
                case 6: c = Color.DarkSeaGreen; break;
                case 7: c = Color.LawnGreen; break;
                case 8: c = Color.LimeGreen; break;
            }
            return c;
        }
    }
}