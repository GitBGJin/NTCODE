using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Common
{
    /// <summary>
    /// 名称：ELMAHRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-27
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 异常日志(软件平台异常日志、系统运行环境异常日志、通讯数据包异常日志)
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ELMAHRepository : BaseGenericRepository<MonitoringBusinessModel, ELMAHEntity>
    {
        /// <summary>
        /// 异常日志DAL
        /// </summary>
        ELMAHDAL g_ELMAHDAL = new ELMAHDAL();

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
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="applicationType">应用类型（空气、地表水、噪声）</param>
        /// <param name="monitoringPointUids">测点数据</param>
        /// <param name="types">日志类型</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(ApplicationType applicationType, string[] monitoringPointUids, string[] types, DateTime dateStart, DateTime dateEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "tstamp")
        {
            return g_ELMAHDAL.GetDataNewPager(applicationType, monitoringPointUids, types, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="applicationType">应用类型（空气、地表水、噪声）</param>
        /// <param name="monitoringPointUids">测点数据</param>
        /// <param name="types">日志类型</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(ApplicationType applicationType, string[] monitoringPointUids, string[] types, DateTime dateStart, DateTime dateEnd, string orderBy = "ExceptionTime")
        {
            return g_ELMAHDAL.GetExportData(applicationType, monitoringPointUids, types, dateStart, dateEnd, orderBy);
        }
    }
}
