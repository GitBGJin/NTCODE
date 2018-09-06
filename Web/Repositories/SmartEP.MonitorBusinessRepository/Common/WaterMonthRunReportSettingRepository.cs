using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;

namespace SmartEP.MonitoringBusinessRepository.Common
{
    /// <summary>
    /// 名称：ReportSamplingRateByHourRepository.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-11-26
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 小时捕集率
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterMonthRunReportSettingRepository : BaseGenericRepository<MonitoringBusinessModel, WaterMonthRunReportSettingEntity>
    {
        /// <summary>
        /// 根据主键Key判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return true;
        }
    }
}
