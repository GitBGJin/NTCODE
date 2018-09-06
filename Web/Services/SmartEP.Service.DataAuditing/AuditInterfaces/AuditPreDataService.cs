using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Common;

namespace SmartEP.Service.DataAuditing.AuditInterfaces
{
    public class AuditPreDataService
    {
        #region 属性
        AuditLogService logService = new AuditLogService();
        AuditPreDatarRepository preDataRep = new AuditPreDatarRepository();
        #endregion

        #region 方法
        /// <summary>
        /// 加载预处理数据
        /// </summary>
        /// <param name="applicationUID">应用程序UID</param>
        /// <param name="PointID">点位ID</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public bool PreData_Load(string applicationUID, string[] PointID, DateTime beginTime, DateTime endTime)
        {
            bool result = false;
            try
            {
                foreach (string portid in PointID)
                {
                    while (beginTime <= endTime)
                    {
                        AuditStatusForDayEntity status = logService.RerieveAuditState(Convert.ToInt32(portid), beginTime, beginTime, applicationUID);
                        //判断是否审核过（审核过从审核小时数据导入，未审核从原始数据导入）
                        result = preDataRep.PreData_Load(applicationUID, portid.Split(','), beginTime, beginTime.AddDays(1).Date.AddSeconds(-1), (status == null || status.Status.Equals("") || status.Status.Equals("0")) ? false : true);
                        beginTime = beginTime.AddDays(1);
                    }
                }
            }
            catch
            {
                return false;
            }
            return result;
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
            return preDataRep.UpdataAuditHourStatus(applicationUID, PointID, beginTime, beginTime.AddDays(1).Date.AddHours(-1), PointType);

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
            return preDataRep.UpdataAuditHourStatusNew(applicationUID, PointID, beginTime, beginTime.AddDays(1).Date.AddSeconds(-1), PointType);

        }


        /// <summary>
        /// 加载预处理数据(超级站)
        /// </summary>
        /// <param name="applicationUID">应用程序UID</param>
        /// <param name="PointID">点位ID</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public bool PreData_Load_Super(string[] PointID, DateTime beginTime, DateTime endTime)
        {
            bool result = false;
            try
            {
                result = preDataRep.PreData_Load_Super(PointID, beginTime, endTime, false);
            }
            catch
            {
                return false;
            }
            return result;
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
            return preDataRep.PreData_SetColor(applicationUID, PointID, factorCode, beginTime, endTime, auditHour);
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
            return preDataRep.PreData_SetColor_Super(PointID, factorCode, beginTime, endTime, auditHour);
        }
        #endregion
    }
}
