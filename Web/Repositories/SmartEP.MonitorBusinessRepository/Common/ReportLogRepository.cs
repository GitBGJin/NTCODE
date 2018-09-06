using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Common
{
    /// <summary>
    /// 名称：ReportLogRepository.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-11-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 监测指标
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ReportLogRepository : BaseGenericRepository<MonitoringBusinessModel, ReportLogEntity>
    {
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
        /// 新增数据
        /// </summary>
        /// <param name="reportLog">实体类对象</param>
        /// <returns></returns>
        public void ReportLogAdd(ReportLogEntity reportLog)
        {
            Add(reportLog);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="customDatum">实体类对象</param>
        public void ReportLogUpdate(object customDatum)
        {
            Update(customDatum);
        }
        /// <summary>
        /// 根据页面pageTypeID、waterOrAirType水或气的类型条件查询数据
        /// </summary>
        /// <param name="pageTypeID">页面ID</param>
        /// <param name="waterOrAirType">水或气的类型0：水，1：气</param>
        /// <returns></returns>
        public IQueryable<ReportLogEntity> CustomDatumRetrieve(string pageTypeID, int waterOrAirType)
        {
            return Retrieve(it => it.PageTypeID == pageTypeID && it.WaterOrAirType == waterOrAirType);
        }
        /// <summary>
        /// 根据主键customID获取数据
        /// </summary>
        /// <param name="customID"></param>
        /// <returns></returns>
        public IQueryable<ReportLogEntity> ReportLogRetrieveByid(int id)
        {
            return Retrieve(it => it.Id == id);
        }
    }
}
