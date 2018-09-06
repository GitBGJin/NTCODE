using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.BaseInfoRepository.Channel;
using SmartEP.Core.Enums;
using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using System.Data;
using SmartEP.Service.OperatingMaintenance.ServiceReference;
using SmartEP.Core.Generic;


namespace SmartEP.Service.OperatingMaintenance.Water
{
    public class MaintainInstrumentService
    {

        public MaintainInstrumentRepository maintainInstrumentRepository = new MaintainInstrumentRepository();
        PartChangeRepository r_partChange = new PartChangeRepository();
        TempGetDataWebServiceSoapClient sr = Singleton<TempGetDataWebServiceSoapClient>.GetInstance();
        #region 增删改
        /// <summary>
        /// 增加站点
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Add(MaintainInstrumentEntity maintainInstrumentEntity)
        {
            maintainInstrumentRepository.Add(maintainInstrumentEntity);
        }


        /// <summary>
        /// 更新站点
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Update(MaintainInstrumentEntity maintainInstrumentEntity)
        {
            maintainInstrumentRepository.Update(maintainInstrumentEntity);
        }


        /// <summary>
        /// 删除站点
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Delete(MaintainInstrumentEntity maintainInstrumentEntity)
        {
            maintainInstrumentRepository.Delete(maintainInstrumentEntity);
        }

        /// <summary>
        /// 批量删除点位信息
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void BatchDelete(List<MaintainInstrumentEntity> maintainInstrumentEntity)
        {
            maintainInstrumentRepository.BatchDelete(maintainInstrumentEntity);
        }

        /// <summary>
        /// 删除空气站点扩展信息
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Delete(int id)
        {
            r_partChange.Delete(id);
        }

        /// <summary>
        /// 批量删除空气点位信息
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        //public void BatchDelete(int[] ids)
        //{
        //    r_partChange.BatchDelete(ids);
        //}
        #endregion

        public MaintainInstrumentEntity RetrieveEntityByUid(string maintainInstrumentUid)
        {
            return maintainInstrumentRepository.Retrieve(p => p.MaintainInstrumentUid == maintainInstrumentUid).FirstOrDefault();
        }


        public IQueryable<MaintainInstrumentEntity> RetrieveListByInstrumentUid(DateTime RepairDate, string PointName, int infoType)
        {

            IQueryable<MaintainInstrumentEntity> instrumentList = from item in maintainInstrumentRepository.RetrieveAll()
                                                                  where item.RepairDate == RepairDate && item.PointName == PointName && item.InfoType == infoType
                                                                  select item;
            return instrumentList;
        }
        public IQueryable<MaintainInstrumentEntity> RetrieveListByDate(DateTime RepairDate, int InfoType)
        {

            IQueryable<MaintainInstrumentEntity> instrumentList = from item in maintainInstrumentRepository.RetrieveAll()
                                                                  where item.RepairDate == RepairDate && item.InfoType == InfoType
                                                                  select item;
            return instrumentList;
        }



        public IQueryable<MaintainInstrumentEntity> RetrieveListByPointName(string PointName, int InfoType)
        {

            IQueryable<MaintainInstrumentEntity> instrumentList = from item in maintainInstrumentRepository.RetrieveAll()
                                                                  where item.PointName == PointName && item.InfoType == InfoType
                                                                  select item;
            return instrumentList;
        }
        public IQueryable<MaintainInstrumentEntity> RetrieveListByPointName(int InfoType)
        {

            IQueryable<MaintainInstrumentEntity> instrumentList = from item in maintainInstrumentRepository.RetrieveAll()
                                                                  where item.InfoType == InfoType
                                                                  select item;
            return instrumentList;
        }
        public IQueryable<MaintainInstrumentEntity> RetrieveListByUids(string[] monitoringInstrumentUids)
        {

            return maintainInstrumentRepository.Retrieve(p => monitoringInstrumentUids.Contains(p.MaintainInstrumentUid));
        }

        public DataTable GetInstrument(string pointId)
        {

            return sr.GetInsTypeByObjectID(pointId);

        }

    }
}





