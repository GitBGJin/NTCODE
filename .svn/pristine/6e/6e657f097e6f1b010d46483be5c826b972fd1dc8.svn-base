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
    /// 名称：DayAQIRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 点位日AQI仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DayAQIRepository : BaseGenericRepository<MonitoringBusinessModel, DayAQIEntity>
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
        PortDayAQIDAL m_PortAQIDAL = Singleton<PortDayAQIDAL>.GetInstance();

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
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段(PointId,DateTime)</param>
        /// <returns></returns>
        public DataView GetRunMonthPager(string[] portIds, DateTime dtStart, DateTime dtEnd)
        {
            return m_PortAQIDAL.GetRunMonthPager(portIds, dtStart, dtEnd);
        }
        #region  百分位数浓度
        /// <summary>
        /// 百分位数浓度_区域
        /// </summary>
        /// <param name="regionguid">区域guid</param>
        /// <param name="dtbegin">开始时间</param>
        /// <param name="dtend">截止时间</param>
        /// <param name="factorcode">因子code</param>
        /// <returns></returns>
        public string getpercent(string regionguid, DateTime dtbegin, DateTime dtend, string factorcode)
        {
            return m_PortAQIDAL.getpercent(regionguid, dtbegin, dtend, factorcode);
        }
        /// <summary>
        /// 百分位数浓度_点位
        /// </summary>
        /// <param name="regionguid">区域guid</param>
        /// <param name="dtbegin">开始时间</param>
        /// <param name="dtend">截止时间</param>
        /// <param name="factorcode">因子code</param>
        /// <returns></returns>
        public string getpercent_Point(string pointId, DateTime dtbegin, DateTime dtend, string factorcode)
        {
            return m_PortAQIDAL.getpercent_Point(pointId, dtbegin, dtend, factorcode);
        }
        #endregion
        #region  综合污染指数
        /// <summary>
        /// 综合污染指数
        /// </summary>
        /// <param name="regionguid">区域guid</param>
        /// <param name="dtbegin">开始时间</param>
        /// <param name="dtend">截止时间</param>
        /// <param name="factorcode">因子数组</param>
        /// <returns></returns>
        public string getSI(string regionguid, DateTime dtbegin, DateTime dtend, string[] factorcode)
        {
            return m_PortAQIDAL.getSI(regionguid, dtbegin, dtend, factorcode);
        }
        #endregion
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetDataMonthDayPager(string[] portIds, DateTime dtStart, DateTime dtEnd)
        {
            return m_PortAQIDAL.GetDataMonthDayPager(portIds, dtStart, dtEnd);
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
        public DataView GetDataMonthDaysPager(string[] portIds, DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_PortAQIDAL.GetDataPager(portIds, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得全月均值，最大最小值
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
        public DataView GetDataPager(string[] portIds, DateTime dateStart, DateTime dateEnd, string[] type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_PortAQIDAL.GetDataPager(portIds, dateStart, dateEnd, type, pageSize, pageNo, out recordTotal, orderBy);
        }
        public DataView GetDataPagerNew(string[] portIds, DateTime dateStart, DateTime dateEnd, string[] type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_PortAQIDAL.GetDataPagerNew(portIds, dateStart, dateEnd, type, pageSize, pageNo, out recordTotal, orderBy);
        }

        public DataView GetOriDataPagerNew(string[] portIds, DateTime dateStart, DateTime dateEnd, string[] type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_PortAQIDAL.GetOriDataPagerNew(portIds, dateStart, dateEnd, type, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetDataPagerDayAQI(string[] portIds, DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_PortAQIDAL.GetDataPagerDayAQI(portIds, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetConcentrationDay(string[] portIds,  DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId")
        {
            return m_PortAQIDAL.GetConcentrationDay(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        /// 获取点位AQI数据，时间点补遗
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetPortAllData(string[] portIds, DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            return m_PortAQIDAL.GetPortAllData(portIds, dateStart, dateEnd, orderBy);
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
        ///  获取多点位平均后的AQI统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="portIds">测点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetGradeStatisticsMutilPoint(IAQIType aqiType, string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetGradeStatisticsMutilPoint(aqiType, portIds, dateStart, dateEnd);
        }


        /// <summary>
        /// 获取时间段内多点的AQI统计数据
        /// </summary>
        /// <param name="aqiType"></param>
        /// <param name="portIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataView GetMutilPointAQIData(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetMutilPointAQIData(portIds, dateStart, dateEnd);
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

        /// <summary>
        /// 取得指定日期内日数据均值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// </returns>
        public DataView GetAvgValue(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetAvgValue(portIds, dateStart, dateEnd);
        }

        /// <summary>
        /// 取得指定日期内日数据最大值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetMaxValue(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetMaxValue(portIds, dateStart, dateEnd);
        }
        /// <summary>
        /// 取得指定日期内日数据最大值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetMaxValueOne(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetMaxValueOne(portIds, dateStart, dateEnd);
        }
        /// <summary>
        /// 取得指定日期内日数据最小值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetMinValue(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetMinValue(portIds, dateStart, dateEnd);
        }
        /// <summary>
        /// 取得指定日期内日数据最小值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetMinValueOne(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetMinValueOne(portIds, dateStart, dateEnd);
        }
        /// <summary>
        /// 取得指定日期内日数据样本数
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetCountValue(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetCountValue(portIds, dateStart, dateEnd);
        }

        /// <summary>
        /// 日数据超标天数统计
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// ValidCount：有效天
        /// OverCount：超标天
        /// PM25_Over：PM25超标天
        /// PM10_Over：PM10超标天
        /// NO2_Over：NO2超标天
        /// SO2_Over：SO2超标天
        /// CO_Over：CO超标天
        /// MaxOneHourO3_Over：MaxOneHourO3超标天
        /// Max8HourO3_Over：Max8HourO3超标天
        /// </returns>
        public DataView GetExceedingData(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_PortAQIDAL.GetExceedingData(portIds, dateStart, dateEnd);
        }
        #endregion

        #region 获取站点日AQI数据
        /// <summary>
        /// 获取站点日AQI数据
        /// </summary>
        /// <param name="PointId">站点Id</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">截止日期</param>
        /// <returns>DataView</returns>
        public DataView GetDataByPoint(int PointId, DateTime StartDate, DateTime EndDate)
        {
            return m_PortAQIDAL.GetDataByPoint(PointId, StartDate, EndDate);
        }
        #endregion

        #region 空气质量日报统计专用方法
        /// <summary>
        /// 获取时间段内指定测点的日数据
        /// </summary>
        /// <param name="PointIds">测点ID数组</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetDayAQIData(List<int> PointIds, DateTime StartDate, DateTime EndDate)
        {
            return m_PortAQIDAL.GetDayAQIData(PointIds, StartDate, EndDate);
        }

        /// <summary>
        /// 计算时间段内指定测点监测因子的浓度及分指数平均值
        /// </summary>
        /// <param name="PointIds">测点ID数组</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetAvgDayAQIData(List<int> PointIds, DateTime StartDate, DateTime EndDate)
        {
            return m_PortAQIDAL.GetAvgDayAQIData(PointIds, StartDate, EndDate);
        }
        #endregion

        public DataView GetPointDataPagerClass(string[] portIds, string[] classDics, DateTime dtBegion, DateTime dtEnd)
        {
            return m_PortAQIDAL.GetPointPager(portIds, classDics, dtBegion, dtEnd);
        }

        public DataView GetPointFirstPollute(string[] portIds, string[] classDics, DateTime dtBegion, DateTime dtEnd)
        {
            return m_PortAQIDAL.GetFirstPollute(portIds, classDics, dtBegion, dtEnd);
        }
    }
}
