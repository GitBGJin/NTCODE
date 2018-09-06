using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.QualityControlOperation.Water;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
    public class DutyRepository : BaseGenericRepository<MonitoringBusinessModel, DutyEntity>
    {
        public override bool IsExist(string strKey)
        {
            return true;
        }
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        AbnormalDAL m_AbnormalDAL = Singleton<AbnormalDAL>.GetInstance();
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="PointId">测点</param>
        /// <param name="dtBegin">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public IQueryable<DutyEntity> GetPages(string[] PointId, DateTime dtBegin, DateTime dtEnd)
        {
            return Retrieve(p => PointId.Contains(p.PointId.ToString()) && p.DateTime >= dtBegin && p.DateTime <= dtEnd);
        }
        /// <summary>
        /// 根据主键DutyID获取数据
        /// </summary>
        /// <param name="PointId">测点</param>
        /// <param name="dtBegin">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public IQueryable<DutyEntity> GetList(Guid dutyId)
        {
            return Retrieve(p => p.DutyId == dutyId);
        }
        /// <summary>
        /// 根据主键DutyID获取数据
        /// </summary>
        /// <param name="PointId">测点</param>
        /// <param name="dtBegin">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public IQueryable<DutyEntity> GetLists(Guid[] dutyId)
        {
            return Retrieve(p => dutyId.Contains(p.DutyId));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Update(DutyEntity model)
        {
            return m_AbnormalDAL.Update(model);
        }
    }
}