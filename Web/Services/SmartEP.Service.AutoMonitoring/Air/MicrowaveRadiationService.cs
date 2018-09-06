using SmartEP.AMSRepository.Air;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Air
{
    public class MicrowaveRadiationService
    {
        /// <summary>
        /// 仓储层数据处理
        /// </summary>
        MicrowaveRadiationRepository m_weiboRepository = Singleton<MicrowaveRadiationRepository>.GetInstance();
        /// <summary>
        /// 站点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="fatror">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetWeiboDataPager(string[] portIds, string[] fatror, DateTime dtBegin, DateTime dtEnd)
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dt = m_weiboRepository.GetWeiboDataPager(portIds,fatror, dtBegin, dtEnd).ToTable();
            dt.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            dt.Columns.Add("factorName", typeof(string)).SetOrdinal(4);
            IQueryable<MonitoringPointEntity> entity = g_MonitoringPointAir.RetrieveListByPointIds(portIds);
            for (int j = 0; j < dt.Rows.Count; j++)//给DataType、portName字段赋值
            {
                int pointId = Convert.ToInt32(dt.Rows[j]["PointId"]);
                string factorId = dt.Rows[j]["PollutantCode"].ToString();
                //dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                dt.Rows[j]["portName"] = entity.Where(x => x.PointId == pointId).Select(t => t.MonitoringPointName).FirstOrDefault();
                if (factorId == "401")
                    dt.Rows[j]["factorName"] = "温度";
                if (factorId == "402")
                    dt.Rows[j]["factorName"] = "蒸汽密度";
                if (factorId == "404")
                    dt.Rows[j]["factorName"] = "相对湿度";
            }
            dt.Columns.Remove("Record");
            dt.Columns.Remove("PointId");
            dt.Columns.Remove("LV2Processor");
            dt.Columns.Remove("PollutantCode");
            dt.Columns.Remove("OrderByNum");
            dt.Columns.Remove("Description");
            dt.Columns.Remove("CreateUser");
            dt.Columns.Remove("CreateTime");
            return dt.DefaultView;
        }
    }
}
