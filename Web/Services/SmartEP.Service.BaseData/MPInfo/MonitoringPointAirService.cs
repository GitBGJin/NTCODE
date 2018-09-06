using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System.Linq.Expressions;
using System.Data;
using log4net;

namespace SmartEP.Service.BaseData.MPInfo
{
    /// <summary>
    /// 名称：MonitoringPointAirService.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-16
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-05-24
    /// 功能摘要：
    /// 空气站点信息基础服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MonitoringPointAirService : AbstractMonitoringPoint
    {
        private MonitoringPointRepository g_Repository = new MonitoringPointRepository();
        private string applicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Air);
        //获取一个日志记录器
        ILog log = LogManager.GetLogger("FileLogging");

        /// <summary>
        /// 根据站点名称获取站点
        /// </summary>
        /// <param name="monitoringPointName"></param>
        /// <returns></returns>
        public MonitoringPointEntity RetrieveEntityByName(string monitoringPointName)
        {
            return g_Repository.Retrieve(p => p.ApplicationUid == applicationValue && p.MonitoringPointName == monitoringPointName).FirstOrDefault();
        }

        /// <summary>
        /// 根据站点ID获取站点
        /// </summary>
        /// <param name="monitoringPointID"></param>
        /// <returns></returns>
        public MonitoringPointEntity RetrieveEntityByID(int monitoringPointID)
        {
            return g_Repository.Retrieve(p => p.ApplicationUid == applicationValue && p.PointId == monitoringPointID).FirstOrDefault();
        }

        /// <summary>
        /// 判断是否存在站点名称
        /// </summary>
        /// <param name="monitoringPointName"></param>
        /// <returns></returns>
        public bool IsExistName(string monitoringPointName)
        {
            return g_Repository.Retrieve(p => p.ApplicationUid == applicationValue && p.MonitoringPointName == monitoringPointName).Count() > 0 ? true : false;
        }

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<MonitoringPointEntity> Retrieve(Expression<Func<MonitoringPointEntity, bool>> predicate)
        {
            return g_Repository.Retrieve(predicate);
        }

        /// <summary>
        /// 获取所有空气点位列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPList()
        {
            return g_Repository.Retrieve(p => p.ApplicationUid == applicationValue).OrderBy(p => p.OrderByNum);
        }

        /// <summary>
        /// 获取所有启用的空气点位列表
        /// </summary>
        /// <returns>MonitoringPointEntity</returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByEnable()
        {
            return RetrieveAirMPList().Where(p => p.EnableOrNot == true);
        }

        /// <summary>
        /// /// <summary>
        /// 根据行政区划类型获取点位列表
        /// </summary>
        /// <param name="regionUid">按地理区域划分,如苏州、张家港、太仓、昆山、常熟</param>
        /// <returns>MonitoringPointEntity</returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByRegion(string regionUid)
        {
            return RetrieveAirMPListByEnable().Where(p => p.RegionUid == regionUid && p.EnableOrNot == true);
        }

        /// <summary>
        /// 根据监测区域类型获取点位列表
        /// </summary>
        /// <param name="MonitoringRegionUid">按监测区域划分,1、市区 2、城区 3、乡镇 4、郊区</param>
        /// <returns>MonitoringPointEntity</returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByMonitoringRegion(string monitoringRegionUid)
        {
            return RetrieveAirMPListByEnable().Where(p => p.MonitoringRegionUid == monitoringRegionUid);
        }

        /// <summary>
        /// 根据控制类型获取点位列表
        /// </summary>
        /// <param name="controlUid"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveListByControlUid(string controlUid)
        {
            return RetrieveAirMPListByEnable().Where(p => p.ContrlUid == controlUid).OrderBy(p => p.PointId);
        }

        /// <summary>
        /// 根据统计城市类型获取点位列表
        /// </summary>
        /// <param name="MonitoringRegionUid">按统计城市划分,1、苏州市区 2、张家港 3、昆山 4、太仓</param>
        /// <returns>MonitoringPointEntity</returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByStatisticalCityType(string cityType)
        {
            return RetrieveAirMPListByEnable().Where(p => p.MonitoringPointExtensionForEQMSAirEntity.CityTypeUid == cityType);
        }

