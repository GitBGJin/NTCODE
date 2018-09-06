namespace SmartEP.ReportLibrary
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
    using SmartEP.Utilities.DataTypes.ExtensionMethods;

    /// <summary>
    /// Summary description for VillageWeekRep.
    /// </summary>
    public partial class VillageWeekRep : Telerik.Reporting.Report
    {
        public VillageWeekRep()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public static DataTable GetData(DateTime beginTime, DateTime endTime, string year, string factorName, string factorCode)
        {
            try
            {
                RegionDayAQIDAL dal = new RegionDayAQIDAL();
                DateTime curBeginTime = beginTime;
                DateTime curEndTime = endTime;
                DateTime perBeginTime = beginTime.AddYears(-1);
                DateTime perEndTime = endTime.AddYears(-1);
                string monthB = beginTime.ToString("MM-dd");
                string monthE = endTime.ToString("MM-dd");
                DateTime baseBeginTime = Convert.ToDateTime(year + "-" + monthB + " 00:00:00");

                DateTime baseEndTime = Convert.ToDateTime(year + "-" + endTime.Month);
                if (baseEndTime.LastDayOfMonth().Day > endTime.Day)
                {
                    baseEndTime = Convert.ToDateTime(year + "-" + monthE + " 23:59:59");
                }
                else
                {
                    baseEndTime = baseEndTime.LastDayOfMonth();
                }

                int num = factorCode == "CO" ? 1 : 1000;

                #region 创建数据源
                DataTable SourceDT = new DataTable();
                SourceDT.Columns.Add("regionName", typeof(string));
                SourceDT.Columns.Add("baseValue", typeof(string));
                SourceDT.Columns.Add("perValue", typeof(string));
                SourceDT.Columns.Add("curValue", typeof(string));
                SourceDT.Columns.Add("compareBase", typeof(string));
                SourceDT.Columns.Add("comparePer", typeof(string));
                SourceDT.Columns.Add("baseRate", typeof(string));
                SourceDT.Columns.Add("perRate", typeof(string));
                SourceDT.Columns.Add("curRate", typeof(string));
                SourceDT.Columns.Add("compareBase2", typeof(string));
                SourceDT.Columns.Add("comparePer2", typeof(string));
                #endregion

                DataView dvCur = dal.GetVillageWeekRepSource(factorCode, num, curBeginTime, curEndTime);
                DataView dvPer = dal.GetVillageWeekRepSource(factorCode, num, perBeginTime, perEndTime);
                DataView dvBase = new DataView();
                if (year != "" && year != null)
                {

                dvBase = dal.GetVillageWeekRepSource(factorCode, num, baseBeginTime, baseEndTime);  //基数年
                }
                // DataView dvBase = dal.GetVillageWeekRepSource(factorCode, num, baseBeginTime, baseEndTime);
                List<string> regionName = new List<string>();
                regionName.Add("张家港市"); regionName.Add("常熟市"); regionName.Add("太仓市"); regionName.Add("昆山市");
                regionName.Add("吴江区"); regionName.Add("吴中区"); regionName.Add("相城区"); regionName.Add("姑苏区");
                regionName.Add("工业园区"); regionName.Add("高新区");
                foreach (string name in regionName)
                {
                    DataRow dr = SourceDT.NewRow();
                    dvBase.RowFilter = "regionName = '" + name + "'";
                    dvPer.RowFilter = "regionName = '" + name + "'";
                    dvCur.RowFilter = "regionName = '" + name + "'";
                    dr["regionName"] = name;

                    dr["baseValue"] = dvBase.Count > 0 ? dvBase[0][factorCode].ToString() : "--";
                    dr["perValue"] = dvPer.Count > 0 ? dvPer[0][factorCode].ToString() : "--";
                    dr["curValue"] = dvCur.Count > 0 ? dvCur[0][factorCode].ToString() : "--";
                    if (dvBase.Count > 0 && dvCur.Count > 0)
                    {
                        if (Convert.ToDecimal(dvBase[0][factorCode]) > 0)
                        {
                            dr["compareBase"] = dvBase.Count > 0 && dvCur.Count > 0 ? ((Convert.ToDecimal(dvCur[0][factorCode]) - Convert.ToDecimal(dvBase[0][factorCode])) / Convert.ToDecimal(dvBase[0][factorCode]) * 100).ToString("0.0") + "%" : "--";
                        }
                        else
                            dr["compareBase"] = "--";
                    }
                    else
                        dr["compareBase"] = "--";

                    if (dvPer.Count > 0 && dvCur.Count > 0)
                    {
                        if (Convert.ToDecimal(dvPer[0][factorCode]) > 0)
                        {
                            dr["comparePer"] = dvPer.Count > 0 && dvCur.Count > 0 ? ((Convert.ToDecimal(dvCur[0][factorCode]) - Convert.ToDecimal(dvPer[0][factorCode])) / Convert.ToDecimal(dvPer[0][factorCode]) * 100).ToString("0.0") + "%" : "--";
                        }
                        else
                            dr["comparePer"] = "--";
                    }
                    else
                        dr["comparePer"] = "--";

                    dr["baseRate"] = dvBase.Count > 0 ? dvBase[0]["DBRate"].ToString() + "%" : "--";
                    dr["perRate"] = dvPer.Count > 0 ? dvPer[0]["DBRate"].ToString() + "%" : "--";
                    dr["curRate"] = dvCur.Count > 0 ? dvCur[0]["DBRate"].ToString() + "%" : "--";
                    if (dvBase.Count > 0 && dvCur.Count > 0)
                    {
                        if (Convert.ToDecimal(dvBase[0]["DBRate"]) > 0)
                        {
                            dr["compareBase2"] = dvBase.Count > 0 && dvCur.Count > 0 ? (Convert.ToDecimal(dvCur[0]["DBRate"]) - Convert.ToDecimal(dvBase[0]["DBRate"])).ToString("0.0") + "%" : "--";
                        }
                        else
                        { dr["compareBas2e"] = "--"; }
                    }
                    else
                    { dr["compareBase2"] = "--"; }

                    if (dvPer.Count > 0 && dvCur.Count > 0)
                    {
                        if (Convert.ToDecimal(dvPer[0]["DBRate"]) > 0)
                        {
                            dr["comparePer2"] = dvPer.Count > 0 && dvCur.Count > 0 ? (Convert.ToDecimal(dvCur[0]["DBRate"]) - Convert.ToDecimal(dvPer[0]["DBRate"])).ToString("0.0") + "%" : "--";
                        }
                        else
                        { dr["comparePer2"] = "--"; }
                    }
                    else
                    { dr["comparePer2"] = "--"; }

                    SourceDT.Rows.Add(dr);
                }
                return SourceDT;
            }
            catch (Exception ex)
            {
                return null;

            }
        }


    }
}