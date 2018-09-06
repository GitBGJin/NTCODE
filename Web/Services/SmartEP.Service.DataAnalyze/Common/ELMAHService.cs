using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Common;
using SmartEP.Service.BaseData.MPInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Common
{
    /// <summary>
    /// 名称：ELMAHService.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-27
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：异常日志(软件平台异常日志、系统运行环境异常日志、通讯数据包异常日志)
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ELMAHService
    {
        /// <summary>
        /// 异常日志仓储层实例化
        /// </summary>
        ELMAHRepository g_ELMAHRepository = Singleton<ELMAHRepository>.GetInstance();
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;

        #region 增删改查
        /// <summary>
        /// 增加对象
        /// </summary>
        /// <param name="entity"></param>
        public void Add(ELMAHEntity entity)
        {
            g_ELMAHRepository.Add(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(ELMAHEntity entity)
        {
            g_ELMAHRepository.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities"></param>
        public void BatchDelete(List<ELMAHEntity> entities)
        {
            g_ELMAHRepository.BatchDelete(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            g_ELMAHRepository.Update();
        }

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<IBaseEntityProperty> Retrieve(Expression<Func<ELMAHEntity, bool>> predicate)
        {
            return this.Retrieve(predicate);
        }
        #endregion

        #region << ADO.NET >>
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
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dt = g_ELMAHRepository.GetDataPager(applicationType, monitoringPointUids, types, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy).ToTable();
            dt.Columns.Add("PointName", typeof(string)).SetOrdinal(0);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                dt.Rows[i]["PointName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
            }
            return dt.DefaultView;
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
            return g_ELMAHRepository.GetExportData(applicationType, monitoringPointUids, types, dateStart, dateEnd, orderBy);
        }

        #endregion
    }
}
