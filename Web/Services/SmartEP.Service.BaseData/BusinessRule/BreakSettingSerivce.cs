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

namespace SmartEP.Service.BaseData.BusinessRule
{
    /// <summary>
    /// 名称：BreakSettingSerivce.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 突变规则配置服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class BreakSettingSerivce
    {
        private BreakSettingRepository g_Repository = Singleton<BreakSettingRepository>.GetInstance();
        private string g_ApplicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Air);

        #region 增删改
        /// <summary>
        /// 增加突变报警设置
        /// </summary>
        /// <param name="breakSetting">实体</param>
        public void Add(BreakSettingEntity breakSetting)
        {
            g_Repository.Add(breakSetting);
        }

        /// <summary>
        /// 更新突变报警设置
        /// </summary>
        /// <param name="breakSetting">实体</param>
        public void Update(BreakSettingEntity breakSetting)
        {
            g_Repository.Update(breakSetting);
        }

        /// <summary>
        /// 删除突变报警设置
        /// </summary>
        /// <param name="breakSetting">实体</param>
        public void Delete(BreakSettingEntity breakSetting)
        {
            g_Repository.Delete(breakSetting);
        }
        #endregion

        /// <summary>
        /// 根据应用程序值返回突变报警设置列表
        /// </summary>
        /// <param name="applicationValue">应用程序值</param>
        /// <returns></returns>
        public IQueryable<BreakSettingEntity> RetrieveListByApplication(ApplicationValue applicationValue)
        {
            string appValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationValue);
            return g_Repository.Retrieve(p => p.ApplicationUid == appValue);
        }


        //<summary>
        //突变报警设置查询（仪器通道Uid、采集数据类型Uid、站点Uid）
        //</summary>
        //<param name="instrumentChannelsUid">仪器通道Uid</param>
        //<param name="dataTypeUid">采集数据类型Uid</param>
        //<param name="monitoringPointUid">站点Uid</param>
        //<returns></returns>
        public IQueryable<BreakSettingEntity> RetrieveList(string instrumentChannelsUid, string dataTypeUid, string monitoringPointUid)
        {
            IQueryable<BreakSettingEntity> offlineSettingEntity = g_Repository.RetrieveAll();
            if (!string.IsNullOrEmpty(monitoringPointUid)) offlineSettingEntity = offlineSettingEntity.Where(p => p.MonitoringPointUid.Contains(monitoringPointUid));
            if (!string.IsNullOrEmpty(dataTypeUid)) offlineSettingEntity = offlineSettingEntity.Where(p => p.DataTypeUid == dataTypeUid);
            if (!string.IsNullOrEmpty(instrumentChannelsUid)) offlineSettingEntity = offlineSettingEntity.Where(p => p.InstrumentChannelsUid == instrumentChannelsUid);
            return offlineSettingEntity;
        }

        /// <summary>
        /// 根据应用程序值、报警用途、数据类型返回离线报警设置列表
        /// </summary>
        /// <param name="applicationValue">应用程序值</param>
        /// <param name="useForUid">用途Uid</param>
        /// <param name="dataTypeUid">数据类型Uid</param>
        /// <returns></returns>
        public IQueryable<BreakSettingEntity> RetrieveListByDataType(string applicationValue, string useForUid, string dataTypeUid)
        {
            return g_Repository.Retrieve(p => p.ApplicationUid == applicationValue && p.UseForUid == useForUid && p.DataTypeUid == dataTypeUid && p.EnableOrNot == true);
        }

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<BreakSettingEntity> Retrieve(Expression<Func<BreakSettingEntity, bool>> predicate)
        {
            return g_Repository.Retrieve(predicate);
        }

        /// <summary>
        /// 设置突变
        /// </summary>
        /// <param name="ApplicationUid">系统Uid</param>
        /// <param name="DataTypeUid">数据类型Uid</param>
        /// <param name="PointUid">测点Uid</param>
        /// <param name="PollutantUid">因子Uid</param>
        /// <param name="ColumnName">设置属性</param>
        /// <param name="Value">设置值</param>
        public void InsertOrUpdate(string ApplicationUid, string DataTypeUid, string PointUid, string PollutantUid, string ColumnName, string Value)
        {
            g_Repository.InsertOrUpdate(ApplicationUid, DataTypeUid, PointUid, PollutantUid, ColumnName, Value);
        }
    }
}
