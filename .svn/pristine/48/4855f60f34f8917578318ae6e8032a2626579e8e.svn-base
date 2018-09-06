using SmartEP.Core.Generic;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using SmartEP.Service.OperatingMaintenance.ServiceReference;
using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Core.Interfaces;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：PartChangeService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-9
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 备品备件更换表服务层类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class PartChangeService
    {
        /// <summary>
        /// 试剂标液更换表仓储层
        /// </summary>
        PartChangeRepository r_partChange = Singleton<PartChangeRepository>.GetInstance();

        /// <summary>
        /// 质控运维WebService接口
        /// </summary>
        //TempGetDataWebServiceSoapClient sr = Singleton<TempGetDataWebServiceSoapClient>.GetInstance();

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
        public int Add(PartChangeEntity[] partChange)
        {
            int num = 0;
            for (int i = 0; i < partChange.Length; i++)
            {
                num += r_partChange.Add(partChange[i]);
            }
            //成功返回1，失败返回0
            if (num == partChange.Length)
            {
                return partChange.Length == 0 ? 2 : 1;
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
        public bool Update(PartChangeEntity partChange)
        {
            return r_partChange.Update(partChange);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Delete(int id)
        {
            return r_partChange.Delete(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="startTime">巡检开始时间</param>
        /// <param name="endTime">巡检结束时间</param>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetList(string taskCode, string missionID = null, DateTime? dtimeStart = null, DateTime? dtimeEnd = null)
        {
            //string strWhere = "TaskCode='" + taskCode + "' and  PointId='" + pointId + "' and ChangeDate between'" + dtBegin + "' and '" + dtEnd + "'";
            //if (MissionID != "")
            //{
            //    strWhere += " and MissionID='" + MissionID + "'";
            //}
            string strWhere = " 1=1 ";
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
                strWhere += string.Format(" and ChangeDate <='{0}' ", dtimeStart);
            }
            if (dtimeEnd != null)
            {
                strWhere += string.Format(" and ChangeDate >='{0}' ", dtimeEnd);
            }
            if (strWhere == " 1=1 ")
            {
                strWhere += " and TaskCode='' ";
            }
            return r_partChange.GetList(strWhere);
        }
        /// <summary>
        /// 维修页面显示备件数据
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetPartList(string MissionID)
        {
            string strWhere = "MissionID='" + MissionID + "'";

            return r_partChange.GetList(strWhere);
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
        /// 通过站点ID和仪器ID获取仪器的备件
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <param name="InstrumentId">仪器ID</param>
        /// <returns></returns>
        public DataTable GetReagent(string pointId, string InstrumentId)
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationDeviceWebServiceUrl, "GetUsingFittingInstance", new object[] { InstrumentId, pointId });
            return objData as DataTable;
        }

        /// <summary>
        /// 更新备品备件
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public void GetInstrumentStatus(string instrumentGuid,string status)
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl, "TempGetDataWebService", "UpdateFittingInstanceStatus", new object[] { instrumentGuid,status });
        }

        /// <summary>
        /// 通过站点ID获取所有的备件数据
        /// </summary>
        /// <param name="pointId">站点Guid</param>
        /// <returns></returns>
        public DataTable GetFitting(string pointId)
        {
            object objData = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl, "TempGetDataWebService", "GetFittingInfoByInstence", new object[] { pointId, "" });
            return objData as DataTable;
        }
    }
}
