using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAuditing.AuditBaseInfo
{
    /// <summary>
    /// 名称：AbstractMonitoringPoint.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-16
    /// 维护人员：
    /// 最新维护人员：季柯
    /// 最新维护日期：2015-08-27
    /// 功能摘要：
    /// 审核站点抽象类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public abstract class AbstractAuditMonitoringPoint
    {
        public AuditMonitoringPointRepository auditmpRepository = new AuditMonitoringPointRepository();
        public MonitoringPointRepository mpRepository = new MonitoringPointRepository();
        #region 增删改
        /// <summary>
        /// 增加审核站点
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Add(AuditMonitoringPointEntity monitoringPoint)
        {
            auditmpRepository.Add(monitoringPoint);
        }

        /// <summary>
        /// 更新审核站点
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Update(AuditMonitoringPointEntity monitoringPoint)
        {
            auditmpRepository.Update(monitoringPoint);
        }

        /// <summary>
        /// 删除审核站点
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Delete(AuditMonitoringPointEntity monitoringPoint)
        {
            auditmpRepository.Delete(monitoringPoint);
        }
        #endregion

        /// <summary>
        /// 获取所有审核站点信息
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPList(string UserGuid)
        {
            IQueryable<MonitoringPointEntity> allPoints = (from item in mpRepository.RetrieveAll().ToList<MonitoringPointEntity>()
                                                           join site in mpRepository.Context.GetAll<V_Point_UserConfigEntity>().Where(p => p.EnableCustomOrNot == true && p.IsUseOrNot == true&&p.UserGuid==UserGuid)
                                                           on item.MonitoringPointUid equals site.MonitoringPointUid
                                                           select item).AsQueryable();
            IQueryable<MonitoringPointEntity> pointList = (from item in allPoints.ToList<MonitoringPointEntity>()
                                                           join t in auditmpRepository.RetrieveAll().ToList<AuditMonitoringPointEntity>() on item.MonitoringPointUid equals t.MonitoringPointUid
                                                           where t.PointType != 1
                                                           select item).AsQueryable();
            return pointList;
        }

        /// <summary>
        /// 获取所有国控点、超级站
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPListSuper(string UserGuid)
        {
            IQueryable<MonitoringPointEntity> allPoints = (from item in mpRepository.RetrieveAll().ToList<MonitoringPointEntity>()
                                                           join site in mpRepository.Context.GetAll<V_Point_UserConfigEntity>().Where(p => p.EnableCustomOrNot == true && p.IsUseOrNot == true && p.UserGuid == UserGuid)
                                                           on item.MonitoringPointUid equals site.MonitoringPointUid
                                                           select item).AsQueryable();
            IQueryable<MonitoringPointEntity> pointList = (from item in allPoints.ToList<MonitoringPointEntity>()
                                                           join t in auditmpRepository.RetrieveAll().ToList<AuditMonitoringPointEntity>() on item.MonitoringPointUid equals t.MonitoringPointUid
                                                           where t.PointType==1
                                                           select item).AsQueryable();
            return pointList;
        }

        //public IQueryable<MonitoringPointEntity> RetrieveMPList()
        //{
        //    IQueryable<MonitoringPointEntity> pointList = (from item in mpRepository.RetrieveAll().ToList<MonitoringPointEntity>()
        //             join t in auditmpRepository.RetrieveAll().ToList<AuditMonitoringPointEntity>() on item.MonitoringPointUid equals t.MonitoringPointUid
        //                                                   //where item.EnableOrNot == true
        //             select item).AsQueryable();
        //    return pointList;
        //}

        /// <summary>
        /// 根据审核类型Uid获取所有审核站点
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<AuditMonitoringPointEntity> RetrieveAuditMPListByAuditTypeUid(string auditTypeUid)
        {
            return auditmpRepository.Retrieve(p => p.AuditTypeUid == auditTypeUid);
        }

        /// <summary>
        /// 根据审核类型Uid获取所有站点
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPListByAuditTypeUid(string auditTypeUid)
        {
            IQueryable<MonitoringPointEntity> pointList = (from item in mpRepository.RetrieveAll().ToList<MonitoringPointEntity>()
                                                           join t in RetrieveAuditMPListByAuditTypeUid(auditTypeUid).ToList<AuditMonitoringPointEntity>() on item.MonitoringPointUid equals t.MonitoringPointUid
                                                           select item).AsQueryable();
            return pointList;
        }

        /// <summary>
        /// 根据审核站点Uid获取审核站点
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public AuditMonitoringPointEntity RetrieveEntityByAuditPointUid(string auditMonitoringPointUid)
        {
            return auditmpRepository.Retrieve(p => p.AuditMonitoringPointUid == auditMonitoringPointUid).FirstOrDefault();
        }

        /// <summary>
        /// 根据站点Uid获取审核站点
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public AuditMonitoringPointEntity RetrieveEntityByPointUid(string monitoringPointUid)
        {
            return auditmpRepository.Retrieve(p => p.MonitoringPointUid == monitoringPointUid).FirstOrDefault();
        }
        /// <summary>
        /// 根据站点ID获取审核站点
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public AuditMonitoringPointEntity RetrieveEntityByPointId(int pointId)
        {
            MonitoringPointEntity pointEntity = mpRepository.RetrieveFirstOrDefault(p => p.PointId == pointId);
            return RetrieveEntityByPointUid(pointEntity == null ? "" : pointEntity.MonitoringPointUid);
        }

        /// <summary>
        /// 根据站点Uid数组获取站点实体集合
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<AuditMonitoringPointEntity> RetrieveListByPointUids(string[] pointUids)
        {
            return auditmpRepository.RetrieveAll().Where(p => pointUids.Contains(p.MonitoringPointUid));
        }

        /// <summary>
        /// 根据审核站点实体集合获取站点Uid数组
        /// </summary>
        /// <param name="pointEntities">审核站点实体集合</param>
        /// <returns></returns>
        public List<string> RetrieveListByPointUids(IQueryable<AuditMonitoringPointEntity> pointEntities)
        {
            return pointEntities.Select(p => p.MonitoringPointUid.ToString()).ToList();
        }
    }
}
