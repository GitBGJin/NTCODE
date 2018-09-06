using SmartEP.AMSRepository.Air;
using SmartEP.AMSRepository.Interfaces;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.AirAutoMonitoring;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Common;
using SmartEP.Service.DataAnalyze.Interfaces;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.DomainModel;
using SmartEP.Service.Frame;
using SmartEP.Service.Core.Enums;

namespace SmartEP.Service.DataAnalyze.Air
{
    /// <summary>
    /// 名称：AirRealTimeOnlineStateService.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：环境空气实时在线状态类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AirRealTimeOnlineStateService : IRealTimeOnline
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

        /// <summary>
        /// 实时数据仓储接口
        /// </summary>
        IInfectantRepository g_IInfectantRepository = null;

        /// <summary>
        /// 离线时间判断
        /// </summary>
        int timeSpanMinute = 149;

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pollutantDataType">数据类型（1分钟、5分钟、1小时）</param>
        public AirRealTimeOnlineStateService(PollutantDataType pollutantDataType)
        {
            switch (pollutantDataType)
            {
                case PollutantDataType.Min5:
                    g_IInfectantRepository = Singleton<InfectantBy5Repository>.GetInstance();
                    timeSpanMinute = 10;
                    break;
                case PollutantDataType.Min60:
                    g_IInfectantRepository = Singleton<InfectantBy60Repository>.GetInstance();
                    timeSpanMinute = 149;
                    break;
                default:
                    g_IInfectantRepository = Singleton<InfectantBy60Repository>.GetInstance();
                    timeSpanMinute = 149;
                    break;
            }
        }
        #endregion

