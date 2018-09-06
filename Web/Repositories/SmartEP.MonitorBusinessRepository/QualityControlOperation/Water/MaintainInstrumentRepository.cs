using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：MaintainInstrumentRepository.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-11-13
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：维修表仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MaintainInstrumentRepository : BaseGenericRepository<MonitoringBusinessModel, MaintainInstrumentEntity>
    {
        public override bool IsExist(string strKey)
        {
            return true;
        }
    }
}
