﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.MonitoringBusinessRepository.Water;
using SmartEP.MonitoringBusinessRepository.Common;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Core.Enums;
using SmartEP.Service.DataAuditing.AuditBaseInfo;
using SmartEP.Service.Frame;

namespace SmartEP.Service.DataAuditing.AuditInterfaces
{
    public class AuditLogService
    {
        #region 变量
        UserService userservice = new UserService();
        #region 空气
        AuditStatusForDayRepository auditStateRep = new AuditStatusForDayRepository();
        AuditAirLogRepository logAirRep = new AuditAirLogRepository();
        AirAuditMonitoringPointService pointAirService = new AirAuditMonitoringPointService();
        AuditAirInfectantByHourRepository auditAirHourRep = new AuditAirInfectantByHourRepository();
        #endregion

        #region 地表水
        AuditWaterLogRepository logWaterRep = new AuditWaterLogRepository();
        WaterAuditMonitoringPointService pointWaterService = new WaterAuditMonitoringPointService();
        #endregion
        #endregion

        #region 审核状态
        /// <summary>
        /// 获取审核状态列表
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<AuditStatusForDayEntity> RerieveAuditStateList(int portId, DateTime startTime, DateTime endTime)
        {
            return auditStateRep.Retrieve(x => x.PointId == portId && x.Date >= startTime && x.Date <= endTime);
        }

        /// <summary>
        /// 获取点位一天的审核状态(单点：已审核、未审核)
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="applicationUID"></param>
        /// <param name="pointType"></param>
        /// <returns></returns>
        public AuditStatusForDayEntity RerieveAuditState(System.Int32 portId, DateTime beginTime, DateTime endTime, string applicationUID)
        {
            return auditStateRep.Retrieve(x => x.PointId == portId && x.Date.Value.Date >= beginTime && x.Date.Value.Date <= endTime && x.ApplicationUid == applicationUID).OrderByDescending(x => x.Status).FirstOrDefault();
        }

        /// <summary>
        /// 获取点位一天的审核状态(多点：已审核、未审核、部分审核,区分仪器)
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="applicationUID"></param>
        /// <param name="pointType">点位类型:0:非超级站；1：超级站</param>
        /// <returns></returns>
        public string RerieveAuditState(string[] portId, DateTime beginTime, DateTime endTime, string applicationUID, string pointType, string[] factors)
        {
            string status = "";
            List<int?> IntPort = new List<int?>();
            foreach (string item in portId)
            {
                if (item != "")
                {
                    IntPort.Add(int.Parse(item));
                }
            }
            try
            {
                if (pointType.Equals("1"))//超级站用SuperStatus字段
                {
                    AuditStatusForDayEntity auditstatus = auditStateRep.Retrieve(x => x.PointId == 204 && x.Date == beginTime).FirstOrDefault();
                    if (auditstatus != null)
                    {
                        int auditCount = auditAirHourRep.GetAuditRecordNumByHourSuper(auditstatus.AuditStatusUid, factors);
                        if (auditCount > 0)
                        {
                            if (auditCount < 24 * factors.Length) status = "2";
                            else status = "1";
                        }
                        else
                        {
                            status = "0";
                        }
                    }
                }
            }
            catch
            {

            }
            return status;
        }

