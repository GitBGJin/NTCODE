using log4net;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.Enums;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Air
{
    /// <summary>
    /// 名称：HourAQIRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-06-02
    /// 功能摘要：
    /// 点位小时AQI仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class HourAQIRepository : BaseGenericRepository<MonitoringBusinessModel, HourAQIEntity>
    {
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return true;
        }

        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        PortHourAQIDAL m_PortAQIDAL = Singleton<PortHourAQIDAL>.GetInstance();

        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_PortAQIDAL.GetDataPager(portIds, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据),计算原始小时数据AQI
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetOriDataPager(string[] portIds, DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_PortAQIDAL.GetOriDataPager(portIds, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 查询测点最新一条原始小时数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetAirQualityNewestOriRTReport(string[] portIds, int pageSize, int pageNo, out int recordTotal, DateTime dt, string orderBy = "DateTime,PointId")
        {
            try
            {
                return m_PortAQIDAL.GetAirQualityNewestOriRTReport(portIds, pageSize, pageNo, out recordTotal, dt, orderBy);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataHoursPager(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetDataHoursPager(portIds, dateStart, dateEnd);
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
        public DataView GetAvgDayData(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetAvgDayData(portIds, dateStart, dateEnd);
        }
        /// <summary>
        /// 取得所有数据供导出(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            return m_PortAQIDAL.GetExportData(portIds, dateStart, dateEnd, orderBy);
        }

        /// <summary>
        /// 根据站点取得最新小时AQI数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetLastData(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetLastData(portIds, dateStart, dateEnd);
        }

        /// <summary>
        /// 各等级天数统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="portIds">测点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点
        /// Level1Count：一级次数
        /// Level2Count：二级次数
        /// Level3Count：三级次数
        /// Level4Count：四级次数
        /// Level5Count：五级次数
        /// Level6Count：六级次数
        /// FineCount：优良次数
        /// OverCount：超标次数
        /// ValidCount：有效次数
        /// </returns>
        public DataView GetGradeStatistics(IAQIType aqiType, string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetGradeStatistics(aqiType, portIds, dateStart, dateEnd);
        }

        /// <summary>
        /// 各污染物首要污染物统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="portIds">测点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// Count：首要污染物次数
        /// </returns>
        public DataView GetContaminantsStatistics(IAQIType aqiType, string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetContaminantsStatistics(aqiType, portIds, dateStart, dateEnd);
        }
        #endregion

        #region 空气质量日报统计专用方法
        /// <summary>
        /// 获取时间段内指定测点的小时数据
        /// </summary>
        /// <param name="PointIds">测点ID数组</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetHourAQIData(List<int> PointIds, DateTime StartDate, DateTime EndDate)
        {
            return m_PortAQIDAL.GetHourAQIData(PointIds, StartDate, EndDate);
        }

        /// <summary>
        /// 计算时间段内指定测点监测因子的浓度及分指数平均值
        /// </summary>
        /// <param name="PointIds">测点ID数组</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetAvgHourAQIData(List<int> PointIds, DateTime StartDate, DateTime EndDate)
        {
            return m_PortAQIDAL.GetAvgHourAQIData(PointIds, StartDate, EndDate);
        }
        #endregion
    }
}
