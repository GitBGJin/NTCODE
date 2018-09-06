using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：RealSamplesService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 实验室比对记录表服务层类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class RealSamplesService
    {
        /// <summary>
        /// 实验室比对仓储层
        /// </summary>
        RealSamplesRepository r_RealSample = new RealSamplesRepository();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="realSampleEntity">实体类</param>
        public void add(RealSampleEntity[] realSampleEntity)
        {
            r_RealSample.add(realSampleEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="realSampleEntity">实体类</param>
        public void delete(RealSampleEntity realSampleEntity)
        {
            r_RealSample.delete(realSampleEntity);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public void Update(RealSampleEntity realSampleEntity)
        {
            r_RealSample.Update(realSampleEntity);
        }

        /// <summary>
        /// 根据质控任务获取数据
        /// </summary>
        /// <param name="SampleNumber">质控任务Id</param>
        /// <param name="Tstamp">时间戳</param>
        /// <returns></returns>
        public List<RealSampleEntity> GetData(string SampleNumber)
        {
            return r_RealSample.RetrieveData(SampleNumber).ToList<RealSampleEntity>();
        }

        /// <summary>
        /// 根据报id获取质控任务记录
        /// </summary>
        /// <param name="id">工作ID</param>
        /// <returns></returns>
        public RealSampleEntity RetrieveEntityByUid(int id)
        {
            return r_RealSample.Retrieve(p => p.Id == id).FirstOrDefault();
        }

    }
}
