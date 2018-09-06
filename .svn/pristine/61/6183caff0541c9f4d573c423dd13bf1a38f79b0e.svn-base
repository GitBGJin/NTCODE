using SmartEP.Data.SqlServer.QualityControlOperation.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Dictionary
{
    public class ReagentConfigRecordRepository
    {
        ReagentConfigRecordDAL d = new ReagentConfigRecordDAL();

        public DataTable GetData(DateTime staDate, DateTime endDate, string points, string factors)
        {
            return d.GetData(staDate, endDate, points, factors);
        }
    }
}
