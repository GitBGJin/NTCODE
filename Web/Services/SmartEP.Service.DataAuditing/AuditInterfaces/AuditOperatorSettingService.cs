using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.MonitoringBusinessRepository.Common;
using SmartEP.DomainModel.MonitoringBusiness;

namespace SmartEP.Service.DataAuditing.AuditInterfaces
{
    /// <summary>
    /// 名称：AuditOperatorSettingService.cs
    /// 创建人：徐龙超
    /// 创建日期：2016-04-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    ///人工审核操作配置
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditOperatorSettingService
    {
        AuditOperatorSettingRepository operatorRep = new AuditOperatorSettingRepository();

        /// <summary>
        /// 获取人工审核操作列表
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="AuditType"></param>
        /// <returns></returns>
        public IQueryable<V_AuditOperatorSettingEntity> GetAuditOperatorList(string ApplicationUid, string AuditType)
        {
            IQueryable<V_AuditOperatorSettingEntity> list = operatorRep.Context.GetAll<V_AuditOperatorSettingEntity>().Where(p => p.ApplicationUid == ApplicationUid && p.AuditType.Equals(AuditType)).OrderBy(p=>p.OrderByNum);
            return list;
        }
        /// <summary>
        /// 获取设备Uid下的所有审核理由
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <returns></returns>
        public IQueryable<V_AuditOperatorSettingEntity> GetAuditReasonList(string ApplicationUid, string AuditOperatorUid)
        {
            return operatorRep.Context.GetAll<V_AuditOperatorSettingEntity>().Where(x => x.ApplicationUid == ApplicationUid && x.AuditOperatorUid == AuditOperatorUid);
        }
        /// <summary>
        /// 获取审核操作对应的标记位
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="AuditType"></param>
        /// <returns></returns>
        public V_AuditOperatorSettingEntity GetAuditOperator(string AuditOperatorUid)
        {
           V_AuditOperatorSettingEntity entity = operatorRep.Context.GetAll<V_AuditOperatorSettingEntity>().Where(p => p.AuditOperatorUid.Equals(AuditOperatorUid)).FirstOrDefault();
           return entity;
        }

    }
}
