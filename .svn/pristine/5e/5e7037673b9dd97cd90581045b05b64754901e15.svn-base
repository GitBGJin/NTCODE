using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Core.Enums;
using SmartEP.DomainModel.BaseData;
using SmartEP.MonitoringBusinessRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAuditing.AuditBaseInfo
{
    /// <summary>
    /// 名称：AbstractMonitoringPoint.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-16
    /// 维护人员：
    /// 最新维护人员：季柯
    /// 最新维护日期：2015-08-27
    /// 功能摘要：
    /// 站点信息抽象类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterAuditMonitoringPointService : AbstractAuditMonitoringPoint
    {
        //地表水应用程序值
        private string applicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Water);

        /// <summary>
        /// 获取所有地表水审核点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveWaterMPList(string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.ApplicationUid == applicationValue);
        }

        /// <summary>
        /// 根据审核类型获取点位列表
        /// </summary>
        /// <param name="pointType"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveWaterByAuditType(string pointType, string UserGuid)
        {
            //return RetrieveAirPointList(pointType);//湖体;
            //#region 注释
            IQueryable<MonitoringPointEntity> pointList = null;
            if (pointType.Equals("0"))
                pointList = RetrieveMPListByWaterSource(UserGuid);//水源地点位
            else if (pointType.Equals("1"))
                pointList = RetrieveMPListByCityRiver(UserGuid);//城区河道点位
            else if (pointType.Equals("2"))
                pointList = RetrieveMPListByFloat(UserGuid);//浮标站点位
            else if (pointType.Equals("3"))
                pointList = RetrieveMPListByManual(UserGuid);//人工监测点位
            else if (pointType.Equals("4"))
                pointList = RetrieveWaterPointList("46540455-062c-4a58-a8bb-ea2280a2d6ce", UserGuid);//入湖河道
            else if (pointType.Equals("5"))
                pointList = RetrieveWaterPointList("c2bd6e53-cdee-4af2-badd-5d32cee2b753", UserGuid);//交接断面
            else if (pointType.Equals("6"))
                pointList = RetrieveWaterPointList("a1be55a0-7aeb-44e7-ad29-577dba1b3200", UserGuid);//湖体
            else if (pointType.Equals("7"))
                pointList = RetrieveMPListByLake(UserGuid);//人工监测点位
            else if (pointType.Equals("8"))
                pointList = RetrieveMPListByCounty(UserGuid);//区县自建站
            return pointList;
            //#endregion
        }


        //<summary>
        //根据地表水站点类型Uid获取地表水审核点位列表 
        //</summary>
        //<param name="siteTypeUid">站点类型Uid</param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveListBySiteTypeName(string siteTypeUid, string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.SiteTypeUid == siteTypeUid);
        }

        //<summary>
        //获取水源地点位
        //</summary>
        //<param name="SiteTypeName"></param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPListByWaterSource(string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.SiteTypeUid.ToLower().Equals("81381370-1744-41fd-b5e4-4ffbee24288e"));
        }
        // <summary>
        // 获取区县地点位
        // </summary>
        // <param name="SiteTypeName"></param>
        // <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPListByCounty(string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.SiteTypeUid.ToLower().Equals("6136514b-894c-4a4f-ba4f-d0a739030e26"));
        }

        //<summary>
        //获取城区河道点位
        //</summary>
        //<param name="SiteTypeName"></param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPListByCityRiver(string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.SiteTypeUid.ToLower().Equals("52c3cea3-4a60-4b80-97e5-4313b3d759db"));
        }

        //<summary>
        //获取浮标站点位
        //</summary>
        //<param name="SiteTypeName"></param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPListByFloat(string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.SiteTypeUid.ToLower().Equals("a10a8482-1d30-44ea-8b03-b724f1ebeaa8"));
        }

        //<summary>
        //获取人工监测点位
        //</summary>
        //<param name="SiteTypeName"></param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPListByManual(string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.SiteTypeUid.ToLower().Equals("160e08ec-1d1b-4095-b898-3a3d925ed4e6"));
        }
        //<summary>
        //获取湖体气象站
        //</summary>
        //<param name="SiteTypeName"></param>
        //<returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveMPListByLake(string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.SiteTypeUid.ToLower().Equals("e8fa7119-4c64-4f68-a012-53706a3a0d83"));
        }
        /// <summary>
        /// 根据conroluid获取点位
        /// </summary>
        /// <param name="controlUID"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveWaterPointList(string siteTypeUID, string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.ApplicationUid == applicationValue && p.SiteTypeUid.ToLower().Equals(siteTypeUID));
        }

    }
}
