using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness
{
    /// <summary>
    /// 名称：ELMAHDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 异常日志(软件平台异常日志、系统运行环境异常日志、通讯数据包异常日志)
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ELMAHDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 虚拟分页类
        /// </summary>
        GridViewPagerDAL g_GridViewPager = Singleton<GridViewPagerDAL>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = string.Empty;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection1 = string.Empty;

        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = string.Empty;
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName1 = string.Empty;
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public ELMAHDAL()
        {
            tableName = "dbo.TB_ELMAH";
            tableName1 = "dbo.STA_Log";
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
            connection1 = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="applicationType">应用类型（空气、地表水、噪声）</param>
        /// <param name="monitoringPointUids">测点数据</param>
        /// <param name="types">日志类型</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(ApplicationType applicationType, string[] monitoringPointUids, string[] types, DateTime dateStart, DateTime dateEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "ExceptionTime")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string portIdsStr = string.Empty;
                if (monitoringPointUids != null && monitoringPointUids.Length > 0)
                {
                    portIdsStr = " AND MonitoringPointUid IN ('" + StringExtensions.GetArrayStrNoEmpty(monitoringPointUids.ToList<string>(), "','") + "')";
                }
                string typesStr = string.Empty;
                if (types != null && types.Length > 0)
                {
                    typesStr = " AND Type IN ('" + StringExtensions.GetArrayStrNoEmpty(types.ToList<string>(), "','") + "')";
                }

                string applicationUidstr = string.Empty;
                applicationUidstr = " AND ApplicationUid = '" + SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationType) + "'";

                orderBy = string.IsNullOrEmpty(orderBy) ? "ExceptionTime" : orderBy;
                string fieldName = @"
                    ErrorId
	                ,ApplicationUid
	                ,MonitoringPointUid
	                ,MonitoringPointName
	                ,Type
	                ,Source
	                ,[User]
	                ,StatusCode
	                ,Sequence
	                ,AllXml
	                ,ExceptionTime
	                ,HandleUser
	                ,HandleDateTime
	                ,HandleMessage
                    ,null as blankspaceColumn";
                string keyName = "ErrorId";
                string where = string.Format(" ExceptionTime>='{0}' AND ExceptionTime<='{1}' ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + typesStr + applicationUidstr;
                return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="applicationType">应用类型（空气、地表水、噪声）</param>
        /// <param name="monitoringPointUids">测点数据</param>
        /// <param name="types">日志类型</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataNewPager(ApplicationType applicationType, string[] monitoringPointUids, string[] types, DateTime dateStart, DateTime dateEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "tstamp")
        {
            recordTotal = 0;
            try
            {
                string portIdsStr = string.Empty;
                if (monitoringPointUids != null && monitoringPointUids.Length > 0)
                {
                    portIdsStr = " AND portid IN ('" + StringExtensions.GetArrayStrNoEmpty(monitoringPointUids.ToList<string>(), "','") + "')";
                }
                string typesStr = string.Empty;
                if (types != null && types.Length > 0)
                {
                    typesStr = " AND logType IN ('" + StringExtensions.GetArrayStrNoEmpty(types.ToList<string>(), "','") + "')";
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "tstamp" : orderBy;

                string sql = string.Format(@"SELECT portid as PointId
				  ,[tstamp]
				  ,[logType]
				  ,[logContent]
				  ,[signature]
				  ,[memo], b.ItemText
                  FROM {0} as a left join dbo.SY_View_CodeMainItem  as b on  a.logType=b.ItemValue and b.codeName='系统日志类型'
                  WHERE tstamp>='{1}' AND tstamp<='{2}' {3} {4}
                  order by portid ,tstamp desc", tableName1, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr, typesStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);

            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="applicationType">应用类型（空气、地表水、噪声）</param>
        /// <param name="monitoringPointUids">测点数据</param>
        /// <param name="types">日志类型</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(ApplicationType applicationType, string[] monitoringPointUids, string[] types, DateTime dateStart, DateTime dateEnd, string orderBy = "ExceptionTime")
        {
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;
            try
            {
                //取得查询行转列字段拼接
                string portIdsStr = string.Empty;
                if (monitoringPointUids != null && monitoringPointUids.Length > 0)
                {
                    portIdsStr = " AND MonitoringPointUid IN ('" + StringExtensions.GetArrayStrNoEmpty(monitoringPointUids.ToList<string>(), "','") + "'";
                }
                string typesStr = string.Empty;
                if (types != null && types.Length > 0)
                {
                    typesStr = " AND Type IN ('" + StringExtensions.GetArrayStrNoEmpty(types.ToList<string>(), "','") + "'";
                }
                string applicationUidstr = string.Empty;
                applicationUidstr = " AND ApplicationUid = '" + SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationType) + "'";

                orderBy = string.IsNullOrEmpty(orderBy) ? "ExceptionTime" : orderBy;
                string fieldName = @"
                    ErrorId
	                ,ApplicationUid
	                ,MonitoringPointUid
	                ,MonitoringPointName
	                ,Type
	                ,Source
	                ,[User]
	                ,StatusCode
	                ,Sequence
	                ,AllXml
	                ,ExceptionTime
	                ,HandleUser
	                ,HandleDateTime
	                ,HandleMessage
                    ,null as blankspaceColumn";
                string keyName = "ErrorId";
                string where = string.Format(" ExceptionTime>='{0}' AND ExceptionTime<='{1}' ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + typesStr + applicationUidstr;
                return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
