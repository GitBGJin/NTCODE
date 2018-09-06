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
using SmartEP.DomainModel.BaseData;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AirQualityAutoMonthReport : SmartEP.WebUI.Common.BasePage
    {
        MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        AirQualityAutoMonthReportService reportService = new AirQualityAutoMonthReportService();
        AirPollutantService airPollutantService = new AirPollutantService();
        DataQueryByHourService dataByHourService = new DataQueryByHourService();
        MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
        bool IScheck = false;
        //SO2;NO;NO2;NOx;CO;O3;PM10;PM2.5;气压;湿度;温度;风速;风向
        string[] factorCode = ("a21026;a21003;a21004;a21002;a21005;a05024;a34002;a34004;a01006;a01002;a01001;a01007;a01008").Split(';');
        string[] factorName = ("SO2;NO;NO2;NOx;CO;O3;PM10;PM2.5;气压;湿度;温度;风速;风向").Split(';');
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();//初始化控件
                //LoadingReport();//加载report
            }
        }

        #region 初始化

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.Date.AddMonths(-1).ToString("yyyy-MM-01"));//初始化时间
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-01")).AddDays(-1);//初始化时间
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
            DateTime dtStart = dtpBegin.SelectedDate.Value.Date;
            DateTime dtEnd = dtpEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1);
            string[] CommonField = ("日期;时间" + (factorCode.Length > 0 ? ";" + string.Join(";", factorCode) : "")).Split(';');
            string[] CommonName = ("日期;时间" + (factorName.Length > 0 ? ";" + string.Join(";", factorName) : "")).Split(';');
            string TempPath = Server.MapPath("../../../Files/TempFile/Excel/" + "AirQualityAutoMonthReportTemp" + ".xls");
            string SavePath = Server.MapPath("../../../Files/TempFile/Excel");
            string SaveFileName = "环境空气质量自动监测数据报表" + dtStart.Date.ToString("yyyyMMdd") + "-" + dtEnd.Date.ToString("yyyyMMdd") + ".xls";

            DataTable dtData = dataByHourService.GetAQAutoMonthReportExportData(portIds, factorList, dtStart, dtEnd).ToTable();
            DataTableToExcel(portIds, dtData, CommonField, TempPath, SavePath, SaveFileName);
        }


        /// <summary>
        /// 测点全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ComboxPoint_ItemCreated(object sender, Telerik.Web.UI.RadComboBoxItemEventArgs e)
        {
            if (!IScheck)
            {
                e.Item.Checked = true;
                IScheck = true;
            }
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
            DateTime dtStart = dtpBegin.SelectedDate.Value.Date;
            DateTime dtEnd = dtpEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1);


            InstanceReportSource instanceReportSource = new InstanceReportSource();
            reportService.BindingPortReport(portIds, factorCode, factorName, factorList, dtStart, dtEnd);
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
        public void DataTableToExcel(string[] portIds, DataTable dtData, string[] CommonField, string TempPath, string SavePath, string SaveFileName, bool IsBackup = false)
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
                for (int i = 0; i < portIds.Length; i++)
                {
                    MonitoringPointEntity pointEntity = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i]));
                    MonitoringPointExtensionForEQMSAirEntity extensionEntity = g_MonitoringPointAir.RetrieveAirExtensionPointListByPointUids(pointEntity.MonitoringPointUid.Split(';')).FirstOrDefault();
                    //string pointName = pointEntity.MonitoringPointName;
                    //string stCode=extensionEntity.
                    dv.RowFilter = "PointId=" + portIds[i];

                    #region 添加sheet以及绑定列
                    if (i > 0)
                    {
                        designer.Workbook.Worksheets.Add();
                        sheet = designer.Workbook.Worksheets[i];
                        sheet.Copy(designer.Workbook.Worksheets[0]);
                    }
                    sheet.Name = pointEntity.MonitoringPointName;

                    Cells cells = sheet.Cells;
                    //cells[1, 1].PutValue("&=$PointName" + portIds[i]);
                    cells[1, 1].PutValue("测点名称  " + pointEntity.MonitoringPointName + "       测点代码 " + (extensionEntity.Stcode != null ? extensionEntity.Stcode : "") + "                                                                                               WXHJ-JL-ZB-24");
                    for (int j = 0; j < CommonField.Length; j++)
                    {
                        cells = sheet.Cells;
                        cells[4, j + 1].PutValue("&=[Month" + portIds[i] + "]." + CommonField[j]);
                    }
                    #endregion
                    #region 添加数据源
                    DataTable resultDT = dv.ToTable();
                    resultDT.TableName = "Month" + portIds[i];
                    designer.SetDataSource(resultDT);//小时数据
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