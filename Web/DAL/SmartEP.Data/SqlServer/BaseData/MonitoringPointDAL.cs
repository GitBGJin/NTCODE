﻿using log4net;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.BaseData
{
    /// <summary>
    /// 名称：MonitoringPointDAL.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-10-13
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-05-24
    /// 功能摘要：
    /// 监测点信息数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MonitoringPointDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);

        //获取一个日志记录器
        ILog log = LogManager.GetLogger("FileLogging");
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public MonitoringPointDAL()
        {
            //connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Hour);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 获取测点数据
        /// </summary>
        /// <param name="dataType">测点类型（All或者空是全部）</param>
        /// <returns></returns>
        public DataTable GetMonitoringPointDataTable(ApplicationType? applicationType = null)//, string enableOrNot = "true"
        {
            //bool isEnableOrNot = (enableOrNot.Trim().ToUpper() == "TRUE") ? true : false;
            DataTable dt = new DataTable();
            string sql = " Select * From MPInfo.TB_MonitoringPoint ";
            string where = " where 1=1 ";
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            if (applicationType == ApplicationType.Air)
            {
                //如果dataType是Air，则不需要地表水的测点数据
                where += " and ApplicationUid!='watrwatr-watr-watr-watr-watrwatrwatr' ";
            }
            else if (applicationType == ApplicationType.Water)
            {
                //如果dataType是Water，则不需要空气的测点数据
                where += " and ApplicationUid!='airaaira-aira-aira-aira-airaairaaira' ";
            }
            sql += where;
            dt = g_DatabaseHelper.ExecuteDataTable(sql, connection);

            return dt;
        }

        /// <summary>
        /// 获取测点数据提供给接口(显示国控、省控、市控等)
        /// </summary>
        /// <param name="dataType">测点类型（All或者空是全部）</param>
        /// <returns></returns>
        public DataTable GetMonitoringPointDataTableForData(ApplicationType? applicationType = null)//, string enableOrNot = "true"
        {
            //bool isEnableOrNot = (enableOrNot.Trim().ToUpper() == "TRUE") ? true : false;
            DataTable dt = new DataTable();
            string sql = " Select A.MonitoringPointUid,A.PointId,A.MonitoringPointName,A.X,A.Y,B.PName,B.CGuid From MPInfo.TB_MonitoringPoint A, dbo.V_Point_Air_SiteMap_Property B ";
            string where = " where 1=1 ";
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            where += " and A.ContrlUid = B.CGuid ";
            sql += where;
            dt = g_DatabaseHelper.ExecuteDataTable(sql, connection);

            return dt;
        }

        /// <summary>
        /// 获取测点信息提供给接口(显示国控、省控、市控，在线信息等)
        /// </summary>
        /// <param name="dataType">测点类型（All或者空是全部）</param>
        /// <returns></returns>
        public DataTable GetPortMessageForData(string[] pointId, string type, string factor, DateTime dtbegin, DateTime dtend)//, string enableOrNot = "true"
        {
            //bool isEnableOrNot = (enableOrNot.Trim().ToUpper() == "TRUE") ? true : false;
            DataTable dt = new DataTable();
            string where = string.Empty;
            if (pointId[0] != "ALL")
            {
                StringBuilder sb = new StringBuilder();
                foreach (string s in pointId)
                {
                    sb.Append("'" + s + "',");
                }
                string field = string.Format(@"a.PointId,b.MonitoringPointName,b.X,b.Y,c.PName,
                                               Class as level, CONVERT(varchar(10),DateTime,121) DateTime");
                string sql = string.Format(@"select {2} from {0} a 
                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId left join [dbo].[dbo.SY_V_Point_Air_Property] c on c.CGuid = b.ContrlUid
                                            where a.PointId in ({1}) and DateTime>='{3}' and DateTime <='{4}' order by a.PointId,DateTime", "[AMS_AirAutoMonitor].[Air].[TB_OriDayAQI]", sb.ToString().Trim(','), field, dtbegin, dtend);
                dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");
            }
            else
            {
                string field = string.Format(@"a.PointId,b.MonitoringPointName,b.X,b.Y,c.PName,
                                               Class as level, CONVERT(varchar(10),DateTime,121) DateTime");
                string sql = string.Format(@"select {2} from {0} a 
                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId left join [dbo].[dbo.SY_V_Point_Air_Property] c on c.CGuid = b.ContrlUid
                                            where DateTime>='{3}' and DateTime <='{4}' order by a.PointId,DateTime ", "[AMS_AirAutoMonitor].[Air].[TB_OriDayAQI]", "", field, dtbegin, dtend);
                dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");
            }
            return dt;
        }

        /// <summary>
        /// 获取测点信息提供给接口(显示国控、省控、市控，在线信息等)
        /// </summary>
        /// <param name="dataType">测点类型（All或者空是全部）</param>
        /// <returns></returns>
        public DataTable GetPortMessageForData(string[] pointId, string type, string factor)//, string enableOrNot = "true"
        {
            //bool isEnableOrNot = (enableOrNot.Trim().ToUpper() == "TRUE") ? true : false;
            DataTable dt = new DataTable();
            string tbname = "[AMS_AirAutoMonitor].[Air].[TB_OriHourAQI]";
            string where = string.Empty;

            if (type.Equals("IAQI"))
            {
                if (pointId[0] != "ALL")
                {
                    string field = string.Format(@"B.PointId, {0} AS Value,{1} as IAQI,C.MonitoringPointName,C.X,C.Y,
                                            case when ({1} > 0 and {1} < 50) then '优' 
                                             when ({1} >= 51 and {1} <= 100) then '良' 
                                             when ({1} >= 101 and {1} <= 150) then '轻度污染' 
                                             when ({1} >= 151 and {1} <= 200) then '中度污染' 
                                             when ({1} >= 201 and {1} <= 300) then '重度污染' 
                                             when ({1} >= 301) then '严重污染' 
                                            else '无效天' 
                                            end as level, CONVERT(varchar(13),B.DateTime,121) DateTime", factor, factor + "_IAQI");
                    string sql = string.Format(@"select {1} from 
(select pointid,max(DateTime) DateTime from {0} group by pointid) A
left join {0} B 
on B.DateTime = A.DateTime and B.PointId = A.pointid
left join [dbo].[SY_MonitoringPoint] C on A.PointId = C.PointId where A.PointId in ({2})", tbname, field, "'" + string.Join("','", pointId) + "'");
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");

                    //                    if (pointId.Length == 1)
                    //                    {
                    //                        string field = string.Format(@"a.PointId, {0} AS Value,{1} as IAQI,b.MonitoringPointName,b.X,b.Y,
                    //                                            case when ({1} > 0 and {1} < 50) then '优' 
                    //                                             when ({1} >= 51 and {1} <= 100) then '良' 
                    //                                             when ({1} >= 101 and {1} <= 150) then '轻度污染' 
                    //                                             when ({1} >= 151 and {1} <= 200) then '中度污染' 
                    //                                             when ({1} >= 201 and {1} <= 300) then '重度污染' 
                    //                                             when ({1} >= 301) then '严重污染' 
                    //                                            else '无效天' 
                    //                                            end as level, CONVERT(varchar(13),DateTime,121) DateTime", factor, factor + "_IAQI");
                    //                        string sql = string.Format(@"select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc", tbname, pointId[0], field);
                    //                        dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");
                    //                    }
                    //                    else
                    //                    {
                    //                        StringBuilder sb = new StringBuilder();
                    //                        string field = string.Format(@"a.PointId, {0} AS Value,{1} as IAQI,b.MonitoringPointName,b.X,b.Y,
                    //                                            case when ({1} > 0 and {1} < 50) then '优' 
                    //                                             when ({1} >= 51 and {1} <= 100) then '良' 
                    //                                             when ({1} >= 101 and {1} <= 150) then '轻度污染' 
                    //                                             when ({1} >= 151 and {1} <= 200) then '中度污染' 
                    //                                             when ({1} >= 201 and {1} <= 300) then '重度污染' 
                    //                                             when ({1} >= 301) then '严重污染' 
                    //                                            else '无效天' 
                    //                                            end as level, CONVERT(varchar(13),DateTime,121) DateTime", factor, factor + "_IAQI");
                    //                        sb.Append("select * from (");
                    //                        for (int i = 0; i < pointId.Length - 1; i++)
                    //                        {
                    //                            string pid = pointId[i];
                    //                            string sql = string.Format(@"(select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc ) union ", tbname, pid, field);
                    //                            sb.Append(sql);
                    //                        }
                    //                        string pid2 = pointId[pointId.Length - 1];
                    //                        string sql3 = string.Format(@"(select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc )) A ", tbname, pid2, field);
                    //                        sb.Append(sql3);
                    //                        dt = g_DatabaseHelper.ExecuteDataTable(sb.ToString(), "AMS_MonitoringBusinessConnection");
                    //                    }
                }
                else
                {
                    string field = string.Format(@"B.PointId, {0} AS Value,{1} as IAQI,C.MonitoringPointName,C.X,C.Y,
                                            case when ({1} > 0 and {1} < 50) then '优' 
                                             when ({1} >= 51 and {1} <= 100) then '良' 
                                             when ({1} >= 101 and {1} <= 150) then '轻度污染' 
                                             when ({1} >= 151 and {1} <= 200) then '中度污染' 
                                             when ({1} >= 201 and {1} <= 300) then '重度污染' 
                                             when ({1} >= 301) then '严重污染' 
                                            else '无效天' 
                                            end as level, CONVERT(varchar(13),B.DateTime,121) DateTime", factor, factor + "_IAQI");
                    string sql = string.Format(@"select {1} from 
(select pointid,max(DateTime) DateTime from {0} group by pointid) A
left join {0} B 
on B.DateTime = A.DateTime and B.PointId = A.pointid
left join [dbo].[SY_MonitoringPoint] C on A.PointId = C.PointId", tbname, field);
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");

                    //                    string sql2 = "select pointId from [dbo].[SY_MonitoringPoint]";
                    //                    DataTable dt2 = g_DatabaseHelper.ExecuteDataTable(sql2, "AMS_MonitoringBusinessConnection");
                    //                    StringBuilder sb = new StringBuilder();
                    //                    sb.Append("select * from (");
                    //                    string field = string.Format(@"a.PointId, {0} AS Value,{1} as IAQI,b.MonitoringPointName,b.X,b.Y,
                    //                                            case when ({1} > 0 and {1} < 50) then '优' 
                    //                                             when ({1} >= 51 and {1} <= 100) then '良' 
                    //                                             when ({1} >= 101 and {1} <= 150) then '轻度污染' 
                    //                                             when ({1} >= 151 and {1} <= 200) then '中度污染' 
                    //                                             when ({1} >= 201 and {1} <= 300) then '重度污染' 
                    //                                             when ({1} >= 301) then '严重污染' 
                    //                                            else '无效天' 
                    //                                            end as level, CONVERT(varchar(13),DateTime,121) DateTime", factor, factor + "_IAQI");
                    //                    for (int i = 0; i < dt2.Rows.Count - 1; i++)
                    //                    {
                    //                        string pid = dt2.Rows[i][0].ToString();
                    //                        string sql = string.Format(@"(select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc ) union ", tbname, pid, field);
                    //                        sb.Append(sql);
                    //                    }
                    //                    string pid2 = dt2.Rows[dt2.Rows.Count - 1][0].ToString();
                    //                    string sql3 = string.Format(@"(select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc )) A ", tbname, pid2, field);
                    //                    sb.Append(sql3);
                    //                    dt = g_DatabaseHelper.ExecuteDataTable(sb.ToString(), "AMS_MonitoringBusinessConnection");
                }
            }
            else if (type.Equals("AQI"))
            {
                if (pointId[0] != "ALL")
                {
                    string field = string.Format(@"A.PointId, {0} AS Value,{1} as IAQI,C.MonitoringPointName,C.X,C.Y,
                                               Class as level, CONVERT(varchar(13),A.DateTime,121) DateTime", factor, factor + "_IAQI");
                    string sql = string.Format(@"select {1} from 
(select pointid,'{3}' DateTime from {0} group by pointid) A
left join {0} B 
on B.DateTime <= A.DateTime and B.DateTime>'{4}' and B.PointId = A.pointid
left join [dbo].[SY_MonitoringPoint] C on A.PointId = C.PointId where A.PointId in ({2})", tbname, field, "'" + string.Join("','", pointId) + "'", DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:59:59"), DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:59:59"));
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");

                    //                    if (pointId.Length == 1)
                    //                    {
                    //                        string field = string.Format(@"a.PointId, {0} AS Value,{1} as IAQI,b.MonitoringPointName,b.X,b.Y,
                    //                                               Class as level, CONVERT(varchar(13),DateTime,121) DateTime", factor, factor + "_IAQI");
                    //                        string sql = string.Format(@"select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc", tbname, pointId[0], field);
                    //                        dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");
                    //                    }
                    //                    else
                    //                    {
                    //                        StringBuilder sb = new StringBuilder();
                    //                        string field = string.Format(@"a.PointId, {0} AS Value,{1} as IAQI,b.MonitoringPointName,b.X,b.Y,
                    //                                               Class as level, CONVERT(varchar(13),DateTime,121) DateTime", factor, factor + "_IAQI");
                    //                        sb.Append("select * from (");
                    //                        for (int i = 0; i < pointId.Length - 1; i++)
                    //                        {
                    //                            string pid = pointId[i];
                    //                            string sql = string.Format(@"(select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc ) union ", tbname, pid, field);
                    //                            sb.Append(sql);
                    //                        }
                    //                        string pid2 = pointId[pointId.Length - 1];
                    //                        string sql3 = string.Format(@"(select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc )) A ", tbname, pid2, field);
                    //                        sb.Append(sql3);
                    //                        dt = g_DatabaseHelper.ExecuteDataTable(sb.ToString(), "AMS_MonitoringBusinessConnection");
                    //                    }
                }
                else
                {
                    string field = string.Format(@"B.PointId, {0} AS Value,{1} as IAQI,C.MonitoringPointName,C.X,C.Y,
                                               Class as level, CONVERT(varchar(13),B.DateTime,121) DateTime", factor, factor + "_IAQI");
                    string sql = string.Format(@"select {1} from 
(select pointid,max(DateTime) DateTime from {0} group by pointid) A
left join {0} B 
on B.DateTime = A.DateTime and B.PointId = A.pointid
left join [dbo].[SY_MonitoringPoint] C on A.PointId = C.PointId ", tbname, field);
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");



                    //                    string sql2 = "select pointId from [dbo].[SY_MonitoringPoint]";
                    //                    DataTable dt2 = g_DatabaseHelper.ExecuteDataTable(sql2, "AMS_MonitoringBusinessConnection");
                    //                    StringBuilder sb = new StringBuilder();
                    //                    sb.Append("select * from (");
                    //                    string field = string.Format(@"a.PointId, {0} AS Value,{1} as IAQI,b.MonitoringPointName,b.X,b.Y,
                    //                                               Class as level, CONVERT(varchar(13),DateTime,121) DateTime", factor, factor + "_IAQI");
                    //                    for (int i = 0; i < dt2.Rows.Count - 1; i++)
                    //                    {
                    //                        string pid = dt2.Rows[i][0].ToString();
                    //                        string sql = string.Format(@"(select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc ) union ", tbname, pid, field);
                    //                        sb.Append(sql);
                    //                    }
                    //                    string pid2 = dt2.Rows[dt2.Rows.Count - 1][0].ToString();
                    //                    string sql3 = string.Format(@"(select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc )) A ", tbname, pid2, field);
                    //                    sb.Append(sql3);
                    //dt = g_DatabaseHelper.ExecuteDataTable(sb.ToString(), "AMS_MonitoringBusinessConnection");

                }
            }
            else if (type.Equals(""))
            {
                if (pointId[0] != "ALL")
                {
                    string field = string.Format(@"B.PointId, {0} AS Value,{1} as IAQI,C.MonitoringPointName,C.X,C.Y,
                                               '' as level, CONVERT(varchar(13),B.DateTime,121) DateTime", factor, factor + "_IAQI");
                    string sql = string.Format(@"select {1} from 
(select pointid,max(DateTime) DateTime from {0} group by pointid) A
left join {0} B 
on B.DateTime = A.DateTime and B.PointId = A.pointid
left join [dbo].[SY_MonitoringPoint] C on A.PointId = C.PointId where A.PointId in ({2})", tbname, field, "'" + string.Join("','", pointId) + "'");
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");

                    //                    if (pointId.Length == 1)
                    //                    {
                    //                        string field = string.Format(@"a.PointId, {0} AS Value,{1} as IAQI,b.MonitoringPointName,b.X,b.Y,
                    //                                               ''as level, CONVERT(varchar(13),DateTime,121) DateTime", factor, factor + "_IAQI");
                    //                        string sql = string.Format(@"select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc", tbname, pointId[0], field);
                    //                        dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");
                    //                    }
                    //                    else
                    //                    {
                    //                        StringBuilder sb = new StringBuilder();
                    //                        string field = string.Format(@"a.PointId, {0} AS Value,{1} as IAQI,b.MonitoringPointName,b.X,b.Y,
                    //                                               ''as level, CONVERT(varchar(13),DateTime,121) DateTime", factor, factor + "_IAQI");
                    //                        sb.Append("select * from (");
                    //                        for (int i = 0; i < pointId.Length - 1; i++)
                    //                        {
                    //                            string pid = pointId[i];
                    //                            string sql = string.Format(@"(select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc ) union ", tbname, pid, field);
                    //                            sb.Append(sql);
                    //                        }
                    //                        string pid2 = pointId[pointId.Length - 1];
                    //                        string sql3 = string.Format(@"(select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc )) A ", tbname, pid2, field);
                    //                        sb.Append(sql3);
                    //                        dt = g_DatabaseHelper.ExecuteDataTable(sb.ToString(), "AMS_MonitoringBusinessConnection");
                    //                    }
                }
                else
                {
                    string field = string.Format(@"B.PointId, {0} AS Value,{1} as IAQI,C.MonitoringPointName,C.X,C.Y,
                                               '' as level, CONVERT(varchar(13),B.DateTime,121) DateTime", factor, factor + "_IAQI");
                    string sql = string.Format(@"select {1} from 
(select pointid,max(DateTime) DateTime from {0} group by pointid) A
left join {0} B 
on B.DateTime = A.DateTime and B.PointId = A.pointid
left join [dbo].[SY_MonitoringPoint] C on A.PointId = C.PointId", tbname, field);
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");

                    //                    string sql2 = "select pointId from [dbo].[SY_MonitoringPoint]";
                    //                    DataTable dt2 = g_DatabaseHelper.ExecuteDataTable(sql2, "AMS_MonitoringBusinessConnection");
                    //                    StringBuilder sb = new StringBuilder();
                    //                    sb.Append("select * from (");
                    //                    string field = string.Format(@"a.PointId, {0} AS Value,{1} as IAQI,b.MonitoringPointName,b.X,b.Y,
                    //                                               ''as level, CONVERT(varchar(13),DateTime,121) DateTime", factor, factor + "_IAQI");
                    //                    for (int i = 0; i < dt2.Rows.Count - 1; i++)
                    //                    {
                    //                        string pid = dt2.Rows[i][0].ToString();
                    //                        string sql = string.Format(@"(select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc ) union ", tbname, pid, field);
                    //                        sb.Append(sql);
                    //                    }
                    //                    string pid2 = dt2.Rows[dt2.Rows.Count - 1][0].ToString();
                    //                    string sql3 = string.Format(@"(select top 1 {2} from {0} a 
                    //                                            left join [dbo].[SY_MonitoringPoint] b on a.PointId = b.PointId 
                    //                                            where a.PointId = {1} order by DateTime desc )) A ", tbname, pid2, field);
                    //                    sb.Append(sql3);
                    //                    dt = g_DatabaseHelper.ExecuteDataTable(sb.ToString(), "AMS_MonitoringBusinessConnection");
                }
            }
            return dt;
        }

        /// <summary>
        /// 获取周边城市测点信息提供给接口(显示国控、省控、市控，在线信息等)
        /// </summary>
        /// <param name="dataType">测点类型（All或者空是全部）</param>
        /// <returns></returns>
        public DataTable GetAroundPortMessageForDataOrder(string order, string type)
        {
            string orderby = order == "1" ? "AQIValue asc" : "AQIValue desc";
            DataTable dt = new DataTable();
            if (type.Equals("2"))
            {
                string sql2 = "select distinct pointId from [AMS_AirAutoMonitor].[dbo].[Air.TB_OriHourAQIAround]";
                DataTable dt2 = g_DatabaseHelper.ExecuteDataTable(sql2, "AMS_AirAutoMonitorConnection");
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from (");
                for (int i = 0; i < dt2.Rows.Count - 1; i++)
                {
                    string pid = dt2.Rows[i][0].ToString();
                    string sql = string.Format(@"(SELECT top 1 * FROM [AMS_AirAutoMonitor].[dbo].[Air.TB_OriHourAQIAround]
                                            where PointId = {0}) union ", pid);
                    sb.Append(sql);
                }
                string pid2 = dt2.Rows[dt2.Rows.Count - 1][0].ToString();
                string sql3 = string.Format(@"(SELECT top 1 * FROM [AMS_AirAutoMonitor].[dbo].[Air.TB_OriHourAQIAround]
                                            where  PointId = {0})) A ", pid2);
                sb.Append(sql3 + " order by " + orderby);
                dt = g_DatabaseHelper.ExecuteDataTable(sb.ToString(), "AMS_AirAutoMonitorConnection");
            }
            else if (type.Equals("1"))
            {
                string sql = string.Format(@" SELECT * FROM [AMS_AirAutoMonitor].[dbo].[Air.TB_OriDayAQIAround]
                                            where 
                                            DateTime>='{0}' and DateTime<'{1}' order by {2}", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"), DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), orderby);
                dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
            }
            return dt;
        }

        /// <summary>
        /// 获取周边城市测点信息提供给接口(显示国控、省控、市控，在线信息等)
        /// </summary>
        /// <param name="dataType">测点类型（All或者空是全部）</param>
        /// <returns></returns>
        public DataTable GetAroundPortData(string pointId, string type)
        {
            DataTable dt = new DataTable();
            if (type.Equals("2"))
            {
                string sql = string.Format(@" SELECT top 1 * FROM [AMS_AirAutoMonitor].[dbo].[Air.TB_OriHourAQIAround]
                                            where PointId = {0} order by Tstamp desc", pointId);
                dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
            }
            else if (type.Equals("1"))
            {
                string sql = string.Format(@" SELECT * FROM [AMS_AirAutoMonitor].[dbo].[Air.TB_OriDayAQIAround]
                                            where PointId = {2} and
                                            DateTime>='{0}' and DateTime<'{1}'", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"), DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), pointId);
                dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
            }
            return dt;
        }

        /// <summary>
        /// 获取测点分类数据提供给接口(显示国控、省控、市控等)
        /// </summary>
        /// <param name="dataType">测点类型（All或者空是全部）</param>
        /// <returns></returns>
        public DataTable GetPortTypes()
        {
            DataTable dt = new DataTable();
            string sql = " select PName PortTypeName,CGuid PortTypeValue from dbo.V_Point_Air_SiteMap_Property where PGuid is null order by POrder desc";
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            dt = g_DatabaseHelper.ExecuteDataTable(sql, connection);

            return dt;
        }

        /// <summary>
        /// 获取地表水测点配置因子与每日数据采集次数
        /// </summary>
        /// <param name="pointId">测点id</param>
        /// <returns></returns>
        public DataTable GetDataCycleAndFactorList(string pointId)
        {
            string sql = "select DataCycle,EvaluateFactorList from MPInfo.TB_MonitoringPointExtensionForEQMSWater as w left join MPInfo.TB_MonitoringPoint as p on p.MonitoringPointUid = w.MonitoringPointUid where p.PointId = '" + pointId + "'";
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 获取地表水测点基本信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetWaterPointBaseInfo()
        {
            string sql = @"select p.pointId,Valley.ItemText as WatersName,w.dataCycle,w.monitoringPointUid,p.MonitoringPointName 
                            from MPInfo.TB_MonitoringPointExtensionForEQMSWater as w 
                            left join MPInfo.TB_MonitoringPoint as p on p.MonitoringPointUid = w.monitoringPointUid
                            left join dbo.SY_View_CodeMainItem as Valley
                                    on w.Valley = Valley.ItemGuid ";
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 获取沙渚锡东周均值表点位信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSZXDPointInfo()
        {
            string SZXDGuid = ConfigurationManager.AppSettings["SZXDGuid"].ToString();
            string sql = "select pointId,monitoringpointname  from MPInfo.TB_MonitoringPoint where MonitoringPointUid in(" + SZXDGuid + ")";
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 获取沙渚锡东周均值点位因子上下限值信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetPointPollutantStandard()
        {
            string sql = string.Format(@"select p.MonitoringPointName as portName,p.PointId as PortId
                                        ,y.ItemText as Unit
                                        ,f.PollutantName,f.PollutantCode,ExcessiveLow as low
                                        ,ExcessiveUpper as upper 
                                        from [BusinessRule].[TB_ExcessiveSetting] as e
                                        left join dbo.V_Point_InstrumentChannels as c on c.InstrumentChannelsUid = e.InstrumentChannelsUid
                                        left join MPInfo.TB_MonitoringPoint as p on p.MonitoringPointUid = e.MonitoringPointUid
                                        left join [Standard].[TB_PollutantCode] as f on f.PollutantCode = c.PollutantCode
                                        left join SY_View_CodeMainItem as y on y.ItemGuid = f.MeasureUnitUid");
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 获取饮用水源地点位信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDrinkWaterPointSource()
        {
            string sql = "select * from MPInfo.TB_MonitoringPoint where SiteTypeUid = '81381370-1744-41fd-b5e4-4ffbee24288e'";
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 获取浮标站点位信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetLakeBuoyPointSource()
        {
            string sql = "select * from MPInfo.TB_MonitoringPoint where SiteTypeUid = 'a10a8482-1d30-44ea-8b03-b724f1ebeaa8'";
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 根据点位名称获取DWCODE
        /// </summary>
        /// <returns></returns>
        public DataTable GetPointDWCODE(string monitoringpointname)
        {
            string sql = "select p.monitoringpointname,a.dwcode from  MPInfo.TB_MonitoringPoint as p left join MPInfo.TB_MonitoringPointExtensionForEQMSAir as a on a.MonitoringPointUid = p.MonitoringPointUid where p.monitoringpointname = '" + monitoringpointname + "'";
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 根据点位ID获取点位名称信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetPointNameByID(string pointid)
        {
            string sql = "select p.monitoringpointname,pointid,v.itemtext from  MPInfo.TB_MonitoringPoint as p inner join dbo.SY_View_CodeMainItem as v on v.itemguid = p.ContrlUid  where p.pointid in (" + pointid + ")";
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 获取地表水点位的地区，属性，集成商，河流名称信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetWaterPointDetailInfo()
        {
            string sql = string.Format(@"
                                 select p.PointId,
                                        v.itemtext as DQ,
                                        water.WatersName as HLMC,
                                        p.MonitoringPointName as DMMC,
                                        c.itemtext as SX,
                                        y.itemtext as YWS,
                                        v.SortNumber as AreaSortNum,
                                        p.OrderByNum as PointSortNum 
                                   from MPInfo.TB_MonitoringPoint as p
                                   left join dbo.SY_View_CodeMainItem as v 
                                     on v.itemguid = replace(p.RegionUid, '4d00bb50-177d-435b-b05d-58a5ef473920', '884007dc-d877-4e4e-8ec5-67a0d84c902c')
                                   left join MPInfo.TB_MonitoringPointExtensionForEQMSWater as water  
                                     on water.monitoringPointUid = p.MonitoringPointUid
                                   left join dbo.SY_View_CodeMainItem as c 
                                     on c.itemguid = p.ContrlUid
                                   left join dbo.SY_View_CodeMainItem as y 
                                     on y.itemguid = p.OperatorsUid 
                                  order by AreaSortNum desc, PointSortNum desc");
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 获取集成商的Guid与名称信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetInstrumentIntegratorInfo()
        {
            string sql = string.Format(@"select itemguid as id,itemtext as name from dbo.SY_View_CodeMainItem where mainguid = '95d0f718-d34b-4744-9ffb-efa3be9b5d5c'");
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 获取区域的Guid与名称信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetRegionInfo()
        {
            string sql = string.Format(@"select itemguid as id,itemtext as name from dbo.SY_View_CodeMainItem where mainguid = 'f7daab25-2b30-4b9c-8688-7f662464cd3f'");
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 获取地表水未停运点位信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetEnablePointInfo()
        {
            string sql = string.Format(@"select * from MPInfo.TB_MonitoringPoint
                                        where ApplicationUid = 'watrwatr-watr-watr-watr-watrwatrwatr'
                                        and EnableOrNot = 1
                                        and SiteTypeUid != '160e08ec-1d1b-4095-b898-3a3d925ed4e6'
                                        and RunStatusUid = '3ee5eaf2-b7fa-4756-b2c5-96e6befbd92d'");
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 获取地表水停运点位信息
        /// </summary>
        /// <returns>PointId、PointName</returns>
        public DataTable GetStopPointInfo()
        {
            string sql = string.Format(@"
                                     select PointId,
                                            MonitoringPointName as PointName 
                                       from MPInfo.TB_MonitoringPoint
                                      where ApplicationUid = 'watrwatr-watr-watr-watr-watrwatrwatr'
                                        and EnableOrNot = 1
                                        and SiteTypeUid != '160e08ec-1d1b-4095-b898-3a3d925ed4e6'
                                        and RunStatusUid = '253a5773-df58-48c4-8ee7-e66359b6e2d0'");
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 根据所选站点ID获取相应区域信息
        /// </summary>
        /// <param name="regionUid"></param>
        /// <returns></returns>
        public DataView GetRegionByPointId(string[] pointIds)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < pointIds.Length; i++)
                {
                    sb.Append("'" + pointIds[i] + "'" + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                string sql = string.Format("SELECT PortId,MonitoringPointName,RegionUid,Region FROM V_Point_UserConfigNew WHERE PortId IN ({0})", sb);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 根据区域名获取相应站点ID
        /// </summary>
        /// <param name="regionUid"></param>
        /// <returns></returns>
        public DataTable GetPointIdByCityName(string CityName)
        {
            try
            {
                string sql = string.Format("SELECT Distinct PortId FROM V_Point_UserConfigNew WHERE Region = '{0}'", CityName);
                return g_DatabaseHelper.ExecuteDataTable(sql, connection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        #endregion
    }
}
