using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.BaseData
{
    public class FactorsConfigDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 虚拟分页类
        /// </summary>
        GridViewPagerDAL g_GridViewPager = Singleton<GridViewPagerDAL>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_BaseDataConnection";

        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public FactorsConfigDAL()
        {
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 获取点位因子对照表
        /// </summary>
        /// <param name="PageTypeId">页面Id</param>
        /// <param name="PointIds"></param>
        /// <returns></returns>
        public DataView GetFactorsById(string PageTypeId, List<int> PointIds)
        {
            try
            {
                string strPointIds = "";
                for (int i = 0; i < PointIds.Count; i++)
                {
                    strPointIds += PointIds[i] + ",";
                }
                strPointIds = strPointIds.TrimEnd(',');
                string strSql = string.Format(@"select PointId,PollutantCodes,CategoryUid,OrderByNum,DecimalDigit from dbo.TB_FactorsRelationConfig 
                                 where PageTypeId='{0}' and PointId in({1}) and EnableOrNot=1 and RowStatus=1
                                 order by PointId,OrderByNum", PageTypeId, strPointIds);
                return g_DatabaseHelper.ExecuteDataView(strSql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取最大化因子编码
        /// </summary>
        /// <param name="PageTypeId"></param>
        /// <param name="PointIds"></param>
        /// <returns></returns>
        public DataView GetMaxFactors(string PageTypeId, List<int> PointIds)
        {
            try
            {
                string strPointIds = "";
                for (int i = 0; i < PointIds.Count; i++)
                {
                    strPointIds += PointIds[i] + ",";
                }
                strPointIds = strPointIds.TrimEnd(',');
                string strSql = string.Format(@"select distinct PollutantCodes from dbo.TB_FactorsRelationConfig 
                                where PageTypeId='{0}' and PointId in({1}) and EnableOrNot=1 and RowStatus=1", PageTypeId, strPointIds);
                return g_DatabaseHelper.ExecuteDataView(strSql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