        /// <summary>
        /// 根据统计区域类型获取点位列表
        /// </summary>
        /// <param name="MonitoringRegionUid">按统计区域划分,1、姑苏区 2、高新区</param>
        /// <returns>MonitoringPointEntity</returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByStatisticalRegionType(string regionType)
        {
            return RetrieveAirMPListByEnable().Where(p => p.MonitoringPointExtensionForEQMSAirEntity.RegionTypeUid == regionType);
        }

        /// <summary>
        /// 根据监测区域获取计算AQI点位列表
        /// </summary>
        /// <param name="MonitoringRegionUid">按监测区域划分,1、市区 2、城区 3、乡镇 4、郊区</param>
        /// <returns>MonitoringPointEntity</returns>
        public IQueryable<MonitoringPointEntity> RetrieveAQIPointListByMonitoringRegion(string monitoringRegionUid)
        {
            return RetrieveAirMPListByMonitoringRegion(monitoringRegionUid).
                Where(p => p.MonitoringPointExtensionForEQMSAirEntity.CalAQIOrNot == true);
        }

        /// <summary>
        /// 根据行政区划获取参与AQI计算点位列表
        /// </summary>
        /// <param name="regionUid">行政区划Uid</param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAQIPointListByRegion(string regionUid)
        {
            return RetrieveAirMPListByRegion(regionUid).
                Where(p => p.MonitoringPointExtensionForEQMSAirEntity.CalRegionAQIOrNot == true);
        }

        /// <summary>
        /// 获取所有启用的国控点位列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByCountryControlled()
        {
            return RetrieveAirMPListByEnable().Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad");
        }

        /// <summary>
        /// 获取所有启用的省控点位列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByProvinceControlled()
        {
            return RetrieveAirMPListByEnable().Where(p => p.ContrlUid == "bc4fca0c-745f-49d7-b9e9-0af67d3219e6");
        }

        /// <summary>
        /// 获取所有启用的市控点位列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByCityControlled()
        {
            return RetrieveAirMPListByEnable().Where(p => p.ContrlUid == "b107d493-b1a3-4ebd-b991-b0e340becec1");
        }

        /// <summary>
        /// 获取所有启用的区控点位列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByRegionControlled()
        {
            return RetrieveAirMPListByEnable().Where(p => p.ContrlUid == "461aefd0-c79f-4ab8-848d-67c623d4bba1");
        }

        /// <summary>
        /// 根据控制类型获取点位列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByControlled(string controlUid)
        {
            return RetrieveAirMPList().Where(p => p.ContrlUid == controlUid);
        }

        /// <summary>
        /// 获取所有启用的创模点位列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByModel()
        {
            return RetrieveAirMPListByEnable().Where(p => p.MonitoringPointExtensionForEQMSAirEntity.ModelOrNot == true);
        }

        /// <summary>
        /// 根据城市均值类型获取所有启用点位列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByCity(CityType cityType)
        {
            //获取城市均值字典Guid
            string cityTypeUid = SmartEP.Core.Enums.EnumMapping.GetDesc(cityType).Split(':')[1];
            return RetrieveAirMPListByEnable().Where(p => p.MonitoringPointExtensionForEQMSAirEntity.CityTypeUid == cityTypeUid);
        }

        /// <summary>
        /// 根据城市均值类型获取所有启用的创模点位列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByCityModel(CityType cityType)
        {
            return RetrieveAirMPListByCity(cityType).Where(p => p.MonitoringPointExtensionForEQMSAirEntity.ModelOrNot == true);
        }

        /// <summary>
        /// 获取所有启用的超级站点位列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListBySuper()
        {
            return RetrieveAirMPListByEnable().Where(p => p.MonitoringPointExtensionForEQMSAirEntity.SuperOrNot == true);
        }

        //<summary>
        //空气站点信息查询（根据站点名、行政区划Uid、站点类型Uid）
        //</summary>
        //<param name="pointName">站点名称</param>
        //<param name="regionUid">行政区划</param>
        //<param name="siteTypeUid">站点类型</param>
        //<param name="controlUid">控制类型</param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPList(string pointName, string regionUid, string siteTypeUid, bool enableOrNot)
        {
            IQueryable<MonitoringPointEntity> monitoringPointEntity = RetrieveAirMPList().Where(p => p.EnableOrNot == enableOrNot);
            if (!string.IsNullOrEmpty(pointName)) monitoringPointEntity = monitoringPointEntity.Where(p => p.MonitoringPointName.Contains(pointName));
            if (!string.IsNullOrEmpty(regionUid)) monitoringPointEntity = monitoringPointEntity.Where(p => p.RegionUid == regionUid);
            if (!string.IsNullOrEmpty(siteTypeUid)) monitoringPointEntity = monitoringPointEntity.Where(p => p.SiteTypeUid == siteTypeUid);
            return monitoringPointEntity.OrderByDescending(x=>x.OrderByNum);
        }

