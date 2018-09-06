using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Utilities.AdoData;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SmartEP.Data.SqlServer.MonitoringBusiness
{
    public class AuditPreDataDAL
    {
        #region << 属性 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        BaseDAHelper g_DBBiz = Singleton<BaseDAHelper>.GetInstance();
        BaseDAHelper g_DBBiz_NotAudit = Singleton<BaseDAHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_MonitoringBusinessConnection";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="applicationType">应用类型（空气、地表水）</param>
        public AuditPreDataDAL()
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Hour);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 加载预处理数据
        /// </summary>
        /// <param name="applicationUID">应用程序UID</param>
        /// <param name="PointID">点位ID</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="IsAudit">是否审核过（审核过从审核小时数据导入，未审核从原始数据导入）</param>
        /// <returns></returns>
        public bool PreData_Load(string applicationUID, string[] PointID, DateTime beginTime, DateTime endTime, bool IsAudit)
        {
            try
            {
                SqlParameter m_pointID = new SqlParameter("@PointList", SqlDbType.NVarChar, 0);
                SqlParameter m_beginTime = new SqlParameter("@DTBegin", SqlDbType.NVarChar, 0);
                SqlParameter m_endTime = new SqlParameter("@DTEnd", SqlDbType.NVarChar, 0);
                SqlParameter m_applicationUID = new SqlParameter("@ApplicationID", SqlDbType.NVarChar, 0);
                g_DBBiz.ClearParameters();
                g_DBBiz.SetProcedureParameters(m_pointID, string.Join(",", PointID));
                g_DBBiz.SetProcedureParameters(m_beginTime, beginTime);
                g_DBBiz.SetProcedureParameters(m_endTime, endTime);
                g_DBBiz.SetProcedureParameters(m_applicationUID, applicationUID);

                SqlParameter m_pointID_NotAudit = new SqlParameter("@PointList", SqlDbType.NVarChar, 0);
                SqlParameter m_beginTime_NotAudit = new SqlParameter("@DTBegin", SqlDbType.NVarChar, 0);
                SqlParameter m_endTime_NotAudit = new SqlParameter("@DTEnd", SqlDbType.NVarChar, 0);
                SqlParameter m_applicationUID_NotAudit = new SqlParameter("@ApplicationID", SqlDbType.NVarChar, 0);
                g_DBBiz_NotAudit.ClearParameters();
                g_DBBiz_NotAudit.SetProcedureParameters(m_pointID_NotAudit, string.Join(",", PointID));
                g_DBBiz_NotAudit.SetProcedureParameters(m_beginTime_NotAudit, beginTime);
                g_DBBiz_NotAudit.SetProcedureParameters(m_endTime_NotAudit, endTime);
                g_DBBiz_NotAudit.SetProcedureParameters(m_applicationUID_NotAudit, applicationUID);
                if (IsAudit)
                    g_DBBiz.ExecuteProcNonQuery("P_Load_AuditPreData_HasAudit", connection);
                else
                    g_DBBiz_NotAudit.ExecuteProcNonQuery("P_Load_AuditPreData_NotAudit", connection);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新审核预处理小时状态表
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="PointType"></param>
        /// <returns></returns>
        public bool UpdataAuditHourStatus(string applicationUID, string[] PointID, DateTime beginTime, DateTime endTime, string PointType)
        {
            try
            {
                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                {
                    SqlParameter m_beginTime = new SqlParameter("@m_begin", SqlDbType.DateTime, 0);
                    SqlParameter m_endTime = new SqlParameter("@m_end", SqlDbType.DateTime, 0);
                    SqlParameter m_pointType = new SqlParameter("@PointType", SqlDbType.NVarChar, 0);
                    SqlParameter m_pointID = new SqlParameter("@Pointlist", SqlDbType.NVarChar, 0);
                    g_DBBiz.ClearParameters();
                    g_DBBiz.SetProcedureParameters(m_pointID, string.Join(",", PointID));
                    g_DBBiz.SetProcedureParameters(m_beginTime, beginTime);
                    g_DBBiz.SetProcedureParameters(m_endTime, endTime);
                    g_DBBiz.SetProcedureParameters(m_pointType, PointType);
                    g_DBBiz.ExecuteProcNonQuery("UP_AuditStatusForHour_Air", connection);//空气
                }
                else
                {
                    SqlParameter m_beginTime_NotAudit = new SqlParameter("@m_begin", SqlDbType.DateTime, 0);
                    SqlParameter m_endTime_NotAudit = new SqlParameter("@m_end", SqlDbType.DateTime, 0);
                    SqlParameter m_pointType_NotAudit = new SqlParameter("@PointType", SqlDbType.NVarChar, 0);
                    SqlParameter m_pointID_NotAudit = new SqlParameter("@Pointlist", SqlDbType.NVarChar, 0);
                    g_DBBiz_NotAudit.ClearParameters();
                    g_DBBiz_NotAudit.SetProcedureParameters(m_beginTime_NotAudit, beginTime);
                    g_DBBiz_NotAudit.SetProcedureParameters(m_endTime_NotAudit, endTime);
                    g_DBBiz_NotAudit.SetProcedureParameters(m_pointID_NotAudit, string.Join(",", PointID));
                    g_DBBiz_NotAudit.SetProcedureParameters(m_pointType_NotAudit, PointType);
                    g_DBBiz_NotAudit.ExecuteProcNonQuery("UP_AuditStatusForHour_Water", connection); //地表水
                }

            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新审核预处理小时状态表(新)
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="PointType"></param>
        /// <returns></returns>
        public bool UpdataAuditHourStatusNew(string applicationUID, string[] PointID, DateTime beginTime, DateTime endTime, string PointType)
        {
            try
            {
                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                {
                    SqlParameter m_beginTime = new SqlParameter("@m_begin", SqlDbType.DateTime, 0);
                    SqlParameter m_endTime = new SqlParameter("@m_end", SqlDbType.DateTime, 0);
                    SqlParameter m_pointType = new SqlParameter("@PointType", SqlDbType.NVarChar, 0);
                    SqlParameter m_pointID = new SqlParameter("@Pointlist", SqlDbType.NVarChar, 0);
                    g_DBBiz.ClearParameters();
                    g_DBBiz.SetProcedureParameters(m_pointID, string.Join(",", PointID));
                    g_DBBiz.SetProcedureParameters(m_beginTime, beginTime);
                    g_DBBiz.SetProcedureParameters(m_endTime, endTime);
                    g_DBBiz.SetProcedureParameters(m_pointType, PointType);
                    g_DBBiz.ExecuteProcNonQuery("UP_AuditStatusForHour_Air", connection);//空气
                }
                else
                {
                    string begintime = beginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    string endtime = endTime.ToString("yyyy-MM-dd HH:mm:ss");
                    SqlParameter m_beginTime = new SqlParameter("@m_begin", SqlDbType.DateTime, 0);
                    SqlParameter m_endTime = new SqlParameter("@m_end", SqlDbType.DateTime, 0);
                    SqlParameter m_pointID = new SqlParameter("@Pointlist", SqlDbType.NVarChar, 0);
                    g_DBBiz.ClearParameters();
                    g_DBBiz.SetProcedureParameters(m_beginTime, begintime);
                    g_DBBiz.SetProcedureParameters(m_endTime, endtime);
                    g_DBBiz.SetProcedureParameters(m_pointID, string.Join(",", PointID));
                    g_DBBiz.ExecuteProcNonQuery("UP_AuditStatusForHour_Water_NEW", connection); //地表水
                }

            }
            catch
            {
                return false;
            }
            return true;
        }



        /// <summary>
        /// 加载预处理数据(超级站)
        /// </summary>
        /// <param name="applicationUID">应用程序UID</param>
        /// <param name="PointID">点位ID</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="IsAudit">是否审核过（审核过从审核小时数据导入，未审核从原始数据导入）</param>
        /// <returns></returns>
        public bool PreData_Load_Super(string[] PointID, DateTime beginTime, DateTime endTime, bool IsAudit)
        {
            try
            {
                SqlParameter m_pointID = new SqlParameter("@PointList", SqlDbType.NVarChar, 0);
                SqlParameter m_beginTime = new SqlParameter("@DTBegin", SqlDbType.NVarChar, 0);
                SqlParameter m_endTime = new SqlParameter("@DTEnd", SqlDbType.NVarChar, 0);
                g_DBBiz.ClearParameters();
                g_DBBiz.SetProcedureParameters(m_pointID, string.Join(",", PointID));
                g_DBBiz.SetProcedureParameters(m_beginTime, beginTime);
                g_DBBiz.SetProcedureParameters(m_endTime, endTime);
                g_DBBiz.ExecuteProcNonQuery("P_Load_AuditPreData_NotAudit_Super", connection);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 对预处理数据着色，根据规则对其数据位进行标记
        /// </summary>
        /// <param name="applicationUID">应用程序UID</param>
        /// <param name="PointID">点位</param>
        /// <param name="factorCode">因子，为空时计算所有因子</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="auditHour">从昨天开始小时数,如:为0 昨天00:00:00点到昨天23:59:59点,为24 今天00:00:00点到今天23:59:59点 </param>
        /// <returns></returns>
        public bool PreData_SetColor(string applicationUID, string[] PointID, string factorCode, DateTime beginTime, DateTime endTime, int auditHour)
        {
            try
            {
                SqlParameter m_factor = new SqlParameter("@AssignFactor", SqlDbType.NVarChar, 0);
                SqlParameter m_pointID = new SqlParameter("@PointList", SqlDbType.NVarChar, 0);
                SqlParameter m_beginTime = new SqlParameter("@DTBegin", SqlDbType.DateTime, 0);
                SqlParameter m_endTime = new SqlParameter("@DTEnd", SqlDbType.DateTime, 0);
                SqlParameter m_auditHour = new SqlParameter("@AuditBeginHour", SqlDbType.Int, 0);
                SqlParameter m_applicationUID = new SqlParameter("@ApplicationID", SqlDbType.NVarChar, 0);
                g_DBBiz.ClearParameters();
                g_DBBiz.SetProcedureParameters(m_factor, factorCode);
                g_DBBiz.SetProcedureParameters(m_pointID, string.Join(",", PointID));
                g_DBBiz.SetProcedureParameters(m_beginTime, beginTime);
                g_DBBiz.SetProcedureParameters(m_endTime, endTime);
                g_DBBiz.SetProcedureParameters(m_auditHour, auditHour);
                g_DBBiz.SetProcedureParameters(m_applicationUID, applicationUID);
                g_DBBiz.ExecuteProcNonQuery("UP_Audit_UseColorRule", connection);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 对预处理数据着色，根据规则对其数据位进行标记 超级站)
        /// </summary>
        /// <param name="applicationUID">应用程序UID</param>
        /// <param name="PointID">点位</param>
        /// <param name="factorCode">因子，为空时计算所有因子</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="auditHour">从昨天开始小时数,如:为0 昨天00:00:00点到昨天23:59:59点,为24 今天00:00:00点到今天23:59:59点 </param>
        /// <returns></returns>
        public bool PreData_SetColor_Super(string[] PointID, string factorCode, DateTime beginTime, DateTime endTime, int auditHour)
        {
            try
            {
                SqlParameter m_factor = new SqlParameter("@AssignFactor", SqlDbType.NVarChar, 0);
                SqlParameter m_pointID = new SqlParameter("@PointList", SqlDbType.NVarChar, 0);
                SqlParameter m_beginTime = new SqlParameter("@DTBegin", SqlDbType.DateTime, 0);
                SqlParameter m_endTime = new SqlParameter("@DTEnd", SqlDbType.DateTime, 0);
                SqlParameter m_auditHour = new SqlParameter("@AuditBeginHour", SqlDbType.Int, 0);
                g_DBBiz.ClearParameters();
                g_DBBiz.SetProcedureParameters(m_factor, factorCode);
                g_DBBiz.SetProcedureParameters(m_pointID, string.Join(",", PointID));
                g_DBBiz.SetProcedureParameters(m_beginTime, beginTime);
                g_DBBiz.SetProcedureParameters(m_endTime, endTime);
                g_DBBiz.SetProcedureParameters(m_auditHour, auditHour);
                g_DBBiz.ExecuteProcNonQuery("UP_Audit_UseColorRule_Super", connection);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}
