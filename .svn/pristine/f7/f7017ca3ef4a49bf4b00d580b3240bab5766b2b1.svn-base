using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Standard
{
    public class FactorsConfigRepository
    {
        #region << ADO.NET >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        FactorsConfigDAL m_FactorsConfigDal = Singleton<FactorsConfigDAL>.GetInstance();

        #region << 方法 >>
        /// <summary>
        /// 获取点位因子对照表
        /// </summary>
        /// <param name="PageTypeId">页面Id</param>
        /// <param name="PointIds"></param>
        /// <returns></returns>
        public DataView GetFactorsById(string PageTypeId, List<int> PointIds)
        {
            return m_FactorsConfigDal.GetFactorsById(PageTypeId, PointIds);
        }

        /// <summary>
        /// 获取最大化因子编码
        /// </summary>
        /// <param name="PageTypeId"></param>
        /// <param name="PointIds"></param>
        /// <returns></returns>
        public DataView GetMaxFactors(string PageTypeId, List<int> PointIds)
        {
            return m_FactorsConfigDal.GetMaxFactors(PageTypeId, PointIds);
        }

        #endregion
        #endregion

    }
}
