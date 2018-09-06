namespace SmartEP.ReportLibrary
{
    using SmartEP.Data.SqlServer.MonitoringBusiness.Water;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for WatershedsWQWeekReport.
    /// </summary>
    public partial class WatershedsWQWeekReport : Telerik.Reporting.Report
    {
        const String myConnName = "ConnStrEqmsAirReport";
        public WatershedsWQWeekReport()
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
            DayReportDAL DayReportDAL = new DayReportDAL();
            DateTime b = Convert.ToDateTime(beginTime);
            DateTime e = Convert.ToDateTime(endTime);
            string pId = portId.ToString();
            DataView dv = DayReportDAL.GetWatershedsWQWeekReportDataSouce(pId, b, e);
            return dv;
        }

        public static string getWQ(object portId, object beginTime, object endTime)
        {
            DayReportDAL DayReportDAL = new DayReportDAL();
            DateTime b = Convert.ToDateTime(beginTime);
            DateTime e = Convert.ToDateTime(endTime);
            string pId = portId.ToString();
            DataView dv = DayReportDAL.GetWatershedsWQDataSouce(pId, b, e);
            string p = string.Empty, d = string.Empty, c = string.Empty, n = string.Empty;
            if (dv.Count > 0)
            {
                p = dv[0]["PH"] != DBNull.Value ? Convert.ToDecimal(dv[0]["PH"]).ToString() : "null";
                d = dv[0]["溶解氧"] != DBNull.Value ? Convert.ToDecimal(dv[0]["溶解氧"]).ToString() : "null";
                c = dv[0]["高锰酸盐指数"] != DBNull.Value ? Convert.ToDecimal(dv[0]["高锰酸盐指数"]).ToString() : "null";
                n = dv[0]["氨氮"] != DBNull.Value ? Convert.ToDecimal(dv[0]["氨氮"]).ToString() : "null";
            }
            else
            {
                p = "null";
                d = "null";
                c = "null";
                n = "null";
            }
            string cls = DayReportDAL.GetLevel(pId, p, n, c, d, "NULL", "NULL") == "" ? "--" : DayReportDAL.GetLevel(pId, p, n, c, d, "NULL", "NULL");
            return cls + "类(总氮，总磷不参与水质评价)";
        }
    }
}