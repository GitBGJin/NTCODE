using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository
{
    public class FrequencyRepository
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        FrequencySettingDAL g_FrequencySettingDAL = Singleton<FrequencySettingDAL>.GetInstance();
        /// <summary>
        /// 更新配置数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ApplicationUid">应用参数类型</param>
        public void insertTable(DataTable dt, string ApplicationUid, string factorcode)
        {
            g_FrequencySettingDAL.insertTable(dt, ApplicationUid, factorcode);
        }
        /// <summary>
        /// 获取频数分布配置信息
        /// </summary>
        /// <param name="ApplicationUid">应用参数类型</param>
        /// <returns></returns>
        public DataView GetSetData(string ApplicationUid, string factorcode)
        {
            return g_FrequencySettingDAL.GetSetData(ApplicationUid, factorcode);
        }
    }
}
