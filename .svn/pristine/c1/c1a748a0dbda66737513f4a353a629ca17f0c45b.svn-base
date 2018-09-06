using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.BusinessRule
{
    public class BreakSettingRepository : BaseGenericRepository<BaseDataModel, BreakSettingEntity>
    {
        /// <summary>
        /// 数据处理借口
        /// </summary>
        BreakSettingDAL g_BreakSettingDAL = Singleton<BreakSettingDAL>.GetInstance();

        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.BreakUid.Equals(strKey)).Count() == 0 ? false : true;
        }

        /// <summary>
        /// 设置突变
        /// </summary>
        /// <param name="ApplicationUid">系统Uid</param>
        /// <param name="DataTypeUid">数据类型Uid</param>
        /// <param name="PointUid">测点Uid</param>
        /// <param name="PollutantUid">因子Uid</param>
        /// <param name="ColumnName">设置属性</param>
        /// <param name="Value">设置值</param>
        public void InsertOrUpdate(string ApplicationUid, string DataTypeUid, string PointUid, string PollutantUid, string ColumnName, string Value)
        {
            g_BreakSettingDAL.InsertOrUpdate(ApplicationUid, DataTypeUid, PointUid, PollutantUid, ColumnName, Value);
        }
    }
}
