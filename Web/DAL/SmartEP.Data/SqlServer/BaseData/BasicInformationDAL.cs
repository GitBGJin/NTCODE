using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.BaseData
{
    /// <summary>
    /// 名称：BasicInformationDAL.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-10-14
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 基本信息数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class BasicInformationDAL
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
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);

        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.TB_PersonalizedSettings";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public BasicInformationDAL()
        {
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 获取基本信息-个性化设置的数据
        /// </summary>
        /// <param name="applicationType">应用程序类型（水、气）</param>
        /// <param name="userUids">UserUid数组</param>
        /// <param name="parameterType">参数类型名称（1、port 2、factor 3、instrument 4、status）</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetPersonalizedSettingsData(ApplicationType? applicationType, string[] userUids, string parameterType, string orderBy = "OrderByNum")
        {
            try
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                string applicationTypeStr = string.Empty;
                string userUidsStr = StringExtensions.GetArrayStrNoEmpty(userUids.ToList<string>(), "','");
                string parameterTypeStr = string.Empty;
                //string keyName = "PersonalizedSettingUid";
                string fieldName = @"PersonalizedSettingUid,ApplicationUid,UserUid,ParameterUid,ParameterType,EnableCustomOrNot,
                                     OrderByNum,Description,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime";
                string where = string.Empty;//查询条件拼接

                if (applicationType == ApplicationType.Air)
                {
                    applicationTypeStr = " AND ApplicationUid='airaaira-aira-aira-aira-airaairaaira' ";
                }
                else if (applicationType == ApplicationType.Water)
                {
                    applicationTypeStr = " AND ApplicationUid='watrwatr-watr-watr-watr-watrwatrwatr' ";
                }
                if (userUids.Length == 1 && !string.IsNullOrEmpty(userUids[0]))
                {
                    userUidsStr = " AND UserUid ='" + userUidsStr + "'";
                }
                else if (!string.IsNullOrEmpty(userUidsStr))
                {
                    userUidsStr = " AND UserUid IN('" + userUidsStr + "')";
                }
                if (!string.IsNullOrWhiteSpace(parameterType))
                {
                    parameterTypeStr = string.Format(" AND  ParameterType='{0}'", parameterType);
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "OrderByNum" : orderBy;
                where = string.Format(" 1=1 {0} {1} {2} ", applicationTypeStr, userUidsStr, parameterTypeStr);
                sqlStringBuilder.AppendFormat(" SELECT {0} FROM {1} WHERE {2} ORDER BY {3} ", fieldName, tableName, where, orderBy);
                return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataView GetApproveMappingInfo()
        {
            string sql = "select Name,(IpAddress + ';' + ApproveId + ';' + ApprovePwd) as Value from DT_ApproveMapping";
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
        #endregion  Method
    }
}
