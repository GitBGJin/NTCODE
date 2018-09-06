using SmartEP.AMSRepository.Air;
using SmartEP.DomainModel.AirAutoMonitoring;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Air
{
    /// <summary>
    /// 名称：AirCalibrationService.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-11-26
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：校准数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AirCalibrationService
    {
        AirCalibrationRepository g_AirCalibrationRepository = new AirCalibrationRepository();

        /// <summary>
        /// 校准数据
        /// </summary>
        /// <param name="PointIds">测点Id</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">截止日期</param>
        /// <param name="CalTypeCodes">校准类型</param>
        /// <returns></returns>
        public DataTable GetData(List<int> PointIds, DateTime StartDate, DateTime EndDate, List<string> CalTypeCodes)
        {
            return g_AirCalibrationRepository.GetData(PointIds, StartDate, EndDate, CalTypeCodes);
        }
    }
}
