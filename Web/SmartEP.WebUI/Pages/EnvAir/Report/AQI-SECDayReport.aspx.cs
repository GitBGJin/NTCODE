using Aspose.Words;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;
using Telerik.Web.UI;
using SmartEP.Service.ReportLibrary.Air;
using SmartEP.Utilities.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AQI_SECDayReport : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private DayAQIService m_DayAQIService = new DayAQIService();
        ReportLogService ReportLogService = new ReportLogService();
        AirPollutantService m_AirPollutantService = new AirPollutantService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ViewState["DisplayPerson"] = PageHelper.GetQueryString("DisplayPerson");
                InitControl();
                LoadingReport(System.DateTime.Now.AddDays(-1).Date);
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        protected void InitControl()
        {
            ReportDate.SelectedDate = System.DateTime.Now.AddDays(-1).Date;
        }

        protected string GetWeek(DateTime dt)
        {
            int i = (int)dt.DayOfWeek;
            string[] WeekDays = { "日", "一", "二", "三", "四", "五", "六" };
            return WeekDays[i];
        }

        /// <summary>
        /// 保存日报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AQI-SECDayReportTemplete.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToMergeField("ReportDate");
            builder.Write(ReportDate.SelectedDate.Value.ToString("yyyy年MM月dd日"));
            builder.MoveToMergeField("TheWeek");
            builder.Write(GetWeek(ReportDate.SelectedDate.Value));
            builder.MoveToMergeField("ReportTime");
            builder.Write(System.DateTime.Now.ToString("HH:mm:ss"));
            DataTable dt = GetReportYesterday();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                builder.MoveToMergeField("AQI");
                builder.Write(dr["AQIValue"] != DBNull.Value ? dr["AQIValue"].ToString() : "--");
                builder.MoveToMergeField("PrimaryPollutant");
                builder.Write(dr["PrimaryPollutant"] != DBNull.Value ? dr["PrimaryPollutant"].ToString() : "--");
                builder.MoveToMergeField("QualityClass");
                builder.Write(dr["Class"] != DBNull.Value ? dr["Class"].ToString() : "--");
                builder.MoveToMergeField("SO2");
                builder.Write(dr["SO2"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["SO2_IAQI"]), 0)).ToString() : "--");
                builder.MoveToMergeField("NO2");
                builder.Write(dr["NO2"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["NO2_IAQI"]), 0)).ToString() : "--");
                builder.MoveToMergeField("PM10");
                builder.Write(dr["PM10"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["PM10_IAQI"]), 0)).ToString() : "--");
                builder.MoveToMergeField("CO");
                builder.Write(dr["CO"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO_IAQI"]), 0)).ToString() : "--");
                builder.MoveToMergeField("O3-8h");
                builder.Write(dr["Max8HourO3"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["Max8HourO3_IAQI"]), 0)).ToString() : "--");
                builder.MoveToMergeField("PM2.5");
                builder.Write(dr["PM25"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["PM25_IAQI"]), 0)).ToString() : "--");
            }
            else
            {
                builder.MoveToMergeField("AQI");
                builder.Write("--");
                builder.MoveToMergeField("PrimaryPollutant");
                builder.Write("--");
                builder.MoveToMergeField("QualityClass");
                builder.Write("--");
                builder.MoveToMergeField("SO2");
                builder.Write("--");
                builder.MoveToMergeField("NO2");
                builder.Write("--");
                builder.MoveToMergeField("PM10");
                builder.Write("--");
                builder.MoveToMergeField("CO");
                builder.Write("--");
                builder.MoveToMergeField("O3-8h");
                builder.Write("--");
                builder.MoveToMergeField("PM2.5");
                builder.Write("--");
            }

            //doc.MailMerge.DeleteFields();
            //doc.Save(this.Response, "AQI-SECDayReportTemplete" + ".doc", ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            //Response.End();

            doc.MailMerge.DeleteFields();
            save(doc);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
        }

        /// <summary>
        /// 保存记录
        /// </summary>
        protected void save(Document doc)
        {
            string filename = "(" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ")" + "AQI-SECDayReport" + ".doc";
            string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/AQI-SECDayReport/" + ReportDate.SelectedDate.Value.Year + "/" + ReportDate.SelectedDate.Value.Month + "/" + filename);
            //添加实体类对象
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointNames = "苏州市区";//测点名称
            customDatum.FactorsNames = "AQI;首要污染物;空气质量类别;SO2;NO2;PM10;CO;O3-8h;PM2.5";
            customDatum.DateTimeRange = ReportDate.SelectedDate.Value.ToString("yyyy年MM月dd日");
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "AQI-SECDayReport";//页面ID
            customDatum.StartDateTime = ReportDate.SelectedDate.Value;
            customDatum.EndDateTime = ReportDate.SelectedDate.Value;
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/AQI-SECDayReport/" + ReportDate.SelectedDate.Value.Year + "/" + ReportDate.SelectedDate.Value.Month + "/" + filename).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            doc.Save(strTarget);
            if (!Directory.Exists(strTarget))
            {
                Alert("保存成功！");
            }
        }

        /// <summary>
        /// 日报预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDisplay_Click(object sender, ImageClickEventArgs e)
        {
            LoadingReport(ReportDate.SelectedDate.Value);
        }

        /// <summary>
        /// 获取昨日日报数据
        /// </summary>
        /// <returns></returns>
        protected DataTable GetReportYesterday()
        {
            int recordTotal = 0;  //数据总行数
            string[] regionUids = new string[1] { "7e05b94c-bbd4-45c3-919c-42da2e63fd43" }; //苏州市区Uid
            DateTime ReportTime = ReportDate.SelectedDate.Value;
            DataTable dt = m_DayAQIService.GetRegionAirQualityDayReport(regionUids, ReportTime, ReportTime, 10, 0, out recordTotal).ToTable();
            return dt;
        }

        /// <summary>
        /// 加载报表
        /// </summary>
        protected void LoadingReport(DateTime time)
        {
            InstanceReportSource instanceReportSource = new InstanceReportSource();
            AQISECDayReportService report = new AQISECDayReportService();
            DataTable dt = GetReportYesterday();
            report.ReportParameters.Add("ReportDate", Telerik.Reporting.ReportParameterType.String, time.ToString("yyyy年MM月dd日"));
            report.ReportParameters.Add("Week", Telerik.Reporting.ReportParameterType.String, "星期" + GetWeek(time));
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                report.ReportParameters.Add("AQI", Telerik.Reporting.ReportParameterType.String, dr["AQIValue"] != DBNull.Value ? dr["AQIValue"].ToString() : "--");
                report.ReportParameters.Add("PrimaryPollutant", Telerik.Reporting.ReportParameterType.String, dr["PrimaryPollutant"] != DBNull.Value ? dr["PrimaryPollutant"].ToString() : "--");
                report.ReportParameters.Add("Class", Telerik.Reporting.ReportParameterType.String, dr["Class"] != DBNull.Value ? dr["Class"].ToString() : "--");
                report.ReportParameters.Add("SO2", Telerik.Reporting.ReportParameterType.String, dr["SO2_IAQI"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["SO2_IAQI"]), 0)).ToString() : "--");
                report.ReportParameters.Add("NO2", Telerik.Reporting.ReportParameterType.String, dr["NO2_IAQI"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["NO2_IAQI"]), 0)).ToString() : "--");
                report.ReportParameters.Add("PM10", Telerik.Reporting.ReportParameterType.String, dr["PM10_IAQI"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["PM10_IAQI"]), 0)).ToString() : "--");
                report.ReportParameters.Add("CO", Telerik.Reporting.ReportParameterType.String, dr["CO_IAQI"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO_IAQI"]), 0)).ToString() : "--");
                report.ReportParameters.Add("Max8HourO3", Telerik.Reporting.ReportParameterType.String, dr["Max8HourO3_IAQI"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["Max8HourO3_IAQI"]), 0)).ToString() : "--");
                report.ReportParameters.Add("PM25", Telerik.Reporting.ReportParameterType.String, dr["PM25_IAQI"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["PM25_IAQI"]), 0)).ToString() : "--");
            }
            else
            {
                report.ReportParameters.Add("AQI", Telerik.Reporting.ReportParameterType.String, "--");
                report.ReportParameters.Add("PrimaryPollutant", Telerik.Reporting.ReportParameterType.String, "--");
                report.ReportParameters.Add("Class", Telerik.Reporting.ReportParameterType.String, "--");
                report.ReportParameters.Add("SO2", Telerik.Reporting.ReportParameterType.String, "--");
                report.ReportParameters.Add("NO2", Telerik.Reporting.ReportParameterType.String, "--");
                report.ReportParameters.Add("PM10", Telerik.Reporting.ReportParameterType.String, "--");
                report.ReportParameters.Add("CO", Telerik.Reporting.ReportParameterType.String, "--");
                report.ReportParameters.Add("Max8HourO3", Telerik.Reporting.ReportParameterType.String, "--");
                report.ReportParameters.Add("PM25", Telerik.Reporting.ReportParameterType.String, "--");
            }
            instanceReportSource.ReportDocument = report;
            this.ReportViewer.ReportSource = instanceReportSource;

            ReportViewer.ShowPrintButton = false;
            ReportViewer.ShowPrintPreviewButton = false;
            ReportViewer.ShowExportGroup = false;

            RegisterScript("SetHeigth();");
        }

    }
}