using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Air
{
    /// <summary>
    /// 名称：RPT_AQIForecastService.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-08-24
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 空气质量预报表服务层类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class RPT_AQIForecastService
    {
        /// <summary>
        /// 空气质量预报表仓储层
        /// </summary>
        RPT_AQIForecastRepository r_AQIForecast = Singleton<RPT_AQIForecastRepository>.GetInstance();
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="AQIForecast">质量预报实体</param>
        /// <returns>成功返回1，失败返回0，实体数组空返回2</returns>
        public int Add(RPT_AQIForecastEntity abnormal)
        {
            return r_AQIForecast.Add(abnormal);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="AQIForecast">质量预报实体</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Update(RPT_AQIForecastEntity abnormal)
        {
            return r_AQIForecast.Update(abnormal);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Delete(string IDs)
        {
            return r_AQIForecast.Delete(IDs);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetList(string strWhere)
        {
            return r_AQIForecast.GetList(strWhere);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetAQIList()
        {
            return r_AQIForecast.GetAQIList();
        }
    }
}
