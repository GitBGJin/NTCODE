using SmartEP.DomainModel;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：StandardSolutionConfigService.cs
    /// 创建人：吕云
    /// 创建日期：2016-08-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：仪器备件配对服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>

    public class InstrumentFittingInstanceService
    {
        InstrumentFittingInstanceRepository relationRepository = new InstrumentFittingInstanceRepository();

        #region 增删改
        /// <summary>
        /// 增加仪器配件
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void Add(OMMP_InstrumentFittingInstanceEntity InstrumentFittingInstance)
        {
            relationRepository.Add(InstrumentFittingInstance);
        }

        /// <summary>
        /// 更新仪器配件
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void Update(OMMP_InstrumentFittingInstanceEntity InstrumentFittingInstance)
        {
            relationRepository.Update(InstrumentFittingInstance);
        }

        /// <summary>
        /// 删除仪器配件
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void Delete(OMMP_InstrumentFittingInstanceEntity InstrumentFittingInstance)
        {
            relationRepository.Delete(InstrumentFittingInstance);
        }

        /// <summary>
        /// 批量删除仪器配件
        /// </summary>
        /// <param name="Acquisition">实体</param>
        public void BatchDelete(List<OMMP_InstrumentFittingInstanceEntity> InstrumentFittingInstance)
        {
            relationRepository.BatchDelete(InstrumentFittingInstance);
        }
        #endregion

        /// <summary>
        /// 根据Uid数组获取仪器备件配对信息
        /// </summary>
        /// <param name="InstrumentFittingUids"></param>
        /// <returns></returns>
        public IQueryable<OMMP_InstrumentFittingInstanceEntity> RetrieveListByUids(string[] InstrumentFittingUids)
        {

            return relationRepository.Retrieve(p => InstrumentFittingUids.Contains(p.RowGuid));
        }

    }
}
