﻿using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.MonitoringBusinessRepository.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.BaseData.MPInfo;

namespace SmartEP.Service.DataAnalyze.Water.DataQuery
{
    /// <summary>
    /// 名称：DataQueryByYearService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：审核年数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataQueryByYearService
    {
        /// <summary>
        /// 地表水年数据
        /// </summary>
        YearReportRepository YearData = Singleton<YearReportRepository>.GetInstance();

        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = null;

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year）</param>
        /// <returns></returns>
        public DataView GetYearDataPager(string[] portIds, IList<IPollutant> factors, int yearFrom
            , int yearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return YearData.GetDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, yearTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year）</param>
        /// <returns></returns>
        public DataView GetYearDataPager(string[] portIds, string[] factors, int yearFrom
            , int yearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return YearData.GetDataPager(portIds, factors, yearFrom, yearTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year）</param>
        /// <returns></returns>
        public DataView GetYearDataPager(string[] portIds, string[] factors, int yearFrom, int yearTo,
           int yearFromB, int yearToB, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return YearData.GetDataPager(portIds, factors, yearFrom, yearTo, yearFromB, yearToB, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <returns></returns>
        public DataView GetYearStatisticalData(string[] portIds, IList<IPollutant> factors, int yearFrom
            , int yearTo)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = dt = YearData.GetStatisticalData(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, yearTo).Table;
                dt.Columns.Add("portName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                }
                return dt.DefaultView;
            }
            return null;
        }
        public DataView GetYearStatisticalData(string[] portIds, string[] factors, int yearFrom
      , int yearTo)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = dt = YearData.GetStatisticalData(portIds, factors, yearFrom, yearTo).Table;
                dt.Columns.Add("portName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                }
                return dt.DefaultView;
            }
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year）</param>
        /// <returns></returns>
        public DataView GetYearExportData(string[] portIds, IList<IPollutant> factors, int yearFrom, int yearTo, string orderBy = "PointId,Year")
        {
            if (YearData != null)
                return YearData.GetExportData(portIds, factors, yearFrom, yearTo, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year）</param>
        /// <returns></returns>
        public DataView GetYearExportData(string[] portIds, string[] factors, int yearFrom, int yearTo, string orderBy = "PointId,Year")
        {
            if (YearData != null)
                return YearData.GetExportData(portIds, factors, yearFrom, yearTo, orderBy);
            return null;
        }
        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <returns></returns>
        public int GetYearAllDataCount(string[] portIds, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            if (YearData != null)
                return YearData.GetAllDataCount(portIds, yearFrom, yearTo);
            return 0;
        }
    }
}
