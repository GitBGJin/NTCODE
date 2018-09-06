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
using SmartEP.Service.BaseData.Standard;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.Core.Enums;


namespace SmartEP.Service.DataAnalyze.Air.DataQuery
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
        /// 空气月数据
        /// </summary>
        MonthReportRepository MonthData = Singleton<MonthReportRepository>.GetInstance();

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
        public DataView GetDataPagerRegion(string[] portIds, IList<IPollutant> factors, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          recordTotal = 0;
          if (factors.IsNotNullOrDBNull())
            return MonthData.GetDataPagerRegion(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetDataPagersRegion(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy="")
        {
          recordTotal = 0;
          if (factors.IsNotNullOrDBNull())
            return MonthData.GetDataPagersRegion(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
          return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetMonthDataPagerAvg(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return MonthData.GetDataPagerAvg(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetMonthDataAvg(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,MonthOfYear")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return MonthData.GetMonthDataAvg(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
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
          int yearFromB, int monthOfYearFromB, int yearToB, int monthOfYearToB, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year desc,MonthOfYear desc")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return MonthData.GetDataPager(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, yearFromB, monthOfYearFromB, yearToB, monthOfYearToB, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetMonthDataPagerDF(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo,
           int pageSize, int pageNo,  string orderBy = "PointId,Year desc,MonthOfYear desc")
        {
            
            if (factors.IsNotNullOrDBNull())
                return MonthData.GetDataPagerDF(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo,  pageSize, pageNo,  orderBy);
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
                g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                DataTable dt = dt = MonthData.GetStatisticalData(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearFrom, monthOfYearFrom, yearTo, monthOfYearTo).Table;
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
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <returns></returns>
        public DataView GetMonthStatisticalData(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                DataTable dt = dt = MonthData.GetStatisticalData(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo).Table;
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
        /// 获取测点统计数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dtDayAvgData"></param>
        /// <param name="factorList"></param>
        /// <param name="factorField"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        /// <summary>
        /// 获取测点统计数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dtDayAvgData"></param>
        /// <param name="factorList"></param>
        /// <param name="factorField"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataTable GetPortsStatisticalData(string[] portIds, DataTable dtDayAvgData, List<SmartEP.Core.Interfaces.IPollutant> factorList, string[] factorField, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo)
        {
            EQIConcentrationService EQIService = new EQIConcentrationService();

            DataTable dt = new DataTable();
            dt.Columns.Add("PointID");
            dt.Columns.Add("Statistical");
            DataView dvStatistical = GetMonthStatisticalData(portIds, factorList, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo);

            for (int j = 0; j < portIds.Length; j++)
            {
                DataRow rowAvg = dt.NewRow();//平均值
                DataRow rowMax = dt.NewRow(); //最大值             
                DataRow rowMin = dt.NewRow();//最小值              
                DataRow rowSample = dt.NewRow();//样本数
                DataRow rowIAQI = dt.NewRow();//IAQI
                DataRow rowBaseUpper = dt.NewRow();//标准限值
                DataRow rowDayAvg = dt.NewRow();//日均值超标数
                DataRow rowOver = dt.NewRow();//超标率
                DataRow rowMaxOver = dt.NewRow();//最大日平均超标倍数

                rowAvg["PointID"] = portIds[j];
                rowAvg["Statistical"] = "平均值";
                rowMax["PointID"] = portIds[j];
                rowMax["Statistical"] = "最大值";
                rowMin["PointID"] = portIds[j];
                rowMin["Statistical"] = "最小值";
                rowSample["PointID"] = portIds[j];
                rowSample["Statistical"] = "样本数";
                rowIAQI["PointID"] = portIds[j];
                rowIAQI["Statistical"] = "IAQI";
                rowBaseUpper["PointID"] = portIds[j];
                rowBaseUpper["Statistical"] = "标准限值";
                rowDayAvg["PointID"] = portIds[j];
                rowDayAvg["Statistical"] = "日均值超标数";
                rowOver["PointID"] = portIds[j];
                rowOver["Statistical"] = "超标率";
                rowMaxOver["PointID"] = portIds[j];
                rowMaxOver["Statistical"] = "最大日平均超标倍数";
                for (int i = 0; i < factorField.Length; i++)
                {
                    if (j == 0)
                        dt.Columns.Add(factorField[i]);
                    EQIConcentrationLimitEntity limitTwo = new EQIConcentrationLimitEntity();
                    #region 平均值、最大值、最小值、样本数
                    dvStatistical.RowFilter = "PointID=" + portIds[j] + " and PollutantCode='" + factorField[i] + "'";
                    if (dvStatistical.Count > 0)
                    {
                        SmartEP.Core.Interfaces.IPollutant fac = factorList.Where(x => x.PollutantCode.Equals(factorField[i])).FirstOrDefault();
                        int pollutantDecimalNum = fac != null ? Convert.ToInt32(fac.PollutantDecimalNum) : 3;
                        if (dvStatistical[0]["Value_Avg"] != DBNull.Value)
                        {
                            rowAvg[factorField[i]] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dvStatistical[0]["Value_Avg"]), pollutantDecimalNum);//平均值
                        }
                        if (dvStatistical[0]["Value_Max"] != DBNull.Value)
                        {
                            rowMax[factorField[i]] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dvStatistical[0]["Value_Max"]), pollutantDecimalNum);//最大值
                        }
                        if (dvStatistical[0]["Value_Min"] != DBNull.Value)
                        {
                            rowMin[factorField[i]] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dvStatistical[0]["Value_Min"]), pollutantDecimalNum);//最小值
                        }
                        rowSample[factorField[i]] = dvStatistical[0]["Value_Count"];//样本数
                    }
                    limitTwo = EQIService.RetrieveAirConcentration(AQIClass.Moderate, factorField[i], EQITimeType.TwentyFour);
                    #endregion

                    #region 标准限值、日均值超标数
                    double baseValue = 0;
                    if (limitTwo != null)
                    {
                        if (limitTwo.Low != null && limitTwo.Upper != null)
                        {
                            //标准限值
                            rowBaseUpper[factorField[i]] = limitTwo.Low.Value.ToString("0.00") + "~" + limitTwo.Upper.Value.ToString("0.00");

                            //日均值超标数
                            if (dtDayAvgData.Compute("Count(" + factorField[i] + ")", factorField[i] + "<" + limitTwo.Low + " or " + factorField[i] + ">" + limitTwo.Upper) != DBNull.Value)
                                rowDayAvg[factorField[i]] = Convert.ToInt32(dtDayAvgData.Compute("Count(" + factorField[i] + ")", "" + factorField[i] + "<" + limitTwo.Low + " or " + factorField[i] + ">" + limitTwo.Upper));
                            else
                                rowDayAvg[factorField[i]] = 0;
                        }
                        else if (limitTwo.Low != null)
                        {
                            rowBaseUpper[factorField[i]] = limitTwo.Low.Value.ToString("0.00");
                            baseValue = Convert.ToDouble(limitTwo.Low.Value);
                            //日均值超标数
                            if (dtDayAvgData.Compute("Count(" + factorField[i] + ")", factorField[i] + "<" + limitTwo.Low) != DBNull.Value)
                                rowDayAvg[factorField[i]] = Convert.ToInt32(dtDayAvgData.Compute("Count(" + factorField[i] + ")", "" + factorField[i] + "<" + limitTwo.Low));
                            else
                                rowDayAvg[factorField[i]] = 0;
                        }
                        else if (limitTwo.Upper != null)
                        {
                            rowBaseUpper[factorField[i]] = limitTwo.Upper.Value.ToString("0.00");
                            //日均值超标数
                            if (dtDayAvgData.Compute("Count(" + factorField[i] + ")", factorField[i] + ">" + limitTwo.Upper) != DBNull.Value)
                                rowDayAvg[factorField[i]] = Convert.ToInt32(dtDayAvgData.Compute("Count(" + factorField[i] + ")", factorField[i] + ">" + limitTwo.Upper));
                            else
                                rowDayAvg[factorField[i]] = 0;
                        }
                    }
                    else
                    {
                        rowDayAvg[factorField[i]] = 0;
                    }
                    #endregion

                    #region 超标率
                    if (!rowDayAvg[factorField[i]].ToString().Equals("--"))
                    {
                        if (rowSample[factorField[i]] != DBNull.Value && !rowSample[factorField[i]].Equals("0"))
                            rowOver[factorField[i]] = ((Convert.ToDouble(rowDayAvg[factorField[i]]) / Convert.ToDouble(rowSample[factorField[i]]) * 100)).ToString("0.0") + "%";
                        else
                            rowOver[factorField[i]] = "0.0%";
                    }
                    #endregion

                    #region 最大日平均超标倍数
                    if (dtDayAvgData.Compute("Max(" + factorField[i] + ")", "") != DBNull.Value && baseValue != 0)
                    {
                        if ((Convert.ToDouble(dtDayAvgData.Compute("Max(" + factorField[i] + ")", "")) - baseValue) > 0)
                            rowMaxOver[factorField[i]] = (Convert.ToDouble(dtDayAvgData.Compute("Max(" + factorField[i] + ")", "")) - baseValue / baseValue).ToString("0.0");
                        else
                            rowMaxOver[factorField[i]] = "0.00";
                    }
                    else
                        rowMaxOver[factorField[i]] = "0.00";
                    #endregion
                }
                dt.Rows.Add(rowAvg);
                dt.Rows.Add(rowMax);
                dt.Rows.Add(rowMin);
                dt.Rows.Add(rowSample);
                dt.Rows.Add(rowBaseUpper);
                dt.Rows.Add(rowDayAvg);
                dt.Rows.Add(rowOver);
                dt.Rows.Add(rowMaxOver);
            }
            return dt;
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
