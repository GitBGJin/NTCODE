using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Air
{
    /// <summary>
    /// 名称：AQIForcastConsultationSheetRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：预报--AQI预报时段详情仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AQIForcastConsultationSheetRepository : BaseGenericRepository<MonitoringBusinessModel, AQIForcastConsultationSheetEntity>
    {
        /// <summary>
        /// 根据主键Key判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return RetrieveCount(x => x.ConsultationSheetUid.Equals(strKey)) == 0 ? false : true;
        }
    }
}
