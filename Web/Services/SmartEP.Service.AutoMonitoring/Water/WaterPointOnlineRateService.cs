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
using SmartEP.DomainModel.WaterAutoMonitoring;
using SmartEP.Service.Frame;
using SmartEP.DomainModel;
using SmartEP.Service.Core.Enums;
using System.Collections;
using System.ComponentModel;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Water;

namespace SmartEP.Service.AutoMonitoring.Water
{
    /// <summary>
    /// 名称：WaterPointOnlineRateService.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-24
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：地表水站点在线率查询类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterPointOnlineRateService : IPointOnlineRate
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
        /// <summary>
        /// 运维商在线仓储层
        /// </summary>
        WaterPointOnlineRateRepository g_WaterPointOnlineRateRepository = Singleton<WaterPointOnlineRateRepository>.GetInstance();
        InstrumentChannelService m_InstrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();
        /// <summary>
        /// 实时数据仓储接口
        /// </summary>
        IInfectantRepository g_IInfectantRepository = null;

        /// <summary>
        /// 实时数据仓储类
        /// </summary>
        InfectantBy60Repository g_InfectantBy60Repository = null;

        /// <summary>
        /// 监测点：【水】测点扩展信息类
        /// </summary>
        MonitoringPointExtensionForEQMSWaterRepository g_ExtensionForEQMSWater = null;

