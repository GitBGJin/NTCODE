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
    /// 名称：WaterInspectionBaseRepository.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-13
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：任务记录表仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterInspectionBaseRepository
    {
        WaterInspectionBaseDAL d_WaterInspectionBaseDAL = Singleton<WaterInspectionBaseDAL>.GetInstance();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回插入条数</returns>
        public int Add(WaterInspectionBaseEntity model)
        {
            if (model != null)
            {
                return d_WaterInspectionBaseDAL.Add(model);
            }
            return 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回是否更新成功</returns>
        public bool Update(WaterInspectionBaseEntity model)
        {
            if (model != null)
            {
                return d_WaterInspectionBaseDAL.Update(model);
            }
            return false;
        }

        /// <summary>
        /// 新增或更新数据（根据TaskCode判断）
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回是否更新成功</returns>
        public bool AddOrUpdateByTaskCode(WaterInspectionBaseEntity model)
        {
            if (model != null)
            {
                return d_WaterInspectionBaseDAL.AddOrUpdateByTaskCode(model);
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
            return d_WaterInspectionBaseDAL.Delete(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return d_WaterInspectionBaseDAL.GetList(strWhere);
        }

        /// <summary>
        /// 根据TID更新表单中对应的TaskCode值
        /// </summary>
        /// <param name="taskCode">任务编号</param>
        /// <param name="TID">下发任务（和TaskCode值一样），临时任务（生成的Guid）</param>
        /// <param name="formCode">表单编号</param>
        /// <returns></returns>
        public int UpdateTaskCodeOfFormByTID(string taskCode, string TID, string formCode)
        {
            return d_WaterInspectionBaseDAL.UpdateTaskCodeOfFormByTID(taskCode, TID, formCode);
        }

        /// <summary>
        /// 根据TempTaskID更新表单中对应的TaskCode值
        /// </summary>
        /// <param name="taskCode">任务编号</param>
        /// <param name="formCode">表单编号</param>
        /// <returns></returns>
        public int UpdateTaskCodeOfFormByTempTaskID(string taskCode, string formCode)
        {
            return d_WaterInspectionBaseDAL.UpdateTaskCodeOfFormByTempTaskID(taskCode, formCode);
        }
    }
}
