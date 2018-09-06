using SmartEP.Core.Enums;
using SmartEP.Data.SqlServer.MonitoringBusiness.WaterLZ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.WaterLZ
{
    public class LZRepository
    {
        LZDAL dal = new LZDAL();
        public DataTable GetTopFiveAlgalDensity(DateTime STime, DateTime ETime,string[] pointIds)
        {
            return dal.GetTopFiveAlgalDensity(STime, ETime, pointIds);

        }
        public DataTable GetBlueAlgalSort(DateTime STime, DateTime ETime, string[] pointIds, string[] factors)
        {
            return dal.GetBlueAlgalSort(STime, ETime, pointIds, factors);
        }
    }
}
