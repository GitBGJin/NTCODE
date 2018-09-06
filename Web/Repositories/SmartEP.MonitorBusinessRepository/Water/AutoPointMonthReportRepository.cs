using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.Data.SqlServer.MonitoringBusiness.Water;

namespace SmartEP.MonitoringBusinessRepository.Water
{
    public class AutoPointMonthReportRepository
    {
        private AutoPointMonthReportDAL autoPointDal = new AutoPointMonthReportDAL();

        /// <summary>
        /// 根据时间获取平均有效运行率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns></returns>
        public double GetAvgValidRunRate(string dateTime)
        {
            return autoPointDal.GetAvgValidRunRate(dateTime);
        }

        /// <summary>
        /// 根据时间获取标样考核合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns></returns>
        public double GetAvgSampleEevaluationRate(string dateTime)
        {
            return autoPointDal.GetAvgSampleEevaluationRate(dateTime);
        }

        /// <summary>
        /// 根据时间获取水样比对合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns></returns>
        public double GetAvgCompareRate(string dateTime)
        {
            return autoPointDal.GetAvgCompareRate(dateTime);
        }

        /// <summary>
        /// 根据时间获取标样考核合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointId：站点ID、Rate：有效率</returns>
        public DataTable GetPointSampleEevaluationRate(string dateTime)
        {
            return autoPointDal.GetPointSampleEevaluationRate(dateTime);
        }

        /// <summary>
        /// 根据时间获取水样比对合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointId：站点ID、Rate：有效率</returns>
        public DataTable GetPointCompareRate(string dateTime)
        {
            return autoPointDal.GetPointCompareRate(dateTime);
        }

        /// <summary>
        /// 根据时间获取区域水站数据有效率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>AreaName: 区域名称、Rate：有效率</returns>
        public DataTable GetAreaDataValidRate(string dateTime)
        {
            return autoPointDal.GetAreaDataValidRate(dateTime);
        }

        /// <summary>
        /// 根据站点ID获取区域列表信息
        /// </summary>
        /// <param name="pointIds">站点ID(多个站点以英文,分割)</param>
        /// <returns>AreaName: 区域名称</returns>
        public DataTable GetAreasByPointIds(string pointIds)
        {
            return autoPointDal.GetAreasByPointIds(pointIds);
        }

        /// <summary>
        /// 根据时间获取水站数据有效率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointId: 站点ID、Rate：有效率</returns>
        public DataTable GetPointDataValidRate(string dateTime)
        {
            return autoPointDal.GetPointDataValidRate(dateTime);
        }

        /// <summary>
        /// 根据时间、区域ID获取区域数据有效率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="areaId">区域ID</param>
        /// <returns></returns>
        public double GetAreaDataValidRate(string dateTime, string areaId)
        {
            return autoPointDal.GetAreaDataValidRate(dateTime, areaId);
        }

        /// <summary>
        /// 根据时间获取运营商数据有效率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>Supplier: 运营商名称、Rate：有效率</returns>
        public DataTable GetSupplierDataValidRate(string dateTime)
        {
            return autoPointDal.GetSupplierDataValidRate(dateTime);
        }

        /// <summary>
        /// 根据站点ID获取运营商列表信息
        /// </summary>
        /// <param name="pointIds">站点ID(多个站点以英文,分割)</param>
        /// <returns>Supplier: 运营商名称、SupplierId：运营商ID</returns>
        public DataTable GetSupplierByPointIds(string pointIds)
        {
            return autoPointDal.GetSupplierByPointIds(pointIds);
        }

        /// <summary>
        /// 根据站点ID和运营商Id筛选站点信息
        /// </summary>
        /// <param name="pointIds">站点ID(多个站点以英文,分割)</param>
        /// <param name="supplierId">运营商ID</param>
        /// <returns>PointName: 站点名称</returns>
        public DataTable GetPointNameByCondition(string pointIds, string supplierId)
        {
            return autoPointDal.GetPointNameByCondition(pointIds, supplierId);
        }

        /// <summary>
        /// 根据时间获取因子数据合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pollutantCode">因子编码</param>
        /// <returns></returns>
        public double GetPollutantValidRate(string dateTime, string pollutantCode, string pointIds)
        {
            return autoPointDal.GetPollutantValidRate(dateTime, pollutantCode, pointIds);
        }

        /// <summary>
        /// 根据时间获取因子比对合格率
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pollutantCode">因子编码</param>
        /// <returns></returns>
        public double GetPollutantCompareValidRate(string dateTime, string pollutantCode, string pointIds)
        {
            return autoPointDal.GetPollutantCompareValidRate(dateTime, pollutantCode, pointIds);
        }

        /// <summary>
        /// 根据时间获取点位各类水质等级数量(剔除总氮的计算)
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>Level: 水质等级、Count：个数</returns>
        public DataTable GetPointLevelCountExcludeTN(string dateTime)
        {
            return autoPointDal.GetPointLevelCountExcludeTN(dateTime);
        }

        /// <summary>
        /// 根据时间获取点位各类水质等级数量
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>Level: 水质等级、Count：个数</returns>
        public DataTable GetPointLevelCount(string dateTime, string pointIds)
        {
            return autoPointDal.GetPointLevelCount(dateTime, pointIds);
        }

        /// <summary>
        /// 根据时间获取点位水质等级(剔除总氮的计算)
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointName: 点位名称、Level: 水质等级</returns>
        public DataTable GetPointLevelExcludeTN(string dateTime)
        {
            return autoPointDal.GetPointLevelExcludeTN(dateTime);
        }

        /// <summary>
        /// 根据时间获取点位水质等级
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointName: 点位名称、Level: 水质等级</returns>
        public DataTable GetPointLevel(string dateTime)
        {
            return autoPointDal.GetPointLevel(dateTime);
        }

        /// <summary>
        /// 根据时间、点位获取点位水质等级(剔除总氮的计算)
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pointId">测点ID</param>
        /// <returns>水质等级</returns>
        public string GetPointLevelExcludeTN(string dateTime, string pointId)
        {
            return autoPointDal.GetPointLevelExcludeTN(dateTime, pointId);
        }

        /// <summary>
        /// 根据时间、点位获取点位水质等级
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pointId">测点ID</param>
        /// <returns>水质等级</returns>
        public string GetPointLevel(string dateTime, string pointId)
        {
            return autoPointDal.GetPointLevel(dateTime, pointId);
        }

        /// <summary>
        /// 根据时间、点位获取点位水质状况
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <param name="pointId">测点ID</param>
        /// <returns>水质状况(优、良好)</returns>
        public string GetPointGrade(string dateTime, string pointId)
        {
            return autoPointDal.GetPointGrade(dateTime, pointId);
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
            return autoPointDal.GetPointPollutantValue(dateTime, pointId, pollutantCode);
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
            return autoPointDal.GetPointListMonthData(startTime, endTime);
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
            return autoPointDal.GetPointListMonthData(pointIds, currMonthTime);
        }

        /// <summary>
        /// 根据时间获取点位首要污染物
        /// </summary>
        /// <param name="dateTime">数据时间(格式：yyyy-MM)</param>
        /// <returns>PointId：站点ID、PointName: 点位名称、PrimaryPollutant: 首要污染物</returns>
        public DataTable GetPointPrimaryPollutant(string dateTime)
        {
            return autoPointDal.GetPointPrimaryPollutant(dateTime);
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
            return autoPointDal.GetThreeMonthLevel(portIds, year, month);
        }
    }
}
