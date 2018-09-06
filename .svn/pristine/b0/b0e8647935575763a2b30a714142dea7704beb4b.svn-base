using SmartEP.MonitoringBusinessRepository.Water;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.Core.Generic;

namespace SmartEP.Service.AutoMonitoring.Water
{
    /// <summary>
    /// 名称：MaintenanceDataService.cs
    /// 创建人：王琳
    /// 创建日期：2015-10-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 本周运维数据服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MaintenanceDataService
    {
        /// <summary>
        /// 本周运维数据仓储层
        /// </summary>
        MaintenanceDataRepository g_MaintenanceDataRepository = Singleton<MaintenanceDataRepository>.GetInstance();

        #region << ORM >>
        #endregion

        #region << ADO.NET >>
        /// <summary>
        /// 本周运维数据
        /// </summary>
        /// <param name="portIds">点位Id</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public DataView GetMaintenanceData(string[] portIds,DateTime beginTime,DateTime endTime)
        {
            return g_MaintenanceDataRepository.GetMaintenanceData(portIds, beginTime, endTime);
        }

        #endregion
    }
}
