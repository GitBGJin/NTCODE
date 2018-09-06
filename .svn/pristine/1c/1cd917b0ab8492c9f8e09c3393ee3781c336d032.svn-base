using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.Core.Generic;
using System.Data;

namespace SmartEP.MonitoringBusinessRepository.Air
{
    /// <summary>
    /// 名称：SuperStationInterfaceRepository.cs
    /// 创建人：蒋月
    /// 创建日期：2017-2-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 超级站接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SuperStationInterfaceRepository
    {
        SuperStationInterfaceDAL d_SuperStationInterfaceDAL = Singleton<SuperStationInterfaceDAL>.GetInstance();
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        public DataView GetSSInterface()
        {
            return d_SuperStationInterfaceDAL.GetSSInterface();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password"></param>
        public void ModifyPassword(string password)
        {
            d_SuperStationInterfaceDAL.ModifyPassword(password);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="password"></param>
        public void ModifyStatus(int communicateStatus, int operateStatus, string interfaceName)
        {
            d_SuperStationInterfaceDAL.ModifyStatus(communicateStatus, operateStatus, interfaceName);
        }


        
    }
}