        #region 获取实时在线状态信息
        /// <summary>
        /// 根据时间获取在线状态信息
        /// </summary>
        /// <param name="netWorkType">联网状态（1：全部，2：在线，3：离线）</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvOnlineRate">联网率信息
        /// 返回的列名
        /// OnlineCount：在线数
        /// OfflineCount：离线数
        /// TotalCount：总数
        /// OnlineRate：联网率
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>结果集
        /// 返回的列名
        /// PointTypeUid：站点类型名称
        /// PointId：站点Id
        /// NetWorking：联网，值为"true"/"false"
        /// NetWorkInfo：联网信息
        /// Tstamp：最近时间
        /// 因子代码（多列，不是固定值）：如a21005
        /// 因子代码_Status（多列，不是固定值）：如a21005_Status
        /// 因子代码_Mark（多列，不是固定值）：如a21005_Mark
        /// 因子代码_OfflineTime（多列，不是固定值）：离线时间，如a21005_OfflineTime
        /// SamplingRate：捕获率
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetOnlineStateDataPager(string netWorkType, string[] portIds, string[] factors,
             DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, out DataView dvOnlineRate,
             string orderBy = "PointId,Tstamp")
        {
            DataTable dtReturn = null;//要返回的视图的表
            DataSamplingConditionRepository dataSamplingConditionRepository = Singleton<DataSamplingConditionRepository>.GetInstance();//站点信息仓储类
            recordTotal = 0;
            dvOnlineRate = null;

            if (g_IInfectantRepository != null)
            {
                DataTable dtLastPort = g_IInfectantRepository.GetLastDataByPort(portIds, factors, dtmStart, dtmEnd).Table;//最新站点在线数据
                DataTable dtLastPollutant = g_IInfectantRepository.GetLastDataByPollutant(portIds, factors, dtmStart, dtmEnd).Table;//最新因子在线数据
                DataTable dtNew = CreateNewDataTable(dtLastPort);//新表生成
                //Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(portIds);//根据测点Id数组获取测点Id和对应的因子Code
                Dictionary<string, string> siteTypeByPointIdsList = GetSiteTypeByPointIds(portIds);//根据站点Id数组获取站点Id和站点类型对应键值对
                Dictionary<string, string> pointStatusList = GetStatusCodeByPointId(portIds);//根据站点Id数组获取站点在线状态
                DataTable dtOnlineRate = CreateOnlineRateDataTable();//创建联网率表
                DataRow drOnlineRate = dtOnlineRate.NewRow();//新建行
                int onlineCount = 0;//在线数
                int offlineCount = 0;//离线数
                IList<string> pointIdOnLineList = new List<string>();//在线站点ID列表

                foreach (DataRow drLastPort in dtLastPort.Rows)
                {
                    DateTime lastPointTime = Convert.ToDateTime(drLastPort["Tstamp"]);//最近站点在线时间
                    DateTime lastPollutantTime = DateTime.MinValue;//最近因子在线时间
                    DataRow drNew = dtNew.NewRow();//新建行
                    DateTime dtmNow = DateTime.Now;//当前时间
                    TimeSpan timeSpan = new TimeSpan(0, timeSpanMinute, 0);//允许的时间差，默认为2小时
                    int realCount = 0;//实测条数
                    int shouldCount = 0;//应测条数
                    decimal samplingRate = 0;//捕获率
                    string pointId = drLastPort["PointId"].ToString();

                    //在线状态:在线1，离线2，报警4，故障8，停运16，始终在线32
                    string statusCode = pointStatusList.ContainsKey(pointId)
                        ? pointStatusList[pointId] : string.Empty;

                    ////该测点对应的因子Code列
                    //IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                    //               ? pointPollutantCodeList[pointId] : new List<string>();

                    //停运状态
                    if (statusCode == "16")
                    {
                        offlineCount++;//离线数累加

                        //如果联网状态是在线，则跳到下一行（1：全部，2：在线，3：离线）
                        if (netWorkType == "2")
                        {
                            continue;
                        }
                        drNew["NetWorking"] = false;
                        drNew["NetWorkInfo"] = "停运";
                    }
                    //如果在线状态为离线，并且当前时间与最近站点在线时间差小于允许的时间差时，则为在线
                    else if ((statusCode == "2" || string.IsNullOrEmpty(statusCode)) && (dtmNow - lastPointTime) < timeSpan)
                    {
                        onlineCount++;//在线数累加
                        if (!pointIdOnLineList.Contains(pointId))
                        {
                            pointIdOnLineList.Add(pointId);
                        }

                        //如果联网状态是离线，则跳到下一行（1：全部，2：在线，3：离线）
                        if (netWorkType == "3")
                        {
                            continue;
                        }
                        statusCode = "1";
                        drNew["NetWorking"] = true;//联网
                        drNew["NetWorkInfo"] = "在线";//联网信息
                    }
                    //如果状态为离线，或者状态不为在线并且当前时间与最近站点在线时间差大于允许的时间差时，则为离线
                    else if (statusCode == "2" || (statusCode != "1" && statusCode != "32" && (dtmNow - lastPointTime) > timeSpan))
                    {
                        offlineCount++;//离线数累加

                        //如果联网状态是在线，则跳到下一行（1：全部，2：在线，3：离线）
                        if (netWorkType == "2")
                        {
                            continue;
                        }
                        drNew["NetWorking"] = false;//联网
                        TimeSpan pointOffTime = DateTime.Now - lastPointTime;//- timeSpan;
                        drNew["NetWorkInfo"] = (pointOffTime.Days > 0)
                            ? string.Format("离线{0}天{1}小时{2}分", pointOffTime.Days, pointOffTime.Hours, pointOffTime.Minutes)
                            : string.Format("离线{0}小时{1}分", pointOffTime.Hours, pointOffTime.Minutes);//联网信息
                    }
                    //如果状态为在线，或者状态不为离线并且当前时间与最近站点在线时间差不大于允许的时间差时，则为在线
                    else if (statusCode == "1" || statusCode == "32" || (statusCode != "2" && (dtmNow - lastPointTime) <= timeSpan))
                    {
                        onlineCount++;//在线数累加
                        if (!pointIdOnLineList.Contains(pointId))
                        {
                            pointIdOnLineList.Add(pointId);
                        }

                        //如果联网状态是离线，则跳到下一行（1：全部，2：在线，3：离线）
                        if (netWorkType == "3")
                        {
                            continue;
                        }
                        drNew["NetWorking"] = true;//联网
                        drNew["NetWorkInfo"] = "在线";//联网信息
                    }
                    drNew["PointTypeUid"] = (siteTypeByPointIdsList.ContainsKey(drLastPort["PointId"].ToString()))
                        ? siteTypeByPointIdsList[drLastPort["PointId"].ToString()] : string.Empty;//站点类型名称
                    drNew["PointId"] = drLastPort["PointId"];//站点Id
                    drNew["Tstamp"] = drLastPort["Tstamp"];//最近时间
                    foreach (DataColumn dcLastPort in dtLastPort.Columns)
                    {
                        if (dcLastPort.ColumnName != "PointId" && dcLastPort.ColumnName != "Tstamp" && !dcLastPort.ColumnName.Contains("_Status")
                            && !dcLastPort.ColumnName.Contains("_Mark"))
                        {
                            ////如果该测点对应的因子列中没有当前因子，则该因子为无效因子，直接跳过
                            //if (!pollutantCodeList.Contains(dcLastPort.ColumnName))
                            //{
                            //    continue;
                            //}
                            shouldCount++;//应测条数累加
                            drNew[dcLastPort.ColumnName] = drLastPort[dcLastPort.ColumnName];//因子列
                            drNew[dcLastPort.ColumnName + "_Status"] = drLastPort[dcLastPort.ColumnName + "_Status"];//因子_Status列
                            drNew[dcLastPort.ColumnName + "_Mark"] = drLastPort[dcLastPort.ColumnName + "_Mark"];//因子_Mark列
                            if (drLastPort[dcLastPort.ColumnName].ToString() == string.Empty && dtLastPollutant.Columns.Contains(dcLastPort.ColumnName))
                            {
                                DataRow[] drsLastPollutant = dtLastPort.Select(string.Format("PointId='{0}'", drLastPort["PointId"]), "Tstamp desc");
                                if (drsLastPollutant.Length > 0)
                                {
                                    drNew[dcLastPort.ColumnName] = drsLastPollutant[0][dcLastPort.ColumnName];//因子列
                                    drNew[dcLastPort.ColumnName + "_Status"] = drsLastPollutant[0][dcLastPort.ColumnName + "_Status"];//因子_Status列
                                    drNew[dcLastPort.ColumnName + "_Mark"] = drsLastPollutant[0][dcLastPort.ColumnName + "_Mark"];//因子_Mark列
                                    drNew[dcLastPort.ColumnName + "_OfflineTime"] = drsLastPollutant[0]["Tstamp"];//因子_OfflineTime列:离线时间
                                    DateTime pollutantTime = Convert.ToDateTime(drsLastPollutant[0]["Tstamp"]);
                                    lastPollutantTime = pollutantTime > lastPollutantTime ? pollutantTime : lastPollutantTime;//取较大的时间
                                }
                            }
                            else
                            {
                                realCount++;//实测条数累加
                            }
                            if (string.IsNullOrWhiteSpace(drNew[dcLastPort.ColumnName].ToString()))
                            {
                                drNew[dcLastPort.ColumnName] = -10000;
                            }
                        }
                    }
                    if (shouldCount > 0)
                    {
                        samplingRate = Math.Round((decimal)realCount * 100 / shouldCount, 2);//该测点捕获率
                        drNew["SamplingRate"] = string.Format("{0}%<br/>{1}/{2}", samplingRate, realCount, shouldCount); //捕获率
                    }
                    drNew["blankspaceColumn"] = string.Empty;
                    dtNew.Rows.Add(drNew);//添加该行
                }
                recordTotal = dtNew.Rows.Count;//当前的行数

                //联网状态为全部或离线时（1：全部，2：在线，3：离线），增加空白数据行
                if (netWorkType == "1" || netWorkType == "3")
                {
                    string[] pointIdsOff = portIds.Except(pointIdOnLineList).ToArray();
                    AddDataRowsByPointSiteTypes(pointIdsOff, siteTypeByPointIdsList, factors.ToList(), dtNew);//根据测点Id数组和站点类型列表增加空白数据行 pointPollutantCodeList
                }
                else if (netWorkType == "2")
                {
                    offlineCount = (portIds.Length >= recordTotal) ? portIds.Length - recordTotal : 0;
                }

                //联网率信息集合
                offlineCount += (dtNew.Rows.Count - recordTotal);//离线数，加上新增的行（增加的空白数据行都默认离线）
                drOnlineRate["OnlineCount"] = onlineCount;//在线数
                drOnlineRate["OfflineCount"] = offlineCount;//离线数，加上新增的行（增加的空白数据行都默认离线）
                drOnlineRate["TotalCount"] = onlineCount + offlineCount;//总数
                drOnlineRate["OnlineRate"] = ((onlineCount + offlineCount) > 0)
                    ? Math.Round((decimal)onlineCount * 100 / (onlineCount + offlineCount), 2).ToString() + "%" : "0%";//联网率
                dtOnlineRate.Rows.Add(drOnlineRate);//添加行
                dvOnlineRate = dtOnlineRate.AsDataView();

                recordTotal = dtNew.Rows.Count;
                if (!dtNew.Columns.Contains("OrderPoint"))
                {
                    dtNew.Columns.Add("OrderPoint", typeof(int));
                }
                foreach (DataRow drOrder in dtNew.Rows)
                {
                    int pointId = int.TryParse(drOrder["PointId"].ToString(), out pointId) ? pointId : 0;
                    drOrder["OrderPoint"] = pointId;
                }
                dtReturn = GetDataPagerByPageSize(dtNew, pageSize, pageNo, "OrderPoint,Tstamp");
            }
            return ((dtReturn == null) ? null : dtReturn.AsDataView());
        }

