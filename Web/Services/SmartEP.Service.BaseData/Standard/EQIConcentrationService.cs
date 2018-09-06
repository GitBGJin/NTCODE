using SmartEP.BaseInfoRepository.Standard;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.Core.Generic;
using SmartEP.Core.Enums;

namespace SmartEP.Service.BaseData.Standard
{
    /// <summary>
    /// 名称：EQIConcentrationService.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-27
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：提供污染物标准浓度值限服务（如空气质量指数浓度标准、水质等级浓度标准）
    /// 版权所有(C)：江苏远大信息股份有限公司
    public class EQIConcentrationService
    {
        //EQI仓储层
        EQIConcentrationRepository eqiConcentrationRepository = Singleton<EQIConcentrationRepository>.GetInstance();
        #region 增删改
        /// <summary>
        /// 增加污染物等级标准浓度
        /// </summary>
        /// <param name="InstrumentChannel">实体</param>
        public void Add(EQIConcentrationLimitEntity eqi)
        {
            eqiConcentrationRepository.Add(eqi);
        }

        /// <summary>
        /// 更新污染物等级标准浓度
        /// </summary>
        /// <param name="InstrumentChannel">实体</param>
        public void Update(EQIConcentrationLimitEntity eqi)
        {
            eqiConcentrationRepository.Update(eqi);
        }

        /// <summary>
        /// 删除污染物等级标准浓度
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Delete(EQIConcentrationLimitEntity eqi)
        {
            eqiConcentrationRepository.Delete(eqi);
        }
        #endregion

        /// <summary>
        /// 根据Uid获取污染物标准浓度限值
        /// </summary>
        /// <param name="EQIConcentrationUid"></param>
        /// <returns></returns>
        public EQIConcentrationLimitEntity RetrieveByUid(string EQIConcentrationUid)
        {
            return eqiConcentrationRepository.RetrieveFirstOrDefault(e => e.EQIConcentrationLimitUid == EQIConcentrationUid);
        }

        /// <summary>
        /// 根据EQIUid获取污染物标准浓度限值
        /// </summary>
        /// <param name="EQIConcentrationUid"></param>
        /// <returns></returns>
        public IQueryable<EQIConcentrationLimitEntity> RetrieveListByEQIUid(string EQIUid)
        {
            return eqiConcentrationRepository.Retrieve(e => e.EQIUid == EQIUid);
        }

        /// <summary>
        /// 根据污染物因子Uid和时间类型Uid取污染物标准浓度限值
        /// </summary>
        /// <param name="EQIConcentrationUid"></param>
        /// <returns></returns>
        public EQIConcentrationLimitEntity RetrieveByUid(string EQIUid, string pollutantUid, string timeTypeUid)
        {
            return eqiConcentrationRepository.RetrieveFirstOrDefault(e => e.EQIUid == EQIUid && e.PollutantUid == pollutantUid && e.TimeTypeUid == timeTypeUid);
        }

        /// <summary>
        /// 根据空气质量类别、因子编码、时间类型获取因子空气污染物标准浓度限值
        /// </summary>
        /// <param name="EQIConcentrationUid"></param>
        /// <returns></returns>
        public EQIConcentrationLimitEntity RetrieveAirConcentration(AQIClass aqiClass, string pollutantCode, EQITimeType eqiTimeType)
        {

            return eqiConcentrationRepository.RetrieveFirstOrDefault(e => e.EQIUid == SmartEP.Core.Enums.EnumMapping.GetDesc(aqiClass) && e.PollutantCode == pollutantCode && e.TimeTypeUid == SmartEP.Core.Enums.EnumMapping.GetDesc(eqiTimeType));
        }

        /// <summary>
        /// 根据空气质量类别和时间类型获取污染物标准浓度限值
        /// </summary>
        /// <param name="EQIConcentrationUid"></param>
        /// <returns></returns>
        public IQueryable<EQIConcentrationLimitEntity> RetrieveAirConcentration(AQIClass aqiClass, EQITimeType eqiTimeType)
        {
            return eqiConcentrationRepository.Retrieve(e => e.EQIUid == SmartEP.Core.Enums.EnumMapping.GetDesc(aqiClass) && e.TimeTypeUid == SmartEP.Core.Enums.EnumMapping.GetDesc(eqiTimeType));
        }


