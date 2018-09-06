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
    /// 名称：TaskActionConfigRepository.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-13
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：任务工作点配置仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class TaskActionConfigRepository
    {
        TaskActionConfigDAL d_TaskActionConfigDAL = Singleton<TaskActionConfigDAL>.GetInstance();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回插入条数</returns>
        public int Add(TaskActionConfigEntity model)
        {
            if (model != null)
            {
                return d_TaskActionConfigDAL.Add(model);
            }
            return 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回是否更新成功</returns>
        public bool Update(TaskActionConfigEntity model)
        {
            if (model != null)
            {
                return d_TaskActionConfigDAL.Update(model);
            }
            return false;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">序号</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return d_TaskActionConfigDAL.Delete(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="strWhere">where语句</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return d_TaskActionConfigDAL.GetList(strWhere);
        }
    }
}