        /// <summary>
        /// 获取实时在线状态信息
        /// </summary>
        /// <param name="netWorkType">联网状态（1：全部，2：在线，3：离线）</param>
        /// <param name="pointTypeUids">站点类型数据</param>     
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvOnlineRate">联网率信息
        /// 返回的列名
        /// OnlineCount：在线数
        /// OfflineCount：离线数
        /// TotalCount：总数
        /// OnlineRate：联网率
        /// </param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>结果集
        /// 返回的列名
        /// PointTypeUid：站点类型名称
        /// PointId：站点Id
        /// NetWorking：联网，值为true/false
        /// NetWorkInfo：联网信息
        /// Tstamp：最近时间
        /// 因子代码（多列，不是固定值）：如a21005
        /// 因子代码_Status（多列，不是固定值）：如a21005_Status
        /// 因子代码_Mark（多列，不是固定值）：如a21005_Mark
        /// 因子代码_OfflineTime（多列，不是固定值）：离线时间，如a21005_OfflineTime
        /// SamplingRate：捕获率
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetRealTimeOnlineStateDataPager(string netWorkType, string[] portIds, string[] factors,
               int pageSize, int pageNo, out int recordTotal, out DataView dvOnlineRate,
               string orderBy = "PointId,Tstamp")
        {
            DateTime dtmEnd = DateTime.Now;//结束时间为现在
            DateTime dtmStart = dtmEnd.AddDays(-7);//开始时间为一星期前
            return GetOnlineStateDataPager(netWorkType, portIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, out dvOnlineRate, orderBy);
        }

