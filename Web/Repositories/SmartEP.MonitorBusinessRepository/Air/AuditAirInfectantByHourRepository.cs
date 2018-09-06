﻿using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Air
{
    /// <summary>
    /// 名称：AuditInfectantByHourRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 空气审核小时数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditAirInfectantByHourRepository : BaseGenericRepository<MonitoringBusinessModel, AuditAirInfectantByHourEntity>
    {
        /// <summary>
        /// 空气审核小时数据DAL
        /// </summary>
        AuditAirInfectantByHourDAL g_AuditAirInfectantByHour = Singleton<AuditAirInfectantByHourDAL>.GetInstance();

        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return true;
        }

        /// <summary>
        /// 行转列数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="isAllDate">是否补充缺失数据</param>
        /// <returns></returns>
        public DataView GetDataView(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, bool isAllDate = false, string PointType = "0")
        {
            return g_AuditAirInfectantByHour.GetDataView(portIds, factors, dtStart, dtEnd, isAllDate, PointType);
        }

        /// <summary>
        /// 行转列数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="isAllDate">是否补充缺失数据</param>
        /// <returns></returns>
        public DataView GetDataViewS(string[] portIds,string InsId, string[] factors, DateTime dtStart, DateTime dtEnd, bool isAllDate = false, string PointType = "0")
        {
            return g_AuditAirInfectantByHour.GetDataViewS(portIds,InsId, factors, dtStart, dtEnd, isAllDate, PointType);
        }

        /// <summary>
        /// 行转列数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="isAllDate">是否补充缺失数据</param>
        /// <returns></returns>
        public DataView GetDataViewSuper(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, bool isAllDate = false, string PointType = "0")
        {
            return g_AuditAirInfectantByHour.GetDataViewSuper(portIds, factors, dtStart, dtEnd, isAllDate, PointType);
        }

        /// <summary>
        /// 取得统计数据固定因子（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            return g_AuditAirInfectantByHour.GetStatisticalData(portIds, factors, dateStart, dateEnd);
        }

        /// <summary>
        /// 取得统计数据所有因子（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalAllData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, bool isAllDate = false)
        {
            return g_AuditAirInfectantByHour.GetStatisticalAllData(portIds, factors, dateStart, dateEnd, isAllDate);
        }

        /// <summary>
        /// 取得统计数据所有因子（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalMutilPoint(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, bool isAllDate = false)
        {
            return g_AuditAirInfectantByHour.GetStatisticalMutilPoint(portIds, factors, dateStart, dateEnd, isAllDate);
        }

        /// <summary>
        /// 根据点位类型、时间段获取审核状态统计信息
        /// </summary>
        /// <param name="application">系统标识Uid</param>
        /// EnumMapping.GetApplicationValue(ApplicationValue.Air)
        /// <param name="SiteType">站点类型Uid</param>
        /// <param name="beginTime">开始日期</param>
        /// <param name="endTime">截止日期</param>
        /// <returns></returns>
        public DataTable AuditFlagStatisticsByPoint(string application, string SiteType, DateTime beginTime, DateTime endTime)
        {
            return g_AuditAirInfectantByHour.AuditFlagStatisticsByPoint(application, SiteType, beginTime, endTime);
        }

        /// <summary>
        /// 更新因子审核状态
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="AuditStatusUid"></param>
        public void AuditFactorStatusUpdate(int PointID, string AuditStatusUid, string UserName)
        {
            g_AuditAirInfectantByHour.AuditFactorStatusUpdate(PointID, AuditStatusUid, UserName);
        }

        /// <summary>
        /// 更新因子审核状态
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="AuditStatusUid"></param>
        public void AuditFactorStatusUpdate(int PointID, string[] factors, string AuditStatusUid, string UserName)
        {
            g_AuditAirInfectantByHour.AuditFactorStatusUpdate(PointID, factors, AuditStatusUid, UserName);
        }

        /// <summary>
        /// 更新因子审核小时状态
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="UserName"></param>
        public void AuditFactorHourStatusUpdate(int PointID, DateTime beginTime, DateTime endTime, string[] factors, string UserName, string PointType)
        {
            g_AuditAirInfectantByHour.AuditFactorHourStatusUpdate(PointID, beginTime, endTime, factors, UserName, PointType);
        }

        /// <summary>
        /// 更新因子审核小时状态
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="UserName"></param>
        public void AuditFactorHourStatusUpdateS(int PointID,string InsId, DateTime beginTime, DateTime endTime, string[] factors, string UserName, string PointType)
        {
            g_AuditAirInfectantByHour.AuditFactorHourStatusUpdateS(PointID,InsId, beginTime, endTime, factors, UserName, PointType);
        }

        /// <summary>
        /// 更新因子审核小时状态(气象)
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="UserName"></param>
        public void AuditFactorHourStatusUpdateWea(int PointID, DateTime beginTime, DateTime endTime, string[] factors, string UserName, string PointType)
        {
            g_AuditAirInfectantByHour.AuditFactorHourStatusUpdateWea(PointID, beginTime, endTime, factors, UserName, PointType);
        }
        /// <summary>
        /// 获取审核预处理小时记录数
        /// </summary>
        /// <param name="AuditStatusUid"></param>
        /// <returns></returns>
        public int GetAuditRecordNumByHour(string AuditStatusUid, string[] factors)
        {
            return g_AuditAirInfectantByHour.GetAuditRecordNumByHour(AuditStatusUid, factors);
        }

        /// <summary>
        /// 获取审核预处理小时记录数(超级站)
        /// </summary>
        /// <param name="AuditStatusUid"></param>
        /// <returns></returns>
        public int GetAuditRecordNumByHourSuper(string AuditStatusUid, string[] factors)
        {
            return g_AuditAirInfectantByHour.GetAuditRecordNumByHourSuper(AuditStatusUid, factors);
        }
        /// <summary>
        /// 获取审核预处理小时记录数(气象)
        /// </summary>
        /// <param name="AuditStatusUid"></param>
        /// <returns></returns>
        public int GetAuditRecordNumByHourWea(int PointID, DateTime beginTime, DateTime endTime)
        {
            return g_AuditAirInfectantByHour.GetAuditRecordNumByHourWea(PointID, beginTime, endTime);
        }
    }
}
