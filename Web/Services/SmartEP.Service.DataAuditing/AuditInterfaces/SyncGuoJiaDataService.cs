using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.MonitoringBusinessRepository.Common;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Data.Enums;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.AdoData;
using SmartEP.Core.Generic;

namespace SmartEP.Service.DataAuditing.AuditInterfaces
{
    public class SyncGuoJiaDataService
    {
        /// <summary>
        /// 审核状态(已审核)
        /// </summary>
        private const string STATUS_AUDITED = "1";

        private string AMS_MonitorBusiness_Conn = EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);

        private const string GuoJia_Air_Conn = "GuoJia_Air_Conn";

        private const string SysType = "GuoJia";

        private SyncGuoJiaDataRepository syncRep = new SyncGuoJiaDataRepository();
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper dbHelper = Singleton<DatabaseHelper>.GetInstance();
        #region << 审核小时数据 >>
        /// <summary>
        /// 按点位和时间段同步国家审核小时数据
        /// </summary>
        /// <param name="portIds">点位ID(多个点位ID以英文,分割)</param>
        /// <param name="factorCodes">因子Code列表(多个点位ID以英文,分割、为空表示所有因子)</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="isCoverData">是否覆盖数据</param>
        public void SyncAuditHourData(string portIds, string factorCodes,
                                      DateTime dtStart, DateTime dtEnd,
                                      bool isCoverData)
        {
            SyncTransferMappingDAL mappingBiz = new SyncTransferMappingDAL();
            DataTable portInfo = mappingBiz.GetPortMapping(SysType, portIds);

            string guoJiaFactors = string.Empty;

            if (!string.IsNullOrEmpty(factorCodes))
            {
                DataTable guoJiaFactorDt = mappingBiz.GetSourceChannels(SysType, factorCodes);
                // 国家同步因子列表
                guoJiaFactors = string.Join("','", guoJiaFactorDt.Select("1=1").Select(x => x["SourceChannel"].ToString()).ToArray());
            }

            foreach (DataRow drPort in portInfo.Rows)
            {
                //本地点位ID
                string LocalPort = drPort["LocalPort"].ToString();
                //国家点位ID
                string SourcePort = drPort["SourcePort"].ToString();

                if (dtStart.Year == dtEnd.Year)
                {
                    SyncHourData(LocalPort, SourcePort, dtStart, dtEnd, isCoverData, guoJiaFactors, factorCodes);
                }
                else
                {
                    // 第二年最新时间
                    DateTime dtStart_NewYear = Convert.ToDateTime(dtEnd.ToString("yyyy-01-01 00:00:00"));

                    // 当前年最后时间
                    DateTime dtEnd_CurYear = Convert.ToDateTime(dtStart.ToString("yyyy-12-31 23:59:59"));

                    SyncHourData(LocalPort, SourcePort, dtStart, dtEnd_CurYear, isCoverData, guoJiaFactors, factorCodes);
                    SyncHourData(LocalPort, SourcePort, dtStart_NewYear, dtEnd, isCoverData, guoJiaFactors, factorCodes);
                }
            }

            if (isCoverData)
            {
                AutoCreateAQIData(portIds.Split(','), dtStart, dtEnd);
            }
        }