        /// <summary>
        /// 获取要导出的实时在线状态信息
        /// </summary>
        /// <param name="netWorkType">联网状态（1：全部，2：在线，3：离线）</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>结果集
        /// 返回的列名
        /// PointTypeUid：站点类型名称
        /// PointId：站点Id
        /// NetWorking：联网，值为true/false
        /// NetWorkInfo：联网信息
        /// Tstamp：最近时间
        /// 因子代码（多列，不是固定值）：如a21005
        /// 因子代码_Status（多列，不是固定值）：如a21005_Status
        /// 因子代码_Mark（多列，不是固定值）：如a21005_Mark
        /// 因子代码_OfflineTime（多列，不是固定值）：离线时间，如a21005_OfflineTime
        /// SamplingRate：捕获率
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetRealTimeOnlineStateExportData(string netWorkType, string[] portIds, string[] factors,
               string orderBy = "PointId,Tstamp")
        {
            int pageSize = int.MaxValue;
            int pageNo = 0;
            int recordTotal = 0;
            DataView dvOnlineRate = null;
            DataTable dtReturn = GetRealTimeOnlineStateDataPager(netWorkType, portIds, factors, pageSize, pageNo, out recordTotal, out dvOnlineRate, orderBy).Table;
            for (int i = 0; i < dtReturn.Rows.Count; i++)
            {
                DataRow drReturn = dtReturn.Rows[i];
                drReturn["SamplingRate"] = drReturn["SamplingRate"].ToString().Replace("<br/>", " \r\n");
            }
            return dtReturn.AsDataView();
        }
        #endregion

