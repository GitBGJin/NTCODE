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
using SmartEP.Utilities.Calendar;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.Core.Enums;
using SmartEP.DomainModel.BaseData;

namespace SmartEP.Service.DataAnalyze.Water.DataQuery
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
        /// 地表水周数据
        /// </summary>
        WeekReportRepository WeekData = Singleton<WeekReportRepository>.GetInstance();

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
                return WeekData.GetDataPager(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo, yearFromB, weekOfYearFromB, yearToB, weekOfYearToB, pageSize, pageNo, out recordTotal, orderBy);
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
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = dt = WeekData.GetStatisticalData(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, weekOfYearFrom, yearTo, weekOfYearTo).Table;
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
        public DataView GetWeekStatisticalData(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom
     , int yearTo, int weekOfYearTo)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = dt = WeekData.GetStatisticalData(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo).Table;
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
        public DataView GetWeekExportData(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom
            , int yearTo, int weekOfYearTo,string orderBy = "PointId,Year,WeekOfYear")
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
        public DataView GetWeekDataReport(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom
            , int yearTo, int weekOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,WeekOfYear")
        {
            recordTotal = 0;
            DataTable dtWeekReport = new DataTable();
            WaterQualityService WaterQuality = new WaterQualityService();
            WaterAnalyzeDAL m_WaterAnalyzeDAL = new WaterAnalyzeDAL();
            MonitoringPointService m_MonitoringPointService = new MonitoringPointService();
            SmartEP.Service.BaseData.Channel.WaterPollutantService m_WaterPollutantService = new SmartEP.Service.BaseData.Channel.WaterPollutantService();
            IQueryable<MonitoringPointEntity> pointList = m_MonitoringPointService.RetrieveListByPointIds(portIds);
            if (factors.IsNotNullOrDBNull())
            {
                dtWeekReport.Columns.Add("PointId", typeof(int));
                dtWeekReport.Columns.Add("RiverName", typeof(string));
                dtWeekReport.Columns.Add("SectionName", typeof(string));
                foreach (string factor in factors)
                {
                    dtWeekReport.Columns.Add("this" + factor, typeof(decimal));
                    dtWeekReport.Columns.Add("last" + factor, typeof(decimal));
                    dtWeekReport.Columns.Add("change" + factor, typeof(decimal));
                }
                dtWeekReport.Columns.Add("WaterClass", typeof(string));
                dtWeekReport.Columns.Add("WaterTarget", typeof(string));
                dtWeekReport.Columns.Add("IsQualified", typeof(string));
                dtWeekReport.Columns.Add("PrimaryPollutant", typeof(string));
                DataView thisWeekData = WeekData.GetDataPager(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
                DataView lastWeekData = new DataView();
                if (yearFrom != 1)
                {
                    lastWeekData = WeekData.GetDataPager(portIds, factors, yearFrom, weekOfYearFrom - 1, yearTo, weekOfYearTo - 1, pageSize, pageNo, out recordTotal, orderBy);
                }
                DataView waterQuality = m_WaterAnalyzeDAL.GetWaterAnalyzeData(portIds);
                for (int i = 0; i < portIds.Length; i++)
                {
                    int w_Value = 0;
                    string O_EvaluateFactorCodes = "";
                    string CalEQIType = "";
                    int portid = int.TryParse(portIds[i], out portid) ? portid : 0;
                    Dictionary<string, Int32> O_WQIValues = new Dictionary<string, int>();
                    DataRow[] qualityRow = waterQuality.ToTable().Select("PointId=" + portIds[i]);
                    DataRow[] thisWeekRow = thisWeekData.ToTable().Select("PointId=" + portIds[i]);
                    DataRow[] lastWeekRow = lastWeekData.ToTable().Select("PointId=" + portIds[i]);
                    DataRow drNew = dtWeekReport.NewRow();
                    string[] pointlist = pointList.Where(t => t.PointId == portid).Select(t => t.MonitoringPointName).ToArray();
                    if (pointlist.Length > 0)
                    {
                        drNew["SectionName"] = pointlist[0];
                    }
                    if (qualityRow.Count() > 0)
                    {
                        drNew["PointId"] = qualityRow[0]["PointId"];
                        drNew["RiverName"] = qualityRow[0]["WatersName"];
                        CalEQIType = qualityRow[0]["CalEQIType"].ToString();//获取功能水质水质类别
                    }
                    if (thisWeekRow.Count() > 0 && qualityRow.Count() > 0)
                    {
                        foreach (string factor in factors)
                        {
                            if (!string.IsNullOrEmpty(thisWeekRow[0][factor].ToString()))
                            {
                                string GetWQL;
                                //获取因子浓度
                                decimal pollutantValue = Convert.ToDecimal(thisWeekRow[0][factor]);
                                //获取单个因子的水质类别
                                #region 获取等级
                                switch (CalEQIType)
                                {
                                    case "湖泊":
                                        GetWQL = WaterQuality.GetWQL(factor, pollutantValue, EQITimeType.One, WaterPointCalWQType.Lake, EQIReurnType.Level);
                                        break;
                                    case "河流":
                                        GetWQL = WaterQuality.GetWQL(factor, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                        break;
                                    default:
                                        GetWQL = WaterQuality.GetWQL(factor, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                        break;
                                }
                                #endregion
                                GetWQL = WaterQuality.GetWQL(factor, pollutantValue, EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                if (!string.IsNullOrEmpty(GetWQL))
                                {
                                    //获取评价因子
                                    O_EvaluateFactorCodes += factor + ";";
                                    O_WQIValues.Add(factor, Convert.ToInt32(GetWQL));
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(O_EvaluateFactorCodes))
                        {
                            O_EvaluateFactorCodes = O_EvaluateFactorCodes.Substring(0, O_EvaluateFactorCodes.Length - 1);
                            //获取水质类别值
                            w_Value = Convert.ToInt32(WaterQuality.GetWQL_Max(EQIReurnType.Value, O_EvaluateFactorCodes, O_WQIValues));
                            string w_Class = WaterQuality.GetWQL_Max(EQIReurnType.Roman, O_EvaluateFactorCodes, O_WQIValues);
                            drNew["WaterClass"] = w_Class;
                            //首要污染物
                            string PrimaryPollutant = WaterQuality.GetWQL_Max(EQIReurnType.Name, O_EvaluateFactorCodes, O_WQIValues);
                            drNew["PrimaryPollutant"] = PrimaryPollutant;
                        }
                        //else
                        //{
                        //    dt.Rows[i]["RealClass"] = "/";
                        //}
                        //if (thisWeekRow[0]["Grade"].IsNotNullOrDBNull())
                        //{
                        //    drNew["WaterClass"] = thisWeekRow[0]["Grade"];
                        //}
                        drNew["WaterTarget"] = qualityRow[0]["Class"];
                        if (w_Value != 0 && qualityRow[0]["IEQI"].IsNotNullOrDBNull())
                        {
                            if (w_Value >= Convert.ToDecimal(waterQuality[0]["IEQI"]))
                            {
                                drNew["IsQualified"] = "×";
                            }
                            else
                            {
                                drNew["IsQualified"] = "√";
                            }
                        }
                    }


                    foreach (string factor in factors)
                    {
                        int factorUnit = Convert.ToInt32(m_WaterPollutantService.GetPollutantInfo(factor).PollutantDecimalNum);
                        decimal thisfactor = -9999;
                        decimal lastfactor = -9999;
                        if (thisWeekRow.Count() > 0)
                        {
                            thisfactor = decimal.TryParse(thisWeekRow[0][factor].ToString(), out thisfactor) ? thisfactor : -9999;
                            if (thisfactor != -9999)
                            {
                                drNew["this" + factor] = (DecimalExtension.GetPollutantValue(thisfactor, factorUnit)).ToString();
                            }
                        }
                        if (lastWeekRow.Count() > 0)
                        {
                            lastfactor = decimal.TryParse(lastWeekRow[0][factor].ToString(), out lastfactor) ? lastfactor : -9999;
                            if (lastfactor != -9999)
                            {
                                drNew["last" + factor] = (DecimalExtension.GetPollutantValue(lastfactor, factorUnit)).ToString();
                            }
                        }
                        if (thisfactor != -9999 && lastfactor != -9999)
                        {
                            drNew["change" + factor] = ((thisfactor - lastfactor) * 100 / lastfactor).ToString("#0.0");
                        }
                    }

                    dtWeekReport.Rows.Add(drNew);
                }
                return dtWeekReport.DefaultView;
            }
            else
            {
                return null;
            }
        }
    }
}
