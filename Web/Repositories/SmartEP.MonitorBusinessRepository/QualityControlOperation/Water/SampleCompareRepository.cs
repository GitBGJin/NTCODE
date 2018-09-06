using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.QualityControlOperation.Water;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：SampleCompareRepository.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-21
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 质控任务记录表仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SampleCompareRepository : BaseGenericRepository<MonitoringBusinessModel, StandardSolutionCheckEntity>
    {
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return RetrieveCount(x => x.Id.Equals(strKey)) == 0 ? false : true;
        }
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        StandardSolutionCheckDAL m_StandardSolutionCheckDAL = Singleton<StandardSolutionCheckDAL>.GetInstance();
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="e_StandardSolutionCheck">实体</param>
        public void AddEntity(StandardSolutionCheckEntity e_StandardSolutionCheck)
        {
            Add(e_StandardSolutionCheck);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实验室记录实体</param>
        public void delete(StandardSolutionCheckEntity entity)
        {
            Delete(entity);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <param name="dtBegin">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public IQueryable<StandardSolutionCheckEntity> RetrieveData(string taskId,int pointId, DateTime dtBegin, DateTime dtEnd)
        {
            return Retrieve(p => p.MissionID == taskId & p.PointId == pointId & p.Tstamp >= dtBegin & p.Tstamp <= dtEnd);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return m_StandardSolutionCheckDAL.GetList(strWhere);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataTable GetLists(string strWhere)
        {
            return m_StandardSolutionCheckDAL.GetLists(strWhere);
        }
    }
}
