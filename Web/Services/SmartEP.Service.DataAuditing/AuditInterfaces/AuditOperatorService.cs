using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.MonitoringBusinessRepository.Water;
using SmartEP.MonitoringBusinessRepository.Common;
using SmartEP.AMSRepository.Air;
using SmartEP.AMSRepository.Water;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.DomainModel.AirAutoMonitoring;
using SmartEP.DomainModel.WaterAutoMonitoring;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.BaseInfoRepository.Channel;
using SmartEP.Service.AutoMonitoring.Air;
using System.Threading;

using SmartEP.DomainModel.BaseData;
using SmartEP.Service.DataAuditing.AuditBaseInfo;
using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Utilities.AdoData;
using System.Configuration;
using SmartEP.Service.BaseData.MPInfo;
using log4net;

namespace SmartEP.Service.DataAuditing.AuditInterfaces
{
    /// <summary>
    /// 名称：AuditOperatorService.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-08-16
    /// 维护人员：
    /// 最新维护人员：徐龙超
    /// 最新维护日期：2015-08-27
    /// 功能摘要：
    /// 审核站点抽象类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    /// 
    public class AuditOperatorService
    {
        #region 变量
        AuditOperatorSettingService operatorService = new AuditOperatorSettingService();
        AuditDataService auditDataService = new AuditDataService();
        AuditLogService logService = new AuditLogService();
        ILog logs = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        AuditDataRepository dataRep = new AuditDataRepository();
        MonitoringPointExtensionForEQMSWaterRepository g_ExtensionForEQMSWater = null;
        MonitoringPointAirService g_MonitoringPointAir = null;
        #region 空气
        AuditAirInfectantByHourRepository auditAirHourRep = new AuditAirInfectantByHourRepository();
        PollutantCodeRepository pollutantCodeRep = new PollutantCodeRepository();
        AuditStatusForDayRepository auditStateRep = new AuditStatusForDayRepository();
        AuditAirLogRepository logAirRep = new AuditAirLogRepository();
        AMSRepository.Air.InfectantBy60Repository Infec60AirRep = new AMSRepository.Air.InfectantBy60Repository();
        AuditMonitoringPointPollutantService auditPollutantService = new AuditMonitoringPointPollutantService();
        AuditAirStatusForHourRepository statusAirHourRep = new AuditAirStatusForHourRepository();//审核状态小时表
        static string controlFactors = "a05024,a21005,a34004,a34002,a21026,a21004";

        DatabaseHelper g_DatabaseHelper = new DatabaseHelper();

        #endregion

        #region 地表水
        AuditWaterInfectantByHourRepository waterHourRep = new AuditWaterInfectantByHourRepository();
        AuditWaterLogRepository logWaterRep = new AuditWaterLogRepository();
        AMSRepository.Water.InfectantBy60Repository Infec60WaterRep = new AMSRepository.Water.InfectantBy60Repository();
        AuditWaterStatusForHourRepository statusWaterHourRep = new AuditWaterStatusForHourRepository();//审核状态小时表
        #endregion

        #region 审核预处理服务
        AuditPreDataService preService = new AuditPreDataService();
        #endregion
        #endregion

        #region 审核操作

        #region 从审核历史表导入审核预处理数据
        public bool GetDataFromHis(string applicationUID, string[] portId, DateTime startTime, DateTime endTime, int days)
        {
            bool isSuccess = true;
            string errorMsg = "";
            try
            {
                if (startTime < DateTime.Now.Date.AddDays(-days))
                {
                    endTime = endTime < DateTime.Now.Date.AddDays(-days) ? endTime : DateTime.Now.Date.AddDays(-10);
                    ApplicationType applicationType = EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID) ? ApplicationType.Air : ApplicationType.Water;
                    dataRep.GetDataFromHis(applicationType, portId, startTime, endTime, out errorMsg);
                }
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }
        #endregion


        #region 修改数据、审核标记【水、气】
        /// <summary>
        /// 时间段置为无效、有效并记录日志信息
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="factor"></param>
        /// <param name="DataTime"></param>
        /// <param name="NewData"></param>
        /// <returns></returns>
        public bool ModifyAuditData(string applicationUID, string[] PointID, string[] factor, string[] DataTime, string[] NewData, string reason, string operatorFlag, string OperationTypeEnum, string UserIP, string UserUid, string CreatUser)
        {
            try
            {
                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                    return ModifyAirAuditDataForRowNew(applicationUID, PointID, factor, DataTime, NewData, reason, operatorFlag, OperationTypeEnum, UserIP, UserUid, CreatUser);
                else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                    return ModifyWaterAuditData(applicationUID, PointID, factor, DataTime, NewData, reason, operatorFlag, OperationTypeEnum, UserIP, UserUid, CreatUser);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 修改审核数据、置为无效、有效并记录日志信息
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="factor"></param>
        /// <param name="DataTime"></param>
        /// <param name="NewData"></param>
        /// <returns></returns>
        public bool ModifyAuditDataNew(string applicationUID, string[] PointID, string[] factor, string[] DataTime, string[] NewData, string reason, string operatorFlag, string OperationTypeEnum, string UserIP, string UserUid, string CreatUser)
        {
            try
            {
                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                    return ModifyAirAuditDataForRow(applicationUID, PointID, factor, DataTime, NewData, reason, operatorFlag, OperationTypeEnum, UserIP, UserUid, CreatUser);
                else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                    return ModifyWaterAuditData(applicationUID, PointID, factor, DataTime, NewData, reason, operatorFlag, OperationTypeEnum, UserIP, UserUid, CreatUser);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 修改审核数据、置为无效、有效并记录日志信息
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="factor"></param>
        /// <param name="DataTime"></param>
        /// <param name="NewData"></param>
        /// <returns></returns>
        public bool ModifyAuditDataForRow(string applicationUID, string[] PointID, string[] factor, string[] DataTime, string[] NewData, string reason, string operatorFlag, string OperationTypeEnum, string UserIP, string UserUid, string CreatUser)
        {
            try
            {
                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                    return ModifyAirAuditDataForRowNew(applicationUID, PointID, factor, DataTime, NewData, reason, operatorFlag, OperationTypeEnum, UserIP, UserUid, CreatUser);
                else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                    return ModifyWaterAuditData(applicationUID, PointID, factor, DataTime, NewData, reason, operatorFlag, OperationTypeEnum, UserIP, UserUid, CreatUser);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 修改审核数据、置为无效、有效并记录日志信息【空气】
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="factor"></param>
        /// <param name="DataTime"></param>
        /// <param name="NewData"></param>
        /// <returns></returns>
        public bool ModifyAirAuditData(string applicationUID, string[] PointID, string[] factor, string[] DataTime, string[] NewData, string reason, string operatorFlag, string OperationTypeEnum, string UserIP, string UserUid, string CreatUser)
        {
            V_AuditOperatorSettingEntity entity = operatorService.GetAuditOperator(operatorFlag);
            int i = 0;
            
            List<AuditAirInfectantByHourEntity> entitiesAdd = new List<AuditAirInfectantByHourEntity>();
            List<AuditAirInfectantByHourEntity> entitiesUpdate = new List<AuditAirInfectantByHourEntity>();
            List<AuditAirLogEntity> logAirReps = new List<AuditAirLogEntity>();
            //for (int i = 0; i < PointID.Length; i++)
            //{
            AuditStatusForDayEntity status = logService.RerieveAuditState(Convert.ToInt32(PointID[i]), Convert.ToDateTime(DataTime[i]).Date, Convert.ToDateTime(DataTime[i]).Date, applicationUID);
            if (status != null)
            {
                #region 状态表不为空
                ////原数据源
                //AuditAirInfectantByHourEntity auditData = auditAirHourRep.Retrieve(x => x.DataDateTime >= Convert.ToDateTime(DataTime[i]) && x.DataDateTime < Convert.ToDateTime(DataTime[i]).AddHours(1) && x.PollutantCode.Equals(factor[i]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();
                //新数据源：防止有重复数据
                //AuditAirInfectantByHourEntity[] auditDatas = auditAirHourRep.Retrieve(x => x.DataDateTime >= Convert.ToDateTime(DataTime[i]) && x.DataDateTime < Convert.ToDateTime(DataTime[i]).AddHours(1) && x.PollutantCode.Equals(factor[i]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).ToArray();

                AuditAirInfectantByHourEntity[] auditDatas = auditAirHourRep.Retrieve(x => x.DataDateTime >= Convert.ToDateTime(DataTime[i]) && x.DataDateTime < Convert.ToDateTime(DataTime[i]).AddHours(1) && x.PollutantCode.Equals(factor[i])).ToArray();

                if (auditDatas.Length > 0)
                {
                    foreach (AuditAirInfectantByHourEntity auditData in auditDatas)
                    {
                        #region 记录审核日志
                        AuditAirLogEntity log = new AuditAirLogEntity();
                        log.AuditLogUid = Guid.NewGuid().ToString();
                        log.AuditStatusUid = status.AuditStatusUid;
                        log.AuditTime = DateTime.Now;
                        log.Tstamp = Convert.ToDateTime(DataTime[i]);
                        log.AuditType = "数据审核";
                        log.PollutantCode = factor[i];
                        log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                        #region 原始值
                        decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i]);
                        log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                        #endregion
                        if (operatorFlag.Equals("modify"))//修改
                            log.AuditPollutantDataValue = NewData[i];
                        else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                            log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";
                        log.OperationTypeEnum = OperationTypeEnum;
                        log.OperationReason = reason;
                        log.UserIP = UserIP;
                        log.UserUid = UserUid;
                        log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                        log.CreatUser = CreatUser;
                        log.CreatDateTime = DateTime.Now;
                        log.UpdateUser = CreatUser;
                        log.UpdateDateTime = DateTime.Now;
                        
                        //logAirRep.Add(log);
                        logAirReps.Add(log);
                        #endregion

                        if (auditData == null)
                        {
                            AuditAirInfectantByHourEntity auditDataNew = new AuditAirInfectantByHourEntity();
                            auditDataNew.AuditHourUid = Guid.NewGuid().ToString();
                            auditDataNew.AuditStatusUid = status.AuditStatusUid;
                            auditDataNew.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                            auditDataNew.PointId = Convert.ToInt32(PointID[i]);
                            auditDataNew.DataDateTime = Convert.ToDateTime(DataTime[i]);
                            auditDataNew.PollutantCode = factor[i];
                            auditDataNew.IsAudit = "1";
                            if (operatorFlag.Equals("modify"))//修改
                            {
                                auditDataNew.PollutantValue = Convert.ToDecimal(NewData[i]) / 1000;
                                auditDataNew.AuditFlag = "MF";
                            }
                            else
                            {

                                auditDataNew.AuditFlag = "," + entity.StatusIdentify;
                            }
                            //auditAirHourRep.Add(auditDataNew);
                            entitiesAdd.Add(auditDataNew);
                        }
                        else
                        {
                            #region 修改审核数据、审核标记
                            if (operatorFlag.Equals("modify"))//修改
                            {
                                auditData.PollutantValue = Convert.ToDecimal(NewData[i]) / 1000;
                                auditData.AuditFlag = "MF";
                            }
                            else
                            {
                                if (entity != null && !entity.StatusIdentify.Equals(""))
                                {
                                    if (entity.StatusIdentify.Equals("VA"))//有效标记位
                                    {
                                        if (auditData.AuditFlag != null)
                                            auditData.AuditFlag = auditData.AuditFlag.Replace("RM", "VA");
                                        else
                                            auditData.AuditFlag = ",VA";
                                        if (!auditData.AuditFlag.Contains("VA"))
                                            auditData.AuditFlag += ",VA";
                                    }
                                    else
                                        auditData.AuditFlag = "," + entity.StatusIdentify;
                                }
                            }
                            auditData.IsAudit = "1";
                            //auditAirHourRep.Update(auditData);
                            entitiesUpdate.Add(auditData);
                            #endregion
                        }
                    }
                }
                else
                {
                    AuditAirInfectantByHourEntity auditData = auditAirHourRep.Retrieve(x => x.DataDateTime >= Convert.ToDateTime(DataTime[i]) && x.DataDateTime < Convert.ToDateTime(DataTime[i]).AddHours(1) && x.PollutantCode.Equals(factor[i]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();
                    #region 记录审核日志
                    AuditAirLogEntity log = new AuditAirLogEntity();
                    log.AuditLogUid = Guid.NewGuid().ToString();
                    log.AuditStatusUid = status.AuditStatusUid;
                    log.AuditTime = DateTime.Now;
                    log.Tstamp = Convert.ToDateTime(DataTime[i]);
                    log.AuditType = "数据审核";
                    log.PollutantCode = factor[i];
                    log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                    #region 原始值
                    decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i]);
                    log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                    #endregion
                    if (operatorFlag.Equals("modify"))//修改
                        log.AuditPollutantDataValue = NewData[i];
                    else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                        log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";
                    log.OperationTypeEnum = OperationTypeEnum;
                    log.OperationReason = reason;
                    log.UserIP = UserIP;
                    log.UserUid = UserUid;
                    log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                    log.CreatUser = CreatUser;
                    log.CreatDateTime = DateTime.Now;
                    log.UpdateUser = CreatUser;
                    log.UpdateDateTime = DateTime.Now;
                    //logAirRep.Add(log);
                    logAirReps.Add(log);
                    #endregion
                    AuditAirInfectantByHourEntity auditDataNew = new AuditAirInfectantByHourEntity();
                    auditDataNew.AuditHourUid = Guid.NewGuid().ToString();
                    auditDataNew.AuditStatusUid = status.AuditStatusUid;
                    auditDataNew.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                    auditDataNew.PointId = Convert.ToInt32(PointID[i]);
                    auditDataNew.DataDateTime = Convert.ToDateTime(DataTime[i]);
                    auditDataNew.PollutantCode = factor[i];
                    auditDataNew.IsAudit = "1";
                    if (operatorFlag.Equals("modify"))//修改
                    {
                        auditDataNew.PollutantValue = Convert.ToDecimal(NewData[i]) / 1000;
                        auditDataNew.AuditFlag = "MF";
                    }
                    else
                    {

                        auditDataNew.AuditFlag = "," + entity.StatusIdentify;
                    }
                    //auditAirHourRep.Add(auditDataNew);
                    entitiesAdd.Add(auditDataNew);
                }
                #endregion
            }
            else
            {
                #region 状态表为空

                #region 新增状态记录
                status = new AuditStatusForDayEntity();
                status.AuditStatusUid = Guid.NewGuid().ToString();
                status.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                status.PointId = Convert.ToInt32(PointID[i]);
                status.Date = Convert.ToDateTime(DataTime[i]);
                status.CreatUser = CreatUser;
                status.CreatDateTime = DateTime.Now;
                status.UpdateUser = CreatUser;
                status.UpdateDateTime = DateTime.Now;
                auditStateRep.Add(status);
                #endregion
                ////原数据源
                //AuditAirInfectantByHourEntity auditData = auditAirHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[i]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();
                //新数据源：防止有重复数据
                //AuditAirInfectantByHourEntity[] auditDatas = auditAirHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[i]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).ToArray();

                AuditAirInfectantByHourEntity[] auditDatas = auditAirHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[i])).ToArray();

                if (auditDatas.Length > 0)
                {
                    foreach (AuditAirInfectantByHourEntity auditData in auditDatas)
                    {
                        #region 记录审核日志
                        AuditAirLogEntity log = new AuditAirLogEntity();
                        log.AuditLogUid = Guid.NewGuid().ToString();
                        log.AuditStatusUid = status.AuditStatusUid;
                        log.AuditTime = DateTime.Now;
                        log.Tstamp = Convert.ToDateTime(DataTime[i]);
                        log.AuditType = "数据审核";
                        log.PollutantCode = factor[i];
                        log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                        #region 原始值
                        decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i]);
                        log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                        #endregion
                        if (operatorFlag.Equals("modify"))//修改
                            log.AuditPollutantDataValue = NewData[i];
                        else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                            log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";
                        log.OperationTypeEnum = OperationTypeEnum;
                        log.OperationReason = reason;
                        log.UserIP = UserIP;
                        log.UserUid = UserUid;
                        log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                        log.CreatUser = CreatUser;
                        log.CreatDateTime = DateTime.Now;
                        log.UpdateUser = CreatUser;
                        log.UpdateDateTime = DateTime.Now;
                        //logAirRep.Add(log);
                        logAirReps.Add(log);
                        #endregion

                        if (auditData == null)
                        {
                            AuditAirInfectantByHourEntity auditDataNew = new AuditAirInfectantByHourEntity();
                            auditDataNew.AuditHourUid = Guid.NewGuid().ToString();
                            auditDataNew.AuditStatusUid = status.AuditStatusUid;
                            auditDataNew.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                            auditDataNew.PointId = Convert.ToInt32(PointID[i]);
                            auditDataNew.DataDateTime = Convert.ToDateTime(DataTime[i]);
                            auditDataNew.PollutantCode = factor[i];
                            auditDataNew.IsAudit = "1";
                            if (operatorFlag.Equals("modify"))//修改
                            {
                                auditDataNew.PollutantValue = Convert.ToDecimal(NewData[i]) / 1000;
                                auditDataNew.AuditFlag = "MF";
                            }
                            else
                            {

                                auditDataNew.AuditFlag = "," + entity.StatusIdentify;
                            }
                            //auditAirHourRep.Add(auditDataNew);
                            entitiesAdd.Add(auditDataNew);
                        }
                        else
                        {
                            #region 修改审核数据、审核标记
                            if (operatorFlag.Equals("modify"))//修改
                            {
                                auditData.PollutantValue = Convert.ToDecimal(NewData[i]) / 1000;
                                auditData.AuditFlag = "MF";
                            }
                            else
                            {
                                if (entity != null && !entity.StatusIdentify.Equals(""))
                                {
                                    if (entity.StatusIdentify.Equals("VA"))//有效标记位
                                    {
                                        if (auditData.AuditFlag != null)
                                            auditData.AuditFlag = auditData.AuditFlag.Replace("RM", "VA");
                                        else
                                            auditData.AuditFlag = ",VA";
                                        if (!auditData.AuditFlag.Contains("VA"))
                                            auditData.AuditFlag += ",VA";
                                    }
                                    else
                                        auditData.AuditFlag = "," + entity.StatusIdentify;
                                }
                            }
                            auditData.IsAudit = "1";
                            //auditAirHourRep.Update(auditData);
                            entitiesUpdate.Add(auditData);
                            #endregion
                        }
                    }
                }
                else
                {
                    #region 记录审核日志
                    AuditAirInfectantByHourEntity auditData = auditAirHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[i]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();

                    AuditAirLogEntity log = new AuditAirLogEntity();
                    log.AuditLogUid = Guid.NewGuid().ToString();
                    log.AuditStatusUid = status.AuditStatusUid;
                    log.AuditTime = DateTime.Now;
                    log.Tstamp = Convert.ToDateTime(DataTime[i]);
                    log.AuditType = "数据审核";
                    log.PollutantCode = factor[i];
                    log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                    #region 原始值
                    decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i]);
                    log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                    #endregion
                    if (operatorFlag.Equals("modify"))//修改
                        log.AuditPollutantDataValue = NewData[i];
                    else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                        log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";
                    log.OperationTypeEnum = OperationTypeEnum;
                    log.OperationReason = reason;
                    log.UserIP = UserIP;
                    log.UserUid = UserUid;
                    log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                    log.CreatUser = CreatUser;
                    log.CreatDateTime = DateTime.Now;
                    log.UpdateUser = CreatUser;
                    log.UpdateDateTime = DateTime.Now;
                    //logAirRep.Add(log);
                    logAirReps.Add(log);
                    #endregion

                    AuditAirInfectantByHourEntity auditDataNew = new AuditAirInfectantByHourEntity();
                    auditDataNew.AuditHourUid = Guid.NewGuid().ToString();
                    auditDataNew.AuditStatusUid = status.AuditStatusUid;
                    auditDataNew.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                    auditDataNew.PointId = Convert.ToInt32(PointID[i]);
                    auditDataNew.DataDateTime = Convert.ToDateTime(DataTime[i]);
                    auditDataNew.PollutantCode = factor[i];
                    auditDataNew.IsAudit = "1";
                    if (operatorFlag.Equals("modify"))//修改
                    {
                        auditDataNew.PollutantValue = Convert.ToDecimal(NewData[i]) / 1000;
                        auditDataNew.AuditFlag = "MF";
                    }
                    else
                    {

                        auditDataNew.AuditFlag = "," + entity.StatusIdentify;
                    }
                    //auditAirHourRep.Add(auditDataNew);
                    entitiesAdd.Add(auditDataNew);
                }