        /// <summary>
        /// 获取点位一天的审核状态(多点：已审核、未审核、部分审核,区分仪器)新
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="applicationUID"></param>
        /// <param name="pointType">点位类型:0:非超级站；1：超级站</param>
        /// <returns></returns>
        public string RerieveAuditStateNew(string[] portId, DateTime beginTime, DateTime endTime, string applicationUID, string pointType, string[] factors, string InstrumentUid)
        {
            string status = "";
            List<int?> IntPort = new List<int?>();
            foreach (string item in portId)
            {
                if (item != "")
                {
                    IntPort.Add(int.Parse(item));
                }
            }
            try
            {
                if (pointType.Equals("1"))//超级站用SuperStatus字段
                {
                    IQueryable<AuditStatusForDayEntity> statusList = (from a in auditStateRep.Retrieve(x => IntPort.Contains(x.PointId) && x.Date.Value.Date >= beginTime && x.Date.Value.Date <= endTime && x.ApplicationUid == applicationUID && x.DataException == InstrumentUid)
                                                                      group a by new { a.PointId, a.Date } into g
                                                                      select new AuditStatusForDayEntity { PointId = g.Max(x => x.PointId), Date = g.Max(x => x.Date), Status = g.Max(x => x.Status) }
                                                      ).Distinct();
                    if (statusList.Count() > 0)
                    {
                        if (statusList.Where(x => x.Status == null).Count() == 0 && statusList.Where(x => x.Status.Equals("1")).Count() == portId.Length * ((endTime.Date - beginTime.Date).Days + 1))
                        {
                            status = "1";//已审核
                        }
                        //else if (!statusList.Select(x => x.Status).ToArray().Contains("1"))
                        else if (statusList.Where(x => x.Status == null).Count() == 0 && statusList.Where(x => x.Status.Equals("0")).Count() == portId.Length * ((endTime.Date - beginTime.Date).Days + 1))
                        {
                            status = "0";//未审核
                        }
                        else
                        {
                            string[] statusArray = statusList.Select(x => x.Status).ToArray();
                            if (statusArray.Contains("2") || statusArray.Contains("1") || statusArray.Contains("1") && (statusArray.Contains("0") || statusArray.Contains("") || statusArray.Contains(null)))
                                status = "2";//部分审核
                        }
                    }
                }
            }
            catch
            {

            }
            return status;
        }

        /// <summary>
        /// 获取点位一天的审核状态(多点：已审核、未审核、部分审核)
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="applicationUID"></param>
        /// <param name="pointType">点位类型:0:非超级站；1：超级站</param>
        /// <returns></returns>
        public string RerieveAuditState(string[] portId, DateTime beginTime, DateTime endTime, string applicationUID, string pointType)
        {
            string status = "";
            List<int?> IntPort = new List<int?>();
            foreach (string item in portId)
            {
                if (item != "")
                {
                    IntPort.Add(int.Parse(item));
                }
            }
            try
            {
                //IQueryable<AuditStatusForDayEntity> statusList = (from a in auditStateRep.Retrieve(x => portId.Contains(x.PointId.ToString()) && x.Date.Value.Date >= beginTime && x.Date.Value.Date <= endTime && x.ApplicationUid == applicationUID)
                //                                                  select new AuditStatusForDayEntity { PointId = a.PointId, Date = a.Date, Status = a.Status }).Distinct();

                if (pointType.Equals("1"))//超级站用SuperStatus字段
                {
                    IQueryable<AuditStatusForDayEntity> statusList = (from a in auditStateRep.Retrieve(x => IntPort.Contains(x.PointId) && x.Date.Value.Date >= beginTime && x.Date.Value.Date <= endTime && x.ApplicationUid == applicationUID)
                                                                      group a by new { a.PointId, a.Date } into g
                                                                      select new AuditStatusForDayEntity { PointId = g.Max(x => x.PointId), Date = g.Max(x => x.Date), SuperStatus = g.Max(x => x.SuperStatus) }
                                                                     ).Distinct();
                    if (statusList.Count() > 0)
                    {
                        if (statusList.Where(x => x.SuperStatus == null).Count() == 0 && statusList.Where(x => x.SuperStatus.Equals("1")).Count() == portId.Length * ((endTime.Date - beginTime.Date).Days + 1))
                        {
                            status = "1";//已审核
                        }
                        else if (statusList.Where(x => x.SuperStatus == null).Count() == 0 && statusList.Where(x => x.SuperStatus.Equals("0")).Count() == portId.Length * ((endTime.Date - beginTime.Date).Days + 1))
                        {
                            status = "0";//未审核
                        }
                        else
                        {
                            string[] statusArray = statusList.Select(x => x.SuperStatus).ToArray();
                            if (statusArray.Contains("2") || statusArray.Contains("1") || statusArray.Contains("1") && (statusArray.Contains("0") || statusArray.Contains("") || statusArray.Contains(null)))
                                status = "2";//部分审核
                        }
                    }
                }
                else   //非超级站用Status字段
                {
                    IQueryable<AuditStatusForDayEntity> statusList = (from a in auditStateRep.Retrieve(x => IntPort.Contains(x.PointId) && x.Date.Value.Date >= beginTime && x.Date.Value.Date <= endTime && x.ApplicationUid == applicationUID)
                                                                      group a by new { a.PointId, a.Date } into g
                                                                      select new AuditStatusForDayEntity { PointId = g.Max(x => x.PointId), Date = g.Max(x => x.Date), Status = g.Max(x => x.Status) }
                                                      ).Distinct();
                    if (statusList.Count() > 0)
                    {
                        if (statusList.Where(x => x.Status == null).Count() == 0 && statusList.Where(x => x.Status.Equals("1")).Count() == portId.Length * ((endTime.Date - beginTime.Date).Days + 1))
                        {
                            status = "1";//已审核
                        }
                        //else if (!statusList.Select(x => x.Status).ToArray().Contains("1"))
                        else if (statusList.Where(x => x.Status == null).Count() == 0 && statusList.Where(x => x.Status.Equals("0")).Count() == portId.Length * ((endTime.Date - beginTime.Date).Days + 1))
                        {
                            status = "0";//未审核
                        }
                        else
                        {
                            string[] statusArray = statusList.Select(x => x.Status).ToArray();
                            if (statusArray.Contains("2") || statusArray.Contains("1") || statusArray.Contains("1") && (statusArray.Contains("0") || statusArray.Contains("") || statusArray.Contains(null)))
                                status = "2";//部分审核
                        }
                    }
                }
            }
            catch
            {
            }
            return status;
        }

