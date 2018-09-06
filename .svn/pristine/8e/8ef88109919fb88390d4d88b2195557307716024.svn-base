using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Data.SqlServer.MonitoringBusiness;

namespace SmartEP.MonitoringBusinessRepository.Common
{
    /// <summary>
    /// 名称：AuditPreDatarRepository.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-08-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 空气审核预处理数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    /// 
    public class AuditPreDatarRepository
    {
        #region 属性
        AuditPreDataDAL g_PreData = new AuditPreDataDAL();
        #endregion

        #region 方法
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
            return g_PreData.PreData_Load(applicationUID, PointID, beginTime, endTime, IsAudit);
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
            return g_PreData.UpdataAuditHourStatus(applicationUID, PointID, beginTime, endTime, PointType);
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
            return g_PreData.UpdataAuditHourStatusNew(applicationUID, PointID, beginTime, endTime, PointType);
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
            return g_PreData.PreData_Load_Super(PointID, beginTime, endTime, IsAudit);
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
            return g_PreData.PreData_SetColor(applicationUID, PointID, factorCode, beginTime, endTime, auditHour);
        }

        /// <summary>
        /// 对预处理数据着色，根据规则对其数据位进行标记(超级站)
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
            return g_PreData.PreData_SetColor_Super(PointID, factorCode, beginTime, endTime, auditHour);
        }
        #endregion
    }
}
