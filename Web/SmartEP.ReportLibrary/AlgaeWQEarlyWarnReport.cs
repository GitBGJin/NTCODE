namespace SmartEP.ReportLibrary
{
    using System;
    using System.Web;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using SmartEP.Data.SqlServer.MonitoringBusiness.Water;
    using SmartEP.Data.SqlServer.BaseData;

    /// <summary>
    /// Summary description for AlgaeWQEarlyWarnReport.
    /// </summary>
    public partial class AlgaeWQEarlyWarnReport : Telerik.Reporting.Report
    {
        public AlgaeWQEarlyWarnReport()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public static DataView getDataSource(object portId, object beginTime, object endTime)
        {
            HourReportDAL HourReportDAL = new HourReportDAL();
            DateTime b = Convert.ToDateTime(beginTime);
            DateTime e = Convert.ToDateTime(endTime);
            string pId = portId.ToString().Replace(';', ',');
            DataView dv = HourReportDAL.GetAlgaeWQEarlyWarnReportDataSource(pId,b,e);
            return dv;


        }

        public static decimal getUpper(object portId, object factorCode)
        {
            MonitoringPointDAL MonitoringPointDAL = new MonitoringPointDAL();
            DataView dv = MonitoringPointDAL.GetPointPollutantStandard().DefaultView;
            dv.RowFilter = "PortId = " + portId.ToString() + " and PollutantCode = '"+factorCode.ToString()+"'";
            if (dv.Count > 0)
                return dv[0]["upper"] != DBNull.Value ? Convert.ToDecimal(dv[0]["upper"]) : 99999999999999999;
            else
                return 99999999999999999;
        }

        public static decimal getLow(object portId, object factorCode)
        {
            MonitoringPointDAL MonitoringPointDAL = new MonitoringPointDAL();
            DataView dv = MonitoringPointDAL.GetPointPollutantStandard().DefaultView;
            dv.RowFilter = "PortId = " + portId.ToString() + " and PollutantCode = '" + factorCode.ToString() + "'";
            if (dv.Count > 0)
                return dv[0]["low"] != DBNull.Value ? Convert.ToDecimal(dv[0]["low"]) : 0;
            else
                return 0;
        }
    }
}