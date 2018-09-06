using SmartEP.BaseInfoRepository.Standard;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.Standard
{
    /// <summary>
    /// 名称：WaterQualityService.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：提供污染等级服务（如空气质量指数、水质等级）
    /// 版权所有(C)：江苏远大信息股份有限公司
    public class WaterQualityService : IEQI
    {
        private EQIRepository eqiRepository = new EQIRepository();

        /// <summary>
        /// 水质标准处理仓储层
        /// </summary>
        private WaterQualityRepository g_WaterQualityRepository = Singleton<WaterQualityRepository>.GetInstance();

        private string applicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Water);
        #region 增删改
        /// <summary>
        /// 增加等级标准
        /// </summary>
        /// <param name="InstrumentChannel">实体</param>
        public void Add(EQIEntity eqi)
        {
            eqiRepository.Add(eqi);
        }

        /// <summary>
        /// 更新等级标准
        /// </summary>
        /// <param name="InstrumentChannel">实体</param>
        public void Update(EQIEntity eqi)
        {
            eqiRepository.Update(eqi);
        }

        /// <summary>
        /// 删除等级标准
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Delete(EQIEntity eqi)
        {
            eqiRepository.Delete(eqi);
        }
        #endregion

        /// <summary>
        /// 获取水质等级标准列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<EQIEntity> RetrieveList()
        {
            return eqiRepository.Retrieve(a => a.ApplicationUid == applicationValue);
        }

        /// <summary>
        /// 根据EQIUid获取水质等级
        /// </summary>
        /// <param name="eqiUid"></param>
        /// <returns></returns>
        public EQIEntity RetrieveByUid(string eqiUid)
        {
            return RetrieveList().FirstOrDefault(a => a.EQIUid == eqiUid);
        }

        /// <summary>
        /// 计算水质污染指数
        /// </summary>
        /// <param name="pollutantCode">因子编码</param>
        /// <param name="pollutantValue">因子浓度</param>
        /// <param name="iEQI">评价水质类别</param>
        /// <param name="hourType">时间类型</param>
        /// <param name="waterPointCalWQType">地表水水质评价站点属性类型</param>
        /// <returns></returns>
        public decimal GetWQI(string pollutantCode, decimal pollutantValue, WaterQualityClass iEQI, EQITimeType hourType, WaterPointCalWQType waterPointCalWQType)
        {
            return g_WaterQualityRepository.GetWQI(pollutantCode, pollutantValue, (Int32)iEQI, SmartEP.Core.Enums.EnumMapping.GetDesc(hourType), SmartEP.Core.Enums.EnumMapping.GetDesc(waterPointCalWQType));
        }

        /// <summary>
        /// 获取首要染污物等(根据因子指数)
        /// </summary>
        /// <param name="ReturnType">返回类型（CODE、NAME、ENAME、VALUE、SIMPLE）</param>
        /// <param name="EvaluateFactorCodes">参与评价因子，以“;”分割</param>
        /// <param name="WQIValues">因子指数列表（Key:PollutantCode、Value:分指数）</param>
        /// <returns></returns>
        public string GetWQI_Max(EQIReurnType ReturnType, string EvaluateFactorCodes, Dictionary<string, Decimal> WQIValues)
        {
            return g_WaterQualityRepository.GetWQI_Max(SmartEP.Core.Enums.EnumMapping.GetDesc(ReturnType), EvaluateFactorCodes, WQIValues);
        }

        /// <summary>
        /// 计算综合污染指数(根据因子指数)
        /// </summary>
        /// <param name="EvaluateFactorCodes">参与评价因子，以“;”分割</param>
        /// <param name="WQIValues">因子指数列表（Key:PollutantCode、Value:分指数）</param>
        /// <returns></returns>
        public string GetWQI_Avg(string EvaluateFactorCodes, Dictionary<string, Decimal> WQIValues)
        {
            return g_WaterQualityRepository.GetWQI_Avg(EvaluateFactorCodes, WQIValues);
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
        public string GetWQL(string pollutantCode, decimal pollutantValue, EQITimeType hourType, WaterPointCalWQType waterPointCalWQType, EQIReurnType returnType)
        {
            return g_WaterQualityRepository.GetWQL(pollutantCode, pollutantValue, SmartEP.Core.Enums.EnumMapping.GetDesc(hourType), SmartEP.Core.Enums.EnumMapping.GetDesc(waterPointCalWQType), SmartEP.Core.Enums.EnumMapping.GetDesc(returnType));
        }

        /// <summary>
        /// 获取首要染污物等(根据因子等级)
        /// </summary>
        /// <param name="ReturnType">返回类型（CODE、NAME、ENAME、VALUE、SIMPLE）</param>
        /// <param name="EvaluateFactorCodes">参与评价因子，以“;”分割</param>
        /// <param name="WQLValues">因子指数列表（Key:PollutantCode、Value:单因子等级）</param>
        /// <returns></returns>
        public string GetWQL_Max(EQIReurnType ReturnType, string EvaluateFactorCodes, Dictionary<string, Int32> WQLValues)
        {
            return g_WaterQualityRepository.GetWQL_Max(SmartEP.Core.Enums.EnumMapping.GetDesc(ReturnType), EvaluateFactorCodes, WQLValues);
        }

        /// <summary>
        /// 取得参与评价的因子
        /// </summary>
        /// <returns></returns>
        public static string[] GetWaterQualityPollutant()
        {
            return new string[] { "w01001", "w21003", "w01019", "w01009", "w21011", "w21001", "w01018", "w01017", "w20122", "w20123", "w21017", "w20128", "w20119", "w20111", "w20115", "w20117", "w20120", "w21016", "w23002", "w21019", "w22001", "w19002", "w02003" };
        }
    }
}
