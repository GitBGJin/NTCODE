using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Common;
using SmartEP.Service.DataAnalyze.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Common
{
    /// <summary>
    /// 名称：DataSamplingConditionService.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-27
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：监测点数据采集情况
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataSamplingConditionService
    {
        /// <summary>
        /// 监测点数据采集情况仓储层
        /// </summary>
        DataSamplingConditionRepository g_DataSamplingConditionRepository = Singleton<DataSamplingConditionRepository>.GetInstance();

        #region 增删改查
        /// <summary>
        /// 增加对象
        /// </summary>
        /// <param name="entity"></param>
        public void Add(DataSamplingConditionEntity entity)
        {
            g_DataSamplingConditionRepository.Add(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(DataSamplingConditionEntity entity)
        {
            g_DataSamplingConditionRepository.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities"></param>
        public void BatchDelete(List<DataSamplingConditionEntity> entities)
        {
            g_DataSamplingConditionRepository.BatchDelete(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            g_DataSamplingConditionRepository.Update();
        }

        /// <summary>
        /// 根据数采仪Guid取得监测点数据采集情况
        /// </summary>
        /// <param name="acquisitionUid">数采仪</param>
        /// <returns></returns>
        public IBaseEntityProperty RetrieveByAcquisitionUid(string acquisitionUid)
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.AcquisitionUid.Equals(acquisitionUid)).FirstOrDefault();
        }
        #endregion

        #region << 查询统计 >>
        /// <summary>
        /// 取得所有测点数据采集情况（包含水、气）
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveAll()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ShowOrNot.Equals(true));
        }

        /// <summary>
        /// 取得气站所有测点数据采集情况
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveAirAll()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Air)) && x.ShowOrNot.Equals(true));
        }

        /// <summary>
        /// 取得水站所有测点数据采集情况
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveWaterAll()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Air)) && x.ShowOrNot.Equals(true));
        }

        /// <summary>
        /// 根据应用类型、状态查询监测点数据采集情况
        /// </summary>
        /// <param name="applicationType">应用类型</param>
        /// <param name="dataSamplingStatus">状态类型</param>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveByStatus(ApplicationType applicationType, DataSamplingStatus dataSamplingStatus)
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ShowOrNot.Equals(true) && x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(applicationType))
                && x.StatusCode.Equals(EnumMapping.GetDesc(dataSamplingStatus)));
        }

        /// <summary>
        /// 根据状态查询监测点数据采集情况
        /// </summary>
        /// <param name="dataSamplingStatus">状态类型</param>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveByStatus(DataSamplingStatus dataSamplingStatus)
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ShowOrNot.Equals(true) && x.StatusCode.Equals(EnumMapping.GetDesc(dataSamplingStatus)));
        }
        #endregion

        #region << 空气查询统计 >>
        /// <summary>
        /// 取得空气在线点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveAirOnline()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Air))
                && x.ShowOrNot.Equals(true) && (x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Online))
                || x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Alarm)) || x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.AlwaysOnline))));
        }

        /// <summary>
        /// 取得空气始终在线点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveAirAlwaysOnline()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Air))
                && x.ShowOrNot.Equals(true) && x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.AlwaysOnline)));
        }

        /// <summary>
        /// 取得空气报警点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveAirAlarm()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Air))
                && x.ShowOrNot.Equals(true) && x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Alarm)));
        }

        /// <summary>
        /// 取得空气离线点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveAirOffline()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Air))
                && x.ShowOrNot.Equals(true) && (x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Offline))
                || x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Failure)) || x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Stop))));
        }

        /// <summary>
        /// 取得空气故障点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveAirFailure()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Air))
                && x.ShowOrNot.Equals(true) && x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Failure)));
        }

        /// <summary>
        /// 取得空气停运点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveAirStop()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Air))
                && x.ShowOrNot.Equals(true) && x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Stop)));
        }
        #endregion

        #region << 地表水查询统计 >>
        /// <summary>
        /// 取得地表水在线点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveWaterOnline()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Water))
                && x.ShowOrNot.Equals(true) && (x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Online))
                || x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Alarm)) || x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.AlwaysOnline))));
        }

        /// <summary>
        /// 取得地表水始终在线点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveWaterAlwaysOnline()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Water))
                && x.ShowOrNot.Equals(true) && x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.AlwaysOnline)));
        }

        /// <summary>
        /// 取得地表水报警点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveWaterAlarm()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Water))
                && x.ShowOrNot.Equals(true) && x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Alarm)));
        }

        /// <summary>
        /// 取得地表水离线点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveWaterOffline()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Water))
                && x.ShowOrNot.Equals(true) && (x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Offline))
                || x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Failure)) || x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Stop))));
        }

        /// <summary>
        /// 取得地表水故障点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveWaterFailure()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Water))
                && x.ShowOrNot.Equals(true) && x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Failure)));
        }

        /// <summary>
        /// 取得地表水停运点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveWaterStop()
        {
            return g_DataSamplingConditionRepository.Retrieve(x => x.ApplicationUid.Equals(EnumMapping.GetApplicationValue(ApplicationType.Water))
                && x.ShowOrNot.Equals(true) && x.StatusCode.Equals(EnumMapping.GetDesc(DataSamplingStatus.Stop)));
        }
        #endregion

        /// <summary>
        /// 获取测点在线状态实时统计数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <returns></returns>
        public DataTable GetSamplingConditionData(ApplicationType applicationType)
        {
            return g_DataSamplingConditionRepository.GetSamplingConditionData(applicationType);
        }

        /// <summary>
        /// 获取测点离线状态信息
        /// </summary>
        /// <param name="pointIds">点位ID</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <returns></returns>
        public DataTable GetOfflinePointInfo(string applicationUid, string[] pointIds)
        {
            return g_DataSamplingConditionRepository.GetOfflinePointInfo(applicationUid, pointIds);
        }
    }
}
