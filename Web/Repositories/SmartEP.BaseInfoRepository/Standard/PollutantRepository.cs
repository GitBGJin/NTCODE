using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Standard
{
    /// <summary>
    /// 名称：PollutantRepository.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-11-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境发布：污染因子数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class PollutantRepository : BaseGenericRepository<BaseDataModel, V_Point_InstrumentChannelEntity>
    {
        /// <summary>
        /// 数据接口
        /// </summary>
        PollutantCodeDAL g_PollutantCodeDAL = Singleton<PollutantCodeDAL>.GetInstance();

        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.MonitoringPointUid.Equals(strKey)).Count() == 0 ? false : true;
        }

        #region 获取代码项配置中的因子数据
        /// <summary>
        /// 获取代码项配置中的因子数据
        /// </summary>
        /// <param name="typeName">代码分类</param>
        /// <param name="codeName">代码名称</param>
        /// <returns>DataTable</returns>
        public DataTable GetPollutantDataByItem(string typeName, string codeName)
        {
            return g_PollutantCodeDAL.GetPollutantDataByItem(typeName, codeName);
        }
        #endregion
    }
}
