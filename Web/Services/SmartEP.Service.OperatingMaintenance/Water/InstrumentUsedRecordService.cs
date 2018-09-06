using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.OperatingMaintenance.ServiceReference;
using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：InstrumentUsedRecordService.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-10-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：仪器使用记录查询类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InstrumentUsedRecordService
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();

        /// <summary>
        /// 仪器使用记录仓储类
        /// </summary>
        InstrumentUsedRecordRepository g_InstrumentUsedRecordRepository = Singleton<InstrumentUsedRecordRepository>.GetInstance();

        /// <summary>
        /// 监测点：【水】测点扩展信息类
        /// </summary>
        MonitoringPointExtensionForEQMSWaterRepository g_ExtensionForEQMSWater = null;

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
        public DataView GetDataPager(string[] portIds,string[] Instruments,string[] Users, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "UsedDate")
        {
            DataTable dt=g_InstrumentUsedRecordRepository.GetDataPager(portIds,Instruments,Users, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("PointName", typeof(string));
            for (int j = 0; j < portIds.Length; j++)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(portIds[j]);
                    dt.Rows[i]["PointName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                }
            }
            DataView dv = new DataView(dt);
            return dv;
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

            return g_InstrumentUsedRecordRepository.GetExportData(portIds, dtmStart, dtmEnd, orderBy);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Add(InstrumentUsedRecordEntity model)
        {
            return g_InstrumentUsedRecordRepository.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Update(InstrumentUsedRecordEntity model)
        {
            return g_InstrumentUsedRecordRepository.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int Delete(Guid id)
        {
            return g_InstrumentUsedRecordRepository.Delete(id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public InstrumentUsedRecordEntity GetModel(Guid id)
        {
            return g_InstrumentUsedRecordRepository.GetModel(id);
        }

        /// <summary>
        /// 通过站点ID获取站点内的所有仪器
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataTable GetInstrument(string pointId)
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl, "TempGetDataWebService", "GetInsTypeByObjectID", new object[] { pointId });
            return objData as DataTable;
            //return sr.GetInsTypeByObjectID(pointId);
        }
        #endregion
    }
}
