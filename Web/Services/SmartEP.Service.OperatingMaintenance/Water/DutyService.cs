using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    public class DutyService
    {
        DutyRepository r_duty = new DutyRepository();
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="reportLog">实体类对象</param>
        /// <returns></returns>
        public void DutyAdd(DutyEntity duty)
        {
            r_duty.Add(duty);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="reportLog">实体类对象</param>
        /// <returns></returns>
        public void DutyUpdate(DutyEntity duty)
        {
            r_duty.Update(duty);
        }

        public IQueryable<DutyEntity> Retrieve(Guid id)
        {
            return r_duty.Retrieve(p => p.DutyId == id);
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="reportLog">实体类对象</param>
        /// <returns></returns>
        public void Delete(DutyEntity duty)
        {
            r_duty.Add(duty);
        }
        /// <summary>
        /// 批量删除对象
        ///  </summary>
        public void BatchDelete(List<DutyEntity> entities)
        {
            r_duty.BatchDelete(entities);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="customID"></param>
        /// <returns></returns>
        public IQueryable<DutyEntity> GetPages(string[] PointId, DateTime dtBegin, DateTime dtEnd)
        {
            return r_duty.GetPages(PointId, dtBegin, dtEnd);
        }
        /// <summary>
        /// 根据数采仪Uid数组获取报废仪器
        /// </summary>
        /// <param name="AcquisitionUid"></param>
        /// <returns></returns>
        public IQueryable<DutyEntity> GetLists(Guid[] dutyId)
        {

            return r_duty.GetLists(dutyId);
        }
        /// <summary>
        /// 根据主键DutyID获取数据
        /// </summary>
        /// <param name="customID"></param>
        /// <returns></returns>
        public IQueryable<DutyEntity> GetList(Guid dutyId)
        {
            return r_duty.GetList(dutyId);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Update(DutyEntity model)
        {
            return r_duty.Update(model);
        }
    }
}
