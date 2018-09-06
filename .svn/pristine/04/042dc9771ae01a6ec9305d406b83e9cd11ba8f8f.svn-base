using SmartEP.Core.Enums;
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
    public class AuditReasonRepository : BaseGenericRepository<MonitoringBusinessModel, AuditReasonEntity>
    {

        AuditReasonDAL g_AuditReasonDAL = new AuditReasonDAL();

        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return true;
        }

        /// <summary>
        /// 生成审核原因数据
        /// </summary>
        /// <param name="applicationType">设备类型Uid</param>
        /// <param name="KeyWords">关键字</param>
        /// <returns></returns>
        public DataTable GetAuditReasonData(ApplicationType applicationType, string KeyWords = null)
        {
            return g_AuditReasonDAL.GetAuditReasonData(applicationType, KeyWords);
        }
    }
}
