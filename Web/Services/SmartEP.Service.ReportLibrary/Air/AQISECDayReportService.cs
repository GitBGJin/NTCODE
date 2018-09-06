namespace SmartEP.Service.ReportLibrary.Air
{
    using SmartEP.Service.BaseData.Channel;
    using SmartEP.Service.DataAnalyze.Air.AQIReport;
    using SmartEP.Service.DataAnalyze.Report;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for AQISECDayReportService.
    /// </summary>
    public partial class AQISECDayReportService : Telerik.Reporting.Report
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private DayAQIService m_DayAQIService = new DayAQIService();
        ReportLogService ReportLogService = new ReportLogService();
        AirPollutantService m_AirPollutantService = new AirPollutantService();

        public AQISECDayReportService()
        {
            InitializeComponent();
            this.ReportTime.Value = "时间：" + System.DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 获取昨日日报数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetReportYesterday()
        {
            int recordTotal = 0;  //数据总行数
            string[] regionUids = new string[1] { "7e05b94c-bbd4-45c3-919c-42da2e63fd43" }; //苏州市区Uid
            DateTime ReportTime = Convert.ToDateTime(ReportParameters["time"].Value);
            DataTable dt = m_DayAQIService.GetRegionAirQualityDayReport(regionUids, ReportTime, ReportTime, 10, 0, out recordTotal).ToTable();
            this.ReportDate.Value = ReportTime.ToString("yyyy年MM月dd日");
            return dt;
        }
    }
}