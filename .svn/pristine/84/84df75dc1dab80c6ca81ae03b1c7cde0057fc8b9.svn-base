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
    public class OutlierSettingService
    {
        private OutlierSettingRepository g_Repository = new OutlierSettingRepository();
        private string g_ApplicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Air);

        #region 增删改
        /// <summary>
        /// 增加离群报警设置
        /// </summary>
        /// <param name="outlierSetting">实体</param>
        public void Add(OutlierSettingEntity outlierSetting)
        {
            g_Repository.Add(outlierSetting);
        }

        /// <summary>
        /// 更新离群报警设置
        /// </summary>
        /// <param name="outlierSetting">实体</param>
        public void Update(OutlierSettingEntity outlierSetting)
        {
            g_Repository.Update(outlierSetting);
        }

        /// <summary>
        /// 删除离群报警设置
        /// </summary>
        /// <param name="outlierSetting">实体</param>
        public void Delete(OutlierSettingEntity outlierSetting)
        {
            g_Repository.Delete(outlierSetting);
        }
        #endregion

        /// <summary>
        /// 根据应用程序值返回离群报警设置列表
        /// </summary>
        /// <param name="applicationValue">应用程序值</param>
        /// <returns></returns>
        public IQueryable<OutlierSettingEntity> RetrieveListByApplication(ApplicationValue applicationValue)
        {
            string appValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationValue);

            return g_Repository.Retrieve(p => p.ApplicationUid == appValue);
        }


        //<summary>
        //离群报警设置查询（仪器通道Uid、采集数据类型Uid、站点Uid）
        //</summary>
        //<param name="instrumentChannelsUid">仪器通道Uid</param>
        //<param name="dataTypeUid">采集数据类型Uid</param>
        //<param name="monitoringPointUid">站点Uid</param>
        //<param name="notifyGradeUid">通知级别Uid</param>
        //<returns></returns>
        public IQueryable<OutlierSettingEntity> RetrieveList(string dataTypeUid, string useForUid, ApplicationValue applicationValue)
        {
            IQueryable<OutlierSettingEntity> outlierSettingEntity = g_Repository.RetrieveAll();
            string appValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationValue);
            outlierSettingEntity = outlierSettingEntity.Where(p => p.ApplicationUid == appValue);
            if (!string.IsNullOrEmpty(dataTypeUid)) outlierSettingEntity = outlierSettingEntity.Where(p => p.DataTypeUid == dataTypeUid);
            if (!string.IsNullOrEmpty(useForUid)) outlierSettingEntity = outlierSettingEntity.Where(p => p.UseForUid == useForUid);
            return outlierSettingEntity;
        }
    }
}
