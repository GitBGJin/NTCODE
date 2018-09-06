using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.DomainModel.MonitoringBusiness;
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
    public class GranuleSpecialDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 存储过程处理类
        /// </summary>
        BaseDAHelper g_DBBiz = Singleton<BaseDAHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_MonitoringBusinessConnection";
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection1 = "AMS_AirAutoMonitorConnection";

        /// <summary>
        /// 审核数据库表名
        /// </summary>
        private string tableName = "AirReport.TB_HourReport";
        /// <summary>
        /// 原始数据库表名
        /// </summary>
        private string tableName1 = " Air.TB_InfectantBy1";
        private string tableName2 = " Air.TB_InfectantBy5";
        private string tableName3 = "Air.TB_InfectantBy60";
        private string connection2 = "Frame_Connection";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public GranuleSpecialDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(ApplicationType.Air, PollutantDataType.Hour);
            tableName3 = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(ApplicationType.Air, PollutantDataType.Min60);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Hour);
            connection1 = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Min60);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetOriHourData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd, string type)
        {

            try
            {
                string TbName = "";
                switch (type)
                {
                    case "Min1":
                        TbName = "Air.TB_InfectantBy1";
                        break;
                    case "Min5":
                        TbName = "Air.TB_InfectantBy5";
                        break;
                    case "Min60":
                        TbName = "Air.TB_InfectantBy60";
                        break;
                }

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string portIdsStr = " AND PointId = " + portIds;

                string sql = string.Format(@"select PointId,Tstamp {1}
							                 from {0}
                                             where  [Tstamp]>='{2}' AND [Tstamp]<='{3}' {4}
							group by PointId,[Tstamp]
							order by PointId,[Tstamp]
               ", TbName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection1);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetOriHourDataNew(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, string type)
        {

            try
            {
                string TbName = "";
                switch (type)
                {
                    case "Min1":
                        TbName = "Air.TB_InfectantBy1";
                        break;
                    case "Min5":
                        TbName = "Air.TB_InfectantBy5";
                        break;
                    case "Min60":
                        TbName = "Air.TB_InfectantBy60";
                        break;
                }

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }
                string pds = string.Empty;
                for (int i = 0; i < portIds.Length;i++ )
                {
                    pds +="'"+ portIds[i]+"',";
                }
                string portIdsStr = " AND PointId in (" + pds.TrimEnd(',')+ ")";

                string sql = string.Format(@"select PointId,Tstamp {1}
							                 from {0}
                                             where  [Tstamp]>='{2}' AND [Tstamp]<='{3}' {4}
							group by PointId,[Tstamp]
							order by PointId,[Tstamp]
               ", TbName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection1);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetOriDayData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {

            try
            {
                string TbName = "Air.TB_InfectantByDay";

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string portIdsStr = " AND PointId = " + portIds;

                string sql = string.Format(@"select PointId,DateTime {1}
							                 from {0}
                                             where  [DateTime]>='{2}' AND [DateTime]<='{3}' {4}
							group by PointId,[DateTime]
							order by PointId,[DateTime]
               ", TbName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection1);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetOriDayDataNew(string [] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {

            try
            {
                string TbName = "Air.TB_InfectantByDay";

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }
                string pds = string.Empty;
                for (int i = 0; i < portIds.Length; i++)
                {
                    pds += "'" + portIds[i] + "',";
                }
                string portIdsStr = " AND PointId in (" + pds.TrimEnd(',') + ")";

                string sql = string.Format(@"select PointId,DateTime {1}
							                 from {0}
                                             where  [DateTime]>='{2}' AND [DateTime]<='{3}' {4}
							group by PointId,[DateTime]
							order by PointId,[DateTime]
               ", TbName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection1);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetOriMonthDataNew(string [] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {

            try
            {
                string TbName = "Air.TB_InfectantByMonth";

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string pds = string.Empty;
                for (int i = 0; i < portIds.Length; i++)
                {
                    pds += "'" + portIds[i] + "',";
                }
                string portIdsStr = " AND PointId in (" + pds.TrimEnd(',') + ")";

                string sql = string.Format(@"select PointId,DateTime {1}
							                 from {0}
                                             where  [DateTime]>='{2}' AND [DateTime]<='{3}' {4}
							group by PointId,[DateTime]
							order by PointId,[DateTime]
               ", TbName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection1);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得审核查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetAuditHourDataNew(string [] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {

            try
            {

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }
                string pds = string.Empty;
                for (int i = 0; i < portIds.Length; i++)
                {
                    pds += "'" + portIds[i] + "',";
                }
                string portIdsStr = " AND PointId in (" + pds.TrimEnd(',') + ")";
                string sql = string.Format(@"select PointId,Tstamp {1}
							                 from {0}
                                             where  [Tstamp]>='{2}' AND [Tstamp]<='{3}' {4}
							group by PointId,[Tstamp]
							order by PointId,[Tstamp]
               ", tableName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetAuditDayDataNew(string [] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {

            try
            {
                string TbName = "AirReport.TB_DayReport";

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string pds = string.Empty;
                for (int i = 0; i < portIds.Length; i++)
                {
                    pds += "'" + portIds[i] + "',";
                }
                string portIdsStr = " AND PointId in (" + pds.TrimEnd(',') + ")";

                string sql = string.Format(@"select PointId,DateTime  {1}
							                 from {0}
                                             where  [DateTime]>='{2}' AND [DateTime]<='{3}' {4}
							group by PointId,[DateTime]
							order by PointId,[DateTime]
               ", TbName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetAuditMonthDataNew(string [] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {

            try
            {
                string TbName = "AirReport.TB_MonthReport";

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string pds = string.Empty;
                for (int i = 0; i < portIds.Length; i++)
                {
                    pds += "'" + portIds[i] + "',";
                }
                string portIdsStr = " AND PointId in (" + pds.TrimEnd(',') + ")";

                string sql = string.Format(@"select PointId,[ReportDateTime] DateTime {1}
							                 from {0}
                                             where  [ReportDateTime]>='{2}' AND [ReportDateTime]<='{3}' {4}
							group by PointId,[ReportDateTime]
							order by PointId,[DateTime]
               ", TbName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetAuditSeasonDataNew(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {

            try
            {
                string TbName = "AirReport.TB_SeasonReport";
                int seasonFrom = yearFrom * 1000 + seasonOfYearFrom;
                int seasonTo = yearTo * 1000 + seasonOfYearTo;
                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string pds = string.Empty;
                for (int i = 0; i < portIds.Length; i++)
                {
                    pds += "'" + portIds[i] + "',";
                }
                string portIdsStr = " AND PointId in (" + pds.TrimEnd(',') + ")";

                string sql = string.Format(@"select PointId,Year, SeasonOfYear  {1}
							                 from {0}
                                             where  (Year*1000 + SeasonOfYear)>= {2} AND (Year*1000 + SeasonOfYear)<={3} {4}
							group by PointId,[Year],SeasonOfYear
							order by PointId,[Year],SeasonOfYear
               ", TbName, factorSql, seasonFrom, seasonTo, portIdsStr);

                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetAuditYearDataNew(string[] portIds, string[] factors, int yearFrom, int yearTo)
        {

            try
            {
                string TbName = "AirReport.TB_YearReport";

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string pds = string.Empty;
                for (int i = 0; i < portIds.Length; i++)
                {
                    pds += "'" + portIds[i] + "',";
                }
                string portIdsStr = " AND PointId in (" + pds.TrimEnd(',') + ")";

                string sql = string.Format(@"select PointId,[Year] {1}
							                 from {0}
                                             where  [Year]>='{2}' AND [Year]<='{3}' {4}
							group by PointId,[Year]
							order by PointId,[Year]
               ", TbName, factorSql, yearFrom, yearTo, portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetAuditWeekDataNew(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom, int yearTo, int weekOfYearTo)
        {

            try
            {
                string TbName = "AirReport.TB_WeekReport";

                string factorSql = string.Empty;
                int weekFrom = yearFrom * 1000 + weekOfYearFrom;
                int weekTo = yearTo * 1000 + weekOfYearTo;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string pds = string.Empty;
                for (int i = 0; i < portIds.Length; i++)
                {
                    pds += "'" + portIds[i] + "',";
                }
                string portIdsStr = " AND PointId in (" + pds.TrimEnd(',') + ")";

                string sql = string.Format(@"select PointId,Year, WeekOfYear  {1}
							                 from {0}
                                             where  (Year*1000 + WeekOfYear)>= {2} AND (Year*1000 + WeekOfYear)<={3} {4}
							group by PointId,[Year],WeekOfYear
							order by PointId,[Year],WeekOfYear
               ", TbName, factorSql, weekFrom, weekTo, portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetOriMonthData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {

            try
            {
                string TbName = "Air.TB_InfectantByMonth";

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string portIdsStr = " AND PointId = " + portIds;

                string sql = string.Format(@"select PointId,DateTime {1}
							                 from {0}
                                             where  [DateTime]>='{2}' AND [DateTime]<='{3}' {4}
							group by PointId,[DateTime]
							order by PointId,[DateTime]
               ", TbName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection1);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得审核查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetAuditHourData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {

            try
            {

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string portIdsStr = " AND PointId = " + portIds;

                string sql = string.Format(@"select PointId,Tstamp {1}
							                 from {0}
                                             where  [Tstamp]>='{2}' AND [Tstamp]<='{3}' {4}
							group by PointId,[Tstamp]
							order by PointId,[Tstamp]
               ", tableName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetAuditDayData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {

            try
            {
                string TbName = "AirReport.TB_DayReport";

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string portIdsStr = " AND PointId = " + portIds;

                string sql = string.Format(@"select PointId,DateTime  {1}
							                 from {0}
                                             where  [DateTime]>='{2}' AND [DateTime]<='{3}' {4}
							group by PointId,[DateTime]
							order by PointId,[DateTime]
               ", TbName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetAuditMonthData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {

            try
            {
                string TbName = "AirReport.TB_MonthReport";

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string portIdsStr = " AND PointId = " + portIds;

                string sql = string.Format(@"select PointId,[ReportDateTime] DateTime {1}
							                 from {0}
                                             where  [ReportDateTime]>='{2}' AND [ReportDateTime]<='{3}' {4}
							group by PointId,[ReportDateTime]
							order by PointId,[DateTime]
               ", TbName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetAuditWeekData(string portIds, string[] factors, int yearFrom, int weekOfYearFrom,int yearTo, int weekOfYearTo)
        {

            try
            {
                string TbName = "AirReport.TB_WeekReport";

                string factorSql = string.Empty;
                int weekFrom = yearFrom * 1000 + weekOfYearFrom;
                int weekTo = yearTo * 1000 + weekOfYearTo;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string portIdsStr = " AND PointId = " + portIds;

                string sql = string.Format(@"select PointId,Year, WeekOfYear  {1}
							                 from {0}
                                             where  (Year*1000 + WeekOfYear)>= {2} AND (Year*1000 + WeekOfYear)<={3} {4}
							group by PointId,[Year],WeekOfYear
							order by PointId,[Year],WeekOfYear
               ", TbName, factorSql, weekFrom, weekTo, portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetAuditSeasonData(string portIds, string[] factors,int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo )
        {

            try
            {
                string TbName = "AirReport.TB_SeasonReport";
                int seasonFrom = yearFrom * 1000 + seasonOfYearFrom;
                int seasonTo = yearTo * 1000 + seasonOfYearTo;
                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string portIdsStr = " AND PointId = " + portIds;

                string sql = string.Format(@"select PointId,Year, SeasonOfYear  {1}
							                 from {0}
                                             where  (Year*1000 + SeasonOfYear)>= {2} AND (Year*1000 + SeasonOfYear)<={3} {4}
							group by PointId,[Year],SeasonOfYear
							order by PointId,[Year],SeasonOfYear
               ", TbName, factorSql, seasonFrom, seasonTo, portIdsStr);

                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得原始查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetAuditYearData(string portIds, string[] factors, int yearFrom, int yearTo)
        {

            try
            {
                string TbName = "AirReport.TB_YearReport";

                string factorSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string portIdsStr = " AND PointId = " + portIds;

                string sql = string.Format(@"select PointId,[Year] {1}
							                 from {0}
                                             where  [Year]>='{2}' AND [Year]<='{3}' {4}
							group by PointId,[Year]
							order by PointId,[Year]
               ", TbName, factorSql, yearFrom, yearTo, portIdsStr);
                DataView Dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return Dv;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
