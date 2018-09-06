using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using SmartEP.Service.BaseData.MPInfo;
using System.Data;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.OperatingMaintenance.ServiceReference;


namespace SmartEP.Service.OperatingMaintenance.Water
{
    public class ScrappedInstrumentService
    {
        MonitoringInstrumentRepository repository = new MonitoringInstrumentRepository();
       AcquisitionInstrumentRepository acqRepository = new AcquisitionInstrumentRepository();
       InstrumentRepository instrumentRepository = new InstrumentRepository();
        ScrappedRepository scrRepository = new ScrappedRepository();
        TempGetDataWebServiceSoapClient sr = Singleton<TempGetDataWebServiceSoapClient>.GetInstance();
        #region 增删改
        /// <summary>
        /// 增加报废仪器
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void Add(ScrappedEntity Scrappedinstrument)
        {
            scrRepository.Add(Scrappedinstrument);
        }

        /// <summary>
        /// 更新报废仪器
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void Update(ScrappedEntity Scrappedinstrument)
        {
            scrRepository.Update(Scrappedinstrument);
        }

        /// <summary>
        /// 删除报废仪器
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void Delete(ScrappedEntity Scrappedinstrument)
        {
            scrRepository.Delete(Scrappedinstrument);
        }

        /// <summary>
        /// 批量删除报废仪器
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void BatchDelete(List<ScrappedEntity> Scrappedinstruments)
        {
            scrRepository.BatchDelete(Scrappedinstruments);
        }
        #endregion


        /// <summary>
        /// 站点是否已存在报废仪器
    
        public bool IsExistByName(string acqusitionName)
        {
            return scrRepository.Retrieve(x => x.InstrumentName.Equals(acqusitionName)).Count() == 0 ? false : true;
        }

     
        
        /// <summary>
        /// 根据数采仪Uid数组获取报废仪器
        /// </summary>
        /// <param name="AcquisitionUid"></param>
        /// <returns></returns>
        public IQueryable<ScrappedEntity> RetrieveListByUids(string[] monitoringInstrumentUids)
        {

            return scrRepository.Retrieve(p => monitoringInstrumentUids.Contains(p.ScrappedInstrumentUid));
        }

        /// <summary>
        /// 根据站点Uid获取报废列表
        /// </summary>
        /// <param name="AcquisitionUid"></param>
        /// <returns></returns>
        public IQueryable<ScrappedEntity> RetrieveListByScrappedInstrumentUid(DateTime ScrappedTime)
        {
            IQueryable<ScrappedEntity> instrumentList = from item in scrRepository.RetrieveAll()
                                                        select item;
    
              instrumentList = from item in scrRepository.RetrieveAll()  
                                                        where item.ScrappedTime== ScrappedTime
                                                          select item;
            return instrumentList;
        }

        /// <summary>
        /// 根据uid获取报废列表
        /// </summary>
        /// <param name="AcquisitionUid"></param>
        /// <returns></returns>
        public IQueryable<ScrappedEntity> RetrieveListByScrappedInstrumentUid()
        {

            IQueryable<ScrappedEntity> instrumentList = from item in scrRepository.RetrieveAll()
                                                        select item;
            return instrumentList;
        }
        ////判断报废仪器是否为数采仪
        //public AcquisitionInstrumentEntity RetrieveEntityByAcquisitionName(string acquisitionName)
        //{
        //    return acqRepository.RetrieveFirstOrDefault(p => p.AcquisitionName == acquisitionName);
        //}
        ////判断报废仪器是否为仪器设备
        //public InstrumentEntity RetrieveEntityByInstrumentName(string instrumentName)
        //{
        //    return instrumentRepository.RetrieveFirstOrDefault(p => p.InstrumentName == instrumentName);
        //}
        //根据报废id获取报废设备
        public ScrappedEntity RetrieveEntityByUid(string scrappedInstrumentUid)
        {
            return scrRepository.Retrieve(p => p.ScrappedInstrumentUid == scrappedInstrumentUid).FirstOrDefault();
        }

        public DataTable GetInstrument(string pointId)
        {
           
            return sr.GetInsTypeByObjectID(pointId);

        }

    }
}
