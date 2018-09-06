﻿using SmartEP.AMSRepository.Interfaces;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.AutoMonitoring;
using SmartEP.DomainModel.AirAutoMonitoring;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.AMSRepository.Air
{
    /// <summary>
    /// 60分钟实时数据仓储类
    /// </summary>
    public class InfectantBy60Repository : BaseGenericRepository<AirAutoMonitoringModel, InfectantBy60Entity>, IInfectantRepository
    {
        public override bool IsExist(string strKey)
        {
            return true;
        }

        #region << ADO.NET >>
        /// <summary>
        /// 60分钟数据处理DAL类
        /// </summary>
        InfectantDAL m_InfectantDAL = new InfectantDAL(ApplicationType.Air, PollutantDataType.Min60);

        /// <summary>
        /// 5分钟和60分钟处理类
        /// </summary>
        Infectant5Or60AirDAL m_Infectant5Or60AirDAL = new Infectant5Or60AirDAL();

        /// <summary>
        /// 获取该测点接近当前时间的数据(5分钟或60分钟)
        /// </summary>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <param name="dataType">类型</param>
        /// <returns></returns>
        public DataTable GetAirRecentTimeDataBy5Or60(string[] portIds, string[] factors, string dataType)
        {
            return m_Infectant5Or60AirDAL.GetAirRecentTimeDataBy5Or60(portIds, factors, dataType);
        }

        /// <summary>
        /// 获取该测点接近当前时间的数据(5分钟或60分钟)
        /// </summary>
        /// <param name="portIds">测点组</param>
        /// <param name="dataType">类型</param>
        /// <returns></returns>
        public DataTable GetAirRecentTimeDataBy5And60(string[] portIds, string dataType)
        {
            return m_Infectant5Or60AirDAL.GetAirRecentTimeDataBy5And60(portIds, dataType);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_InfectantDAL.GetDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetAvgDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_InfectantDAL.GetAvgDataPagerMin60(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 常规站补充缺失数据时间
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerAllTime(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "time.tstamp desc")
        {
            return m_InfectantDAL.GetDataPagerAllTime(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetDataPagerNew(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_InfectantDAL.GetDataPagerNew(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetVOCsKQYDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_InfectantDAL.GetVOCsKQYDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得VOC外标因子原始小时虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetDataVOCWPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_InfectantDAL.GetDataVOCWPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得VOC外标因子原始小时虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetVOCKQYDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_InfectantDAL.GetDataVOCKQYPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetHourAvgData(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_InfectantDAL.GetHourAvgData(portIds, factors, dateStart, dateEnd, type, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据) NT O3做处理
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetDataPagerForO3(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_InfectantDAL.GetDataPagerForO3(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得所有小时数据（缺失数据全显示）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerForO3AllTime(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "time.tstamp desc")
        {
            return m_InfectantDAL.GetDataPagerForO3AllTime(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得所有小时数据（缺失数据全显示）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerForO3AllTimeNew(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "time.tstamp desc")
        {
            return m_InfectantDAL.GetDataPagerForO3AllTimeNew(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得所有小时数据（缺失数据全显示）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerForO3AllTimeVOCs(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "time.tstamp desc")
        {
            return m_InfectantDAL.GetDataPagerForO3AllTimeVOCs(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 离子色谱仪取得所有小时数据（缺失数据全显示）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerForO3AllTimeLZSPY(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "time.tstamp desc")
        {
            return m_InfectantDAL.GetDataPagerForO3AllTimeLZSPY(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TypeNames"></param>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataTable GetVOCDataAll(string[] TypeNames, DateTime dt, DateTime ds)
        {
            return m_InfectantDAL.GetVOCDataAll(TypeNames, dt, ds);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="TypeNames"></param>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataTable GetVOCDataAll(string unit,string[] TypeNames, DateTime dt, DateTime ds)
        {
            return m_InfectantDAL.GetVOCDataAll(unit,TypeNames, dt, ds);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="TypeNames"></param>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataTable GetVOCWDataAll(string unit, string[] TypeNames, DateTime dt, DateTime ds)
        {
            return m_InfectantDAL.GetVOCWDataAll(unit, TypeNames, dt, ds);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TypeNames"></param>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public DataTable GetVOCDataAll(string[] TypeNames, DateTime dt, DateTime ds,string orderby)
        {
            return m_InfectantDAL.GetVOCDataAll(TypeNames, dt, ds,orderby);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="TypeNames"></param>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public DataTable GetVOCDataAll(string unit,string[] TypeNames, DateTime dt, DateTime ds, string orderby)
        {
            return m_InfectantDAL.GetVOCDataAll(unit,TypeNames, dt, ds, orderby);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,Tstamp")
        {
            return m_InfectantDAL.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
        }

        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public int GetAllDataCount(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_InfectantDAL.GetAllDataCount(portIds, dateStart, dateEnd);
        }

        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            return m_InfectantDAL.GetStatisticalData(portIds, factors, dateStart, dateEnd);
        }

        /// <summary>
        /// 取得指定站点指定时间内最新一条数据
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetLastDataByPort(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_InfectantDAL.GetLastDataByPort(portIds, factors, dtStart, dtEnd);
        }

        /// <summary>
        /// 取得指定因子指定时间范围内最新一条数据
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetLastDataByPollutant(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_InfectantDAL.GetLastDataByPollutant(portIds, factors, dtStart, dtEnd);
        }

        /// <summary>
        /// 返回指定站点、因子的捕获条数
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetPollutantValueCount(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_InfectantDAL.GetPollutantValueCount(portIds, factors, dtStart, dtEnd);
        }
        #endregion

        /// <summary>
        /// 从小时数据表中, 分组统计取得各测点的日均值
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegion"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetDayAvgData(string[] portIds, IList<IPollutant> factors, DateTime dtBegion, DateTime dtEnd)
        {
            return m_InfectantDAL.GetDayAvgData(portIds, factors, dtBegion, dtEnd);
        }

        /// <summary>
        /// 从小时数据表中, 分组统计取得各测点的小时值
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegion"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetDataList(string portIds, DateTime dtTime)
        {
            return m_InfectantDAL.GetDataList(portIds, dtTime);
        }
        public DataView GetDataLists(string[] portIds, DateTime dtBegin, DateTime dtEnd)
        {
            return m_InfectantDAL.GetDataLists(portIds, dtBegin, dtEnd);
        }
        public DataView GetHourDatas(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd, string orderBy = "PointId")
        {
            return m_InfectantDAL.GetHourDatas(portIds, factors, dtStart, dtEnd);
        }
        public void GetAddData(string pointId, string PointAQM, string FileName, DateTime tstamp, DateTime dtTime, string Number, int RecordNumber, int XMLDBF)
        {
            m_InfectantDAL.GetAddData(pointId, PointAQM, FileName, tstamp, dtTime, Number, RecordNumber, XMLDBF);
        }
        public void GetUpdateData(string pointId, DateTime tstamp, int RecordNumber)
        {
            m_InfectantDAL.GetUpdateData(pointId, tstamp, RecordNumber);
        }
        /// <summary>
        /// 常规站补充缺失数据时间(包括O3-8因子)
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerAllTimeWithO8(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "time.tstamp desc")
        {
          return m_InfectantDAL.GetDataPagerAllTimeWithO8(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 常规站补充缺失数据时间(包括O3-8因子)
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerAllTimeWithO8Region(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          return m_InfectantDAL.GetDataPagerAllTimeWithO8Region(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
    }
}
