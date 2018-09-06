using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.BaseData
{
    /// <summary>
    /// 名称：BreakSettingDAL.cs
    /// 创建人：朱佳伟
    /// 创建日期：2016-01-13
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：突变规则设置
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class BreakSettingDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);

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
            StringBuilder strSql = new StringBuilder();
            if (ColumnName == "CompareBeforeGroups" || ColumnName == "BreakMultipleForUpper" || ColumnName == "AdvanceUpper"
                || ColumnName == "BreakMultipleForLow" || ColumnName == "AdvanceLow")
            {
                if (string.IsNullOrEmpty(Value) || Convert.ToDecimal(Value) == 0)
                {
                    strSql.Append(" if exists(select * from [BusinessRule].[TB_BreakSetting] ");
                    strSql.Append(" where ApplicationUid='" + ApplicationUid + "' and DataTypeUid='" + DataTypeUid + "' ");
                    strSql.Append(" and monitoringPointUid='" + PointUid + "' and InstrumentChannelsUid='" + PollutantUid + "') ");
                    strSql.Append(" begin ");
                    strSql.Append(" Update [BusinessRule].[TB_BreakSetting] set [" + ColumnName + "] = NULL ");
                    strSql.Append(" where ApplicationUid='" + ApplicationUid + "' and DataTypeUid='" + DataTypeUid + "' ");
                    strSql.Append(" and monitoringPointUid='" + PointUid + "' and InstrumentChannelsUid='" + PollutantUid + "' ");
                    strSql.Append(" end ");
                }
                else
                {
                    strSql.Append(" if exists(select * from [BusinessRule].[TB_BreakSetting] ");
                    strSql.Append(" where ApplicationUid='" + ApplicationUid + "' and DataTypeUid='" + DataTypeUid + "' ");
                    strSql.Append(" and monitoringPointUid='" + PointUid + "' and InstrumentChannelsUid='" + PollutantUid + "') ");
                    strSql.Append(" begin ");
                    strSql.Append(" Update [BusinessRule].[TB_BreakSetting] set [" + ColumnName + "] = '" + Convert.ToDecimal(Value) + "' ");
                    strSql.Append(" where ApplicationUid='" + ApplicationUid + "' and DataTypeUid='" + DataTypeUid + "' ");
                    strSql.Append(" and monitoringPointUid='" + PointUid + "' and InstrumentChannelsUid='" + PollutantUid + "' ");
                    strSql.Append(" end ");
                    strSql.Append(" else ");
                    strSql.Append(" begin ");
                    strSql.Append(" insert into [BusinessRule].[TB_BreakSetting] ([RowGuid],[BreakUid],[ApplicationUid],[DataTypeUid],[monitoringPointUid],[InstrumentChannelsUid] ");
                    strSql.Append(" ,[" + ColumnName + "],[AddDate],[RowStatus]) ");
                    strSql.Append(" values('" + Guid.NewGuid().ToString() + "','" + Guid.NewGuid().ToString() + "','" + ApplicationUid + "','" + DataTypeUid + "','" + PointUid + "','" + PollutantUid + "' ");
                    strSql.Append(" ,'" + Value + "',GETDATE(),1)");
                    strSql.Append(" end ");
                }
            }
            if (ColumnName == "ReplaceValue" || ColumnName == "EnableOrNot" || ColumnName == "NotifyOrNot")
            {
                strSql.Append(" if exists(select * from [BusinessRule].[TB_BreakSetting] ");
                strSql.Append(" where ApplicationUid='" + ApplicationUid + "' and DataTypeUid='" + DataTypeUid + "' ");
                strSql.Append(" and monitoringPointUid='" + PointUid + "' and InstrumentChannelsUid='" + PollutantUid + "') ");
                strSql.Append(" begin ");
                strSql.Append(" Update [BusinessRule].[TB_BreakSetting] set [" + ColumnName + "] = '" + Value + "' ");
                strSql.Append(" where ApplicationUid='" + ApplicationUid + "' and DataTypeUid='" + DataTypeUid + "' ");
                strSql.Append(" and monitoringPointUid='" + PointUid + "' and InstrumentChannelsUid='" + PollutantUid + "' ");
                strSql.Append(" end ");
                strSql.Append(" else ");
                strSql.Append(" begin ");
                strSql.Append(" insert into [BusinessRule].[TB_BreakSetting] ([RowGuid],[BreakUid],[ApplicationUid],[DataTypeUid],[monitoringPointUid],[InstrumentChannelsUid] ");
                strSql.Append(" ,[" + ColumnName + "],[AddDate],[RowStatus]) ");
                strSql.Append(" values('" + Guid.NewGuid().ToString() + "','" + Guid.NewGuid().ToString() + "','" + ApplicationUid + "','" + DataTypeUid + "','" + PointUid + "','" + PollutantUid + "' ");
                strSql.Append(" ,'" + Value + "',GETDATE(),1)");
                strSql.Append(" end ");
            }
            strSql.Append(" delete from [BusinessRule].[TB_BreakSetting] where ([CompareBeforeGroups]=0 or [CompareBeforeGroups] is null) ");
            strSql.Append(" and ([BreakMultipleForLow]=0 or [BreakMultipleForLow] is null ) ");
            strSql.Append(" and ([AdvanceLow]=0 or [AdvanceLow] is null ) ");
            strSql.Append(" and ([BreakMultipleForUpper]=0 or [BreakMultipleForUpper] is null ) ");
            strSql.Append(" and ([AdvanceUpper]=0 or [AdvanceUpper] is null ) ");
            g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
        }
    }
}
