using SmartEP.AMSRepository.Water;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.WaterAutoMonitoring;
using SmartEP.Service.AutoMonitoring.Interfaces;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Water
{
    /// <summary>
    /// 名称：InfectantByRTService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 实时数据服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InfectantByRTService : IMonitoringData, IInfectantDALService
    {
        /// <summary>
        /// 1分钟数据仓储层
        /// </summary>
        InfectantByRTRepository g_InfectantByRTRepository = Singleton<InfectantByRTRepository>.GetInstance();

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
            return g_InfectantByRTRepository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp >= startTime && x.Tstamp <= endTime && pollutantCodes.Contains(x.PollutantCode));
        }

        /// <summary>
        /// 追加监测数据
        /// </summary>
        /// <param name="baseEntity">1分钟model实体</param>
        public void SaveMonitoringData(IBaseEntityProperty baseEntity)
        {
            g_InfectantByRTRepository.Add(baseEntity);
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
            return g_InfectantByRTRepository.RetrieveCount(x => x.PointId.Equals(portId) && x.Tstamp.Equals(tstamp) && x.PollutantCode.Equals(pollutantCode)) == 0;
        }

        /// <summary>
        /// 更新监测数据
        /// <summary>
        public void UpdateMonitoringData()
        {
            g_InfectantByRTRepository.SaveChanges();
        }

        /// <summary>
        /// 根据时间段删除监测数据
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        public void DeleteMonitoringData(DateTime startTime, DateTime endTime)
        {
            IQueryable<InfectantByRTEntity> query = g_InfectantByRTRepository.Retrieve(x => x.Tstamp >= startTime && x.Tstamp <= endTime);
            if (query != null && query.Count() > 0) g_InfectantByRTRepository.BatchDelete(query.ToList<InfectantByRTEntity>());
        }

        /// <summary>
        /// 根据监测点删除监测数据
        /// </summary>
        /// <param name="portId">测点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        public void DeleteMonitoringData(int portId, DateTime startTime, DateTime endTime)
        {
            IQueryable<InfectantByRTEntity> query = g_InfectantByRTRepository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp >= startTime && x.Tstamp <= endTime);
            if (query != null && query.Count() > 0) g_InfectantByRTRepository.BatchDelete(query.ToList<InfectantByRTEntity>());
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
            return TypeConversionExtensions.TryTo<decimal?, decimal>(g_InfectantByRTRepository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp >= startTime && x.Tstamp <= endTime).Average<InfectantByRTEntity>(x => x.PollutantValue));
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
            return TypeConversionExtensions.TryTo<decimal?, decimal>(g_InfectantByRTRepository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp >= startTime && x.Tstamp <= endTime).Max<InfectantByRTEntity>(x => x.PollutantValue));
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
            return TypeConversionExtensions.TryTo<decimal?, decimal>(g_InfectantByRTRepository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp >= startTime && x.Tstamp <= endTime).Min<InfectantByRTEntity>(x => x.PollutantValue));
        }

        /// <summary>
        /// 获取最新一条监测数据
        /// </summary>
        /// <param name="portId">测点</param>
        /// <param name="pollutantCodes">因子</param>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveNewestMonitoringData(int portId, List<string> pollutantCodes)
        {
            InfectantByRTEntity entity = g_InfectantByRTRepository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp <= DateTime.Now).OrderByDescending(x => x.Tstamp).FirstOrDefault();
            if (entity != null)
                return g_InfectantByRTRepository.Retrieve(x => x.PointId.Equals(portId) && entity.Tstamp.Equals(x.Tstamp) && pollutantCodes.Contains(x.PollutantCode));
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
            IQueryable<DateTime> dateQuery = g_InfectantByRTRepository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp <= DateTime.Now).GroupBy(x => x.Tstamp).OrderByDescending(x => x.Key).Select(x => x.Key).Take(number);
            if (dateQuery != null && dateQuery.Count() > 0)
            {
                DateTime dateMin = dateQuery.Last();
                return g_InfectantByRTRepository.Retrieve(x => x.PointId.Equals(portId) && x.Tstamp >= dateMin && pollutantCodes.Contains(x.PollutantCode));
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
            return g_InfectantByRTRepository.RetrieveCount(x => x.PointId.Equals(portId) && x.Tstamp >= startTime && x.Tstamp <= endTime);
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
                return g_InfectantByRTRepository.GetDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
                return g_InfectantByRTRepository.GetDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
            return g_InfectantByRTRepository.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
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
            return g_InfectantByRTRepository.GetAllDataCount(portIds, dateStart, dateEnd);
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
                return g_InfectantByRTRepository.GetStatisticalData(portIds, factors.Select(p => p.PollutantCode).ToArray(), dateStart, dateEnd);
            return null;
        }
        #endregion
    }
}
