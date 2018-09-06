using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.MPInfo
{
    public class MonitoringInstrumentRepository : BaseGenericRepository<BaseDataModel, MonitoringInstrumentEntity>
    {
        PortInfoDAL d_PortInfoDAL = new PortInfoDAL();
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.MonitoringinstrumentUid.Equals(strKey)).Count() == 0 ? false : true;
        }
        public DataTable GetPoint_Category_Instrument(string PUid)
        {
            return d_PortInfoDAL.GetPoint_Category_Instrument(PUid);
        }
    }
}
