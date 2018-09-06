using SmartEP.AMSRepository.Interfaces;
using SmartEP.AMSRepository.Water;
using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.AutoMonitoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using SmartEP.Service.Frame;
using SmartEP.DomainModel;
using SmartEP.Service.Core.Enums;
using SmartEP.BaseInfoRepository.Channel;

namespace SmartEP.Service.AutoMonitoring.Water
{
    /// <summary>
    /// 名称：WaterDataSamplingRateService.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：地表水数据捕获率查询类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterDataSamplingRateService : IDataSamplingRate
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();

        /// <summary>
        /// 实时数据仓储接口
        /// </summary>
        IInfectantRepository g_IInfectantRepository = null;

        /// <summary>
        /// 监测点：【水】测点扩展信息类
        /// </summary>
        MonitoringPointExtensionForEQMSWaterRepository g_ExtensionForEQMSWater = null;

        #region 实现接口
        /// <summary>
        /// 点位某一小时总捕获率
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmHour">日期（小时）</param>
        /// <returns></returns>
        public decimal GetHourSamplingRate(int portId, DateTime dtmHour)
        {
            DateTime dtmStart = dtmHour;//开始时间
            DateTime dtmEnd = dtmHour;//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            return GetPointSamplingRate(pointIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 点位某天总捕获率
        /// </summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmDay">日期（天）</param>
        /// <returns></returns>
        public decimal GetDaySamplingRate(int portId, DateTime dtmDay)
        {
            DateTime dtmStart = dtmDay;//开始时间
            DateTime dtmEnd = dtmDay;//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            return GetPointSamplingRate(pointIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 点位某月总捕获率
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="monthOfYear">月</param>
        /// <returns></returns>
        public decimal GetMonthSamplingRate(int portId, int year, int monthOfYear)
        {
            if (monthOfYear < 1 || monthOfYear > 12)
            {
                return 0;
            }

            DateTime dtmStart = new DateTime(year, monthOfYear, 1);//开始时间
            DateTime dtmEnd = new DateTime(year, monthOfYear + 1, 1).AddDays(-1);//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            return GetPointSamplingRate(pointIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 点位某季度总捕获率
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="seasonOfYear">季</param>
        /// <returns></returns>
        public decimal GetSeasonSamplingRate(int portId, int year, int seasonOfYear)
        {
            if (seasonOfYear < 1 || seasonOfYear > 4)
            {
                return 0;
            }

            int monthStart = seasonOfYear * 3 - 2;//开始月
            int monthEnd = seasonOfYear * 3;//结束月
            DateTime dtmStart = new DateTime(year, monthStart, 1);//开始时间
            DateTime dtmEnd = (monthEnd == 12) ? new DateTime(year + 1, 1, 1).AddDays(-1) : new DateTime(year, monthEnd + 1, 1).AddDays(-1);//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            return GetPointSamplingRate(pointIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 点位某年总捕获率
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <returns></returns>
        public decimal GetYearSamplingRate(int portId, int year)
        {
            DateTime dtmStart = new DateTime(year, 1, 1);//开始时间
            DateTime dtmEnd = new DateTime(year + 1, 1, 1).AddDays(-1);//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            return GetPointSamplingRate(pointIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 点位某一小时各个污染物捕获率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmHour">日期（小时）</param>
        /// <returns></returns>
        public DataView GetHourSamplingRateDetail(int portId, DateTime dtmHour)
        {
            DateTime dtmStart = dtmHour;//开始时间
            DateTime dtmEnd = dtmHour;//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            int recordTotal = 0;
            return GetPointSamplingRateDetail(pointIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal);
        }

        /// <summary>
        /// 点位某天各个污染物捕获率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="dtmDay">日期（天）</param>
        /// <returns></returns>
        public DataView GetDaySamplingRateDetail(int portId, DateTime dtmDay)
        {
            DateTime dtmStart = dtmDay;//开始时间
            DateTime dtmEnd = dtmDay;//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            int recordTotal = 0;
            return GetPointSamplingRateDetail(pointIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal);
        }

        /// <summary>
        /// 点位某月各个污染物捕获率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="monthOfYear">月</param>
        /// <returns></returns>
        public DataView GetMonthSamplingRateDetail(int portId, int year, int monthOfYear)
        {
            if (monthOfYear < 1 || monthOfYear > 12)
            {
                return null;
            }

            DateTime dtmStart = new DateTime(year, monthOfYear, 1);//开始时间
            DateTime dtmEnd = new DateTime(year, monthOfYear + 1, 1).AddDays(-1);//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            int recordTotal = 0;
            return GetPointSamplingRateDetail(pointIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal);
        }

        /// <summary>
        /// 点位某季度各个污染物捕获率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <param name="seasonOfYear">季</param>
        /// <returns></returns>
        public DataView GetSeasonSamplingRateDetail(int portId, int year, int seasonOfYear)
        {
            if (seasonOfYear < 1 || seasonOfYear > 4)
            {
                return null;
            }

            int monthStart = seasonOfYear * 3 - 2;//开始月
            int monthEnd = seasonOfYear * 3;//结束月
            DateTime dtmStart = new DateTime(year, monthStart, 1);//开始时间
            DateTime dtmEnd = (monthEnd == 12) ? new DateTime(year + 1, 1, 1).AddDays(-1) : new DateTime(year, monthEnd + 1, 1).AddDays(-1);//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            int recordTotal = 0;
            return GetPointSamplingRateDetail(pointIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal);
        }

        /// <summary>
        /// 点位某年各个污染物捕获率明细
        /// <summary>
        /// <param name="portId">测点Id</param>
        /// <param name="year">年</param>
        /// <returns></returns>
        public DataView GetYearSamplingRateDetail(int portId, int year)
        {
            DateTime dtmStart = new DateTime(year, 1, 1);//开始时间
            DateTime dtmEnd = new DateTime(year + 1, 1, 1).AddDays(-1);//结束时间
            string[] pointIds = { portId.ToString() };
            string[] factors = GetPollutantCodesByPointIds(pointIds);//根据测点Id数组获取因子列
            int recordTotal = 0;
            return GetPointSamplingRateDetail(pointIds, factors, dtmStart, dtmEnd, int.MaxValue, 0, out recordTotal);
        }
        #endregion

        #region 总捕获率
        /// <summary>
        /// 获取某一时间段监测点数据总捕获率（因子不可选，测点自动关联因子）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public decimal GetPointSamplingRate(string[] portIds, DateTime dtmStart, DateTime dtmEnd)
        {
            string[] factors = GetPollutantCodesByPointIds(portIds);//根据测点Id数组获取因子列
            return GetPointSamplingRate(portIds, factors, dtmStart, dtmEnd);
        }

        /// <summary>
        /// 获取某一时间段监测点数据总捕获率（因子可选）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public decimal GetPointSamplingRate(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd)
        {
            decimal shouldDays = (decimal)(dtmEnd - dtmStart).TotalDays + 1;//每个因子应测天数
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            DataTable dtNew = new DataTable();//新建表
            int allShouldCount = 0;//总应测条数
            int allRealCount = 0;//总实测条数
            decimal allSampRate = 0;//总捕获率
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            InfectantBy60Service infectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();

            if (infectantBy60Service != null)
            {
                Dictionary<string, int> pointDataCycles = new Dictionary<string, int>();//测点、每天应测条数字典
                DataTable dtSampRate = infectantBy60Service.GetPollutantValueCount(portIds, factors, dtmStart, dtmEnd).Table;

                for (int i = 0; i < portIds.Length; i++)
                {
                    string pointId = portIds[i];
                    IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                      ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                    int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == pointId)
                                                              .Select(p => p.DataCycle).FirstOrDefault();//每天应测条数
                    if (dataCycle != null && dataCycle != 0)
                    {
                        int shouldCount = (int)Math.Floor(dataCycle.Value * shouldDays);//应测条数=一天几条*天数
                        int rowShouldCount = shouldCount * pollutantCodeList.Count(t => factors.Contains(t));
                        allShouldCount += rowShouldCount;
                    }
                }

                //循环记录每个测点的各因子的条数
                for (int i = 0; i < dtSampRate.Rows.Count; i++)
                {
                    DataRow drSampRate = dtSampRate.Rows[i];
                    IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(drSampRate["PointId"].ToString()))
                                      ? pointPollutantCodeList[drSampRate["PointId"].ToString()] : new List<string>(); //该测点对应的因子Code列
                    if (pollutantCodeList.Contains(drSampRate["PollutantCode"].ToString()))
                    {
                        int realCount = int.TryParse(dtSampRate.Rows[i]["Count"].ToString(), out realCount) ? realCount : 0;//实测条数
                        allRealCount += realCount;
                    }
                }
                if (allShouldCount > 0)
                {
                    allRealCount = (allRealCount > allShouldCount) ? allShouldCount : allRealCount;
                    allSampRate = Math.Round((decimal)allRealCount * 100 / allShouldCount, 2);
                }
            }
            return allSampRate;
        }
        #endregion

        #region 捕获率明细
        /// <summary>
        /// 获取某一时间段监测点各个污染物捕获率明细（因子不可选，测点自动关联因子）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 同GetPointSamplingRateDetail
        /// </returns>
        public DataView GetPointSamplingRateDetail(string[] portIds, DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                string[] factors = GetPollutantCodesByPointIds(portIds);//根据测点Id数组获取因子列
                return GetPointSamplingRateDetail(portIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取某一时间段监测点各个污染物捕获率明细（因子可选）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// 因子代码（多列，不是固定值）：如a21005
        /// TotalRealCount：合计实测条数
        /// TotalShouldCount：合计应测条数
        /// TotalValue：合计捕获率值
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetPointSamplingRateDetail(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                return GetSamplingRateDataPager(portIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取最近24小时数据捕获率明细
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// PointName：测点名称
        /// IntegratorName：集成商名称
        /// PollutantCode：因子Code
        /// PollutantName：因子名称
        /// Days：天数
        /// ShouldCount：应测数据条数
        /// RealCount：实测数据条数
        /// SamplingRate：捕获率
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetSamplingRateDetailBy24Hours(string[] portIds, int pageSize, int pageNo,
             out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            DateTime dtmEnd = DateTime.Now;
            DateTime dtmStart = dtmEnd.AddDays(-1);
            decimal shouldDays = (decimal)(dtmEnd - dtmStart).TotalDays;//天数
            int totalRealCount = 0;//合计实测条数
            int totalShouldCount = 0;//合计应测条数
            string[] factors;
            DataTable dtNew = new DataTable();//新建表
            DataTable dtPointData = new DataTable();//测点和因子的数据表 
            InfectantBy60Service infectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            Dictionary<string, string> pointInstrumentList = GetInstrumentNamesByPointIds(portIds);//根据测点Id数组获取测点Id和集成商名称的对应关系列 
            dtNew.Columns.Add("PointId", typeof(int));//测点Id
            dtNew.Columns.Add("PointName", typeof(string));//测点名称
            dtNew.Columns.Add("IntegratorName", typeof(string));//集成商名称
            dtNew.Columns.Add("Days", typeof(decimal));//天数
            dtNew.Columns.Add("ShouldCount", typeof(int));//应测数据条数
            dtNew.Columns.Add("RealCount", typeof(int));//实测数据条数
            dtNew.Columns.Add("SamplingRate", typeof(string));//捕获率    
            dtNew.Columns.Add("blankspaceColumn", typeof(string));//空白列       
            recordTotal = 0;
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();

            if (infectantBy60Service != null)
            {
                factors = GetPollutantCodesByPointIds(portIds);//根据测点Id数组获取因子列
                dtPointData = infectantBy60Service.GetPollutantValueCount(portIds, factors, dtmStart, dtmEnd).Table;
                portIds = portIds.Distinct().ToArray();
                for (int i = 0; i < portIds.Length; i++)
                {
                    int pointId = int.TryParse(portIds[i], out pointId) ? pointId : 0;
                    int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == pointId.ToString())
                                       .Select(p => p.DataCycle).FirstOrDefault();//每天应测条数
                    if (dataCycle != null && dataCycle != 0)
                    {
                        int shouldCount = (int)Math.Floor(dataCycle.Value * shouldDays);//应测条数=一天几条*天数
                        DataRow[] drsPointData = dtPointData.Select(string.Format("PointId='{0}'", pointId));
                        if (drsPointData.Length > 0)
                        {
                            //根据测点Id获取测点名称
                            MonitoringPointEntity entity = g_MonitoringPointWater.RetrieveEntityByPointId(pointId);
                            string pointName = (entity != null) ? entity.MonitoringPointName : string.Empty;
                            IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId.ToString()))
                                  ? pointPollutantCodeList[pointId.ToString()] : new List<string>();//该测点对应的因子Code列
                            int rowRealCount = 0;//该测点的实测条数
                            int rowShouldCount = shouldCount * pollutantCodeList.Count(t => factors.Contains(t));//该测点的应测条数
                            totalShouldCount += rowShouldCount;

                            if (rowShouldCount > 0)
                            {
                                DataRow drNew = dtNew.NewRow();

                                foreach (DataRow drPointData in drsPointData)
                                {
                                    int realCount = int.TryParse(drPointData["Count"].ToString(), out realCount) ? realCount : 0;//实测条数
                                    realCount = (realCount > shouldCount) ? shouldCount : realCount;
                                    rowRealCount += realCount;
                                    totalRealCount += realCount;
                                }
                                drNew["PointId"] = pointId;
                                drNew["PointName"] = pointName;
                                drNew["IntegratorName"] = pointInstrumentList[pointId.ToString()];
                                drNew["Days"] = shouldDays;
                                drNew["ShouldCount"] = rowShouldCount;
                                drNew["RealCount"] = rowRealCount;
                                rowRealCount = (rowRealCount > rowShouldCount) ? rowShouldCount : rowRealCount;
                                drNew["SamplingRate"] = Math.Round((decimal)rowRealCount * 100 / rowShouldCount, 2).ToString() + "%";
                                dtNew.Rows.Add(drNew);
                            }
                        }
                    }
                }
                AddDataRowsByPointInstruments(portIds, pointInstrumentList, pointPollutantCodeList, shouldDays, dtNew);//根据测点Id数组和集成商列表增加空白数据行
                recordTotal = dtNew.Rows.Count;
                dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, "PointId,IntegratorName");
            }
            return (dtNew != null ? dtNew.AsDataView() : null);
        }

        /// <summary>
        /// 获取最近N小时数据捕获率明细
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="hours">小时数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// PointName：测点名称
        /// IntegratorName：集成商名称
        /// PollutantCode：因子Code
        /// PollutantName：因子名称
        /// Days：天数
        /// ShouldCount：应测数据条数
        /// RealCount：实测数据条数
        /// SamplingRate：捕获率
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetSamplingRateDetailByHours(string[] portIds, int hours, int pageSize, int pageNo,
             out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            DateTime dtmEnd = DateTime.Now;
            DateTime dtmStart = dtmEnd.AddDays(-hours);
            decimal shouldDays = (decimal)(dtmEnd - dtmStart).TotalDays;//天数
            int totalRealCount = 0;//合计实测条数
            int totalShouldCount = 0;//合计应测条数
            string[] factors;
            DataTable dtNew = new DataTable();//新建表
            DataTable dtPointData = new DataTable();//测点和因子的数据表 
            InfectantBy60Service infectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            Dictionary<string, string> pointInstrumentList = GetInstrumentNamesByPointIds(portIds);//根据测点Id数组获取测点Id和集成商名称的对应关系列 
            dtNew.Columns.Add("PointId", typeof(int));//测点Id
            dtNew.Columns.Add("PointName", typeof(string));//测点名称
            dtNew.Columns.Add("IntegratorName", typeof(string));//集成商名称
            dtNew.Columns.Add("Days", typeof(decimal));//天数
            dtNew.Columns.Add("ShouldCount", typeof(int));//应测数据条数
            dtNew.Columns.Add("RealCount", typeof(int));//实测数据条数
            dtNew.Columns.Add("SamplingRate", typeof(string));//捕获率    
            dtNew.Columns.Add("blankspaceColumn", typeof(string));//空白列       
            recordTotal = 0;
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();

            if (infectantBy60Service != null)
            {
                factors = GetPollutantCodesByPointIds(portIds);//根据测点Id数组获取因子列
                dtPointData = infectantBy60Service.GetPollutantValueCount(portIds, factors, dtmStart, dtmEnd).Table;
                portIds = portIds.Distinct().ToArray();
                for (int i = 0; i < portIds.Length; i++)
                {
                    int pointId = int.TryParse(portIds[i], out pointId) ? pointId : 0;
                    int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == pointId.ToString())
                                       .Select(p => p.DataCycle).FirstOrDefault();//每天应测条数
                    if (dataCycle != null && dataCycle != 0)
                    {
                        int shouldCount = (int)Math.Floor(dataCycle.Value * shouldDays);//应测条数=一天几条*天数
                        DataRow[] drsPointData = dtPointData.Select(string.Format("PointId='{0}'", pointId));
                        if (drsPointData.Length > 0)
                        {
                            //根据测点Id获取测点名称
                            MonitoringPointEntity entity = g_MonitoringPointWater.RetrieveEntityByPointId(pointId);
                            string pointName = (entity != null) ? entity.MonitoringPointName : string.Empty;
                            IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId.ToString()))
                                  ? pointPollutantCodeList[pointId.ToString()] : new List<string>();//该测点对应的因子Code列
                            int rowRealCount = 0;//该测点的实测条数
                            int rowShouldCount = shouldCount * pollutantCodeList.Count(t => factors.Contains(t));//该测点的应测条数
                            totalShouldCount += rowShouldCount;

                            if (rowShouldCount > 0)
                            {
                                DataRow drNew = dtNew.NewRow();

                                foreach (DataRow drPointData in drsPointData)
                                {
                                    int realCount = int.TryParse(drPointData["Count"].ToString(), out realCount) ? realCount : 0;//实测条数
                                    realCount = (realCount > shouldCount) ? shouldCount : realCount;
                                    rowRealCount += realCount;
                                    totalRealCount += realCount;
                                }
                                drNew["PointId"] = pointId;
                                drNew["PointName"] = pointName;
                                drNew["IntegratorName"] = pointInstrumentList[pointId.ToString()];
                                drNew["Days"] = shouldDays;
                                drNew["ShouldCount"] = rowShouldCount;
                                drNew["RealCount"] = rowRealCount;
                                rowRealCount = (rowRealCount > rowShouldCount) ? rowShouldCount : rowRealCount;
                                drNew["SamplingRate"] = Math.Round((decimal)rowRealCount * 100 / rowShouldCount, 2).ToString() + "%";
                                dtNew.Rows.Add(drNew);
                            }
                        }
                    }
                }
                AddDataRowsByPointInstruments(portIds, pointInstrumentList, pointPollutantCodeList, shouldDays, dtNew);//根据测点Id数组和集成商列表增加空白数据行
                recordTotal = dtNew.Rows.Count;
                dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, "PointId,IntegratorName");
            }
            return (dtNew != null ? dtNew.AsDataView() : null);
        }
        #endregion

        #region 获取数据捕获率详情数据
        /// <summary>
        /// 获取测点的捕获率详情数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// PointName：测点名称
        /// IntegratorName：集成商名称
        /// PollutantCode：因子Code
        /// PollutantName：因子名称
        /// Days：天数
        /// ShouldCount：应测数据条数
        /// RealCount：实测数据条数
        /// SamplingRate：捕获率
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetSamplingRateDetailData(string[] portIds, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo,
            out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            decimal shouldDays = (decimal)(dtmEnd - dtmStart).TotalDays + 1;//天数
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            int totalRealCount = 0;//合计实测条数
            int totalShouldCount = 0;//合计应测条数
            int allRealCount = 0;//总实测条数
            int allShouldCount = 0;//总应测条数
            DataTable dtNew = new DataTable();//新建表
            DataTable dtPointData = new DataTable();//测点和因子的数据表 
            DataTable dtStatistical;//统计行
            InfectantBy60Service infectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();
            IList<string> pointListDistinct = new List<string>();
            IList<PollutantCodeEntity> pollutantEntityList = GetPollutantDataByPointIds(portIds);//根据测点Id数组获取因子数据列
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            Dictionary<string, string> pointInstrumentList = GetInstrumentNamesByPointIds(portIds);//根据测点Id数组获取测点Id和集成商名称的对应关系列 
            dtNew.Columns.Add("PointId", typeof(int));//测点Id
            dtNew.Columns.Add("PointName", typeof(string));//测点名称
            dtNew.Columns.Add("IntegratorName", typeof(string));//集成商名称
            dtNew.Columns.Add("PollutantCode", typeof(string));//因子Code
            dtNew.Columns.Add("PollutantName", typeof(string));//因子名称
            dtNew.Columns.Add("Days", typeof(int));//天数
            dtNew.Columns.Add("ShouldCount", typeof(int));//应测数据条数
            dtNew.Columns.Add("RealCount", typeof(int));//实测数据条数
            dtNew.Columns.Add("SamplingRate", typeof(string));//捕获率
            dtNew.Columns.Add("blankspaceColumn", typeof(string));//空白列
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
            dtStatistical = dtNew.Clone();
            DataRow drStatistical = dtStatistical.NewRow();//统计行信息
            drStatistical["PointName"] = "合计";
            drStatistical["Days"] = shouldDays;
            dtStatistical.Rows.Add(drStatistical);

            if (infectantBy60Service != null)
            {
                dtPointData = infectantBy60Service.GetPollutantValueCount(portIds, pollutantEntityList.Select(t => t.PollutantCode).ToArray(), dtmStart, dtmEnd).Table;
                portIds = portIds.Distinct().ToArray();
                for (int i = 0; i < portIds.Length; i++)
                {
                    int pointId = int.TryParse(portIds[i], out pointId) ? pointId : 0;
                    int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == pointId.ToString())
                                        .Select(p => p.DataCycle).FirstOrDefault();//每天应测条数
                    if (dataCycle != null && dataCycle != 0)
                    {
                        int shouldCount = (int)Math.Floor(dataCycle.Value * shouldDays);//应测条数=一天几条*天数
                        DataRow[] drsPointData = dtPointData.Select(string.Format("PointId='{0}'", pointId));
                        if (drsPointData.Length > 0)
                        {
                            //根据测点Id获取测点名称
                            MonitoringPointEntity entity = g_MonitoringPointWater.RetrieveEntityByPointId(pointId);
                            string pointName = (entity != null) ? entity.MonitoringPointName : string.Empty;
                            IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId.ToString()))
                                       ? pointPollutantCodeList[pointId.ToString()] : new List<string>();//该测点对应的因子Code列

                            foreach (DataRow drPointData in drsPointData)
                            {
                                int realCount = int.TryParse(drPointData["Count"].ToString(), out realCount) ? realCount : 0;//实测条数
                                realCount = (realCount > shouldCount) ? shouldCount : realCount;
                                DataRow drNew = dtNew.NewRow();
                                drNew["PointId"] = pointId;
                                drNew["PointName"] = pointName;
                                drNew["IntegratorName"] = pointInstrumentList[pointId.ToString()];
                                drNew["PollutantCode"] = drPointData["PollutantCode"];
                                PollutantCodeEntity pollutantEntity = pollutantEntityList.FirstOrDefault(t => t.PollutantCode == drPointData["PollutantCode"].ToString());
                                drNew["PollutantName"] = (pollutantEntity != null) ? pollutantEntity.PollutantName : string.Empty;

                                //如果测点中有该因子则记录捕获率信息
                                if (pollutantCodeList.Contains(drPointData["PollutantCode"].ToString()))
                                {
                                    drNew["Days"] = shouldDays;
                                    drNew["ShouldCount"] = shouldCount;
                                    drNew["RealCount"] = realCount;
                                    drNew["SamplingRate"] = Math.Round((decimal)realCount * 100 / shouldCount, 2).ToString() + "%";
                                    totalRealCount += realCount;
                                    totalShouldCount += shouldCount;
                                }
                                dtNew.Rows.Add(drNew);
                            }
                        }
                    }
                }
            }
            AddDataRowsByPointPollutants(portIds, pollutantEntityList.Select(t => t.PollutantCode).ToArray(), pollutantEntityList,
                pointInstrumentList, pointPollutantCodeList, shouldDays, dtNew);//根据测点Id数组和因子Code数组增加空白数据行
            recordTotal = dtNew.Rows.Count;
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowRealCount = int.TryParse(drNew["RealCount"].ToString(), out rowRealCount) ? rowRealCount : 0;
                int rowShouldCount = int.TryParse(drNew["ShouldCount"].ToString(), out rowShouldCount) ? rowShouldCount : 0;
                allRealCount += rowRealCount;//总实测条数
                allShouldCount += rowShouldCount;//总应测条数
            }
            if (allShouldCount > 0)
            {
                decimal allSamplingRate = Math.Round((decimal)allRealCount * 100 / allShouldCount, 2);//有效运行率=有效总数/应测总数
                drStatistical["RealCount"] = allRealCount;
                drStatistical["ShouldCount"] = allShouldCount;
                drStatistical["SamplingRate"] = allSamplingRate.ToString() + "%";
            }
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, "PointId,IntegratorName,PollutantCode");
            return (dtNew != null ? dtNew.AsDataView() : null);
        }

        /// <summary>
        /// 获取测点的捕获率详情数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// PointName：测点名称
        /// IntegratorName：集成商名称
        /// PollutantCode：因子Code
        /// PollutantName：因子名称
        /// Days：天数
        /// ShouldCount：应测数据条数
        /// RealCount：实测数据条数
        /// SamplingRate：捕获率
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetSamplingRateDetailData(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo,
            out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            decimal shouldDays = (decimal)(dtmEnd - dtmStart).TotalDays + 1;//天数
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            int totalRealCount = 0;//合计实测条数
            int totalShouldCount = 0;//合计应测条数
            int allRealCount = 0;//总实测条数
            int allShouldCount = 0;//总应测条数
            DataTable dtNew = new DataTable();//新建表
            DataTable dtPointData = new DataTable();//测点和因子的数据表 
            DataTable dtStatistical;//统计行
            InfectantBy60Service infectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();
            IList<string> pointListDistinct = new List<string>();
            IList<PollutantCodeEntity> pollutantEntityList = GetPollutantDataByPollutantCodes(factors);//根据因子Code数组获取因子数据列
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            Dictionary<string, string> pointInstrumentList = GetInstrumentNamesByPointIds(portIds);//根据测点Id数组获取测点Id和集成商名称的对应关系列 
            dtNew.Columns.Add("PointId", typeof(int));//测点Id
            dtNew.Columns.Add("PointName", typeof(string));//测点名称
            dtNew.Columns.Add("IntegratorName", typeof(string));//集成商名称
            dtNew.Columns.Add("PollutantCode", typeof(string));//因子Code
            dtNew.Columns.Add("PollutantName", typeof(string));//因子名称
            dtNew.Columns.Add("Days", typeof(int));//天数
            dtNew.Columns.Add("ShouldCount", typeof(int));//应测数据条数
            dtNew.Columns.Add("RealCount", typeof(int));//实测数据条数
            dtNew.Columns.Add("SamplingRate", typeof(string));//捕获率
            dtNew.Columns.Add("blankspaceColumn", typeof(string));//空白列
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
            dtStatistical = dtNew.Clone();
            DataRow drStatistical = dtStatistical.NewRow();//统计行信息
            drStatistical["PointName"] = "合计";
            drStatistical["Days"] = shouldDays;
            dtStatistical.Rows.Add(drStatistical);

            if (infectantBy60Service != null)
            {
                dtPointData = infectantBy60Service.GetPollutantValueCount(portIds, factors, dtmStart, dtmEnd).Table;
                portIds = portIds.Distinct().ToArray();
                for (int i = 0; i < portIds.Length; i++)
                {
                    int pointId = int.TryParse(portIds[i], out pointId) ? pointId : 0;
                    int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == pointId.ToString())
                                        .Select(p => p.DataCycle).FirstOrDefault();//每天应测条数
                    if (dataCycle != null && dataCycle != 0)
                    {
                        int shouldCount = (int)Math.Floor(dataCycle.Value * shouldDays);//应测条数=一天几条*天数
                        DataRow[] drsPointData = dtPointData.Select(string.Format("PointId='{0}'", pointId));
                        if (drsPointData.Length > 0)
                        {
                            //根据测点Id获取测点名称
                            MonitoringPointEntity entity = g_MonitoringPointWater.RetrieveEntityByPointId(pointId);
                            string pointName = (entity != null) ? entity.MonitoringPointName : string.Empty;
                            IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId.ToString()))
                                       ? pointPollutantCodeList[pointId.ToString()] : new List<string>();//该测点对应的因子Code列

                            foreach (DataRow drPointData in drsPointData)
                            {
                                int realCount = int.TryParse(drPointData["Count"].ToString(), out realCount) ? realCount : 0;//实测条数
                                realCount = (realCount > shouldCount) ? shouldCount : realCount;
                                DataRow drNew = dtNew.NewRow();
                                drNew["PointId"] = pointId;
                                drNew["PointName"] = pointName;
                                drNew["IntegratorName"] = pointInstrumentList[pointId.ToString()];
                                drNew["PollutantCode"] = drPointData["PollutantCode"];
                                PollutantCodeEntity pollutantEntity = pollutantEntityList.FirstOrDefault(t => t.PollutantCode == drPointData["PollutantCode"].ToString());
                                drNew["PollutantName"] = (pollutantEntity != null) ? pollutantEntity.PollutantName : string.Empty;

                                //如果测点中有该因子则记录捕获率信息
                                if (pollutantCodeList.Contains(drPointData["PollutantCode"].ToString()))
                                {
                                    drNew["Days"] = shouldDays;
                                    drNew["ShouldCount"] = shouldCount;
                                    drNew["RealCount"] = realCount;
                                    drNew["SamplingRate"] = Math.Round((decimal)realCount * 100 / shouldCount, 2).ToString() + "%";
                                    totalRealCount += realCount;
                                    totalShouldCount += shouldCount;
                                }
                                dtNew.Rows.Add(drNew);
                            }
                        }
                    }
                }
            }
            AddDataRowsByPointPollutants(portIds, factors, pollutantEntityList, pointInstrumentList,
                pointPollutantCodeList, shouldDays, dtNew);//根据测点Id数组和因子Code数组增加空白数据行
            recordTotal = dtNew.Rows.Count;
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowRealCount = int.TryParse(drNew["RealCount"].ToString(), out rowRealCount) ? rowRealCount : 0;
                int rowShouldCount = int.TryParse(drNew["ShouldCount"].ToString(), out rowShouldCount) ? rowShouldCount : 0;
                allRealCount += rowRealCount;//总实测条数
                allShouldCount += rowShouldCount;//总应测条数
            }
            if (allShouldCount > 0)
            {
                decimal allSamplingRate = Math.Round((decimal)allRealCount * 100 / allShouldCount, 2);//有效运行率=有效总数/应测总数
                drStatistical["RealCount"] = allRealCount;
                drStatistical["ShouldCount"] = allShouldCount;
                drStatistical["SamplingRate"] = allSamplingRate.ToString() + "%";
            }
            dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, "PointId,IntegratorName,PollutantCode");
            return (dtNew != null ? dtNew.AsDataView() : null);
        }
        #endregion

        #region 获取数据基础方法
        /// <summary>
        /// 取得虚拟分页查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// 因子代码（多列，不是固定值）：如a21005
        /// TotalRealCount：合计实测条数
        /// TotalShouldCount：合计应测条数
        /// TotalValue：合计捕获率值
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetSamplingRateDataPager(string[] portIds, string[] factors
            , DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId")
        {
            decimal shouldDays = (decimal)(dtmEnd - dtmStart).TotalDays + 1;//每个因子应测天数
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            DataTable dtNew = CreateNewDataTableByPollutant(factors);//新建表
            DataTable dtReturn;//要返回的视图的表
            int startIndex = pageSize * pageNo;//要返回的数据的开始位置(从0开始)
            int endIndex = pageSize * (pageNo + 1);//要返回的数据的结束位置(数据取到小于这个值)
            recordTotal = 0;
            InfectantBy60Service infectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();

            if (infectantBy60Service != null)
            {
                DataTable dtSampRate = infectantBy60Service.GetPollutantValueCount(portIds, factors, dtmStart, dtmEnd).Table;
                for (int i = 0; i < dtSampRate.Rows.Count; i++)
                {
                    DataRow drSampRate = dtSampRate.Rows[i];
                    string pointId = drSampRate["PointId"].ToString();
                    string pollutantCode = drSampRate["PollutantCode"].ToString();
                    DataRow[] drsNew = dtNew.Select(string.Format("PointId='{0}'", pointId));//有该测点的记录行
                    int realCount = int.TryParse(drSampRate["Count"].ToString(), out realCount) ? realCount : 0;//实测条数
                    int shouldCount = 0;//应测条数
                    decimal samplingRate = 0;//捕获率
                    int totalRealCount = 0;//合计实测条数
                    int totalShouldCount = 0;//合计应测条数
                    decimal totalSamplingRate = 0;//合计捕获率
                    DataRow drNew;
                    IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                   ? pointPollutantCodeList[pointId] : new List<string>();//该测点对应的因子Code列
                    int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == drSampRate["PointId"].ToString())
                                        .Select(p => p.DataCycle).FirstOrDefault();//每天应测条数
                    if (dataCycle != null && dataCycle != 0)
                    {
                        shouldCount = (int)Math.Floor(dataCycle.Value * shouldDays);//应测条数=一天几条*天数
                        realCount = (realCount > shouldCount) ? shouldCount : realCount;
                        samplingRate = Math.Round((decimal)realCount * 100 / shouldCount, 2);
                    }

                    //如果新表中没有该测点，则新增该测点数据
                    if (drsNew.Length == 0)
                    {
                        drNew = dtNew.NewRow();//新建行
                        drNew["PointId"] = pointId;//测点Id

                        //如果测点中有该因子则记录捕获率信息
                        if (pollutantCodeList.Contains(pollutantCode))
                        {
                            drNew[pollutantCode] = string.Format("{0}%<br/>{1}:{2}", samplingRate, realCount, shouldCount);//该因子捕获率
                            totalRealCount = realCount;
                            totalShouldCount = shouldCount;
                            totalRealCount = (totalRealCount > totalShouldCount) ? totalShouldCount : totalRealCount;
                            totalSamplingRate = Math.Round((decimal)totalRealCount * 100 / totalShouldCount, 2);
                            drNew["TotalRealCount"] = totalRealCount;//合计实测条数
                            drNew["TotalShouldCount"] = totalShouldCount;//合计应测条数
                            drNew["TotalValue"] = string.Format("{0}%<br/>{1}:{2}", totalSamplingRate, totalRealCount, totalShouldCount);//合计捕获率
                        }
                        else
                        {
                            drNew["TotalRealCount"] = 0;//合计实测条数
                            drNew["TotalShouldCount"] = 0;//合计应测条数
                            drNew["TotalValue"] = string.Empty;//合计捕获率
                        }
                        dtNew.Rows.Add(drNew);//添加新行到新表中
                    }
                    else if (pollutantCodeList.Contains(pollutantCode)) //如果新表中有该测点并且测点中有该因子，则添加该测点数据
                    {
                        drNew = drsNew[0];//该测点的行
                        drNew[pollutantCode] = string.Format("{0}%<br/>{1}:{2}", samplingRate, realCount, shouldCount);//该因子捕获率
                        totalRealCount = (int)drNew["TotalRealCount"] + realCount;
                        totalShouldCount = (int)drNew["TotalShouldCount"] + shouldCount;
                        totalRealCount = (totalRealCount > totalShouldCount) ? totalShouldCount : totalRealCount;
                        totalSamplingRate = Math.Round((decimal)totalRealCount * 100 / totalShouldCount, 2);
                        drNew["TotalRealCount"] = totalRealCount;//合计实测条数
                        drNew["TotalShouldCount"] = totalShouldCount;//合计应测条数
                        drNew["TotalValue"] = string.Format("{0}%<br/>{1}:{2}", totalSamplingRate, totalRealCount, totalShouldCount);//合计捕获率
                    }
                }
                AddDataRowsByPointIds(portIds, pointPollutantCodeList, shouldDays, dtNew);//根据测点Id数组增加空白数据行
                recordTotal = dtNew.Rows.Count;
                dtReturn = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
                return dtReturn.AsDataView();
            }
            return null;
        }

        /// <summary>
        /// 取得要导出的数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// Tstamp：日期
        /// 因子代码（多列，不是固定值）：如a21005
        /// 因子代码_Status（多列，不是固定值）：如a21005_Status
        /// </returns>
        public DataView GetSamplingRateExportData(string[] portIds, string[] factors
            , DateTime dtmStart, DateTime dtmEnd, string orderBy = "PointId")
        {
            decimal shouldDays = (decimal)(dtmEnd - dtmStart).TotalDays + 1;//每个因子应测天数
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            DataTable dtNew = CreateNewDataTableByPollutant(factors);//新建表
            InfectantBy60Service infectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();

            if (infectantBy60Service != null)
            {
                DataTable dtSampRate = infectantBy60Service.GetPollutantValueCount(portIds, factors, dtmStart, dtmEnd).Table;
                for (int i = 0; i < dtSampRate.Rows.Count; i++)
                {
                    DataRow drSampRate = dtSampRate.Rows[i];
                    string pointId = drSampRate["PointId"].ToString();
                    string pollutantCode = drSampRate["PollutantCode"].ToString();
                    DataRow[] drsNew = dtNew.Select(string.Format("PointId='{0}'", pointId));//有该测点的记录行
                    int realCount = int.TryParse(drSampRate["Count"].ToString(), out realCount) ? realCount : 0;//实测条数
                    int shouldCount = 0;//应测条数
                    decimal samplingRate = 0;//捕获率
                    int totalRealCount = 0;//合计实测条数
                    int totalShouldCount = 0;//合计应测条数
                    decimal totalSamplingRate = 0;//合计捕获率
                    DataRow drNew;
                    IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                   ? pointPollutantCodeList[pointId] : new List<string>();//该测点对应的因子Code列
                    int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == drSampRate["PointId"].ToString())
                                        .Select(p => p.DataCycle).FirstOrDefault();//每天应测条数
                    if (dataCycle != null && dataCycle != 0)
                    {
                        shouldCount = (int)Math.Floor(dataCycle.Value * shouldDays);//应测条数=一天几条*天数
                        realCount = (realCount > shouldCount) ? shouldCount : realCount;
                        samplingRate = Math.Round((decimal)realCount * 100 / shouldCount, 2);
                    }

                    //如果新表中没有该测点，则新增该测点数据
                    if (drsNew.Length == 0)
                    {
                        drNew = dtNew.NewRow();//新建行
                        drNew["PointId"] = pointId;//测点Id

                        //如果测点中有该因子则记录捕获率信息
                        if (pollutantCodeList.Contains(pollutantCode))
                        {
                            drNew[pollutantCode] = string.Format("{0}% \r\n{1}:{2}", samplingRate, realCount, shouldCount);//该因子捕获率
                            totalRealCount = realCount;
                            totalShouldCount = shouldCount;
                            totalRealCount = (totalRealCount > totalShouldCount) ? totalShouldCount : totalRealCount;
                            totalSamplingRate = Math.Round((decimal)totalRealCount * 100 / totalShouldCount, 2);
                            drNew["TotalRealCount"] = totalRealCount;//合计实测条数
                            drNew["TotalShouldCount"] = totalShouldCount;//合计应测条数
                            drNew["TotalValue"] = string.Format("{0}% \r\n{1}:{2}", totalSamplingRate, totalRealCount, totalShouldCount);//合计捕获率
                        }
                        else
                        {
                            drNew["TotalRealCount"] = 0;//合计实测条数
                            drNew["TotalShouldCount"] = 0;//合计应测条数
                            drNew["TotalValue"] = string.Empty;//合计捕获率
                        }
                        dtNew.Rows.Add(drNew);//添加新行到新表中
                    }
                    else if (pollutantCodeList.Contains(pollutantCode)) //如果新表中有该测点并且测点中有该因子，则添加该测点数据
                    {
                        drNew = drsNew[0];//该测点的行
                        drNew[pollutantCode] = string.Format("{0}% \r\n{1}:{2}", samplingRate, realCount, shouldCount);//该因子捕获率
                        totalRealCount = (int)drNew["TotalRealCount"] + realCount;
                        totalShouldCount = (int)drNew["TotalShouldCount"] + shouldCount;
                        totalRealCount = (totalRealCount > totalShouldCount) ? totalShouldCount : totalRealCount;
                        totalSamplingRate = Math.Round((decimal)totalRealCount * 100 / totalShouldCount, 2);
                        drNew["TotalRealCount"] = totalRealCount;//合计实测条数
                        drNew["TotalShouldCount"] = totalShouldCount;//合计应测条数
                        drNew["TotalValue"] = string.Format("{0}% \r\n{1}:{2}", totalSamplingRate, totalRealCount, totalShouldCount);//合计捕获率
                    }
                }
                AddDataRowsByPointIds(portIds, pointPollutantCodeList, shouldDays, dtNew);//根据测点Id数组增加空白数据行
                dtNew.DefaultView.Sort = orderBy;
                dtNew = dtNew.DefaultView.ToTable();
                return dtNew.AsDataView();
            }
            return null;
        }

        /// <summary>
        /// 取得数据总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public int GetSamplingRateAllDataCount(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd)
        {
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            InfectantBy60Service infectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();

            if (infectantBy60Service != null)
            {
                DataTable dtSampRate = infectantBy60Service.GetPollutantValueCount(portIds, factors, dtmStart, dtmEnd).Table;
                var query = from t in dtSampRate.AsEnumerable()
                            group t by new { PointId = t.Field<int>("PointId") } into m
                            select new
                            {
                                m.Key.PointId,
                                Count = m.Count()
                            };
                return query.Count();
            }
            return 0;
        }

        /// <summary>
        /// 取得统计数据（合计）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PollutantCode：因子代码
        /// PollutantTotal：因子数据
        /// </returns>
        public DataView GetSamplingRateStatisticalData(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd)
        {
            decimal shouldDays = (decimal)(dtmEnd - dtmStart).Days + 1;//每个因子应测天数
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            DataTable dtNew = new DataTable();//新建表
            decimal allRealCount = 0;//总合计实测条数
            int allShouldCount = 0;//总合计应测条数
            decimal allSampRate = 0;//总合计捕获率
            InfectantBy60Service infectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
            dtNew.Columns.Add("PollutantCode", typeof(string));//增加因子代码列
            dtNew.Columns.Add("PollutantTotal", typeof(string));//增加因子合计列

            if (infectantBy60Service != null)
            {
                DataTable dtSampRate = infectantBy60Service.GetPollutantValueCount(portIds, factors, dtmStart, dtmEnd).Table;

                for (int i = 0; i < factors.Length; i++)
                {
                    string pollutantCode = factors[i];
                    if (dtNew.Select(string.Format("PollutantCode='{0}'", pollutantCode)).Length == 0)
                    {
                        int totalRealCount = 0;//该因子的所有测点的总实测条数
                        int totalShouldCount = 0;//该因子的所有测点的总应测条数
                        decimal totalSamplingRate = 0;//该因子的所有测点的总捕获率
                        DataRow[] drsSampRate = dtSampRate.Select(string.Format("PollutantCode='{0}'", pollutantCode));//该因子的所有测点数据
                        DataRow drNew = dtNew.NewRow();
                        drNew["PollutantCode"] = pollutantCode;
                        dtNew.Rows.Add(drNew);

                        foreach (string pointId in portIds)//循环累计应测条数
                        {
                            IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                      ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                            if (pollutantCodeList.Contains(pollutantCode))//当前测点中有该因子，则累加应测条数
                            {
                                int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == pointId)
                                                              .Select(p => p.DataCycle).FirstOrDefault();//每天应测条数
                                if (dataCycle != null && dataCycle != 0)
                                {
                                    int shouldCount = (int)Math.Floor(dataCycle.Value * shouldDays);//应测条数=一天几条*天数
                                    totalShouldCount += shouldCount;//不同站点的应测条数累加
                                }
                            }
                        }
                        for (int j = 0; j < drsSampRate.Length; j++)//循环累计实测条数
                        {
                            int realCount = int.TryParse(drsSampRate[j]["Count"].ToString(), out realCount) ? realCount : 0;//实测条数
                            totalRealCount += realCount;
                        }
                        if (totalShouldCount > 0)
                        {
                            //计算捕获率
                            totalRealCount = (totalRealCount > totalShouldCount) ? totalShouldCount : totalRealCount;
                            totalSamplingRate = Math.Round((decimal)totalRealCount * 100 / totalShouldCount, 2);
                            drNew["PollutantTotal"] = string.Format("{0}%<br/>{1}:{2}", totalSamplingRate, totalRealCount, totalShouldCount);
                            allRealCount += totalRealCount;
                            allShouldCount += totalShouldCount;
                        }
                    }
                }
                if (allShouldCount == 0)
                {
                    dtNew.Rows.Add("TotalValue", null);//增加总合计数据
                }
                else
                {
                    allRealCount = (allRealCount > allShouldCount) ? allShouldCount : allRealCount;
                    allSampRate = Math.Round(allRealCount * 100 / allShouldCount, 2);
                    dtNew.Rows.Add("TotalValue", string.Format("{0}%<br/>{1}:{2}", allSampRate, allRealCount, allShouldCount));//增加总合计数据
                }
                return dtNew.AsDataView();
            }
            return null;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 改变列的数据类型
        /// </summary>
        /// <param name="dt">要改变的表</param>
        private void SetDataColumnDataType(DataTable dt)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName != "PointId" && dc.ColumnName != "Tstamp" && !dc.ColumnName.Contains("_Status")
                    && !dc.ColumnName.Contains("_Mark"))
                {
                    dc.DataType = typeof(string);
                }
            }
        }

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <param name="dtOld">原始表</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序方式（如：PointId,Tstamp）</param>
        /// <returns></returns>
        private DataTable GetDataPagerByPageSize(DataTable dtOld, int pageSize, int pageNo, string orderBy)
        {
            if (dtOld != null)
            {
                int startIndex = pageSize * pageNo;//要返回的数据的开始位置(从0开始)
                int endIndex = pageSize * (pageNo + 1);//要返回的数据的结束位置(数据取到小于这个值)
                DataTable dtReturn = dtOld.Clone();
                endIndex = (endIndex > dtOld.Rows.Count) ? dtOld.Rows.Count : endIndex;//如果结束位置大于最大行数，则改为最大行数的值
                orderBy = (orderBy != null) ? orderBy : string.Empty;
                string[] orderByValues = orderBy.Split(',');
                bool isOrderByContains = true;
                foreach (string orderByValue in orderByValues)
                {
                    isOrderByContains = dtOld.Columns.Contains(orderByValue);
                }
                if (isOrderByContains)
                {
                    dtOld.DefaultView.Sort = orderBy;
                    dtOld = dtOld.DefaultView.ToTable();
                }
                for (int i = startIndex; i < endIndex; i++)
                {
                    DataRow dr = dtOld.Rows[i];
                    dtReturn.Rows.Add(dr.ItemArray);
                }
                return dtReturn;
            }
            return null;
        }

        /// <summary>
        /// 根据因子数据生成新表
        /// </summary>
        /// <param name="factors"></param>
        /// <returns></returns>
        private DataTable CreateNewDataTableByPollutant(IList<IPollutant> factors)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PointId", typeof(string));
            foreach (IPollutant factor in factors)
            {
                if (!dt.Columns.Contains(factor.PollutantCode))
                {
                    dt.Columns.Add(factor.PollutantCode, typeof(string));
                }
            }
            dt.Columns.Add("TotalRealCount", typeof(int));
            dt.Columns.Add("TotalShouldCount", typeof(int));
            dt.Columns.Add("TotalValue", typeof(string));
            dt.Columns.Add("blankspaceColumn", typeof(string));
            return dt;
        }

        /// <summary>
        /// 根据因子数组生成新表
        /// </summary>
        /// <param name="pollutantCodes"></param>
        /// <returns></returns>
        private DataTable CreateNewDataTableByPollutant(string[] pollutantCodes)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PointId", typeof(string));
            foreach (string pollutantCode in pollutantCodes)
            {
                if (!dt.Columns.Contains(pollutantCode))
                {
                    dt.Columns.Add(pollutantCode, typeof(string));
                }
            }
            dt.Columns.Add("TotalRealCount", typeof(int));
            dt.Columns.Add("TotalShouldCount", typeof(int));
            dt.Columns.Add("TotalValue", typeof(string));
            dt.Columns.Add("blankspaceColumn", typeof(string));
            return dt;
        }

        /// <summary>
        /// 根据测点Id数组获取因子列
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <returns></returns>
        private string[] GetPollutantCodesByPointIds(string[] pointIds)
        {
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointWater.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            string[] pollutantCodes = GetPollutantCodesByPointEntitys(monitorPointQueryable.ToArray());
            return pollutantCodes;
        }

        /// <summary>
        /// 根据测点数组获取因子列
        /// </summary>
        /// <param name="monitoringPointEntitys">测点数组</param>
        /// <returns></returns>
        private string[] GetPollutantCodesByPointEntitys(MonitoringPointEntity[] monitoringPointEntitys)
        {
            IList<string> pollutantList = new List<string>();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            foreach (MonitoringPointEntity monitoringPointEntity in monitoringPointEntitys)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                    instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                foreach (PollutantCodeEntity pollutantCodeEntity in pollutantCodeQueryable)
                {
                    pollutantList.Add(pollutantCodeEntity.PollutantCode);
                }
            }
            return pollutantList.ToArray();
        }

        /// <summary>
        /// 根据测点Id数组获取因子数据列
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantDataByPointIds(string[] pointIds)
        {
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointWater.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            IList<PollutantCodeEntity> pollutantCodeList = null;
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                            instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);
                if (pollutantCodeList == null)
                {
                    pollutantCodeList = pollutantCodeQueryable.ToList();
                }
                else
                {
                    pollutantCodeList = pollutantCodeList.Union(pollutantCodeQueryable).ToList();
                }
            }
            return pollutantCodeList;
        }

        /// <summary>
        /// 根据因子Code数组获取因子数据列
        /// </summary>
        /// <param name="pollutantCodes">因子Code数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantDataByPollutantCodes(string[] pollutantCodes)
        {
            PollutantCodeRepository channelRepository = new PollutantCodeRepository();
            IList<PollutantCodeEntity> pollutantCodeList = channelRepository.Retrieve(t => pollutantCodes.Contains(t.PollutantCode)).ToList();
            return pollutantCodeList;
        }

        /// <summary>
        /// 根据测点数组获取测点Id列
        /// </summary>
        /// <param name="monitoringPointEntitys">测点数组</param>
        /// <returns></returns>
        private string[] GetPointIdsByPointEntitys(MonitoringPointEntity[] monitoringPointEntitys)
        {
            IList<string> pointIdList = new List<string>();
            foreach (MonitoringPointEntity monitoringPointEntity in monitoringPointEntitys)
            {
                if (!pointIdList.Contains(monitoringPointEntity.PointId.ToString()))
                {
                    pointIdList.Add(monitoringPointEntity.PointId.ToString());
                }
            }
            return pointIdList.ToArray();
        }

        /// <summary>
        /// 根据测点Id数组获取测点Id和集成商名称的对应关系列 
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetInstrumentNamesByPointIds(string[] pointIds)
        {
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            Dictionary<string, string> pointInstrumentList = new Dictionary<string, string>();
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointWater.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            IQueryable<V_CodeMainItemEntity> codeMainItemQueryable = dictionaryService.RetrieveList(DictionaryType.AMS, "集成商类型");//获取城市类型
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                if (!pointInstrumentList.ContainsKey(monitoringPointEntity.PointId.ToString()))
                {
                    string instrumentName = codeMainItemQueryable.Where(t => t.ItemGuid.Equals(monitoringPointEntity.InstrumentIntegratorUid))
                                                .Select(t => t.ItemText).FirstOrDefault();//集成商名称
                    pointInstrumentList.Add(monitoringPointEntity.PointId.ToString(), instrumentName);
                }
            }
            return pointInstrumentList;
        }

        /// <summary>
        /// 根据测点Id数组获取测点Id和对应的因子Code
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        private Dictionary<string, IList<string>> GetPointPollutantCodeByPointIds(string[] pointIds)
        {
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointWater.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            Dictionary<string, IList<string>> pointPollutantCodeList = new Dictionary<string, IList<string>>();
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                if (!pointPollutantCodeList.ContainsKey(monitoringPointEntity.PointId.ToString()))
                {
                    IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                        instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                    pointPollutantCodeList.Add(monitoringPointEntity.PointId.ToString(), pollutantCodeQueryable.Select(t => t.PollutantCode).ToList());
                }
            }
            return pointPollutantCodeList;
        }

        /// <summary>
        /// 根据站点Id获取IPoint列
        /// </summary>
        /// <param name="pointIds">站点Id</param>
        /// <returns></returns>
        private string[] GetPointList(params int[] pointIds)
        {
            IList<string> pointList = new List<string>();
            foreach (int pointId in pointIds)
            {
                MonitoringPointEntity monitorPointEntity = g_MonitoringPointWater.RetrieveEntityByPointId(pointId);
                if (monitorPointEntity != null)
                {
                    pointList.Add(monitorPointEntity.PointId.ToString());
                }
            }
            return pointList.ToArray();
        }

        /// <summary>
        /// 根据测点Id数组增加空白数据行
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <param name="pointPollutantCodeList">测点Id和对应的因子Code列表</param>
        /// <param name="shouldDays">应测天数</param>
        /// <param name="dt">表</param>
        private void AddDataRowsByPointIds(string[] pointIds, Dictionary<string, IList<string>> pointPollutantCodeList, decimal shouldDays, DataTable dt)
        {
            if (pointIds == null || pointIds.Length == 0 || dt == null)
            {
                return;
            }
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
            pointIds = pointIds.Distinct().ToArray();
            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                if (dt.Select(string.Format("PointId='{0}'", pointId)).Length == 0)
                {
                    DataRow dr = dt.NewRow();
                    IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                   ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                    int totalShouldCount = 0;//合计应测条数
                    int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == pointId)
                                        .Select(p => p.DataCycle).FirstOrDefault();//每天应测条数
                    dr["PointId"] = pointId;

                    if (dataCycle == null || dataCycle == 0)
                    {
                        continue;
                    }
                    foreach (string pollutantCode in pollutantCodeList)
                    {
                        if (dt.Columns.Contains(pollutantCode))//该因子属于该测点，才计算有效率
                        {
                            int shouldCount = (int)Math.Floor(dataCycle.Value * shouldDays);//应测条数=一天几条*天数
                            dr[pollutantCode] = string.Format("{0}%<br/>{1}:{2}", 0, 0, shouldCount);//该因子捕获率
                            totalShouldCount += shouldCount;
                            dr["TotalRealCount"] = 0;//合计实测条数
                            dr["TotalShouldCount"] = totalShouldCount;//合计应测条数
                            dr["TotalValue"] = string.Format("{0}%<br/>{1}:{2}", 0, 0, totalShouldCount);//合计捕获率
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
        }

        /// <summary>
        /// 根据测点Id数组和因子Code数组增加空白数据行 
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <param name="pollutantCodes">因子Code数组</param>
        /// <param name="pollutantCodes">因子数据列表</param>
        /// <param name="pointInstrumentList">集成商列表</param>
        /// <param name="pointPollutantCodeList">测点Id和对应的因子Code列表</param>
        /// <param name="shouldDays">应测天数</param>
        /// <param name="dt">表</param>
        private void AddDataRowsByPointPollutants(string[] pointIds, string[] pollutantCodes,
            IList<PollutantCodeEntity> pollutantEntityList, Dictionary<string, string> pointInstrumentList,
            Dictionary<string, IList<string>> pointPollutantCodeList, decimal shouldDays, DataTable dt)
        {
            if (pointIds == null || pointIds.Length == 0 || dt == null)
            {
                return;
            }
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
            pointIds = pointIds.Distinct().ToArray();
            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                for (int j = 0; j < pollutantCodes.Length; j++)
                {
                    string pollutantCode = pollutantCodes[j];
                    if (dt.Select(string.Format("PointId='{0}' and PollutantCode='{1}'", pointId, pollutantCode)).Length == 0)
                    {
                        DataRow dr = dt.NewRow();
                        MonitoringPointEntity entity = g_MonitoringPointWater.RetrieveEntityByPointId(int.Parse(pointId));//根据测点Id获取测点名称
                        PollutantCodeEntity pollutantEntity = pollutantEntityList.FirstOrDefault(t => t.PollutantCode == pollutantCode);
                        string pointName = (entity != null) ? entity.MonitoringPointName : string.Empty;
                        IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                   ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                        int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == pointId)
                                            .Select(p => p.DataCycle).FirstOrDefault();//每天应测条数
                        dr["PointId"] = pointIds[i];
                        dr["PointName"] = pointName;
                        dr["IntegratorName"] = pointInstrumentList[pointId.ToString()];
                        dr["PollutantCode"] = pollutantCode;
                        dr["PollutantName"] = (pollutantEntity != null) ? pollutantEntity.PollutantName : string.Empty;

                        if (dataCycle == null || dataCycle == 0)
                        {
                            continue;
                        }
                        if (pollutantCodeList.Contains(pollutantCode))//该因子属于该测点，才计算有效率
                        {
                            int shouldCount = (int)Math.Floor(dataCycle.Value * shouldDays);//应测条数=一天几条*天数
                            dr["Days"] = shouldDays;
                            dr["ShouldCount"] = shouldCount;
                            dr["RealCount"] = 0;
                            dr["SamplingRate"] = "0%";
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
        }

        /// <summary>
        /// 根据测点Id数组和集成商列表增加空白数据行 
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <param name="pointInstrumentList">集成商列表</param>
        /// <param name="pointPollutantCodeList">测点Id和对应的因子Code列表</param>
        /// <param name="shouldDays">应测天数</param>
        /// <param name="dt">表</param>
        private void AddDataRowsByPointInstruments(string[] pointIds, Dictionary<string, string> pointInstrumentList,
            Dictionary<string, IList<string>> pointPollutantCodeList, decimal shouldDays, DataTable dt)
        {
            if (pointIds == null || pointIds.Length == 0 || dt == null)
            {
                return;
            }
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();
            pointIds = pointIds.Distinct().ToArray();
            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                if (dt.Select(string.Format("PointId='{0}'", pointId)).Length == 0)
                {
                    DataRow dr = dt.NewRow();
                    MonitoringPointEntity entity = g_MonitoringPointWater.RetrieveEntityByPointId(int.Parse(pointId));//根据测点Id获取测点名称
                    string pointName = (entity != null) ? entity.MonitoringPointName : string.Empty;
                    IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                   ? pointPollutantCodeList[pointId] : new List<string>(); //该测点对应的因子Code列
                    int shouldCount = 0;
                    int rowShouldCount = 0;
                    int? dataCycle = g_ExtensionForEQMSWater.Retrieve(p => p.MonitoringPointEntity.PointId.ToString() == pointId)
                                            .Select(p => p.DataCycle).FirstOrDefault();//每天应测条数
                    dr["PointId"] = pointIds[i];
                    dr["PointName"] = pointName;
                    dr["IntegratorName"] = pointInstrumentList[pointId.ToString()];

                    if (dataCycle == null || dataCycle == 0)
                    {
                        continue;
                    }

                    shouldCount = (int)Math.Floor(dataCycle.Value * shouldDays);//应测条数=一天几条*天数
                    rowShouldCount = shouldCount * pollutantCodeList.Count(t => dt.Columns.Contains(t));
                    if (rowShouldCount > 0)//有应测条数才计算有效率
                    {
                        dr["Days"] = shouldDays;
                        dr["ShouldCount"] = rowShouldCount;
                        dr["RealCount"] = 0;
                        dr["SamplingRate"] = "0%";
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        #endregion
    }
}