        #region 获取在线率信息
        /// <summary>
        /// 获取在线率明细数据
        /// </summary>
        /// <param name="operations">运维商数据</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>在线率结果集
        /// 返回的列名
        /// OperationsId：运维商Id
        /// OperationsName：运维商名称
        /// PointId：测点Id
        /// PointName：测点名称
        /// DateTime：日期
        /// PointOnlineCount：测点在线条数
        /// PointOnlineShouldCount：测点应测条数
        /// PointOnlineRate：测点在线率
        /// OperationsOnlineCount：运维商在线条数
        /// OperationsOnlineShouldCount：运维商应测条数
        /// OperationsOnlineRate：运维商在线率
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetOnlineRateDataPager(string[] operations, string[] portIds
            , DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            DataTable dtNew = new DataTable();//新建表
            DataTable dtReturn;//要返回的视图的表
            int shouldDayCount = 24;//每个测点每天应测条数
            int startIndex = pageSize * pageNo;//要返回的数据的开始位置(从0开始)
            int endIndex = pageSize * (pageNo + 1);//要返回的数据的结束位置(数据取到小于这个值)
            Dictionary<string, string> pointInstrumentList = GetInstrumentNamesByPointIds(portIds);//根据测点Id数组获取测点Id和集成商名称的对应关系列 
            recordTotal = 0;
            dtmStart = DateTime.Parse(dtmStart.ToString("yyyy-MM-dd"));
            dtmEnd = DateTime.Parse(dtmEnd.ToString("yyyy-MM-dd"));
            g_IInfectantRepository = Singleton<InfectantBy60Repository>.GetInstance();
            g_InfectantBy60Repository = new InfectantBy60Repository();
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();

            if (g_IInfectantRepository != null)
            {
                IList<InfectantBy60Entity> entityList = g_InfectantBy60Repository
                          .Retrieve(t => portIds.Contains(t.PointId.ToString()) && t.Tstamp >= dtmStart && t.Tstamp <= dtmEnd).ToList();//获取符合条件的集合
                var queryableByHour = entityList.GroupBy(t => new { t.PointId, Tstamp = DateTime.Parse(t.Tstamp.ToString("yyyy-MM-dd HH:00")) })
                      .Select(t => new { t.Key.PointId, t.Key.Tstamp, HourCount = t.Count() });//按小时分组
                var queryableByDay = queryableByHour
                         .GroupBy(t => new { t.PointId, Tstamp = DateTime.Parse(t.Tstamp.ToString("yyyy-MM-dd")) })
                         .Select(t => new { t.Key.PointId, t.Key.Tstamp, DayCount = t.Count() })
                         .OrderBy(t => t.PointId).ThenBy(t => t.Tstamp);//按天分组

                dtNew.Columns.Add("OperationsId", typeof(string));//运维商Id
                dtNew.Columns.Add("OperationsName", typeof(string));//运维商名称
                dtNew.Columns.Add("PointId", typeof(int));//测点Id
                dtNew.Columns.Add("PointName", typeof(string));//测点名称
                dtNew.Columns.Add("DateTime", typeof(DateTime));//日期
                dtNew.Columns.Add("PointOnlineCount", typeof(decimal));//测点在线条数
                dtNew.Columns.Add("PointOnlineShouldCount", typeof(decimal));//测点应测条数
                dtNew.Columns.Add("PointOnlineRate", typeof(string));//测点在线率
                dtNew.Columns.Add("OperationsOnlineCount", typeof(string));//运维商在线条数
                dtNew.Columns.Add("OperationsOnlineShouldCount", typeof(string));//运维商应测条数
                dtNew.Columns.Add("OperationsOnlineRate", typeof(string));//运维商在线率
                dtNew.Columns.Add("blankspaceColumn", typeof(string));//空白列
                IList<string> operationsTimeList = new List<string>();//运维商+时间的列表
                foreach (string pointId in portIds)
                {
                    for (DateTime date = dtmStart; date <= dtmEnd; date = date.AddDays(1))
                    {
                        //当新表中不存在该测点Id和时间的组合时才添加
                        if (dtNew.Select(string.Format("PointId='{0}' and DateTime = '{1}'", pointId, date)).Length == 0)
                        {
                            DataRow drNew = dtNew.NewRow();
                            MonitoringPointEntity entity = g_MonitoringPointWater.RetrieveEntityByPointId(int.Parse(pointId));
                            drNew["PointName"] = (entity != null) ? entity.MonitoringPointName : string.Empty;
                            drNew["PointId"] = pointId;
                            drNew["OperationsId"] = pointId;
                            drNew["OperationsName"] = pointInstrumentList[pointId];
                            drNew["DateTime"] = date;
                            var pointDayData = queryableByDay.Where(t => t.PointId.ToString() == pointId
                                                    && t.Tstamp == date).FirstOrDefault();
                            int pointOnlineCount = pointDayData != null ? pointDayData.DayCount : 0;
                            drNew["PointOnlineCount"] = pointOnlineCount;
                            drNew["PointOnlineShouldCount"] = shouldDayCount;
                            decimal pointOnlineRate = Math.Round((decimal)pointOnlineCount * 100 / shouldDayCount, 2);
                            drNew["PointOnlineRate"] = string.Format("{0}%<br/>{1}:{2}", pointOnlineRate, pointOnlineCount, shouldDayCount);
                            dtNew.Rows.Add(drNew);

                            //如果不存在运维商+时间的组合，则添加
                            if (!operationsTimeList.Contains(drNew["OperationsName"].ToString() + ";" + drNew["DateTime"].ToString()))
                            {
                                operationsTimeList.Add(drNew["OperationsName"].ToString() + ";" + drNew["DateTime"].ToString());
                            }
                        }
                    }
                }
                foreach (string operationsTime in operationsTimeList)
                {
                    //计算运维商在线数据
                    string[] operationsTimes = operationsTime.Split(';');
                    DataRow[] drsOperationsTime = dtNew.Select(string.Format("OperationsName='{0}' and DateTime='{1}'", operationsTimes[0], operationsTimes[1]));
                    int operationsOnlineShouldCount = shouldDayCount * drsOperationsTime.Length;//运维商应测条数=每个测点应测条数*测点数量
                    object objOperationsOnlineCount = dtNew.Compute("sum(PointOnlineCount)",
                        string.Format("isnull(OperationsName,'')='{0}' and DateTime='{1}'", operationsTimes[0], operationsTimes[1]));//运维商在线条数=测点在线条数之和
                    decimal operationsOnlineCount = decimal.TryParse(objOperationsOnlineCount.ToString(), out operationsOnlineCount) ? operationsOnlineCount : 0;
                    foreach (DataRow drOperationsTime in drsOperationsTime)
                    {
                        drOperationsTime["OperationsOnlineCount"] = operationsOnlineCount;
                        drOperationsTime["OperationsOnlineShouldCount"] = operationsOnlineShouldCount;
                        decimal operationsOnlineRate = Math.Round(operationsOnlineCount * 100 / operationsOnlineShouldCount, 2);
                        drOperationsTime["OperationsOnlineRate"] = string.Format("{0}%<br/>{1}:{2}",
                                                operationsOnlineRate, operationsOnlineCount, operationsOnlineShouldCount);
                    }
                }
                AddDataRowsByPointOperations(portIds, pointInstrumentList, dtNew);//根据测点Id数组和运维商列表增加空白数据行 
                recordTotal = dtNew.Rows.Count;
                dtReturn = GetDataPagerByPageSize(dtNew, pageSize, pageNo, "OperationsName,PointId,DateTime desc");
                return dtReturn.AsDataView();
            }
            return null;
        }

