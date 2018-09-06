namespace SmartEP.ReportLibrary
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Text;
    using System.Data;
    using System.Data.SqlClient;
    using System.Configuration;
    using SmartEP.Data.SqlServer.MonitoringBusiness.Water;
    using SmartEP.Data.SqlServer.BaseData;

    /// <summary>
    /// Summary description for YearReport.
    /// </summary>
    public partial class Rep_MonthSelf : Telerik.Reporting.Report
    {
        public Rep_MonthSelf()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public static string getValidDataRate(object StartDay, object EndDay, object point)
        {

            try
            {

                HourReportDAL DAL = new HourReportDAL();
                MonitoringPointDAL BaseDAL = new MonitoringPointDAL();
                DateTime startTime = Convert.ToDateTime(StartDay);
                DateTime endTime = Convert.ToDateTime(EndDay);
                int sumShould = 0;
                int dataCycle = 0;
                int factorCount = 0;
                string[] factorList;
                int sumAll = DAL.GetDataCountByPointId(point.ToString(), startTime, endTime);
                DataView waterDv = BaseDAL.GetDataCycleAndFactorList(point.ToString()).DefaultView;
                if (waterDv.Count > 0)
                {
                    factorList = waterDv[0]["EvaluateFactorList"].ToString().Split(';');
                    dataCycle = Convert.ToInt32(waterDv[0]["DataCycle"]);
                    factorCount = factorList.Length; ;
                }
                int days = (endTime - startTime).Days;
                sumShould = days * dataCycle * factorCount;
                decimal validRate = 0;
                if (sumShould != 0)
                    validRate = Math.Round(Convert.ToDecimal(sumAll * 100) / sumShould, 2);
                if (validRate <= 0)
                    validRate = 0;
                else if (validRate > 100)
                    validRate = 100;

                return "数据有效率：" + validRate.ToString() + "%";
            }
            catch (Exception ex)
            {
                return "出错啦！" + ex.ToString();
            }

        }

        public static DataView getDataSource(Object pointId,object StartDay, object EndDay)
        {
            DayReportDAL DayReportDAL = new DayReportDAL();
            DataView dv = DayReportDAL.GetRep_MonthSelfDataSouce(Convert.ToString(pointId),Convert.ToDateTime(StartDay),Convert.ToDateTime(EndDay));
            return dv;
        }
    }
}