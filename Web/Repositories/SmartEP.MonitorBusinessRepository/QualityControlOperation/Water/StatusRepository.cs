using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.QualityControlOperation.Water;
using SmartEP.DomainModel.WaterQualityControlOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：StatusRepository.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：状态表仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class StatusRepository
    {
        StatusDAL d_StatusDAL = Singleton<StatusDAL>.GetInstance();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回插入条数</returns>
        public int Add(StatusEntity model)
        {
            if (model != null)
            {
                return d_StatusDAL.Add(model);
            }
            return 0;
        }

        /// <summary>
        /// 批量增加数据
        /// </summary>
        /// <param name="models">实体类数组</param>
        /// <returns>返回插入条数</returns>
        public int AddBatch(params StatusEntity[] models)
        {
            if (models != null && models.Length > 0)
            {
                return d_StatusDAL.AddBatch(models);
            }
            return 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回是否更新成功</returns>
        public bool Update(StatusEntity model)
        {
            if (model != null)
            {
                return d_StatusDAL.Update(model);
            }
            return false;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return d_StatusDAL.Delete(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return d_StatusDAL.GetList(strWhere);
        }
    }
}
