using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Data.SqlServer.MonitoringBusiness;
namespace SmartEP.MonitoringBusinessRepository.Common
{
    public class AuditPollutantRepository
    {
        AuditPollutantDAL pollutantDAL = new AuditPollutantDAL();
        /// <summary>
        /// 获取审核因子配置数量
        /// </summary>
        /// <param name="pointID"></param>
        /// <param name="PointType"></param>
        /// <returns></returns>
        public string[] GetAuditFactorsCount(int pointID, string applicationUid, string PointType = "0")
        {
            return pollutantDAL.GetAuditFactorsCount(pointID,applicationUid,PointType);
        }
    }
}
