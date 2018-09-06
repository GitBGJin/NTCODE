namespace SmartEP.ReportLibrary
{
    using System;
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
    /// Summary description for THDrinkWaterMonitoringReport.
    /// </summary>
    public partial class THDrinkWaterMonitoringReport : Telerik.Reporting.Report
    {
        const String myConnName = "ConnStrEqmsAirReport";
        public THDrinkWaterMonitoringReport()
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
            DataView dv = DayReportDAL.GetTHDrinkWaterMonitoringReportDataSouce(Convert.ToString(portId), Convert.ToDateTime(beginTime), Convert.ToDateTime(endTime));
            DataTable sourceDT = new DataTable();
            sourceDT.Columns.Add("portname", typeof(string));
            sourceDT.Columns.Add("SW", typeof(string));
            sourceDT.Columns.Add("TMD", typeof(string));
            sourceDT.Columns.Add("RJY", typeof(string));
            sourceDT.Columns.Add("CODmn", typeof(string));
            sourceDT.Columns.Add("NH3", typeof(string));
            sourceDT.Columns.Add("TP", typeof(string));
            sourceDT.Columns.Add("TN", typeof(string));
            sourceDT.Columns.Add("YLS", typeof(string));
            sourceDT.Columns.Add("ZMD", typeof(string));
            sourceDT.Columns.Add("PH", typeof(string));

            MonitoringPointDAL MonitoringPointDAL = new MonitoringPointDAL();
            DataView dvPoint = MonitoringPointDAL.GetPointNameByID(Convert.ToString(portId)).DefaultView;
            string PointName = string.Empty;
            foreach (string p in Convert.ToString(portId).Split(','))
            {
                dvPoint.RowFilter = "pointid = '" + p + "'";
                PointName = dvPoint[0]["monitoringpointname"].ToString();

                dv.RowFilter = "portName = '" + PointName + "'";
                DataRow sdr = sourceDT.NewRow();
                if (dv.Count > 0)
                {
                    sdr["portname"] = PointName;
                    sdr["SW"] = dv[0]["SW"] != DBNull.Value ? dv[0]["SW"].ToString() : null;
                    sdr["TMD"] = dv[0]["TMD"] != DBNull.Value ? dv[0]["TMD"].ToString() : null;
                    sdr["RJY"] = dv[0]["RJY"] != DBNull.Value ? dv[0]["RJY"].ToString() : null;
                    sdr["CODmn"] = dv[0]["CODmn"] != DBNull.Value ? dv[0]["CODmn"].ToString() : null;
                    sdr["NH3"] = dv[0]["NH3"] != DBNull.Value ? dv[0]["NH3"].ToString() : null;
                    sdr["TP"] = dv[0]["TP"] != DBNull.Value ? dv[0]["TP"].ToString() : null;
                    sdr["TN"] = dv[0]["TN"] != DBNull.Value ? dv[0]["TN"].ToString() : null;
                    sdr["YLS"] = dv[0]["YLS"] != DBNull.Value ? dv[0]["YLS"].ToString() : null;
                    sdr["ZMD"] = dv[0]["ZMD"] != DBNull.Value ? dv[0]["ZMD"].ToString() : null;
                    sdr["PH"] = dv[0]["PH"] != DBNull.Value ? dv[0]["PH"].ToString() : null;

                    sourceDT.Rows.Add(sdr);
                }
                else
                {
                    sdr["portname"] = PointName;
                    sdr["SW"] = null;
                    sdr["TMD"] = null;
                    sdr["RJY"] = null;
                    sdr["CODmn"] = null;
                    sdr["NH3"] = null;
                    sdr["TP"] = null;
                    sdr["TN"] = null;
                    sdr["YLS"] = null;
                    sdr["ZMD"] = null;
                    sdr["PH"] = null;

                    sourceDT.Rows.Add(sdr);
                }

            }


            return sourceDT.DefaultView;
        }

        public static string getRate(object factor, object factorValue)
        {
            string result = string.Empty;
            switch (factor.ToString())
            {
                case "PH":
                    if (factorValue != null)
                    {
                        if (Convert.ToDecimal(factorValue) < 6)
                        {
                            //result = "(" +((6 - Convert.ToDecimal(factorValue)) / 6).ToString("0.00") + ")";
                            result = "(未达标)";
                        }
                        else if (Convert.ToDecimal(factorValue) > 9)
                        {
                            //result = "(" + ((Convert.ToDecimal(factorValue) - 9) / 9).ToString("0.00") + ")";
                            result = "(未达标)";
                        }
                        else
                            result = "/";
                    }
                    else
                        result = "/ ";
                    ;
                    break;
                case "RJY":
                    if (factorValue != null)
                    {
                        if (Convert.ToDecimal(factorValue) < 5)
                        {
                            result = "(" + ((5 - Convert.ToDecimal(factorValue)) / 5).ToString("0.00") + ")";
                        }
                        else
                            result = "/";
                    }
                    else
                        result = "/ ";
                    break;
                case "CODmn":
                    if (factorValue != null)
                    {
                        if (Convert.ToDecimal(factorValue) > 6)
                        {
                            result = "(" + ((Convert.ToDecimal(factorValue) - 6) / 6).ToString("0.00") + ")";
                        }
                        else
                            result = "/";
                    }
                    else
                        result = "(1.00)";
                    break;
                case "NH3":
                    if (factorValue != null)
                    {
                        if (Convert.ToDecimal(factorValue) > 1)
                        {
                            result = "(" + ((Convert.ToDecimal(factorValue) - 1) / 1).ToString("0.00") + ")";
                        }
                        else
                            result = "/";
                    }
                    else
                        result = "(1.00)";
                    break;
                case "TP":
                    if (factorValue != null)
                    {
                        if (Convert.ToDouble(factorValue) > 0.05)
                        {
                            result = "(" + (Convert.ToDecimal((Convert.ToDouble(factorValue) - 0.05) / 0.05)).ToString("0.00") + ")";
                        }
                        else
                            result = "/";
                    }
                    else
                        result = "(1.00)";
                    break;

                case "TN":
                    if (factorValue != null)
                    {
                        if (Convert.ToDecimal(factorValue) > 1)
                        {
                            result = "(" + ((Convert.ToDecimal(factorValue) - 1) / 1).ToString("0.00") + ")";
                        }
                        else
                            result = "/";
                    }
                    else
                        result = "(1.00)";
                    break;
            }
            return result.ToString();
        }
    }
}