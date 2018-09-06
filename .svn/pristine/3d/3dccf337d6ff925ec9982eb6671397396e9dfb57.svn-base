using SmartEP.Core.Generic;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.OperatingMaintenance.ServiceReference;
using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：StandardSolutionChangeService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-9
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 试剂标液更换表服务层类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class StandardSolutionChangeService
    {
        /// <summary>
        /// 试剂标液更换表仓储层
        /// </summary>
        StandardSolutionChangeRepository r_StandardSolutionChange = Singleton<StandardSolutionChangeRepository>.GetInstance();

        /// <summary>
        /// 质控运维WebService接口
        /// </summary>
        //TempGetDataWebServiceSoapClient sr = Singleton<TempGetDataWebServiceSoapClient>.GetInstance();

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
        /// 运维平台任务处理WebService路径
        /// </summary>
        private string m_OperationDeviceWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationDeviceWebServiceUrl"].ToString();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="standardSolutionChange">试剂标液更换实体</param>
        /// <returns>成功返回1，失败返回0，实体数组空返回2</returns>
        public int Add(StandardSolutionChangeEntity[] standardSolutionChange)
        {
            int num = 0;
            for (int i = 0; i < standardSolutionChange.Length; i++)
            {
                num += r_StandardSolutionChange.Add(standardSolutionChange[i]);
            }
            //成功返回1，失败返回0
            if (num == standardSolutionChange.Length)
            {
                return standardSolutionChange.Length == 0 ? 2 : 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="standardSolutionChange">试剂标液更换实体</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Update(StandardSolutionChangeEntity standardSolutionChange)
        {
            return r_StandardSolutionChange.Update(standardSolutionChange);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Delete(int id)
        {
            return r_StandardSolutionChange.Delete(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetList(string taskCode, int pointId, string missionID = null, DateTime? dtimeStart = null, DateTime? dtimeEnd = null)
        {
            //string strWhere = "TaskCode='" + taskCode + "' and PointId='" + pointId + "' and LastChangeDate>='" + dtBegin + "' and LastChangeDate<='" + dtend + "'";
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
                strWhere += string.Format(" and LastChangeDate <='{0}' ", dtimeStart);
            }
            if (dtimeEnd != null)
            {
                strWhere += string.Format(" and LastChangeDate >='{0}' ", dtimeEnd);
            }
            if (strWhere == " 1=1 and PointId='" + pointId + "'")
            {
                strWhere += " and TaskCode='' ";
            }
            DataTable dt = r_StandardSolutionChange.GetList(strWhere);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["PointName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
            }
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
        /// 通过站点ID和仪器ID获取仪器的标液
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <param name="InstrumentId">仪器ID</param>
        /// <returns></returns>
        public DataTable GetReagent(string InstrumentId, string pointId)
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationDeviceWebServiceUrl, "GetUsingReagent", new object[] { InstrumentId, pointId });
            return objData as DataTable;
        }
        /// <summary>
        /// 获取所有的标液数据
        /// </summary>
        /// <param name="pointId">站点Guid</param>
        /// <returns></returns>
        public DataTable GetReagent(string rowGuid)
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl, "TempGetDataWebService", "GetReagentByRowGuid", new object[] { rowGuid });
            return objData as DataTable;
        }
    }
}
