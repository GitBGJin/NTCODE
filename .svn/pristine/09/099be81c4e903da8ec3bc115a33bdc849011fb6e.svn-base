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
    public class RepeatLimitSettingService
    {
        private RepeatLimitSettingRepository g_Repository = new RepeatLimitSettingRepository();
        private string g_ApplicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Air);

        #region 增删改
        /// <summary>
        /// 增加重复限报警设置
        /// </summary>
        /// <param name="repeatLimitSetting">实体</param>
        public void Add(RepeatLimitSettingEntity repeatLimitSetting)
        {
            g_Repository.Add(repeatLimitSetting);
        }

        /// <summary>
        /// 更新重复限报警设置
        /// </summary>
        /// <param name="repeatLimitSetting">实体</param>
        public void Update(RepeatLimitSettingEntity repeatLimitSetting)
        {
            g_Repository.Update(repeatLimitSetting);
        }

        /// <summary>
        /// 删除重复限报警设置
        /// </summary>
        /// <param name="repeatLimitSetting">实体</param>
        public void Delete(RepeatLimitSettingEntity repeatLimitSetting)
        {
            g_Repository.Delete(repeatLimitSetting);
        }
        #endregion

        /// <summary>
        /// 根据应用程序值返回重复限报警设置列表
        /// </summary>
        /// <param name="applicationValue">应用程序值</param>
        /// <returns></returns>
        public IQueryable<RepeatLimitSettingEntity> RetrieveListByApplication(ApplicationValue applicationValue)
        {
            string appValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationValue);

            return g_Repository.Retrieve(p => p.ApplicationUid == appValue);
        }


        //<summary>
        //重复限报警设置查询（仪器通道Uid、通知级别Uid、站点Uid）
        //</summary>
        //<param name="instrumentChannelsUid">仪器通道Uid</param>
        //<param name="monitoringPointUid">站点Uid</param>
        //<param name="notifyGradeUid">通知级别Uid</param>
        //<returns></returns>
        public IQueryable<RepeatLimitSettingEntity> RetrieveList(string instrumentChannelsUid, string monitoringPointUid, string notifyGradeUid)
        {
            IQueryable<RepeatLimitSettingEntity> repeatLimitSettingEntity = g_Repository.RetrieveAll();
            if (!string.IsNullOrEmpty(monitoringPointUid)) repeatLimitSettingEntity = repeatLimitSettingEntity.Where(p => p.MonitoringPointUid.Contains(monitoringPointUid));
            if (!string.IsNullOrEmpty(notifyGradeUid)) repeatLimitSettingEntity = repeatLimitSettingEntity.Where(p => p.NotifyGradeUid == notifyGradeUid);
            if (!string.IsNullOrEmpty(instrumentChannelsUid)) repeatLimitSettingEntity = repeatLimitSettingEntity.Where(p => p.InstrumentChannelsUid == instrumentChannelsUid);
            return repeatLimitSettingEntity;
        }

        /// <summary>
        /// 根据应用程序值、报警用途、数据类型返回离线报警设置列表
        /// </summary>
        /// <param name="applicationValue">应用程序值</param>
        /// <returns></returns>
        public IQueryable<RepeatLimitSettingEntity> RetrieveListByDataType(string applicationUid, string useForUid, string dataTypeUid)
        {
            return g_Repository.Retrieve(p => p.ApplicationUid == applicationUid && p.UseForUid == useForUid && p.DataTypeUid == dataTypeUid && p.EnableOrNot == true);
        }
    }
}
