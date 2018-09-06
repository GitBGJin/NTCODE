using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.BaseInfoRepository.Channel;
using SmartEP.Core.Enums;
using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Service.Frame;
using SmartEP.DomainModel;
using SmartEP.Service.Core.Enums;

namespace SmartEP.Service.BaseData.MPInfo
{
    /// <summary>
    /// 名称：InstrumentChannelService.cs
    /// 创建人：邱奇
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：提供仪器通道信息服务
    /// 版权所有(C)：江苏远大信息股份有限公司

    /// </summary>

    public class InstrumentChannelService
    {
        private InstrumentChannelRepository instrumentChannelRepository = new InstrumentChannelRepository();
        private PollutantCodeRepository channelRepository = new PollutantCodeRepository();
        #region 增删改
        /// <summary>
        /// 增加仪器通道
        /// </summary>
        /// <param name="InstrumentChannel">实体</param>
        public void Add(InstrumentChannelEntity instrumentChannel)
        {
            instrumentChannelRepository.Add(instrumentChannel);
        }

        /// <summary>
        /// 更新仪器通道
        /// </summary>
        /// <param name="InstrumentChannel">实体</param>
        public void Update(InstrumentChannelEntity instrumentChannel)
        {
            instrumentChannelRepository.Update(instrumentChannel);
        }

        /// <summary>
        /// 删除仪器通道
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Delete(InstrumentChannelEntity instrumentChannel)
        {
            instrumentChannelRepository.Delete(instrumentChannel);
        }
        #endregion

        /// <summary>
        /// 获取所有仪器通道列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<InstrumentChannelEntity> RetrieveList()
        {
            IQueryable<InstrumentChannelEntity> instrumentChannel = instrumentChannelRepository.RetrieveAll();
            return instrumentChannel;
        }

        /// <summary>
        /// 根据通道Uid获取仪器通道
        /// </summary>
        /// <param name="channelUid"></param>
        /// <returns></returns>
        public InstrumentChannelEntity RetrieveEntityByUid(string instrumentChannelsUid)
        {
            return instrumentChannelRepository.RetrieveFirstOrDefault(p => p.InstrumentChannelsUid == instrumentChannelsUid);
        }

        /// <summary>
        /// 根据仪器Uid获取仪器监测的所有通道
        /// </summary>
        /// <param name="InstrumentUid">仪器Uid</param>
        /// <returns></returns>
        public IQueryable<InstrumentChannelEntity> RetrieveListByInstrumentUid(string instrumentUid)
        {
            IQueryable<InstrumentChannelEntity> instrumentChannel = instrumentChannelRepository.Retrieve(p => p.InstrumentUid == instrumentUid);
            return instrumentChannel;
        }

        /// <summary>
        /// 根据站点Uid获取所有监测仪器状态通道
        /// </summary>
        /// <param name="InstrumentUid">站点Uid</param>
        /// <returns></returns>
        public IQueryable<PollutantCodeEntity> RetrieveStatusChannelListByPointUid(string pointUid)
        {
            MonitoringInstrumentService instruService = new MonitoringInstrumentService();
            IQueryable<InstrumentEntity> instrumentList = instruService.RetrieveListByPointUid(pointUid);

            IQueryable<PollutantCodeEntity> channelList = from item in channelRepository.RetrieveAll()
                                                          join p in instrumentChannelRepository.RetrieveAll() on item.PollutantUid equals p.PollutantUid
                                                          join t in instrumentList on p.InstrumentUid equals t.RowGuid
                                                          select item;
            return channelList.Distinct();
        }

        /// <summary>
        /// 根据站点Uid获取所有监测通道
        /// </summary>
        /// <param name="InstrumentUid">站点Uid</param>
        /// <returns></returns>
        public IQueryable<PollutantCodeEntity> RetrieveChannelListByPointUid(string pointUid)
        {
            MonitoringInstrumentService instruService = new MonitoringInstrumentService();
            IQueryable<InstrumentEntity> instrumentList = instruService.RetrieveListByPointUid(pointUid);
            if (instrumentList != null)
            {
                IQueryable<PollutantCodeEntity> channelList = from item in channelRepository.Retrieve(x => x.TypeUid.Equals("ae39f55e-5c43-4b4a-b224-0b925b5f3c9f"))
                                                              join p in instrumentChannelRepository.RetrieveAll() on item.PollutantUid equals p.PollutantUid
                                                              join t in instrumentList on p.InstrumentUid equals t.RowGuid
                                                              select item;
                if (channelList != null)
                {
                    return channelList.Distinct();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据站点Uid获取所有监测通道
        /// </summary>
        /// <param name="InstrumentUid">站点Uid</param>
        /// <returns></returns>
        public IQueryable<PollutantCodeEntity> RetrieveChannelListByPointUids(string[] pointUid)
        {
            MonitoringInstrumentService instruService = new MonitoringInstrumentService();
            IQueryable<InstrumentEntity> instrumentList = instruService.RetrieveListByPointUidss(pointUid);

            IQueryable<PollutantCodeEntity> channelList = from item in channelRepository.Retrieve(x => x.TypeUid.Equals("ae39f55e-5c43-4b4a-b224-0b925b5f3c9f"))
                                                          join p in instrumentChannelRepository.RetrieveAll() on item.PollutantUid equals p.PollutantUid
                                                          join t in instrumentList on p.InstrumentUid equals t.RowGuid
                                                          orderby item.OrderByNum descending
                                                          select item;
            return channelList.Distinct();
        }

        /// <summary>
        /// 根据站点Uid获取所有监测通道
        /// </summary>
        /// <param name="InstrumentUid">站点Uid</param>
        /// <returns></returns>
        public IQueryable<PollutantCodeEntity> RetrieveChannelListByFactors(string[] pointUid, string[] factors)
        {
            MonitoringInstrumentService instruService = new MonitoringInstrumentService();
            IQueryable<InstrumentEntity> instrumentList = instruService.RetrieveListByPointUidss(pointUid);

            IQueryable<PollutantCodeEntity> channelList = from item in channelRepository.Retrieve(x => x.TypeUid.Equals("ae39f55e-5c43-4b4a-b224-0b925b5f3c9f"))
                                                          join p in instrumentChannelRepository.RetrieveAll() on item.PollutantUid equals p.PollutantUid
                                                          join t in instrumentList on p.InstrumentUid equals t.RowGuid
                                                          where factors.Contains(item.PollutantCode)
                                                          orderby item.OrderByNum descending
                                                          select item;
            return channelList.Distinct();
        }
        /// <summary>
        /// 根据仪器Uid获取仪器监测的所有状态
        /// </summary>
        /// <param name="InstrumentUid">仪器Uid</param>
        /// <returns></returns>
        public IQueryable<PollutantCodeEntity> RetrieveInstrumentStateListByInstrumentUid(string instrumentUid)
        {
            DictionaryService g_dicService = new DictionaryService();
            string guid = g_dicService.GetValueByText(DictionaryType.AMS, "通道类型", "状态");
            IQueryable<PollutantCodeEntity> channelList = from item in channelRepository.Retrieve(p => p.TypeUid == guid)
                                                          join t in instrumentChannelRepository.RetrieveAll() on item.PollutantUid equals t.PollutantUid
                                                          where t.InstrumentUid == instrumentUid
                                                          select item;
            return channelList;
        }
    }
}
