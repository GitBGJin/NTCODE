using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SmartEP.MonitoringBusinessRepository.Water
{
    /// <summary>
    /// 名称：MonitoringBusinessRepository.cs
    /// 创建人：王琳
    /// 创建日期：2015-10-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 本周运维数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MaintenanceDataRepository
    {
        /// <summary>
        /// 本周运维数据DAL
        /// </summary>
        MaintenanceDataDAL g_MaintenanceData = Singleton<MaintenanceDataDAL>.GetInstance();

        /// <summary>
        /// 本周运维数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public DataView GetMaintenanceData(string[] portIds,DateTime beginTime,DateTime endTime)
        {
            return g_MaintenanceData.GetMaintenanceData(portIds, beginTime, endTime);
        }
    }
}
