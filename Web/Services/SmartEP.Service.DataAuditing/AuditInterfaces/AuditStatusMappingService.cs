using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.MonitoringBusinessRepository.Common;
using SmartEP.DomainModel.MonitoringBusiness;

namespace SmartEP.Service.DataAuditing.AuditInterfaces
{
    /// <summary>
    /// 名称：AuditStatusMappingService.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-08-16
    /// 维护人员：
    /// 最新维护人员：徐龙超
    /// 最新维护日期：2015-08-27
    /// 功能摘要：
    /// 审核站点抽象类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    /// 
    public class AuditStatusMappingService
    {
        AuditStatusMappingRepository statusRep = new AuditStatusMappingRepository();
        public IQueryable<AuditStatusMappingEntity> GetDataStatusMappingList(string ApplicationUid)
        {
            return statusRep.Retrieve(x => x.ApplicationUid == ApplicationUid && x.IsUsed == true).OrderBy(x=>x.OrderByNum);
        }

        /// <summary>
        /// 根据审核标记获取对应标记名称
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="AuditFlag"></param>
        /// <returns></returns>
        public IQueryable<AuditStatusMappingEntity> GetDataStatusMappingList(string ApplicationUid,string AuditFlag)
        {
            string[] flags = AuditFlag.ToUpper().Split(',');
            return statusRep.Retrieve(x => x.ApplicationUid == ApplicationUid && x.IsUsed == true && flags.Contains(x.StatusIdentify.ToUpper())).OrderBy(x => x.OrderByNum);
        }
    }
}
