using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Core.Generic;
using System.Data;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.DomainModel.BaseData;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.Utilities.Web.WebServiceHelper;
using SmartEP.Service.OperatingMaintenance.ServiceReference;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：AbnormalService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-9
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 异常表服务层类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AbnormalService
    {
        /// <summary>
        /// 异常表仓储层
        /// </summary>
        AbnormalRepository r_Abnormal = Singleton<AbnormalRepository>.GetInstance();

        /// <summary>
        /// 异常配置表仓储层
        /// </summary>
        AbnormalConfigRepository r_AbnormalConfig = Singleton<AbnormalConfigRepository>.GetInstance();

        /// <summary>
        /// 质控运维WebService接口
        /// </summary>
        //TempGetDataWebServiceSoapClient sr = Singleton<TempGetDataWebServiceSoapClient>.GetInstance();

        /// <summary>
        /// 提供仪器通道信息服务
        /// </summary>
        InstrumentChannelService s_InstrumentChannel = Singleton<InstrumentChannelService>.GetInstance();

        /// <summary>
        /// 水的测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();

        /// <summary>
        /// 运维平台获取数据WebService路径
        /// </summary>
        private string m_OperationOMMPGetDataWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationOMMPGetDataWebServiceUrl"].ToString();

        /// <summary>
        /// 运维平台任务处理WebService路径
        /// </summary>
        private string m_OperationTaskWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationTaskWebServiceUrl"].ToString();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="abnormal">异常实体</param>
        /// <returns>成功返回1，失败返回0，实体数组空返回2</returns>
        public int Add(AbnormalEntity[] abnormal)
        {
            int num = 0;
            for (int i = 0; i < abnormal.Length; i++)
            {
                num += r_Abnormal.Add(abnormal[i]);
            }
            //成功返回1，失败返回0
            if (num == abnormal.Length)
            {
                return abnormal.Length == 0 ? 2 : 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="abnormal">异常实体</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Update(AbnormalEntity abnormal)
        {
            return r_Abnormal.Update(abnormal);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Delete(int id)
        {
            return r_Abnormal.Delete(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetList(string MissionID, int pointId, DateTime dtBegin, DateTime dtend)
        {
            string strWhere = "PointId='" + pointId + "' and DateTime>='" + dtBegin + "' and DateTime<='" + dtend + "'";
            if (MissionID != "")
            {
                strWhere += " and MissionID='" + MissionID + "'";
            }
            DataTable dt = r_Abnormal.GetList(strWhere);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["PointName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
            }
            return dt;
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetListDuty(string[] pointId, DateTime dtBegin, DateTime dtend)
        {
            string where = string.Empty;
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(pointId.ToList<string>(), ",");
            if (pointId.Length == 1 && !string.IsNullOrEmpty(pointId[0]))
            {
                where = " PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                where = " PointId IN(" + portIdsStr + ")";
            }
            where += string.Format(" AND DateTime>='{0}' AND DateTime<='{1}' ", dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtend.ToString("yyyy-MM-dd HH:mm:ss"))+" order by PointId";

            DataTable dt = r_Abnormal.GetListDuty(where);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["PointName"] = g_MonitoringPointWater.RetrieveEntityByPointId(Convert.ToInt32(dt.Rows[i]["PointId"])).MonitoringPointName;
            }
            return dt;
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetList(string taskCode, int pointId, string missionID = null, DateTime? dtimeStart = null, DateTime? dtimeEnd = null)
        {
            //string strWhere = "TaskCode='" + taskCode + "' and PointId='" + pointId + "' and DateTime>='" + dtBegin + "' and DateTime<='" + dtend + "'";
            //if (MissionID != "")
            //{
            //    strWhere += " and MissionID='" + MissionID + "'";
            //}
            string strWhere = " 1=1 and PointId='" + pointId + "'";
            if (!string.IsNullOrWhiteSpace(taskCode))
            {
                strWhere += string.Format(" and TaskCode ='{0}' ", taskCode);
            }
            if (!string.IsNullOrWhiteSpace(missionID))
            {
                strWhere += string.Format(" and MissionID ='{0}' ", missionID);
            }
            if (dtimeStart != null)
            {
                strWhere += string.Format(" and DateTime <='{0}' ", dtimeStart);
            }
            if (dtimeEnd != null)
            {
                strWhere += string.Format(" and DateTime >='{0}' ", dtimeEnd);
            }
            if (strWhere == " 1=1 and PointId='" + pointId + "'")
            {
                strWhere += " and TaskCode='' ";
            }
            DataTable dt = r_Abnormal.GetList(strWhere);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["PointName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
            }
            return dt;
        }

        public DataView GetDataPager(string[] portIds, DateTime dtmStart, DateTime dtmEnd)
        {
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                portIdsStr = "PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                portIdsStr = "PointId IN(" + portIdsStr + ")";
            }
            string strWhere = portIdsStr + " and DateTime>='" + dtmStart + "' and DateTime<='" + dtmEnd + "'";
            DataTable dt = r_Abnormal.GetList(strWhere);
            //dt.Columns.Add("PointName", typeof(string));
            //for (int j = 0; j < portIds.Length; j++)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        int pointId = Convert.ToInt32(portIds[j]);
            //        dt.Rows[i]["PointName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
            //    }
            //}
            DataView dv = new DataView(dt);
            return dv;
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetListId(int Id)
        {
            string strWhere = "id='" + Id + "'";
            DataTable dt = r_Abnormal.GetList(strWhere);
            return dt;
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetLists(string Id, string name, DateTime time)
        {
            string strWhere = "PointId=" + Id + " and AbnormalItemType='" + name + "' and DateTime='" + time + "'";
            DataTable dt = r_Abnormal.GetListDuty(strWhere);
            return dt;
        }
        /// <summary>
        /// 通过站点ID获取站点内的所有仪器
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataTable GetInstrument(string pointId)
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl, "TempGetDataWebService", "GetInsTypeByObjectID", new object[] { pointId });
            return objData as DataTable;
        }
        /// <summary>
        /// 通过站点ID获取站点内的所有仪器
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataTable GetInstrumentNew(string[] pointId)
        {
            DataTable objData = r_AbnormalConfig.GetInstrumentNew(pointId);
            return objData;
        }
        /// <summary>
        /// 通过站点ID获取站点内的所有因子
        /// </summary>
        /// <param name="pointId"></param>
        /// <returns></returns>
        public List<PollutantCodeEntity> GetFactor(string pointId)
        {
            List<PollutantCodeEntity> l_PollutantCodeEntity = s_InstrumentChannel.RetrieveChannelListByPointUid(pointId).ToList<PollutantCodeEntity>();
            return l_PollutantCodeEntity;
        }

        /// <summary>
        /// 根据监测项类型获取异常列表
        /// </summary>
        /// <param name="AbnormalItemType">监测项类型</param>
        /// <returns></returns>
        public DataTable GetAbnormalName(string AbnormalItemType)
        {
            string strWhere = "AbnormalItemType='" + AbnormalItemType + "'";
            return r_AbnormalConfig.GetList(strWhere);
        }

    }
}
