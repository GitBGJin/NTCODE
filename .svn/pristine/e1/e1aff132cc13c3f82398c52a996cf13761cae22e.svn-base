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
using SmartEP.DomainModel.BaseData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AirQualityRoutineMonthReport : SmartEP.WebUI.Common.BasePage
    {
        MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        AirQualityRoutineMonthReportService reportService = new AirQualityRoutineMonthReportService();
        AirPollutantService airPollutantService = new AirPollutantService();
        DataQueryByDayService dataByDayService = new DataQueryByDayService();
        MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
        DayAQIService dayAQIService = new DayAQIService();
        //SO2;NO;NO2;NOx;CO;O3;PM10;PM2.5;气压;湿度;温度;风速;风向   a05024臭氧
        string[] factorCode = ("a21026;a21004;a21002;a21005;a05024;a34002;a34004;a21018;a25044;a25002;a20058").Split(';');
        string[] factorField = ("a21026;a21004;a21002;a21005;MaxOneHourO3;Max8HourO3;a34002;a34004;a21018;a25044;a25002;a20058").Split(';');
        string[] factorName = ("二氧化硫;二氧化氮;氮氧化物;一氧化碳;臭氧8;臭氧1;可吸入微粒物;细粒子;氟化物;苯并(a)蓖;苯;汞").Split(';');
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
            string[] CommonField = ("monthBegin;dayBegin;hourBegin;minuteBegin;monthEnd;dayEnd;hourEnd;minuteEnd" + (factorField.Length > 0 ? ";" + string.Join(";", factorField) : "")).Split(';');
            string TempPath = Server.MapPath("../../../Files/TempFile/Excel/" + "AirQualityRoutineMonthReportTemp" + ".xls");
            string SavePath = Server.MapPath("../../../Files/TempFile/Excel");
            string SaveFileName = "环境空气质量例行监测成果表" + dtStart.Date.ToString("yyyyMMdd") + "-" + dtEnd.Date.ToString("yyyyMMdd") + ".xls";

            DataTable dtData = dataByDayService.GetAQRoutineMonthReportExportData(portIds, factorList, dtStart, dtEnd).ToTable();//日数据
            DataTable dtStatistical = GetStatisticalData(portIds, factorList, dtStart, dtEnd);//统计数据
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
            DataTable dtStatistical = GetStatisticalData(portIds, factorList, dtStart, dtEnd);//统计数据

            InstanceReportSource instanceReportSource = new InstanceReportSource();
            reportService.BindingPortReport(portIds, dtStatistical, factorField, factorName, factorList, dtStart, dtEnd);
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
                    MonitoringPointEntity pointEntity = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i]));
                    MonitoringPointExtensionForEQMSAirEntity extensionEntity = g_MonitoringPointAir.RetrieveAirExtensionPointListByPointUids(pointEntity.MonitoringPointUid.Split(';')).FirstOrDefault();
                    //string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;
                    dv.RowFilter = "PointId=" + portIds[i];
                    dvStatistical.RowFilter = "PointId=" + portIds[i];
                    #region 添加sheet以及绑定列
                    if (i > 0)
                    {
                        designer.Workbook.Worksheets.Add();
                        sheet = designer.Workbook.Worksheets[i];
                        sheet.Copy(designer.Workbook.Worksheets[0]);
                    }
                    sheet.Name = pointEntity.MonitoringPointName;

                    Cells cells = sheet.Cells;
                    cells[1, 1].PutValue(string.Format(@"单位(公章)          测点代码 320200       月度 {0}                                           共 {1} 页 第 {2} 页", dtpBegin.SelectedDate.Value.Date.Month, portIds.Length, i + 1));
                    cells[4, 0].PutValue(pointEntity.MonitoringPointName);
                    cells[4, 1].PutValue(extensionEntity.Stcode != null ? extensionEntity.Stcode : "");
                    //cells[5, 10].PutValue("555");
                    for (int j = 0; j < CommonField.Length; j++)
                    {
                        cells = sheet.Cells;
                        cells[4, j + 2].PutValue("&=[Month" + portIds[i] + "]." + CommonField[j]);
                        //cells[5, j + 10].PutValue("&=[Statistical" + portIds[i] + "]." + CommonField[j]);
                        if (dvStatistical.Count > 0 && j > 7)
                            cells[5, j + 2].PutValue(dvStatistical[0][CommonField[j]]);
                    }
                    #endregion
                    #region 添加数据源
                    DataTable resultDT = dv.ToTable();
                    resultDT.TableName = "Month" + portIds[i];
                    designer.SetDataSource(resultDT);//小时数据

                    //DataTable resultStatistical = dvStatistical.ToTable();
                    //resultStatistical.TableName = "Statistical" + portIds[i];
                    //designer.SetDataSource(resultStatistical);//统计数据
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
        /// 获取统计行
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factorList"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        private DataTable GetStatisticalData(string[] portIds, List<SmartEP.Core.Interfaces.IPollutant> factorList, DateTime dtStart, DateTime dtEnd)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PointID");
            dt.Columns.Add("Statistical");
            DataView dvStatistical = dataByDayService.GetDayStatisticalData(portIds, factorList, dtStart, dtEnd);
            DataView dtAQIStatistical = dayAQIService.GetPointsAvgValue(portIds, dtStart, dtEnd);

            for (int j = 0; j < portIds.Length; j++)
            {
                DataRow row = dt.NewRow();
                row["PointID"] = portIds[j];
                row["Statistical"] = "平均值";
                for (int i = 0; i < factorField.Length; i++)
                {
                    if (j == 0)
                        dt.Columns.Add(factorField[i]);
                    if (factorField.Equals("MaxOneHourO3") || factorField.Equals("Max8HourO3"))
                    {
                        dtAQIStatistical.RowFilter = "PointID=" + portIds[j];
                        if (dtAQIStatistical.Count > 0)
                        {
                            if (dtAQIStatistical[0][factorField[i]] != DBNull.Value)
                            {
                                //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                                AirPollutantService m_AirPollutantService = new AirPollutantService();
                                int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factorField[i]).PollutantDecimalNum);
                                row[factorField[i]] = Convert.ToDecimal(DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtAQIStatistical[0][factorField[i]]), DecimalNum));
                            }
                        }
                        //dtAQIStatistical.AsEnumerable().ToList().ForEach(x => dt.Rows.Add(x.Field<string>("MaxOneHourO3")));
                    }
                    else
                    {
                        dvStatistical.RowFilter = "PointID=" + portIds[j] + " and PollutantCode='" + factorField[i] + "'";
                        if (dvStatistical.Count > 0)
                        {
                            if (dvStatistical[0]["Value_Avg"] != DBNull.Value)
                            {
                                //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                                AirPollutantService m_AirPollutantService = new AirPollutantService();
                                int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factorField[i]).PollutantDecimalNum);
                                row[factorField[i]] = Convert.ToDecimal(DecimalExtension.GetPollutantValue(Convert.ToDecimal(dvStatistical[0]["Value_Avg"]), DecimalNum));
                            }
                        }
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        #endregion
    }
}