using System;
using System.Text;
using System.Data;
using SmartEP.MonitoringBusinessRepository.Water;

namespace SmartEP.Service.ReportLibrary.Water
{
    public class AutoPointMonthReportService
    {
        private AutoPointMonthReportRepository autoPointRep = new AutoPointMonthReportRepository();

        /// <summary>
        /// 根据时间获取平均有效运行率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns></returns>
        public double GetAvgValidRunRate(string dateTime)
        {
            return autoPointRep.GetAvgValidRunRate(dateTime);
        }

        /// <summary>
        /// 根据时间获取标样考核合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns></returns>
        public double GetAvgSampleEevaluationRate(string dateTime)
        {
            return autoPointRep.GetAvgSampleEevaluationRate(dateTime);
        }

        /// <summary>
        /// 根据时间获取水样比对合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns></returns>
        public double GetAvgCompareRate(string dateTime)
        {
            return autoPointRep.GetAvgCompareRate(dateTime);
        }

        /// <summary>
        /// 根据时间获取标样考核合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointId：站点ID、Rate：有效率</returns>
        public DataTable GetPointSampleEevaluationRate(string dateTime)
        {
            return autoPointRep.GetPointSampleEevaluationRate(dateTime);
        }

        /// <summary>
        /// 根据时间获取水样比对合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointId：站点ID、Rate：有效率</returns>
        public DataTable GetPointCompareRate(string dateTime)
        {
            return autoPointRep.GetPointCompareRate(dateTime);
        }

        /// <summary>
        /// 根据时间获取区域水站数据有效率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>AreaName: 区域名称、Rate：有效率</returns>
        public DataTable GetAreaDataValidRate(string dateTime)
        {
            return autoPointRep.GetAreaDataValidRate(dateTime);
        }

        /// <summary>
        /// 根据站点ID获取区域列表信息
        /// </summary>
        /// <param name="pointIds">站点ID(多个站点以英文,分割)</param>
        /// <returns>AreaName: 区域名称</returns>
        public DataTable GetAreasByPointIds(string pointIds)
        {
            return autoPointRep.GetAreasByPointIds(pointIds);
        }

        /// <summary>
        /// 根据站点ID获取区域监管描述信息
        /// </summary>
        /// <param name="pointIds">站点ID(多个站点以英文,分割)</param>
        /// <param name="totalPointCount">监测站点个数</param>
        /// <returns>109个站点由市区、江阴、宜兴、锡山负责例行监管工作；</returns>
        public string GetAreaMonitorDesc(string pointIds, int totalPointCount)
        {
            DataTable dataDt = autoPointRep.GetAreasByPointIds(pointIds);
            StringBuilder areaMonitorSb = new StringBuilder();
            areaMonitorSb.AppendFormat(@"{0}个站点由", totalPointCount);

            foreach(DataRow dataRow in dataDt.Rows)
            {
                areaMonitorSb.Append(dataRow["AreaName"].ToString()).Append("、");
            }

            //去除最后一个,符号
            areaMonitorSb.Remove(areaMonitorSb.Length - 1, 1);

            areaMonitorSb.Append("负责例行监管工作；");

            return areaMonitorSb.ToString();
        }


        /// <summary>
        /// 根据时间获取水站数据有效率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointId: 站点ID、Rate：有效率</returns>
        public DataTable GetPointDataValidRate(string dateTime)
        {
            return autoPointRep.GetPointDataValidRate(dateTime);
        }

        /// <summary>
        /// 根据时间、区域ID获取区域数据有效率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="areaId">区域ID</param>
        /// <returns></returns>
        public double GetAreaDataValidRate(string dateTime, string areaId)
        {
            return autoPointRep.GetAreaDataValidRate(dateTime, areaId);
        }

        /// <summary>
        /// 根据时间获取运营商数据有效率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>Supplier: 运营商名称、Rate：有效率</returns>
        public DataTable GetSupplierDataValidRate(string dateTime)
        {
            return autoPointRep.GetSupplierDataValidRate(dateTime);
        }

        /// <summary>
        /// 根据站点ID获取运营商列表信息
        /// </summary>
        /// <param name="pointIds">站点ID(多个站点以英文,分割)</param>
        /// <returns>Supplier: 运营商名称、SupplierId：运营商ID</returns>
        public DataTable GetSupplierByPointIds(string pointIds)
        {
            return autoPointRep.GetSupplierByPointIds(pointIds);
        }

