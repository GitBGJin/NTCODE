namespace SmartEP.Service.ReportLibrary.Air
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;

    /// <summary>
    /// Summary description for EvaluationReport.
    /// </summary>
    public partial class EvaluationReportService : Telerik.Reporting.Report
    {
        public EvaluationReportService()
        {
            InitializeComponent();

        }

        private DataTable GetEvaluationData()
        {
            DataTable dt = new DataTable();
            return dt;
        }
    }
}