using SmartEP.Core.Interfaces;
using SmartEP.Data.Enums;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.BusinessRule
{
    /// <summary>
    /// 离线配置
    /// </summary>
    public class OfflineSettingRepository : BaseGenericRepository<BaseDataModel, OfflineSettingEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.OffLineSettingUid.Equals(strKey)).Count() == 0 ? false : true;
        }

        /// <summary>
        /// 取得虚拟分页数据和总行数
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="whereString">WHERE条件</param>
        /// <param name="recordTotal">数据总行数</param>
        /// <returns></returns>
        public DataView GetGridViewPager(int pageSize, int pageNo, string whereString, out int recordTotal)
        {
            return new GridViewPagerDAL().GetGridViewPager("V_OfflineSetting", "*", "OffLineSettingUid", pageSize, pageNo, "OrderByNum desc"
                , whereString, SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData), out recordTotal);
        }

        /// <summary>
        /// 取得导出数据
        /// </summary>
        /// <param name="whereString"></param>
        /// <returns></returns>
        public DataTable GetGridDataAll(string whereString)
        {
            int recordTotal = 0;
            StringBuilder sbSelect = new StringBuilder();
            sbSelect.AppendLine("monitoringPointName AS '测点'");
            sbSelect.AppendLine(",OffLineTimeSpan AS '离线判定(分)'");
            sbSelect.AppendLine(",CASE WHEN EnableOrNot =1 THEN '是' ELSE '否' END AS '是否启用'");
            sbSelect.AppendLine(",CASE WHEN NotifyOrNot =1 THEN '是' ELSE '否' END AS '是否通知'");
            sbSelect.AppendLine(",Description AS '描述'");
            DataView dvAll = new GridViewPagerDAL().GetGridViewPager("V_OfflineSetting", sbSelect.ToString(), "OffLineSettingUid", 999999999, 1, "OrderByNum desc", whereString, SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData), out recordTotal);
            DataTable dtAll = dvAll.ToTable();
            if (dtAll.Columns[0].ColumnName == "rows")
            {
                dtAll.Columns[0].ColumnName = "序号";
            }
            return dtAll;
        }
    }
}
