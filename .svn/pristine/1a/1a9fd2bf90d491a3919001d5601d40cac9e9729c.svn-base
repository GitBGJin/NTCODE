using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
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
    /// 名称：SamplingRecordDetailService.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-10-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：采样记录详情查询类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SamplingRecordDetailService
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();

        /// <summary>
        /// 采样记录详情仓储类
        /// </summary>
        SamplingRecordDetailRepository g_SamplingRecordDetailRepository = Singleton<SamplingRecordDetailRepository>.GetInstance();

        /// <summary>
        /// 监测点：【水】测点扩展信息类
        /// </summary>
        MonitoringPointExtensionForEQMSWaterRepository g_ExtensionForEQMSWater = null;

        #region 获取数据基础方法
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
            return g_SamplingRecordDetailRepository.GetDataPager(portIds, sampleNumbers, dtmStart, dtmEnd, out recordTotal, pageSize, pageNo, orderBy);
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
        public DataView GetData(Guid guid, string orderBy = "SamplingTime")
        {
            DataTable dt = new DataTable();
            dt = g_SamplingRecordDetailRepository.GetData(guid, orderBy).Table;
            dt.Columns.Add("PollutantName", typeof(string));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string code = dt.Rows[i]["PollutantCode"].ToString();
                    switch (code)
                    {
                        case "w01010":
                            dt.Rows[i]["PollutantName"] = "水温";
                            break;
                        case "w01001":
                            dt.Rows[i]["PollutantName"] = "PH值";
                            break;
                        case "w01009":
                            dt.Rows[i]["PollutantName"] = "溶解氧";
                            break;
                        case "w01014":
                            dt.Rows[i]["PollutantName"] = "电导率";
                            break;
                        case "w02023":
                            dt.Rows[i]["PollutantName"] = "叶绿素";
                            break;
                        case "w19011":
                            dt.Rows[i]["PollutantName"] = "藻密度";
                            break;
                    }
                }
            }
            return new DataView(dt);
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
        public DataView GetDataNew(Guid guid,string orderBy = "SamplingTime")
        {
            DataTable dt = new DataTable();
            dt = g_SamplingRecordDetailRepository.GetDataNew(guid, orderBy).Table;

            dt.Columns.Add("PollutantName", typeof(string)).SetOrdinal(3);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string code = dt.Rows[i]["PollutantCode"].ToString();
                    switch (code)
                    {
                        case "w01010":
                            dt.Rows[i]["PollutantName"] = "水温";
                            break;
                        case "w01001":
                            dt.Rows[i]["PollutantName"] = "PH值";
                            break;
                        case "w01009":
                            dt.Rows[i]["PollutantName"] = "溶解氧";
                            break;
                        case "w01014":
                            dt.Rows[i]["PollutantName"] = "电导率";
                            break;
                        case "w02023":
                            dt.Rows[i]["PollutantName"] = "叶绿素";
                            break;
                        case "w19011":
                            dt.Rows[i]["PollutantName"] = "藻密度";
                            break;
                    }
                }
            }
            return new DataView(dt);
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
            return g_SamplingRecordDetailRepository.GetExportData(portIds, sampleNumbers, dtmStart, dtmEnd, orderBy);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public int Add(SamplingRecordDetailEntity model)
        {
            return g_SamplingRecordDetailRepository.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public int Update(SamplingRecordDetailEntity model)
        {
            return g_SamplingRecordDetailRepository.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public int Update(RealSampleEntity model)
        {
            return g_SamplingRecordDetailRepository.Update(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int Delete(Guid id)
        {
            return g_SamplingRecordDetailRepository.Delete(id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public SamplingRecordDetailEntity GetModel(Guid id)
        {
            return g_SamplingRecordDetailRepository.GetModel(id);
        }
        #endregion

    }
}