        /// <summary>
        /// 获取在线率明细导出数据
        /// </summary>
        /// <param name="operations">运维商数据</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>在线率结果集
        /// 返回列名
        /// OperationsId：运维商Id
        /// OperationsName：运维商名称
        /// PointId：测点Id
        /// PointName：测点名称
        /// DateTime：日期
        /// PointOnlineCount：测点在线条数
        /// PointOnlineShouldCount：测点应测条数
        /// PointOnlineRate：测点在线率
        /// OperationsOnlineCount：运维商在线条数
        /// OperationsOnlineShouldCount：运维商应测条数
        /// OperationsOnlineRate：运维商在线率
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetOnlineRateExportData(string[] operations, string[] portIds,
             DateTime dtmStart, DateTime dtmEnd, string orderBy = "PointId,Tstamp")
        {
            DataTable dtNew = new DataTable();//新建表
            DataTable dtReturn;//要返回的视图的表
            int shouldDayCount = 24;//每个测点每天应测条数

            Dictionary<string, string> pointInstrumentList = GetInstrumentNamesByPointIds(portIds);//根据测点Id数组获取测点Id和集成商名称的对应关系列 

            dtmStart = DateTime.Parse(dtmStart.ToString("yyyy-MM-dd"));
            dtmEnd = DateTime.Parse(dtmEnd.ToString("yyyy-MM-dd"));
            g_IInfectantRepository = Singleton<InfectantBy60Repository>.GetInstance();
            g_InfectantBy60Repository = new InfectantBy60Repository();
            g_ExtensionForEQMSWater = Singleton<MonitoringPointExtensionForEQMSWaterRepository>.GetInstance();

            if (g_IInfectantRepository != null)
            {
                IList<InfectantBy60Entity> entityList = g_InfectantBy60Repository
                          .Retrieve(t => portIds.Contains(t.PointId.ToString()) && t.Tstamp >= dtmStart && t.Tstamp <= dtmEnd).ToList();//获取符合条件的集合
                var queryableByHour = entityList.GroupBy(t => new { t.PointId, Tstamp = DateTime.Parse(t.Tstamp.ToString("yyyy-MM-dd HH:00")) })
                      .Select(t => new { t.Key.PointId, t.Key.Tstamp, HourCount = t.Count() });//按小时分组
                var queryableByDay = queryableByHour
                         .GroupBy(t => new { t.PointId, Tstamp = DateTime.Parse(t.Tstamp.ToString("yyyy-MM-dd")) })
                         .Select(t => new { t.Key.PointId, t.Key.Tstamp, DayCount = t.Count() })
                         .OrderBy(t => t.PointId).ThenBy(t => t.Tstamp);//按天分组

                dtNew.Columns.Add("OperationsId", typeof(string));//运维商Id
                dtNew.Columns.Add("OperationsName", typeof(string));//运维商名称
                dtNew.Columns.Add("PointId", typeof(int));//测点Id
                dtNew.Columns.Add("PointName", typeof(string));//测点名称
                dtNew.Columns.Add("DateTime", typeof(DateTime));//日期
                dtNew.Columns.Add("PointOnlineCount", typeof(decimal));//测点在线条数
                dtNew.Columns.Add("PointOnlineShouldCount", typeof(decimal));//测点应测条数
                dtNew.Columns.Add("PointOnlineRate", typeof(string));//测点在线率
                dtNew.Columns.Add("OperationsOnlineCount", typeof(string));//运维商在线条数
                dtNew.Columns.Add("OperationsOnlineShouldCount", typeof(string));//运维商应测条数
                dtNew.Columns.Add("OperationsOnlineRate", typeof(string));//运维商在线率
                dtNew.Columns.Add("blankspaceColumn", typeof(string));//空白列
                IList<string> operationsTimeList = new List<string>();//运维商+时间的列表
                foreach (string pointId in portIds)
                {
                    for (DateTime date = dtmStart; date <= dtmEnd; date = date.AddDays(1))
                    {
                        //当新表中不存在该测点Id和时间的组合时才添加
                        if (dtNew.Select(string.Format("PointId='{0}' and DateTime = '{1}'", pointId, date)).Length == 0)
                        {
                            DataRow drNew = dtNew.NewRow();
                            MonitoringPointEntity entity = g_MonitoringPointWater.RetrieveEntityByPointId(int.Parse(pointId));
                            drNew["PointName"] = (entity != null) ? entity.MonitoringPointName : string.Empty;
                            drNew["PointId"] = pointId;
                            drNew["OperationsId"] = pointId;
                            drNew["OperationsName"] = pointInstrumentList[pointId];
                            drNew["DateTime"] = date;
                            var pointDayData = queryableByDay.Where(t => t.PointId.ToString() == pointId
                                                    && t.Tstamp == date).FirstOrDefault();
                            int pointOnlineCount = pointDayData != null ? pointDayData.DayCount : 0;
                            drNew["PointOnlineCount"] = pointOnlineCount;
                            drNew["PointOnlineShouldCount"] = shouldDayCount;
                            decimal pointOnlineRate = Math.Round((decimal)pointOnlineCount * 100 / shouldDayCount, 2);
                            drNew["PointOnlineRate"] = string.Format("{0}%   {1}:{2}", pointOnlineRate, pointOnlineCount, shouldDayCount);
                            dtNew.Rows.Add(drNew);

                            //如果不存在运维商+时间的组合，则添加
                            if (!operationsTimeList.Contains(drNew["OperationsName"].ToString() + ";" + drNew["DateTime"].ToString()))
                            {
                                operationsTimeList.Add(drNew["OperationsName"].ToString() + ";" + drNew["DateTime"].ToString());
                            }
                        }
                    }
                }
                foreach (string operationsTime in operationsTimeList)
                {
                    //计算运维商在线数据
                    string[] operationsTimes = operationsTime.Split(';');
                    DataRow[] drsOperationsTime = dtNew.Select(string.Format("OperationsName='{0}' and DateTime='{1}'", operationsTimes[0], operationsTimes[1]));
                    int operationsOnlineShouldCount = shouldDayCount * drsOperationsTime.Length;//运维商应测条数=每个测点应测条数*测点数量
                    object objOperationsOnlineCount = dtNew.Compute("sum(PointOnlineCount)",
                        string.Format("isnull(OperationsName,'')='{0}' and DateTime='{1}'", operationsTimes[0], operationsTimes[1]));//运维商在线条数=测点在线条数之和
                    decimal operationsOnlineCount = decimal.TryParse(objOperationsOnlineCount.ToString(), out operationsOnlineCount) ? operationsOnlineCount : 0;
                    foreach (DataRow drOperationsTime in drsOperationsTime)
                    {
                        drOperationsTime["OperationsOnlineCount"] = operationsOnlineCount;
                        drOperationsTime["OperationsOnlineShouldCount"] = operationsOnlineShouldCount;
                        decimal operationsOnlineRate = Math.Round(operationsOnlineCount * 100 / operationsOnlineShouldCount, 2);
                        drOperationsTime["OperationsOnlineRate"] = string.Format("{0}%   {1}:{2}",
                                                operationsOnlineRate, operationsOnlineCount, operationsOnlineShouldCount);
                    }
                }
                AddDataRowsByPointOperations(portIds, pointInstrumentList, dtNew);//根据测点Id数组和运维商列表增加空白数据行 
                //recordTotal = dtNew.Rows.Count;
                //dtReturn = GetDataPagerByPageSize(dtNew, pageSize, pageNo, "OperationsName,PointId,DateTime desc");
                //return dtReturn.AsDataView();

                AddDataRowsByPointOperations(portIds, pointInstrumentList, dtNew);//根据测点Id数组和运维商列表增加空白数据行 
                dtNew.DefaultView.Sort = "OperationsName,PointId,DateTime desc";
                dtNew = dtNew.DefaultView.ToTable();
                return dtNew.AsDataView();
            }
            return null;
        }
        #endregion

