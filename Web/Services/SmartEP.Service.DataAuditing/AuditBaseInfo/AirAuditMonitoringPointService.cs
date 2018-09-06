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
    public class AirAuditMonitoringPointService : AbstractAuditMonitoringPoint
    {
        private string applicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Air);

        /// <summary>
        /// 获取所有空气审核点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPList(string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.ApplicationUid == applicationValue);
        }


        /// <summary>
        /// 根据审核类型获取点位列表
        /// </summary>
        /// <param name="pointType"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirByAuditType(string pointType, string UserGuid)
        {
            //return RetrieveAirPointList(pointType);

            //#region 注释
            IQueryable<MonitoringPointEntity> pointList = null;
            if (pointType.Equals("0"))
                pointList = RetrieveAirMPListByCountryControlled(UserGuid);//国控点
            else if (pointType.Equals("1"))
                pointList = RetrieveAirMPListByCityControlled(UserGuid);//市控点位
            else if (pointType.Equals("2"))
                pointList = RetrieveAirMPListBySuper(UserGuid);//超级站点位
            else if (pointType.Equals("3"))
                pointList = RetrieveAirMPListByProvinceControlled(UserGuid);//省控点位
            else if (pointType.Equals("4"))
                pointList = RetrieveAirMPListByRegionControlled(UserGuid);//区控点位
            else if (pointType.Equals("5"))
                pointList = RetrieveAirMPListByModel(UserGuid);//创模点位
            else if (pointType.Equals("6"))
                pointList = RetrieveAirMPListZY();//中意项目
            else if (pointType.Equals("7"))
                pointList = RetrieveAirPointList("bdf0837a-eb59-4c4a-a05f-c774a17f3077", UserGuid);//路边站
            else if (pointType.Equals("8"))
                pointList = RetrieveAirPointList("c1158eb6-4d69-4846-a963-d16b9d2794ca", UserGuid);//渔洋山
            else if (pointType.Equals("9"))
                pointList = RetrieveAirPointList("19ae1c65-e78e-41f8-973d-efd737bd4d16",UserGuid);//移动车
            return pointList;
            //#endregion
        }

        /// <summary>
        /// 获取所有空气审核国控点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByCountryControlled(string UserGuid)
        {
            //MonitoringPointEntity point = new MonitoringPointEntity();
            //List<MonitoringPointEntity> pointList = RetrieveMPList().Where(p => p.ApplicationUid == applicationValue && p.ContrlUid.ToLower().Equals("6fadff52-2338-4319-9f1d-7317823770ad")).ToList<MonitoringPointEntity>();
            //pointList.Add(new MonitoringPointEntity { MonitoringPointUid = ("6fadff52-2338-4319-9f1d-7317823770ad").ToUpper(), ContrlUid = null, MonitoringPointName = "国控点" });
            //return pointList.AsQueryable<MonitoringPointEntity>();
            return RetrieveMPList(UserGuid).Where(p => p.ApplicationUid == applicationValue && p.ContrlUid.ToLower().Equals("6fadff52-2338-4319-9f1d-7317823770ad"));
        }

        /// <summary>
        /// 获取所有空气审核省控点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByProvinceControlled(string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.ApplicationUid == applicationValue && p.ContrlUid.ToLower().Equals("bc4fca0c-745f-49d7-b9e9-0af67d3219e6"));
        }

        /// <summary>
        /// 获取所有空气审核市控点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByCityControlled(string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.ApplicationUid == applicationValue && p.ContrlUid.ToLower().Equals("b107d493-b1a3-4ebd-b991-b0e340becec1"));
        }

        /// <summary>
        /// 获取所有空气审核区控点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByRegionControlled(string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.ApplicationUid == applicationValue && p.ContrlUid.ToLower().Equals("461aefd0-c79f-4ab8-848d-67c623d4bba1"));
        }


        /// <summary>
        /// 获取所有空气审核超级站点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListBySuper(string UserGuid)
        {
            return RetrieveMPListSuper(UserGuid).Where(p => p.ApplicationUid == applicationValue && p.MonitoringPointExtensionForEQMSAirEntity.SuperOrNot == true);
        }

        /// <summary>
        /// 获取所有空气审核创模点位
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListByModel(string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.ApplicationUid == applicationValue && p.MonitoringPointExtensionForEQMSAirEntity.ModelOrNot == true);
        }

        /// <summary>
        /// 获取所有空气审核中意项目
        /// </summary>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirMPListZY()
        {
            return RetrieveMPListByAuditTypeUid("0490d15e-70ab-43a1-b59e-e735c8f83cc9");
        }

        /// <summary>
        /// 根据conroluid获取点位
        /// </summary>
        /// <param name="controlUID"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirPointList(string controlUID, string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.ApplicationUid == applicationValue && p.ContrlUid.ToLower().Equals(controlUID));
        }

        /// <summary>
        /// 根据SiteTypeUID获取点位
        /// </summary>
        /// <param name="SiteTypeUID"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirPointListBySiteType(string SiteTypeUID, string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.ApplicationUid == applicationValue && p.SiteTypeUid.ToLower().Equals(SiteTypeUID.ToLower()));
        }
        /// <summary>
        /// 根据站点id获取站点信息
        /// </summary>
        /// <param name="PointId"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> RetrieveAirPointListByPointId(int PointId, string UserGuid)
        {
            return RetrieveMPList(UserGuid).Where(p => p.ApplicationUid == applicationValue && p.PointId.Equals(PointId));
        }
    }
}
