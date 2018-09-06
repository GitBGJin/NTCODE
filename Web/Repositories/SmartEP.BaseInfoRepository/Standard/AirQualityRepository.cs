using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Standard
{
    public class AirQualityRepository
    {
        /// <summary>
        /// 空气标准处理DAL
        /// </summary>
        AirQualityDAL g_AirQualityDAL = Singleton<AirQualityDAL>.GetInstance();

        /// <summary>
        /// 计算空气污染指数
        /// </summary>
        /// <param name="pollutantCode">因子编码</param>
        /// <param name="pollutantValue">因子浓度</param>
        /// <param name="hourType">时间类型</param>
        /// <returns></returns>
        public decimal GetAQI(string pollutantCode, decimal pollutantValue, int hourType)
        {
            return g_AirQualityDAL.GetAQI(pollutantCode, pollutantValue, hourType);
        }

        /// <summary>
        /// 计算空气综合污染指数等（根据因子指数）
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
            return g_AirQualityDAL.GetAQI_Avg(AQI_SO2, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3_8, AQI_PM25, ReturnType);
        }

        /// <summary>
        /// 计算空气质量等级相关数据（根据因子指数）
        /// </summary>
        /// <param name="AQI_MaxValue">因子指数</param>
        /// <param name="ReturnType">返回值类型</param>
        /// <returns></returns>
        public string GetAQI_Grade(int AQI_MaxValue, string ReturnType)
        {
            return g_AirQualityDAL.GetAQI_Grade(AQI_MaxValue, ReturnType);
        }
    }
}
