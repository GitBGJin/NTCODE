namespace SmartEP.Service.ReportLibrary.Air
{
    using SmartEP.Service.DataAnalyze.Air.AQIReport;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for AirQualityWeekReportService.
    /// </summary>
    public partial class AirQualityWeekReportService : Telerik.Reporting.Report
    {
        #region 属性
        private static string[] regionGuids = null;
        private static DateTime dtBegin = DateTime.Now;
        private static DateTime dtEnd = DateTime.Now;
        #endregion
        DayAQIService m_DayAQIService = new DayAQIService();
        public AirQualityWeekReportService()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// 获取苏州市区水质自动监测周报数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAirQualityWeekData()
        {
            DataTable dt = m_DayAQIService.GetRegionsTable(regionGuids, dtBegin, dtEnd);
            dt.Columns.Add("cycle", typeof(string));
            TimeSpan ts = dtEnd - new DateTime(dtEnd.Year, 1, 1);
            int intcycle = 0;
            if ((ts.Days + 1) % 7 == 0)
            {
                intcycle = (int)((1 + ts.Days) / 7);
            }
            else
            {
                intcycle = (int)((1 + ts.Days) / 7) + 1;
            }
            dt.Rows[0]["cycle"] = "第 期";
            for (int i = 0; i < 3; i++)
            {
                if (i == 2)
                {
                    dt.Columns["2013"].ColumnName = "c2";
                }
                else if (dt.Columns.Contains(dtEnd.AddYears(-i).Year.ToString()))
                {
                    dt.Columns[dtEnd.AddYears(-i).Year.ToString()].ColumnName = "c" + (5 - i);
                }
            }
            return dt;
        }

        /// <summary>
        /// 报表加载初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void report_NeedDataSource(object sender, EventArgs e)
        {
            regionGuids = ReportParameters["regionGuids"].Value.ToString().Split(';');
            dtBegin = Convert.ToDateTime(ReportParameters["beginTime"].Value);
            dtEnd = Convert.ToDateTime(ReportParameters["endTime"].Value);
            TimeSpan ts = dtEnd - new DateTime(dtEnd.Year, 1, 1);
            int intcycle = 0;
            if ((1 + ts.Days) % 7 == 0)
            {
                intcycle = (int)((ts.Days + 1) / 7);
            }
            else
            {
                intcycle = (int)((ts.Days + 1) / 7) + 1;
            }
        }
    }
}