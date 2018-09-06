using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTDataProcessApplication
{
    /// <summary>
    /// 名称：AQIDAL.cs
    /// 创建人：吕云
    /// 创建日期：2017-5-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：AQI计算
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AQICalculateDAL
    {
        #region <<变量>>
        /// <summary>
        /// 获取一个日志记录器
        /// </summary>
        ILog log = LogManager.GetLogger("App.Logging");

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string BaseDataConnection = "AMS_BaseDataConnection";
        private string AirAutoMonitorConnection = "AMS_AirAutoMonitorConnection";
        private string Frame_Connection = "Frame_Connection";
        private string MonitoringBusinessConnection = "AMS_MonitoringBusinessConnection";

        DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
        #endregion
        #region <<方法>>
        /// <summary>
        /// 获取浓度限值和空气质量分指数
        /// </summary>
        /// <param name="PollutantCode">因子编码</param>
        /// <param name="TimeTypeUid">时间类型Uid</param>
        /// <returns></returns>
        public DataTable GetFactorLimitAndIAQI(string PollutantCode, string TimeTypeUid)
        {
            try
            {
                string sql = string.Format(@"select * from [Audit].[TB_AQI]
                                            where [PollutantCode]='{0}'
                                            and [TimeTypeUid]='{1}'
                                            order by [IAQI]", PollutantCode, TimeTypeUid);
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点小时因子浓度数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="PollutantCode">因子编码</param>
        /// <param name="Tstamp">时间</param>
        /// <returns></returns>
        public DataTable GetHourRegionValue(string[] PointIds, string PollutantCode, DateTime Tstamp)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Format(@"select * from [dbo].[SY_Air_InfectantBy60]
                                                where 1=1  and PointId in({0}) and [PollutantCode]='{1}'
                                                and Tstamp ='{2}'
                                                and [dbo].[F_ValidValueByFlagNT](PollutantValue,[Status],',') is not null"
                , pointStr, PollutantCode, Tstamp);
                //log.Info("计算区域浓度-------"+sql.ToString());
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取区域时间段数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="PollutantCode">因子</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataTable GetHourRegionValueByTime(string[] PointIds, string PollutantCode, DateTime StartTime, DateTime EndTime)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Format(@"select PointId, AVG(PollutantValue) PollutantValue from [dbo].[SY_Air_InfectantBy60]
                                                where 1=1  and PointId in({0}) and [PollutantCode]='{1}'
                                                and Tstamp >='{2}' and tstamp<='{3}'
                                                and [dbo].[F_ValidValueByFlagNT](PollutantValue,[Status],',') is not null
                                                group by PointId"
                , pointStr, PollutantCode, StartTime, EndTime);
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点日因子浓度数据
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCode"></param>
        /// <param name="Tstamp"></param>
        /// <returns></returns>
        public DataTable GetDayRegionValue(string[] PointIds, string PollutantCode, DateTime Tstamp)
        {
            try
            {
                string time = Tstamp.ToString("yyyy-MM-dd");
                string pointStr = string.Join(",", PointIds);
                //获取有效日数据
                string sql = string.Format(@"select * from [dbo].[SY_Air_InfectantByDay]
                                                where 1=1  and PointId in({0}) and [PollutantCode]='{1}'
                                                and Datetime ='{2}'
                                                and PollutantValue is not null"
                , pointStr, PollutantCode, time);
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点时间段数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="PollutantCode">因子</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataTable GetDayRegionValueByTime(string[] PointIds, string PollutantCode, DateTime StartTime, DateTime EndTime)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                //获取有效日数据
                string sql = string.Format(@"select * from [dbo].[SY_Air_InfectantByDay]
                                                where 1=1  and PointId in({0}) and [PollutantCode]='{1}'
                                                and Datetime >='{2}' and Datetime<='{3}'
                                                and PollutantValue is not null"
                , pointStr, PollutantCode, StartTime, EndTime);
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点可跨天臭氧8小时数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="Tstamp">时间</param>
        /// <returns></returns>
        public DataTable GetO3_8NTRegionValue(string[] PointIds, DateTime Tstamp)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                //获取最大臭氧8小时数据与其他日AQI参数数据
                string sql = string.Format(@"select * from [Air].[TB_OriHourAQI]
  where 1=1 and PointId in({0})
  and DateTime ='{1}'
  and Recent8HoursO3NT is not null and Recent8HoursO3NT !=''"
                    , pointStr, Tstamp);
                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点最大臭氧8小时数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="Tstamp">时间</param>
        /// <returns></returns>
        public DataTable GetO3_8RegionValue(string[] PointIds, DateTime Tstamp)
        {
            try
            {
                string time = Tstamp.ToString("yyyy-MM-dd");
                string pointStr = string.Join(",", PointIds);
                //获取最大臭氧8小时数据与其他日AQI参数数据
                string sql = string.Format(@"select * from [Air].[TB_OriDayAQI]
  where 1=1 and PointId in({0})
  and DateTime ='{1}'
  and Max8HourO3 is not null and Max8HourO3 !=''"
                    , pointStr, time);
                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点臭氧时间段均值
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataTable GetO3_8RegionValueByTime(string[] PointIds, DateTime StartTime, DateTime EndTime)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                //获取最大臭氧8小时数据与其他日AQI参数数据
                string sql = string.Format(@"select * from [Air].[TB_OriDayAQI]
  where 1=1 and PointId in({0})
  and DateTime>='{1}' and DateTime<='{2}'
  and Max8HourO3 is not null and Max8HourO3 !=''"
                    , pointStr, StartTime, EndTime);
                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 获取因子相关配置信息
        /// </summary>
        /// <param name="PollutantCode"></param>
        /// <returns></returns>
        public DataTable GetPollutantUnit(string PollutantCode)
        {
            try
            {
                string sql = string.Format(@"select * from [dbo].[V_Factor_Air_SiteMap] where [PID]='{0}'", PollutantCode);
                return g_DatabaseHelper.ExecuteDataTable(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }

        }
        /// <summary>
        /// 获取数据无效标记位配置信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetFlag(string[] Flag)
        {
            try
            {
                string FlagStr = "'";
                FlagStr += string.Join("','", Flag);
                FlagStr += "'";

                string sql = string.Format(@"select * from [dbo].[DT_Flag] where [EnableOrNot]=1 and isValid=0 and Flag in({0})", FlagStr);
                return g_DatabaseHelper.ExecuteDataTable(sql, BaseDataConnection);
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
