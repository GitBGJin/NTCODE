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
    /// 名称：DataQueryByWeekService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：审核周数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataQueryByWeekService
    {
        /// <summary>
        /// 空气周数据
        /// </summary>
        WeekReportRepository WeekData = Singleton<WeekReportRepository>.GetInstance();

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
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetWeekDataPager(string[] portIds, IList<IPollutant> factors, int yearFrom, int weekOfYearFrom
            , int yearTo, int weekOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,WeekOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return WeekData.GetDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, weekOfYearFrom, yearTo, weekOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetWeekDataPagerAvg(string[] portIds, IList<IPollutant> factors, int yearFrom, int weekOfYearFrom
            , int yearTo, int weekOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,WeekOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return WeekData.GetDataPagerAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, weekOfYearFrom, yearTo, weekOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetWeekDataAvg(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom
            , int yearTo, int weekOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,WeekOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return WeekData.GetWeekDataAvg(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetWeekDataPager(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom
            , int yearTo, int weekOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,WeekOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return WeekData.GetDataPager(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetWeekDataPager(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom, int yearTo, int weekOfYearTo,
           int yearFromB, int weekOfYearFromB, int yearToB, int weekOfYearToB, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,WeekOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return WeekData.GetDataPager(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo,yearFromB,  weekOfYearFromB,  yearToB,  weekOfYearToB, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetWeekDataPagerDF(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom, int yearTo, int weekOfYearTo,
            int pageSize, int pageNo, string orderBy = "PointId,Year,WeekOfYear")
        {
            
            if (factors.IsNotNullOrDBNull())
                return WeekData.GetDataPagerDF(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo,  pageSize, pageNo,  orderBy);
            return null;
        }
        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <returns></returns>
        public DataView GetWeekStatisticalData(string[] portIds, IList<IPollutant> factors, int yearFrom, int weekOfYearFrom
            , int yearTo, int weekOfYearTo)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                DataTable dt = dt = WeekData.GetStatisticalData(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, weekOfYearFrom, yearTo, weekOfYearTo).Table;
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
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetWeekExportData(string[] portIds, IList<IPollutant> factors, int yearFrom, int weekOfYearFrom
            , int yearTo, int weekOfYearTo, int pageSize, string orderBy = "PointId,Year,WeekOfYear")
        {
            if (WeekData != null)
                return WeekData.GetExportData(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo, orderBy);
            return null;
        }

        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="yearFrom">开始年</param>
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public int GetWeekAllDataCount(string[] portIds, int yearFrom, int weekOfYearFrom, int yearTo, int weekOfYearTo)
        {
            if (WeekData != null)
                return WeekData.GetAllDataCount(portIds, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo);
            return 0;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetDataPagerRegion(string[] portIds, IList<IPollutant> factors, int yearFrom, int weekOfYearFrom
            , int yearTo, int weekOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          recordTotal = 0;
          if (factors.IsNotNullOrDBNull())
            return WeekData.GetDataPagerRegion(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, weekOfYearFrom, yearTo, weekOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
          return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetDataPagersRegion(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom
            , int yearTo, int weekOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy="")
        {
          recordTotal = 0;
          if (factors.IsNotNullOrDBNull())
            return WeekData.GetDataPagerRegion(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
          return null;
        }
    }
}