        /// <summary>
        /// 根据站点获取运营商维护描述信息
        /// </summary>
        /// <param name="pointIds"></param>
        /// <param name="totalPointCount"></param>
        /// <returns></returns>
        public string GetSupplierProtectDesc(string pointIds, int totalPointCount)
        {
            //无锡市环境监测中心站运营商ID
            string wuXiCitySupplierId = "c6be0099-b07d-4eeb-8dce-f34e67b5ffc4";
            //宜兴市环境监测站运营商ID
            string yiXinCitySupplierId = "a7763db6-3397-41be-a0cb-a464e57c9d2c";

            bool isContainWuXiCity = false;
            bool isContainYiXinCity = false;

            DataTable dataDt = autoPointRep.GetSupplierByPointIds(pointIds);
            string supplierId = string.Empty;

            // 其它运维商描述信息（排除无锡市环境监测中心站、宜兴市环境监测站）
            StringBuilder complexProtectDescSb = new StringBuilder();
            // 运维商个数（排除无锡市环境监测中心站、宜兴市环境监测站）
            int complexSupplierCount = 0;

            foreach (DataRow dataRow in dataDt.Rows)
            {
                supplierId = dataRow["SupplierId"].ToString();

                if (wuXiCitySupplierId.Equals(supplierId))
                {// 包含无锡市环境监测中心站
                    isContainWuXiCity = true;
                    continue;
                }

                if (yiXinCitySupplierId.Equals(supplierId))
                {// 包含宜兴市环境监测站
                    isContainYiXinCity = true;
                    continue;
                }

                if (complexProtectDescSb.Length > 0)
                {
                    complexProtectDescSb.Append("、");
                }

                complexProtectDescSb.Append(dataRow["Supplier"].ToString());

                complexSupplierCount++;
            }

            //运维站点个数（排除无锡市环境监测中心站、宜兴市环境监测站运维的站点）
            int yunWeiPointCount = totalPointCount;

            StringBuilder wuXiAndYiXinProtectDescSb = new StringBuilder();

            if (isContainWuXiCity)
            {// 包含无锡市环境监测中心站
                wuXiAndYiXinProtectDescSb.Append("由无锡市环境监测中心站负责");

                StringBuilder wuXiCityPointSb = new StringBuilder();
                DataTable wuXiDataDt = autoPointRep.GetPointNameByCondition(pointIds, wuXiCitySupplierId);

                foreach (DataRow dataRow in wuXiDataDt.Rows)
                {
                    if (wuXiCityPointSb.Length > 0)
                    {
                        wuXiCityPointSb.Append("、");
                    }

                    wuXiCityPointSb.Append(dataRow["PointName"].ToString());
                }

                wuXiAndYiXinProtectDescSb.Append(wuXiCityPointSb.ToString());
                wuXiAndYiXinProtectDescSb.Append("站点的运行维护工作，");

                yunWeiPointCount = yunWeiPointCount - wuXiDataDt.Rows.Count;
            }

            if (isContainYiXinCity)
            {// 包含宜兴市环境监测站
                wuXiAndYiXinProtectDescSb.Append("由宜兴市环境监测站负责");

                StringBuilder yiXinCityPointSb = new StringBuilder();

                DataTable yiXinDataDt = autoPointRep.GetPointNameByCondition(pointIds, yiXinCitySupplierId);

                foreach (DataRow dataRow in yiXinDataDt.Rows)
                {
                    if (yiXinCityPointSb.Length > 0)
                    {
                        yiXinCityPointSb.Append("、");
                    }

                    yiXinCityPointSb.Append(dataRow["PointName"].ToString());
                }

                wuXiAndYiXinProtectDescSb.Append(yiXinCityPointSb.ToString());
                wuXiAndYiXinProtectDescSb.Append("站点的运行维护工作，");

                yunWeiPointCount = yunWeiPointCount - yiXinDataDt.Rows.Count;
            }

            StringBuilder protectDescSb = new StringBuilder();
            if (complexSupplierCount > 0)
            {
                protectDescSb.Append("由").Append(complexProtectDescSb);
                protectDescSb.AppendFormat(@"{0}家公司负责{1}个站点的运行维护工作，", complexSupplierCount, yunWeiPointCount);
            }

            protectDescSb.Append(wuXiAndYiXinProtectDescSb.ToString());

            if (protectDescSb.Length > 0)
            {//去除最后一个,符号
                protectDescSb.Remove(protectDescSb.Length - 1, 1).Append("。");
            }

            return protectDescSb.ToString();
        }

        /// <summary>
        /// 根据时间获取因子数据合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pollutantCode">因子编码</param>
        /// <returns></returns>
        public double GetPollutantValidRate(string dateTime, string pollutantCode, string pointIds)
        {
            return autoPointRep.GetPollutantValidRate(dateTime, pollutantCode, pointIds);
        }

