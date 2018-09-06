﻿using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.MonitoringBusinessRepository.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.BaseData.Standard;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.Core.Enums;
using SmartEP.DomainModel.MonitoringBusiness;

namespace SmartEP.Service.DataAnalyze.Air.DataQuery
{
    /// <summary>
    /// 名称：DataQueryByHourService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：审核小时数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataQueryByHourService
    {
        /// <summary>
        /// 空气小时数据
        /// </summary>
        HourReportRepository HourData = Singleton<HourReportRepository>.GetInstance();

        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;

        /// <summary>
        /// 取得测点名称
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <returns></returns>
        public DataView GetPointName(string portIds)
        {
            return HourData.GetPointName(portIds);
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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetHourDataPager(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp,PointId")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetNewHourDataPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp,PointId")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetNewDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetNewHourDataPagerAvg(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp,PointId")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetNewDataPagerAvg(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetHourDataAvg(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp,PointId")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetHourDataAvg(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetNewHourDataPager(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            if (factors.IsNotNullOrDBNull())
                return HourData.GetNewDataPager(portIds, factors, dtStart, dtEnd);
            return null;
        }

        /// <summary>
        /// 取得全月小时数
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetAvgDayData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            if (factors.IsNotNullOrDBNull())
                return HourData.GetAvgDayData(portIds, factors, dateStart, dateEnd);
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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetHourDataPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
         , int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp,PointId")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetHourDataPagerNew(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
         , int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp,PointId")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetDataPagerNew(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetVOCsKQYHourDataPagerNew(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
         , int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp,PointId")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetVOCsKQYDataPagerNew(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataHourPager(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,
            DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,tstamp desc")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetDataHourPager(portIds, factors, dtBegin, dtEnd, dtFrom, dtTo, type, pageSize, pageNo, out recordTotal, orderBy);
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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetHourData(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,
            DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,tstamp desc,Type")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetDataPager(portIds, factors, dtBegin, dtEnd, dtFrom, dtTo, type, pageSize, pageNo, out recordTotal, orderBy);
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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetHourDataDF(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,
             string type, int pageSize, int pageNo, string orderBy = "PointId,tstamp desc,Type")
        {
            
            if (factors.IsNotNullOrDBNull())
                return HourData.GetDataPagerDF(portIds, factors, dtBegin, dtEnd,  type, pageSize, pageNo, orderBy);
            return null;
        }
        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetHourStatisticalData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                DataTable dt = HourData.GetStatisticalData(portIds, factors.Select(p => p.PollutantCode).ToArray(), dateStart, dateEnd).Table;
                dt.Columns.Add("portName", typeof(string)).SetOrdinal(0);
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
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetHourStatisticalData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                DataTable dt = HourData.GetStatisticalData(portIds, factors, dateStart, dateEnd).Table;
                dt.Columns.Add("portName", typeof(string)).SetOrdinal(0);
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
        /// 取得统计数据 按日分组（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetHourStatisticalDataByDay(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                DataTable dt = HourData.GetStatisticalDataByDay(portIds, factors, dateStart, dateEnd).Table;
                return dt.DefaultView;
            }
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
        public DataTable GetPortsStatisticalData(string[] portIds, List<SmartEP.Core.Interfaces.IPollutant> factorList, string[] factorField, DateTime dtStart, DateTime dtEnd)
        {
            EQIConcentrationService EQIService = new EQIConcentrationService();

            DataTable dt = new DataTable();
            dt.Columns.Add("PointID");
            dt.Columns.Add("Statistical");
            DataView dvStatistical = GetHourStatisticalData(portIds, factorList, dtStart, dtEnd);

            for (int j = 0; j < portIds.Length; j++)
            {
                DataRow rowAvg = dt.NewRow();//平均值
                DataRow rowMax = dt.NewRow(); //最大值             
                DataRow rowMin = dt.NewRow();//最小值              
                DataRow rowSample = dt.NewRow();//样本数
                DataRow rowBaseUpper = dt.NewRow();//标准限值

                rowAvg["PointID"] = portIds[j];
                rowAvg["Statistical"] = "平均值";
                rowMax["PointID"] = portIds[j];
                rowMax["Statistical"] = "最大值";
                rowMin["PointID"] = portIds[j];
                rowMin["Statistical"] = "最小值";
                rowSample["PointID"] = portIds[j];
                rowSample["Statistical"] = "样本数";
                rowBaseUpper["PointID"] = portIds[j];
                rowBaseUpper["Statistical"] = "标准限值";
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
                            rowAvg[factorField[i]] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dvStatistical[0]["Value_Avg"]), pollutantDecimalNum); ;//平均值
                        }
                        if (dvStatistical[0]["Value_Max"] != DBNull.Value)
                        {
                            //rowMax[factorField[i]] = dvStatistical[0]["Value_Max"];//最大值
                            rowMax[factorField[i]] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dvStatistical[0]["Value_Max"]), pollutantDecimalNum);
                        }
                        if (dvStatistical[0]["Value_Min"] != DBNull.Value)
                        {
                            //rowMin[factorField[i]] = dvStatistical[0]["Value_Min"];//最小值
                            rowMin[factorField[i]] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dvStatistical[0]["Value_Min"]), pollutantDecimalNum);
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
                        }
                        else if (limitTwo.Low != null)
                        {
                            rowBaseUpper[factorField[i]] = limitTwo.Low.Value.ToString("0.00");
                            baseValue = Convert.ToDouble(limitTwo.Low.Value);
                        }
                        else if (limitTwo.Upper != null)
                        {
                            rowBaseUpper[factorField[i]] = limitTwo.Upper.Value.ToString("0.00");
                        }
                    }
                    #endregion
                }
                dt.Rows.Add(rowAvg);
                dt.Rows.Add(rowMax);
                dt.Rows.Add(rowMin);
                dt.Rows.Add(rowSample);
                dt.Rows.Add(rowBaseUpper);
            }
            return dt;
        }


        /// <summary>
        /// 取得全部查询数据(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetHourExportData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string orderBy = "Tstamp,PointId")
        {
            if (HourData != null)
                return HourData.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
            return null;
        }
        /// <summary>
        /// 取得全部查询数据(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetNewHourExportData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string orderBy = "Tstamp,PointId")
        {
            if (HourData != null)
                return HourData.GetNewExportData(portIds, factors, dateStart, dateEnd, orderBy);
            return null;
        }
        /// <summary>
        /// 环境空气质量自动监测数据报表导出
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetAQAutoMonthReportExportData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string orderBy = "Tstamp,PointId")
        {
            if (HourData != null)
                return HourData.GetAQAutoMonthReportExportData(portIds, factors, dateStart, dateEnd, orderBy);
            return null;
        }

        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public int GetHourAllDataCount(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            if (HourData != null)
                return HourData.GetAllDataCount(portIds, dateStart, dateStart);
            return 0;
        }

        #region 获取时间点下的测点，因子数据
        /// <summary>
        /// 获取时间点下的测点，因子数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DateLists">监测日期数组</param>
        /// <returns></returns>
        public DataTable GetHourData(List<int> PointIds, List<string> PollutantCodes, List<DateTime> DateLists)
        {
            return HourData.GetHourData(PointIds, PollutantCodes, DateLists);
        }
        #endregion

        #region 捕获有效时数
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetCaptureDataPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, bool IsOrNotexport = false)
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataView dv = new DataView();

            DataTable newdtb = new DataTable();

            dv = HourData.GetCaptureDataPager(portIds, factors, dtStart, dtEnd);  // 本期

            int timeCount = ((dtEnd - dtStart).Days + 1) * 24 * factors.Length;
            DataTable dt = dv.ToTable();   //本期
            string PointName = "";
            for (int i = 0; i < portIds.Length; i++)
            {
                if (portIds[i] != "")
                {
                    PointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;
                    newdtb.Columns.Add(PointName, typeof(string));
                }
            }
            newdtb.Columns.Add("数据类型", typeof(string)).SetOrdinal(0);

            DataRow[] Rowdt;
            for (int i = 0; i < 3; i++)
            {
                DataRow newRow = newdtb.NewRow();
                newdtb.Rows.Add(newRow);
            }
            newdtb.Rows[0]["数据类型"] = "有效数据捕获率";
            newdtb.Rows[1]["数据类型"] = "有效数据";
            newdtb.Rows[2]["数据类型"] = "应测数据";
            for (int i = 0; i < portIds.Length; i++)
            {
                decimal count = 0;

                PointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;

                Rowdt = dt.Select("PointId='" + portIds[i] + "'");

                if (Rowdt.Length > 0)
                {
                    for (int j = 0; j < factors.Length; j++)
                    {
                        count += Convert.ToDecimal(Rowdt[0][factors[j]]);
                    }
                }
                if (IsOrNotexport)
                {
                    newdtb.Rows[0][PointName] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                    newdtb.Rows[1][PointName] = count;
                    newdtb.Rows[2][PointName] = timeCount;
                }
                else
                {
                    newdtb.Rows[0][PointName] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                    newdtb.Rows[1][PointName] = count;
                    newdtb.Rows[2][PointName] = timeCount;
                    //newRow[PointName] = DecimalExtension.GetRoundValue(count / timeCount * 100, 1).ToString() + "%<br/>" + count + ":" + timeCount;
                }
            }
            return newdtb.DefaultView;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetSuperCaptureDataPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, bool IsOrNotexport = false)
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataView dv = new DataView();
            DataView dvty = null;
            DataTable newdtb = new DataTable();
            newdtb.Columns.Add("数据类型", typeof(string)).SetOrdinal(0);
            dv = HourData.GetSuperCaptureDataPager(portIds, factors, dtStart, dtEnd);  // 本期
            DataTable dt = dv.ToTable();   //本期
            DataTable dtCate = HourData.GetCategoryUidByPollutantCode(factors).ToTable();
            for (int i = 0; i < dtCate.Rows.Count; i++)
            {
                newdtb.Columns.Add(dtCate.Rows[i]["CategoryUid"].ToString(), typeof(string));
            }
            if (factors.Contains("a90162"))
            {
                string[] fac = new string[] { "a05024" };
                dvty = HourData.GetSuperCaptureDataPager(portIds, fac, dtStart, dtEnd);
            }
            DataRow[] Rowdt;
            for (int i = 0; i < 3; i++)
            {
                DataRow newRow = newdtb.NewRow();
                newdtb.Rows.Add(newRow);
            }
            newdtb.Rows[0]["数据类型"] = "有效数据捕获率";
            newdtb.Rows[1]["数据类型"] = "实测数据";
            newdtb.Rows[2]["数据类型"] = "应测数据";
            for (int i = 0; i < dtCate.Rows.Count; i++)
            {
                decimal count = 0;
                int x = 0;
                DataView dvC = HourData.GetPollutantCodeByUid(dtCate.Rows[i]["CategoryUid"].ToString());
                string[] dvArr = dtToArr(dvC.ToTable());
                for (int m = 0; m < factors.Count(); m++)
                {
                    if (dvArr.Contains(factors[m]))
                    {
                        x += 1;
                    }
                }
                int timeCount = ((dtEnd - dtStart).Days + 1) * 24 * x;
                //dt.Rows.Contains(dtCate.Rows[i]["CategoryUid"].ToString())
                //if (1==1)
                //{
                if (dtCate.Rows[i]["CategoryUid"].ToString() == "aabe91e0-29a4-427c-becc-0b29f1224422")
                {
                    if (dvty != null && dvty.ToTable().Rows.Count>0)
                    {
                        count += Convert.ToDecimal(dvty.ToTable().Rows[0]["a05024"].ToString());
                    }
                }
                Rowdt = dt.Select("CategoryUid='" + dtCate.Rows[i]["CategoryUid"].ToString() + "'");
                if (Rowdt.Length > 0)
                {
                    for (int j = 0; j < factors.Length; j++)
                    {
                        count += Convert.ToDecimal(Rowdt[0][factors[j]]);
                    }
                }
                if (IsOrNotexport)
                {
                    newdtb.Rows[0][dtCate.Rows[i]["CategoryUid"].ToString()] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                    newdtb.Rows[1][dtCate.Rows[i]["CategoryUid"].ToString()] = count;
                    newdtb.Rows[2][dtCate.Rows[i]["CategoryUid"].ToString()] = timeCount;
                }
                else
                {
                    newdtb.Rows[0][dtCate.Rows[i]["CategoryUid"].ToString()] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                    newdtb.Rows[1][dtCate.Rows[i]["CategoryUid"].ToString()] = count;
                    newdtb.Rows[2][dtCate.Rows[i]["CategoryUid"].ToString()] = timeCount;
                }
                //}
                //else
                //{
                //    //Rowdt = dt.Select("CategoryUid='" + dtCate.Rows[i]["CategoryUid"].ToString() + "'");
                //    //if (Rowdt.Length > 0)
                //    //{
                //    //    for (int j = 0; j < factors.Length; j++)
                //    //    {
                //    //        count += Convert.ToDecimal(Rowdt[0][factors[j]]);
                //    //    }
                //    //}
                //    count = 0;
                //    if (IsOrNotexport)
                //    {
                //        newdtb.Rows[0][dt.Rows[i]["CategoryUid"].ToString()] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                //        newdtb.Rows[1][dt.Rows[i]["CategoryUid"].ToString()] = count;
                //        newdtb.Rows[2][dt.Rows[i]["CategoryUid"].ToString()] = timeCount;
                //    }
                //    else
                //    {
                //        newdtb.Rows[0][dt.Rows[i]["CategoryUid"].ToString()] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                //        newdtb.Rows[1][dt.Rows[i]["CategoryUid"].ToString()] = count;
                //        newdtb.Rows[2][dt.Rows[i]["CategoryUid"].ToString()] = timeCount;
                //    }
                //}

            }
            for (int i = 0; i < dtCate.Rows.Count; i++)
            {
                DataTable dtName = HourData.GetCategory(dtCate.Rows[i]["CategoryUid"].ToString()).ToTable();

                newdtb.Columns[dtCate.Rows[i]["CategoryUid"].ToString()].ColumnName = dtName.Rows[0]["category"].ToString();
            }
            return newdtb.DefaultView;
        }
        #endregion

        
        
        #region 有效率有效时数
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetEffectiveData(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, bool IsOrNotexport = false)
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataView dv = new DataView();

            DataTable newdtb = new DataTable();
            string PointName = "";
            for (int i = 0; i < portIds.Length; i++)
            {
                if (portIds[i] != "")
                {
                    PointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;
                    newdtb.Columns.Add(PointName, typeof(string));
                }
            }
            newdtb.Columns.Add("数据类型", typeof(string)).SetOrdinal(0);

            dv = HourData.GetEffectiveData(portIds, factors, dtStart, dtEnd);  // 本期

            int timeCount = ((dtEnd - dtStart).Days + 1) * 24 * factors.Length;
            DataTable dt = dv.ToTable();   //本期
            DataRow[] Rowdt;
            for (int i = 0; i < 3; i++)
            {
                DataRow newRow = newdtb.NewRow();
                newdtb.Rows.Add(newRow);
            }
            newdtb.Rows[0]["数据类型"] = "数据捕获率";
            newdtb.Rows[1]["数据类型"] = "实测数据";
            newdtb.Rows[2]["数据类型"] = "应测数据";
            for (int i = 0; i < portIds.Length; i++)
            {
                decimal count = 0;

                PointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;

                Rowdt = dt.Select("PointId='" + portIds[i] + "'");

                if (Rowdt.Length > 0)
                {
                    for (int j = 0; j < factors.Length; j++)
                    {
                        count += Convert.ToDecimal(Rowdt[0][factors[j]]);
                    }
                }
                if (IsOrNotexport)
                {
                    newdtb.Rows[0][PointName] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString()+"%";
                    newdtb.Rows[1][PointName] =count;
                    newdtb.Rows[2][PointName] = timeCount;
                }
                else
                {
                    newdtb.Rows[0][PointName] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                    newdtb.Rows[1][PointName] = count;
                    newdtb.Rows[2][PointName] = timeCount;
                  //  newRow[PointName] = DecimalExtension.GetRoundValue(count / timeCount * 100, 1).ToString() + "%<br/>" + count + ":" + timeCount;
                }
            }
            return newdtb.DefaultView;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetSuperEffectiveData(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, bool IsOrNotexport = false)
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataView dv = new DataView();
            DataView dvty = null;
            DataTable newdtb = new DataTable();
            newdtb.Columns.Add("数据类型", typeof(string)).SetOrdinal(0);
            dv = HourData.GetSuperEffectiveData(portIds, factors, dtStart, dtEnd);  // 本期
            if (factors.Contains("a90162"))
            {
                string[] fac=new string[] { "a05024" };
                dvty = HourData.GetSuperEffectiveData(portIds, fac, dtStart, dtEnd);
            }
            DataTable dt = dv.ToTable();   //本期
            DataTable dtCate = HourData.GetCategoryUidByPollutantCode(factors).ToTable();
            for (int i = 0; i < dtCate.Rows.Count; i++)
            {
                newdtb.Columns.Add(dtCate.Rows[i]["CategoryUid"].ToString(), typeof(string));
            }
            DataRow[] Rowdt;
            for (int i = 0; i < 3; i++)
            {
                DataRow newRow = newdtb.NewRow();
                newdtb.Rows.Add(newRow);
            }
            newdtb.Rows[0]["数据类型"] = "数据捕获率";
            newdtb.Rows[1]["数据类型"] = "实测数据";
            newdtb.Rows[2]["数据类型"] = "应测数据";
            for (int i = 0; i < dtCate.Rows.Count; i++)
            {
                decimal count = 0;
                int x = 0;
                DataView dvC = HourData.GetPollutantCodeByUid(dtCate.Rows[i]["CategoryUid"].ToString());
                string[] dvArr = dtToArr(dvC.ToTable());
                for (int m = 0; m < factors.Count();m++ )
                {
                    if (dvArr.Contains(factors[m]))
                    {
                        x += 1;
                    }
                }
                int timeCount = ((dtEnd - dtStart).Days + 1) * 24 * x;
                Rowdt = dt.Select("CategoryUid='" + dtCate.Rows[i]["CategoryUid"].ToString() + "'");
                if (dtCate.Rows[i]["CategoryUid"].ToString() == "aabe91e0-29a4-427c-becc-0b29f1224422")
                {
                    if (dvty != null && dvty.ToTable().Rows.Count > 0)
                    {
                        count += Convert.ToDecimal(dvty.ToTable().Rows[0]["a05024"].ToString());
                    }
                }
                if (Rowdt.Length > 0)
                {
                    for (int j = 0; j < factors.Length; j++)
                    {
                        count += Convert.ToDecimal(Rowdt[0][factors[j]]);
                    }
                }
                if (IsOrNotexport)
                {
                    newdtb.Rows[0][dtCate.Rows[i]["CategoryUid"].ToString()] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                    newdtb.Rows[1][dtCate.Rows[i]["CategoryUid"].ToString()] = count;
                    newdtb.Rows[2][dtCate.Rows[i]["CategoryUid"].ToString()] = timeCount;
                }
                else
                {
                    newdtb.Rows[0][dtCate.Rows[i]["CategoryUid"].ToString()] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                    newdtb.Rows[1][dtCate.Rows[i]["CategoryUid"].ToString()] = count;
                    newdtb.Rows[2][dtCate.Rows[i]["CategoryUid"].ToString()] = timeCount;
                }
                //}
                //else
                //{
                //    //Rowdt = dt.Select("CategoryUid='" + dtCate.Rows[i]["CategoryUid"].ToString() + "'");
                //    //if (Rowdt.Length > 0)
                //    //{
                //    //    for (int j = 0; j < factors.Length; j++)
                //    //    {
                //    //        count += Convert.ToDecimal(Rowdt[0][factors[j]]);
                //    //    }
                //    //}
                //    count = 0;
                //    if (IsOrNotexport)
                //    {
                //        newdtb.Rows[0][dt.Rows[i]["CategoryUid"].ToString()] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                //        newdtb.Rows[1][dt.Rows[i]["CategoryUid"].ToString()] = count;
                //        newdtb.Rows[2][dt.Rows[i]["CategoryUid"].ToString()] = timeCount;
                //    }
                //    else
                //    {
                //        newdtb.Rows[0][dt.Rows[i]["CategoryUid"].ToString()] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                //        newdtb.Rows[1][dt.Rows[i]["CategoryUid"].ToString()] = count;
                //        newdtb.Rows[2][dt.Rows[i]["CategoryUid"].ToString()] = timeCount;
                //    }
                //}
                
            }
            for (int i = 0; i < dtCate.Rows.Count; i++)
            {
                DataTable dtName = HourData.GetCategory(dtCate.Rows[i]["CategoryUid"].ToString()).ToTable();

                newdtb.Columns[dtCate.Rows[i]["CategoryUid"].ToString()].ColumnName = dtName.Rows[0]["category"].ToString();
            }
            return newdtb.DefaultView;
        }
        /// <summary>
        /// DataTable转换为一维字符串数组
        /// </summary>
        /// <returns></returns>
        public static string[] dtToArr(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new string[0];
            }
            else
            {
                string[] sr = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.IsDBNull(dt.Rows[i][0]))
                    {
                        sr[i] = "";
                    }
                    else
                    {
                        sr[i] = dt.Rows[i][0] + "";
                    }
                }
                return sr;
            }
        }
        #endregion
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetCaptureDataPagerNew(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, bool IsOrNotexport = false)
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataView dv = new DataView();

            DataTable newdtb = new DataTable();

            dv = HourData.GetCaptureDataPagerNew(portIds, factors, dtStart, dtEnd);  // 本期
            //int timeCount = ((dtEnd - dtStart).Days + 1) * 24 * factors.Length;
            DataTable dt = dv.ToTable();   //本期
            string PointName = "";

            for (int j = 0; dtStart.AddMonths(j)<dtEnd.AddSeconds(1); j++)
            {
                newdtb.Columns.Add(dtStart.AddMonths(j).ToString("yyyy-MM"), typeof(string));

            }
            newdtb.Columns.Add("有效数据捕获率", typeof(string)).SetOrdinal(0);
            DataRow[] Rowdt;
            for (int i = 0; i < portIds.Length; i++)
            {
                if (portIds[i] != "")
                {
                    PointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;
                    //newdtb.Columns.Add(PointName, typeof(string));
                    DataRow newRow = newdtb.NewRow();
                    newdtb.Rows.Add(newRow);
                }
                newdtb.Rows[i]["有效数据捕获率"] = PointName;
                for (int k = 0; dtStart.AddMonths(k) < dtEnd.AddSeconds(1); k++)
                {
                    int timeCount = ((dtStart.AddMonths(k + 1).AddSeconds(-1) - dtStart.AddMonths(k)).Days + 1) * 24 * factors.Length;
                    decimal count = 0;
                    Rowdt = dt.Select("PointId='" + portIds[i] + "' and Tst='" + dtStart.AddMonths(k).ToString("yyyy-MM") + "'");
                    if (Rowdt.Length > 0)
                    {
                        for (int j = 0; j < factors.Length; j++)
                        {
                            count += Convert.ToDecimal(Rowdt[0][factors[j]]);
                        }
                    }
                    if (IsOrNotexport)
                    {
                        newdtb.Rows[i][dtStart.AddMonths(k).ToString("yyyy-MM")] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";

                    }
                    else
                    {
                        newdtb.Rows[i][dtStart.AddMonths(k).ToString("yyyy-MM")] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                    }
                }
                
            }
            return newdtb.DefaultView;
        }

        /// <summary>
        /// 考核数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetCheckNew(string[] portIds, DateTime dtStart, DateTime dtEnd, bool IsOrNotexport = false)
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataView dv = new DataView();

            DataTable newdtb = new DataTable();

            dv = HourData.GetCheckNew(portIds, dtStart, dtEnd);  // 本期
            DataTable dt = dv.ToTable();   //本期
            string PointName = "";
            string[] ObjectGuid = new string[8] { "2854FF1F-CECE-4671-9FA4-C2FE218C97DD", "9FDBCF2D-AD77-48D0-82D2-1047D911101D", "7BC6D72A-EFA9-473A-B0FB-8403A191B8D4", "06B15D01-E2AD-4985-932D-A4CA5B0E6D37", "ED562DBA-F87B-4F20-B206-E80DF71B47D5", "61E458B2-3809-4886-B835-6FED72E0840B", "65BF2591-F9BD-461F-B2B2-BA614F2BDF02", "B6A57CD4-AF2F-458E-B6A8-55F14E91A5D2" };
            for (int j = 0; dtStart.AddMonths(j) < dtEnd.AddSeconds(1); j++)
            {
                newdtb.Columns.Add(dtStart.AddMonths(j).ToString("yyyy-MM"), typeof(string));

            }
            newdtb.Columns.Add("考核要求", typeof(string)).SetOrdinal(0);
            DataRow[] Rowdt;
            for (int i = 0; i < portIds.Length; i++)
            {
                if (portIds[i] != "")
                {
                    PointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;
                    //newdtb.Columns.Add(PointName, typeof(string));
                    DataRow newRow = newdtb.NewRow();
                    newdtb.Rows.Add(newRow);
                }
                newdtb.Rows[i]["考核要求"] = PointName;
                for (int k = 0; dtStart.AddMonths(k) < dtEnd.AddSeconds(1); k++)
                {
                    
                    Rowdt = dt.Select("MaintenanceObjectGuid='" + ObjectGuid[i] + "' and Tst='" + dtStart.AddMonths(k).ToString("yyyy-MM") + "'");
                    
                    if (IsOrNotexport)
                    {
                        //newdtb.Rows[i][dtStart.AddMonths(k).ToString("yyyy-MM")] = 

                    }
                    else
                    {
                        //newdtb.Rows[i][dtStart.AddMonths(k).ToString("yyyy-MM")] = 
                    }
                }

            }
            return newdtb.DefaultView;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetEffectiveDataNew(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, bool IsOrNotexport = false)
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataView dv = new DataView();

            DataTable newdtb = new DataTable();
            
            dv = HourData.GetEffectiveDataNew(portIds, factors, dtStart, dtEnd);  // 本期
            DataTable dt = dv.ToTable();   //本期
            string PointName = "";
            for (int j = 0; dtStart.AddMonths(j) < dtEnd.AddSeconds(1); j++)
            {
                newdtb.Columns.Add(dtStart.AddMonths(j).ToString("yyyy-MM"), typeof(string));

            }
            newdtb.Columns.Add("子站数据运行率", typeof(string)).SetOrdinal(0);
            DataRow[] Rowdt;
            for (int i = 0; i < portIds.Length; i++)
            {
                if (portIds[i] != "")
                {
                    PointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;
                    //newdtb.Columns.Add(PointName, typeof(string));
                    DataRow newRow = newdtb.NewRow();
                    newdtb.Rows.Add(newRow);
                }
                newdtb.Rows[i]["子站数据运行率"] = PointName;
                for (int k = 0; dtStart.AddMonths(k) < dtEnd.AddSeconds(1); k++)
                {
                    int timeCount = ((dtStart.AddMonths(k + 1).AddSeconds(-1) - dtStart.AddMonths(k)).Days + 1) * 24 * factors.Length;
                    decimal count = 0;
                    Rowdt = dt.Select("PointId='" + portIds[i] + "' and Tst='" + dtStart.AddMonths(k).ToString("yyyy-MM") + "'");
                    if (Rowdt.Length > 0)
                    {
                        for (int j = 0; j < factors.Length; j++)
                        {
                            count += Convert.ToDecimal(Rowdt[0][factors[j]]);
                        }
                    }
                    if (IsOrNotexport)
                    {
                        newdtb.Rows[i][dtStart.AddMonths(k).ToString("yyyy-MM")] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";

                    }
                    else
                    {
                        newdtb.Rows[i][dtStart.AddMonths(k).ToString("yyyy-MM")] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
                    }
                }

            }
            return newdtb.DefaultView;
        }
            




            //int timeCount = ((dtEnd - dtStart).Days + 1) * 24 * factors.Length;

            //DataRow[] Rowdt;
            //for (int i = 0; i < 3; i++)
            //{
            //    DataRow newRow = newdtb.NewRow();
            //    newdtb.Rows.Add(newRow);
            //}
            //newdtb.Rows[0]["数据类型"] = "系统正常运行率";
            ////newdtb.Rows[1]["数据类型"] = "实测数据";
            ////newdtb.Rows[2]["数据类型"] = "应测数据";
            //for (int i = 0; i < portIds.Length; i++)
            //{
            //    decimal count = 0;

            //    PointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;

            //    Rowdt = dt.Select("PointId='" + portIds[i] + "'");

            //    if (Rowdt.Length > 0)
            //    {
            //        for (int j = 0; j < factors.Length; j++)
            //        {
            //            count += Convert.ToDecimal(Rowdt[0][factors[j]]);
            //        }
            //    }
            //    if (IsOrNotexport)
            //    {
            //        newdtb.Rows[0][PointName] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
            //    }
            //    else
            //    {
            //        newdtb.Rows[0][PointName] = DecimalExtension.GetRoundValue(count / timeCount * 100, 2).ToString() + "%";
            //    }
            //}



        public DataView GetNewHourDataPagerWidthO8(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
          recordTotal = 0;
          if (factors.IsNotNullOrDBNull())
            return HourData.GetNewHourDataPagerWidthO8(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
          return null;
        }
        public DataView GetNewHourDataPagerWidthRegionO8(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "sortNumber desc,Tstamp")
        {
          recordTotal = 0;
          if (factors.IsNotNullOrDBNull())
            return HourData.GetNewHourDataPagerWidthRegionO8(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
          return null;
        }
      /// <summary>
      /// 根据pointId获取区域
      /// </summary>
      /// <param name="PointID"></param>
      /// <returns></returns>
        public DataTable GetRegionWithPointId(string[] PointID)
        {
          return HourData.GetRegionWithPointId(PointID);
        }
    }
}
