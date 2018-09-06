using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.Core.Generic;
using System.Data;

namespace SmartEP.Service.DataAnalyze.Air
{
    public class SuperStationInterfaceService
    {
        SuperStationInterfaceRepository g_SuperStationInterfaceRepository = Singleton<SuperStationInterfaceRepository>.GetInstance();
        #region 获取超级站接口
        public DataView getSSInterface()
        {
            try
            {
                return g_SuperStationInterfaceRepository.GetSSInterface();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 修改密码
        public void ModifyPassword(string password)
        {
            try
            {
                g_SuperStationInterfaceRepository.ModifyPassword(password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 更新状态
        public void ModifyStatus(int communicateStatus, int operateStatus, string interfaceName)
        {
            try
            {
                g_SuperStationInterfaceRepository.ModifyStatus(communicateStatus, operateStatus, interfaceName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        
    }
}
