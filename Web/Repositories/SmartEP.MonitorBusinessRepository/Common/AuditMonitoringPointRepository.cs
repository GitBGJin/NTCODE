using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Common
{
    /// <summary>
    /// 名称：AuditMonitoringPoint.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 审核监测点
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditMonitoringPointRepository : BaseGenericRepository<MonitoringBusinessModel, AuditMonitoringPointEntity>
    {
        /// <summary>
        /// 测点审核配置DAL
        /// </summary>
        AuditMonitoringPointDAL g_AuditMonitoringPointDAL = Singleton<AuditMonitoringPointDAL>.GetInstance();

        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return true;
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
            g_AuditMonitoringPointDAL.GetData(PointUids, MianLists, DetailLists, ApplicationUid, PointType);
        }
        #endregion
    }
}
