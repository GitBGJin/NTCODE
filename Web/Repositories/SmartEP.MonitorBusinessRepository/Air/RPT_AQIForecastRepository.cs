using SmartEP.Core.Generic;
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
    /// 名称：RPT_AQIForecastRepository.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-08-24
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：空气质量预报表仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class RPT_AQIForecastRepository
    {
        RPT_AQIForecastDAL d_AQIForecastDAL = Singleton<RPT_AQIForecastDAL>.GetInstance();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回插入条数</returns>
        public int Add(RPT_AQIForecastEntity model)
        {
            if (model != null)
            {
                return d_AQIForecastDAL.Add(model);
            }
            return 0;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回是否更新成功</returns>
        public bool Update(RPT_AQIForecastEntity model)
        {
            if (model != null)
            {
                return d_AQIForecastDAL.Update(model);
            }
            return false;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="IDs">IDs</param>
        /// <returns></returns>
        public bool Delete(string IDs)
        {
            return d_AQIForecastDAL.Delete(IDs);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return d_AQIForecastDAL.GetList(strWhere);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAQIList()
        {
            return d_AQIForecastDAL.GetAQIList();
        }
    }
}
