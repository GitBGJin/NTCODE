using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Common
{
    /// <summary>
    /// 名称：AuditDataRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-10-11
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 审核处理数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditDataRepository
    {
        /// <summary>
        /// 审核数据DAL
        /// </summary>
        AuditDataDAL g_AuditDataDAL = Singleton<AuditDataDAL>.GetInstance();

        /// <summary>
        /// 生成审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateData(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GenerateData(applicationType, portIds, dateStart, dateEnd, out errMsg);
        }
        /// <summary>
        /// 生成审核数据(超级站审核区分仪器，Update By Rondo)
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateData(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, string[] factors, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GenerateData(applicationType, portIds, dateStart, dateEnd, factors, out errMsg);
        }
        /// <summary>
        /// 生成审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataDay(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GenerateDataDay(applicationType, portIds, dateStart, dateEnd, out errMsg);
        }
        /// <summary>
        /// 生成审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataNew(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GenerateDataNew(applicationType, portIds, dateStart, dateEnd, out errMsg);
        }
        /// <summary>
        /// 生成审核数据（超级站）
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataSuper(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GenerateDataSuper(applicationType, portIds, dateStart, dateEnd, out errMsg);
        }

        /// <summary>
        /// 从审核后小时数据生成审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataFromHourReport(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GenerateDataFromHourReport(applicationType, portIds, dateStart, dateEnd, out errMsg);
        }
        /// <summary>
        /// 从审核后小时数据生成审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataFromHoursReport(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GenerateDataFromHoursReport(applicationType, portIds, dateStart, dateEnd, out errMsg);
        }
        /// <summary>
        /// 生成区域审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="regionGuids">区域数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataRegion(ApplicationType applicationType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GenerateDataRegion(applicationType, regionGuids, dateStart, dateEnd, out errMsg);
        }

        /// <summary>
        /// 生成城市审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="cityGuids">区域数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataCity(ApplicationType applicationType, string[] cityGuids, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GenerateDataCity(applicationType, cityGuids, dateStart, dateEnd, out errMsg);
        }

        /// <summary>
        /// 从审核后日数据生成审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="portIds">站点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataFromDayReport(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GenerateDataFromDayReport(applicationType, portIds, dateStart, dateEnd, out errMsg);
        }

        /// <summary>
        /// 生成区域审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="regionGuids">区域数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataRegionFromDay(ApplicationType applicationType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GenerateDataRegionFromDay(applicationType, regionGuids, dateStart, dateEnd, out errMsg);
        }

        /// <summary>
        /// 生成城市审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="cityGuids">区域数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateDataCityFromDay(ApplicationType applicationType, string[] cityGuids, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GenerateDataCityFromDay(applicationType, cityGuids, dateStart, dateEnd, out errMsg);
        }

        /// <summary>
        /// 从审核历史表导入审核预处理数据
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="portIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool GetDataFromHis(ApplicationType applicationType, string[] portIds, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            return g_AuditDataDAL.GetDataFromHis(applicationType, portIds, dateStart, dateEnd, out errMsg);
        }
        /// <summary>
        /// 生成城市审核数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="cityGuids">区域数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public DataView getDataViewDayXJSNew(DateTime dtBegin, DateTime dtEnd, string[] portIds)
        {
            return g_AuditDataDAL.getDataViewDayXJSNew(dtBegin, dtEnd, portIds);
        }
    }
}
