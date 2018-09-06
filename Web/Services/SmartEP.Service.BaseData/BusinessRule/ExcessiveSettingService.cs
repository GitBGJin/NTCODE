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
using SmartEP.Service.BaseData.Channel;

namespace SmartEP.Service.BaseData.BusinessRule
{
    public class ExcessiveSettingService
    {
        private ExcessiveSettingRepository g_Repository = new ExcessiveSettingRepository();
        private string g_ApplicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Air);
        private MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        InstrumentChannelService channelService = new InstrumentChannelService();
        //private AirPollutantService airPollutanService = new AirPollutantService();
        private MonitoringPointWaterService pointWaterService = new MonitoringPointWaterService();
        //private WaterPollutantService waterPollutanService = new WaterPollutantService();
        #region 增删改
        /// <summary>
        /// 增加超标报警设置
        /// </summary>
        /// <param name="excessiveSetting">实体</param>
        public void Add(ExcessiveSettingEntity excessiveSetting)
        {
            g_Repository.Add(excessiveSetting);
        }

        /// <summary>
        /// 更新超标报警设置
        /// </summary>
        /// <param name="excessiveSetting">实体</param>
        public void Update(ExcessiveSettingEntity excessiveSetting)
        {
            g_Repository.Update(excessiveSetting);
        }

        /// <summary>
        /// 删除超标报警设置
        /// </summary>
        /// <param name="excessiveSetting">实体</param>
        public void Delete(ExcessiveSettingEntity excessiveSetting)
        {
            g_Repository.Delete(excessiveSetting);
        }
        #endregion

        /// <summary>
        /// 根据应用程序值返回超标报警设置列表
        /// </summary>
        /// <param name="applicationValue">应用程序值</param>
        /// <returns></returns>
        public IQueryable<ExcessiveSettingEntity> RetrieveListByApplication(ApplicationValue applicationValue)
        {
            string appValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationValue);

