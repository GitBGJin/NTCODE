using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.BaseInfoRepository.BusinessRule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SmartEP.DomainModel.BaseData;
using SmartEP.Core.Enums;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.BaseInfoRepository.MPInfo;

namespace SmartEP.Service.BaseData.BusinessRule
{
    /// <summary>
    /// 名称：OfflineSettingService.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 离线规则配置服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class OfflineSettingService
    {
        //离线配置仓储层
        private OfflineSettingRepository offlineSettingRepository = new OfflineSettingRepository();
        //站点信息仓储层
        private MonitoringPointRepository monitoringPointRepository = new MonitoringPointRepository();
        //站点信息服务
        private MonitoringPointService pointService = new MonitoringPointService();

        #region 增删改
        /// <summary>
        /// 增加对象
        /// </summary>
        /// <param name="entity"></param>
        public void Add(OfflineSettingEntity entity)
        {
            offlineSettingRepository.Add(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(OfflineSettingEntity entity)
        {
            offlineSettingRepository.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities"></param>
        public void BatchDelete(List<OfflineSettingEntity> entities)
        {
            offlineSettingRepository.BatchDelete(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            offlineSettingRepository.Update();
        }
        #endregion

        /// <summary>
        /// 根据应用程序值和报警用途返回离线报警设置列表
        /// </summary>
        /// <param name="applicationValue">应用程序值</param>
        /// <returns></returns>
        public IQueryable<OfflineSettingEntity> RetrieveList(ApplicationValue applicationValue,string useForUid)
        {
            string appValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationValue);
            return offlineSettingRepository.Retrieve(p => p.ApplicationUid == appValue && p.UseForUid == useForUid);
        }

        /// <summary>
        /// 根据应用程序值、报警用途、数据类型返回离线报警设置列表
        /// </summary>
        /// <param name="applicationValue">应用程序值</param>
        /// <returns></returns>
        public IQueryable<OfflineSettingEntity> RetrieveListByDataType(string applicationValue, string useForUid,string dataTypeUid)
        {
            return offlineSettingRepository.Retrieve(p => p.ApplicationUid == applicationValue && p.UseForUid == useForUid && p.DataTypeUid == dataTypeUid);
        }


        //<summary>
        //离线报警设置查询（通知级别Uid、采集数据类型Uid、站点Uid）
        //</summary>
        //<param name="notifyGradeUid">通知级别Uid</param>
        //<param name="dataTypeUid">采集数据类型Uid</param>
        //<param name="monitoringPointUid">站点Uid</param>
        //<returns></returns>
        public IQueryable<OfflineSettingEntity> RetrieveList(string notifyGradeUid, string dataTypeUid, string monitoringPointUid)
        {
            IQueryable<OfflineSettingEntity> offlineSettingEntity = offlineSettingRepository.RetrieveAll();
            if (!string.IsNullOrEmpty(monitoringPointUid)) offlineSettingEntity = offlineSettingEntity.Where(p => p.MonitoringPointUid.Contains(monitoringPointUid));
            if (!string.IsNullOrEmpty(dataTypeUid)) offlineSettingEntity = offlineSettingEntity.Where(p => p.DataTypeUid == dataTypeUid);
            if (!string.IsNullOrEmpty(notifyGradeUid)) offlineSettingEntity = offlineSettingEntity.Where(p => p.NotifyGradeUid == notifyGradeUid);
            return offlineSettingEntity;
        }

        /// <summary>
        /// 根据应用程序值、报警用途、数据类型返回离线报警设置的多个站点id
        /// </summary>
        /// <param name="applicationValue"></param>
        /// <param name="useForUid"></param>
        /// <param name="dataTypeUid"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrievePointList(string applicationValue, string useForUid, string dataTypeUid)
        {
            IQueryable<MonitoringPointEntity> pointEntities = from o in offlineSettingRepository.Retrieve(p => p.ApplicationUid == applicationValue && p.UseForUid == useForUid && p.DataTypeUid == dataTypeUid)
                                                             join m in monitoringPointRepository.RetrieveAll() on o.MonitoringPointUid equals m.MonitoringPointUid
                                                             select m;
           return pointEntities;  
        }
    }
}
