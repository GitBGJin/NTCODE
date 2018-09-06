﻿using SmartEP.Core.Generic;
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
    /// 名称：StandardSolutionChangeRepository.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：试剂标液更换仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class StandardSolutionChangeRepository
    {
        StandardSolutionChangeDAL d_StandardSolutionChangeDAL = Singleton<StandardSolutionChangeDAL>.GetInstance();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回插入条数</returns>
        public int Add(StandardSolutionChangeEntity model)
        {
            if (model != null)
            {
                return d_StandardSolutionChangeDAL.Add(model);
            }
            return 0;
        }

        /// <summary>
        /// 批量增加数据
        /// </summary>
        /// <param name="models">实体类数组</param>
        /// <returns>返回插入条数</returns>
        public int AddBatch(params StandardSolutionChangeEntity[] models)
        {
            if (models != null && models.Length > 0)
            {
                return d_StandardSolutionChangeDAL.AddBatch(models);
            }
            return 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回是否更新成功</returns>
        public bool Update(StandardSolutionChangeEntity model)
        {
            if (model != null)
            {
                return d_StandardSolutionChangeDAL.Update(model);
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
            return d_StandardSolutionChangeDAL.Delete(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return d_StandardSolutionChangeDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获取前几条数据
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="pointId"></param>
        /// <param name="ChangeDate"></param>
        /// <param name="filedOrder"></param>
        /// <returns></returns>
        public DataTable GetList(int Top, string strWhere, string filedOrder)
        {
            return d_StandardSolutionChangeDAL.GetList(Top, strWhere, filedOrder);
        }
    }
}