            return g_Repository.Retrieve(p => p.ApplicationUid == appValue);
        }


        //<summary>
        //超标报警设置查询（仪器通道Uid、采集数据类型Uid、站点Uid）
        //</summary>
        //<param name="instrumentChannelsUid">仪器通道Uid</param>
        //<param name="dataTypeUid">采集数据类型Uid</param>
        //<param name="monitoringPointUid">站点Uid</param>
        //<param name="notifyGradeUid">通知级别Uid</param>
        //<returns></returns>
        public IQueryable<ExcessiveSettingEntity> RetrieveList(string instrumentChannelsUid, string dataTypeUid, string monitoringPointUid, string notifyGradeUid)
        {
            IQueryable<ExcessiveSettingEntity> excessiveSettingEntity = g_Repository.RetrieveAll();
            if (!string.IsNullOrEmpty(monitoringPointUid)) excessiveSettingEntity = excessiveSettingEntity.Where(p => p.MonitoringPointUid.Contains(monitoringPointUid));
            if (!string.IsNullOrEmpty(dataTypeUid)) excessiveSettingEntity = excessiveSettingEntity.Where(p => p.DataTypeUid == dataTypeUid);
            if (!string.IsNullOrEmpty(notifyGradeUid)) excessiveSettingEntity = excessiveSettingEntity.Where(p => p.NotifyGradeUid == notifyGradeUid);
            if (!string.IsNullOrEmpty(instrumentChannelsUid)) excessiveSettingEntity = excessiveSettingEntity.Where(p => p.InstrumentChannelsUid == instrumentChannelsUid);
            return excessiveSettingEntity;
        }

        ///<summary>
        ///超标报警设置查询（采集数据类型Uid、通知级别Uid,应用程序值）
        ///</summary>
        ///<param name="dataTypeUid">采集数据类型Uid</param>
        ///<param name="notifyGradeUid">通知级别Uid</param>
        ///<param name="applicationValue">应用程序值</param>
        ///<returns></returns>
        public IQueryable<ExcessiveSettingEntity> RetrieveListByDataType(string applicationUid, string useForUid, string dataTypeUid)
        {
            IQueryable<ExcessiveSettingEntity> excessiveSettingEntity = g_Repository.RetrieveAll();
            excessiveSettingEntity = excessiveSettingEntity.Where(p => p.ApplicationUid == applicationUid);
            if (!string.IsNullOrEmpty(dataTypeUid)) excessiveSettingEntity = excessiveSettingEntity.Where(p => p.DataTypeUid == dataTypeUid);
            if (!string.IsNullOrEmpty(useForUid)) excessiveSettingEntity = excessiveSettingEntity.Where(p => p.UseForUid == useForUid);
            return excessiveSettingEntity;
        }

        /// <summary>
        /// 获取因子超标配置
        /// </summary>
        /// <param name="instrumentChannelsUid"></param>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<ExcessiveSettingInfo> RetrieveListByFactor(ApplicationValue mode, string[] factors, string[] pointID)
        {
            IQueryable<MonitoringPointEntity> pointList = null;
            IQueryable<InstrumentChannelEntity> factorList = null;
            if (mode == ApplicationValue.Air)
            {
                pointList = pointAirService.RetrieveListByPointIds(pointID);
                factorList = channelService.RetrieveList().Where(x => factors.Contains(x.PollutantCode));
            }
            else if (mode == ApplicationValue.Water)
            {
                pointList = pointWaterService.RetrieveListByPointIds(pointID);
                factorList = channelService.RetrieveList().Where(x => factors.Contains(x.PollutantCode));
            }

            //pollutantCodeRepository.Context.GetAll<V_Factor_SiteMap_UserConfigEntity>().Where(p => p.ApplicationUid == applicationUid && p.UserGuid == userGuid);
            IQueryable<ExcessiveSettingInfo> excessiveSettingEntity = from excess in g_Repository.RetrieveAll()
                                                                      join point in pointList
                                                                          on excess.MonitoringPointUid equals point.MonitoringPointUid
                                                                      join fac in factorList on excess.InstrumentChannelsUid equals fac.InstrumentChannelsUid
                                                                      where excess.ApplicationUid == SmartEP.Core.Enums.EnumMapping.GetApplicationValue(mode) && excess.NotifyGradeUid == "9df318d1-b104-4425-a846-69e66222d53c"
                                                                      select new ExcessiveSettingInfo { PointID = point.PointId, PollutantGuid = fac.PollutantUid, PollutantCode = fac.PollutantCode, excessiveLow = excess.ExcessiveLow.Value, excessiveUpper = excess.ExcessiveUpper.Value };
            return excessiveSettingEntity;
        }


        /// <summary>
        /// 获取因子超标上下限
        /// </summary>
        /// <param name="instrumentChannelsUid"></param>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public IQueryable<ExcessiveSettingInfo> RetrieveListByPointAndFactor(ApplicationValue mode, string[] factors, string[] pointID, string DataTypeUid)
        {
            IQueryable<MonitoringPointEntity> pointList = null;
            IQueryable<InstrumentChannelEntity> factorList = null;
            if (mode == ApplicationValue.Air)
            {
                pointList = pointAirService.RetrieveListByPointIds(pointID);
                factorList = channelService.RetrieveList().Where(x => factors.Contains(x.PollutantCode));
            }
            else if (mode == ApplicationValue.Water)
            {
                pointList = pointWaterService.RetrieveListByPointIds(pointID);
                factorList = channelService.RetrieveList().Where(x => factors.Contains(x.PollutantCode));
            }
            IQueryable<ExcessiveSettingEntity> ecx = g_Repository.RetrieveAll();
            //pollutantCodeRepository.Context.GetAll<V_Factor_SiteMap_UserConfigEntity>().Where(p => p.ApplicationUid == applicationUid && p.UserGuid == userGuid);
            IQueryable<ExcessiveSettingInfo> excessiveSettingEntity = from excess in g_Repository.RetrieveAll()
                                                                      join point in pointList
                                                                          on excess.MonitoringPointUid equals point.MonitoringPointUid
                                                                      join fac in factorList on excess.InstrumentChannelsUid equals fac.InstrumentChannelsUid
                                                                      where excess.ApplicationUid == SmartEP.Core.Enums.EnumMapping.GetApplicationValue(mode) && excess.NotifyGradeUid == "e1111f5e-6ef7-4f95-8778-bd7062ce2aae" && excess.DataTypeUid == DataTypeUid
                                                                      select new ExcessiveSettingInfo { PointID = point.PointId, PollutantGuid = fac.PollutantUid, PollutantCode = fac.PollutantCode, excessiveLow = (excess.ExcessiveLow != null) ? excess.ExcessiveLow.Value : -9999, excessiveUpper = (excess.ExcessiveUpper != null) ? excess.ExcessiveUpper.Value : 99999 };
            return excessiveSettingEntity;
        }
    }
}
