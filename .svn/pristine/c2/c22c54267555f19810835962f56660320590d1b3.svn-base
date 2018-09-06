using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.MPInfo
{
    /// <summary>
    /// 名称：AbstractMonitoringPoint.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-16
    /// 维护人员：
    /// 最新维护人员：季柯
    /// 最新维护日期：2015-08-27
    /// 功能摘要：
    /// 站点信息抽象类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public abstract class AbstractMonitoringPoint
    {
        public MonitoringPointRepository monitoringRepository = new MonitoringPointRepository();
        public MonitoringPointExtensionForEQMSAirRepository monitoringAirRepository = new MonitoringPointExtensionForEQMSAirRepository();
        public MonitoringPointExtensionForEQMSWaterRepository monitoringWaterRepository = new MonitoringPointExtensionForEQMSWaterRepository();
        #region 增删改
        /// <summary>
        /// 增加站点
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Add(MonitoringPointEntity monitoringPoint)
        {
            monitoringRepository.Add(monitoringPoint);
        }

        /// <summary>
        /// 增加空气扩展信息
        /// </summary>
        /// <param name="monitoringPointAir">空气扩展信息实体</param>
        public void Add(MonitoringPointExtensionForEQMSAirEntity monitoringPointAir)
        {
            monitoringAirRepository.Add(monitoringPointAir);
        }

        /// <summary>
        /// 增加地表水扩展信息
        /// </summary>
        /// <param name="monitoringPointWater">地表水扩展信息实体</param>
        public void Add(MonitoringPointExtensionForEQMSWaterEntity monitoringPointWater)
        {
            monitoringWaterRepository.Add(monitoringPointWater);
        }

        /// <summary>
        /// 更新站点
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Update(MonitoringPointEntity monitoringPoint)
        {
            monitoringRepository.Update(monitoringPoint);
        }

        /// <summary>
        /// 更新空气站点扩展信息
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Update(MonitoringPointExtensionForEQMSAirEntity monitoringPoint)
        {
            monitoringAirRepository.Update(monitoringPoint);
        }

        /// <summary>
        /// 更新地表水站点扩展信息
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Update(MonitoringPointExtensionForEQMSWaterEntity monitoringPoint)
        {
            monitoringWaterRepository.Update(monitoringPoint);
        }


        /// <summary>
        /// 删除站点
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Delete(MonitoringPointEntity monitoringPoint)
        {
            monitoringRepository.Delete(monitoringPoint);
        }

        /// <summary>
        /// 批量删除点位信息
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void BatchDelete(List<MonitoringPointEntity> monitoringPoints)
        {
            monitoringRepository.BatchDelete(monitoringPoints);
        }

        /// <summary>
        /// 删除空气站点扩展信息
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Delete(MonitoringPointExtensionForEQMSAirEntity monitoringPoint)
        {
            monitoringAirRepository.Delete(monitoringPoint);
        }

        /// <summary>
        /// 批量删除空气点位信息
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void BatchDelete(List<MonitoringPointExtensionForEQMSAirEntity> monitoringPoints)
        {
            monitoringAirRepository.BatchDelete(monitoringPoints);
        }

        /// <summary>
        /// 删除地表水站点扩展信息
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Delete(MonitoringPointExtensionForEQMSWaterEntity monitoringPoint)
        {
            monitoringWaterRepository.Delete(monitoringPoint);
        }

        /// <summary>
        /// 批量删除地表水点位信息
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void BatchDelete(List<MonitoringPointExtensionForEQMSWaterEntity> monitoringPoints)
        {
            monitoringWaterRepository.BatchDelete(monitoringPoints);
        }
        #endregion

        /// <summary>
        /// 根据站点Uid获取站点
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public MonitoringPointEntity RetrieveEntityByUid(string monitoringPointUid)
        {
            return monitoringRepository.Retrieve(p => p.MonitoringPointUid == monitoringPointUid).FirstOrDefault();
        }
        /// <summary>
        /// 根据站点ID获取站点
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public MonitoringPointEntity RetrieveEntityByPointId(int pointId)
        {
            return monitoringRepository.Retrieve(p => p.PointId == pointId).FirstOrDefault();
        }

        /// <summary>
        /// 根据站点ID数组获取站点
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveListByPointIds(string[] pointIds)
        {
            return monitoringRepository.RetrieveAll().Where(p => pointIds.Contains(p.PointId.ToString()));
        }

        /// <summary>
        /// 根据集成商数组获取站点
        /// </summary>
        /// <param name="integratorUids"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveListByintegratorUids(string[] integratorUids)
        {
            return monitoringRepository.RetrieveAll().Where(p => integratorUids.Contains(p.InstrumentIntegratorUid.ToString()));
        }

        /// <summary>
        /// 根据站点Uid数组获取站点实体集合
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveListByPointUids(string[] pointUids)
        {
            return monitoringRepository.RetrieveAll().Where(p => pointUids.Contains(p.MonitoringPointUid));
        }

        /// <summary>
        /// 根据站点Uid数组获取空气扩展站点实体集合
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointExtensionForEQMSAirEntity> RetrieveAirExtensionPointListByPointUids(string[] pointUids)
        {
            return monitoringAirRepository.Retrieve(p => pointUids.Contains(p.MonitoringPointUid));
        }

        /// <summary>
        /// 根据站点Uid数组获取地表水扩展站点实体集合
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointExtensionForEQMSWaterEntity> RetrieveWaterExtensionPointListByPointUids(string[] pointUids)
        {
            return monitoringWaterRepository.Retrieve(p => pointUids.Contains(p.MonitoringPointUid));
        }

        /// <summary>
        /// 根据站点Uid数组获取站点名称字符串，多个站点以逗号","隔开
        /// </summary>
        /// <param name="pointUids"></param>
        /// <returns></returns>
        public string RetrievePointNamesByPointUids(string[] pointUids)
        {
            return SmartEP.Utilities.DataTypes.ExtensionMethods.StringExtensions.GetArrayStrNoEmpty(monitoringRepository.Retrieve(p => pointUids.Contains(p.MonitoringPointUid)).Select(p => p.MonitoringPointName).ToList(), ";");
        }

        /// <summary>
        /// 根据站点实体集合获取站点Id数组
        /// </summary>
        /// <param name="pointEntities">站点实体集合</param>
        /// <returns></returns>
        public List<string> RetrieveListByPointIds(IQueryable<MonitoringPointEntity> pointEntities)
        {
            return pointEntities.Select(p => p.PointId.ToString()).ToList();
        }

        /// <summary>
        /// 根据站点实体集合获取站点Uid数组
        /// </summary>
        /// <param name="pointEntities">站点实体集合</param>
        /// <returns></returns>
        public List<string> RetrieveListByPointUids(IQueryable<MonitoringPointEntity> pointEntities)
        {
            return pointEntities.Select(p => p.MonitoringPointUid.ToString()).ToList();
        }

        /// <summary>
        /// 更新点位状态
        /// </summary>
        /// <param name="pointId">点位id</param>
        /// <param name="runStatusUid">状态Uid</param>
        /// <returns></returns>
        public void UpdateRunStatus(int pointId, string runStatusUid)
        {
            MonitoringPointEntity entity = monitoringRepository.RetrieveFirstOrDefault(p => p.PointId == pointId);
            if (entity != null) entity.RunStatusUid = runStatusUid;
            monitoringRepository.Update(entity);
        }
    }
}
