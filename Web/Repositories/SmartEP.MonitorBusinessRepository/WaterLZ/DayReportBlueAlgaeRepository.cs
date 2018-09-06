using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness.WaterLZ;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;

namespace SmartEP.MonitoringBusinessRepository.WaterLZ
{
    /// <summary>
    /// 名称：DayReportBlueAlgaeRepository.cs
    /// 创建人：吕云
    /// 创建日期：2016-07-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 蓝藻预警发布：日数据处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DayReportBlueAlgaeRepository:BaseGenericRepository<MonitoringBusinessModel, WaterDayReportEntity>
    {
        public override bool IsExist(string strKey)
        {
            throw new NotImplementedException();
        }
        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        DayReportDAL m_DayReportDAC = Singleton<DayReportDAL>.GetInstance();

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal)
        {
            return m_DayReportDAC.GetDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal);
        }

        #endregion
    }
}
