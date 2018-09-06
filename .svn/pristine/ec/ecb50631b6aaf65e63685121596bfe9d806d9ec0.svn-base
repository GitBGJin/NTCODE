using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Service.AutoMonitoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Water
{
    /// <summary>
    /// 名称：MonitoringDataWater.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 原始数据服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MonitoringDataWater : IMonitoringData, IInfectantDALService
    {
        /// <summary>
        /// 实时监控数据公用接口（ORM）
        /// </summary>
        IMonitoringData g_IMonitoringData = null;

        /// <summary>
        /// 实时监控数据公用接口（DAL）
        /// </summary>
        IInfectantDALService g_IInfectantDALService = null;

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pollutantDataType">数据类型（1分钟、5分钟、1小时）</param>
        public MonitoringDataWater(PollutantDataType pollutantDataType)
        {
            switch (pollutantDataType)
            {
                case PollutantDataType.Min1:
                    g_IMonitoringData = Singleton<InfectantBy1Service>.GetInstance();
                    g_IInfectantDALService = Singleton<InfectantBy1Service>.GetInstance();
                    break;
                case PollutantDataType.Min5:
                    g_IMonitoringData = Singleton<InfectantBy5Service>.GetInstance();
                    g_IInfectantDALService = Singleton<InfectantBy5Service>.GetInstance();
                    break;
                case PollutantDataType.Min60:
                    g_IMonitoringData = Singleton<InfectantBy60Service>.GetInstance();
                    g_IInfectantDALService = Singleton<InfectantBy60Service>.GetInstance();
                    break;
                case PollutantDataType.RealTime:
                    g_IMonitoringData = Singleton<InfectantByRTService>.GetInstance();
                    g_IInfectantDALService = Singleton<InfectantByRTService>.GetInstance();
                    break;
                default:
                    g_IMonitoringData = Singleton<InfectantBy60Service>.GetInstance();
                    g_IInfectantDALService = Singleton<InfectantBy60Service>.GetInstance();
                    break;
            }
        }
        #endregion

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
            return g_IMonitoringData != null ? g_IMonitoringData.RetrieveMonitoringData(portId, startTime, endTime, pollutantCodes) : null;
        }

        /// <summary>
        /// 追加监测数据
        /// </summary>
        /// <param name="baseEntity">60分钟model实体</param>
        public void SaveMonitoringData(IBaseEntityProperty baseEntity)
        {
            if (g_IMonitoringData != null)
                g_IMonitoringData.SaveMonitoringData(baseEntity);
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
            return g_IMonitoringData != null ? g_IMonitoringData.IsExist(portId, tstamp, pollutantCode) : false;
        }

        /// <summary>
        /// 更新监测数据
        /// <summary>
        public void UpdateMonitoringData()
        {
            if (g_IMonitoringData != null)
                g_IMonitoringData.UpdateMonitoringData();
        }

        /// <summary>
        /// 根据时间段删除监测数据
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        public void DeleteMonitoringData(DateTime startTime, DateTime endTime)
        {
            if (g_IMonitoringData != null)
                g_IMonitoringData.DeleteMonitoringData(startTime, endTime);
        }

        /// <summary>
        /// 根据监测点删除监测数据
        /// </summary>
        /// <param name="portId">测点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        public void DeleteMonitoringData(int portId, DateTime startTime, DateTime endTime)
        {
            if (g_IMonitoringData != null)
                g_IMonitoringData.DeleteMonitoringData(portId, startTime, endTime);
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
            return g_IMonitoringData != null ? g_IMonitoringData.RetrieveAvgMonitoringDataByPollutant(portId, startTime, endTime, pollutantCode) : -99999M;
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
            return g_IMonitoringData != null ? g_IMonitoringData.RetrieveMaxMonitoringDataByPollutant(portId, startTime, endTime, pollutantCode) : -99999M; ;
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
            return g_IMonitoringData != null ? g_IMonitoringData.RetrieveMinMonitoringDataByPollutant(portId, startTime, endTime, pollutantCode) : -99999M; ;
        }

        /// <summary>
        /// 获取最新一条监测数据
        /// </summary>
        /// <param name="portId">测点</param>
        /// <param name="pollutantCodes">因子</param>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> RetrieveNewestMonitoringData(int portId, List<string> pollutantCodes)
        {
            return g_IMonitoringData != null ? g_IMonitoringData.RetrieveNewestMonitoringData(portId, pollutantCodes) : null; ;
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
            return g_IMonitoringData != null ? g_IMonitoringData.RetrieveNewestMonitoringData(portId, pollutantCodes, number) : null; ;
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
            return g_IMonitoringData != null ? g_IMonitoringData.RetrieveMonitoringDataRecordsByPollutant(portId, startTime, endTime, pollutantCode) : 0;
        }
        #endregion

        #region << ADO.NET >>
        /// <summary>
        /// 取得不同数据类型对应的DAL接口
        /// </summary>
        /// <param name="pollutantDataType">污染物类型</param>
        /// <returns></returns>
        public static IInfectantDALService GetInfectantDALService(PollutantDataType pollutantDataType)
        {
            IInfectantDALService iService = null;
            switch (pollutantDataType)
            {
                case PollutantDataType.Min1:
                    iService = Singleton<InfectantBy1Service>.GetInstance();
                    break;
                case PollutantDataType.Min5:
                    iService = Singleton<InfectantBy5Service>.GetInstance();
                    break;
                case PollutantDataType.Min60:
                    iService = Singleton<InfectantBy60Service>.GetInstance();
                    break;
                case PollutantDataType.RealTime:
                    iService = Singleton<InfectantByRTService>.GetInstance();
                    break;
                default:
                    iService = Singleton<InfectantBy60Service>.GetInstance();
                    break;
            }
            return iService;
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
        public DataView GetDataPager(string[] portIds, IList<IPollutant> factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "")
        {
            recordTotal = 0;
            return g_IMonitoringData != null ? g_IInfectantDALService.GetDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy) : null;
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
            return g_IMonitoringData != null ? g_IInfectantDALService.GetDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy) : null;
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
            return g_IMonitoringData != null ? g_IInfectantDALService.GetExportData(portIds, factors, dateStart, dateEnd, orderBy) : null;
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
            return g_IMonitoringData != null ? g_IInfectantDALService.GetAllDataCount(portIds, dateStart, dateEnd) : 0;
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
            return g_IMonitoringData != null ? g_IInfectantDALService.GetStatisticalData(portIds, factors, dateStart, dateEnd) : null;
        }
        #endregion
    }
}
