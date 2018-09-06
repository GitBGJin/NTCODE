using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.MonitoringBusinessRepository.Common;

namespace SmartEP.Service.DataAnalyze.Report
{
    public class CalibrationReportService
    {
        CalibrationReportRepository _calibrationRep = new CalibrationReportRepository();

        /// <summary>
        /// 按点位获取校准报告数据（按日统计）
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pointid"></param>
        /// <returns></returns>
        public DataView GetCalibrationDayData(DateTime beginTime, DateTime endTime, string[] pointid, string[] factors)
        {
            return _calibrationRep.GetCalibrationDayData(beginTime, endTime, pointid, factors);
        }

        /// <summary>
        /// 按点位获取校准报告数据（按小时统计）
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pointid"></param>
        /// <returns></returns>
        public DataView GetCalibrationHourData(DateTime beginTime, DateTime endTime, string[] pointid, string[] factors)
        {
            return _calibrationRep.GetCalibrationHourData(beginTime, endTime, pointid, factors);
        }
    }
}
