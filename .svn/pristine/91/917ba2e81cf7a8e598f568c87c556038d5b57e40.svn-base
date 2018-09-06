using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Report
{
    /// <summary>
    /// 名称：CustomDataService.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-10-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 自定义数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class CustomDataService : BaseGenericRepository<MonitoringBusinessModel, CustomDatumEntity>
    {
        public override bool IsExist(string strKey)
        {
            return true;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="customDatum">实体类对象</param>
        public void CustomDatumAdd(CustomDatumEntity customDatum)
        {
            Add(customDatum);
        }

        /// <summary>
        /// 获取自定义所有数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<CustomDatumEntity> CustomDatumAllRetrieve()
        {
            return Retrieve(it => it.Flag == 0);
        }
        /// <summary>
        /// 根据页面pageTypeID、waterOrAirType水或气的类型条件查询数据
        /// </summary>
        /// <param name="pageTypeID">页面ID</param>
        /// <param name="waterOrAirType">水或气的类型0：水，1：气</param>
        /// <returns></returns>
        public IQueryable<CustomDatumEntity> CustomDatumRetrieve(string pageTypeID, int waterOrAirType)
        {
            return Retrieve(it => it.PageTypeID == pageTypeID && it.WaterOrAirType == waterOrAirType && it.Flag == 0);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="customDatum">实体类对象</param>
        public void CustomDatumUpdate(object customDatum)
        {
            Update(customDatum);
        }

        /// <summary>
        /// 根据页面pageTypeID、waterOrAirType水或气的类型、groupGanme条件查询数据
        /// </summary>
        /// <param name="pageTypeID">页面ID</param>
        /// <param name="waterOrAirType">水或气的类型0：水，1：气</param>
        /// <param name="groupGanme">组名</param>
        /// <returns></returns>
        public IQueryable<CustomDatumEntity> UpdateCustomDatumRetrieve(string pageTypeID, int waterOrAirType,string groupGanme)
        {
            return Retrieve(it => it.PageTypeID == pageTypeID && it.WaterOrAirType == waterOrAirType && it.GroupName == groupGanme && it.Flag == 0);
        }

        /// <summary>
        /// 根据主键customID获取数据
        /// </summary>
        /// <param name="customID"></param>
        /// <returns></returns>
        public IQueryable<CustomDatumEntity> CustomDatumRetrieveByCustomID(int customID)
        {
            return Retrieve(it => it.CustomID == customID&& it.Flag == 0);
        }
    }
}
