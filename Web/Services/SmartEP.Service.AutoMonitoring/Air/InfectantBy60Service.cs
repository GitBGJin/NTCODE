﻿using SmartEP.AMSRepository.Air;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.AirAutoMonitoring;
using SmartEP.Service.AutoMonitoring.Interfaces;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Air
{
    /// <summary>
    /// 名称：InfectantBy60Service.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 60分钟数据服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InfectantBy60Service : IMonitoringData, IInfectantDALService
    {
        /// <summary>
        /// 60分钟数据仓储层
        /// </summary>
        InfectantBy60Repository g_InfectantBy60Repository = Singleton<InfectantBy60Repository>.GetInstance();

        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;

        #region << ORM >>
        /// <summary>
        /// 获取监测数据
        /// </summary>
        /// <param name="portId">站点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="pollutantCodes">因子数组</param>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveMonitoringData(System.Int32 portId, DateTime startTime, DateTime endTime, List<string> pollutantCodes)
        {
            return g_InfectantBy60Repository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp >= startTime && x.Tstamp <= endTime && pollutantCodes.Contains(x.PollutantCode));
        }

        /// <summary>
        /// 追加监测数据
        /// </summary>
        /// <param name="baseEntity">60分钟model实体</param>
        public void SaveMonitoringData(IBaseEntityProperty baseEntity)
        {
            g_InfectantBy60Repository.Add(baseEntity);
        }

        /// <summary>
        /// 监测数据是否存在
        /// </summary>
        /// <param name="portId">测点</param>
        /// <param name="tstamp">时间</param>
        /// <param name="pollutantCode">因子</param>
        /// <returns></returns>
        public bool IsExist(int portId, DateTime tstamp, string pollutantCode)
        {
            return g_InfectantBy60Repository.RetrieveCount(x => x.PointId.Equals(portId) && x.Tstamp.Equals(tstamp) && x.PollutantCode.Equals(pollutantCode)) == 0;
        }

        /// <summary>
        /// 更新监测数据
        /// <summary>
        public void UpdateMonitoringData()
        {
            g_InfectantBy60Repository.SaveChanges();
        }

        /// <summary>
        /// 根据时间段删除监测数据
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        public void DeleteMonitoringData(DateTime startTime, DateTime endTime)
        {
            IQueryable<InfectantBy60Entity> query = g_InfectantBy60Repository.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime);
            if (query != null && query.Count() > 0) g_InfectantBy60Repository.BatchDelete(query.ToList<InfectantBy60Entity>());
        }

        /// <summary>
        /// 根据监测点删除监测数据
        /// </summary>
        /// <param name="portId">测点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        public void DeleteMonitoringData(int portId, DateTime startTime, DateTime endTime)
        {
            IQueryable<InfectantBy60Entity> query = g_InfectantBy60Repository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp >= startTime && x.Tstamp <= endTime);
            if (query != null && query.Count() > 0) g_InfectantBy60Repository.BatchDelete(query.ToList<InfectantBy60Entity>());
        }

        /// <summary>
        /// 根据污染物获取平均值
        /// </summary>
        /// <param name="portId">测点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="pollutantCode">因子</param>
        /// <returns></returns>
        public decimal RetrieveAvgMonitoringDataByPollutant(int portId, DateTime startTime, DateTime endTime, string pollutantCode)
        {
            return TypeConversionExtensions.TryTo<decimal?, decimal>(g_InfectantBy60Repository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp >= startTime && x.Tstamp <= endTime).Average<InfectantBy60Entity>(x => x.PollutantValue));
        }

        /// <summary>
        /// 根据污染物获取最大值
        /// </summary>
        /// <param name="portId">测点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="pollutantCode">因子</param>
        /// <returns></returns>
        public decimal RetrieveMaxMonitoringDataByPollutant(int portId, DateTime startTime, DateTime endTime, string pollutantCode)
        {
            return TypeConversionExtensions.TryTo<decimal?, decimal>(g_InfectantBy60Repository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp >= startTime && x.Tstamp <= endTime).Max<InfectantBy60Entity>(x => x.PollutantValue));
        }

        /// <summary>
        /// 根据污染物获取最小值
        /// </summary>
        /// <param name="portId">测点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="pollutantCode">因子</param>
        /// <returns></returns>
        public decimal RetrieveMinMonitoringDataByPollutant(int portId, DateTime startTime, DateTime endTime, string pollutantCode)
        {
            return TypeConversionExtensions.TryTo<decimal?, decimal>(g_InfectantBy60Repository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp >= startTime && x.Tstamp <= endTime).Min<InfectantBy60Entity>(x => x.PollutantValue));
        }

        /// <summary>
        /// 获取最新一条监测数据
        /// </summary>
        /// <param name="portId">测点</param>
        /// <param name="pollutantCodes">因子</param>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveNewestMonitoringData(int portId, List<string> pollutantCodes)
        {
            InfectantBy60Entity entity = g_InfectantBy60Repository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp <= DateTime.Now).OrderByDescending(x => x.Tstamp).FirstOrDefault();
            if (entity != null)
                return g_InfectantBy60Repository.Retrieve(x => x.PointId.Equals(portId) && entity.Tstamp.Equals(x.Tstamp) && pollutantCodes.Contains(x.PollutantCode));
            return null;
        }

        /// <summary>
        /// 获取最新N条监测数据
        /// </summary>
        /// <param name="portId">测点</param>
        /// <param name="pollutantCodes">因子数组</param>
        /// <param name="number">条数</param>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveNewestMonitoringData(int portId, List<string> pollutantCodes, int number)
        {
            IQueryable<DateTime> dateQuery = g_InfectantBy60Repository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp <= DateTime.Now).GroupBy(x => x.Tstamp).OrderByDescending(x => x.Key).Select(x => x.Key).Take(number);
            if (dateQuery != null && dateQuery.Count() > 0)
            {
                DateTime dateMin = dateQuery.Last();
                return g_InfectantBy60Repository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp >= dateMin && pollutantCodes.Contains(x.PollutantCode));
            }
            return null;
        }

        /// <summary>
        /// 根据污染物获取记录数
        /// </summary>
        /// <param name="portId">测点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="pollutantCode">因子</param>
        /// <returns></returns>
        public int RetrieveMonitoringDataRecordsByPollutant(int portId, DateTime startTime, DateTime endTime, string pollutantCode)
        {
            return g_InfectantBy60Repository.RetrieveCount(x => x.PointId.Equals(portId) && x.Tstamp >= startTime && x.Tstamp <= endTime);
        }
        #endregion

        #region << ADO.NET >>
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
        public DataView GetDataPager(string[] portIds, IList<IPollutant> factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetDataPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetAvgDataPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetAvgDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 常规站补充缺失数据时间
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerAllTime(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetDataPagerAllTime(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetDataPagerNew(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetDataPagerNew(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetVOCsKQYDataPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetVOCsKQYDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetVOCKQYDataPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetVOCKQYDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得VOC外标因子原始小时虚拟分页查询数据和总行数(行转列数据)
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
        public DataView GetVOCWDataPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetDataVOCWPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetHourAvgData(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetHourAvgData(portIds, factors, dtStart, dtEnd,type, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据) NT O3做处理
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
        public DataView GetDataPagerForO3(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetDataPagerForO3(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得所有小时数据（缺失数据全显示）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerForO3AllTime(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetDataPagerForO3AllTime(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得所有小时数据（缺失数据全显示）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerForO3AllTimeNew(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetDataPagerForO3AllTimeNew(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得所有小时数据（缺失数据全显示）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerForO3AllTimeVOCs(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetDataPagerForO3AllTimeVOCs(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 根据VOC类别名称和时间获取类别总值
        /// </summary>
        /// <param name="TypeNames"></param>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataTable GetVOCDataAll(string[] TypeNames, DateTime dt, DateTime ds)
        {
            if (TypeNames.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetVOCDataAll(TypeNames, dt, ds);
            return null;
        }
        /// <summary>
        /// 根据VOC类别名称和时间获取类别总值
        /// </summary>
        /// <param name="TypeNames"></param>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataTable GetVOCDataAll(string unit,string[] TypeNames, DateTime dt, DateTime ds)
        {
            if (TypeNames.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetVOCDataAll(unit,TypeNames, dt, ds);
            return null;
        }
        /// <summary>
        /// 根据VOC类别名称和时间获取类别总值
        /// </summary>
        /// <param name="TypeNames"></param>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataTable GetVOCWDataAll(string unit, string[] TypeNames, DateTime dt, DateTime ds)
        {
            if (TypeNames.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetVOCWDataAll(unit, TypeNames, dt, ds);
            return null;
        }
        /// <summary>
        /// 根据VOC类别名称和时间获取类别总值
        /// </summary>
        /// <param name="TypeNames"></param>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataTable GetVOCDataAll(string[] TypeNames, DateTime dt, DateTime ds,string orderby)
        {
            if (TypeNames.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetVOCDataAll(TypeNames, dt, ds, orderby);
            return null;
        }
        public DataTable GetVOCDataAll(string unit,string[] TypeNames, DateTime dt, DateTime ds, string orderby)
        {
            if (TypeNames.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetVOCDataAll(unit,TypeNames, dt, ds, orderby);
            return null;
        }
        /// <summary>
        /// 离子色谱仪取得所有小时数据（缺失数据全显示）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerForO3AllTimeLZSPY(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return g_InfectantBy60Repository.GetDataPagerForO3AllTimeLZSPY(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
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
            , DateTime dateStart, DateTime dateEnd, string orderBy = "")
        {
            return g_InfectantBy60Repository.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
        }

        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public int GetAllDataCount(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return g_InfectantBy60Repository.GetAllDataCount(portIds, dateStart, dateEnd);
        }

        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                DataTable dt = g_InfectantBy60Repository.GetStatisticalData(portIds, factors.Select(p => p.PollutantCode).ToArray(), dateStart, dateEnd).Table;
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
        /// 返回指定站点、因子的捕获条数
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetPollutantValueCount(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return g_InfectantBy60Repository.GetPollutantValueCount(portIds, factors, dtStart, dtEnd);
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
            return g_InfectantBy60Repository.GetDayAvgData(portIds, factors, dtBegion, dtEnd);

        }

        public DataView GetDataList(string portIds, DateTime dtTime)
        {
            return g_InfectantBy60Repository.GetDataList(portIds, dtTime);

        }

        public DataView GetDataLists(string[] portIds, DateTime dtBegin, DateTime dtEnd)
        {
            return g_InfectantBy60Repository.GetDataLists(portIds, dtBegin, dtEnd);

        }
        public DataView GetHourDatas(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd, string orderBy = "PointId")
        {
            return g_InfectantBy60Repository.GetHourDatas(portIds, factors, dtStart, dtEnd);
        }
        public void GetAddData(string pointId, string PointAQM, string FileName, DateTime tstamp, DateTime dtTime, string Number, int RecordNumber, int XMLDBF)
        {
            g_InfectantBy60Repository.GetAddData(pointId, PointAQM, FileName, tstamp, dtTime, Number, RecordNumber, XMLDBF);
        }
      
        public void GetUpdateData(string pointId, DateTime tstamp, int RecordNumber)
        {
            g_InfectantBy60Repository.GetUpdateData(pointId, tstamp, RecordNumber);
        }
        /// <summary>
        /// 常规站补充缺失数据时间(包含O3-8因子)
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerAllTimeWithO8(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
          recordTotal = 0;
          if (factors.IsNotNullOrDBNull())
            return g_InfectantBy60Repository.GetDataPagerAllTimeWithO8(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
          return null;
        }
        /// <summary>
        /// 常规站补充缺失数据时间(包含O3-8因子)
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetDataPagerAllTimeWithO8Region(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "sortNumber desc,tstamp desc")
        {
          recordTotal = 0;
          if (factors.IsNotNullOrDBNull())
            return g_InfectantBy60Repository.GetDataPagerAllTimeWithO8Region(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
          return null;
        }
    }
}
