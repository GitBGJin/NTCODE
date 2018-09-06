using log4net;
using SmartEP.Data.SqlServer.AutoMonitoring;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Air
{
    public class OrigionAQIService
    {
        #region <<变量>>
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器

        OrigionAQIDAL d_OrigionAQIDAL = new OrigionAQIDAL();

        #endregion

        #region <<方法>>
        /// <summary>
        /// 获取南通市辖区区域小时AQI信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetOriRegionHourAQI(string RegionUid)
        {
            try
            {
                return d_OrigionAQIDAL.GetOriRegionHourAQI(RegionUid);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取南通市辖区区域日AQI信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetOriRegionLastDayAQI(string RegionUid)
        {
            try
            {
                return d_OrigionAQIDAL.GetOriRegionLastDayAQI(RegionUid);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        #endregion
    }
}
