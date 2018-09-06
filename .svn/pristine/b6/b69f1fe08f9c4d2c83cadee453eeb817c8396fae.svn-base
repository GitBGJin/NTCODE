using SmartEP.Core.Generic;
using SmartEP.DomainModel.Framework;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Air;
using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Air
{
    /// <summary>
    /// 名称：InstrumentFaultService.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-05-24
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：仪器表单处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InstrumentFaultService
    {
        /// <summary>
        /// 仪器使用记录仓储类
        /// </summary>
        InstrumentFaultRepository g_InstrumentFault = Singleton<InstrumentFaultRepository>.GetInstance();
        QualityControlDataSearchService dataSearchService = new QualityControlDataSearchService();

        /// <summary>
        /// 质控运维WebService接口
        /// </summary>
        //TempGetDataWebServiceSoapClient sr = Singleton<TempGetDataWebServiceSoapClient>.GetInstance();
        /// <summary>
        /// 运维平台获取数据WebService路径
        /// </summary>
        private string m_OperationOMMPGetDataWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationOMMPGetDataWebServiceUrl"].ToString();

        /// <summary>
        /// 运维平台任务处理WebService路径
        /// </summary>
        private string m_OperationTaskWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationTaskWebServiceUrl"].ToString();

        #region 获取数据基础方法
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
            DataTable dt = g_InstrumentFault.GetDataPager(Users, dtmStart, dtmEnd, operateContent,objectType, pageSize, pageNo, out recordTotal, orderBy).Table;
            //dt.Columns.Add("InstrumentName", typeof(string));
            //DataView dv = dataSearchService.GetDataPager(objectType);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow[] dr = dv.ToTable().Select("RowGuid='" + dt.Rows[i]["InstanceGuid"].ToString() + "'");
            //    if (dr.Length > 0)
            //        dt.Rows[i]["InstrumentName"] = dr[0]["InstrumentName"].ToString();
            //}
            DataView dvNew = new DataView(dt);
            return dvNew;
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
            DataTable dt = g_InstrumentFault.GetDataNewPager(Users, dtmStart, dtmEnd, operateContent, objectType, pageSize, pageNo, out recordTotal, orderBy).Table;
            //dt.Columns.Add("InstrumentName", typeof(string));
            //DataView dv = dataSearchService.GetDataPager(objectType);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow[] dr = dv.ToTable().Select("RowGuid='" + dt.Rows[i]["InstanceGuid"].ToString() + "'");
            //    if (dr.Length > 0)
            //        dt.Rows[i]["InstrumentName"] = dr[0]["InstrumentName"].ToString();
            //}
            DataView dvNew = new DataView(dt);
            return dvNew;
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

            return g_InstrumentFault.GetExportData(portIds, dtmStart, dtmEnd, orderBy);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Add(InstrumentInstanceTimeRecordEntity model)
        {
            return g_InstrumentFault.Add(model);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int AddRecord2(InstrumentInstanceRecord2Entity model)
        {
            return g_InstrumentFault.AddRecord2(model);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int AddRecord3(InstrumentInstanceRecord3Entity model)
        {
            return g_InstrumentFault.AddRecord3(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Update(InstrumentInstanceTimeRecordEntity model)
        {
            return g_InstrumentFault.Update(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int UpdateRecord2(InstrumentInstanceRecord2Entity model)
        {
            return g_InstrumentFault.UpdateRecord2(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int UpdateRecord3(InstrumentInstanceRecord3Entity model)
        {
            return g_InstrumentFault.UpdateRecord3(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int DeleteRecord2(int id)
        {
            return g_InstrumentFault.DeleteRecord2(id);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int DeleteRecord3(int id)
        {
            return g_InstrumentFault.DeleteRecord3(id);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public InstrumentInstanceTimeRecordEntity GetModel(int rowGuid)
        {
            return g_InstrumentFault.GetModel(rowGuid);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public InstrumentInstanceRecord2Entity GetModelRecord2(int rowGuid)
        {
            return g_InstrumentFault.GetModelRecord2(rowGuid);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public InstrumentInstanceRecord3Entity GetModelRecord3(int rowGuid)
        {
            return g_InstrumentFault.GetModelRecord3(rowGuid);
        }
        #endregion
    }
}
