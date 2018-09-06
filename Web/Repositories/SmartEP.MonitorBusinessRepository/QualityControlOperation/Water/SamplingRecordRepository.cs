using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.QualityControlOperation.Water;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.WaterQualityControlOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：SamplingRecordRepository.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 采样记录仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SamplingRecordRepository //: BaseGenericRepository<BaseDataModel, SamplingRecordEntity>
    {
        ///// <summary>
        ///// 根据key主键判断记录是否存在
        ///// </summary>
        ///// <param name="strKey"></param>
        ///// <returns></returns>
        //public override bool IsExist(string strKey)
        //{
        //    return RetrieveCount(x => x.id.Equals(strKey)) == 0 ? false : true;
        //}

        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        SamplingRecordDAL m_SamplingRecordDAL = Singleton<SamplingRecordDAL>.GetInstance();

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "SamplingDate")
        {
            return m_SamplingRecordDAL.GetDataPager(portIds, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
        }

        public DataView GetData(string missionId,DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "SamplingDate")
        {
            return m_SamplingRecordDAL.GetData(missionId,dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得所有数据供导出
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, DateTime dtmStart, DateTime dtmEnd, string orderBy = "SamplingDate")
        {
            return m_SamplingRecordDAL.GetExportData(portIds, dtmStart, dtmEnd, orderBy);
        }

        ///// <summary>
        ///// 取得详情表所有数据供导出
        ///// </summary>
        ///// <param name="ids">主键数据</param>
        ///// <param name="portIds">测点数据</param>
        ///// <param name="sampleNumbers">样品编号</param>
        ///// <param name="dateStart">开始时间</param>
        ///// <param name="dateEnd">截止时间</param>
        ///// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        ///// <returns></returns>
        //public DataView GetDetailTableExportData(string[] ids, string[] portIds, string[] sampleNumbers, DateTime dtmStart, DateTime dtmEnd, string orderBy = "SamplingDate")
        //{
        //    return m_SamplingRecordDAL.GetDetailTableExportData(ids, portIds, sampleNumbers, dtmStart, dtmEnd, orderBy);
        //}

        /// <summary>
        /// 批量增加数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public int Add(params SamplingRecordEntity[] models)
        {
            return m_SamplingRecordDAL.Add(models);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public int Update(params SamplingRecordEntity[] models)
        {
            return m_SamplingRecordDAL.Update(models);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int Delete(Guid id)
        {
            return m_SamplingRecordDAL.Delete(id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public SamplingRecordEntity GetModel(Guid id)
        {
            return m_SamplingRecordDAL.GetModel(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return m_SamplingRecordDAL.GetList(strWhere);
        }
        #endregion
    }
}
