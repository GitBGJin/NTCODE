using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Water
{
    /// <summary>
    /// 名称：AutoMonitoringMonthSummaryRepository.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-12-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：水质自动监测月度小结
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AutoMonitoringMonthSummaryRepository : BaseGenericRepository<MonitoringBusinessModel, AutoMonitoringMonthSummaryEntity>
    {
        public override bool IsExist(string strKey)
        {
            throw new NotImplementedException();
        }
    }
}
