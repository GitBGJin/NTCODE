using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.BaseData
{
    /// <summary>
    /// 名称：PollutantCodeDAL.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-12-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：因子数据
    /// 环境发布：
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class PollutantCodeDAL
    {
        #region 变量
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
        #endregion

        #region 方法
        /// <summary>
        /// 获取代码项配置中的因子数据
        /// </summary>
        /// <param name="typeName">代码分类</param>
        /// <param name="codeName">代码名称</param>
        /// <returns>DataTable</returns>
        public DataTable GetPollutantDataByItem(string typeName, string codeName)
        {
            if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(codeName))
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select item.ItemValue,item.ItemText ");
            strSql.Append(",p.PollutantName,p.PollutantUid,unititem.ItemText as Unit ");
            strSql.Append("from dbo.SY_View_CodeMainItem item ");
            strSql.Append("inner join [Standard].[TB_PollutantCode] p on item.ItemValue=p.PollutantCode and p.IsUseOrNot=1 ");
            strSql.Append("left join dbo.SY_View_CodeMainItem unititem on p.MeasureUnitUid=unititem.ItemGuid ");
            strSql.Append("where item.TypeName='" + typeName + "' and item.CodeName ='" + codeName + "' ");
            strSql.Append("order by item.SortNumber desc");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion
    }
}
