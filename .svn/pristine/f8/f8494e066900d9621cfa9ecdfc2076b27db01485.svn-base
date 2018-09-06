using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.QualityControlOperation.Air;
using SmartEP.DomainModel.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Air
{
    /// <summary>
    /// 名称：InstrumentFaultRepository.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-05-24
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 仪器表单处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InstrumentFaultRepository
    {
        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        InstrumentFaultDAL m_InstrumentFaultDAL = Singleton<InstrumentFaultDAL>.GetInstance();

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
        public DataView GetDataPager(string[] Users, DateTime dtmStart, DateTime dtmEnd, string operateContent, string objectType, int pageSize, int pageNo, out int recordTotal, string orderBy = "OperateDate")
        {
            return m_InstrumentFaultDAL.GetDataPager( Users, dtmStart, dtmEnd,operateContent,objectType, pageSize, pageNo, out recordTotal, orderBy);
        }
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
        public DataView GetDataNewPager(string[] Users, DateTime dtmStart, DateTime dtmEnd, string operateContent, string objectType, int pageSize, int pageNo, out int recordTotal, string orderBy = "OperateDate")
        {
            return m_InstrumentFaultDAL.GetDataNewPager(Users, dtmStart, dtmEnd, operateContent, objectType, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得所有数据供导出
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, DateTime dtmStart, DateTime dtmEnd, string orderBy = "OccurTime")
        {
            return m_InstrumentFaultDAL.GetExportData(portIds, dtmStart, dtmEnd, orderBy);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Add(InstrumentInstanceTimeRecordEntity model)
        {
            return m_InstrumentFaultDAL.Add(model);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int AddRecord2(InstrumentInstanceRecord2Entity model)
        {
            return m_InstrumentFaultDAL.AddRecord2(model);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int AddRecord3(InstrumentInstanceRecord3Entity model)
        {
            return m_InstrumentFaultDAL.AddRecord3(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Update(InstrumentInstanceTimeRecordEntity model)
        {
            return m_InstrumentFaultDAL.Update(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int UpdateRecord2(InstrumentInstanceRecord2Entity model)
        {
            return m_InstrumentFaultDAL.UpdateRecord2(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int UpdateRecord3(InstrumentInstanceRecord3Entity model)
        {
            return m_InstrumentFaultDAL.UpdateRecord3(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int DeleteRecord2(int id)
        {
            return m_InstrumentFaultDAL.DeleteRecord2(id);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int DeleteRecord3(int id)
        {
            return m_InstrumentFaultDAL.DeleteRecord3(id);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public InstrumentInstanceTimeRecordEntity GetModel(int rowguid)
        {
            return m_InstrumentFaultDAL.GetModel(rowguid);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public InstrumentInstanceRecord2Entity GetModelRecord2(int rowGuid)
        {
            return m_InstrumentFaultDAL.GetModelRecord2(rowGuid);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public InstrumentInstanceRecord3Entity GetModelRecord3(int rowGuid)
        {
            return m_InstrumentFaultDAL.GetModelRecord3(rowGuid);
        }
        #endregion
    }
}
