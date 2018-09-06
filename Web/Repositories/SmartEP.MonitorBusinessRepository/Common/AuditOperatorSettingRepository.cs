using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Common
{
    /// <summary>
    /// 名称：AuditOperatorSettingRepository.cs
    /// 创建人：徐龙超
    /// 创建日期：2016-04-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    ///人工审核操作配置
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditOperatorSettingRepository : BaseGenericRepository<MonitoringBusinessModel, AuditStatusMappingEntity>
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
    }
}
