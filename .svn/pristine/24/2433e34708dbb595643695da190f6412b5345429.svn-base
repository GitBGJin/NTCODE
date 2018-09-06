using SmartEP.AMSRepository.Air;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.AutoMonitoring;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Air
{
    public class AirRealTimeOnlineStateNewService : BaseGenericRepository<MonitoringBusinessModel, V_DataSamplingConditionEntity>
    {

        InfectantBy60Repository g_InfectantBy60Repository = new InfectantBy60Repository();
        public override bool IsExist(string strKey)
        {
            return true;
        }

        /// <summary>
        /// 获取实时在线状态数据（中心平台）
        /// </summary>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="netWorkType">联网类型</param>
        /// <returns></returns>
        public DataView GetRealTimeOnlineStateDataPager(string[] portIds, string[] factors, string netWorkType, Dictionary<string, int> dicStatusCode, out DataView dvOnlineRate, string dataType, int pageSize, int pageNo, out int recordTotal,
            string orderBy = "PointId,Tstamp")
        {
            //获取站点联网状态
            DataTable dtDataOnlineState = null;
            //储存测点ID
            List<string> strPointID = new List<string>();
            //全部
            if (netWorkType.Equals("9999"))
            {
                //dtDataOnlineState = ConvertToDataTable(Retrieve(it => it.ApplicationUid == "airaaira-aira-aira-aira-airaairaaira" && portIds.Contains(it.PointId.ToString())));
                //这里包括在线和离线 如果获取全部状态使用上面的方法
                dtDataOnlineState = ConvertToDataTable(Retrieve(it => it.ApplicationUid == "airaaira-aira-aira-aira-airaairaaira" && portIds.Contains(it.PointId.ToString()) && (it.StatusCode == "0" || it.StatusCode == "1")));

            }
            else
            {
                dtDataOnlineState = ConvertToDataTable(Retrieve(it => it.ApplicationUid == "airaaira-aira-aira-aira-airaairaaira" && portIds.Contains(it.PointId.ToString()) && it.StatusCode == netWorkType));
            }


            //生成统计信息表
            DataTable dtStatisticsTable = CreateStatisticsTable(dicStatusCode);
            //按因子生成表结构（绑定此表数据）
            DataTable dtRealTimeOnlineStateTable = CreateRealTimeOnlineStateTable(factors);
            if (dtDataOnlineState != null && dtDataOnlineState.Rows.Count > 0)
            {
                #region 添加pointID
                foreach (DataRow dr in dtDataOnlineState.Rows)
                {
                    strPointID.Add(dr["PointId"].ToString());
                }
                #endregion

                #region 统计
                int AllCount = 0;
                DataRow drStatisticsTable = dtStatisticsTable.NewRow();//新建一行
                foreach (KeyValuePair<string, int> kv in dicStatusCode)
                {
                    DataRow[] drDataOnlineState = dtDataOnlineState.Select(string.Format("StatusCode='{0}'", kv.Value));
                    if (drDataOnlineState != null && drDataOnlineState.Count() > 0)
                    {
                        drStatisticsTable[kv.Key] = drDataOnlineState.Count();
                        AllCount += drDataOnlineState.Count();
                    }
                    else
                    {
                        drStatisticsTable[kv.Key] = 0;
                    }
                }

                drStatisticsTable["TotalCount"] = AllCount;
                if (AllCount != 0 && drStatisticsTable["OnlineCount"] != DBNull.Value)
                {
                    drStatisticsTable["OnlineRate"] = Convert.ToDecimal(string.Format("{0:N2}", (Convert.ToDecimal(drStatisticsTable["OnlineCount"]) / AllCount) * 100));
                }
                else
                {
                    drStatisticsTable["OnlineRate"] = 0;
                }
                dtStatisticsTable.Rows.Add(drStatisticsTable);
                #endregion

                #region list页面

                //获取因子数据
                DataTable dtRecentTimeData = g_InfectantBy60Repository.GetAirRecentTimeDataBy5Or60(portIds, factors, dataType);

                //去掉因子的引号
                for (int i = 0; i < factors.Length; i++)
                {
                    factors[i] = factors[i].Replace("'", "");
                }
                //把因子的行转化为列
                //DataTable dtRecentTimeData = dvWaterRecentTimeData.Table;

                //把在线数据表和因子数据表拼接为一个数据表
                foreach (string portID in strPointID.Distinct())
                {
                    bool flag = true;
                    DataRow drRealTimeOnlineStateTable = dtRealTimeOnlineStateTable.NewRow();
                    drRealTimeOnlineStateTable["PointId"] = portID;
                    //获取站点联网状态数据
                    DataRow[] drDataOnlineState = dtDataOnlineState.Select(string.Format("PointId='{0}'", portID));
                    if (drDataOnlineState != null && drDataOnlineState.Count() > 0)
                    {
                        drRealTimeOnlineStateTable["NetWorking"] = drDataOnlineState[0]["StatusCode"].ToString();//联网状态
                        if (!drDataOnlineState[0]["StatusCode"].Equals("0"))
                        {
                            flag = false;
                            drRealTimeOnlineStateTable["NetWorkInfo"] = GetNetWorkName(drDataOnlineState[0]["StatusCode"].ToString());
                        }
                    }

                    //获取站点数据组
                    DataRow[] drRecentTimeData = dtRecentTimeData.Select(string.Format("PointId='{0}'", portID));
                    if (drRecentTimeData != null && drRecentTimeData.Count() > 0)
                    {
                        DateTime tstamp = Convert.ToDateTime(drRecentTimeData[0]["Tstamp"]);//因为时间都一样 取第一个
                        if (flag)//离线
                        {
                            TimeSpan pointOffTime = DateTime.Now - tstamp;
                            drRealTimeOnlineStateTable["NetWorkInfo"] = (pointOffTime.Days > 0)
                                ? string.Format("离线{0}天{1}小时{2}分", pointOffTime.Days, pointOffTime.Hours, pointOffTime.Minutes)
                                : string.Format("离线{0}小时{1}分", pointOffTime.Hours, pointOffTime.Minutes);//联网信息
                        }

                        drRealTimeOnlineStateTable["Tstamp"] = tstamp;

                        //填充因子数据
                        foreach (string factor in factors)
                        {
                            DataRow[] drFactor = drRecentTimeData.CopyToDataTable().Select(string.Format("PollutantCode='{0}'", factor));
                            if (drFactor != null && drFactor.Count() > 0)
                            {
                                drRealTimeOnlineStateTable[factor] = drFactor[0]["PollutantValue"];//因子对应的值
                                drRealTimeOnlineStateTable[factor + "_Status"] = drFactor[0]["Status"].ToString();
                                drRealTimeOnlineStateTable[factor + "_Mark"] = drFactor[0]["Mark"].ToString();
                            }
                        }
                    }

                    dtRealTimeOnlineStateTable.Rows.Add(drRealTimeOnlineStateTable);
                }



                #endregion

            }

            dvOnlineRate = dtStatisticsTable.DefaultView;
            recordTotal = dtRealTimeOnlineStateTable.Rows.Count;
            DataTable dtData = GetDataPagerByPageSize(dtRealTimeOnlineStateTable, pageSize, pageNo, "PointId");
            return dtData.DefaultView;
        }

        /// <summary>
        /// 获取状态类型名称
        /// </summary>
        /// <param name="netWorkType">状态类型</param>
        /// <returns></returns>
        public string GetNetWorkName(string netWorkType)
        {
            string statusName = "未检测到状态";
            switch (netWorkType)
            {
                case "1": statusName = "在线"; break;
                case "4": statusName = "报警"; break;
                case "8": statusName = "故障"; break;
                case "16": statusName = "停运"; break;
                case "32": statusName = "始终在线"; break;
                default: break;

            }
            return statusName;

        }
        /// <summary>
        /// 中心平台按因子生成列
        /// </summary>
        /// <param name="factors">因子组</param>
        /// <returns></returns>
        private DataTable CreateRealTimeOnlineStateTable(string[] factors)
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("PointId", typeof(int));//测点             
            dtNew.Columns.Add("NetWorking", typeof(string));//联网状态            
            dtNew.Columns.Add("NetWorkInfo", typeof(string));//联网信息             
            dtNew.Columns.Add("Tstamp", typeof(DateTime));//联网时间             
            //按因子Code生成列
            foreach (string factor in factors)
            {
                dtNew.Columns.Add(factor, typeof(decimal));
                dtNew.Columns.Add(factor + "_Status", typeof(string));
                dtNew.Columns.Add(factor + "_Mark", typeof(string));

            }
            dtNew.Columns.Add("blankspaceColumn", typeof(string));//空白列            
            return dtNew;
        }
        /// <summary>
        /// 生成統計信息（中心平台）
        /// </summary>
        /// <returns></returns>
        private DataTable CreateStatisticsTable(Dictionary<string, int> dicStatusCode)
        {

            DataTable dtNew = new DataTable();
            foreach (KeyValuePair<string, int> kv in dicStatusCode)
            {
                dtNew.Columns.Add(kv.Key, typeof(decimal));
            }

            dtNew.Columns.Add("OnlineRate", typeof(decimal));//在线率
            dtNew.Columns.Add("TotalCount", typeof(decimal));//联网总数

            return dtNew;
        }

        /// <summary>
        /// 获取所有的状态个数(首页原件)
        /// </summary>
        /// <param name="portIds">测点组</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public DataTable GetRealTimeOnlineStateDataPager(string[] portIds, Dictionary<string, int> dicStatusCode)
        {
            DataTable dtHomePageTable = CreateHomePageTable(dicStatusCode);//生成结构表，用于填充数据
            DataTable dtDataOnlineState = ConvertToDataTable(Retrieve(it => it.ApplicationUid == "airaaira-aira-aira-aira-airaairaaira" && portIds.Contains(it.PointId.ToString())));
            if (dtDataOnlineState != null && dtDataOnlineState.Rows.Count > 0)
            {
                int AllCount = 0;
                DataRow drHomePageTable = dtHomePageTable.NewRow();//新建一行
                foreach (KeyValuePair<string, int> kv in dicStatusCode)
                {
                    DataRow[] drDataOnlineState = dtDataOnlineState.Select(string.Format("StatusCode='{0}'", kv.Value));
                    if (drDataOnlineState != null && drDataOnlineState.Count() > 0)
                    {
                        drHomePageTable[kv.Key] = drDataOnlineState.Count();
                        AllCount += drDataOnlineState.Count();
                    }
                }

                drHomePageTable["TotalCount"] = AllCount;
                dtHomePageTable.Rows.Add(drHomePageTable);
            }

            return dtHomePageTable;
        }
        /// <summary>
        /// 生成临时表(首页原件)
        /// </summary>
        /// <returns></returns>
        private DataTable CreateHomePageTable(Dictionary<string, int> dicStatusCode)
        {
            DataTable dtNew = new DataTable();

            //按Key值生成列
            foreach (KeyValuePair<string, int> kv in dicStatusCode)
            {
                dtNew.Columns.Add(kv.Key, typeof(int));
            }

            dtNew.Columns.Add("TotalCount", typeof(int));//总数            
            return dtNew;
        }

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <param name="dtOld">原始表</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序方式（如：PointId）</param>
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
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(V_DataSamplingConditionEntity)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (V_DataSamplingConditionEntity item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(V_DataSamplingConditionEntity)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }

        /// <summary>
        /// 获取实时在线状态数据（中心平台）
        /// </summary>
        /// <param name="portIds">测点组</param>
        /// <param name="netWorkType">联网类型</param>
        /// /// <param name="dataType">数据类型</param>
        /// <returns></returns>
        public DataView Get5MinOfflineInfo(string[] portIds, string netWorkType,  string dataType)
        {
            //获取站点联网状态
            DataTable dtDataOnlineState = null;
            //储存测点ID
            List<string> strPointID = new List<string>();
            //全部
            if (netWorkType.Equals("9999"))
            {
                //这里包括在线和离线 如果获取全部状态使用上面的方法
                dtDataOnlineState = ConvertToDataTable(Retrieve(it => it.ApplicationUid == "airaaira-aira-aira-aira-airaairaaira" && portIds.Contains(it.PointId.ToString()) && (it.StatusCode == "0" || it.StatusCode == "1")));

            }
            else
            {
                dtDataOnlineState = ConvertToDataTable(Retrieve(it => it.ApplicationUid == "airaaira-aira-aira-aira-airaairaaira" && portIds.Contains(it.PointId.ToString()) && it.StatusCode == netWorkType));
            }


            //生成表结构（绑定此表数据）
            DataTable dtRealTimeOnlineStateTable = Create0fflineStateTable();
            if (dtDataOnlineState != null && dtDataOnlineState.Rows.Count > 0)
            {
                #region 添加pointID
                foreach (DataRow dr in dtDataOnlineState.Rows)
                {
                    strPointID.Add(dr["PointId"].ToString());
                }
                #endregion
                #region list页面
                DataTable dtRecentTimeData = g_InfectantBy60Repository.GetAirRecentTimeDataBy5And60(portIds, dataType);
                foreach (string portID in strPointID.Distinct())
                {
                    bool flag = true;
                    DataRow drRealTimeOnlineStateTable = dtRealTimeOnlineStateTable.NewRow();
                    drRealTimeOnlineStateTable["PointId"] = portID;
                    //获取站点联网状态数据
                    DataRow[] drDataOnlineState = dtDataOnlineState.Select(string.Format("PointId='{0}'", portID));
                    if (drDataOnlineState != null && drDataOnlineState.Count() > 0)
                    {
                        drRealTimeOnlineStateTable["NetWorking"] = drDataOnlineState[0]["StatusCode"].ToString();//联网状态
                        if (!drDataOnlineState[0]["StatusCode"].Equals("0"))
                        {
                            flag = false;
                            drRealTimeOnlineStateTable["NetWorkInfo"] = GetNetWorkName(drDataOnlineState[0]["StatusCode"].ToString());
                        }
                    }

                    //获取站点数据组
                    DataRow[] drRecentTimeData = dtRecentTimeData.Select(string.Format("PointId='{0}'", portID));
                    if (drRecentTimeData != null && drRecentTimeData.Count() > 0)
                    {
                        DateTime tstamp = Convert.ToDateTime(drRecentTimeData[0]["Tstamp"]);//因为时间都一样 取第一个
                        if (flag)//离线
                        {
                            TimeSpan pointOffTime = DateTime.Now - tstamp;
                            drRealTimeOnlineStateTable["NetWorkInfo"] = (pointOffTime.Days > 0)
                                ? string.Format("{0}天{1}小时{2}分", pointOffTime.Days, pointOffTime.Hours, pointOffTime.Minutes)
                                : string.Format("{0}小时{1}分", pointOffTime.Hours, pointOffTime.Minutes);//联网信息
                        }

                        drRealTimeOnlineStateTable["Tstamp"] = tstamp;
                    }

                    dtRealTimeOnlineStateTable.Rows.Add(drRealTimeOnlineStateTable);
                }



                #endregion

            }

            return dtRealTimeOnlineStateTable.DefaultView;
        }

        /// <summary>
        /// 中心平台生成列
        /// </summary>
        /// <returns></returns>
        private DataTable Create0fflineStateTable()
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("PointId", typeof(int));//测点             
            dtNew.Columns.Add("NetWorking", typeof(string));//联网状态            
            dtNew.Columns.Add("NetWorkInfo", typeof(string));//联网信息             
            dtNew.Columns.Add("Tstamp", typeof(DateTime));//联网时间             
            return dtNew;
        }
    }
}
