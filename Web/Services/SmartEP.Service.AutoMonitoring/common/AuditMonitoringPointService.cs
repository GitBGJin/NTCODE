using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.common
{
    /// <summary>
    /// 名称：AuditMonitoringPointService.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-11-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：站点因子审核数据配置
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditMonitoringPointService
    {
        /// <summary>
        /// 测点审核配置数据
        /// </summary>
        AuditMonitoringPointRepository g_AuditMonitoringPointRepository = Singleton<AuditMonitoringPointRepository>.GetInstance();

        /// <summary>
        /// 因子审核配置数据
        /// </summary>
        AuditMonitoringPointPollutantRepository g_AuditMonitoringPointPollutantRepository = Singleton<AuditMonitoringPointPollutantRepository>.GetInstance();

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<AuditMonitoringPointEntity> MainRetrieve(Expression<Func<AuditMonitoringPointEntity, bool>> predicate)
        {
            return g_AuditMonitoringPointRepository.Retrieve(predicate);
        }

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<AuditMonitoringPointPollutantEntity> DetailRetrieve(Expression<Func<AuditMonitoringPointPollutantEntity, bool>> predicate)
        {
            return g_AuditMonitoringPointPollutantRepository.Retrieve(predicate);
        }

        #region 批量生成审核配置数据
        /// <summary>
        /// 批量生成审核配置数据
        /// </summary>
        /// <param name="PointUids">监测点Uid数组</param>
        /// <param name="MianLists">主表实体数组</param>
        /// <param name="DetailLists">从表实体数组</param>
        /// <param name="ApplicationUid">系统类型Uid</param>
        /// <param name="PointType">监测点类型 1超级，0普通</param>
        public void GetData(List<string> PointUids, List<AuditMonitoringPointEntity> MianLists, List<AuditMonitoringPointPollutantEntity> DetailLists, string ApplicationUid, int PointType)
        {
            g_AuditMonitoringPointRepository.GetData(PointUids, MianLists, DetailLists, ApplicationUid, PointType);
        }
        #endregion
    }
}
