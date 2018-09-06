using SmartEP.Core.Generic;
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
    /// 名称：DataQueryByMonthService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：审核月数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataQueryByMonthService
    {
        /// <summary>
        /// 地表水月数据
        /// </summary>
        MonthReportRepository MonthData = Singleton<MonthReportRepository>.GetInstance();
        /// <summary>
        /// 地表水日数据
        /// </summary>
        DayReportRepository DayData = Singleton<DayReportRepository>.GetInstance();
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = null;
        /// <summary>
        /// 因子
        /// </summary>
        SmartEP.Service.BaseData.Channel.WaterPollutantService m_WaterPollutantService = new SmartEP.Service.BaseData.Channel.WaterPollutantService();

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,MonthOfYear）</param>
        /// <returns></returns>
        public DataView GetMonthDataPager(string[] portIds, IList<IPollutant> factors, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,MonthOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return MonthData.GetDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,MonthOfYear）</param>
        /// <returns></returns>
        public DataView GetMonthDataPager(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,MonthOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return MonthData.GetDataPager(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,MonthOfYear）</param>
        /// <returns></returns>
        public DataView GetMonthDataPager(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo,
          int yearFromB, int monthOfYearFromB, int yearToB, int monthOfYearToB, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,MonthOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return MonthData.GetDataPager(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, yearFromB, monthOfYearFromB, yearToB, monthOfYearToB, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param> 
        /// <param name="monthOfYearTo">结束月数</param>
        /// <returns></returns>
        public DataView GetMonthStatisticalData(string[] portIds, IList<IPollutant> factors, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = dt = MonthData.GetStatisticalData(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, monthOfYearFrom, yearTo, monthOfYearTo).Table;
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
        public DataView GetMonthStatisticalData(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom
        , int yearTo, int monthOfYearTo)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = dt = MonthData.GetStatisticalData(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo).Table;
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
        /// 取得统计数据-周报（最大值、最小值、平均值、水质类别）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param> 
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <returns></returns>
        public DataView GetMonthData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = DayData.GetStatisticalData(portIds, factors, dateStart, dateEnd).Table;
                DataTable dt2 = MonthData.GetDataPager(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, pageSize, pageNo, out recordTotal, orderBy).ToTable();
                dt.Columns.Add("Grade", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        for (int k = 3; k < 3 + factors.Length; k++)
                        {
                            if (dt.Rows[i]["PointId"].ToString() == dt2.Rows[j]["PointId"].ToString() && dt.Rows[i]["PollutantCode"].ToString() == dt2.Columns[k].ColumnName)
                            {
                                dt.Rows[i]["Grade"] = dt2.Rows[j][k + 2 * factors.Length].ToString();
                            }
                        }
                    }
                }
                dt.Columns.Add("pointName", typeof(string));
                dt.Columns.Add("pollutantName", typeof(string));
                dt.Columns.Add("pollutantUnit", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["pointName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    IPollutant Ifactor = m_WaterPollutantService.GetPollutantInfo(dt.Rows[i]["PollutantCode"].ToString());
                    dt.Rows[i]["pollutantName"] = Ifactor.PollutantName;
                    dt.Rows[i]["pollutantUnit"] = Ifactor.PollutantMeasureUnit;
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
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetMonthExportData(string[] portIds, IList<IPollutant> factors, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo, string orderBy = "PointId,Year,MonthOfYear")
        {
            if (MonthData != null)
                return MonthData.GetExportData(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetMonthExportData(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo, string orderBy = "PointId,Year,MonthOfYear")
        {
            if (MonthData != null)
                return MonthData.GetExportData(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, orderBy);
            return null;
        }
        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <returns></returns>
        public int GetMonthAllDataCount(string[] portIds, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo)
        {
            if (MonthData != null)
                return MonthData.GetAllDataCount(portIds, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo);
            return 0;
        }
    }
}