        #region 获取实时在线信息
        /// <summary>
        /// 获取实时在线信息
        /// </summary>
        /// <returns>返回结果集
        /// 返回的列名
        /// OnlineCount：在线数
        /// OfflineCount：离线数
        /// AlwaysOnlineCount：始终在线数
        /// TotalCount：总数
        /// OnlineRate：在线率
        /// OfflineRate：离线数
        /// </returns>
        public DataView GetRealTimeOnlineInfoData()
        {
            DataTable dtNew = new DataTable();//新建表
            DataSamplingConditionService dataSamplingConditionService = Singleton<DataSamplingConditionService>.GetInstance();
            dtNew.Columns.Add("OnlineCount", typeof(int));//在线数
            dtNew.Columns.Add("OfflineCount", typeof(int));//离线数
            dtNew.Columns.Add("AlwaysOnlineCount", typeof(int));//始终在线数
            dtNew.Columns.Add("TotalCount", typeof(int));//总数
            dtNew.Columns.Add("OnlineRate", typeof(string));//在线率
            dtNew.Columns.Add("OfflineRate", typeof(string));//离线数
            DataRow drNew = dtNew.NewRow();
            int onlineCount = dataSamplingConditionService.RetrieveAirOnline().Count();
            int offlineCount = dataSamplingConditionService.RetrieveAirOffline().Count();
            int alwaysOnlineCount = dataSamplingConditionService.RetrieveAirAlwaysOnline().Count();
            int totalCount = dataSamplingConditionService.RetrieveAirAll().Count();
            drNew["OnlineCount"] = onlineCount;
            drNew["OfflineCount"] = offlineCount;
            drNew["AlwaysOnlineCount"] = alwaysOnlineCount;
            drNew["TotalCount"] = totalCount;
            drNew["OnlineRate"] = (totalCount > 0) ? Math.Round((decimal)onlineCount * 100 / totalCount, 2).ToString() + "%" : "0%";
            drNew["OfflineRate"] = (totalCount > 0) ? Math.Round((decimal)offlineCount * 100 / totalCount, 2).ToString() + "%" : "0%";
            dtNew.Rows.Add(drNew);
            return dtNew.AsDataView();
        }

        /// <summary>
        /// 获取实时在线信息
        /// </summary>
        /// <param name="regionUids">区域Uid数组</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// OnlineCount：在线数
        /// OfflineCount：离线数
        /// AlwaysOnlineCount：始终在线数
        /// TotalCount：总数
        /// OnlineRate：在线率
        /// OfflineRate：离线数
        /// </returns>
        public DataView GetRealTimeOnlineInfoData(string[] regionUids)
        {
            DataTable dtNew = new DataTable();//新建表
            DataSamplingConditionService dataSamplingConditionService = Singleton<DataSamplingConditionService>.GetInstance();
            dtNew.Columns.Add("OnlineCount", typeof(int));//在线数
            dtNew.Columns.Add("OfflineCount", typeof(int));//离线数
            dtNew.Columns.Add("AlwaysOnlineCount", typeof(int));//始终在线数
            dtNew.Columns.Add("TotalCount", typeof(int));//总数
            dtNew.Columns.Add("OnlineRate", typeof(string));//在线率
            dtNew.Columns.Add("OfflineRate", typeof(string));//离线数
            DataRow drNew = dtNew.NewRow();
            int onlineCount = dataSamplingConditionService.RetrieveAirOnline().Count();
            int offlineCount = dataSamplingConditionService.RetrieveAirOffline().Count();
            int alwaysOnlineCount = dataSamplingConditionService.RetrieveAirAlwaysOnline().Count();
            int totalCount = dataSamplingConditionService.RetrieveAirAll().Count();
            drNew["OnlineCount"] = onlineCount;
            drNew["OfflineCount"] = offlineCount;
            drNew["AlwaysOnlineCount"] = alwaysOnlineCount;
            drNew["TotalCount"] = totalCount;
            drNew["OnlineRate"] = (totalCount > 0) ? Math.Round((decimal)onlineCount * 100 / totalCount, 2).ToString() + "%" : "0%";
            drNew["OfflineRate"] = (totalCount > 0) ? Math.Round((decimal)offlineCount * 100 / totalCount, 2).ToString() + "%" : "0%";
            dtNew.Rows.Add(drNew);
            return dtNew.AsDataView();
        }
        #endregion