        /// <summary>
        /// 根据审核状态Guid获取点位id
        /// </summary>
        /// <param name="AuditStatusUid"></param>
        /// <param name="applicationUID"></param>
        /// <returns></returns>
        public int GetPointID(string AuditStatusUid, string applicationUID)
        {
            AuditStatusForDayEntity status = auditStateRep.Retrieve(x => x.AuditStatusUid.Equals(AuditStatusUid) && x.ApplicationUid.Equals(applicationUID)).FirstOrDefault();
            return status.PointId.Value;

        }
        #endregion

        #region 审核日志
        /// <summary>
        /// 根据点位、时间段获取审核日志（空气）(单站)
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<AuditAirLogEntity> RerieveAirLog(int portId, string applicationUid, DateTime startTime, DateTime endTime)
        {
            IQueryable<AuditStatusForDayEntity> statusList = auditStateRep.Retrieve(x => x.PointId == portId && x.Date.Value.Date >= startTime.Date && x.Date.Value.Date <= endTime.Date && x.ApplicationUid == applicationUid);
            IQueryable<AuditAirLogEntity> logList = from sta in statusList
                                                    join log in logAirRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime)
                                                        on sta.AuditStatusUid.ToUpper() equals log.AuditStatusUid.ToUpper()
                                                    select log;
            return logList.OrderByDescending(x => x.Tstamp);
        }

