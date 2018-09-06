using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Air
{
    /// <summary>
    /// 名称：OM_TaskItemConfigService.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-04-02
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 运维任务项配置服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class OM_TaskItemConfigService
    {
        /// <summary>
        /// 运维任务项配置表仓储层
        /// </summary>
        OM_TaskItemConfigRepository r_OM_TaskItemConfigRepository = Singleton<OM_TaskItemConfigRepository>.GetInstance();
        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere">查询条件（例如：1=1）</param>
        /// <returns></returns>
        public DataTable GetList(string FormCode, string Type, string TaskType)
        {
            string strWhere = "IsUsed=1 and RowStatus=1";
            strWhere += string.Format(" and Type='{0}' ", Type);
            strWhere += string.Format(" and TaskType='{0}' ", TaskType);
            strWhere += string.Format(" and FormCode='{0}' ", FormCode);
            return r_OM_TaskItemConfigRepository.GetList(strWhere);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere">查询条件（例如：1=1）</param>
        /// <returns></returns>
        public DataTable GetListTask(string FormCode, string FormName)
        {
            string strWhere = "RowStatus='1'";
            if (!string.IsNullOrWhiteSpace(FormCode))
            {
                strWhere += string.Format(" and FormCode='{0}' ", FormCode);
            }
            if (!string.IsNullOrWhiteSpace(FormName))
            {
                strWhere += string.Format(" and MissionName='{0}' ", FormName);
            }
            return r_OM_TaskItemConfigRepository.GetListTask(strWhere);
        }
    }
}