        #region 获取运行状态信息
        /// <summary>
        /// 获取运行状态信息
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// OnlineCount：在线数
        /// OfflineCount：离线数
        /// AlarmCount：报警数
        /// FailureCount：故障数
        /// StopCount：停运数
        /// AlwaysOnlineCount：始终在线数
        /// TotalCount：总数
        /// </returns>
        public DataView GetRunningStateInfoData(string[] portIds)
        {
            DataTable dtNew = new DataTable();//新建表
            DataSamplingConditionService dataSamplingConditionService = Singleton<DataSamplingConditionService>.GetInstance();
            dtNew.Columns.Add("OnlineCount", typeof(int));//在线数
            dtNew.Columns.Add("OfflineCount", typeof(int));//离线数
            dtNew.Columns.Add("AlarmCount", typeof(int));//报警数
            dtNew.Columns.Add("FailureCount", typeof(int));//故障数
            dtNew.Columns.Add("StopCount", typeof(int));//停运数
            dtNew.Columns.Add("AlwaysOnlineCount", typeof(int));//始终在线数
            dtNew.Columns.Add("TotalCount", typeof(int));//总数
            DataRow drNew = dtNew.NewRow();
            IList<string> acquisitionUidList = GetAcquisitionUidsByPointId(portIds);//根据站点Id数组获取数采仪Id列表
            int onlineCount = (dataSamplingConditionService.RetrieveAirOnline() as IQueryable<DataSamplingConditionEntity>)
                              .Count(t => acquisitionUidList.Contains(t.AcquisitionUid));
            int offlineCount = (dataSamplingConditionService.RetrieveAirOffline() as IQueryable<DataSamplingConditionEntity>)
                              .Count(t => acquisitionUidList.Contains(t.AcquisitionUid));
            int alarmCount = (dataSamplingConditionService.RetrieveAirAlarm() as IQueryable<DataSamplingConditionEntity>)
                              .Count(t => acquisitionUidList.Contains(t.AcquisitionUid));
            int failureCount = (dataSamplingConditionService.RetrieveAirFailure() as IQueryable<DataSamplingConditionEntity>)
                              .Count(t => acquisitionUidList.Contains(t.AcquisitionUid));
            int stopCount = (dataSamplingConditionService.RetrieveAirStop() as IQueryable<DataSamplingConditionEntity>)
                              .Count(t => acquisitionUidList.Contains(t.AcquisitionUid));
            int alwaysOnlineCount = (dataSamplingConditionService.RetrieveAirAlwaysOnline() as IQueryable<DataSamplingConditionEntity>)
                              .Count(t => acquisitionUidList.Contains(t.AcquisitionUid));
            int totalCount = (dataSamplingConditionService.RetrieveAirAll() as IQueryable<DataSamplingConditionEntity>)
                              .Count(t => acquisitionUidList.Contains(t.AcquisitionUid));
            drNew["OnlineCount"] = onlineCount;
            drNew["OfflineCount"] = offlineCount;
            drNew["AlarmCount"] = alarmCount;
            drNew["FailureCount"] = failureCount;
            drNew["StopCount"] = stopCount;
            drNew["AlwaysOnlineCount"] = alwaysOnlineCount;
            drNew["TotalCount"] = totalCount;
            dtNew.Rows.Add(drNew);
            return dtNew.AsDataView();
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
        /// 根据站点Id数组获取数采仪Id列表
        /// </summary>
        /// <param name="pointIds">站点Id数据</param>
        /// <returns></returns>
        private IList<string> GetAcquisitionUidsByPointId(string[] pointIds)
        {
            AcquisitionInstrumentService acquisitionInstrumentService = Singleton<AcquisitionInstrumentService>.GetInstance();//数采仪接口
            pointIds = pointIds.Distinct().ToArray();
            IQueryable<MonitoringPointEntity> pointQueryable = g_MonitoringPointAir.RetrieveListByPointIds(pointIds);
            IList<string> acquisitionUidList = new List<string>();
            foreach (MonitoringPointEntity pointEntity in pointQueryable)
            {
                AcquisitionInstrumentEntity acquisitionEntity = acquisitionInstrumentService.RetrieveEntityByMonitoringPointUid(pointEntity.MonitoringPointUid);
                if (acquisitionEntity != null && !acquisitionUidList.Contains(acquisitionEntity.AcquisitionUid.ToString()))
                {
                    acquisitionUidList.Add(acquisitionEntity.AcquisitionUid.ToString());
                }
            }
            return acquisitionUidList;
        }

        /// <summary>
        /// 根据站点Id数组获取站点在线状态
        /// </summary>
        /// <param name="pointIds">站点Id数据</param>
        /// <returns></returns>
        private Dictionary<string, string> GetStatusCodeByPointId(string[] pointIds)
        {
            AcquisitionInstrumentService acquisitionInstrumentService = Singleton<AcquisitionInstrumentService>.GetInstance();//数采仪接口
            DataSamplingConditionService dataSamplingConditionService = Singleton<DataSamplingConditionService>.GetInstance();//监测点数据采集情况
            pointIds = pointIds.Distinct().ToArray();
            IQueryable<MonitoringPointEntity> pointQueryable = g_MonitoringPointAir.RetrieveListByPointIds(pointIds);
            IQueryable<AcquisitionInstrumentEntity> acquisitionInstrumentQueryable = acquisitionInstrumentService.RetrieveList();//获取所有数采仪列表
            IQueryable<DataSamplingConditionEntity> dataSamplingConditionEntityQueryable = dataSamplingConditionService.RetrieveAirAll()
                                            as IQueryable<DataSamplingConditionEntity>;//取得气站所有测点数据采集情况
            Dictionary<string, string> pointStatusList = new Dictionary<string, string>();
            foreach (MonitoringPointEntity pointEntity in pointQueryable)
            {
                AcquisitionInstrumentEntity acquisitionEntity = acquisitionInstrumentQueryable.Where(t => t.MonitoringPointUid == pointEntity.MonitoringPointUid)
                                                                        .FirstOrDefault();
                if (acquisitionEntity != null && !pointStatusList.ContainsKey(pointEntity.PointId.ToString()))
                {
                    DataSamplingConditionEntity dataSamplingConditionEntity =
                        dataSamplingConditionEntityQueryable.Where(t => t.AcquisitionUid.Equals(acquisitionEntity.AcquisitionUid))
                                                            .FirstOrDefault();
                    if (dataSamplingConditionEntity != null)
                    {
                        pointStatusList.Add(pointEntity.PointId.ToString(), dataSamplingConditionEntity.StatusCode);
                    }
                }
            }
            return pointStatusList;
        }

        /// <summary>
        /// 根据站点类型Uid数组获取站点和站点类型对应键值对
        /// </summary>
        /// <param name="pointTypeUids">站点类型数组</param>
        /// <returns></returns>
        private Dictionary<string, string> GetPointsByPointTypeUids(string[] pointTypeUids)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (pointTypeUids == null || pointTypeUids.Length == 0)
            {
                return dictionary;
            }
            foreach (string pointTypeUid in pointTypeUids)
            {
                //空气站点信息查询（根据站点名、行政区划Uid、站点类型Uid）
                IQueryable<MonitoringPointEntity> entityQueryable = g_MonitoringPointAir.RetrieveAirMPList(null, null, pointTypeUid, true);
                foreach (MonitoringPointEntity pointEntity in entityQueryable)
                {
                    if (!dictionary.ContainsKey(pointEntity.PointId.ToString()))
                    {
                        dictionary.Add(pointEntity.PointId.ToString(), pointTypeUid);
                    }
                }
            }
            return dictionary;
        }