                #endregion
            }
            //}
            logs.Error("数据库链接" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
            if (entitiesAdd.Count > 0)
            {
                auditAirHourRep.BatchAdd(entitiesAdd);
            }
            if (entitiesUpdate.Count > 0)
            {
                auditAirHourRep.BatchUpdate(entitiesUpdate);
            }
            if (logAirReps.Count > 0)
            {
                logAirRep.BatchAdd(logAirReps);
            }
            logs.Error("结束" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
            return true;
        }
        /// <summary>
        /// 修改审核数据、置为无效、有效并记录日志信息【空气】
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="factor"></param>
        /// <param name="DataTime"></param>
        /// <param name="NewData"></param>
        /// <returns></returns>
        public bool ModifyAirAuditDataForRow(string applicationUID, string[] PointID, string[] factor, string[] DataTime, string[] NewData, string reason, string operatorFlag, string OperationTypeEnum, string UserIP, string UserUid, string CreatUser)
        {
            try
            {
                V_AuditOperatorSettingEntity entity = operatorService.GetAuditOperator(operatorFlag);
                //int i = 0;

                List<AuditAirInfectantByHourEntity> entitiesAdd = new List<AuditAirInfectantByHourEntity>();
                List<AuditAirInfectantByHourEntity> entitiesUpdate = new List<AuditAirInfectantByHourEntity>();
                List<AuditAirLogEntity> logAirReps = new List<AuditAirLogEntity>();
                string sql = string.Empty;
                string insertSql = string.Empty;
                string factorsql = string.Empty;
                string factorAll = string.Empty;
                string facAuditFlag = string.Empty;
                string InstrumentSql = string.Empty;
                string pointIdSql = string.Empty;
                string InstrumentCodeSql = string.Empty;
                string InstrumentCode = "";
                InstrumentSql = string.Format(@"select InstrumentName FROM V_Point_InstrumentChannels where PollutantCode='{0}'", factor[0]);
                string InstrumentName = g_DatabaseHelper.ExecuteDataTable(InstrumentSql, "AMS_BaseDataConnection").Rows[0][0].ToString();
                pointIdSql = string.Format(@"select PollutantCode from V_Point_InstrumentChannels where InstrumentName='{0}' and PointId={1}", InstrumentName, PointID[0]);

                List<string> Dates = new List<string>(DataTime);
                string[] dat = Dates.Distinct().ToArray();

                string facS = string.Empty;

                for (int i = 0; i < PointID.Length; i++)
                {
                    AuditStatusForDayEntity status = logService.RerieveAuditState(Convert.ToInt32(PointID[i]), Convert.ToDateTime(DataTime[i]).Date, Convert.ToDateTime(DataTime[i]).Date, applicationUID);
                    if (status != null)
                    {
                        for (int j = 0; j < factor.Length; j++)
                        {
                            factorsql += "'" + factor[j] + "',";
                            factorAll += factor[j] + ",";
                        }
                        #region 注释
//                        for (int j = 0; j < factor.Length; j++)
//                        {
//                            factorsql += "'" + factor[j] + "',";
//                            factorAll += factor[j] + ",";
//                        }
//                        if (factor.Length > 1)
//                        {
//                            if (factor.Length == factor.Distinct().ToArray().Length)
//                            {
//                                if (factor.Length <= g_DatabaseHelper.ExecuteDataTable(pointIdSql, "AMS_BaseDataConnection").Rows.Count && PointID.Length == factor.Length)
//                                {
//                                    InstrumentCodeSql = string.Format(@"  select PollutantName
//  FROM [AMS_BaseData].[Standard].[TB_PollutantCode]
//  where PollutantCode ='{0}'", factor[i]);
//                                    InstrumentCode = "" + g_DatabaseHelper.ExecuteDataTable(InstrumentCodeSql, "AMS_BaseDataConnection").Rows[0][0].ToString();
//                                }
//                                else
//                                {
//                                    InstrumentCode = "所有因子";
//                                }
//                            }
//                            else
//                            {
//                                InstrumentCodeSql = string.Format(@"  select PollutantName
//  FROM [AMS_BaseData].[Standard].[TB_PollutantCode]
//  where PollutantCode ='{0}'", factor[i]);
//                                InstrumentCode = "" + g_DatabaseHelper.ExecuteDataTable(InstrumentCodeSql, "AMS_BaseDataConnection").Rows[0][0].ToString();
//                            }

//                        }
//                        else
//                        {
//                            for (int k = 0; k < factor.Length; k++)
//                            {
//                                facS += "'" + factor[k] + "',";
//                            }
//                            InstrumentCodeSql = string.Format(@"  select PollutantName
//  FROM [AMS_BaseData].[Standard].[TB_PollutantCode]
//  where PollutantCode in({0})", facS.TrimEnd(','));
//                            DataTable dtCode = g_DatabaseHelper.ExecuteDataTable(InstrumentCodeSql, "AMS_BaseDataConnection");
//                            for (int k = 0; k < dtCode.Rows.Count; k++)
//                            {
//                                InstrumentCode += dtCode.Rows[k][0].ToString() + ",";
//                            }
//                            InstrumentCode = InstrumentCode.TrimEnd(',');
//                        }
//                        insertSql = string.Format(@"INSERT INTO [Audit].[TB_AuditAirLog] 
//  ([AuditLogUid]
//      ,[AuditStatusUid]
//      ,[tstamp]
//      ,[AuditTime]
//      ,[AuditType]
//      ,[PollutantCode]
//      ,[PollutantName]
//      ,[AuditPollutantDataValue]
//      ,[OperationTypeEnum]
//      ,[OperationReason]
//      ,[UserIP]
//      ,[UserUid]
//      ,[Description]
//      ,[CreatUser]
//      ,[CreatDateTime]
//      ,[UpdateUser]
//      ,[UpdateDateTime])
//   VALUES ({0},'{1}','{2}','{3}','{4}','{5}','{6}',N'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')"
//           , "(SELECT NEWID())", status.AuditStatusUid, Convert.ToDateTime(DataTime[i]).ToString("yyyy-MM-dd HH:mm:ss")
//           , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "数据审核", factor[i], InstrumentName, InstrumentCode, OperationTypeEnum, reason
//           , UserIP, UserUid, operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "")
//           , CreatUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CreatUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
//                        logs.Error(insertSql);
//                        g_DatabaseHelper.ExecuteInsert(insertSql, "AMS_MonitoringBusinessConnection");
                    #endregion
                        #region
                        //                #region 状态表不为空
                        //                for (int j = 0; j < factor.Length; j++)
                        //                {
                        //                    factorsql += "'" + factor[j] + "',";
                        //                    logs.Error("第" + (j + 1) + "个因子开始" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
                        //                    AuditAirInfectantByHourEntity[] auditDatas = auditAirHourRep.Retrieve(x => x.DataDateTime >= Convert.ToDateTime(DataTime[i]) && x.DataDateTime < Convert.ToDateTime(DataTime[i]).AddHours(1) && x.PollutantCode.Equals(factor[j])).ToArray();

                        //                    if (auditDatas.Length > 0)
                        //                    {
                        //                        foreach (AuditAirInfectantByHourEntity auditData in auditDatas)
                        //                        {
                        //                            #region 记录审核日志
                        //                            AuditAirLogEntity log = new AuditAirLogEntity();
                        //                            log.AuditLogUid = Guid.NewGuid().ToString();
                        //                            log.AuditStatusUid = status.AuditStatusUid;
                        //                            log.AuditTime = DateTime.Now;
                        //                            log.Tstamp = Convert.ToDateTime(DataTime[i]);
                        //                            log.AuditType = "数据审核";
                        //                            log.PollutantCode = factor[j];
                        //                            log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                        //                            #region 原始值
                        //                            decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[j]);
                        //                            log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                        //                            #endregion
                        //                            if (operatorFlag.Equals("modify"))//修改
                        //                                log.AuditPollutantDataValue = NewData[i];
                        //                            else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                        //                                log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";
                        //                            log.OperationTypeEnum = OperationTypeEnum;
                        //                            log.OperationReason = reason;
                        //                            log.UserIP = UserIP;
                        //                            log.UserUid = UserUid;
                        //                            log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                        //                            log.CreatUser = CreatUser;
                        //                            log.CreatDateTime = DateTime.Now;
                        //                            log.UpdateUser = CreatUser;
                        //                            log.UpdateDateTime = DateTime.Now;

                        //                            //logAirRep.Add(log);
                        //                            logAirReps.Add(log);
                        //                            #endregion
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        #region 记录审核日志
                        //                        AuditAirInfectantByHourEntity auditData = auditAirHourRep.Retrieve(x => x.DataDateTime >= Convert.ToDateTime(DataTime[i]) && x.DataDateTime < Convert.ToDateTime(DataTime[i]).AddHours(1) && x.PollutantCode.Equals(factor[j]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();
                        //                        AuditAirLogEntity log = new AuditAirLogEntity();
                        //                        log.AuditLogUid = Guid.NewGuid().ToString();
                        //                        log.AuditStatusUid = status.AuditStatusUid;
                        //                        log.AuditTime = DateTime.Now;
                        //                        log.Tstamp = Convert.ToDateTime(DataTime[i]);
                        //                        log.AuditType = "数据审核";
                        //                        log.PollutantCode = factor[j];
                        //                        log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                        //                        #region 原始值
                        //                        decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[j]);
                        //                        log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                        //                        #endregion
                        //                        if (operatorFlag.Equals("modify"))//修改
                        //                            log.AuditPollutantDataValue = NewData[i];
                        //                        else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                        //                            log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";
                        //                        log.OperationTypeEnum = OperationTypeEnum;
                        //                        log.OperationReason = reason;
                        //                        log.UserIP = UserIP;
                        //                        log.UserUid = UserUid;
                        //                        log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                        //                        log.CreatUser = CreatUser;
                        //                        log.CreatDateTime = DateTime.Now;
                        //                        log.UpdateUser = CreatUser;
                        //                        log.UpdateDateTime = DateTime.Now;

                        //                        //logAirRep.Add(log);
                        //                        logAirReps.Add(log);
                        //                        #endregion
                        //                    }
                        //                }

                        //                
                        //                #endregion
                        #endregion
                        #region 修改审核数据、审核标记
                        if (operatorFlag.Equals("modify"))//修改
                        {
                            facAuditFlag = "MF";
                        }
                        else
                        {
                            if (entity != null && !entity.StatusIdentify.Equals(""))
                            {
                                if (entity.StatusIdentify.Equals("VA"))//有效标记位
                                {
                                    facAuditFlag = "VA";
                                }
                                else
                                    facAuditFlag = entity.StatusIdentify;
                            }
                        }
                        #endregion
                        sql = string.Format(@"update [Audit].[TB_AuditAirInfectantByHour] set AuditFlag = '{0}',IsAudit = {1}
WHERE  ApplicationUid = 'airaaira-aira-aira-aira-airaairaaira' AND PointId = {2} AND DataDateTime>= '{3}' 
AND DataDateTime<'{4}'  AND PollutantCode in ({5})", facAuditFlag, 1, PointID[i], Convert.ToDateTime(DataTime[i]).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(DataTime[i]).AddHours(1).ToString("yyyy-MM-dd HH:mm:ss"), factorsql.TrimEnd(','));
                        logs.Error(sql);
                        g_DatabaseHelper.ExecuteScalar(sql, "AMS_MonitoringBusinessConnection");

                    }
                    else
                    {
                        #region 状态表为空

                        #region 新增状态记录
                        status = new AuditStatusForDayEntity();
                        status.AuditStatusUid = Guid.NewGuid().ToString();
                        status.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                        status.PointId = Convert.ToInt32(PointID[i]);
                        status.Date = Convert.ToDateTime(DataTime[i]);
                        status.CreatUser = CreatUser;
                        status.CreatDateTime = DateTime.Now;
                        status.UpdateUser = CreatUser;
                        status.UpdateDateTime = DateTime.Now;
                        auditStateRep.Add(status);
                        #endregion
                        for (int j = 0; j < factor.Length; j++)
                        {
                            AuditAirInfectantByHourEntity[] auditDatas = auditAirHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[j])).ToArray();

                            if (auditDatas.Length > 0)
                            {
                                foreach (AuditAirInfectantByHourEntity auditData in auditDatas)
                                {
                                    #region 记录审核日志
                                    AuditAirLogEntity log = new AuditAirLogEntity();
                                    log.AuditLogUid = Guid.NewGuid().ToString();
                                    log.AuditStatusUid = status.AuditStatusUid;
                                    log.AuditTime = DateTime.Now;
                                    log.Tstamp = Convert.ToDateTime(DataTime[i]);
                                    log.AuditType = "数据审核";
                                    log.PollutantCode = factor[j];
                                    log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                                    #region 原始值
                                    decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[j]);
                                    log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                                    #endregion
                                    if (operatorFlag.Equals("modify"))//修改
                                        log.AuditPollutantDataValue = NewData[i];
                                    else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                                        log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";
                                    log.OperationTypeEnum = OperationTypeEnum;
                                    log.OperationReason = reason;
                                    log.UserIP = UserIP;
                                    log.UserUid = UserUid;
                                    log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                                    log.CreatUser = CreatUser;
                                    log.CreatDateTime = DateTime.Now;
                                    log.UpdateUser = CreatUser;
                                    log.UpdateDateTime = DateTime.Now;

                                    //logAirRep.Add(log);
                                    logAirReps.Add(log);
                                    #endregion

                                    if (auditData == null)
                                    {
                                        AuditAirInfectantByHourEntity auditDataNew = new AuditAirInfectantByHourEntity();
                                        auditDataNew.AuditHourUid = Guid.NewGuid().ToString();
                                        auditDataNew.AuditStatusUid = status.AuditStatusUid;
                                        auditDataNew.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                                        auditDataNew.PointId = Convert.ToInt32(PointID[i]);
                                        auditDataNew.DataDateTime = Convert.ToDateTime(DataTime[i]);
                                        auditDataNew.PollutantCode = factor[j];
                                        auditDataNew.IsAudit = "1";
                                        if (operatorFlag.Equals("modify"))//修改
                                        {
                                            auditDataNew.PollutantValue = Convert.ToDecimal(NewData[i]) / 1000;
                                            auditDataNew.AuditFlag = "MF";
                                        }
                                        else
                                        {

                                            auditDataNew.AuditFlag = "," + entity.StatusIdentify;
                                        }
                                        //auditAirHourRep.Add(auditDataNew);
                                        //实体类数组
                                        entitiesAdd.Add(auditDataNew);
                                    }
                                    else
                                    {
                                        #region 修改审核数据、审核标记
                                        if (operatorFlag.Equals("modify"))//修改
                                        {
                                            auditData.PollutantValue = Convert.ToDecimal(NewData[i]) / 1000;
                                            auditData.AuditFlag = "MF";
                                        }
                                        else
                                        {
                                            if (entity != null && !entity.StatusIdentify.Equals(""))
                                            {
                                                if (entity.StatusIdentify.Equals("VA"))//有效标记位
                                                {
                                                    if (auditData.AuditFlag != null)
                                                        auditData.AuditFlag = auditData.AuditFlag.Replace("RM", "VA");
                                                    else
                                                        auditData.AuditFlag = ",VA";
                                                    if (!auditData.AuditFlag.Contains("VA"))
                                                        auditData.AuditFlag += ",VA";
                                                }
                                                else
                                                    auditData.AuditFlag = "," + entity.StatusIdentify;
                                            }
                                        }
                                        auditData.IsAudit = "1";
                                        //auditAirHourRep.Update(auditData);
                                        //实体类数组
                                        entitiesUpdate.Add(auditData);
                                        #endregion
                                    }
                                }
                            }
                            //如果没有数据
                            else
                            {
                                #region 记录审核日志
                                AuditAirInfectantByHourEntity auditData = auditAirHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[j]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();
                                AuditAirLogEntity log = new AuditAirLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.AuditTime = DateTime.Now;
                                log.Tstamp = Convert.ToDateTime(DataTime[i]);
                                log.AuditType = "数据审核";
                                log.PollutantCode = factor[j];
                                log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                                #region 原始值
                                decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[j]);
                                log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                                #endregion
                                if (operatorFlag.Equals("modify"))//修改
                                    log.AuditPollutantDataValue = NewData[i];
                                else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                                    log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";
                                log.OperationTypeEnum = OperationTypeEnum;
                                log.OperationReason = reason;
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                logAirRep.Add(log);
                                #endregion
                                AuditAirInfectantByHourEntity auditDataNew = new AuditAirInfectantByHourEntity();
                                auditDataNew.AuditHourUid = Guid.NewGuid().ToString();
                                auditDataNew.AuditStatusUid = status.AuditStatusUid;
                                auditDataNew.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                                auditDataNew.PointId = Convert.ToInt32(PointID[i]);
                                auditDataNew.DataDateTime = Convert.ToDateTime(DataTime[i]);
                                auditDataNew.PollutantCode = factor[j];
                                auditDataNew.IsAudit = "1";
                                if (operatorFlag.Equals("modify"))//修改
                                {
                                    auditDataNew.PollutantValue = Convert.ToDecimal(NewData[i]) / 1000;
                                    auditDataNew.AuditFlag = "MF";
                                }
                                else
                                {

                                    auditDataNew.AuditFlag = "," + entity.StatusIdentify;
                                }
                                //auditAirHourRep.Add(auditDataNew);
                                //实体类数组
                                entitiesAdd.Add(auditDataNew);
                            }
                        }

                        #endregion
                    }
                }
                AuditStatusForDayEntity statusR = logService.RerieveAuditState(Convert.ToInt32(PointID[0]), Convert.ToDateTime(DataTime[0]).Date, Convert.ToDateTime(DataTime[0]).Date, applicationUID);
                if (statusR != null)
                {
                    if (dat.Length > 1)//时间不一致说明选中的数组不是同一个时间点，每个不同的时间需要一条日志记录
                    {
                        string dte = DataTime[0];
                        for (int j = 0; j < dat.Length; j++)
                        {
                            for (int k = 0; k < DataTime.Length;k++ )
                            {
                                if (DataTime[k].Equals(dat[j]))
                                {
                                    facS += "'" + factor[k] + "',";
                                    dte = DataTime[k];
                                }
                            }
                            InstrumentCodeSql = string.Format(@"  select PollutantName
                              FROM [AMS_BaseData].[Standard].[TB_PollutantCode]
                              where PollutantCode in({0})", facS.TrimEnd(','));
                            facS = "";
                            DataTable dtCode = g_DatabaseHelper.ExecuteDataTable(InstrumentCodeSql, "AMS_BaseDataConnection");
                            for (int k = 0; k < dtCode.Rows.Count; k++)
                            {
                                InstrumentCode += dtCode.Rows[k][0].ToString() + ",";
                            }
                            InstrumentCode = InstrumentCode.TrimEnd(',');
                            insertSql += string.Format(@"INSERT INTO [Audit].[TB_AuditAirLog] 
                              ([AuditLogUid]
                                  ,[AuditStatusUid]
                                  ,[tstamp]
                                  ,[AuditTime]
                                  ,[AuditType]
                                  ,[PollutantCode]
                                  ,[PollutantName]
                                  ,[AuditPollutantDataValue]
                                  ,[OperationTypeEnum]
                                  ,[OperationReason]
                                  ,[UserIP]
                                  ,[UserUid]
                                  ,[Description]
                                  ,[CreatUser]
                                  ,[CreatDateTime]
                                  ,[UpdateUser]
                                  ,[UpdateDateTime])
                               VALUES ({0},'{1}','{2}','{3}','{4}','{5}','{6}',N'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}'); "
           , "(SELECT NEWID())", statusR.AuditStatusUid, Convert.ToDateTime(dte).ToString("yyyy-MM-dd HH:mm:ss")
           , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "数据审核", factor[0], InstrumentName, InstrumentCode, OperationTypeEnum, reason
           , UserIP, UserUid, operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "")
           , CreatUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CreatUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            InstrumentCode = "";
                        }
                        logs.Error(insertSql);
                        g_DatabaseHelper.ExecuteInsert(insertSql, "AMS_MonitoringBusinessConnection");
                    }
                    else//时间一致说明选中的数组是同一个时间点，所以只需一个日志记录
                    {
                        for (int k = 0; k < factor.Length; k++)
                        {
                            facS += "'" + factor[k] + "',";
                        }
                        InstrumentCodeSql = string.Format(@"  select PollutantName
                              FROM [AMS_BaseData].[Standard].[TB_PollutantCode]
                              where PollutantCode in({0})", facS.TrimEnd(','));
                        DataTable dtCode = g_DatabaseHelper.ExecuteDataTable(InstrumentCodeSql, "AMS_BaseDataConnection");
                        for (int k = 0; k < dtCode.Rows.Count; k++)
                        {
                            InstrumentCode += dtCode.Rows[k][0].ToString() + ",";
                        }
                        InstrumentCode = InstrumentCode.TrimEnd(',');
                        #region 注释
                        //                            if (factor.Length > 1)
                        //                            {
                        //                                if (factor.Length == factor.Distinct().ToArray().Length)
                        //                                {
                        //                                    if (factor.Length <= g_DatabaseHelper.ExecuteDataTable(pointIdSql, "AMS_BaseDataConnection").Rows.Count && PointID.Length == factor.Length)
                        //                                    {
                        //                                        InstrumentCodeSql = string.Format(@"  select PollutantName
                        //                              FROM [AMS_BaseData].[Standard].[TB_PollutantCode]
                        //                              where PollutantCode ='{0}'", factor[i]);
                        //                                        InstrumentCode = "" + g_DatabaseHelper.ExecuteDataTable(InstrumentCodeSql, "AMS_BaseDataConnection").Rows[0][0].ToString();
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        InstrumentCode = "所有因子";
                        //                                    }
                        //                                }
                        //                                else
                        //                                {
                        //                                    InstrumentCodeSql = string.Format(@"  select PollutantName
                        //                              FROM [AMS_BaseData].[Standard].[TB_PollutantCode]
                        //                              where PollutantCode ='{0}'", factor[i]);
                        //                                    InstrumentCode = "" + g_DatabaseHelper.ExecuteDataTable(InstrumentCodeSql, "AMS_BaseDataConnection").Rows[0][0].ToString();
                        //                                }

                        //                            }
                        //                            else
                        //                            {
                        //                                for (int k = 0; k < factor.Length; k++)
                        //                                {
                        //                                    facS += "'" + factor[k] + "',";
                        //                                }
                        //                                InstrumentCodeSql = string.Format(@"  select PollutantName
                        //                              FROM [AMS_BaseData].[Standard].[TB_PollutantCode]
                        //                              where PollutantCode in({0})", facS.TrimEnd(','));
                        //                                DataTable dtCode = g_DatabaseHelper.ExecuteDataTable(InstrumentCodeSql, "AMS_BaseDataConnection");
                        //                                for (int k = 0; k < dtCode.Rows.Count; k++)
                        //                                {
                        //                                    InstrumentCode += dtCode.Rows[k][0].ToString() + ",";
                        //                                }
                        //                                InstrumentCode = InstrumentCode.TrimEnd(',');
                        //                            }
                        #endregion
                        insertSql = string.Format(@"INSERT INTO [Audit].[TB_AuditAirLog] 
                              ([AuditLogUid]
                                  ,[AuditStatusUid]
                                  ,[tstamp]
                                  ,[AuditTime]
                                  ,[AuditType]
                                  ,[PollutantCode]
                                  ,[PollutantName]
                                  ,[AuditPollutantDataValue]
                                  ,[OperationTypeEnum]
                                  ,[OperationReason]
                                  ,[UserIP]
                                  ,[UserUid]
                                  ,[Description]
                                  ,[CreatUser]
                                  ,[CreatDateTime]
                                  ,[UpdateUser]
                                  ,[UpdateDateTime])
                               VALUES ({0},'{1}','{2}','{3}','{4}','{5}','{6}',N'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')"
           , "(SELECT NEWID())", statusR.AuditStatusUid, Convert.ToDateTime(DataTime[0]).ToString("yyyy-MM-dd HH:mm:ss")
           , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "数据审核", factor[0], InstrumentName, InstrumentCode, OperationTypeEnum, reason
           , UserIP, UserUid, operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "")
           , CreatUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CreatUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        logs.Error(insertSql);
                        g_DatabaseHelper.ExecuteInsert(insertSql, "AMS_MonitoringBusinessConnection");
                    }
                }
                if (entitiesAdd.Count > 0)
                {
                    auditAirHourRep.BatchAdd(entitiesAdd);
                }
                if (entitiesUpdate.Count > 0)
                {
                    auditAirHourRep.BatchUpdate(entitiesUpdate);
                }
                if (logAirReps.Count > 0)
                {
                    logAirRep.BatchAdd(logAirReps);
                }
                logs.Error("结束" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
                return true;
            }
            catch (Exception ex)
            {
                logs.Error(ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 修改审核数据、置为无效、有效并记录日志信息【空气】（新）
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="factor"></param>
        /// <param name="DataTime"></param>
        /// <param name="NewData"></param>
        /// <returns></returns>
        public bool ModifyAirAuditDataForRowNew(string applicationUID, string[] PointID, string[] factor, string[] DataTime, string[] NewData, string reason, string operatorFlag, string OperationTypeEnum, string UserIP, string UserUid, string CreatUser)
        {
            try
            {
                V_AuditOperatorSettingEntity entity = operatorService.GetAuditOperator(operatorFlag);
                //int i = 0;
                
                List<AuditAirInfectantByHourEntity> entitiesAdd = new List<AuditAirInfectantByHourEntity>();
                List<AuditAirInfectantByHourEntity> entitiesUpdate = new List<AuditAirInfectantByHourEntity>();
                List<AuditAirLogEntity> logAirReps = new List<AuditAirLogEntity>();
                string sql = string.Empty;
                string insertSql = string.Empty;
                string factorsql = string.Empty;
                string factorAll = string.Empty;
                string facAuditFlag = string.Empty;
                string InstrumentSql = string.Empty;
                string pointIdSql = string.Empty;
                string InstrumentCodeSql = string.Empty;
                string InstrumentCode = "";
                InstrumentSql = string.Format(@"select InstrumentName FROM V_Point_InstrumentChannels where PollutantCode='{0}'", factor[0]);
                string InstrumentName = g_DatabaseHelper.ExecuteDataTable(InstrumentSql, "AMS_BaseDataConnection").Rows[0][0].ToString();
                pointIdSql = string.Format(@"select PollutantCode from V_Point_InstrumentChannels where InstrumentName='{0}' and PointId={1}", InstrumentName, PointID[0]);
                
                string facS = string.Empty;
                
                for (int i = 0; i < PointID.Length; i++)
                {
                    AuditStatusForDayEntity status = logService.RerieveAuditState(Convert.ToInt32(PointID[i]), Convert.ToDateTime(DataTime[i]).Date, Convert.ToDateTime(DataTime[i]).Date, applicationUID);
                    if (status != null)
                    {
                        for (int j = 0; j < factor.Length; j++)
                        {
                            factorsql += "'" + factor[j] + "',";
                            factorAll += factor[j] + ",";
                        }
                        if (factor.Length > 1)
                        {
                            if (factor.Length == factor.Distinct().ToArray().Length)
                            {
                                if (factor.Length <= g_DatabaseHelper.ExecuteDataTable(pointIdSql, "AMS_BaseDataConnection").Rows.Count && PointID.Length==factor.Length)
                                {
                                    InstrumentCodeSql = string.Format(@"  select PollutantName
  FROM [AMS_BaseData].[Standard].[TB_PollutantCode]
  where PollutantCode ='{0}'", factor[i]);
                                    InstrumentCode = "" + g_DatabaseHelper.ExecuteDataTable(InstrumentCodeSql, "AMS_BaseDataConnection").Rows[0][0].ToString();
                                }
                                else
                                {
                                    InstrumentCode = "所有因子";
                                }
                            }
                            else
                            {
                                InstrumentCodeSql = string.Format(@"  select PollutantName
  FROM [AMS_BaseData].[Standard].[TB_PollutantCode]
  where PollutantCode ='{0}'", factor[i]);
                                InstrumentCode = "" + g_DatabaseHelper.ExecuteDataTable(InstrumentCodeSql, "AMS_BaseDataConnection").Rows[0][0].ToString();
                            }

                        }
                        else
                        {
                            for (int k = 0; k < factor.Length; k++)
                            {
                                facS += "'" + factor[k] + "',";
                            }
                            InstrumentCodeSql = string.Format(@"  select PollutantName
  FROM [AMS_BaseData].[Standard].[TB_PollutantCode]
  where PollutantCode in({0})", facS.TrimEnd(','));
                            DataTable dtCode = g_DatabaseHelper.ExecuteDataTable(InstrumentCodeSql, "AMS_BaseDataConnection");
                            for (int k = 0; k < dtCode.Rows.Count; k++)
                            {
                                InstrumentCode += dtCode.Rows[k][0].ToString() + ",";
                            }
                            InstrumentCode = InstrumentCode.TrimEnd(',');
                        }


                        #region
                        //                #region 状态表不为空
                        //                for (int j = 0; j < factor.Length; j++)
                        //                {
                        //                    factorsql += "'" + factor[j] + "',";
                        //                    logs.Error("第" + (j + 1) + "个因子开始" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
                        //                    AuditAirInfectantByHourEntity[] auditDatas = auditAirHourRep.Retrieve(x => x.DataDateTime >= Convert.ToDateTime(DataTime[i]) && x.DataDateTime < Convert.ToDateTime(DataTime[i]).AddHours(1) && x.PollutantCode.Equals(factor[j])).ToArray();

                        //                    if (auditDatas.Length > 0)
                        //                    {
                        //                        foreach (AuditAirInfectantByHourEntity auditData in auditDatas)
                        //                        {
                        //                            #region 记录审核日志
                        //                            AuditAirLogEntity log = new AuditAirLogEntity();
                        //                            log.AuditLogUid = Guid.NewGuid().ToString();
                        //                            log.AuditStatusUid = status.AuditStatusUid;
                        //                            log.AuditTime = DateTime.Now;
                        //                            log.Tstamp = Convert.ToDateTime(DataTime[i]);
                        //                            log.AuditType = "数据审核";
                        //                            log.PollutantCode = factor[j];
                        //                            log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                        //                            #region 原始值
                        //                            decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[j]);
                        //                            log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                        //                            #endregion
                        //                            if (operatorFlag.Equals("modify"))//修改
                        //                                log.AuditPollutantDataValue = NewData[i];
                        //                            else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                        //                                log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";
                        //                            log.OperationTypeEnum = OperationTypeEnum;
                        //                            log.OperationReason = reason;
                        //                            log.UserIP = UserIP;
                        //                            log.UserUid = UserUid;
                        //                            log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                        //                            log.CreatUser = CreatUser;
                        //                            log.CreatDateTime = DateTime.Now;
                        //                            log.UpdateUser = CreatUser;
                        //                            log.UpdateDateTime = DateTime.Now;

                        //                            //logAirRep.Add(log);
                        //                            logAirReps.Add(log);
                        //                            #endregion
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        #region 记录审核日志
                        //                        AuditAirInfectantByHourEntity auditData = auditAirHourRep.Retrieve(x => x.DataDateTime >= Convert.ToDateTime(DataTime[i]) && x.DataDateTime < Convert.ToDateTime(DataTime[i]).AddHours(1) && x.PollutantCode.Equals(factor[j]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();
                        //                        AuditAirLogEntity log = new AuditAirLogEntity();
                        //                        log.AuditLogUid = Guid.NewGuid().ToString();
                        //                        log.AuditStatusUid = status.AuditStatusUid;
                        //                        log.AuditTime = DateTime.Now;
                        //                        log.Tstamp = Convert.ToDateTime(DataTime[i]);
                        //                        log.AuditType = "数据审核";
                        //                        log.PollutantCode = factor[j];
                        //                        log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                        //                        #region 原始值
                        //                        decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[j]);
                        //                        log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                        //                        #endregion
                        //                        if (operatorFlag.Equals("modify"))//修改
                        //                            log.AuditPollutantDataValue = NewData[i];
                        //                        else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                        //                            log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";
                        //                        log.OperationTypeEnum = OperationTypeEnum;
                        //                        log.OperationReason = reason;
                        //                        log.UserIP = UserIP;
                        //                        log.UserUid = UserUid;
                        //                        log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                        //                        log.CreatUser = CreatUser;
                        //                        log.CreatDateTime = DateTime.Now;
                        //                        log.UpdateUser = CreatUser;
                        //                        log.UpdateDateTime = DateTime.Now;

                        //                        //logAirRep.Add(log);
                        //                        logAirReps.Add(log);
                        //                        #endregion
                        //                    }
                        //                }

                        //                
                        //                #endregion
                        #endregion
                        #region 修改审核数据、审核标记
                        if (operatorFlag.Equals("modify"))//修改
                        {
                            facAuditFlag = "MF";
                        }
                        else
                        {
                            if (entity != null && !entity.StatusIdentify.Equals(""))
                            {
                                if (entity.StatusIdentify.Equals("VA"))//有效标记位
                                {
                                    facAuditFlag = "VA";
                                }
                                else
                                    facAuditFlag = entity.StatusIdentify;
                            }
                        }
                        #endregion

                        insertSql = string.Format(@"INSERT INTO [Audit].[TB_AuditAirLog] 
  ([AuditLogUid]
      ,[AuditStatusUid]
      ,[tstamp]
      ,[AuditTime]
      ,[AuditType]
      ,[PollutantCode]
      ,[PollutantName]
      ,[AuditPollutantDataValue]
      ,[OperationTypeEnum]
      ,[OperationReason]
      ,[UserIP]
      ,[UserUid]
      ,[Description]
      ,[CreatUser]
      ,[CreatDateTime]
      ,[UpdateUser]
      ,[UpdateDateTime])
   VALUES ({0},'{1}','{2}','{3}','{4}','{5}','{6}',N'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')"
           , "(SELECT NEWID())", status.AuditStatusUid, Convert.ToDateTime(DataTime[i]).ToString("yyyy-MM-dd HH:mm:ss")
           , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "数据审核", factor[i], InstrumentName, InstrumentCode, OperationTypeEnum, reason
           , UserIP, UserUid, operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "")
           , CreatUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CreatUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        logs.Error(insertSql);
                        g_DatabaseHelper.ExecuteInsert(insertSql, "AMS_MonitoringBusinessConnection");


                        sql = string.Format(@"update [Audit].[TB_AuditAirInfectantByHour] set AuditFlag = '{0}',IsAudit = {1}
WHERE  ApplicationUid = 'airaaira-aira-aira-aira-airaairaaira' AND PointId = {2} AND DataDateTime>= '{3}' 
AND DataDateTime<'{4}'  AND PollutantCode in ({5})", facAuditFlag, 1, PointID[i], Convert.ToDateTime(DataTime[i]).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(DataTime[i]).AddHours(1).ToString("yyyy-MM-dd HH:mm:ss"), factorsql.TrimEnd(','));
                        logs.Error(sql);
                        g_DatabaseHelper.ExecuteScalar(sql, "AMS_MonitoringBusinessConnection");

                    }
                    else
                    {
                        #region 状态表为空

                        #region 新增状态记录
                        status = new AuditStatusForDayEntity();
                        status.AuditStatusUid = Guid.NewGuid().ToString();
                        status.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                        status.PointId = Convert.ToInt32(PointID[i]);
                        status.Date = Convert.ToDateTime(DataTime[i]);
                        status.CreatUser = CreatUser;
                        status.CreatDateTime = DateTime.Now;
                        status.UpdateUser = CreatUser;
                        status.UpdateDateTime = DateTime.Now;
                        auditStateRep.Add(status);
                        #endregion
                        for (int j = 0; j < factor.Length; j++)
                        {
                            AuditAirInfectantByHourEntity[] auditDatas = auditAirHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[j])).ToArray();

                            if (auditDatas.Length > 0)
                            {
                                foreach (AuditAirInfectantByHourEntity auditData in auditDatas)
                                {
                                    #region 记录审核日志
                                    AuditAirLogEntity log = new AuditAirLogEntity();
                                    log.AuditLogUid = Guid.NewGuid().ToString();
                                    log.AuditStatusUid = status.AuditStatusUid;
                                    log.AuditTime = DateTime.Now;
                                    log.Tstamp = Convert.ToDateTime(DataTime[i]);
                                    log.AuditType = "数据审核";
                                    log.PollutantCode = factor[j];
                                    log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                                    #region 原始值
                                    decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[j]);
                                    log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                                    #endregion
                                    if (operatorFlag.Equals("modify"))//修改
                                        log.AuditPollutantDataValue = NewData[i];
                                    else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                                        log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";
                                    log.OperationTypeEnum = OperationTypeEnum;
                                    log.OperationReason = reason;
                                    log.UserIP = UserIP;
                                    log.UserUid = UserUid;
                                    log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                                    log.CreatUser = CreatUser;
                                    log.CreatDateTime = DateTime.Now;
                                    log.UpdateUser = CreatUser;
                                    log.UpdateDateTime = DateTime.Now;

                                    //logAirRep.Add(log);
                                    logAirReps.Add(log);
                                    #endregion

                                    if (auditData == null)
                                    {
                                        AuditAirInfectantByHourEntity auditDataNew = new AuditAirInfectantByHourEntity();
                                        auditDataNew.AuditHourUid = Guid.NewGuid().ToString();
                                        auditDataNew.AuditStatusUid = status.AuditStatusUid;
                                        auditDataNew.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                                        auditDataNew.PointId = Convert.ToInt32(PointID[i]);
                                        auditDataNew.DataDateTime = Convert.ToDateTime(DataTime[i]);
                                        auditDataNew.PollutantCode = factor[j];
                                        auditDataNew.IsAudit = "1";
                                        if (operatorFlag.Equals("modify"))//修改
                                        {
                                            auditDataNew.PollutantValue = Convert.ToDecimal(NewData[i]) / 1000;
                                            auditDataNew.AuditFlag = "MF";
                                        }
                                        else
                                        {

                                            auditDataNew.AuditFlag = "," + entity.StatusIdentify;
                                        }
                                        //auditAirHourRep.Add(auditDataNew);
                                        //实体类数组
                                        entitiesAdd.Add(auditDataNew);
                                    }
                                    else
                                    {
                                        #region 修改审核数据、审核标记
                                        if (operatorFlag.Equals("modify"))//修改
                                        {
                                            auditData.PollutantValue = Convert.ToDecimal(NewData[i]) / 1000;
                                            auditData.AuditFlag = "MF";
                                        }
                                        else
                                        {
                                            if (entity != null && !entity.StatusIdentify.Equals(""))
                                            {
                                                if (entity.StatusIdentify.Equals("VA"))//有效标记位
                                                {
                                                    if (auditData.AuditFlag != null)
                                                        auditData.AuditFlag = auditData.AuditFlag.Replace("RM", "VA");
                                                    else
                                                        auditData.AuditFlag = ",VA";
                                                    if (!auditData.AuditFlag.Contains("VA"))
                                                        auditData.AuditFlag += ",VA";
                                                }
                                                else
                                                    auditData.AuditFlag = "," + entity.StatusIdentify;
                                            }
                                        }
                                        auditData.IsAudit = "1";
                                        //auditAirHourRep.Update(auditData);
                                        //实体类数组
                                        entitiesUpdate.Add(auditData);
                                        #endregion
                                    }
                                }
                            }
                            //如果没有数据
                            else
                            {
                                #region 记录审核日志
                                AuditAirInfectantByHourEntity auditData = auditAirHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[j]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();
                                AuditAirLogEntity log = new AuditAirLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.AuditTime = DateTime.Now;
                                log.Tstamp = Convert.ToDateTime(DataTime[i]);
                                log.AuditType = "数据审核";
                                log.PollutantCode = factor[j];
                                log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                                #region 原始值
                                decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[j]);
                                log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                                #endregion
                                if (operatorFlag.Equals("modify"))//修改
                                    log.AuditPollutantDataValue = NewData[i];
                                else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                                    log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";
                                log.OperationTypeEnum = OperationTypeEnum;
                                log.OperationReason = reason;
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                logAirRep.Add(log);
                                #endregion
                                AuditAirInfectantByHourEntity auditDataNew = new AuditAirInfectantByHourEntity();
                                auditDataNew.AuditHourUid = Guid.NewGuid().ToString();
                                auditDataNew.AuditStatusUid = status.AuditStatusUid;
                                auditDataNew.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                                auditDataNew.PointId = Convert.ToInt32(PointID[i]);
                                auditDataNew.DataDateTime = Convert.ToDateTime(DataTime[i]);
                                auditDataNew.PollutantCode = factor[j];
                                auditDataNew.IsAudit = "1";
                                if (operatorFlag.Equals("modify"))//修改
                                {
                                    auditDataNew.PollutantValue = Convert.ToDecimal(NewData[i]) / 1000;
                                    auditDataNew.AuditFlag = "MF";
                                }
                                else
                                {

                                    auditDataNew.AuditFlag = "," + entity.StatusIdentify;
                                }
                                //auditAirHourRep.Add(auditDataNew);
                                //实体类数组
                                entitiesAdd.Add(auditDataNew);
                            }
                        }

                        #endregion
                    }
                }
                
                if (entitiesAdd.Count > 0)
                {
                    auditAirHourRep.BatchAdd(entitiesAdd);
                }
                if (entitiesUpdate.Count > 0)
                {
                    auditAirHourRep.BatchUpdate(entitiesUpdate);
                }
                if (logAirReps.Count > 0)
                {
                    logAirRep.BatchAdd(logAirReps);
                }
                logs.Error("结束" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
                return true;
            }
            catch(Exception ex)
            {
                logs.Error(ex.ToString());
                return false;
            }
            
        }
        /// <summary>
        /// 修改审核数据、置为无效、有效并记录日志信息【空气】
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="factor"></param>
        /// <param name="DataTime"></param>
        /// <param name="NewData"></param>
        /// <returns></returns>
        public bool ModifyAirAuditDataSuper(string[] PointID, string[] factor, string[] DataTime, string[] NewData, string reason, string operatorFlag, string OperationTypeEnum, string UserIP, string UserUid, string CreatUser, string[] Pollutant)
        {
            string applicationUID = "airaaira-aira-aira-aira-airaairaaira";
            V_AuditOperatorSettingEntity entity = operatorService.GetAuditOperator(operatorFlag);

            for (int i = 0; i < PointID.Length; i++)
            {
                AuditStatusForDayEntity status = logService.RerieveAuditState(Convert.ToInt32(PointID[i]), Convert.ToDateTime(DataTime[i]).Date, Convert.ToDateTime(DataTime[i]).Date, applicationUID);
                if (status != null)
                {
                    #region 记录审核日志
                    AuditAirLogEntity log = new AuditAirLogEntity();
                    log.AuditLogUid = Guid.NewGuid().ToString();
                    log.AuditStatusUid = status.AuditStatusUid;
                    log.AuditTime = DateTime.Now;
                    log.Tstamp = Convert.ToDateTime(DataTime[i]);
                    log.AuditType = "数据审核";
                    log.PollutantCode = factor[i];
                    log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == Pollutant[i]).PollutantName;
                    #region 原始值
                    decimal sourceValue = auditDataService.GetSourcePolltantValueWeibo(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i], Pollutant[i]);
                    log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                    #endregion
                    if (operatorFlag.Equals("modify"))//修改
                        log.AuditPollutantDataValue = NewData[i];
                    log.OperationTypeEnum = OperationTypeEnum;
                    log.OperationReason = reason;
                    log.UserIP = UserIP;
                    log.UserUid = UserUid;
                    log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                    log.CreatUser = CreatUser;
                    log.CreatDateTime = DateTime.Now;
                    log.UpdateUser = CreatUser;
                    log.UpdateDateTime = DateTime.Now;
                    logAirRep.Add(log);
                    #endregion
                }
                else
                {
                    #region 状态表为空

                    #region 新增状态记录
                    status = new AuditStatusForDayEntity();
                    status.AuditStatusUid = Guid.NewGuid().ToString();
                    status.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                    status.PointId = Convert.ToInt32(PointID[i]);
                    status.Date = Convert.ToDateTime(DataTime[i]);
                    status.CreatUser = CreatUser;
                    status.CreatDateTime = DateTime.Now;
                    status.UpdateUser = CreatUser;
                    status.UpdateDateTime = DateTime.Now;
                    auditStateRep.Add(status);
                    #endregion

                    #region 记录审核日志
                    AuditAirLogEntity log = new AuditAirLogEntity();
                    log.AuditLogUid = Guid.NewGuid().ToString();
                    log.AuditStatusUid = status.AuditStatusUid;
                    log.AuditTime = DateTime.Now;
                    log.Tstamp = Convert.ToDateTime(DataTime[i]);
                    log.AuditType = "数据审核";
                    log.PollutantCode = factor[i];
                    log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                    #region 原始值
                    decimal sourceValue = auditDataService.GetSourcePolltantValueWeibo(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i], Pollutant[i]);
                    log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                    #endregion
                    if (operatorFlag.Equals("modify"))//修改
                        log.AuditPollutantDataValue = NewData[i];
                    log.OperationTypeEnum = OperationTypeEnum;
                    log.OperationReason = reason;
                    log.UserIP = UserIP;
                    log.UserUid = UserUid;
                    log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                    log.CreatUser = CreatUser;
                    log.CreatDateTime = DateTime.Now;
                    log.UpdateUser = CreatUser;
                    log.UpdateDateTime = DateTime.Now;
                    logAirRep.Add(log);
                    #endregion
                    #endregion
                }
            }

            string updateAuditTable = "update [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_Weibo] set [" + factor[0] + "]='" + NewData[0] + "' where pointid='" + PointID[0] + "' and pollutantcode='" + Pollutant[0] + "' and datetime='" + DataTime[0] + "'";
            DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
            g_DatabaseHelper.ExecuteNonQuery(updateAuditTable, "AMS_MonitoringBusinessConnection");

            return true;
        }

        /// <summary>
        /// 修改审核数据、置为无效、有效并记录日志信息【空气】
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="factor"></param>
        /// <param name="DataTime"></param>
        /// <param name="NewData"></param>
        /// <returns></returns>
        public bool ModifyAirAuditDataLijingpu(string[] PointID, string[] factor, string[] DataTime, string[] NewData, string reason, string operatorFlag, string OperationTypeEnum, string UserIP, string UserUid, string CreatUser, string[] Pollutant)
        {
            try
            {
                if (Pollutant.Contains("ASP"))
                {
                    string applicationUID = "airaaira-aira-aira-aira-airaairaaira";
                    V_AuditOperatorSettingEntity entity = operatorService.GetAuditOperator(operatorFlag);

                    for (int i = 0; i < PointID.Length; i++)
                    {
                        AuditStatusForDayEntity status = logService.RerieveAuditState(Convert.ToInt32(PointID[i]), Convert.ToDateTime(DataTime[i]).Date, Convert.ToDateTime(DataTime[i]).Date, applicationUID);
                        if (status != null)
                        {
                            #region 记录审核日志
                            AuditAirLogEntity log = new AuditAirLogEntity();
                            log.AuditLogUid = Guid.NewGuid().ToString();
                            log.AuditStatusUid = status.AuditStatusUid;
                            log.AuditTime = DateTime.Now;
                            log.Tstamp = Convert.ToDateTime(DataTime[i]);
                            log.AuditType = "数据审核";
                            log.PollutantCode = factor[i];
                            log.PollutantName = "ASP:" + factor[i];
                            #region 原始值
                            decimal sourceValue = auditDataService.GetSourcePolltantValueLijingpuAPS(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i], Pollutant[i]);
                            log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                            #endregion
                            if (operatorFlag.Equals("modify"))//修改
                                log.AuditPollutantDataValue = NewData[i];
                            log.OperationTypeEnum = OperationTypeEnum;
                            log.OperationReason = reason;
                            log.UserIP = UserIP;
                            log.UserUid = UserUid;
                            log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                            log.CreatUser = CreatUser;
                            log.CreatDateTime = DateTime.Now;
                            log.UpdateUser = CreatUser;
                            log.UpdateDateTime = DateTime.Now;
                            logAirRep.Add(log);
                            #endregion
                        }
                        else
                        {
                            #region 状态表为空

                            #region 新增状态记录
                            status = new AuditStatusForDayEntity();
                            status.AuditStatusUid = Guid.NewGuid().ToString();
                            status.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                            status.PointId = Convert.ToInt32(PointID[i]);
                            status.Date = Convert.ToDateTime(DataTime[i]);
                            status.CreatUser = CreatUser;
                            status.CreatDateTime = DateTime.Now;
                            status.UpdateUser = CreatUser;
                            status.UpdateDateTime = DateTime.Now;
                            auditStateRep.Add(status);
                            #endregion

                            #region 记录审核日志
                            AuditAirLogEntity log = new AuditAirLogEntity();
                            log.AuditLogUid = Guid.NewGuid().ToString();
                            log.AuditStatusUid = status.AuditStatusUid;
                            log.AuditTime = DateTime.Now;
                            log.Tstamp = Convert.ToDateTime(DataTime[i]);
                            log.AuditType = "数据审核";
                            log.PollutantCode = factor[i];
                            log.PollutantName = "ASP:" + factor[i];
                            #region 原始值
                            decimal sourceValue = auditDataService.GetSourcePolltantValueLijingpuAPS(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i], Pollutant[i]);
                            log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                            #endregion
                            if (operatorFlag.Equals("modify"))//修改
                                log.AuditPollutantDataValue = NewData[i];
                            log.OperationTypeEnum = OperationTypeEnum;
                            log.OperationReason = reason;
                            log.UserIP = UserIP;
                            log.UserUid = UserUid;
                            log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                            log.CreatUser = CreatUser;
                            log.CreatDateTime = DateTime.Now;
                            log.UpdateUser = CreatUser;
                            log.UpdateDateTime = DateTime.Now;
                            logAirRep.Add(log);
                            #endregion
                            #endregion
                        }
                    }


                    string updateAuditTable = "update [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_L] set [" + factor[0] + "]='" + NewData[0] + "' where pointid='" + PointID[0] + "' and datetime='" + DataTime[0] + "'";
                    DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
                    g_DatabaseHelper.ExecuteNonQuery(updateAuditTable, "AMS_MonitoringBusinessConnection");
                }
                else
                {
                    string applicationUID = "airaaira-aira-aira-aira-airaairaaira";
                    V_AuditOperatorSettingEntity entity = operatorService.GetAuditOperator(operatorFlag);

                    for (int i = 0; i < PointID.Length; i++)
                    {
                        AuditStatusForDayEntity status = logService.RerieveAuditState(Convert.ToInt32(PointID[i]), Convert.ToDateTime(DataTime[i]).Date, Convert.ToDateTime(DataTime[i]).Date, applicationUID);
                        if (status != null)
                        {
                            #region 记录审核日志
                            AuditAirLogEntity log = new AuditAirLogEntity();
                            log.AuditLogUid = Guid.NewGuid().ToString();
                            log.AuditStatusUid = status.AuditStatusUid;
                            log.AuditTime = DateTime.Now;
                            log.Tstamp = Convert.ToDateTime(DataTime[i]);
                            log.AuditType = "数据审核";
                            log.PollutantCode = factor[i];
                            log.PollutantName = "ASP:" + factor[i];
                            #region 原始值
                            decimal sourceValue = auditDataService.GetSourcePolltantValueLijingpuL72(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i], Pollutant[i]);
                            log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                            #endregion
                            if (operatorFlag.Equals("modify"))//修改
                                log.AuditPollutantDataValue = NewData[i];
                            log.OperationTypeEnum = OperationTypeEnum;
                            log.OperationReason = reason;
                            log.UserIP = UserIP;
                            log.UserUid = UserUid;
                            log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                            log.CreatUser = CreatUser;
                            log.CreatDateTime = DateTime.Now;
                            log.UpdateUser = CreatUser;
                            log.UpdateDateTime = DateTime.Now;
                            logAirRep.Add(log);
                            #endregion
                        }
                        else
                        {
                            #region 状态表为空

                            #region 新增状态记录
                            status = new AuditStatusForDayEntity();
                            status.AuditStatusUid = Guid.NewGuid().ToString();
                            status.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                            status.PointId = Convert.ToInt32(PointID[i]);
                            status.Date = Convert.ToDateTime(DataTime[i]);
                            status.CreatUser = CreatUser;
                            status.CreatDateTime = DateTime.Now;
                            status.UpdateUser = CreatUser;
                            status.UpdateDateTime = DateTime.Now;
                            auditStateRep.Add(status);
                            #endregion

                            #region 记录审核日志
                            AuditAirLogEntity log = new AuditAirLogEntity();
                            log.AuditLogUid = Guid.NewGuid().ToString();
                            log.AuditStatusUid = status.AuditStatusUid;
                            log.AuditTime = DateTime.Now;
                            log.Tstamp = Convert.ToDateTime(DataTime[i]);
                            log.AuditType = "数据审核";
                            log.PollutantCode = factor[i];
                            log.PollutantName = "ASP:" + factor[i];
                            #region 原始值
                            decimal sourceValue = auditDataService.GetSourcePolltantValueLijingpuL72(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i], Pollutant[i]);
                            log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                            #endregion
                            if (operatorFlag.Equals("modify"))//修改
                                log.AuditPollutantDataValue = NewData[i];
                            log.OperationTypeEnum = OperationTypeEnum;
                            log.OperationReason = reason;
                            log.UserIP = UserIP;
                            log.UserUid = UserUid;
                            log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                            log.CreatUser = CreatUser;
                            log.CreatDateTime = DateTime.Now;
                            log.UpdateUser = CreatUser;
                            log.UpdateDateTime = DateTime.Now;
                            logAirRep.Add(log);
                            #endregion
                            #endregion
                        }
                    }

                    string updateAuditTable = "update [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_M] set [" + factor[0] + "]='" + NewData[0] + "' where pointid='" + PointID[0] + "' and datetime='" + DataTime[0] + "'";
                    DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
                    g_DatabaseHelper.ExecuteNonQuery(updateAuditTable, "AMS_MonitoringBusinessConnection");
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 修改审核数据、置为无效、有效并记录日志信息【地表水】
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="factor"></param>
        /// <param name="DataTime"></param>
        /// <param name="NewData"></param>
        /// <returns></returns>
        public bool ModifyWaterAuditData(string applicationUID, string[] PointID, string[] factor, string[] DataTime, string[] NewData, string reason, string operatorFlag, string OperationTypeEnum, string UserIP, string UserUid, string CreatUser)
        {
            V_AuditOperatorSettingEntity entity = operatorService.GetAuditOperator(operatorFlag);

            for (int i = 0; i < PointID.Length; i++)
            {
                AuditStatusForDayEntity status = logService.RerieveAuditState(Convert.ToInt32(PointID[i]), Convert.ToDateTime(DataTime[i]).Date, Convert.ToDateTime(DataTime[i]).Date, applicationUID);
                if (status != null)
                {
                    #region 状态表不为空
                    AuditWaterInfectantByHourEntity auditData = waterHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[i]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();

                    #region 记录审核日志
                    AuditWaterLogEntity log = new AuditWaterLogEntity();
                    log.AuditLogUid = Guid.NewGuid().ToString();
                    log.AuditStatusUid = status.AuditStatusUid;
                    log.Tstamp = auditData.DataDateTime;
                    log.AuditType = "数据审核";
                    log.AuditTime = DateTime.Now;
                    log.PollutantCode = factor[i];
                    log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                    #region 原始值
                    decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i]);
                    log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                    #endregion
                    if (operatorFlag.Equals("modify"))//修改
                        log.AuditPollutantDataValue = NewData[i];
                    else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                        log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";

                    log.OperationTypeEnum = OperationTypeEnum;
                    log.OperationReason = reason;
                    log.UserIP = UserIP;
                    log.UserUid = UserUid;
                    log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                    log.CreatUser = CreatUser;
                    log.CreatDateTime = DateTime.Now;
                    log.UpdateUser = CreatUser;
                    log.UpdateDateTime = DateTime.Now;
                    logWaterRep.Add(log);
                    #endregion

                    if (auditData == null)
                    {
                        auditData = new AuditWaterInfectantByHourEntity();
                        auditData.AuditHourUid = Guid.NewGuid().ToString();
                        auditData.AuditStatusUid = status.AuditStatusUid;
                        auditData.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Water);
                        auditData.PointId = Convert.ToInt32(PointID[i]);
                        auditData.DataDateTime = Convert.ToDateTime(DataTime[i]);
                        auditData.PollutantCode = factor[i];
                        auditData.IsAudit = "1";
                        if (operatorFlag.Equals("modify"))//修改
                        {
                            auditData.PollutantValue = Convert.ToDecimal(NewData[i]);
                            auditData.AuditFlag = "MF";
                        }
                        else
                        {
                            auditData.AuditFlag = "," + entity.StatusIdentify;
                        }
                        waterHourRep.Add(auditData);
                    }
                    else
                    {
                        #region 修改审核数据、审核标记
                        if (operatorFlag.Equals("modify"))//修改
                        {
                            auditData.PollutantValue = Convert.ToDecimal(NewData[i]);
                            auditData.AuditFlag = "MF";
                        }
                        else
                        {
                            if (entity != null && !entity.StatusIdentify.Equals(""))
                            {
                                if (entity.StatusIdentify.Equals("VA"))//有效标记位
                                {
                                    if (auditData.AuditFlag != null)
                                        auditData.AuditFlag = auditData.AuditFlag.Replace("RM", "VA");
                                    else
                                        auditData.AuditFlag = ",VA";
                                    if (!auditData.AuditFlag.Contains("VA"))
                                        auditData.AuditFlag += ",VA";
                                }
                                else
                                    auditData.AuditFlag = "," + entity.StatusIdentify;
                            }
                        }
                        auditData.IsAudit = "1";
                        waterHourRep.Update(auditData);
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region 状态表为空

                    #region 新增状态记录
                    status = new AuditStatusForDayEntity();
                    status.AuditStatusUid = Guid.NewGuid().ToString();
                    status.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Water);
                    status.PointId = Convert.ToInt32(PointID[i]);
                    status.Date = Convert.ToDateTime(DataTime[i]);
                    status.CreatUser = CreatUser;
                    status.CreatDateTime = DateTime.Now;
                    status.UpdateUser = CreatUser;
                    status.UpdateDateTime = DateTime.Now;
                    auditStateRep.Add(status);
                    #endregion

                    AuditWaterInfectantByHourEntity auditData = waterHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[i]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();

                    #region 记录审核日志
                    AuditWaterLogEntity log = new AuditWaterLogEntity();
                    log.AuditLogUid = Guid.NewGuid().ToString();
                    log.AuditStatusUid = status.AuditStatusUid;
                    log.Tstamp = auditData.DataDateTime;
                    log.AuditType = "数据审核";
                    log.AuditTime = DateTime.Now;
                    log.PollutantCode = factor[i];
                    log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                    #region 原始值
                    decimal sourceValue = auditDataService.GetSourcePolltantValue(applicationUID, Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i]);
                    log.SourcePollutantDataValue = sourceValue == -99999 ? "" : sourceValue.ToString();
                    #endregion
                    if (operatorFlag.Equals("modify"))//修改
                        log.AuditPollutantDataValue = NewData[i];
                    else if (entity != null && !entity.StatusIdentify.Equals(""))//标记位操作写入日志
                        log.AuditPollutantDataValue = auditData != null && auditData.PollutantValue != null ? auditData.PollutantValue.ToString() + "(" + entity.StatusIdentify + ")" : "(" + entity.StatusIdentify + ")";

