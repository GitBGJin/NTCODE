using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.MonitoringBusinessRepository.Air;

namespace SmartEP.Service.DataAnalyze.Air.DataQuery
{
    public class DustHazeDayService
    {
        DustHazeDayRepository dustRep = new DustHazeDayRepository();
        /// <summary>
        /// 获取灰霾天数统计
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataView GetDustHazeDayStatistical(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return dustRep.GetDustHazeDayStatistical(portIds, dateStart, dateEnd); 
        }
    }
}
