using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：RealSamplesRepository.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 实验室比对记录表仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class RealSamplesRepository : BaseGenericRepository<MonitoringBusinessModel, RealSampleEntity>
    {
        public override bool IsExist(string id)
        {
            return Retrieve(x => x.Id.Equals(Convert.ToInt32(id))).Count() == 0 ? false : true;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">实验室记录实体</param>
        public void add(RealSampleEntity[] entity)
        {
            Add(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实验室记录实体</param>
        public void delete(RealSampleEntity entity)
        {
            Delete(entity);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="SampleNumber">样品编号</param>
        /// <param name="Tstamp">时间戳</param>
        /// <returns></returns>
        public IQueryable<RealSampleEntity> RetrieveData(string TaskCode)
        {
            return Retrieve(p => p.TaskCode == TaskCode);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="SampleNumber">样品编号</param>
        /// <param name="Tstamp">时间戳</param>
        /// <returns></returns>
        public IQueryable<RealSampleEntity> RetrieveDataNew(Guid Id)
        {
            return Retrieve(p => p.SampleGuid == Id);
        }
    }
}
