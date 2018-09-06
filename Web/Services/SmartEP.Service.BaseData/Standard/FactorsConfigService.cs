using SmartEP.BaseInfoRepository.Standard;
using SmartEP.Core.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.Standard
{
    public class FactorsConfigService
    {
        FactorsConfigRepository FactorsConfig = Singleton<FactorsConfigRepository>.GetInstance();

        /// <summary>
        /// 获取点位因子对照表
        /// </summary>
        /// <param name="PageTypeId">页面Id</param>
        /// <param name="PointIds"></param>
        /// <returns></returns>
        public DataView GetFactorsById(string PageTypeId, List<int> PointIds)
        {
            return FactorsConfig.GetFactorsById(PageTypeId, PointIds);
        }

         /// <summary>
        /// 获取最大化因子编码
        /// </summary>
        /// <param name="PageTypeId"></param>
        /// <param name="PointIds"></param>
        /// <returns></returns>
        public DataView GetMaxFactors(string PageTypeId, List<int> PointIds)
        {
            return FactorsConfig.GetMaxFactors(PageTypeId, PointIds);
        }

    }
}
