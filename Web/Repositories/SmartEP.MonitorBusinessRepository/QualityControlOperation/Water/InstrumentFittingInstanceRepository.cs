using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
    public class InstrumentFittingInstanceRepository : BaseGenericRepository<FrameworkModel, OMMP_InstrumentFittingInstanceEntity>
    {
        public override bool IsExist(string strKey)
        {
            return true;
        }
    }
}
