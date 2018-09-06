﻿using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository.Public;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Public
{
    public class PublicReportService
    {
        PublicReportRepository PublicReport = Singleton<PublicReportRepository>.GetInstance();

        /// <summary>
        /// 获取系统运行有效率
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public DataView GetRuningEffectRate(string ApplicationUid, List<int> PointIds, List<string> PollutantCodes, DateTime BeginTime, DateTime EndTime)
        {
            return PublicReport.GetRuningEffectRate(ApplicationUid, PointIds, PollutantCodes, BeginTime, EndTime);
        }

        /// <summary>
        /// 获取系统应测数据
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public DataView GetShouldRuningEffectRate(string ApplicationUid, List<int> PointIds, List<string> PollutantCodes, DateTime BeginTime, DateTime EndTime)
        {
            return PublicReport.GetShouldRuningEffectRate(ApplicationUid, PointIds, PollutantCodes, BeginTime, EndTime);
        }

        /// <summary>
        /// 获取系统运行有效率（不确定因子）
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="dtPoints"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public DataView GetRunningEffectRateByUncertainFactors(string ApplicationUid, DataTable dtPoints, DateTime BeginTime, DateTime EndTime)
        {
            return PublicReport.GetRunningEffectRateByUncertainFactors(ApplicationUid, dtPoints, BeginTime, EndTime);
        }
        /// <summary>
        /// 获取系统应测记录数
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="dtPoints"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public DataView GetShouldRecordByUncertainFactors(string ApplicationUid, DataTable dtPoints, DateTime BeginTime, DateTime EndTime)
        {
            return PublicReport.GetShouldRecordByUncertainFactors(ApplicationUid, dtPoints, BeginTime, EndTime);
        }
        /// <summary>
        /// 获取系统运行有效率
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <param name="YearBegin"></param>
        /// <param name="MonthBegin"></param>
        /// <param name="YearEnd"></param>
        /// <param name="MonthEnd"></param>
        /// <returns></returns>
        public DataView GetRunningEffectRateByMonth(string ApplicationUid, List<int> PointIds, List<string> PollutantCodes, int YearBegin, int MonthBegin, int YearEnd, int MonthEnd)
        {
            return PublicReport.GetRunningEffectRateByMonth(ApplicationUid, PointIds, PollutantCodes, YearBegin, MonthBegin, YearEnd, MonthEnd);
        }

        /// <summary>
        /// 获取有效数据捕获率
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public DataView GetDataSamplingRate(string ApplicationUid, List<int> PointIds, List<string> PollutantCodes, DateTime BeginTime, DateTime EndTime)
        {
            return PublicReport.GetDataSamplingRate(ApplicationUid, PointIds, PollutantCodes, BeginTime, EndTime);
        }

          /// <summary>
        /// 获取质控数据合格率
        /// </summary>
        /// <param name="MissionIds"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public DataView GetQualityControlData(List<string> MissionIds, List<int> PointIds, List<string> PollutantCodes, DateTime BeginTime, DateTime EndTime)
        {
            return PublicReport.GetQualityControlData(MissionIds, PointIds, PollutantCodes, BeginTime, EndTime);
        }

        /// <summary>
        /// 获取异常情况处理率
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public DataView GetExceptionHandingRate(List<int> PointIds, List<string> PollutantCodes, DateTime BeginTime, DateTime EndTime)
        {
            return PublicReport.GetExceptionHandingRate(PointIds, PollutantCodes, BeginTime, EndTime);
        }
    }
}
