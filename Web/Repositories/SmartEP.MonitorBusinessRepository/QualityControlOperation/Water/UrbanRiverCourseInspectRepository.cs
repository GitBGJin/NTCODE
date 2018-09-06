using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.QualityControlOperation.Water;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.WaterQualityControlOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：UrbanRiverCourseInspectRepository.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-11-03
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 城区河道巡检仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class UrbanRiverCourseInspectRepository //: BaseGenericRepository<BaseDataModel, UrbanRiverCourseInspectEntity>
    {
        ///// <summary>
        ///// 根据key主键判断记录是否存在
        ///// </summary>
        ///// <param name="strKey"></param>
        ///// <returns></returns>
        //public override bool IsExist(string strKey)
        //{
        //    return RetrieveCount(x => x.id.Equals(strKey)) == 0 ? false : true;
        //}

        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        UrbanRiverCourseInspectDAL m_UrbanRiverCourseInspectDAL = Singleton<UrbanRiverCourseInspectDAL>.GetInstance();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回新增的行数</returns>
        public int Add(UrbanRiverCourseInspectEntity model)
        {
            if (model != null)
            {
                return m_UrbanRiverCourseInspectDAL.Add(model);
            }
            return 0;
        }

        /// <summary>
        /// 批量新增数据
        /// </summary>
        /// <param name="model">实体类数组</param>
        /// <returns>返回新增的行数</returns>
        public int AddBatch(params UrbanRiverCourseInspectEntity[] models)
        {
            if (models != null && models.Length > 0)
            {
                return m_UrbanRiverCourseInspectDAL.AddBatch(models);
            }
            return 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回更新的行数</returns>
        public int Update(UrbanRiverCourseInspectEntity model)
        {
            if (model != null)
            {
                return m_UrbanRiverCourseInspectDAL.Update(model);
            }
            return 0;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回删除的行数</returns>
        public int Delete(int id)
        {
            return m_UrbanRiverCourseInspectDAL.Delete(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns>返回数据集</returns>
        public DataTable GetList(string strWhere)
        {
            return m_UrbanRiverCourseInspectDAL.GetList(strWhere);
        }
        #endregion
    }
}