        /// <summary>
        /// 根据站点属性（控制类型）获取站点选择SiteMap绑定数据源
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<V_Point_SiteMap_UserConfig_PropertyEntity> RetrieveSiteMapListByProperty(string userGuid)
        {
            return g_Repository.Context.GetAll<V_Point_SiteMap_UserConfig_PropertyEntity>().Where(p => p.ApplicationUid == applicationValue && p.UserGuid == userGuid);
        }

        /// <summary>
        /// 根据行政区划获取站点选择SiteMap绑定数据源
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<V_Point_SiteMap_UserConfig_RegionEntity> RetrieveSiteMapListByRegion(string userGuid)
        {
            return g_Repository.Context.GetAll<V_Point_SiteMap_UserConfig_RegionEntity>().Where(p => p.ApplicationUid == applicationValue && p.UserGuid == userGuid);
        }

        /// <summary>
        /// 根据站点类型获取站点选择SiteMap绑定数据源
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<V_Point_SiteMap_UserConfig_TypeEntity> RetrieveSiteMapListByType(string userGuid)
        {
            return g_Repository.Context.GetAll<V_Point_SiteMap_UserConfig_TypeEntity>().Where(p => p.ApplicationUid == applicationValue && p.UserGuid == userGuid);
        }

        /// <summary>
        /// 根据城市均值类型获取空气点位列表
        /// </summary>
        /// <param name="cityType">城市均值枚举</param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrievePointListByCity(CityType cityType)
        {
            string cityTypeUid = SmartEP.Core.Enums.EnumMapping.GetDesc(cityType);
            return RetrieveAirMPListByEnable().Where(p => p.MonitoringPointExtensionForEQMSAirEntity.CityTypeUid == cityTypeUid);
        }

        /// <summary>
        /// 根据区域类型Uid获取空气点位列表
        /// </summary>
        /// <param name="regionTypeUid">区域类型Uid</param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrievePointListByRegionUid(string regionTypeUid)
        {
            return RetrieveAirMPListByEnable().Where(p => p.MonitoringPointExtensionForEQMSAirEntity.RegionTypeUid == regionTypeUid);
        }

        /// <summary>
        /// 根据站点取得站点视图
        /// </summary>
        /// <param name="portIds"></param>
        /// <returns></returns>
        public IQueryable<V_Point_AirEntity> RetrieveVPointAirByPort(string[] portIds)
        {
            if (portIds.Length == 0)
                return null;
            return g_Repository.Context.GetAll<V_Point_AirEntity>().Where(p => portIds.Contains(p.PointId.ToString()));
        }

        /// <summary>
        /// 根据站点数组取得区域
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <returns></returns>
        public string[] GetRegionByPort(string[] portIds)
        {
            if (portIds.Length == 0)
                return null;
            IQueryable<V_Point_AirEntity> query = RetrieveVPointAirByPort(portIds);
            if (query == null || query.Count() == 0)
                return null;
            return query.Select(x => x.RegionTypeUid).ToArray<string>();
        }

        /// <summary>
        /// 根据站点数组取得城市
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <returns></returns>
        public string[] GetCityByPort(string[] portIds)
        {
            if (portIds.Length == 0)
                return null;
            IQueryable<V_Point_AirEntity> query = RetrieveVPointAirByPort(portIds);
            if (query == null || query.Count() == 0)
                return null;
            return query.Select(x => x.CityTypeUid).ToArray<string>();
        }

        /// <summary>
        /// 根据所选站点ID获取相应区域信息
        /// </summary>
        /// <returns></returns>
        public DataView GetRegionByPointId(string[] pointIds)
        {
            try
            {
                return g_Repository.GetRegionByPointId(pointIds);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 根据区域名获取相应站点ID
        /// </summary>
        /// <returns></returns>
        public DataTable GetPointIdByCityName(string CityName)
        {
            try
            {
                return g_Repository.GetPointIdByCityName(CityName);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
    }
}
