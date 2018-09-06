using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTAnalysisApplication
{
    public class DAL
    {
        #region <<变量>>
        /// <summary>
        /// 获取一个日志记录器
        /// </summary>
        ILog log = LogManager.GetLogger("App.Logging");

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string BaseData_Connection = "AMS_BaseDataConnection";
        private string AirAutoMonitor_Connection = "AMS_AirAutoMonitorConnection";

        DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
        #endregion

        #region <<方法>>
        /// <summary>
        /// 获取所有因子
        /// </summary>
        /// <returns></returns>
        public DataTable GetFactors()
        {
            try
            {
                string sql = string.Format(@" select * from [dbo].[SYS_Factors_Mapping] order by id");
                return g_DatabaseHelper.ExecuteDataTable(sql, BaseData_Connection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取所有站点
        /// </summary>
        /// <returns></returns>
        public DataTable GetPoint()
        {
            try
            {
                string sql = string.Format(@" select * from [dbo].[SYS_Point_Mapping] order by id");
                return g_DatabaseHelper.ExecuteDataTable(sql, BaseData_Connection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 新增小时缓存数据
        /// </summary>
        /// <param name="PointId">站点Id</param>
        /// <param name="Tstamp">时间</param>
        /// <param name="ReceiveTime">接收时间</param>
        /// <param name="Status">数据标记</param>
        /// <param name="DataFlag">原始标记</param>
        /// <param name="AuditFlag">审核标记</param>
        /// <param name="PollutantCode">因子code</param>
        /// <param name="PollutantValue">因子值</param>
        /// <param name="MonitoringDataTypeCode">监控数据类型Code</param>
        /// <param name="CreatUser">创建人</param>
        /// <param name="CreatDateTime">创建时间</param>
        /// <returns></returns>
        public bool Add60Buffer(string PointId, DateTime Tstamp, DateTime ReceiveTime, string Status, string DataFlag, string AuditFlag, string PollutantCode, decimal? PollutantValue, string MonitoringDataTypeCode, string CreatUser, DateTime CreatDateTime)
        {
            try
            {
                string ParaName = string.Empty;//参数名
                string ParaValue = string.Empty;//参数值

                ParaName += "PointId";
                ParaValue += string.Format("'{0}'", PointId);

                if (Tstamp!=null)
                {
                    ParaName += ",Tstamp";
                    ParaValue += string.Format(",'{0}'", Tstamp);
                }
                if (ReceiveTime != null)
                {
                    ParaName += ",ReceiveTime";
                    ParaValue += string.Format(",'{0}'", ReceiveTime);
                }
                if (!string.IsNullOrWhiteSpace(Status))
                {
                    ParaName += ",Status";
                    ParaValue += string.Format(",'{0}'", Status);
                }
                if (!string.IsNullOrWhiteSpace(DataFlag))
                {
                    ParaName += ",DataFlag";
                    ParaValue += string.Format(",'{0}'", DataFlag);
                }
                if (!string.IsNullOrWhiteSpace(AuditFlag))
                {
                    ParaName += ",AuditFlag";
                    ParaValue += string.Format(",'{0}'", AuditFlag);
                }
                if (!string.IsNullOrWhiteSpace(PollutantCode))
                {
                    ParaName += ",PollutantCode";
                    ParaValue += string.Format(",'{0}'", PollutantCode);
                }
                if (PollutantValue!=null)
                {
                    ParaName += ",PollutantValue";
                    ParaValue += string.Format(",'{0}'", PollutantValue);
                }
                if (!string.IsNullOrWhiteSpace(MonitoringDataTypeCode))
                {
                    ParaName += ",MonitoringDataTypeCode";
                    ParaValue += string.Format(",'{0}'", MonitoringDataTypeCode);
                }
                if (!string.IsNullOrWhiteSpace(CreatUser))
                {
                    ParaName += ",CreatUser";
                    ParaValue += string.Format(",'{0}'", CreatUser);
                }
                if (CreatDateTime != null)
                {
                    ParaName += ",CreatDateTime";
                    ParaValue += string.Format(",'{0}'", CreatDateTime);
                }
                string sql = string.Format(@"  insert into [Air].[TB_InfectantBy60Buffer] ({0}) values ({1})", ParaName, ParaValue);

                g_DatabaseHelper.ExecuteInsert(sql, AirAutoMonitor_Connection);
                return true;

            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return false;
            }
        }
        #endregion
    }
}
