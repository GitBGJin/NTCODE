using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using SmartEP.Service.BaseData.MPInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：SamplingRecordService.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-10-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：采样记录查询类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SamplingRecordService
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();

        /// <summary>
        /// 采样记录仓储类
        /// </summary>
        SamplingRecordRepository g_SamplingRecordRepository = Singleton<SamplingRecordRepository>.GetInstance();
        /// <summary>
        /// 采样记录从表
        /// </summary>
        SamplingRecordDetailService g_SamplingRecordDetail = Singleton<SamplingRecordDetailService>.GetInstance();
        /// <summary>
        /// 监测点：【水】测点扩展信息类
        /// </summary>
        MonitoringPointExtensionForEQMSWaterRepository g_ExtensionForEQMSWater = null;

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
        public DataView GetDataPager(string[] portIds, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "SamplingDate")
        {
            DataTable dt = new DataTable();
            dt = g_SamplingRecordRepository.GetDataPager(portIds, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("SampleNumber", typeof(string));  //样品编号
            dt.Columns.Add("SamplingTime", typeof(string)); //采样时间
            dt.Columns.Add("SamplingPosition", typeof(string));//采样位置
            dt.Columns.Add("InstrumentNumber", typeof(string)); //仪器编号
            dt.Columns.Add("ComparisonAnalysisProject", typeof(string)); //比对分析项目
            dt.Columns.Add("Remark", typeof(string));  //备注
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string id = dt.Rows[i]["id"].ToString();
                    SamplingRecordDetailEntity entity = g_SamplingRecordDetail.GetModel(new Guid(id));
                    dt.Rows[i]["SampleNumber"] = entity.SampleNumber;
                    dt.Rows[i]["SamplingTime"] = entity.SamplingTime;
                    dt.Rows[i]["SamplingPosition"] = entity.SamplingPosition;
                    dt.Rows[i]["InstrumentNumber"] = entity.InstrumentNumber;
                    dt.Rows[i]["ComparisonAnalysisProject"] = entity.ComparisonAnalysisProject;
                    dt.Rows[i]["Remark"] = entity.Remark;
                }
            }
            return new DataView(dt);
        }

        public DataView GetData(string missionId,DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "SamplingDate")
        {
            DataTable dt = new DataTable();
            dt = g_SamplingRecordRepository.GetData(missionId,dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy).Table;
            return new DataView(dt);
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
            DataTable dt = new DataTable();
            dt = g_SamplingRecordRepository.GetExportData(portIds, dtmStart, dtmEnd, orderBy).Table;
            dt.Columns.Add("SampleNumber", typeof(string));  //样品编号
            dt.Columns.Add("SamplingTime", typeof(string)); //采样时间
            dt.Columns.Add("SamplingPosition", typeof(string));//采样位置
            dt.Columns.Add("InstrumentNumber", typeof(string)); //仪器编号
            dt.Columns.Add("ComparisonAnalysisProject", typeof(string)); //比对分析项目
            dt.Columns.Add("Remark", typeof(string));  //备注
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string id = dt.Rows[i]["id"].ToString();
                    SamplingRecordDetailEntity entity = g_SamplingRecordDetail.GetModel(new Guid(id));
                    dt.Rows[i]["SampleNumber"] = entity.SampleNumber;
                    dt.Rows[i]["SamplingTime"] = entity.SamplingTime;
                    dt.Rows[i]["SamplingPosition"] = entity.SamplingPosition;
                    dt.Rows[i]["InstrumentNumber"] = entity.InstrumentNumber;
                    dt.Rows[i]["ComparisonAnalysisProject"] = entity.ComparisonAnalysisProject;
                    dt.Rows[i]["Remark"] = entity.Remark;
                }
            }
            return new DataView(dt);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public int Add(SamplingRecordEntity model)
        {
            return g_SamplingRecordRepository.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public int Update(SamplingRecordEntity model)
        {
            return g_SamplingRecordRepository.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int Delete(Guid id)
        {
            return g_SamplingRecordRepository.Delete(id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public SamplingRecordEntity GetModel(Guid id)
        {
            return g_SamplingRecordRepository.GetModel(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataTable GetList(string taskCode)
        {
            string strWhere = string.Format(@" TaskCode='{0}' ", taskCode);
            return g_SamplingRecordRepository.GetList(strWhere);
        }
        #endregion
    }
}