        #region 私有方法
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
        /// 根据测点Id数组获取测点Id和集成商名称的对应关系列 
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetInstrumentNamesByPointIds(string[] pointIds)
        {
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointWater.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            Dictionary<string, string> pointInstrumentList = new Dictionary<string, string>();
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                if (!pointInstrumentList.ContainsKey(monitoringPointEntity.PointId.ToString()))
                {
                    IQueryable<V_CodeMainItemEntity> codeMainItemQueryable = dictionaryService.RetrieveList(DictionaryType.AMS, "集成商类型");//获取城市类型
                    string instrumentName = codeMainItemQueryable.Where(t => t.ItemGuid.Equals(monitoringPointEntity.InstrumentIntegratorUid))
                                                .Select(t => t.ItemText).FirstOrDefault();//集成商名称
                    pointInstrumentList.Add(monitoringPointEntity.PointId.ToString(), instrumentName);
                }
            }
            return pointInstrumentList;
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
        /// 根据测点Id数组和运维商列表增加空白数据行 
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <param name="pointOperationsList">运维商列表</param>
        /// <param name="dt">表</param>
        private void AddDataRowsByPointOperations(string[] pointIds, Dictionary<string, string> pointOperationsList, DataTable dt)
        {
            if (pointIds == null || pointIds.Length == 0 || dt == null)
            {
                return;
            }
            pointIds = pointIds.Distinct().ToArray();
            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                if (dt.Select(string.Format("PointId='{0}'", pointId)).Length == 0)
                {
                    DataRow dr = dt.NewRow();
                    MonitoringPointEntity entity = g_MonitoringPointWater.RetrieveEntityByPointId(int.Parse(pointId));//根据测点Id获取测点名称
                    string pointName = (entity != null) ? entity.MonitoringPointName : string.Empty;
                    dr["PointId"] = pointIds[i];
                    dr["PointName"] = pointName;
                    dr["OperationsName"] = pointOperationsList[pointId.ToString()];
                    dt.Rows.Add(dr);
                }
            }
        }
        #endregion

        public DataView DataSamplingRateRetrieve(string[] instrumentUid, string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OperationsUId", typeof(string));//运维商UId
            //dt.Columns.Add("OperationsName", typeof(string));//运维商名称
            dt.Columns.Add("PointId", typeof(string));//测点Id
            dt.Columns.Add("PointName", typeof(string));//测点名称
            dt.Columns.Add("DateTime", typeof(string));//日期
            dt.Columns.Add("PointOnlineCount", typeof(decimal));//测点在线条数
            dt.Columns.Add("PointOnlineShouldCount", typeof(decimal));//测点应测条数
            dt.Columns.Add("PointOnlineRate", typeof(string));//测点在线率
            dt.Columns.Add("OperationsOnlineRate", typeof(string));//运维商在线率
            DataTable dtDataSamplingRate = ConvertToDataTable(g_WaterPointOnlineRateRepository.Retrieve(it => it.ReportDateTime >= dtmStart && it.ReportDateTime <= dtmEnd && portIds.Contains(it.PointId.ToString()) && factors.Contains(it.PollutantCode) && it.ApplicationUid == "watrwatr-watr-watr-watr-watrwatrwatr"));
            foreach (string pointId in portIds)
            {
                DataRow drNew = dt.NewRow();
                MonitoringPointEntity entity = g_MonitoringPointWater.RetrieveEntityByPointId(int.Parse(pointId));
                IList<string> list = new List<string>();
                string factorCode = string.Empty;
                string[] factor;
                IQueryable<PollutantCodeEntity> p = m_InstrumentChannelService.RetrieveChannelListByPointUid(entity.MonitoringPointUid);
                list = list.Union(p.Select(t => t.PollutantCode)).ToList();
                factor = list.ToArray();
                foreach (string f in factors)
                {
                    factorCode += "'" + f + "',";
                }
                factorCode.TrimEnd(',');
                DataRow[] drSamplingRate = dtDataSamplingRate.Select("PointId='" + pointId + "' and PollutantCode in (" + factorCode + ")");
                decimal PointOnlineCount = 0;
                decimal PointOnlineShouldCount = 0;
                foreach (DataRow drItem in drSamplingRate)
                {
                    decimal tempCount = decimal.TryParse(drItem["ActualCollectionNumber"].ToString(), out tempCount) ? tempCount : -9999;
                    if (tempCount != -9999)
                    {
                        PointOnlineCount += tempCount;
                    }
                    decimal tempShouldCount = decimal.TryParse(drItem["SamplingNumber"].ToString(), out tempShouldCount) ? tempShouldCount : -9999;
                    if (tempShouldCount != -9999)
                    {

                        TimeSpan ts1 = new TimeSpan(dtmEnd.Ticks);
                        TimeSpan ts2 = new TimeSpan(dtmStart.Ticks);
                        TimeSpan ts = ts1.Subtract(ts2).Duration();
                        int dateDiff = ts.Days;

                        //PointOnlineShouldCount = tempShouldCount * (dtmEnd.Day - dtmStart.Day + 1) * factor.Length;
                        PointOnlineShouldCount = tempShouldCount * (dateDiff + 1) * factor.Length;
                    }
                }
                string PointOnlineRate = "";
                if (PointOnlineShouldCount != 0)
                {
                    PointOnlineRate = (PointOnlineCount * 100 / PointOnlineShouldCount).ToString("0.00") + "%";
                }
                else
                {
                    PointOnlineRate = "0%";
                }
                drNew["OperationsUId"] = entity.InstrumentIntegratorUid;
                drNew["PointId"] = pointId;
                drNew["PointName"] = entity.MonitoringPointName;
                drNew["DateTime"] = dtmStart.ToString("yyyy/MM/dd") + "~" + dtmEnd.ToString("yyyy/MM/dd");
                drNew["PointOnlineCount"] = PointOnlineCount;
                drNew["PointOnlineShouldCount"] = PointOnlineShouldCount;
                drNew["PointOnlineRate"] = PointOnlineRate;
                dt.Rows.Add(drNew);
            }
            foreach (string instrument in instrumentUid)
            {
                DataRow[] drInstrument = dt.Select("OperationsUId='" + instrument + "'");
                decimal SumPointOnline = 0;
                decimal SumPointOnlineShould = 0;
                string OperationsOnlineRate = string.Empty;
                if (drInstrument.Length > 0)
                {
                    DataTable dtInstrument = drInstrument.CopyToDataTable();
                    SumPointOnline = decimal.TryParse(dtInstrument.Compute("sum(PointOnlineCount)", "PointOnlineCount is not null").ToString(), out SumPointOnline) ? SumPointOnline : 0;
                    SumPointOnlineShould = decimal.TryParse(dtInstrument.Compute("sum(PointOnlineShouldCount)", "PointOnlineShouldCount is not null").ToString(), out SumPointOnlineShould) ? SumPointOnlineShould : 0;
                }
                if (SumPointOnlineShould != 0)
                {
                    OperationsOnlineRate = (SumPointOnline * 100 / SumPointOnlineShould).ToString("0.00") + "%";
                }
                else
                {
                    OperationsOnlineRate = "0%";
                }
                foreach (DataRow dritem in drInstrument)
                {
                    dritem["OperationsOnlineRate"] = OperationsOnlineRate;
                }

            }
            return dt.DefaultView;
        }
        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(ReportSamplingRateByDayEntity)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (ReportSamplingRateByDayEntity item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(ReportSamplingRateByDayEntity)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }
    }
}
