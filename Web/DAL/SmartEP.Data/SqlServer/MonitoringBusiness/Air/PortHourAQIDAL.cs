﻿using log4net;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：PortHourAQIDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-06-02
    /// 功能摘要：
    /// 环境空气发布：点位小时AQI数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class PortHourAQIDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 虚拟分页类
        /// </summary>
        protected GridViewPagerDAL g_GridViewPager = Singleton<GridViewPagerDAL>.GetInstance();

        /// <summary>
        /// 存储过程处理类
        /// </summary>
        BaseDAHelper g_DBBiz = Singleton<BaseDAHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string connection = null;

        /// <summary>
        /// 数据库表名
        /// </summary>
        protected string tableName = null;

        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器

        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public PortHourAQIDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.HourAQI);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
        }
        #endregion

        #region << 数据查询方法 >>
        /// <summary>
        /// 查询测点最新一条原始小时数据提供给接口
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetAirQualityNewestOriRTReportForData(string factor, string orderBy = "DateTime,PointId")
        {
            try
            {
                string tbname = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.OriHourAQI);
                string field = string.Format(@"PointId, {0},{1}, 
                                            case when ({1} > 0 and {1} < 50) then '一级' 
                                             when ({1} >= 51 and {1} <= 100) then '二级' 
                                             when ({1} >= 101 and {1} <= 150) then '三级' 
                                             when ({1} >= 151 and {1} <= 200) then '四级' 
                                             when ({1} >= 201 and {1} <= 300) then '五级' 
                                             when ({1} >= 301) then '六级' 
                                            else '无效天' 
                                            end as Grade, 
                                            case when ({1} > 0 and {1} < 50) then '优' 
                                             when ({1} >= 51 and {1} <= 100) then '良' 
                                             when ({1} >= 101 and {1} <= 150) then '轻度污染' 
                                             when ({1} >= 151 and {1} <= 200) then '中度污染' 
                                             when ({1} >= 201 and {1} <= 300) then '重度污染' 
                                             when ({1} >= 301) then '严重污染' 
                                            else '无效天' 
                                            end as Class, DateTime", factor, factor + "_IAQI");
                string con = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
                string sql = string.Format(@"select B.PointId,{2},{3},Convert(nvarchar(13),B.DateTime,21) DateTime,Grade,Class,[dbo].[SY_MonitoringPoint].MonitoringPointName  PortName from
(select PointId,Max(DateTime) DateTime from {1} group by pointId) A
left join 
(select {0} from {1}) B
on A.PointId = B.PointId and A.DateTime = B.DateTime
left join [dbo].[SY_MonitoringPoint]
on A.PointId = [dbo].[SY_MonitoringPoint].PointId
order by B.PointId
                                        ", field, tbname, factor, factor + "_IAQI");
                DataView dv = g_DatabaseHelper.ExecuteDataView(sql, con);

                //StringBuilder sb = new StringBuilder();
                //sb.Append("select * from (");
                //for (int i = 0; i < portIds.Length - 1; i++)
                //{
                //    string sql = string.Format("(select top 1 {2} from {0} where PointId = {1} order by DateTime desc) union ", tbname, portIds[i], field);
                //    sb.Append(sql);
                //}
                //string sqllast = string.Format("(select top 1 {2} from {0} where PointId = {1} order by DateTime desc)) A", tbname, portIds[portIds.Length - 1], field);
                //sb.Append(sqllast);
                //DataView dv = g_DatabaseHelper.ExecuteDataView(sb.ToString(), con);
                return dv;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 查询测点最新一条原始小时数据提供给接口
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetFactorNewestOriForData(string portId, string factor, string orderBy = "DateTime,PointId")
        {
            try
            {
                string tbname = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.OriHourAQI);
                string field = "DateTime," + factor + " value";
                string con = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
                //StringBuilder sb = new StringBuilder();
                //sb.Append("select * from (");
                //for (int i = 0; i < portIds.Length - 1; i++)
                //{
                //    string sql = string.Format("(select top 1 {2} from {0} where PointId = {1} order by DateTime desc) union ", tbname, portIds[i], field);
                //    sb.Append(sql);
                //}
                //string sqllast = string.Format("(select top 1 {2} from {0} where PointId = {1} order by DateTime desc)) A", tbname, portIds[portIds.Length - 1], field);
                //sb.Append(sqllast);
                string sql = string.Format("select top 24 {0} from {1} where PointId = {2} order by DateTime desc", field, tbname, portId);
                DataView dv = g_DatabaseHelper.ExecuteDataView(sql, con);
                return dv;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 查询测点最新一条原始小时数据(不传因子)
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetAirQualityNewestOriRTReportWithOutFac(string[] portIds, string orderBy = "DateTime,PointId")
        {
            try
            {
                string tbname = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.OriHourAQI);
                string con = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
                string factors=string.Empty;
//                string fieldName = @"PointId
//                    ,CONVERT(varchar(13),DateTime,121) DateTime
//                    ,SO2
//                    ,SO2_IAQI
//                    ,NO2
//                    ,NO2_IAQI
//                    ,PM10
//                    ,PM10_IAQI
//                    ,Recent24HoursPM10
//                    ,Recent24HoursPM10_IAQI
//                    ,CO
//                    ,CO_IAQI
//                    ,O3
//                    ,O3_IAQI
//                    ,Recent8HoursO3
//                    ,Recent8HoursO3_IAQI
//                    ,Recent8HoursO3NT
//                    ,Recent8HoursO3NT_IAQI
//                    ,PM25
//                    ,PM25_IAQI
//                    ,Recent24HoursPM25
//                    ,Recent24HoursPM25_IAQI
//                    ,AQIValue
//                    ,PrimaryPollutant
//                    ,Range
//                    ,RGBValue
//                    ,PicturePath
//                    ,Class
//                    ,Grade
//                    ,HealthEffect
//                    ,TakeStep";
                //StringBuilder sb = new StringBuilder();
                //sb.Append("select * from (");
                string fieldName = string.Format(@"b.PointId
                     ,CONVERT(varchar(13),'{0}') DateTime
                    ,SO2
                    ,SO2_IAQI
                    ,NO2
                    ,NO2_IAQI
                    ,PM10
                    ,PM10_IAQI
                    ,Recent24HoursPM10
                    ,Recent24HoursPM10_IAQI
                    ,CO
                    ,CO_IAQI
                    ,O3
                    ,O3_IAQI
                    ,Recent8HoursO3
                    ,Recent8HoursO3_IAQI
                    ,Recent8HoursO3NT
                    ,Recent8HoursO3NT_IAQI
                    ,PM25
                    ,PM25_IAQI
                    ,Recent24HoursPM25
                    ,Recent24HoursPM25_IAQI
                    ,AQIValue
                    ,PrimaryPollutant
                    ,Range
                    ,RGBValue
                    ,PicturePath
                    ,Class
                    ,Grade
                    ,HealthEffect
                    ,TakeStep ", DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:59:59"));
                StringBuilder sb = new StringBuilder();
                sb.Append("select ");
                //if (portIds[0] == "ALL")
                //{
                //    string[] pids = new MonitoringPointDAL().GetMonitoringPointDataTableForData().AsEnumerable().Select(t => Convert.ToString(t.Field<int>("PointId"))).ToArray();
                //    for (int i = 0; i < pids.Length - 1; i++)
                //    {
                //        string sql = string.Format("(select top 1 {2} from {0} where  PointId = {1} order by DateTime desc) union ", tbname, pids[i], fieldName);
                //        sb.Append(sql);
                //    }
                //    string sqllast = string.Format("(select top 1 {2} from {0} where  PointId = {1} order by DateTime desc)) A", tbname, pids[pids.Length - 1], fieldName);
                //    sb.Append(sqllast);
                //}
                //else
                //{
                //    for (int i = 0; i < portIds.Length - 1; i++)
                //    {
                //        string sql = string.Format("(select top 1 {2} from {0} where  PointId = {1} order by DateTime desc) union ", tbname, portIds[i], fieldName);
                //        sb.Append(sql);
                //    }
                //    string sqllast = string.Format("(select top 1 {2} from {0} where  PointId = {1} order by DateTime desc)) A", tbname, portIds[portIds.Length - 1], fieldName);
                //    sb.Append(sqllast);
                //}
                if (portIds[0] == "ALL")
                {
                    string[] pids = new MonitoringPointDAL().GetMonitoringPointDataTableForData().AsEnumerable().Select(t => Convert.ToString(t.Field<int>("PointId"))).ToArray();
                    for (int i = 0; i < pids.Length; i++)
                    {
                        factors += pids[i]+",";
                    }
                    string sqllast = string.Format(" {0} from {1} as a right join (select distinct PointId from Air.TB_OriHourAQI where PointId in ({2})) as b on a.PointId = b.PointId and DateTime <= '{3}' and DateTime > '{4}' order by b.PointId", fieldName, tbname, factors.TrimEnd(','), DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:59:59"), DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:59:59"));
                    sb.Append(sqllast);
                }
                else
                {
                    for (int i = 0; i < portIds.Length; i++)
                    {
                        factors += portIds[i] + ",";
                    }
                    string sqllast = string.Format(" {0} from {1} as a right join (select distinct PointId from Air.TB_OriHourAQI where PointId in ({2})) as b on a.PointId = b.PointId and DateTime <= '{3}' and DateTime > '{4}' order by b.PointId", fieldName, tbname, factors.TrimEnd(','), DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:59:59"), DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:59:59"));
                    sb.Append(sqllast);
                }
                DataView dv = g_DatabaseHelper.ExecuteDataView(sb.ToString(), con);
                return dv;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 查询测点最新一条原始小时因子浓度数据(不传因子)
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetFactorByPointId(string[] portIds, string orderBy = "DateTime,PointId")
        {
            try
            {
                DateTime dtstart = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd HH:00:00"))).AddHours(-1);
                DateTime dtend = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd HH:00:00")));
                string tbname = "Air.TB_InfectantBy60";
                string con = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
                string fieldName = "Tstamp,PointId,PollutantCode,PollutantValue";
                StringBuilder sb = new StringBuilder();
                if (portIds[0] == "ALL")
                {
                    string[] pids = new MonitoringPointDAL().GetMonitoringPointDataTableForData().AsEnumerable().Select(t => Convert.ToString(t.Field<int>("PointId"))).ToArray();
                    StringBuilder sbpid = new StringBuilder();
                    foreach (string s in pids)
                    {
                        sbpid.Append("'" + s + "',");
                    }
                    string sql = string.Format("select {2} from {0} where PointId in ({1}) and PollutantCode in ('a34002','a34004','a21004','a21026','a21005','a05024') and Tstamp <= '{4}' and Tstamp >= '{3}'", tbname, sbpid.ToString().TrimEnd(','), fieldName, dtstart, dtend);
                    sb.Append(sql);
                }
                else
                {
                    StringBuilder sbpid = new StringBuilder();
                    foreach (string s in portIds)
                    {
                        sbpid.Append("'" + s + "',");
                    }
                    string sql = string.Format("select {2} from {0} where PointId in ({1}) and PollutantCode in ('a34002','a34004','a21004','a21026','a21005','a05024') and Tstamp <= '{4}' and Tstamp >= '{3}'", tbname, sbpid.ToString().TrimEnd(','), fieldName, dtstart, dtend);
                    sb.Append(sql);
                }
                DataView dv = g_DatabaseHelper.ExecuteDataView(sb.ToString(), con);
                if (dv.Count > 0)
                {
                    return dv;
                }
                else
                {
                    DateTime dtstartnew = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd HH:00:00"))).AddHours(-2);
                    DateTime dtendnew = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd HH:00:00"))).AddHours(-1);
                    StringBuilder sbnew = new StringBuilder();
                    if (portIds[0] == "ALL")
                    {
                        string[] pids = new MonitoringPointDAL().GetMonitoringPointDataTableForData().AsEnumerable().Select(t => Convert.ToString(t.Field<int>("PointId"))).ToArray();
                        StringBuilder sbpid = new StringBuilder();
                        foreach (string s in pids)
                        {
                            sbpid.Append("'" + s + "',");
                        }
                        string sql = string.Format("select {2} from {0} where PointId in ({1}) and PollutantCode in ('a34002','a34004','a21004','a21026','a21005','a05024') and Tstamp <= '{4}' and Tstamp >= '{3}'", tbname, sbpid.ToString().TrimEnd(','), fieldName, dtstartnew, dtendnew);
                        sbnew.Append(sql);
                    }
                    else
                    {
                        StringBuilder sbpid = new StringBuilder();
                        foreach (string s in portIds)
                        {
                            sbpid.Append("'" + s + "',");
                        }
                        string sql = string.Format("select {2} from {0} where PointId in ({1}) and PollutantCode in ('a34002','a34004','a21004','a21026','a21005','a05024') and Tstamp <= '{4}' and Tstamp >= '{3}'", tbname, sbpid.ToString().TrimEnd(','), fieldName, dtstartnew, dtendnew);
                        sbnew.Append(sql);
                    }
                    DataView dvnew = g_DatabaseHelper.ExecuteDataView(sbnew.ToString(), con);
                    return dvnew;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 查询AQI数据提供给接口
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段(PointId,DateTime)</param>
        /// <returns></returns>
        public DataView GetOriHourDataForData(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
            recordTotal = 0;

            string fieldName = @"PointId
                        ,DateTime
                        ,AQIValue
                        ,Class
                        ,Grade
                        ,PrimaryPollutant
                        ,RGBValue
                        ,HealthEffect
                        ,TakeStep";
            string keyName = "id";

            //查询条件拼接
            string where = string.Empty;
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                where = " PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                where = " PointId IN(" + portIdsStr + ")";
            }
            where += string.Format(" AND DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            string OritableName = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.OriHourAQI);
            string Oriconnection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
            return g_GridViewPager.GetGridViewPager(OritableName, fieldName, keyName, pageSize, pageNo, orderBy, where, Oriconnection, out recordTotal);
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段(PointId,DateTime)</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
            recordTotal = 0;

            string fieldName = @"PointId pid,DateTime dt,SO2,SO2_IAQI,NO2,NO2_IAQI,PM10,PM10_IAQI,Recent24HoursPM10,Recent24HoursPM10_IAQI,CO,CO_IAQI,O3,O3_IAQI,Recent8HoursO3,Recent8HoursO3_IAQI,Recent8HoursO3NT,Recent8HoursO3NT_IAQI,PM25,PM25_IAQI,Recent24HoursPM25,Recent24HoursPM25_IAQI,AQIValue,PrimaryPollutant,Range,RGBValue,PicturePath,Class,Grade,HealthEffect,TakeStep";
            string keyName = "id";
            string conn = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
            //查询条件拼接
            string where = string.Empty;
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                where = " PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                where = " PointId IN(" + portIdsStr + ")";
            }
            //DateTime dtEndFillNull = dtEnd > DateTime.Now ? DateTime.Now.AddHours(-1) : dtEnd.AddHours(-1);
            DateTime dtEndFillNull = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                    DateTime.Now.AddHours(-1) : dtEnd;
            where += string.Format(" AND DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEndFillNull.ToString("yyyy-MM-dd HH:mm:ss"));
            string allHoursSql = string.Format(@"select time.PointId,time.Tstamp DateTime,data.* from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", "", StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEndFillNull.ToString("yyyy-MM-dd HH:mm:ss"));
            string sql = string.Format("{3} left join (select {0} from {1} where {2}) data on convert(varchar(13),time.Tstamp,120)=convert(varchar(13),data.dt,120) and time.pointId=data.pid", fieldName, "[AMS_MonitorBusiness].[AirRelease].[TB_HourAQI]", where, allHoursSql);
            DataView dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, conn, out recordTotal);
            return dv;
            //return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据),计算原始小时数据AQI
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段(PointId,DateTime)</param>
        /// <returns></returns>
        public DataView GetOriDataPager(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
            recordTotal = 0;

            string fieldName = @"PointId pid,DateTime dt,SO2,SO2_IAQI,NO2,NO2_IAQI,PM10,PM10_IAQI,Recent24HoursPM10,Recent24HoursPM10_IAQI,CO,CO_IAQI,O3,O3_IAQI,Recent8HoursO3,Recent8HoursO3_IAQI,Recent8HoursO3NT,Recent8HoursO3NT_IAQI,PM25,PM25_IAQI,Recent24HoursPM25,Recent24HoursPM25_IAQI,AQIValue,PrimaryPollutant,Range,RGBValue,PicturePath,Class,Grade,HealthEffect,TakeStep";
            string keyName = "id";

            //查询条件拼接
            string where = string.Empty;
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                where = " PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                where = " PointId IN(" + portIdsStr + ")";
            }
            //DateTime dtEndFillNull = dtEnd > DateTime.Now ? DateTime.Now.AddHours(-1) : dtEnd.AddHours(-1);
            DateTime dtEndFillNull = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                    DateTime.Now.AddHours(-1) : dtEnd;
            where += string.Format(" AND DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            string OritableName = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.OriHourAQI);
            string Oriconnection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
            string allHoursSql = string.Format(@"select time.PointId,time.Tstamp DateTime,data.* from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", ""
                , StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEndFillNull.ToString("yyyy-MM-dd HH:mm:ss"));
            string sql = string.Format("{3} left join (select {0} from {1} where {2}) data on convert(varchar(13),time.Tstamp,120)=convert(varchar(13),data.dt,120) and time.pointId=data.pid", fieldName, OritableName, where, allHoursSql);
            DataView dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, Oriconnection, out recordTotal);
            return dv;
            //return g_GridViewPager.GetGridViewPager(OritableName, fieldName, keyName, pageSize, pageNo, orderBy, where, Oriconnection, out recordTotal);
        }

        /// <summary>
        /// 获取臭氧每天的最大浓度值
        /// </summary>
        public DataView GetOriDataPager(string portIds, DateTime dtStart, DateTime dtEnd)
        {
            string OritableName = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.OriHourAQI);
            string Oriconnection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
            string sql = string.Format(@"select PointId,CONVERT(varchar(10),DateTime,121) DateTime,Max([Recent8HoursO3]) Recent8HoursO3
                              FROM {3}
                              WHERE DateTime>='{1}' and DateTime<='{2}' and PointId={0}
                              group by PointId,CONVERT(varchar(10),DateTime,121)", portIds, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), OritableName);
            DataView dv = g_DatabaseHelper.ExecuteDataView(sql, Oriconnection);
            return dv;
        }

        /// <summary>
        /// 获取臭氧(NT)最大浓度值
        /// </summary>
        public DataView GetOriDataPagerO3ForNT(string[] portIds, DateTime dtStart, DateTime dtEnd, string type)
        {
            if (type.Equals("OriData"))
            {
                string OritableName = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.OriHourAQI);
                string Oriconnection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
                string sql = string.Format(@"select CONVERT(varchar(13),DateTime,121) DateTime,AVG(Convert(decimal(18,5),Recent8HoursO3NT)) Recent8HoursO3
                                            from {3}
                                            WHERE DateTime>='{1}' and DateTime<='{2}' and PointId in ({0}) and Recent8HoursO3NT is not null and Recent8HoursO3NT !=''
                                            group by CONVERT(varchar(13),DateTime,121)", "'" + string.Join("','", portIds) + "'", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), OritableName);
                DataView dv = g_DatabaseHelper.ExecuteDataView(sql, Oriconnection);
                return dv;
            }
            else
            {
                string Oriconnection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
                string sql = string.Format(@"select CONVERT(varchar(13),DateTime,121) DateTime,AVG(Convert(decimal(18,5),Recent8HoursO3NT)) Recent8HoursO3
                                            from {3}
                                            WHERE DateTime>='{1}' and DateTime<='{2}' and PointId in ({0}) and Recent8HoursO3NT is not null and Recent8HoursO3NT !=''
                                            group by CONVERT(varchar(13),DateTime,121)", "'" + string.Join("','", portIds) + "'", dtStart, dtEnd, "AirRelease.TB_HourAQI");
                DataView dv = g_DatabaseHelper.ExecuteDataView(sql, Oriconnection);
                return dv;
            }
            //dbo.SY_OriHourAQI
        }

        /// <summary>
        /// 针对南通数据需获取区域数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetOriRTData(string[] portIds, DateTime dtStart, DateTime dtEnd, string orderBy, string isCheck)
        {
            string tbname = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.OriHourAQI);
            string con = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
            string fieldName = @"SO2
                    ,SO2_IAQI
                    ,NO2
                    ,NO2_IAQI
                    ,PM10
                    ,PM10_IAQI
                    ,CO
                    ,CO_IAQI
                    ,O3
                    ,O3_IAQI
                    ,PM25
                    ,PM25_IAQI
                    ,AQIValue
                    ,PrimaryPollutant
                    ,RGBValue
                    ,Class
                    ,Grade";
            StringBuilder sb = new StringBuilder();
            foreach (string pid in portIds)
            {
                sb.Append("'" + pid + "',");
            }
            string sql = string.Empty;
            if (isCheck.Equals("1"))
            {
                sql = string.Format(@"
                                            select PointId,DateTime,{4} from {0} 
                                            where PointId in ({1}) and DateTime >= '{2}' and DateTime <= '{3}'
                                            union 
                                            select * from (
select 999 as PointId,DateTime,AVG(CONVERT(decimal(6,4),SO2)) SO2
,[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),SO2)),'a21026',24) SO2_IAQI
,AVG(CONVERT(decimal(6,4),NO2)) NO2
,[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),SO2)),'a21004',24) NO2_IAQI
,AVG(CONVERT(decimal(6,4),PM10)) PM10
,[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),PM10)),'a34002',24) PM10_IAQI
,AVG(CONVERT(decimal(6,4),CO)) CO
,[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),CO)),'a21005',24) CO_IAQI
,AVG(CONVERT(decimal(6,4),O3)) O3
,[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),O3)),'a05024',1) O3_IAQI
,AVG(CONVERT(decimal(6,4),PM25)) PM25
,[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),PM25)),'a34004',24) PM25_IAQI
,[dbo].[SY_F_GetAQI_Max_CNV_Day] ([dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),SO2)),'a21026',24),[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),SO2)),'a21004',24),[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),PM10)),'a34002',24),[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),CO)),'a21005',24),[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),O3)),'a05024',1),[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),PM25)),'a34004',24),'V') AQIValue
,[dbo].[SY_F_GetAQI_Max_CNV_Day] ([dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),SO2)),'a21026',24),[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),SO2)),'a21004',24),[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),PM10)),'a34002',24),[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),CO)),'a21005',24),[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),O3)),'a05024',1),[dbo].[SY_f_getAQI](AVG(CONVERT(decimal(6,4),PM25)),'a34004',24),'N') PrimaryPollutant
,'--' RGBValue
,'--' Class
,'--' Grade from {0}
where PointId in ({1}) and DateTime <= '{3}' and DateTime >= '{2}'
group by DateTime) A
                                            order by PointId,DateTime Asc
                                            ", tbname, sb.ToString().Trim(','), dtStart, dtEnd, fieldName);
            }
            else
            {
                sql = string.Format(@"select PointId,DateTime,{4} from {0} 
                                    where PointId in ({1}) and DateTime >= '{2}' and DateTime <= '{3}'
                                    order by PointId,DateTime Asc
                                    ", tbname, sb.ToString().Trim(','), dtStart, dtEnd, fieldName);
            }
            return g_DatabaseHelper.ExecuteDataView(sql, con);
        }

        /// <summary>
        /// 获取空气质量原始小时数据实时报,南通市辖区区域专用方法
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段(PointId,DateTime)</param>
        /// <returns></returns>
        public DataTable GetRegionOriDataPager(DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, string OorA, out int recordTotal, string orderBy = "DateTime desc")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "DateTime desc";
            recordTotal = 0;

            string fieldName = @" '南通市区' pid,DateTime dt,SO2,SO2_IAQI,NO2,NO2_IAQI,PM10,PM10_IAQI,Recent24HoursPM10,Recent24HoursPM10_IAQI,CO,CO_IAQI,O3,O3_IAQI,Recent8HoursO3,Recent8HoursO3_IAQI,Recent8HoursO3NT,Recent8HoursO3NT_IAQI,PM25,PM25_IAQI,Recent24HoursPM25,Recent24HoursPM25_IAQI,AQIValue,PrimaryPollutant,Range,RGBValue,PicturePath,Class,Grade,HealthEffect,TakeStep";
            string tbname = "";
            string conn = "";
            if (OorA == "Audit")
            {
                tbname = " [AMS_MonitorBusiness].[AirRelease].[TB_RegionHourAQI] ";
                //conn = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
                conn = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
            }
            else if (OorA == "Ori")
            {
                tbname = " [AMS_AirAutoMonitor].[Air].[TB_OriRegionHourAQI] ";
                conn = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
            }
            //DateTime dtEndFillNull = dtEnd > DateTime.Now ? DateTime.Now.AddHours(-1) : dtEnd.AddHours(-1);
            DateTime dtEndFillNull = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                    DateTime.Now.AddHours(-1) : dtEnd;
            string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' AND MonitoringRegionUid='b6e983c4-4f92-4be3-bbac-d9b71c470640' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            string allHoursSql = string.Format(@"select time.PointId,time.Tstamp DateTime,data.* from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", "", "南通市区", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEndFillNull.ToString("yyyy-MM-dd HH:mm:ss"));
            string sql = string.Format("{3} left join (select {0} from {1} where {2}) data on convert(varchar(13),time.Tstamp,120)=convert(varchar(13),data.dt,120) and time.pointId=data.pid", fieldName, tbname, where, allHoursSql);
            DataView dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, conn, out recordTotal);
            return dv.Table;
            //string sql = string.Format("select {0} from {1} where {2} order by {3}", fieldName, tbname, where, orderBy);
            //DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, conn);
            //recordTotal = dt.Rows.Count;
            //return dt;
        }

        /// <summary>
        /// 查询测点最新一条原始小时数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetAirQualityNewestOriRTReport(string[] portIds, int pageSize, int pageNo, out int recordTotal, DateTime dt, string orderBy = "DateTime,PointId")
        {
            try
            {
                DateTime dt1 = dt.AddHours(-1);
                string tbname = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.OriHourAQI);
                string con = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
                StringBuilder sb = new StringBuilder();
                foreach (string pid in portIds)
                {
                    sb.Append("'" + pid + "',");
                }
                string fieldName = @"b.PointId
                    ,DateTime
                    ,SO2
                    ,SO2_IAQI
                    ,NO2
                    ,NO2_IAQI
                    ,PM10
                    ,PM10_IAQI
                    ,CO
                    ,CO_IAQI
                    ,O3
                    ,O3_IAQI
                    ,PM25
                    ,PM25_IAQI
                    ,AQIValue
                    ,PrimaryPollutant
                    ,RGBValue
                    ,Class
                    ,Grade";
                string sql = string.Format("select {0} from {1} as a right join (select distinct PointId from {1} where PointId in ({4})) as b on a.PointId = b.PointId and DateTime <= '{2}' and DateTime > '{3}' order by {5}", fieldName, tbname, dt.ToString("yyyy-MM-dd HH:mm:ss"), dt1.ToString("yyyy-MM-dd HH:mm:ss"), sb.ToString().Trim(','), orderBy);
                DataView dv = g_DatabaseHelper.ExecuteDataView(sql, con);
                //判断是否有遗漏
                //if (dv == null || dv.Count < portIds.Length)
                //{
                //    if (dv == null)
                //    {
                //        StringBuilder sb = new StringBuilder();
                //        sb.Append("select * from (");
                //        for (int i = 0; i < portIds.Length - 1; i++)
                //        {
                //            string sqlC = string.Format("(select top 1 {3} from {0} where DateTime <='{2}' and PointId = {1} order by DateTime desc) union ", tbname, portIds[i], dt, fieldName);
                //            sb.Append(sqlC);
                //        }
                //        string sqllast = string.Format("(select top 1 {3} from {0} where DateTime <='{2}' and PointId = {1} order by DateTime desc)) A", tbname, portIds[portIds.Length - 1], dt, fieldName);
                //        sb.Append(sqllast);
                //        dv = g_DatabaseHelper.ExecuteDataView(sb.ToString(), con);
                //    }
                //    else
                //    {
                //        List<string> list = dv.Table.AsEnumerable().Select(d => Convert.ToString(d.Field<int>("PointId"))).ToList<string>();
                //        var expectedArray = portIds.ToList().Except(list).ToArray();
                //        StringBuilder sb = new StringBuilder();
                //        if (expectedArray.Length > 1)
                //        {
                //            sb.Append("select * from (");
                //            for (int i = 0; i < expectedArray.Length - 1; i++)
                //            {
                //                string sqlC = string.Format("(select top 1 {3} from {0} where DateTime <='{2}' and PointId = {1} order by DateTime desc) union ", tbname, expectedArray[i], dt, fieldName);
                //                sb.Append(sqlC);
                //            }
                //            string sqllast = string.Format("(select top 1 {3} from {0} where DateTime <='{2}' and PointId = {1} order by DateTime desc)) A", tbname, expectedArray[expectedArray.Length - 1], dt, fieldName);
                //            sb.Append(sqllast);
                //        }
                //        else
                //        {
                //            string sqlC = string.Format("select top 1 {3} from {0} where DateTime <='{2}' and PointId = {1} order by DateTime desc ", tbname, expectedArray[0], dt, fieldName);
                //            sb.Append(sqlC);
                //        }
                //        DataView dvC = g_DatabaseHelper.ExecuteDataView(sb.ToString(), con);
                //        dv.Table.Merge(dvC.Table);
                //    }
                //}
                recordTotal = dv.Count;
                return dv;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 查询所选站点当天的小时AQI
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dt2"></param>
        /// <param name="dt"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetOriRTAQI(string[] portIds, DateTime dt2, DateTime dt, string orderBy = "DateTime,PointId")
        {
            try
            {
                string tbname = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.OriHourAQI);
                string con = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
                StringBuilder sb = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                foreach (string pid in portIds)
                {
                    sb.Append("'" + pid + "',");
                    sb2.Append("max(case PointId when " + pid + " then AQIValue END) as [" + pid + "],");
                }
                string sql = string.Format(@"select * from (
                                            select convert(varchar(13),DateTime,120) as DateTime,{4} from {0} 
                                            where PointId in ({1}) and DateTime >= '{2}' and DateTime <= '{3}' group by convert(varchar(13),DateTime,120)) as a 
                                            full join  
                                            (select  convert(varchar(13),DateTime,120)  as dt,AVG(Convert(decimal(6,4),SO2)) SO2
,AVG(Convert(decimal(6,4),NO2)) NO2 
,AVG(Convert(decimal(6,4),PM10)) PM10 
,AVG(Convert(decimal(6,4),CO)) CO 
,AVG(Convert(decimal(6,4),O3)) O3 
,AVG(Convert(decimal(6,4),PM25)) PM25
,'' AQIValue
from  Air.TB_OriHourAQI
                                            where PointId in ({1}) and DateTime <= '{3}' and DateTime >= '{2}' group by DateTime) as b
                                            on b.dt = a.DateTime order by DateTime
                                            ", tbname, sb.ToString().Trim(','), dt2, dt, sb2.ToString().Trim(','));
                return g_DatabaseHelper.ExecuteDataView(sql, con);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 获取站点最新数据的时间
        /// </summary>
        /// <param name="portIds"></param>
        /// <returns></returns>
        public DateTime GetOriAQINewestTime(string[] portIds)
        {
            string tbname = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.OriHourAQI);
            string con = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
            StringBuilder sb = new StringBuilder();
            foreach (string pid in portIds)
            {
                sb.Append("'" + pid + "',");
            }
            string sql = string.Format("select top 1 DateTime from {0} where PointId in ({1}) order by DateTime desc", tbname, sb.ToString().Trim(','));
            DateTime time = (DateTime)g_DatabaseHelper.ExecuteScalar(sql, con);
            return time;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段(PointId,DateTime)</param>
        /// <returns></returns>
        public DataView GetDataHoursPager(string[] portIds, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                //查询条件拼接
                string where = string.Empty;
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    where = " PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    where = " PointId IN(" + portIdsStr + ")";
                }
                where += string.Format(" AND DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                string sql = string.Format(@"select PointId
                                ,SUM(CASE WHEN SO2 is not null THEN 1 ELSE 0 END) AS SO2Count
								,SUM(CASE WHEN NO2 is not null THEN 1 ELSE 0 END) AS NO2Count
								,SUM(CASE WHEN [CO] is not null THEN 1 ELSE 0 END) AS COCount
								,SUM(CASE WHEN [O3] is not null THEN 1 ELSE 0 END) AS MaxO3Count
								,SUM(CASE WHEN [O3] is not null and Convert(varchar(2),datepart(hour,DateTime))>7 THEN 1 ELSE 0 END) AS Max8O3Count
								,SUM(CASE WHEN [PM10] is not null THEN 1 ELSE 0 END) AS PM10Count
								,SUM(CASE WHEN [PM25] is not null THEN 1 ELSE 0 END) AS PM25Count
								FROM {0}
								where {1} 
							    group by PointId
                                order By PointId
               ", tableName, where);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="autoMonitorType">查询数据类型</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段(PointId,DateTime)</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;

            string fieldName = GetFieldName();
            string keyName = "id";
            //查询条件拼接
            string where = string.Empty;
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                where = " PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                where = " PointId IN(" + portIdsStr + ")";
            }
            where += string.Format(" AND DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));

            return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
        }

        /// <summary>
        /// 根据站点取得最新小时AQI数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetLastData(string[] portIds, DateTime dtStart, DateTime dtEnd, string orderBy = "portId,Tstamp Desc")
        {
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                //拼接where条件
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND ( SO2 is not null  OR NO2 is not null  OR PM10 is not null  OR CO is not null  OR O3 is not null  AND PM25 is not null )",
                    dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

                string sql = string.Format(@"
                WITH lastData AS
                (
                    SELECT PointId AS portId
	                    ,MAX(DateTime) AS Tstamp
                    FROM {0}
                    where {1}
                    GROUP BY PointId
                )
                SELECT {2}
                FROM lastData
                INNER JOIN {0} AS data
	                ON lastData.portId = data.PointId AND lastData.Tstamp = data.DateTime
                    ORDER BY {3}
                ", tableName, where, GetFieldName(), orderBy);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 取得全月小时数
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetAvgDayData(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                string sql = string.Format(@"select  a.PointId
								,SUM(CASE WHEN [SO] >0 THEN 1 ELSE 0 END) AS SO2Count
								,SUM(CASE WHEN [NO] >0 THEN 1 ELSE 0 END) AS NO2Count
								,SUM(CASE WHEN [CO] >0 THEN 1 ELSE 0 END) AS COCount
								,SUM(CASE WHEN [MaxOneO3] >0 THEN 1 ELSE 0 END) AS MaxO3Count
								,SUM(CASE WHEN [MaxO3] >0 and Convert(varchar(2),datepart(hour,DateTime))>7 THEN 1 ELSE 0 END) AS Max8O3Count
								,SUM(CASE WHEN [PM10] >0 THEN 1 ELSE 0 END) AS PM10Count
								,SUM(CASE WHEN [PM25] >0 THEN 1 ELSE 0 END) AS PM25Count
								,Max([SO]) as SO2Max
								,Max([NO]) as NO2Max
								,Max([CO]) as COMax
								,Max([MaxOneO3]) as MaxO3Max
								,Max([MaxO3]) as Max8O3Max
								,Max([PM10]) as PM10Max
								,Max(PM25) as PM25Max
								from
								(
								select PointId
                                ,DateTime
								,CONVERT (decimal(18,6),[SO2]) as [SO]
								,CONVERT (decimal(18,6),[NO2]) as [NO]
								,CONVERT (decimal(18,6),[CO]) as [CO]
								,CONVERT (decimal(18,6),[O3]) as [MaxOneO3]
								,CONVERT (decimal(18,6),[O3]) as [MaxO3]
								,CONVERT (decimal(18,6),[PM10]) as [PM10]
								,CONVERT (decimal(18,6),PM25) as PM25
								FROM {0}
								where  DateTime>='{1}' AND DateTime<='{2}' {3} and
								([SO2] is not null or NO2 is not null or CO is not null or  O3 is not null or PM10 is not null or PM25 is not null )
								) a group by PointId
               ", tableName, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得查询字段
        /// </summary>
        /// <param name="aQIDataType"></param>
        /// <returns></returns>
        private string GetFieldName()
        {
            string fieldName = @"PointId
                    ,DateTime
                    ,SO2
                    ,SO2_IAQI
                    ,NO2
                    ,NO2_IAQI
                    ,PM10
                    ,PM10_IAQI
                    ,Recent24HoursPM10
                    ,Recent24HoursPM10_IAQI
                    ,CO
                    ,CO_IAQI
                    ,O3
                    ,O3_IAQI
                    ,Recent8HoursO3
                    ,Recent8HoursO3_IAQI
                    ,Recent8HoursO3NT
                    ,Recent8HoursO3NT_IAQI
                    ,PM25
                    ,PM25_IAQI
                    ,Recent24HoursPM25
                    ,Recent24HoursPM25_IAQI
                    ,AQIValue
                    ,PrimaryPollutant
                    ,Range
                    ,RGBValue
                    ,PicturePath
                    ,Class
                    ,Grade
                    ,HealthEffect
                    ,TakeStep";
            return fieldName;
        }
        #endregion

        #region << 环境质量统计 >>
        /// <summary>
        /// 各等级天数统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="portIds">测点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点
        /// Level1Count：一级次数
        /// Level2Count：二级次数
        /// Level3Count：三级次数
        /// Level4Count：四级次数
        /// Level5Count：五级次数
        /// Level6Count：六级次数
        /// FineCount：优良次数
        /// OverCount：超标次数
        /// ValidCount：有效次数
        /// </returns>
        public DataView GetGradeStatistics(IAQIType aqiType, string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length > 0) portIdsStr = string.Format(" and PointId IN ({0}) ", portIdsStr);
                string sql = string.Format(@"
                SELECT PointId
                    ,SUM(CASE WHEN {0} >= 0 AND {0}<=50 THEN 1 ELSE 0 END) AS Level1Count
                    ,SUM(CASE WHEN {0} > 50 AND {0}<=100 THEN 1 ELSE 0 END) AS Level2Count
                    ,SUM(CASE WHEN {0} > 100 AND {0}<=150 THEN 1 ELSE 0 END) AS Level3Count
                    ,SUM(CASE WHEN {0} > 150 AND {0}<=200 THEN 1 ELSE 0 END) AS Level4Count
                    ,SUM(CASE WHEN {0} > 200 AND {0}<=300 THEN 1 ELSE 0 END) AS Level5Count
                    ,SUM(CASE WHEN {0} > 300 THEN 1 ELSE 0 END) AS Level6Count
                    ,SUM(CASE WHEN {0} >= 0 AND {0} <= 100 THEN 1 ELSE 0 END) AS FineCount
                    ,SUM(CASE WHEN {0} > 100 THEN 1 ELSE 0 END) AS OverCount
                    ,SUM(CASE WHEN {0} >= 0 THEN 1 ELSE 0 END) AS ValidCount
                FROM {1}
                WHERE DateTime>='{2}' AND DateTime<='{3}' {4}
                GROUP BY PointId ", SmartEP.Core.Enums.EnumMapping.GetIAQITypeColumn(aqiType), tableName, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 各污染物首要污染物统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="portIds">测点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// Count：首要污染物次数
        /// </returns>
        public DataView GetContaminantsStatistics(IAQIType aqiType, string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length > 0) portIdsStr = string.Format(" and PointId IN ({0}) ", portIdsStr);
            string sql = string.Format(@"
                SELECT PointId
                    ,SUM(CASE WHEN {0}=AQIValue AND AQIValue>50 THEN 1 ELSE 0 END) AS Count
                FROM {1}
                WHERE DateTime>='{2}' AND DateTime<='{3}' {4}
                GROUP BY PointId ", SmartEP.Core.Enums.EnumMapping.GetIAQITypeColumn(aqiType), tableName, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
        #endregion

        #region << 生成点位小时AQI >>
        /// <summary>
        /// 生成点位小时AQI
        /// </summary>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="portIds">站点列表</param>
        public void ExportAQIData(DateTime dateStart, DateTime dateEnd, string[] portIds)
        {
            try
            {
                if (portIds == null || portIds.Length == 0)
                    return;
                g_DBBiz.ClearParameters();
                SqlParameter pramViewName = new SqlParameter();
                pramViewName = new SqlParameter();
                pramViewName.SqlDbType = SqlDbType.DateTime;
                pramViewName.ParameterName = "@m_begin";
                pramViewName.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramViewName);

                SqlParameter pramFieldName = new SqlParameter();
                pramFieldName = new SqlParameter();
                pramFieldName.SqlDbType = SqlDbType.DateTime;
                pramFieldName.ParameterName = "@m_end";
                pramFieldName.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramFieldName);

                SqlParameter pramKeyName = new SqlParameter();
                pramKeyName = new SqlParameter();
                pramKeyName.SqlDbType = SqlDbType.NVarChar;
                pramKeyName.ParameterName = "@m_portlist";
                pramKeyName.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","); ;
                g_DBBiz.SetProcedureParameters(pramKeyName);

                g_DBBiz.ExecuteProcNonQuery("UP_AirReport_HourAQI_Port_Mul", connection);
            }
            catch (Exception ex)
            {
                //return null;
                throw ex;
            }
        }
        #endregion

        #region 空气质量日报统计专用方法
        /// <summary>
        /// 获取时间段内指定测点的小时数据
        /// </summary>
        /// <param name="PointIds">测点ID数组</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetHourAQIData(List<int> PointIds, DateTime StartDate, DateTime EndDate)
        {
            if (PointIds == null || PointIds.Count == 0 || StartDate == null || EndDate == null || EndDate < StartDate)
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select b.MonitoringPointName as PointName,a.DateTime,a.SO2,a.SO2_IAQI,a.NO2,a.NO2_IAQI,a.PM10,a.PM10_IAQI,a.CO,a.CO_IAQI,a.O3,a.O3_IAQI");
            strSql.Append(",a.PM25,a.PM25_IAQI,a.AQIValue,a.PrimaryPollutant,a.Grade,a.Class,a.RGBValue ");
            strSql.Append("from " + tableName + " as a ");
            strSql.Append("inner join dbo.SY_MonitoringPoint as b on a.PointId=b.PointId ");
            strSql.Append("where a.DateTime>='" + StartDate.ToString("yyyy-MM-dd HH:00:00") + "' and a.DateTime<='" + EndDate.ToString("yyyy-MM-dd HH:00:00") + "' ");
            if (PointIds.Count == 1)
            {
                strSql.Append("and a.PointId='" + PointIds[0] + "' ");
            }
            else
            {
                strSql.Append("and (");
                for (int i = 0; i < PointIds.Count; i++)
                {
                    if (i == 0)
                    {
                        strSql.Append(" a.PointId='" + PointIds[i] + "' ");
                    }
                    else
                    {
                        strSql.Append(" or a.PointId='" + PointIds[i] + "' ");
                    }
                }
                strSql.Append(")");
            }
            strSql.Append("order by b.OrderByNum desc,a.DateTime desc ");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /// <summary>
        /// 计算时间段内指定测点监测因子的浓度及分指数平均值
        /// </summary>
        /// <param name="PointIds">测点ID数组</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetAvgHourAQIData(List<int> PointIds, DateTime StartDate, DateTime EndDate)
        {
            if (PointIds == null || PointIds.Count == 0 || StartDate == null || EndDate == null || EndDate < StartDate)
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CONVERT(decimal(18,4),AVG(CAST ((case SO2 when null then '0' else SO2 end) as decimal(18,4)))) as AVGSO2 ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case SO2 when null then '0' else SO2 end) as decimal(18,4)))),'a21026',1) as AVGSO2_IAQI ");
            strSql.Append(",CONVERT(decimal(18,4),AVG(CAST ((case NO2 when null then '0' else NO2 end) as decimal(18,4)))) as AVGNO2 ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case NO2 when null then '0' else NO2 end) as decimal(18,4)))),'a21004',1) as AVGNO2_IAQI ");
            strSql.Append(",CONVERT(decimal(18,4),AVG(CAST ((case PM10 when null then '0' else PM10 end) as decimal(18,4)))) as AVGPM10 ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case PM10 when null then '0' else PM10 end) as decimal(18,4)))),'a34002',24) as AVGPM10_IAQI ");
            strSql.Append(",CONVERT(decimal(18,4),AVG(CAST ((case CO when null then '0' else CO end) as decimal(18,4)))) as AVGCO ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case CO when null then '0' else CO end) as decimal(18,4)))),'a21005',1) as AVGCO_IAQI ");
            strSql.Append(",CONVERT(decimal(18,4),AVG(CAST ((case O3 when null then '0' else O3 end) as decimal(18,4)))) as AVGO3 ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case O3 when null then '0' else O3 end) as decimal(18,4)))),'a05024',1) as AVGO3_IAQI ");
            strSql.Append(",CONVERT(decimal(18,4),AVG(CAST ((case Recent8HoursO3 when null then '0' else Recent8HoursO3 end) as decimal(18,4)))) as AVGO3_8 ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case Recent8HoursO3 when null then '0' else Recent8HoursO3 end) as decimal(18,4)))),'a05024',8) as AVGO3_8_IAQI ");
            strSql.Append(",CONVERT(decimal(18,4),AVG(CAST ((case PM25 when null then '0' else PM25 end) as decimal(18,4)))) as AVGPM25 ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case PM25 when null then '0' else PM25 end) as decimal(18,4)))),'a34004',24) as AVGPM25_IAQI ");
            strSql.Append(",dbo.F_GetAQI_Max_CNV_Hour(dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case SO2 when null then '0' else SO2 end) as decimal(18,4)))),'a21026',1)");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case NO2 when null then '0' else NO2 end) as decimal(18,4)))),'a21004',1)");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case PM10 when null then '0' else PM10 end) as decimal(18,4)))),'a34002',24)");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case CO when null then '0' else CO end) as decimal(18,4)))),'a21005',1)");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case O3 when null then '0' else O3 end) as decimal(18,4)))),'a05024',1)");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case Recent8HoursO3 when null then '0' else Recent8HoursO3 end) as decimal(18,4)))),'a05024',8)");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case PM25 when null then '0' else PM25 end) as decimal(18,4)))),'a34004',24)");
            strSql.Append(",'V') as AQI_MaxValue ");
            strSql.Append("from " + tableName + " ");
            strSql.Append("where DateTime>='" + StartDate.ToString("yyyy-MM-dd HH:00:00") + "' and DateTime<='" + EndDate.ToString("yyyy-MM-dd HH:00:00") + "' ");
            if (PointIds.Count == 1)
            {
                strSql.Append("and PointId='" + PointIds[0] + "' ");
            }
            else
            {
                strSql.Append("and (");
                for (int i = 0; i < PointIds.Count; i++)
                {
                    if (i == 0)
                    {
                        strSql.Append(" PointId='" + PointIds[i] + "' ");
                    }
                    else
                    {
                        strSql.Append(" or PointId='" + PointIds[i] + "' ");
                    }
                }
                strSql.Append(")");
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion
    }
}
