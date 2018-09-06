using SmartEP.BaseInfoRepository.Channel;
using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Core.Enums;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;

namespace SmartEP.Service.DataAuditing.AuditBaseInfo
{
    /// <summary>
    /// 名称：AuditMonitoringPointPollutantService.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 审核因子
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditMonitoringPointPollutantService
    {
        public AuditMonitoringPointRepository auditmpRepository = new AuditMonitoringPointRepository();
        public AuditMonitoringPointPollutantRepository auditmpPollutantRepository = new AuditMonitoringPointPollutantRepository();
        public PollutantCodeRepository pollutantCodeRepository = new PollutantCodeRepository();
        AirAuditMonitoringPointService airPointService = new AirAuditMonitoringPointService();
        WaterAuditMonitoringPointService waterPointService = new WaterAuditMonitoringPointService();
        #region 增删改
        /// <summary>
        /// 增加审核站点因子
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Add(AuditMonitoringPointPollutantEntity monitoringPoint)
        {
            auditmpRepository.Add(monitoringPoint);
        }

        /// <summary>
        /// 更新审核站点因子
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Update(AuditMonitoringPointPollutantEntity monitoringPoint)
        {
            auditmpRepository.Update(monitoringPoint);
        }

        /// <summary>
        /// 删除审核站点因子
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Delete(AuditMonitoringPointPollutantEntity monitoringPoint)
        {
            auditmpRepository.Delete(monitoringPoint);
        }
        #endregion

        /// <summary>
        /// 根据Uid获取审核因子
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public IQueryable<AuditMonitoringPointPollutantEntity> RetrieveListByUid(string auditMonitoringPointUid)
        {
            return auditmpPollutantRepository.Retrieve(p => p.AuditMonitoringPointUid == auditMonitoringPointUid);
        }

        /// <summary>
        /// 根据站点Uid获取审核因子
        /// </summary>
        /// <param name="monitoringPointUid">实体</param>
        public IQueryable<AuditMonitoringPointPollutantEntity> RetrieveListByPointUid(string monitoringPointUid)
        {
            return auditmpPollutantRepository.Retrieve(p => p.AuditMonitoringPointEntity.MonitoringPointUid == monitoringPointUid);
        }

        /// <summary>
        /// 更加测点id、应用程序guid获取因子列表（单站）
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public IQueryable<AuditMonitoringPointPollutantEntity> RetrieveListByPointID(int PointID, string application, string UserGuid)
        {
            string monitoringPointUid = "";
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(application))
                monitoringPointUid = airPointService.RetrieveAirMPList(UserGuid).Where(x => x.PointId == PointID).Select(x => x.MonitoringPointUid).FirstOrDefault();
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(application))
                monitoringPointUid = waterPointService.RetrieveWaterMPList(UserGuid).Where(x => x.PointId == PointID).Select(x => x.MonitoringPointUid).FirstOrDefault();
            return auditmpPollutantRepository.Retrieve(p => p.AuditMonitoringPointEntity.MonitoringPointUid.Trim().ToUpper().Equals(monitoringPointUid.Trim().ToUpper()));
        }

        /// <summary>
        ///根据测点id、应用程序guid获取因子列表（多站）
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public IQueryable<AuditMonitoringPointPollutantEntity> RetrieveListByPointID(string[] PointID, string application, string UserGuid, int PointType = 0)
        {

            string[] monitoringPointUid = null;
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(application))//气
            {
                if (PointType == 0)//非超级站
                {
                    monitoringPointUid = airPointService.RetrieveAirMPList(UserGuid).Where(x => PointID.Contains(x.PointId.ToString())).Select(x => x.MonitoringPointUid).ToArray();
                    return auditmpPollutantRepository.Retrieve(p => monitoringPointUid.Contains(p.AuditMonitoringPointEntity.MonitoringPointUid.Trim()) && (p.AuditMonitoringPointEntity.PointType == null || p.AuditMonitoringPointEntity.PointType != 1));
                }
                else
                {
                    //超级站
                    monitoringPointUid = airPointService.RetrieveAirMPListBySuper(UserGuid).Where(x => PointID.Contains(x.PointId.ToString())).Select(x => x.MonitoringPointUid).ToArray();
                    return auditmpPollutantRepository.Retrieve(p => monitoringPointUid.Contains(p.AuditMonitoringPointEntity.MonitoringPointUid.Trim()) && p.AuditMonitoringPointEntity.PointType == 1);
                }

            }
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(application))//地表水
                monitoringPointUid = waterPointService.RetrieveWaterMPList(UserGuid).Where(x => PointID.Contains(x.PointId.ToString())).Select(x => x.MonitoringPointUid).ToArray();
            return auditmpPollutantRepository.Retrieve(p => monitoringPointUid.Contains(p.AuditMonitoringPointEntity.MonitoringPointUid.Trim()));
            #region 注释
            //string monitoringPointUid = "";
            //if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(application))
            //{
            //    if (PointType == 0)
            //    {
            //        monitoringPointUid = airPointService.RetrieveAirMPList(UserGuid).Where(x => PointID.Contains(x.PointId.ToString())).Select(x => x.MonitoringPointUid).FirstOrDefault();
            //        return auditmpPollutantRepository.Retrieve(p => monitoringPointUid.Contains(p.AuditMonitoringPointEntity.MonitoringPointUid.Trim()) && (p.AuditMonitoringPointEntity.PointType != 1 || p.AuditMonitoringPointEntity.PointType==null));
            //    }
            //    else
            //    {
            //        monitoringPointUid = airPointService.RetrieveAirMPListBySuper(UserGuid).Where(x => PointID.Contains(x.PointId.ToString())).Select(x => x.MonitoringPointUid).FirstOrDefault();
            //        return auditmpPollutantRepository.Retrieve(p => monitoringPointUid.Contains(p.AuditMonitoringPointEntity.MonitoringPointUid.Trim()) && p.AuditMonitoringPointEntity.PointType == 1);
            //    }

            //}
            //else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(application))
            //    monitoringPointUid = waterPointService.RetrieveWaterMPList(UserGuid).Where(x => PointID.Contains(x.PointId.ToString())).Select(x => x.MonitoringPointUid).FirstOrDefault();
            //return auditmpPollutantRepository.Retrieve(p => p.AuditMonitoringPointEntity.MonitoringPointUid.Trim().ToUpper().Equals(monitoringPointUid.Trim().ToUpper()));
            #endregion
        }

        /// <summary>
        /// 根据点位类型、应用程序guid获取因子列表（多站）
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public IQueryable<AuditMonitoringPointPollutantEntity> RetrieveListByPointType(string PointType, string application, string UserGuid)
        {
            string[] monitoringPointUid = null;
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(application))
                monitoringPointUid = airPointService.RetrieveAirMPList(UserGuid).Where(x => x.SiteTypeUid.Equals(PointType)).Select(x => x.MonitoringPointUid).ToArray();
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(application))
                monitoringPointUid = waterPointService.RetrieveWaterMPList(UserGuid).Where(x => x.SiteTypeUid.Equals(PointType)).Select(x => x.MonitoringPointUid).ToArray();
            return auditmpPollutantRepository.Retrieve(p => monitoringPointUid.Contains(p.AuditMonitoringPointEntity.MonitoringPointUid.Trim()));
        }

        /// <summary>
        /// 根据站点Uid获取审核因子
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<PollutantCodeEntity> RetrievePollutantListByPointUid(string monitoringPointUid)
        {
            IQueryable<PollutantCodeEntity> pollutantList = (from item in pollutantCodeRepository.RetrieveAll().ToList()
                                                             join t in auditmpPollutantRepository.Retrieve(p => p.AuditMonitoringPointEntity.MonitoringPointUid == monitoringPointUid).ToList()
                                                             on item.PollutantUid equals t.PollutantUid
                                                             select item).AsQueryable();
            return pollutantList;
        }


        /// <summary>
        /// 根据站点Uid数组获取审核因子
        /// </summary>
        /// <param name="IQueryable<PollutantCodeEntity>">实体</param>
        public IQueryable<PollutantCodeEntity> RetrievePollutantListByPointUids(string[] monitoringPointUids)
        {
            IQueryable<PollutantCodeEntity> pollutantList = from item in pollutantCodeRepository.RetrieveAll()
                                                            join t in auditmpPollutantRepository.Retrieve(p => monitoringPointUids.Contains(p.AuditMonitoringPointEntity.MonitoringPointUid))
                                                            on item.PollutantUid equals t.PollutantUid
                                                            select item;
            return pollutantList.Distinct();
        }

        /// <summary>
        /// 根据点位ID，应用程序ID，用户权限获取所配置的因子
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <param name="applicationUid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<V_Factor_SiteMap_UserConfigEntity> RetrieveSiteMapList(int pointID, string applicationUid, string userGuid)
        {
            IQueryable<V_Factor_SiteMap_UserConfigEntity> factorSiteMap = pollutantCodeRepository.Context.GetAll<V_Factor_SiteMap_UserConfigEntity>().Where(p => p.ApplicationUid == applicationUid && p.UserGuid == userGuid);
            IQueryable<AuditMonitoringPointPollutantEntity> auditPollutant = RetrieveListByPointID(pointID, applicationUid, userGuid);
            IQueryable<V_Factor_SiteMap_UserConfigEntity> factorList = (from f in factorSiteMap.ToList<V_Factor_SiteMap_UserConfigEntity>()
                                                                        join a in auditPollutant.ToList<AuditMonitoringPointPollutantEntity>() on f.GID equals a.PollutantUid
                                                                        select f).AsQueryable();
            string[] PGuid = (from b in factorList select b.PGuid).Distinct().ToArray();
            IQueryable<V_Factor_SiteMap_UserConfigEntity> auditFactorList = (from f in factorSiteMap
                                                                             where f.PGuid == null && PGuid.Contains(f.CGuid)
                                                                             select f).Union(factorList);
            return auditFactorList;
        }        

        /// <summary>
        /// 根据点位ID，应用程序ID，用户权限获取所配置的因子(单站)
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <param name="applicationUid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<PointPollutantInfo> RetrieveSiteMapPollutantList(int pointID, string applicationUid, string userGuid)
        {
            string[] Codes = new string[] { "a99070", "a99074", "a99072" };
            IQueryable<V_Factor_SiteMap_UserConfigEntity> factorSiteMap = pollutantCodeRepository.Context.GetAll<V_Factor_SiteMap_UserConfigEntity>().Where(p => p.ApplicationUid == applicationUid && p.UserGuid == userGuid);
            IQueryable<AuditMonitoringPointPollutantEntity> auditPollutant = RetrieveListByPointID(pointID, applicationUid, userGuid);
            IQueryable<PointPollutantInfo> factorList = (from f in factorSiteMap.ToList<V_Factor_SiteMap_UserConfigEntity>()
                                                         join a in auditPollutant.ToList<AuditMonitoringPointPollutantEntity>() on f.GID equals a.PollutantUid
                                                         where !Codes.Contains(f.PID)
                                                         select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, PollutantDecimalNum = f.DecimalDigit.Value, PollutantUnit = f.MeasureUnitName, Tag = a.ReadOnly == null ? "0" : (a.ReadOnly.Value == true ? "1" : "0") }).AsQueryable();//子节点
            string[] PGuid = (from b in factorList select b.PGuid).Distinct().ToArray();
            IQueryable<PointPollutantInfo> auditFactorList = (from f in factorSiteMap
                                                              where f.PGuid == null && PGuid.Contains(f.CGuid)
                                                              select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, Tag = "" }).Union(factorList);
            return auditFactorList;
        }

        /// <summary>
        /// 根据点位ID，仪器id、应用程序ID，用户权限获取所配置的因子(单站)
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <param name="applicationUid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<PointPollutantInfo> RetrieveSiteMapPollutantList(int pointID,string InsId, string applicationUid, string userGuid)
        {
            string[] Codes = new string[] { "a99070", "a99074", "a99072" };
            IQueryable<V_Factor_SiteMap_UserConfigEntity> factorSiteMap = pollutantCodeRepository.Context.GetAll<V_Factor_SiteMap_UserConfigEntity>().Where(p => p.ApplicationUid == applicationUid && p.UserGuid == userGuid);
            IQueryable<AuditMonitoringPointPollutantEntity> auditPollutant = RetrieveListByPointID(pointID, applicationUid, userGuid);
            IQueryable<PointPollutantInfo> factorList = (from f in factorSiteMap.ToList<V_Factor_SiteMap_UserConfigEntity>()
                                                         join a in auditPollutant.ToList<AuditMonitoringPointPollutantEntity>() on f.GID equals a.PollutantUid
                                                         where !Codes.Contains(f.PID)
                                                         select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, PollutantDecimalNum = f.DecimalDigit.Value, PollutantUnit = f.MeasureUnitName, Tag = a.ReadOnly == null ? "0" : (a.ReadOnly.Value == true ? "1" : "0") }).AsQueryable();//子节点
            string[] PGuid = (from b in factorList select b.PGuid).Distinct().ToArray();
            IQueryable<PointPollutantInfo> auditFactorList = (from f in factorSiteMap
                                                              where f.PGuid == null && PGuid.Contains(f.CGuid) && f.CGuid == InsId
                                                              select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, Tag = "" }).Union(factorList);
            return auditFactorList;
        }

        /// <summary>
        /// 根据点位ID，应用程序ID，用户权限获取所配置的因子(单站)
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <param name="applicationUid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<PointPollutantInfo> RetrieveHgSiteMapPollutantList(int pointID, string applicationUid, string userGuid)
        {
            string[] HgCodes = new string[] { "a99070", "a99074", "a99072" };
            IQueryable<V_Factor_SiteMap_UserConfigEntity> factorSiteMap = pollutantCodeRepository.Context.GetAll<V_Factor_SiteMap_UserConfigEntity>().Where(p => p.ApplicationUid == applicationUid && p.UserGuid == userGuid);
            IQueryable<AuditMonitoringPointPollutantEntity> auditPollutant = RetrieveListByPointID(pointID, applicationUid, userGuid);
            IQueryable<PointPollutantInfo> factorList = (from f in factorSiteMap.ToList<V_Factor_SiteMap_UserConfigEntity>()
                                                         join a in auditPollutant.ToList<AuditMonitoringPointPollutantEntity>() on f.GID equals a.PollutantUid
                                                         where HgCodes.Contains(f.PID)
                                                         select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, PollutantDecimalNum = f.DecimalDigit.Value, PollutantUnit = f.MeasureUnitName, Tag = a.ReadOnly == null ? "0" : (a.ReadOnly.Value == true ? "1" : "0") }).AsQueryable();//子节点
            string[] PGuid = (from b in factorList select b.PGuid).Distinct().ToArray();
            IQueryable<PointPollutantInfo> auditFactorList = (from f in factorSiteMap
                                                              where f.PGuid == null && PGuid.Contains(f.CGuid)
                                                              select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, Tag = "" }).Union(factorList);
            return auditFactorList;
        }

        /// <summary>
        /// 根据点位ID，应用程序ID，用户权限获取Hg的因子(超级站)
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <param name="applicationUid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<PointPollutantInfo> RetrieveHgSiteMapPollutantList(string[] pointID, string applicationUid, string userGuid, int PointType = 0)
        {
            string[] HgCodes = new string[] { "a99070", "a99074", "a99072" };
            IQueryable<V_Factor_SiteMap_UserConfigEntity> factorSiteMap = pollutantCodeRepository.Context.GetAll<V_Factor_SiteMap_UserConfigEntity>().Where(p => p.ApplicationUid == applicationUid && p.UserGuid == userGuid);
            IQueryable<AuditMonitoringPointPollutantEntity> auditPollutant = RetrieveListByPointID(pointID, applicationUid, userGuid, PointType);
            IQueryable<PointPollutantInfo> factorList = (from f in factorSiteMap.ToList<V_Factor_SiteMap_UserConfigEntity>()
                                                         join a in auditPollutant.ToList<AuditMonitoringPointPollutantEntity>() on f.GID.ToLower() equals a.PollutantUid.ToLower()
                                                         where HgCodes.Contains(f.PID)
                                                         select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, POrder = f.POrder, COrder = f.COrder, PollutantDecimalNum = f.DecimalDigit.Value, PollutantUnit = f.MeasureUnitName, Tag = a.ReadOnly == null ? "0" : (a.ReadOnly.Value == true ? "1" : "0") }).AsQueryable();//子节点
            string[] PGuid = (from b in factorList select b.PGuid).Distinct().ToArray();

            factorList = from f in factorList
                         group f by f.PID into g
                         select new PointPollutantInfo
                         {
                             PGuid = g.Max(x => x.PGuid),
                             PID = (g.Max(x => x.PID) != null ? g.Max(x => x.PID).ToString() : "")
                                   + ":" + (g.Max(x => x.PollutantDecimalNum) != null ? g.Max(x => x.PollutantDecimalNum).ToString() : "2")
                                   + ":" + (g.Max(x => x.PollutantUnit) != null ? g.Max(x => x.PollutantUnit).ToString() : "")
                                    + ":" + (g.Min(x => x.Tag) != null ? g.Min(x => x.Tag).ToString() : "0"),
                             PName = g.Max(x => x.PName),
                             CGuid = g.Max(x => x.CGuid),
                             POrder = g.Max(x => x.POrder),
                             COrder = g.Max(x => x.COrder),
                             PollutantDecimalNum = g.Max(x => x.PollutantDecimalNum),
                             PollutantUnit = g.Max(x => x.PollutantUnit),
                             Tag = g.Min(x => x.Tag)
                         };
            IQueryable<PointPollutantInfo> auditFactorList = (from f in factorSiteMap
                                                              where f.PGuid == null && PGuid.Contains(f.CGuid)
                                                              select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, POrder = f.POrder, COrder = f.COrder, Tag = "" }).Union(factorList.Distinct()).OrderByDescending(x => x.POrder).OrderByDescending(x => x.COrder);
            return auditFactorList;
        }

        /// <summary>
        /// 根据点位ID，应用程序ID，用户权限获取所配置的因子(多站)
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <param name="applicationUid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<PointPollutantInfo> RetrieveSiteMapPollutantList(string[] pointID, string applicationUid, string userGuid, int PointType = 0)
        {
            string[] Codes = new string[] { "a99070", "a99074", "a99072" };
            IQueryable<V_Factor_SiteMap_UserConfigEntity> factorSiteMap = pollutantCodeRepository.Context.GetAll<V_Factor_SiteMap_UserConfigEntity>().Where(p => p.ApplicationUid == applicationUid && p.UserGuid == userGuid);
            IQueryable<AuditMonitoringPointPollutantEntity> auditPollutant = RetrieveListByPointID(pointID, applicationUid, userGuid, PointType);
            IQueryable<PointPollutantInfo> factorList = (from f in factorSiteMap.ToList<V_Factor_SiteMap_UserConfigEntity>()
                                                         join a in auditPollutant.ToList<AuditMonitoringPointPollutantEntity>() on f.GID.ToLower() equals a.PollutantUid.ToLower()
                                                         where !Codes.Contains(f.PID)
                                                         select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, POrder = f.POrder, COrder = f.COrder, PollutantDecimalNum = f.DecimalDigit.Value, PollutantUnit = f.MeasureUnitName, Tag = a.ReadOnly == null ? "0" : (a.ReadOnly.Value == true ? "1" : "0") }).AsQueryable();//子节点
            string[] PGuid = (from b in factorList select b.PGuid).Distinct().ToArray();

            factorList = from f in factorList
                         group f by f.PID into g
                         select new PointPollutantInfo
                         {
                             PGuid = g.Max(x => x.PGuid),
                             PID = (g.Max(x => x.PID) != null ? g.Max(x => x.PID).ToString() : "")
                                   + ":" + (g.Max(x => x.PollutantDecimalNum) != null ? g.Max(x => x.PollutantDecimalNum).ToString() : "2")
                                   + ":" + (g.Max(x => x.PollutantUnit) != null ? g.Max(x => x.PollutantUnit).ToString() : "")
                                    + ":" + (g.Min(x => x.Tag) != null ? g.Min(x => x.Tag).ToString() : "0"),
                             PName = g.Max(x => x.PName),
                             CGuid = g.Max(x => x.CGuid),
                             POrder = g.Max(x => x.POrder),
                             COrder = g.Max(x => x.COrder),
                             PollutantDecimalNum = g.Max(x => x.PollutantDecimalNum),
                             PollutantUnit = g.Max(x => x.PollutantUnit),
                             Tag = g.Min(x => x.Tag)
                         };
            IQueryable<PointPollutantInfo> auditFactorList = (from f in factorSiteMap
                                                              where f.PGuid == null && PGuid.Contains(f.CGuid)
                                                              select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, POrder = f.POrder, COrder = f.COrder, Tag = "" }).Union(factorList.Distinct()).OrderByDescending(x => x.POrder).OrderByDescending(x => x.COrder);
            return auditFactorList;
        }

        /// <summary>
        /// 根据点位ID，应用程序ID，用户权限获取所配置的因子(多站)(超级站）
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <param name="applicationUid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<PointPollutantInfo> RetrieveSiteMapPollutantList(string[] pointID, string applicationUid, string userGuid, string[] Codes, int PointType = 0)
        {
            IQueryable<V_Factor_SiteMap_UserConfigEntity> factorSiteMap = pollutantCodeRepository.Context.GetAll<V_Factor_SiteMap_UserConfigEntity>().Where(p => p.ApplicationUid == applicationUid && p.UserGuid == userGuid);
            IQueryable<AuditMonitoringPointPollutantEntity> auditPollutant = RetrieveListByPointID(pointID, applicationUid, userGuid, PointType);
            IQueryable<PointPollutantInfo> factorList = (from f in factorSiteMap.ToList<V_Factor_SiteMap_UserConfigEntity>()
                                                         join a in auditPollutant.ToList<AuditMonitoringPointPollutantEntity>() on f.GID.ToLower() equals a.PollutantUid.ToLower()
                                                         where Codes.Contains(f.PID)
                                                         select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, POrder = f.POrder, COrder = f.COrder, PollutantDecimalNum = f.DecimalDigit.Value, PollutantUnit = f.MeasureUnitName, Tag = a.ReadOnly == null ? "0" : (a.ReadOnly.Value == true ? "1" : "0") }).AsQueryable();//子节点
            string[] PGuid = (from b in factorList select b.PGuid).Distinct().ToArray();

            factorList = from f in factorList
                         group f by f.PID into g
                         select new PointPollutantInfo
                         {
                             PGuid = g.Max(x => x.PGuid),
                             PID = (g.Max(x => x.PID) != null ? g.Max(x => x.PID).ToString() : "")
                                   + ":" + (g.Max(x => x.PollutantDecimalNum) != null ? g.Max(x => x.PollutantDecimalNum).ToString() : "2")
                                   + ":" + (g.Max(x => x.PollutantUnit) != null ? g.Max(x => x.PollutantUnit).ToString() : "")
                                    + ":" + (g.Min(x => x.Tag) != null ? g.Min(x => x.Tag).ToString() : "0"),
                             PName = g.Max(x => x.PName),
                             CGuid = g.Max(x => x.CGuid),
                             POrder = g.Max(x => x.POrder),
                             COrder = g.Max(x => x.COrder),
                             PollutantDecimalNum = g.Max(x => x.PollutantDecimalNum),
                             PollutantUnit = g.Max(x => x.PollutantUnit),
                             Tag = g.Min(x => x.Tag)
                         };
            IQueryable<PointPollutantInfo> auditFactorList = (from f in factorSiteMap
                                                              where f.PGuid == null && PGuid.Contains(f.CGuid)
                                                              select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, POrder = f.POrder, COrder = f.COrder, Tag = "" }).Union(factorList.Distinct()).OrderByDescending(x => x.POrder).OrderByDescending(x => x.COrder);
            return auditFactorList;
        }

        /// <summary>
        /// 根据站点类型，应用程序ID，用户权限获取所配置的因子(多站)
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <param name="applicationUid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<PointPollutantInfo> RetrieveSiteMapPollutantList(string pointType, string applicationUid, string userGuid)
        {
            string[] Codes = new string[] { "a99070", "a99074", "a99072" };
            IQueryable<V_Factor_SiteMap_UserConfigEntity> factorSiteMap = pollutantCodeRepository.Context.GetAll<V_Factor_SiteMap_UserConfigEntity>().Where(p => p.ApplicationUid == applicationUid && p.UserGuid == userGuid);
            IQueryable<AuditMonitoringPointPollutantEntity> auditPollutant = RetrieveListByPointType(pointType, applicationUid, userGuid);
            IQueryable<PointPollutantInfo> factorList = (from f in factorSiteMap.ToList<V_Factor_SiteMap_UserConfigEntity>()
                                                         join a in auditPollutant.ToList<AuditMonitoringPointPollutantEntity>() on f.GID.ToLower() equals a.PollutantUid.ToLower()
                                                         where !Codes.Contains(f.PID)
                                                         select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, POrder = f.POrder, COrder = f.COrder, PollutantDecimalNum = f.DecimalDigit.Value, PollutantUnit = f.MeasureUnitName, Tag = a.ReadOnly == null ? "0" : (a.ReadOnly.Value == true ? "1" : "0") }).AsQueryable();//子节点
            string[] PGuid = (from b in factorList select b.PGuid).Distinct().ToArray();
            factorList = from f in factorList
                         group f by f.PID into g
                         select new PointPollutantInfo
                         {
                             PGuid = g.Max(x => x.PGuid),
                             PID = g.Max(x => x.PID),
                             PName = g.Max(x => x.PName),
                             CGuid = g.Max(x => x.CGuid),
                             POrder = g.Max(x => x.POrder),
                             COrder = g.Max(x => x.COrder),
                             PollutantDecimalNum = g.Max(x => x.PollutantDecimalNum),
                             PollutantUnit = g.Max(x => x.PollutantUnit),
                             Tag = g.Min(x => x.Tag)
                         };

            IQueryable<PointPollutantInfo> auditFactorList = (from f in factorSiteMap
                                                              where f.PGuid == null && PGuid.Contains(f.CGuid)
                                                              select new PointPollutantInfo { PGuid = f.PGuid, PID = f.PID, PName = f.PName, CGuid = f.CGuid, POrder = f.POrder, COrder = f.COrder, Tag = "" }).Union(factorList.Distinct()).OrderByDescending(x => x.POrder).OrderByDescending(x => x.COrder);
            return auditFactorList;
        }

        /// <summary>
        /// 获取审核因子配置数量
        /// </summary>
        /// <param name="pointID"></param>
        /// <param name="applicationUid"></param>
        /// <param name="PointType"></param>
        /// <returns></returns>
        public string[] GetAuditFactorsCount(int pointID, string applicationUid, string PointType = "0")
        {
            AuditPollutantRepository hourRep = new AuditPollutantRepository();
            return hourRep.GetAuditFactorsCount(pointID, applicationUid, PointType);
        }
    }
}
