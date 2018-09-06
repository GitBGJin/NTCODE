using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.AutoMonitoring;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.AMSRepository.Air
{
    public class MicrowaveRadiationRepository
    {
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        MicrowaveRadiationDAL m_microwaveRadiation = Singleton<MicrowaveRadiationDAL>.GetInstance();
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="fatror">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetWeiboDataPager(string[] portIds,string[] fatror, DateTime dtBegin, DateTime dtEnd)
        {
            return m_microwaveRadiation.GetWeiboDataPager(portIds,fatror, dtBegin, dtEnd);
        }
    }
}
