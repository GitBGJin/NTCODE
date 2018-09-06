using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.MonitoringBusinessRepository.Water;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water
{
    /// <summary>
    /// 名称：WaterGrossStatisticsService.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-04
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：地表水总量统计类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterGrossStatisticsService
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();

        /// <summary>
        /// 小时报告类
        /// </summary>
        HourReportRepository g_HourReportRepository;

        #region 获取总量统计
        /// <summary>
        /// 获取总量统计数据
        /// </summary>
        /// <param name="pointIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvStatistical">合计数据</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns>返回结果集
        /// 返回的列名
        /// PointId：测点Id
        /// Tstamp：时间
        /// 因子代码（多列，不是固定值）：如w01009
        /// TotalValue：合计
        /// blankspaceColumn：空白列
        /// </returns>
        public DataView GetGrossStatisticsData(string[] pointIds, string[] factors, DateTime dtmStart, DateTime dtmEnd,
           int pageSize, int pageNo, out int recordTotal, out DataView dvStatistical, string orderBy = "PointId,Tstamp")
        {
            dtmStart = new DateTime(dtmStart.Year, dtmStart.Month, dtmStart.Day, 0, 0, 0);
            dtmEnd = new DateTime(dtmEnd.Year, dtmEnd.Month, dtmEnd.Day, 23, 59, 59);
            factors = factors.Union(new string[] { "w01029" }).ToArray();//加上瞬时流量Code
            DataTable dtNew = CreateNewDataTableByPollutant(factors);//根据因子数组生成新表
            //decimal hours = (decimal)(dtmEnd - dtmStart).TotalHours;//小时
            int second = 3600;//秒
            Dictionary<string, IList<string>> pointPollutantCodeList = GetPointPollutantCodeByPointIds(pointIds);//根据站点Id数组获取站点Id和站点类型对应键值对
            recordTotal = 0;
            dvStatistical = null;
            g_HourReportRepository = Singleton<HourReportRepository>.GetInstance();

            if (g_HourReportRepository != null)
            {
                DataTable dtHour = g_HourReportRepository.GetDataPager(pointIds, factors, dtmStart, dtmEnd, pageSize, pageNo, out recordTotal, orderBy).Table;//审核后的数据
                for (int i = 0; i < dtHour.Rows.Count; i++)
                {
                    string pointId = dtHour.Rows[i]["PointId"].ToString();//测点Id
                    IList<string> pollutantCodeList = (pointPollutantCodeList.ContainsKey(pointId))
                                        ? pointPollutantCodeList[pointId] : new List<string>();//该测点对应的因子Code列

                    //如果新表中没有该测点的数据，则添加
                    if (dtNew.Select(string.Format("PointId='{0}'", pointId)).Length == 0)
                    {
                        DataRow[] drsHour = dtHour.Select(string.Format("PointId='{0}'", pointId));
                        DataRow drNew = dtNew.NewRow();
                        drNew["PointId"] = pointId;
                        //drNew["Tstamp"] = null;

                        foreach (DataColumn dcNew in dtNew.Columns)
                        {
                            if (dcNew.ColumnName != "PointId" && dcNew.ColumnName != "Tstamp" && dcNew.ColumnName != "w01029"
                                && dcNew.ColumnName != "TotalValue" && dcNew.ColumnName != "blankspaceColumn"
                                && dtHour.Columns.Contains(dcNew.ColumnName))
                            {
                                decimal totalValue = 0;

                                //如果该测点对应的因子列中没有当前因子，则该因子为无效因子，直接跳过
                                if (!pollutantCodeList.Contains(dcNew.ColumnName))
                                {
                                    continue;
                                }
                                foreach (DataRow drHour in drsHour)
                                {
                                    if (!string.IsNullOrWhiteSpace(drHour[dcNew.ColumnName].ToString())
                                        && !string.IsNullOrWhiteSpace(drHour["w01029"].ToString()))
                                    {
                                        decimal pollutantValue = decimal.TryParse(drHour[dcNew.ColumnName].ToString(), out pollutantValue) ? pollutantValue : 0;
                                        decimal flow = decimal.TryParse(drHour["w01029"].ToString(), out flow) ? flow : 0;//流速，m3/s
                                        decimal total = Math.Round(pollutantValue * second * flow * 1000, 4);//总量=浓度 * 时间 * 流量
                                        totalValue += total;
                                    }
                                }
                                drNew[dcNew.ColumnName] = totalValue;
                            }
                        }
                        dtNew.Rows.Add(drNew);
                    }
                }
                dvStatistical = dtNew.Clone().AsDataView();
                AddDataRowsByPointIds(pointIds, dtNew);//根据测点Id数组增加空白数据行
                recordTotal = dtNew.Rows.Count;
                dtNew = GetDataPagerByPageSize(dtNew, pageSize, pageNo, orderBy);
            }
            return (dtNew != null) ? dtNew.AsDataView() : null;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 根据因子数组生成新表
        /// </summary>
        /// <param name="pollutantCodes"></param>
        /// <returns></returns>
        private DataTable CreateNewDataTableByPollutant(string[] pollutantCodes)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PointId", typeof(string));
            dt.Columns.Add("Tstamp", typeof(DateTime));
            foreach (string pollutantCode in pollutantCodes)
            {
                if (!dt.Columns.Contains(pollutantCode))
                {
                    dt.Columns.Add(pollutantCode, typeof(string));
                }
            }
            dt.Columns.Add("TotalValue", typeof(string));
            dt.Columns.Add("blankspaceColumn", typeof(string));
            return dt;
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
        /// 根据测点Id数组增加空白数据行
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <param name="dt">表</param>
        private void AddDataRowsByPointIds(string[] pointIds, DataTable dt)
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
                    dr["PointId"] = pointId;
                    dt.Rows.Add(dr);
                }
            }
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
        #endregion
    }
}
