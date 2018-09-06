using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.OperatingMaintenance.ServiceReference;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：StandardSolutionCheckService.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-10-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：质控任务查询类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class StandardSolutionCheckService
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();

        /// <summary>
        /// 质控运维WebService接口
        /// </summary>
        //TempGetDataWebServiceSoapClient sr = Singleton<TempGetDataWebServiceSoapClient>.GetInstance();

        /// <summary>
        /// 采样记录仓储类
        /// </summary>
        StandardSolutionCheckRepository g_StandardSolutionCheckRepository = Singleton<StandardSolutionCheckRepository>.GetInstance();

        /// <summary>
        /// 任务配置数据库处理
        /// </summary>
        TaskConfigService g_TaskConfigService = Singleton<TaskConfigService>.GetInstance();

        /// <summary>
        /// 提供仪器通道信息服务
        /// </summary>
        InstrumentChannelService s_InstrumentChannel = Singleton<InstrumentChannelService>.GetInstance();

        /// <summary>
        /// 监测点：【水】测点扩展信息类
        /// </summary>
        MonitoringPointExtensionForEQMSWaterRepository g_ExtensionForEQMSWater = null;

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
        public DataView GetDataPager(string[] portIds, string missionId, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            return g_StandardSolutionCheckRepository.GetDataPager(portIds, missionId, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);

        }
        /// <summary>
        /// 根据仪器类型取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPagers(string[] portIds, string taskCode, string missionId, string Type, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            return g_StandardSolutionCheckRepository.GetDataPagers(portIds, taskCode, missionId, Type, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);

        }

        /// <summary>
        /// 质控任务取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPagerQControl(string[] portIds, string[] missionIds, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string[] evaluate, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            DataTable dt = new DataTable();
            dt = g_StandardSolutionCheckRepository.GetDataPagerQControl(portIds, missionIds, pollutantCodes, dtmStart, dtmEnd, evaluate, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("MissionName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string missionId = dt.Rows[i]["MissionID"].ToString();
                string strWhere = missionId;
                dt.Rows[i]["MissionName"] = g_TaskConfigService.GetName(strWhere)[0];
            }
            return dt.AsDataView();
        }
        /// <summary>
        /// 质控任务取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPagerQControl(string[] portIds, string missionId, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string[] evaluate, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            DataTable dt = new DataTable();
            dt = g_StandardSolutionCheckRepository.GetDataPagerQControl(portIds, missionId, pollutantCodes, dtmStart, dtmEnd, evaluate, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("MissionName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string MissionId = dt.Rows[i]["MissionID"].ToString();
                string strWhere = MissionId;
                dt.Rows[i]["MissionName"] = g_TaskConfigService.GetName(strWhere)[0];
            }
            return dt.AsDataView();
        }

        /// <summary>
        /// 质控任务取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPagerQControlNew(string[] portIds, string missionId, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string[] evaluate, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            DataTable dt = new DataTable();
            dt = g_StandardSolutionCheckRepository.GetDataPagerQControlNew(portIds, missionId, pollutantCodes, dtmStart, dtmEnd, evaluate, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("MissionName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string MissionId = dt.Rows[i]["MissionID"].ToString();
                string strWhere = MissionId;
                dt.Rows[i]["MissionName"] = g_TaskConfigService.GetName(strWhere)[0];

                string PollutantName = dt.Rows[i]["PollutantName"].ToString();
                switch (PollutantName)
                {
                    case "PH值":
                        {
                            if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.1)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                    case "溶解氧":
                        {
                            if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.5)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                    case "氨氮":
                        {
                            if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.02)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                    case "总磷":
                        {
                            if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.02)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                    default:
                        {
                            if (dt.Rows[i]["RelativeOffset"].ToString() != "" && !dt.Rows[i]["RelativeOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["RelativeOffset"].ToString()) <= 10)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                }
            }
            return dt.AsDataView();
        }

        /// <summary>
        /// 质控任务取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPagerQControlNew2(string[] portIds, string missionId, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            DataTable dt = new DataTable();
            dt = g_StandardSolutionCheckRepository.GetDataPagerQControlNew2(portIds, missionId, pollutantCodes, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("MissionName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string MissionId = dt.Rows[i]["MissionID"].ToString();
                string strWhere = MissionId;
                dt.Rows[i]["MissionName"] = g_TaskConfigService.GetName(strWhere)[0];
                dt.Rows[i]["Tester"] += ";苏明玉";
                string PollutantName = dt.Rows[i]["PollutantName"].ToString();
                switch (PollutantName)
                {
                    case "pH值":
                        {
                            if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.1)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                    case "溶解氧":
                        {
                            if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.5)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                    case "氨氮":
                        {
                            if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.02)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                    case "总磷":
                        {
                            if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.02)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                    default:
                        {
                            if (dt.Rows[i]["RelativeOffset"].ToString() != "" && !dt.Rows[i]["RelativeOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["RelativeOffset"].ToString()) <= 10)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                }
            }
            return dt.AsDataView();
        }

        public string GetDecimalDigit(string PollutantName)
        {
            return g_StandardSolutionCheckRepository.GetDecimalDigit(PollutantName);
        }

        /// <summary>
        /// 质控任务取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetAvgDataControl(string[] portIds, string[] missionIds, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string[] evaluate, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("RelativeAvg", typeof(string));
            dtNew.Columns.Add("RelativeMax", typeof(string));
            dtNew.Columns.Add("RelativeMin", typeof(string));
            dtNew.Columns.Add("AbsoluteAvg", typeof(string));
            dtNew.Columns.Add("AbsoluteMax", typeof(string));
            dtNew.Columns.Add("AbsoluteMin", typeof(string));
            DataTable dt = new DataTable();
            dt = g_StandardSolutionCheckRepository.GetDataPagerQControl(portIds, missionIds, pollutantCodes, dtmStart, dtmEnd, evaluate, pageSize, pageNo, out recordTotal, orderBy).Table;
            decimal RelativeAvg = 0;
            decimal RelativeMax = 0;
            decimal RelativeMin = 9999;
            decimal RelativeCount = 0;
            decimal AbsoluteAvg = 0;
            decimal AbsoluteMax = 0;
            decimal AbsoluteMin = 9999;
            decimal AbsoluteCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string factorCode = dt.Rows[i]["PollutantCode"].ToString();
                if (factorCode == "w01009" || factorCode == "w01010" || factorCode == "w01003" || factorCode == "w01001" || factorCode == "w01014" || factorCode == "w21003" || factorCode == "w21011")
                {
                    if (dt.Rows[i]["AbsoluteOffset"].ToString() != "")
                    {
                        AbsoluteCount++;
                        AbsoluteAvg += Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]);
                        if (AbsoluteMax < Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]))
                            AbsoluteMax = Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]);
                        if (AbsoluteMin > Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]))
                            AbsoluteMin = Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]);
                    }
                }
                else
                {
                    if (dt.Rows[i]["RelativeOffset"].ToString() != "")
                    {
                        RelativeCount++;
                        RelativeAvg += Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]);
                        if (RelativeMax < Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]))
                            RelativeMax = Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]);
                        if (RelativeMin > Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]))
                            RelativeMin = Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]);
                    }
                }
            }
            DataRow newRow = dtNew.NewRow();
            if (RelativeCount != 0)
            {
                newRow["RelativeAvg"] = DecimalExtension.GetPollutantValue(RelativeAvg / RelativeCount, 1) + "%";
                newRow["RelativeMax"] = DecimalExtension.GetPollutantValue(RelativeMax, 1) + "%";
                newRow["RelativeMin"] = DecimalExtension.GetPollutantValue(RelativeMin, 1) + "%";
            }
            else
            {
                newRow["RelativeAvg"] = "/";
                newRow["RelativeMax"] = "/";
                newRow["RelativeMin"] = "/";
            }

            if (AbsoluteCount != 0)
            {
                newRow["AbsoluteAvg"] = DecimalExtension.GetPollutantValue(AbsoluteAvg / AbsoluteCount, 3);
                newRow["AbsoluteMax"] = DecimalExtension.GetPollutantValue(AbsoluteMax, 3);
                newRow["AbsoluteMin"] = DecimalExtension.GetPollutantValue(AbsoluteMin, 3);
            }
            else
            {
                newRow["AbsoluteAvg"] = "/";
                newRow["AbsoluteMax"] = "/";
                newRow["AbsoluteMin"] = "/";
            }
            dtNew.Rows.Add(newRow);
            return dtNew.AsDataView();
        }
        /// <summary>
        /// 质控任务取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetAvgDataControl(string[] portIds, string missionId, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string[] evaluate, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("RelativeAvg", typeof(string));
            dtNew.Columns.Add("RelativeMax", typeof(string));
            dtNew.Columns.Add("RelativeMin", typeof(string));
            dtNew.Columns.Add("AbsoluteAvg", typeof(string));
            dtNew.Columns.Add("AbsoluteMax", typeof(string));
            dtNew.Columns.Add("AbsoluteMin", typeof(string));
            DataTable dt = new DataTable();
            dt = g_StandardSolutionCheckRepository.GetDataPagerQControl(portIds, missionId, pollutantCodes, dtmStart, dtmEnd, evaluate, pageSize, pageNo, out recordTotal, orderBy).Table;
            decimal RelativeAvg = 0;
            decimal RelativeMax = 0;
            decimal RelativeMin = 9999;
            decimal RelativeCount = 0;
            decimal AbsoluteAvg = 0;
            decimal AbsoluteMax = 0;
            decimal AbsoluteMin = 9999;
            decimal AbsoluteCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string factorCode = dt.Rows[i]["PollutantCode"].ToString();
                if (factorCode == "w01009" || factorCode == "w01010" || factorCode == "w01003" || factorCode == "w01001" || factorCode == "w01014" || factorCode == "w21003" || factorCode == "w21011")
                {
                    if (dt.Rows[i]["AbsoluteOffset"].ToString() != "")
                    {
                        AbsoluteCount++;
                        AbsoluteAvg += Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]);
                        if (AbsoluteMax < Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]))
                            AbsoluteMax = Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]);
                        if (AbsoluteMin > Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]))
                            AbsoluteMin = Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]);
                    }
                }
                else
                {
                    if (dt.Rows[i]["RelativeOffset"].ToString() != "")
                    {
                        RelativeCount++;
                        RelativeAvg += Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]);
                        if (RelativeMax < Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]))
                            RelativeMax = Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]);
                        if (RelativeMin > Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]))
                            RelativeMin = Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]);
                    }
                }
            }
            DataRow newRow = dtNew.NewRow();
            if (RelativeCount != 0)
            {
                newRow["RelativeAvg"] = DecimalExtension.GetPollutantValue(RelativeAvg / RelativeCount, 1) + "%";
                newRow["RelativeMax"] = DecimalExtension.GetPollutantValue(RelativeMax, 1) + "%";
                newRow["RelativeMin"] = DecimalExtension.GetPollutantValue(RelativeMin, 1) + "%";
            }
            else
            {
                newRow["RelativeAvg"] = "/";
                newRow["RelativeMax"] = "/";
                newRow["RelativeMin"] = "/";
            }

            if (AbsoluteCount != 0)
            {
                newRow["AbsoluteAvg"] = DecimalExtension.GetPollutantValue(AbsoluteAvg / AbsoluteCount, 3);
                newRow["AbsoluteMax"] = DecimalExtension.GetPollutantValue(AbsoluteMax, 3);
                newRow["AbsoluteMin"] = DecimalExtension.GetPollutantValue(AbsoluteMin, 3);
            }
            else
            {
                newRow["AbsoluteAvg"] = "/";
                newRow["AbsoluteMax"] = "/";
                newRow["AbsoluteMin"] = "/";
            }
            dtNew.Rows.Add(newRow);
            return dtNew.AsDataView();
        }
        /// <summary>
        /// 质控任务取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="Type">仪器类型</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetAvgDataControlNew(string[] portIds, string missionId, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string[] evaluate, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("RelativeAvg", typeof(string));
            dtNew.Columns.Add("RelativeMax", typeof(string));
            dtNew.Columns.Add("RelativeMin", typeof(string));
            dtNew.Columns.Add("AbsoluteAvg", typeof(string));
            dtNew.Columns.Add("AbsoluteMax", typeof(string));
            dtNew.Columns.Add("AbsoluteMin", typeof(string));
            DataTable dt = new DataTable();
            dt = g_StandardSolutionCheckRepository.GetDataPagerQControlNew2(portIds, missionId, pollutantCodes, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy).Table;
            decimal RelativeAvg = 0;
            decimal RelativeMax = 0;
            decimal RelativeMin = 9999;
            decimal RelativeCount = 0;
            decimal AbsoluteAvg = 0;
            decimal AbsoluteMax = 0;
            decimal AbsoluteMin = 9999;
            decimal AbsoluteCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string factorCode = dt.Rows[i]["PollutantCode"].ToString();
                if (factorCode == "w01009" || factorCode == "w01010" || factorCode == "w01003" || factorCode == "w01001" || factorCode == "w01014" || factorCode == "w21003" || factorCode == "w21011")
                {
                    if (dt.Rows[i]["AbsoluteOffset"].ToString() != "")
                    {
                        AbsoluteCount++;
                        AbsoluteAvg += Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]);
                        if (AbsoluteMax < Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]))
                            AbsoluteMax = Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]);
                        if (AbsoluteMin > Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]))
                            AbsoluteMin = Convert.ToDecimal(dt.Rows[i]["AbsoluteOffset"]);
                    }
                }
                else
                {
                    if (dt.Rows[i]["RelativeOffset"].ToString() != "")
                    {
                        RelativeCount++;
                        RelativeAvg += Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]);
                        if (RelativeMax < Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]))
                            RelativeMax = Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]);
                        if (RelativeMin > Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]))
                            RelativeMin = Convert.ToDecimal(dt.Rows[i]["RelativeOffset"]);
                    }
                }
            }
            DataRow newRow = dtNew.NewRow();
            if (RelativeCount != 0)
            {
                newRow["RelativeAvg"] = DecimalExtension.GetPollutantValue(RelativeAvg / RelativeCount, 1) + "%";
                newRow["RelativeMax"] = DecimalExtension.GetPollutantValue(RelativeMax, 1) + "%";
                newRow["RelativeMin"] = DecimalExtension.GetPollutantValue(RelativeMin, 1) + "%";
            }
            else
            {
                newRow["RelativeAvg"] = "/";
                newRow["RelativeMax"] = "/";
                newRow["RelativeMin"] = "/";
            }

            if (AbsoluteCount != 0)
            {
                newRow["AbsoluteAvg"] = DecimalExtension.GetPollutantValue(AbsoluteAvg / AbsoluteCount, 3);
                newRow["AbsoluteMax"] = DecimalExtension.GetPollutantValue(AbsoluteMax, 3);
                newRow["AbsoluteMin"] = DecimalExtension.GetPollutantValue(AbsoluteMin, 3);
            }
            else
            {
                newRow["AbsoluteAvg"] = "/";
                newRow["AbsoluteMax"] = "/";
                newRow["AbsoluteMin"] = "/";
            }
            dtNew.Rows.Add(newRow);
            return dtNew.AsDataView();
        }

        public DataView GetDataToExcel(string[] portIds, string[] missionIds, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string orderBy = "TaskCode,ActionID")
        {
            return g_StandardSolutionCheckRepository.GetDataToExcel(portIds, missionIds, pollutantCodes, dtmStart, dtmEnd, orderBy);
        }

        /// <summary>
        /// 取得所有数据供导出
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, DateTime dtmStart, DateTime dtmEnd, string orderBy = "Tstamp")
        {
            return g_StandardSolutionCheckRepository.GetExportData(portIds, dtmStart, dtmEnd, orderBy);
        }

        public DataView GetNum(string[] portIds, string[] missionIds, string[] actionIDs, string[] pollutantCodes, DateTime dtmStart, DateTime dtmEnd, string orderBy = "TaskCode,ActionID")
        {
            return g_StandardSolutionCheckRepository.GetNum(portIds, missionIds, actionIDs, pollutantCodes, dtmStart, dtmEnd, orderBy);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public void Add(StandardSolutionCheckEntity model)
        {
            g_StandardSolutionCheckRepository.Add(model);
        }

        ///// <summary>
        ///// 批量增加数据
        ///// </summary>
        ///// <param name="model">采样记录实体数组</param>
        ///// <returns></returns>
        //public bool Add(StandardSolutionCheckEntity[] models)
        //{
        //    return g_StandardSolutionCheckRepository.Add(models);
        //}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public void Update(StandardSolutionCheckEntity model)
        {
            g_StandardSolutionCheckRepository.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="actionID">工作ID</param>
        /// <param name="pointId">测点Id</param>
        /// <param name="pollutantCode">因子代码</param>
        /// <param name="tstamp">时间戳</param>
        /// <returns></returns>
        //public bool Delete(string actionID, string pointId, string pollutantCode, DateTime tstamp, string SampleNumber)
        //{
        //    return g_StandardSolutionCheckRepository.Delete(actionID, pointId, pollutantCode, tstamp, SampleNumber);
        //}
        public void Delete(int id)
        {
            StandardSolutionCheckEntity model;
            model = g_StandardSolutionCheckRepository.RetrieveFirstOrDefault(p => p.Id == id);
            g_StandardSolutionCheckRepository.Delete(model);
        }
        /// <summary>
        /// 根据报id获取质控任务记录
        /// </summary>
        /// <param name="id">工作ID</param>
        /// <returns></returns>
        public StandardSolutionCheckEntity RetrieveEntityByUid(int id)
        {
            return g_StandardSolutionCheckRepository.Retrieve(p => p.Id == id).FirstOrDefault();
        }



        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public DataTable GetList(int id)
        {
            DataTable dt = new DataTable();
            string strWhere = "id=" + id;
            dt = g_StandardSolutionCheckRepository.GetList(strWhere);
            dt.Columns.Add("MissionName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string missionId = dt.Rows[i]["MissionID"].ToString();
                string stWhere = missionId;
                dt.Rows[i]["MissionName"] = g_TaskConfigService.GetName(stWhere)[0];
            }
            return dt;
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public DataTable GetListNew(int id)
        {
            DataTable dt = new DataTable();
            string strWhere = "ss.id=" + id;
            dt = g_StandardSolutionCheckRepository.GetListNew(strWhere);
            dt.Columns.Add("MissionName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string missionId = dt.Rows[i]["MissionID"].ToString();
                string stWhere = missionId;
                dt.Rows[i]["MissionName"] = g_TaskConfigService.GetName(stWhere)[0];

                string PollutantName = dt.Rows[i]["PollutantName"].ToString();
                switch (PollutantName)
                {
                    case "pH值":
                        {
                            if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.1)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                    case "溶解氧":
                        {
                            if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.5)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                    case "氨氮":
                        {
                            if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.02)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                    case "总磷":
                        {
                            if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.02)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                    default:
                        {
                            if (dt.Rows[i]["RelativeOffset"].ToString() != "" && !dt.Rows[i]["RelativeOffset"].ToString().Contains("&nbsp"))
                            {
                                if (double.Parse(dt.Rows[i]["RelativeOffset"].ToString()) <= 10)
                                {
                                    dt.Rows[i]["Evaluate"] = "合格";
                                }
                                else
                                {
                                    dt.Rows[i]["Evaluate"] = "不合格";
                                }
                            }
                            break;
                        }
                }
            }
            return dt;
        }

        /// <summary>
        /// 根据因子code查看比对限值
        /// </summary>
        /// <param name="PollutantCode">因子code</param>
        /// <returns></returns>
        public string GetCompareLimitValue(string pollutantCode)
        {
            return g_StandardSolutionCheckRepository.GetCompareLimitValue(pollutantCode);
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
        /// <summary>
        /// 获取标液
        /// </summary>
        /// <param name="InsID">仪器ID</param>
        /// <returns></returns>
        public DataTable GetReagent(string InsID)
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl, "TempGetDataWebService", "GetReagentInfoByInsID", new object[] { InsID });
            return objData as DataTable;
            //return sr.GetReagentInfoByInsID(InsID);
        }
        /// <summary>
        /// 通过站点ID获取站点内的所有因子
        /// </summary>
        /// <param name="pointId"></param>
        /// <returns></returns>
        public List<PollutantCodeEntity> GetFactor(string pointId)
        {
            List<PollutantCodeEntity> l_PollutantCodeEntity = s_InstrumentChannel.RetrieveChannelListByPointUid(pointId).ToList<PollutantCodeEntity>();
            return l_PollutantCodeEntity;
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns>获取符合条件的数据</returns>
         public DataTable GetList(string taskCode, string ActionID = null, DateTime? dtimeStart = null, DateTime? dtimeEnd = null)
        {
            //string strWhere = "TaskCode='" + taskCode + "' and MissionID='" + missionId + "' and  PointId='" + pointId + "' and Tstamp>='" + datetime + "' and Tstamp<='" + endtime + "'";
            //if (!string.IsNullOrWhiteSpace(aciontId))
            //{
            //    strWhere += " and  ActionID='" + aciontId + "'";
            //}
          DataTable dt = new DataTable();
         
            string strWhere = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(taskCode))
            {
                strWhere += string.Format(" and ss.TaskCode ='{0}' ", taskCode);
            }
            if (!string.IsNullOrWhiteSpace(ActionID))
            {
                strWhere += string.Format(" and ss.ActionID ='{0}' ", ActionID);
            }
            if (dtimeStart != null)
            {
                strWhere += string.Format(" and ss.Tstamp <='{0}' ", dtimeStart);
            }
            if (dtimeEnd != null)
            {
                strWhere += string.Format(" and ss.Tstamp >='{0}' ", dtimeEnd);
            }
            if (strWhere == " 1=1 ")
            {
                strWhere += " and ss.TaskCode='' ";
            }
            dt = g_StandardSolutionCheckRepository.GetList(strWhere);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
              string PollutantName = dt.Rows[i]["PollutantName"].ToString();
              switch (PollutantName)
              {
                case "pH值":
                  {
                    if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                    {
                      if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.1)
                      {
                        dt.Rows[i]["Evaluate"] = "合格";
                      }
                      else
                      {
                        dt.Rows[i]["Evaluate"] = "不合格";
                      }
                    }
                    break;
                  }
                case "溶解氧":
                  {
                    if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                    {
                      if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.5)
                      {
                        dt.Rows[i]["Evaluate"] = "合格";
                      }
                      else
                      {
                        dt.Rows[i]["Evaluate"] = "不合格";
                      }
                    }
                    break;
                  }
                case "氨氮":
                  {
                    if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                    {
                      if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.02)
                      {
                        dt.Rows[i]["Evaluate"] = "合格";
                      }
                      else
                      {
                        dt.Rows[i]["Evaluate"] = "不合格";
                      }
                    }
                    break;
                  }
                case "总磷":
                  {
                    if (dt.Rows[i]["AbsoluteOffset"].ToString() != "" && !dt.Rows[i]["AbsoluteOffset"].ToString().Contains("&nbsp"))
                    {
                      if (double.Parse(dt.Rows[i]["AbsoluteOffset"].ToString()) <= 0.02)
                      {
                        dt.Rows[i]["Evaluate"] = "合格";
                      }
                      else
                      {
                        dt.Rows[i]["Evaluate"] = "不合格";
                      }
                    }
                    break;
                  }
                default:
                  {
                    if (dt.Rows[i]["RelativeOffset"].ToString() != "" && !dt.Rows[i]["RelativeOffset"].ToString().Contains("&nbsp"))
                    {
                      if (double.Parse(dt.Rows[i]["RelativeOffset"].ToString()) <= 10)
                      {
                        dt.Rows[i]["Evaluate"] = "合格";
                      }
                      else
                      {
                        dt.Rows[i]["Evaluate"] = "不合格";
                      }
                    }
                    break;
                  }
              }
            }
            return dt;
        }

        /// <summary>
        /// 获取所有的标液数据
        /// </summary>
        /// <param name="pointId">站点Guid</param>
        /// <returns></returns>
        public DataTable GetReagents(string rowGuid)
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl, "TempGetDataWebService", "GetReagentByRowGuid", new object[] { rowGuid });
            return objData as DataTable;
        }
        #endregion
    }
}
