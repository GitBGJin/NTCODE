using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.QualityControlOperation.Water;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.DomainModel.WaterQualityControlOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：SamplingRecordDetailRepository.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 采样记录详情仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SamplingRecordDetailRepository //: BaseGenericRepository<BaseDataModel, SamplingRecordDetailEntity>
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
        SamplingRecordDetailDAL m_SamplingRecordDetailDAL = Singleton<SamplingRecordDetailDAL>.GetInstance();

        /// <summary>
        /// 取得详情表的查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="sampleNumbers">样品编号</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] sampleNumbers,
            DateTime dtmStart, DateTime dtmEnd, out int recordTotal, int pageSize = int.MaxValue, int pageNo = 0,
             string orderBy = "SamplingTime")
        {
            return m_SamplingRecordDetailDAL.GetDataPager(portIds, sampleNumbers, dtmStart, dtmEnd, out recordTotal, pageSize, pageNo, orderBy);
        }

        /// <summary>
        /// 取得详情表的查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="sampleNumbers">样品编号</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetData(Guid id,string orderBy = "SamplingTime")
        {
            return m_SamplingRecordDetailDAL.GetData(id, orderBy);
        }
        /// <summary>
        /// 取得详情表的查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="sampleNumbers">样品编号</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataNew(Guid id, string orderBy = "SamplingTime")
        {
            return m_SamplingRecordDetailDAL.GetDataNew(id, orderBy);
        }
        /// <summary>
        /// 取得详情表所有数据供导出
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="sampleNumbers">样品编号</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, string[] sampleNumbers, DateTime dtmStart, DateTime dtmEnd, string orderBy = "SamplingTime")
        {
            return m_SamplingRecordDetailDAL.GetExportData(portIds, sampleNumbers, dtmStart, dtmEnd, orderBy);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public int Add(params SamplingRecordDetailEntity[] models)
        {
            return m_SamplingRecordDetailDAL.Add(models);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public int Update(SamplingRecordDetailEntity model)
        {
            return m_SamplingRecordDetailDAL.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public int Update(RealSampleEntity model)
        {
            return m_SamplingRecordDetailDAL.Update(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int Delete(Guid id)
        {
            return m_SamplingRecordDetailDAL.Delete(id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public SamplingRecordDetailEntity GetModel(Guid id)
        {
            return m_SamplingRecordDetailDAL.GetModel(id);
        }
        #endregion
    }
}
