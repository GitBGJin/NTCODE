using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness.Water;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;

namespace SmartEP.MonitoringBusinessRepository.Water
{
    /// <summary>
    /// 地表水日数据仓储类
    /// </summary>
    public class DayReportRepository : BaseGenericRepository<MonitoringBusinessModel, WaterDayReportEntity>
    {

        public override bool IsExist(string strKey)
        {
            throw new NotImplementedException();
        }

        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        DayReportDAL m_DayReportDAC = Singleton<DayReportDAL>.GetInstance();

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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_DayReportDAC.GetDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd
            , DateTime dtFrom, DateTime dtTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime desc")
        {
            return m_DayReportDAC.GetDataPager(portIds, factors, dateStart, dateEnd, dtFrom, dtTo, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string LVTable, string orderBy = "PointId,DateTime")
        {
            return m_DayReportDAC.GetExportData(portIds, factors, dateStart, dateEnd, LVTable, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            return m_DayReportDAC.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
        }

        public DataView GetExportData(string[] portIds, string[] factors
           , DateTime dateStart, DateTime dateEnd, string LVTable, string orderBy = "PointId,DateTime")
        {
            return m_DayReportDAC.GetExportData(portIds, factors, dateStart, dateEnd, LVTable, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetExportDataReport(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            return m_DayReportDAC.GetExportDataReport(portIds, factors, dateStart, dateEnd, orderBy);
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetExportNewDataReport_SZ(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            return m_DayReportDAC.GetExportNewDataReport_SZ(portIds, factors, dateStart, dateEnd, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetExportDataReport_SZ(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            return m_DayReportDAC.GetExportDataReport_SZ(portIds, factors, dateStart, dateEnd, orderBy);
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
            return m_DayReportDAC.GetAllDataCount(portIds, dateStart, dateEnd);
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
            return m_DayReportDAC.GetStatisticalData(portIds, factors, dateStart, dateEnd);
        }
        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalDataNew(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            return m_DayReportDAC.GetStatisticalDataNew(portIds, factors, dateStart, dateEnd);
        }

        /// <summary>
        /// 取得指定范围内的各指标的污染指数和水质等级
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="WQPollutants">所有参与统计的因子(WaterQualityService.GetWaterQualityPollutant()</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetWaterQualityData(string[] portIds, string[] WQPollutants, DateTime dateStart, DateTime dateEnd)
        {
            return m_DayReportDAC.GetWaterQualityData(portIds, WQPollutants, dateStart, dateEnd);
        }


        public DataView GetRegionWQData(string[] portIds, IList<IPollutant> factors
        , DateTime dtStart, DateTime dtEnd, string orderBy = "DateTime")
        {
            return m_DayReportDAC.GetRegionWQData(portIds, factors, dtStart, dtEnd, orderBy);
        }

        /// <summary>
        /// 获取测点相关基础信息及相关因子日数据及等级
        /// </summary>
        /// <param name="PointIDs">测点数组</param>
        /// <param name="PollutantCodes">因子Code数组</param>
        /// <param name="Date">日期</param>
        /// <returns></returns>
        public DataTable GetWaterIEQI(List<int> PointIDs, List<string> PollutantCodes, DateTime Date)
        {
            return m_DayReportDAC.GetWaterIEQI(PointIDs, PollutantCodes, Date);
        }

        /// <summary>
        /// 获取自动点下的点位因子数据
        /// </summary>
        /// <param name="PointIds">站点ID数组</param>
        /// <param name="PollutantCodes">因子Code数组</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public DataTable GetWaterDayReport(List<int> PointIds, List<string> PollutantCodes, DateTime date)
        {
            return m_DayReportDAC.GetWaterDayReport(PointIds, PollutantCodes, date);
        }

        /// <summary>
        /// 水质自动监测月度小结，获取最大藻密度值与点位信息
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <param name="PointIds">监测点ID数组</param>
        /// <param name="PointIds">监测因子Code数组</param>
        /// <param name="SiteTypeUids">监测点类型Uid数组</param>
        /// <param name="isSiteType">是否监测点类型</param>
        /// <returns></returns>
        public DataTable GetMaxPollutantValue(DateTime datetime, List<int> PointIds, List<string> PollutantCodes, List<string> SiteTypeUids, bool isSiteType)
        {
            return m_DayReportDAC.GetMaxPollutantValue(datetime, PointIds, PollutantCodes, SiteTypeUids, isSiteType);
        }

        /// <summary>
        /// 取得无锡水质周报所需日数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataTable GetDataForWeekReport(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_DayReportDAC.GetDataForWeekReport(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 取得无锡水质周报所需VOC日数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataTable GetVOCDataForWeekReport(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_DayReportDAC.GetVOCDataForWeekReport(portIds, factors, dtStart, dtEnd);
        }

        /// <summary>
        /// 获取水源地测点日均监测值
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <returns></returns>
        public DataView GetJSGDayReport(DateTime beginTime, DateTime endTime, List<int> PointIds, List<string> PollutantCodes)
        {
            return m_DayReportDAC.GetJSGDayReport(beginTime, endTime, PointIds, PollutantCodes);
        }

        /// <summary>
        /// 获取水质现场调查相关监测数据
        /// </summary>
        /// <param name="PointIds">点位Id</param>
        /// <param name="Date">采集日期</param>
        /// <returns></returns>
        public DataView GetJSGSamplingData(DateTime BeginTime, DateTime EndTime, List<int> PointIds, List<string> PollutantCodes)
        {
            return m_DayReportDAC.GetJSGSamplingData(BeginTime, EndTime, PointIds, PollutantCodes);
        }

        #region 水质运行月报数据
        /// <summary>
        /// 获取水质运行月报数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataView GetRMStatisticalData(List<int> PointIds, DateTime BeginDate, DateTime EndDate)
        {
            return m_DayReportDAC.GetRMStatisticalData(PointIds, BeginDate, EndDate);
        }

        /// <summary>
        /// 获取水质运行月报数据（平均值）
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataView GetRMStatisticalDataAvg(List<int> PointIds, DateTime BeginDate, DateTime EndDate)
        {
            return m_DayReportDAC.GetRMStatisticalDataAvg(PointIds, BeginDate, EndDate);
        }

        /// <summary>
        /// 获取水质运行月报数据（日数据）
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataView GetRMDayData(List<int> PointIds, DateTime BeginDate, DateTime EndDate)
        {
            return m_DayReportDAC.GetRMDayData(PointIds, BeginDate, EndDate);
        }

         /// <summary>
        /// 获取水质类别或因子限值
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public DataView GetGradeOrLimit(List<int> PointIds, int Year, int Month)
        {
            return m_DayReportDAC.GetGradeOrLimit(PointIds, Year, Month);
        }

        #endregion

        #endregion
    }
}