        /// <summary>
        /// 根据时间获取因子比对合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pollutantCode">因子编码</param>
        /// <returns></returns>
        public double GetPollutantCompareValidRate(string dateTime, string pollutantCode, string pointIds)
        {
            return autoPointRep.GetPollutantCompareValidRate(dateTime, pollutantCode, pointIds);
        }

        /// <summary>
        /// 根据时间获取点位各类水质等级数量(剔除总氮的计算)
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>Level: 水质等级、Count：个数</returns>
        public DataTable GetPointLevelCountExcludeTN(string dateTime)
        {
            return autoPointRep.GetPointLevelCountExcludeTN(dateTime);
        }

        /// <summary>
        /// 根据时间获取点位各类水质等级数量
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>Level: 水质等级、Count：个数</returns>
        public DataTable GetPointLevelCount(string dateTime, string pointIds)
        {
            return autoPointRep.GetPointLevelCount(dateTime, pointIds);
        }

        /// <summary>
        /// 根据时间获取点位水质等级(剔除总氮的计算)
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointName: 点位名称、Level: 水质等级</returns>
        public DataTable GetPointLevelExcludeTN(string dateTime)
        {
            return autoPointRep.GetPointLevelExcludeTN(dateTime);
        }

        /// <summary>
        /// 根据时间获取点位水质等级
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointName: 点位名称、Level: 水质等级</returns>
        public DataTable GetPointLevel(string dateTime)
        {
            return autoPointRep.GetPointLevel(dateTime);
        }

        /// <summary>
        /// 根据时间、点位获取点位水质等级(剔除总氮的计算)
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pointId">测点ID</param>
        /// <returns>水质等级</returns>
        public string GetPointLevelExcludeTN(string dateTime, string pointId)
        {
            return autoPointRep.GetPointLevelExcludeTN(dateTime, pointId);
        }

        /// <summary>
        /// 根据时间、点位获取点位水质状况
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pointId">测点ID</param>
        /// <returns>水质状况(优、良好)</returns>
        public string GetPointGrade(string dateTime, string pointId)
        {
            return autoPointRep.GetPointGrade(dateTime, pointId);
        }

        /// <summary>
        /// 根据时间、点位获取点位水质等级
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pointId">测点ID</param>
        /// <returns>水质等级</returns>
        public string GetPointLevel(string dateTime, string pointId)
        {
            return autoPointRep.GetPointLevel(dateTime, pointId);
        }

        /// <summary>
        /// 根据条件获取点位因子浓度值
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pointId">测点ID</param>
        /// <param name="pollutantCode">因子编码</param>
        /// <returns></returns>
        public decimal GetPointPollutantValue(string dateTime, string pointId, string pollutantCode)
        {
            return autoPointRep.GetPointPollutantValue(dateTime, pointId, pollutantCode);
        }

        /// <summary>
        /// 根据时间获取点位月度因子平均浓度值
        /// </summary>
        /// <param name="startTime">开始时间(格式：yyyy-MM)</param>
        /// <param name="endTime">结束时间(格式：yyyy-MM)</param>
        /// <returns>
        /// PointName: 点位名称
        /// Tstamp: 数据时间
        /// do：溶解氧
        /// mnO4 ：高锰酸盐指数
        /// nh: 氨氮
        /// tp：总磷
        /// tn：总氮
        /// algalDensity：藻密度
        /// </returns>
        public DataTable GetPointListMonthData(string startTime, string endTime)
        {
            return autoPointRep.GetPointListMonthData(startTime, endTime);
        }

        /// <summary>
        /// 根据站点和时间获取点位(当月、上个月、去年当前月)月度因子平均浓度值
        /// </summary>
        /// <param name="pointIds">测点ID列表</param>
        /// <param name="currMonthTime">当前月时间</param>
        /// <returns>
        /// PointName: 点位名称
        /// Tstamp: 数据时间
        /// do：溶解氧
        /// mnO4 ：高锰酸盐指数
        /// nh: 氨氮
        /// tp：总磷
        /// tn：总氮
        /// algalDensity：藻密度
        /// </returns>
        public DataTable GetPointListMonthData(string[] pointIds, DateTime currMonthTime)
        {
            return autoPointRep.GetPointListMonthData(pointIds, currMonthTime);
        }

        /// <summary>
        /// 根据时间获取点位首要污染物
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointId：站点ID、PointName: 点位名称、PrimaryPollutant: 首要污染物</returns>
        public DataTable GetPointPrimaryPollutant(string dateTime)
        {
            return autoPointRep.GetPointPrimaryPollutant(dateTime);
        }

        /// <summary>
        /// 取得太湖西部9条入湖河流当月、上月、上一年等级数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataView GetThreeMonthLevel(string[] portIds, int year, int month)
        {
            return autoPointRep.GetThreeMonthLevel(portIds, year, month);
        }
    }
}