        /// <summary>
        /// 根据站点Id数组获取站点Id和站点类型对应键值对
        /// </summary>
        /// <param name="pointIds">站点Id数组</param>
        /// <returns></returns>
        private Dictionary<string, string> GetSiteTypeByPointIds(string[] pointIds)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (pointIds == null || pointIds.Length == 0)
            {
                return dictionary;
            }
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointAir.RetrieveListByPointIds(pointIds); //根据站点ID数组获取站点
            IQueryable<V_CodeMainItemEntity> codeMainItemQueryable = dictionaryService.RetrieveList(DictionaryType.Air, "空气站点类型");//获取城市类型
            foreach (MonitoringPointEntity monitorPointEntity in monitorPointQueryable)
            {
                if (!dictionary.ContainsKey(monitorPointEntity.PointId.ToString()))
                {
                    string siteTypeName = codeMainItemQueryable.Where(t => t.ItemGuid.Equals(monitorPointEntity.SiteTypeUid))
                            .Select(t => t.ItemText).FirstOrDefault();//站点类型名称
                    dictionary.Add(monitorPointEntity.PointId.ToString(), siteTypeName);
                }
            }
            return dictionary;
        }

        /// <summary>
        /// 创建联网率表
        /// </summary>
        /// <returns></returns>
        private DataTable CreateOnlineRateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OnlineCount", typeof(int));//在线数
            dt.Columns.Add("OfflineCount", typeof(int));//离线数
            dt.Columns.Add("TotalCount", typeof(int));//总数            
            dt.Columns.Add("OnlineRate", typeof(string));//联网率
            return dt;
        }

        /// <summary>
        /// 根据原始表的列为新表生成列
        /// </summary>
        /// <param name="dtOld">原始表</param>
        /// <returns></returns>
        private DataTable CreateNewDataTable(DataTable dtOld)
        {
            DataTable dtNew = null;
            if (dtOld != null)
            {
                dtNew = new DataTable();
                dtNew.Columns.Add("PointTypeUid", typeof(string));//站点类型Uid
                dtNew.Columns.Add("PointId", typeof(string));//站点Id
                dtNew.Columns.Add("NetWorking", typeof(string));//联网，值为"true"/"false"
                dtNew.Columns.Add("NetWorkInfo", typeof(string));//联网信息
                dtNew.Columns.Add("Tstamp", typeof(DateTime));//最近时间
                foreach (DataColumn dcOld in dtOld.Columns)
                {
                    //如果新表中没有该列，则添加
                    if (!dcOld.ColumnName.Contains("_") && !dtNew.Columns.Contains(dcOld.ColumnName))
                    {
                        dtNew.Columns.Add(dcOld.ColumnName, dcOld.DataType);//因子列
                        dtNew.Columns.Add(dcOld.ColumnName + "_Status", typeof(string));
                        dtNew.Columns.Add(dcOld.ColumnName + "_Mark", typeof(string));
                        dtNew.Columns.Add(dcOld.ColumnName + "_OfflineTime", typeof(DateTime));//离线时间
                    }
                }
                dtNew.Columns.Add("SamplingRate", typeof(string));//捕获率
                if (dtNew.Columns.Contains("blankspaceColumn"))
                {
                    dtNew.Columns.Remove("blankspaceColumn");
                }
                dtNew.Columns.Add("blankspaceColumn", typeof(string));//空白列
            }
            return dtNew;
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
        /// 根据测点Id数组获取测点Id和对应的因子Code
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        private Dictionary<string, IList<string>> GetPointPollutantCodeByPointIds(string[] pointIds)
        {
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointAir.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
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
        /// 根据测点Id获取IPoint列
        /// </summary>
        /// <param name="pointIds">测点Id</param>
        /// <returns></returns>
        private string[] GetPointList(params int[] pointIds)
        {
            IList<string> pointList = new List<string>();
            foreach (int pointId in pointIds)
            {
                MonitoringPointEntity monitorPointEntity = g_MonitoringPointAir.RetrieveEntityByPointId(pointId);
                if (monitorPointEntity != null)
                {
                    pointList.Add(monitorPointEntity.PointId.ToString());
                }
            }
            return pointList.ToArray();
        }

        /// <summary>
        /// 根据测点Id数组和站点类型列表增加空白数据行 
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <param name="siteTypeByPointIdsList">站点类型列表</param>
        /// <param name="dt">表</param>
        private void AddDataRowsByPointSiteTypes(string[] pointIds, Dictionary<string, string> siteTypeByPointIdsList,
                         IList<string> pollutantCodeList, DataTable dt)//Dictionary<string, IList<string>> pointPollutantCodeList
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
                    int shouldCount = 0;//应测条数                    
                    //IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                    //               ? pointPollutantCodeList[pointId] : new List<string>();//该测点对应的因子Code列
                    dr["PointId"] = pointId;
                    dr["PointTypeUid"] = (siteTypeByPointIdsList.ContainsKey(pointId))
                        ? siteTypeByPointIdsList[pointId] : string.Empty;//站点类型Uid
                    if (dt.Columns.Contains("NetWorking"))
                    {
                        dr["NetWorking"] = false;
                    }
                    foreach (string pollutantCode in pollutantCodeList)
                    {
                        if (dt.Columns.Contains(pollutantCode))
                        {
                            shouldCount++;
                            if (string.IsNullOrWhiteSpace(dr[pollutantCode].ToString()))
                            {
                                dr[pollutantCode] = -10000;
                            }
                        }
                    }
                    if (shouldCount > 0)
                    {
                        dr["SamplingRate"] = string.Format("{0}%<br/>{1}/{2}", 0, 0, shouldCount); //捕获率
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        #endregion
    }
}
