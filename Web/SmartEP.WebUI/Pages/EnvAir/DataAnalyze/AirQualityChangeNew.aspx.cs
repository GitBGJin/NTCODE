using Aspose.Cells;
using Aspose.Words;
using Aspose.Words.Tables;
using log4net;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Service.Frame;
using SmartEP.Utilities.Office;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class AirQualityChangeNew : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private ConcentrationAQIService m_ConcentrationAQIService;
        DictionaryService dicService = new DictionaryService();
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        ConcentrationAQIService concentrationAQIService = new ConcentrationAQIService();
        DataQueryByDayService m_DataQueryByDayService = Singleton<DataQueryByDayService>.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            m_ConcentrationAQIService = new ConcentrationAQIService();
            if (!IsPostBack)
            {
                InitControl();
                DataTable dtn = new DataTable();
                dtn.Columns.Add("PointName", typeof(string));
                dtn.Columns.Add("LastP", typeof(string));
                dtn.Columns.Add("ThisP", typeof(string));
                dtn.Columns.Add("PCompare", typeof(string));
                dtn.Columns.Add("LastC", typeof(string));
                dtn.Columns.Add("ThisC", typeof(string));
                dtn.Columns.Add("CCompare", typeof(string));
                grdAvgDayRange.DataSource = dtn.DefaultView;
                //grdAvgDayRange.VirtualItemCount = 0;
                timer.Enabled = true;
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            rdpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01"));
            rdpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            //rdpBegin.SelectedDate = Convert.ToDateTime("2017-08-01");
            //rdpEnd.SelectedDate = Convert.ToDateTime("2017-08-31");

            int yearNow = DateTime.Now.AddYears(-1).Year;
            int year = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Year"]);
            for (int i = yearNow; i > year; i--)
            {
                ListItem li = new ListItem();
                li.Value = i.ToString();
                li.Text = i.ToString();
                SelectYear.Items.Add(li);
            }
        }
        #endregion
        protected void timer_Tick(object sender, EventArgs e)
        {
            grdAvgDayRange.CurrentPageIndex = 0;
            grdAvgDayRange.Rebind();
            timer.Enabled = false;
        }
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            //DataTable dt=new DataTable();
            //dt.Columns.Add("PointId", typeof(string));
            //dt.Columns.Add("ComparisonYearP", typeof(string));
            //dt.Columns.Add("ThisYearP", typeof(string));
            //dt.Columns.Add("PollutionCompare", typeof(string));
            //dt.Columns.Add("ComparisonYearC", typeof(string));
            //dt.Columns.Add("ThisYearC", typeof(string));
            //dt.Columns.Add("ComplianceCompare", typeof(string));

            //每页显示数据个数            
            int pageSize = grdAvgDayRange.PageSize;
            //当前页的序号
            int pageNo = grdAvgDayRange.CurrentPageIndex;
            int recordTotal = 0;
            string[] PointId = new string[] { "187", "188", "189", "190", "191", "192", "193", "194", "195", "196", "197", "198", "199", "200", "201", "202", "203" };
            DateTime mBegion = rdpBegin.SelectedDate.Value;
            DateTime mEnd = rdpEnd.SelectedDate.Value;
            //SelectYear
            string key = ReportType.SelectedValue;
            DataView dvs = m_ConcentrationAQIService.GetAirQualityChangePager(PointId, ReportType.SelectedValue, mBegion, mEnd, SelectYear.Value, pageSize, pageNo, out recordTotal);
            recordTotal = dvs.ToTable().Rows.Count;
            //数据总行数
            grdAvgDayRange.VirtualItemCount = recordTotal;
            grdAvgDayRange.DataSource = dvs;
        }

        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
            }
        }
        /// <summary>
        /// grid生成列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAvgDayRange_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                //dt.Columns.Add("PointName", typeof(string));    //城市
                //dt.Columns.Add("Density", typeof(string));      //浓度值
                //dt.Columns.Add("DCompare", typeof(string));     //同比变化情况
                //dt.Columns.Add("Drop", typeof(string));         //降幅
                //dt.Columns.Add("Proportion", typeof(string));   //比例
                //dt.Columns.Add("PCompares", typeof(string));     //同比变化情况
                //dt.Columns.Add("Increase", typeof(string));     //升幅
                //dt.Columns.Add("Evaluation", typeof(string));   //综合评价
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                if (col.DataField == "PointName")
                {
                    col.HeaderText = "地区";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Evaluation")
                {
                    col.HeaderText = "综合评价"; 
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Increase")
                {
                    col.HeaderText = "升幅目标(百分点)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "PCompares")
                {
                    col.HeaderText = "同比变化情况(百分点)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Proportion")
                {
                    col.HeaderText = "比例（%）";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Drop")
                {
                    col.HeaderText = "降幅目标(%)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "DCompare")
                {
                    col.HeaderText = "同比变化情况(%)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Density")
                {
                    col.HeaderText = "PM2.5浓度值(μg/m³)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "LastP")
                {
                    col.HeaderText = "比对年份数据(重污染天数)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "ThisP")
                {
                    col.HeaderText = "本期年份数据(重污染天数)";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField.Contains("PCompare"))
                {
                    col.HeaderText = "与比对年份比较(重污染天数)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    //col.ColumnGroupName = "PM25";
                }
                else if (col.DataField.Contains("LastC"))
                {
                    col.HeaderText = "比对年份数据(达标率)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    //col.ColumnGroupName = "PM10";
                }
                else if (col.DataField.Contains("ThisC"))
                {
                    col.HeaderText = "本期年份数据(达标率)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    //col.ColumnGroupName = "NO2";
                }
                else if (col.DataField.Contains("CCompare"))
                {
                    col.HeaderText = "与比对年份比较(达标率)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    //col.ColumnGroupName = "SO2";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (rdpBegin.SelectedDate.Value.Year != rdpEnd.SelectedDate.Value.Year)
            {
                Alert("时间范围不能跨年！");
                return;
            }
            grdAvgDayRange.CurrentPageIndex = 0;
            grdAvgDayRange.Rebind();
        }

        #region 报表设计
        /// <summary>
        /// 月报表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF1(DocumentBuilder builder, DataTable dt, DateTime dts, DateTime dte, String year)
        {
            try
            {
                builder.Font.ClearFormatting();
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.HorizontalMerge = Aspose.Words.Tables.CellMerge.None;
                builder.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Font.Size = 10;
                List<double> widthList = new List<double>();
                double width, widths = 0;
                width = 70;
                widths = 45.380;
                for (int i = 0; i <= dt.Rows.Count + 1; i++)
                {
                    if (i == 0)
                    {
                        builder.Font.Bold = true;
                        for (int j = 0; j < 7; j++)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.Width = width;
                            switch (j)
                            {
                                case 0:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;//垂直合并开始 
                                    //builder.CellFormat.VerticalMerge = CellMerge.Previous;//垂直合并结束
                                    //builder.Write("地区");
                                    break;
                                case 1:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;//垂直合并开始 
                                    //builder.CellFormat.HorizontalMerge = CellMerge.First;//水平合并开始  
                                    builder.Write("重污染天数(天)");
                                    break;
                                case 2:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    //builder.CellFormat.HorizontalMerge = CellMerge.Previous;//水平合并结束
                                    builder.Write("重污染天数(天)");
                                    break;
                                case 3:
                                    //builder.CellFormat.VerticalMerge = CellMerge.Previous;//垂直合并结束
                                    //builder.CellFormat.HorizontalMerge = CellMerge.Previous;//水平合并结束
                                    builder.Write("重污染天数(天)");
                                    break;
                                case 4:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;//垂直合并开始 
                                    //builder.CellFormat.HorizontalMerge = CellMerge.First;//水平合并开始
                                    builder.Write("AQI达标率(%)");
                                    break;
                                case 5:
                                    builder.Write("AQI达标率(%)");
                                    break;
                                case 6:
                                    //builder.CellFormat.VerticalMerge = CellMerge.Previous;//垂直合并结束
                                    //builder.CellFormat.HorizontalMerge = CellMerge.Previous;//水平合并结束
                                    builder.Write("AQI达标率(%)");
                                    break;
                            }
                        }
                        builder.EndRow();
                    }
                    else if (i == 1)
                    {
                        builder.Font.Bold = true;
                        for (int j = 0; j < 7; j++)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.Width = width;
                            switch (j)
                            {
                                case 0:
                                    //builder.CellFormat.VerticalMerge = CellMerge.Previous;//垂直合并结束
                                    builder.Write("地区");
                                    break;
                                case 1:
                                    //builder.CellFormat.HorizontalMerge = CellMerge.First;//水平合并开始  
                                    builder.Write(year + "年（" + dts.Month + "." + dts.Day + "-" + dte.Month + "." + dte.Day + ")");
                                    break;
                                case 2:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write(dts.Year + "年（" + dts.Month + "." + dts.Day + "-" + dte.Month + "." + dte.Day + ")");
                                    break;
                                case 3:
                                    builder.Write("与" + year + "年同期比较");
                                    break;
                                case 4:
                                    builder.Write(year + "年（" + dts.Month + "." + dts.Day + "-" + dte.Month + "." + dte.Day + ")");
                                    //builder.Write("AQI达标率(%)");
                                    break;
                                case 5:
                                    builder.Write(dts.Year + "年（" + dts.Month + "." + dts.Day + "-" + dte.Month + "." + dte.Day + ")");
                                    break;
                                case 6:
                                    builder.Write("与" + year + "年同期比较");
                                    break;
                            }
                        }
                        builder.EndRow();
                    }
                    else
                    {
                        builder.Font.Bold = true;
                        for (int j = 0; j < 7; j++)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.Width = width;
                            builder.Write(dt.Rows[i - 2][j].ToString());
                        }
                        builder.EndRow();
                    }
                }

                builder.EndTable();
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        /// <summary>
        /// 月报表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF1New(DocumentBuilder builder, DataTable dt, DateTime dts, DateTime dte, String year)
        {
            try
            {
                builder.Font.ClearFormatting();
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Font.Size = 10;
                List<double> widthList = new List<double>();
                double width, widths = 0;
                width = 70;
                widths = 45.380;
                for (int i = 0; i <= dt.Rows.Count + 2; i++)
                {
                    if (i == 0)
                    {
                        builder.Font.Bold = true;
                        for (int j = 0; j < 8; j++)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.HorizontalMerge = CellMerge.None;
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.CellFormat.Width = width;
                            switch (j)
                            {
                                case 0:
                                    builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("城市");
                                    break;
                                case 1:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;//垂直合并开始 
                                    builder.CellFormat.HorizontalMerge = CellMerge.First;//水平合并开始  
                                    builder.Write("PM2.5");
                                    break;
                                case 2:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                    //builder.Write("重污染天数(天)");
                                    //builder.Write("PM2.5");
                                    break;
                                case 3:
                                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                    //builder.Write("PM2.5");
                                    break;
                                case 4:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;//垂直合并开始 
                                    builder.CellFormat.HorizontalMerge = CellMerge.First;//水平合并开始
                                    builder.Write("优良天数比例");
                                    break;
                                case 5:
                                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;//水平合并开始
                                    //builder.Write("优良天数比例");
                                    break;
                                case 6:
                                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;//水平合并开始
                                    //builder.Write("优良天数比例");
                                    break;
                                case 7:
                                    //builder.CellFormat.HorizontalMerge = CellMerge.Previous;//水平合并开始
                                    builder.Write("综合评价");
                                    break;
                            }
                        }
                        builder.EndRow();
                    }
                    else if (i == 1)
                    {
                        builder.Font.Bold = true;
                        for (int j = 0; j < 8; j++)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.HorizontalMerge = CellMerge.None;
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.CellFormat.Width = width;
                            switch (j)
                            {
                                case 0:
                                    builder.CellFormat.VerticalMerge = CellMerge.Previous;//垂直合并结束
                                    //builder.Write("地区");
                                    break;
                                case 1:
                                    builder.CellFormat.HorizontalMerge = CellMerge.First;//水平合并开始  
                                    builder.Write("现状");
                                    break;
                                case 2:
                                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                    //builder.Write("现状");
                                    break;
                                case 3:
                                    builder.Write(DateTime.Now.Year + "年确保目标");
                                    break;
                                case 4:
                                    builder.CellFormat.HorizontalMerge = CellMerge.First;//水平合并开始  
                                    builder.Write("现状");
                                    break;
                                case 5:
                                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                    //builder.Write("现状");
                                    break;
                                case 6:
                                    builder.Write(DateTime.Now.Year + "年确保目标");
                                    break;
                                case 7:
                                    builder.Write("综合评价");
                                    break;
                            }
                        }
                        builder.EndRow();
                    }
                    else if (i == 2)
                    {
                        builder.Font.Bold = true;
                        for (int j = 0; j < 8; j++)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.HorizontalMerge = CellMerge.None;
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.CellFormat.Width = width;
                            switch (j)
                            {
                                case 0:
                                    builder.CellFormat.VerticalMerge = CellMerge.Previous;//垂直合并结束
                                    //builder.Write("地区");
                                    break;
                                case 1:
                                    //builder.CellFormat.HorizontalMerge = CellMerge.First;//水平合并开始  
                                    builder.Write("浓度值(μg/m³)");
                                    break;
                                case 2:
                                    //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                    builder.Write("同比变化情况（%）");
                                    break;
                                case 3:
                                    builder.Write("同比降幅（%）");
                                    break;
                                case 4:
                                    //builder.CellFormat.HorizontalMerge = CellMerge.First;//水平合并开始  
                                    builder.Write("比例（%）");
                                    break;
                                case 5:
                                    builder.Write("同比变化情况（百分点）");
                                    break;
                                case 6:
                                    builder.Write("同比升幅（百分点）");
                                    break;
                                case 7:
                                    builder.Write("综合评价");
                                    break;
                            }
                        }
                        builder.EndRow();
                    }
                    else
                    {
                        builder.Font.Bold = true;
                        for (int j = 0; j < 8; j++)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.Width = width;
                            builder.Write(dt.Rows[i - 3][j].ToString());
                        }
                        builder.EndRow();
                    }
                }

                builder.EndTable();
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        public void MoveToMF2(DocumentBuilder builder, DateTime dte)
        {
            //builder.InsertCell();
            //builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Write(dte.Year + "年第" + dte.Month + "期");
            //builder.EndRow();
            //builder.EndTable();
        }

        public void MoveToMF3(DocumentBuilder builder)
        {
            //builder.InsertCell();
            //builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Write(DateTime.Now.ToString("yyyy年MM月dd日"));
            //builder.EndRow();
            //builder.EndTable();
        }

        public void MoveToMF4(DocumentBuilder builder, DateTime dte)
        {
            //builder.InsertCell();
            //builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Write(dte.ToString("MM月dd日"));
            //builder.EndRow();
            //builder.EndTable();
        }
        #endregion


        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            try
            {
                int pagesize = int.MaxValue;
                int pageno = 0;
                int recordTotal = 0;
                string[] PointId = new string[] { "187", "188", "189", "190", "191", "192", "193", "194", "195", "196", "197", "198", "199", "200", "201", "202", "203" };
                DateTime mBegion = rdpBegin.SelectedDate.Value;
                DateTime mEnd = rdpEnd.SelectedDate.Value;
                //SelectYear

                DataView dvs = m_ConcentrationAQIService.GetAirQualityChangePager(PointId, ReportType.SelectedValue, mBegion, mEnd, SelectYear.Value, pagesize, pageno, out recordTotal);
                string filename = string.Empty;
                Document doc = new Document();
                if (ReportType.SelectedValue == "Rate")
                {
                    string path = "D:\\Sinoyd\\南通超级站\\南通20170908\\南通\\Files";
                    doc = new Document(System.IO.Path.Combine(path, "月报.docx"));
                    DocumentBuilder builder = new DocumentBuilder(doc);
                    builder.MoveToMergeField("MF1");
                    MoveToMF1(builder, dvs.ToTable(), mBegion, mEnd, SelectYear.Value);
                    builder.MoveToMergeField("MF2");
                    MoveToMF2(builder, mEnd);
                    builder.MoveToMergeField("MF3");
                    MoveToMF3(builder);
                    builder.MoveToMergeField("MF4");
                    MoveToMF4(builder, mEnd);
                    doc.MailMerge.DeleteFields();
                    filename = "各县(市)、区达标率情况月报.docx";
                }
                else
                {
                    string path = "D:\\Sinoyd\\南通超级站\\南通20170908\\南通\\Files";
                    doc = new Document(System.IO.Path.Combine(path, "空气质量变化.docx"));
                    DocumentBuilder builder = new DocumentBuilder(doc);
                    builder.MoveToMergeField("MF1");
                    MoveToMF1New(builder, dvs.ToTable(), mBegion, mEnd, SelectYear.Value);

                    builder.MoveToMergeField("MF2");
                    MoveToMF2(builder, mEnd);
                    builder.MoveToMergeField("MF3");
                    MoveToMF3(builder);
                    builder.MoveToMergeField("MF4");
                    MoveToMF4(builder, mEnd);
                    doc.MailMerge.DeleteFields();
                    filename = "各县(市)、区空气质量变化情况月报.docx";
                }
                //DataView dv = new DataView();

                string strTarget = "C:\\Users\\Administrator\\Desktop\\" + filename;
                strTarget = Server.MapPath("../" + filename);
                doc.Save(strTarget);
                doc.Save(this.Response, filename, Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Docx));
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
    }
}