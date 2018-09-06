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
    /// 名称：StandardSolutionCheckRepository.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 质控任务仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class StandardSolutionCheckRepository : BaseGenericRepository<MonitoringBusinessModel, StandardSolutionCheckEntity>
    {
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return RetrieveCount(x => x.Id.Equals(Convert.ToInt32(strKey))) == 0 ? false : true;
        }

        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        StandardSolutionCheckDAL m_StandardSolutionCheckDAL = Singleton<StandardSolutionCheckDAL>.GetInstance();

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string missionId, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            return m_StandardSolutionCheckDAL.GetDataPager(portIds, missionId, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPagers(string[] portIds,string taskCode,string missionId, string type, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            return m_StandardSolutionCheckDAL.GetDataPagers(portIds, taskCode,missionId, type, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 质控任务取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPagerQControl(string[] portIds, string[] missionIds, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd,string[] evaluate, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            return m_StandardSolutionCheckDAL.GetDataPagerQControl(portIds, missionIds,pollutantCodes, dtmStart, dtmEnd,evaluate, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 质控任务取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPagerQControl(string[] portIds, string missionId, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string[] evaluate, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            return m_StandardSolutionCheckDAL.GetDataPagerQControl(portIds, missionId, pollutantCodes, dtmStart, dtmEnd, evaluate, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 质控任务取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPagerQControlNew(string[] portIds, string missionId, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string[] evaluate, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            return m_StandardSolutionCheckDAL.GetDataPagerQControlNew(portIds, missionId, pollutantCodes, dtmStart, dtmEnd, evaluate, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 质控任务取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPagerQControlNew2(string[] portIds, string missionId, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            return m_StandardSolutionCheckDAL.GetDataPagerQControlNew2(portIds, missionId, pollutantCodes, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
        }

        public string GetDecimalDigit(string PollutantName)
        {
            return m_StandardSolutionCheckDAL.GetDecimalDigit(PollutantName);
        }

        /// <summary>
        /// 获取导出Excel的性能考核(行转列数据)
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="missionIds"></param>
        /// <param name="pollutantCodes"></param>
        /// <param name="dtmStart"></param>
        /// <param name="dtmEnd"></param>
        /// <param name="actionID"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataToExcel(string[] portIds, string[] missionIds, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string orderBy = "TaskCode,ActionID")
        {
            return m_StandardSolutionCheckDAL.GetDataToExcel(portIds, missionIds, pollutantCodes, dtmStart, dtmEnd, orderBy);
        }

        /// <summary>
        /// 取得所有数据供导出
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, DateTime dtmStart, DateTime dtmEnd, string orderBy = "Tstamp")
        {
            return m_StandardSolutionCheckDAL.GetExportData(portIds, dtmStart, dtmEnd, orderBy);
        }

        public DataView GetNum(string[] portIds, string[] missionIds, string[] actionIDs, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string orderBy = "TaskCode,ActionID")
        {
            return m_StandardSolutionCheckDAL.GetNum(portIds, missionIds, actionIDs, pollutantCodes, dtmStart, dtmEnd, orderBy);
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
        public DataTable GetListNew(string strWhere)
        {
            return m_StandardSolutionCheckDAL.GetListNew(strWhere);
        }
        /// <summary>
        /// 根据因子code查看比对限值
        /// </summary>
        /// <param name="PollutantCode">因子code</param>
        /// <returns></returns>
        public string GetCompareLimitValue(string pollutantCode)
        {
            return m_StandardSolutionCheckDAL.GetCompareLimitValue(pollutantCode);
        }
        ///// <summary>
        ///// 增加一条数据
        ///// </summary>
        ///// <param name="model">采样记录实体</param>
        ///// <returns></returns>
        //public bool Add(StandardSolutionCheckEntity model)
        //{
        //    return m_StandardSolutionCheckDAL.Add(model);
        //}

        ///// <summary>
        ///// 批量增加数据
        ///// </summary>
        ///// <param name="models">采样记录实体数组</param>
        ///// <returns></returns>
        //public bool Add(StandardSolutionCheckEntity[] models)
        //{
        //    return m_StandardSolutionCheckDAL.Add(models);
        //}

        ///// <summary>
        ///// 更新一条数据
        ///// </summary>
        ///// <param name="model">采样记录实体</param>
        ///// <returns></returns>
        //public bool Update(StandardSolutionCheckEntity model)
        //{
        //    return m_StandardSolutionCheckDAL.Update(model);
        //}

        ///// <summary>
        ///// 删除一条数据
        ///// </summary>
        ///// <param name="actionID">工作ID</param>
        ///// <param name="pointId">测点Id</param>
        ///// <param name="pollutantCode">因子代码</param>
        ///// <param name="tstamp">时间戳</param>
        ///// <returns></returns>
        //public bool Delete(string actionID, string pointId, string pollutantCode, DateTime tstamp, string SampleNumber)
        //{
        //    return m_StandardSolutionCheckDAL.Delete(actionID, pointId, pollutantCode, tstamp, SampleNumber);
        //}
        #endregion
    }
}
