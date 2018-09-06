using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.MPInfo
{
    /// <summary>
    /// 名称：InstrumentService.cs
    /// 创建人：邱奇
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：季柯
    /// 最新维护日期：2015-9-2
    /// 功能摘要：提供监测仪器服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MonitoringInstrumentService
    {
        private MonitoringInstrumentRepository repository = new MonitoringInstrumentRepository();
        private AcquisitionInstrumentRepository acqRepository = new AcquisitionInstrumentRepository();
        private InstrumentRepository instrumentRepository = new InstrumentRepository();
        #region 增删改
        /// <summary>
        /// 增加监测仪器
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void Add(MonitoringInstrumentEntity monitoringInstrument)
        {
            repository.Add(monitoringInstrument);
        }

        /// <summary>
        /// 更新监测仪器
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void Update(MonitoringInstrumentEntity monitoringInstrument)
        {
            repository.Update(monitoringInstrument);
        }

        /// <summary>
        /// 删除监测仪器
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void Delete(MonitoringInstrumentEntity monitoringInstrument)
        {
            repository.Delete(monitoringInstrument);
        }

        /// <summary>
        /// 批量删除监测仪器
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void BatchDelete(List<MonitoringInstrumentEntity> monitoringInstruments)
        {
            repository.BatchDelete(monitoringInstruments);
        }
        #endregion


        /// <summary>
        /// 站点是否已存在仪器
        /// </summary>
        /// <param name="pointUid">站点Uid</param>
        /// <param name="instrumentUid">仪器Uid</param>
        public bool IsExistByPointUid(string pointUid, string instrumentUid)
        {
            string acqusionUid = acqRepository.RetrieveFirstOrDefault(p => p.MonitoringPointUid == pointUid).AcquisitionUid;
            return repository.Retrieve(p => p.AcquisitionUid == acqusionUid && p.InstrumentUid == instrumentUid).Count() > 0 ? true : false;
        }

        /// <summary>
        /// 数采仪是否已存在仪器
        /// </summary>
        /// <param name="acqusionInstrumentUid">数采仪Uid</param>
        /// <param name="instrumentUid">仪器Uid</param>
        public bool IsExistByAcqUid(string acqusionInstrumentUid, string instrumentUid)
        {
            IQueryable<MonitoringInstrumentEntity> entities = repository.Retrieve(p => p.AcquisitionUid == acqusionInstrumentUid && p.InstrumentUid == instrumentUid);
            return entities.Count() > 0 ? true : false;
        }

        /// <summary>
        /// 根据站点Uid和仪器Uid获取监测仪器
        /// </summary>
        /// <param name="pointUid">站点Uid</param>
        /// <param name="instrumentUid">仪器Uid</param>
        public MonitoringInstrumentEntity RetrieveByPointUid(string pointUid, string instrumentUid)
        {
            string acqusionUid = acqRepository.RetrieveFirstOrDefault(p => p.MonitoringPointUid == pointUid).AcquisitionUid;
            return repository.RetrieveFirstOrDefault(p => p.AcquisitionUid == acqusionUid && p.InstrumentUid == instrumentUid);
        }

        /// <summary>
        /// 根据数采仪Uid获取仪器
        /// </summary>
        /// <param name="AcquisitionUid"></param>
        /// <returns></returns>
        public IQueryable<MonitoringInstrumentEntity> RetrieveListByAcquisitionUid(string AcquisitionUid)
        {
            return repository.Retrieve(p => p.AcquisitionUid == AcquisitionUid);
        }

        /// <summary>
        /// 根据数采仪Uid数组获取仪器
        /// </summary>
        /// <param name="AcquisitionUid"></param>
        /// <returns></returns>
        public IQueryable<MonitoringInstrumentEntity> RetrieveListByUids(string[] monitoringInstrumentUids)
        {
            // return repository.Retrieve(p => monitoringInstrumentUids.Contains(p.MonitoringinstrumentUid));
            return repository.Retrieve(p => monitoringInstrumentUids.Contains(p.InstrumentUid));
        }

        /// <summary>
        /// 根据站点Uid获取监测仪器列表
        /// </summary>
        /// <param name="monitoringPointUid">站点Uid</param>
        /// <returns></returns>
        public IQueryable<InstrumentEntity> RetrieveListByPointUid(string monitoringPointUid)
        {
            AcquisitionInstrumentEntity acEntity = acqRepository.Retrieve(p => p.MonitoringPointUid == monitoringPointUid).FirstOrDefault();
            if (acEntity != null)
            {
                string acquisitionUid = acEntity.AcquisitionUid;
                IQueryable<MonitoringInstrumentEntity> monitorInstrList = repository.Retrieve(p => p.AcquisitionUid == acquisitionUid);
                if (monitorInstrList != null)
                {
                    IQueryable<InstrumentEntity> instrumentList = from item in instrumentRepository.RetrieveAll()
                                                                  join t in monitorInstrList on item.RowGuid equals t.InstrumentUid
                                                                  select item;
                    return instrumentList;
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
        /// 根据站点Uid数组获取监测仪器列表
        /// </summary>
        /// <param name="monitoringPointUids">站点Uid数组</param>
        /// <returns></returns>
        public IQueryable<InstrumentEntity> RetrieveListByPointUids(string[] monitoringPointUids)
        {
            string[] acquisitionUids = acqRepository.Retrieve(p => monitoringPointUids.Contains(p.MonitoringPointUid)).Select(t => t.AcquisitionUid).ToArray();
            IQueryable<MonitoringInstrumentEntity> monitorInstrList = repository.Retrieve(p => acquisitionUids.Contains(p.AcquisitionUid));
            IQueryable<InstrumentEntity> instrumentQueryable = from item in instrumentRepository.RetrieveAll()
                                                               join t in monitorInstrList on item.RowGuid equals t.InstrumentUid
                                                               select item;
            IList<InstrumentEntity> instrumentListNew = new List<InstrumentEntity>();
            foreach (InstrumentEntity instrumentEntity in instrumentQueryable)
            {
                if (instrumentListNew.Count(t => t.InstrumentName == instrumentEntity.InstrumentName) == 0)
                {
                    instrumentListNew.Add(instrumentEntity);
                }
            }
            return instrumentListNew.AsQueryable();
        }
        /// <summary>
        /// 根据站点Uid数组获取监测仪器列表
        /// </summary>
        /// <param name="monitoringPointUids">站点Uid数组</param>
        /// <returns></returns>
        public IQueryable<InstrumentEntity> RetrieveListByPointUidss(string[] monitoringPointUids)
        {
            string[] acquisitionUids = acqRepository.Retrieve(p => monitoringPointUids.Contains(p.MonitoringPointUid)).Select(t => t.AcquisitionUid).ToArray();
            IQueryable<MonitoringInstrumentEntity> monitorInstrList = repository.Retrieve(p => acquisitionUids.Contains(p.AcquisitionUid));
            IQueryable<InstrumentEntity> instrumentList = from item in instrumentRepository.RetrieveAll()
                                                               join t in monitorInstrList on item.RowGuid equals t.InstrumentUid
                                                               select item;
            return instrumentList;
        }

        /// <summary>
        /// 根据仪器名称和站点Uid获取监测仪器列表
        /// </summary>
        /// <param name="AcquisitionUid"></param>
        /// <returns></returns>
        public IQueryable<InstrumentEntity> RetrieveListByPointUid(string monitoringPointUid, string instrumentName)
        {
            string acquisitionUid = acqRepository.Retrieve(p => p.MonitoringPointUid == monitoringPointUid).FirstOrDefault().AcquisitionUid;
            IQueryable<MonitoringInstrumentEntity> monitorInstrList = repository.Retrieve(p => p.AcquisitionUid == acquisitionUid);
            IQueryable<InstrumentEntity> instrumentList = from item in instrumentRepository.RetrieveAll()
                                                          join t in monitorInstrList on item.RowGuid equals t.InstrumentUid
                                                          select item;
            //IQueryable<InstrumentEntity> instrumentList = from item in instrumentRepository.RetrieveAll()
            //                                              join t in monitorInstrList on item.Id.ToString()  equals t.InstrumentUid
            //                                             select item;
            if (!string.IsNullOrEmpty(instrumentName)) instrumentList = instrumentList.Where(p => p.InstrumentName.Contains(instrumentName.Trim()));
            return instrumentList;
        }
        /// <summary>
        /// 根据站点Uid获取站点仪器因子类型配置信息
        /// </summary>
        /// <param name="PUid"></param>
        /// <returns></returns>
        public DataTable GetPoint_Category_Instrument(string PUid)
        {
            return repository.GetPoint_Category_Instrument(PUid);
        }

    }
}