                    log.OperationTypeEnum = OperationTypeEnum;
                    log.OperationReason = reason;
                    log.UserIP = UserIP;
                    log.UserUid = UserUid;
                    log.Description = operatorFlag.Equals("modify") ? "数据修改" : (entity != null ? entity.OperatorName : "");
                    log.CreatUser = CreatUser;
                    log.CreatDateTime = DateTime.Now;
                    log.UpdateUser = CreatUser;
                    log.UpdateDateTime = DateTime.Now;
                    logWaterRep.Add(log);
                    #endregion

                    if (auditData == null)
                    {
                        auditData = new AuditWaterInfectantByHourEntity();
                        auditData.AuditHourUid = Guid.NewGuid().ToString();
                        auditData.AuditStatusUid = status.AuditStatusUid;
                        auditData.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Water);
                        auditData.PointId = Convert.ToInt32(PointID[i]);
                        auditData.DataDateTime = Convert.ToDateTime(DataTime[i]);
                        auditData.PollutantCode = factor[i];
                        auditData.IsAudit = "1";
                        if (operatorFlag.Equals("modify"))//修改
                        {
                            auditData.PollutantValue = Convert.ToDecimal(NewData[i]);
                            auditData.AuditFlag = "MF";
                        }
                        else
                        {
                            auditData.AuditFlag = "," + entity.StatusIdentify;
                        }
                        waterHourRep.Add(auditData);
                    }
                    else
                    {
                        #region 修改审核数据、审核标记
                        if (operatorFlag.Equals("modify"))//修改
                        {
                            auditData.PollutantValue = Convert.ToDecimal(NewData[i]);
                            auditData.AuditFlag = "MF";
                        }
                        else
                        {
                            if (entity != null && !entity.StatusIdentify.Equals(""))
                            {
                                if (entity.StatusIdentify.Equals("VA"))//有效标记位
                                {
                                    if (auditData.AuditFlag != null)
                                        auditData.AuditFlag = auditData.AuditFlag.Replace("RM", "VA");
                                    else
                                        auditData.AuditFlag = ",VA";
                                    if (!auditData.AuditFlag.Contains("VA"))
                                        auditData.AuditFlag += ",VA";
                                }
                                else
                                    auditData.AuditFlag = "," + entity.StatusIdentify;
                            }
                        }
                        auditData.IsAudit = "1";
                        waterHourRep.Update(auditData);
                        #endregion
                    }
                    #endregion
                }
            }

            return true;
        }
        #endregion

        #region 还原数据【水、气】
        /// <summary>
        /// 数据还原
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="PointID"></param>
        /// <param name="factor"></param>
        /// <param name="DataTime"></param>
        /// <param name="reason"></param>
        /// <param name="OperationTypeEnum"></param>
        /// <param name="UserIP"></param>
        /// <param name="UserUid"></param>
        /// <param name="CreatUser"></param>
        /// <returns></returns>
        public bool RestoreAuditData(string applicationUID, string[] PointID, string[] factor, string[] DataTime, string reason, string OperationTypeEnum, string UserIP, string UserUid, string CreatUser)
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                return RestoreAirAuditData(PointID, factor, DataTime, reason, OperationTypeEnum, UserIP, UserUid, CreatUser);
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                return RestoreWaterAuditData(PointID, factor, DataTime, reason, OperationTypeEnum, UserIP, UserUid, CreatUser);
            return true;
        }

        /// <summary>
        /// 数据还原【空气】
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="factor"></param>
        /// <param name="DataTime"></param>
        /// <param name="reason"></param>
        /// <param name="operatorFlag"></param>
        /// <param name="OperationTypeEnum"></param>
        /// <param name="UserIP"></param>
        /// <param name="UserUid"></param>
        /// <param name="CreatUser"></param>
        /// <returns></returns>
        public bool RestoreAirAuditData(string[] PointID, string[] factor, string[] DataTime, string reason, string OperationTypeEnum, string UserIP, string UserUid, string CreatUser)
        {
            //新建数组
            List<AuditAirInfectantByHourEntity> entitiesAdd = new List<AuditAirInfectantByHourEntity>();
            List<AuditAirInfectantByHourEntity> entitiesUpdate = new List<AuditAirInfectantByHourEntity>();
            List<AuditAirLogEntity> logAirReps = new List<AuditAirLogEntity>();
            string sql = string.Empty;
            string insertSql = string.Empty;
            string deleteSql = string.Empty;
            string insertDataSql = string.Empty;
            string updateDataSql = string.Empty;
            string factorsql = string.Empty;
            string factorAll = string.Empty;
            string facAuditFlag = string.Empty;
            string InstrumentSql = string.Empty;
            InstrumentSql = string.Format(@"select InstrumentName FROM V_Point_InstrumentChannels where PollutantCode='{0}'", factor[0]);
            string InstrumentName = g_DatabaseHelper.ExecuteDataTable(InstrumentSql, "AMS_BaseDataConnection").Rows[0][0].ToString();

            //int i = 0;
            for (int i = 0; i < PointID.Length; i++)
            {
                AuditStatusForDayEntity status = auditStateRep.Retrieve(x => x.PointId == Convert.ToInt32(PointID[i]) && x.Date.Value.Date == Convert.ToDateTime(DataTime[i]).Date).FirstOrDefault();
                for (int j = 0; j < factor.Length; j++)
                {
                    if (status != null)
                    {
                        factorsql += "'" + factor[j] + "',";
                        //object aa = Infec60AirRep.RetrieveFirstOrDefault(x => x.Tstamp == Convert.ToDateTime(DataTime[i]) && x.PointId == Convert.ToInt32(PointID[i]));
                        ////原方法
                        //AuditAirInfectantByHourEntity auditData = auditAirHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[i]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();
                        DomainModel.AirAutoMonitoring.InfectantBy60Entity infec60 = auditDataService.InfectantBy60AirInfo(Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[j]);
                        //修改后
                        #region 注释
                        //AuditAirInfectantByHourEntity[] auditDatas = auditAirHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PointId.Equals(PointID[i]) && x.PollutantCode.Equals(factor[j])).ToArray();

                        //if (auditDatas.Length > 0)
                        //{
                        //    foreach (AuditAirInfectantByHourEntity auditData in auditDatas)
                        //    {
                        //        #region 修改审核数据、审核标记
                        //        if (infec60 != null)
                        //        {

                        //            if (auditData == null)
                        //            {
                        //                //从原始数据或报表数据插入 
                        //                AuditAirInfectantByHourEntity auditDataNew = new AuditAirInfectantByHourEntity();
                        //                auditDataNew.AuditHourUid = Guid.NewGuid().ToString();
                        //                auditDataNew.AuditStatusUid = status.AuditStatusUid;
                        //                auditDataNew.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                        //                auditDataNew.PointId = Convert.ToInt32(PointID[i]);
                        //                auditDataNew.DataDateTime = Convert.ToDateTime(DataTime[i]);
                        //                auditDataNew.DataFlag = infec60.Status;
                        //                auditDataNew.AuditFlag = infec60.Mark;
                        //                auditDataNew.PollutantCode = factor[j];
                        //                auditDataNew.PollutantValue = infec60.PollutantValue;
                        //                auditDataNew.IsAudit = "0";
                        //                //auditAirHourRep.Add(auditDataNew);
                        //                //存入数组
                        //                entitiesAdd.Add(auditDataNew);
                        //            }
                        //            else
                        //            {
                        //                //从原始数据或报表数据更新
                        //                auditData.DataFlag = infec60.Status;
                        //                auditData.AuditFlag = infec60.Mark;
                        //                auditData.IsAudit = "0";
                        //                //auditAirHourRep.Update(auditData);
                        //                //存入数组
                        //                entitiesUpdate.Add(auditData);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (auditData != null)
                        //            {
                        //                //从原始数据或报表数据更新
                        //                auditData.DataFlag = string.Empty;
                        //                auditData.AuditFlag = string.Empty;
                        //                auditData.PollutantValue = null;
                        //                auditData.IsAudit = "0";
                        //                //auditAirHourRep.Update(auditData);
                        //                //存入数组
                        //                entitiesUpdate.Add(auditData);
                        //            }
                        //        }
                        //        #endregion

                        //        #region 记录审核日志（旧）现不使用
                        //        //AuditAirLogEntity log = new AuditAirLogEntity();
                        //        //log.AuditLogUid = Guid.NewGuid().ToString();
                        //        //log.AuditStatusUid = status.AuditStatusUid;
                        //        //log.Tstamp = auditData.DataDateTime;
                        //        //log.AuditType = "数据审核";
                        //        //log.AuditTime = DateTime.Now;
                        //        //log.PollutantCode = factor[j];
                        //        //log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                        //        //log.SourcePollutantDataValue = auditData.PollutantValue.ToString();
                        //        //log.AuditPollutantDataValue = auditData.PollutantValue.ToString();
                        //        //log.OperationTypeEnum = OperationTypeEnum;
                        //        //log.OperationReason = reason;
                        //        //log.UserIP = UserIP;
                        //        //log.UserUid = UserUid;
                        //        //log.Description = "";
                        //        //log.CreatUser = CreatUser;
                        //        //log.CreatDateTime = DateTime.Now;
                        //        //log.UpdateUser = CreatUser;
                        //        //log.UpdateDateTime = DateTime.Now;
                        //        ////logAirRep.Add(log);
                        //        ////存入数组
                        //        //logAirReps.Add(log);
                        //        #endregion
                        //    }
                        //}
                        //else
                        //{
                        //    #region 修改审核数据、审核标记
                        //    //AuditAirInfectantByHourEntity auditData = auditAirHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[i]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();

                        //    AuditAirInfectantByHourEntity auditData = auditAirHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[i])).FirstOrDefault();

                        //    if (infec60 != null)
                        //    {
                        //        if (auditData == null)
                        //        {
                        //            //从原始数据或报表数据插入 
                        //            AuditAirInfectantByHourEntity auditDataNew = new AuditAirInfectantByHourEntity();
                        //            auditDataNew.AuditHourUid = Guid.NewGuid().ToString();
                        //            auditDataNew.AuditStatusUid = status.AuditStatusUid;
                        //            auditDataNew.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Air);
                        //            auditDataNew.PointId = Convert.ToInt32(PointID[i]);
                        //            auditDataNew.DataDateTime = Convert.ToDateTime(DataTime[i]);
                        //            auditDataNew.DataFlag = infec60.Status;
                        //            auditDataNew.AuditFlag = infec60.Mark;
                        //            auditDataNew.PollutantCode = factor[j];
                        //            auditDataNew.PollutantValue = infec60.PollutantValue;
                        //            auditDataNew.IsAudit = "0";
                        //            //auditAirHourRep.Add(auditDataNew);
                        //            //存入数组
                        //            entitiesAdd.Add(auditDataNew);
                        //        }
                        //        else
                        //        {
                        //            //从原始数据或报表数据更新
                        //            auditData.DataFlag = infec60.Status;
                        //            auditData.AuditFlag = infec60.Mark;
                        //            auditData.IsAudit = "0";
                        //            //auditAirHourRep.Update(auditData);
                        //            //存入数组
                        //            entitiesUpdate.Add(auditData);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (auditData != null)
                        //        {
                        //            //从原始数据或报表数据更新
                        //            auditData.DataFlag = string.Empty;
                        //            auditData.AuditFlag = string.Empty;
                        //            auditData.PollutantValue = null;
                        //            auditData.IsAudit = "0";
                        //            //auditAirHourRep.Update(auditData);
                        //            //存入数组
                        //            entitiesUpdate.Add(auditData);
                        //        }
                        //    }
                        //    #endregion

                        //    #region 记录审核日志(旧)现不使用
                        //    //AuditAirLogEntity log = new AuditAirLogEntity();
                        //    //log.AuditLogUid = Guid.NewGuid().ToString();
                        //    //log.AuditStatusUid = status.AuditStatusUid;
                        //    //log.Tstamp = auditData.DataDateTime;
                        //    //log.AuditType = "数据审核";
                        //    //log.AuditTime = DateTime.Now;
                        //    //log.PollutantCode = factor[j];
                        //    //log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                        //    //log.SourcePollutantDataValue = auditData.PollutantValue.ToString();
                        //    //log.AuditPollutantDataValue = auditData.PollutantValue.ToString();
                        //    //log.OperationTypeEnum = OperationTypeEnum;
                        //    //log.OperationReason = reason;
                        //    //log.UserIP = UserIP;
                        //    //log.UserUid = UserUid;
                        //    //log.Description = "";
                        //    //log.CreatUser = CreatUser;
                        //    //log.CreatDateTime = DateTime.Now;
                        //    //log.UpdateUser = CreatUser;
                        //    //log.UpdateDateTime = DateTime.Now;
                        //    ////logAirRep.Add(log);
                        //    ////存入数组
                        //    //logAirReps.Add(log);
                        //    #endregion
                        //}
                        #endregion
                        if (infec60 != null)
                        {
                            insertDataSql += string.Format(@"insert into [Audit].[TB_AuditAirInfectantByHour]
                                ([AuditHourUid],[AuditStatusUid],[ApplicationUid],[PointId] ,[DataDateTime],[dataFlag],[AuditFlag]
                                ,[PollutantCode],[PollutantValue],[IsAudit],[CreatUser],[CreatDateTime])
                                values
                                ({0},'{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')  
                                ", "(SELECT NEWID())", status.AuditStatusUid
                                    , EnumMapping.GetApplicationValue(ApplicationValue.Air), Convert.ToInt32(PointID[i]), Convert.ToDateTime(DataTime[i]).ToString("yyyy-MM-dd HH:00:00")
                                    , infec60.Status, infec60.Mark, factor[j], infec60.PollutantValue, "0", CreatUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            insertDataSql += string.Format(@"insert into [Audit].[TB_AuditAirInfectantByHour]
                                ([AuditHourUid],[AuditStatusUid],[ApplicationUid],[PointId] ,[DataDateTime],[dataFlag],[AuditFlag]
                                ,[PollutantCode],[PollutantValue],[IsAudit],[CreatUser],[CreatDateTime])
                                values
                                ({0},'{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')  
                                ", "(SELECT NEWID())", status.AuditStatusUid
                                    , EnumMapping.GetApplicationValue(ApplicationValue.Air), Convert.ToInt32(PointID[i]), Convert.ToDateTime(DataTime[i]).ToString("yyyy-MM-dd HH:00:00")
                                    , "", "", factor[j], "", "0", CreatUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                }
                deleteSql = string.Format(@"delete from [Audit].[TB_AuditAirInfectantByHour] where PointId={0} and PollutantCode in({1})
                and DataDateTime>='{2}' and DataDateTime<'{3}'", PointID[i], factorsql.TrimEnd(',')
                    , Convert.ToDateTime(DataTime[i]).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(DataTime[i]).AddHours(1).ToString("yyyy-MM-dd HH:mm:ss"));
                g_DatabaseHelper.ExecuteScalar(deleteSql, "AMS_MonitoringBusinessConnection");
                if (insertDataSql.Length > 0)
                {
                    g_DatabaseHelper.ExecuteScalar(insertDataSql, "AMS_MonitoringBusinessConnection");
                }
                //if (updateDataSql.Length>0)
                //{
                //    g_DatabaseHelper.ExecuteScalar(updateDataSql, "AMS_MonitoringBusinessConnection");
                //}

                logs.Error("新增日志记录开始" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
                insertSql = string.Format(@"INSERT INTO [Audit].[TB_AuditAirLog] 
  ([AuditLogUid]
      ,[AuditStatusUid]
      ,[tstamp]
      ,[AuditTime]
      ,[AuditType]
      ,[PollutantCode]
      ,[PollutantName]
      ,[AuditPollutantDataValue]
      ,[OperationTypeEnum]
      ,[OperationReason]
      ,[UserIP]
      ,[UserUid]
      ,[Description]
      ,[CreatUser]
      ,[CreatDateTime]
      ,[UpdateUser]
      ,[UpdateDateTime])
   VALUES ({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')"
           , "(SELECT NEWID())", status.AuditStatusUid, Convert.ToDateTime(DataTime[i]).ToString("yyyy-MM-dd HH:mm:ss")
           , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "数据审核", factor[i], InstrumentName,"所有因子", OperationTypeEnum, reason
           , UserIP, UserUid, reason
           , CreatUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CreatUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                logs.Error(insertSql);
                g_DatabaseHelper.ExecuteInsert(insertSql, "AMS_MonitoringBusinessConnection");
            }
            
            
            logs.Error("新增日志记录结束" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
            logs.Error("数据库链接" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));

            if (logAirReps.Count > 0)
            {
                logAirRep.BatchAdd(logAirReps);
            }
            if (entitiesAdd.Count > 0)
            {
                auditAirHourRep.BatchAdd(entitiesAdd);
            }
            if (entitiesUpdate.Count > 0)
            {
                auditAirHourRep.BatchUpdate(entitiesUpdate);
            }
            logs.Error("结束" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
            #region 重新着色
            //Thread t1 = new Thread(() =>
            //{
            //    for (int j = 0; j < factor.Length; j++)
            //    {
            //        if (status != null)
            //        {
            //            preService.PreData_SetColor(EnumMapping.GetApplicationValue(ApplicationValue.Air), PointID, factor[j], Convert.ToDateTime(DataTime[i]), Convert.ToDateTime(DataTime[i]), 0);
            //        }
            //    }
            //});
            //t1.IsBackground = true;
            //t1.Start();
            #endregion

            return true;
        }

        /// <summary>
        /// 数据还原【地表水】
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="factor"></param>
        /// <param name="DataTime"></param>
        /// <param name="reason"></param>
        /// <param name="OperationTypeEnum"></param>
        /// <param name="UserIP"></param>
        /// <param name="UserUid"></param>
        /// <param name="CreatUser"></param>
        /// <returns></returns>
        public bool RestoreWaterAuditData(string[] PointID, string[] factor, string[] DataTime, string reason, string OperationTypeEnum, string UserIP, string UserUid, string CreatUser)
        {
            for (int i = 0; i < PointID.Length; i++)
            {
                AuditStatusForDayEntity status = auditStateRep.Retrieve(x => x.PointId == Convert.ToInt32(PointID[i]) && x.Date.Value.Date == Convert.ToDateTime(DataTime[i]).Date).FirstOrDefault();
                if (status != null)
                {
                    AuditWaterInfectantByHourEntity auditData = waterHourRep.Retrieve(x => x.DataDateTime == Convert.ToDateTime(DataTime[i]) && x.PollutantCode.Equals(factor[i]) && x.AuditStatusUid.Equals(status.AuditStatusUid)).FirstOrDefault();
                    DomainModel.WaterAutoMonitoring.InfectantBy60Entity infec60 = auditDataService.InfectantBy60WaterInfo(Convert.ToDateTime(DataTime[i]), Convert.ToInt32(PointID[i]), factor[i]);
                    #region 修改审核数据、审核标记
                    if (infec60 != null)
                    {
                        if (auditData == null)
                        {

                            //从原始数据或报表数据插入 
                            auditData = new AuditWaterInfectantByHourEntity();
                            auditData.AuditHourUid = Guid.NewGuid().ToString();
                            auditData.AuditStatusUid = status.AuditStatusUid;
                            auditData.ApplicationUid = EnumMapping.GetApplicationValue(ApplicationValue.Water);
                            auditData.PointId = Convert.ToInt32(PointID[i]);
                            auditData.DataDateTime = Convert.ToDateTime(DataTime[i]);
                            auditData.DataFlag = infec60.Status;
                            auditData.AuditFlag = "";
                            auditData.PollutantCode = factor[i];
                            auditData.PollutantValue = infec60.PollutantValue;
                            auditData.IsAudit = "0";
                            waterHourRep.Add(auditData);
                        }
                        else
                        {
                            //从原始数据或报表数据更新
                            auditData.DataFlag = infec60.Status;
                            auditData.AuditFlag = "";
                            auditData.PollutantValue = infec60.PollutantValue;
                            auditData.IsAudit = "0";
                            waterHourRep.Update(auditData);
                        }
                    }
                    else
                    {
                        if (auditData != null)
                        {
                            //从原始数据或报表数据更新
                            auditData.DataFlag = string.Empty;
                            auditData.AuditFlag = string.Empty;
                            auditData.PollutantValue = null;
                            auditData.IsAudit = "0";
                            waterHourRep.Update(auditData);
                        }
                    }
                    #endregion

                    #region 重新着色
                    #endregion

                    #region 记录审核日志
                    AuditWaterLogEntity log = new AuditWaterLogEntity();
                    log.AuditLogUid = Guid.NewGuid().ToString();
                    log.AuditStatusUid = status.AuditStatusUid;
                    log.Tstamp = auditData.DataDateTime;
                    log.AuditType = "数据审核";
                    log.AuditTime = DateTime.Now;
                    log.PollutantCode = factor[i];
                    log.PollutantName = pollutantCodeRep.RetrieveFirstOrDefault(x => x.PollutantCode == log.PollutantCode).PollutantName;
                    log.SourcePollutantDataValue = auditData.PollutantValue.ToString();
                    log.AuditPollutantDataValue = auditData.PollutantValue.ToString();
                    log.OperationTypeEnum = OperationTypeEnum;
                    log.OperationReason = reason;
                    log.UserIP = UserIP;
                    log.UserUid = UserUid;
                    log.Description = "";
                    log.CreatUser = CreatUser;
                    log.CreatDateTime = DateTime.Now;
                    log.UpdateUser = CreatUser;
                    log.UpdateDateTime = DateTime.Now;
                    logWaterRep.Add(log);
                    #endregion
                }
            }

            return true;
        }
        #endregion

        /// <summary>
        /// 根据仪器uid获取因子类型数组
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string[] getFactors(string InsId, string portid, string applicationUID, string UserUid, string PointType)
        {
            AuditMonitoringPointPollutantService pollutantService = new AuditMonitoringPointPollutantService();//因子接口
            string uid = string.Empty;
            switch (InsId)
            {
                case "56dd6e9b-4c8f-4e67-a70f-b6a277cb44d7":
                    uid = "e5b6d666-24d1-473a-b15a-33a36245d44f";
                    break;
                case "6e4aa38a-f68b-490b-9cd7-3b92c7805c2d":
                    uid = "14b38adf-d899-4362-99ff-6a9e9dd35485";
                    break;
                case "3745f768-a789-4d58-9578-9e41fde5e5f0":
                    uid = "fbc6dyta-d06c-678g-b5y0-89b12be5bda3";
                    break;
                case "1589850e-0df1-4d9d-b508-4a77def158ba":
                    uid = "5575a0e1-d948-4566-9dcd-4b4767688add";
                    break;
                case "a6b3d80c-8281-4bc6-af47-f0febf568a5c":
                    uid = "59f02681-093f-48f0-9cac-ac59acd7038f";
                    break;
                case "da4f968f-cc6e-4fec-8219-6167d100499d":
                    uid = "aabe91e0-29a4-427c-becc-0b29f1224422";
                    break;
                case "9ef57f3c-8cce-4fe3-980f-303bbcfde260":
                    uid = "da92c7c1-4932-4007-a6d5-2866aa8c63f1";
                    break;
                case "6cd5c158-8a79-4540-a8b1-2a062759c9a0":
                    uid = "339B72C4-7295-4D31-B9EB-23342CB3697E";
                    break;
            }
            //List<PointPollutantInfo> PointPollutantInfoss = pollutantService.RetrieveSiteMapPollutantList(Convert.ToInt32(portid), applicationUID, UserUid).Where(p => p.PID != "").ToList<PointPollutantInfo>();
            List<PointPollutantInfo> PointPollutantInfos = pollutantService.RetrieveSiteMapPollutantList(Convert.ToInt32(portid), applicationUID, UserUid).Where(p => p.PGuid != null && p.PGuid.Equals(uid)).ToList<PointPollutantInfo>();
            if (uid.Equals("339B72C4-7295-4D31-B9EB-23342CB3697E"))
            {
                int x = 0;  //计数器，因a05024出现过两次的情况
                foreach (PointPollutantInfo ppi in PointPollutantInfos)
                {
                    if (ppi.PID.Equals("a05024") && PointType.Equals("1"))
                    {
                        x++;
                        if (x == 2)
                        {
                            PointPollutantInfos.Remove(ppi);
                            break;
                        }
                    }
                }
            }
            if (uid.Equals("aabe91e0-29a4-427c-becc-0b29f1224422"))
            {
                string[] factors = new string[PointPollutantInfos.Count+1];
                for (int i = 0; i < PointPollutantInfos.Count; i++)
                {
                    factors[i] = PointPollutantInfos[i].PID;
                };
                factors[PointPollutantInfos.Count] = "a05024";
                return factors;
            }
            else
            {
                string[] factors = new string[PointPollutantInfos.Count];
                for (int i = 0; i < PointPollutantInfos.Count; i++)
                {
                    factors[i] = PointPollutantInfos[i].PID;
                };
                return factors;
            }
        }

        #region 提交审核
        /// <summary>
        /// 审核提交
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="CreatUser"></param>
        /// <param name="IsAuditSuggestion"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public bool SubmitAudit(string applicationUID, string[] PointID, string InsId, DateTime beginTime, DateTime endTime, string UserIP, string UserUid, string CreatUser, bool IsAuditSuggestion, string Description, bool IsCreateDBF, string PointType)
        {
            List<AuditAirLogEntity> airLogList = new List<AuditAirLogEntity>();
            List<AuditWaterLogEntity> waterLogList = new List<AuditWaterLogEntity>();
            AuditMonitoringPointPollutantService pollutantService = new AuditMonitoringPointPollutantService();//因子接口
            bool isSuccess = true;
            #region 生成报表数据
            try
            {
                string[] factors = getFactors(InsId, PointID[0], applicationUID, UserUid, PointType);
                GeneratorAuditReportService heater = new GeneratorAuditReportService();
                ReportDataService reportService = new ReportDataService();
                reportService.datetime = beginTime;
                heater.AuditOperator += reportService.GenerateData;   //注册方法
                ApplicationType applicationType = EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID) ? ApplicationType.Air : ApplicationType.Water;
                if (PointID[0].Equals("204"))
                {
                    heater.ReportGenerator(applicationType, PointID, beginTime, endTime, "", IsCreateDBF, factors);   //会自动调用注册过对象的方法
                }
                else
                {
                    heater.ReportGenerator(applicationType, PointID, beginTime, endTime, "", IsCreateDBF);   //会自动调用注册过对象的方法
                }
                isSuccess = reportService.isSuccess;
            }
            catch
            {
                isSuccess = false;
            }
            #endregion
            
            #region 更新审核状态
            if (isSuccess)
            {
                foreach (string portid in PointID)
                {
                    //根据仪器uid获取因子
                    string[] factors = getFactors(InsId, portid, applicationUID, UserUid, PointType);

                    string InstrumentSql = string.Empty;
                    InstrumentSql = string.Format(@"select InstrumentName FROM V_Point_InstrumentChannels where PollutantCode='{0}'", factors[0]);
                    string InstrumentName = g_DatabaseHelper.ExecuteDataTable(InstrumentSql, "AMS_BaseDataConnection").Rows[0][0].ToString();

                    if (factors.Length <= 0) continue;
                    string factorsql = string.Empty;
                    for (int j = 0; j < factors.Length;j++ )
                    {
                        factorsql += "'"+factors[j]+"',";
                    }
                    #region 修改因子审核状态（小时）
                    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//空气
                    {
                        if (PointType.Equals("1"))
                        {
                            auditAirHourRep.AuditFactorHourStatusUpdateS(Convert.ToInt32(portid), InsId, beginTime, endTime.AddDays(1).AddSeconds(-1), factors, CreatUser, PointType);

                            string sql = string.Format(@"update [Audit].[TB_AuditAirInfectantByHour] set IsAudit = {0}
WHERE  ApplicationUid = 'airaaira-aira-aira-aira-airaairaaira' AND PointId = {1} AND DataDateTime>= '{2}' 
AND DataDateTime<'{3}'  AND PollutantCode in ({4})", 1, Convert.ToInt32(portid), beginTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"), factorsql.TrimEnd(','));
                            g_DatabaseHelper.ExecuteScalar(sql, "AMS_MonitoringBusinessConnection");
                        }
                        else
                        {
                            auditAirHourRep.AuditFactorHourStatusUpdate(Convert.ToInt32(portid), beginTime, endTime.AddDays(1).AddSeconds(-1), factors, CreatUser, PointType);
                            string sql = string.Format(@"update [Audit].[TB_AuditAirInfectantByHour] set IsAudit = {0}
WHERE  ApplicationUid = 'airaaira-aira-aira-aira-airaairaaira' AND PointId = {1} AND DataDateTime>= '{2}' 
AND DataDateTime<'{3}'  AND PollutantCode in ({4})", 1, Convert.ToInt32(portid), beginTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"), factorsql.TrimEnd(','));
                            g_DatabaseHelper.ExecuteScalar(sql, "AMS_MonitoringBusinessConnection");
                        }
                    }
                    else
                    {
                        //水
                        waterHourRep.AuditFactorHourStatusUpdate(Convert.ToInt32(portid), beginTime, endTime.AddDays(1).AddSeconds(-1), factors, CreatUser);
                    }
                    #endregion

                    //原逻辑是按照站点、日期判断审核状态
                    List<PointPollutantInfo> PointPollutantInfosAll = pollutantService.RetrieveSiteMapPollutantList(Convert.ToInt32(portid), applicationUID, UserUid).Where(p => p.PID != "").ToList<PointPollutantInfo>();
                    string[] factorsAll = new string[PointPollutantInfosAll.Count];
                    for (int i = 0; i < PointPollutantInfosAll.Count; i++)
                    {
                        factorsAll[i] = PointPollutantInfosAll[i].PID;
                    }
                    for (DateTime date = beginTime; date <= endTime; date = date.AddDays(1))
                    {
                        try
                        {
                            #region 获取审核状态
                            int flag = 0;
                            AuditStatusForDayEntity status = auditStateRep.Retrieve(x => x.PointId == Convert.ToInt32(portid) && x.Date == date && x.DataException == InsId).FirstOrDefault();
                            //AuditStatusForDayEntity status = auditStateRep.Retrieve(x => x.PointId == Convert.ToInt32(portid) && x.Date == date).FirstOrDefault();
                            if (status == null)
                            {
                                status = new AuditStatusForDayEntity();
                                status.AuditStatusUid = Guid.NewGuid().ToString();
                                status.ApplicationUid = applicationUID;
                                status.DataException = InsId;
                                status.PointId = Convert.ToInt32(portid);
                                status.Date = date;
                                status.IsAuditSuggestion = IsAuditSuggestion;
                                status.CreatDateTime = DateTime.Now;
                                status.CreatUser = CreatUser;
                                status.UpdateDateTime = DateTime.Now;
                                status.UpdateUser = CreatUser;
                                status.Status = "1";
                                //status.SuperStatus = "";
                                auditStateRep.Add(status);
                            }
                            else
                            {
                                flag = 1;
                                status.UpdateDateTime = DateTime.Now;
                                status.UpdateUser = CreatUser;
                            }
                            #endregion

                            #region 修改因子审核状态
                            int auditCount = 0;
                            if (flag == 1)//当审核状态有数据时预处理表才会有数据
                            {
                                string state = "";
                                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                                {
                                    //修改因子审核状态（预处理表）
                                    //auditAirHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);
                                    auditAirHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), factors, status.AuditStatusUid, CreatUser);
                                    status.SuperStatus = "1";
                                    status.Status = "1";
                                    ////判断1天的审核状态
                                    //if (factorsAll.Length <= 0) continue;
                                    //auditCount = auditAirHourRep.GetAuditRecordNumByHour(status.AuditStatusUid, factorsAll);
                                    //if (auditCount > 0)
                                    //{
                                    //    if (auditCount < 24 * factorsAll.Length) state = "2";
                                    //    else state = "1";
                                    //}
                                    //if (PointType.Equals("1"))
                                    //    status.SuperStatus = state;
                                    //else
                                    //    status.Status = state;
                                }
                                else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                                {
                                    //修改因子审核状态（预处理表）
                                    waterHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);

                                    //判断1天的审核状态
                                    g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();

                                    AMSRepository.Water.InfectantBy60Repository r = new AMSRepository.Water.InfectantBy60Repository();

                                    //获取点位应测数据条数

                                    List<int> l = r.Retrieve(p => p.PointId == Convert.ToInt32(portid) && p.Tstamp >= beginTime && p.Tstamp <= endTime.AddDays(1).AddSeconds(-1)).GroupBy(g => g.Tstamp).Select(f => f.Count()).ToList<int>();
                                    int dataCycle = l.Count;
                                    AuditWaterStatusForHourEntity[] AuditWaterStatusForHourEntitys = statusWaterHourRep.Retrieve(x => x.PointId == Convert.ToInt32(portid) && x.Date >= Convert.ToDateTime(beginTime.ToString("yyyy-MM-dd 00:00:00")) && x.Date <= Convert.ToDateTime(endTime.ToString("yyyy-MM-dd 23:59:59")) && x.Status == "1").ToArray();

                                    if (AuditWaterStatusForHourEntitys.Length < Convert.ToInt32(dataCycle))
                                        state = "2";
                                    else
                                        state = "1";
                                    //auditCount = waterHourRep.GetAuditRecordNumByHour(status.AuditStatusUid, factors);
                                    //if (auditCount > 0)
                                    //{
                                    //    g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
                                    //    //获取点位应测数据条数
                                    //    int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == portid).Select(p => p.DataCycle).FirstOrDefault();
                                    //    if (auditCount < dataCycle * factors.Length) state = "2";
                                    //    else state = "1";
                                    //}
                                    status.Status = state;
                                }

                                auditStateRep.Update(status);
                            }

                            #region 修改审核状态表
                            //if (flag == 1)//当审核状态有数据时预处理表才会有数据
                            //{
                            //    string state = "";
                            //    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                            //    {
                            //        auditAirHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);
                            //        if (auditCount > 0)
                            //        {
                            //            if (auditCount != 24) state = "2";
                            //            else state = "1";
                            //        }
                            //    }
                            //    else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                            //    {
                            //        waterHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);
                            //        if (auditCount > 0)
                            //        {
                            //            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
                            //            int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == portid).Select(p => p.DataCycle).FirstOrDefault();
                            //            if (auditCount != dataCycle) state = "2";
                            //            else state = "1";
                            //        }
                            //    }
                            //    status.Status = state;
                            //    auditStateRep.Update(status);
                            //}
                            #endregion
                            #endregion

                            #region 记录审核日志
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//空气
                            {
                                AuditAirLogEntity log = new AuditAirLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.PollutantCode = factors[0];
                                log.PollutantName = InstrumentName;
                                log.Tstamp = date.Date;
                                log.AuditType = "数据审核";
                                log.AuditTime = DateTime.Now;
                                log.OperationTypeEnum = "提交审核";
                                log.OperationReason = "提交审核";
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "提交审核成功";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                airLogList.Add(log);
                            }
                            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//水
                            {
                                AuditWaterLogEntity log = new AuditWaterLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = date.Date;
                                log.AuditType = "数据审核";
                                log.OperationTypeEnum = "提交审核";
                                log.OperationReason = "提交审核";
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                logWaterRep.Add(log);
                            }
                            #endregion
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
                try
                {
                    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//空气
                    {
                        if (airLogList != null && airLogList.Count > 0)
                            logAirRep.BatchAdd(airLogList);
                    }
                    else
                    {
                        if (waterLogList != null && waterLogList.Count > 0)
                            logWaterRep.BatchAdd(waterLogList);
                    }
                }
                catch
                {
                    isSuccess = false;
                }
            }
            #endregion
            return isSuccess;
        }

        /// <summary>
        /// 手动审核提交
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="CreatUser"></param>
        /// <param name="IsAuditSuggestion"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public bool SubmitAuditNew(string applicationUID, string[] PointID, DateTime beginTime, DateTime endTime, string type, string UserIP, string UserUid, string CreatUser, bool IsAuditSuggestion, string Description, bool IsCreateDBF, string PointType)
        {
            bool isSuccess = true;
            #region 生成报表数据
            try
            {
                GeneratorAuditReportService heater = new GeneratorAuditReportService();
                ReportDataService reportService = new ReportDataService();
                reportService.datetime = beginTime;
                heater.AuditOperator += reportService.GenerateData;   //注册方法
                ApplicationType applicationType = EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID) ? ApplicationType.Air : ApplicationType.Water;
                heater.ReportGenerator(applicationType, PointID, beginTime, endTime, type, IsCreateDBF);   //会自动调用注册过对象的方法
                isSuccess = reportService.isSuccess;
            }
            catch
            {
                isSuccess = false;
            }
            #endregion

            #region 更新审核状态
            if (isSuccess)
            {
                List<AuditAirLogEntity> airLogList = new List<AuditAirLogEntity>();
                List<AuditWaterLogEntity> waterLogList = new List<AuditWaterLogEntity>();
                AuditMonitoringPointPollutantService pollutantService = new AuditMonitoringPointPollutantService();//因子接口
                foreach (string portid in PointID)
                {
                    #region 获取审核因子配置个数
                    List<PointPollutantInfo> PointPollutantInfos = pollutantService.RetrieveSiteMapPollutantList(Convert.ToInt32(portid), applicationUID, UserUid).Where(p => p.PID != "").ToList<PointPollutantInfo>();
                    string[] factors = new string[PointPollutantInfos.Count];
                    for (int i = 0; i < PointPollutantInfos.Count; i++)
                    {
                        factors[i] = PointPollutantInfos[i].PID;
                    }
                    if (factors.Length <= 0) continue;
                    #endregion

                    #region 修改因子审核状态（小时）
                    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//空气
                    {
                        auditAirHourRep.AuditFactorHourStatusUpdate(Convert.ToInt32(portid), beginTime, endTime.AddDays(1).AddHours(-1), factors, CreatUser, PointType);
                    }
                    else
                    {
                        //水
                        waterHourRep.AuditFactorHourStatusUpdate(Convert.ToInt32(portid), beginTime, endTime.AddDays(1).AddMilliseconds(-1), factors, CreatUser);
                    }
                    #endregion

                    for (DateTime date = beginTime; date <= endTime; date = date.AddDays(1))
                    {
                        try
                        {
                            #region 获取审核状态
                            int flag = 0;
                            AuditStatusForDayEntity status = auditStateRep.Retrieve(x => x.PointId == Convert.ToInt32(portid) && x.Date == date).FirstOrDefault();
                            if (status == null)
                            {
                                status = new AuditStatusForDayEntity();
                                status.AuditStatusUid = Guid.NewGuid().ToString();
                                status.ApplicationUid = applicationUID;
                                status.PointId = Convert.ToInt32(portid);
                                status.Date = date;
                                status.IsAuditSuggestion = IsAuditSuggestion;
                                status.CreatDateTime = DateTime.Now;
                                status.CreatUser = CreatUser;
                                status.UpdateDateTime = DateTime.Now;
                                status.UpdateUser = CreatUser;
                                status.Status = "";
                                //status.SuperStatus = "";
                                auditStateRep.Add(status);
                            }
                            else
                            {
                                flag = 1;
                                status.UpdateDateTime = DateTime.Now;
                                status.UpdateUser = CreatUser;
                            }
                            #endregion

                            #region 修改因子审核状态
                            int auditCount = 0;
                            if (flag == 1)//当审核状态有数据时预处理表才会有数据
                            {
                                string state = "";
                                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                                {
                                    //修改因子审核状态（预处理表）
                                    auditAirHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);

                                    //判断1天的审核状态
                                    auditCount = auditAirHourRep.GetAuditRecordNumByHour(status.AuditStatusUid, factors);
                                    if (auditCount > 0)
                                    {

                                        if (auditCount < 24 * factors.Length) state = "2";
                                        else state = "1";
                                    }
                                    if (PointType.Equals("1"))
                                        status.SuperStatus = state;
                                    else
                                        status.Status = state;
                                }
                                else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                                {
                                    //修改因子审核状态（预处理表）
                                    waterHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);

                                    //判断1天的审核状态
                                    //g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
                                    AMSRepository.Water.InfectantBy60Repository r = new AMSRepository.Water.InfectantBy60Repository();
                                    //获取点位应测数据条数
                                    int l = r.Retrieve(p => p.PointId == Convert.ToInt32(portid) && p.Tstamp >= beginTime && p.Tstamp <= endTime.AddDays(1).AddSeconds(-1)).Select(x => x.Tstamp).Distinct().Count();
                                    int dataCycle = l;

                                    AuditWaterStatusForHourEntity[] AuditWaterStatusForHourEntitys = statusWaterHourRep.Retrieve(x => x.PointId == Convert.ToInt32(portid) && x.Date >= Convert.ToDateTime(beginTime.ToString("yyyy-MM-dd 00:00:00")) && x.Date <= Convert.ToDateTime(endTime.ToString("yyyy-MM-dd 23:59:59")) && x.Status == "1").ToArray();

                                    if (AuditWaterStatusForHourEntitys.Length < Convert.ToInt32(dataCycle))
                                        state = "2";
                                    else
                                        state = "1";
                                    //auditCount = waterHourRep.GetAuditRecordNumByHour(status.AuditStatusUid, factors);
                                    //if (auditCount > 0)
                                    //{
                                    //    g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
                                    //    //获取点位应测数据条数
                                    //    int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == portid).Select(p => p.DataCycle).FirstOrDefault();
                                    //    if (auditCount < dataCycle * factors.Length) state = "2";
                                    //    else state = "1";
                                    //}
                                    status.Status = state;
                                }

                                auditStateRep.Update(status);
                            }

                            #region 修改审核状态表
                            //if (flag == 1)//当审核状态有数据时预处理表才会有数据
                            //{
                            //    string state = "";
                            //    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                            //    {
                            //        auditAirHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);
                            //        if (auditCount > 0)
                            //        {
                            //            if (auditCount != 24) state = "2";
                            //            else state = "1";
                            //        }
                            //    }
                            //    else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                            //    {
                            //        waterHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);
                            //        if (auditCount > 0)
                            //        {
                            //            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
                            //            int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == portid).Select(p => p.DataCycle).FirstOrDefault();
                            //            if (auditCount != dataCycle) state = "2";
                            //            else state = "1";
                            //        }
                            //    }
                            //    status.Status = state;
                            //    auditStateRep.Update(status);
                            //}
                            #endregion
                            #endregion

                            #region 记录审核日志
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//空气
                            {
                                AuditAirLogEntity log = new AuditAirLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = date.Date;
                                log.AuditType = "数据审核";
                                log.AuditTime = DateTime.Now;
                                log.OperationTypeEnum = "提交审核";
                                log.OperationReason = "提交审核";
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                airLogList.Add(log);
                            }
                            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//水
                            {
                                AuditWaterLogEntity log = new AuditWaterLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = date.Date;
                                log.AuditType = "数据审核";
                                log.OperationTypeEnum = "提交审核";
                                log.OperationReason = "提交审核";
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                logWaterRep.Add(log);
                            }
                            #endregion
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
                try
                {
                    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//空气
                    {
                        if (airLogList != null && airLogList.Count > 0)
                            logAirRep.BatchAdd(airLogList);
                    }
                    else
                    {
                        if (waterLogList != null && waterLogList.Count > 0)
                            logWaterRep.BatchAdd(waterLogList);
                    }
                }
                catch
                {
                    isSuccess = false;
                }
            }
            #endregion
            return isSuccess;
        }

        /// <summary>
        /// 气象审核提交
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="CreatUser"></param>
        /// <param name="IsAuditSuggestion"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public bool SubmitAuditWeather(string applicationUID, string[] PointID, string[] factors, DateTime beginTime, DateTime endTime, string UserIP, string UserUid, string CreatUser, bool IsAuditSuggestion, string Description, bool IsCreateDBF, string PointType)
        {
            bool isSuccess = true;
            #region 生成报表数据
            try
            {
                GeneratorAuditReportService heater = new GeneratorAuditReportService();
                ReportDataService reportService = new ReportDataService();
                reportService.datetime = beginTime;
                heater.AuditOperator += reportService.GenerateData;   //注册方法
                ApplicationType applicationType = EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID) ? ApplicationType.Air : ApplicationType.Water;
                heater.ReportGenerator(applicationType, PointID, beginTime, endTime, "", IsCreateDBF);   //会自动调用注册过对象的方法
                isSuccess = reportService.isSuccess;
            }
            catch
            {
                isSuccess = false;
            }
            #endregion

            #region 更新审核状态
            if (isSuccess)
            {
                List<AuditAirLogEntity> airLogList = new List<AuditAirLogEntity>();
                List<AuditWaterLogEntity> waterLogList = new List<AuditWaterLogEntity>();
                AuditMonitoringPointPollutantService pollutantService = new AuditMonitoringPointPollutantService();//因子接口
                foreach (string portid in PointID)
                {
                    #region 获取审核因子配置个数
                    g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                    if (portid != "1")
                    {
                        string guid = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portid)).MonitoringPointUid;//获取站点名称
                        List<PollutantCodeEntity> PointPollutantInfos = pollutantService.RetrievePollutantListByPointUid(guid).ToList<PollutantCodeEntity>();
                        factors = new string[PointPollutantInfos.Count];
                        for (int i = 0; i < PointPollutantInfos.Count; i++)
                        {
                            factors[i] = PointPollutantInfos[i].PollutantCode;
                        }
                    }
                    if (factors.Length <= 0) continue;
                    #endregion

                    #region 修改因子审核状态（小时）
                    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//空气
                    {
                        auditAirHourRep.AuditFactorHourStatusUpdateWea(Convert.ToInt32(portid), beginTime, endTime.AddDays(1).AddSeconds(-1), factors, CreatUser, PointType);
                    }
                    else
                    {
                        //水
                        waterHourRep.AuditFactorHourStatusUpdate(Convert.ToInt32(portid), beginTime, endTime.AddDays(1).AddSeconds(-1), factors, CreatUser);
                    }
                    #endregion

                    for (DateTime date = beginTime; date <= endTime; date = date.AddDays(1))
                    {
                        try
                        {
                            #region 获取审核状态
                            int flag = 0;
                            AuditStatusForDayEntity status = auditStateRep.Retrieve(x => x.PointId == Convert.ToInt32(portid) && x.Date == date).FirstOrDefault();
                            if (status == null)
                            {
                                status = new AuditStatusForDayEntity();
                                status.AuditStatusUid = Guid.NewGuid().ToString();
                                status.ApplicationUid = applicationUID;
                                status.PointId = Convert.ToInt32(portid);
                                status.Date = date;
                                status.IsAuditSuggestion = IsAuditSuggestion;
                                status.CreatDateTime = DateTime.Now;
                                status.CreatUser = CreatUser;
                                status.UpdateDateTime = DateTime.Now;
                                status.UpdateUser = CreatUser;
                                status.Status = "";
                                //status.SuperStatus = "";
                                auditStateRep.Add(status);
                            }
                            else
                            {
                                flag = 1;
                                status.UpdateDateTime = DateTime.Now;
                                status.UpdateUser = CreatUser;
                            }
                            #endregion

                            #region 修改因子审核状态
                            int auditCount = 0;
                            if (flag == 1)//当审核状态有数据时预处理表才会有数据
                            {
                                string state = "";
                                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                                {
                                    //修改因子审核状态（预处理表）
                                    auditAirHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);

                                    //判断1天的审核状态
                                    auditCount = auditAirHourRep.GetAuditRecordNumByHourWea(Convert.ToInt32(portid), beginTime, endTime.AddDays(1).AddSeconds(-1));
                                    if (auditCount > 0)
                                    {

                                        if (auditCount < 24) state = "2";
                                        else state = "1";
                                    }
                                    if (PointType.Equals("1"))
                                        status.SuperStatus = state;
                                    else
                                        status.Status = state;
                                }
                                auditStateRep.Update(status);
                            }

                            #region 修改审核状态表
                            //if (flag == 1)//当审核状态有数据时预处理表才会有数据
                            //{
                            //    string state = "";
                            //    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                            //    {
                            //        auditAirHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);
                            //        if (auditCount > 0)
                            //        {
                            //            if (auditCount != 24) state = "2";
                            //            else state = "1";
                            //        }
                            //    }
                            //    else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                            //    {
                            //        waterHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);
                            //        if (auditCount > 0)
                            //        {
                            //            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
                            //            int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == portid).Select(p => p.DataCycle).FirstOrDefault();
                            //            if (auditCount != dataCycle) state = "2";
                            //            else state = "1";
                            //        }
                            //    }
                            //    status.Status = state;
                            //    auditStateRep.Update(status);
                            //}
                            #endregion
                            #endregion

                            #region 记录审核日志
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//空气
                            {
                                AuditAirLogEntity log = new AuditAirLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = date.Date;
                                log.AuditType = "数据审核";
                                log.AuditTime = DateTime.Now;
                                log.OperationTypeEnum = "提交审核";
                                log.OperationReason = "提交审核";
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                airLogList.Add(log);
                            }
                            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//水
                            {
                                AuditWaterLogEntity log = new AuditWaterLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = date.Date;
                                log.AuditType = "数据审核";
                                log.OperationTypeEnum = "提交审核";
                                log.OperationReason = "提交审核";
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                logWaterRep.Add(log);
                            }
                            #endregion
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
                try
                {
                    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//空气
                    {
                        if (airLogList != null && airLogList.Count > 0)
                            logAirRep.BatchAdd(airLogList);
                    }
                    else
                    {
                        if (waterLogList != null && waterLogList.Count > 0)
                            logWaterRep.BatchAdd(waterLogList);
                    }
                }
                catch
                {
                    isSuccess = false;
                }
            }
            #endregion
            return isSuccess;
        }

        /// <summary>
        /// 审核提交（超级站）
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="CreatUser"></param>
        /// <param name="IsAuditSuggestion"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public bool SubmitAuditSuper(string applicationUID, string[] PointID, DateTime beginTime, DateTime endTime, string UserIP, string UserUid, string CreatUser, bool IsAuditSuggestion, string Description, bool IsCreateDBF, string PointType)
        {
            bool isSuccess = true;
            #region 生成报表数据
            try
            {
                GeneratorAuditReportService heater = new GeneratorAuditReportService();
                ReportDataService reportService = new ReportDataService();
                reportService.datetime = beginTime;
                heater.AuditOperator += reportService.GenerateDataSuper;   //注册方法
                ApplicationType applicationType = EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID) ? ApplicationType.Air : ApplicationType.Water;
                heater.ReportGenerator(applicationType, PointID, beginTime, endTime, "", IsCreateDBF);   //会自动调用注册过对象的方法
                isSuccess = reportService.isSuccess;
            }
            catch
            {
                isSuccess = false;
            }

            #region 更新审核状态
            if (isSuccess)
            {
                List<AuditAirLogEntity> airLogList = new List<AuditAirLogEntity>();
                foreach (string portid in PointID)
                {
                    for (DateTime date = beginTime; date <= endTime; date = date.AddDays(1))
                    {
                        try
                        {
                            #region 获取审核状态
                            int flag = 0;
                            AuditStatusForDayEntity status = auditStateRep.Retrieve(x => x.PointId == Convert.ToInt32(portid) && x.Date == date).FirstOrDefault();
                            if (status == null)
                            {
                                status = new AuditStatusForDayEntity();
                                status.AuditStatusUid = Guid.NewGuid().ToString();
                                status.ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
                                status.PointId = Convert.ToInt32(portid);
                                status.Date = date;
                                status.IsAuditSuggestion = IsAuditSuggestion;
                                status.CreatDateTime = DateTime.Now;
                                status.CreatUser = CreatUser;
                                status.UpdateDateTime = DateTime.Now;
                                status.UpdateUser = CreatUser;
                                status.Status = "";
                                auditStateRep.Add(status);
                            }
                            else
                            {
                                flag = 1;
                                status.UpdateDateTime = DateTime.Now;
                                status.UpdateUser = CreatUser;
                            }
                            #endregion

                            #region 修改因子审核状态
                            if (flag == 1)//当审核状态有数据时预处理表才会有数据
                            {
                                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals("airaaira-aira-aira-aira-airaairaaira"))
                                {
                                    status.SuperStatus = "2";
                                }

                                auditStateRep.Update(status);
                            }
                            #endregion

                            #region 记录审核日志
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals("airaaira-aira-aira-aira-airaairaaira"))//空气
                            {
                                AuditAirLogEntity log = new AuditAirLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = date.Date;
                                log.AuditType = "数据审核";
                                log.AuditTime = DateTime.Now;
                                log.OperationTypeEnum = "提交审核";
                                log.OperationReason = "提交审核";
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                airLogList.Add(log);
                            }
                            #endregion
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
                try
                {
                    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals("airaaira-aira-aira-aira-airaairaaira"))//空气
                    {
                        if (airLogList != null && airLogList.Count > 0)
                            logAirRep.BatchAdd(airLogList);
                    }
                }
                catch
                {
                    isSuccess = false;
                }
            }
            #endregion
            #endregion

            #region 更新审核状态
            if (isSuccess)
            {
                List<AuditAirLogEntity> airLogList = new List<AuditAirLogEntity>();
                List<AuditWaterLogEntity> waterLogList = new List<AuditWaterLogEntity>();
                foreach (string portid in PointID)
                {
                    #region 获取审核因子配置个数
                    string[] factors = auditPollutantService.GetAuditFactorsCount(Convert.ToInt32(portid), applicationUID, PointType);
                    if (factors.Length <= 0) continue;
                    #endregion

                    #region 修改因子审核状态（小时）
                    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//空气
                    {
                        auditAirHourRep.AuditFactorHourStatusUpdate(Convert.ToInt32(portid), beginTime, endTime.AddDays(1).AddSeconds(-1), factors, CreatUser, PointType);
                    }
                    else
                    {
                        //水
                        waterHourRep.AuditFactorHourStatusUpdate(Convert.ToInt32(portid), beginTime, endTime.AddDays(1).AddHours(-1), factors, CreatUser);
                    }
                    #endregion

                    for (DateTime date = beginTime; date <= endTime; date = date.AddDays(1))
                    {
                        try
                        {
                            #region 获取审核状态
                            int flag = 0;
                            AuditStatusForDayEntity status = auditStateRep.Retrieve(x => x.PointId == Convert.ToInt32(portid) && x.Date == date).FirstOrDefault();
                            if (status == null)
                            {
                                status = new AuditStatusForDayEntity();
                                status.AuditStatusUid = Guid.NewGuid().ToString();
                                status.ApplicationUid = applicationUID;
                                status.PointId = Convert.ToInt32(portid);
                                status.Date = date;
                                status.IsAuditSuggestion = IsAuditSuggestion;
                                status.CreatDateTime = DateTime.Now;
                                status.CreatUser = CreatUser;
                                status.UpdateDateTime = DateTime.Now;
                                status.UpdateUser = CreatUser;
                                status.Status = "";
                                //status.SuperStatus = "";
                                auditStateRep.Add(status);
                            }
                            else
                            {
                                flag = 1;
                                status.UpdateDateTime = DateTime.Now;
                                status.UpdateUser = CreatUser;
                            }
                            #endregion

                            #region 修改因子审核状态
                            int auditCount = 0;
                            if (flag == 1)//当审核状态有数据时预处理表才会有数据
                            {
                                string state = "";
                                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                                {
                                    //修改因子审核状态（预处理表）
                                    auditAirHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);

                                    //判断1天的审核状态
                                    auditCount = auditAirHourRep.GetAuditRecordNumByHour(status.AuditStatusUid, factors);
                                    if (auditCount > 0)
                                    {

                                        if (auditCount < 24 * factors.Length) state = "2";
                                        else state = "1";
                                    }
                                    if (PointType.Equals("1"))
                                        status.SuperStatus = state;
                                    else
                                        status.Status = state;
                                }
                                else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                                {
                                    //修改因子审核状态（预处理表）
                                    waterHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);

                                    //判断1天的审核状态
                                    auditCount = waterHourRep.GetAuditRecordNumByHour(status.AuditStatusUid, factors);
                                    if (auditCount > 0)
                                    {
                                        g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
                                        //获取点位应测数据条数
                                        int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == portid).Select(p => p.DataCycle).FirstOrDefault();
                                        if (auditCount < dataCycle * factors.Length) state = "2";
                                        else state = "1";
                                    }
                                    status.Status = state;
                                }

                                auditStateRep.Update(status);
                            }

                            #region 修改审核状态表
                            //if (flag == 1)//当审核状态有数据时预处理表才会有数据
                            //{
                            //    string state = "";
                            //    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))
                            //    {
                            //        auditAirHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);
                            //        if (auditCount > 0)
                            //        {
                            //            if (auditCount != 24) state = "2";
                            //            else state = "1";
                            //        }
                            //    }
                            //    else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                            //    {
                            //        waterHourRep.AuditFactorStatusUpdate(Convert.ToInt32(portid), status.AuditStatusUid, CreatUser);
                            //        if (auditCount > 0)
                            //        {
                            //            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
                            //            int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == portid).Select(p => p.DataCycle).FirstOrDefault();
                            //            if (auditCount != dataCycle) state = "2";
                            //            else state = "1";
                            //        }
                            //    }
                            //    status.Status = state;
                            //    auditStateRep.Update(status);
                            //}
                            #endregion
                            #endregion

                            #region 记录审核日志
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//空气
                            {
                                AuditAirLogEntity log = new AuditAirLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = date.Date;
                                log.AuditType = "数据审核";
                                log.AuditTime = DateTime.Now;
                                log.OperationTypeEnum = "提交审核";
                                log.OperationReason = "提交审核";
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                airLogList.Add(log);
                            }
                            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//水
                            {
                                AuditWaterLogEntity log = new AuditWaterLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = date.Date;
                                log.AuditType = "数据审核";
                                log.OperationTypeEnum = "提交审核";
                                log.OperationReason = "提交审核";
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                logWaterRep.Add(log);
                            }
                            #endregion
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
                try
                {
                    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//空气
                    {
                        if (airLogList != null && airLogList.Count > 0)
                            logAirRep.BatchAdd(airLogList);
                    }
                    else
                    {
                        if (waterLogList != null && waterLogList.Count > 0)
                            logWaterRep.BatchAdd(waterLogList);
                    }
                }
                catch
                {
                    isSuccess = false;
                }
            }
            #endregion
            return isSuccess;
        }

        /// <summary>
        /// 审核提交（超级站）微波辐射
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="CreatUser"></param>
        /// <param name="IsAuditSuggestion"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public bool SubmitAuditSuper(string[] PointID, DateTime beginTime, DateTime endTime, string UserIP, string UserUid, string CreatUser, bool IsAuditSuggestion, string Description)
        {
            bool isSuccess = true;
            #region 生成报表数据
            try
            {
                DatabaseHelper dbhelp = new DatabaseHelper();
                string isExist = "select * from [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_WeiboReport] where pointid='" + PointID[0] + "' and datetime>='" + beginTime.ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + endTime.ToString("yyyy-MM-dd 23:59:59") + "'";
                DataTable dt = dbhelp.ExecuteDataTable(isExist, "AMS_MonitoringBusinessConnection");
                if (dt.Rows.Count > 0)
                {
                    string deleteSql = "delete from [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_WeiboReport] where pointid='" + PointID[0] + "' and datetime>='" + beginTime.ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + endTime.ToString("yyyy-MM-dd 23:59:59") + "'";
                    dbhelp.ExecuteNonQuery(deleteSql, "AMS_MonitoringBusinessConnection");
                }
                string insertSql = "insert into [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_WeiboReport] " +
"([PointId],[DateTime],[LV2Processor],PollutantCode,[0.00],[0.05],[0.10],[0.15],[0.20],[0.25],[0.30],[0.35],[0.40],[0.45],[0.50],[0.60],[0.70],[0.80],[0.90],[1.00],[1.10],[1.20],[1.30],[1.40],[1.50],[1.60],[1.70],[1.80],[1.90],[2.00],[2.25],[2.50],[2.75],[3.00],[3.25],[3.50],[3.75],[4.00],[4.25],[4.50],[4.75],[5.00],[5.25],[5.50],[5.75],[6.00],[6.25],[6.50],[6.75],[7.00],[7.25],[7.50],[7.75],[8.00],[8.25],[8.50],[8.75],[9.00],[9.25],[9.50],[9.75],[10.00])" +
"(select [PointId],[DateTime],[LV2Processor],PollutantCode,[0.00],[0.05],[0.10],[0.15],[0.20],[0.25],[0.30],[0.35],[0.40],[0.45],[0.50],[0.60],[0.70],[0.80],[0.90],[1.00],[1.10],[1.20],[1.30],[1.40],[1.50],[1.60],[1.70],[1.80],[1.90],[2.00],[2.25],[2.50],[2.75],[3.00],[3.25],[3.50],[3.75],[4.00],[4.25],[4.50],[4.75],[5.00],[5.25],[5.50],[5.75],[6.00],[6.25],[6.50],[6.75],[7.00],[7.25],[7.50],[7.75],[8.00],[8.25],[8.50],[8.75],[9.00],[9.25],[9.50],[9.75],[10.00]  FROM [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_Weibo]" +
" where pointid='" + PointID[0] + "' and datetime>='" + beginTime.ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + endTime.ToString("yyyy-MM-dd 23:59:59") + "')";
                dbhelp.ExecuteNonQuery(insertSql, "AMS_MonitoringBusinessConnection");

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            #region 更新审核状态
            if (isSuccess)
            {
                List<AuditAirLogEntity> airLogList = new List<AuditAirLogEntity>();
                foreach (string portid in PointID)
                {
                    for (DateTime date = beginTime; date <= endTime; date = date.AddDays(1))
                    {
                        try
                        {
                            #region 获取审核状态
                            int flag = 0;
                            AuditStatusForDayEntity status = auditStateRep.Retrieve(x => x.PointId == Convert.ToInt32(portid) && x.Date == date).FirstOrDefault();
                            if (status == null)
                            {
                                status = new AuditStatusForDayEntity();
                                status.AuditStatusUid = Guid.NewGuid().ToString();
                                status.ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
                                status.PointId = Convert.ToInt32(portid);
                                status.Date = date;
                                status.IsAuditSuggestion = IsAuditSuggestion;
                                status.CreatDateTime = DateTime.Now;
                                status.CreatUser = CreatUser;
                                status.UpdateDateTime = DateTime.Now;
                                status.UpdateUser = CreatUser;
                                status.Status = "";
                                auditStateRep.Add(status);
                            }
                            else
                            {
                                flag = 1;
                                status.UpdateDateTime = DateTime.Now;
                                status.UpdateUser = CreatUser;
                            }
                            #endregion

                            #region 修改因子审核状态
                            if (flag == 1)//当审核状态有数据时预处理表才会有数据
                            {
                                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals("airaaira-aira-aira-aira-airaairaaira"))
                                {
                                    status.SuperStatus = "2";
                                }

                                auditStateRep.Update(status);
                            }
                            #endregion

                            #region 记录审核日志
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals("airaaira-aira-aira-aira-airaairaaira"))//空气
                            {
                                AuditAirLogEntity log = new AuditAirLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = date.Date;
                                log.AuditType = "数据审核";
                                log.AuditTime = DateTime.Now;
                                log.OperationTypeEnum = "提交审核";
                                log.OperationReason = "提交审核";
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                airLogList.Add(log);
                            }
                            #endregion
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
                try
                {
                    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals("airaaira-aira-aira-aira-airaairaaira"))//空气
                    {
                        if (airLogList != null && airLogList.Count > 0)
                            logAirRep.BatchAdd(airLogList);
                    }
                }
                catch
                {
                    isSuccess = false;
                }
            }
            #endregion

            #endregion
            return isSuccess;
        }

        /// <summary>
        /// 审核提交（超级站）粒径谱
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="CreatUser"></param>
        /// <param name="IsAuditSuggestion"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public bool SubmitAuditLijingpu(string[] PointID, DateTime beginTime, DateTime endTime, string UserIP, string UserUid, string CreatUser, bool IsAuditSuggestion, string Description)
        {
            bool isSuccess = true;
            #region 生成报表数据
            try
            {
                DatabaseHelper dbhelp = new DatabaseHelper();

                #region 大粒径APS
                string isExistL = "select * from [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_LReport] where pointid='" + PointID[0] + "' and datetime>='" + beginTime.ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + endTime.ToString("yyyy-MM-dd 23:59:59") + "'";
                DataTable dtL = dbhelp.ExecuteDataTable(isExistL, "AMS_MonitoringBusinessConnection");
                if (dtL.Rows.Count > 0)
                {
                    string deleteSqlL = "delete from [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_LReport] where pointid='" + PointID[0] + "' and datetime>='" + beginTime.ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + endTime.ToString("yyyy-MM-dd 23:59:59") + "'";
                    dbhelp.ExecuteNonQuery(deleteSqlL, "AMS_MonitoringBusinessConnection");
                }
                string insertSqlL = "insert into [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_LReport] " +
"([PointId],[DateTime],[0.523],[0.542],[0.583],[0.626],[0.673],[0.723],[0.777],[0.835],[0.898],[0.965],[1.037],[1.114],[1.197],[1.286],[1.382],[1.486],[1.596],[1.715],[1.843],[1.981],[2.129],[2.288],[2.458],[2.642],[2.839],[3.051],[3.278],[3.523],[3.786],[4.068],[4.371],[4.698],[5.048],[5.425],[5.829],[6.264],[6.732],[7.234],[7.774],[8.354],[8.977],[9.647],[10.37],[11.14],[11.97],[12.86],[13.82],[14.86],[15.96],[17.15],[18.43],[19.81])" +
"(SELECT [PointId],[DateTime],[0.523],[0.542],[0.583],[0.626],[0.673],[0.723],[0.777],[0.835],[0.898],[0.965],[1.037],[1.114],[1.197],[1.286],[1.382],[1.486],[1.596],[1.715],[1.843],[1.981],[2.129],[2.288],[2.458],[2.642],[2.839],[3.051],[3.278],[3.523],[3.786],[4.068],[4.371],[4.698],[5.048],[5.425],[5.829],[6.264],[6.732],[7.234],[7.774],[8.354],[8.977],[9.647],[10.37],[11.14],[11.97],[12.86],[13.82],[14.86],[15.96],[17.15],[18.43],[19.81]" +
"FROM [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_L]" +
" where pointid='" + PointID[0] + "' and datetime>='" + beginTime.ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + endTime.ToString("yyyy-MM-dd 23:59:59") + "')";
                dbhelp.ExecuteNonQuery(insertSqlL, "AMS_MonitoringBusinessConnection");
                #endregion

                #region 中粒径L72
                string isExistM = "select * from [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_MReport] where pointid='" + PointID[0] + "' and datetime>='" + beginTime.ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + endTime.ToString("yyyy-MM-dd 23:59:59") + "'";
                DataTable dtM = dbhelp.ExecuteDataTable(isExistM, "AMS_MonitoringBusinessConnection");
                if (dtM.Rows.Count > 0)
                {
                    string deleteSqlM = "delete from [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_MReport] where pointid='" + PointID[0] + "' and datetime>='" + beginTime.ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + endTime.ToString("yyyy-MM-dd 23:59:59") + "'";
                    dbhelp.ExecuteNonQuery(deleteSqlM, "AMS_MonitoringBusinessConnection");
                }
                string insertSqlM = "insert into [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_MReport] " +
"([PointId],[DateTime],[13.6],[14.1],[14.6],[15.1],[15.7],[16.3],[16.8],[17.5],[18.1],[18.8],[19.5],[20.2],[20.9],[21.7],[22.5],[23.3],[24.1],[25.0],[25.9],[26.9],[27.9],[28.9],[30.0],[31.1],[32.2],[33.4],[34.6],[35.9],[37.2],[38.5],[40.0],[41.4],[42.9],[44.5],[46.1],[47.8],[49.6],[51.4],[53.3],[55.2],[57.3],[59.4],[61.5],[63.8],[66.1],[68.5],[71.0],[73.7],[76.4],[79.1],[82.0],[85.1],[88.2],[91.4],[94.7],[98.2],[101.8],[105.5],[109.4],[113.4],[117.6],[121.9],[126.3],[131.0],[135.8],[140.7],[145.9],[151.2],[156.8],[162.5],[168.5],[174.7],[181.1],[187.7],[194.6],[201.7],[209.1],[216.7],[224.7],[232.9],[241.4],[250.3],[259.5],[269.0],[278.8],[289.0],[299.6],[310.6],[322.0],[333.8],[346.0],[358.7],[371.8],[385.4],[399.5],[414.2],[429.4],[445.1],[461.4],[478.3],[495.8],[514.0],[532.8],[552.3],[572.5],[593.5],[615.3],[637.8],[661.2],[685.4],[710.5],[736.5])" +
"(SELECT [PointId],[DateTime],[13.6],[14.1],[14.6],[15.1],[15.7],[16.3],[16.8],[17.5],[18.1],[18.8],[19.5],[20.2],[20.9],[21.7],[22.5],[23.3],[24.1],[25.0],[25.9],[26.9],[27.9],[28.9],[30.0],[31.1],[32.2],[33.4],[34.6],[35.9],[37.2],[38.5],[40.0],[41.4],[42.9],[44.5],[46.1],[47.8],[49.6],[51.4],[53.3],[55.2],[57.3],[59.4],[61.5],[63.8],[66.1],[68.5],[71.0],[73.7],[76.4],[79.1],[82.0],[85.1],[88.2],[91.4],[94.7],[98.2],[101.8],[105.5],[109.4],[113.4],[117.6],[121.9],[126.3],[131.0],[135.8],[140.7],[145.9],[151.2],[156.8],[162.5],[168.5],[174.7],[181.1],[187.7],[194.6],[201.7],[209.1],[216.7],[224.7],[232.9],[241.4],[250.3],[259.5],[269.0],[278.8],[289.0],[299.6],[310.6],[322.0],[333.8],[346.0],[358.7],[371.8],[385.4],[399.5],[414.2],[429.4],[445.1],[461.4],[478.3],[495.8],[514.0],[532.8],[552.3],[572.5],[593.5],[615.3],[637.8],[661.2],[685.4],[710.5],[736.5] FROM [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_M]" +
" where pointid='" + PointID[0] + "' and datetime>='" + beginTime.ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + endTime.ToString("yyyy-MM-dd 23:59:59") + "')";
                dbhelp.ExecuteNonQuery(insertSqlM, "AMS_MonitoringBusinessConnection");
                #endregion

                isSuccess = true;

                #region 更新审核状态
                if (isSuccess)
                {
                    List<AuditAirLogEntity> airLogList = new List<AuditAirLogEntity>();
                    foreach (string portid in PointID)
                    {
                        for (DateTime date = beginTime; date <= endTime; date = date.AddDays(1))
                        {
                            try
                            {
                                #region 获取审核状态
                                int flag = 0;
                                AuditStatusForDayEntity status = auditStateRep.Retrieve(x => x.PointId == Convert.ToInt32(portid) && x.Date == date).FirstOrDefault();
                                if (status == null)
                                {
                                    status = new AuditStatusForDayEntity();
                                    status.AuditStatusUid = Guid.NewGuid().ToString();
                                    status.ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
                                    status.PointId = Convert.ToInt32(portid);
                                    status.Date = date;
                                    status.IsAuditSuggestion = IsAuditSuggestion;
                                    status.CreatDateTime = DateTime.Now;
                                    status.CreatUser = CreatUser;
                                    status.UpdateDateTime = DateTime.Now;
                                    status.UpdateUser = CreatUser;
                                    status.Status = "";
                                    auditStateRep.Add(status);
                                }
                                else
                                {
                                    flag = 1;
                                    status.UpdateDateTime = DateTime.Now;
                                    status.UpdateUser = CreatUser;
                                }
                                #endregion

                                #region 修改因子审核状态
                                if (flag == 1)//当审核状态有数据时预处理表才会有数据
                                {
                                    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals("airaaira-aira-aira-aira-airaairaaira"))
                                    {
                                        status.SuperStatus = "2";
                                    }

                                    auditStateRep.Update(status);
                                }
                                #endregion

                                #region 记录审核日志
                                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals("airaaira-aira-aira-aira-airaairaaira"))//空气
                                {
                                    AuditAirLogEntity log = new AuditAirLogEntity();
                                    log.AuditLogUid = Guid.NewGuid().ToString();
                                    log.AuditStatusUid = status.AuditStatusUid;
                                    log.Tstamp = date.Date;
                                    log.AuditType = "数据审核";
                                    log.AuditTime = DateTime.Now;
                                    log.OperationTypeEnum = "提交审核";
                                    log.OperationReason = "提交审核";
                                    log.UserIP = UserIP;
                                    log.UserUid = UserUid;
                                    log.Description = "";
                                    log.CreatUser = CreatUser;
                                    log.CreatDateTime = DateTime.Now;
                                    log.UpdateUser = CreatUser;
                                    log.UpdateDateTime = DateTime.Now;
                                    airLogList.Add(log);
                                }
                                #endregion
                            }
                            catch
                            {
                                return false;
                            }
                        }
                    }
                    try
                    {
                        if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals("airaaira-aira-aira-aira-airaairaaira"))//空气
                        {
                            if (airLogList != null && airLogList.Count > 0)
                                logAirRep.BatchAdd(airLogList);
                        }
                    }
                    catch
                    {
                        isSuccess = false;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            #endregion
            return isSuccess;
        }

        #region 重新加载审核数据（非国控点）(单站)
        /// <summary>
        /// 删除审核
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="CreatUser"></param>
        /// <param name="IsAuditSuggestion"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public bool DeleteAudit(string applicationUID, int PointID, DateTime beginTime, DateTime endTime, string CreatUser, string UserIP, string UserUid, string OperationTypeEnum, string reason, string Description)
        {
            try
            {
                Thread t1 = null;
                while (beginTime.Date <= endTime.Date)
                {
                    Mutex mutex = new Mutex();
                    AuditStatusForDayEntity status = logService.RerieveAuditState(PointID, beginTime, endTime, applicationUID);
                    if (status != null)
                    {
                        #region 修改审核状态
                        status.Status = "0";
                        auditStateRep.Update(status);
                        #endregion

                        #region 删除预处理数据、记录日志信息
                        if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//环境空气
                        {
                            #region 删除预处理数据
                            IQueryable<AuditAirInfectantByHourEntity> airHourList = auditDataService.RetrieveAirAuditHourDataByID(PointID, beginTime, endTime);
                            auditAirHourRep.Delete(airHourList);
                            #endregion
                            #region 记录审核日志
                            AuditAirLogEntity log = new AuditAirLogEntity();
                            log.AuditLogUid = Guid.NewGuid().ToString();
                            log.AuditStatusUid = status.AuditStatusUid;
                            log.Tstamp = beginTime.Date;
                            log.AuditType = "数据审核";
                            log.AuditTime = DateTime.Now;
                            log.OperationTypeEnum = OperationTypeEnum;
                            log.OperationReason = reason;
                            log.UserIP = UserIP;
                            log.UserUid = UserUid;
                            log.Description = Description;
                            log.CreatUser = CreatUser;
                            log.CreatDateTime = DateTime.Now;
                            log.UpdateUser = CreatUser;
                            log.UpdateDateTime = DateTime.Now;
                            logAirRep.Add(log);
                            #endregion
                        }
                        else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//地表水
                        {
                            #region 删除预处理数据
                            IQueryable<AuditWaterInfectantByHourEntity> airHourList = auditDataService.RetrieveWaterAuditHourDataByID(PointID, beginTime, endTime);
                            waterHourRep.Delete(airHourList);
                            #endregion
                            #region 记录审核日志
                            AuditWaterLogEntity log = new AuditWaterLogEntity();
                            log.AuditLogUid = Guid.NewGuid().ToString();
                            log.AuditStatusUid = status.AuditStatusUid;
                            log.Tstamp = beginTime.Date;
                            log.AuditType = "数据审核";
                            log.AuditTime = DateTime.Now;
                            log.OperationTypeEnum = OperationTypeEnum;
                            log.OperationReason = reason;
                            log.UserIP = UserIP;
                            log.UserUid = UserUid;
                            log.Description = "删除审核";
                            log.CreatUser = CreatUser;
                            log.CreatDateTime = DateTime.Now;
                            log.UpdateUser = CreatUser;
                            log.UpdateDateTime = DateTime.Now;
                            logAirRep.Add(log);
                            #endregion
                        }
                        #endregion
                    }
                    //else
                    //{
                    //    if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID) && auditDataService.IsExitInfectantBy60Air(PointID, beginTime, endTime)
                    //        || EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID) && auditDataService.IsExitInfectantBy60Water(PointID, beginTime, endTime))
                    //    {
                    //        AuditStatusForDayEntity sta = new AuditStatusForDayEntity();
                    //        sta.AuditStatusUid = Guid.NewGuid().ToString();
                    //        sta.ApplicationUid = applicationUID;
                    //        sta.PointId = PointID;
                    //        sta.Date = beginTime;
                    //        sta.Status = "0";
                    //        sta.CreatDateTime = DateTime.Now;
                    //        sta.CreatUser = CreatUser;
                    //        sta.UpdateDateTime = DateTime.Now;
                    //        sta.UpdateUser = CreatUser;
                    //        auditStateRep.Add(sta);
                    //    }
                    //}
                    #region 加载预处理数据及着色
                    t1 = new Thread(() =>
                    {
                        lock (this)
                        {
                            mutex.WaitOne();
                            preService.PreData_Load(applicationUID, PointID.ToString().Split(','), beginTime, endTime);
                            preService.PreData_SetColor(applicationUID, PointID.ToString().Split(','), "", beginTime, endTime, 0);
                            mutex.ReleaseMutex();
                        }
                    });
                    t1.IsBackground = true;
                    t1.Start();
                    Thread.Sleep(500);
                    #endregion
                    beginTime = beginTime.AddDays(1);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 重新加载审核数据（非国控点）(多站)
        /// <summary>
        /// 删除审核
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="CreatUser"></param>
        /// <param name="IsAuditSuggestion"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public bool DeleteAudit(string applicationUID, string[] PointIDs, DateTime beginTime, DateTime endTime, string CreatUser, string UserIP, string UserUid, string OperationTypeEnum, string reason, string Description)
        {
            try
            {
                //Thread t1 = null;
                while (beginTime.Date <= endTime.Date)
                {
                    for (int i = 0; i < PointIDs.Length; i++)
                    {
                        int PointID = Convert.ToInt32(PointIDs[i]);
                        Mutex mutex = new Mutex();
                        AuditStatusForDayEntity status = logService.RerieveAuditState(PointID, beginTime, endTime, applicationUID);
                        if (status != null)
                        {
                            #region 修改审核状态
                            status.Status = "0";
                            status.UpdateDateTime = DateTime.Now;
                            status.UpdateUser = CreatUser;
                            auditStateRep.Update(status);
                            #endregion

                            #region 删除预处理数据、记录日志信息
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//环境空气
                            {
                                #region 删除预处理数据
                                IQueryable<AuditAirInfectantByHourEntity> airHourList = auditDataService.RetrieveAirAuditHourDataByID(PointID, beginTime, endTime);
                                auditAirHourRep.Delete(airHourList);
                                #endregion

                                #region 删除审核预处理小时状态
                                IQueryable<AuditAirStatusForHourEntity> airstatusHourList = statusAirHourRep.Retrieve(x => x.PointId == PointID && x.Date >= beginTime && x.Date <= endTime && x.PointIdType.Equals("0"));//非超级站
                                statusAirHourRep.Delete(airstatusHourList);
                                #endregion

                                #region 记录审核日志
                                AuditAirLogEntity log = new AuditAirLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = beginTime.Date;
                                log.AuditType = "数据审核";
                                log.AuditTime = DateTime.Now;
                                log.OperationTypeEnum = OperationTypeEnum;
                                log.OperationReason = reason;
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = Description;
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                logAirRep.Add(log);
                                #endregion
                            }
                            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//地表水
                            {
                                #region 删除预处理数据
                                IQueryable<AuditWaterInfectantByHourEntity> airHourList = auditDataService.RetrieveWaterAuditHourDataByID(PointID, beginTime, endTime);
                                waterHourRep.Delete(airHourList);
                                #endregion

                                #region 删除审核预处理小时状态
                                IQueryable<AuditWaterStatusForHourEntity> waterstatusHourList = statusWaterHourRep.Retrieve(x => x.PointId == PointID && x.Date >= beginTime && x.Date <= endTime);
                                statusWaterHourRep.Delete(waterstatusHourList);
                                #endregion

                                #region 记录审核日志
                                AuditWaterLogEntity log = new AuditWaterLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = beginTime.Date;
                                log.AuditType = "数据审核";
                                log.OperationTypeEnum = OperationTypeEnum;
                                log.OperationReason = reason;
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "删除审核";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                logAirRep.Add(log);
                                #endregion
                            }
                            #endregion
                        }
                        #region 加载预处理数据及着色
                        //t1 = new Thread(() =>
                        //{
                        //    lock (this)
                        //    {
                        //mutex.WaitOne();

                        preService.PreData_Load(applicationUID, PointID.ToString().Split(','), beginTime, endTime);//加载预处理数据
                        preService.UpdataAuditHourStatus(applicationUID, PointID.ToString().Split(','), beginTime, endTime, "0");//加载审核状态小时表
                        //preService.PreData_SetColor(applicationUID, PointID.ToString().Split(','), "", beginTime, endTime, 0);//执行审核规则
                        //mutex.ReleaseMutex();
                        //    }
                        //});
                        //t1.IsBackground = true;
                        //t1.Start();
                        //Thread.Sleep(500);
                        #endregion
                    }
                    beginTime = beginTime.AddDays(1);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion


        #region 重新加载审核数据(多站)
        /// <summary>
        /// 删除审核
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="CreatUser"></param>
        /// <param name="IsAuditSuggestion"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public bool DeleteAuditNew(string applicationUID, string[] PointIDs, DateTime beginTime, DateTime endTime, string CreatUser, string UserIP, string UserUid, string OperationTypeEnum, string reason, string Description)
        {
            try
            {
                //Thread t1 = null;
                while (beginTime.Date <= endTime.Date)
                {
                    endTime = beginTime.AddDays(1).Date.AddSeconds(-1);
                    for (int i = 0; i < PointIDs.Length; i++)
                    {
                        int PointID = Convert.ToInt32(PointIDs[i]);
                        Mutex mutex = new Mutex();
                        AuditStatusForDayEntity status = logService.RerieveAuditState(PointID, beginTime, endTime, applicationUID);
                        if (status != null)
                        {
                            #region 修改审核状态
                            status.Status = "0";
                            status.UpdateDateTime = DateTime.Now;
                            status.UpdateUser = CreatUser;
                            auditStateRep.Update(status);
                            #endregion

                            #region 删除预处理数据、记录日志信息
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//环境空气
                            {
                                #region 删除预处理数据
                                IQueryable<AuditAirInfectantByHourEntity> airHourList = auditDataService.RetrieveAirAuditHourDataByID(PointID, beginTime, endTime);
                                auditAirHourRep.Delete(airHourList);
                                #endregion

                                #region 删除审核预处理小时状态
                                IQueryable<AuditAirStatusForHourEntity> airstatusHourList = statusAirHourRep.Retrieve(x => x.PointId == PointID && x.Date >= beginTime && x.Date <= endTime && x.PointIdType.Equals("0"));//非超级站
                                statusAirHourRep.Delete(airstatusHourList);
                                #endregion

                                #region 记录审核日志
                                AuditAirLogEntity log = new AuditAirLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = beginTime.Date;
                                log.AuditType = "数据审核";
                                log.AuditTime = DateTime.Now;
                                log.OperationTypeEnum = OperationTypeEnum;
                                log.OperationReason = reason;
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = Description;
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                logAirRep.Add(log);
                                #endregion
                            }
                            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//地表水
                            {

                                #region 删除预处理数据
                                IQueryable<AuditWaterInfectantByHourEntity> airHourList = auditDataService.RetrieveWaterAuditHourDataByID(PointID, beginTime, endTime);
                                waterHourRep.Delete(airHourList);
                                #endregion

                                #region 删除审核预处理小时状态
                                IQueryable<AuditWaterStatusForHourEntity> waterstatusHourList = statusWaterHourRep.Retrieve(x => x.PointId == PointID && x.Date >= beginTime && x.Date <= endTime);
                                statusWaterHourRep.Delete(waterstatusHourList);
                                #endregion

                                #region 记录审核日志
                                AuditWaterLogEntity log = new AuditWaterLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = beginTime.Date;
                                log.AuditType = "数据审核";
                                log.OperationTypeEnum = OperationTypeEnum;
                                log.OperationReason = reason;
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "删除审核";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                logAirRep.Add(log);
                                #endregion
                            }
                            #endregion
                        }
                        #region 加载预处理数据及着色
                        //t1 = new Thread(() =>
                        //{
                        //    lock (this)
                        //    {
                        //mutex.WaitOne();

                        preService.PreData_Load(applicationUID, PointID.ToString().Split(','), beginTime, endTime);//加载预处理数据
                        preService.UpdataAuditHourStatusNew(applicationUID, PointID.ToString().Split(','), beginTime, endTime, "0");//加载审核状态小时表
                        //preService.PreData_SetColor(applicationUID, PointID.ToString().Split(','), "", beginTime, endTime, 0);//执行审核规则
                        //mutex.ReleaseMutex();
                        //    }
                        //});
                        //t1.IsBackground = true;
                        //t1.Start();
                        //Thread.Sleep(500);
                        #endregion
                    }
                    beginTime = beginTime.AddDays(1);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 重新加载审核数据（国控点）（无锡）
        /// <summary>
        /// 按点位和时间段同步国家审核小时数据
        /// </summary>
        /// <param name="portIds">点位ID(多个点位ID以英文,分割)</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        public bool DeleteAuditWX(string PointIDs, DateTime beginTime, DateTime endTime, string CreatUser, string UserIP, string UserUid, string OperationTypeEnum, string reason, string Description)
        {
            try
            {
                SyncGuoJiaDataService guojiaService = new SyncGuoJiaDataService();
                guojiaService.SyncAuditHourData(PointIDs, controlFactors, beginTime, endTime, true);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion


        #region 重新加载审核数据（国控点）（苏州）
        /// <summary>
        /// 按点位和时间段同步国家审核小时数据
        /// </summary>
        /// <param name="portIds">点位ID(多个点位ID以英文,分割)</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        public bool DeleteAudit(string PointIDs, DateTime beginTime, DateTime endTime, string CreatUser, string UserIP, string UserUid, string OperationTypeEnum, string reason, string Description)
        {
            try
            {
                SyncGuoJiaDataService guojiaService = new SyncGuoJiaDataService();//同步国控点数据
                guojiaService.SyncAuditHourData(PointIDs, controlFactors, beginTime, endTime, true);
            }
            catch
            {
                return false;
            }
            try
            {
                preService.PreData_Load_Super(PointIDs.ToString().Split(','), beginTime, endTime);//同步六个指标外的数据
                preService.PreData_SetColor_Super(PointIDs.ToString().Split(','), "", beginTime, endTime, 0);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion


        #region 重新加载审核数据（超级站）(多站)
        /// <summary>
        /// 删除审核
        /// </summary>
        /// <param name="applicationUID"></param>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="CreatUser"></param>
        /// <param name="IsAuditSuggestion"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public bool DeleteAuditSuper(string applicationUID, string[] PointIDs, DateTime beginTime, DateTime endTime, string CreatUser, string UserIP, string UserUid, string OperationTypeEnum, string reason, string Description)
        {
            MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            try
            {
                //Thread t1 = null;
                while (beginTime.Date <= endTime.Date)
                {
                    for (int i = 0; i < PointIDs.Length; i++)
                    {
                        int PointID = Convert.ToInt32(PointIDs[i]);
                        MonitoringPointEntity pointEntity = g_MonitoringPointAir.RetrieveEntityByPointId(PointID);

                        Mutex mutex = new Mutex();
                        AuditStatusForDayEntity status = logService.RerieveAuditState(PointID, beginTime, endTime, applicationUID);
                        if (status != null)
                        {
                            #region 修改审核状态
                            status.SuperStatus = "0";
                            status.UpdateDateTime = DateTime.Now;
                            status.UpdateUser = CreatUser;
                            auditStateRep.Update(status);
                            #endregion

                            #region 删除预处理数据、记录日志信息
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//环境空气
                            {
                                #region 删除预处理数据
                                IQueryable<AuditAirInfectantByHourEntity> airHourList = auditDataService.RetrieveAirAuditHourDataByID(PointID, beginTime, endTime);
                                auditAirHourRep.Delete(airHourList);
                                #endregion

                                #region 删除审核预处理小时状态
                                IQueryable<AuditAirStatusForHourEntity> airstatusHourList = statusAirHourRep.Retrieve(x => x.PointId == PointID && x.Date >= beginTime && x.Date <= endTime && x.PointIdType.Equals("1"));//超级站
                                statusAirHourRep.Delete(airstatusHourList);
                                #endregion

                                #region 记录审核日志
                                AuditAirLogEntity log = new AuditAirLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = beginTime.Date;
                                log.AuditType = "数据审核";
                                log.AuditTime = DateTime.Now;
                                log.OperationTypeEnum = OperationTypeEnum;
                                log.OperationReason = reason;
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = Description;
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                logAirRep.Add(log);
                                #endregion
                            }
                            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//地表水
                            {
                                #region 删除预处理数据
                                IQueryable<AuditWaterInfectantByHourEntity> airHourList = auditDataService.RetrieveWaterAuditHourDataByID(PointID, beginTime, endTime);
                                waterHourRep.Delete(airHourList);
                                #endregion

                                #region 删除审核预处理小时状态
                                IQueryable<AuditAirStatusForHourEntity> airstatusHourList = statusAirHourRep.Retrieve(x => x.PointId == PointID && x.Date >= beginTime && x.Date <= endTime && x.PointIdType.Equals("1"));//超级站
                                statusAirHourRep.Delete(airstatusHourList);
                                #endregion

                                #region 记录审核日志
                                AuditWaterLogEntity log = new AuditWaterLogEntity();
                                log.AuditLogUid = Guid.NewGuid().ToString();
                                log.AuditStatusUid = status.AuditStatusUid;
                                log.Tstamp = beginTime.Date;
                                log.AuditType = "数据审核";
                                log.OperationTypeEnum = OperationTypeEnum;
                                log.OperationReason = reason;
                                log.UserIP = UserIP;
                                log.UserUid = UserUid;
                                log.Description = "删除审核";
                                log.CreatUser = CreatUser;
                                log.CreatDateTime = DateTime.Now;
                                log.UpdateUser = CreatUser;
                                log.UpdateDateTime = DateTime.Now;
                                logAirRep.Add(log);
                                #endregion
                            }
                            #endregion
                        }
                        #region 加载预处理数据及着色
                        //t1 = new Thread(() =>
                        //{
                        //    lock (this)
                        //    {
                        //        mutex.WaitOne();

                        #region 加载预处理数据
                        if (pointEntity.ContrlUid.Equals("6fadff52-2338-4319-9f1d-7317823770ad"))//超级站且是国控点
                        {
                            try
                            {
                                SyncGuoJiaDataService guojiaService = new SyncGuoJiaDataService();
                                guojiaService.SyncAuditHourData(PointID.ToString(), null, beginTime, endTime, true);
                            }
                            catch
                            {
                            }
                            preService.PreData_Load_Super(PointID.ToString().Split(','), beginTime, endTime);
                            preService.PreData_SetColor_Super(PointID.ToString().Split(','), "", beginTime, endTime, 0);
                        }
                        else
                        {
                            preService.PreData_Load(applicationUID, PointID.ToString().Split(','), beginTime, endTime);
                            preService.PreData_SetColor(applicationUID, PointID.ToString().Split(','), "", beginTime, endTime, 0);
                        }
                        #endregion
                        //加载审核状态小时表
                        preService.UpdataAuditHourStatus(applicationUID, PointID.ToString().Split(','), beginTime, endTime, "1");

                        //        mutex.ReleaseMutex();
                        //    }
                        //});
                        //t1.IsBackground = true;
                        //t1.Start();
                        //Thread.Sleep(500);
                        #endregion
                    }
                    beginTime = beginTime.AddDays(1);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion
        #endregion
        #endregion
    }
}
