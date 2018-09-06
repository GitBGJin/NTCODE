﻿using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.MonitoringBusinessRepository.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.BaseData.MPInfo;

namespace SmartEP.Service.DataAnalyze.Air.DataQuery
{
    /// <summary>
    /// 名称：DataQueryBySeasonService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：审核季数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataQueryBySeasonService
    {
        /// <summary>
        /// 空气季数据
        /// </summary>
        SeasonReportRepository SeasonData = Singleton<SeasonReportRepository>.GetInstance();

        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,SeasonOfYear）</param>
        /// <returns></returns>
        public DataView GetSeasonDataPager(string[] portIds, IList<IPollutant> factors, int yearFrom, int seasonOfYearFrom
            , int yearTo, int seasonOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,SeasonOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return SeasonData.GetDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,SeasonOfYear）</param>
        /// <returns></returns>
        public DataView GetSeasonDataPagerRegion(string[] portIds, IList<IPollutant> factors, int yearFrom, int seasonOfYearFrom
            , int yearTo, int seasonOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy="" )
        {
          recordTotal = 0;
          if (factors.IsNotNullOrDBNull())
            return SeasonData.GetSeasonDataPagerRegion(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
          return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,SeasonOfYear）</param>
        /// <returns></returns>
        public DataView GetSeasonDataPagersRegion(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom
            , int yearTo, int seasonOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
          recordTotal = 0;
          if (factors.IsNotNullOrDBNull())
            return SeasonData.GetSeasonDataPagerRegion(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
          return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,SeasonOfYear）</param>
        /// <returns></returns>
        public DataView GetSeasonDataPagerAvg(string[] portIds, IList<IPollutant> factors, int yearFrom, int seasonOfYearFrom
            , int yearTo, int seasonOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,SeasonOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return SeasonData.GetDataPagerAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,SeasonOfYear）</param>
        /// <returns></returns>
        public DataView GetSeasonDataAvg(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom
            , int yearTo, int seasonOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,SeasonOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return SeasonData.GetSeasonDataAvg(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,SeasonOfYear）</param>
        /// <returns></returns>
        public DataView GetSeasonDataPager(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom
            , int yearTo, int seasonOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,SeasonOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return SeasonData.GetDataPager(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,SeasonOfYear）</param>
        /// <returns></returns>
        public DataView GetSeasonDataPager(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo,
             int yearFromB, int seasonOfYearFromB, int yearToB, int seasonOfYearToB, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,SeasonOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return SeasonData.GetDataPager(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, yearFromB, seasonOfYearFromB, yearToB, seasonOfYearToB, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,SeasonOfYear）</param>
        /// <returns></returns>
        public DataView GetSeasonDataPagerDF(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo,
              int pageSize, int pageNo,  string orderBy = "PointId,Year,SeasonOfYear")
        {
            
            if (factors.IsNotNullOrDBNull())
                return SeasonData.GetDataPagerDF(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo,  pageSize, pageNo,  orderBy);
            return null;
        }
        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <returns></returns>
        public DataView GetSeasonStatisticalData(string[] portIds, IList<IPollutant> factors, int yearFrom, int seasonOfYearFrom
            , int yearTo, int seasonOfYearTo)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                DataTable dt = dt = SeasonData.GetStatisticalData(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo).Table;
                dt.Columns.Add("portName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
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
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetSeasonExportData(string[] portIds, IList<IPollutant> factors, int yearFrom, int seasonOfYearFrom
            , int yearTo, int seasonOfYearTo, string orderBy = "PointId,Year,SeasonOfYear")
        {
            if (SeasonData != null)
                return SeasonData.GetExportData(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, orderBy);
            return null;
        }

        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <returns></returns>
        public int GetSeasonAllDataCount(string[] portIds, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            if (SeasonData != null)
                return SeasonData.GetAllDataCount(portIds, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo);
            return 0;
        }
    }
}
