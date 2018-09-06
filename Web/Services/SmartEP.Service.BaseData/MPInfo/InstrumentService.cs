using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.BaseInfoRepository.Channel;
using SmartEP.Core.Enums;
using SmartEP.BaseInfoRepository.MPInfo;

namespace SmartEP.Service.BaseData.MPInfo
{
    /// <summary>
    /// 名称：InstrumentService.cs
    /// 创建人：邱奇
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：提供仪器库服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>

    public class InstrumentService
    {
        private InstrumentRepository g_Repository = new InstrumentRepository();
        private InstrumentChannelRepository g_InstrumentChannelRepository = new InstrumentChannelRepository();
        #region 增删改
        /// <summary>
        /// 增加仪器
        /// </summary>
        /// <param name="Instrument">实体</param>
        public void Add(InstrumentEntity instrument)
        {
            g_Repository.Add(instrument);
        }

        /// <summary>
        /// 更新仪器
        /// </summary>
        /// <param name="Instrument">实体</param>
        public void Update(InstrumentEntity instrument)
        {
            g_Repository.Update(instrument);
        }

        /// <summary>
        /// 删除仪器
        /// </summary>
        /// <param name="Instrument">实体</param>
        public void Delete(InstrumentEntity instrument)
        {
            g_Repository.Delete(instrument);
        }
        #endregion

        /// <summary>
        /// 获取所有仪器列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<InstrumentEntity> RetrieveList()
        {
            IQueryable<InstrumentEntity> instrument = g_Repository.RetrieveAll();
            return instrument;
        }

        /// <summary>
        /// 获取所有空气仪器列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<InstrumentEntity> RetrieveAirInstrumentList()
        {
            IQueryable<InstrumentEntity> instrument = g_Repository.Retrieve(p => p.BusinessTypeUid == "837a4d34-3813-42d8-affc-517aed39608d");
            return instrument;
        }

        /// <summary>
        /// 获取所有水质仪器列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<InstrumentEntity> RetrieveWaterInstrumentList()
        {
            IQueryable<InstrumentEntity> instrument = g_Repository.Retrieve(p => p.BusinessTypeUid == "f0c202e8-7490-4f8b-bb3b-987183559dd5");
            return instrument;
        }

        /// <summary>
        /// 根据仪器Uid获取仪器
        /// </summary>
        /// <param name="InstrumentUid"></param>
        /// <returns></returns>
        public IQueryable<InstrumentEntity> RetrieveInstrumentEntityByUid(string instrumentUid)
        {
            IQueryable<InstrumentEntity> instrument = g_Repository.Retrieve(p => p.RowGuid == instrumentUid);
            return instrument;
        }

        /// <summary>
        /// 根据仪器Uid获取监测因子列表
        /// </summary>
        /// <param name="InstrumentUid"></param>
        /// <returns></returns>
        public IQueryable<InstrumentChannelEntity> RetrieveInstrumentChannelListByInstrumentUid(string instrumentUid)
        {
            IQueryable<InstrumentChannelEntity> instrumentChannel = g_InstrumentChannelRepository.Retrieve(p => p.InstrumentUid == instrumentUid);
            return instrumentChannel;
        }

        /// <summary>
        /// 根据应用类型，业务类型和仪器类型获取仪器列表
        /// </summary>
        /// <param name="ApplyTypeGuid">应用类型</param>
        /// <param name="BusinessTypeGuid">业务类型</param>
        /// <param name="InstrumentTypeGuid">仪器类型</param>
        /// <returns></returns>
        public IQueryable<InstrumentEntity> RetrieveList(string applyTypeGuid, string businessTypeGuid, string instrumentTypeGuid)
        {
            IQueryable<InstrumentEntity> instrumentEntity = g_Repository.RetrieveAll();
            if (applyTypeGuid != "") instrumentEntity = instrumentEntity.Where(p => p.ApplyTypeUid.Contains(applyTypeGuid));
            if (businessTypeGuid != "") instrumentEntity = instrumentEntity.Where(p => p.BusinessTypeUid == businessTypeGuid);
            if (instrumentTypeGuid != "") instrumentEntity = instrumentEntity.Where(p => p.InstrumentTypeUid == instrumentTypeGuid);
            return instrumentEntity;
        }
    }
}
