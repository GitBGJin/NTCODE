using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.BaseInfoRepository.BusinessRule;
using SmartEP.Service.Core.Common;
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
    public class NegativeLimitSetting
    {
        private NegativeLimitSettingRepository g_Repository = new NegativeLimitSettingRepository();
        private string g_ApplicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Air);

        #region 增删改
        /// <summary>
        /// 增加负值限报警设置
        /// </summary>
        /// <param name="negativeLimitSetting">实体</param>
        public void Add(NegativeLimitSettingEntity negativeLimitSetting)
        {
            g_Repository.Add(negativeLimitSetting);
        }

        /// <summary>
        /// 更新负值限报警设置
        /// </summary>
        /// <param name="negativeLimitSetting">实体</param>
        public void Update(NegativeLimitSettingEntity negativeLimitSetting)
        {
            g_Repository.Update(negativeLimitSetting);
        }

        /// <summary>
        /// 删除负值限报警设置
        /// </summary>
        /// <param name="negativeLimitSetting">实体</param>
        public void Delete(NegativeLimitSettingEntity negativeLimitSetting)
        {
            g_Repository.Delete(negativeLimitSetting);
        }
        #endregion

        /// <summary>
        /// 根据应用程序值返回负值限报警设置列表
        /// </summary>
        /// <param name="applicationValue">应用程序值</param>
        /// <returns></returns>
        public IQueryable<NegativeLimitSettingEntity> RetrieveListByApplication(ApplicationValue applicationValue)
        {
            string appValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationValue);

            return g_Repository.Retrieve(p => p.ApplicationUid == appValue);
        }


        //<summary>
        //负值限报警设置查询（通知级别Uid、采集数据类型Uid、站点Uid、仪器通道Uid）
        //</summary>
        //<param name="instrumentChannelsUid">仪器通道Uid</param>
        //<param name="dataTypeUid">采集数据类型Uid</param>
        //<param name="monitoringPointUid">站点Uid</param>
        //<param name="notifyGradeUid">通知级别Uid</param>
        //<returns></returns>
        public IQueryable<NegativeLimitSettingEntity> RetrieveList(string instrumentChannelsUid, string dataTypeUid, string monitoringPointUid, string notifyGradeUid)
        {
            IQueryable<NegativeLimitSettingEntity> negativeLimitSettingEntity = g_Repository.RetrieveAll();
            if (!string.IsNullOrEmpty(monitoringPointUid)) negativeLimitSettingEntity = negativeLimitSettingEntity.Where(p => p.MonitoringPointUid.Contains(monitoringPointUid));
            if (!string.IsNullOrEmpty(dataTypeUid)) negativeLimitSettingEntity = negativeLimitSettingEntity.Where(p => p.DataTypeUid == dataTypeUid);
            if (!string.IsNullOrEmpty(notifyGradeUid)) negativeLimitSettingEntity = negativeLimitSettingEntity.Where(p => p.NotifyGradeUid == notifyGradeUid);
            if (!string.IsNullOrEmpty(instrumentChannelsUid)) negativeLimitSettingEntity = negativeLimitSettingEntity.Where(p => p.InstrumentChannelsUid == instrumentChannelsUid);
            return negativeLimitSettingEntity;
        }
    }
}
