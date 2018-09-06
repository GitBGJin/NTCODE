using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.MonitoringBusinessRepository.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Utilities.Calendar;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.Core.Enums;
using System.Linq.Expressions;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.BaseInfoRepository.Alarm;

namespace SmartEP.Service.DataAnalyze.Water.DataQuery
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
        /// 地表水日数据
        /// </summary>
        DataQueryByHourService HourData = Singleton<DataQueryByHourService>.GetInstance();

        /// <summary>
        /// 地表水日数据
        /// </summary>
        DayReportRepository DayData = Singleton<DayReportRepository>.GetInstance();

        /// <summary>
        /// 地表水周数据
        /// </summray>
        WeekReportRepository WeekData = Singleton<WeekReportRepository>.GetInstance();

        /// <summary>
        /// 联系方式
        /// </summray>
        CreatAlarmRepository g_CreatAlarmRepository = Singleton<CreatAlarmRepository>.GetInstance();

        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = null;

        /// <summary>
        /// 因子
        /// </summary>
        SmartEP.Service.BaseData.Channel.WaterPollutantService m_WaterPollutantService = new SmartEP.Service.BaseData.Channel.WaterPollutantService();

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
        public DataView GetDayDataPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
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
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = dt = DayData.GetStatisticalData(portIds, factors, dateStart, dateEnd).Table;
                dt.Columns.Add("portName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
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
        public DataView GetDayStatisticalDataNew(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = dt = DayData.GetStatisticalDataNew(portIds, factors, dateStart, dateEnd).Table;
                dt.Columns.Add("portName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                }
                return dt.DefaultView;
            }
            return null;
        }
        /// <summary>
        /// 取得统计数据-周报（最大值、最小值、平均值、水质类别）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param> 
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <returns></returns>
        public DataView GetWeekStatisticalData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, int yearFrom, int weekOfYearFrom
            , int yearTo, int weekOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,WeekOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = DayData.GetStatisticalData(portIds, factors, dateStart, dateEnd).Table;
                DataTable dt2 = WeekData.GetDataPager(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo, pageSize, pageNo, out recordTotal, orderBy).ToTable();
                dt.Columns.Add("Grade", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        for (int k = 3; k < 3 + factors.Length; k++)
                        {
                            if (dt.Rows[i]["PointId"].ToString() == dt2.Rows[j]["PointId"].ToString() && dt.Rows[i]["PollutantCode"].ToString() == dt2.Columns[k].ColumnName)
                            {
                                dt.Rows[i]["Grade"] = dt2.Rows[j][k + 2 * factors.Length].ToString();
                            }
                        }
                    }
                }
                dt.Columns.Add("pointName", typeof(string));
                dt.Columns.Add("pollutantName", typeof(string));
                dt.Columns.Add("pollutantUnit", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["pointName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    dt.Rows[i]["pollutantName"] = m_WaterPollutantService.GetPollutantInfo(dt.Rows[i]["PollutantCode"].ToString()).PollutantName;
                    dt.Rows[i]["pollutantUnit"] = m_WaterPollutantService.GetPollutantInfo(dt.Rows[i]["PollutantCode"].ToString()).PollutantMeasureUnit;
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
        public DataView GetDayExportData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string LVTable, string orderBy = "PointId,DateTime")
        {
            if (DayData != null)
                return DayData.GetExportData(portIds, factors, dateStart, dateEnd, LVTable, orderBy);
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
        public DataView GetDayExportData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            if (DayData != null)
                return DayData.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
            return null;
        }

        public DataView GetDayExportData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, string LVTable, string orderBy = "PointId,DateTime")
        {
            if (DayData != null)
                return DayData.GetExportData(portIds, factors, dateStart, dateEnd, LVTable, orderBy);
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
        /// 枯水期日数据
        /// </summary>
        /// <param name="SiteTypeUid"></param>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDayHourDataReport_SZ(string SiteTypeUid, string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string Name, string auditor, string orderBy = "PointId,DateTime")
        {
            if (DayData != null)
            {
                string repeatStr = "";
                int repeqtIndex = 0;
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                ReportContentService g_ReportContentService = Singleton<ReportContentService>.GetInstance();
                DataView dtDetail = g_ReportContentService.getList(portIds, dateStart, dateEnd, "rainless", Name);
                portIds = g_MonitoringPointWater.RetrieveWaterMPList().Where(p => p.SiteTypeUid == SiteTypeUid && portIds.Contains(p.PointId.ToString())).Select(x => x.PointId.ToString()).ToArray();
                if (portIds.Length == 0 || portIds == null) portIds = ("-1").Split(';');
                var port = from s in portIds
                           orderby s ascending //或descending
                           select s;
                portIds = port.ToArray();
                //DataTable dt = DayData.GetExportNewDataReport_SZ(portIds, factors, dateStart, dateEnd, orderBy).ToTable();
                DataView dvAvg = HourData.GetHourStatisticalDataNew(portIds, factors.Select(t => t.PollutantCode).ToArray(), dateStart, dateEnd);
                DataView dvNumber = g_CreatAlarmRepository.GetNumberByName(Name);
                DataTable dt = new DataTable();
                dt.Columns.Add("PortCode1", typeof(string));
                dt.Columns.Add("PortCode2", typeof(string));
                dt.Columns.Add("PortName", typeof(string));
                dt.Columns.Add("PointId", typeof(string));
                dt.Columns.Add("Tstamp", typeof(string));
                foreach (string factorItem in factors.Select(t => t.PollutantCode).ToArray())
                {
                    dt.Columns.Add(factorItem, typeof(string));
                }
                dt.Columns.Add("EditUser", typeof(string));
                dt.Columns.Add("PhoneNum", typeof(string));
                dt.Columns.Add("AuditUser", typeof(string));
                dt.Columns.Add("Detail", typeof(string));
                if (!portIds.Contains("-1"))
                {
                    foreach (string pointid in portIds)
                    {
                        dvAvg.RowFilter = "PointId='" + pointid + "'";
                        if (dvAvg.Count > 0)
                        {
                            DataRow drNew = dt.NewRow();
                            int pointId = Convert.ToInt32(pointid);
                            if (!repeatStr.Contains(";" + pointId))
                            {
                                repeatStr += ";" + pointId;
                                repeqtIndex++;
                            }
                            dtDetail.RowFilter = "ptitle='" + pointid + "' and endtime='"
                                + new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day).ToString("yyyy-MM-dd") + "'";
                            string strDetail = "";
                            if (dtDetail.Count > 0)
                            {
                                strDetail = dtDetail[0]["pcontent"].ToString();
                            }
                            string strNumber = "";
                            if (dvNumber.Count > 0)
                            {
                                strNumber = dvNumber[0]["Number"].ToString();
                            }
                            if (strNumber == "")
                            {
                                dtDetail.RowFilter = "ptitle='" + Name + "' and endtime='"
                               + new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day).ToString("yyyy-MM-dd") + "'";
                                if (dtDetail.Count > 0)
                                {
                                    strNumber = dtDetail[0]["pcontent"].ToString();
                                }
                            }
                            drNew["Tstamp"] = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day).ToString("yyyy-MM-dd");
                            drNew["PointId"] = pointid;
                            drNew["PortCode1"] = "320500";
                            string PortName = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                            drNew["PortName"] = PointName(PortName);

                            if (SiteTypeUid == "81381370-1744-41fd-b5e4-4ffbee24288e")
                            {
                                drNew["PortCode2"] = "YYS" + (repeqtIndex < 10 ? "0" + repeqtIndex : repeqtIndex.ToString());
                            }
                            else
                            {
                                drNew["PortCode2"] = PointCode(PortName);
                            }

                            drNew["EditUser"] = Name;
                            drNew["PhoneNum"] = strNumber;
                            drNew["AuditUser"] = auditor;
                            drNew["Detail"] = strDetail;
                            foreach (string factorItem in factors.Select(t => t.PollutantCode).ToArray())
                            {
                                dvAvg.RowFilter = "PointId='" + pointid + "' and PollutantCode='" + factorItem + "'";
                                if (dvAvg.Count > 0)
                                {
                                    drNew[factorItem] = dvAvg[0]["Value_Avg"];
                                }
                            }
                            dt.Rows.Add(drNew);
                        }
                    }
                }
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                //    if (!repeatStr.Contains(";" + pointId))
                //    {
                //        repeatStr += ";" + pointId;
                //        repeqtIndex++;
                //    }
                //    dt.Rows[i]["PortCode1"] = "320500";
                //    dt.Rows[i]["PortCode2"] = SiteTypeUid == "81381370-1744-41fd-b5e4-4ffbee24288e" ? "YYS" + (repeqtIndex < 10 ? "0" + repeqtIndex : repeqtIndex.ToString()) : "FB" + repeqtIndex;
                //    dt.Rows[i]["PortName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                //    dt.Rows[i]["EditUser"] = Name;
                //    dt.Rows[i]["PhoneNum"] = "68338036";
                //    dt.Rows[i]["AuditUser"] = auditor;
                //    dt.Rows[i]["Detail"] = "";
                //}
                DataView dv = dt.DefaultView;
                dv.Sort = "PortCode2";
                return dv;
            }
            else
            {
                return new DataView(); ;
            }
        }
        public string PointName(string pontname)
        {
            if (pontname.Contains("漫山"))
            {
                return "漫山";
            }
            else if (pontname.Contains("胥湖心"))
            {
                return "胥湖心";
            }
            else if (pontname.Contains("漾西港"))
            {
                return "漾西港";
            }
            else if (pontname.Contains("泽山"))
            {
                return "泽山";
            }
            else if (pontname.Contains("小梅口"))
            {
                return "小梅口";
            }
            else if (pontname.Contains("新塘港"))
            {
                return "新塘港";
            }
            else if (pontname.Contains("西山西"))
            {
                return "西山西";
            }
            else if (pontname.Contains("14号灯标"))
            {
                return "14号灯标";
            }
            else
            {
                return pontname;
            }
        }
        /// <summary>
        /// 浮标站点位代码对应
        /// </summary>
        /// <param name="pontname">点位名称</param>
        /// <returns></returns>
        public string PointCode(string pontname)
        {
            if (pontname.Contains("漫山"))
            {
                return "FB1";
            }
            else if (pontname.Contains("胥湖心"))
            {
                return "FB2";
            }
            else if (pontname.Contains("漾西港"))
            {
                return "FB3";
            }
            else if (pontname.Contains("泽山"))
            {
                return "FB4";
            }
            else if (pontname.Contains("小梅口"))
            {
                return "FB5";
            }
            else if (pontname.Contains("新塘港"))
            {
                return "FB6";
            }
            else if (pontname.Contains("西山西"))
            {
                return "FB7";
            }
            else
            {
                return "FB8";
            }
        }
        /// <summary>
        /// 枯水期周数据
        /// </summary>
        /// <param name="SiteTypeUid"></param>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDayExportDataReport_SZ(string SiteTypeUid, string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string Name, string auditor, string orderBy = "PointId,DateTime")
        {
            if (DayData != null)
            {
                string repeatStr = "";
                int repeqtIndex = 0;
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                ReportContentService g_ReportContentService = Singleton<ReportContentService>.GetInstance();
                DataView dtDetail = g_ReportContentService.getList(portIds, dateStart, dateEnd, "rainless", Name);
                portIds = g_MonitoringPointWater.RetrieveWaterMPList().Where(p => p.SiteTypeUid == SiteTypeUid && portIds.Contains(p.PointId.ToString())).Select(x => x.PointId.ToString()).ToArray();
                if (portIds.Length == 0 || portIds == null) portIds = ("-1").Split(';');
                DataTable dt = DayData.GetExportNewDataReport_SZ(portIds, factors, dateStart, dateEnd, orderBy).ToTable();
                DataView dvNumber = g_CreatAlarmRepository.GetNumberByName(Name);
                dt.Columns.Add("PortCode1", typeof(string));
                dt.Columns.Add("PortCode2", typeof(string));
                dt.Columns.Add("PortName", typeof(string));
                dt.Columns.Add("EditUser", typeof(string));
                dt.Columns.Add("PhoneNum", typeof(string));
                dt.Columns.Add("AuditUser", typeof(string));
                dt.Columns.Add("Detail", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    DateTime dtend = DateTime.Parse(dt.Rows[i]["Tstamp"].ToString());
                    if (!repeatStr.Contains(";" + pointId))
                    {
                        repeatStr += ";" + pointId;
                        repeqtIndex++;
                    }
                    dtDetail.RowFilter = "ptitle='" + pointId + "' and endtime='"
                                + dtend.Date.ToString("yyyy-MM-dd") + "'";
                    string strDetail = "";
                    if (dtDetail.Count > 0)
                    {
                        strDetail = dtDetail[0]["pcontent"].ToString();
                    }
                    string strNumber = "";
                    if (dvNumber.Count > 0)
                    {
                        strNumber = dvNumber[0]["Number"].ToString();
                    }
                    if (strNumber == "")
                    {
                        dtDetail.RowFilter = "ptitle='" + Name + "' and endtime='"
                       + new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day).ToString("yyyy-MM-dd") + "'";
                        if (dtDetail.Count > 0)
                        {
                            strNumber = dtDetail[0]["pcontent"].ToString();
                        }
                    }
                    dt.Rows[i]["PortCode1"] = "320500";
                    string PortName = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    dt.Rows[i]["PortName"] = PointName(PortName);

                    if (SiteTypeUid == "81381370-1744-41fd-b5e4-4ffbee24288e")
                    {
                        dt.Rows[i]["PortCode2"] = "YYS" + (repeqtIndex < 10 ? "0" + repeqtIndex : repeqtIndex.ToString());
                    }
                    else
                    {
                        dt.Rows[i]["PortCode2"] = PointCode(PortName);
                    }
                    dt.Rows[i]["EditUser"] = Name;
                    dt.Rows[i]["PhoneNum"] = strNumber;
                    dt.Rows[i]["AuditUser"] = auditor;
                    dt.Rows[i]["Detail"] = strDetail;
                }
                return dt.DefaultView;
            }
            else
            {
                return new DataView(); ;
            }
        }


        /// <summary>
        /// 枯水期日数据
        /// </summary>
        /// <param name="SiteTypeUid"></param>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDayExportDataReport(string SiteTypeUid, string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            if (DayData != null)
            {
                string repeatStr = "";
                int repeqtIndex = 0;
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                portIds = g_MonitoringPointWater.RetrieveWaterMPList().Where(p => p.SiteTypeUid == SiteTypeUid && portIds.Contains(p.PointId.ToString())).Select(x => x.PointId.ToString()).ToArray();
                if (portIds.Length == 0 || portIds == null) portIds = ("-1").Split(';');
                DataTable dt = DayData.GetExportDataReport(portIds, factors, dateStart, dateEnd, orderBy).ToTable();
                dt.Columns.Add("PortCode1", typeof(string));
                dt.Columns.Add("PortCode2", typeof(string));
                dt.Columns.Add("PortName", typeof(string));
                dt.Columns.Add("EditUser", typeof(string));
                dt.Columns.Add("PhoneNum", typeof(string));
                dt.Columns.Add("AuditUser", typeof(string));
                dt.Columns.Add("Detail", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    if (!repeatStr.Contains(";" + pointId))
                    {
                        repeatStr += ";" + pointId;
                        repeqtIndex++;
                    }
                    dt.Rows[i]["PortCode1"] = "320500";
                    dt.Rows[i]["PortCode2"] = SiteTypeUid == "81381370-1744-41fd-b5e4-4ffbee24288e" ? "YYS" + (repeqtIndex < 10 ? "0" + repeqtIndex : repeqtIndex.ToString()) : "FB" + repeqtIndex;
                    dt.Rows[i]["PortName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    dt.Rows[i]["EditUser"] = "徐诗琴";
                    dt.Rows[i]["PhoneNum"] = "68338036";
                    dt.Rows[i]["AuditUser"] = "吕清";
                    dt.Rows[i]["Detail"] = "";
                }
                return dt.DefaultView;
            }
            else
            {
                return new DataView(); ;
            }
        }

        /// <summary>
        /// 水质周报日数据(如果一年的第一周周一恰好为1月1日，则该周数为1，如果不是，则第一周的数据中本年数据week_id为0，上一年数据的week_id为去年最后一周的周数)
        /// </summary>
        /// <param name="SiteTypeUid"></param>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDayExportDataWeekReport(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            if (DayData != null)
            {
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                portIds = g_MonitoringPointWater.RetrieveWaterMPList().Where(p => portIds.Contains(p.PointId.ToString())).Select(x => x.PointId.ToString()).ToArray();
                //数据源
                DataTable dt = DayData.GetExportDataReport(portIds, factors, dateStart, dateEnd, orderBy).ToTable();
                dt.Columns.Add("stcode", typeof(string));
                dt.Columns.Add("stname", typeof(string));
                dt.Columns.Add("rscode", typeof(string));
                dt.Columns.Add("rsname", typeof(string));
                dt.Columns.Add("ye", typeof(string));
                dt.Columns.Add("mon", typeof(string));
                dt.Columns.Add("day", typeof(string));
                dt.Columns.Add("week_id", typeof(string));
                dt.Columns.Add("week", typeof(string));
                dt.Columns.Add("OILS", typeof(string));
                dt.Columns.Add("PHEN", typeof(string));
                dt.Columns.Add("ECO", typeof(string));
                dt.Columns["week"].SetOrdinal(1);
                dt.Columns["week_id"].SetOrdinal(1);
                dt.Columns["day"].SetOrdinal(1);
                dt.Columns["mon"].SetOrdinal(1);
                dt.Columns["ye"].SetOrdinal(1);
                dt.Columns["rsname"].SetOrdinal(1);
                dt.Columns["rscode"].SetOrdinal(1);
                dt.Columns["stname"].SetOrdinal(1);
                dt.Columns["stcode"].SetOrdinal(1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    DateTime tstamp = Convert.ToDateTime(dt.Rows[i]["Tstamp"]);
                    DateTime firstDay = Convert.ToDateTime(Convert.ToDateTime(dt.Rows[i]["Tstamp"]).ToString("yyyy-01-01"));
                    dt.Rows[i]["stcode"] = "320700";
                    dt.Rows[i]["stname"] = "苏州市站";
                    dt.Rows[i]["rscode"] = "1";
                    dt.Rows[i]["rsname"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    dt.Rows[i]["ye"] = tstamp.Year;
                    dt.Rows[i]["mon"] = tstamp.Month;
                    dt.Rows[i]["day"] = tstamp.Day;
                    dt.Rows[i]["week_id"] = firstDay.DayOfWeek == DayOfWeek.Monday ? ChinaDate.WeekOfYear(tstamp) : ChinaDate.WeekOfYear(tstamp) - 1;
                    dt.Rows[i]["week"] = tstamp.DayOfWeek;
                    dt.Rows[i]["OILS"] = "-1";
                    dt.Rows[i]["PHEN"] = "0";
                    dt.Rows[i]["ECO"] = "-1";
                }
                return dt.DefaultView;
            }
            else
            {
                return new DataView(); ;
            }
        }

        /// <summary>
        /// 蓝藻日报表
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetBlueAlgaeDayReport(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd)
        {
            WaterQualityAnalysis waterAnalysis = new WaterQualityAnalysis();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            if (portIds != null)
            {
                DataTable dt = DayData.GetExportDataReport(portIds, factors, dtStart, dtEnd).ToTable();

                dt.Columns.Add("Region", typeof(string));
                dt.Columns.Add("Month", typeof(string));
                dt.Columns.Add("Day", typeof(string));
                dt.Columns.Add("PortName", typeof(string));
                dt.Columns.Add("Class", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    MonitoringPointEntity pointInfo = g_MonitoringPointWater.RetrieveEntityByPointId(pointId);
                    dt.Rows[i]["PortName"] = pointInfo.MonitoringPointName;
                    dt.Rows[i]["Region"] = g_MonitoringPointWater.RetrieveWaterExtensionPointListByPointUids(pointInfo.MonitoringPointUid.Split(';')).FirstOrDefault().WatersName;
                    dt.Rows[i]["Month"] = Convert.ToDateTime(dt.Rows[i]["Tstamp"]).Month;
                    dt.Rows[i]["Day"] = Convert.ToDateTime(dt.Rows[i]["Tstamp"]).Day;

                    #region 水质类别
                    string EvaluateFactorCodes = "";
                    string WQL = "";
                    WaterQualityService WaterQuality = new WaterQualityService();
                    Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
                    foreach (IPollutant fac in factors)
                    {
                        EvaluateFactorCodes = fac.PollutantCode + ";";
                        int digit = Convert.ToInt32(factors.Where(x => x.PollutantCode == fac.PollutantCode).Select(x => x.PollutantDecimalNum).FirstOrDefault());
                        decimal pollutantValue = dt.Rows[i][fac.PollutantCode] != DBNull.Value ? Convert.ToDecimal(dt.Rows[i][fac.PollutantCode]) : 99999;
                        WQL = WaterQuality.GetWQL(fac.PollutantCode, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                        if (!WQL.Equals(""))
                            WQIValues.Add(fac.PollutantCode, Convert.ToInt32(WQL));
                    }
                    if (!EvaluateFactorCodes.Equals(""))
                        EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);
                    dt.Rows[i]["Class"] = WaterQuality.GetWQL_Max(EQIReurnType.Class, EvaluateFactorCodes, WQIValues);
                    #endregion
                }
                return dt.DefaultView;
            }
            else
                return new DataView();
        }


        /// <summary>
        /// 获取区域水质状况
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetRegionDayAnalysisData(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd)
        {
            WaterQualityAnalysis waterAnalysis = new WaterQualityAnalysis();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            int record = 0;
            if (portIds != null)
            {
                //DataTable dt = DayData.GetDataPager(portIds, factors.Select(x=>x.PollutantCode).ToArray(), dtStart, dtEnd,10000,0,out record).ToTable();
                DataTable dt = DayData.GetRegionWQData(portIds, factors, dtStart, dtEnd).ToTable();
                dt.Columns.Add("PortName", typeof(string));
                string pointName = "当前流域";
                if (portIds.Length == 1)
                {
                    MonitoringPointEntity pointInfo = g_MonitoringPointWater.RetrieveEntityByPointId(Convert.ToInt32(portIds[0]));
                    pointName = pointInfo.MonitoringPointName;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["PortName"] = pointName;
                }
                return dt.DefaultView;
            }
            else
                return new DataView();
        }

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<WaterDayReportEntity> Retrieve(Expression<Func<WaterDayReportEntity, bool>> predicate)
        {
            return DayData.Retrieve(predicate);
        }

        /// <summary>
        /// 获取测点相关基础信息及相关因子日数据及等级
        /// </summary>
        /// <param name="PointIDs">测点数组</param>
        /// <param name="PollutantCodes">因子Code数组</param>
        /// <param name="Date">日期</param>
        /// <returns></returns>
        public DataTable GetWaterIEQI(List<int> PointIDs, List<string> PollutantCodes, DateTime Date)
        {
            return DayData.GetWaterIEQI(PointIDs, PollutantCodes, Date);
        }

        /// <summary>
        /// 获取自动点下的点位因子数据
        /// </summary>
        /// <param name="PointIds">站点ID数组</param>
        /// <param name="PollutantCodes">因子Code数组</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public DataTable GetWaterDayReport(List<int> PointIds, List<string> PollutantCodes, DateTime date)
        {
            return DayData.GetWaterDayReport(PointIds, PollutantCodes, date);
        }


        /// <summary>
        /// 水质自动监测月度小结，获取最大藻密度值与点位信息
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <param name="PointIds">监测点ID数组</param>
        /// <param name="PointIds">监测因子Code数组</param>
        /// <param name="SiteTypeUids">监测点类型Uid数组</param>
        /// <param name="isSiteType">是否监测点类型</param>
        /// <returns></returns>
        public DataTable GetMaxPollutantValue(DateTime datetime, List<int> PointIds, List<string> PollutantCodes, List<string> SiteTypeUids, bool isSiteType)
        {
            return DayData.GetMaxPollutantValue(datetime, PointIds, PollutantCodes, SiteTypeUids, isSiteType);
        }


        /// <summary>
        /// 取得无锡水质周报所需日数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataTable GetDataForWeekReport(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd)
        {
            return DayData.GetDataForWeekReport(portIds, factors, dtStart, dtEnd);
        }

        /// <summary>
        /// 取得无锡水质周报所需VOC日数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataTable GetVOCDataForWeekReport(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd)
        {
            return DayData.GetVOCDataForWeekReport(portIds, factors, dtStart, dtEnd);
        }

        /// <summary>
        /// 获取水源地测点日均监测值
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <returns></returns>
        public DataView GetJSGDayReport(DateTime beginTime, DateTime endTime, List<int> PointIds, List<string> PollutantCodes)
        {
            return DayData.GetJSGDayReport(beginTime, endTime, PointIds, PollutantCodes);
        }

        /// <summary>
        /// 获取水质现场调查相关监测数据
        /// </summary>
        /// <param name="PointIds">点位Id</param>
        /// <param name="Date">采集日期</param>
        /// <returns></returns>
        public DataView GetJSGSamplingData(DateTime BeginTime, DateTime EndTime, List<int> PointIds, List<string> PollutantCodes)
        {
            return DayData.GetJSGSamplingData(BeginTime, EndTime, PointIds, PollutantCodes);
        }

        #region 水质运行月报数据
        /// <summary>
        /// 获取水质运行月报数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataView GetRMStatisticalData(List<int> PointIds, DateTime BeginDate, DateTime EndDate)
        {
            return DayData.GetRMStatisticalData(PointIds, BeginDate, EndDate);
        }

        /// <summary>
        /// 获取水质运行月报数据（平均值）
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataView GetRMStatisticalDataAvg(List<int> PointIds, DateTime BeginDate, DateTime EndDate)
        {
            return DayData.GetRMStatisticalDataAvg(PointIds, BeginDate, EndDate);
        }

        /// <summary>
        /// 获取水质运行月报数据（日数据）
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataView GetRMDayData(List<int> PointIds, DateTime BeginDate, DateTime EndDate)
        {
            return DayData.GetRMDayData(PointIds, BeginDate, EndDate);
        }

        /// <summary>
        /// 获取水质类别或因子限值
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public DataView GetGradeOrLimit(List<int> PointIds, int Year, int Month)
        {
            return DayData.GetGradeOrLimit(PointIds, Year, Month);
        }

        #endregion

    }
}
