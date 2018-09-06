using log4net;
using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.AutoMonitoring
{
    /// <summary>
    /// 名称：OrigionAQIDAL.cs
    /// 创建人：吕云
    /// 创建日期：2017-07-06
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：原始AQI数据处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class OrigionAQIDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string connection = "AMS_AirAutoMonitorConnection";
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        #endregion

        #region <<方法>>
        /// <summary>
        /// 获取南通市辖区区域小时AQI信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetOriRegionHourAQI(string RegionUid)
        {
            try
            {
                string sql = string.Format(@"select top 1  * FROM [Air].[TB_OriRegionHourAQI]
where MonitoringRegionUid='{0}'  and AQIValue is not null
order by DateTime desc", RegionUid);
                return g_DatabaseHelper.ExecuteDataTable(sql, connection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取南通市辖区区域日AQI信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetOriRegionLastDayAQI(string RegionUid)
        {
            try
            {
                string sql = string.Format(@"select top 2  *   FROM [Air].[TB_OriRegionDayAQIReport]
where MonitoringRegionUid='{0}' 
order by [ReportDateTime] desc", RegionUid);
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
