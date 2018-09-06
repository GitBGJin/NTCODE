using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.MPInfo
{
    public class PortInfoRepository
    {
        PortInfoDAL g_PortInfoDAL = Singleton<PortInfoDAL>.GetInstance();
        /// <summary>
        /// 根据用户权限获取站点
        /// </summary>
        /// <param name="userguid"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public DataTable GetPointByUserGuid(string userguid, string datatype)
        {
            return g_PortInfoDAL.GetPointByUserGuid(userguid, datatype);
        }
        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="SYSType"></param>
        /// <param name="pointGuid"></param>
        /// <returns></returns>
        public string GetPortInfo(string SYSType, string pointGuid)//已更新的方法
        {
            return g_PortInfoDAL.GetPortInfo(SYSType, pointGuid);
        }
    }
}
