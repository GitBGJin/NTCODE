using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Service.BaseData.MPInfo;
using Telerik.Reporting;
using SmartEP.Service.ReportLibrary.Air;
using SmartEP.Service.BaseData.Channel;
using System.Data;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using Aspose.Cells;
using System.IO;
using SmartEP.Core.Generic;
using SmartEP.Utilities.IO;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Utilities.Calendar;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class PointWeekReport : SmartEP.WebUI.Common.BasePage
    {
        MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        DataQueryByDayService dataByDayService = new DataQueryByDayService();
        MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
        DayAQIService dayAQIService = new DayAQIService();
        private IList<SmartEP.Core.Interfaces.IPollutant> factors = null;
        private IList<IPoint> points = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();//初始化控件
            }
        }

        #region 初始化

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            MonthTime.SelectedDate = DateTime.Now.Date;//初始化时间
            BindWeekComboBox();//绑定周
            SetLiteral();//显示周日期范围
            pointCbxRsm_SelectedChanged();
        }
        #endregion

        #region 事件
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uploadData_Click(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pointCbxRsm_SelectedChanged();
            }
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            List<SmartEP.Core.Interfaces.IPollutant> factorList = factorCbxRsm.GetFactors();
            DateTime dtStart = Convert.ToDateTime(weekList.SelectedValue);
            DateTime dtEnd = DateTime.ParseExact(weekList.SelectedValue, "yyyy-MM-dd", null).AddDays(6);

            string[] CommonField = ("DateTime" + (factorList.Select(x => x.PollutantCode).ToArray().Length > 0 ? ";" + string.Join(";", factorList.Select(x => x.PollutantCode).ToArray()) : "")).Split(';');
            string[] CommonName = ("日期" + (factorList.Select(x => x.PollutantName).ToArray().Length > 0 ? ";" + string.Join(";", factorList.Select(x => x.PollutantName).ToArray()) : "")).Split(';');
            string[] CommonUnit = ("" + (factorList.Select(x => x.PollutantMeasureUnit).ToArray().Length > 0 ? ";" + string.Join(";", factorList.Select(x => x.PollutantMeasureUnit).ToArray()) : "")).Split(';');
            string TempPath = Server.MapPath("../../../Files/TempFile/Excel/" + "PointReportTemp" + ".xls");
            string SavePath = Server.MapPath("../../../Files/TempFile/Excel");
            string SaveFileName = "大气监测周报表" + dtStart.Date.ToString("yyyyMMdd") + "-" + dtEnd.Date.ToString("yyyyMMdd") + ".xls";

            DataTable dtData = new DataTable();
            DataTable dtStatistical = new DataTable();
            GetDataSource(out dtData, out dtStatistical);
            DataTableToExcel(portIds, dtData, dtStatistical, CommonField, CommonName, CommonUnit, TempPath, SavePath, SaveFileName, dtEnd);
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadingReport();//加载report
            RegisterScript("SetHeigth();");
        }

        /// <summary>
        /// 报表加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReportViewer1_Load(object sender, EventArgs e)
        {
            LoadingReport();
        }

        /// <summary>
        /// 日期选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MonthTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekComboBox();
            SetLiteral();
        }

        /// <summary>
        /// 周选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void weekList_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            SetLiteral();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 加载报表数据
        /// </summary>
        private void LoadingReport()
        {
            if (!IsPostBack)
            {
                pointCbxRsm_SelectedChanged();
            }
            List<SmartEP.Core.Interfaces.IPollutant> factorList = factorCbxRsm.GetFactors();
            points = pointCbxRsm.GetPoints();
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            DateTime dtStart = Convert.ToDateTime(weekList.SelectedValue);
            DateTime dtEnd = DateTime.ParseExact(weekList.SelectedValue, "yyyy-MM-dd", null).AddDays(6);

            DataTable dtData = new DataTable();
            DataTable dtStatistical = new DataTable();
            GetDataSource(out dtData, out dtStatistical);
            InstanceReportSource instanceReportSource = new InstanceReportSource();
            PointReport reportService = new PointReport();
            reportService.BindingPortReport(portIds, dtStatistical, dtData, factorList.Select(x => x.PollutantCode).ToArray(), factorList.Select(x => x.PollutantName).ToArray(), factorList, dtStart, dtEnd, "周", weekList.SelectedItem.Text);
            instanceReportSource.ReportDocument = reportService;
            this.ReportViewer1.ReportSource = instanceReportSource;
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="dtStatistical"></param>
        private void GetDataSource(out DataTable dtData, out DataTable dtStatistical)
        {
            if (!IsPostBack)
            {
                pointCbxRsm_SelectedChanged();
            }
            List<SmartEP.Core.Interfaces.IPollutant> factorList = factorCbxRsm.GetFactors();
            points = pointCbxRsm.GetPoints();
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            DateTime dtStart = Convert.ToDateTime(weekList.SelectedValue);
            DateTime dtEnd = DateTime.ParseExact(weekList.SelectedValue, "yyyy-MM-dd", null).AddDays(6);

            dtData = dataByDayService.GetAQRoutineMonthReportExportData(portIds, factorList, dtStart, dtEnd).ToTable();
            dtStatistical = dayAQIService.GetPortsStatisticalData(portIds, dtData, factorList, factorList.Select(x => x.PollutantCode).ToArray(), dtStart, dtEnd);//统计数据
        }

        /// <summary>
        /// 创建Excel导出文件
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dtData"></param>
        /// <param name="CommonField"></param>
        /// <param name="TempPath"></param>
        /// <param name="SavePath"></param>
        /// <param name="SaveFileName"></param>
        /// <param name="IsBackup"></param>
        public void DataTableToExcel(string[] portIds, DataTable dtData, DataTable dtStatistical, string[] CommonField, string[] CommonName, string[] CommonUnit, string TempPath, string SavePath, string SaveFileName, DateTime dtEnd, bool IsBackup = false)
        {
            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //创建一个workbookdesigner对象
            WorkbookDesigner designer = new WorkbookDesigner();

            //制定报表模板
            string path = System.IO.Path.Combine(TempPath);
            designer.Open(path);

            if (designer.Workbook.Worksheets.Count > 0)
            {
                Worksheet sheet = designer.Workbook.Worksheets[0];
                //设置Datatable对象 
                DataView dv = dtData.DefaultView;
                DataView dvStatistical = dtStatistical.DefaultView;


                for (int i = 0; i < portIds.Length; i++)
                {
                    string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;
                    dv.RowFilter = "PointId=" + portIds[i];
                    dvStatistical.RowFilter = "PointId=" + portIds[i];

                    #region 添加sheet以及绑定列
                    if (i > 0)
                    {
                        designer.Workbook.Worksheets.Add();
                        sheet = designer.Workbook.Worksheets[i];
                        sheet.Copy(designer.Workbook.Worksheets[0]);
                    }
                    sheet.Name = pointName;

                    #region Style
                    Aspose.Cells.Style style1 = designer.Workbook.Styles[designer.Workbook.Styles.Add()];//新增样式 
                    style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
                    style1.Font.Name = "宋体";//文字字体 
                    style1.Font.Size = 12;//文字大小 
                    style1.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.None;
                    style1.Borders[BorderType.RightBorder].LineStyle = CellBorderType.None;
                    style1.Borders[BorderType.TopBorder].LineStyle = CellBorderType.None;
                    style1.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.None;

                    Aspose.Cells.Style style2 = designer.Workbook.Styles[designer.Workbook.Styles.Add()];//新增样式 
                    style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
                    style2.Font.Name = "宋体";//文字字体 
                    style2.Font.Size = 12;//文字大小 
                    style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Double;
                    style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                    Aspose.Cells.Style style3 = designer.Workbook.Styles[designer.Workbook.Styles.Add()];//新增样式 
                    style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
                    style3.Font.Name = "宋体";//文字字体 
                    style3.Font.Size = 12;//文字大小 
                    style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                    Aspose.Cells.Style style4 = designer.Workbook.Styles[designer.Workbook.Styles.Add()];//新增样式 
                    style4.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
                    style4.Font.Name = "宋体";//文字字体 
                    style4.Font.Size = 12;//文字大小 
                    style4.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    style4.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    style4.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    style4.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Double;
                    #endregion


                    Cells cells = sheet.Cells;
                    cells[1, 1].PutValue(string.Format(@"测点名称：{0}", pointName));
                    cells[5, 1].PutValue("&=[Statistical" + portIds[i] + "].Statistical");
                    cells[5, 1].SetStyle(style3);
                    for (int j = 0; j < CommonField.Length; j++)
                    {
                        cells = sheet.Cells;
                        if (j < 1)
                        {
                            //cells.Merge(2, 0, 2, 0);//合并单元格 
                            cells[2, j + 1].PutValue(CommonName[j]);
                            //cells[2, j + 1].SetStyle(style2);
                        }
                        else
                        {
                            cells[2, j + 1].PutValue(CommonName[j]);
                            cells[3, j + 1].PutValue(CommonUnit[j]);
                            if (j == CommonField.Length - 1)
                            {
                                cells[2, j + 1].SetStyle(style2);
                                cells[3, j + 1].SetStyle(style4);
                            }
                            else
                            {
                                cells[2, j + 1].SetStyle(cells[2, 2].GetStyle());
                                cells[3, j + 1].SetStyle(cells[3, 2].GetStyle());
                            }
                        }

                        cells[4, j + 1].PutValue("&=[Month" + portIds[i] + "]." + CommonField[j]);
                        cells[4, j + 1].SetStyle(style3);
                        if (j > 0)
                        {
                            cells[5, j + 1].PutValue("&=[Statistical" + portIds[i] + "]." + CommonField[j]);
                            cells[5, j + 1].SetStyle(style3);
                        }
                    }
                    cells[1, CommonField.Length].PutValue(string.Format(@"日期：{0}", dtEnd.ToString("yyyy年MM月") + "  " + weekList.SelectedItem.Text));
                    cells.Merge(0, 1, 1, CommonField.Length);//合并单元格（标题）
                    cells[0, 1].PutValue(pointName + "大气监测周报表");
                    cells[0, 1].SetStyle(style1);
                    #endregion
                    #region 添加数据源
                    DataTable resultDT = dv.ToTable();
                    resultDT.TableName = "Month" + portIds[i];
                    designer.SetDataSource(resultDT);//小时数据

                    DataTable resultStatistical = dvStatistical.ToTable();
                    resultStatistical.TableName = "Statistical" + portIds[i];
                    designer.SetDataSource(resultStatistical);//统计数据
                    #endregion
                }
            }

            //根据数据源处理生成报表内容
            designer.Process();

            if (IsBackup)
            {
                //保存Excel文件
                string fileToSave = System.IO.Path.Combine(SavePath + "/" + SaveFileName);
                if (File.Exists(fileToSave))
                {
                    File.Delete(fileToSave);
                }
                else
                {
                    FileManager.CreateFile(SaveFileName, SavePath);
                }
                designer.Save(fileToSave, FileFormatType.Excel2003);
            }
            designer.Save(HttpUtility.UrlEncode(SaveFileName, System.Text.Encoding.UTF8), SaveType.OpenInExcel, FileFormatType.Excel2003, System.Web.HttpContext.Current.Response);
        }

        /// <summary>
        /// 站点因子关联
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
            points = pointCbxRsm.GetPoints();
            InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
            IList<string> list = new List<string>();
            string[] factor;
            string factors = string.Empty;
            foreach (IPoint point in points)
            {
                IQueryable<PollutantCodeEntity> p = m_InstrumentChannelService.RetrieveChannelListByPointUid(point.PointGuid);
                list = list.Union(p.Select(t => t.PollutantName)).ToList();
            }
            factor = list.ToArray();
            foreach (string f in factor)
            {
                factors += f + ";";
            }
            factorCbxRsm.SetFactorValuesFromNames(factors);
        }

        /// <summary>
        /// 绑定周
        /// </summary>
        private void BindWeekComboBox()
        {
            if (MonthTime.SelectedDate > System.DateTime.Now)
            {
                Alert("选择时间必须小于等于当前时间！");
                return;
            }
            weekList.DataValueField = "value";
            weekList.DataTextField = "text";
            weekList.DataSource = ChinaDate.GetWeekOfMonth(MonthTime.SelectedDate.Value);
            weekList.DataBind();
            SetLiteral();
        }

        /// <summary>
        /// 显示所选周的日期
        /// </summary>
        private void SetLiteral()
        {
            DateTime endDate = DateTime.ParseExact(weekList.SelectedValue, "yyyy-MM-dd", null).AddDays(6);
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;

            DateTime firstDay = Convert.ToDateTime(endDate.ToString("yyyy-01-01"));
            int weekid = firstDay.DayOfWeek == DayOfWeek.Monday ? ChinaDate.WeekOfYear(endDate) : ChinaDate.WeekOfYear(endDate) - 1;
            Literal1.Text = string.Format("时间：从{0:yyyy-MM-dd}到{1:yyyy-MM-dd}；全年第{2}周", weekList.SelectedValue, endDate, weekid);
        }
        #endregion
    }
}