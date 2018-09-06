using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Air
{
    public class FrequencyService
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        FrequencyRepository g_FrequencyRepository = Singleton<FrequencyRepository>.GetInstance();
        /// <summary>
        /// 更新配置数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ApplicationUid"></param>
        public void insertTable(DataTable dt, string ApplicationUid,string factorcode)
        {
            g_FrequencyRepository.insertTable(dt, ApplicationUid, factorcode);
        }
        /// <summary>
        /// 获取频数分布配置信息
        /// </summary>
        /// <param name="applicationtype"></param>
        /// <returns></returns>
        public DataView GetSetData(string ApplicationUid, string factorcode)
        {
            return g_FrequencyRepository.GetSetData(ApplicationUid, factorcode);
        }
    }
}
