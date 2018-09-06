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
    /// 名称：InstrumentUsedRecordRepository.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 仪器使用记录仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InstrumentUsedRecordRepository //: BaseGenericRepository<BaseDataModel, InstrumentUsedRecordEntity>
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
        InstrumentUsedRecordDAL m_InstrumentUsedRecordDAL = Singleton<InstrumentUsedRecordDAL>.GetInstance();

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
        public DataView GetDataPager(string[] portIds, string[] Instruments,string[] Users, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "UsedDate")
        {
            return m_InstrumentUsedRecordDAL.GetDataPager(portIds, Instruments,Users, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 取得所有数据供导出
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, DateTime dtmStart, DateTime dtmEnd, string orderBy = "UsedDate")
        {
            return m_InstrumentUsedRecordDAL.GetExportData(portIds, dtmStart, dtmEnd, orderBy);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Add(InstrumentUsedRecordEntity model)
        {
            return m_InstrumentUsedRecordDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Update(InstrumentUsedRecordEntity model)
        {
            return m_InstrumentUsedRecordDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int Delete(Guid id)
        {
            return m_InstrumentUsedRecordDAL.Delete(id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public InstrumentUsedRecordEntity GetModel(Guid id)
        {
            return m_InstrumentUsedRecordDAL.GetModel(id);
        }
        #endregion
    }
}
