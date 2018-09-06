using SmartEP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness
{
    /// <summary>
    /// 名称：AuditReasonDAL.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-11-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境发布：审核原因配置
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditReasonDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        SmartEP.Utilities.AdoData.DatabaseHelper g_DatabaseHelper = SmartEP.Core.Generic.Singleton<SmartEP.Utilities.AdoData.DatabaseHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(Enums.DataConnectionType.MonitoringBusiness);

        #endregion

        /// <summary>
        /// 生成审核原因数据
        /// </summary>
        /// <param name="applicationType">设备类型Uid</param>
        /// <param name="KeyWords">关键字</param>
        /// <returns></returns>
        public DataTable GetAuditReasonData(ApplicationType applicationType, string KeyWords = null)
        {
            string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [Audit].[TB_AuditReason] where [ApplicationUid]='" + ApplicationUid + "' and [EnableOrNot]=1 ");
            if (!string.IsNullOrEmpty(KeyWords))
            {
                strSql.Append(" and [ReasonContent] like '%" + KeyWords + "%' ");
            }
            strSql.Append(" order by [OrderByNum] desc, [ReasonContent] asc ");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
    }
}
