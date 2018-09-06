using SmartEP.Core.Enums;
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
    /// 名称：EQIConcentrationLimitDAL.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-12-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：获取因子对应的分类标准
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class EQIConcentrationLimitDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);

        #region 获取因子对应标准的评价范围
        /// <summary>
        /// 获取因子对应标准的评价范围
        /// </summary>
        /// <param name="IEQI">评价标准</param>
        /// <param name="PollutantCodes">因子Code数组</param>
        /// <param name="applicationtype">应用类型</param>
        /// <param name="waterPointCalWQType">水域类型</param>
        /// 河流"d8197909-568e-4319-874c-3ad7cbc92a7e"
        /// 湖、库"e82cd86f-71ba-4f87-8e5c-6ac7ca055a6b"
        /// <returns></returns>
        public DataTable GetIEQIByPollutantCodes(List<int> IEQI, List<string> PollutantCodes, ApplicationType applicationtype, string waterPointCalWQType)
        {
            string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationtype);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.IEQI,a.Class ");
            if (PollutantCodes != null)
            {
                foreach (string code in PollutantCodes)
                {
                    if (code == "w01001")//ph
                    {
                        strSql.Append(",'6～9' as '" + code + "'");
                    }
                    else if (code == "w01009")//溶解氧（取下限）
                    {
                        strSql.Append(",(select top 1 Low ");
                        strSql.Append("from [Standard].[TB_EQIConcentrationLimit] ");
                        strSql.Append("where PollutantCode='" + code + "' ");
                        strSql.Append("and EQIUid=a.EQIUid and PointAttributeUid='" + waterPointCalWQType + "') as '" + code + "' ");
                    }
                    else//其它（取上限）
                    {
                        strSql.Append(",(select top 1 Upper ");
                        strSql.Append("from [Standard].[TB_EQIConcentrationLimit] ");
                        strSql.Append("where PollutantCode='" + code + "' ");
                        strSql.Append("and EQIUid=a.EQIUid and PointAttributeUid='" + waterPointCalWQType + "') as '" + code + "' ");
                    }
                }
            }
            strSql.Append("from [Standard].[TB_EQI] a ");
            strSql.Append("where a.ApplicationUid='" + ApplicationUid + "' and a.RowStatus=1 ");
            if (IEQI.Count > 0)
            {
                if (IEQI.Count == 1)
                {
                    strSql.Append("and a.IEQI='" + IEQI[0] + "' ");
                }
                else
                {
                    strSql.Append("and (");
                    for (int i = 0; i < IEQI.Count; i++)
                    {
                        if (i == 0)
                        {
                            strSql.Append(" a.IEQI='" + IEQI[i] + "' ");
                        }
                        else
                        {
                            strSql.Append(" or a.IEQI='" + IEQI[i] + "' ");
                        }
                    }
                    strSql.Append(")");
                }
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion
    }
}
