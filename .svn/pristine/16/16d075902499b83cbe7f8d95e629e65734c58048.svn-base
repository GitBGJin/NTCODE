namespace SmartEP.Service.ReportLibrary.Water
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;
    using SmartEP.Service.DataAnalyze.Water.DataQuery;
    //using SmartEP.Service.ReportLibrary.Common;
    using SmartEP.Service.BaseData.Standard;
    using SmartEP.DomainModel.BaseData;
    using SmartEP.Service.Core.Enums;

    /// <summary>
    /// Summary description for BlueAlgaeDayReportService.
    /// </summary>
    public partial class BlueAlgaeDayReportService : Telerik.Reporting.Report
    {
        #region 属性
        private static string[] factors =null;
        private static string[] portIds = null;
        private static DateTime beginTime = DateTime.Now;
        private static DateTime endTime = DateTime.Now;
        #endregion
        DataQueryByHourService waterHourRep = new DataQueryByHourService();
        EQIConcentrationService EQIService = new EQIConcentrationService();
        public BlueAlgaeDayReportService()
        {
            InitializeComponent();

            this.title.Value = endTime.Year + "年太湖水污染及蓝藻监测预警工作日报表";
        }

        /// <summary>
        /// 获取蓝藻日报数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetBlueAlgaeDayData()
        {
            DataTable dt = waterHourRep.GetBlueAlgaeDayData(portIds, factors, beginTime, endTime).ToTable();
            return dt;
        }

        /// <summary>
        /// 获取标准限值
        /// </summary>
        /// <returns></returns>
        public DataTable GetLimit()
        {
            return EQIService.GetEQILimit(factors);
        }

        /// <summary>
        /// 报表加载初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void report_NeedDataSource(object sender, EventArgs e)
        {
            factors = ReportParameters["factors"].Value.ToString().Split(';');
            portIds = ReportParameters["portIds"].Value.ToString().Split(';');
            beginTime = Convert.ToDateTime(ReportParameters["beginTime"].Value);
            endTime = Convert.ToDateTime(ReportParameters["endTime"].Value);

            this.title.Value = endTime.Year + "年太湖水污染及蓝藻监测预警工作日报表";
        }

    }
}