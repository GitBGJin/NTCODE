using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.AutoMonitoring
{
    public class MicrowaveRadiationDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();


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
        /// <param name="aQIDataType">AQI数据类型</param>
        public MicrowaveRadiationDAL()
        {
            // tableName = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);
        }
        #endregion

        #region << 数据查询方法 >>
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetDataBasePager()
        {
            string tableName1 = "TB_OMMP_InstrumentInfo";
            try
            {
                string sql = string.Format(@"select id, [RowGuid],InstrumentName+'/'+SpecificationModel as InstrumentType ,SpecificationModel
                                             from {0} 
                                            where ObjectType=2 and RowStatus=1",
                                           tableName1);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="fatror">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetWeiboDataPager(string[] portIds, string[] fatror, DateTime dtBegin, DateTime dtEnd)
        {
            string tableName1 = "TB_SuperStation_Weibo";
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
                string snStr = string.Empty;
                if (fatror != null && fatror.Length > 0)
                    snStr = " AND [PollutantCode] IN ('" + StringExtensions.GetArrayStrNoEmpty(fatror.ToList(), "','") + "')";
                string sql = string.Format(@"select *
											from {0}
                                             WHERE DateTime >= '{1}' and DateTime <= '{2}' {3} {4}
                                             order by DateTime",
                                           tableName1, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }


        #endregion
    }
}