        /// <summary>
        /// 根据点位、时间段获取审核日志（地表水）(单站)
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<AuditWaterLogEntity> RerieveWaterLog(int portId, string applicationUid, DateTime startTime, DateTime endTime)
        {
            IQueryable<AuditStatusForDayEntity> statusList = auditStateRep.Retrieve(x => x.PointId == portId && x.Date.Value.Date >= startTime.Date && x.Date.Value.Date <= endTime.Date && x.ApplicationUid == applicationUid);
            IQueryable<AuditWaterLogEntity> logList = from sta in statusList
                                                      join log in logWaterRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime)
                                                        on sta.AuditStatusUid.ToUpper() equals log.AuditStatusUid.ToUpper()
                                                      select log;
            return logList.OrderByDescending(x => x.Tstamp); ;
        }

        /// <summary>
        /// 根据点位、时间段获取审核日志（空气）(多站)
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<AuditAirLogEntity> RerieveAirLog(string[] portId, string applicationUid, DateTime startTime, DateTime endTime)
        {
            IQueryable<AuditStatusForDayEntity> statusList = auditStateRep.Retrieve(x => portId.Contains(x.PointId.ToString()) && x.Date.Value.Date >= startTime.Date && x.Date.Value.Date <= endTime.Date && x.ApplicationUid == applicationUid);
            IQueryable<AuditAirLogEntity> logList = logAirRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime && x.PollutantCode != null && !x.PollutantCode.Equals(""));
            IQueryable<AuditAirLogEntity> logs = from sta in statusList
                                                 join log in logList
                                                        on sta.AuditStatusUid.ToUpper() equals log.AuditStatusUid.ToUpper()
                                                 select log;
            return logs.OrderByDescending(x => x.Tstamp);
        }

        /// <summary>
        /// 根据点位、时间段获取审核日志（地表水）(多站)
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<AuditWaterLogEntity> RerieveWaterLog(string[] portId, string applicationUid, DateTime startTime, DateTime endTime)
        {
            List<int?> IntPort = new List<int?>();
            foreach (string item in portId)
            {
                if (item != "")
                {
                    IntPort.Add(int.Parse(item));
                }
            }
            IQueryable<AuditStatusForDayEntity> statusList = auditStateRep.Retrieve(x => IntPort.Contains(x.PointId) && x.Date.Value.Date >= startTime.Date && x.Date.Value.Date <= endTime.Date && x.ApplicationUid == applicationUid);
            IQueryable<AuditWaterLogEntity> logList = from sta in statusList
                                                      join log in logWaterRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime && x.PollutantCode != null && !x.PollutantCode.Equals(""))
                                                        on sta.AuditStatusUid.ToUpper() equals log.AuditStatusUid.ToUpper()
                                                      select log;
            return logList.OrderByDescending(x => x.Tstamp); ;
        }

        /// <summary>
        /// 根据点位、因子、时间段获取审核日志（空气）
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="applicationUid"></param>
        /// <param name="factorCode"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<AuditAirLogEntity> RerieveAirLog(int portId, string applicationUid, string factorCode, DateTime startTime, DateTime endTime)
        {
            IQueryable<AuditStatusForDayEntity> statusList = auditStateRep.Retrieve(x => x.PointId == portId && x.Date.Value.Date >= startTime.Date && x.Date.Value.Date <= endTime.Date && x.ApplicationUid == applicationUid);
            IQueryable<AuditAirLogEntity> logList = from sta in statusList
                                                    join log in logAirRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime && x.PollutantCode == factorCode)
                                                        on sta.AuditStatusUid equals log.AuditStatusUid
                                                    select log;
            return logList;
        }

        /// <summary>
        /// 多测点位、因子、时间段获取审核日志（空气）
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="factorCode"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<AuditLogInfo> RerieveAirLog(string[] portId, string[] factorCode, DateTime startTime, DateTime endTime, string UserGuid)
        {
            //int [] pList = Array.ConvertAll<string, int>(portId, s => int.Parse(s));
            List<int?> IntPort = new List<int?>();
            foreach (string item in portId)
            {
                if (item != "")
                {
                    IntPort.Add(int.Parse(item));
                }
            }
            IQueryable<AuditStatusForDayEntity> statusList = auditStateRep.Retrieve(x => IntPort.Contains(x.PointId.Value) && x.Date.Value.Date >= startTime.Date && x.Date.Value.Date <= endTime.Date && x.ApplicationUid == EnumMapping.GetApplicationValue(ApplicationValue.Air));
            var logs = logAirRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime && x.AuditStatusUid != null && factorCode.Contains(x.PollutantCode));
            if (factorCode == null || factorCode.Length == 0)
            {
                logs = logAirRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime);
            }
            IQueryable<AuditLogInfo> logList = (from sta in statusList.ToList()
                                                join log in logs.ToList()
                                                on sta.AuditStatusUid.ToUpper() equals log.AuditStatusUid.ToUpper()
                                                join port in pointAirService.RetrieveAirMPList(UserGuid).ToList()
                                                on sta.PointId equals port.PointId
                                                select new AuditLogInfo
                                                {
                                                    tstamp = log.Tstamp.Value,
                                                    PointName = port.MonitoringPointName,
                                                    CreatUser = log.CreatUser,
                                                    AuditType = log.AuditType,
                                                    AuditPollutantDataValue = log.AuditPollutantDataValue,
                                                    PollutantName = log.PollutantName,
                                                    
                                                    SourcePollutantDataValue = log.SourcePollutantDataValue,
                                                    AuditReason = log.OperationReason,
                                                    Description=log.Description,
                                                    UpdateTime=log.UpdateDateTime
                                                }).AsQueryable<AuditLogInfo>();
            return logList;
        }

        /// <summary>
        /// 根据点位、因子、时间段获取审核日志（地表水）
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="applicationUid"></param>
        /// <param name="factorCode"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<AuditWaterLogEntity> RerieveWaterLog(int portId, string applicationUid, string factorCode, DateTime startTime, DateTime endTime)
        {
            IQueryable<AuditStatusForDayEntity> statusList = auditStateRep.Retrieve(x => x.PointId == portId && x.Date.Value.Date >= startTime.Date && x.Date.Value.Date <= endTime.Date && x.ApplicationUid == applicationUid);
            IQueryable<AuditWaterLogEntity> logList = from sta in statusList
                                                      join log in logWaterRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime && x.PollutantCode == factorCode)
                                                        on sta.AuditStatusUid equals log.AuditStatusUid
                                                      select log;
            return logList;
        }

        /// <summary>
        /// 多测点位、因子、时间段获取审核日志（地表水）
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="applicationUid"></param>
        /// <param name="factorCode"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<AuditLogInfo> RerieveWaterLog(string type, string[] portId, string[] factorCode, DateTime startTime, DateTime endTime, string UserGuid)
        {
            List<int?> IntPort = new List<int?>();
            foreach (string item in portId)
            {
                if (item != "")
                {
                    IntPort.Add(int.Parse(item));
                }
            }
            IQueryable<AuditStatusForDayEntity> statusList = auditStateRep.Retrieve(x => IntPort.Contains(x.PointId.Value) && x.Date.Value.Date >= startTime.Date && x.Date.Value.Date <= endTime.Date && x.ApplicationUid == EnumMapping.GetApplicationValue(ApplicationValue.Water));
            var logs = logWaterRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime && factorCode.Contains(x.PollutantCode) && x.OperationTypeEnum == type);
            IList<User> userList = userservice.GetAllUser();
            if (factorCode == null || factorCode.Length == 0)
            {
                logs = logWaterRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime);
            }

            IQueryable<AuditLogInfo> logList = (from sta in statusList.ToList()
                                                join log in logs.ToList()
                                                    on sta.AuditStatusUid equals log.AuditStatusUid
                                                join port in pointWaterService.RetrieveWaterMPList(UserGuid).ToList()
                                                    on sta.PointId equals port.PointId
                                                join use in userList
                                                    on log.UserUid equals use.Id.ToString()
                                                select new AuditLogInfo
                                                {
                                                    tstamp = log.Tstamp.Value,
                                                    PointName = port.MonitoringPointName,
                                                    CreatUser = use.CName,
                                                    OperationTypeEnum = log.OperationTypeEnum,
                                                    AuditPollutantDataValue = log.AuditPollutantDataValue,
                                                    PollutantName = log.PollutantName,
                                                    SourcePollutantDataValue = log.SourcePollutantDataValue,
                                                    AuditReason = log.OperationReason,
                                                    Description = log.Description,
                                                    AuditTime = log.AuditTime.Value

                                                }).AsQueryable<AuditLogInfo>();
            return logList;
        }
        public IQueryable<AuditLogInfo> RerieveWaterLog(string[] portId, string[] factorCode, DateTime startTime, DateTime endTime, string UserGuid)
        {
            List<int?> IntPort = new List<int?>();
            foreach (string item in portId)
            {
                if (item != "")
                {
                    IntPort.Add(int.Parse(item));
                }
            }
            IQueryable<AuditStatusForDayEntity> statusList = auditStateRep.Retrieve(x => IntPort.Contains(x.PointId.Value) && x.Date.Value.Date >= startTime.Date && x.Date.Value.Date <= endTime.Date && x.ApplicationUid == EnumMapping.GetApplicationValue(ApplicationValue.Water));
            var logs = logWaterRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime && factorCode.Contains(x.PollutantCode));
            IList<User> userList = userservice.GetAllUser();
            if (factorCode == null || factorCode.Length == 0)
            {
                logs = logWaterRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime);
            }

            IQueryable<AuditLogInfo> logList = (from sta in statusList.ToList()
                                                join log in logs.ToList()
                                                    on sta.AuditStatusUid equals log.AuditStatusUid
                                                join port in pointWaterService.RetrieveWaterMPList(UserGuid).ToList()
                                                    on sta.PointId equals port.PointId
                                                join use in userList
                                                    on log.UserUid equals use.Id.ToString()
                                                select new AuditLogInfo
                                                {
                                                    tstamp = log.Tstamp.Value,
                                                    PointName = port.MonitoringPointName,
                                                    CreatUser = use.CName,
                                                    OperationTypeEnum = log.OperationTypeEnum,
                                                    AuditPollutantDataValue = log.AuditPollutantDataValue,
                                                    PollutantName = log.PollutantName,
                                                    SourcePollutantDataValue = log.SourcePollutantDataValue,
                                                    AuditReason = log.OperationReason,
                                                    Description = log.Description,
                                                    AuditTime = log.AuditTime.Value

                                                }).AsQueryable<AuditLogInfo>();
            return logList;
        }
        public IQueryable<AuditLogInfo> RerieveWaterLog(string type, string[] portId, DateTime startTime, DateTime endTime, string UserGuid)
        {
            List<int?> IntPort = new List<int?>();
            foreach (string item in portId)
            {
                if (item != "")
                {
                    IntPort.Add(int.Parse(item));
                }
            }
            IQueryable<AuditStatusForDayEntity> statusList = auditStateRep.Retrieve(x => IntPort.Contains(x.PointId.Value) && x.Date.Value.Date >= startTime.Date && x.Date.Value.Date <= endTime.Date && x.ApplicationUid == EnumMapping.GetApplicationValue(ApplicationValue.Water));
            IList<User> userList = userservice.GetAllUser();
            var logs = logWaterRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime && x.OperationTypeEnum == type);
            IQueryable<AuditLogInfo> logList = (from sta in statusList.ToList()
                                                join log in logs.ToList()
                                                    on sta.AuditStatusUid equals log.AuditStatusUid
                                                join port in pointWaterService.RetrieveWaterMPList(UserGuid).ToList()
                                                    on sta.PointId equals port.PointId
                                                join use in userList
                                                    on log.UserUid equals use.Id.ToString()
                                                select new AuditLogInfo
                                                {
                                                    tstamp = log.Tstamp.Value,
                                                    PointName = port.MonitoringPointName,
                                                    CreatUser = use.CName,
                                                    OperationTypeEnum = log.OperationTypeEnum,
                                                    AuditPollutantDataValue = log.AuditPollutantDataValue,
                                                    PollutantName = log.PollutantName,
                                                    SourcePollutantDataValue = log.SourcePollutantDataValue,
                                                    AuditReason = log.OperationReason,
                                                    Description = log.Description,
                                                    //AuditTime = log.AuditTime.Value

                                                }).AsQueryable<AuditLogInfo>();
            return logList;
        }
        public IQueryable<AuditLogInfo> RerieveWaterLog2(string type, string[] portId, DateTime startTime, DateTime endTime, string UserGuid)
        {
            List<int?> IntPort = new List<int?>();
            foreach (string item in portId)
            {
                if (item != "")
                {
                    IntPort.Add(int.Parse(item));
                }
            }
            IQueryable<AuditStatusForDayEntity> statusList = auditStateRep.Retrieve(x => IntPort.Contains(x.PointId.Value) && x.Date.Value.Date >= startTime.Date && x.Date.Value.Date <= endTime.Date && x.ApplicationUid == EnumMapping.GetApplicationValue(ApplicationValue.Water));
            var logs = logWaterRep.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime && x.OperationTypeEnum == type);
            IQueryable<AuditLogInfo> logList = (from sta in statusList.ToList()
                                                join log in logs.ToList()
                                                    on sta.AuditStatusUid equals log.AuditStatusUid
                                                join port in pointWaterService.RetrieveWaterMPList(UserGuid).ToList()
                                                    on sta.PointId equals port.PointId
                                                select new AuditLogInfo
                                                {
                                                    tstamp = log.Tstamp.Value,
                                                    PointName = port.MonitoringPointName,
                                                    OperationTypeEnum = log.OperationTypeEnum,
                                                    AuditPollutantDataValue = log.AuditPollutantDataValue,
                                                    PollutantName = log.PollutantName,
                                                    SourcePollutantDataValue = log.SourcePollutantDataValue,
                                                    AuditReason = log.OperationReason,
                                                    Description = log.Description,
                                                    //AuditTime = log.AuditTime.Value

                                                }).AsQueryable<AuditLogInfo>();
            return logList;
        }
        #endregion
    }
}
