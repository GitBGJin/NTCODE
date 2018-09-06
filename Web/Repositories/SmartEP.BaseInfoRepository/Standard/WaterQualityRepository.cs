using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Standard
{
    /// <summary>
    /// 名称：WaterQualityRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-09-06
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 水质标准处理仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterQualityRepository
    {
        /// <summary>
        /// 水质标准处理DAL
        /// </summary>
        WaterQualityDAL g_WaterQualityDAL = Singleton<WaterQualityDAL>.GetInstance();

        /// <summary>
        /// 水质分析数据处理DAL
        /// </summary>
        WaterAnalyzeDAL g_WaterAnalyzeDAL = Singleton<WaterAnalyzeDAL>.GetInstance();

        /// <summary>
        /// 计算水质污染指数
        /// </summary>
        /// <param name="pollutantCode">因子编码</param>
        /// <param name="pollutantValue">因子浓度</param>
        /// <param name="iEQI">评价水质类别</param>
        /// <param name="hourType">时间类型</param>
        /// <param name="waterPointCalWQType">地表水水质评价站点属性类型</param>
        /// <returns></returns>
        public decimal GetWQI(string pollutantCode, decimal pollutantValue, Int32 iEQI, string hourType, string waterPointCalWQType)
        {
            return g_WaterQualityDAL.GetWQI(pollutantCode, pollutantValue, iEQI, hourType, waterPointCalWQType);
        }

        /// <summary>
        /// 获取首要染污物等(根据因子指数)
        /// </summary>
        /// <param name="ReturnType">返回类型（CODE、NAME、ENAME、VALUE、SIMPLE）</param>
        /// <param name="EvaluateFactorCodes">参与评价因子，以“;”分割</param>
        /// <param name="WQIValues">因子指数列表（Key:PollutantCode、Value:分指数）</param>
        /// <returns></returns>
        public string GetWQI_Max(string ReturnType, string EvaluateFactorCodes, Dictionary<string, Decimal> WQIValues)
        {
            return g_WaterQualityDAL.GetWQI_Max(ReturnType, EvaluateFactorCodes, WQIValues);
        }

        /// <summary>
        /// 计算综合污染指数(根据因子指数)
        /// </summary>
        /// <param name="EvaluateFactorCodes">参与评价因子，以“;”分割</param>
        /// <param name="WQIValues">因子指数列表（Key:PollutantCode、Value:分指数）</param>
        /// <returns></returns>
        public string GetWQI_Avg(string EvaluateFactorCodes, Dictionary<string, Decimal> WQIValues)
        {
            return g_WaterQualityDAL.GetWQI_Avg(EvaluateFactorCodes, WQIValues);
        }

        /// <summary>
        /// 计算水质等级
        /// </summary>
        /// <param name="pollutantCode">因子编码</param>
        /// <param name="pollutantValue">因子浓度</param>
        /// <param name="hourType">时间类型</param>
        /// <param name="waterPointCalWQType">地表水水质评价站点属性类型</param>
        /// <param name="returnType">返回值类型</param>
        /// <returns></returns>
        public string GetWQL(string pollutantCode, decimal pollutantValue, string hourType, string waterPointCalWQType, string returnType)
        {
            return g_WaterQualityDAL.GetWQL(pollutantCode, pollutantValue, hourType, waterPointCalWQType, returnType);
        }

        /// <summary>
        /// 获取首要染污物等(根据因子等级)
        /// </summary>
        /// <param name="ReturnType">返回类型（CODE、NAME、ENAME、VALUE、SIMPLE）</param>
        /// <param name="EvaluateFactorCodes">参与评价因子，以“;”分割</param>
        /// <param name="WQLValues">因子指数列表（Key:PollutantCode、Value:单因子等级）</param>
        /// <returns></returns>
        public string GetWQL_Max(string ReturnType, string EvaluateFactorCodes, Dictionary<string, Int32> WQLValues)
        {
            return g_WaterQualityDAL.GetWQL_Max(ReturnType, EvaluateFactorCodes, WQLValues);
        }               
     
        /// <summary>
        /// 根据站点获取水质分析数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <returns></returns>
        public DataView GetWaterAnalyzeData(string[] portIds)
        {
            return g_WaterAnalyzeDAL.GetWaterAnalyzeData(portIds);
        }
    }
}
