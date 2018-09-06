using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.DomainModel.BaseData;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Interfaces;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.DataAnalyze.Air.DataQuery;

namespace SmartEP.Service.DataAnalyze.Air.AQIReport
{
    public class MonthAQIService : IDayAQI
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;

        /// <summary>
        /// 点位日AQI
        /// </summary>
        DayAQIRepository pointDayAQI = null;

        /// <summary>
        /// 区域日AQI
        /// </summary>
        RegionDayAQIRepository regionDayAQI = null;

        /// <summary>
        /// 区域名称
        /// </summary>
        DictionaryService g_DictionaryService = new DictionaryService();
        //获取因子小数位

        /// <summary>
        /// 空气污染指数
        /// </summary>
        AQIService s_AQIService = new AQIService();
        DayAQIService dayAQI = new DayAQIService();

        #region 根据站点统计
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey">主键值</param>
        /// <returns></returns>
        public bool PIsExist(string strKey)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.IsExist(strKey);
            return false;
        }

        /// <summary>
        /// 获取各级别天数统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="portIds">测点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetPortGradeStatistics(IAQIType aqiType, string[] portIds, DateTime dtStart, DateTime dtEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetGradeStatistics(aqiType, portIds, dtStart, dtEnd);
            return null;
        }

        /// <summary>
        /// 获取所有天数
        /// </summary>
        /// <param name="Ponint">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public int GetPortAllDays(MonitoringPointEntity Point, DateTime dtStart, DateTime dtEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.Retrieve(p => p.PointId == Point.PointId && p.DateTime >= dtStart && p.DateTime <= dtEnd && !string.IsNullOrEmpty(p.AQIValue)).Count();
            return 0;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetPortDataPager(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            if (pointDayAQI != null)
                return pointDayAQI.GetDataPager(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetPortDataPagerDayAQI(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            if (pointDayAQI != null)
                return pointDayAQI.GetDataPagerDayAQI(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得导出数据（行转列数据）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetPortExportData(string[] portIds, DateTime dtStart, DateTime dtEnd, string orderBy = "DateTime,PointId")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetExportData(portIds, dtStart, dtEnd, orderBy);
            return null;
        }


        /// <summary>
        /// 获取根据单位转换浓度值后的新数据视图
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        private DataView GetNewViewByTurnData(DataView dv)
        {
            DataView dvNew = new DataView();
            DataTable dt = dv.ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                //mg/m3转成μg/m3的浓度值
                if (!string.IsNullOrWhiteSpace(dr["SO2"].ToString()))
                {
                    dr["SO2"] = (decimal.Parse(dr["SO2"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["NO2"].ToString()))
                {
                    dr["NO2"] = (decimal.Parse(dr["NO2"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["PM10"].ToString()))
                {
                    dr["PM10"] = (decimal.Parse(dr["PM10"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["CO"].ToString()))
                {
                    dr["CO"] = (decimal.Parse(dr["CO"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["PM25"].ToString()))
                {
                    dr["PM25"] = (decimal.Parse(dr["PM25"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["MaxOneHourO3"].ToString()))
                {
                    dr["MaxOneHourO3"] = (decimal.Parse(dr["MaxOneHourO3"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["Max8HourO3"].ToString()))
                {
                    dr["Max8HourO3"] = (decimal.Parse(dr["Max8HourO3"].ToString()) * 1000).ToString("G0");
                }
            }
            dvNew = dt.AsDataView();
            return dvNew;
        }

        /// <summary>
        /// 空气大月报月均值统计
        /// </summary>
        /// <param name="pointids"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataView GetMonthAvgData(string[] pointids, DateTime beginTime, DateTime endTime)
        {
            string[] factors = { "SO2", "NO2", "PM10", "PM25", "Max8HourO3", "CO" };
            string[] factorCode = { "a21026", "a21004", "a34002", "a34004", "a05024", "a21005" };
            DataTable dt = new DataTable();
            PortDayAQIDAL dayDal = new PortDayAQIDAL();
            DataView dvAvg = dayDal.GetAvgValue(pointids, beginTime, endTime);
            DataView dvAvgLastMonth = dayDal.GetAvgValue(pointids, beginTime.AddMonths(-1), endTime.AddMonths(-1));
            DataView dvAvgLastYear = dayDal.GetAvgValue(pointids, beginTime.AddYears(-1), endTime.AddYears(-1));

            DataView regionDV = dayAQI.GetMutilPointAQIData(pointids, beginTime, endTime);
            DataView regionDVLastMonth = dayAQI.GetMutilPointAQIData(pointids, beginTime.AddMonths(-1), endTime.AddMonths(-1));
            DataView regionDVLastYear = dayAQI.GetMutilPointAQIData(pointids, beginTime.AddYears(-1), endTime.AddYears(-1));

            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            int num = 0;


            dt.Columns.Add("DateID");
            dt.Columns.Add("FactorID");
            dt.Columns.Add("项目");

            foreach (string fac in factors)
            {
                string columnName = "";
                int DecimalNum = 3;
                AirPollutantService airService = new AirPollutantService();
                SmartEP.Core.Interfaces.IPollutant pollutant = airService.GetPollutantInfo(factorCode[num]);
                if (pollutant != null && pollutant.PollutantDecimalNum != null)
                    DecimalNum = Convert.ToInt32(pollutant.PollutantDecimalNum);

                if (fac.Equals("SO2")) columnName = "二氧化硫";
                else if (fac.Contains("NO2")) columnName = "二氧化氮";
                else if (fac.Contains("PM10")) columnName = "可吸入颗粒";
                else if (fac.Contains("PM25")) columnName = "细粒子";
                else if (fac.Contains("Max8HourO3")) columnName = "臭氧8小时";
                else if (fac.Contains("CO")) columnName = "一氧化碳";

                DataRow row = dt.NewRow();
                row["项目"] = columnName;
                row["DateID"] = 1;
                row["FactorID"] = num;

                DataRow rowLastMonth = dt.NewRow();
                rowLastMonth["项目"] = columnName;
                rowLastMonth["DateID"] = 2;
                rowLastMonth["FactorID"] = num;

                DataRow rowLastYear = dt.NewRow();
                rowLastYear["项目"] = columnName;
                rowLastYear["DateID"] = 3;
                rowLastYear["FactorID"] = num;
                #region 测点
                foreach (string pointid in pointids)
                {
                    string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointid)).MonitoringPointName;
                    if (num == 0)
                        dt.Columns.Add(pointName);

                    dvAvg.RowFilter = "";
                    dvAvg.RowFilter = "Pointid=" + pointid;
                    dvAvgLastMonth.RowFilter = "";
                    dvAvgLastMonth.RowFilter = "Pointid=" + pointid;
                    dvAvgLastYear.RowFilter = "";
                    dvAvgLastYear.RowFilter = "Pointid=" + pointid;

                    if (dvAvg.Count > 0)
                        row[pointName] = dvAvg[0][fac] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(dvAvg[0][fac]), DecimalNum).ToString() : "--";
                    else
                        row[pointName] = "--";

                    if (dvAvgLastMonth.Count > 0)
                        rowLastMonth[pointName] = dvAvgLastMonth[0][fac] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(dvAvgLastMonth[0][fac]), DecimalNum).ToString() : "--";
                    else
                        rowLastMonth[pointName] = "--";
                    if (dvAvgLastYear.Count > 0 && dvAvg.Count > 0)
                        rowLastYear[pointName] = dvAvgLastYear[0][fac] != DBNull.Value && dvAvg[0][fac] != DBNull.Value ? (DecimalExtension.GetRoundValue(Convert.ToDecimal(dvAvg[0][fac]), DecimalNum) - DecimalExtension.GetRoundValue(Convert.ToDecimal(dvAvgLastYear[0][fac]), DecimalNum)).ToString() : "";
                    else
                        rowLastYear[pointName] = "--";
                }
                #endregion

                #region 全市平均
                if (num == 0)
                    dt.Columns.Add("全市平均");

                if (regionDV.Count > 0)
                    row["全市平均"] = regionDV[0][fac] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(regionDV[0][fac]), DecimalNum).ToString() : "--";
                else
                    row["全市平均"] = "--";

                if (regionDVLastMonth.Count > 0)
                    rowLastMonth["全市平均"] = regionDVLastMonth[0][fac] != DBNull.Value ? DecimalExtension.GetRoundValue(Convert.ToDecimal(regionDVLastMonth[0][fac]), DecimalNum).ToString() : "--";
                else
                    rowLastMonth["全市平均"] = "--";
                if (dvAvgLastYear.Count > 0 && dvAvg.Count > 0)
                    rowLastYear["全市平均"] = regionDVLastYear[0][fac] != DBNull.Value && regionDV[0][fac] != DBNull.Value ? (DecimalExtension.GetRoundValue(Convert.ToDecimal(regionDV[0][fac]), DecimalNum) - DecimalExtension.GetRoundValue(Convert.ToDecimal(regionDVLastYear[0][fac]), DecimalNum)).ToString() : "";
                else
                    rowLastYear["全市平均"] = "--";
                #endregion
                dt.Rows.Add(row);
                dt.Rows.Add(rowLastMonth);
                dt.Rows.Add(rowLastYear);
                num++;
            }
            DataView dv = dt.DefaultView;
            dv.Sort = "DateID,FactorID asc";
            return dv;

        }

        #endregion
        #region 根据区域统计
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey">主键值</param>
        /// <returns></returns>
        public bool RIsExist(string strKey)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.IsExist(strKey);
            return false;
        }
        /// <summary>
        /// 获取各级别天数统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="portIds">测点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetAreaGradeStatistics(IAQIType aqiType, string[] regionGuids, DateTime dtStart, DateTime dtEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetGradeStatistics(aqiType, regionGuids, dtStart, dtEnd);
            return null;
        }

        /// <summary>
        /// 获取监测天数
        /// </summary>
        /// <param name="Ponint">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public int GetRegionMonitoringDays(string regionGuids, IAQIType AQIType, DateTime dtStart, DateTime dtEnd)
        {
            int MonitoringDays = 0;
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                switch (AQIType)
                {
                    case IAQIType.SO2_IAQI:
                        MonitoringDays = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuids && p.ReportDateTime >= dtStart && p.ReportDateTime <= dtEnd && !string.IsNullOrEmpty(p.SO2_IAQI)).Count();
                        break;
                    case IAQIType.NO2_IAQI:
                        MonitoringDays = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuids && p.ReportDateTime >= dtStart && p.ReportDateTime <= dtEnd && !string.IsNullOrEmpty(p.NO2_IAQI)).Count();
                        break;
                    case IAQIType.PM10_IAQI:
                        MonitoringDays = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuids && p.ReportDateTime >= dtStart && p.ReportDateTime <= dtEnd && !string.IsNullOrEmpty(p.PM10_IAQI)).Count();
                        break;
                    case IAQIType.PM25_IAQI:
                        MonitoringDays = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuids && p.ReportDateTime >= dtStart && p.ReportDateTime <= dtEnd && !string.IsNullOrEmpty(p.PM25_IAQI)).Count();
                        break;
                    case IAQIType.CO_IAQI:
                        MonitoringDays = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuids && p.ReportDateTime >= dtStart && p.ReportDateTime <= dtEnd && !string.IsNullOrEmpty(p.CO_IAQI)).Count();
                        break;
                    case IAQIType.Max8HourO3_IAQI:
                        MonitoringDays = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuids && p.ReportDateTime >= dtStart && p.ReportDateTime <= dtEnd && !string.IsNullOrEmpty(p.Max8HourO3_IAQI)).Count();
                        break;
                }
            return MonitoringDays;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataMonthAQI(DateTime dtStart, DateTime dtEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetDataMonthPager(dtStart, dtEnd);
            return null;
        }
        /// <summary>
        /// 获取空气质量日报
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetDataMonthPager(DateTime dtStart, DateTime dtEnd)
        {

            DateTime mBegion = Convert.ToDateTime(dtStart.ToString("yyyy-MM-01"));  //本月第一天
            DateTime mEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd"));   //本月当天
            DateTime smBegion = Convert.ToDateTime(dtStart.AddYears(-1).ToString("yyyy-MM-01"));   //去年本月第一天
            DateTime smEnd = Convert.ToDateTime(dtEnd.AddYears(-1).ToString("yyyy-MM-dd"));      //去年本月当天
            DateTime imBegion = Convert.ToDateTime(dtStart.ToString("yyyy-01-01"));   //本期第一天
            DateTime imEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd"));   //本期当天
            DateTime siBegion = Convert.ToDateTime(dtStart.AddYears(-1).ToString("yyyy-01-01"));  //去年本期第一天
            DateTime simEnd = Convert.ToDateTime(dtEnd.AddYears(-1).ToString("yyyy-MM-dd"));      //去年本期当天

            DataView dv = GetDataMonthAQI(mBegion, mEnd);  // 本月
            DataView dvT = GetDataMonthAQI(smBegion, smEnd);  //  上年本月
            DataView dvN = GetDataMonthAQI(imBegion, imEnd);   //  本期
            DataView dvNew = GetDataMonthAQI(siBegion, simEnd);   // 上年本期

            DataTable newdtb = new DataTable();
            newdtb.Columns.Add("Month1", typeof(string));
            newdtb.Columns.Add("MonthYear1", typeof(string));
            newdtb.Columns.Add("Month2", typeof(string));
            newdtb.Columns.Add("MonthYear2", typeof(string));
            newdtb.Columns.Add("Month3", typeof(string));
            newdtb.Columns.Add("MonthYear3", typeof(string));
            newdtb.Columns.Add("Month4", typeof(string));
            newdtb.Columns.Add("MonthYear4", typeof(string));
            newdtb.Columns.Add("Month5", typeof(string));
            newdtb.Columns.Add("MonthYear5", typeof(string));
            newdtb.Columns.Add("Month6", typeof(string));
            newdtb.Columns.Add("MonthYear6", typeof(string));
            newdtb.Columns.Add("Str", typeof(string));

            DataRow newRow = newdtb.NewRow();
            if (dv.Count > 0)
            {
                DataTable dt = dv.Table;
                decimal max = 0;
                for (int j = 0; j < 6; j++)
                {
                    string factorCode = dt.Columns[j].ColumnName;
                    int num = 24;
                    if (factorCode == "a05024")
                    {
                        num = 8;
                    }
                    if (dv[0][j] != DBNull.Value)
                    {
                        decimal value = DecimalExtension.GetRoundValue(Convert.ToDecimal(dv[0][j]), 3);
                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, value, num), 0);
                        if (max < temp)
                        {
                            max = temp;
                        }
                    }
                }
                newRow["Month1"] = (max != 0 ? max.ToString() : "--");
                newRow["Month2"] = (dv[0]["Good"] != DBNull.Value ? dv[0]["Good"].ToString() : "--");
                if (dv[0]["ValueCount"] != DBNull.Value && Convert.ToDecimal(dv[0]["ValueCount"]) != 0)
                {
                    if (dv[0]["Good"] != DBNull.Value)
                    {
                        newRow["Month3"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dv[0]["Good"]) / Convert.ToDecimal(dv[0]["ValueCount"]) * 100, 1).ToString();
                    }
                }
                else
                {
                    newRow["Month3"] = "--";
                }
            }
            else
            {
                newRow["Month1"] = "--";
                newRow["Month2"] = "--";
                newRow["Month3"] = "--";
            }
            if (dvT.Count > 0)
            {
                DataTable dt = dvT.Table;
                decimal max = 0;
                for (int j = 0; j < 6; j++)
                {
                    string factorCode = dt.Columns[j].ColumnName;
                    int num = 24;
                    if (factorCode == "a05024")
                    {
                        num = 8;
                    }
                    if (dvT[0][j] != DBNull.Value)
                    {
                        decimal value = DecimalExtension.GetRoundValue(Convert.ToDecimal(dvT[0][j]), 3);
                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, value, num), 0);
                        if (max < temp)
                        {
                            max = temp;
                        }
                    }
                }
                newRow["MonthYear1"] = (max != 0 ? max.ToString() : "--");
                newRow["MonthYear2"] = (dvT[0]["Good"] != DBNull.Value ? dvT[0]["Good"].ToString() : "--");
                if (dvT[0]["ValueCount"] != DBNull.Value && Convert.ToDecimal(dvT[0]["ValueCount"]) != 0)
                {
                    if (dvT[0]["Good"] != DBNull.Value)
                    {
                        newRow["MonthYear3"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dvT[0]["Good"]) / Convert.ToDecimal(dvT[0]["ValueCount"]) * 100, 1).ToString();
                    }
                }
                else
                {
                    newRow["MonthYear3"] = "--";
                }
            }
            else
            {
                newRow["MonthYear1"] = "--";
                newRow["MonthYear2"] = "--";
                newRow["MonthYear3"] = "--";
            }
            if (dvN.Count > 0)
            {
                DataTable dt = dvN.Table;
                decimal max = 0;
                for (int j = 0; j < 6; j++)
                {
                    string factorCode = dt.Columns[j].ColumnName;
                    int num = 24;
                    if (factorCode == "a05024")
                    {
                        num = 8;
                    }
                    if (dvN[0][j] != DBNull.Value)
                    {
                        decimal value = DecimalExtension.GetRoundValue(Convert.ToDecimal(dvN[0][j]), 3);
                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, value, num), 0);
                        if (max < temp)
                        {
                            max = temp;
                        }
                    }
                }
                newRow["Month4"] = (max != 0 ? max.ToString() : "--");
                newRow["Month5"] = (dvN[0]["Good"] != DBNull.Value ? dvN[0]["Good"].ToString() : "--");
                newRow["Str"] = (dvN[0]["Days"] != DBNull.Value ? dvN[0]["Days"].ToString() : "--");
                if (dvN[0]["ValueCount"] != DBNull.Value && Convert.ToDecimal(dvN[0]["ValueCount"]) != 0)
                {
                    if (dvN[0]["Good"] != DBNull.Value)
                    {
                        newRow["Month6"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dvN[0]["Good"]) / Convert.ToDecimal(dvN[0]["ValueCount"]) * 100, 1).ToString();
                    }
                }
                else
                {
                    newRow["Month6"] = "--";
                    newRow["Str"] = "--";
                }
            }
            else
            {
                newRow["Month4"] = "--";
                newRow["Month5"] = "--";
                newRow["Month6"] = "--";
            }
            if (dvNew.Count > 0)
            {
                DataTable dt = dvNew.Table;
                decimal max = 0;
                for (int j = 0; j < 6; j++)
                {
                    string factorCode = dt.Columns[j].ColumnName;
                    int num = 24;
                    if (factorCode == "a05024")
                    {
                        num = 8;
                    }
                    if (dvN[0][j] != DBNull.Value)
                    {
                        decimal value = DecimalExtension.GetRoundValue(Convert.ToDecimal(dvNew[0][j]), 3);
                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factorCode, value, num), 0);
                        if (max < temp)
                        {
                            max = temp;
                        }
                    }
                }
                newRow["MonthYear4"] = (max != 0 ? max.ToString() : "--");
                newRow["MonthYear5"] = (dvNew[0]["Good"] != DBNull.Value ? dvNew[0]["Good"].ToString() : "--");
                if (dvNew[0]["ValueCount"] != DBNull.Value && Convert.ToDecimal(dvNew[0]["ValueCount"]) != 0)
                {
                    if (dvNew[0]["Good"] != DBNull.Value)
                    {
                        newRow["MonthYear6"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(dvNew[0]["Good"]) / Convert.ToDecimal(dvNew[0]["ValueCount"]) * 100, 1).ToString();
                    }
                }
                else
                {
                    newRow["MonthYear6"] = "--";
                }
            }
            else
            {
                newRow["MonthYear4"] = "--";
                newRow["MonthYear5"] = "--";
                newRow["MonthYear6"] = "--";
            }
            newdtb.Rows.Add(newRow);
            return newdtb.DefaultView;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetAreaDataPager(string[] regionGuids, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "ReportDateTime,MonitoringRegionUid")
        {
            DateTime dtSta = dtStart.AddYears(-1);
            DateTime dtEn = dtEnd.AddYears(-1);
            DateTime dtStaF = Convert.ToDateTime(dtStart.ToString("2013-MM-dd"));
            DateTime dtEnT = Convert.ToDateTime(dtEnd.ToString("2013-MM-dd"));
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            recordTotal = 0;
            if (regionDayAQI != null)
            {
                DataView dv = new DataView();
                DataView dvN = new DataView();
                DataView dvNew = new DataView();
                dv = regionDayAQI.GetDataPager(regionGuids, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);   //2015
                dvN = regionDayAQI.GetDataPager(regionGuids, dtSta, dtEn, pageSize, pageNo, out recordTotal, orderBy);   //2014
                dvNew = regionDayAQI.GetDataPager(regionGuids, dtStaF, dtEnT, pageSize, pageNo, out recordTotal, orderBy);  //2013
                DataTable newdtb = new DataTable();
                newdtb.Columns.Add("regionName", typeof(string));
                newdtb.Columns.Add("2014_PM25", typeof(string));
                newdtb.Columns.Add("2015_PM25", typeof(string));
                newdtb.Columns.Add("2013_PM25", typeof(string));
                newdtb.Columns.Add("2014_Db", typeof(string));
                newdtb.Columns.Add("2015_Db", typeof(string));
                newdtb.Columns.Add("2013_Db", typeof(string));

                DataTable dt = dv.ToTable();   //2015
                DataTable dtN = dvN.ToTable();   //2014
                DataTable dtNew = dvNew.ToTable();  //2013
                DataRow[] Rowdt;
                DataRow[] RowdtN;
                DataRow[] RowdtNew;

                decimal sumA = 0;
                decimal sumB = 0;

                string regionGuid = "";
                string quanshi = "";
                for (int i = 0; i < regionGuids.Length; i++)
                {

                    if (regionGuids[i] == "全市均值")
                        quanshi = "全市均值";
                    else
                    {
                        string regionName = "";
                        decimal PM25 = 0;
                        decimal PM25T = 0;
                        decimal PM25N = 0;
                        decimal count = 0;
                        decimal countT = 0;
                        decimal countN = 0;
                        decimal countD = 0;
                        decimal countTD = 0;
                        decimal countND = 0;
                        decimal countDC = 0;
                        decimal countTC = 0;
                        decimal countNC = 0;
                        DataRow newRow = newdtb.NewRow();
                        regionName = g_DictionaryService.GetCodeDictionaryTextByValue(regionGuids[i]);
                        if (regionName == "苏州市区")
                        {
                            regionGuid += regionGuids[i] + ";";
                            regionName = "市区均值";
                        }
                        if (regionName == "张家港市")
                            regionGuid += regionGuids[i] + ";";
                        if (regionName == "常熟市")
                            regionGuid += regionGuids[i] + ";";
                        if (regionName == "太仓市")
                            regionGuid += regionGuids[i] + ";";
                        if (regionName == "昆山市")
                            regionGuid += regionGuids[i] + ";";
                        if (regionName == "吴江区")
                            regionGuid += regionGuids[i] + ";";

                        Rowdt = dt.Select("MonitoringRegionUid='" + regionGuids[i] + "'");   //2015
                        RowdtN = dtN.Select("MonitoringRegionUid='" + regionGuids[i] + "'");   //2014
                        RowdtNew = dtNew.Select("MonitoringRegionUid='" + regionGuids[i] + "'");   //2013

                        for (int j = 0; j < Rowdt.Length; j++)
                        {
                            if (Rowdt[j]["PM25"].IsNotNullOrDBNull())
                            {
                                PM25 += Convert.ToDecimal(Rowdt[j]["PM25"]);
                                count++;
                            }
                            if (Rowdt[j]["AQIValue"].IsNotNullOrDBNull())
                            {
                                if (Convert.ToInt32(Rowdt[j]["AQIValue"]) >= 0 && Convert.ToInt32(Rowdt[j]["AQIValue"]) <= 100)
                                    countD++;
                                else
                                    countDC++;
                            }
                        }
                        for (int m = 0; m < RowdtN.Length; m++)
                        {
                            if (RowdtN[m]["PM25"].IsNotNullOrDBNull())
                            {
                                PM25T += Convert.ToDecimal(RowdtN[m]["PM25"]);
                                countT++;
                            }
                            if (RowdtN[m]["AQIValue"].IsNotNullOrDBNull())
                            {
                                if (Convert.ToInt32(RowdtN[m]["AQIValue"]) >= 0 && Convert.ToInt32(RowdtN[m]["AQIValue"]) <= 100)
                                    countTD++;
                                else
                                    countTC++;
                            }
                        }
                        for (int n = 0; n < RowdtNew.Length; n++)
                        {
                            if (RowdtNew[n]["PM25"].IsNotNullOrDBNull())
                            {
                                PM25N += Convert.ToDecimal(RowdtNew[n]["PM25"]);
                                countN++;
                            }
                            if (RowdtNew[n]["AQIValue"].IsNotNullOrDBNull())
                            {
                                if (Convert.ToInt32(RowdtNew[n]["AQIValue"]) >= 0 && Convert.ToInt32(RowdtNew[n]["AQIValue"]) <= 100)
                                    countND++;
                                else
                                    countNC++;
                            }
                        }
                        newRow["regionName"] = regionName; ;

                        if (countN != 0)
                        {

                            decimal b = (PM25N * 1000) / countN;
                            sumB += b;
                            if (count != 0)
                            {
                                decimal a = (PM25 * 1000) / count;
                                newRow["2013_PM25"] = (((a - b) / b) * 100).ToString("0.0") + "%";
                            }
                            else
                            {
                                newRow["2013_PM25"] = "/";
                            }
                        }
                        else
                        {
                            newRow["2013_PM25"] = "/";
                        }

                        if (count != 0)
                        {
                            newRow["2015_PM25"] = ((PM25 * 1000) / count).ToString("0.0");
                            sumA += (PM25 * 1000) / count;
                            if ((countD + countDC) != 0)
                                newRow["2015_Db"] = ((countD / (countD + countDC)) * 100).ToString("0.0") + "%";
                            else
                                newRow["2015_Db"] = "/";
                        }
                        else
                        {
                            newRow["2015_PM25"] = "/";
                            newRow["2015_Db"] = "/";
                        }
                        if (countT != 0)
                        {
                            newRow["2014_PM25"] = ((PM25T * 1000) / countT).ToString("0.0");
                            if (countTD != 0)
                                newRow["2014_Db"] = (((countD - countTD) / countTD) * 100).ToString("0.0") + "%";
                            else
                                newRow["2014_Db"] = "/";
                        }
                        else
                        {
                            newRow["2014_PM25"] = "/";
                            newRow["2014_Db"] = "/";
                        }
                        if ((countND + countNC) != 0)
                        {
                            newRow["2013_Db"] = ((countND / (countND + countNC)) * 100).ToString("0.0") + "%";
                        }
                        else
                        {
                            newRow["2013_Db"] = "/";
                        }
                        newdtb.Rows.Add(newRow);
                    }
                }
                DataView newdv = new DataView();
                string[] regionIds = regionGuid.Trim(';').Split(';');
                if (quanshi == "全市均值")
                {
                    DataTable dtnew = regionDayAQI.GetMonthReportData(regionIds, dtStart, dtEnd).Table;  //2015
                    DataTable dtnewN = regionDayAQI.GetMonthReportData(regionIds, dtSta, dtEn).Table;  //2014
                    DataTable dtnewE = regionDayAQI.GetMonthReportData(regionIds, dtStaF, dtEnT).Table;  //2013
                    decimal sum3 = 0;
                    decimal sum4 = 0;
                    decimal sum5 = 0;
                    decimal count4 = 0;
                    decimal count5 = 0;
                    DataRow newRows = newdtb.NewRow();
                    newRows["regionName"] = quanshi;
                    for (int n = 0; n < newdtb.Rows.Count; n++)
                    {
                        if (newdtb.Rows[n]["regionName"].ToString() == "市区均值")
                        {
                            if (newdtb.Rows[n]["2014_PM25"].ToString() != "/")
                            {
                                sum4 += Convert.ToDecimal(newdtb.Rows[n]["2014_PM25"]);
                                count4++;
                            }
                            if (newdtb.Rows[n]["2015_PM25"].ToString() != "/")
                            {
                                sum5 += Convert.ToDecimal(newdtb.Rows[n]["2015_PM25"]);
                                count5++;
                            }
                        }
                        if (newdtb.Rows[n]["regionName"].ToString() == "张家港市")
                        {
                            if (newdtb.Rows[n]["2014_PM25"].ToString() != "/")
                            {
                                sum4 += Convert.ToDecimal(newdtb.Rows[n]["2014_PM25"]);
                                count4++;
                            }
                            if (newdtb.Rows[n]["2015_PM25"].ToString() != "/")
                            {
                                sum5 += Convert.ToDecimal(newdtb.Rows[n]["2015_PM25"]);
                                count5++;
                            }
                        }
                        if (newdtb.Rows[n]["regionName"].ToString() == "常熟市")
                        {
                            if (newdtb.Rows[n]["2014_PM25"].ToString() != "/")
                            {
                                sum4 += Convert.ToDecimal(newdtb.Rows[n]["2014_PM25"]);
                                count4++;
                            }
                            if (newdtb.Rows[n]["2015_PM25"].ToString() != "/")
                            {
                                sum5 += Convert.ToDecimal(newdtb.Rows[n]["2015_PM25"]);
                                count5++;
                            }
                        }
                        if (newdtb.Rows[n]["regionName"].ToString() == "太仓市")
                        {
                            if (newdtb.Rows[n]["2014_PM25"].ToString() != "/")
                            {
                                sum4 += Convert.ToDecimal(newdtb.Rows[n]["2014_PM25"]);
                                count4++;
                            }
                            if (newdtb.Rows[n]["2015_PM25"].ToString() != "/")
                            {
                                sum5 += Convert.ToDecimal(newdtb.Rows[n]["2015_PM25"]);
                                count5++;
                            }
                        }
                        if (newdtb.Rows[n]["regionName"].ToString() == "昆山市")
                        {
                            if (newdtb.Rows[n]["2014_PM25"].ToString() != "/")
                            {
                                sum4 += Convert.ToDecimal(newdtb.Rows[n]["2014_PM25"]);
                                count4++;
                            }
                            if (newdtb.Rows[n]["2015_PM25"].ToString() != "/")
                            {
                                sum5 += Convert.ToDecimal(newdtb.Rows[n]["2015_PM25"]);
                                count5++;
                            }
                        }
                        if (newdtb.Rows[n]["regionName"].ToString() == "吴江区")
                        {
                            if (newdtb.Rows[n]["2014_PM25"].ToString() != "/")
                            {
                                sum4 += Convert.ToDecimal(newdtb.Rows[n]["2014_PM25"]);
                                count4++;
                            }
                            if (newdtb.Rows[n]["2015_PM25"].ToString() != "/")
                            {
                                sum5 += Convert.ToDecimal(newdtb.Rows[n]["2015_PM25"]);
                                count5++;
                            }
                        }
                    }
                    Rowdt = dtnew.Select();   //2015
                    RowdtN = dtnewN.Select();   //2014
                    RowdtNew = dtnewE.Select();   //2013

                    if (count4 != 0)
                        newRows["2014_PM25"] = (sum4 / count4).ToString("0.0");
                    else
                        newRows["2014_PM25"] = "/";
                    if (count5 != 0)
                        newRows["2015_PM25"] = (sum5 / count5).ToString("0.0");
                    else
                        newRows["2015_PM25"] = "/";
                    if (sumB != 0)
                        newRows["2013_PM25"] = (((sumA - sumB) / sumB) * 100).ToString("0.0") + "%";
                    else
                        newRows["2013_PM25"] = "/";
                    if (RowdtN.Length > 0)
                    {
                        if (Convert.ToDecimal(RowdtN[0]["StandardCount"]) != 0)
                        {
                            decimal sub = 0;
                            if (Rowdt.Length > 0)
                                sub = Convert.ToDecimal(Rowdt[0]["StandardCount"]) - Convert.ToDecimal(RowdtN[0]["StandardCount"]);
                            else
                                sub = Convert.ToDecimal(RowdtN[0]["StandardCount"]);
                            newRows["2014_Db"] = (sub / Convert.ToDecimal(RowdtN[0]["StandardCount"]) * 100).ToString("0.0") + "%";

                        }
                        else
                            newRows["2014_Db"] = "/";
                    }
                    else
                        newRows["2014_Db"] = "/";
                    if (Rowdt.Length > 0)
                    {
                        if (Convert.ToDecimal(Rowdt[0]["ValidCount"]) != 0)
                            newRows["2015_Db"] = (Convert.ToDecimal(Rowdt[0]["StandardCount"]) / Convert.ToDecimal(Rowdt[0]["ValidCount"])).ToString("0.0") + "%";
                        else
                            newRows["2015_Db"] = "/";
                    }
                    else
                        newRows["2015_Db"] = "/";
                    if (RowdtNew.Length > 0)
                    {
                        if (Convert.ToDecimal(RowdtNew[0]["ValidCount"]) != 0)
                            newRows["2013_Db"] = (Convert.ToDecimal(RowdtNew[0]["StandardCount"]) / Convert.ToDecimal(RowdtNew[0]["ValidCount"])).ToString("0.0") + "%";
                        else
                            newRows["2013_Db"] = "/";
                    }
                    else
                        newRows["2013_Db"] = "/";
                    newdtb.Rows.Add(newRows);
                }
                newdv = new DataView(newdtb);
                return newdv;
            }
            return null;
        }


        /// <summary>
        /// 各市、区空气环境质量改善情况统计表
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="year"></param>
        /// <param name="factorName"></param>
        /// <param name="factorCode"></param>
        /// <returns></returns>
        public DataView GetData(DateTime beginTime, DateTime endTime, string year, string factorName, string factorCode)
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
                if (year != "" && year != null)
                {
                    dvBase = dal.GetVillageWeekRepSource(factorCode, num, baseBeginTime, baseEndTime);  //基数年
                }
                List<string> regionName = new List<string>();
                regionName.Add("张家港市"); regionName.Add("常熟市"); regionName.Add("太仓市"); regionName.Add("昆山市"); regionName.Add("吴江区");
                regionName.Add("吴中区"); regionName.Add("相城区"); regionName.Add("姑苏区"); regionName.Add("工业园区"); regionName.Add("高新区");
                foreach (string name in regionName)
                {
                    DataRow dr = SourceDT.NewRow();
                    dr["regionName"] = name;
                    dvBase.RowFilter = "regionName = '" + name + "'";
                    dvPer.RowFilter = "regionName = '" + name + "'";
                    dvCur.RowFilter = "regionName = '" + name + "'";
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
                return SourceDT.DefaultView;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        /// <summary>
        /// 各市、区空PM2.5浓度及比较统计表
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="year"></param>
        /// <param name="factorName"></param>
        /// <param name="factorCode"></param>
        /// <returns></returns>
        public DataView GetWeekData(DateTime beginTime, DateTime endTime, string year, string factorName, string factorCode)
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
                List<string> regionName = new List<string>();
                regionName.Add("张家港市"); regionName.Add("常熟市"); regionName.Add("太仓市"); regionName.Add("昆山市"); regionName.Add("吴江区");
                regionName.Add("吴中区"); regionName.Add("相城区"); regionName.Add("姑苏区"); regionName.Add("工业园区"); regionName.Add("高新区");
                foreach (string name in regionName)
                {
                    DataRow dr = SourceDT.NewRow();
                    dr["regionName"] = name;
                    dvBase.RowFilter = "regionName = '" + name + "'";
                    dvPer.RowFilter = "regionName = '" + name + "'";
                    dvCur.RowFilter = "regionName = '" + name + "'";
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
                return SourceDT.DefaultView;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        /// <summary>
        /// 取得导出数据（行转列数据）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetAreaExportData(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string orderBy = "ReportDateTime,MonitoringRegionUid")
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetExportData(regionGuids, dtStart, dtEnd, orderBy);
            return null;
        }

        /// <summary>
        /// 各污染物首要污染物统计
        /// </summary>
        /// <param name="aqiType">AQI分指标</param>
        /// <param name="portIds">测点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetAreaContaminantsStatistics(IAQIType aqiType, string[] regionGuids, DateTime dtStart, DateTime dtEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetContaminantsStatistics(aqiType, regionGuids, dtStart, dtEnd);

            return null;
        }

        /// <summary>
        /// 全市年数据统计
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// RegionName：区域
        /// FactorName：因子
        /// DayMinValue：最小值
        /// DayMaxValue：最大值
        /// OutDays：超标天数
        /// MonitorDays：监控天数
        /// OutRate：超标率
        /// OutBiggestFactor：最大超标倍数
        /// YearAverage：年均值
        /// </returns>
        public DataView GetAllYearData(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                EQIConcentrationService EQIConcentration = new EQIConcentrationService();
                EQIConcentrationLimitEntity entity = null;
                DataView MinValues = GetRegionsMinValue(regionGuids, dateStart, dateEnd);
                DataView MaxValues = GetRegionsMaxValue(regionGuids, dateStart, dateEnd);
                DataView ExceedingDatas = GetRegionsExceedingData(regionGuids, dateStart, dateEnd);
                DataView AvgValues = GetRegionsAvgValue(regionGuids, dateStart, dateEnd);
                DictionaryService dictionary = new DictionaryService();
                DataTable dt = new DataTable();
                dt.Columns.Add("RegionName", typeof(string));
                dt.Columns.Add("FactorName", typeof(string));
                dt.Columns.Add("DayMinValue", typeof(string));
                dt.Columns.Add("DayMaxValue", typeof(string));
                dt.Columns.Add("OutDays", typeof(string));
                dt.Columns.Add("MonitorDays", typeof(string));
                dt.Columns.Add("OutRate", typeof(string));
                dt.Columns.Add("OutBiggestFactor", typeof(string));
                dt.Columns.Add("YearAverage", typeof(string));

                List<IAQIType> AQITypes = new List<IAQIType>();
                AQITypes.Add(IAQIType.SO2_IAQI);
                AQITypes.Add(IAQIType.NO2_IAQI);
                AQITypes.Add(IAQIType.PM10_IAQI);
                AQITypes.Add(IAQIType.PM25_IAQI);
                AQITypes.Add(IAQIType.CO_IAQI);
                AQITypes.Add(IAQIType.Max8HourO3_IAQI);

                for (int i = 0; i < regionGuids.Length; i++)
                {
                    string RegionName = dictionary.GetTextByValue(Core.Enums.DictionaryType.Air, "行政区划", regionGuids[i]);
                    DataRow[] drMin = MinValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                    DataRow[] drMax = MaxValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                    DataRow[] drExceeding = ExceedingDatas.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                    DataRow[] drAvg = AvgValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");

                    if (drMin.Length > 0 && drMax.Length > 0 && drExceeding.Length > 0 && drAvg.Length > 0)
                    {
                        for (int j = 0; j < AQITypes.Count; j++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RegionName"] = RegionName;
                            switch (AQITypes[j])
                            {
                                case IAQIType.SO2_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.TwentyFour);
                                    dr["FactorName"] = "二氧化硫";
                                    dr["DayMinValue"] = drMin[0]["SO2"];
                                    dr["DayMaxValue"] = drMax[0]["SO2"];
                                    dr["OutDays"] = drExceeding[0]["SO2_Over"];
                                    dr["MonitorDays"] = GetRegionMonitoringDays(regionGuids[i], AQITypes[j], dateStart, dateEnd);
                                    decimal outSO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    dr["OutRate"] = Math.Round(outSO2, 2).ToString() + "%";
                                    decimal dSO2 = (decimal)(Convert.ToDecimal(drMax[0]["SO2"]) - entity.Upper);
                                    dr["OutBiggestFactor"] = Math.Round(dSO2, 2).ToString();
                                    dr["YearAverage"] = drAvg[0]["SO2"];
                                    break;
                                case IAQIType.NO2_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.TwentyFour);
                                    dr["FactorName"] = "二氧化氮";
                                    dr["DayMinValue"] = drMin[0]["NO2"];
                                    dr["DayMaxValue"] = drMax[0]["NO2"];
                                    dr["OutDays"] = drExceeding[0]["NO2_Over"];
                                    dr["MonitorDays"] = GetRegionMonitoringDays(regionGuids[i], AQITypes[j], dateStart, dateEnd);
                                    decimal outNO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    dr["OutRate"] = Math.Round(outNO2, 2).ToString() + "%";
                                    decimal dNO2 = (decimal)(Convert.ToDecimal(drMax[0]["NO2"]) - entity.Upper);
                                    dr["OutBiggestFactor"] = Math.Round(dNO2, 2).ToString();
                                    dr["YearAverage"] = drAvg[0]["NO2"];
                                    break;
                                case IAQIType.PM10_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.TwentyFour);
                                    dr["FactorName"] = "可吸入颗粒物";
                                    dr["DayMinValue"] = drMin[0]["PM10"];
                                    dr["DayMaxValue"] = drMax[0]["PM10"];
                                    dr["OutDays"] = drExceeding[0]["PM10_Over"];
                                    dr["MonitorDays"] = GetRegionMonitoringDays(regionGuids[i], AQITypes[j], dateStart, dateEnd);
                                    decimal outPM10 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    dr["OutRate"] = Math.Round(outPM10, 2).ToString() + "%";
                                    decimal dPM10 = (decimal)(Convert.ToDecimal(drMax[0]["PM10"]) - entity.Upper);
                                    dr["OutBiggestFactor"] = Math.Round(dPM10, 2).ToString();
                                    dr["YearAverage"] = drAvg[0]["PM10"];
                                    break;
                                case IAQIType.PM25_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.TwentyFour);
                                    dr["FactorName"] = "细颗粒物";
                                    dr["DayMinValue"] = drMin[0]["PM25"];
                                    dr["DayMaxValue"] = drMax[0]["PM25"];
                                    dr["OutDays"] = drExceeding[0]["PM25_Over"];
                                    dr["MonitorDays"] = GetRegionMonitoringDays(regionGuids[i], AQITypes[j], dateStart, dateEnd);
                                    decimal outPM25 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    dr["OutRate"] = Math.Round(outPM25, 2).ToString() + "%";
                                    dr["OutBiggestFactor"] = ((Convert.ToDecimal(drMax[0]["PM25"]) - entity.Upper) / entity.Upper).ToString();
                                    decimal dPM25 = (decimal)(Convert.ToDecimal(drMax[0]["PM25"]) - entity.Upper);
                                    dr["OutBiggestFactor"] = Math.Round(dPM25, 2).ToString();
                                    dr["YearAverage"] = drAvg[0]["PM25"];
                                    break;
                                case IAQIType.CO_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.TwentyFour);
                                    dr["FactorName"] = "一氧化碳";
                                    dr["DayMinValue"] = drMin[0]["CO"];
                                    dr["DayMaxValue"] = drMax[0]["CO"];
                                    dr["OutDays"] = drExceeding[0]["CO_Over"];
                                    dr["MonitorDays"] = GetRegionMonitoringDays(regionGuids[i], AQITypes[j], dateStart, dateEnd);
                                    decimal outCO = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    dr["OutRate"] = Math.Round(outCO, 2).ToString() + "%";
                                    dr["OutBiggestFactor"] = ((Convert.ToDecimal(drMax[0]["CO"]) - entity.Upper) / entity.Upper).ToString();
                                    decimal dCO = (decimal)(Convert.ToDecimal(drMax[0]["CO"]) - entity.Upper);
                                    dr["OutBiggestFactor"] = Math.Round(dCO, 2).ToString();
                                    dr["YearAverage"] = drAvg[0]["CO"];
                                    break;
                                case IAQIType.Max8HourO3_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Eight);
                                    dr["FactorName"] = "臭氧-8小时";
                                    dr["DayMinValue"] = drMin[0]["Max8HourO3"];
                                    dr["DayMaxValue"] = drMax[0]["Max8HourO3"];
                                    dr["OutDays"] = drExceeding[0]["Max8HourO3_Over"];
                                    dr["MonitorDays"] = GetRegionMonitoringDays(regionGuids[i], AQITypes[j], dateStart, dateEnd);
                                    decimal outMax8HourO3 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    dr["OutRate"] = Math.Round(outMax8HourO3, 2).ToString() + "%";
                                    decimal dMax8HourO3 = (decimal)(Convert.ToDecimal(drMax[0]["Max8HourO3"]) - entity.Upper);
                                    dr["OutBiggestFactor"] = Math.Round(dMax8HourO3, 2).ToString();
                                    dr["YearAverage"] = drAvg[0]["Max8HourO3"];
                                    break;
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取指定日期内日数据最大值数据
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：
        /// MonitoringRegionUid：区域ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetRegionsMaxValue(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetMaxValue(regionGuids, dateStart, dateEnd);
            return null;
        }

        /// <summary>
        /// 获取指定日期内日数据最小值数据
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：
        /// MonitoringRegionUid：区域ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetRegionsMinValue(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetMinValue(regionGuids, dateStart, dateEnd);
            return null;
        }

        /// <summary>
        /// 获取指定日期内日数据均值数据
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：
        /// MonitoringRegionUid：区域ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// </returns>
        public DataView GetRegionsAvgValue(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetAvgValue(regionGuids, dateStart, dateEnd);
            return null;
        }

        /// <summary>
        /// 区域日数据超标天数统计
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// MonitoringRegionUid：站点ID
        /// ValidCount：有效天
        /// OverCount：超标天
        /// PM25_Over：PM25超标天
        /// PM10_Over：PM10超标天
        /// NO2_Over：NO2超标天
        /// SO2_Over：SO2超标天
        /// CO_Over：CO超标天
        /// MaxOneHourO3_Over：MaxOneHourO3超标天
        /// Max8HourO3_Over：Max8HourO3超标天
        /// </returns>
        public DataView GetRegionsExceedingData(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            DateTime dtSta = dateStart.AddYears(-1);
            DateTime dtEn = dateEnd.AddYears(-1);
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
            {
                DataTable dt = new DataTable();   //2015
                DataTable dtNew = new DataTable();  //2014
                dt = regionDayAQI.GetExceedingData(regionGuids, dateStart, dateEnd).Table;  //2015
                dtNew = regionDayAQI.GetExceedingData(regionGuids, dtSta, dtEn).Table;   //2014

                DataTable newdt = new DataTable();  //2014
                newdt.Columns.Add("regionName", typeof(string));
                newdt.Columns.Add("ValidCount", typeof(string));
                newdt.Columns.Add("StanProportion", typeof(string));
                newdt.Columns.Add("LastYearProportion", typeof(string));
                newdt.Columns.Add("StandardCount", typeof(string));
                newdt.Columns.Add("PM25_Over", typeof(string));
                newdt.Columns.Add("PM10_Over", typeof(string));
                newdt.Columns.Add("NO2_Over", typeof(string));
                newdt.Columns.Add("SO2_Over", typeof(string));
                newdt.Columns.Add("CO_Over", typeof(string));
                newdt.Columns.Add("Max8HourO3_Over", typeof(string));
                string regionName = "";

                DataRow[] Rowdt;
                DataRow[] RowdtNew;

                string regionGuid = "";
                string quanshi = "";
                //string[] regionIds= "";
                for (int i = 0; i < regionGuids.Length; i++)
                {
                    if (regionGuids[i] == "全市均值")
                        quanshi = "全市均值";
                    else
                    {
                        DataRow newRow = newdt.NewRow();
                        regionName = g_DictionaryService.GetCodeDictionaryTextByValue(regionGuids[i]);

                        if (regionName == "苏州市区")
                        {
                            regionGuid += regionGuids[i] + ";";
                            regionName = "市区";
                        }
                        if (regionName == "张家港市")
                            regionGuid += regionGuids[i] + ";";
                        if (regionName == "常熟市")
                            regionGuid += regionGuids[i] + ";";
                        if (regionName == "太仓市")
                            regionGuid += regionGuids[i] + ";";
                        if (regionName == "昆山市")
                            regionGuid += regionGuids[i] + ";";
                        if (regionName == "吴江区")
                            regionGuid += regionGuids[i] + ";";


                        Rowdt = dt.Select("MonitoringRegionUid='" + regionGuids[i] + "'");   //2015
                        RowdtNew = dtNew.Select("MonitoringRegionUid='" + regionGuids[i] + "'");   //2014
                        if (Rowdt.Length > 0)
                        {
                            newRow["regionName"] = regionName;
                            newRow["LastYearProportion"] = "/";
                            newRow["ValidCount"] = Rowdt[0]["ValidCount"].ToString();
                            newRow["StandardCount"] = Rowdt[0]["StandardCount"].ToString();
                            newRow["PM25_Over"] = Rowdt[0]["PM25_Over"].ToString();
                            newRow["PM10_Over"] = Rowdt[0]["PM10_Over"].ToString();
                            newRow["NO2_Over"] = Rowdt[0]["NO2_Over"].ToString();
                            newRow["SO2_Over"] = Rowdt[0]["SO2_Over"].ToString();
                            newRow["CO_Over"] = Rowdt[0]["CO_Over"].ToString();
                            newRow["Max8HourO3_Over"] = Rowdt[0]["Max8HourO3_Over"].ToString();
                            if (Rowdt[0]["StandardCount"].IsNotNullOrDBNull())
                            {
                                newRow["StanProportion"] = ((Convert.ToDouble(Rowdt[0]["StandardCount"]) / Convert.ToDouble(Rowdt[0]["ValidCount"])) * 100).ToString("0.0") + "%";
                            }
                            else
                            {
                                newRow["StanProportion"] = "/";
                            }
                            if (RowdtNew.Length > 0)
                            {
                                if (RowdtNew[0]["StandardCount"].IsNotNullOrDBNull())
                                {
                                    newRow["LastYearProportion"] = ((Convert.ToDouble(RowdtNew[0]["StandardCount"]) / Convert.ToDouble(RowdtNew[0]["ValidCount"])) * 100).ToString("0.0") + "%";
                                }
                                else
                                {
                                    newRow["LastYearProportion"] = "/";
                                }
                            }
                        }
                        else
                        {
                            newRow["regionName"] = regionName;
                            newRow["LastYearProportion"] = "/";
                            newRow["ValidCount"] = "/";
                            newRow["StandardCount"] = "/";
                            newRow["PM25_Over"] = "/";
                            newRow["PM10_Over"] = "/";
                            newRow["NO2_Over"] = "/";
                            newRow["SO2_Over"] = "/";
                            newRow["CO_Over"] = "/";
                            newRow["Max8HourO3_Over"] = "/";
                            newRow["StanProportion"] = "/";
                            if (RowdtNew.Length > 0)
                            {
                                if (RowdtNew[0]["StandardCount"].IsNotNullOrDBNull())
                                {
                                    newRow["LastYearProportion"] = ((Convert.ToDouble(RowdtNew[0]["StandardCount"]) / Convert.ToDouble(RowdtNew[0]["ValidCount"])) * 100).ToString("0.0") + "%";
                                }
                                else
                                {
                                    newRow["LastYearProportion"] = "/";
                                }
                            }
                        }
                        newdt.Rows.Add(newRow);
                    }
                }
                DataView newdv = new DataView();
                string[] regionIds = regionGuid.Trim(';').Split(';');
                if (quanshi == "全市均值" && regionIds != null)
                {
                    DataTable dtNewt = new DataTable();  //2015
                    DataTable dtNewn = new DataTable();  //2014
                    dtNewt = regionDayAQI.GetMonthData(regionIds, dateStart, dateEnd).Table;  //2015
                    dtNewn = regionDayAQI.GetMonthData(regionIds, dtSta, dtEn).Table;   //2014

                    Rowdt = dtNewt.Select();   //2015
                    RowdtNew = dtNewn.Select();   //2014

                    DataRow newRows = newdt.NewRow();
                    if (dtNewt.Rows.Count > 0)
                    {
                        newRows["regionName"] = quanshi;
                        newRows["LastYearProportion"] = "/";
                        newRows["ValidCount"] = Rowdt[0]["ValidCount"].ToString();
                        newRows["StandardCount"] = Rowdt[0]["StandardCount"].ToString();
                        newRows["PM25_Over"] = Rowdt[0]["PM25_Over"].ToString();
                        newRows["PM10_Over"] = Rowdt[0]["PM10_Over"].ToString();
                        newRows["NO2_Over"] = Rowdt[0]["NO2_Over"].ToString();
                        newRows["SO2_Over"] = Rowdt[0]["SO2_Over"].ToString();
                        newRows["CO_Over"] = Rowdt[0]["CO_Over"].ToString();
                        newRows["Max8HourO3_Over"] = Rowdt[0]["Max8HourO3_Over"].ToString();
                        if (Rowdt[0]["StandardCount"].IsNotNullOrDBNull())
                        {
                            newRows["StanProportion"] = ((Convert.ToDouble(Rowdt[0]["StandardCount"]) / Convert.ToDouble(Rowdt[0]["ValidCount"])) * 100).ToString("0.0") + "%";
                        }
                        else
                        {
                            newRows["StanProportion"] = "/";
                        }
                        if (dtNewn.Rows.Count > 0)
                        {
                            if (RowdtNew[0]["StandardCount"].IsNotNullOrDBNull())
                            {
                                newRows["LastYearProportion"] = ((Convert.ToDouble(RowdtNew[0]["StandardCount"]) / Convert.ToDouble(RowdtNew[0]["ValidCount"])) * 100).ToString("0.0") + "%";
                            }
                            else
                            {
                                newRows["LastYearProportion"] = "/";
                            }
                        }
                    }
                    else
                    {
                        newRows["regionName"] = quanshi;
                        newRows["LastYearProportion"] = "/";
                        newRows["ValidCount"] = "/";
                        newRows["StandardCount"] = "/";
                        newRows["PM25_Over"] = "/";
                        newRows["PM10_Over"] = "/";
                        newRows["NO2_Over"] = "/";
                        newRows["SO2_Over"] = "/";
                        newRows["CO_Over"] = "/";
                        newRows["Max8HourO3_Over"] = "/";
                        newRows["StanProportion"] = "/";
                        if (dtNewn.Rows.Count > 0)
                        {
                            if (RowdtNew[0]["StandardCount"].IsNotNullOrDBNull())
                            {
                                newRows["LastYearProportion"] = ((Convert.ToDouble(RowdtNew[0]["StandardCount"]) / Convert.ToDouble(RowdtNew[0]["ValidCount"])) * 100).ToString("0.0") + "%";
                            }
                            else
                            {
                                newRows["LastYearProportion"] = "/";
                            }
                        }
                    }
                    newdt.Rows.Add(newRows);
                }
                newdv = new DataView(newdt);
                return newdv;
            }
            return null;
        }

        /// <summary>
        /// 获取主要污染物年均值
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// RegionName：区域名称
        /// Year：年份
        /// PM25Concentration：PM2.5浓度
        /// PM10Concentration：PM10浓度
        /// NO2Concentration：二氧化氮浓度
        /// SO2Concentration：二氧化硫浓度
        /// COConcentration：一氧化碳浓度
        /// Max8HourO3Concentration：最大8小时臭氧浓度
        /// PM25LimitValue：PM2.5年均浓度限值
        /// PM10LimitValue：PM10年均浓度限值
        /// NO2LimitValue：二氧化氮年均浓度限值
        /// SO2LimitValue：二氧化硫年均浓度限值
        /// COLimitValue：一氧化碳年均浓度限值
        /// Max8HourO3LimitValue：最大8小时臭氧年均浓度限值
        /// </returns>
        public DataView GetPrimaryPollutantAvgYearData(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            EQIConcentrationService EQIConcentration = new EQIConcentrationService();
            DataView AvgValues = GetRegionsAvgValue(regionGuids, dateStart, dateEnd);
            DictionaryService dictionary = new DictionaryService();
            DataTable dt = new DataTable();
            dt.Columns.Add("RegionName", typeof(string));
            dt.Columns.Add("Year", typeof(string));
            dt.Columns.Add("PM25Concentration", typeof(string));
            dt.Columns.Add("PM10Concentration", typeof(string));
            dt.Columns.Add("NO2Concentration", typeof(string));
            dt.Columns.Add("SO2Concentration", typeof(string));
            dt.Columns.Add("COConcentration", typeof(string));
            dt.Columns.Add("Max8HourO3Concentration", typeof(string));
            dt.Columns.Add("PM25LimitValue", typeof(string));
            dt.Columns.Add("PM10LimitValue", typeof(string));
            dt.Columns.Add("NO2LimitValue", typeof(string));
            dt.Columns.Add("SO2LimitValue", typeof(string));
            dt.Columns.Add("COLimitValue", typeof(string));
            dt.Columns.Add("Max8HourO3LimitValue", typeof(string));

            DateTime baseStart = Convert.ToDateTime("2013-1-1");
            DateTime baseEnd = Convert.ToDateTime("2013-12-31");
            DataView baseAvgValues = GetRegionsAvgValue(regionGuids, baseStart, baseEnd);
            DateTime[,] Date = new DateTime[,] { { dateStart, dateEnd }, { baseStart, baseEnd } };

            for (int i = 0; i < regionGuids.Length; i++)
            {
                string RegionName = dictionary.GetTextByValue(Core.Enums.DictionaryType.Air, "行政区划", regionGuids[i]);
                DataRow[] drAvg = AvgValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                DataRow[] basedrAvg = baseAvgValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");

                for (int j = 0; j < Date.GetLength(0); j++)
                {
                    if (drAvg.Length > 0 && basedrAvg.Length > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["RegionName"] = RegionName;
                        dr["Year"] = Date[j, 0].Year;
                        if (Date[j, 0].Year.Equals(2013))
                        {
                            dr["PM25Concentration"] = basedrAvg[0]["PM25"];
                            dr["PM10Concentration"] = basedrAvg[0]["PM10"];
                            dr["NO2Concentration"] = basedrAvg[0]["NO2"];
                            dr["SO2Concentration"] = basedrAvg[0]["SO2"];
                            dr["COConcentration"] = basedrAvg[0]["CO"];
                            dr["Max8HourO3Concentration"] = basedrAvg[0]["Max8HourO3"];
                        }
                        else
                        {
                            dr["PM25Concentration"] = drAvg[0]["PM25"];
                            dr["PM10Concentration"] = drAvg[0]["PM10"];
                            dr["NO2Concentration"] = drAvg[0]["NO2"];
                            dr["SO2Concentration"] = drAvg[0]["SO2"];
                            dr["COConcentration"] = drAvg[0]["CO"];
                            dr["Max8HourO3Concentration"] = drAvg[0]["Max8HourO3"];
                        }
                        dr["PM25LimitValue"] = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.TwentyFour).Upper.ToString();
                        dr["PM10LimitValue"] = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.TwentyFour).Upper.ToString();
                        dr["NO2LimitValue"] = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.TwentyFour).Upper.ToString();
                        dr["SO2LimitValue"] = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.TwentyFour).Upper.ToString();
                        dr["COLimitValue"] = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.TwentyFour).Upper.ToString();
                        dr["Max8HourO3LimitValue"] = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Eight).Upper.ToString();

                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt.DefaultView;
        }


        /// <summary>
        /// 主要污染物月均值
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// RegionName：区域名称
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// 8HourO3：8HourO3浓度
        /// </returns>
        public DataView GetRegionPollutantMonthAvg(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            DictionaryService dictionary = new DictionaryService();
            DataView dvAvgValues = GetRegionsAvgValue(regionGuids, dateStart, dateEnd);
            DataTable dt = new DataTable();
            dt.Columns.Add("RegionName", typeof(string));
            dt.Columns.Add("PM25", typeof(decimal));
            dt.Columns.Add("PM10", typeof(decimal));
            dt.Columns.Add("NO2", typeof(decimal));
            dt.Columns.Add("SO2", typeof(decimal));
            dt.Columns.Add("CO", typeof(decimal));
            dt.Columns.Add("8HourO3", typeof(decimal));
            decimal p5sum = 0;
            decimal p1sum = 0;
            decimal nsum = 0;
            decimal ssum = 0;
            decimal csum = 0;
            decimal hsum = 0;
            decimal p5count = 0;
            decimal p1count = 0;
            decimal ncount = 0;
            decimal scount = 0;
            decimal ccount = 0;
            decimal hcount = 0;
            for (int i = 0; i < regionGuids.Length; i++)
            {
                if (regionGuids[i] != "全市均值")
                {
                    string RegionName = g_DictionaryService.GetCodeDictionaryTextByValue(regionGuids[i]);
                    if (RegionName == "苏州市区")
                    {
                        RegionName = "市区";
                    }
                    DataRow[] drAvgValues = dvAvgValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                    if (drAvgValues.Length > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["RegionName"] = RegionName;
                        if (!string.IsNullOrEmpty(drAvgValues[0]["PM25"].ToString()))
                        {
                            dr["PM25"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["PM25"]) * 1000, 1);
                            p5sum += DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["PM25"]) * 1000, 1);
                            p5count++;
                        }
                        else
                            dr["PM25"] = "/";
                        if (!string.IsNullOrEmpty(drAvgValues[0]["PM10"].ToString()))
                        {
                            dr["PM10"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["PM10"]) * 1000, 0);
                            p1sum += DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["PM10"]) * 1000, 0);
                            p1count++;
                        }
                        else
                            dr["PM10"] = "/";
                        if (!string.IsNullOrEmpty(drAvgValues[0]["NO2"].ToString()))
                        {
                            dr["NO2"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["NO2"]) * 1000, 0);
                            nsum += DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["NO2"]) * 1000, 0);
                            ncount++;
                        }
                        else
                            dr["NO2"] = "/";
                        if (!string.IsNullOrEmpty(drAvgValues[0]["SO2"].ToString()))
                        {
                            dr["SO2"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["SO2"]) * 1000, 0);
                            ssum += DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["SO2"]) * 1000, 0);
                            scount++;
                        }
                        else
                            dr["SO2"] = "/";
                        if (!string.IsNullOrEmpty(drAvgValues[0]["CO"].ToString()))
                        {
                            dr["CO"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["CO"]), 2);
                            csum += DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["CO"]), 2);
                            ccount++;
                        }
                        else
                            dr["CO"] = "/";
                        if (!string.IsNullOrEmpty(drAvgValues[0]["Max8HourO3"].ToString()))
                        {
                            dr["8HourO3"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["Max8HourO3"]) * 1000, 0);
                            hsum += DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["Max8HourO3"]) * 1000, 0);
                            hcount++;
                        }
                        else
                            dr["8HourO3"] = "/";
                        dt.Rows.Add(dr);
                    }
                }
            }
            DataView newdv = new DataView();
            for (int j = 0; j < regionGuids.Length; j++)
            {
                if (regionGuids[j] == "全市均值")
                {
                    DataRow newRows = dt.NewRow();

                    newRows["RegionName"] = "全市均值";
                    if (p5count != 0)
                        newRows["PM25"] = DecimalExtension.GetRoundValue(p5sum / p5count, 1);
                    else
                        newRows["PM25"] = "/";
                    if (p1count != 0)
                        newRows["PM10"] = DecimalExtension.GetRoundValue(p1sum / p1count, 0);
                    else
                        newRows["PM10"] = "/";
                    if (ncount != 0)
                        newRows["NO2"] = DecimalExtension.GetRoundValue(nsum / ncount, 0);
                    else
                        newRows["NO2"] = "/";
                    if (scount != 0)
                        newRows["SO2"] = DecimalExtension.GetRoundValue(ssum / scount, 0);
                    else
                        newRows["SO2"] = "/";
                    if (ccount != 0)
                        newRows["CO"] = DecimalExtension.GetRoundValue(csum / ccount, 2);
                    else
                        newRows["CO"] = "/";
                    if (hcount != 0)
                        newRows["8HourO3"] = DecimalExtension.GetRoundValue(hsum / hcount, 0);
                    else
                        newRows["8HourO3"] = "/";
                    dt.Rows.Add(newRows);

                }
                newdv = new DataView(dt);
            }
            return newdv;
        }

        /// <summary>
        /// 空气质量等级分布
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView RealTimeGradeDistribution(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            DictionaryService dictionary = new DictionaryService();

            DataTable dt = new DataTable();
            dt.Columns.Add("RegionName", typeof(string));
            dt.Columns.Add("Great", typeof(int));
            dt.Columns.Add("Good", typeof(int));
            dt.Columns.Add("LightPollution", typeof(int));
            dt.Columns.Add("MiddlePollution", typeof(int));
            dt.Columns.Add("HighPollution", typeof(int));
            dt.Columns.Add("SeriousPollution", typeof(int));

            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();

            for (int i = 0; i < regionGuids.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr["RegionName"] = dictionary.GetTextByValue(Core.Enums.DictionaryType.Air, "行政区划", regionGuids[i]);
                List<MonitoringPointEntity> monitoringPointEntity = g_MonitoringPointAir.RetrieveAirMPListByRegion(regionGuids[i]).ToList();
                if (monitoringPointEntity.Count > 0)
                {
                    string[] pointIds = monitoringPointEntity.Select(p => p.PointId.ToString()).ToArray();
                    DataTable dv = pointDayAQI.GetLastData(pointIds, dateStart, dateEnd).Table;

                    dr["Great"] = dv.Select("convert(AQIValue,'System.Int32')>=0 and convert(AQIValue,'System.Int32')<=50").Count();
                    dr["Good"] = dv.Select("convert(AQIValue,'System.Int32')>=51 and convert(AQIValue,'System.Int32')<=100").Count();
                    dr["LightPollution"] = dv.Select("convert(AQIValue,'System.Int32')>=101 and convert(AQIValue,'System.Int32')<=150").Count();
                    dr["MiddlePollution"] = dv.Select("convert(AQIValue,'System.Int32')>=151 and convert(AQIValue,'System.Int32')<=200").Count();
                    dr["HighPollution"] = dv.Select("convert(AQIValue,'System.Int32')>=201 and convert(AQIValue,'System.Int32')<=300").Count();
                    dr["SeriousPollution"] = dv.Select("convert(AQIValue,'System.Int32')>=301").Count();

                    dt.Rows.Add(dr);
                }
            }
            return dt.AsDataView();
        }

        /// <summary>
        /// 区域污染持续天数及污染程度简表
        /// </summary>
        /// <param name="portsId">站点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：DateTime，ContinuousDays，LightPollution，ModeratePollution，HighPollution，SeriousPollution
        /// </returns>
        public DataView GetRegionsContinuousDaysTable(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DateTime", typeof(string));
            dt.Columns.Add("ContinuousDays", typeof(int));
            dt.Columns.Add("LightPollution", typeof(int));
            dt.Columns.Add("ModeratePollution", typeof(int));
            dt.Columns.Add("HighPollution", typeof(int));
            dt.Columns.Add("SeriousPollution", typeof(int));
            int record = 0;
            DataTable AllData = GetAreaDataPager(regionGuids, dateStart, dateEnd, 999999999, 0, out record, "ReportDateTime Asc").Table;
            DataTable NewExceedingDays = AllData.Clone();
            DataRow[] AllExceedingDays = AllData.Select("convert(AQIValue,'System.Int32')>100");

            if (AllExceedingDays.Length > 0)
            {
                NewExceedingDays = AllExceedingDays.CopyToDataTable();
                DataView dv = NewExceedingDays.DefaultView;
                dv.Sort = "DateTime asc";
                List<List<DateTime>> ContinuousDaysList = new List<List<DateTime>>();
                List<DateTime> ContinuousDays = new List<DateTime>();

                for (int i = 1; i < NewExceedingDays.Rows.Count; i++)
                {
                    DateTime firstValue = Convert.ToDateTime(dv[i - 1]["DateTime"]);
                    DateTime secondValue = Convert.ToDateTime(dv[i]["DateTime"]);
                    int poor = (secondValue - firstValue).Days;
                    if (poor.Equals(1))
                    {
                        if (!ContinuousDays.Contains(firstValue))
                        {
                            ContinuousDays.Add(firstValue);
                        }
                        if (!ContinuousDays.Contains(secondValue))
                        {
                            ContinuousDays.Add(secondValue);
                        }
                        if (i == dv.Count - 1)
                        {
                            ContinuousDaysList.Add(ContinuousDays);
                        }
                    }
                    else
                    {
                        if (ContinuousDays.Count > 0)
                        {
                            ContinuousDaysList.Add(ContinuousDays);
                        }
                        ContinuousDays = new List<DateTime>();
                    }
                }

                for (int i = 0; i < ContinuousDaysList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    List<DateTime> dateTimeArray = ContinuousDaysList[i];
                    DataTable ExceedingDays = NewExceedingDays.Clone();
                    ExceedingDays = NewExceedingDays.Select("DateTime>=" + dateTimeArray[0] + " and ReportDateTime<=" + dateTimeArray[dateTimeArray.Count - 1]).CopyToDataTable();
                    DataRow[] LightPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=101 and convert(AQIValue,'System.Int32')<=150");
                    DataRow[] ModeratePollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=151 and convert(AQIValue,'System.Int32')<=200");
                    DataRow[] HighPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=201 and convert(AQIValue,'System.Int32')<=300");
                    DataRow[] SeriousPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>300");

                    int outDays = dateTimeArray.Count;
                    int LightPollutionDays = LightPollution.Length;
                    int ModeratePollutionDays = ModeratePollution.Length;
                    int HighPollutionDays = HighPollution.Length;
                    int SeriousPollutionDays = SeriousPollution.Length;

                    dr["DateTime"] = dateTimeArray[0].ToString("MM-dd") + "—" + dateTimeArray[dateTimeArray.Count - 1].ToString("MM-dd");
                    dr["ContinuousDays"] = outDays;
                    dr["LightPollution"] = LightPollutionDays;
                    dr["ModeratePollution"] = ModeratePollutionDays;
                    dr["HighPollution"] = HighPollutionDays;
                    dr["SeriousPollution"] = SeriousPollutionDays;

                    dt.Rows.Add(dr);
                }
                return dt.DefaultView;
            }
            return null;
        }

        /// <summary>
        /// 获取不同区域污染情况分析数据
        /// </summary>
        /// <param name="regionGuid">区域</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="AQIType">污染因子</param>
        /// <param name="pollutionGrade">污染等级</param>
        /// <returns>returnDataTable</returns>
        public DataView GetRegionsPollution(string regionGuid, DateTime[,] dateTime, int pollutionClass)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            List<List<RegionDayAQIReportEntity>> AllData = new List<List<RegionDayAQIReportEntity>>();
            DataTable dt = new DataTable();
            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                dt.Columns.Add(dateTime[i, 0].Year.ToString() + "(" + i + ")", typeof(string));
                dt.Columns.Add("AQIValue" + "(" + i + ")", typeof(string));
                dt.Columns.Add("PrimaryPollutant" + "(" + i + ")", typeof(string));
                dt.Columns.Add("Grade" + "(" + i + ")", typeof(string));

                DateTime dateStart = dateTime[i, 0];
                DateTime dateEnd = dateTime[i, 1];
                List<RegionDayAQIReportEntity> data = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuid && p.ReportDateTime >= dateStart && p.ReportDateTime <= dateEnd && p.Class == pollutionClass.ToString()).ToList<RegionDayAQIReportEntity>();
                AllData.Add(data);
            }

            if (AllData.Count > 0)
            {
                int Max = AllData[0].Count;
                for (int i = 1; i < AllData.Count; i++)
                {
                    if (AllData[i].Count > Max)
                    {
                        Max = AllData[i].Count;
                    }
                }

                for (int i = 0; i < Max; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        dr[k] = "";
                    }
                    dt.Rows.Add(dr);
                }

                if (dateTime.GetLength(0) == 1)
                {
                    List<RegionDayAQIReportEntity> data1 = AllData[0];
                    for (int i = 0; i < data1.Count; i++)
                    {
                        dt.Rows[i][0] = data1[i].ReportDateTime.ToString();
                        dt.Rows[i][1] = data1[i].AQIValue.ToString();
                        dt.Rows[i][2] = data1[i].PrimaryPollutant.ToString();
                        dt.Rows[i][3] = data1[i].Class.ToString();
                    }
                }

                if (dateTime.GetLength(0) == 2)
                {
                    List<RegionDayAQIReportEntity> data1 = AllData[0];
                    List<RegionDayAQIReportEntity> data2 = AllData[1];
                    for (int i = 0; i < data1.Count; i++)
                    {
                        dt.Rows[i][0] = data1[i].ReportDateTime.ToString();
                        dt.Rows[i][1] = data1[i].AQIValue.ToString();
                        dt.Rows[i][2] = data1[i].PrimaryPollutant.ToString();
                        dt.Rows[i][3] = data1[i].Class.ToString();
                    }
                    for (int i = 0; i < data2.Count; i++)
                    {
                        dt.Rows[i][0] = data2[i].ReportDateTime.ToString();
                        dt.Rows[i][1] = data2[i].AQIValue.ToString();
                        dt.Rows[i][2] = data2[i].PrimaryPollutant.ToString();
                        dt.Rows[i][3] = data2[i].Class.ToString();
                    }
                }

                if (dateTime.GetLength(0) == 3)
                {
                    List<RegionDayAQIReportEntity> data1 = AllData[0];
                    List<RegionDayAQIReportEntity> data2 = AllData[1];
                    List<RegionDayAQIReportEntity> data3 = AllData[2];
                    for (int i = 0; i < data1.Count; i++)
                    {
                        dt.Rows[i][0] = data1[i].ReportDateTime.ToString();
                        dt.Rows[i][1] = data1[i].AQIValue.ToString();
                        dt.Rows[i][2] = data1[i].PrimaryPollutant.ToString();
                        dt.Rows[i][3] = data1[i].Class.ToString();
                    }
                    for (int i = 0; i < data2.Count; i++)
                    {
                        dt.Rows[i][4] = data2[i].ReportDateTime.ToString();
                        dt.Rows[i][5] = data2[i].AQIValue.ToString();
                        dt.Rows[i][6] = data2[i].PrimaryPollutant.ToString();
                        dt.Rows[i][7] = data2[i].Class.ToString();
                    }
                    for (int i = 0; i < data3.Count; i++)
                    {
                        dt.Rows[i][8] = data3[i].ReportDateTime.ToString();
                        dt.Rows[i][9] = data3[i].AQIValue.ToString();
                        dt.Rows[i][10] = data3[i].PrimaryPollutant.ToString();
                        dt.Rows[i][11] = data3[i].Class.ToString();
                    }
                }
            }
            return dt.DefaultView;
        }

        /// <summary>
        /// 获取空气质量日报
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetRegionAirQualityDayReport(string[] regionGuids, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "ReportDateTime,MonitoringRegionUid")
        {
            recordTotal = 0;
            DictionaryService dictionary = new DictionaryService();
            DataTable dt = new DataTable();
            dt = GetAreaDataPager(regionGuids, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("RegionName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string regionUid = dt.Rows[i]["MonitoringRegionUid"].ToString();
                dt.Rows[i]["RegionName"] = dictionary.GetTextByValue(Core.Enums.DictionaryType.Air, "行政区划", regionUid);
            }
            return dt.AsDataView();
        }
        #endregion

        #region 接口实现
        /// <summary>
        /// 获取某段时间内某一监测点AQI
        /// </summary>
        /// <returns></returns>
        public int GetTimePortAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点AQI
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点AQI
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetTimePortAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetTimePortPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        public int GetTimePortOutDay()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsOutDay()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsOutDay()
        {
            return 0;
        }
        #endregion

        #region 苏州市环境空气质量月报
        /// <summary>
        /// 苏州市环境空气质量月报
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// </returns>
        public DataSet GetRegionsMonthAllData(DateTime dateStart, DateTime dateEnd, string strYearJiShu)
        {
            //月报中AQI部分数据是以当前月的上个月为基准的，酸雨部分是以当前月为基准的
            #region 日期和时间
            DateTime dateLastMonthStart = dateStart.AddMonths(-1);//当前月的上一个月
            DateTime dateLastMonthEnd = new DateTime(dateEnd.Year, dateEnd.Month, 1).AddDays(-1);//当前月的上一个月
            DateTime dateLastYearStart = dateStart.AddYears(-1);//上一年的当前月
            DateTime dateLastYearEnd = new DateTime(dateEnd.Year - 1, dateEnd.Month, 1).AddMonths(1).AddDays(-1);//上一年的当前月
            int yearJiShu = int.TryParse(strYearJiShu, out yearJiShu) ? yearJiShu : dateStart.Year;
            int monthNow = dateStart.Month;
            DateTime dateJiShuStart = new DateTime(yearJiShu, monthNow, 1);//基数年月
            DateTime dateJiShuEnd = new DateTime(yearJiShu, dateEnd.Month, 1).AddMonths(1).AddDays(-1);//基数年月
            DateTime dateQuanNianStart = new DateTime(dateStart.Year, 1, 1);//所在年的一月
            DateTime dateQuanNianEnd = dateEnd;//所在年的当前月
            DateTime dateLastQuanNianStart = dateQuanNianStart.AddYears(-1);//去年的一月
            DateTime dateLastQuanNianEnd = dateLastYearEnd;//去年的当前月
            DateTime dateJiShuQuanNianStart = new DateTime(dateJiShuStart.Year, 1, 1);//基数年的一月
            DateTime dateJiShuQuanNianEnd = dateJiShuEnd;//基数年的当前月
            #endregion
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            DataQueryByDayService dataQueryByDayService = Singleton<DataQueryByDayService>.GetInstance();
            RegionDayAQIRepository regionDayAQIRepository = Singleton<RegionDayAQIRepository>.GetInstance();
            DayReportRepository dayReportRepository = Singleton<DayReportRepository>.GetInstance();
            DataSet dsReturn = CreateMonthReportDataTable();//创建月报表中的各种数据的数据集
            //string yearNow = dateStart.Year.ToString();//当前年
            //string yearLast = dateLastYearStart.Year.ToString();//上一年

            if (regionDayAQI != null && regionDayAQIRepository != null && dayReportRepository != null)
            {
                IList<string> regionUidQuXianList = new List<string> { "7e05b94c-bbd4-45c3-919c-42da2e63fd43", "66d2abd1-ca39-4e39-909f-da872704fbfd"	,
                           "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff", "57b196ed-5038-4ad0-a035-76faee2d7a98","2e2950cd-dbab-43b3-811d-61bd7569565a"	,
                           "2fea3cb2-8b95-45e6-8a71-471562c4c89c" };//五区县和市区GUID
                IList<string> regionUidQuanShiList = new List<string> { "7e05b94c-bbd4-45c3-919c-42da2e63fd43", "66d2abd1-ca39-4e39-909f-da872704fbfd"	,
                           "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff", "57b196ed-5038-4ad0-a035-76faee2d7a98","2e2950cd-dbab-43b3-811d-61bd7569565a"	,
                           "2fea3cb2-8b95-45e6-8a71-471562c4c89c" ,"5a566145-4884-453c-93ad-16e4344c85c9"};//五区县和市区GUID及全市
                IList<string> regionQuanShiList = new List<string> { "5a566145-4884-453c-93ad-16e4344c85c9" };//五区县和市区GUID及全市
                IList<string> nowUnOverPollutantQSList = new List<string>();//全市当前月没有超标的污染列表
                IList<string> nowUnOverPollutantSQList = new List<string>();//市区当前月没有超标的污染列表
                IList<string> nowUnOverPollutantLYQSList = new List<string>();//全市上一年月没有超标的污染列表
                IList<string> nowUnOverPollutantLMQSList = new List<string>();//全市上一个月没有超标的污染列表
                DataTable dtNowMonth = new DataTable();   //当前年月
                DataTable dtNowMonthPoint = new DataTable();   //测点当前年月
                DataTable dtLastYear = new DataTable();  //去年同月
                DataTable dtLastYearPoint = new DataTable();  //测点去年同月
                DataTable dtJiShu = new DataTable();  //基数年
                DataTable dtQuanNian = new DataTable();  //截止到当前月
                DataTable dtLastQuanNian = new DataTable();  //截止到去年当前月
                DataTable dtJiShuQuanNian = new DataTable();  //截止到基数年当前月
                DataTable dtLastMonth = new DataTable();   //上一个月
                DataTable dtNowQuanShi = regionDayAQI.GetRegionsHalfYearData(dateStart, dateEnd).Table;  //当前月全市
                DataTable dtLastYQuanShi = regionDayAQI.GetRegionsHalfYearData(dateLastYearStart, dateLastYearEnd).Table;  //上一年月全市
                DataTable dtJiShuQuanShi = regionDayAQI.GetRegionsHalfYearData(dateJiShuStart, dateJiShuEnd).Table;  //基数年月全市
                //DataTable dtLastMQuanShi = regionDayAQI.GetRegionsHalfYearData(dateLastMonthStart, dateLastMonthEnd).Table;   //上一个月全市
                DataTable dtQuanNianQuanShi = regionDayAQI.GetRegionsHalfYearData(dateQuanNianStart, dateQuanNianEnd).Table;  //截止到所在年月全市
                DataTable dtLYQuanNianQuanShi = regionDayAQI.GetRegionsHalfYearData(dateLastQuanNianStart, dateLastQuanNianEnd).Table;   //截止到上一年月全市             
                DataTable dtJiQuanNianQuanShi = regionDayAQI.GetRegionsHalfYearData(dateJiShuQuanNianStart, dateJiShuQuanNianEnd).Table;   //截止到基数年月全市             
                DataTable dtNowMaxValueQuXian = regionDayAQIRepository.GetMaxValue(regionUidQuXianList.ToArray(), dateStart, dateEnd).Table;//取得指定日期内日数据最大值数据
                DataTable dtNowMinValueQuXian = regionDayAQIRepository.GetMinValue(regionUidQuXianList.ToArray(), dateStart, dateEnd).Table;//取得指定日期内日数据最小值数据
                DataTable dtNowMaxValueQuanShi = regionDayAQIRepository.GetMaxValue(regionQuanShiList.ToArray(), dateStart, dateEnd).Table;//取得指定日期内日数据最大值数据
                DataTable dtNowMinValueQuanShi = regionDayAQIRepository.GetMinValue(regionQuanShiList.ToArray(), dateStart, dateEnd).Table;//取得指定日期内日数据最小值数据
                DataTable dtNowAvgValueQuXian = regionDayAQIRepository.GetAvgValue(regionUidQuXianList.ToArray(), dateStart, dateEnd).Table;//取得指定日期内日数据最小值数据
                DataTable dtText = dsReturn.Tables["Text"];  //创建月报表文字说明中的数据表
                DataTable dtBiaoAirInfo = dsReturn.Tables["BiaoAirInfo"];//重要信息表数据
                DataTable dtBiao1 = dsReturn.Tables["Biao1"];//表1数据
                DataTable dtBiao2 = dsReturn.Tables["Biao2"];//表2数据
                DataTable dtBiao3 = dsReturn.Tables["Biao3"];//表3数据
                DataTable dtBiao4 = dsReturn.Tables["Biao4"];//表4数据
                DataTable dtTu1 = dsReturn.Tables["Tu1"];//图1数据
                DataTable dtTu2 = dsReturn.Tables["Tu2"];//图2数据
                DataRow drText = dtText.NewRow();
                dtText.Rows.Add(drText);
                decimal standardRateMax = 0;
                decimal standardRateMin = int.MaxValue;
                decimal AQIMax = 0;
                decimal AQIMin = int.MaxValue;
                string chaoFactorOneQS = string.Empty;
                string chaoFactorTwoQS = string.Empty;
                string AQIMinShiqu = "0";
                string AQIMaxShiqu = "0";
                decimal AQIAvgShiQu = 0;
                Dictionary<string, string> daBiaoBiJiaoQSDictionary = new Dictionary<string, string>();//全市各区县达标天数与上年同月比较
                Dictionary<string, string> daBiaoBiJiaoPIDictionary = new Dictionary<string, string>();//全市各区县达标天数与上年同月比较
                #region 区域GUID
                // '7e05b94c-bbd4-45c3-919c-42da2e63fd43'                 --市区
                //,'66d2abd1-ca39-4e39-909f-da872704fbfd'					--张家港市
                //,'d7d7a1fe-493a-4b3f-8504-b1850f6d9eff'					--常熟市
                //,'57b196ed-5038-4ad0-a035-76faee2d7a98'					--太仓市
                //,'2e2950cd-dbab-43b3-811d-61bd7569565a'					--昆山市
                //,'2fea3cb2-8b95-45e6-8a71-471562c4c89c'					--吴江区
                //,'e1c104f3-aaf3-4d0e-9591-36cdc83be15a'					--吴中区
                //,'8756bd44-ff18-46f7-aedf-615006d7474c'					--相城区
                //,'6a4e7093-f2c6-46b4-a11f-0f91b4adf379'					--姑苏区
                //,'69a993ff-78c6-459b-9322-ee77e0c8cd68'					--工业园区
                //,'f320aa73-7c55-45d4-a363-e21408e0aac3'					--高新区
                #endregion

                #region 获取数据
                dtNowMonth = regionDayAQIRepository.GetRegionsAllDataByDate(dateStart, dateEnd).Table;  //当前年月
                dtNowMonth.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dtNowMonth.Rows.Count; i++)
                {
                    dtNowMonth.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dtNowMonth.Rows[i]["MonitoringRegionUid"].ToString());
                }
                dtLastYear = regionDayAQIRepository.GetRegionsAllDataByDate(dateLastYearStart, dateLastYearEnd).Table;   //上一年月
                dtLastYear.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dtLastYear.Rows.Count; i++)
                {
                    dtLastYear.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dtLastYear.Rows[i]["MonitoringRegionUid"].ToString());
                }
                dtNowMonthPoint = regionDayAQIRepository.GetPointsAllDataByDate(dateStart, dateEnd).Table;  //测点当前年月
                dtNowMonthPoint.Columns.Add("pointName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dtNowMonthPoint.Rows.Count; i++)
                {
                    dtNowMonthPoint.Rows[i]["pointName"] = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(dtNowMonthPoint.Rows[i]["PointId"])).MonitoringPointName;
                }
                dtLastYearPoint = regionDayAQIRepository.GetPointsAllDataByDate(dateLastYearStart, dateLastYearEnd).Table;  //测点当前年月
                dtLastYearPoint.Columns.Add("pointName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dtLastYearPoint.Rows.Count; i++)
                {
                    dtLastYearPoint.Rows[i]["pointName"] = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(dtLastYearPoint.Rows[i]["PointId"])).MonitoringPointName;
                }
                if (int.TryParse(strYearJiShu, out yearJiShu))
                {
                    dtJiShu = dayReportRepository.GetRegionBaseDataByDate(dateStart, dateEnd, strYearJiShu).Table;//基数年（以上个月为基础）
                    //dtN = regionDayAQIRepository.GetRegionsAllData(dtSta.AddYears(-1), dtEn.AddYears(-1)).Table;   //2014
                    dtJiShu.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                    for (int i = 0; i < dtJiShu.Rows.Count; i++)
                    {
                        dtJiShu.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dtJiShu.Rows[i]["MonitoringRegionUid"].ToString());
                    }
                    dtJiShuQuanNian = dayReportRepository.GetRegionBaseDataByDate(dateJiShuQuanNianStart, dateJiShuQuanNianEnd, strYearJiShu).Table;   //截止到基数年当前月
                    dtJiShuQuanNian.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                    for (int i = 0; i < dtJiShuQuanNian.Rows.Count; i++)
                    {
                        dtJiShuQuanNian.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dtJiShuQuanNian.Rows[i]["MonitoringRegionUid"].ToString());
                    }
                }
                dtQuanNian = regionDayAQIRepository.GetRegionsAllDataByDate(dateQuanNianStart, dateQuanNianEnd).Table;   //截止到当前年月
                dtQuanNian.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dtQuanNian.Rows.Count; i++)
                {
                    dtQuanNian.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dtQuanNian.Rows[i]["MonitoringRegionUid"].ToString());
                }
                dtLastQuanNian = regionDayAQIRepository.GetRegionsAllDataByDate(dateLastQuanNianStart, dateLastQuanNianEnd).Table;   //截止到去年当前月
                dtLastQuanNian.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dtLastQuanNian.Rows.Count; i++)
                {
                    dtLastQuanNian.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dtLastQuanNian.Rows[i]["MonitoringRegionUid"].ToString());
                }
                dtLastMonth = regionDayAQIRepository.GetRegionsAllDataByDate(dateLastMonthStart, dateLastMonthEnd).Table;   //上一个月
                dtLastMonth.Columns.Add("regionName", typeof(string)).SetOrdinal(0);
                for (int i = 0; i < dtLastMonth.Rows.Count; i++)
                {
                    dtLastMonth.Rows[i]["regionName"] = g_DictionaryService.GetCodeDictionaryTextByValue(dtLastMonth.Rows[i]["MonitoringRegionUid"].ToString());
                }
                #endregion

                bool isLastY = (dtLastYear.Rows.Count > 0);
                bool isJiShu = (dtJiShu.Rows.Count > 0);
                bool isLastYQS = (dtLastYQuanShi.Rows.Count > 0);
                bool isJiShuQS = (dtJiShuQuanShi.Rows.Count > 0);
                bool isLastM = (dtLastMonth.Rows.Count > 0);
                //bool isLastMQS = (dtLastMQuanShi.Rows.Count > 0);
                bool isLYQuanNianQS = (dtLYQuanNianQuanShi.Rows.Count > 0);
                bool isJiQuanNianQS = (dtJiQuanNianQuanShi.Rows.Count > 0);

                #region 苏州市区截止到当前月
                DataRow[] drsQuanNian = dtQuanNian.Select(string.Format("MonitoringRegionUid='{0}'", "7e05b94c-bbd4-45c3-919c-42da2e63fd43"));

                if (drsQuanNian.Length > 0)
                {
                    DataRow drQuanNian = drsQuanNian[0];
                    decimal PM25_C = DecimalExtension.GetRoundValue((decimal.TryParse(drQuanNian["a34004"].ToString(), out PM25_C) ? PM25_C : 0) * 1000, 1);
                    decimal standardRate = 0;//达标率

                    drText["Important1_PM25"] = DecimalExtension.GetRoundValue(PM25_C, 1).ToString("0.0");

                    //达标率
                    if (drQuanNian["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drQuanNian["ValidCount"]) != 0)
                    {
                        if (drQuanNian["StandardCount"].IsNotNullOrDBNull())
                        {
                            standardRate = DecimalExtension.GetRoundValue(Convert.ToDecimal(drQuanNian["StandardCount"])
                                            / Convert.ToDecimal(drQuanNian["ValidCount"]) * 100, 1);   //市区达标率
                            drText["Important2_DaBiaoRateShiQu"] = standardRate.ToString("0.0") + "%";
                        }
                    }

                    #region 上一年月
                    DataRow[] drsLastQuanNian = dtLastQuanNian.Select(string.Format("MonitoringRegionUid='{0}'", "7e05b94c-bbd4-45c3-919c-42da2e63fd43"));

                    if (drsLastQuanNian.Length > 0)
                    {
                        DataRow drLastQuanNian = drsLastQuanNian[0];
                        decimal PM25_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(drLastQuanNian["a34004"].ToString(), out PM25_CLY) ? PM25_CLY : 0) * 1000, 1);
                        decimal PM25Rate;

                        #region PM2.5浓度比较
                        if (PM25_CLY > 0)
                        {
                            PM25Rate = DecimalExtension.GetRoundValue((PM25_C - PM25_CLY) / PM25_CLY * 100, 1);

                            if (PM25Rate > 0)
                            {
                                drText["Important1_LastBi"] = "相比上升了" + PM25Rate.ToString("0.0") + "%";
                            }
                            else if (PM25Rate == 0)
                            {
                                drText["Important1_LastBi"] = "持平";
                            }
                            else
                            {
                                drText["Important1_LastBi"] = "相比下降了" + (-PM25Rate).ToString("0.0") + "%";
                            }
                        }
                        #endregion

                        #region 达标率比较
                        if (drLastQuanNian["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drLastQuanNian["ValidCount"]) != 0)
                        {
                            if (drLastQuanNian["StandardCount"].IsNotNullOrDBNull())
                            {
                                decimal standardRateL = DecimalExtension.GetRoundValue(Convert.ToDecimal(drLastQuanNian["StandardCount"])
                                                 / Convert.ToDecimal(drLastQuanNian["ValidCount"]) * 100, 1);   //达标率

                                //if (standardRateL > 0)
                                //{
                                //decimal lastRate = DecimalExtension.GetRoundValue((standardRate - standardRateL) / standardRateL * 100, 1);
                                decimal lastRate = standardRate - standardRateL;

                                if (lastRate > 0)
                                {
                                    drText["Important2_LastBiShiQu"] = "相比上升了" + lastRate.ToString("0.0") + "个百分点";
                                }
                                else if (lastRate == 0)
                                {
                                    drText["Important2_LastBiShiQu"] = "持平";
                                }
                                else
                                {
                                    drText["Important2_LastBiShiQu"] = "相比下降了" + (-lastRate).ToString("0.0") + "个百分点";
                                }
                                //}
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region 基数年月
                    DataRow[] drsJiShuQuanNian = dtJiShuQuanNian.Select(string.Format("MonitoringRegionUid='{0}'", "7e05b94c-bbd4-45c3-919c-42da2e63fd43"));

                    if (drsJiShuQuanNian.Length > 0)
                    {
                        DataRow drJiShuQuanNian = drsJiShuQuanNian[0];
                        decimal PM25_CLMJS = DecimalExtension.GetRoundValue((decimal.TryParse(drJiShuQuanNian["a34004"].ToString(), out PM25_CLMJS) ? PM25_CLMJS : 0), 1);
                        decimal PM25Rate;

                        #region PM2.5浓度比较
                        if (PM25_CLMJS > 0)
                        {
                            PM25Rate = DecimalExtension.GetRoundValue((PM25_C - PM25_CLMJS) / PM25_CLMJS * 100, 1);
                            if (PM25Rate > 0)
                            {
                                drText["Important1_JiShuBi"] = "相比上升了" + PM25Rate.ToString("0.0") + "%";
                            }
                            else if (PM25Rate == 0)
                            {
                                drText["Important1_JiShuBi"] = "持平";
                            }
                            else
                            {
                                drText["Important1_JiShuBi"] = "相比下降了" + (-PM25Rate).ToString("0.0") + "%";
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion

                #region 苏州全市截止到当前月
                if (dtQuanNian.Rows.Count > 0)
                {
                    //当前年月市区和区县的数据
                    DataTable dtQuanNianQuXian = dtQuanNian.Select(string.Format("MonitoringRegionUid='{0}'", "5a566145-4884-453c-93ad-16e4344c85c9")).CopyToDataTable();
                    //DataTable dtQuanNianQuXian = dtQuanNian.AsEnumerable()
                    //    .Where(t => regionUidQuXianList.Contains(t.Field<string>("MonitoringRegionUid"))).CopyToDataTable();

                    //上一年月市区和区县的数据
                    DataTable dtLastQuanNianQuXian = dtLastQuanNian.Select(string.Format("MonitoringRegionUid='{0}'", "5a566145-4884-453c-93ad-16e4344c85c9")).CopyToDataTable();
                    //DataTable dtLastQuanNianQuXian = dtLastQuanNian.AsEnumerable()
                    //    .Where(t => regionUidQuXianList.Contains(t.Field<string>("MonitoringRegionUid"))).CopyToDataTable();

                    //基数年月市区和区县的数据
                    //DataTable dtJiShuQuanNianQuXian = dtJiShuQuanNian.Select(string.Format("MonitoringRegionUid='{0}'", "5a566145-4884-453c-93ad-16e4344c85c9")).CopyToDataTable();
                    DataTable dtJiShuQuanNianQuXian = dtJiShuQuanNian.AsEnumerable()
                        .Where(t => regionUidQuXianList.Contains(t.Field<string>("MonitoringRegionUid"))).CopyToDataTable();

                    //所有区县值算全市平均值
                    decimal PM25_CQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtQuanNianQuXian.Compute("AVG(a34004)", "").ToString(), out PM25_CQS) ? PM25_CQS : 0) * 1000, 1);
                    //decimal PM10_CQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtQuanNianQuXian.Compute("AVG(a34002)", "").ToString(), out PM10_CQS) ? PM10_CQS : 0) * 1000, 1);
                    //decimal NO2_CQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtQuanNianQuXian.Compute("AVG(a21004)", "").ToString(), out NO2_CQS) ? NO2_CQS : 0) * 1000, 1);
                    //decimal SO2_CQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtQuanNianQuXian.Compute("AVG(a21026)", "").ToString(), out SO2_CQS) ? SO2_CQS : 0) * 1000, 1);
                    //decimal CO_CQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtQuanNianQuXian.Compute("AVG(a21005)", "").ToString(), out CO_CQS) ? CO_CQS : 0), 1);
                    //decimal O3_CQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtQuanNianQuXian.Compute("AVG(a05024)", "").ToString(), out O3_CQS) ? O3_CQS : 0) * 1000, 1);
                    decimal PM25_CLYQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastQuanNianQuXian.Compute("AVG(a34004)", "").ToString(), out PM25_CLYQS) ? PM25_CLYQS : 0) * 1000, 1);
                    decimal PM25_CJiQS = DecimalExtension.GetRoundValue(decimal.TryParse(dtJiShuQuanNianQuXian.Compute("AVG(a34004)", "").ToString(), out PM25_CJiQS) ? PM25_CJiQS : 0, 1);
                    decimal standardRate = 0;//达标天数比例
                    decimal standardCountQS = 0;//全市达标天数
                    decimal validCountQS = 0;//全市有效天数
                    decimal standardCountLYQS = 0;//全市上一年月达标天数
                    decimal validCountLYQS = 0;//全市上一年月有效天数
                    decimal standardCountJiQS = 0;//全市基数年月达标天数
                    decimal validCountJiQS = 0;//全市基数年月有效天数
                    decimal PM25JiNowRateQS = DecimalExtension.GetRoundValue((PM25_CQS - PM25_CJiQS) / PM25_CJiQS * 100, 1);
                    //decimal daBiaoJiRateQS = 0;//全市基数年月达标率
                    //decimal daBiaoNowQS = 0;//全市当前年月达标率
                    decimal daBiaoLastRateQS = 0;//全市上一年月达标率
                    decimal daBiaoLastRateDiffQS = 0;//全市上一年月与当前年月达标率比较

                    #region 全市达标率
                    if (dtQuanNianQuanShi.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtQuanNianQuanShi.Rows.Count; i++)
                        {
                            decimal AQIcount = 0;
                            string primaryPollutantName = string.Empty;

                            for (int j = 1; j < 7; j++)
                            {
                                string columnName = dtQuanNianQuanShi.Columns[j].ColumnName;
                                string pollutantName = GetPollutantNameByCode(columnName);//根据因子Code获取因子名称
                                int num = 24;

                                if (columnName == "a05024")
                                {
                                    num = 8;
                                }

                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtQuanNianQuanShi.Rows[i][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(columnName, Con, num), 0);

                                if (AQIcount < temp)
                                {
                                    AQIcount = temp;
                                    primaryPollutantName = pollutantName;
                                }
                            }
                            if (AQIcount > 0)
                            {
                                validCountQS++;
                                if (AQIcount <= 100)
                                {
                                    standardCountQS++;
                                }
                            }
                        }
                        if (validCountQS != 0)
                        {
                            standardRate = DecimalExtension.GetRoundValue(standardCountQS / validCountQS * 100, 1);
                            drText["Important2_DaBiaoRateQuanShi"] = standardRate + "%";
                        }
                    }
                    #endregion

                    #region 全市上一年月
                    if (isLYQuanNianQS)
                    {
                        //全市上一年月达标率
                        for (int i = 0; i < dtLYQuanNianQuanShi.Rows.Count; i++)
                        {
                            decimal AQIcount = 0;

                            for (int j = 1; j < 7; j++)
                            {
                                string columnName = dtLYQuanNianQuanShi.Columns[j].ColumnName;
                                int num = 24;

                                if (columnName == "a05024")
                                {
                                    num = 8;
                                }
                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtLYQuanNianQuanShi.Rows[i][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(columnName, Con, num), 0);
                                if (AQIcount < temp)
                                {
                                    AQIcount = temp;
                                }
                            }
                            if (AQIcount > 0)
                            {
                                validCountLYQS++;
                                if (AQIcount <= 100)
                                {
                                    standardCountLYQS++;
                                }
                            }
                        }
                        if (validCountLYQS != 0)
                        {
                            daBiaoLastRateQS = DecimalExtension.GetRoundValue(standardCountLYQS / validCountLYQS * 100, 1);

                            //达标率比较
                            //if (daBiaoLastRateQS > 0)
                            //{
                            //daBiaoLastRateDiffQS = DecimalExtension.GetRoundValue((standardRate - daBiaoLastRateQS) / daBiaoLastRateQS * 100, 1);
                            daBiaoLastRateDiffQS = standardRate - daBiaoLastRateQS;

                            if (daBiaoLastRateDiffQS > 0)
                            {
                                drText["Important2_LastBiQuanShi"] = "相比上升了" + daBiaoLastRateDiffQS.ToString("0.0") + "个百分点";
                            }
                            else if (daBiaoLastRateDiffQS == 0)
                            {
                                drText["Important2_LastBiQuanShi"] = "持平";
                            }
                            else
                            {
                                drText["Important2_LastBiQuanShi"] = "相比下降了" + (-daBiaoLastRateDiffQS).ToString("0.0") + "个百分点";
                            }
                            //}
                        }
                    }
                    #endregion

                    #region 全市基数年月
                    if (isJiQuanNianQS)
                    {
                        ////全市基数年月达标率
                        //for (int i = 0; i < dtJiQuanNianQuanShi.Rows.Count; i++)
                        //{
                        //    decimal AQIcount = 0;

                        //    for (int j = 1; j < 7; j++)
                        //    {
                        //        string columnName = dtJiQuanNianQuanShi.Columns[j].ColumnName;
                        //        int num = 24;

                        //        if (columnName == "a05024")
                        //        {
                        //            num = 8;
                        //        }
                        //        decimal Con = Convert.ToDecimal(dtJiQuanNianQuanShi.Rows[i][j]);
                        //        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(columnName, Con, num), 0);
                        //        if (AQIcount < temp)
                        //        {
                        //            AQIcount = temp;
                        //        }
                        //    }
                        //    if (AQIcount > 0)
                        //    {
                        //        validCountJiQS++;
                        //        if (AQIcount <= 100)
                        //        {
                        //            standardCountJiQS++;
                        //        }
                        //    }
                        //}
                        //if (validCountJiQS != 0)
                        //{
                        //daBiaoJiRateQS = DecimalExtension.GetRoundValue(standardCountJiQS / validCountJiQS * 100, 1);
                        //}
                    }
                    #endregion

                    #region 表 苏州市环境空气PM2.5浓度和达标天数比例情况
                    foreach (DataRow drQuanNian in dtQuanNian.Rows)
                    {
                        string monitoringRegionUid = drQuanNian["MonitoringRegionUid"].ToString();

                        string regionName = drQuanNian["regionName"].ToString();
                        DataRow drBiaoAirInfo = dtBiaoAirInfo.NewRow();
                        DataRow[] drsLastQuanNian = dtLastQuanNian.Select(string.Format("MonitoringRegionUid='{0}'", monitoringRegionUid));
                        DataRow[] drsJiShuQuanNian = dtJiShuQuanNian.Select(string.Format("MonitoringRegionUid='{0}'", monitoringRegionUid));
                        decimal PM25_CB = DecimalExtension.GetRoundValue((decimal.TryParse(drQuanNian["a34004"].ToString(), out PM25_CB) ? PM25_CB : 0) * 1000, 1);
                        decimal PM25_CLYB = 0;
                        decimal PM25_CLMJSB = 0;
                        decimal PM25JiNowRateB = 0;
                        //decimal daBiaoJiRate = 0;
                        decimal daBiaoNowRate = 0;
                        decimal daBiaoLastRate = 0;
                        decimal daBiaoLastRateDiff = 0;//达标比率差值

                        #region 数据处理
                        if (drQuanNian["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drQuanNian["ValidCount"]) != 0)
                        {
                            if (drQuanNian["StandardCount"].IsNotNullOrDBNull())
                            {
                                daBiaoNowRate = DecimalExtension.GetRoundValue(Convert.ToDecimal(drQuanNian["StandardCount"])
                                                / Convert.ToDecimal(drQuanNian["ValidCount"]) * 100, 1);   //市区达标率
                            }
                        }
                        if (drsLastQuanNian.Length > 0)
                        {
                            DataRow drLastQuanNian = drsLastQuanNian[0];
                            PM25_CLYB = DecimalExtension.GetRoundValue((decimal.TryParse(drLastQuanNian["a34004"].ToString(), out PM25_CLYB) ? PM25_CLYB : 0) * 1000, 1);

                            if (drLastQuanNian["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drLastQuanNian["ValidCount"]) != 0)
                            {
                                if (drLastQuanNian["StandardCount"].IsNotNullOrDBNull())
                                {
                                    daBiaoLastRate = DecimalExtension.GetRoundValue(Convert.ToDecimal(drLastQuanNian["StandardCount"])
                                                     / Convert.ToDecimal(drLastQuanNian["ValidCount"]) * 100, 1);   //达标率

                                    //if (daBiaoLastRate > 0)
                                    //{
                                    //daBiaoLastRateDiff = DecimalExtension.GetRoundValue((daBiaoNowRate - daBiaoLastRate) / daBiaoLastRate * 100, 1);
                                    daBiaoLastRateDiff = daBiaoNowRate - daBiaoLastRate;
                                    //}
                                }
                            }
                        }
                        if (drsJiShuQuanNian.Length > 0)
                        {
                            DataRow drJiShuQuanNian = drsJiShuQuanNian[0];
                            PM25_CLMJSB = DecimalExtension.GetRoundValue((decimal.TryParse(drJiShuQuanNian["a34004"].ToString(), out PM25_CLMJSB) ? PM25_CLMJSB : 0), 1);

                            //if (drJiShuQuanNian["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drJiShuQuanNian["ValidCount"]) != 0)
                            //{
                            //    if (drJiShuQuanNian["StandardCount"].IsNotNullOrDBNull())
                            //    {
                            //        daBiaoJiRate = DecimalExtension.GetRoundValue(Convert.ToDecimal(drJiShuQuanNian["StandardCount"])
                            //                        / Convert.ToDecimal(drJiShuQuanNian["ValidCount"]) * 100, 1);   //市区达标率
                            //    }
                            //}
                        }
                        if (PM25_CLMJSB > 0)
                        {
                            PM25JiNowRateB = DecimalExtension.GetRoundValue((PM25_CB - PM25_CLMJSB) / PM25_CLMJSB * 100, 1);
                        }
                        #endregion

                        drBiaoAirInfo["MonitoringRegionUid"] = monitoringRegionUid;//区域GUID 
                        drBiaoAirInfo["RegionEn"] = GetRegionENByGuid(monitoringRegionUid);//英文名称
                        drBiaoAirInfo["RegionName"] = regionName;//中文名称
                        drBiaoAirInfo["PM25Last"] = PM25_CLYB.ToString();//PM2.5上一年浓度
                        drBiaoAirInfo["PM25Now"] = PM25_CB.ToString();//PM2.5当前年浓度
                        drBiaoAirInfo["PM25Ji"] = PM25JiNowRateB.ToString() + "%";//PM2.5基数年浓度
                        drBiaoAirInfo["DaBiaoJi"] = daBiaoLastRate.ToString() + "%";//达标天数基数年比例
                        drBiaoAirInfo["DaBiaoNow"] = daBiaoNowRate.ToString() + "%";//达标天数当前年比例
                        drBiaoAirInfo["DaBiaoLast"] = daBiaoLastRateDiff.ToString() + "%";//达标天数上一年比例
                        dtBiaoAirInfo.Rows.Add(drBiaoAirInfo);
                    }

                    DataRow drBiaoAirInfoQS = dtBiaoAirInfo.NewRow();
                    drBiaoAirInfoQS["MonitoringRegionUid"] = "";//区域GUID 
                    drBiaoAirInfoQS["RegionEn"] = GetRegionENByGuid("");//英文名称
                    drBiaoAirInfoQS["RegionName"] = "全市";//中文名称
                    drBiaoAirInfoQS["PM25Last"] = PM25_CLYQS.ToString();//PM2.5上一年浓度
                    drBiaoAirInfoQS["PM25Now"] = PM25_CQS.ToString();//PM2.5当前年浓度
                    drBiaoAirInfoQS["PM25Ji"] = PM25JiNowRateQS.ToString() + "%";//PM2.5基数年浓度
                    drBiaoAirInfoQS["DaBiaoJi"] = daBiaoLastRateQS.ToString("0.0") + "%";//达标天数基数年比例
                    drBiaoAirInfoQS["DaBiaoNow"] = standardRate + "%";//达标天数当前年比例
                    drBiaoAirInfoQS["DaBiaoLast"] = daBiaoLastRateDiffQS.ToString("0.0") + "%";//达标天数上一年比例
                    dtBiaoAirInfo.Rows.Add(drBiaoAirInfoQS);
                    #endregion
                }
                #endregion

                #region 苏州市区、吴江区和四市（县）数据，范围值
                foreach (DataRow drNowMonth in dtNowMonth.Rows)
                {
                    string monitoringRegionUid = drNowMonth["MonitoringRegionUid"].ToString();
                    string regionName = drNowMonth["regionName"].ToString();

                    //不是苏州市区、吴江区和四市（县）的不参与计算
                    if (!regionUidQuXianList.Contains(monitoringRegionUid))
                    {
                        continue;
                    }

                    #region 达标天数比例
                    if (drNowMonth["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drNowMonth["ValidCount"]) != 0)
                    {
                        if (drNowMonth["StandardCount"].IsNotNullOrDBNull())
                        {
                            decimal standardRate = DecimalExtension.GetRoundValue(Convert.ToDecimal(drNowMonth["StandardCount"])
                                             / Convert.ToDecimal(drNowMonth["ValidCount"]) * 100, 1);   //达标率
                            //decimal AQIValue = 0;

                            //for (int j = 2; j < 8; j++)
                            //{
                            //    string columnName = dtNowMonth.Columns[j].ColumnName;
                            //    int num = 24;

                            //    if (columnName == "a05024")
                            //    {
                            //        num = 8;
                            //    }
                            //    decimal Con = Convert.ToDecimal(drNowMonth[j]);
                            //    decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(columnName, Con, num), 0);
                            //    if (AQIValue < temp)
                            //    {
                            //        AQIValue = temp;
                            //    }
                            //}
                            if (standardRateMax < standardRate)
                            {
                                standardRateMax = standardRate;
                            }
                            if (standardRateMin > standardRate)
                            {
                                standardRateMin = standardRate;
                            }

                            #region 全市达标天数与上年比较
                            DataRow[] drsLastYear = dtLastYear.Select(string.Format("MonitoringRegionUid='{0}'", monitoringRegionUid));
                            if (drsLastYear.Length > 0)
                            {
                                DataRow drLastYear = drsLastYear[0];
                                if (drLastYear["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drLastYear["ValidCount"]) != 0)
                                {
                                    if (drLastYear["StandardCount"].IsNotNullOrDBNull())
                                    {
                                        decimal standardRateL = DecimalExtension.GetRoundValue(Convert.ToDecimal(drLastYear["StandardCount"])
                                                         / Convert.ToDecimal(drLastYear["ValidCount"]) * 100, 1);   //上一年同月达标率
                                        if (standardRate > standardRateL)
                                        {
                                            daBiaoBiJiaoQSDictionary.Add(regionName, "有所上升");
                                        }
                                        else if (standardRate == standardRateL)
                                        {
                                            daBiaoBiJiaoQSDictionary.Add(regionName, "持平");
                                        }
                                        else
                                        {
                                            daBiaoBiJiaoQSDictionary.Add(regionName, "有所下降");
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                drText["M101_DaBiaoRateRange"] = standardRateMin.ToString("0.0") + "%～" + standardRateMax.ToString("0.0") + "%";

                //全市达标天数与上年比较
                BiJiaoDaBiaoRateQS(daBiaoBiJiaoQSDictionary, drText);

                #region AQI范围
                decimal nowMinAQIQuXian = decimal.TryParse(dtNowMinValueQuanShi.Compute("MIN(AQIValue)", "").ToString(), out nowMinAQIQuXian) ? nowMinAQIQuXian : 0;//最小值
                decimal nowMaxAQIQuXian = decimal.TryParse(dtNowMaxValueQuanShi.Compute("MAX(AQIValue)", "").ToString(), out nowMaxAQIQuXian) ? nowMaxAQIQuXian : 0;//最大值
                drText["M101_AQIRange"] = nowMinAQIQuXian.ToString("0") + "～" + nowMaxAQIQuXian.ToString("0");
                #endregion
                #endregion

                #region 国控点数据
                Dictionary<string, decimal> SubDictionary = new Dictionary<string, decimal>();//首要污染物天数列表
                IList<string> PointOne = new List<string>();
                decimal overPoint = 0;
                foreach (DataRow drNowMonthPoint in dtNowMonthPoint.Rows)
                {
                    string pointId = drNowMonthPoint["PointId"].ToString();
                    string pointName = drNowMonthPoint["pointName"].ToString();

                    ////不是苏州市区、吴江区和四市（县）的不参与计算
                    //if (!regionUidQuXianList.Contains(pointId))
                    //{
                    //    continue;
                    //}

                    #region 达标天数比例
                    if (drNowMonthPoint["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drNowMonthPoint["ValidCount"]) != 0)
                    {
                        if (drNowMonthPoint["StandardCount"].IsNotNullOrDBNull())
                        {
                            decimal standardRate = DecimalExtension.GetRoundValue(Convert.ToDecimal(drNowMonthPoint["StandardCount"])
                                             / Convert.ToDecimal(drNowMonthPoint["ValidCount"]) * 100, 1);   //达标率

                            //if (standardRateMax < standardRate)
                            //{
                            //    standardRateMax = standardRate;
                            //}
                            //if (standardRateMin > standardRate)
                            //{
                            //    standardRateMin = standardRate;
                            //}

                            #region 全市达标天数与上年比较
                            DataRow[] drsLastYearPoint = dtLastYearPoint.Select(string.Format("PointId={0}", pointId));
                            decimal sub = 0;
                            decimal temp = 0;
                            if (drsLastYearPoint.Length > 0)
                            {
                                DataRow drLastYearPoint = drsLastYearPoint[0];
                                if (drLastYearPoint["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drLastYearPoint["ValidCount"]) != 0)
                                {
                                    if (drLastYearPoint["StandardCount"].IsNotNullOrDBNull())
                                    {
                                        decimal standardRateL = DecimalExtension.GetRoundValue(Convert.ToDecimal(drLastYearPoint["StandardCount"])
                                                         / Convert.ToDecimal(drLastYearPoint["ValidCount"]) * 100, 1);   //上一年同月达标率
                                        if (standardRate > standardRateL)
                                        {
                                            daBiaoBiJiaoPIDictionary.Add(pointName, "有所上升");
                                            sub = standardRateL - standardRate;
                                        }
                                        else if (standardRate == standardRateL)
                                        {
                                            daBiaoBiJiaoPIDictionary.Add(pointName, "持平");
                                            sub = standardRateL - standardRate;
                                        }
                                        else
                                        {
                                            daBiaoBiJiaoPIDictionary.Add(pointName, "有所下降");
                                            sub = standardRateL - standardRate;
                                        }
                                    }
                                }
                            }

                            #region 下降最多
                            if (sub > 0)
                                SubDictionary.Add(pointName, sub);

                            PointOne = SubDictionary.Where(t => t.Value == SubDictionary.Values.Max()).Select(t => t.Key).ToList();

                            if (PointOne.Count > 0)
                                drText["M106_DuiBi"] = PointOne.Aggregate((a, b) => a + "、" + b);
                            #endregion

                            #endregion
                        }
                    }
                    #endregion
                }

                //全市达标天数与上年比较
                BiJiaoDaBiaoRatePI(daBiaoBiJiaoPIDictionary, drText);

                #endregion

                #region 市区AQI均值
                DataRow[] drsNowAvgValueShiQu = dtNowAvgValueQuXian
                            .Select(string.Format("MonitoringRegionUid='{0}'", "7e05b94c-bbd4-45c3-919c-42da2e63fd43"));
                if (drsNowAvgValueShiQu.Length > 0)
                {
                    DataRow drNowAvgValue = drsNowAvgValueShiQu[0];

                    for (int j = 0; j < 8; j++)
                    {
                        string columnName = dtNowAvgValueQuXian.Columns[j].ColumnName;
                        string pollutantCode = GetPollutantCodeByName(columnName);//根据因子名称获取因子Code
                        int num = 24;

                        if (columnName == "MonitoringRegionUid")
                        {
                            continue;
                        }
                        if (columnName == "Max8HourO3")
                        {
                            num = 8;
                        }
                        decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(drNowAvgValue[j]), 4);
                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(pollutantCode, Con, num), 0);
                        if (AQIAvgShiQu < temp)
                        {
                            AQIAvgShiQu = temp;
                        }
                    }
                    drText["M102_AQIAvg"] = AQIAvgShiQu.ToString("0");
                }
                #endregion

                #region 市区AQI范围
                DataRow[] drsNowMinValueShiQu = dtNowMinValueQuXian
                            .Select(string.Format("MonitoringRegionUid='{0}'", "7e05b94c-bbd4-45c3-919c-42da2e63fd43"));
                DataRow[] drsNowMaxValueShiQu = dtNowMaxValueQuXian
                            .Select(string.Format("MonitoringRegionUid='{0}'", "7e05b94c-bbd4-45c3-919c-42da2e63fd43"));
                if (drsNowMinValueShiQu.Length > 0)
                {
                    DataRow drNowMinValue = drsNowMinValueShiQu[0];
                    AQIMinShiqu = string.Format("{0:0}", drNowMinValue["AQIValue"]);
                }
                if (drsNowMaxValueShiQu.Length > 0)
                {
                    DataRow drNowMaxValue = drsNowMaxValueShiQu[0];
                    AQIMaxShiqu = string.Format("{0:0}", drNowMaxValue["AQIValue"]);
                }
                drText["M102_AQIRange"] = AQIMinShiqu + "～" + AQIMaxShiqu;
                #endregion

                #region 苏州市区单月
                DataRow[] drsNowMonth = dtNowMonth.Select(string.Format("MonitoringRegionUid='{0}'", "7e05b94c-bbd4-45c3-919c-42da2e63fd43"));

                if (drsNowMonth.Length > 0)
                {
                    DataRow drNowMonth = drsNowMonth[0];
                    decimal PM25_C = DecimalExtension.GetRoundValue((decimal.TryParse(drNowMonth["a34004"].ToString(), out PM25_C) ? PM25_C : 0) * 1000, 1);
                    decimal PM10_C = DecimalExtension.GetRoundValue((decimal.TryParse(drNowMonth["a34002"].ToString(), out PM10_C) ? PM10_C : 0) * 1000, 0);
                    decimal NO2_C = DecimalExtension.GetRoundValue((decimal.TryParse(drNowMonth["a21004"].ToString(), out NO2_C) ? NO2_C : 0) * 1000, 0);
                    decimal SO2_C = DecimalExtension.GetRoundValue((decimal.TryParse(drNowMonth["a21026"].ToString(), out SO2_C) ? SO2_C : 0) * 1000, 0);
                    decimal CO_C = DecimalExtension.GetRoundValue((decimal.TryParse(drNowMonth["a21005"].ToString(), out CO_C) ? CO_C : 0), 2);
                    decimal O3_C = DecimalExtension.GetRoundValue((decimal.TryParse(drNowMonth["a05024"].ToString(), out O3_C) ? O3_C : 0) * 1000, 0);
                    decimal standardRate = 0;
                    decimal validCount = decimal.TryParse(drNowMonth["ValidCount"].ToString(), out validCount) ? validCount : 0;//有效天数
                    decimal optimal = decimal.TryParse(drNowMonth["Optimal"].ToString(), out optimal) ? optimal : 0;//优的天数
                    decimal benign = decimal.TryParse(drNowMonth["benign"].ToString(), out benign) ? benign : 0;//良的天数
                    decimal lightPollution = decimal.TryParse(drNowMonth["LightPollution"].ToString(), out lightPollution) ? lightPollution : 0;//轻度污染的天数
                    decimal moderatePollution = decimal.TryParse(drNowMonth["ModeratePollution"].ToString(), out moderatePollution) ? moderatePollution : 0;//中度污染的天数
                    decimal highPollution = decimal.TryParse(drNowMonth["HighPollution"].ToString(), out highPollution) ? highPollution : 0;//重度污染的天数
                    decimal seriousPollution = decimal.TryParse(drNowMonth["SeriousPollution"].ToString(), out seriousPollution) ? seriousPollution : 0;//严重污染的天数
                    decimal PM25_OverCount = decimal.TryParse(drNowMonth["PM25_OverCount"].ToString(), out PM25_OverCount) ? PM25_OverCount : 0;//超标天数
                    decimal PM10_OverCount = decimal.TryParse(drNowMonth["PM10_OverCount"].ToString(), out PM10_OverCount) ? PM10_OverCount : 0;//超标天数
                    decimal O3_OverCount = decimal.TryParse(drNowMonth["O3_OverCount"].ToString(), out O3_OverCount) ? O3_OverCount : 0;//超标天数
                    decimal SO2_OverCount = decimal.TryParse(drNowMonth["SO2_OverCount"].ToString(), out SO2_OverCount) ? SO2_OverCount : 0;//超标天数
                    decimal NO2_OverCount = decimal.TryParse(drNowMonth["NO2_OverCount"].ToString(), out NO2_OverCount) ? NO2_OverCount : 0;//超标天数
                    decimal CO_OverCount = decimal.TryParse(drNowMonth["CO_OverCount"].ToString(), out CO_OverCount) ? CO_OverCount : 0;//超标天数
                    int PM25_PriDays = int.TryParse(drNowMonth["PM25"].ToString(), out PM25_PriDays) ? PM25_PriDays : 0;
                    int PM10_PriDays = int.TryParse(drNowMonth["PM10"].ToString(), out PM10_PriDays) ? PM10_PriDays : 0;
                    int NO2_PriDays = int.TryParse(drNowMonth["NO2"].ToString(), out NO2_PriDays) ? NO2_PriDays : 0;
                    int SO2_PriDays = int.TryParse(drNowMonth["SO2"].ToString(), out SO2_PriDays) ? SO2_PriDays : 0;
                    int CO_PriDays = int.TryParse(drNowMonth["CO"].ToString(), out CO_PriDays) ? CO_PriDays : 0;
                    int O3_PriDays = int.TryParse(drNowMonth["O3"].ToString(), out O3_PriDays) ? O3_PriDays : 0;
                    IList<string> overDaysOne = new List<string>();
                    IList<string> overDaysTwo = new List<string>();
                    int overDays = 0;
                    Dictionary<string, int> primaryDaysDictionary = new Dictionary<string, int>();//首要污染物天数列表
                    Dictionary<string, decimal> factorBiDictionary = new Dictionary<string, decimal>();//浓度列表
                    factorBiDictionary.Add("PM2.5", PM25_C);
                    factorBiDictionary.Add("PM10", PM10_C);
                    factorBiDictionary.Add("NO2", NO2_C);
                    factorBiDictionary.Add("SO2", SO2_C);
                    factorBiDictionary.Add("CO", CO_C);
                    factorBiDictionary.Add("O3日最大8小时", O3_C);

                    //市区浓度均值
                    drText["M202_PM25ShiQu"] = DecimalExtension.GetRoundValue(PM25_C, 1).ToString();
                    drText["M203_PM10ShiQu"] = DecimalExtension.GetRoundValue(PM10_C, 0).ToString();
                    drText["M204_NO2ShiQu"] = DecimalExtension.GetRoundValue(NO2_C, 0).ToString();
                    drText["M204_SO2ShiQu"] = DecimalExtension.GetRoundValue(SO2_C, 0).ToString();
                    drText["M204_COShiQu"] = DecimalExtension.GetRoundValue(CO_C, 2).ToString();
                    drText["M204_O38ShiQu"] = DecimalExtension.GetRoundValue(O3_C, 0).ToString();

                    //达标率
                    if (drNowMonth["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drNowMonth["ValidCount"]) != 0)
                    {
                        validCount = Convert.ToDecimal(drNowMonth["ValidCount"]);
                        if (drNowMonth["StandardCount"].IsNotNullOrDBNull())
                        {
                            standardRate = DecimalExtension.GetRoundValue(Convert.ToDecimal(drNowMonth["StandardCount"])
                                            / Convert.ToDecimal(drNowMonth["ValidCount"]) * 100, 1);   //市区达标率
                            drText["M102_DaBiaoRate"] = standardRate.ToString("0.0") + "%";
                        }
                    }

                    #region 添加市区当前月有超标的污染列表
                    if (PM25_OverCount == 0)
                    {
                        nowUnOverPollutantSQList.Add("PM2.5");
                    }
                    if (PM10_OverCount == 0)
                    {
                        nowUnOverPollutantSQList.Add("PM10");
                    }
                    if (O3_OverCount == 0)
                    {
                        nowUnOverPollutantSQList.Add("O3日最大8小时");
                    }
                    if (SO2_OverCount == 0)
                    {
                        nowUnOverPollutantSQList.Add("SO2");
                    }
                    if (NO2_OverCount == 0)
                    {
                        nowUnOverPollutantSQList.Add("NO2");
                    }
                    if (CO_OverCount == 0)
                    {
                        nowUnOverPollutantSQList.Add("CO");
                    }
                    #endregion

                    #region 上一年月
                    DataRow[] drsLastYear = dtLastYear.Select(string.Format("MonitoringRegionUid='{0}'", "7e05b94c-bbd4-45c3-919c-42da2e63fd43"));

                    if (drsLastYear.Length > 0)
                    {
                        DataRow drLastYear = drsLastYear[0];
                        decimal PM25_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(drLastYear["a34004"].ToString(), out PM25_CLY) ? PM25_CLY : 0) * 1000, 1);
                        decimal PM10_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(drLastYear["a34002"].ToString(), out PM10_CLY) ? PM10_CLY : 0) * 1000, 0);
                        decimal NO2_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(drLastYear["a21004"].ToString(), out NO2_CLY) ? NO2_CLY : 0) * 1000, 0);
                        decimal SO2_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(drLastYear["a21026"].ToString(), out SO2_CLY) ? SO2_CLY : 0) * 1000, 0);
                        decimal CO_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(drLastYear["a21005"].ToString(), out CO_CLY) ? CO_CLY : 0), 2);
                        decimal O3_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(drLastYear["a05024"].ToString(), out O3_CLY) ? O3_CLY : 0) * 1000, 0);
                        decimal PM25Rate;
                        Dictionary<string, decimal> factorBiLYDictionary = new Dictionary<string, decimal>();//浓度列表
                        factorBiLYDictionary.Add("PM2.5", PM25_CLY);
                        factorBiLYDictionary.Add("PM10", PM10_CLY);
                        factorBiLYDictionary.Add("NO2", NO2_CLY);
                        factorBiLYDictionary.Add("SO2", SO2_CLY);
                        factorBiLYDictionary.Add("CO", CO_CLY);
                        factorBiLYDictionary.Add("O3日最大8小时", O3_CLY);

                        #region PM2.5浓度比较
                        //if (PM25_CLY > 0)
                        //{
                        //    PM25Rate = DecimalExtension.GetRoundValue((PM25_C - PM25_CLY) / PM25_CLY * 100, 1);
                        //    if (PM25Rate > 0)
                        //    {
                        //        drText["Important1_LastBi"] = "相比上升了" + PM25Rate.ToString("0.0") + "%";
                        //    }
                        //    else if (PM25Rate == 0)
                        //    {
                        //        drText["Important1_LastBi"] = "持平";
                        //    }
                        //    else
                        //    {
                        //        drText["Important1_LastBi"] = "相比下降了" + (-PM25Rate).ToString("0.0") + "%";
                        //    }
                        //}
                        #endregion

                        #region 达标率比较
                        if (drLastYear["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drLastYear["ValidCount"]) != 0)
                        {
                            if (drLastYear["StandardCount"].IsNotNullOrDBNull())
                            {
                                decimal standardRateL = DecimalExtension.GetRoundValue(Convert.ToDecimal(drLastYear["StandardCount"])
                                                 / Convert.ToDecimal(drLastYear["ValidCount"]) * 100, 1);   //市区达标率

                                //if (standardRateL > 0)
                                //{
                                //    decimal lastRate = DecimalExtension.GetRoundValue((standardRate - standardRateL) / standardRateL * 100, 1);
                                decimal lastRate = standardRate - standardRateL;

                                if (lastRate > 0)
                                {
                                    drText["M103_LastToNowDaRateShiQu"] = "由" + standardRateL.ToString("0.0") + "%"
                                               + "上升为" + standardRate.ToString("0.0") + "%";
                                }
                                else if (lastRate == 0)
                                {
                                    drText["M103_LastToNowDaRateShiQu"] = "持平";
                                }
                                else
                                {
                                    drText["M103_LastToNowDaRateShiQu"] = "由" + standardRateL.ToString("0.0") + "%"
                                                + "下降为" + standardRate.ToString("0.0") + "%";
                                }
                                //}
                            }
                        }
                        #endregion

                        //比较污染物浓度比例
                        BiJiaoPollutantRate(factorBiDictionary, factorBiLYDictionary, "与上年" + monthNow + "月相比", drText);
                    }
                    #endregion

                    #region 基数年月
                    DataRow[] drsJiShu = dtJiShu.Select(string.Format("MonitoringRegionUid='{0}'", "7e05b94c-bbd4-45c3-919c-42da2e63fd43"));

                    if (drsJiShu.Length > 0)
                    {
                        DataRow drJiShu = drsJiShu[0];
                        decimal PM25_CLMJS = (decimal.TryParse(drJiShu["a34004"].ToString(), out PM25_CLMJS) ? PM25_CLMJS : 0) * 1000;
                        decimal PM25Rate;

                        #region PM2.5浓度比较
                        //if (PM25_CLMJS > 0)
                        //{
                        //    PM25Rate = DecimalExtension.GetRoundValue((PM25_C - PM25_CLMJS) / PM25_CLMJS * 100, 1);
                        //    if (PM25Rate > 0)
                        //    {
                        //        drText["Important1_JiShuBi"] = "相比上升了" + PM25Rate.ToString("0.0") + "%";
                        //    }
                        //    else if (PM25Rate == 0)
                        //    {
                        //        drText["Important1_JiShuBi"] = "持平";
                        //    }
                        //    else
                        //    {
                        //        drText["Important1_JiShuBi"] = "相比下降了" + (-PM25Rate).ToString("0.0") + "%";
                        //    }
                        //}
                        #endregion
                    }
                    #endregion

                    #region 上一个月
                    DataRow[] drsLastMonth = dtLastMonth.Select(string.Format("MonitoringRegionUid='{0}'", "7e05b94c-bbd4-45c3-919c-42da2e63fd43"));

                    if (drsLastMonth.Length > 0)
                    {
                        DataRow drLastMonth = drsLastMonth[0];
                        decimal PM25_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(drLastMonth["a34004"].ToString(), out PM25_CLM) ? PM25_CLM : 0) * 1000, 1);
                        decimal PM10_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(drLastMonth["a34002"].ToString(), out PM10_CLM) ? PM10_CLM : 0) * 1000, 0);
                        decimal NO2_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(drLastMonth["a21004"].ToString(), out NO2_CLM) ? NO2_CLM : 0) * 1000, 0);
                        decimal SO2_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(drLastMonth["a21026"].ToString(), out SO2_CLM) ? SO2_CLM : 0) * 1000, 0);
                        decimal CO_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(drLastMonth["a21005"].ToString(), out CO_CLM) ? CO_CLM : 0), 2);
                        decimal O3_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(drLastMonth["a05024"].ToString(), out O3_CLM) ? O3_CLM : 0) * 1000, 0);
                        Dictionary<string, decimal> factorBiLMDictionary = new Dictionary<string, decimal>();//浓度列表
                        factorBiLMDictionary.Add("PM2.5", PM25_CLM);
                        factorBiLMDictionary.Add("PM10", PM10_CLM);
                        factorBiLMDictionary.Add("NO2", NO2_CLM);
                        factorBiLMDictionary.Add("SO2", SO2_CLM);
                        factorBiLMDictionary.Add("CO", CO_CLM);
                        factorBiLMDictionary.Add("O3日最大8小时", O3_CLM);

                        //比较污染物浓度比例
                        BiJiaoPollutantRate(factorBiDictionary, factorBiLMDictionary, "与上月相比", drText);
                    }
                    #endregion

                    #region 首要污染物最多和第二多的因子
                    if (PM25_PriDays > 0)
                        primaryDaysDictionary.Add("PM2.5", Convert.ToInt32(PM25_PriDays));
                    if (PM10_PriDays > 0)
                        primaryDaysDictionary.Add("PM10", Convert.ToInt32(PM10_PriDays));
                    if (NO2_PriDays > 0)
                        primaryDaysDictionary.Add("NO2", Convert.ToInt32(NO2_PriDays));
                    if (SO2_PriDays > 0)
                        primaryDaysDictionary.Add("SO2", Convert.ToInt32(SO2_PriDays));
                    if (CO_PriDays > 0)
                        primaryDaysDictionary.Add("CO", Convert.ToInt32(CO_PriDays));
                    if (O3_PriDays > 0)
                        primaryDaysDictionary.Add("O3日最大8小时", Convert.ToInt32(O3_PriDays));
                    overDaysOne = primaryDaysDictionary.Where(t => t.Value == primaryDaysDictionary.Values.Max()).Select(t => t.Key).ToList();
                    foreach (KeyValuePair<string, int> overDayKeyValue in primaryDaysDictionary)
                    {
                        if (!overDaysOne.Contains(overDayKeyValue.Key))
                        {
                            int overDay = overDayKeyValue.Value;
                            if (overDays < overDay)
                            {
                                overDays = overDay;
                            }
                        }
                    }
                    overDaysTwo = primaryDaysDictionary.Where(t => t.Value == overDays).Select(t => t.Key).ToList();
                    if (overDaysOne.Count > 0)
                        drText["M102_ChaoFactorOne"] = overDaysOne.Aggregate((a, b) => a + "、" + b);
                    if (overDaysTwo.Count > 0)
                        drText["M102_ChaoFactorTwo"] = "，其次为" + overDaysTwo.Aggregate((a, b) => a + "、" + b) + "。";
                    else
                        drText["M102_ChaoFactorTwo"] = "。";
                    #endregion

                    #region 图2 XXXX年XX月苏州市区环境空气质量类别分布
                    if (validCount > 0)
                    {
                        DataRow drTu2 = dtTu2.NewRow();
                        drTu2["优"] = DecimalExtension.GetRoundValue(optimal / validCount * 100, 1);
                        drTu2["良"] = DecimalExtension.GetRoundValue(benign / validCount * 100, 1);
                        drTu2["轻度污染"] = DecimalExtension.GetRoundValue(lightPollution / validCount * 100, 1);
                        drTu2["中度污染"] = DecimalExtension.GetRoundValue(moderatePollution / validCount * 100, 1);
                        drTu2["重度污染"] = DecimalExtension.GetRoundValue(highPollution / validCount * 100, 1);
                        drTu2["严重污染"] = DecimalExtension.GetRoundValue(seriousPollution / validCount * 100, 1);
                        dtTu2.Rows.Add(drTu2);
                    }
                    #endregion
                }
                #endregion

                #region 苏州全市单月，用区域数据计算全市数据
                if (dtNowMonth.Rows.Count > 0)
                {
                    //市区和区县的数据
                    DataTable dtNowMonthQuanShi = dtNowMonth.Select(string.Format("MonitoringRegionUid='{0}'", "5a566145-4884-453c-93ad-16e4344c85c9")).CopyToDataTable();
                    DataTable dtNowMonthQuXian = dtNowMonth.AsEnumerable()
                        .Where(t => regionUidQuXianList.Contains(t.Field<string>("MonitoringRegionUid"))).CopyToDataTable();

                    //所有区县值算全市平均值
                    decimal PM25_CQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtNowMonthQuanShi.Rows[0]["a34004"].ToString(), out PM25_CQS) ? PM25_CQS : 0) * 1000, 1);
                    decimal PM10_CQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtNowMonthQuanShi.Rows[0]["a34002"].ToString(), out PM10_CQS) ? PM10_CQS : 0) * 1000, 0);
                    decimal NO2_CQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtNowMonthQuanShi.Rows[0]["a21004"].ToString(), out NO2_CQS) ? NO2_CQS : 0) * 1000, 0);
                    decimal SO2_CQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtNowMonthQuanShi.Rows[0]["a21026"].ToString(), out SO2_CQS) ? SO2_CQS : 0) * 1000, 0);
                    decimal CO_CQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtNowMonthQuanShi.Rows[0]["a21005"].ToString(), out CO_CQS) ? CO_CQS : 0), 2);
                    decimal O3_CQS = DecimalExtension.GetRoundValue((decimal.TryParse(dtNowMonthQuanShi.Rows[0]["a05024"].ToString(), out O3_CQS) ? O3_CQS : 0) * 1000, 0);
                    int PM25_PriDays = int.TryParse(dtNowMonthQuXian.Compute("SUM(PM25)", "").ToString(), out PM25_PriDays) ? PM25_PriDays : 0;
                    int PM10_PriDays = int.TryParse(dtNowMonthQuXian.Compute("SUM(PM10)", "").ToString(), out PM10_PriDays) ? PM10_PriDays : 0;
                    int NO2_PriDays = int.TryParse(dtNowMonthQuXian.Compute("SUM(NO2)", "").ToString(), out NO2_PriDays) ? NO2_PriDays : 0;
                    int SO2_PriDays = int.TryParse(dtNowMonthQuXian.Compute("SUM(SO2)", "").ToString(), out SO2_PriDays) ? SO2_PriDays : 0;
                    int CO_PriDays = int.TryParse(dtNowMonthQuXian.Compute("SUM(CO)", "").ToString(), out CO_PriDays) ? CO_PriDays : 0;
                    int O3_PriDays = int.TryParse(dtNowMonthQuXian.Compute("SUM(O3)", "").ToString(), out O3_PriDays) ? O3_PriDays : 0;
                    decimal standardRate = 0;//达标天数比例
                    decimal standardCountQS = 0;//全市达标天数
                    decimal validCountQS = 0;//全市有效天数
                    decimal standardCountLYQS = 0;//全市上一年月达标天数
                    decimal validCountLYQS = 0;//全市上一年月有效天数
                    decimal optimalQS = 0;//优的天数
                    decimal benignQS = 0;//良的天数
                    decimal lightPollutionQS = 0;//轻度污染的天数
                    decimal moderatePollutionQS = 0;//中度污染的天数
                    decimal highPollutionQS = 0;//重度污染的天数
                    decimal seriousPollutionQS = 0;//严重污染的天数
                    Dictionary<string, int> overDaysQSDictionary = new Dictionary<string, int>();//全市因子超标天数列表
                    Dictionary<string, decimal> factorBiDictionary = new Dictionary<string, decimal>();//污染物浓度列表
                    factorBiDictionary.Add("PM2.5", PM25_CQS);
                    factorBiDictionary.Add("PM10", PM10_CQS);
                    factorBiDictionary.Add("NO2", NO2_CQS);
                    factorBiDictionary.Add("SO2", SO2_CQS);
                    factorBiDictionary.Add("CO", CO_CQS);
                    factorBiDictionary.Add("O3日最大8小时", O3_CQS);

                    //计算全市因子浓度范围
                    ComputePollutantValueRange(dtNowMonthQuXian, drText);

                    #region 添加全市当前月有超标的污染列表
                    decimal PM25_OverCountNow = 0;
                    decimal PM10_OverCountNow = 0;
                    decimal O3_OverCountNow = 0;
                    decimal SO2_OverCountNow = 0;
                    decimal NO2_OverCountNow = 0;
                    decimal CO_OverCountNow = 0;

                    foreach (DataRow drNowMonthQuXian in dtNowMonthQuXian.Rows)
                    {
                        if (PM25_OverCountNow == 0)
                        {
                            PM25_OverCountNow = decimal.TryParse(drNowMonthQuXian["PM25_OverCount"].ToString(), out PM25_OverCountNow) ? PM25_OverCountNow : 0;//超标天数
                        }
                        if (PM10_OverCountNow == 0)
                        {
                            PM10_OverCountNow = decimal.TryParse(drNowMonthQuXian["PM10_OverCount"].ToString(), out PM10_OverCountNow) ? PM10_OverCountNow : 0;//超标天数
                        }
                        if (O3_OverCountNow == 0)
                        {
                            O3_OverCountNow = decimal.TryParse(drNowMonthQuXian["O3_OverCount"].ToString(), out O3_OverCountNow) ? O3_OverCountNow : 0;//超标天数
                        }
                        if (SO2_OverCountNow == 0)
                        {
                            SO2_OverCountNow = decimal.TryParse(drNowMonthQuXian["SO2_OverCount"].ToString(), out SO2_OverCountNow) ? SO2_OverCountNow : 0;//超标天数
                        }
                        if (NO2_OverCountNow == 0)
                        {
                            NO2_OverCountNow = decimal.TryParse(drNowMonthQuXian["NO2_OverCount"].ToString(), out NO2_OverCountNow) ? NO2_OverCountNow : 0;//超标天数
                        }
                        if (CO_OverCountNow == 0)
                        {
                            CO_OverCountNow = decimal.TryParse(drNowMonthQuXian["CO_OverCount"].ToString(), out CO_OverCountNow) ? CO_OverCountNow : 0;//超标天数
                        }
                    }
                    if (PM25_OverCountNow == 0)
                    {
                        nowUnOverPollutantQSList.Add("PM2.5");
                    }
                    if (PM10_OverCountNow == 0)
                    {
                        nowUnOverPollutantQSList.Add("PM10");
                    }
                    if (O3_OverCountNow == 0)
                    {
                        nowUnOverPollutantQSList.Add("O3日最大8小时");
                    }
                    if (SO2_OverCountNow == 0)
                    {
                        nowUnOverPollutantQSList.Add("SO2");
                    }
                    if (NO2_OverCountNow == 0)
                    {
                        nowUnOverPollutantQSList.Add("NO2");
                    }
                    if (CO_OverCountNow == 0)
                    {
                        nowUnOverPollutantQSList.Add("CO");
                    }
                    #endregion

                    #region 全市达标率以及首要污染物最多和第二多的因子
                    if (dtNowMonth.Rows.Count > 0)
                    {

                        Dictionary<string, int> primaryDaysQSDictionary = new Dictionary<string, int>();//全市首要污染物天数列表
                        IList<string> primaryDaysOne = new List<string>();
                        IList<string> primaryDaysTwo = new List<string>();
                        int overDays = 0;
                        #region 首要污染物最多和第二多的因子
                        if (PM25_PriDays > 0)
                            primaryDaysQSDictionary.Add("PM2.5", Convert.ToInt32(PM25_PriDays));
                        if (PM10_PriDays > 0)
                            primaryDaysQSDictionary.Add("PM10", Convert.ToInt32(PM10_PriDays));
                        if (NO2_PriDays > 0)
                            primaryDaysQSDictionary.Add("NO2", Convert.ToInt32(NO2_PriDays));
                        if (SO2_PriDays > 0)
                            primaryDaysQSDictionary.Add("SO2", Convert.ToInt32(SO2_PriDays));
                        if (CO_PriDays > 0)
                            primaryDaysQSDictionary.Add("CO", Convert.ToInt32(CO_PriDays));
                        if (O3_PriDays > 0)
                            primaryDaysQSDictionary.Add("O3日最大8小时", Convert.ToInt32(O3_PriDays));
                        #endregion
                        //}


                        for (int i = 0; i < dtNowQuanShi.Rows.Count; i++)
                        {
                            decimal AQIcount = 0;
                            string primaryPollutantName = string.Empty;

                            for (int j = 1; j < 7; j++)
                            {
                                string columnName = dtNowQuanShi.Columns[j].ColumnName;
                                string pollutantName = GetPollutantNameByCode(columnName);//根据因子Code获取因子名称
                                int num = 24;

                                if (columnName == "a05024")
                                {
                                    num = 8;
                                }

                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtNowQuanShi.Rows[i][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(columnName, Con, num), 0);

                                if (AQIcount < temp)
                                {
                                    AQIcount = temp;
                                    primaryPollutantName = pollutantName;
                                }

                                //因子超标天数
                                if (temp > 100)
                                {
                                    if (overDaysQSDictionary.ContainsKey(pollutantName))
                                    {
                                        int primaryDays = overDaysQSDictionary[pollutantName];
                                        primaryDays += 1;
                                        overDaysQSDictionary[pollutantName] = primaryDays;
                                    }
                                    else
                                    {
                                        overDaysQSDictionary.Add(pollutantName, 1);
                                    }
                                }
                            }

                            #region 各类天数
                            if (AQIcount > 0)
                            {
                                validCountQS++;
                                if (AQIcount <= 100)
                                {
                                    standardCountQS++;
                                }

                                //各类别污染天数
                                if (AQIcount > 0 && AQIcount <= 50)
                                {
                                    optimalQS++;//优的天数
                                }
                                else if (AQIcount >= 51 && AQIcount <= 100)
                                {
                                    benignQS++;//良的天数
                                }
                                else if (AQIcount >= 101 && AQIcount <= 150)
                                {
                                    lightPollutionQS++;//轻度污染的天数
                                }
                                else if (AQIcount >= 151 && AQIcount <= 200)
                                {
                                    moderatePollutionQS++; ;//中度污染的天数
                                }
                                else if (AQIcount >= 201 && AQIcount <= 300)
                                {
                                    highPollutionQS++; ;//重度污染的天数
                                }
                                else if (AQIcount >= 301)
                                {
                                    seriousPollutionQS++; ;//严重污染的天数
                                }
                            }
                            #endregion
                        }
                        if (validCountQS != 0)
                        {
                            standardRate = DecimalExtension.GetRoundValue(standardCountQS / validCountQS * 100, 1);
                            drText["M101_DaBiaoRateQuanShi"] = standardRate + "%";//全市达标率

                            #region 图1 XXXX年XX月全市环境空气质量类别分布
                            DataRow drTu1 = dtTu1.NewRow();
                            drTu1["优"] = DecimalExtension.GetRoundValue(optimalQS / validCountQS * 100, 1);
                            drTu1["良"] = DecimalExtension.GetRoundValue(benignQS / validCountQS * 100, 1);
                            drTu1["轻度污染"] = DecimalExtension.GetRoundValue(lightPollutionQS / validCountQS * 100, 1);
                            drTu1["中度污染"] = DecimalExtension.GetRoundValue(moderatePollutionQS / validCountQS * 100, 1);
                            drTu1["重度污染"] = DecimalExtension.GetRoundValue(highPollutionQS / validCountQS * 100, 1);
                            drTu1["严重污染"] = DecimalExtension.GetRoundValue(seriousPollutionQS / validCountQS * 100, 1);
                            dtTu1.Rows.Add(drTu1);
                            #endregion
                        }

                        #region 首要污染物最多和第二多的因子
                        primaryDaysOne = primaryDaysQSDictionary.Where(t => t.Value == primaryDaysQSDictionary.Values.Max()).Select(t => t.Key).ToList();
                        foreach (KeyValuePair<string, int> primaryDaysKeyValue in primaryDaysQSDictionary)
                        {
                            if (!primaryDaysOne.Contains(primaryDaysKeyValue.Key))
                            {
                                int overDay = primaryDaysKeyValue.Value;
                                if (overDays < overDay)
                                {
                                    overDays = overDay;
                                }
                            }
                        }
                        primaryDaysTwo = primaryDaysQSDictionary.Where(t => t.Value == overDays).Select(t => t.Key).ToList();
                        if (primaryDaysOne.Count > 0)
                            drText["M101_ChaoFactorOneQuanShi"] = primaryDaysOne.Aggregate((a, b) => a + "、" + b);
                        if (primaryDaysTwo.Count > 0)
                            drText["M101_ChaoFactorTowQuanShi"] = "，其次为" + primaryDaysTwo.Aggregate((a, b) => a + "、" + b) + "。";
                        else
                            drText["M101_ChaoFactorTowQuanShi"] = "。";
                        #endregion
                    }
                    #endregion

                    #region 全市上一年月
                    if (isLastYQS)
                    {
                        //全市上一年月达标率
                        for (int i = 0; i < dtLastYQuanShi.Rows.Count; i++)
                        {
                            decimal AQIcount = 0;

                            for (int j = 1; j < 7; j++)
                            {
                                string columnName = dtLastYQuanShi.Columns[j].ColumnName;
                                int num = 24;

                                if (columnName == "a05024")
                                {
                                    num = 8;
                                }
                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(dtLastYQuanShi.Rows[i][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(columnName, Con, num), 0);
                                if (AQIcount < temp)
                                {
                                    AQIcount = temp;
                                }
                            }
                            if (AQIcount > 0)
                            {
                                validCountLYQS++;
                                if (AQIcount <= 100)
                                {
                                    standardCountLYQS++;
                                }
                            }
                        }
                        //if (validCountLYQS != 0)
                        //{
                        //    decimal standardRateL = DecimalExtension.GetRoundValue(standardCountLYQS / validCountLYQS * 100, 1);

                        //    //达标率比较
                        //    //if (standardRateL > 0)
                        //    //{
                        //    //decimal lastRate = DecimalExtension.GetRoundValue((standardRate - standardRateL) / standardRateL * 100, 1);
                        //    decimal lastRate = standardRate - standardRateL;

                        //    if (lastRate > 0)
                        //    {
                        //        drText["M103_LastToNowDaRateQuanShi"] = "由" + standardRateL.ToString("0.0") + "%"
                        //                  + "上升为" + standardRate.ToString("0.0") + "%";
                        //    }
                        //    else if (lastRate == 0)
                        //    {
                        //        drText["M103_LastToNowDaRateQuanShi"] = "持平";
                        //    }
                        //    else
                        //    {
                        //        drText["M103_LastToNowDaRateQuanShi"] = "由" + standardRateL.ToString("0.0") + "%"
                        //                    + "下降为" + standardRate.ToString("0.0") + "%";
                        //    }
                        //    //}
                        //}
                    }
                    #endregion

                    #region 表数据
                    foreach (DataRow drNowMonthQuXian in dtNowMonthQuXian.Rows)
                    {
                        string monitoringRegionUid = drNowMonthQuXian["MonitoringRegionUid"].ToString();
                        string regionName = drNowMonthQuXian["regionName"].ToString();
                        DataRow drBiao1 = dtBiao1.NewRow();
                        DataRow drBiao3 = dtBiao3.NewRow();

                        #region 表1  XXXX年XX月苏州市环境空气质量
                        drBiao1["MonitoringRegionUid"] = monitoringRegionUid;//区域GUID 
                        drBiao1["RegionEn"] = GetRegionENByGuid(monitoringRegionUid);//英文名称
                        drBiao1["RegionName"] = regionName;//中文名称
                        drBiao1["DaBiaoDays"] = drNowMonthQuXian["StandardCount"].ToString();//达标天数
                        if (drNowMonthQuXian["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drNowMonthQuXian["ValidCount"]) != 0)
                        {
                            if (drNowMonthQuXian["StandardCount"].IsNotNullOrDBNull())
                            {
                                decimal daBiaoRate = DecimalExtension.GetRoundValue(Convert.ToDecimal(drNowMonthQuXian["StandardCount"])
                                                / Convert.ToDecimal(drNowMonthQuXian["ValidCount"]) * 100, 1);
                                drBiao1["DaBiaoRate"] = daBiaoRate.ToString("0.0") + "%";//达标天数比例
                            }
                        }
                        if (isLastY)
                        {
                            DataRow[] drsLastYear = dtLastYear.Select(string.Format("MonitoringRegionUid='{0}'", monitoringRegionUid));
                            if (drsLastYear.Length > 0)
                            {
                                DataRow drLastYear = drsLastYear[0];

                                if (drLastYear["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drLastYear["ValidCount"]) != 0)
                                {
                                    if (drLastYear["StandardCount"].IsNotNullOrDBNull())
                                    {
                                        decimal daBiaoLastRate = DecimalExtension.GetRoundValue(Convert.ToDecimal(drLastYear["StandardCount"])
                                                         / Convert.ToDecimal(drLastYear["ValidCount"]) * 100, 1);
                                        drBiao1["DaBiaoLastRate"] = daBiaoLastRate.ToString("0.0") + "%";//上年同月达标天数比例
                                    }
                                }

                            }
                        }
                        drBiao1["PM25ChaoDays"] = drNowMonthQuXian["PM25_OverCount"].ToString();//PM2.5超标天数
                        drBiao1["PM10ChaoDays"] = drNowMonthQuXian["PM10_OverCount"].ToString();//PM10超标天数
                        drBiao1["NO2ChaoDays"] = drNowMonthQuXian["NO2_OverCount"].ToString();//NO2超标天数
                        drBiao1["SO2ChaoDays"] = drNowMonthQuXian["SO2_OverCount"].ToString();//SO2超标天数
                        drBiao1["COChaoDays"] = drNowMonthQuXian["CO_OverCount"].ToString();//CO超标天数
                        drBiao1["O38ChaoDays"] = drNowMonthQuXian["O3_OverCount"].ToString();//O3-8小时超标天数
                        dtBiao1.Rows.Add(drBiao1);


                        #endregion

                        #region 表3 XXXX年XX月苏州市各项污染物月均值
                        decimal PM25Biao3 = DecimalExtension.GetRoundValue((decimal.TryParse(drNowMonthQuXian["a34004"].ToString(), out PM25Biao3) ? PM25Biao3 : 0) * 1000, 1);
                        decimal PM10Biao3 = DecimalExtension.GetRoundValue((decimal.TryParse(drNowMonthQuXian["a34002"].ToString(), out PM10Biao3) ? PM10Biao3 : 0) * 1000, 0);
                        decimal NO2Biao3 = DecimalExtension.GetRoundValue((decimal.TryParse(drNowMonthQuXian["a21004"].ToString(), out NO2Biao3) ? NO2Biao3 : 0) * 1000, 0);
                        decimal SO2Biao3 = DecimalExtension.GetRoundValue((decimal.TryParse(drNowMonthQuXian["a21026"].ToString(), out SO2Biao3) ? SO2Biao3 : 0) * 1000, 0);
                        decimal COBiao3 = DecimalExtension.GetRoundValue((decimal.TryParse(drNowMonthQuXian["a21005"].ToString(), out COBiao3) ? COBiao3 : 0), 2);
                        decimal O38Biao3 = DecimalExtension.GetRoundValue((decimal.TryParse(drNowMonthQuXian["a05024"].ToString(), out O38Biao3) ? O38Biao3 : 0) * 1000, 0);
                        drBiao3["MonitoringRegionUid"] = monitoringRegionUid;//区域GUID 
                        drBiao3["RegionEn"] = GetRegionENByGuid(monitoringRegionUid);//英文名称
                        drBiao3["RegionName"] = regionName;//中文名称
                        drBiao3["PM25"] = PM25Biao3.ToString("0.0");//PM2.5浓度
                        drBiao3["PM10"] = PM10Biao3.ToString("0");//PM10浓度
                        drBiao3["NO2"] = NO2Biao3.ToString("0");//NO2浓度
                        drBiao3["SO2"] = SO2Biao3.ToString("0");//SO2浓度
                        drBiao3["CO"] = COBiao3.ToString("0.00");//CO浓度
                        drBiao3["O38"] = O38Biao3.ToString("0");//O38浓度
                        dtBiao3.Rows.Add(drBiao3);
                        #endregion
                    }


                    #region 表1 苏州全市的达标情况
                    DataRow drBiao1QS = dtBiao1.NewRow();
                    drBiao1QS["MonitoringRegionUid"] = "";//区域GUID 
                    drBiao1QS["RegionEn"] = GetRegionENByGuid("");//英文名称
                    drBiao1QS["RegionName"] = "全市";//中文名称
                    drBiao1QS["DaBiaoDays"] = standardCountQS.ToString("0");//达标天数
                    if (validCountQS > 0)
                    {
                        decimal daBiaoRateQS = DecimalExtension.GetRoundValue(standardCountQS / validCountQS * 100, 1);
                        drBiao1QS["DaBiaoRate"] = daBiaoRateQS.ToString("0.0") + "%";//达标天数比例
                    }
                    if (validCountLYQS > 0)
                    {
                        decimal daBiaoLastRate = DecimalExtension.GetRoundValue(standardCountLYQS / validCountLYQS * 100, 1);
                        drBiao1QS["DaBiaoLastRate"] = daBiaoLastRate.ToString("0.0") + "%";//上年同月达标天数比例
                    }
                    if (overDaysQSDictionary.ContainsKey("PM2.5"))
                    {
                        drBiao1QS["PM25ChaoDays"] = overDaysQSDictionary["PM2.5"];//PM2.5超标天数
                    }
                    else
                    {
                        drBiao1QS["PM25ChaoDays"] = 0;
                    }
                    if (overDaysQSDictionary.ContainsKey("PM10"))
                    {
                        drBiao1QS["PM10ChaoDays"] = overDaysQSDictionary["PM10"];//PM10超标天数
                    }
                    else
                    {
                        drBiao1QS["PM10ChaoDays"] = 0;
                    }
                    if (overDaysQSDictionary.ContainsKey("NO2"))
                    {
                        drBiao1QS["NO2ChaoDays"] = overDaysQSDictionary["NO2"];//NO2超标天数
                    }
                    else
                    {
                        drBiao1QS["NO2ChaoDays"] = 0;
                    }
                    if (overDaysQSDictionary.ContainsKey("SO2"))
                    {
                        drBiao1QS["SO2ChaoDays"] = overDaysQSDictionary["SO2"];//SO2超标天数
                    }
                    else
                    {
                        drBiao1QS["SO2ChaoDays"] = 0;
                    }
                    if (overDaysQSDictionary.ContainsKey("CO"))
                    {
                        drBiao1QS["COChaoDays"] = overDaysQSDictionary["CO"];//CO超标天数
                    }
                    else
                    {
                        drBiao1QS["COChaoDays"] = 0;
                    }
                    if (overDaysQSDictionary.ContainsKey("O3日最大8小时"))
                    {
                        drBiao1QS["O38ChaoDays"] = overDaysQSDictionary["O3日最大8小时"];//O3-8小时超标天数
                    }
                    else
                    {
                        drBiao1QS["O38ChaoDays"] = 0;
                    }
                    dtBiao1.Rows.Add(drBiao1QS);
                    #endregion

                    #region 表3 苏州全市的浓度
                    DataRow drBiao3QS = dtBiao3.NewRow();
                    drBiao3QS["MonitoringRegionUid"] = "";
                    drBiao3QS["RegionEn"] = GetRegionENByGuid("");
                    drBiao3QS["RegionName"] = "全市";
                    drBiao3QS["PM25"] = PM25_CQS.ToString();
                    drBiao3QS["PM10"] = PM10_CQS.ToString();
                    drBiao3QS["NO2"] = NO2_CQS.ToString();
                    drBiao3QS["SO2"] = SO2_CQS.ToString();
                    drBiao3QS["CO"] = CO_CQS.ToString();
                    drBiao3QS["O38"] = O3_CQS.ToString();
                    dtBiao3.Rows.Add(drBiao3QS);
                    #endregion
                    #endregion

                    #region 上一年月
                    if (isLastY)
                    {
                        //市区和区县的数据
                        DataTable dtLastYearhNewQuXian = dtLastYear.Select(string.Format("MonitoringRegionUid='{0}'", "5a566145-4884-453c-93ad-16e4344c85c9")).CopyToDataTable();
                        DataTable dtLastYearhQuXian = dtLastYear.AsEnumerable()
                            .Where(t => regionUidQuXianList.Contains(t.Field<string>("MonitoringRegionUid"))).CopyToDataTable();

                        //所有区县值算全市平均值
                        decimal PM25_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastYearhNewQuXian.Rows[0]["a34004"].ToString(), out PM25_CQS) ? PM25_CQS : 0) * 1000, 1);
                        decimal PM10_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastYearhNewQuXian.Rows[0]["a34002"].ToString(), out PM10_CQS) ? PM10_CQS : 0) * 1000, 0);
                        decimal NO2_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastYearhNewQuXian.Rows[0]["a21004"].ToString(), out NO2_CQS) ? NO2_CQS : 0) * 1000, 0);
                        decimal SO2_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastYearhNewQuXian.Rows[0]["a21026"].ToString(), out SO2_CQS) ? SO2_CQS : 0) * 1000, 0);
                        decimal CO_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastYearhNewQuXian.Rows[0]["a21005"].ToString(), out CO_CQS) ? CO_CQS : 0), 2);
                        decimal O3_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastYearhNewQuXian.Rows[0]["a05024"].ToString(), out O3_CQS) ? O3_CQS : 0) * 1000, 0);
                        //decimal PM25_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastYearhQuXian.Compute("AVG(a34004)", "").ToString(), out PM25_CLY) ? PM25_CLY : 0) * 1000, 1);
                        //decimal PM10_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastYearhQuXian.Compute("AVG(a34002)", "").ToString(), out PM10_CLY) ? PM10_CLY : 0) * 1000, 0);
                        //decimal NO2_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastYearhQuXian.Compute("AVG(a21004)", "").ToString(), out NO2_CLY) ? NO2_CLY : 0) * 1000, 0);
                        //decimal SO2_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastYearhQuXian.Compute("AVG(a21026)", "").ToString(), out SO2_CLY) ? SO2_CLY : 0) * 1000, 0);
                        //decimal CO_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastYearhQuXian.Compute("AVG(a21005)", "").ToString(), out CO_CLY) ? CO_CLY : 0), 2);
                        //decimal O3_CLY = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastYearhQuXian.Compute("AVG(a05024)", "").ToString(), out O3_CLY) ? O3_CLY : 0) * 1000, 0);
                        IList<string> upFactorList = new List<string>();
                        IList<string> equalFactorList = new List<string>();
                        IList<string> downFactorList = new List<string>();
                        Dictionary<string, decimal> factorBiLYDictionary = new Dictionary<string, decimal>();
                        factorBiLYDictionary.Add("PM2.5", PM25_CLY);
                        factorBiLYDictionary.Add("PM10", PM10_CLY);
                        factorBiLYDictionary.Add("NO2", NO2_CLY);
                        factorBiLYDictionary.Add("SO2", SO2_CLY);
                        factorBiLYDictionary.Add("CO", CO_CLY);
                        factorBiLYDictionary.Add("O3日最大8小时", O3_CLY);

                        //主要污染物比较
                        BijiaoPollutant(factorBiDictionary, factorBiLYDictionary, "与上年" + monthNow + "月相比", drText, ref upFactorList,
                            ref equalFactorList, ref downFactorList);

                        #region 添加全市上一年月有超标的污染列表
                        decimal PM25_OverCountLY = 0;
                        decimal PM10_OverCountLY = 0;
                        decimal O3_OverCountLY = 0;
                        decimal SO2_OverCountLY = 0;
                        decimal NO2_OverCountLY = 0;
                        decimal CO_OverCountLY = 0;

                        foreach (DataRow drLastYearhQuXian in dtLastYearhQuXian.Rows)
                        {
                            if (PM25_OverCountLY == 0)
                            {
                                PM25_OverCountLY = decimal.TryParse(drLastYearhQuXian["PM25_OverCount"].ToString(), out PM25_OverCountLY) ? PM25_OverCountLY : 0;//超标天数
                            }
                            if (PM10_OverCountLY == 0)
                            {
                                PM10_OverCountLY = decimal.TryParse(drLastYearhQuXian["PM10_OverCount"].ToString(), out PM10_OverCountLY) ? PM10_OverCountLY : 0;//超标天数
                            }
                            if (O3_OverCountLY == 0)
                            {
                                O3_OverCountLY = decimal.TryParse(drLastYearhQuXian["O3_OverCount"].ToString(), out O3_OverCountLY) ? O3_OverCountLY : 0;//超标天数
                            }
                            if (SO2_OverCountLY == 0)
                            {
                                SO2_OverCountLY = decimal.TryParse(drLastYearhQuXian["SO2_OverCount"].ToString(), out SO2_OverCountLY) ? SO2_OverCountLY : 0;//超标天数
                            }
                            if (NO2_OverCountLY == 0)
                            {
                                NO2_OverCountLY = decimal.TryParse(drLastYearhQuXian["NO2_OverCount"].ToString(), out NO2_OverCountLY) ? NO2_OverCountLY : 0;//超标天数
                            }
                            if (CO_OverCountLY == 0)
                            {
                                CO_OverCountLY = decimal.TryParse(drLastYearhQuXian["CO_OverCount"].ToString(), out CO_OverCountLY) ? CO_OverCountLY : 0;//超标天数
                            }
                        }
                        if (PM25_OverCountLY == 0)
                        {
                            nowUnOverPollutantLYQSList.Add("PM2.5");
                        }
                        if (PM10_OverCountLY == 0)
                        {
                            nowUnOverPollutantLYQSList.Add("PM10");
                        }
                        if (O3_OverCountLY == 0)
                        {
                            nowUnOverPollutantLYQSList.Add("O3日最大8小时");
                        }
                        if (SO2_OverCountLY == 0)
                        {
                            nowUnOverPollutantLYQSList.Add("SO2");
                        }
                        if (NO2_OverCountLY == 0)
                        {
                            nowUnOverPollutantLYQSList.Add("NO2");
                        }
                        if (CO_OverCountLY == 0)
                        {
                            nowUnOverPollutantLYQSList.Add("CO");
                        }
                        #endregion
                    }
                    #endregion

                    #region 上一个月
                    if (isLastM)
                    {
                        //市区和区县的数据
                        DataTable dtLastMonthQuXian = dtLastMonth.AsEnumerable()
                            .Where(t => regionUidQuXianList.Contains(t.Field<string>("MonitoringRegionUid"))).CopyToDataTable();

                        DataTable dtLastMonthQuXianQuanShi = dtLastMonth.Select(string.Format("MonitoringRegionUid='{0}'", "5a566145-4884-453c-93ad-16e4344c85c9")).CopyToDataTable();
                        //所有区县值算全市平均值
                        decimal PM25_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastMonthQuXianQuanShi.Rows[0]["a34004"].ToString(), out PM25_CQS) ? PM25_CQS : 0) * 1000, 1);
                        decimal PM10_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastMonthQuXianQuanShi.Rows[0]["a34002"].ToString(), out PM10_CQS) ? PM10_CQS : 0) * 1000, 0);
                        decimal NO2_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastMonthQuXianQuanShi.Rows[0]["a21004"].ToString(), out NO2_CQS) ? NO2_CQS : 0) * 1000, 0);
                        decimal SO2_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastMonthQuXianQuanShi.Rows[0]["a21026"].ToString(), out SO2_CQS) ? SO2_CQS : 0) * 1000, 0);
                        decimal CO_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastMonthQuXianQuanShi.Rows[0]["a21005"].ToString(), out CO_CQS) ? CO_CQS : 0), 2);
                        decimal O3_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastMonthQuXianQuanShi.Rows[0]["a05024"].ToString(), out O3_CQS) ? O3_CQS : 0) * 1000, 0);

                        //decimal PM25_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastMonthQuXian.Rows[0]["a34004"].ToString(), out PM25_CLM) ? PM25_CLM : 0) * 1000, 1);
                        //decimal PM10_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastMonthQuXian.Rows[0]["a34002"].ToString(), out PM10_CLM) ? PM10_CLM : 0) * 1000, 0);
                        //decimal NO2_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastMonthQuXian.Rows[0]["a21004"].ToString(), out NO2_CLM) ? NO2_CLM : 0) * 1000, 0);
                        //decimal SO2_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastMonthQuXian.Rows[0]["a21026"].ToString(), out SO2_CLM) ? SO2_CLM : 0) * 1000, 0);
                        //decimal CO_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastMonthQuXian.Rows[0]["a21005"].ToString(), out CO_CLM) ? CO_CLM : 0), 2);
                        //decimal O3_CLM = DecimalExtension.GetRoundValue((decimal.TryParse(dtLastMonthQuXian.Rows[0]["a05024"].ToString(), out O3_CLM) ? O3_CLM : 0) * 1000, 0);
                        IList<string> upFactorList = new List<string>();
                        IList<string> equalFactorList = new List<string>();
                        IList<string> downFactorList = new List<string>();
                        Dictionary<string, decimal> factorBiLMDictionary = new Dictionary<string, decimal>();
                        factorBiLMDictionary.Add("PM2.5", PM25_CLM);
                        factorBiLMDictionary.Add("PM10", PM10_CLM);
                        factorBiLMDictionary.Add("NO2", NO2_CLM);
                        factorBiLMDictionary.Add("SO2", SO2_CLM);
                        factorBiLMDictionary.Add("CO", CO_CLM);
                        factorBiLMDictionary.Add("O3日最大8小时", O3_CLM);

                        //主要污染物比较
                        BijiaoPollutant(factorBiDictionary, factorBiLMDictionary, "与上月相比", drText, ref upFactorList,
                            ref equalFactorList, ref downFactorList);

                        #region 添加全市上个年月有超标的污染列表
                        decimal PM25_OverCountLM = 0;
                        decimal PM10_OverCountLM = 0;
                        decimal O3_OverCountLM = 0;
                        decimal SO2_OverCountLM = 0;
                        decimal NO2_OverCountLM = 0;
                        decimal CO_OverCountLM = 0;

                        foreach (DataRow drLastMonthQuXian in dtLastMonthQuXian.Rows)
                        {
                            if (PM25_OverCountLM == 0)
                            {
                                PM25_OverCountLM = decimal.TryParse(drLastMonthQuXian["PM25_OverCount"].ToString(), out PM25_OverCountLM) ? PM25_OverCountLM : 0;//超标天数
                            }
                            if (PM10_OverCountLM == 0)
                            {
                                PM10_OverCountLM = decimal.TryParse(drLastMonthQuXian["PM10_OverCount"].ToString(), out PM10_OverCountLM) ? PM10_OverCountLM : 0;//超标天数
                            }
                            if (O3_OverCountLM == 0)
                            {
                                O3_OverCountLM = decimal.TryParse(drLastMonthQuXian["O3_OverCount"].ToString(), out O3_OverCountLM) ? O3_OverCountLM : 0;//超标天数
                            }
                            if (SO2_OverCountLM == 0)
                            {
                                SO2_OverCountLM = decimal.TryParse(drLastMonthQuXian["SO2_OverCount"].ToString(), out SO2_OverCountLM) ? SO2_OverCountLM : 0;//超标天数
                            }
                            if (NO2_OverCountLM == 0)
                            {
                                NO2_OverCountLM = decimal.TryParse(drLastMonthQuXian["NO2_OverCount"].ToString(), out NO2_OverCountLM) ? NO2_OverCountLM : 0;//超标天数
                            }
                            if (CO_OverCountLM == 0)
                            {
                                CO_OverCountLM = decimal.TryParse(drLastMonthQuXian["CO_OverCount"].ToString(), out CO_OverCountLM) ? CO_OverCountLM : 0;//超标天数
                            }
                        }
                        if (PM25_OverCountLM == 0)
                        {
                            nowUnOverPollutantLMQSList.Add("PM2.5");
                        }
                        if (PM10_OverCountLM == 0)
                        {
                            nowUnOverPollutantLMQSList.Add("PM10");
                        }
                        if (O3_OverCountLM == 0)
                        {
                            nowUnOverPollutantLMQSList.Add("O3日最大8小时");
                        }
                        if (SO2_OverCountLM == 0)
                        {
                            nowUnOverPollutantLMQSList.Add("SO2");
                        }
                        if (NO2_OverCountLM == 0)
                        {
                            nowUnOverPollutantLMQSList.Add("NO2");
                        }
                        if (CO_OverCountLM == 0)
                        {
                            nowUnOverPollutantLMQSList.Add("CO");
                        }
                        #endregion
                    }
                    #endregion
                }
                if (dtNowMonthPoint.Rows.Count > 0)
                {
                    foreach (DataRow drNowMonthCeDian in dtNowMonthPoint.Rows)
                    {
                        string PointId = drNowMonthCeDian["PointId"].ToString();
                        string pointName = drNowMonthCeDian["pointName"].ToString();
                        DataRow drBiao2 = dtBiao2.NewRow();
                        #region 表2  XXXX年XX月国控点环境空气质量
                        drBiao2["MonitoringRegionUid"] = PointId;//区域GUID 
                        drBiao2["RegionEn"] = GetPointENById(PointId);//英文名称
                        drBiao2["RegionName"] = pointName;//中文名称
                        //drBiao2["PointId"] = PointId;//区域GUID 
                        //drBiao2["PointEn"] = GetRegionENByGuid(PointId);//英文名称
                        //drBiao2["PointName"] = pointName;//中文名称
                        drBiao2["DaBiaoDays"] = drNowMonthCeDian["StandardCount"].ToString();//达标天数
                        if (drNowMonthCeDian["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drNowMonthCeDian["ValidCount"]) != 0)
                        {
                            if (drNowMonthCeDian["StandardCount"].IsNotNullOrDBNull())
                            {
                                decimal daBiaoRate = DecimalExtension.GetRoundValue(Convert.ToDecimal(drNowMonthCeDian["StandardCount"])
                                                / Convert.ToDecimal(drNowMonthCeDian["ValidCount"]) * 100, 1);
                                drBiao2["DaBiaoRate"] = daBiaoRate.ToString("0.0") + "%";//达标天数比例
                            }
                        }
                        if (isLastY)
                        {
                            DataRow[] drsLastYear = dtLastYearPoint.Select(string.Format("PointId={0}", PointId));
                            if (drsLastYear.Length > 0)
                            {
                                DataRow drLastYear = drsLastYear[0];

                                if (drLastYear["ValidCount"].IsNotNullOrDBNull() && Convert.ToDecimal(drLastYear["ValidCount"]) != 0)
                                {
                                    if (drLastYear["StandardCount"].IsNotNullOrDBNull())
                                    {
                                        decimal daBiaoLastRate = DecimalExtension.GetRoundValue(Convert.ToDecimal(drLastYear["StandardCount"])
                                                         / Convert.ToDecimal(drLastYear["ValidCount"]) * 100, 1);
                                        drBiao2["DaBiaoLastRate"] = daBiaoLastRate.ToString("0.0") + "%";//上年同月达标天数比例
                                    }
                                }

                            }
                        }
                        drBiao2["PM25ChaoDays"] = drNowMonthCeDian["PM25_OverCount"].ToString();//PM2.5超标天数
                        drBiao2["PM10ChaoDays"] = drNowMonthCeDian["PM10_OverCount"].ToString();//PM10超标天数
                        drBiao2["NO2ChaoDays"] = drNowMonthCeDian["NO2_OverCount"].ToString();//NO2超标天数
                        drBiao2["SO2ChaoDays"] = drNowMonthCeDian["SO2_OverCount"].ToString();//SO2超标天数
                        drBiao2["COChaoDays"] = drNowMonthCeDian["CO_OverCount"].ToString();//CO超标天数
                        drBiao2["O38ChaoDays"] = drNowMonthCeDian["O3_OverCount"].ToString();//O3-8小时超标天数
                        dtBiao2.Rows.Add(drBiao2);
                        #endregion
                    }
                }
                #endregion

                #region 全市、市区浓度比较数据调整
                BiJiaoPollutantChaoBiao(nowUnOverPollutantQSList, nowUnOverPollutantSQList, nowUnOverPollutantLYQSList,
                    nowUnOverPollutantLMQSList, "与上年" + monthNow + "月", drText);
                #endregion
            }

            return dsReturn;
        }

        /// <summary>
        /// 创建月报表中文字说明的数据表
        /// </summary>
        /// <returns></returns>
        private DataTable CreateMonthReportTextDataTable()
        {
            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("Important1_PM25", typeof(string));
            dtReturn.Columns.Add("Important1_LastBi", typeof(string));
            dtReturn.Columns.Add("Important1_JiShuBi", typeof(string));
            dtReturn.Columns.Add("Important2_DaBiaoRateShiQu", typeof(string));
            dtReturn.Columns.Add("Important2_LastBiShiQu", typeof(string));
            dtReturn.Columns.Add("Important2_DaBiaoRateQuanShi", typeof(string));
            dtReturn.Columns.Add("Important2_LastBiQuanShi", typeof(string));
            dtReturn.Columns.Add("Important3_SuanRateShiQu", typeof(string));
            dtReturn.Columns.Add("Important3_LastBiShiQu", typeof(string));
            dtReturn.Columns.Add("Important3_SuanRateQuanShi", typeof(string));
            dtReturn.Columns.Add("Important3_LastBiQuanShi", typeof(string));
            dtReturn.Columns.Add("M101_DaBiaoRateRange", typeof(string));
            dtReturn.Columns.Add("M101_AQIRange", typeof(string));
            dtReturn.Columns.Add("M101_DaBiaoRateQuanShi", typeof(string));
            dtReturn.Columns.Add("M101_ChaoFactorOneQuanShi", typeof(string));
            dtReturn.Columns.Add("M101_ChaoFactorTowQuanShi", typeof(string));
            dtReturn.Columns.Add("M102_AQIRange", typeof(string));
            dtReturn.Columns.Add("M102_AQIAvg", typeof(string));
            dtReturn.Columns.Add("M102_DaBiaoRate", typeof(string));
            dtReturn.Columns.Add("M102_ChaoFactorOne", typeof(string));
            dtReturn.Columns.Add("M102_ChaoFactorTwo", typeof(string));
            dtReturn.Columns.Add("M103_LastToNowDaRateQuanShi", typeof(string));
            dtReturn.Columns.Add("M103_LastToNowDaRateShiQu", typeof(string));
            dtReturn.Columns.Add("M104_UnEffectDaysShiQu", typeof(string));
            dtReturn.Columns.Add("M104_UnEffectDatesShiQu", typeof(string));
            dtReturn.Columns.Add("M105_pHAvgQuanShiNowM", typeof(string));
            dtReturn.Columns.Add("M105_SuanRateQuanShiNowM", typeof(string));
            dtReturn.Columns.Add("M105_DuiBiQuanShiNowM", typeof(string));
            dtReturn.Columns.Add("M105_pHAvgShiQuNowM", typeof(string));
            dtReturn.Columns.Add("M105_SuanRateShiQuNowM", typeof(string));
            dtReturn.Columns.Add("M105_DuiBiShiQuNowM", typeof(string));
            dtReturn.Columns.Add("M106", typeof(string));
            dtReturn.Columns.Add("M106_DuiBi", typeof(string));
            dtReturn.Columns.Add("M201_FactorDuiBiQuanShi", typeof(string));
            dtReturn.Columns.Add("M201_FactorDuiBiLastMQuanShi", typeof(string));
            dtReturn.Columns.Add("M202_PM25RangeQuanShi", typeof(string));
            dtReturn.Columns.Add("M202_PM25ShiQu", typeof(string));
            dtReturn.Columns.Add("M202_PM25BiLastYShiQu", typeof(string));
            dtReturn.Columns.Add("M202_PM25BiLastMShiQu", typeof(string));
            dtReturn.Columns.Add("M203_PM10RangeQuanShi", typeof(string));
            dtReturn.Columns.Add("M203_PM10ShiQu", typeof(string));
            dtReturn.Columns.Add("M203_PM10BiLastYShiQu", typeof(string));
            dtReturn.Columns.Add("M203_PM10BiLastMShiQu", typeof(string));
            dtReturn.Columns.Add("M204_NO2RangeQuanShi", typeof(string));
            dtReturn.Columns.Add("M204_NO2ShiQu", typeof(string));
            dtReturn.Columns.Add("M204_NO2BiLastYShiQu", typeof(string));
            dtReturn.Columns.Add("M204_NO2BiLastMShiQu", typeof(string));
            dtReturn.Columns.Add("M204_SO2RangeQuanShi", typeof(string));
            dtReturn.Columns.Add("M204_SO2ShiQu", typeof(string));
            dtReturn.Columns.Add("M204_SO2BiLastYShiQu", typeof(string));
            dtReturn.Columns.Add("M204_SO2BiLastMShiQu", typeof(string));
            dtReturn.Columns.Add("M204_CORangeQuanShi", typeof(string));
            dtReturn.Columns.Add("M204_COShiQu", typeof(string));
            dtReturn.Columns.Add("M204_COBiLastYShiQu", typeof(string));
            dtReturn.Columns.Add("M204_COBiLastMShiQu", typeof(string));
            dtReturn.Columns.Add("M204_O38RangeQuanShi", typeof(string));
            dtReturn.Columns.Add("M204_O38ShiQu", typeof(string));
            dtReturn.Columns.Add("M204_O38BiLastYShiQu", typeof(string));
            dtReturn.Columns.Add("M204_O38BiLastMShiQu", typeof(string));
            dtReturn.Columns.Add("M301_SuanYu", typeof(string));
            dtReturn.Columns.Add("M401_QiXiang", typeof(string));
            return dtReturn;
        }
        /// <summary>
        /// 市区无效日期
        /// </summary>
        /// <returns></returns>
        public DataTable GetMonthReportTimeDataTable(DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            DataTable dt = new DataTable();
            if (regionDayAQI != null)
                dt = regionDayAQI.GetMonthReportTimeDataTable(dateStart, dateEnd).Table;
            DataTable newdt = new DataTable();
            newdt.Columns.Add("DateTime", typeof(string));
            int count = 0;
            string time = "";
            if (dt.Rows.Count > 0)
            {
                count = dt.Columns.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    time += Convert.ToDateTime(dt.Rows[i][0]).ToString("MM月dd日") + "、";
                }
                time = time.Trim('、');
            }
            if (time != "")
                time = "(" + time + ")";
            DataRow dr = newdt.NewRow();
            dr["DateTime"] = count.ToString() + "天无效天" + time;
            newdt.Rows.Add(dr);
            return newdt;
        }
        /// <summary>
        /// 创建月报表中的各种数据的数据集
        /// </summary>
        /// <returns></returns>
        private DataSet CreateMonthReportDataTable()
        {
            DataSet ds = new DataSet();
            DataTable dtBiaoAirInfo = new DataTable("BiaoAirInfo");//苏州市环境空气PM2.5浓度和达标天数比例情况
            DataTable dtBiao1 = new DataTable("Biao1");//表1  XXXX年XX月苏州市环境空气质量
            DataTable dtBiao2 = new DataTable("Biao2");//表2 XXXX年XX月国控点环境空气质量
            DataTable dtBiao3 = new DataTable("Biao3");//表3  XXXX年XX月苏州市各项污染物月均值
            DataTable dtBiao4 = new DataTable("Biao4");//表4  XXXX年XX月苏州市降水监测结果统计
            DataTable dtTu1 = new DataTable("Tu1");//图1数据
            DataTable dtTu2 = new DataTable("Tu2");//图2数据
            DataTable dtText = CreateMonthReportTextDataTable();//创建月报表中文字说明的数据表
            dtText.TableName = "Text";
            ds.Tables.AddRange(new DataTable[] { dtText, dtBiaoAirInfo, dtBiao1, dtBiao2, dtBiao3, dtBiao4, dtTu1, dtTu2 });

            dtBiaoAirInfo.Columns.Add("MonitoringRegionUid", typeof(string));//区域GUID 
            dtBiaoAirInfo.Columns.Add("RegionEn", typeof(string));//英文名称
            dtBiaoAirInfo.Columns.Add("RegionName", typeof(string));//中文名称
            dtBiaoAirInfo.Columns.Add("PM25Last", typeof(string));//PM2.5上一年浓度
            dtBiaoAirInfo.Columns.Add("PM25Now", typeof(string));//PM2.5当前年浓度
            dtBiaoAirInfo.Columns.Add("PM25Ji", typeof(string));//PM2.5基数年浓度
            dtBiaoAirInfo.Columns.Add("DaBiaoJi", typeof(string));//达标天数基数年比例
            dtBiaoAirInfo.Columns.Add("DaBiaoNow", typeof(string));//达标天数当前年比例
            dtBiaoAirInfo.Columns.Add("DaBiaoLast", typeof(string));//达标天数上一年比例

            dtBiao1.Columns.Add("MonitoringRegionUid", typeof(string));//区域GUID 
            dtBiao1.Columns.Add("RegionEn", typeof(string));//英文名称
            dtBiao1.Columns.Add("RegionName", typeof(string));//中文名称
            dtBiao1.Columns.Add("DaBiaoDays", typeof(string));//达标天数
            dtBiao1.Columns.Add("DaBiaoRate", typeof(string));//达标天数比例
            dtBiao1.Columns.Add("DaBiaoLastRate", typeof(string));//上年同月达标天数比例
            dtBiao1.Columns.Add("PM25ChaoDays", typeof(string));//PM2.5超标天数
            dtBiao1.Columns.Add("PM10ChaoDays", typeof(string));//PM10超标天数
            dtBiao1.Columns.Add("NO2ChaoDays", typeof(string));//NO2超标天数
            dtBiao1.Columns.Add("SO2ChaoDays", typeof(string));//SO2超标天数
            dtBiao1.Columns.Add("COChaoDays", typeof(string));//CO超标天数
            dtBiao1.Columns.Add("O38ChaoDays", typeof(string));//O3-8小时超标天数

            dtBiao2.Columns.Add("MonitoringRegionUid", typeof(string));//测点Id
            dtBiao2.Columns.Add("RegionEn", typeof(string));//英文名称
            dtBiao2.Columns.Add("RegionName", typeof(string));//中文名称
            dtBiao2.Columns.Add("DaBiaoDays", typeof(string));//达标天数
            dtBiao2.Columns.Add("DaBiaoRate", typeof(string));//达标天数比例
            dtBiao2.Columns.Add("DaBiaoLastRate", typeof(string));//上年同月达标天数比例
            dtBiao2.Columns.Add("PM25ChaoDays", typeof(string));//PM2.5超标天数
            dtBiao2.Columns.Add("PM10ChaoDays", typeof(string));//PM10超标天数
            dtBiao2.Columns.Add("NO2ChaoDays", typeof(string));//NO2超标天数
            dtBiao2.Columns.Add("SO2ChaoDays", typeof(string));//SO2超标天数
            dtBiao2.Columns.Add("COChaoDays", typeof(string));//CO超标天数
            dtBiao2.Columns.Add("O38ChaoDays", typeof(string));//O3-8小时超标天数

            dtBiao3.Columns.Add("MonitoringRegionUid", typeof(string));//区域GUID 
            dtBiao3.Columns.Add("RegionEn", typeof(string));//英文名称
            dtBiao3.Columns.Add("RegionName", typeof(string));//中文名称
            dtBiao3.Columns.Add("PM25", typeof(string));//PM2.5浓度
            dtBiao3.Columns.Add("PM10", typeof(string));//PM10浓度
            dtBiao3.Columns.Add("NO2", typeof(string));//NO2浓度
            dtBiao3.Columns.Add("SO2", typeof(string));//SO2浓度
            dtBiao3.Columns.Add("CO", typeof(string));//CO浓度
            dtBiao3.Columns.Add("O38", typeof(string));//O38浓度

            dtBiao4.Columns.Add("MonitoringRegionUid", typeof(string));//区域GUID 
            dtBiao4.Columns.Add("RegionEn", typeof(string));//英文名称
            dtBiao4.Columns.Add("RegionName", typeof(string));//中文名称
            dtBiao4.Columns.Add("YangPin", typeof(string));//样品数(个)
            dtBiao4.Columns.Add("Min", typeof(string));//pH最小值
            dtBiao4.Columns.Add("Max", typeof(string));//pH最大值
            dtBiao4.Columns.Add("Avg", typeof(string));//降水pH均值
            dtBiao4.Columns.Add("PinLv", typeof(string));//酸雨频率(%)

            dtTu1.Columns.Add("优", typeof(decimal));
            dtTu1.Columns.Add("良", typeof(decimal));
            dtTu1.Columns.Add("轻度污染", typeof(decimal));
            dtTu1.Columns.Add("中度污染", typeof(decimal));
            dtTu1.Columns.Add("重度污染", typeof(decimal));
            dtTu1.Columns.Add("严重污染", typeof(decimal));

            dtTu2.Columns.Add("优", typeof(decimal));
            dtTu2.Columns.Add("良", typeof(decimal));
            dtTu2.Columns.Add("轻度污染", typeof(decimal));
            dtTu2.Columns.Add("中度污染", typeof(decimal));
            dtTu2.Columns.Add("重度污染", typeof(decimal));
            dtTu2.Columns.Add("严重污染", typeof(decimal));

            return ds;
        }

        /// <summary>
        /// 主要污染物比较
        /// </summary>
        /// <param name="factorBiDictionary"></param>
        /// <param name="factorBiLDictionary"></param>
        /// <param name="strLastTime"></param>
        /// <param name="drText"></param>
        /// <param name="upFactorList"></param>
        /// <param name="equalFactorList"></param>
        /// <param name="downFactorList"></param>
        private void BijiaoPollutant(Dictionary<string, decimal> factorBiDictionary, Dictionary<string, decimal> factorBiLDictionary,
            string strLastTime, DataRow drText, ref IList<string> upFactorList, ref IList<string> equalFactorList,
            ref IList<string> downFactorList)
        {
            int upFactorCount = 0;
            int equalFactorCount = 0;
            int downFactorCount = 0;
            string upFactors = string.Empty;
            string equalFactors = string.Empty;
            string downFactors = string.Empty;
            string msg = string.Empty;
            string msg1 = string.Empty;
            string msg2 = string.Empty;
            string msg3 = "其余{0}项污染物";

            if (strLastTime.StartsWith("与上年"))
            {
                msg1 = "全市{0}月均值" + strLastTime;
                msg2 = "{0}";
            }
            else if (strLastTime.StartsWith("与上月"))
            {
                msg1 = strLastTime + "，{0}月均值";
                msg2 = "{0}月均值";
            }
            foreach (KeyValuePair<string, decimal> keyValue in factorBiDictionary)
            {
                string pollutantName = keyValue.Key;
                decimal pollutantNow = keyValue.Value;

                if (factorBiLDictionary.ContainsKey(pollutantName))
                {
                    decimal pollutantL = factorBiLDictionary[pollutantName];

                    if (pollutantNow > pollutantL)
                    {
                        upFactorList.Add(pollutantName);
                    }
                    else if (pollutantNow == pollutantL)
                    {
                        equalFactorList.Add(pollutantName);
                    }
                    else
                    {
                        downFactorList.Add(pollutantName);
                    }
                }
            }
            upFactorCount = upFactorList.Count;
            equalFactorCount = equalFactorList.Count;
            downFactorCount = downFactorList.Count;
            if (upFactorCount > 0)
            {
                upFactors = upFactorList.Aggregate((a, b) => a + "、" + b);
                upFactors = upFactors.Replace("、" + upFactorList[upFactorCount - 1], "和" + upFactorList[upFactorCount - 1]);
            }
            if (equalFactorCount > 0)
            {
                equalFactors = equalFactorList.Aggregate((a, b) => a + "、" + b);
                equalFactors = equalFactors.Replace("、" + equalFactorList[equalFactorCount - 1], "和" + equalFactorList[equalFactorCount - 1]);
            }
            if (downFactorCount > 0)
            {
                downFactors = downFactorList.Aggregate((a, b) => a + "、" + b);
                downFactors = downFactors.Replace("、" + downFactorList[downFactorCount - 1], "和" + downFactorList[downFactorCount - 1]);
            }
            if (upFactorCount == 0 && equalFactorCount == 0 && downFactorCount == 0)
            {
                return;
            }
            if (equalFactorCount == 6)
            {
                msg = string.Format(msg1 + "均持平", "所有污染物");
            }
            else if (upFactorCount == 6)
            {
                msg = string.Format(msg1 + "均有所上升", "所有污染物");
            }
            else if (downFactorCount == 6)
            {
                msg = string.Format(msg1 + "均有所下降", "所有污染物");
            }
            else
            {
                if (upFactorCount == 0)
                {
                    if (equalFactorCount < downFactorCount)
                    {
                        msg1 = (equalFactorCount > 1) ? string.Format(msg1 + "均持平", equalFactors) : string.Format(msg1 + "持平", equalFactors);
                        msg3 = (downFactorCount > 1) ? string.Format(msg3 + "均有所下降", GetChinaNum(downFactorCount))
                            : string.Format(msg3 + "有所下降", GetChinaNum(downFactorCount));
                    }
                    else
                    {
                        msg1 = (downFactorCount > 1)
                            ? string.Format(msg1 + "均有所下降", downFactors) : string.Format(msg1 + "有所下降", downFactors);
                        msg3 = (equalFactorCount > 1)
                            ? string.Format(msg3 + "均持平", GetChinaNum(equalFactorCount))
                            : string.Format(msg3 + "持平", GetChinaNum(equalFactorCount));
                    }
                }
                else if (equalFactorCount == 0)
                {
                    if (upFactorCount < downFactorCount)
                    {
                        msg1 = (upFactorCount > 1)
                            ? string.Format(msg1 + "均有所上升", upFactors) : string.Format(msg1 + "有所上升", upFactors);
                        msg3 = (downFactorCount > 1)
                            ? string.Format(msg3 + "均有所下降", GetChinaNum(downFactorCount))
                            : string.Format(msg3 + "有所下降", GetChinaNum(downFactorCount));
                    }
                    else
                    {
                        msg1 = (downFactorCount > 1)
                            ? string.Format(msg1 + "均有所下降", downFactors) : string.Format(msg1 + "有所下降", downFactors);
                        msg3 = (upFactorCount > 1)
                            ? string.Format(msg3 + "均有所上升", GetChinaNum(upFactorCount))
                            : string.Format(msg3 + "有所上升", GetChinaNum(upFactorCount));
                    }
                }
                else if (downFactorCount == 0)
                {
                    if (upFactorCount < equalFactorCount)
                    {
                        msg1 = (upFactorCount > 1)
                            ? string.Format(msg1 + "均有所上升", upFactors) : string.Format(msg1 + "有所上升", upFactors);
                        msg3 = (equalFactorCount > 1)
                            ? string.Format(msg3 + "均持平", GetChinaNum(equalFactorCount))
                            : string.Format(msg3 + "持平", GetChinaNum(equalFactorCount));
                    }
                    else
                    {
                        msg1 = (equalFactorCount > 1)
                            ? string.Format(msg1 + "均持平", equalFactors) : string.Format(msg1 + "持平", equalFactors);
                        msg3 = (upFactorCount > 1)
                            ? string.Format(msg3 + "均有所上升", GetChinaNum(upFactorCount))
                            : string.Format(msg3 + "有所上升", GetChinaNum(upFactorCount));
                    }
                }
                else
                {
                    msg1 = (upFactorCount > 1)
                             ? string.Format(msg1 + "均有所上升", upFactors) : string.Format(msg1 + "有所上升", upFactors);
                    msg2 = (equalFactorCount > 1)
                             ? string.Format(msg2 + "均持平", equalFactors) : string.Format(msg2 + "持平", equalFactors);
                    msg3 = (downFactorCount > 1)
                        ? string.Format(msg3 + "均有所下降", GetChinaNum(downFactorCount))
                        : string.Format(msg3 + "有所下降", GetChinaNum(downFactorCount));
                }

                if (msg2.Contains("{0}"))
                {
                    msg = msg1 + "，" + msg3;
                }
                else
                {
                    msg = msg1 + "，" + msg2 + "，" + msg3;
                }
            }
            if (strLastTime.StartsWith("与上年"))
            {
                drText["M201_FactorDuiBiQuanShi"] = msg;
            }
            else if (strLastTime.StartsWith("与上月"))
            {
                drText["M201_FactorDuiBiLastMQuanShi"] = msg;
            }
        }

        private string GetChinaNum(int num)
        {
            string strNum = string.Empty;
            switch (num)
            {
                case 1:
                    strNum = "一";
                    break;
                case 2:
                    strNum = "二";
                    break;
                case 3:
                    strNum = "三";
                    break;
                case 4:
                    strNum = "四";
                    break;
                case 5:
                    strNum = "五";
                    break;
                case 6:
                    strNum = "六";
                    break;
                default: break;
            }
            return strNum;
        }

        /// <summary>
        /// 计算全市因子浓度范围
        /// </summary>
        /// <param name="dtValue"></param>
        /// <param name="dr"></param>
        private void ComputePollutantValueRange(DataTable dtValue, DataRow drText)
        {
            IList<decimal> PM25ValueList = new List<decimal>();
            IList<decimal> PM10ValueList = new List<decimal>();
            IList<decimal> NO2ValueList = new List<decimal>();
            IList<decimal> SO2ValueList = new List<decimal>();
            IList<decimal> COValueList = new List<decimal>();
            IList<decimal> O3ValueList = new List<decimal>();

            foreach (DataRow drValue in dtValue.Rows)
            {
                decimal PM25_C = DecimalExtension.GetRoundValue((decimal.TryParse(drValue["a34004"].ToString(), out PM25_C) ? PM25_C : 0) * 1000, 1);
                decimal PM10_C = DecimalExtension.GetRoundValue((decimal.TryParse(drValue["a34002"].ToString(), out PM10_C) ? PM10_C : 0) * 1000, 0);
                decimal NO2_C = DecimalExtension.GetRoundValue((decimal.TryParse(drValue["a21004"].ToString(), out NO2_C) ? NO2_C : 0) * 1000, 0);
                decimal SO2_C = DecimalExtension.GetRoundValue((decimal.TryParse(drValue["a21026"].ToString(), out SO2_C) ? SO2_C : 0) * 1000, 0);
                decimal CO_C = DecimalExtension.GetRoundValue((decimal.TryParse(drValue["a21005"].ToString(), out CO_C) ? CO_C : 0), 2);
                decimal O3_C = DecimalExtension.GetRoundValue((decimal.TryParse(drValue["a05024"].ToString(), out O3_C) ? O3_C : 0) * 1000, 0);

                PM25ValueList.Add(PM25_C);
                PM10ValueList.Add(PM10_C);
                NO2ValueList.Add(NO2_C);
                SO2ValueList.Add(SO2_C);
                COValueList.Add(CO_C);
                O3ValueList.Add(O3_C);
            }
            drText["M202_PM25RangeQuanShi"] = PM25ValueList.Min().ToString("0.0") + "～" + PM25ValueList.Max().ToString("0.0");
            drText["M203_PM10RangeQuanShi"] = PM10ValueList.Min().ToString("0") + "～" + PM10ValueList.Max().ToString("0");
            drText["M204_NO2RangeQuanShi"] = NO2ValueList.Min().ToString("0") + "～" + NO2ValueList.Max().ToString("0");
            drText["M204_SO2RangeQuanShi"] = SO2ValueList.Min().ToString("0") + "～" + SO2ValueList.Max().ToString("0");
            drText["M204_CORangeQuanShi"] = COValueList.Min().ToString("0.00") + "～" + COValueList.Max().ToString("0.00");
            drText["M204_O38RangeQuanShi"] = O3ValueList.Min().ToString("0") + "～" + O3ValueList.Max().ToString("0");
        }

        /// <summary>
        /// 比较污染物浓度比例
        /// </summary>
        /// <param name="factorBiDictionary"></param>
        /// <param name="factorBiLDictionary"></param>
        /// <param name="strLastTime"></param>
        /// <param name="drText"></param>
        private void BiJiaoPollutantRate(Dictionary<string, decimal> factorBiDictionary, Dictionary<string, decimal> factorBiLDictionary,
            string strLastTime, DataRow drText)
        {
            foreach (KeyValuePair<string, decimal> keyValue in factorBiDictionary)
            {
                string pollutantName = keyValue.Key;
                decimal pollutantNow = keyValue.Value;
                if (factorBiLDictionary.ContainsKey(pollutantName))
                {
                    decimal pollutantL = factorBiLDictionary[pollutantName];

                    if (pollutantL > 0)
                    {
                        string msg = strLastTime;
                        string columnName = string.Empty;
                        decimal rate = DecimalExtension.GetRoundValue((pollutantNow - pollutantL) / pollutantL * 100, 1);

                        if (rate > 0)
                        {
                            msg += "上升了" + rate.ToString("0.0") + "%";
                        }
                        else if (rate == 0)
                        {
                            msg += "持平";
                        }
                        else
                        {
                            msg += "下降了" + (-rate).ToString("0.0") + "%";
                        }
                        if (strLastTime.StartsWith("与上年"))
                        {
                            switch (pollutantName)
                            {
                                case "PM2.5":
                                    columnName = "M202_PM25BiLastYShiQu";
                                    break;
                                case "PM10":
                                    columnName = "M203_PM10BiLastYShiQu";
                                    break;
                                case "NO2":
                                    columnName = "M204_NO2BiLastYShiQu";
                                    break;
                                case "SO2":
                                    columnName = "M204_SO2BiLastYShiQu";
                                    break;
                                case "CO":
                                    columnName = "M204_COBiLastYShiQu";
                                    break;
                                case "O3日最大8小时":
                                    columnName = "M204_O38BiLastYShiQu";
                                    break;
                            }
                        }
                        else if (strLastTime.StartsWith("与上月"))
                        {
                            switch (pollutantName)
                            {
                                case "PM2.5":
                                    columnName = "M202_PM25BiLastMShiQu";
                                    break;
                                case "PM10":
                                    columnName = "M203_PM10BiLastMShiQu";
                                    break;
                                case "NO2":
                                    columnName = "M204_NO2BiLastMShiQu";
                                    break;
                                case "SO2":
                                    columnName = "M204_SO2BiLastMShiQu";
                                    break;
                                case "CO":
                                    columnName = "M204_COBiLastMShiQu";
                                    break;
                                case "O3日最大8小时":
                                    columnName = "M204_O38BiLastMShiQu";
                                    break;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(columnName))
                        {
                            drText[columnName] = msg;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 全市达标天数与上年比较
        /// </summary>
        /// <param name="daBiaoBiJiaoQSDictionary"></param>
        /// <param name="drText"></param>
        private void BiJiaoDaBiaoRateQS(Dictionary<string, string> daBiaoBiJiaoQSDictionary, DataRow drText)
        {
            //全市各区县达标天数与上年同月比较的类型有几种，上升、持平、下降
            int daBiaoBiJiaoQSTypeCount = daBiaoBiJiaoQSDictionary.Select(t => t.Value).Distinct().Count();
            int daBiaoUpQSTypeCount = daBiaoBiJiaoQSDictionary.Where(t => t.Value == "有所上升").Count();
            int daBiaoEqualQSTypeCount = daBiaoBiJiaoQSDictionary.Where(t => t.Value == "持平").Count();
            int daBiaoDownQSTypeCount = daBiaoBiJiaoQSDictionary.Where(t => t.Value == "有所下降").Count();
            string upFactors = string.Empty;
            string equalFactors = string.Empty;
            string downFactors = string.Empty;
            string msg = string.Empty;

            //上升、持平、下降的区县
            if (daBiaoUpQSTypeCount > 0)
            {
                IList<string> upFactorList = daBiaoBiJiaoQSDictionary.Where(t => t.Value == "有所上升").Select(t => t.Key).ToList();
                upFactors = upFactorList.Aggregate((a, b) => a + "、" + b);
                upFactors = upFactors.Replace("、" + upFactorList[daBiaoUpQSTypeCount - 1], "和" + upFactorList[daBiaoUpQSTypeCount - 1]);
            }
            if (daBiaoEqualQSTypeCount > 0)
            {
                IList<string> equalFactorList = daBiaoBiJiaoQSDictionary.Where(t => t.Value == "持平").Select(t => t.Key).ToList();
                equalFactors = equalFactorList.Aggregate((a, b) => a + "、" + b);
                equalFactors = equalFactors.Replace("、" + equalFactorList[daBiaoEqualQSTypeCount - 1], "和" + equalFactorList[daBiaoEqualQSTypeCount - 1]);
            }
            if (daBiaoDownQSTypeCount > 0)
            {
                IList<string> downFactorList = daBiaoBiJiaoQSDictionary.Where(t => t.Value == "有所下降").Select(t => t.Key).ToList();
                downFactors = downFactorList.Aggregate((a, b) => a + "、" + b);
                downFactors = downFactors.Replace("、" + downFactorList[daBiaoDownQSTypeCount - 1], "和" + downFactorList[daBiaoDownQSTypeCount - 1]);
            }
            if (daBiaoBiJiaoQSTypeCount == 1)
            {
                msg = "均" + daBiaoBiJiaoQSDictionary.Select(t => t.Value).FirstOrDefault();
            }
            else if (daBiaoBiJiaoQSTypeCount == 2)
            {
                if (daBiaoUpQSTypeCount == 0)
                {
                    if (daBiaoEqualQSTypeCount < daBiaoDownQSTypeCount)
                    {
                        msg = equalFactors + "环境空气质量达标率持平，其余各地环境空气质量达标率均有所下降";
                    }
                    else
                    {
                        msg = downFactors + "环境空气质量达标率有所下降，其余各地环境空气质量达标率均持平";
                    }
                }
                else if (daBiaoEqualQSTypeCount == 0)
                {
                    if (daBiaoUpQSTypeCount < daBiaoDownQSTypeCount)
                    {
                        msg = upFactors + "环境空气质量达标率有所上升，其余各地环境空气质量达标率均有所下降";
                    }
                    else
                    {
                        msg = downFactors + "环境空气质量达标率有所下降，其余各地环境空气质量达标率均有所上升";
                    }
                }
                else if (daBiaoDownQSTypeCount == 0)
                {
                    if (daBiaoUpQSTypeCount < daBiaoEqualQSTypeCount)
                    {
                        msg = upFactors + "环境空气质量达标率有所上升，其余各地环境空气质量达标率均持平";
                    }
                    else
                    {
                        msg = equalFactors + "环境空气质量达标率持平，其余各地环境空气质量达标率均有所上升";
                    }
                }
            }
            else if (daBiaoBiJiaoQSTypeCount == 3)
            {
                msg = upFactors + "环境空气质量达标率有所上升，" + equalFactors + "持平，其余各地环境空气质量达标率均有所下降";
            }
            drText["M103_LastToNowDaRateQuanShi"] = msg;
        }
        /// <summary>
        /// 全市达标天数与上年比较
        /// </summary>
        /// <param name="daBiaoBiJiaoQSDictionary"></param>
        /// <param name="drText"></param>
        private void BiJiaoDaBiaoRatePI(Dictionary<string, string> daBiaoBiJiaoPIDictionary, DataRow drText)
        {
            //全市各区县达标天数与上年同月比较的类型有几种，上升、持平、下降
            int daBiaoBiJiaoQSTypeCount = daBiaoBiJiaoPIDictionary.Select(t => t.Value).Distinct().Count();
            int daBiaoUpQSTypeCount = daBiaoBiJiaoPIDictionary.Where(t => t.Value == "有所上升").Count();
            int daBiaoEqualQSTypeCount = daBiaoBiJiaoPIDictionary.Where(t => t.Value == "持平").Count();
            int daBiaoDownQSTypeCount = daBiaoBiJiaoPIDictionary.Where(t => t.Value == "有所下降").Count();
            string upFactors = string.Empty;
            string equalFactors = string.Empty;
            string downFactors = string.Empty;
            string msg = string.Empty;

            //上升、持平、下降的区县
            if (daBiaoUpQSTypeCount > 0)
            {
                IList<string> upFactorList = daBiaoBiJiaoPIDictionary.Where(t => t.Value == "有所上升").Select(t => t.Key).ToList();
                upFactors = upFactorList.Aggregate((a, b) => a + "、" + b);
                upFactors = upFactors.Replace("、" + upFactorList[daBiaoUpQSTypeCount - 1], "和" + upFactorList[daBiaoUpQSTypeCount - 1]);
            }
            if (daBiaoEqualQSTypeCount > 0)
            {
                IList<string> equalFactorList = daBiaoBiJiaoPIDictionary.Where(t => t.Value == "持平").Select(t => t.Key).ToList();
                equalFactors = equalFactorList.Aggregate((a, b) => a + "、" + b);
                equalFactors = equalFactors.Replace("、" + equalFactorList[daBiaoEqualQSTypeCount - 1], "和" + equalFactorList[daBiaoEqualQSTypeCount - 1]);
            }
            if (daBiaoDownQSTypeCount > 0)
            {
                IList<string> downFactorList = daBiaoBiJiaoPIDictionary.Where(t => t.Value == "有所下降").Select(t => t.Key).ToList();
                downFactors = downFactorList.Aggregate((a, b) => a + "、" + b);
                downFactors = downFactors.Replace("、" + downFactorList[daBiaoDownQSTypeCount - 1], "和" + downFactorList[daBiaoDownQSTypeCount - 1]);
            }
            if (daBiaoBiJiaoQSTypeCount == 1)
            {
                msg = "均" + daBiaoBiJiaoPIDictionary.Select(t => t.Value).FirstOrDefault();
            }
            else if (daBiaoBiJiaoQSTypeCount == 2)
            {
                if (daBiaoUpQSTypeCount == 0)
                {
                    if (daBiaoEqualQSTypeCount < daBiaoDownQSTypeCount)
                    {
                        msg = equalFactors + "环境空气质量达标率持平，其余各地环境空气质量达标率均有所下降";
                    }
                    else
                    {
                        msg = downFactors + "环境空气质量达标率有所下降，其余各地环境空气质量达标率均持平";
                    }
                }
                else if (daBiaoEqualQSTypeCount == 0)
                {
                    if (daBiaoUpQSTypeCount < daBiaoDownQSTypeCount)
                    {
                        msg = upFactors + "环境空气质量达标率有所上升，其余各地环境空气质量达标率均有所下降";
                    }
                    else
                    {
                        msg = downFactors + "环境空气质量达标率有所下降，其余各地环境空气质量达标率均有所上升";
                    }
                }
                else if (daBiaoDownQSTypeCount == 0)
                {
                    if (daBiaoUpQSTypeCount < daBiaoEqualQSTypeCount)
                    {
                        msg = upFactors + "环境空气质量达标率有所上升，其余各地环境空气质量达标率均持平";
                    }
                    else
                    {
                        msg = equalFactors + "环境空气质量达标率持平，其余各地环境空气质量达标率均有所上升";
                    }
                }
            }
            else if (daBiaoBiJiaoQSTypeCount == 3)
            {
                msg = upFactors + "环境空气质量达标率有所上升，" + equalFactors + "持平，其余各地环境空气质量达标率均有所下降";
            }
            drText["M106"] = msg;
        }
        /// <summary>
        /// 全市、市区浓度比较数据调整
        /// </summary>
        /// <param name="nowOverPollutantQSList"></param>
        /// <param name="nowOverPollutantSQList"></param>
        /// <param name="drText"></param>
        private void BiJiaoPollutantChaoBiao(IList<string> nowUnOverPollutantQSList, IList<string> nowUnOverPollutantSQList,
            IList<string> nowUnOverPollutantLYQSList, IList<string> nowUnOverPollutantLMQSList, string strLYTime, DataRow drText)
        {
            foreach (string nowOverPollutantQS in nowUnOverPollutantQSList)
            {
                string columnName1 = string.Empty;
                string columnName2 = string.Empty;
                string columnName3 = string.Empty;
                string columnName4 = string.Empty;
                string msg = string.Empty;

                switch (nowOverPollutantQS)
                {
                    //case "PM2.5":
                    //    columnName1 = "M202_PM25RangeQuanShi";
                    //    columnName2 = "M202_PM25ShiQu";
                    //    columnName3 = "M202_PM25BiLastYShiQu";
                    //    columnName4 = "M202_PM25BiLastMShiQu";
                    //    msg = string.Format("    全市{0}日均值未出现超标（图3）。", nowOverPollutantQS);
                    //    break;
                    //case "PM10":
                    //    columnName1 = "M203_PM10RangeQuanShi";
                    //    columnName2 = "M203_PM10ShiQu";
                    //    columnName3 = "M203_PM10BiLastYShiQu";
                    //    columnName4 = "M203_PM10BiLastMShiQu";
                    //    msg = string.Format("    全市{0}日均值未出现超标（图4）。", nowOverPollutantQS);
                    //    break;
                    //case "NO2":
                    //    columnName1 = "M204_NO2RangeQuanShi";
                    //    columnName2 = "M204_NO2ShiQu";
                    //    columnName3 = "M204_NO2BiLastYShiQu";
                    //    columnName4 = "M204_NO2BiLastMShiQu";
                    //    msg = string.Format("    全市{0}日均值未出现超标（图5）。", nowOverPollutantQS);
                    //    break;
                    //case "SO2":
                    //    columnName1 = "M204_SO2RangeQuanShi";
                    //    columnName2 = "M204_SO2ShiQu";
                    //    columnName3 = "M204_SO2BiLastYShiQu";
                    //    columnName4 = "M204_SO2BiLastMShiQu";
                    //    msg = string.Format("    全市{0}日均值未出现超标（图6）。", nowOverPollutantQS);
                    //    break;
                    case "CO":
                        columnName1 = "M204_CORangeQuanShi";
                        columnName2 = "M204_COShiQu";
                        columnName3 = "M204_COBiLastYShiQu";
                        columnName4 = "M204_COBiLastMShiQu";
                        msg = string.Format("    全市{0}日均值未出现超标（图7）。", nowOverPollutantQS);
                        break;
                    case "O3日最大8小时":
                        columnName1 = "M204_O38RangeQuanShi";
                        columnName2 = "M204_O38ShiQu";
                        columnName3 = "M204_O38BiLastYShiQu";
                        columnName4 = "M204_O38BiLastMShiQu";
                        msg = string.Format("    全市{0}值未出现超标。", nowOverPollutantQS);
                        break;
                    default: break;
                }

                if (nowUnOverPollutantLYQSList.Contains(nowOverPollutantQS) && nowUnOverPollutantLMQSList.Contains(nowOverPollutantQS))
                {
                    msg += strLYTime + "和上月持平。\r\n";
                }
                else if (nowUnOverPollutantLYQSList.Contains(nowOverPollutantQS) && !nowUnOverPollutantLMQSList.Contains(nowOverPollutantQS))
                {
                    msg += strLYTime + "持平，与上月相比有所下降。\r\n";
                }
                else if (!nowUnOverPollutantLYQSList.Contains(nowOverPollutantQS) && nowUnOverPollutantLMQSList.Contains(nowOverPollutantQS))
                {
                    msg += strLYTime + "相比有所下降，与上月持平。\r\n";
                }
                else
                {
                    msg += strLYTime + "和上月相比均有所下降。\r\n";
                }
                if (!string.IsNullOrWhiteSpace(columnName1))
                {
                    drText[columnName1] = msg;
                    drText[columnName2] = string.Empty;
                    drText[columnName3] = string.Empty;
                    drText[columnName4] = string.Empty;
                }
            }
        }

        /// <summary>
        /// 根据区域Guid获取英文名
        /// </summary>
        /// <param name="monitoringRegionUid"></param>
        /// <returns></returns>
        private string GetRegionENByGuid(string monitoringRegionUid)
        {
            string regionEN = string.Empty;

            switch (monitoringRegionUid)
            {
                case "6a4e7093-f2c6-46b4-a11f-0f91b4adf379":
                    regionEN = "GuSu";
                    break;
                case "69a993ff-78c6-459b-9322-ee77e0c8cd68":
                    regionEN = "GongYeYuan";
                    break;
                case "f320aa73-7c55-45d4-a363-e21408e0aac3":
                    regionEN = "GaoXin";
                    break;
                case "8756bd44-ff18-46f7-aedf-615006d7474c":
                    regionEN = "XiangCheng";
                    break;
                case "e1c104f3-aaf3-4d0e-9591-36cdc83be15a":
                    regionEN = "WuZhong";
                    break;
                case "7e05b94c-bbd4-45c3-919c-42da2e63fd43":
                    regionEN = "ShiQu";
                    break;
                case "66d2abd1-ca39-4e39-909f-da872704fbfd":
                    regionEN = "ZhangJiaGang";
                    break;
                case "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff":
                    regionEN = "ChangShu";
                    break;
                case "57b196ed-5038-4ad0-a035-76faee2d7a98":
                    regionEN = "TaiCang";
                    break;
                case "2e2950cd-dbab-43b3-811d-61bd7569565a":
                    regionEN = "KunShan";
                    break;
                case "2fea3cb2-8b95-45e6-8a71-471562c4c89c":
                    regionEN = "WuJiang";
                    break;
                case "":
                    regionEN = "QuanShi";
                    break;
                default: break;
            }
            return regionEN;
        }
        /// <summary>
        /// 根据测点Id获取英文名
        /// </summary>
        /// <param name="monitoringRegionUid"></param>
        /// <returns></returns>
        private string GetPointENById(string PointId)
        {
            string pointEN = string.Empty;

            switch (PointId)
            {
                case "1":
                    pointEN = "NanMen";
                    break;
                case "2":
                    pointEN = "CaiXiang";
                    break;
                case "3":
                    pointEN = "ZhaGangChang";
                    break;
                case "4":
                    pointEN = "WuZhong";
                    break;
                case "5":
                    pointEN = "GaoXin";
                    break;
                case "6":
                    pointEN = "YuanQu";
                    break;
                case "7":
                    pointEN = "XiangCheng";
                    break;
                default: break;
            }
            return pointEN;
        }
        /// <summary>
        /// 根据因子Code获取因子名称
        /// </summary>
        /// <param name="pollutantCode"></param>
        /// <returns></returns>
        private string GetPollutantNameByCode(string pollutantCode)
        {
            string pollutantName = string.Empty;

            switch (pollutantCode)
            {
                case "a34004":
                    pollutantName = "PM2.5";
                    break;
                case "a34002":
                    pollutantName = "PM10";
                    break;
                case "a21026":
                    pollutantName = "SO2";
                    break;
                case "a21004":
                    pollutantName = "NO2";
                    break;
                case "a21005":
                    pollutantName = "CO";
                    break;
                case "a05024":
                    pollutantName = "O3日最大8小时";
                    break;
                default: break;
            }
            return pollutantName;
        }

        /// <summary>
        /// 根据因子名称获取因子Code
        /// </summary>
        /// <param name="pollutantName"></param>
        /// <returns></returns>
        private string GetPollutantCodeByName(string pollutantName)
        {
            string pollutantCode = string.Empty;

            switch (pollutantName)
            {
                case "PM2.5":
                    pollutantCode = "a34004";
                    break;
                case "PM25":
                    pollutantCode = "a34004";
                    break;
                case "PM10":
                    pollutantCode = "a34002";
                    break;
                case "SO2":
                    pollutantCode = "a21026";
                    break;
                case "NO2":
                    pollutantCode = "a21004";
                    break;
                case "CO":
                    pollutantCode = "a21005";
                    break;
                case "O3日最大8小时":
                    pollutantCode = "a05024";
                    break;
                case "Max8HourO3":
                    pollutantCode = "a05024";
                    break;
                case "MaxOneHourO3":
                    pollutantCode = "a05024";
                    break;
                default: break;
            }
            return pollutantCode;
        }
        #endregion
    }
}
