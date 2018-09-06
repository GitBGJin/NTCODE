using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.BaseData
{
    public class AirQualityDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
        #endregion

        /// <summary>
        /// 计算空气污染指数
        /// </summary>
        /// <param name="pollutantCode">因子编码</param>
        /// <param name="pollutantValue">因子浓度</param>
        /// <param name="hourType">时间类型</param>
        /// <returns></returns>
        public decimal GetAQI(string pollutantCode, decimal pollutantValue, int hourType)
        {
            string sql = string.Format("select dbo.f_getAQI('{0}','{1}',{2})", pollutantValue, pollutantCode, hourType);
            object ret = g_DatabaseHelper.ExecuteScalar(sql, connection);
            if (ret != null && ret != DBNull.Value)
                return Convert.ToDecimal(ret);
            return 0M;
        }

        /// <summary>
        ///  计算空气综合污染指数等（根据因子指数）
        /// </summary>
        /// <param name="AQI_SO2">SO2指数</param>
        /// <param name="AQI_NO2">NO2指数</param>
        /// <param name="AQI_PM10">PM10指数</param>
        /// <param name="AQI_CO">CO指数</param>
        /// <param name="AQI_O3_8">O3_8指数</param>
        /// <param name="AQI_PM25">PM25指数</param>
        /// <param name="ReturnType">返回数据类型</param>
        /// <returns></returns>
        public string GetAQI_Avg(int AQI_SO2, int AQI_NO2, int AQI_PM10, int AQI_CO, int AQI_O3_8, int AQI_PM25, string ReturnType)
        {
            string sql = string.Format("select dbo.F_GetAQI_Max_CNV_Day({0},{1},{2},{3},{4},{5},'')", AQI_SO2, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3_8, AQI_PM25, ReturnType);
            object ret = g_DatabaseHelper.ExecuteScalar(sql, connection);
            if (ret != null && ret != DBNull.Value)
                return ret.ToString();
            return string.Empty;
        }

        /// <summary>
        /// 计算空气质量等级相关数据（根据因子指数）
        /// </summary>
        /// <param name="AQI_MaxValue">因子指数</param>
        /// <param name="ReturnType">返回值类型</param>
        /// <returns></returns>
        public string GetAQI_Grade(int AQI_MaxValue, string ReturnType)
        {
            string sql = string.Format("select dbo.F_GetAQI_Grade({0},'{1}')", AQI_MaxValue, ReturnType);
            object ret = g_DatabaseHelper.ExecuteScalar(sql, connection);
            if (ret != null && ret != DBNull.Value)
                return ret.ToString();
            return string.Empty;
        }

    }}