        /// <summary>
        ///更新审核数据
        /// </summary>
        /// <param name="dt">数据源</param>
        public void UpdateAuditData(string portIds, DataTable dt, DateTime dtStart, DateTime dtEnd, DateTime dtMark)
        {
            syncRep.UpdateAuditData(dt);
            if (dtStart == dtMark && dtEnd == dtMark)
            {
            }
            else
            {
                AutoCreateAQIData(portIds.Split(','), Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd 00:00:00")), Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd 23:59:59")));
            }
        }
        /// <summary>
        /// 获取国家审核日志
        /// </summary>
        /// <param name="LocalPortId">点位ID(多个点位ID以英文,分割)</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataTable GuojiaAuditAirLog(string LocalPortIds, string pollutantCodes, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                SyncTransferMappingDAL mappingBiz = new SyncTransferMappingDAL();
                DataTable portInfo = mappingBiz.GetPortMapping(SysType, LocalPortIds);
                string SourcePort = "";
                foreach (DataRow drPort in portInfo.Rows)
                {
                    SourcePort += drPort["SourcePort"].ToString() + "','";
                }
                SourcePort = "'" + SourcePort.TrimEnd('\'').TrimEnd(',');
                string appUid = SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Core.Enums.ApplicationType.Air);

                string conn_db_str = GuoJia_Air_Conn;
                // 审核日志表完整表名
                string verifyLogFullTblName = "[dbo].[DataVerifyLogCity]";
                // 审核日志数据
                DataTable dt = syncRep.GuojiaAuditAirLog(SourcePort, dtStart, dtEnd, verifyLogFullTblName, conn_db_str);
                DataTable localdt = syncRep.LocalAuditAirLog(dtStart, dtEnd, pollutantCodes);
                foreach (DataRow dr in dt.Rows)
                {
                    if (!pollutantCodes.Contains(dr["PollutantCode"].ToString()))
                    {
                        dr.Delete();
                    }
                    else
                    {
                        string LocalPortId = dr["PointId"].ToString();
                        // 当天数据是否需要同步状态标识（0：同步、1：不同步）
                        DateTime dtStatus = DateTime.Parse(DateTime.Parse(dr["TimePoint"].ToString()).ToString("yyyy-MM-dd 00:00:00"));
                        string status = string.Empty;
                        string auditStatusUid = syncRep.GetAuditStatusUid(appUid, LocalPortId, dtStatus, out status);

                        if (string.IsNullOrEmpty(auditStatusUid))
                        {// 日概况数据表中不存在数据
                            auditStatusUid = Guid.NewGuid().ToString();
                            // 添加数据
                            syncRep.AddAuditStatusForDayData(auditStatusUid, appUid, LocalPortId, dtStatus);
                        }

                        dr["AuditStatusUid"] = auditStatusUid;
                        DataRow[] drArry = localdt.Select("AuditStatusUid='" + auditStatusUid + "' and tstamp='" + dr["TimePoint"].ToString() + "' and AuditTime='"
                              + dr["VerifyTime"].ToString() + "' and  PollutantCode='" + dr["PollutantCode"].ToString() + "'");
                        if (drArry.Length > 0)
                        {
                            dr.Delete();
                        }
                    }
                }
                dt.AcceptChanges();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新本地日志信息
        /// </summary>
        /// <param name="dtGuoJia"></param>
        public void copyAuditLog(DataTable dtGuoJia)
        {
            syncRep.copyAuditLog(dtGuoJia);
        }
        /// <summary>
        /// 执行小时数据同步
        /// </summary>
        /// <param name="LocalPortId">本地点位ID</param>
        /// <param name="SourcePort">国家库点位ID</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="isCoverData">是否覆盖数据</param>
        /// <param name="guoJiaFactors">国家因子列表(为空表示所有因子)</param>
        /// <param name="localFactors">本地因子列表(为空表示所有因子)</param>
        private void SyncHourData(string LocalPortId, string SourcePort,
                                  DateTime dtStart, DateTime dtEnd, bool isCoverData,
                                  string guoJiaFactors, string localFactors)
        {
            try
            {
                string linkServerName = System.Configuration.ConfigurationManager.AppSettings["airLinkServiceName"].ToString();
                string linkDBName = System.Configuration.ConfigurationManager.AppSettings["airLinkDBName"].ToString();
                string IsUseLinkServer = System.Configuration.ConfigurationManager.AppSettings["IsUseLinkService"].ToString();

                string tableName = string.Format("Air_h_{0}_{1}_App", dtStart.Year, SourcePort);

                string fullTableNameByLink = string.Format("[{0}].[{1}].[dbo].[{2}]",
                                                           linkServerName,
                                                           linkDBName,
                                                           tableName);
                string fullTableNameByDb = string.Format("[dbo].[{0}]", tableName);

                // 审核小时表完整表名
                string hourFullTableName = fullTableNameByDb;
                string conn_db_str = GuoJia_Air_Conn;

                // 审核日志表完整表名
                string verifyLogFullTblName = "[dbo].[DataVerifyLogCity]";

                //是否启用Link链接（0: 不启用，1：启用）
                if ("1".Equals(IsUseLinkServer))
                {// 启用Link链接
                    hourFullTableName = fullTableNameByLink;
                    conn_db_str = AMS_MonitorBusiness_Conn;

                    verifyLogFullTblName = string.Format("[{0}].[{1}].[{2}]",
                                                           linkServerName,
                                                           linkDBName,
                                                           verifyLogFullTblName);
                }

                string appUid = SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Core.Enums.ApplicationType.Air);
                // 当天数据是否需要同步状态标识（0：同步、1：不同步）
                string status = string.Empty;

                while (dtStart <= dtEnd)
                {
                    string auditStatusUid = syncRep.GetAuditStatusUid(appUid, LocalPortId, dtStart, out status);

                    if (string.IsNullOrEmpty(auditStatusUid))
                    {// 日概况数据表中不存在数据
                        auditStatusUid = Guid.NewGuid().ToString();
                        // 添加数据
                        syncRep.AddAuditStatusForDayData(auditStatusUid, appUid, LocalPortId, dtStart);
                    }

                    //同步审核小时数据
                    syncRep.SyncAuditAirInfectantByHourData(auditStatusUid, appUid, LocalPortId, dtStart, hourFullTableName, conn_db_str, guoJiaFactors, localFactors, false);

                    if ("12-31".Equals(dtStart.ToString("MM-dd")))
                    {//同步每年最后1小时数据，只需同步新年表中最新1小时数据便可
                        syncRep.SyncAuditAirInfectantByHourData(auditStatusUid, appUid, LocalPortId, dtStart, hourFullTableName, conn_db_str, guoJiaFactors, localFactors, true);
                    }

                    // 同步审核日志数据
                    syncRep.SyncAuditAirLogData(auditStatusUid, SourcePort, dtStart, verifyLogFullTblName, conn_db_str);

                    if (!isCoverData && !STATUS_AUDITED.Equals(status))
                    {// 不勾选覆盖并且未同步过数据时，需要按单个测点生成AQI数据
                        AutoCreateAQIData(new string[] { LocalPortId }, Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd 00:00:00")), Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd 23:59:59")));
                    }

                    dtStart = dtStart.AddDays(1);
                }
            }
            catch (Exception e)
            {
                //CommonFunction.WriteErrorLog("End Sync ([Audit].[TB_AuditAirInfectantByHour]) Data Error: " + e.Message);
            }
        }

        /// <summary>
        /// 获取因子的名称信息
        /// </summary>
        /// <returns>LocalChannel：本地因子Code、PollutantName：因子名称、SourceChannel：国家库因子</returns>
        public DataTable GetChannelNameInfo()
        {
            return new SyncTransferMappingDAL().GetChannelNameInfo(SysType);
        }

        /// <summary>
        /// 自动生成AQI数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        private void AutoCreateAQIData(string[] portIds, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = "PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "PointId IN(" + portIdsStr + ")";
                }
                string localCodeWhere = "PollutantCode IN('a34002','a34004','a21026','a21004','a21005','a05024')";
                String queryDataSql = string.Format(@"
                                               select * from [AirReport].[TB_HourReport] 
                                                where {0} 
                                                  and Tstamp >= '{1}' 
                                                  and Tstamp <  '{2}'
                                                  and {3}"
                                         , portIdsStr
                                         , dtStart.ToString("yyyy-MM-dd 00:00:00.000")
                                         , dtStart.AddDays(1).ToString("yyyy-MM-dd 00:00:00.000")
                                         , localCodeWhere);
                //先删除表中数据避免重复插入
                DataTable dtHour = dbHelper.ExecuteDataTable(queryDataSql, AMS_MonitorBusiness_Conn);
                if (dtHour.Rows.Count > 0)
                {
                    string errMsg = string.Empty;
                    AuditDataRepository auditRep = new AuditDataRepository();
                    auditRep.GenerateData(SmartEP.Core.Enums.ApplicationType.Air, portIds, dtStart, dtEnd, out errMsg);

                    MonitoringPointAirService pointService = new MonitoringPointAirService();
                    auditRep.GenerateDataRegion(SmartEP.Core.Enums.ApplicationType.Air, pointService.GetRegionByPort(portIds), dtStart, dtEnd, out errMsg);
                    auditRep.GenerateDataCity(SmartEP.Core.Enums.ApplicationType.Air, pointService.GetCityByPort(portIds), dtStart, dtEnd, out errMsg);

                    AuditDataService auditDataService = new AuditDataService();
                    auditDataService.CreateExportFile(dtStart, dtEnd, out errMsg);
                }
                else
                {

                    string errMsg = string.Empty;
                    AuditDataRepository auditRep = new AuditDataRepository();
                    auditRep.GenerateDataDay(SmartEP.Core.Enums.ApplicationType.Air, portIds, dtStart, dtEnd, out errMsg);
                    MonitoringPointAirService pointService = new MonitoringPointAirService();
                    auditRep.GenerateDataRegion(SmartEP.Core.Enums.ApplicationType.Air, pointService.GetRegionByPort(portIds), dtStart, dtEnd, out errMsg);
                    auditRep.GenerateDataCity(SmartEP.Core.Enums.ApplicationType.Air, pointService.GetCityByPort(portIds), dtStart, dtEnd, out errMsg);

                    AuditDataService auditDataService = new AuditDataService();
                    auditDataService.CreateExportFile(dtStart, dtEnd, out errMsg);
                }
            }
            catch (Exception e)
            {
                //CommonFunction.WriteErrorLog("Using ENV Api Auto Create AQI Data Error: " + e.Message);
            }
        }
        #endregion

        #region << 同步AQI数据 >>
        /// <summary>
        /// 同步国家RTD_DayAQI数据（指定测点和时间）
        /// </summary>
        /// <param name="portIds">点位ID(多个点位ID以英文,分割)</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        public void SyncRTD_DayAQI(string portIds, DateTime dtStart, DateTime dtEnd)
        {
            syncRep.SyncRTD_DayAQI(SysType, portIds, dtStart, dtEnd);
        }

        /// <summary>
        /// 按时间区段同步国家RTD_CityDayAQI数据
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        public void SyncRTD_CityDayAQI(DateTime dtStart, DateTime dtEnd)
        {
            syncRep.SyncRTD_CityDayAQI(dtStart, dtEnd);
        }
        #endregion
    }
}
