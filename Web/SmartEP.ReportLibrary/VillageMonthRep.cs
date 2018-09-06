namespace SmartEP.ReportLibrary
{
    using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using SmartEP.Utilities.DataTypes.ExtensionMethods;

    /// <summary>
    /// Summary description for VillageMonthRep.
    /// </summary>
    public partial class VillageMonthRep : Telerik.Reporting.Report
    {
        public VillageMonthRep()
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
                int num = 1000;

                #region 创建数据源
                DataTable SourceDT = new DataTable();
                SourceDT.Columns.Add("regionName", typeof(string));
                SourceDT.Columns.Add("curRate", typeof(string));
                SourceDT.Columns.Add("compare", typeof(string));
                SourceDT.Columns.Add("curValue", typeof(string));
                SourceDT.Columns.Add("comparePer", typeof(string));
                SourceDT.Columns.Add("compareBase", typeof(string));
                #endregion

                DataView dvCur = dal.GetVillageWeekRepSource(factorCode, num, curBeginTime, curEndTime);  //本年
                DataView dvPer = dal.GetVillageWeekRepSource(factorCode, num, perBeginTime, perEndTime);  //前年
                DataView dvBase = new DataView();
                if (year != "")
                {
                    dvBase = dal.GetVillageWeekRepSource(factorCode, num, baseBeginTime, baseEndTime);  //基数年
                }
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
                  
                    dr["curRate"] = dvCur.Count > 0 ? dvCur[0]["DBRate"].ToString() + "%" : "--";
                    if (dvPer.Count > 0 && dvCur.Count > 0)
                    {
                        if (Convert.ToDecimal(dvPer[0]["DBRate"]) > 0)
                        {
                            dr["compare"] = dvPer.Count > 0 && dvCur.Count > 0 ? (Convert.ToDecimal(dvCur[0]["DBRate"]) - Convert.ToDecimal(dvPer[0]["DBRate"])).ToString("0.0") + "%" : "--";
                        }
                        else
                        { dr["compare"] = "--"; }
                    }
                    else
                    { dr["compare"] = "--"; }

                    dr["curValue"] = dvCur.Count > 0 ? dvCur[0][factorCode].ToString() : "--";
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