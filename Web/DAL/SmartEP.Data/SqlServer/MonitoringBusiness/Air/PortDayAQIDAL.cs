using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：PortDayAQIDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：点位日AQI数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class PortDayAQIDAL
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
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public PortDayAQIDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.DayAQI);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
        }
        #endregion

        #region << 数据查询方法 >>
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
        public DataView GetOriDataForData(string[] portIds, DateTime dtStart, DateTime dtEnd, string[] qualityType, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            string con = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
            string tb = "[Air].[TB_OriDayAQI]";
            string where1 = string.Empty;
            string where2 = string.Empty;
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
            recordTotal = 0;

            string fieldName = @"PointId
                        ,CONVERT(varchar(10),DateTime,121) DateTime
                        ,AQIValue
                        ,Class
                        ,Grade
                        ,PrimaryPollutant
,HealthEffect
,TakeStep";
            string keyName = "id";

            //查询条件拼接
            string where = string.Empty;
            if (portIds[0] == "ALL")
            {
                where = "";
            }
            else
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    where = " PointId =" + portIdsStr + "AND";
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    where = " PointId IN(" + portIdsStr + ")" + "AND";
                }
            }
            string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qualityType.ToList<string>(), "','");
            if (qualityTypeStr != "")
            {
                qualityTypeStr = "'" + qualityTypeStr + "'";
            }
            where += string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(qualityTypeStr))
            {
                where2 += "Class IN(" + qualityTypeStr + ")";
            }
            if (where1 != "")
                where += " and (" + where2 + where1 + ")";
            else if (where2 != "")
                where += " and " + where2;
            return g_GridViewPager.GetGridViewPager(tb, fieldName, keyName, pageSize, pageNo, orderBy, where, con, out recordTotal);
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
        public DataView GetDataMonthDayPager(string[] portIds, DateTime dtStart, DateTime dtEnd)
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
								,SUM(CASE WHEN [AQIValue] >0 THEN 1 ELSE 0 END) AS [AQIValue]
								from {0}
								where {1}
                                 group By PointId
                                 order By PointId
               ", tableName, where);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

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
        public DataView GetRunMonthPager(string[] portIds, DateTime dtStart, DateTime dtEnd)
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
							,dbo.F_Round(AVG(CASE WHEN SO2 IS NOT NULL THEN CONVERT(decimal(5,3),SO2) END),3) AS a21026
							,dbo.F_Round(AVG(CASE WHEN NO2 IS NOT NULL THEN CONVERT(decimal(5,3),NO2) END),3) AS a21004
							,dbo.F_Round(AVG(CASE WHEN PM10 IS NOT NULL THEN CONVERT(decimal(5,3),PM10) END),3) AS a34002
							,dbo.F_Round(AVG(CASE WHEN CO IS NOT NULL THEN CONVERT(decimal(5,1),CO) END),1) AS a21005
							,dbo.F_Round(AVG(CASE WHEN MaxOneHourO3 IS NOT NULL THEN CONVERT(decimal(5,3),MaxOneHourO3) END),3) AS MaxOneHourO3
							,dbo.F_Round(AVG(CASE WHEN Max8HourO3 IS NOT NULL THEN CONVERT(decimal(5,3),Max8HourO3) END),3) AS Max8HourO3
							,dbo.F_Round(AVG(CASE WHEN PM25 IS NOT NULL THEN CONVERT(decimal(5,3),PM25) END),3) AS a34004
							,SUM(CASE WHEN [AQIValue] >0 THEN 1 ELSE 0 END) AS AQIValue
							from {0} 
                            where {1}
							group by PointId
							order by PointId
               ", tableName, where);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 取得全月均值，最大最小值
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
                //因子编码
                string SO = System.Configuration.ConfigurationManager.AppSettings["SO2"];
                string NO = System.Configuration.ConfigurationManager.AppSettings["NO2"];
                string COs = System.Configuration.ConfigurationManager.AppSettings["CO"];
                string O3_1 = System.Configuration.ConfigurationManager.AppSettings["O3-1h"];
                string O3_8 = System.Configuration.ConfigurationManager.AppSettings["O3-8h"];
                string PM10s = System.Configuration.ConfigurationManager.AppSettings["PM10"];
                string PM25s = System.Configuration.ConfigurationManager.AppSettings["PM25"];
                decimal SO2 = Convert.ToDecimal(SO);
                decimal NO2 = Convert.ToDecimal(NO);
                decimal CO = Convert.ToDecimal(COs);
                decimal O31h = Convert.ToDecimal(O3_1);
                decimal O38h = Convert.ToDecimal(O3_8);
                decimal PM10 = Convert.ToDecimal(PM10s);
                decimal PM25 = Convert.ToDecimal(PM25s);

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
								,SUM(CASE WHEN [MaxO3] >0 THEN 1 ELSE 0 END) AS Max8O3Count
								,SUM(CASE WHEN [PM10] >0 THEN 1 ELSE 0 END) AS PM10Count
								,SUM(CASE WHEN [PM25] >0 THEN 1 ELSE 0 END) AS PM25Count
                                ,SUM(CASE WHEN [SO] >{4} THEN 1 ELSE 0 END) AS SO2Xian
								,SUM(CASE WHEN [NO] >{5} THEN 1 ELSE 0 END) AS NO2Xian
								,SUM(CASE WHEN [CO] >{6}THEN 1 ELSE 0 END) AS COXian
		                        ,SUM(CASE WHEN [MaxOneO3] >{7}THEN 1 ELSE 0 END) AS MaxO3Xian
		                        ,SUM(CASE WHEN [MaxO3] >{8}THEN 1 ELSE 0 END) AS Max8O3Xian
								,SUM(CASE WHEN [PM10] >{9} THEN 1 ELSE 0 END) AS PM10Xian
								,SUM(CASE WHEN [PM25] >{10} THEN 1 ELSE 0 END) AS PM25Xian
								,AVG(CONVERT (decimal(18,6),[SO])) as SO2Value
								,AVG(CONVERT (decimal(18,6),[NO])) as NO2Value
								,AVG(CONVERT (decimal(18,6),[CO])) as COValue
								,AVG(CONVERT (decimal(18,6),[MaxOneO3])) as MaxO3Value
								,AVG(CONVERT (decimal(18,6),[MaxO3])) as Max8O3Value
								,AVG(CONVERT (decimal(18,6),[PM10])) as PM10Value
								,AVG(CONVERT (decimal(18,6),[PM25])) as PM25Value
								,SUM(CASE WHEN [SOAQI] >100 THEN 1 ELSE 0 END) AS SO2AQI
								,SUM(CASE WHEN [NOAQI] >100 THEN 1 ELSE 0 END) AS NO2AQI
								,SUM(CASE WHEN [COAQI] >100 THEN 1 ELSE 0 END) AS COAQI
								,SUM(CASE WHEN [MaxOneO3AQI] >100 THEN 1 ELSE 0 END) AS MaxO3AQI
								,SUM(CASE WHEN [MaxO3AQI] >100 THEN 1 ELSE 0 END) AS Max8O3AQI
								,SUM(CASE WHEN [PM10AQI] >100 THEN 1 ELSE 0 END) AS PM10AQI
								,SUM(CASE WHEN [PM25AQI] >100 THEN 1 ELSE 0 END) AS Pm25AQI
								,SUM(CASE WHEN [SOAQI] >0 THEN 1 ELSE 0 END) AS SO2AQICount
								,SUM(CASE WHEN [NOAQI] >0 THEN 1 ELSE 0 END) AS NO2AQICount
								,SUM(CASE WHEN [COAQI] >0 THEN 1 ELSE 0 END) AS COAQICount
								,SUM(CASE WHEN [MaxOneO3AQI] >0 THEN 1 ELSE 0 END) AS MaxO3AQICount
								,SUM(CASE WHEN [MaxO3AQI] >0 THEN 1 ELSE 0 END) AS Max8O3AQICount
								,SUM(CASE WHEN [PM10AQI] >0 THEN 1 ELSE 0 END) AS PM10AQICount
								,SUM(CASE WHEN [PM25AQI] >0 THEN 1 ELSE 0 END) AS Pm25AQICount
								,Max([SO]) as SO2Max
								,Max([NO]) as NO2Max
								,Max([CO]) as COMax
								,Max([MaxOneO3]) as MaxO3Max
								,Max([MaxO3]) as Max8O3Max
								,Max([PM10]) as PM10Max
								,Max(PM25) as PM25Max
								,Min([SO]) as SO2Min
								,Min([NO]) as NO2Min
								,Min([CO]) as COMin
								,Min([MaxOneO3]) as MaxO3Min
								,Min([MaxO3]) as Max8O3Min
								,Min([PM10]) as PM10Min
								,Min(PM25) as PM25Min
								from
								(
								select PointId
								,CONVERT (decimal(18,6),[SO2]) as [SO]
								,CONVERT (decimal(18,6),[NO2]) as [NO]
								,CONVERT (decimal(18,6),[CO]) as [CO]
								,CONVERT (decimal(18,6),[MaxOneHourO3]) as [MaxOneO3]
								,CONVERT (decimal(18,6),[Max8HourO3]) as [MaxO3]
								,CONVERT (decimal(18,6),[PM10]) as [PM10]
								,CONVERT (decimal(18,6),[PM25]) as [PM25]
								,CONVERT (decimal(18,6),[SO2_IAQI]) as [SOAQI]
								,CONVERT (decimal(18,6),[NO2_IAQI]) as [NOAQI]
								,CONVERT (decimal(18,6),[CO_IAQI]) as [COAQI]
								,CONVERT (decimal(18,6),[MaxOneHourO3_IAQI]) as [MaxOneO3AQI]
								,CONVERT (decimal(18,6),[Max8HourO3_IAQI]) as [MaxO3AQI]
								,CONVERT (decimal(18,6),[PM10_IAQI]) as [PM10AQI]
								,CONVERT (decimal(18,6),[PM25_IAQI]) as [PM25AQI]
								from {0}
								where DateTime>='{1}' AND DateTime<='{2}' {3}
								) a group by PointId
               ", tableName, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr, SO2, NO2, CO, O31h, O38h, PM10, PM25);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
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
        public DataView GetDataPager(string[] portIds, DateTime dtStart, DateTime dtEnd, string[] qualityType, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            string where1 = string.Empty;
            string where2 = string.Empty;
            if (qualityType.Contains("无效天"))
            {
                if (qualityType.Length > 1)
                {
                    where1 += " or AQIValue is NULL or ltrim(rtrim(AQIValue))=''";
                }
                else
                {
                    where1 += " AQIValue is NULL or ltrim(rtrim(AQIValue))=''";
                }
                var list = qualityType.ToList();
                list.RemoveAt(list.IndexOf("无效天"));
                qualityType = list.ToArray();
            }
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
            recordTotal = 0;

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
            string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qualityType.ToList<string>(), "','");
            if (qualityTypeStr != "")
            {
                qualityTypeStr = "'" + qualityTypeStr + "'";
            }
            where += string.Format(" AND DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(qualityTypeStr))
            {
                where2 += "Class IN(" + qualityTypeStr + ")";
            }
            if (where1 != "")
                where += " and (" + where2 + where1 + ")";
            else if (where2 != "")
                where += " and " + where2;
            return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)新方法（补全不存在数据的日期）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段(PointId,DateTime)</param>
        /// <returns></returns>
        public DataView GetDataPagerNew(string[] portIds, DateTime dtStart, DateTime dtEnd, string[] qualityType, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            string where1 = string.Empty;
            string where2 = string.Empty;
            string where = string.Empty;
            if (qualityType.Contains("无效天"))
            {
                if (qualityType.Length > 1)
                {
                    where1 += " or AQIValue is NULL or ltrim(rtrim(AQIValue))=''";
                }
                else
                {
                    where1 += " AQIValue is NULL or ltrim(rtrim(AQIValue))=''";
                }
                var list = qualityType.ToList();
                list.RemoveAt(list.IndexOf("无效天"));
                qualityType = list.ToArray();
            }
            string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qualityType.ToList<string>(), "','");
            if (qualityTypeStr != "")
            {
                qualityTypeStr = "'" + qualityTypeStr + "'";
            }
            if (!string.IsNullOrEmpty(qualityTypeStr))
            {
                where2 += "Class IN(" + qualityTypeStr + ")";
            }
            if (where1 != "")
                where += " (" + where2 + where1 + ")";
            else if (where2 != "")
                where += " " + where2;
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
            string sqlCount = "select count(convert(varchar(10),dateadd(dd,number,'" + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + "'),120)) from master..spt_values B where B.type='p'  and B.number <= datediff(dd,'" + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + "','" + dtEnd.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss") + "')";
            int countOnPort = (int)g_DatabaseHelper.ExecuteScalar(sqlCount, connection);
            recordTotal = countOnPort * portIds.Length;
            string portIdsStr = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from (");
            if (portIds != null && portIds.Length > 0)
            {
                //DateTime dtEndFillNull = dtEnd > DateTime.Now ? DateTime.Now.AddDays(-1) : dtEnd.AddDays(-1);
                DateTime dtEndFillNull = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) ?
                    DateTime.Now.AddDays(-1) : dtEnd;
                for (int i = 0; i < portIds.Length; i++)
                {
                    portIdsStr = portIds[i];
                    string sql = "select '" + portIdsStr + "' PointId,convert(varchar(10),dateadd(dd,number,'" + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + "'),120) DateTime,A.SO2,A.SO2_IAQI,A.NO2,A.NO2_IAQI,A.PM10,A.PM10_IAQI,A.CO,A.CO_IAQI,A.MaxOneHourO3 ,A.MaxOneHourO3_IAQI,A.Max8HourO3,A.Max8HourO3_IAQI,A.PM25,A.PM25_IAQI,A.AQIValue,A.PrimaryPollutant,A.Range,A.RGBValue,A.PicturePath,A.Class ,A.Grade,A.HealthEffect,A.TakeStep from master..spt_values B left join (select * from [AirRelease].[TB_DayAQI] where " + where + ") A on A.DateTime = convert(varchar(10),dateadd(dd,number,'" + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + "'),120) and A.PointId='" + portIdsStr + "' where B.type='p'  and B.number <= datediff(dd,'" + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + "','" + dtEndFillNull.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                    if (i == 0)
                    {
                        strSql.Append(sql);
                    }
                    else
                    {
                        strSql.AppendFormat(@" union {0}", sql);
                    }
                }
                strSql.Append(") AS M order by " + orderBy);
                DataView dt = g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
                return dt;
            }
            return null;

        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数,南通市辖区区域专用方法（补全不存在数据的日期）
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="OorA">原始或审核</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段(PointId,DateTime)</param>
        /// <returns></returns>
        public DataView GetRegionDataPagerNew(DateTime dtStart, DateTime dtEnd, string[] qualityType, int pageSize, int pageNo, string OorA, out int recordTotal, string orderBy = "DateTime desc")
        {
            string where1 = string.Empty;
            string where2 = string.Empty;
            string where = string.Empty;
            if (qualityType.Contains("无效天"))
            {
                if (qualityType.Length > 1)
                {
                    where1 += " or AQIValue is NULL or ltrim(rtrim(AQIValue))=''";
                }
                else
                {
                    where1 += " AQIValue is NULL or ltrim(rtrim(AQIValue))=''";
                }
                var list = qualityType.ToList();
                list.RemoveAt(list.IndexOf("无效天"));
                qualityType = list.ToArray();
            }
            string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qualityType.ToList<string>(), "','");
            if (qualityTypeStr != "")
            {
                qualityTypeStr = "'" + qualityTypeStr + "'";
            }
            if (!string.IsNullOrEmpty(qualityTypeStr))
            {
                where2 += "Class IN(" + qualityTypeStr + ")";
            }
            if (where1 != "")
                where += " (" + where2 + where1 + ")";
            else if (where2 != "")
                where += " " + where2;
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "DateTime desc";
            string tbname = "";
            string conn = "";
            if (OorA == "Audit")
            {
                tbname = " [AirReport].[TB_RegionDayAQIReport] ";
                conn = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
            }
            else if (OorA == "Ori")
            {
                tbname = " [Air].[TB_OriRegionDayAQIReport] ";
                conn = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
            }
            string field = @"   A.SO2,A.SO2_IAQI,
                                A.NO2,A.NO2_IAQI,
                                A.PM10,A.PM10_IAQI,
                                A.CO,A.CO_IAQI,A.MaxOneHourO3 ,
                                A.MaxOneHourO3_IAQI,
                                A.Max8HourO3,A.Max8HourO3_IAQI,
                                A.PM25,A.PM25_IAQI,
                                A.AQIValue,A.PrimaryPollutant,
                                A.Range,A.RGBValue,A.PicturePath,A.Class ,A.Grade,A.HealthEffect,A.TakeStep";
            string sql = string.Format(@"   select '南通市区' PointId,convert(varchar(10),dateadd(dd,number,'{0}'),120) DateTime,{1} from master..spt_values B 
                                            left join (select * from {5} where {2}) A on A.ReportDateTime = convert(varchar(10),dateadd(dd,number,'{0}'),120) 
                                            and A.MonitoringRegionUid='b6e983c4-4f92-4be3-bbac-d9b71c470640' where B.type='p'  and B.number <= datediff(dd,'{0}','{3}') order by {4}"
                                            , dtStart, field, where, dtEnd, orderBy, tbname);
            DataView dt = g_DatabaseHelper.ExecuteDataView(sql, conn);
            recordTotal = dt.Count;
            return dt;
        }

        /// <summary>
        /// 取得虚拟分页原始日数据查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段(PointId,DateTime)</param>
        /// <returns></returns>
        public DataView GetOriDataPagerNew(string[] portIds, DateTime dtStart, DateTime dtEnd, string[] qualityType, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            string con = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
            string where1 = string.Empty;
            string where2 = string.Empty;
            string where = string.Empty;
            if (qualityType.Contains("无效天"))
            {
                if (qualityType.Length > 1)
                {
                    where1 += " or AQIValue is NULL";
                }
                else
                {
                    where1 += " AQIValue is NULL";
                }
                var list = qualityType.ToList();
                list.RemoveAt(list.IndexOf("无效天"));
                qualityType = list.ToArray();
            }
            string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qualityType.ToList<string>(), "','");
            if (qualityTypeStr != "")
            {
                qualityTypeStr = "'" + qualityTypeStr + "'";
            }
            if (!string.IsNullOrEmpty(qualityTypeStr))
            {
                where2 += "Class IN(" + qualityTypeStr + ")";
            }
            if (where1 != "")
                where += " (" + where2 + where1 + ")";
            else if (where2 != "")
                where += " " + where2;
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
            string portIdsStr = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from (");
            if (portIds != null && portIds.Length > 0)
            {
                //DateTime dtEndFillNull = dtEnd > DateTime.Now ? DateTime.Now.AddDays(-1) : dtEnd.AddDays(-1);
                DateTime dtEndFillNull = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) ?
                    DateTime.Now.AddDays(-1) : dtEnd;
                for (int i = 0; i < portIds.Length; i++)
                {
                    portIdsStr = portIds[i];
                    string sql = "select '" + portIdsStr + "' PointId,convert(varchar(10),dateadd(dd,number,'" + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + "'),120) DateTime,A.SO2,A.SO2_IAQI,A.NO2,A.NO2_IAQI,A.PM10,A.PM10_IAQI,A.CO,A.CO_IAQI,A.MaxOneHourO3 ,A.MaxOneHourO3_IAQI,A.Max8HourO3,A.Max8HourO3_IAQI,A.PM25,A.PM25_IAQI,A.AQIValue,A.PrimaryPollutant,A.Range,A.RGBValue,A.PicturePath,A.Class ,A.Grade,A.HealthEffect,A.TakeStep  from master..spt_values B left join (select * from [Air].[TB_OriDayAQI] where " + where + ") A on A.DateTime = convert(varchar(10),dateadd(dd,number,'" + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + "'),120) and A.PointId='" + portIdsStr + "' where B.type='p'  and B.number <= datediff(dd,'" + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + "','" + dtEndFillNull.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                    if (i == 0)
                    {
                        strSql.Append(sql);
                    }
                    else
                    {
                        strSql.AppendFormat(@" union {0}", sql);
                    }
                }
                strSql.Append(") AS M order by " + orderBy);
                DataView dt = g_DatabaseHelper.ExecuteDataView(strSql.ToString(), con);
                recordTotal = dt.ToTable().Rows.Count;
                return dt;
            }
            recordTotal = 0;
            return null;
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
        public DataView GetDataPagerDayAQI(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "DateTime";
            recordTotal = 0;

            string fieldName = @"DateTime,AVG(CONVERT (decimal(18,6),[AQIValue])) as AQIValueAvg";
            string keyName = "id";
            string groupBy = "DateTime";
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
            where += string.Format(" AND DateTime>='{0}' AND DateTime<='{1}' and AQIValue is not null", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));

            return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
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
        public DataView GetConcentrationDay(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId")
        {
            recordTotal = 0;
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length > 0) portIdsStr = string.Format(" and PointId IN ({0}) ", portIdsStr);

                string sql = string.Format(@"SELECT PointId
                                ,[dbo].[F_Round](AVG(CONVERT (decimal(18,3),PM25)),3) as a34004
								,[dbo].[F_Round](AVG(CONVERT (decimal(18,3),PM10)),3) as a34002
								,[dbo].[F_Round](AVG(CONVERT (decimal(18,3),NO2)),3) as a21004
								,[dbo].[F_Round](AVG(CONVERT (decimal(18,3),SO2)),3) as a21026
								,[dbo].[F_Round](AVG(CONVERT (decimal(18,3),CO)),1) as a21005
								,[dbo].[F_Round](AVG(CONVERT (decimal(18,3),Max8HourO3)),3) as a05024
                                FROM {0}
                                WHERE DateTime >= '{1}' and DateTime <= '{2}' {3} 
                                GROUP BY PointId 
                                order by PointId", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.DayAQI), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
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
        /// 获取点位AQI数据，时间点补遗
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetPortAllData(string[] portIds, DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
            //查询条件拼接
            string where = string.Empty;
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                where = "Where PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                where = "Where PointId IN(" + portIdsStr + ")";
            }
            where += string.Format(" AND DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            string sql = string.Format(@"SELECT allInfo.*
                                                ,Convert(numeric(18,3),dayData.SO2) AS SO2,dayData.SO2_IAQI
                                                ,Convert(numeric(18,3),dayData.NO2) AS NO2,dayData.NO2_IAQI
                                                ,Convert(numeric(18,3),dayData.PM10) AS PM10,dayData.PM10_IAQI
                                                ,Convert(numeric(18,3),dayData.CO) AS CO,dayData.CO_IAQI
                                                ,Convert(numeric(18,3),dayData.MaxOneHourO3) AS MaxOneHourO3,dayData.MaxOneHourO3_IAQI
                                                ,Convert(numeric(18,3),dayData.Max8HourO3) AS Max8HourO3 ,dayData.Max8HourO3_IAQI
                                                ,Convert(numeric(18,3),dayData.PM25) AS PM25,dayData.PM25_IAQI
                                                ,dayData.AQIValue,dayData.PrimaryPollutant
                                                ,dayData.Range ,dayData.RGBValue
                                        FROM dbo.F_GetAllDataByDay('{1}',',','{2}','{3}') AS allInfo
                                        LEFT JOIN {0} dayData
	                                    ON allInfo.PointID = dayData.PointId AND allInfo.[DateTime] = dayData.[DateTime]
                                        WHERE allInfo.PointId IN({1}) AND allInfo.[DateTime]>='{2}' AND allInfo.[DateTime]<='{3}' "
                                         , tableName, string.Join(",", portIds), dtStart.ToString("yyyy-MM-dd"), dtEnd.ToString("yyyy-MM-dd"));

            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }


        /// <summary>
        /// 根据站点取得最新小时AQI数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetLastData(string[] portIds, DateTime dtStart, DateTime dtEnd)
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
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

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
                ", tableName, where, GetFieldName());
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
                        ,CO
                        ,CO_IAQI
                        ,MaxOneHourO3
                        ,MaxOneHourO3_IAQI
                        ,Max8HourO3
                        ,Max8HourO3_IAQI
                        ,PM25
                        ,PM25_IAQI
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

        /// <summary>
        /// 获取时间段内多点的AQI统计数据
        /// </summary>
        /// <param name="aqiType"></param>
        /// <param name="portIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataView GetMutilPointAQIData(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string sql = string.Format(@"SELECT * FROM [dbo].[F_PointAQIByTime]('{2}','{0}','{1}')", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), string.Join(";", portIds));
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region  百分位数浓度
        /// <summary>
        /// 百分位数浓度_区域
        /// </summary>
        /// <param name="regionguid"></param>
        /// <param name="dtbegin"></param>
        /// <param name="dtend"></param>
        /// <param name="factorcode"></param>
        /// <returns></returns>
        public string getpercent(string regionguid, DateTime dtbegin, DateTime dtend, string factorcode)
        {
            string pervalue = "";
            SqlConnection myConn = new SqlConnection(GetSqlConnection(connection));
            if (myConn.State != ConnectionState.Open)
            {
                myConn.Open();
            }
            System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand("UP_GetPerValue", myConn);
            myCommand.CommandType = CommandType.StoredProcedure;
            //添加输入查询参数、赋予值
            myCommand.Parameters.Add("@MonitoringRegionUid", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@MonitoringRegionUid"].Value = regionguid;
            myCommand.Parameters.Add("@factorCode", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@factorCode"].Value = factorcode;
            myCommand.Parameters.Add("@StartTime", SqlDbType.DateTime);
            myCommand.Parameters["@StartTime"].Value = dtbegin;
            myCommand.Parameters.Add("@EndTime", SqlDbType.DateTime);
            myCommand.Parameters["@EndTime"].Value = dtend;

            //添加输出参数
            myCommand.Parameters.Add("@RetrunValue", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@RetrunValue"].Direction = ParameterDirection.Output;
            myCommand.ExecuteNonQuery();
            //得到存储过程输出参数
            pervalue = myCommand.Parameters["@RetrunValue"].Value.ToString();

            if (myConn.State == ConnectionState.Open)
            {
                myConn.Close();
            }
            return pervalue;

        }
        /// <summary>
        /// 百分位数浓度_点位
        /// </summary>
        /// <param name="regionguid"></param>
        /// <param name="dtbegin"></param>
        /// <param name="dtend"></param>
        /// <param name="factorcode"></param>
        /// <returns></returns>
        public string getpercent_Point(string PointId, DateTime dtbegin, DateTime dtend, string factorcode)
        {
            string pervalue = "";
            SqlConnection myConn = new SqlConnection(GetSqlConnection(connection));
            if (myConn.State != ConnectionState.Open)
            {
                myConn.Open();
            }
            System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand("UP_GetPerValue_Day", myConn);
            myCommand.CommandType = CommandType.StoredProcedure;
            //添加输入查询参数、赋予值
            myCommand.Parameters.Add("@PointId", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@PointId"].Value = PointId;
            myCommand.Parameters.Add("@factorCode", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@factorCode"].Value = factorcode;
            myCommand.Parameters.Add("@StartTime", SqlDbType.DateTime);
            myCommand.Parameters["@StartTime"].Value = dtbegin;
            myCommand.Parameters.Add("@EndTime", SqlDbType.DateTime);
            myCommand.Parameters["@EndTime"].Value = dtend;

            //添加输出参数
            myCommand.Parameters.Add("@RetrunValue", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@RetrunValue"].Direction = ParameterDirection.Output;
            myCommand.ExecuteNonQuery();
            //得到存储过程输出参数
            pervalue = myCommand.Parameters["@RetrunValue"].Value.ToString();

            if (myConn.State == ConnectionState.Open)
            {
                myConn.Close();
            }
            return pervalue;

        }
        /// <summary>
        /// 取得连接字符串
        /// </summary>
        /// <param name="appSettingsName"></param>
        /// <returns></returns>
        private string GetSqlConnection(string appSettingsName)
        {
            return ConfigurationManager.ConnectionStrings[appSettingsName].ConnectionString;
        }
        #endregion
        #region 综合污染指数
        public string getSI(string regionguid, DateTime dtbegin, DateTime dtend, string[] factorcode)
        {
            string strfactorcode = string.Join(",", factorcode);
            string pervalue = "";
            SqlConnection myConn = new SqlConnection(GetSqlConnection(connection));
            if (myConn.State != ConnectionState.Open)
            {
                myConn.Open();
            }
            System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand("UP_GetSI", myConn);
            myCommand.CommandType = CommandType.StoredProcedure;
            //添加输入查询参数、赋予值
            myCommand.Parameters.Add("@MonitoringRegionUid", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@MonitoringRegionUid"].Value = regionguid;
            myCommand.Parameters.Add("@m_pollutantCodeList", SqlDbType.NVarChar, 1000);
            myCommand.Parameters["@m_pollutantCodeList"].Value = strfactorcode;
            myCommand.Parameters.Add("@m_begin", SqlDbType.DateTime);
            myCommand.Parameters["@m_begin"].Value = dtbegin;
            myCommand.Parameters.Add("@m_end", SqlDbType.DateTime);
            myCommand.Parameters["@m_end"].Value = dtend;

            //添加输出参数
            myCommand.Parameters.Add("@RetrunValue", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@RetrunValue"].Direction = ParameterDirection.Output;
            myCommand.ExecuteNonQuery();
            //得到存储过程输出参数
            pervalue = myCommand.Parameters["@RetrunValue"].Value.ToString();

            if (myConn.State == ConnectionState.Open)
            {
                myConn.Close();
            }
            return pervalue;

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
        /// 获取多点位平均后的AQI统计
        /// </summary>
        /// <param name="aqiType"></param>
        /// <param name="portIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataView GetGradeStatisticsMutilPoint(IAQIType aqiType, string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string sql = string.Format(@"
                SELECT Count(CASE WHEN {0} >= 0 AND {0}<=50 THEN 1 ELSE NULL END) AS Level1Count
                    ,Count(CASE WHEN {0} > 50 AND {0}<=100 THEN 1 ELSE NULL END) AS Level2Count
                    ,Count(CASE WHEN {0} > 100 AND {0}<=150 THEN 1 ELSE NULL END) AS Level3Count
                    ,Count(CASE WHEN {0} > 150 AND {0}<=200 THEN 1 ELSE NULL END) AS Level4Count
                    ,Count(CASE WHEN {0} > 200 AND {0}<=300 THEN 1 ELSE NULL END) AS Level5Count
                    ,Count(CASE WHEN {0} > 300 THEN 1 ELSE NULL END) AS Level6Count
                    ,Count(CASE WHEN {0} >= 0 AND {0} <= 100 THEN 1 ELSE NULL END) AS FineCount
                    ,Count(CASE WHEN {0} > 100 THEN 1 ELSE NULL END) AS OverCount
                    ,Count(CASE WHEN {0} >= 0 THEN 1 ELSE NULL END) AS ValidCount
                FROM [dbo].[F_PointAQIByTime]('{3}','{1}','{2}')", SmartEP.Core.Enums.EnumMapping.GetIAQITypeColumn(aqiType), dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), string.Join(";", portIds));
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

        /// <summary>
        /// 取得指定日期内日数据均值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// </returns>
        public DataView GetAvgValue(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length > 0) portIdsStr = string.Format(" and PointId IN ({0}) ", portIdsStr);
                string sql = string.Format(@"
                    SELECT PointId
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([PM25])),5)) AS [PM25]
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([PM10])),5)) AS [PM10]
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([NO2])),5)) AS [NO2]
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([SO2])),5)) AS [SO2]
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([CO])),5)) AS [CO]
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([MaxOneHourO3])),5)) AS [MaxOneHourO3]
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([Max8HourO3])),5)) AS [Max8HourO3]
                    FROM {0}
                    WHERE DateTime >= '{1}' and DateTime <= '{2}' {3}
                    GROUP BY PointId ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.DayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 取得指定日期内日数据最大值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
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
        public DataView GetMaxValue(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length > 0) portIdsStr = string.Format(" and PointId IN ({0}) ", portIdsStr);
                string sql = string.Format(@"
                    SELECT PointId
                        ,MAX(dbo.F_ValidValueStr([PM25])) AS [PM25]
                        ,MAX(dbo.F_ValidValueStr([PM10])) AS [PM10]
                        ,MAX(dbo.F_ValidValueStr([NO2])) AS [NO2]
                        ,MAX(dbo.F_ValidValueStr([SO2])) AS [SO2]
                        ,MAX(dbo.F_ValidValueStr([CO])) AS [CO]
                        ,MAX(dbo.F_ValidValueStr([MaxOneHourO3])) AS [MaxOneHourO3]
                        ,MAX(dbo.F_ValidValueStr([Max8HourO3])) AS [Max8HourO3]
                        ,MAX(dbo.F_ValidValueStr([PM25_IAQI])) AS [PM25_IAQI]
                        ,MAX(dbo.F_ValidValueStr([PM10_IAQI])) AS [PM10_IAQI]
                        ,MAX(dbo.F_ValidValueStr([NO2_IAQI])) AS [NO2_IAQI]
                        ,MAX(dbo.F_ValidValueStr([SO2_IAQI])) AS [SO2_IAQI]
                        ,MAX(dbo.F_ValidValueStr([CO_IAQI])) AS [CO_IAQI]
                        ,MAX(dbo.F_ValidValueStr([MaxOneHourO3_IAQI])) AS [MaxOneHourO3_IAQI]
                        ,MAX(dbo.F_ValidValueStr([Max8HourO3_IAQI])) AS [Max8HourO3_IAQI]
                        ,MAX(dbo.F_ValidValueStr([AQIValue])) AS [AQIValue]
                    FROM {0}
                    WHERE DateTime >= '{1}' and DateTime <= '{2}' {3}
                    GROUP BY PointId ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.DayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得指定日期内日数据最大值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
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
        public DataView GetMaxValueOne(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length > 0) portIdsStr = string.Format(" and PointId IN ({0}) ", portIdsStr);
                string sql = string.Format(@"
                    SELECT MAX(dbo.F_ValidValueStr([PM25])) AS [PM25]
                        ,MAX(dbo.F_ValidValueStr([PM10])) AS [PM10]
                        ,MAX(dbo.F_ValidValueStr([NO2])) AS [NO2]
                        ,MAX(dbo.F_ValidValueStr([SO2])) AS [SO2]
                        ,MAX(dbo.F_ValidValueStr([CO])) AS [CO]
                        ,MAX(dbo.F_ValidValueStr([MaxOneHourO3])) AS [MaxOneHourO3]
                        ,MAX(dbo.F_ValidValueStr([Max8HourO3])) AS [Max8HourO3]
                        ,MAX(dbo.F_ValidValueStr([PM25_IAQI])) AS [PM25_IAQI]
                        ,MAX(dbo.F_ValidValueStr([PM10_IAQI])) AS [PM10_IAQI]
                        ,MAX(dbo.F_ValidValueStr([NO2_IAQI])) AS [NO2_IAQI]
                        ,MAX(dbo.F_ValidValueStr([SO2_IAQI])) AS [SO2_IAQI]
                        ,MAX(dbo.F_ValidValueStr([CO_IAQI])) AS [CO_IAQI]
                        ,MAX(dbo.F_ValidValueStr([MaxOneHourO3_IAQI])) AS [MaxOneHourO3_IAQI]
                        ,MAX(dbo.F_ValidValueStr([Max8HourO3_IAQI])) AS [Max8HourO3_IAQI]
                        ,MAX(dbo.F_ValidValueStr([AQIValue])) AS [AQIValue]
                    FROM {0}
                    WHERE DateTime >= '{1}' and DateTime <= '{2}' {3}", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.DayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得指定日期内日数据最小值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
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
        public DataView GetMinValue(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length > 0) portIdsStr = string.Format(" and PointId IN ({0}) ", portIdsStr);
                string sql = string.Format(@"
                    SELECT PointId
                        ,MIN(dbo.F_ValidValueStr([PM25])) AS [PM25]
                        ,MIN(dbo.F_ValidValueStr([PM10])) AS [PM10]
                        ,MIN(dbo.F_ValidValueStr([NO2])) AS [NO2]
                        ,MIN(dbo.F_ValidValueStr([SO2])) AS [SO2]
                        ,MIN(dbo.F_ValidValueStr([CO])) AS [CO]
                        ,MIN(dbo.F_ValidValueStr([MaxOneHourO3])) AS [MaxOneHourO3]
                        ,MIN(dbo.F_ValidValueStr([Max8HourO3])) AS [Max8HourO3]
                        ,MIN(dbo.F_ValidValueStr([PM25_IAQI])) AS [PM25_IAQI]
                        ,MIN(dbo.F_ValidValueStr([PM10_IAQI])) AS [PM10_IAQI]
                        ,MIN(dbo.F_ValidValueStr([NO2_IAQI])) AS [NO2_IAQI]
                        ,MIN(dbo.F_ValidValueStr([SO2_IAQI])) AS [SO2_IAQI]
                        ,MIN(dbo.F_ValidValueStr([CO_IAQI])) AS [CO_IAQI]
                        ,MIN(dbo.F_ValidValueStr([MaxOneHourO3_IAQI])) AS [MaxOneHourO3_IAQI]
                        ,MIN(dbo.F_ValidValueStr([Max8HourO3_IAQI])) AS [Max8HourO3_IAQI]
                        ,MIN(dbo.F_ValidValueStr([AQIValue])) AS [AQIValue]
                    FROM {0}
                    WHERE DateTime >= '{1}' and DateTime <= '{2}' {3}
                    GROUP BY PointId ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.DayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得指定日期内日数据最小值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
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
        public DataView GetMinValueOne(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length > 0) portIdsStr = string.Format(" and PointId IN ({0}) ", portIdsStr);
                string sql = string.Format(@"
                    SELECT MIN(dbo.F_ValidValueStr([PM25])) AS [PM25]
                        ,MIN(dbo.F_ValidValueStr([PM10])) AS [PM10]
                        ,MIN(dbo.F_ValidValueStr([NO2])) AS [NO2]
                        ,MIN(dbo.F_ValidValueStr([SO2])) AS [SO2]
                        ,MIN(dbo.F_ValidValueStr([CO])) AS [CO]
                        ,MIN(dbo.F_ValidValueStr([MaxOneHourO3])) AS [MaxOneHourO3]
                        ,MIN(dbo.F_ValidValueStr([Max8HourO3])) AS [Max8HourO3]
                        ,MIN(dbo.F_ValidValueStr([PM25_IAQI])) AS [PM25_IAQI]
                        ,MIN(dbo.F_ValidValueStr([PM10_IAQI])) AS [PM10_IAQI]
                        ,MIN(dbo.F_ValidValueStr([NO2_IAQI])) AS [NO2_IAQI]
                        ,MIN(dbo.F_ValidValueStr([SO2_IAQI])) AS [SO2_IAQI]
                        ,MIN(dbo.F_ValidValueStr([CO_IAQI])) AS [CO_IAQI]
                        ,MIN(dbo.F_ValidValueStr([MaxOneHourO3_IAQI])) AS [MaxOneHourO3_IAQI]
                        ,MIN(dbo.F_ValidValueStr([Max8HourO3_IAQI])) AS [Max8HourO3_IAQI]
                        ,MIN(dbo.F_ValidValueStr([AQIValue])) AS [AQIValue]
                    FROM {0}
                    WHERE DateTime >= '{1}' and DateTime <= '{2}' {3}", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.DayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得指定日期内日数据样本数
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
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
        public DataView GetCountValue(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length > 0) portIdsStr = string.Format(" and PointId IN ({0}) ", portIdsStr);
                string sql = string.Format(@"
                    SELECT PointId
                        ,Count(dbo.F_ValidValueStr([PM25])) AS [PM25]
                        ,Count(dbo.F_ValidValueStr([PM10])) AS [PM10]
                        ,Count(dbo.F_ValidValueStr([NO2])) AS [NO2]
                        ,Count(dbo.F_ValidValueStr([SO2])) AS [SO2]
                        ,Count(dbo.F_ValidValueStr([CO])) AS [CO]
                        ,Count(dbo.F_ValidValueStr([MaxOneHourO3])) AS [MaxOneHourO3]
                        ,Count(dbo.F_ValidValueStr([Max8HourO3])) AS [Max8HourO3]
                        ,Count(dbo.F_ValidValueStr([PM25_IAQI])) AS [PM25_IAQI]
                        ,Count(dbo.F_ValidValueStr([PM10_IAQI])) AS [PM10_IAQI]
                        ,Count(dbo.F_ValidValueStr([NO2_IAQI])) AS [NO2_IAQI]
                        ,Count(dbo.F_ValidValueStr([SO2_IAQI])) AS [SO2_IAQI]
                        ,Count(dbo.F_ValidValueStr([CO_IAQI])) AS [CO_IAQI]
                        ,Count(dbo.F_ValidValueStr([MaxOneHourO3_IAQI])) AS [MaxOneHourO3_IAQI]
                        ,Count(dbo.F_ValidValueStr([Max8HourO3_IAQI])) AS [Max8HourO3_IAQI]
                        ,Count(dbo.F_ValidValueStr([AQIValue])) AS [AQIValue]
                    FROM {0}
                    WHERE DateTime >= '{1}' and DateTime <= '{2}' {3}
                    GROUP BY PointId ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.DayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 日数据超标天数统计
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
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
        public DataView GetExceedingData(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length > 0) portIdsStr = string.Format(" and PointId IN ({0}) ", portIdsStr);
                string sql = string.Format(@"
                    SELECT PointId
	                    ,SUM(CASE WHEN [AQIValue] >=0 THEN 1 ELSE 0 END) AS ValidCount
	                    ,SUM(CASE WHEN [AQIValue] > 100 THEN 1 ELSE 0 END) AS OverCount
                        ,SUM(CASE WHEN [PM25_IAQI] > 100 THEN 1 ELSE 0 END) AS PM25_Over
                        ,SUM(CASE WHEN [PM10_IAQI] > 100 THEN 1 ELSE 0 END) AS PM10_Over
                        ,SUM(CASE WHEN [NO2_IAQI] > 100 THEN 1 ELSE 0 END) AS NO2_Over
                        ,SUM(CASE WHEN [SO2_IAQI] > 100 THEN 1 ELSE 0 END) AS SO2_Over
                        ,SUM(CASE WHEN [CO_IAQI] > 100 THEN 1 ELSE 0 END) AS CO_Over
                        ,SUM(CASE WHEN [MaxOneHourO3_IAQI] > 100 THEN 1 ELSE 0 END) AS MaxOneHourO3_Over
                        ,SUM(CASE WHEN [Max8HourO3_IAQI] > 100 THEN 1 ELSE 0 END) AS Max8HourO3_Over
                    FROM {0}
                    WHERE DateTime >= '{1}' and DateTime <= '{2}' {3} 
                    GROUP BY PointId ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.DayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region << 生成点位小时AQI >>
        /// <summary>
        /// 生成点位日AQI
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

                g_DBBiz.ExecuteProcNonQuery("UP_AirReport_DayAQI_Port_Mul", connection);
            }
            catch (Exception ex)
            {
                //return null;
                throw ex;
            }
        }
        #endregion

        #region 获取站点日AQI数据
        /// <summary>
        /// 获取站点日AQI数据
        /// </summary>
        /// <param name="PointId">站点Id</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">截止日期</param>
        /// <returns>DataView</returns>
        public DataView GetDataByPoint(int PointId, DateTime StartDate, DateTime EndDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from " + tableName);
            strSql.Append(" where PointId=" + PointId + " and DateTime>='" + StartDate + "' and DateTime<='" + EndDate + "' ");
            strSql.Append(" and AQIValue is not null order by DateTime");
            return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
        }
        #endregion

        #region 空气质量日报统计专用方法
        /// <summary>
        /// 获取时间段内指定测点的日数据
        /// </summary>
        /// <param name="PointIds">测点ID数组</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetDayAQIData(List<int> PointIds, DateTime StartDate, DateTime EndDate)
        {
            if (PointIds == null || PointIds.Count == 0 || StartDate == null || EndDate == null || EndDate < StartDate)
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select b.MonitoringPointName as PointName,a.DateTime,a.SO2,a.SO2_IAQI,a.NO2,a.NO2_IAQI,a.PM10,a.PM10_IAQI,a.CO,a.CO_IAQI");
            strSql.Append(",a.MaxOneHourO3 as O3,a.MaxOneHourO3_IAQI as O3_IAQI,a.Max8HourO3,a.Max8HourO3_IAQI");
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
        public DataTable GetAvgDayAQIData(List<int> PointIds, DateTime StartDate, DateTime EndDate)
        {
            if (PointIds == null || PointIds.Count == 0 || StartDate == null || EndDate == null || EndDate < StartDate)
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CONVERT(decimal(18,4),AVG(CAST ((case SO2 when null then '0' else SO2 end) as decimal(18,4)))) as AVGSO2 ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case SO2 when null then '0' else SO2 end) as decimal(18,4)))),'a21026',24) as AVGSO2_IAQI ");
            strSql.Append(",CONVERT(decimal(18,4),AVG(CAST ((case NO2 when null then '0' else NO2 end) as decimal(18,4)))) as AVGNO2 ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case NO2 when null then '0' else NO2 end) as decimal(18,4)))),'a21004',24) as AVGNO2_IAQI ");
            strSql.Append(",CONVERT(decimal(18,4),AVG(CAST ((case PM10 when null then '0' else PM10 end) as decimal(18,4)))) as AVGPM10 ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case PM10 when null then '0' else PM10 end) as decimal(18,4)))),'a34002',24) as AVGPM10_IAQI ");
            strSql.Append(",CONVERT(decimal(18,4),AVG(CAST ((case CO when null then '0' else CO end) as decimal(18,4)))) as AVGCO ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case CO when null then '0' else CO end) as decimal(18,4)))),'a21005',24) as AVGCO_IAQI ");
            strSql.Append(",CONVERT(decimal(18,4),AVG(CAST ((case MaxOneHourO3 when null then '0' else MaxOneHourO3 end) as decimal(18,4)))) as AVGO3 ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case MaxOneHourO3 when null then '0' else MaxOneHourO3 end) as decimal(18,4)))),'a05024',1) as AVGO3_IAQI ");
            strSql.Append(",CONVERT(decimal(18,4),AVG(CAST ((case Max8HourO3 when null then '0' else Max8HourO3 end) as decimal(18,4)))) as AVGO3_8 ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case Max8HourO3 when null then '0' else Max8HourO3 end) as decimal(18,4)))),'a05024',8) as AVGO3_8_IAQI ");
            strSql.Append(",CONVERT(decimal(18,4),AVG(CAST ((case PM25 when null then '0' else PM25 end) as decimal(18,4)))) as AVGPM25 ");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case PM25 when null then '0' else PM25 end) as decimal(18,4)))),'a34004',24) as AVGPM25_IAQI ");
            strSql.Append(",dbo.F_GetAQI_Max_CNV_Day(dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case SO2 when null then '0' else SO2 end) as decimal(18,4)))),'a21026',24)");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case NO2 when null then '0' else NO2 end) as decimal(18,4)))),'a21004',24)");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case PM10 when null then '0' else PM10 end) as decimal(18,4)))),'a34002',24)");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case CO when null then '0' else CO end) as decimal(18,4)))),'a21005',24)");
            strSql.Append(",dbo.F_GetAQI(CONVERT(decimal(18,4),AVG(CAST ((case Max8HourO3 when null then '0' else Max8HourO3 end) as decimal(18,4)))),'a05024',8)");
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

        /// <summary>
        /// 查询某时间段内各个站点对应的AQIValue和日期
        /// </summary>
        /// <param name="portIds">站点名称</param>
        /// <param name="classDics">污染级别</param>
        /// <param name="dtBegion">开始日期</param>
        /// <param name="dtEnd">结束日期</param>
        /// <returns></returns>
        public DataView GetPointPager(string[] portIds, string[] classDics, DateTime dtBegion, DateTime dtEnd)
        {
            try
            {
                string tableName = "AirRelease.TB_DayAQI";
                string sql = string.Empty;
                sql = string.Format(@"select PointId,AQIValue,DateTime from {0} where DateTime >= '{1}' AND   DateTime <= '{2}' order by DateTime  ", tableName, dtBegion, dtEnd);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询某段时间内各个站点的AQIValue，日期以及首要污染物
        /// </summary>
        /// <param name="portIds">站点名称</param>
        /// <param name="classDics">污染级别</param>
        /// <param name="dtBegion">开始日期</param>
        /// <param name="dtEnd">结束日期</param>
        /// <returns></returns>
        public DataView GetFirstPollute(string[] portIds, string[] classDics, DateTime dtBegion, DateTime dtEnd)
        {
            try
            {
                string tableName = "AirRelease.TB_DayAQI";
                string sql = string.Empty;
                sql = string.Format(@"select PointId,AQIValue,DateTime,PrimaryPollutant from {0} where DateTime>='{1}' AND  DateTime <= '{2}' order by DateTime", tableName, dtBegion, dtEnd);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
