using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.MPInfo
{
    /// <summary>
    /// 名称：MonitoringPointWaterService.cs
    /// 创建人：邱奇
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：季柯
    /// 最新维护日期：2015-08-27
    /// 功能摘要：提供地表水点位信息服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>

    public class MonitoringPointWaterService : AbstractMonitoringPoint
    {
        //点位信息仓储层
        private MonitoringPointRepository pointRepository = new MonitoringPointRepository();
        //应用程序值
        private string applicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Water);

        /// <summary>
        /// 判断是否存在站点名称
        /// </summary>
        /// <param name="monitoringPointName"></param>
        /// <returns></returns>
        public bool IsExistName(string monitoringPointName)
        {
            return pointRepository.Retrieve(p => p.ApplicationUid == applicationValue && p.MonitoringPointName == monitoringPointName).Count() > 0 ? true : false;
        }

        /// <summary>
        /// 根据站点ID获取站点
        /// </summary>
        /// <param name="monitoringPointID"></param>
        /// <returns></returns>
        public MonitoringPointEntity RetrieveEntityByID(int monitoringPointID)
        {
            return pointRepository.Retrieve(p => p.ApplicationUid == applicationValue && p.PointId == monitoringPointID).FirstOrDefault();
        }

        /// <summary>
        /// 根据站点名称获取地表水站点
        /// </summary>
        /// <param name="monitoringPointName"></param> 
        /// <returns></returns>
        public MonitoringPointEntity RetrieveEntityByName(string monitoringPointName)
        {
            return pointRepository.Retrieve(p => p.ApplicationUid == applicationValue && p.MonitoringPointName == monitoringPointName).FirstOrDefault();
        }

        /// <summary>
        /// 获取所有地表水点位列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveWaterMPList()
        {
            return pointRepository.Retrieve(p => p.ApplicationUid == applicationValue).OrderBy(p=>p.OrderByNum);
        }

        /// <summary>
        /// 获取所有启用的地表水点位列表
        /// </summary>
        /// <returns>MonitoringPointEntity</returns>
        public IQueryable<MonitoringPointEntity> RetrieveWaterMPListByEnable()
        {
            return RetrieveWaterMPList().Where(p => p.EnableOrNot == true);
        }


        //<summary>
        //地表水站点信息查询（根据站点名、行政区划Uid、站点类型Uid）
        //</summary>
        //<param name="pointName">站点名称</param>
        //<param name="regionUid">行政区划</param>
        //<param name="siteTypeUid">站点类型</param>
        //<param name="controlUid">控制类型</param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveWaterMPList(string pointName, string regionUid, string siteTypeUid, bool enableOrNot)
        {
            IQueryable<MonitoringPointEntity> monitoringPointEntity = RetrieveWaterMPList().Where(p => p.ApplicationUid == applicationValue && p.EnableOrNot == enableOrNot);
            if (!string.IsNullOrEmpty(pointName)) monitoringPointEntity = monitoringPointEntity.Where(p => p.MonitoringPointName.Contains(pointName));
            if (!string.IsNullOrEmpty(regionUid)) monitoringPointEntity = monitoringPointEntity.Where(p => p.RegionUid == regionUid);
            if (!string.IsNullOrEmpty(siteTypeUid)) monitoringPointEntity = monitoringPointEntity.Where(p => p.SiteTypeUid == siteTypeUid);
            return monitoringPointEntity;
        }

        //<summary>
        //地表水站点信息查询（根据站点名、行政区划Uid、站点类型Uid）
        //</summary>
        //<param name="pointName">站点名称</param>
        //<param name="regionUid">行政区划</param>
        //<param name="siteTypeUid">站点类型</param>
        //<param name="controlUid">控制类型</param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveWaterMPList(string pointName, string regionUid, string siteTypeUid)
        {
            IQueryable<MonitoringPointEntity> monitoringPointEntity = RetrieveWaterMPList().Where(p => p.ApplicationUid == applicationValue);
            if (!string.IsNullOrEmpty(pointName)) monitoringPointEntity = monitoringPointEntity.Where(p => p.MonitoringPointName.Contains(pointName));
            if (!string.IsNullOrEmpty(regionUid)) monitoringPointEntity = monitoringPointEntity.Where(p => p.RegionUid == regionUid);
            if (!string.IsNullOrEmpty(siteTypeUid)) monitoringPointEntity = monitoringPointEntity.Where(p => p.SiteTypeUid == siteTypeUid);
            return monitoringPointEntity;
        }

        //<summary>
        //根据地表水站点类型中文名称获取点位列表(浮标站，饮用水源地等)
        //</summary>
        //<param name="SiteTypeName"></param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveListBySiteTypeName(string siteTypeUid)
        {
            return RetrieveWaterMPListByEnable().Where(p => p.SiteTypeUid == siteTypeUid);
        }

        //<summary>
        //获取水源地点位
        //</summary>
        //<param name="SiteTypeName"></param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPListByWaterSource()
        {
            return RetrieveWaterMPListByEnable().Where(p => p.SiteTypeUid == "81381370-1744-41fd-b5e4-4ffbee24288e");
        }

        //<summary>
        //获取城区河道点位
        //</summary>
        //<param name="SiteTypeName"></param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPListByCityRiver()
        {
            return RetrieveWaterMPListByEnable().Where(p => p.SiteTypeUid == "46540455-062c-4a58-a8bb-ea2280a2d6ce");
        }

        //<summary>
        //获取浮标站点位
        //</summary>
        //<param name="SiteTypeName"></param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPListByFloat()
        {
            return RetrieveWaterMPListByEnable().Where(p => p.SiteTypeUid == "a10a8482-1d30-44ea-8b03-b724f1ebeaa8");
        }

        //<summary>
        //获取人工监测点位
        //</summary>
        //<param name="SiteTypeName"></param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPListByManual()
        {
            return RetrieveWaterMPListByEnable().Where(p => p.SiteTypeUid == "160e08ec-1d1b-4095-b898-3a3d925ed4e6");
        }

        /// <summary>
        /// 根据站点Uid获取默认评价因子
        /// </summary>
        /// <param name="monitoringPointUid"></param>
        /// <returns></returns>
        public string RetrieveCalEQIPollutantList(string monitoringPointUid)
        {
            MonitoringPointEntity entity = pointRepository.RetrieveFirstOrDefault(p => p.MonitoringPointUid == monitoringPointUid);
            return entity.MonitoringPointExtensionForEQMSWaterEntity.EvaluateFactorList;
        }

        /// <summary>
        /// 根据站点属性（控制类型）获取站点选择SiteMap绑定数据源
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<V_Point_SiteMap_UserConfig_PropertyEntity> RetrieveSiteMapListByProperty(string userGuid)
        {
            return pointRepository.Context.GetAll<V_Point_SiteMap_UserConfig_PropertyEntity>().Where(p => p.ApplicationUid == applicationValue && p.UserGuid == userGuid);
        }

        /// <summary>
        /// 根据行政区划获取站点选择SiteMap绑定数据源
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<V_Point_SiteMap_UserConfig_RegionEntity> RetrieveSiteMapListByRegion(string userGuid)
        {
            return pointRepository.Context.GetAll<V_Point_SiteMap_UserConfig_RegionEntity>().Where(p => p.ApplicationUid == applicationValue && p.UserGuid == userGuid);
        }

        /// <summary>
        /// 根据站点类型获取站点选择SiteMap绑定数据源
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<V_Point_SiteMap_UserConfig_TypeEntity> RetrieveSiteMapListByType(string userGuid)
        {
            return pointRepository.Context.GetAll<V_Point_SiteMap_UserConfig_TypeEntity>().Where(p => p.ApplicationUid == applicationValue && p.UserGuid == userGuid);
        }
        /// <summary>
        /// 取得自定义控件返回值
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="rsmType"></param>
        /// <param name="pointType"></param>
        /// <param name="defaultStrList"></param>
        /// <param name="notIn"></param>
        /// <param name="userGuid"></param>
        /// <param name="IsCheckAll"></param>
        /// <param name="isAllSel"></param>
        /// <returns></returns>
        public DataView GetRsmData(ApplicationType applicationType, CbxRsmType rsmType, RsmPointMode pointType, String defaultStrList, String notIn, String userGuid, Boolean IsCheckAll = false, bool isAllSel = false)
        {
            return pointRepository.GetRsmData(applicationType, rsmType, pointType, "", "", userGuid, IsCheckAll = false, isAllSel = false);
        }
    }
}
