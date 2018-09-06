using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Air
{
    
    public class GranuleSpecialRepository
    {
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        GranuleSpecialDAL d_GranuleSpecial = Singleton<GranuleSpecialDAL>.GetInstance();
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetOriHourData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd, string type)
        {
            return d_GranuleSpecial.GetOriHourData(portIds, factors, dtStart, dtEnd, type);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetOriHourDataNew(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, string type)
        {
            return d_GranuleSpecial.GetOriHourDataNew(portIds, factors, dtStart, dtEnd, type);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetOriDayData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return d_GranuleSpecial.GetOriDayData(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetOriDayDataNew(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return d_GranuleSpecial.GetOriDayDataNew(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetOriMonthData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return d_GranuleSpecial.GetOriMonthData(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetOriMonthDataNew(string [] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return d_GranuleSpecial.GetOriMonthDataNew(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 审核数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditHourDataNew(string [] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return d_GranuleSpecial.GetAuditHourDataNew(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditDayDataNew(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return d_GranuleSpecial.GetAuditDayDataNew(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditMonthDataNew(string [] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return d_GranuleSpecial.GetAuditMonthDataNew(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditSeasonDataNew(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            return d_GranuleSpecial.GetAuditSeasonDataNew(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditYearDataNew(string[] portIds, string[] factors, int yearFrom, int yearTo)
        {
            return d_GranuleSpecial.GetAuditYearDataNew(portIds, factors, yearFrom, yearTo);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditWeekDataNew(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom, int yearTo, int weekOfYearTo)
        {
            return d_GranuleSpecial.GetAuditWeekDataNew(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo);
        }
        /// <summary>
        /// 审核数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditHourData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return d_GranuleSpecial.GetAuditHourData(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditDayData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return d_GranuleSpecial.GetAuditDayData(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditMonthData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return d_GranuleSpecial.GetAuditMonthData(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditWeekData(string portIds, string[] factors, int yearFrom, int weekOfYearFrom,int yearTo, int weekOfYearTo)
        {
            return d_GranuleSpecial.GetAuditWeekData(portIds, factors,yearFrom,weekOfYearFrom, yearTo,weekOfYearTo);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditSeasonData(string portIds, string[] factors,int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            return d_GranuleSpecial.GetAuditSeasonData(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo);
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditYearData(string portIds, string[] factors, int yearFrom, int yearTo)
        {
            return d_GranuleSpecial.GetAuditYearData(portIds, factors, yearFrom, yearTo);
        }
    }
}
