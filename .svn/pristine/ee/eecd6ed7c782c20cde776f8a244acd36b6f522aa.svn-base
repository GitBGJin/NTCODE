using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness.Water;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Water
{
    /// <summary>
    /// 名称：AuditInfectantByMinuteRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 空气审核分钟数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditWaterInfectantByMinuteRepository : BaseGenericRepository<MonitoringBusinessModel, AuditWaterInfectantByMinuteEntity>
    {
        /// <summary>
        /// 地表水审核分钟数据DAL
        /// </summary>
        AuditWaterInfectantByMinuteDAL g_AuditWaterInfectantByMinute = Singleton<AuditWaterInfectantByMinuteDAL>.GetInstance();

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
        /// 行转列数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetDataView(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return g_AuditWaterInfectantByMinute.GetDataView(portIds, factors, dtStart, dtEnd);
        }
    }
}