        /// <summary>
        /// 根据地表水污染物标准浓度限值
        /// </summary>
        /// <param name="EQIConcentrationUid"></param>
        /// <returns></returns>
        public EQIConcentrationLimitEntity RetrieveWaterConcentration(WaterQualityClass waterClass, string pollutantCode, WaterPointCalWQType calWQType)
        {
            return eqiConcentrationRepository.RetrieveFirstOrDefault(e => e.EQIUid == SmartEP.Core.Enums.EnumMapping.GetDesc(waterClass) && e.PollutantCode == pollutantCode && e.PointAttributeUid == SmartEP.Core.Enums.EnumMapping.GetDesc(calWQType));
        }

        /// <summary>
        /// 根据地表水污染物数组标准浓度限值
        /// </summary>
        /// <param name="EQIConcentrationUid"></param>
        /// <returns></returns>
        public IQueryable<EQIConcentrationLimitEntity> RetrieveWaterConcentrationList(WaterQualityClass waterClass, string[] pollutantCode, WaterPointCalWQType calWQType)
        {
            return eqiConcentrationRepository.Retrieve(e => e.EQIUid == SmartEP.Core.Enums.EnumMapping.GetDesc(waterClass) && pollutantCode.Contains(e.PollutantCode) && e.PointAttributeUid == SmartEP.Core.Enums.EnumMapping.GetDesc(calWQType));
        }

        /// <summary>
        /// 获取标准限值（蓝藻日报）
        /// </summary>
        /// <param name="factors"></param>
        /// <returns></returns>
        public DataTable GetEQILimit(string[] factors)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");

            DataRow rowTree = dt.NewRow();
            rowTree["Name"] = "III类标准限值(mg/L)";

            DataRow rowFour = dt.NewRow();
            rowFour["Name"] = "IV类类标准限值(mg/L)";

            DataRow rowFive = dt.NewRow();
            rowFive["Name"] = "V类类标准限值(mg/L)";
            for (int j = 0; j < factors.Length; j++)
            {
                dt.Columns.Add(factors[j]);

                EQIConcentrationLimitEntity limitTree = RetrieveWaterConcentration(WaterQualityClass.Three, factors[j], WaterPointCalWQType.River);
                EQIConcentrationLimitEntity limitFour = RetrieveWaterConcentration(WaterQualityClass.Four, factors[j], WaterPointCalWQType.River);
                EQIConcentrationLimitEntity limitFive = RetrieveWaterConcentration(WaterQualityClass.Five, factors[j], WaterPointCalWQType.River);
                if (limitTree != null)
                {
                    if (limitTree.Low != null && limitTree.Upper != null)
                        rowTree[factors[j]] = limitTree.Low.Value.ToString("0.00") + "~" + limitTree.Upper.Value.ToString("0.00");
                    else if (limitTree.Low != null)
                        rowTree[factors[j]] = limitTree.Low.Value.ToString("0.00");
                    else if (limitTree.Upper != null)
                        rowTree[factors[j]] = limitTree.Upper.Value.ToString("0.00");
                }

                if (limitFour != null)
                {
                    if (limitFour.Low != null && limitFour.Upper != null)
                        rowFour[factors[j]] = limitFour.Low.Value.ToString("0.00") + "~" + limitFour.Upper.Value.ToString("0.00");
                    else if (limitTree.Low != null)
                        rowFour[factors[j]] = limitFour.Low.Value.ToString("0.00");
                    else if (limitTree.Upper != null)
                        rowFour[factors[j]] = limitFour.Upper.Value.ToString("0.00");
                }

                if (limitFive != null)
                {
                    if (limitFive.Low != null && limitFive.Upper != null)
                        rowFive[factors[j]] = limitFive.Low.Value.ToString("0.00") + "~" + limitFive.Upper.Value.ToString("0.00");
                    else if (limitTree.Low != null)
                        rowFive[factors[j]] = limitFive.Low.Value.ToString("0.00");
                    else if (limitTree.Upper != null)
                        rowFive[factors[j]] = limitFive.Upper.Value.ToString("0.00");
                }
            }
            dt.Rows.Add(rowTree);
            dt.Rows.Add(rowFour);
            dt.Rows.Add(rowFive);
            return dt;
        }

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
        public DataTable GetIEQIByPollutantCodes(List<int> IEQI, List<string> PollutantCodes, ApplicationType applicationtype, WaterPointCalWQType waterPointCalWQType)
        {
            return eqiConcentrationRepository.GetIEQIByPollutantCodes(IEQI, PollutantCodes, applicationtype, SmartEP.Core.Enums.EnumMapping.GetDesc(waterPointCalWQType));
        }
        #endregion

    }
}
