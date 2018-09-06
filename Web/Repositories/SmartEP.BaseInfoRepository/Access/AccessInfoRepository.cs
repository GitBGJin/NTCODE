using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.BaseData;

namespace SmartEP.BaseInfoRepository.Access
{
    public class AccessInfoRepository
    {
        AccessInfoDAL g_AccessInfoDAL = new AccessInfoDAL();
        public int InserAccessInfo(string pointId, string cardNumber, string registerName, string stationWay, DateTime stationDate)
        {
            int num=g_AccessInfoDAL.InserAccessInfo(pointId, cardNumber, registerName, stationWay, stationDate);
            return num;
        }
    }
}
