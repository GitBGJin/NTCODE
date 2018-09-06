using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.MonitoringBusinessRepository.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.BaseData.Standard;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.Core.Enums;

namespace SmartEP.Service.DataAnalyze.Air.DataQuery
{
    /// <summary>
    /// 名称：DataQueryByDayService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：审核日数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataQueryByDayService
    {
        /// <summary>
        /// 空气日数据
        /// </summary>
        DayReportRepository DayData = Singleton<DayReportRepository>.GetInstance();
        /// <summary>
        /// 空气小时数据
        /// </summary>
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        /// <summary>
        /// 空气月数据
        /// </summary>
        MonthReportRepository MonthData = Singleton<MonthReportRepository>.GetInstance();
        /// <summary>
        /// 空气时数据
        /// </summary>
        HourReportRepository HourData = Singleton<HourReportRepository>.GetInstance();
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;

        /// <summary>
        /// 点位日AQI
        /// </summary>
        DayAQIRepository pointDayAQI = null;
        /// <summary>
        /// 点位时AQI
        /// </summary>
        HourAQIRepository pointHourAQI = null;

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayDataPager(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return DayData.GetDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetAvgDayDataPager(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return DayData.GetAvgDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayDataAvg(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return DayData.GetDayDataAvg(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetDayDataPager(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            if (factors.IsNotNullOrDBNull())
                return DayData.GetDataPager(portIds, factors, dtStart, dtEnd);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayData(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return DayData.GetDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayData(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd,
            DateTime dtFrom, DateTime dtTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime desc")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return DayData.GetDataPager(portIds, factors, dtStart, dtEnd, dtFrom, dtTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayDataDF(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd,
             int pageSize, int pageNo,  string orderBy = "PointId,DateTime desc")
        {
            
            if (factors.IsNotNullOrDBNull())
                return DayData.GetDataPagerDF(portIds, factors, dtStart, dtEnd,  pageSize, pageNo, orderBy);
            return null;
        }

        /// <summary>
        /// 例行月报表(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayDataPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("Datetime", typeof(string));
            dt.Columns.Add("STCODE", typeof(decimal));
            dt.Columns.Add("YE", typeof(decimal));
            dt.Columns.Add("MON", typeof(decimal));
            dt.Columns.Add("DA", typeof(int));
            dt.Columns.Add("PCODE", typeof(decimal));
            dt.Columns.Add("PointId", typeof(decimal));
            dt.Columns.Add("SMO", typeof(decimal));
            dt.Columns.Add("SDA", typeof(decimal));
            dt.Columns.Add("SHO", typeof(decimal));
            dt.Columns.Add("SMI", typeof(decimal));
            dt.Columns.Add("EMO", typeof(decimal));
            dt.Columns.Add("EDA", typeof(decimal));
            dt.Columns.Add("EHO", typeof(decimal));
            dt.Columns.Add("EMI", typeof(decimal));
            dt.Columns.Add("SO2", typeof(decimal));
            dt.Columns.Add("NOX", typeof(decimal));
            dt.Columns.Add("NO2", typeof(decimal));
            dt.Columns.Add("TSP", typeof(decimal));
            dt.Columns.Add("CO", typeof(decimal));
            dt.Columns.Add("PM10", typeof(decimal));
            dt.Columns.Add("PM2d5", typeof(decimal));
            dt.Columns.Add("O3_1", typeof(decimal));
            dt.Columns.Add("O3_8", typeof(decimal));
            dt.Columns.Add("WS", typeof(decimal));
            dt.Columns.Add("WD", typeof(decimal));
            dt.Columns.Add("TEMP", typeof(decimal));
            dt.Columns.Add("RH", typeof(decimal));
            dt.Columns.Add("PRESS", typeof(decimal));
            dt.Columns.Add("VISIBILITY", typeof(decimal));
            if (factors.IsNotNullOrDBNull())
            {
                DataTable dtDayReport = DayData.GetDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy).ToTable();
                dtDayReport.Columns.Add("PointCode", typeof(string));
                for (int j = 0; j < dtDayReport.Rows.Count; j++)//给DataType、portName字段赋值
                {
                    int pointId = Convert.ToInt32(dtDayReport.Rows[j]["PointId"]);
                    string str = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).Description;//获取站点名称
                    for (int i = 3; i > 0; i--)
                    {
                        string strO = str.Substring(11, i - 1);
                        if (strO != "0")
                        {
                            dtDayReport.Rows[j]["PointCode"] = str.Substring(str.Length - i);
                            break;
                        }
                    }
                }
                DataTable dvDayAQI = pointDayAQI.GetDataPager(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy).ToTable();
                dvDayAQI.Columns.Add("PointCode", typeof(string));
                for (int j = 0; j < dvDayAQI.Rows.Count; j++)//给DataType、portName字段赋值
                {
                    int pointId = Convert.ToInt32(dvDayAQI.Rows[j]["PointId"]);
                    string str = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).Description;//获取站点名称
                    for (int i = 3; i > 0; i--)
                    {
                        string strO = str.Substring(11, i - 1);
                        if (strO != "0")
                        {
                            dvDayAQI.Rows[j]["PointCode"] = str.Substring(str.Length - i);
                            break;
                        }
                    }
                }
                foreach (DataRow itemRow in dvDayAQI.Rows)
                {
                    DataRow drNew = dt.NewRow();
                    drNew["Datetime"] = itemRow["DateTime"];
                    if (itemRow["DateTime"] != DBNull.Value && itemRow["DateTime"].ToString() != "")
                    {
                        drNew["YE"] = Convert.ToDecimal(Convert.ToDateTime(itemRow["DateTime"]).Year);
                        drNew["MON"] = Convert.ToDecimal(Convert.ToDateTime(itemRow["DateTime"]).Month);
                        drNew["DA"] = Convert.ToInt32(Convert.ToDateTime(itemRow["DateTime"]).Day);
                    }
                    drNew["PCODE"] = Convert.ToDecimal(itemRow["PointCode"]);
                    drNew["PointId"] = Convert.ToInt32(itemRow["PointId"]);
                    drNew["STCODE"] = 320500;
                    if (itemRow["DateTime"] != DBNull.Value && itemRow["DateTime"].ToString() != "")
                    {
                        drNew["SMO"] = Convert.ToDecimal(Convert.ToDateTime(itemRow["DateTime"]).Month);
                        drNew["SDA"] = Convert.ToDecimal(Convert.ToDateTime(itemRow["DateTime"]).Day);
                        drNew["SHO"] = 0;
                        drNew["SMI"] = 0;
                        drNew["EMO"] = Convert.ToDecimal(Convert.ToDateTime(itemRow["DateTime"]).Month);
                        drNew["EDA"] = Convert.ToDecimal(Convert.ToDateTime(itemRow["DateTime"]).Day);
                        drNew["EHO"] = 23;
                        drNew["EMI"] = 59;
                    }
                    if (itemRow["SO2"] != DBNull.Value && itemRow["SO2"].ToString() != "")
                    {
                        drNew["SO2"] = Convert.ToDecimal(itemRow["SO2"]);
                    }
                    if (itemRow["NO2"] != DBNull.Value && itemRow["NO2"].ToString() != "")
                    {
                        drNew["NO2"] = Convert.ToDecimal(itemRow["NO2"]);
                    }
                    if (itemRow["CO"] != DBNull.Value && itemRow["CO"].ToString() != "")
                    {
                        drNew["CO"] = Convert.ToDecimal(itemRow["CO"]);
                    }
                    if (itemRow["PM10"] != DBNull.Value && itemRow["PM10"].ToString() != "")
                    {
                        drNew["PM10"] = Convert.ToDecimal(itemRow["PM10"]);
                    }
                    if (itemRow["PM25"] != DBNull.Value && itemRow["PM25"].ToString() != "")
                    {
                        drNew["PM2d5"] = Convert.ToDecimal(itemRow["PM25"]);
                    }
                    if (itemRow["MaxOneHourO3"] != DBNull.Value && itemRow["MaxOneHourO3"].ToString() != "")
                    {
                        drNew["O3_1"] = Convert.ToDecimal(itemRow["MaxOneHourO3"]);
                    }
                    if (itemRow["Max8HourO3"] != DBNull.Value && itemRow["Max8HourO3"].ToString() != "")
                    {
                        drNew["O3_8"] = Convert.ToDecimal(itemRow["Max8HourO3"]);
                    }
                    DataRow[] dr = dtDayReport.Select("DateTime='" + itemRow["DateTime"] + "' and " + "PointId='" + itemRow["PointId"] + "'");
                    dt.Rows.Add(drNew);
                }
                for (int i = 0; i < dtDayReport.Rows.Count; i++)
                {
                    DataRow[] dr = dt.Select("DateTime='" + dtDayReport.Rows[i]["DateTime"] + "' and " + "PointId='" + dtDayReport.Rows[i]["PointId"] + "'");
                    if (dr.Count() > 0)
                    {
                        if (dtDayReport.Rows[i]["a01007"] != DBNull.Value && dtDayReport.Rows[i]["a01007"].ToString() != "")
                        {
                            dr[0]["WS"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01007"]);
                        }
                        if (dtDayReport.Rows[i]["a01008"] != DBNull.Value && dtDayReport.Rows[i]["a01008"].ToString() != "")
                        {
                            dr[0]["WD"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01008"]);
                        }
                        if (dtDayReport.Rows[i]["a01001"] != DBNull.Value && dtDayReport.Rows[i]["a01001"].ToString() != "")
                        {
                            dr[0]["TEMP"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01001"]);
                        }
                        if (dtDayReport.Rows[i]["a01004"] != DBNull.Value && dtDayReport.Rows[i]["a01004"].ToString() != "")
                        {
                            dr[0]["RH"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01004"]);
                        }
                        if (dtDayReport.Rows[i]["a01006"] != DBNull.Value && dtDayReport.Rows[i]["a01006"].ToString() != "")
                        {
                            dr[0]["PRESS"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01006"]);
                        }
                        if (dtDayReport.Rows[i]["a01020"] != DBNull.Value && dtDayReport.Rows[i]["a01020"].ToString() != "")
                        {
                            dr[0]["VISIBILITY"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01020"]);
                        }
                        if (dtDayReport.Rows[i]["a21002"] != DBNull.Value && dtDayReport.Rows[i]["a21002"].ToString() != "")
                        {
                            dr[0]["NOX"] = Convert.ToDecimal(dtDayReport.Rows[i]["a21002"]);
                        }
                    }
                    else
                    {
                        DataRow drNew = dt.NewRow();
                        drNew["Datetime"] = dtDayReport.Rows[i]["DateTime"];
                        if (dtDayReport.Rows[i]["DateTime"] != DBNull.Value && dtDayReport.Rows[i]["DateTime"].ToString() != "")
                        {
                            drNew["YE"] = Convert.ToDecimal(Convert.ToDateTime(dtDayReport.Rows[i]["DateTime"]).Year);
                            drNew["MON"] = Convert.ToDecimal(Convert.ToDateTime(dtDayReport.Rows[i]["DateTime"]).Month);
                            drNew["DA"] = Convert.ToInt32(Convert.ToDateTime(dtDayReport.Rows[i]["DateTime"]).Day);
                        }
                        drNew["PCODE"] = Convert.ToDecimal(dtDayReport.Rows[i]["PointCode"]);
                        drNew["PointId"] = Convert.ToInt32(dtDayReport.Rows[i]["PointId"]);
                        drNew["STCODE"] = 320500;
                        if (dtDayReport.Rows[i]["DateTime"] != DBNull.Value && dtDayReport.Rows[i]["DateTime"].ToString() != "")
                        {
                            drNew["SMO"] = Convert.ToDecimal(Convert.ToDateTime(dtDayReport.Rows[i]["DateTime"]).Month);
                            drNew["SDA"] = Convert.ToDecimal(Convert.ToDateTime(dtDayReport.Rows[i]["DateTime"]).Day);
                            drNew["SHO"] = 0;
                            drNew["SMI"] = 0;
                            drNew["EMO"] = Convert.ToDecimal(Convert.ToDateTime(dtDayReport.Rows[i]["DateTime"]).Month);
                            drNew["EDA"] = Convert.ToDecimal(Convert.ToDateTime(dtDayReport.Rows[i]["DateTime"]).Day);
                            drNew["EHO"] = 23;
                            drNew["EMI"] = 59;
                        }
                        if (dtDayReport.Rows[i]["a01007"] != DBNull.Value && dtDayReport.Rows[i]["a01007"].ToString() != "")
                        {
                            drNew["WS"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01007"]);
                        }
                        if (dtDayReport.Rows[i]["a01008"] != DBNull.Value && dtDayReport.Rows[i]["a01008"].ToString() != "")
                        {
                            drNew["WD"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01008"]);
                        }
                        if (dtDayReport.Rows[i]["a01001"] != DBNull.Value && dtDayReport.Rows[i]["a01001"].ToString() != "")
                        {
                            drNew["TEMP"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01001"]);
                        }
                        if (dtDayReport.Rows[i]["a01004"] != DBNull.Value && dtDayReport.Rows[i]["a01004"].ToString() != "")
                        {
                            drNew["RH"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01004"]);
                        }
                        if (dtDayReport.Rows[i]["a01006"] != DBNull.Value && dtDayReport.Rows[i]["a01006"].ToString() != "")
                        {
                            drNew["PRESS"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01006"]);
                        }
                        if (dtDayReport.Rows[i]["a01020"] != DBNull.Value && dtDayReport.Rows[i]["a01020"].ToString() != "")
                        {
                            drNew["VISIBILITY"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01020"]);
                        }
                        if (dtDayReport.Rows[i]["a21002"] != DBNull.Value && dtDayReport.Rows[i]["a21002"].ToString() != "")
                        {
                            drNew["NOX"] = Convert.ToDecimal(dtDayReport.Rows[i]["a21002"]);
                        }
                        dt.Rows.Add(drNew);
                    }
                }
                dt.Columns.Remove("Datetime");
                dt.Columns.Remove("PointId");
                return dt.DefaultView;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 运行月报表结果统计(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,MonthOfYear）</param>
        /// <returns></returns>
        public DataView GetMonthDataPager(string[] portIds, IList<IPollutant> factors, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            DateTime dtStart = new DateTime(yearFrom, monthOfYearFrom, 1);
            DateTime dtEndTemp = new DateTime(yearTo, monthOfYearTo, 1);
            DateTime dtEnd = dtEndTemp.AddMonths(1).AddMilliseconds(-1);
            DataView dvDayAQI = new DataView();
            if (factors.IsNotNullOrDBNull())
            {
                bool flag = false;
                foreach (IPollutant factor in factors)
                {
                    if (factor.PollutantCode == "a05024")
                    {
                        flag = true;
                    }
                }

                if (flag)
                {
                    dvDayAQI = pointDayAQI.GetRunMonthPager(portIds, dtStart, dtEnd);
                }
                if (dvDayAQI.Count > 0)
                {
                    for (int i = 0; i < dvDayAQI.Count; i++)
                    {
                        if (dvDayAQI[i]["AQIValue"].IsNotNullOrDBNull())
                        {
                            if (monthOfYearTo != 2 && Convert.ToInt32(dvDayAQI[i]["AQIValue"]) < 27)
                            {
                                dvDayAQI[i]["a21026"] = -1;
                                dvDayAQI[i]["a21004"] = -1;
                                dvDayAQI[i]["a34002"] = -1;
                                dvDayAQI[i]["a21005"] = -1;
                                dvDayAQI[i]["MaxOneHourO3"] = -1;
                                dvDayAQI[i]["Max8HourO3"] = -1;
                                dvDayAQI[i]["a34004"] = -1;
                            }
                            else if (monthOfYearTo == 2 && Convert.ToInt32(dvDayAQI[i]["AQIValue"]) < 25)
                            {
                                dvDayAQI[i]["a21026"] = -1;
                                dvDayAQI[i]["a21004"] = -1;
                                dvDayAQI[i]["a34002"] = -1;
                                dvDayAQI[i]["a21005"] = -1;
                                dvDayAQI[i]["MaxOneHourO3"] = -1;
                                dvDayAQI[i]["Max8HourO3"] = -1;
                                dvDayAQI[i]["a34004"] = -1;
                            }
                        }
                    }

                }
            }
            return dvDayAQI;
        }
        /// <summary>
        /// 运行月报表(子站运行情况)
        /// </summary>
        public DataView GetMonthRun(string[] portIds, DateTime dtStart)
        {
            string WeatherPollutant = System.Configuration.ConfigurationManager.AppSettings["WeatherPollutant"];
            string[] WeatherPollutantarry = WeatherPollutant.Split(',');
            string WeatherFactor = "";
            foreach (string Weatheritem in WeatherPollutantarry)
            {
                WeatherFactor += Weatheritem + ",";

            }
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            DateTime dtBegion = new DateTime(dtStart.Year, dtStart.Month, 1);
            DateTime dtEnd = dtBegion.AddMonths(1).AddMilliseconds(-1);
            int strMonthDays = dtEnd.Day;
            int strMonthHours = dtEnd.Day * 24;
            ////每页显示数据个数            
            //int pageSize = 999999;
            ////当前页的序号
            //int pageNo = 0;

            ////数据总行数
            //int recordTotal = 0;
            DataView hourWeatherReport = new DataView();
            string[] WeatherFactors = new string[] { };
            if (!string.IsNullOrEmpty(WeatherFactor))
            {

                WeatherFactors = WeatherFactor.Trim().Trim(',').Split(',');
                // hourWeatherReport = HourData.GetDataPager(portIds, WeatherFactors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
                hourWeatherReport = HourData.GetAvgDayData(portIds, WeatherFactors, dtBegion, dtEnd);
            }
            //    DataView hourReport = HourData.GetDataPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
            //   DataView dayReport = DayData.GetDataPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
            //   DataView dayAQI = pointDayAQI.GetDataPager(portIds, dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
            DataView dayAQI = pointDayAQI.GetDataMonthDayPager(portIds, dtBegion, dtEnd);
            //  DataView hourAQI = pointHourAQI.GetDataPager(portIds, dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
            // DataView hourAQI = pointHourAQI.GetDataHoursPager(portIds, dtBegion, dtEnd);
            DataTable hourDt = pointHourAQI.GetDataHoursPager(portIds, dtBegion, dtEnd).ToTable();
            hourDt.Columns.Add("AQIValue", typeof(int));
            for (int i = 0; i < hourDt.Rows.Count; i++)
            {
                int temp = Convert.ToInt32(hourDt.Rows[i][1]);
                for (int j = 1; j <= 7; j++)
                {
                    if (hourDt.Columns[j].ColumnName != "Max8O3Count")
                    {
                        if (temp > Convert.ToInt32(hourDt.Rows[i][j]))
                            temp = Convert.ToInt32(hourDt.Rows[i][j]);
                    }
                }
                hourDt.Rows[i]["AQIValue"] = temp;
            }
            DataView hourAQI = hourDt.DefaultView;
            int intValidMonthDays = 0;
            int intValidMonthHours = 0;
            int intPollute = 0;
            int intWeather = 0;

            DataTable dvMonthRun = new DataTable();
            dvMonthRun.Columns.Add("PointID", typeof(string));
            dvMonthRun.Columns.Add("MonthDays", typeof(int));
            dvMonthRun.Columns.Add("MonthHours", typeof(int));
            dvMonthRun.Columns.Add("ValidMonthDays", typeof(int));
            dvMonthRun.Columns.Add("ValidMonthHours", typeof(int));
            dvMonthRun.Columns.Add("intPollute", typeof(int));
            dvMonthRun.Columns.Add("intWeather", typeof(int));
            foreach (string pointId in portIds)
            {
                intPollute = 0;
                intWeather = 0;
                //for (int i = 0; i < dayAQI.Count; i++)
                //{
                //    if (pointId == dayAQI[i]["PointId"].ToString())
                //        intValidMonthDays = Convert.ToInt32(dayAQI[i]["AQIValue"]);
                //}
                if (dayAQI.Count > 0)
                {
                    if (dayAQI.ToTable().Select("PointId=" + pointId).Count() > 0)
                        intValidMonthDays = Convert.ToInt32(dayAQI.ToTable().Select("PointId=" + pointId)[0]["AQIValue"]);
                }
                //for (int i = 0; i < hourAQI.Count; i++)
                //{
                //    if (pointId == hourAQI[i]["PointId"].ToString())
                //        intValidMonthHours = Convert.ToInt32(hourAQI[i]["AQIValue"]);
                //}
                if (hourAQI.Count > 0)
                {
                    if (hourAQI.ToTable().Select("PointId=" + pointId).Count() > 0)
                    {
                        intValidMonthHours = Convert.ToInt32(hourAQI.ToTable().Select("PointId=" + pointId)[0]["AQIValue"]);
                        intPollute = Convert.ToInt32(hourAQI.ToTable().Select("PointId=" + pointId)[0]["SO2Count"]);
                        intPollute += Convert.ToInt32(hourAQI.ToTable().Select("PointId=" + pointId)[0]["NO2Count"]);
                        intPollute += Convert.ToInt32(hourAQI.ToTable().Select("PointId=" + pointId)[0]["COCount"]);
                        intPollute += Convert.ToInt32(hourAQI.ToTable().Select("PointId=" + pointId)[0]["MaxO3Count"]);
                        intPollute += Convert.ToInt32(hourAQI.ToTable().Select("PointId=" + pointId)[0]["Max8O3Count"]);
                        intPollute += Convert.ToInt32(hourAQI.ToTable().Select("PointId=" + pointId)[0]["PM10Count"]);
                        intPollute += Convert.ToInt32(hourAQI.ToTable().Select("PointId=" + pointId)[0]["PM25Count"]);
                    }
                }
                //foreach (string factor in factors)
                //{
                //    intPollute += hourReport.ToTable().Select(factor + " is not null and pointId=" + pointId).Count();
                //    if (factor == "a05024")
                //    {
                //        intPollute += hourAQI.ToTable().Select("Recent8HoursO3 is not null and PointId=" + pointId).Count();
                //    }
                //}

                //foreach (string weather in WeatherFactors)
                //{
                //    intWeather += hourWeatherReport.ToTable().Select(weather + " is not null and pointId=" + pointId).Count();
                //}

                if (hourWeatherReport.Count > 0)
                {
                    if (hourWeatherReport.ToTable().Select("PointId=" + pointId).Count() > 0)
                    {
                        intWeather = Convert.ToInt32(hourWeatherReport.ToTable().Select("PointId=" + pointId)[0]["TempCount"]);
                        intWeather += Convert.ToInt32(hourWeatherReport.ToTable().Select("PointId=" + pointId)[0]["RHCount"]);
                        intWeather += Convert.ToInt32(hourWeatherReport.ToTable().Select("PointId=" + pointId)[0]["PressCount"]);
                        intWeather += Convert.ToInt32(hourWeatherReport.ToTable().Select("PointId=" + pointId)[0]["WdCount"]);
                        intWeather += Convert.ToInt32(hourWeatherReport.ToTable().Select("PointId=" + pointId)[0]["WsCount"]);
                    }
                }
                DataRow drNew = dvMonthRun.NewRow();
                drNew["PointID"] = pointId;
                drNew["MonthDays"] = strMonthDays;
                drNew["MonthHours"] = strMonthHours;
                drNew["ValidMonthDays"] = intValidMonthDays;
                drNew["ValidMonthHours"] = intValidMonthHours;
                drNew["intPollute"] = intPollute;
                drNew["intWeather"] = intWeather;
                dvMonthRun.Rows.Add(drNew);
            }
            return dvMonthRun.DefaultView;
        }
        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetDayStatisticalData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                DataTable dt = dt = DayData.GetStatisticalData(portIds, factors.Select(p => p.PollutantCode).ToArray(), dateStart, dateEnd).Table;
                dt.Columns.Add("portName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                }
                return dt.DefaultView;
            }
            return null;
        }

        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetDayStatisticalData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                DataTable dt = dt = DayData.GetStatisticalData(portIds, factors, dateStart, dateEnd).Table;
                dt.Columns.Add("portName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                }
                return dt.DefaultView;
            }
            return null;
        }

        /// <summary>
        /// 取得虚拟分页查询数据(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayExportData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            if (DayData != null)
                return DayData.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
            return null;
        }

        /// <summary>
        /// 环境空气质量例行监测成果表导出
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetAQRoutineMonthReportExportData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            if (DayData != null)
                return DayData.GetAQRoutineMonthReportExportData(portIds, factors, dateStart, dateEnd, orderBy);
            return null;
        }



        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public int GetDayAllDataCount(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            if (DayData != null)
                return DayData.GetAllDataCount(portIds, dateStart, dateStart);
            return 0;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayReportPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return DayData.GetDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 日均值月报表(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayOfMonthDataPager(string[] portIds, string[] factorCodes, string[] factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("Datetime", typeof(string));
            dt.Columns.Add("PointId", typeof(decimal));
            dt.Columns.Add("DA", typeof(decimal));
            dt.Columns.Add("SO2", typeof(decimal));
            dt.Columns.Add("NO2", typeof(decimal));
            dt.Columns.Add("CO", typeof(decimal));
            dt.Columns.Add("O3-1h", typeof(decimal));
            dt.Columns.Add("O3-8h", typeof(decimal));
            dt.Columns.Add("PM10", typeof(decimal));
            dt.Columns.Add("PM2.5", typeof(decimal));
            dt.Columns.Add("Temp", typeof(decimal));
            dt.Columns.Add("RH", typeof(decimal));
            dt.Columns.Add("Press", typeof(decimal));
            dt.Columns.Add("Wd", typeof(decimal));
            dt.Columns.Add("Ws", typeof(decimal));

            if (factors.IsNotNullOrDBNull())
            {
                DataView dvDayReport = DayData.GetDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                DataView dvDayAQI = pointDayAQI.GetDataPager(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                DataTable dtDayReport = dvDayReport.ToTable();
                foreach (DataRow itemRow in dvDayAQI.ToTable().Rows)
                {
                    DataRow drNew = dt.NewRow();
                    drNew["Datetime"] = itemRow["DateTime"];
                    drNew["PointId"] = Convert.ToDecimal(itemRow["PointId"]);
                    if (itemRow["DateTime"] != DBNull.Value && itemRow["DateTime"].ToString() != "")
                    {
                        drNew["DA"] = Convert.ToDecimal(Convert.ToDateTime(itemRow["DateTime"]).Day);
                    }
                    if (itemRow["SO2"] != DBNull.Value && itemRow["SO2"].ToString() != "")
                    {
                        drNew["SO2"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["SO2"]) * 1000, 0);
                    }
                    if (itemRow["NO2"] != DBNull.Value && itemRow["NO2"].ToString() != "")
                    {
                        drNew["NO2"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["NO2"]) * 1000, 0);
                    }
                    if (itemRow["CO"] != DBNull.Value && itemRow["CO"].ToString() != "")
                    {
                        drNew["CO"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["CO"]), 1);
                    }
                    if (itemRow["PM10"] != DBNull.Value && itemRow["PM10"].ToString() != "")
                    {
                        drNew["PM10"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["PM10"]) * 1000, 0);
                    }
                    if (itemRow["PM25"] != DBNull.Value && itemRow["PM25"].ToString() != "")
                    {
                        drNew["PM2.5"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["PM25"]) * 1000, 0);
                    }
                    if (itemRow["MaxOneHourO3"] != DBNull.Value && itemRow["MaxOneHourO3"].ToString() != "")
                    {
                        drNew["O3-1h"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["MaxOneHourO3"]) * 1000, 0);
                    }
                    if (itemRow["Max8HourO3"] != DBNull.Value && itemRow["Max8HourO3"].ToString() != "")
                    {
                        drNew["O3-8h"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["Max8HourO3"]) * 1000, 0);
                    }
                    DataRow[] dr = dtDayReport.Select("DateTime='" + itemRow["DateTime"] + "' and " + "PointId='" + itemRow["PointId"] + "'");
                    dt.Rows.Add(drNew);
                }
                for (int i = 0; i < dvDayReport.Count; i++)
                {
                    DataRow[] dr = dt.Select("DateTime='" + dtDayReport.Rows[i]["DateTime"] + "' and " + "PointId='" + dtDayReport.Rows[i]["PointId"] + "'");
                    if (dr.Count() > 0)
                    {
                        if (dtDayReport.Rows[i]["a01001"] != DBNull.Value && dtDayReport.Rows[i]["a01001"].ToString() != "")
                        {
                            dr[0]["Temp"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01001"]);
                        }
                        if (dtDayReport.Rows[i]["a01002"] != DBNull.Value && dtDayReport.Rows[i]["a01002"].ToString() != "")
                        {
                            dr[0]["RH"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01002"]);
                        }
                        if (dtDayReport.Rows[i]["a01006"] != DBNull.Value && dtDayReport.Rows[i]["a01006"].ToString() != "")
                        {
                            dr[0]["Press"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01006"]);
                        }
                        if (dtDayReport.Rows[i]["a01008"] != DBNull.Value && dtDayReport.Rows[i]["a01008"].ToString() != "")
                        {
                            dr[0]["Wd"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01008"]);
                        }
                        if (dtDayReport.Rows[i]["a01007"] != DBNull.Value && dtDayReport.Rows[i]["a01007"].ToString() != "")
                        {
                            dr[0]["Ws"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01007"]);
                        }
                    }
                    else
                    {
                        DataRow drNew = dt.NewRow();
                        drNew["Datetime"] = dtDayReport.Rows[i]["DateTime"];
                        if (dtDayReport.Rows[i]["DateTime"] != DBNull.Value && dtDayReport.Rows[i]["DateTime"].ToString() != "")
                        {
                            drNew["DA"] = Convert.ToDecimal(Convert.ToDateTime(dtDayReport.Rows[i]["DateTime"]).Day);
                        }
                        drNew["PointId"] = Convert.ToDecimal(dtDayReport.Rows[i]["PointId"]);
                        if (dtDayReport.Rows[i]["a01001"] != DBNull.Value && dtDayReport.Rows[i]["a01001"].ToString() != "")
                        {
                            drNew["Temp"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01001"]);
                        }
                        if (dtDayReport.Rows[i]["a01002"] != DBNull.Value && dtDayReport.Rows[i]["a01002"].ToString() != "")
                        {
                            drNew["RH"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01002"]);
                        }
                        if (dtDayReport.Rows[i]["a01006"] != DBNull.Value && dtDayReport.Rows[i]["a01006"].ToString() != "")
                        {
                            drNew["Press"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01006"]);
                        }
                        if (dtDayReport.Rows[i]["a01008"] != DBNull.Value && dtDayReport.Rows[i]["a01008"].ToString() != "")
                        {
                            drNew["Wd"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01008"]);
                        }
                        if (dtDayReport.Rows[i]["a01007"] != DBNull.Value && dtDayReport.Rows[i]["a01007"].ToString() != "")
                        {
                            drNew["Ws"] = Convert.ToDecimal(dtDayReport.Rows[i]["a01007"]);
                        }
                        dt.Rows.Add(drNew);
                    }
                }
                dt.Columns.Remove("Datetime");
                return dt.DefaultView;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 月日均值统计
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// </returns>
        public DataView GetAvgDayOfMonthData(string[] portIds, string[] factorCodes, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            EQIConcentrationService EQIConcentration = new EQIConcentrationService();
            EQIConcentrationLimitEntity entity = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("PointId", typeof(decimal));
            dt.Columns.Add("DA", typeof(string));
            dt.Columns.Add("SO2", typeof(decimal));
            dt.Columns.Add("NO2", typeof(decimal));
            dt.Columns.Add("CO", typeof(decimal));
            dt.Columns.Add("O3-1h", typeof(decimal));
            dt.Columns.Add("O3-8h", typeof(decimal));
            dt.Columns.Add("PM10", typeof(decimal));
            dt.Columns.Add("PM2.5", typeof(decimal));
            dt.Columns.Add("Temp", typeof(decimal));
            dt.Columns.Add("RH", typeof(decimal));
            dt.Columns.Add("Press", typeof(decimal));
            dt.Columns.Add("Wd", typeof(decimal));
            dt.Columns.Add("Ws", typeof(decimal));

            if (factors.IsNotNullOrDBNull())
            {
                DataView dvDayReport = DayData.GetAvgDayData(portIds, factors, dtStart, dtEnd);  //全月均值，最大最小值(5个)
                DataView dvDayAQI = pointDayAQI.GetAvgDayData(portIds, dtStart, dtEnd);  //全月均值，最大最小值(6个常规因子)
                DataView dvHourReport = m_HourData.GetAvgDayData(portIds, factors, dtStart, dtEnd);  //全月运行小时数及最大值(5个)
                DataView dvHourAQI = pointHourAQI.GetAvgDayData(portIds, dtStart, dtEnd);  //全月运行小时数及最大值(6个常规因子)
                DataTable dtDayReport = dvDayReport.ToTable();
                DataTable dtHourReport = dvHourReport.ToTable();
                foreach (DataRow itemRow in dvDayAQI.ToTable().Rows)
                {
                    DataRow drNew = dt.NewRow();
                    drNew["PointId"] = Convert.ToDecimal(itemRow["PointId"]);
                    drNew["DA"] = "运行的有效天数";
                    if (itemRow["SO2Count"] != DBNull.Value && itemRow["SO2Count"].ToString() != "" && Convert.ToDecimal(itemRow["SO2Count"]) != 0)
                    {
                        drNew["SO2"] = Convert.ToDecimal(itemRow["SO2Count"]);
                    }
                    if (itemRow["NO2Count"] != DBNull.Value && itemRow["NO2Count"].ToString() != "" && Convert.ToDecimal(itemRow["NO2Count"]) != 0)
                    {
                        drNew["NO2"] = Convert.ToDecimal(itemRow["NO2Count"]);
                    }
                    if (itemRow["COCount"] != DBNull.Value && itemRow["COCount"].ToString() != "" && Convert.ToDecimal(itemRow["COCount"]) != 0)
                    {
                        drNew["CO"] = Convert.ToDecimal(itemRow["COCount"]);
                    }
                    if (itemRow["MaxO3Count"] != DBNull.Value && itemRow["MaxO3Count"].ToString() != "" && Convert.ToDecimal(itemRow["MaxO3Count"]) != 0)
                    {
                        drNew["O3-1h"] = Convert.ToDecimal(itemRow["MaxO3Count"]);
                    }
                    if (itemRow["Max8O3Count"] != DBNull.Value && itemRow["Max8O3Count"].ToString() != "" && Convert.ToDecimal(itemRow["Max8O3Count"]) != 0)
                    {
                        drNew["O3-8h"] = Convert.ToDecimal(itemRow["Max8O3Count"]);
                    }
                    if (itemRow["PM10Count"] != DBNull.Value && itemRow["PM10Count"].ToString() != "" && Convert.ToDecimal(itemRow["PM10Count"]) != 0)
                    {
                        drNew["PM10"] = Convert.ToDecimal(itemRow["PM10Count"]);
                    }
                    if (itemRow["PM25Count"] != DBNull.Value && itemRow["PM25Count"].ToString() != "" && Convert.ToDecimal(itemRow["PM25Count"]) != 0)
                    {
                        drNew["PM2.5"] = Convert.ToDecimal(itemRow["PM25Count"]);
                    }
                    dt.Rows.Add(drNew);
                    for (int i = 0; i < dvDayReport.Count; i++)
                    {
                        DataRow[] dr = dt.Select("PointId='" + dtDayReport.Rows[i]["PointId"] + "'");
                        if (dr.Count() > 0)
                        {
                            if (dtDayReport.Rows[i]["TempCount"] != DBNull.Value && dtDayReport.Rows[i]["TempCount"].ToString() != "" && Convert.ToDecimal(dtDayReport.Rows[i]["TempCount"]) != 0)
                            {
                                dr[0]["Temp"] = Convert.ToDecimal(dtDayReport.Rows[i]["TempCount"]);
                            }
                            if (dtDayReport.Rows[i]["RHCount"] != DBNull.Value && dtDayReport.Rows[i]["RHCount"].ToString() != "" && Convert.ToDecimal(dtDayReport.Rows[i]["RHCount"]) != 0)
                            {
                                dr[0]["RH"] = Convert.ToDecimal(dtDayReport.Rows[i]["RHCount"]);
                            }
                            if (dtDayReport.Rows[i]["PressCount"] != DBNull.Value && dtDayReport.Rows[i]["PressCount"].ToString() != "" && Convert.ToDecimal(dtDayReport.Rows[i]["PressCount"]) != 0)
                            {
                                dr[0]["Press"] = Convert.ToDecimal(dtDayReport.Rows[i]["PressCount"]);
                            }
                            if (dtDayReport.Rows[i]["WdCount"] != DBNull.Value && dtDayReport.Rows[i]["WdCount"].ToString() != "" && Convert.ToDecimal(dtDayReport.Rows[i]["WdCount"]) != 0)
                            {
                                dr[0]["Wd"] = Convert.ToDecimal(dtDayReport.Rows[i]["WdCount"]);
                            }
                            if (dtDayReport.Rows[i]["WsCount"] != DBNull.Value && dtDayReport.Rows[i]["WsCount"].ToString() != "" && Convert.ToDecimal(dtDayReport.Rows[i]["WsCount"]) != 0)
                            {
                                dr[0]["Ws"] = Convert.ToDecimal(dtDayReport.Rows[i]["WsCount"]);
                            }
                        }
                        else
                        {
                            drNew["DA"] = "运行的有效天数";
                            drNew["PointId"] = Convert.ToDecimal(dtDayReport.Rows[i]["PointId"]);
                            if (dtDayReport.Rows[i]["TempCount"] != DBNull.Value && dtDayReport.Rows[i]["TempCount"].ToString() != "" && Convert.ToDecimal(dtDayReport.Rows[i]["TempCount"]) != 0)
                            {
                                drNew["Temp"] = Convert.ToDecimal(dtDayReport.Rows[i]["TempCount"]);
                            }
                            if (dtDayReport.Rows[i]["RHCount"] != DBNull.Value && dtDayReport.Rows[i]["RHCount"].ToString() != "" && Convert.ToDecimal(dtDayReport.Rows[i]["RHCount"]) != 0)
                            {
                                drNew["RH"] = Convert.ToDecimal(dtDayReport.Rows[i]["RHCount"]);
                            }
                            if (dtDayReport.Rows[i]["PressCount"] != DBNull.Value && dtDayReport.Rows[i]["PressCount"].ToString() != "" && Convert.ToDecimal(dtDayReport.Rows[i]["PressCount"]) != 0)
                            {
                                drNew["Press"] = Convert.ToDecimal(dtDayReport.Rows[i]["PressCount"]);
                            }
                            if (dtDayReport.Rows[i]["WdCount"] != DBNull.Value && dtDayReport.Rows[i]["WdCount"].ToString() != "" && Convert.ToDecimal(dtDayReport.Rows[i]["WdCount"]) != 0)
                            {
                                drNew["Wd"] = Convert.ToDecimal(dtDayReport.Rows[i]["WdCount"]);
                            }
                            if (dtDayReport.Rows[i]["WsCount"] != DBNull.Value && dtDayReport.Rows[i]["WsCount"].ToString() != "" && Convert.ToDecimal(dtDayReport.Rows[i]["WsCount"]) != 0)
                            {
                                drNew["Ws"] = Convert.ToDecimal(dtDayReport.Rows[i]["WsCount"]);
                            }
                            dt.Rows.Add(drNew);
                        }
                    }
                    DataRow newRows = dt.NewRow();
                    newRows["PointId"] = Convert.ToDecimal(itemRow["PointId"]);
                    newRows["DA"] = "月(日)平均值";
                    if (itemRow["SO2Value"] != DBNull.Value && itemRow["SO2Value"].ToString() != "")
                    {
                        newRows["SO2"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["SO2Value"]) * 1000, 0);
                    }
                    if (itemRow["NO2Value"] != DBNull.Value && itemRow["NO2Value"].ToString() != "")
                    {
                        newRows["NO2"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["NO2Value"]) * 1000, 0);
                    }
                    if (itemRow["COValue"] != DBNull.Value && itemRow["COValue"].ToString() != "")
                    {
                        newRows["CO"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["COValue"]), 1);
                    }
                    if (itemRow["MaxO3Value"] != DBNull.Value && itemRow["MaxO3Value"].ToString() != "")
                    {
                        newRows["O3-1h"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["MaxO3Value"]) * 1000, 0);
                    }
                    if (itemRow["Max8O3Value"] != DBNull.Value && itemRow["Max8O3Value"].ToString() != "")
                    {
                        newRows["O3-8h"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["Max8O3Value"]) * 1000, 0);
                    }
                    if (itemRow["PM10Value"] != DBNull.Value && itemRow["PM10Value"].ToString() != "")
                    {
                        newRows["PM10"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["PM10Value"]) * 1000, 0);
                    }
                    if (itemRow["PM25Value"] != DBNull.Value && itemRow["PM25Value"].ToString() != "")
                    {
                        newRows["PM2.5"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["PM25Value"]) * 1000, 0);
                    }
                    dt.Rows.Add(newRows);
                    for (int i = 0; i < dvDayReport.Count; i++)
                    {
                        DataRow[] dr = dt.Select("PointId='" + dtDayReport.Rows[i]["PointId"] + "'");
                        if (dr.Count() > 0)
                        {
                            if (dtDayReport.Rows[i]["TempValue"] != DBNull.Value && dtDayReport.Rows[i]["TempValue"].ToString() != "")
                            {
                                dr[1]["Temp"] = Convert.ToDecimal(dtDayReport.Rows[i]["TempValue"]);
                            }
                            if (dtDayReport.Rows[i]["RHValue"] != DBNull.Value && dtDayReport.Rows[i]["RHValue"].ToString() != "")
                            {
                                dr[1]["RH"] = Convert.ToDecimal(dtDayReport.Rows[i]["RHValue"]);
                            }
                            if (dtDayReport.Rows[i]["PressValue"] != DBNull.Value && dtDayReport.Rows[i]["PressValue"].ToString() != "")
                            {
                                dr[1]["Press"] = Convert.ToDecimal(dtDayReport.Rows[i]["PressValue"]);
                            }
                            if (dtDayReport.Rows[i]["WdValue"] != DBNull.Value && dtDayReport.Rows[i]["WdValue"].ToString() != "")
                            {
                                dr[1]["Wd"] = Convert.ToDecimal(dtDayReport.Rows[i]["WdValue"]);
                            }
                            if (dtDayReport.Rows[i]["WsValue"] != DBNull.Value && dtDayReport.Rows[i]["WsValue"].ToString() != "")
                            {
                                dr[1]["Ws"] = Convert.ToDecimal(dtDayReport.Rows[i]["WsValue"]);
                            }
                        }
                        else
                        {
                            newRows["DA"] = "月(日)平均值";
                            newRows["PointId"] = Convert.ToDecimal(dtDayReport.Rows[i]["PointId"]);
                            if (dtDayReport.Rows[i]["TempValue"] != DBNull.Value && dtDayReport.Rows[i]["TempValue"].ToString() != "")
                            {
                                newRows["Temp"] = Convert.ToDecimal(dtDayReport.Rows[i]["TempValue"]);
                            }
                            if (dtDayReport.Rows[i]["RHValue"] != DBNull.Value && dtDayReport.Rows[i]["RHValue"].ToString() != "")
                            {
                                newRows["RH"] = Convert.ToDecimal(dtDayReport.Rows[i]["RHValue"]);
                            }
                            if (dtDayReport.Rows[i]["PressValue"] != DBNull.Value && dtDayReport.Rows[i]["PressValue"].ToString() != "")
                            {
                                newRows["Press"] = Convert.ToDecimal(dtDayReport.Rows[i]["PressValue"]);
                            }
                            if (dtDayReport.Rows[i]["WdValue"] != DBNull.Value && dtDayReport.Rows[i]["WdValue"].ToString() != "")
                            {
                                newRows["Wd"] = Convert.ToDecimal(dtDayReport.Rows[i]["WdValue"]);
                            }
                            if (dtDayReport.Rows[i]["WsValue"] != DBNull.Value && dtDayReport.Rows[i]["WsValue"].ToString() != "")
                            {
                                newRows["Ws"] = Convert.ToDecimal(dtDayReport.Rows[i]["WsValue"]);
                            }
                            dt.Rows.Add(newRows);
                        }
                    }
                    DataRow newRow = dt.NewRow();
                    newRow["PointId"] = Convert.ToDecimal(itemRow["PointId"]);
                    newRow["DA"] = "最大日均值";
                    if (itemRow["SO2Max"] != DBNull.Value && itemRow["SO2Max"].ToString() != "")
                    {
                        newRow["SO2"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["SO2Max"]) * 1000, 0);
                    }
                    if (itemRow["NO2Max"] != DBNull.Value && itemRow["NO2Max"].ToString() != "")
                    {
                        newRow["NO2"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["NO2Max"]) * 1000, 0);
                    }
                    if (itemRow["COMax"] != DBNull.Value && itemRow["COMax"].ToString() != "")
                    {
                        newRow["CO"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["COMax"]), 1);
                    }
                    if (itemRow["MaxO3Max"] != DBNull.Value && itemRow["MaxO3Max"].ToString() != "")
                    {
                        newRow["O3-1h"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["MaxO3Max"]) * 1000, 0);
                    }
                    if (itemRow["Max8O3Max"] != DBNull.Value && itemRow["Max8O3Max"].ToString() != "")
                    {
                        newRow["O3-8h"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["Max8O3Max"]) * 1000, 0);
                    }
                    if (itemRow["PM10Max"] != DBNull.Value && itemRow["PM10Max"].ToString() != "")
                    {
                        newRow["PM10"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["PM10Max"]) * 1000, 0);
                    }
                    if (itemRow["PM25Max"] != DBNull.Value && itemRow["PM25Max"].ToString() != "")
                    {
                        newRow["PM2.5"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["PM25Max"]) * 1000, 0);
                    }
                    dt.Rows.Add(newRow);
                    for (int i = 0; i < dvDayReport.Count; i++)
                    {
                        DataRow[] dr = dt.Select("PointId='" + dtDayReport.Rows[i]["PointId"] + "'");
                        if (dr.Count() > 0)
                        {
                            if (dtDayReport.Rows[i]["TempMax"] != DBNull.Value && dtDayReport.Rows[i]["TempMax"].ToString() != "")
                            {
                                dr[2]["Temp"] = Convert.ToDecimal(dtDayReport.Rows[i]["TempMax"]);
                            }
                            if (dtDayReport.Rows[i]["RHMax"] != DBNull.Value && dtDayReport.Rows[i]["RHMax"].ToString() != "")
                            {
                                dr[2]["RH"] = Convert.ToDecimal(dtDayReport.Rows[i]["RHMax"]);
                            }
                            if (dtDayReport.Rows[i]["PressMax"] != DBNull.Value && dtDayReport.Rows[i]["PressMax"].ToString() != "")
                            {
                                dr[2]["Press"] = Convert.ToDecimal(dtDayReport.Rows[i]["PressMax"]);
                            }
                            if (dtDayReport.Rows[i]["WdMax"] != DBNull.Value && dtDayReport.Rows[i]["WdMax"].ToString() != "")
                            {
                                dr[2]["Wd"] = Convert.ToDecimal(dtDayReport.Rows[i]["WdMax"]);
                            }
                            if (dtDayReport.Rows[i]["WsMax"] != DBNull.Value && dtDayReport.Rows[i]["WsMax"].ToString() != "")
                            {
                                dr[2]["Ws"] = Convert.ToDecimal(dtDayReport.Rows[i]["WsMax"]);
                            }
                        }
                        else
                        {
                            newRow["DA"] = "最大日均值";
                            newRow["PointId"] = Convert.ToDecimal(dtDayReport.Rows[i]["PointId"]);
                            if (dtDayReport.Rows[i]["TempMax"] != DBNull.Value && dtDayReport.Rows[i]["TempMax"].ToString() != "")
                            {
                                newRow["Temp"] = Convert.ToDecimal(dtDayReport.Rows[i]["TempMax"]);
                            }
                            if (dtDayReport.Rows[i]["RHMax"] != DBNull.Value && dtDayReport.Rows[i]["RHMax"].ToString() != "")
                            {
                                newRow["RH"] = Convert.ToDecimal(dtDayReport.Rows[i]["RHMax"]);
                            }
                            if (dtDayReport.Rows[i]["PressMax"] != DBNull.Value && dtDayReport.Rows[i]["PressMax"].ToString() != "")
                            {
                                newRow["Press"] = Convert.ToDecimal(dtDayReport.Rows[i]["PressMax"]);
                            }
                            if (dtDayReport.Rows[i]["WdMax"] != DBNull.Value && dtDayReport.Rows[i]["WdMax"].ToString() != "")
                            {
                                newRow["Wd"] = Convert.ToDecimal(dtDayReport.Rows[i]["WdMax"]);
                            }
                            if (dtDayReport.Rows[i]["WsMax"] != DBNull.Value && dtDayReport.Rows[i]["WsMax"].ToString() != "")
                            {
                                newRow["Ws"] = Convert.ToDecimal(dtDayReport.Rows[i]["WsMax"]);
                            }
                            dt.Rows.Add(newRow);
                        }
                    }
                    DataRow Row = dt.NewRow();
                    Row["PointId"] = Convert.ToDecimal(itemRow["PointId"]);
                    Row["DA"] = "最大超标倍数";
                    if (itemRow["SO2Max"] != DBNull.Value && itemRow["SO2Max"].ToString() != "")
                    {
                        entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.TwentyFour);
                        if (entity.IsNotNullOrDBNull() && Convert.ToDecimal(itemRow["SO2Max"]) > entity.Upper)
                            Row["SO2"] = DecimalExtension.GetPollutantValue((Convert.ToDecimal(itemRow["SO2Max"]) - Convert.ToDecimal(entity.Upper)) / Convert.ToDecimal(entity.Upper), 3).ToString();
                    }
                    if (itemRow["NO2Max"] != DBNull.Value && itemRow["NO2Max"].ToString() != "")
                    {
                        entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.TwentyFour);
                        if (entity.IsNotNullOrDBNull() && Convert.ToDecimal(itemRow["NO2Max"]) > entity.Upper)
                            Row["NO2"] = DecimalExtension.GetPollutantValue((Convert.ToDecimal(itemRow["NO2Max"]) - Convert.ToDecimal(entity.Upper)) / Convert.ToDecimal(entity.Upper), 3).ToString();
                    }
                    if (itemRow["COMax"] != DBNull.Value && itemRow["COMax"].ToString() != "")
                    {
                        entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.TwentyFour);
                        if (entity.IsNotNullOrDBNull() && Convert.ToDecimal(itemRow["COMax"]) > entity.Upper)
                            Row["CO"] = DecimalExtension.GetPollutantValue((Convert.ToDecimal(itemRow["COMax"]) - Convert.ToDecimal(entity.Upper)) / Convert.ToDecimal(entity.Upper), 3).ToString();
                    }
                    //if (itemRow["MaxO3Max"] != DBNull.Value && itemRow["MaxO3Max"].ToString() != "")
                    //{
                    //    string O3_1 = System.Configuration.ConfigurationManager.AppSettings["O3-1h"];
                    //    decimal O31h = Convert.ToDecimal(O3_1);
                    //    //entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.TwentyFour);
                    //    if (Convert.ToDecimal(itemRow["MaxO3Max"]) > O31h)
                    //        Row["O3-1h"] = DecimalExtension.GetPollutantValue((Convert.ToDecimal(itemRow["MaxO3Max"]) - O31h) / O31h, 3).ToString();
                    //}
                    if (itemRow["Max8O3Max"] != DBNull.Value && itemRow["Max8O3Max"].ToString() != "")
                    {
                        string O3_8 = System.Configuration.ConfigurationManager.AppSettings["O3-8h"];
                        decimal O38h = Convert.ToDecimal(O3_8);
                        //entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.TwentyFour);
                        if (Convert.ToDecimal(itemRow["Max8O3Max"]) > O38h)
                            Row["O3-8h"] = DecimalExtension.GetPollutantValue((Convert.ToDecimal(itemRow["Max8O3Max"]) - O38h) / O38h, 3).ToString();
                    }
                    if (itemRow["PM10Max"] != DBNull.Value && itemRow["PM10Max"].ToString() != "")
                    {
                        entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.TwentyFour);
                        if (entity.IsNotNullOrDBNull() && Convert.ToDecimal(itemRow["PM10Max"]) > entity.Upper)
                            Row["PM10"] = DecimalExtension.GetPollutantValue((Convert.ToDecimal(itemRow["PM10Max"]) - Convert.ToDecimal(entity.Upper)) / Convert.ToDecimal(entity.Upper), 3).ToString();
                    }
                    if (itemRow["PM25Max"] != DBNull.Value && itemRow["PM25Max"].ToString() != "")
                    {
                        entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.TwentyFour);
                        if (entity.IsNotNullOrDBNull() && Convert.ToDecimal(itemRow["PM25Max"]) > entity.Upper)
                            Row["PM2.5"] = DecimalExtension.GetPollutantValue((Convert.ToDecimal(itemRow["PM25Max"]) - Convert.ToDecimal(entity.Upper)) / Convert.ToDecimal(entity.Upper), 3).ToString();
                    }
                    dt.Rows.Add(Row);

                    DataRow dayRow = dt.NewRow();
                    dayRow["PointId"] = Convert.ToDecimal(itemRow["PointId"]);
                    dayRow["DA"] = "日均值超标率(%)";
                    if (itemRow["SO2Count"] != DBNull.Value && Convert.ToDecimal(itemRow["SO2Count"]) != 0)
                    {
                        if (itemRow["SO2Xian"] != DBNull.Value)
                        {
                            dayRow["SO2"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["SO2Xian"]) / Convert.ToDecimal(itemRow["SO2Count"]) * 100, 0);
                        }
                    }
                    if (itemRow["NO2Count"] != DBNull.Value && Convert.ToDecimal(itemRow["NO2Count"]) != 0)
                    {
                        if (itemRow["NO2Xian"] != DBNull.Value)
                        {
                            dayRow["NO2"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["NO2Xian"]) / Convert.ToDecimal(itemRow["NO2Count"]) * 100, 0);
                        }
                    }
                    if (itemRow["COCount"] != DBNull.Value && Convert.ToDecimal(itemRow["COCount"]) != 0)
                    {
                        if (itemRow["COXian"] != DBNull.Value && itemRow["COXian"].ToString() != "")
                        {
                            dayRow["CO"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["COXian"]) / Convert.ToDecimal(itemRow["COCount"]) * 100, 0);
                        }
                    }
                    //if (itemRow["MaxO3Count"] != DBNull.Value && Convert.ToDecimal(itemRow["MaxO3Count"]) != 0)
                    //{
                    //    if (itemRow["MaxO3Xian"] != DBNull.Value && itemRow["MaxO3Xian"].ToString() != "")
                    //    {
                    //        dayRow["O3-1h"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["MaxO3Xian"]) / Convert.ToDecimal(itemRow["MaxO3Count"]) * 100, 0);
                    //    }
                    //}
                    if (itemRow["Max8O3Count"] != DBNull.Value && Convert.ToDecimal(itemRow["Max8O3Count"]) != 0)
                    {
                        if (itemRow["Max8O3Xian"] != DBNull.Value && itemRow["Max8O3Xian"].ToString() != "")
                        {
                            dayRow["O3-8h"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["Max8O3Xian"]) / Convert.ToDecimal(itemRow["Max8O3Count"]) * 100, 0);
                        }
                    }
                    if (itemRow["PM10Count"] != DBNull.Value && Convert.ToDecimal(itemRow["PM10Count"]) != 0)
                    {
                        if (itemRow["PM10Xian"] != DBNull.Value)
                        {
                            dayRow["PM10"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["PM10Xian"]) / Convert.ToDecimal(itemRow["PM10Count"]) * 100, 0);
                        }
                    }
                    if (itemRow["PM25Count"] != DBNull.Value && Convert.ToDecimal(itemRow["PM25Count"]) != 0)
                    {
                        if (itemRow["PM25Xian"] != DBNull.Value)
                        {
                            dayRow["PM2.5"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["PM25Xian"]) / Convert.ToDecimal(itemRow["PM25Count"]) * 100, 0);
                        }
                    }
                    dt.Rows.Add(dayRow);

                    DataRow minRow = dt.NewRow();
                    minRow["PointId"] = Convert.ToDecimal(itemRow["PointId"]);
                    minRow["DA"] = "最小日均值";
                    if (itemRow["SO2Min"] != DBNull.Value && itemRow["SO2Min"].ToString() != "")
                    {
                        minRow["SO2"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["SO2Min"]) * 1000, 0);
                    }
                    if (itemRow["NO2Min"] != DBNull.Value && itemRow["NO2Min"].ToString() != "")
                    {
                        minRow["NO2"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["NO2Min"]) * 1000, 0);
                    }
                    if (itemRow["COMin"] != DBNull.Value && itemRow["COMin"].ToString() != "")
                    {
                        minRow["CO"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["COMin"]), 1);
                    }
                    if (itemRow["MaxO3Min"] != DBNull.Value && itemRow["MaxO3Min"].ToString() != "")
                    {
                        minRow["O3-1h"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["MaxO3Min"]) * 1000, 0);
                    }
                    if (itemRow["Max8O3Min"] != DBNull.Value && itemRow["Max8O3Min"].ToString() != "")
                    {
                        minRow["O3-8h"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["Max8O3Min"]) * 1000, 0);
                    }
                    if (itemRow["PM10Min"] != DBNull.Value && itemRow["PM10Min"].ToString() != "")
                    {
                        minRow["PM10"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["PM10Min"]) * 1000, 0);
                    }
                    if (itemRow["PM25Min"] != DBNull.Value && itemRow["PM25Min"].ToString() != "")
                    {
                        minRow["PM2.5"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["PM25Min"]) * 1000, 0);
                    }
                    dt.Rows.Add(minRow);
                    for (int i = 0; i < dvDayReport.Count; i++)
                    {
                        DataRow[] dr = dt.Select("PointId='" + dtDayReport.Rows[i]["PointId"] + "'");
                        if (dr.Count() > 0)
                        {
                            if (dtDayReport.Rows[i]["TempMin"] != DBNull.Value && dtDayReport.Rows[i]["TempMin"].ToString() != "")
                            {
                                dr[5]["Temp"] = Convert.ToDecimal(dtDayReport.Rows[i]["TempMin"]);
                            }
                            if (dtDayReport.Rows[i]["RHMin"] != DBNull.Value && dtDayReport.Rows[i]["RHMin"].ToString() != "")
                            {
                                dr[5]["RH"] = Convert.ToDecimal(dtDayReport.Rows[i]["RHMin"]);
                            }
                            if (dtDayReport.Rows[i]["PressMin"] != DBNull.Value && dtDayReport.Rows[i]["PressMin"].ToString() != "")
                            {
                                dr[5]["Press"] = Convert.ToDecimal(dtDayReport.Rows[i]["PressMin"]);
                            }
                            if (dtDayReport.Rows[i]["WdMin"] != DBNull.Value && dtDayReport.Rows[i]["WdMin"].ToString() != "")
                            {
                                dr[5]["Wd"] = Convert.ToDecimal(dtDayReport.Rows[i]["WdMin"]);
                            }
                            if (dtDayReport.Rows[i]["WsMin"] != DBNull.Value && dtDayReport.Rows[i]["WsMin"].ToString() != "")
                            {
                                dr[5]["Ws"] = Convert.ToDecimal(dtDayReport.Rows[i]["WsMin"]);
                            }
                        }
                        else
                        {
                            minRow["DA"] = "最小日均值";
                            minRow["PointId"] = Convert.ToDecimal(dtDayReport.Rows[i]["PointId"]);
                            if (dtDayReport.Rows[i]["TempMin"] != DBNull.Value && dtDayReport.Rows[i]["TempMin"].ToString() != "")
                            {
                                minRow["Temp"] = Convert.ToDecimal(dtDayReport.Rows[i]["TempMin"]);
                            }
                            if (dtDayReport.Rows[i]["RHMin"] != DBNull.Value && dtDayReport.Rows[i]["RHMin"].ToString() != "")
                            {
                                minRow["RH"] = Convert.ToDecimal(dtDayReport.Rows[i]["RHMin"]);
                            }
                            if (dtDayReport.Rows[i]["PressMin"] != DBNull.Value && dtDayReport.Rows[i]["PressMin"].ToString() != "")
                            {
                                minRow["Press"] = Convert.ToDecimal(dtDayReport.Rows[i]["PressMin"]);
                            }
                            if (dtDayReport.Rows[i]["WdMin"] != DBNull.Value && dtDayReport.Rows[i]["WdMin"].ToString() != "")
                            {
                                minRow["Wd"] = Convert.ToDecimal(dtDayReport.Rows[i]["WdMin"]);
                            }
                            if (dtDayReport.Rows[i]["WsMin"] != DBNull.Value && dtDayReport.Rows[i]["WsMin"].ToString() != "")
                            {
                                minRow["Ws"] = Convert.ToDecimal(dtDayReport.Rows[i]["WsMin"]);
                            }
                            dt.Rows.Add(minRow);
                        }
                    }
                }
                foreach (DataRow itemRow in dvHourAQI.ToTable().Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["PointId"] = Convert.ToDecimal(itemRow["PointId"]);
                    newRow["DA"] = "全月最大一次值";
                    if (itemRow["SO2Max"] != DBNull.Value && itemRow["SO2Max"].ToString() != "")
                    {
                        newRow["SO2"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["SO2Max"]) * 1000, 0);
                    }
                    if (itemRow["NO2Max"] != DBNull.Value && itemRow["NO2Max"].ToString() != "")
                    {
                        newRow["NO2"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["NO2Max"]) * 1000, 0);
                    }
                    if (itemRow["COMax"] != DBNull.Value && itemRow["COMax"].ToString() != "")
                    {
                        newRow["CO"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["COMax"]), 1);
                    }
                    if (itemRow["MaxO3Max"] != DBNull.Value && itemRow["MaxO3Max"].ToString() != "")
                    {
                        newRow["O3-1h"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["MaxO3Max"]) * 1000, 0);
                    }
                    if (itemRow["Max8O3Max"] != DBNull.Value && itemRow["Max8O3Max"].ToString() != "")
                    {
                        newRow["O3-8h"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["Max8O3Max"]) * 1000, 0);
                    }
                    if (itemRow["PM10Max"] != DBNull.Value && itemRow["PM10Max"].ToString() != "")
                    {
                        newRow["PM10"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["PM10Max"]) * 1000, 0);
                    }
                    if (itemRow["PM25Max"] != DBNull.Value && itemRow["PM25Max"].ToString() != "")
                    {
                        newRow["PM2.5"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(itemRow["PM25Max"]) * 1000, 0);
                    }
                    dt.Rows.Add(newRow);
                    for (int i = 0; i < dvHourReport.Count; i++)
                    {
                        DataRow[] dr = dt.Select("PointId='" + dtHourReport.Rows[i]["PointId"] + "'");
                        if (dr.Count() > 0)
                        {
                            if (dtHourReport.Rows[i]["TempMax"] != DBNull.Value && dtHourReport.Rows[i]["TempMax"].ToString() != "")
                            {
                                dr[6]["Temp"] = Convert.ToDecimal(dtHourReport.Rows[i]["TempMax"]);
                            }
                            if (dtHourReport.Rows[i]["RHMax"] != DBNull.Value && dtHourReport.Rows[i]["RHMax"].ToString() != "")
                            {
                                dr[6]["RH"] = Convert.ToDecimal(dtHourReport.Rows[i]["RHMax"]);
                            }
                            if (dtHourReport.Rows[i]["PressMax"] != DBNull.Value && dtHourReport.Rows[i]["PressMax"].ToString() != "")
                            {
                                dr[6]["Press"] = Convert.ToDecimal(dtHourReport.Rows[i]["PressMax"]);
                            }
                            if (dtHourReport.Rows[i]["WdMax"] != DBNull.Value && dtHourReport.Rows[i]["WdMax"].ToString() != "")
                            {
                                dr[6]["Wd"] = Convert.ToDecimal(dtHourReport.Rows[i]["WdMax"]);
                            }
                            if (dtHourReport.Rows[i]["WsMax"] != DBNull.Value && dtHourReport.Rows[i]["WsMax"].ToString() != "")
                            {
                                dr[6]["Ws"] = Convert.ToDecimal(dtHourReport.Rows[i]["WsMax"]);
                            }
                        }
                        else
                        {
                            newRow["DA"] = "全月最大一次值";
                            newRow["PointId"] = Convert.ToDecimal(dtHourReport.Rows[i]["PointId"]);
                            if (dtHourReport.Rows[i]["TempMax"] != DBNull.Value && dtHourReport.Rows[i]["TempMax"].ToString() != "")
                            {
                                newRow["Temp"] = Convert.ToDecimal(dtHourReport.Rows[i]["TempMax"]);
                            }
                            if (dtHourReport.Rows[i]["RHMax"] != DBNull.Value && dtHourReport.Rows[i]["RHMax"].ToString() != "")
                            {
                                newRow["RH"] = Convert.ToDecimal(dtHourReport.Rows[i]["RHMax"]);
                            }
                            if (dtHourReport.Rows[i]["PressMax"] != DBNull.Value && dtHourReport.Rows[i]["PressMax"].ToString() != "")
                            {
                                newRow["Press"] = Convert.ToDecimal(dtHourReport.Rows[i]["PressMax"]);
                            }
                            if (dtHourReport.Rows[i]["WdMax"] != DBNull.Value && dtHourReport.Rows[i]["WdMax"].ToString() != "")
                            {
                                newRow["Wd"] = Convert.ToDecimal(dtHourReport.Rows[i]["WdMax"]);
                            }
                            if (dtHourReport.Rows[i]["WsMax"] != DBNull.Value && dtHourReport.Rows[i]["WsMax"].ToString() != "")
                            {
                                newRow["Ws"] = Convert.ToDecimal(dtHourReport.Rows[i]["WsMax"]);
                            }
                            dt.Rows.Add(newRow);
                        }
                    }
                    DataRow drNew = dt.NewRow();
                    drNew["PointId"] = Convert.ToDecimal(itemRow["PointId"]);
                    drNew["DA"] = "全月运行小时数";
                    if (itemRow["SO2Count"] != DBNull.Value && itemRow["SO2Count"].ToString() != "" && Convert.ToDecimal(itemRow["SO2Count"]) != 0)
                    {
                        drNew["SO2"] = Convert.ToDecimal(itemRow["SO2Count"]);
                    }
                    if (itemRow["NO2Count"] != DBNull.Value && itemRow["NO2Count"].ToString() != "" && Convert.ToDecimal(itemRow["NO2Count"]) != 0)
                    {
                        drNew["NO2"] = Convert.ToDecimal(itemRow["NO2Count"]);
                    }
                    if (itemRow["COCount"] != DBNull.Value && itemRow["COCount"].ToString() != "" && Convert.ToDecimal(itemRow["COCount"]) != 0)
                    {
                        drNew["CO"] = Convert.ToDecimal(itemRow["COCount"]);
                    }
                    if (itemRow["MaxO3Count"] != DBNull.Value && itemRow["MaxO3Count"].ToString() != "" && Convert.ToDecimal(itemRow["MaxO3Count"]) != 0)
                    {
                        drNew["O3-1h"] = Convert.ToDecimal(itemRow["MaxO3Count"]);
                    }
                    if (itemRow["Max8O3Count"] != DBNull.Value && itemRow["Max8O3Count"].ToString() != "" && Convert.ToDecimal(itemRow["Max8O3Count"]) != 0)
                    {
                        drNew["O3-8h"] = Convert.ToDecimal(itemRow["Max8O3Count"]);
                    }
                    if (itemRow["PM10Count"] != DBNull.Value && itemRow["PM10Count"].ToString() != "" && Convert.ToDecimal(itemRow["PM10Count"]) != 0)
                    {
                        drNew["PM10"] = Convert.ToDecimal(itemRow["PM10Count"]);
                    }
                    if (itemRow["PM25Count"] != DBNull.Value && itemRow["PM25Count"].ToString() != "" && Convert.ToDecimal(itemRow["PM25Count"]) != 0)
                    {
                        drNew["PM2.5"] = Convert.ToDecimal(itemRow["PM25Count"]);
                    }
                    dt.Rows.Add(drNew);
                    for (int i = 0; i < dvHourReport.Count; i++)
                    {
                        DataRow[] dr = dt.Select("PointId='" + dtHourReport.Rows[i]["PointId"] + "'");
                        if (dr.Count() > 0)
                        {
                            if (dtHourReport.Rows[i]["TempCount"] != DBNull.Value && dtHourReport.Rows[i]["TempCount"].ToString() != "" && Convert.ToDecimal(dtHourReport.Rows[i]["TempCount"]) != 0)
                            {
                                dr[7]["Temp"] = Convert.ToDecimal(dtHourReport.Rows[i]["TempCount"]);
                            }
                            if (dtHourReport.Rows[i]["RHCount"] != DBNull.Value && dtHourReport.Rows[i]["RHCount"].ToString() != "" && Convert.ToDecimal(dtHourReport.Rows[i]["RHCount"]) != 0)
                            {
                                dr[7]["RH"] = Convert.ToDecimal(dtHourReport.Rows[i]["RHCount"]);
                            }
                            if (dtHourReport.Rows[i]["PressCount"] != DBNull.Value && dtHourReport.Rows[i]["PressCount"].ToString() != "" && Convert.ToDecimal(dtHourReport.Rows[i]["PressCount"]) != 0)
                            {
                                dr[7]["Press"] = Convert.ToDecimal(dtHourReport.Rows[i]["PressCount"]);
                            }
                            if (dtHourReport.Rows[i]["WdCount"] != DBNull.Value && dtHourReport.Rows[i]["WdCount"].ToString() != "" && Convert.ToDecimal(dtHourReport.Rows[i]["WdCount"]) != 0)
                            {
                                dr[7]["Wd"] = Convert.ToDecimal(dtHourReport.Rows[i]["WdCount"]);
                            }
                            if (dtHourReport.Rows[i]["WsCount"] != DBNull.Value && dtHourReport.Rows[i]["WsCount"].ToString() != "" && Convert.ToDecimal(dtHourReport.Rows[i]["WsCount"]) != 0)
                            {
                                dr[7]["Ws"] = Convert.ToDecimal(dtHourReport.Rows[i]["WsCount"]);
                            }
                        }
                        else
                        {
                            drNew["DA"] = "全月运行小时数";
                            drNew["PointId"] = Convert.ToDecimal(dtHourReport.Rows[i]["PointId"]);
                            if (dtHourReport.Rows[i]["TempCount"] != DBNull.Value && dtHourReport.Rows[i]["TempCount"].ToString() != "" && Convert.ToDecimal(dtHourReport.Rows[i]["TempCount"]) != 0)
                            {
                                drNew["Temp"] = Convert.ToDecimal(dtHourReport.Rows[i]["TempCount"]);
                            }
                            if (dtHourReport.Rows[i]["RHCount"] != DBNull.Value && dtHourReport.Rows[i]["RHCount"].ToString() != "" && Convert.ToDecimal(dtHourReport.Rows[i]["RHCount"]) != 0)
                            {
                                drNew["RH"] = Convert.ToDecimal(dtHourReport.Rows[i]["RHCount"]);
                            }
                            if (dtHourReport.Rows[i]["PressCount"] != DBNull.Value && dtHourReport.Rows[i]["PressCount"].ToString() != "" && Convert.ToDecimal(dtHourReport.Rows[i]["PressCount"]) != 0)
                            {
                                drNew["Press"] = Convert.ToDecimal(dtHourReport.Rows[i]["PressCount"]);
                            }
                            if (dtHourReport.Rows[i]["WdCount"] != DBNull.Value && dtHourReport.Rows[i]["WdCount"].ToString() != "" && Convert.ToDecimal(dtHourReport.Rows[i]["WdCount"]) != 0)
                            {
                                drNew["Wd"] = Convert.ToDecimal(dtHourReport.Rows[i]["WdCount"]);
                            }
                            if (dtHourReport.Rows[i]["WsCount"] != DBNull.Value && dtHourReport.Rows[i]["WsCount"].ToString() != "" && Convert.ToDecimal(dtHourReport.Rows[i]["WsCount"]) != 0)
                            {
                                drNew["Ws"] = Convert.ToDecimal(dtHourReport.Rows[i]["WsCount"]);
                            }
                            dt.Rows.Add(drNew);
                        }
                    }
                }
                return dt.DefaultView;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 日均值月报表(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayOfMonthNewPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("Datetime", typeof(string));
            dt.Columns.Add("PointId", typeof(decimal));
            foreach (string factor in factors)
            {
                dt.Columns.Add(factor, typeof(decimal));
            }

            if (factors.IsNotNullOrDBNull())
            {
                DataView dvDayReport = HourData.GetDataNewPager(portIds, factors, dtStart, dtEnd);
                foreach (DataRow itemRow in dvDayReport.ToTable().Rows)
                {
                    DataRow drNew = dt.NewRow();
                    drNew["PointId"] = Convert.ToDecimal(itemRow["PointId"]);
                    if (itemRow["DateTime"] != DBNull.Value && itemRow["DateTime"].ToString() != "")
                    {
                        drNew["Datetime"] = Convert.ToDateTime(itemRow["DateTime"]).ToString("dd-MM-yyyy");
                    }
                    foreach (string factor in factors)
                    {
                        if (itemRow[factor] != DBNull.Value && itemRow[factor].ToString() != "")
                        {
                            drNew[factor] = Convert.ToDecimal(itemRow[factor]);
                        }
                    }
                    dt.Rows.Add(drNew);
                }
                if (dt.Columns.Contains("a21026"))
                    dt.Columns["a21026"].ColumnName = "SO2";
                if (dt.Columns.Contains("a21004"))
                    dt.Columns["a21004"].ColumnName = "NO2";
                if (dt.Columns.Contains("a21005"))
                    dt.Columns["a21005"].ColumnName = "CO";
                if (dt.Columns.Contains("a05024"))
                    dt.Columns["a05024"].ColumnName = "O3";
                if (dt.Columns.Contains("a34002"))
                    dt.Columns["a34002"].ColumnName = "PM10";
                if (dt.Columns.Contains("a34004"))
                    dt.Columns["a34004"].ColumnName = "PM2.5";
                if (dt.Columns.Contains("a90969"))
                    dt.Columns["a90969"].ColumnName = "Natural";
                if (dt.Columns.Contains("a21028"))
                    dt.Columns["a21028"].ColumnName = "H2S";
                if (dt.Columns.Contains("a21001"))
                    dt.Columns["a21001"].ColumnName = "NH3";
                return dt.DefaultView;
            }
            else
            {

                return null;
            }
        }
        /// <summary>
        /// 月日均值统计
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// </returns>
        public DataView GetAvgDayOfMonthNewData(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            EQIConcentrationService EQIConcentration = new EQIConcentrationService();
            DataTable dt = new DataTable();
            dt.Columns.Add("PointId", typeof(decimal));
            dt.Columns.Add("Datetime", typeof(string));
            foreach (string factor in factors)
            {
                dt.Columns.Add(factor, typeof(decimal));
            }
            if (factors.IsNotNullOrDBNull())
            {
                DataView dvDayReport = HourData.GetAvgDayNewData(portIds, factors, dtStart, dtEnd);  //全月均值，最大最小值(5个)
                foreach (DataRow itemRow in dvDayReport.ToTable().Rows)
                {
                    DataRow drNew = dt.NewRow();
                    drNew["PointId"] = Convert.ToDecimal(itemRow["PointId"]);
                    drNew["Datetime"] = "有效天";
                    foreach (string factor in factors)
                    {
                        if (itemRow[factor + "Count"] != DBNull.Value && itemRow[factor + "Count"].ToString() != "" && Convert.ToDecimal(itemRow[factor + "Count"]) != 0)
                        {
                            drNew[factor] = Convert.ToDecimal(itemRow[factor + "Count"]);
                        }
                    }

                    dt.Rows.Add(drNew);
                    DataRow newRows = dt.NewRow();
                    newRows["PointId"] = Convert.ToDecimal(itemRow["PointId"]);
                    newRows["Datetime"] = "月均值";
                    foreach (string factor in factors)
                    {
                        if (itemRow[factor + "Avg"] != DBNull.Value && itemRow[factor + "Avg"].ToString() != "")
                        {
                            newRows[factor] = Convert.ToDecimal(itemRow[factor + "Avg"]);
                        }
                    }
                    if (dt.Columns.Contains("a21026"))
                        dt.Columns["a21026"].ColumnName = "SO2";
                    if (dt.Columns.Contains("a21004"))
                        dt.Columns["a21004"].ColumnName = "NO2";
                    if (dt.Columns.Contains("a21005"))
                        dt.Columns["a21005"].ColumnName = "CO";
                    if (dt.Columns.Contains("a05024"))
                        dt.Columns["a05024"].ColumnName = "O3";
                    if (dt.Columns.Contains("a34002"))
                        dt.Columns["a34002"].ColumnName = "PM10";
                    if (dt.Columns.Contains("a34004"))
                        dt.Columns["a34004"].ColumnName = "PM2.5";
                    if (dt.Columns.Contains("a90969"))
                        dt.Columns["a90969"].ColumnName = "Natural";
                    if (dt.Columns.Contains("a21028"))
                        dt.Columns["a21028"].ColumnName = "H2S";
                    if (dt.Columns.Contains("a21001"))
                        dt.Columns["a21001"].ColumnName = "NH3";
                    dt.Rows.Add(newRows);
                }

                return dt.DefaultView;
            }
            else
            {
                return null;
            }
        }

        #region 获取数据
        /// <summary>
        /// 获取基准数据
        /// </summary>
        /// <param name="pointList">测点数据</param>
        /// <param name="DataType">因子数据</param>     
        /// <returns></returns>
        public DataView GetCheckDataDayAQI(string[] pointList, string DataType, string Orderby = "DataType,PointId,DateTime")
        {
            string strWhere = "1=1";
            if (pointList == null || pointList.Length == 0)
            {
                return null;
            }
            else
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(pointList.ToList<string>(), ",");
                strWhere += " and PointId in (" + portIdsStr + ")";
            }
            if (DataType != "")
            {
                strWhere += " and DataType='" + DataType + "'  ";
            }
            strWhere += " order by " + Orderby;
            return DayData.GetCheckDataDayAQI(strWhere);
        }
        /// <summary>
        /// 获取平均浓度
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factor">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="DataType">类型</param>
        /// <returns></returns>
        public DataView GetConcentrationDay(string[] portIds,  DateTime dtStart, DateTime dtEnd, string DataType)
        {
            return DayData.GetConcentrationDay(portIds,  dtStart, dtEnd, DataType);
        }
        /// <summary>
        /// 获取基准数据类型
        /// </summary>   
        /// <returns></returns>
        public DataTable GetCheckDataType()
        {
            return DayData.GetCheckDataType();
        }
        #endregion
        #region 获取区域数据
        /// <summary>
        /// 获取基准数据
        /// </summary>
        /// <param name="RegionUids">区域</param>
        /// <param name="DataType">因子数据</param>     
        /// <returns></returns>
        public DataView GetCheckRegionDayAQI(string[] RegionUids, string DataType, string Orderby = "DataType,RegionUid,DateTime")
        {
            string strWhere = "1=1";
            if (RegionUids == null || RegionUids.Length == 0)
            {
                return null;
            }
            else
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(RegionUids.ToList<string>(), ",");
                strWhere += " and RegionUid in (" + portIdsStr + ")";
            }
            if (DataType != "")
            {
                strWhere += " and DataType='" + DataType + "'  ";
            }
            strWhere += " order by " + Orderby;
            return DayData.GetCheckRegionDayAQI(strWhere);
        }
        /// <summary>
        /// 获取平均浓度
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factor">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="DataType">类型</param>
        /// <returns></returns>
        public DataView GetRegionConcentrationDay(string[] RegionUids,  DateTime dtStart, DateTime dtEnd, string DataType)
        {
            return DayData.GetRegionConcentrationDay(RegionUids, dtStart, dtEnd, DataType);
        }
        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        public DataView GetRegionYearBaseData(DateTime dateStart, DateTime dateEnd, string year)
        {
            return DayData.GetRegionYearBaseData(dateStart, dateEnd, year);
        }
        /// <summary>
        /// 季报
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        public DataView GetRegionSeasonBaseData(DateTime dateStart, DateTime dateEnd, string year)
        {
            return DayData.GetRegionSeasonBaseData(dateStart, dateEnd, year);
        }
        /// <summary>
        /// 获取基准数据类型
        /// </summary>   
        /// <returns></returns>
        public DataTable GetCheckRegionDataType()
        {
            return DayData.GetCheckRegionDataType();
        }
        #endregion
        #region 插入数据
        public void insertTable(DataTable dt, string DataType, string[] portIds)
        {
            DayData.insertTable(dt, DataType, portIds);
        }
        #endregion
        #region 时间段内因子比值数据
        /// <summary>
        /// 时间段内因子比值数据
        /// </summary>
        /// <param name="StartDate">开始日期（时）</param>
        /// <param name="EndDate">开始日期（时）</param>
        /// <param name="PointId">站点ID</param>
        /// <param name="FactorCodes">比对因子Code数组</param>
        /// <returns></returns>
        public DataTable GetHourAvgCompareData(DateTime StartDate, DateTime EndDate, Int32 PointId, string[] FactorCodes)
        {
            return HourData.GetHourAvgCompareData(StartDate, EndDate, PointId, FactorCodes);
        }
        #endregion

        #region 时间段内测点因子数据
        /// <summary>
        /// 时间段内测点因子数据
        /// </summary>
        /// <param name="StartDate">开始日期（时）</param>
        /// <param name="EndDate">截止日期（时）</param>
        /// <param name="PointId">站点ID</param>
        /// <param name="factorCodes">因子Code数组</param>
        /// <returns></returns>
        public DataTable GetHourDate(DateTime StartDate, DateTime EndDate, Int32 PointId, string[] factorCodes)
        {
            return HourData.GetHourDate(StartDate, EndDate, PointId, factorCodes);
        }
        #endregion

        #region 获取时间点下的测点，因子数据
        /// <summary>
        /// 获取时间点下的测点，因子数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DateLists">监测日期数组</param>
        /// <returns></returns>
        public DataTable GetDayData(List<int> PointIds, List<string> PollutantCodes, List<DateTime> DateLists)
        {
            return DayData.GetDayData(PointIds, PollutantCodes, DateLists);
        }
        #endregion
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayDataRegionPager(string[] portIds, IList<IPollutant> factors, DateTime dtBegion, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy="sortNumber desc,DateTime")
        {
          recordTotal = 0;
          if (factors.IsNotNullOrDBNull())
            return DayData.GetDayDataRegionPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
          return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayDataRegionPagers(string[] portIds, string[] factors, DateTime dtBegion, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "sortNumber desc,DateTime")
        {
          recordTotal = 0;
          if (factors.IsNotNullOrDBNull())
            return DayData.GetDayDataRegionPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
          return null;
        }
    }
}
