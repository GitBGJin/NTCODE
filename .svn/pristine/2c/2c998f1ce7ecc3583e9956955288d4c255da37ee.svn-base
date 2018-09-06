using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using System.Data;

namespace SmartEP.MonitoringBusinessRepository.Air
{
    public class SyncGuoJiaDataRepository
    {
        private SyncGuoJiaDataDAL sync = new SyncGuoJiaDataDAL();

        /// <summary>
        /// 同步审核小时数据
        /// </summary>
        /// <param name="auditStatusUid">状态UID</param>
        /// <param name="appUid">应用类型UID</param>
        /// <param name="pointId">点位ID</param>
        /// <param name="date">数据时间</param>
        /// <param name="guoJiaTblName">国家库数据表完整表名</param>
        /// <param name="conn_db_str">数据库完整名</param>
        /// <param name="guoJiaFactors">国家因子列表(为空表示所有因子)</param>
        /// <param name="localFactors">本地因子列表(为空表示所有因子)</param>
        /// <param name="isPerYearLastHour">是否为同步每年最后一小时数据</param>
        public void SyncAuditAirInfectantByHourData(string auditStatusUid, string appUid,
                                                    string pointId, DateTime date,
                                                    string guoJiaTblName, string conn_db_str,
                                                    string guoJiaFactors, string localFactors,
                                                    bool isPerYearLastHour)
        {
            sync.SyncAuditAirInfectantByHourData(auditStatusUid, appUid, pointId, date, guoJiaTblName, conn_db_str, guoJiaFactors, localFactors, isPerYearLastHour);
        }

        /// <summary>
        /// 判断是否存在日概况数据
        /// </summary>
        /// <param name="appUid">应用类型UID</param>
        /// <param name="pointId">点位ID</param>
        /// <param name="date">数据时间</param>
        /// <param name="status">审核状态(0：未审核、1：已审核)</param>
        /// <returns></returns>
        public string GetAuditStatusUid(string appUid, string pointId, DateTime date, out string status)
        {
            return sync.GetAuditStatusUid(appUid, pointId, date, out status);
        }

        /// <summary>
        /// 添加日概况数据
        /// </summary>
        /// <param name="auditStatusUid">状态UID</param>
        /// <param name="appUid">应用类型UID</param>
        /// <param name="pointId">点位ID</param>
        /// <param name="date">数据时间</param>
        public void AddAuditStatusForDayData(string auditStatusUid, string appUid,
                                             string pointId, DateTime date)
        {
            sync.AddAuditStatusForDayData(auditStatusUid, appUid, pointId, date);
        }
        /// <summary>
        ///更新审核数据
        /// </summary>
        /// <param name="dt">数据源</param>
        public void UpdateAuditData(DataTable dt)
        {
            sync.UpdateAuditData(dt);
        }
        /// <summary>
        /// 更新日概况数据状态
        /// </summary>
        /// <param name="auditStatusUid">状态UID</param>
        /// <param name="appUid">应用类型UID</param>
        /// <param name="pointId">点位ID</param>
        /// <param name="date">数据时间</param>
        public void updateAuditStatusForDayDataStatus(string auditStatusUid)
        {
            sync.updateAuditStatusForDayDataStatus(auditStatusUid);
        }

        /// <summary>
        /// 同步审核日志数据
        /// </summary>
        /// <param name="auditStatusUid">状态UID</param>
        /// <param name="date">数据时间</param>
        /// <param name="SourcePort">国家点位ID</param>
        /// <param name="guoJiaTblName">国家库审核日志完整表名</param>
        /// <param name="conn_db_str">数据库完整名</param>
        public void SyncAuditAirLogData(string auditStatusUid, string SourcePort, DateTime date,
                                        string guoJiaTblName, string conn_db_str)
        {
            sync.SyncAuditAirLogData(auditStatusUid, SourcePort, date, guoJiaTblName, conn_db_str);
        }
        /// <summary>
        /// 获取本地国控点审核日志
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateEnd"></param>
        /// <param name="PollutantCode"></param>
        /// <returns></returns>
        public DataTable LocalAuditAirLog(DateTime date, DateTime dateEnd, string PollutantCode)
        {
            return sync.LocalAuditAirLog(date, dateEnd, PollutantCode);
        }
        /// <summary>
        /// 更新本地日志信息
        /// </summary>
        /// <param name="dtGuoJia"></param>
        public void copyAuditLog(DataTable dtGuoJia)
        {
            sync.copyAuditLog(dtGuoJia);
        }
        /// <summary>
        /// 获取审核超过时间超过一天的日志
        /// </summary>
        /// <param name="SourcePort">点位ID(多个点位ID以英文,分割)</param>
        /// <param name="date">数据时间</param>
        /// <param name="dateEnd">数据时间</param>
        /// <param name="guoJiaTblName">国家库审核日志完整表名</param>
        /// <param name="conn_db_str">数据库完整名</param>
        /// <returns></returns>
        public DataTable GuojiaAuditAirLog(string SourcePort, DateTime date, DateTime dateEnd,
                                       string guoJiaTblName, string conn_db_str)
        {
            return sync.GuojiaAuditAirLog(SourcePort, date, dateEnd, guoJiaTblName, conn_db_str);
        }
        /// <summary>
        /// 按点位ID和时间段同步日AQI数据
        /// </summary>
        /// <param name="SysType">系统类型</param>
        /// <param name="portIds">点位ID(多个点位ID以英文,分割)</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        public void SyncRTD_DayAQI(string SysType, string portIds, DateTime dtStart, DateTime dtEnd)
        {
            SyncTransferMappingDAL mappingBiz = new SyncTransferMappingDAL();
            DataTable portInfo = mappingBiz.GetPortMapping(SysType, portIds);

            foreach (DataRow drPort in portInfo.Rows)
            {
                //本地点位ID
                string LocalPort = drPort["LocalPort"].ToString();
                //国家点位ID
                string SourcePort = drPort["SourcePort"].ToString();

                sync.SyncOnePointRTD_DayAQI(LocalPort, SourcePort, dtStart, dtEnd);
            }
        }

        /// <summary>
        /// 按时间区段同步国家RTD_CityDayAQI数据
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        public void SyncRTD_CityDayAQI(DateTime dtStart, DateTime dtEnd)
        {
            sync.SyncRTD_CityDayAQI(dtStart, dtEnd);
        }
    }
}
