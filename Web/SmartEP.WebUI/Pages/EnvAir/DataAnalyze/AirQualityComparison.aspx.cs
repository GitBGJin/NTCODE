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
    public partial class AirQualityComparison : SmartEP.WebUI.Common.BasePage
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
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_ConcentrationAQIService = new ConcentrationAQIService();
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
            rdpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            rdpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));

            Year.Items.Clear();//年开始
            int yearNow = DateTime.Now.AddYears(-1).Year;
            int year = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Year"]);
            for (int i = yearNow; i > year; i--)
            {
                RadComboBoxItem cmbItemYearBegin = new RadComboBoxItem();
                cmbItemYearBegin.Text = i.ToString();
                cmbItemYearBegin.Value = i.ToString();
                if (i == yearNow - 1)
                    cmbItemYearBegin.Checked = true;
                Year.Items.Add(cmbItemYearBegin);
            }
            Year.DataBind();
            
            string names = ConfigurationManager.AppSettings["NTRegionPointName"].ToString();    //从配置文件获取默认站点
            string namess = ConfigurationManager.AppSettings["NTRegionPointNames"].ToString();
            pointCbxRsm.SetPointValuesFromNames(namess);
            pointCbxRsmCity.SetPointValuesFromNames(names);

        }
        #endregion
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            string[] portIds=new string[]{};
            DateTime mBegion = rdpBegin.SelectedDate.Value;
            DateTime mEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.AddMonths(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"));
            //每页显示数据个数            
            int pageSize = int.MaxValue;
            int pageNo = 0;
            int recordTotal = 0;
            string yearAll = "";
            foreach (RadComboBoxItem item in Year.CheckedItems)
            {
                yearAll = ("," + item.Value.ToString()) + yearAll;
            }
            string[] years = yearAll.TrimStart(',').Split(',');
            DataView dv = new DataView();
            if (rbtnlType.SelectedValue == "CityProper")
            {
                portIds = pointCbxRsmCity.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                dv = concentrationAQIService.GetCityProperComparisonPager(portIds, mBegion, mEnd, years, pageSize, pageNo, out recordTotal);  // 本期
            }
            else
            {
                portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                dv = concentrationAQIService.GetComparisonPager(portIds, mBegion, mEnd, years, pageSize, pageNo, out recordTotal);  // 本期
            }
            if(dv.Count>0)
            {
                grdAvgDayRange.DataSource = dv;
                grdAvgDayRange.VirtualItemCount = dv.Count;
            }
            else
            {
                grdAvgDayRange.DataSource = new DataView();
                grdAvgDayRange.VirtualItemCount = dv.Count;
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
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;

                if (col.DataField == "Compliance")
                {
                    col.HeaderText = "AQI达标率(%)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Tstamp")
                {
                    col.HeaderText = "时间";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField.Contains("Excellent"))
                {
                    col.HeaderText = "优(天)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    //col.ColumnGroupName = "PM25";
                }
                else if (col.DataField.Contains("Good"))
                {
                    col.HeaderText = "良(天)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    //col.ColumnGroupName = "PM10";
                }
                else if (col.DataField.Contains("Light"))
                {
                    col.HeaderText = "轻度污染(天)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    //col.ColumnGroupName = "NO2";
                }
                else if (col.DataField.Contains("Moderate"))
                {
                    col.HeaderText = "中度污染(天)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    //col.ColumnGroupName = "SO2";
                }
                else if (col.DataField.Contains("Severe"))
                {
                    col.HeaderText = "重度污染(天)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    //col.ColumnGroupName = "CO";
                }
                else if (col.DataField.Contains("SO2") || col.DataField.Contains("NO2") || col.DataField.Contains("O3") || col.DataField.Contains("PM10") || col.DataField.Contains("PM2.5"))
                {
                    col.HeaderText = col.DataField+"浓度(ug/m3)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    //col.ColumnGroupName = "O3";
                }
                else if (col.DataField.Contains("CO"))
                {
                    col.HeaderText = col.DataField + "浓度(mg/m3)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    //col.ColumnGroupName = "AQI";
                }
            }
            catch (Exception ex) 
            {
                log.Error(ex.ToString());
            }
        }
        #endregion
        #region 保存记录
        /// <summary>
        /// 保存记录
        /// </summary>
        public void save(Document doc)
        {
            try
            {
                string filename = "空气质量比对报表.doc";
                string strTarget = "C:\\Users\\Administrator\\Desktop\\" + filename;
                //customDatumData.UpdateDateTime = DateTime.Now;
                //customDatumData.ReportName = ("../../../Pages/EnvWater/Report/ReportFile/AutoMonitorSystemRunMonthReportTemplete/" + customDatumData.StartDateTime.Value.Year + "/" + customDatumData.StartDateTime.Value.Month + "/" + filename).ToString();
                ////更新数据
                //ReportLogService.ReportLogUpdate(customDatumData);
                strTarget = Server.MapPath("../" + filename);
                
                //doc.Save(strTarget, Aspose.Words.SaveFormat.Doc, Aspose.Words.SaveType.OpenInWord, Response);
                doc.Save(strTarget);
                doc.Save(this.Response, filename, Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Docx));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                //Response.End();
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }
            
        }
        #endregion
        #region 报表设计
        /// <summary>
        /// 水质自动监测子站运行情况表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF1(DocumentBuilder builder, DataTable dt)
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
                double width,widths = 0;
                width = 55;
                widths = 45.380;
                for (int i = 0; i <= dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        builder.Font.Bold = true;
                        for (int j = 0; j < 13; j++)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.HorizontalMerge = CellMerge.None;
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.CellFormat.Width = width;
                            switch (j)
                            {
                                case 0:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("时间");
                                    break;
                                case 1:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("AQI达标率(%)");
                                    break;
                                case 2:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("优(天)");
                                    break;
                                case 3:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("良(天)");
                                    break;
                                case 4:
                                    //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                    builder.Write("轻度污染(天)");
                                    break;
                                case 5:
                                    //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                    builder.Write("中度污染(天)");
                                    break;
                                case 6:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                    builder.Write("重度污染(天)");
                                    break;
                                case 7:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("SO2浓度(ug/m3)");
                                    break;
                                case 8:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("NO2浓度(ug/m3)");
                                    break;
                                case 9:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("CO浓度(mg/m3)");
                                    break;
                                case 10:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("O3浓度(ug/m3)");
                                    break;
                                case 11:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("PM10浓度(ug/m3)");
                                    break;
                                case 12:
                                    //builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("PM2.5浓度(ug/m3)");
                                    break;
                            }
                        }
                        builder.EndRow();
                    }
                    else
                    {
                        builder.Font.Bold = true;
                        for (int j = 0; j < 13; j++)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.Width = width;
                            builder.Write(dt.Rows[i - 1][j].ToString());
                        }
                        builder.EndRow();
                    }
                }
                builder.EndTable();
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion
        #region Excel导出
        /// <summary>
        /// 导出空气质量实时报
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private void DataTableToExcel(DataView dv, string fileName, string sheetName, string factor, string factorStr, string COnAQI)
        {
            
            //DataTable dtNew = dv.ToTable();
            //DateTime dtBegion = rdpBegin.SelectedDate.Value;
            //DateTime dtEnd = rdpEnd.SelectedDate.Value;
            //Workbook workbook = new Workbook();
            //Worksheet sheet = workbook.Worksheets[0];
            //Cells cells = sheet.Cells;
            //Aspose.Cells.Style cellStyle = workbook.Styles[workbook.Styles.Add()];
            //workbook.FileName = fileName;
            //sheet.Name = sheetName;
            //cellStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            //cellStyle.Font.Name = "宋体"; //文字字体
            //cellStyle.Font.Size = 12;//文字大小
            //cellStyle.IsLocked = false; //单元格解锁
            //cellStyle.IsTextWrapped = true; //单元格内容自动换行


            //#region 表头

            ////第二行
            //string name = "";
            //string names = "";
            //int count = 0;
            //foreach (DataColumn c in dtNew.Columns)
            //{
            //    name = c.ColumnName;
            //    names += (name + ",");
            //    if (name != "PointId" && name != "PointName")
            //    {
            //        ++count;
            //        if (name.Contains("a34004"))
            //        {
            //            name = "PM2.5" + name.Substring(6, name.Length - 6);
            //        }
            //        else if (name.Contains("a34002"))
            //        {
            //            name = "PM10" + name.Substring(6, name.Length - 6);
            //        }
            //        else if (name.Contains("a21005"))
            //        {
            //            name = "一氧化碳(CO)" + name.Substring(6, name.Length - 6);
            //        }
            //        else if (name.Contains("a21004"))
            //        {
            //            name = "二氧化氮(NO2)" + name.Substring(6, name.Length - 6);
            //        }
            //        else if (name.Contains("a21026"))
            //        {
            //            name = "二氧化硫(SO2)" + name.Substring(6, name.Length - 6);
            //        }
            //        else if (name.Contains("a05024"))
            //        {
            //            name = "臭氧(8小时最值)" + name.Substring(6, name.Length - 6);
            //        }
            //        cells[1, count].PutValue(name);
            //        cells.Merge(1, count, 1, 1);
            //    }
            //}
            //string[] nameAll = names.Trim(',').Split(',');

            ////第一行
            //cells[0, 0].PutValue("地区");
            //cells.Merge(0, 0, 2, 1);
            //if (COnAQI == "1")
            //{
            //    if (factor == "a21005")
            //        cells[0, 1].PutValue(factorStr + "平均浓度" + "(mg/m³)");
            //    else
            //        cells[0, 1].PutValue(factorStr + "平均浓度" + "(μg/m³)");
            //}
            //else
            //{
            //    cells[0, 1].PutValue("平均AQI");
            //}
            //cells.Merge(0, 1, 1, nameAll.Length - 2);


            //cells.SetRowHeight(0, 30);//设置行高
            //cells.SetRowHeight(1, 30);//设置行高
            //cells.SetColumnWidth(0, 20);//设置列宽
            //for (int i = 1; i <= nameAll.Length - 2; i++)
            //{
            //    cells.SetColumnWidth(i, 15);//设置列宽
            //}
            //#endregion


            //for (int i = 0; i < dtNew.Rows.Count; i++)
            //{
            //    DataRow drNew = dtNew.Rows[i];
            //    int rowIndex = i + 2;
            //    cells[rowIndex, 0].PutValue(drNew["PointName"].ToString());
            //    for (int j = 1; j <= count; j++)
            //    {
            //        int m = j + 1;
            //        cells[rowIndex, j].PutValue(drNew[nameAll[m]].ToString());
            //    }
            //}
            //foreach (Aspose.Words.Tables.Cell cell in cells)
            //{
            //    if (!cell.IsStyleSet)
            //    {
            //        cell.SetStyle(cellStyle);
            //    }
            //}
            //Response.Clear();
            //Response.Buffer = true;
            //Response.Charset = "utf-8";
            //Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName)));
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.ContentType = "application/ms-excel";
            //Response.BinaryWrite(workbook.SaveToStream().ToArray());
            //Response.End();
           
        }
        #endregion
        /// <summary>
        /// 查询类型切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvPoint.Style["display"] = "none";
            //comboCity.Visible = false;
            switch (rbtnlType.SelectedValue)
            {
                case "CityProper":
                    //comboCity.Visible = true;
                    dvPoint.Style["display"] = "normal";
                    pointCbxRsm.Visible = false;
                    pointCbxRsmCity.Visible = true;
                    break;
                case "Port":
                    dvPoint.Style["display"] = "normal";
                    pointCbxRsm.Visible = true;
                    pointCbxRsmCity.Visible = false;
                    break;
            }
        }

        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            try
            {
                string[] portIds = new string[] { }; 
                DateTime mBegion = rdpBegin.SelectedDate.Value;
                DateTime mEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.AddMonths(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                //每页显示数据个数            
                int pageSize = int.MaxValue;
                //当前页的序号
                int pageNo = 0;
                int recordTotal = 0;
                string yearAll = "";
                foreach (RadComboBoxItem item in Year.CheckedItems)
                {
                    yearAll += (item.Value.ToString() + ",");
                }
                string[] years = yearAll.Trim(',').Split(',');
                //DataView dv = concentrationAQIService.GetComparisonPager(portIds, mBegion, mEnd, years, pageSize, pageNo, out recordTotal);  // 本期
                DataView dv = new DataView();
                if (rbtnlType.SelectedValue == "CityProper")
                {
                    portIds = pointCbxRsmCity.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                    dv = concentrationAQIService.GetCityProperComparisonPager(portIds, mBegion, mEnd, years, pageSize, pageNo, out recordTotal);  // 本期
                }
                else
                {
                    portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                    dv = concentrationAQIService.GetComparisonPager(portIds, mBegion, mEnd, years, pageSize, pageNo, out recordTotal);  // 本期
                }
                
                
                string path = "D:\\Sinoyd\\南通超级站\\南通20170908\\南通\\Files";
                Document doc = new Document(System.IO.Path.Combine(path, "例子.doc"));
                DocumentBuilder builder = new DocumentBuilder(doc);
                builder.MoveToMergeField("MF1");
                MoveToMF1(builder, dv.ToTable());
                doc.MailMerge.DeleteFields();
                //save(doc);

                string filename = "空气质量比对报表.doc";
                //string strTarget = "C:\\Users\\Administrator\\Desktop\\" + filename;
                string strTarget = "D:\\Sinoyd\\南通超级站\\南通20170908\\南通\\Files\\" + filename;
                strTarget = Server.MapPath("../" + filename);
                //doc.Save(strTarget);
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