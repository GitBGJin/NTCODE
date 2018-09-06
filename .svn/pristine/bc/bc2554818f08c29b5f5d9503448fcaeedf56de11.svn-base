using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;

namespace SmartEP.MonitoringBusinessRepository.Air
{
    public class DustHazeDayRepository
    {
        DustHazeDayDAL dustDAL = new DustHazeDayDAL();

        /// <summary>
        /// 获取灰霾天数统计
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataView GetDustHazeDayStatistical(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return dustDAL.GetDustHazeDayStatistical(portIds, dateStart, dateEnd);
        }
    }
}
