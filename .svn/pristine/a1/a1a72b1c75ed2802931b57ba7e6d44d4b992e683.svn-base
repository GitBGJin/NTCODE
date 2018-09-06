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

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AirQualityStatisticalMonthReport : SmartEP.WebUI.Common.BasePage
    {
        MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        AirQualityStatisticalMonthReportService reportService = new AirQualityStatisticalMonthReportService();
        AirPollutantService airPollutantService = new AirPollutantService();
        DataQueryByDayService dataByDayService = new DataQueryByDayService();
        //EQIConcentrationService EQIService = new EQIConcentrationService();
        MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
        DayAQIService dayAQIService = new DayAQIService();
        //SO2;NO;NO2;NOx;CO;O3;PM10;PM2.5;气压;湿度;温度;风速;风向   a05024臭氧
        //string[] factorCode = ("a21026;a21004;a21002;a21005;a05024;a34002;a34004").Split(';');
        //string[] factorField = ("a21026;a21004;a21002;a21005;MaxOneHourO3;Max8HourO3;a34002;a34004").Split(';');
        //string[] factorName = ("二氧化硫;二氧化氮;氮氧化物;一氧化碳;臭氧8;臭氧1;可吸入微粒物;细粒子").Split(';');
        string[] factorCode = ("a34002;a21004;a21026;a21002;a05024;a21005").Split(';');
        string[] factorField = ("a34002;a21004;a21026;a21002;Max8HourO3;MaxOneHourO3;a21005").Split(';');
        string[] factorName = ("可吸入微粒物;二氧化氮;二氧化硫;细粒子;臭氧1;臭氧8;一氧化碳").Split(';');
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();//初始化控件
                LoadingReport();//加载report
            }
        }

        #region 初始化

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.Date.AddMonths(-1).ToString("yyyy-MM-01"));//初始化时间
            //dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-01")).AddDays(-1);//初始化时间
            BindingPoint();//绑定测点
        }

        /// <summary>
        /// 绑定测点
        /// </summary>
        private void BindingPoint()
        {
            var pointList = pointAirService.RetrieveAirMPListByCountryControlled();
            ComboxPoint.DataValueField = "PointId";
            ComboxPoint.DataTextField = "MonitoringPointName";
            ComboxPoint.DataSource = pointList;
            ComboxPoint.DataBind();
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
            string[] portIds = ComboxPoint.CheckedItems.Select(x => x.Value).ToArray();
            List<SmartEP.Core.Interfaces.IPollutant> factorList = airPollutantService.GetDefaultFactors(factorCode);
            DateTime dtStart = Convert.ToDateTime(dtpBegin.SelectedDate.Value.Date.ToString("yyyy-MM-01"));
            DateTime dtEnd = dtStart.AddMonths(1).AddDays(-1);
            string[] CommonField = ("DateTime" + (factorField.Length > 0 ? ";" + string.Join(";", factorField) : "")).Split(';');
            string TempPath = Server.MapPath("../../../Files/TempFile/Excel/" + "AirQualityStatisticalMonthReportTemp" + ".xls");
            string SavePath = Server.MapPath("../../../Files/TempFile/Excel");
            string SaveFileName = "空气质量统计月报" + dtStart.Date.ToString("yyyyMMdd") + "-" + dtEnd.Date.ToString("yyyyMMdd") + ".xls";

            DataTable dtData = dataByDayService.GetAQRoutineMonthReportExportData(portIds, factorList, dtStart, dtEnd).ToTable();//日数据
            DataTable dtStatistical = dayAQIService.GetPortsStatisticalData(portIds, dtData, factorList,factorField, dtStart, dtEnd);//统计数据
            DataTableToExcel(portIds, dtData, dtStatistical, CommonField, TempPath, SavePath, SaveFileName);
        }

        /// <summary>
        /// 测点全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ComboxPoint_ItemCreated(object sender, Telerik.Web.UI.RadComboBoxItemEventArgs e)
        {
            e.Item.Checked = true;
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadingReport();//加载report
            RegisterScript("SetHeigth();");
        }
        #endregion

        #region 方法
        /// <summary>
        /// 加载报表数据
        /// </summary>
        private void LoadingReport()
        {
            string[] portIds = ComboxPoint.CheckedItems.Select(x => x.Value).ToArray();
            List<SmartEP.Core.Interfaces.IPollutant> factorList = airPollutantService.GetDefaultFactors(factorCode);
            DateTime dtStart = Convert.ToDateTime(dtpBegin.SelectedDate.Value.Date.ToString("yyyy-MM-01"));
            DateTime dtEnd = dtStart.AddMonths(1).AddDays(-1);
            DataTable dtData = dataByDayService.GetAQRoutineMonthReportExportData(portIds, factorList, dtStart, dtEnd).ToTable();
            DataTable dtStatistical = dayAQIService.GetPortsStatisticalData(portIds, dtData, factorList,factorField, dtStart, dtEnd);//统计数据

            InstanceReportSource instanceReportSource = new InstanceReportSource();
            reportService.BindingPortReport(portIds, dtStatistical, dtData, factorField, factorName, factorList, dtStart, dtEnd);
            instanceReportSource.ReportDocument = reportService;
            this.ReportViewer1.ReportSource = instanceReportSource;
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
        public void DataTableToExcel(string[] portIds, DataTable dtData, DataTable dtStatistical, string[] CommonField, string TempPath, string SavePath, string SaveFileName, bool IsBackup = false)
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

                    Cells cells = sheet.Cells;
                    cells[1, 1].PutValue(string.Format(@"      测点名称  {0}                                                                                    月份 {1}               							",pointName, dtpBegin.SelectedDate.Value.Date.Month));
                    cells[5, 1].PutValue("&=[Statistical" + portIds[i] + "].Statistical");
                    for (int j = 0; j < CommonField.Length; j++)
                    {
                        cells = sheet.Cells;
                        cells[4, j + 1].PutValue("&=[Month" + portIds[i] + "]." + CommonField[j]);
                        //if (dvStatistical.Count > 0)
                        if (j > 0)
                            cells[5, j + 1].PutValue("&=[Statistical" + portIds[i] + "]." + CommonField[j]);
                    }
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
        #endregion
    }
}