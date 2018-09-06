using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using System.Data;

namespace SmartEP.MonitoringBusinessRepository.Common
{
    public class CalibrationReportRepository
    {
        CalibrationReportDAL _calibrationDal = new CalibrationReportDAL();

        /// <summary>
        /// 按点位获取校准报告数据(按日统计)
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pointid"></param>
        /// <returns></returns>
        public DataView GetCalibrationDayData(DateTime beginTime, DateTime endTime, string[] pointid, string[] factors)
        {
            return _calibrationDal.GetCalibrationDayData(beginTime, endTime, pointid,factors);
        }

        /// <summary>
        /// 按点位获取校准报告数据(按小时统计)
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pointid"></param>
        /// <returns></returns>
        public DataView GetCalibrationHourData(DateTime beginTime, DateTime endTime, string[] pointid, string[] factors)
        {
            return _calibrationDal.GetCalibrationHourData(beginTime, endTime, pointid, factors);
        }
    }
}
