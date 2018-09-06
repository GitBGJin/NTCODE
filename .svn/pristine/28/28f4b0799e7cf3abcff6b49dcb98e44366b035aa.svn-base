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
    /// 名称：RegionHourAQIRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 区域、全市、区县小时AQI仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class RegionHourAQIRepository : BaseGenericRepository<MonitoringBusinessModel, RegionHourAQIEntity>
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
        RegionHourAQIDAL m_RegionAQIDAL = Singleton<RegionHourAQIDAL>.GetInstance();

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,DateTime)</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] regionGuids, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "MonitoringRegionUid,DateTime", RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetDataPager(regionGuids, dtStart, dtEnd, pageSize, pageNo, out recordTotal, regionAQIStatisticalType, orderBy);
        }

        /// <summary>
        /// 获取苏州大市AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetAQIHourBase(DateTime BeginTime, DateTime EndTime)
        {
            return m_RegionAQIDAL.GetAQIHourBase(BeginTime, EndTime);
        }
        /// <summary>
        /// 获取苏州大市AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetPointAQIHourBase(DateTime BeginTime, DateTime EndTime, int points, string dataType)
        {
            return m_RegionAQIDAL.GetPointAQIHourBase(BeginTime, EndTime, points, dataType);
        }
        /// <summary>
        /// 获取苏州大市AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetPointAQIHourBaseOver23(DateTime BeginTime, DateTime EndTime, int points, string dataType, object avgO3)
        {
            return m_RegionAQIDAL.GetPointAQIHourBaseOver23(BeginTime, EndTime, points, dataType, avgO3);
        }
        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="regionGuids">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,DateTime)</param>
        /// <returns></returns>
        public DataView GetExportData(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string orderBy = "MonitoringRegionUid,DateTime", RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetExportData(regionGuids, dtStart, dtEnd, regionAQIStatisticalType, orderBy);
        }

        /// <summary>
        /// 根据站点取得最新小时AQI数据
        /// </summary>
        /// <param name="regions">区域</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns></returns>
        public DataView GetLastData(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetLastData(regions, dateStart, dateEnd, regionAQIStatisticalType);
        }

        /// <summary>
        /// 各等级天数统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="regionGuids">测点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns></returns>
        public DataView GetGradeStatistics(IAQIType aqiType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetGradeStatistics(aqiType, regionGuids, dateStart, dateEnd, regionAQIStatisticalType);
        }

        /// <summary>
        /// 各污染物首要污染物统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="regionGuids">测点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns></returns>
        public DataView GetContaminantsStatistics(IAQIType aqiType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetContaminantsStatistics(aqiType, regionGuids, dateStart, dateEnd, regionAQIStatisticalType);
        }
        #endregion
    }
}
