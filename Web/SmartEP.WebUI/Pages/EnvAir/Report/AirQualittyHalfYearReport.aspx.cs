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
using SmartEP.Service.DataAnalyze.Water;
using Aspose.Cells;
using System.Drawing;
using Aspose.Cells.Charts;
using SmartEP.Utilities.Office;
using System.Drawing.Imaging;
using Aspose.Cells.Drawing;
using Aspose.Words.Drawing;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AirQualittyHalfYearReport : SmartEP.WebUI.Common.BasePage
    {
        /// 数据处理服务
        /// </summary>
        private YearAQIService m_YearAQIService = new YearAQIService();
        ReportLogService ReportLogService = new ReportLogService();
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
            string[] factors = { "a34004H", "a34002H", "a21026H", "a21004H", "a05024H", "a21005H", "standrateH", "level" };
            string[] factorNames = { "PM2.5浓度（μg/m3）", "PM10浓度（μg/m3）", "SO2浓度（μg/m3）", "NO2浓度（μg/m3）", "O3日最大8小时平均浓度超标率(%)", "CO浓度（mg/m3）", "环境空气质量达标天数比例(%)", "环境空气质量达标天数比例(%)" };
            DataTable dtnew = new DataTable();
            DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).FirstDayOfMonth();

            System.DateTime endDate = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd")).LastDayOfMonth();
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;

            var dtNew = new DataTable();
            int year = 1;
            dtNew = m_YearAQIService.getRegionData(dtBegin, endDate,year);

            DataTable Newdt = m_YearAQIService.GetLevelAllData(dtBegin, endDate);
            int k = 1;
            string filename = "data";
            foreach (string strfactor in factors)
            {
                if (strfactor != "level")
                {
                    string factor = strfactor.Trim('H');
                    DataTable dt = dtNew.Select("factor='" + factor + "'").CopyToDataTable();
                    if (strfactor != "standrateH")
                        dt.Columns.Remove("全市");
                    dt.Columns["year"].SetOrdinal(0);
                    dt.Columns.Remove("factor");
                    string folderPath = strfactor + ".xls";
                    string mySaveFolder = System.Web.HttpContext.Current.Server.MapPath(".") + @"\Tmp\" + folderPath + @"";

                    //DataTabletoExcel(dt, mySaveFolder);
                    ExcelHelper.CreateExcel(dt, mySaveFolder, strfactor);
                    #region 数据转成折线图
                    string title = factorNames[k - 1];
                    ToLineChart(folderPath, title, strfactor, endDate, dt.Columns.Count);
                    #endregion
                    #region 折线图转成图片
                    LineChartToImg(strfactor);
                    #endregion
                    k++;
                }
                else
                {
                    DataTable dt = Newdt;
                    string folderPath = strfactor + ".xls";
                    string mySaveFolder = System.Web.HttpContext.Current.Server.MapPath(".") + @"\Tmp\" + folderPath + @"";

                    //DataTabletoExcel(dt, mySaveFolder);
                    ExcelHelper.CreateExcel(dt, mySaveFolder, strfactor);
                    #region 数据转成折线图
                    string title = factorNames[k - 1];
                    ToLineChart(folderPath, title, strfactor, endDate, dt.Columns.Count);
                    #endregion
                    #region 折线图转成图片
                    LineChartToImg(strfactor);
                    #endregion
                    k++;
                }
            }


        }
        //数据转成chart图表(折线图)
        private void ToLineChart(string filename, string title, string sheetName, DateTime endDate, int Count)
        {
            try
            {
                WorkbookDesigner designer = new WorkbookDesigner();
                string path = Server.MapPath("Tmp/" + filename);
                designer.Workbook.Open(path);
                Workbook workbook = designer.Workbook;
                //创建一个chart到页面
                if (sheetName != "level")
                    CreateLineChart(workbook, title, sheetName, ChartType.Column);
                else
                    CreatePerChart(workbook, title, sheetName, ChartType.Pie3DExploded, Count);
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
        private void CreateLineChart(Workbook workbook, string title, string sheetName, ChartType ff)
        {
            try
            {
                //创建一个折线图
                Worksheet worksheet = workbook.Worksheets[0];
                worksheet.Charts.Add(ff, 1, 1, 25, 10);
                Aspose.Cells.Charts.Chart chart = workbook.Worksheets[0].Charts[0];
                chart.ChartArea.Border.IsVisible = false;//chart图标边框不显示
                chart.ChartArea.Area.ForegroundColor = Color.White;
                chart.PlotArea.Area.ForegroundColor = Color.White;
                chart.PlotArea.Border.IsVisible = true;
                chart.PlotArea.Shadow = false;
                chart.ShowDataTable = true;  //显示模拟计算表
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
                chart.Title.Font.IsSubscript = true;
                chart.Title.Font.IsSuperscript = true;

                int count = Convert.ToInt32(6);
                //数字英语对照

                if (sheetName != "standrateH" && sheetName != "level")
                    chart.NSeries.Add(sheetName + "!B2:G3", false);
                if (sheetName == "standrateH")
                    chart.NSeries.Add(sheetName + "!B2:H3", false);

                //chart.NSeries.Add("Sheet1!B2:B5", false);

                //Set NSeries Category Datasource
                //chart.NSeries.CategoryData = "Sheet1!B1:" + Eng + "1";
                if (sheetName != "standrateH" && sheetName != "level")
                    chart.NSeries.CategoryData = sheetName + "!B1:G1";
                if (sheetName == "standrateH")
                    chart.NSeries.CategoryData = sheetName + "!B1:H1";

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
                if (sheetName == "level")
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
                chart.ValueAxis.AxisLine.IsVisible = false;

                chart.ValueAxis.Title.Text = title;
                chart.ValueAxis.Title.Font.Size = 19;
                chart.ValueAxis.Title.Font.Name = "宋体";
                chart.ValueAxis.Title.Font.IsBold = true;

                // chart.ValueAxis.TickLabels.TextDirection = TextDirectionType.LeftToRight;
                //设置Legend位置以及样式
                chart.ShowLegend = false;
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

        private void CreatePerChart(Workbook workbook, string title, string sheetName, ChartType ff, int Count)
        {
            try
            {
                //创建一个折线图
                Worksheet worksheet = workbook.Worksheets[0];
                worksheet.Charts.Add(ff, 1, 1, 25, 10);
                Aspose.Cells.Charts.Chart chart = workbook.Worksheets[0].Charts[0];
                chart.ChartArea.Border.IsVisible = false;//chart图标边框不显示
                chart.ChartArea.Area.ForegroundColor = Color.White;

                chart.PlotArea.Area.ForegroundColor = Color.White;
                chart.PlotArea.Border.IsVisible = false;
                chart.PlotArea.Shadow = false;
                chart.ValueAxis.Title.Font.IsSubscript = true;
                chart.ValueAxis.Title.Font.IsSuperscript = true;
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
                if (sheetName == "level")
                {
                    string str = "";
                    switch (Count)
                    {
                        case 1: str = "!A2:A2";
                            break;
                        case 2: str = "!A2:B2";
                            break;
                        case 3: str = "!A2:C2";
                            break;
                        case 4: str = "!A2:D2";
                            break;
                        case 5: str = "!A2:E2";
                            break;
                        case 6: str = "!A2:F2";
                            break;
                    }
                    chart.NSeries.Add(sheetName + str, false);
                }
                if (sheetName == "level")
                {
                    string str = "";
                    switch (Count)
                    {
                        case 1: str = "!A1:A1";
                            break;
                        case 2: str = "!A1:B1";
                            break;
                        case 3: str = "!A1:C1";
                            break;
                        case 4: str = "!A1:D1";
                            break;
                        case 5: str = "!A1:E1";
                            break;
                        case 6: str = "!A1:F1";
                            break;
                    }
                    chart.NSeries.CategoryData = sheetName + str;
                }


                Cells cells = workbook.Worksheets[0].Cells;
                Aspose.Cells.Charts.Series ser = chart.NSeries[0];

                //Apply the 3D formatting
                ShapePropertyCollection spPr = ser.ShapeProperties;
                Format3D fmt3d = spPr.Format3D;
                fmt3d.LightingAngle = 180;

                for (int i = 0; i < chart.NSeries.Count; i++)
                {
                    Series aSeries = chart.NSeries[i];
                    ChartPointCollection chartPoints = aSeries.Points;
                    for (int j = 0; j < chartPoints.Count; j++)
                    {
                        ChartPoint point = chartPoints[j];
                        point.Area.ForegroundColor = GetLevelColor(j);

                    }

                    chart.NSeries[i].DataLabels.ShowValue = true;
                    chart.NSeries[i].DataLabels.NumberFormat = "0.0%";
                    //chart.NSeries[i].DataLabels.ShowPercentage = true;  //百分位符号
                    chart.NSeries[i].DataLabels.TextFont.Color = Color.Black;
                    chart.NSeries[i].DataLabels.ShowCategoryName = false;
                    chart.NSeries[i].DataLabels.Font.Size = 16;
                 
                }
                #region 折线图
                //设置x轴上数据的样式为灰色
                chart.CategoryAxis.TickLabels.Font.Color = Color.White;
                chart.CategoryAxis.TickLabelPosition = TickLabelPositionType.NextToAxis;
                chart.CategoryAxis.HasMultiLevelLabels = true;
                //设置y轴的样式
                chart.ValueAxis.TickLabelPosition = TickLabelPositionType.Low;
                chart.ValueAxis.TickLabels.Font.Color = Color.White;
                chart.ValueAxis.TickLabels.NumberFormat = "";
                chart.ValueAxis.HasMultiLevelLabels = true;
                chart.ValueAxis.AxisLine.IsVisible = false;
                // chart.ValueAxis.TickLabels.TextDirection = TextDirectionType.LeftToRight;
                //设置Legend位置以及样式
                chart.ShowLegend = true;
                chart.Legend.Position = LegendPositionType.Bottom;
                chart.Legend.Border.IsVisible = false;
                chart.Legend.TextFont.Color = Color.Black;
                chart.Legend.Border.Color = Color.White;
                chart.Legend.Font.Size = 16;
                #endregion
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
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-01")).FirstDayOfMonth();
            DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate);
            dtpEnd.SelectedDate = DateTime.Now;
            System.DateTime dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM")).LastDayOfMonth();
            if (dtEnd > System.DateTime.Now)
                dtEnd = System.DateTime.Now;
            txtDateF.Text = dtBegin.ToString("yyyy年MM月dd日");
            txtDateT.Text = dtEnd.ToString("yyyy年MM月dd日");

            Bind();

            createimg();
            string[] imgSrcList1 = { "Cache/standrateH.jpg" };
            Repeater1.DataSource = imgSrcList1;
            Repeater1.DataBind();
            string[] imgSrcList2 = { "Cache/level.jpg" };
            Repeater2.DataSource = imgSrcList2;
            Repeater2.DataBind();
            string[] imgSrcList3 = { "Cache/a34004H.jpg" };
            Repeater3.DataSource = imgSrcList3;
            Repeater3.DataBind();
            string[] imgSrcList4 = { "Cache/a34002H.jpg" };
            Repeater4.DataSource = imgSrcList4;
            Repeater4.DataBind();
            string[] imgSrcList5 = { "Cache/a21004H.jpg" };
            Repeater5.DataSource = imgSrcList5;
            Repeater5.DataBind();
            string[] imgSrcList6 = { "Cache/a21026H.jpg" };
            Repeater6.DataSource = imgSrcList6;
            Repeater6.DataBind();
            string[] imgSrcList7 = { "Cache/a05024H.jpg" };
            Repeater7.DataSource = imgSrcList7;
            Repeater7.DataBind();
            string[] imgSrcList8 = { "Cache/a21005H.jpg" };
            Repeater8.DataSource = imgSrcList8;
            Repeater8.DataBind();
        }
        #endregion
        #region 绑定数据
        public void Bind()
        {
            DataTable dtnew = new DataTable();

            if (dtpBegin.SelectedDate == null)
            {
                return;
            }
            DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).FirstDayOfMonth();
            int month = dtBegin.Month;
            string title = "";
            if (month <= 6)
                title = "(" + dtBegin.ToString("yyyy") + "年上半年)";
            else
                title = "(" + dtBegin.ToString("yyyy") + "年下半年)";
            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd")).LastDayOfMonth();
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;

            DateTime startDate = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).FirstDayOfMonth();
            string timeRange = string.Format("{0:yyyy年MM月dd日} 到 {1:yyyy年MM月dd日}", startDate, endDate);
            titleText = "(" + timeRange + ")";

            var dataViewM = new DataView();
            dataViewM = m_YearAQIService.GetRegionsAllData(dtBegin, endDate);
            dtnew = dataViewM.ToTable();
            M1.InnerText = title;
            M2.InnerText = DateTime.Now.ToString("yyyy") + "年" + DateTime.Now.Month.ToString() + "月";
            string yearRange = dtBegin.ToString("yyyy") + "年" + dtBegin.Month.ToString() + "-" + endDate.Month.ToString() + "月，";
            string yearStr = dtBegin.ToString("yyyy") + "年" + dtBegin.Month.ToString() + "-" + endDate.Month.ToString() + "月";
            string yearBase = dtBegin.AddYears(-1).Year.ToString() + "年" + dtBegin.AddYears(-1).Month.ToString() + "-" + endDate.AddYears(-1).Month.ToString() + "月";
            M100.InnerText = yearStr + "全市各地环境空气质量达标天数比例";
            M99.InnerText = yearStr + "苏州市区环境空气质量各类别天数比例";
            M98.InnerText = yearStr + "全市各地细颗粒物平均浓度";
            M97.InnerText = yearStr + "全市各地可吸入颗粒物平均浓度";
            M96.InnerText = yearStr + "全市各地二氧化氮平均浓度";
            M95.InnerText = yearStr + "全市各地二氧化硫平均浓度";
            M94.InnerText = yearStr + "全市各地臭氧日最大8小时平均浓度超标率";
            M93.InnerText = yearStr + "全市各地一氧化碳平均浓度";
            M92.InnerText = yearBase + "苏州市区空气质量排名（从优到劣）情况";
            M91.InnerText = yearStr + "全市各地酸雨发生频率";
            string a = "";
            string b = "";
            string PM25str = "";
            string PM10str = "";
            string NO2str = "";
            string SO2str = "";
            string O3str = "";
            string COstr = "";
            if (dtnew.Rows.Count > 0)
            {
                a = yearRange + "全市环境空气质量达标天数比例为" + dtnew.Rows[0][0].ToString() + "，各地达标天数比例介于" + dtnew.Rows[0][1].ToString() + "之间；达标天数比例由高到低依次为" + dtnew.Rows[0][2].ToString() + "。全市空气质量指数（AQI）均值为" + dtnew.Rows[0][3].ToString()
                    + "，与上年同期相比" + dtnew.Rows[0][4].ToString() + "（上年同期全市AQI均值为" + dtnew.Rows[0][5].ToString() + "）。";
                M3.Text = a;

                b = yearRange + "苏州市区空气质量达标天数比例为" + dtnew.Rows[0][6].ToString() + "，其中优占" + dtnew.Rows[0][7].ToString() + "，良占" + dtnew.Rows[0][8].ToString() + "；超标天数比例为" + dtnew.Rows[0][9].ToString() + "。市区AQI均值为" +
                    dtnew.Rows[0][10].ToString() + "，与上年同期相比" + dtnew.Rows[0][11].ToString() + "，（上年同期市区AQI均值为" + dtnew.Rows[0][12].ToString() + "）。" + dtnew.Rows[0][13].ToString();
                M31.Text = b;

                PM25str = yearRange + "全市各地细颗粒物平均浓度分布在" + dtnew.Rows[0][14].ToString() + "。";
                M32.Text = PM25str;

                PM10str = yearRange + "全市各地可吸入颗粒物平均浓度分布在" + dtnew.Rows[0][15].ToString() + "。";
                M33.Text = PM10str;

                NO2str = yearRange + "全市各地二氧化氮平均浓度分布在" + dtnew.Rows[0][16].ToString() + "。";
                M34.Text = NO2str;

                SO2str = yearRange + "全市各地二氧化硫平均浓度分布在" + dtnew.Rows[0][17].ToString() + "。";
                M35.Text = SO2str;

                O3str = yearRange + dtnew.Rows[0][18].ToString() + "。";
                M36.Text = O3str;

                COstr = yearRange + "全市各地一氧化碳平均浓度分布在" + dtnew.Rows[0][19].ToString() + "。";
                M37.Text = COstr;

                M38.Text = yearRange + "【苏州市区环境空气质量在全国74个城市中排名平均为第47位，总体处于中等水平；在全省13个城市中排名平均为第8位，总体处于中等水平。】";
                M39.Text = yearRange + "【全市酸雨发生频率为37.1%，与上年同期相比，上升0.5个百分点。降水pH均值为5.12，酸度同比持平。市区酸雨发生频率为52.8％，同比上升19.1个百分点。】";
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

            if (dtpBegin.SelectedDate == null)
            {
                return;
            }
            DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).FirstDayOfMonth();
            int month = dtBegin.Month;
            string title = "";
            if (month <= 6)
                title = "(" + dtBegin.ToString("yyyy") + "年上半年)";
            else
                title = "(" + dtBegin.ToString("yyyy") + "年下半年)";
            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd")).LastDayOfMonth();
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;

            DateTime startDate = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).FirstDayOfMonth();
            string timeRange = string.Format("{0:yyyy年MM月dd日} 到 {1:yyyy年MM月dd日}", startDate, endDate);
            titleText = "(" + timeRange + ")";

            var dataViewM = new DataView();
            dataViewM = m_YearAQIService.GetRegionsAllData(dtBegin, endDate);
            dtnew = dataViewM.ToTable();
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AirQualittyHalfYearReport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToMergeField("M1");
            builder.Write(title);
            builder.MoveToMergeField("M2");
            builder.Write(DateTime.Now.ToString("yyyy") + "年" + DateTime.Now.Month.ToString() + "月");
            string yearRange = dtBegin.ToString("yyyy") + "年" + dtBegin.Month.ToString() + "-" + endDate.Month.ToString() + "月，";
            string yearStr = dtBegin.ToString("yyyy") + "年" + dtBegin.Month.ToString() + "-" + endDate.Month.ToString() + "月";
            builder.MoveToMergeField("M100");
            builder.Write(yearStr + "全市各地环境空气质量达标天数比例");
            builder.MoveToMergeField("M99");
            builder.Write(yearStr + "苏州市区环境空气质量各类别天数比例");
            builder.MoveToMergeField("M98");
            builder.Write(yearStr + "全市各地细颗粒物平均浓度");
            builder.MoveToMergeField("M97");
            builder.Write(yearStr + "全市各地可吸入颗粒物平均浓度");
            builder.MoveToMergeField("M96");
            builder.Write(yearStr + "全市各地二氧化氮平均浓度");
            builder.MoveToMergeField("M95");
            builder.Write(yearStr + "全市各地二氧化硫平均浓度");
            builder.MoveToMergeField("M94");
            builder.Write(yearStr + "全市各地臭氧日最大8小时平均浓度超标率");
            builder.MoveToMergeField("M93");
            builder.Write(yearStr + "全市各地一氧化碳平均浓度");
            builder.MoveToMergeField("M92");
            builder.Write(yearStr);
            builder.MoveToMergeField("M91");
            builder.Write(yearStr);
            string a = "";
            string b = "";
            string PM25str = "";
            string PM10str = "";
            string NO2str = "";
            string SO2str = "";
            string O3str = "";
            string COstr = "";
            string str1 = "";
            string str2 = "";
            if (dtnew.Rows.Count > 0)
            {
                a = yearRange + "全市环境空气质量达标天数比例为" + dtnew.Rows[0][0].ToString() + "，各地达标天数比例介于" + dtnew.Rows[0][1].ToString() + "之间；达标天数比例由高到低依次为" + dtnew.Rows[0][2].ToString() + "。全市空气质量指数（AQI）均值为" + dtnew.Rows[0][3].ToString()
                    + "，与上年同期相比" + dtnew.Rows[0][4].ToString() + "（上年同期全市AQI均值为" + dtnew.Rows[0][5].ToString() + "）。";
                builder.MoveToMergeField("M3");
                builder.Write(a);

                b = yearRange + "苏州市区空气质量达标天数比例为" + dtnew.Rows[0][6].ToString() + "，其中优占" + dtnew.Rows[0][7].ToString() + "，良占" + dtnew.Rows[0][8].ToString() + "；超标天数比例为" + dtnew.Rows[0][9].ToString() + "。市区AQI均值为" +
                    dtnew.Rows[0][10].ToString() + "，与上年同期相比" + dtnew.Rows[0][11].ToString() + "，（上年同期市区AQI均值为" + dtnew.Rows[0][12].ToString() + "）。" + dtnew.Rows[0][13].ToString();
                builder.MoveToMergeField("M31");
                builder.Write(b);

                PM25str = yearRange + "全市各地细颗粒物平均浓度分布在" + dtnew.Rows[0][14].ToString() + "。";
                builder.MoveToMergeField("M32");
                builder.Write(PM25str);

                PM10str = yearRange + "全市各地可吸入颗粒物平均浓度分布在" + dtnew.Rows[0][15].ToString() + "。";
                builder.MoveToMergeField("M33");
                builder.Write(PM10str);

                NO2str = yearRange + "全市各地二氧化氮平均浓度分布在" + dtnew.Rows[0][16].ToString() + "。";
                builder.MoveToMergeField("M34");
                builder.Write(NO2str);

                SO2str = yearRange + "全市各地二氧化硫平均浓度分布在" + dtnew.Rows[0][17].ToString() + "。";
                builder.MoveToMergeField("M35");
                builder.Write(SO2str);

                O3str = yearRange + dtnew.Rows[0][18].ToString() + "。";
                builder.MoveToMergeField("M36");
                builder.Write(O3str);

                COstr = yearRange + "全市各地一氧化碳平均浓度分布在" + dtnew.Rows[0][19].ToString() + "。";
                builder.MoveToMergeField("M37");
                builder.Write(COstr);

                str1 = yearRange + "苏州市区环境空气质量在全国74个城市中排名平均为第47位，总体处于中等水平；在全省13个城市中排名平均为第8位，总体处于中等水平。";
                builder.MoveToMergeField("M38");
                builder.Write(str1);

                str2 = yearRange + "全市酸雨发生频率为37.1%，与上年同期相比，上升0.5个百分点。降水pH均值为5.12，酸度同比持平。市区酸雨发生频率为52.8％，同比上升19.1个百分点。";
                builder.MoveToMergeField("M39");
                builder.Write(str2);
                doc.MailMerge.DeleteFields();

                //txtDateF.Text = dtBegin.ToString("yyyy年MM月dd日");
                //txtDateT.Text = dtEnd.ToString("yyyy年MM月dd日");
                string[] strPointCodes = { "" };
                string pointCodes = string.Join(";", strPointCodes);
                string[] strPointNames = { "" };
                string pointNames = string.Join(";", strPointNames);
                //添加实体类对象
                string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "AirQualittyHalfYearReport" + ".doc";
                ReportLogEntity customDatum = new ReportLogEntity();
                customDatum.PointIds = pointCodes;//测点Code
                customDatum.PointNames = pointNames;//测点名称
                customDatum.FactorCodes = "";//因子Code
                customDatum.FactorsNames = "";//因子名称
                customDatum.DateTimeRange = dtpBegin.SelectedDate.Value.Year.ToString() + "年" + dtpBegin.SelectedDate.Value.Month.ToString() + "月";
                customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
                customDatum.PageTypeID = "AirQualittyHalfYearReport";//页面ID
                customDatum.StartDateTime = dtBegin;
                customDatum.EndDateTime = endDate;
                customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
                customDatum.ExportName = title + "苏州市环境质量分析报告";
                customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/AirQualittyHalfYearReport/" + dtpBegin.SelectedDate.Value.Year + "/" + dtpBegin.SelectedDate.Value.Month + "/" + filename).ToString();
                customDatum.CreatDateTime = DateTime.Now;
                //添加数据
                ReportLogService.ReportLogAdd(customDatum);

                string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/AirQualittyHalfYearReport/" + dtpBegin.SelectedDate.Value.Year + "/" + dtpBegin.SelectedDate.Value.Month + "/" + filename);
                doc.Save(strTarget);
                doc.Save(this.Response, "AirQualittyHalfYearReport.doc", Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Docx));
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
            string[] factors = { "a34004H", "a34002H", "a21026H", "a21004H", "a05024H", "a21005H", "standrateH", "level" };

            if (dtpBegin.SelectedDate == null)
            {
                return;
            }
            DataTable dtnew = new DataTable();

            if (dtpBegin.SelectedDate == null)
            {
                return;
            }
            DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).FirstDayOfMonth();
            int month = dtBegin.Month;
            string title = "";
            if (month <= 6)
                title = "(" + dtBegin.ToString("yyyy") + "年上半年)";
            else
                title = "(" + dtBegin.ToString("yyyy") + "年下半年)";
            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd")).LastDayOfMonth();
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;

            DateTime startDate = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).FirstDayOfMonth();
            string timeRange = string.Format("{0:yyyy年MM月dd日} 到 {1:yyyy年MM月dd日}", startDate, endDate);
            titleText = "(" + timeRange + ")";

            var dataViewM = new DataView();
            dataViewM = m_YearAQIService.GetRegionsAllData(dtBegin, endDate);
            dtnew = dataViewM.ToTable();
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AirQualittyHalfYearReport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);
            #region 图片插入Word
            //for (int n = 8; n <= 9; n++)
            //{
            //    string mark = "p" + n.ToString();
            //    Aspose.Words.Drawing.Shape shape2 = new Aspose.Words.Drawing.Shape(doc, ShapeType.TextBox);
            //}
            foreach (string factor in factors)
            {
                string mark = factor;

                Aspose.Words.Drawing.Shape shape2 = new Aspose.Words.Drawing.Shape(doc, ShapeType.Image);
                shape2.ImageData.SetImage(Server.MapPath("Cache/" + factor + ".jpg"));


                if (factor != "level")
                {
                    shape2.Width = 421;
                    shape2.Height = 180;
                }
                else
                {
                    shape2.Width = 240;
                    shape2.Height = 240;
                }
                shape2.HorizontalAlignment = HorizontalAlignment.Center;
                shape2.BehindText = true;
                shape2.WrapType = WrapType.Tight;
                if (doc.Range.Bookmarks[mark] != null)
                {
                    builder.MoveToBookmark(mark);
                    builder.InsertNode(shape2);
                    if (factor == "level")
                    {
                        builder.Writeln();
                        builder.Writeln();
                        builder.Writeln();
                        builder.Writeln();
                        builder.Writeln();
                        builder.Writeln();
                        builder.Writeln();
                        builder.Writeln();
                        builder.Writeln();
                    }
                }

            }

            #endregion
            builder.MoveToMergeField("M1");
            builder.Write(title);
            builder.MoveToMergeField("M2");
            builder.Write(DateTime.Now.ToString("yyyy") + "年" + DateTime.Now.Month.ToString() + "月");
            string yearRange = dtBegin.ToString("yyyy") + "年" + dtBegin.Month.ToString() + "-" + endDate.Month.ToString() + "月，";
            string yearStr = dtBegin.ToString("yyyy") + "年" + dtBegin.Month.ToString() + "-" + endDate.Month.ToString() + "月";

            builder.MoveToMergeField("M100");
            builder.Write(yearStr + "全市各地环境空气质量达标天数比例");
            builder.MoveToMergeField("M99");
            builder.Write(yearStr + "苏州市区环境空气质量各类别天数比例");
            builder.MoveToMergeField("M98");
            builder.Write(yearStr + "全市各地细颗粒物平均浓度");
            builder.MoveToMergeField("M97");
            builder.Write(yearStr + "全市各地可吸入颗粒物平均浓度");
            builder.MoveToMergeField("M96");
            builder.Write(yearStr + "全市各地二氧化氮平均浓度");
            builder.MoveToMergeField("M95");
            builder.Write(yearStr + "全市各地二氧化硫平均浓度");
            builder.MoveToMergeField("M94");
            builder.Write(yearStr + "全市各地臭氧日最大8小时平均浓度超标率");
            builder.MoveToMergeField("M93");
            builder.Write(yearStr + "全市各地一氧化碳平均浓度");
            builder.MoveToMergeField("M92");
            builder.Write(yearStr);
            builder.MoveToMergeField("M91");
            builder.Write(yearStr);
            string a = "";
            string b = "";
            string PM25str = "";
            string PM10str = "";
            string NO2str = "";
            string SO2str = "";
            string O3str = "";
            string COstr = "";
            string str1 = "";
            string str2 = "";
            if (dtnew.Rows.Count > 0)
            {
                a = yearRange + "全市环境空气质量达标天数比例为" + dtnew.Rows[0][0].ToString() + "，各地达标天数比例介于" + dtnew.Rows[0][1].ToString() + "之间；达标天数比例由高到低依次为" + dtnew.Rows[0][2].ToString() + "。全市空气质量指数（AQI）均值为" + dtnew.Rows[0][3].ToString()
                    + "，与上年同期相比" + dtnew.Rows[0][4].ToString() + "（上年同期全市AQI均值为" + dtnew.Rows[0][5].ToString() + "）。";
                builder.MoveToMergeField("M3");
                builder.Write(a);

                b = yearRange + "苏州市区空气质量达标天数比例为" + dtnew.Rows[0][6].ToString() + "，其中优占" + dtnew.Rows[0][7].ToString() + "，良占" + dtnew.Rows[0][8].ToString() + "；超标天数比例为" + dtnew.Rows[0][9].ToString() + "。市区AQI均值为" +
                    dtnew.Rows[0][10].ToString() + "，与上年同期相比" + dtnew.Rows[0][11].ToString() + "，（上年同期市区AQI均值为" + dtnew.Rows[0][12].ToString() + "）。" + dtnew.Rows[0][13].ToString();
                builder.MoveToMergeField("M31");
                builder.Write(b);

                PM25str = yearRange + "全市各地细颗粒物平均浓度分布在" + dtnew.Rows[0][14].ToString() + "。";
                builder.MoveToMergeField("M32");
                builder.Write(PM25str);

                PM10str = yearRange + "全市各地可吸入颗粒物平均浓度分布在" + dtnew.Rows[0][15].ToString() + "。";
                builder.MoveToMergeField("M33");
                builder.Write(PM10str);

                NO2str = yearRange + "全市各地二氧化氮平均浓度分布在" + dtnew.Rows[0][16].ToString() + "。";
                builder.MoveToMergeField("M34");
                builder.Write(NO2str);

                SO2str = yearRange + "全市各地二氧化硫平均浓度分布在" + dtnew.Rows[0][17].ToString() + "。";
                builder.MoveToMergeField("M35");
                builder.Write(SO2str);

                O3str = yearRange + dtnew.Rows[0][18].ToString() + "。";
                builder.MoveToMergeField("M36");
                builder.Write(O3str);

                COstr = yearRange + "全市各地一氧化碳平均浓度分布在" + dtnew.Rows[0][19].ToString() + "。";
                builder.MoveToMergeField("M37");
                builder.Write(COstr);

                str1 = yearRange + "苏州市区环境空气质量在全国74个城市中排名平均为第47位，总体处于中等水平；在全省13个城市中排名平均为第8位，总体处于中等水平。";
                builder.MoveToMergeField("M38");
                builder.Write(str1);

                str2 = yearRange + "全市酸雨发生频率为37.1%，与上年同期相比，上升0.5个百分点。降水pH均值为5.12，酸度同比持平。市区酸雨发生频率为52.8％，同比上升19.1个百分点。";
                builder.MoveToMergeField("M39");
                builder.Write(str2);
                doc.MailMerge.DeleteFields();
                //    DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-01"));  //本月第一天
                endDate = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd"));   //本月当天
                string[] strPointCodes = { };//站点Code
                string pointCodes = string.Join(";", strPointCodes);
                string[] strPointNames = { };//站点名称
                string pointNames = string.Join(";", strPointNames);
                string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "AirQualittyHalfYearReport" + ".doc";
                //添加实体类对象
                ReportLogEntity customDatum = new ReportLogEntity();
                customDatum.PointIds = "7e05b94c-bbd4-45c3-919c-42da2e63fd43;66d2abd1-ca39-4e39-909f-da872704fbfd;d7d7a1fe-493a-4b3f-8504-b1850f6d9eff;57b196ed-5038-4ad0-a035-76faee2d7a98;2e2950cd-dbab-43b3-811d-61bd7569565a;2fea3cb2-8b95-45e6-8a71-471562c4c89c";//测点Code
                customDatum.PointNames = "市区均值;张家港;常熟市;太仓市;昆山市;吴江区";//测点名称
                customDatum.FactorCodes = "a21026;a21004;a34002;a34004;a21005;a05024";//因子Code
                customDatum.FactorsNames = "二氧化硫;二氧化氮;PM10;PM2.5;一氧化碳;臭氧";//因子名称
                customDatum.DateTimeRange = dtpBegin.SelectedDate.Value.Year.ToString() + "年" + dtpBegin.SelectedDate.Value.Month.ToString() + "月";
                customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
                customDatum.PageTypeID = "AirQualittyHalfYearReport";//页面ID
                customDatum.StartDateTime = dtBegin;
                customDatum.EndDateTime = endDate;
                customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
                customDatum.ExportName = title + "苏州市环境质量分析报告";
                customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/AirQualittyHalfYearReport/" + dtpBegin.SelectedDate.Value.Year + "/" + dtpBegin.SelectedDate.Value.Month + "/" + filename).ToString();
                customDatum.CreatDateTime = DateTime.Now;
                //添加数据
                ReportLogService.ReportLogAdd(customDatum);

                string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/AirQualittyHalfYearReport/" + dtpBegin.SelectedDate.Value.Year + "/" + dtpBegin.SelectedDate.Value.Month + "/" + filename);
                doc.Save(strTarget);
                string pageid = "AirBulletin";
                Dictionary<string, string> pcontent = new Dictionary<string, string>();
                List<string> r = new List<string>();
                if (M3.Text != "")
                {
                    r.Add("3");
                    pcontent.Add("3", M3.Text);
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
                if (M38.Text != "")
                {
                    r.Add("38");
                    pcontent.Add("38", M38.Text);
                }
                if (M39.Text != "")
                {
                    r.Add("39");
                    pcontent.Add("39", M39.Text);
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
        protected void dtpBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int yearF = dtpBegin.SelectedDate.Value.Year;
            int yearT = dtpEnd.SelectedDate.Value.Year;
            if (yearF != yearT)
            {
                int sub = yearF - yearT;
                dtpEnd.SelectedDate = dtpEnd.SelectedDate.Value.AddYears(+sub);
            }
            if (dtpBegin.SelectedDate.ToString() != "")
            {
                DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).FirstDayOfMonth();
                System.DateTime dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd")).LastDayOfMonth();
                if (dtEnd > System.DateTime.Now)
                    dtEnd = System.DateTime.Now;
                txtDateF.Text = dtBegin.ToString("yyyy年MM月dd日");
                txtDateT.Text = dtEnd.ToString("yyyy年MM月dd日");
            }
            else
            {
                Alert("时间不能为空");
                return;
            }
            Bind();
            createimg();
        }

        protected void dtpEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int yearF = dtpBegin.SelectedDate.Value.Year;
            int yearT = dtpEnd.SelectedDate.Value.Year;
            if (yearF != yearT)
            {
                int sub = yearT - yearF;
                dtpBegin.SelectedDate = dtpBegin.SelectedDate.Value.AddYears(+sub);
            }
            if (dtpBegin.SelectedDate.ToString() != "")
            {
                DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).FirstDayOfMonth();
                System.DateTime endDate = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd")).LastDayOfMonth();
                if (endDate > System.DateTime.Now)
                    endDate = System.DateTime.Now;
                txtDateF.Text = dtBegin.ToString("yyyy年MM月dd日");
                txtDateT.Text = endDate.ToString("yyyy年MM月dd日");
            }
            else
            {
                Alert("时间不能为空");
                return;
            }
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
            }
            return c;
        }
        private Color GetLevelColor(int i)
        {
            Color color = new Color();
            switch (i)
            {
                case 0:
                    color = Color.Green;
                    break;
                case 1:
                    color = Color.Yellow;
                    break;
                case 2:
                    color = Color.Orange;
                    break;
                case 3:
                    color = Color.Red;
                    break;
                case 4:
                    color = Color.Purple;
                    break;
                case 5:
                    color = Color.Black;
                    break;
            }
            return color;
        }
    }
}