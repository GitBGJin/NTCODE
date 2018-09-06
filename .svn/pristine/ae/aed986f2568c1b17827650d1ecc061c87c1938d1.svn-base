using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using SmartEP.Data.SqlServer.MonitoringBusiness.Water;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Water
{
    /// <summary>
    /// 地表水审核小时数据仓储类
    /// </summary>
    public class HourReportRepository : BaseGenericRepository<MonitoringBusinessModel, WaterHourReportEntity>
    {

        public override bool IsExist(string strKey)
        {
            throw new NotImplementedException();
        }

        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        HourReportDAL m_HourReportDAC = Singleton<HourReportDAL>.GetInstance();

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
            return m_HourReportDAC.GetDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetNewDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_HourReportDAC.GetNewDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtBegin">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetDataHourPager(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,
            DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,tstamp desc")
        {
            return m_HourReportDAC.GetDataHourPager(portIds, factors, dtBegin, dtEnd, dtFrom, dtTo, type, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtBegin">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,
            DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,tstamp desc,Type")
        {
            return m_HourReportDAC.GetDataPager(portIds, factors, dtBegin, dtEnd, dtFrom, dtTo, type, pageSize, pageNo, out recordTotal, orderBy);
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
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            return m_HourReportDAC.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
        }

	public DataView GetExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string lv, string orderBy = "PointId,DateTime")
        {
            return m_HourReportDAC.GetExportData(portIds, factors, dateStart, dateEnd, lv, orderBy);
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
            return m_HourReportDAC.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
        }

	 public DataView GetExportData(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, string lv, string orderBy = "PointId,DateTime")
        {
            return m_HourReportDAC.GetExportData(portIds, factors, dateStart, dateEnd, lv, orderBy);
        }
        /// <summary>
        /// 获得小时审核前数据
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="tstamp"></param>
        /// <returns></returns>
        public DataView GetHourValue(string pointId, string tstamp)
        {
            return m_HourReportDAC.GetHourValue(pointId, tstamp);
        }
        /// <summary>
        /// 获得小时审核原因
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="tstamp"></param>
        /// <returns></returns>
        public DataView GetHourReason(string pointId, string tstamp)
        {
            return m_HourReportDAC.GetHourReason(pointId, tstamp);
        }


        /// <summary>
        /// 获得数据比对分析审核原因
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="tstamp"></param>
        /// <param name="factorCode"></param>
        /// <returns></returns>
        public DataView GetCompareReason(string tstamp, string factorCode)
        {
            return m_HourReportDAC.GetCompareReason(tstamp, factorCode);
        }


        /// <summary>
        /// 报表数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetExportDataReport(string[] portIds, IList<IPollutant> factors
           , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            return m_HourReportDAC.GetExportDataReport(portIds, factors, dateStart, dateEnd, orderBy);
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
            return m_HourReportDAC.GetAllDataCount(portIds, dateStart, dateEnd);
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
            return m_HourReportDAC.GetStatisticalData(portIds, factors, dateStart, dateEnd);
        }
        public DataView GetStatisticalDataNew(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            return m_HourReportDAC.GetStatisticalDataNew(portIds, factors, dateStart, dateEnd);
        }
        /// <summary>
        /// 取得统计数据（最大值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalMaxTstampData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            return m_HourReportDAC.GetStatisticalMaxTstampData(portIds, factors, dateStart, dateEnd);
        }

        /// <summary>
        /// 取得指定范围内的各指标的污染指数和水质等级
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="WQPollutants">所有参与统计的因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetWaterQualityData(string[] portIds, string[] WQPollutants, DateTime dateStart, DateTime dateEnd)
        {
            return m_HourReportDAC.GetWaterQualityData(portIds, WQPollutants, dateStart, dateEnd);
        }

        /// <summary>
        /// 蓝藻日报预警（小时数据前一天8点到第二天12点数据平均值）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetBlueAlgaeDayData(string[] portIds, string[] factors
    , DateTime dtStart, DateTime dtEnd)
        {
            return m_HourReportDAC.GetBlueAlgaeDayData(portIds, factors, dtStart, dtEnd);
        }
        #endregion
    }
}
