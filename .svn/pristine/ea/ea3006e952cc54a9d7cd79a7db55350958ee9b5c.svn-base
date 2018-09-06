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
    /// 名称：TaskFileInfoRepository.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-11-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 任务文件信息仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class TaskFileInfoRepository //: BaseGenericRepository<BaseDataModel, TaskFileInfoEntity>
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
        TaskFileInfoDAL m_TaskFileInfoDAL = Singleton<TaskFileInfoDAL>.GetInstance();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回新增的行数</returns>
        public int Add(TaskFileInfoEntity model)
        {
            if (model != null)
            {
                return m_TaskFileInfoDAL.Add(model);
            }
            return 0;
        }

        /// <summary>
        /// 批量新增数据
        /// </summary>
        /// <param name="model">实体类数组</param>
        /// <returns>返回新增的行数</returns>
        public int AddBatch(params TaskFileInfoEntity[] models)
        {
            if (models != null && models.Length > 0)
            {
                return m_TaskFileInfoDAL.AddBatch(models);
            }
            return 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回更新的行数</returns>
        public int Update(TaskFileInfoEntity model)
        {
            if (model != null)
            {
                return m_TaskFileInfoDAL.Update(model);
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
            return m_TaskFileInfoDAL.Delete(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns>返回数据集</returns>
        public DataTable GetList(string strWhere)
        {
            return m_TaskFileInfoDAL.GetList(strWhere);
        }
        #endregion
    }
}
