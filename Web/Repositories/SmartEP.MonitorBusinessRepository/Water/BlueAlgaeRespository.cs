using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.MonitoringBusiness.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Water
{
    /// <summary>
    /// 蓝藻数据
    /// </summary>
    public class BlueAlgaeRespository
    {
        BlueAlgaeDAL d_BlueAlgaeDAL = Singleton<BlueAlgaeDAL>.GetInstance();
        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="PointType">点位类型</param>
        /// <returns></returns>
        public string GetPortInfo(string userguid)
        {
            return d_BlueAlgaeDAL.GetPortInfo(userguid);
        }
        /// <summary>
        /// 获取日均值变化浓度
        /// </summary>
        /// <param name="PointId"></param>
        /// <param name="dtstart"></param>
        /// <param name="dtend"></param>
        /// <returns></returns>
        public DataTable GetDayAvg(string PointId, string dtstart, string dtend)
        {
            return d_BlueAlgaeDAL.GetDayAvg(PointId, dtstart, dtend);
        }
        /// <summary>
        /// 获取藻密度小时图
        /// </summary>
        /// <param name="userguid"></param>
        /// <returns></returns>
        public DataTable GetAlgaeImg(string userguid)
        {
            return d_BlueAlgaeDAL.GetAlgaeImg(userguid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="factorcode"></param>
        /// <returns></returns>
        public DataTable GetWQLDayReport(string factorcode, string pointids)
        {
            return d_BlueAlgaeDAL.GetWQLDayReport(factorcode, pointids);
        }
    }
}
