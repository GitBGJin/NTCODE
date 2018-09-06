using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Air;
using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Air
{
    /// <summary>
    /// 名称：MaintenanceService.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-05-01
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 运维数据查询
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MaintenanceService
    {
        /// <summary>
        /// 运维平台获取数据WebService路径
        /// </summary>
        private string m_OperationOMMPGetDataWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationOMMPGetDataWebServiceUrl"].ToString();
        MaintenanceRepository r_MaintenanceRepository = Singleton<MaintenanceRepository>.GetInstance();
        OM_TaskItemDataService s_OM_TaskItemDataService = Singleton<OM_TaskItemDataService>.GetInstance();
        #region << 数据查询方法 >>
        /// <summary>
        /// 根据站点guid查询仪器
        /// </summary>
        /// <param name="pointGuids">站点guid</param>
        /// <returns></returns>
        public DataView GetInstanceByGuid(string[] pointGuids, string objectType, string IsSpareParts = "")
        {
            return r_MaintenanceRepository.GetInstanceByGuid(pointGuids, objectType, IsSpareParts);
        }
        /// <summary>
        /// 查询可用备件信息
        /// </summary>
        /// <param name="FittingName">配件名</param>
        /// <returns></returns>
        public DataView GetFittingInstance(string FittingName, string ObjectType)
        {
            return r_MaintenanceRepository.GetFittingInstance(FittingName, ObjectType);
        }
        /// <summary>
        /// 查询仪器的配件具体信息
        /// </summary>
        /// <param name="InstrumentInstanceGuid">具体仪器实例GUID</param>
        /// <returns></returns>
        public DataView GetInstrumentFittingInstance(string InstrumentInstanceGuid, string FittingName)
        {
            return r_MaintenanceRepository.GetInstrumentFittingInstance(InstrumentInstanceGuid, FittingName);

        }

        /// <summary>
        /// 查询仪器的配件
        /// </summary>
        /// <param name="rowguid">仪器GUID</param>
        /// <returns></returns>
        public DataView GetFitting(string rowguid)
        {
            return r_MaintenanceRepository.GetFitting(rowguid);
        }
        /// <summary>
        /// 查询仪器清单
        /// </summary>
        /// <param name="FixedAssetNumbers">仪器编号</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetInstanceList(string[] FixedAssetNumbers, DateTime dtStart, DateTime dtEnd)
        {
            return r_MaintenanceRepository.GetInstanceList(FixedAssetNumbers, dtStart, dtEnd);
        }
        /// <summary>
        /// 获取仪器所在测点
        /// </summary>
        /// <param name="InstrumentInstanceGuid">仪器实例Guid</param>
        /// <returns></returns>
        public string GetInstanceSite(string InstrumentInstanceGuid)
        {
            return r_MaintenanceRepository.GetInstanceSite(InstrumentInstanceGuid);
        }
        /// <summary>
        /// 向出入库中添加仪器/备件的出入库信息
        /// </summary>
        /// <param name="status">出入库状态</param>
        /// <param name="note">提示内容</param>
        /// <param name="InstanceGuid">仪器/配件实例Guid</param>
        /// <returns></returns>
        public int InsertInstrumentInstanceSite(int status, string note, string InstanceGuid)
        {
            return r_MaintenanceRepository.InsertInstrumentInstanceSite(status, note, InstanceGuid);
        }
        /// <summary>
        /// 查看仪器是否已选择当前配件类型
        /// </summary>
        /// <param name="InstrumentInstanceGuid">仪器实例Guid</param>
        ///<param name="FittingGuid">配件类型Guid</param>
        /// <returns></returns>
        public string GetIsFitting(string InstrumentInstanceGuid, string FittingGuid)
        {
            return r_MaintenanceRepository.GetIsFitting(InstrumentInstanceGuid, FittingGuid);
        }
        /// <summary>
        /// 查看仪器已选择当前配件类型的配对RowGuid
        /// </summary>
        /// <param name="InstrumentInstanceGuid">仪器实例Guid</param>
        ///<param name="FittingGuid">配件类型Guid</param>
        /// <returns></returns>
        public string GetIsFittingKey(string InstrumentInstanceGuid, string FittingGuid)
        {
            return r_MaintenanceRepository.GetIsFittingKey(InstrumentInstanceGuid, FittingGuid);
        }

        /// <summary>
        /// 查询仪器出入记录
        /// </summary>
        /// <param name="FixedAssetNumbers">仪器编号</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetInstanceSite(string[] FixedAssetNumbers, DateTime dtStart, DateTime dtEnd)
        {
            return r_MaintenanceRepository.GetInstanceSite(FixedAssetNumbers, dtStart, dtEnd);
        }
        /// <summary>
        /// 查询仪器出入记录
        /// </summary>
        /// <param name="pointGuids">站点guid</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetInstanceSiteByPoint(string[] pointGuids, DateTime dtStart, DateTime dtEnd)
        {
            return r_MaintenanceRepository.GetInstanceSiteByPoint(pointGuids, dtStart, dtEnd);
        }
        /// <summary>
        /// 仪器维护记录
        /// </summary>
        /// <param name="portIds">站点id</param>
        /// <param name="pointGuids">站点guid</param>
        /// <param name="taskType">任务类型MissionId</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="taskItem">任务项</param>
        /// <param name="FixedAssetNumbers">仪器编号</param>
        /// <returns></returns>
        public DataTable GetInstancemaintainByPoint(string[] portIds, string[] pointGuids, string taskType, DateTime dtStart, DateTime dtEnd,
            string[] taskItem, string[] FixedAssetNumbers)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("pointName", typeof(string));
            dt.Columns.Add("InstanceName", typeof(string));
            dt.Columns.Add("InstrumentInstanceGuid", typeof(string));
            dt.Columns.Add("SpecificationModel", typeof(string));
            dt.Columns.Add("FixedAssetNumber", typeof(string));
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("AddFlow", typeof(string));
            dt.Columns.Add("Pressure", typeof(string));
            dt.Columns.Add("LampEnergy", typeof(string));
            dt.Columns.Add("SamplingFilm", typeof(string));
            dt.Columns.Add("EffectiveLoad", typeof(string));
            dt.Columns.Add("Membrane", typeof(string));
            dt.Columns.Add("AlarmInfo", typeof(string));
            dt.Columns.Add("ActionUser", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            if (portIds != null && portIds.Length > 0)
            {
                DataView dvInstanceInfo = GetInstanceByGuid(pointGuids, "2");//1水2气
                string strWhere = " 1=1 ";
                if (portIds != null)
                {
                    string strPointids = string.Join(",", portIds);
                    strWhere += string.Format(" and PointId in ({0}) ", strPointids);
                }

                if (!string.IsNullOrWhiteSpace(taskType))
                {
                    strWhere += string.Format(" and MissionID ='{0}' ", taskType);
                }
                if (dtStart != null)
                {
                    strWhere += string.Format(" and ActionDate >='{0}' ", dtStart);
                }
                if (dtEnd != null)
                {
                    strWhere += string.Format(" and FinishDate <='{0}' ", dtEnd);
                }
                DataTable dtTaskInfo = r_MaintenanceRepository.GetList(strWhere);
                string taskItemGuid = "";
                if (taskItem.Length > 0)
                {
                    taskItemGuid = "'" + string.Join("','", taskItem) + "'";
                }
                DataView dtItemInfo = s_OM_TaskItemDataService.GetListData(taskItemGuid, dtStart, dtEnd).DefaultView;
                foreach (DataRow dr in dtTaskInfo.Rows)
                {
                    foreach (string FixedAssetNumber in FixedAssetNumbers)
                    {
                        dtItemInfo.RowFilter = "TaskCode='" + dr["TaskCode"].ToString() + "' and UniversalValue3='" + FixedAssetNumber + "'";
                        if (dtItemInfo.Count > 0)
                        {
                            DataRow drNew = dt.NewRow();
                            drNew["pointName"] = dr["PointName"].ToString();
                            drNew["InstanceName"] = dtItemInfo[0]["UniversalValue3"].ToString();
                            dvInstanceInfo.RowFilter = "FixedAssetNumber='" + FixedAssetNumber + "'";
                            if (dvInstanceInfo.Count > 0)
                            {
                                drNew["InstrumentInstanceGuid"] = dvInstanceInfo[0]["InstrumentInstanceGuid"];
                                drNew["SpecificationModel"] = dvInstanceInfo[0]["SpecificationModel"];
                            }
                            drNew["FixedAssetNumber"] = FixedAssetNumber;
                            drNew["Tstamp"] = dtItemInfo[0]["MaintainceDate"].ToString();
                            drNew["ActionUser"] = dr["ActionUserName"].ToString();
                            string AddFlow = "";
                            string Pressure = "";
                            string LampEnergy = "";
                            string SamplingFilm = "";
                            string EffectiveLoad = "";
                            string Membrane = "";
                            string AlarmInfo = "";
                            drNew["AddFlow"] = dtItemInfo[0]["Remark"].ToString();
                            drNew["Pressure"] = dtItemInfo[0]["Remark"].ToString();

                            foreach (DataRow dRow in dtItemInfo)
                            {

                                if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("采样流量"))
                                {
                                    if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("主"))
                                    {
                                        AddFlow += "主：" + dRow["Remark"].ToString();
                                    }
                                    else if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("辅"))
                                    {
                                        AddFlow += "辅：" + dRow["Remark"].ToString();
                                    }
                                    else
                                    {
                                        AddFlow += dRow["Remark"].ToString();
                                    }
                                }
                                else if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("压力"))
                                {
                                    if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("池压"))
                                    {
                                        Pressure += "池压：" + dRow["Remark"].ToString();
                                    }
                                    else if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("样气"))
                                    {
                                        Pressure += "样气：" + dRow["Remark"].ToString();
                                    }
                                    else
                                    {
                                        Pressure += dRow["Remark"].ToString();
                                    }
                                }
                                else if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("紫外灯能量"))
                                {
                                    LampEnergy += dRow["Remark"].ToString();
                                }
                                else if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("更换采样膜"))
                                {
                                    SamplingFilm += dRow["Remark"].ToString();
                                }
                                else if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("膜有效负荷"))
                                {
                                    EffectiveLoad += dRow["Remark"].ToString();
                                }
                                else if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("更换滤膜"))
                                {
                                    if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("主"))
                                    {
                                        Membrane += "主：" + dRow["Remark"].ToString() + "    ";
                                    }
                                    else if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("辅"))
                                    {
                                        Membrane += "辅：" + dRow["Remark"].ToString();
                                    }
                                    else
                                    {
                                        Membrane += dRow["Remark"].ToString();
                                    }
                                }
                                else if (dvInstanceInfo[0]["UniversalValue4"].ToString().Contains("报警信息"))
                                {
                                    AlarmInfo += dRow["Remark"].ToString();
                                }
                            }
                            drNew["AddFlow"] = AddFlow;
                            drNew["Pressure"] = Pressure;
                            drNew["LampEnergy"] = LampEnergy;
                            drNew["SamplingFilm"] = SamplingFilm;
                            drNew["EffectiveLoad"] = EffectiveLoad;
                            drNew["Membrane"] = Membrane;
                            drNew["AlarmInfo"] = AlarmInfo;
                            dt.Rows.Add(drNew);
                        }
                    }
                }
            }
            return dt;
        }
        /// <summary>
        /// 查询仪器维修记录
        /// </summary>
        /// <param name="pointGuids">站点guid</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataTable GetInstanceRepairByPoint(string[] portIds, string[] pointGuids, string taskType, string[] FixedAssetNumbers, string[] taskItem, DateTime dtStart, DateTime dtEnd, Dictionary<string, string> itemConfig, bool istrue)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("pointName", typeof(string));
                dt.Columns.Add("InstanceName", typeof(string));
                dt.Columns.Add("InstrumentInstanceGuid", typeof(string));
                dt.Columns.Add("SpecificationModel", typeof(string));
                dt.Columns.Add("FixedAssetNumber", typeof(string));
                dt.Columns.Add("faultType", typeof(string));
                dt.Columns.Add("faultPerson", typeof(string));
                dt.Columns.Add("faultTime", typeof(string));
                dt.Columns.Add("reasonAndProcess", typeof(string));
                dt.Columns.Add("repairPerson", typeof(string));
                dt.Columns.Add("repairTime", typeof(string));
                dt.Columns.Add("fittingName", typeof(string));
                dt.Columns.Add("fittingPerson", typeof(string));
                dt.Columns.Add("fittingTime", typeof(string));
                if (portIds != null && portIds.Length > 0)
                {
                    DataView dvInstanceInfo = GetInstanceByGuid(pointGuids, "2");//1水2气
                    string strWhere = " 1=1 ";
                    if (portIds != null)
                    {
                        string strPointids = string.Join(",", portIds);
                        strWhere += string.Format(" and PointId in ({0}) ", strPointids);
                    }

                    if (!string.IsNullOrWhiteSpace(taskType))
                    {
                        strWhere += string.Format(" and MissionID ='{0}' ", taskType);
                    }
                    if (dtStart != null)
                    {
                        strWhere += string.Format(" and ActionDate >='{0}' ", dtStart);
                    }
                    if (dtEnd != null)
                    {
                        strWhere += string.Format(" and FinishDate <='{0}' ", dtEnd);
                    }
                    DataTable dtTaskInfo = r_MaintenanceRepository.GetList(strWhere);
                    string taskItemGuid = "";
                    if (taskItem.Length > 0)
                    {
                        taskItemGuid = "'" + string.Join("','", taskItem) + "'";
                    }
                    string strFixedAssetNumbers = "";
                    if (FixedAssetNumbers.Length > 0)
                    {
                        strFixedAssetNumbers = "'" + string.Join("','", FixedAssetNumbers) + "'";
                    }
                    DataView dtItemInfo = s_OM_TaskItemDataService.GetListDatabyDevice(taskItemGuid, dtStart, dtEnd, strFixedAssetNumbers, new string[] { }, !istrue).DefaultView;
                    foreach (DataRow dr in dtTaskInfo.Rows)
                    {
                        dtItemInfo.RowFilter = "TaskCode='" + dr["TaskCode"].ToString() + "'";
                        DataRow drNew = dt.NewRow();
                        drNew["pointName"] = dr["PointName"].ToString();
                        if (dtItemInfo.Count > 0)
                        {
                            //drNew["InstrumentInstanceGuid"]=
                            drNew["InstanceName"] = dtItemInfo[0]["UniversalValue2"];
                            dvInstanceInfo.RowFilter = "FixedAssetNumber='" + dtItemInfo[0]["UniversalValue3"].ToString() + "'";
                            if (dvInstanceInfo.Count > 0)
                            {
                                drNew["InstrumentInstanceGuid"] = dvInstanceInfo[0]["InstrumentInstanceGuid"];
                                drNew["SpecificationModel"] = dvInstanceInfo[0]["SpecificationModel"];
                            }
                            drNew["FixedAssetNumber"] = dtItemInfo[0]["UniversalValue3"];
                            for (int i = 0; i < dtItemInfo.Count; i++)
                            {
                                string Name = itemConfig[dtItemInfo[i]["TaskItemGuid"].ToString()];
                                if (Name.Contains("故障现象"))
                                {
                                    drNew["faultType"] = dtItemInfo[i]["ItemValue"].ToString();
                                    drNew["faultPerson"] = dtItemInfo[i]["MaintainceUser"].ToString();
                                    drNew["faultTime"] = dtItemInfo[i]["ItemRecordDate"].ToString();
                                }
                                else if (Name.Contains("故障原因和检修过程"))
                                {
                                    drNew["reasonAndProcess"] = dtItemInfo[i]["ItemValue"].ToString();
                                    drNew["repairPerson"] = dtItemInfo[i]["MaintainceUser"].ToString();
                                    drNew["repairTime"] = dtItemInfo[i]["ItemRecordDate"].ToString();
                                }
                                else if (Name.Contains("更换零件名称"))
                                {
                                    drNew["fittingName"] = dtItemInfo[i]["ItemValue"].ToString();
                                    drNew["fittingPerson"] = dtItemInfo[i]["MaintainceUser"].ToString();
                                    drNew["fittingTime"] = dtItemInfo[i]["ItemRecordDate"].ToString();
                                }
                            }
                        }
                        dt.Rows.Add(drNew);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        /// 获取仪器检修任务信息
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataTable GetListDevice(string[] portIds, string[] pointGuids, string[] FixedAssetNumbers, DateTime dtBegin, DateTime dtEnd, string taskType, string[] taskItem, string userName, bool istrue, string[] rcbTypes)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("pointName", typeof(string));
            dt.Columns.Add("InstanceName", typeof(string));
            dt.Columns.Add("SpecificationModel", typeof(string));
            dt.Columns.Add("FixedAssetNumber", typeof(string));
            dt.Columns.Add("faultType", typeof(string));
            dt.Columns.Add("itemCount", typeof(string));
            dt.Columns.Add("dealDays", typeof(string));
            dt.Columns.Add("ActionUserName", typeof(string));
            dt.Columns.Add("startTime", typeof(string));
            dt.Columns.Add("endTime", typeof(string));
            dt.Columns.Add("detail", typeof(string));
            if (portIds != null && portIds.Length > 0)
            {
                string strWhere = " 1=1 ";
                if (portIds != null)
                {
                    string strPointids = string.Join(",", portIds);
                    strWhere += string.Format(" and PointId in ({0}) ", strPointids);
                }

                if (!string.IsNullOrWhiteSpace(taskType))
                {
                    strWhere += string.Format(" and MissionID ='{0}' ", taskType);
                }
                if (dtBegin != null)
                {
                    strWhere += string.Format(" and ActionDate >='{0}' ", dtBegin);
                }
                if (dtEnd != null)
                {
                    strWhere += string.Format(" and FinishDate <='{0}' ", dtEnd);
                }
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    strWhere += string.Format(" and ActionUserName like '%{0}%' ", userName);
                }
                DataTable dtTaskInfo = r_MaintenanceRepository.GetList(strWhere);
                string taskItemGuid = "";
                if (taskItem.Length > 0)
                {
                    taskItemGuid = "'" + string.Join("','", taskItem) + "'";
                }
                string strFixedAssetNumbers = "";
                if (FixedAssetNumbers.Length > 0)
                {
                    strFixedAssetNumbers = "'" + string.Join("','", FixedAssetNumbers) + "'";
                }
                DataView dtItemInfo = s_OM_TaskItemDataService.GetListDatabyDevice(taskItemGuid, dtBegin, dtEnd, strFixedAssetNumbers, rcbTypes, true).DefaultView;
                DataView dvInstanceInfo = GetInstanceByGuid(pointGuids, "2");//1水2气
                if (!istrue)
                {
                    foreach (DataRow dr in dtTaskInfo.Rows)
                    {
                        dtItemInfo.RowFilter = "TaskCode='" + dr["TaskCode"].ToString() + "'";
                        if (dtItemInfo.Count > 0)
                        {
                            for (int i = 0; i < dtItemInfo.Count; i++)
                            {
                                DataRow drNew = dt.NewRow();
                                drNew["pointName"] = dr["PointName"].ToString();
                                drNew["InstanceName"] = dtItemInfo[i]["UniversalValue2"].ToString();
                                dvInstanceInfo.RowFilter = "FixedAssetNumber='" + dtItemInfo[i]["UniversalValue3"].ToString() + "'";
                                if (dvInstanceInfo.Count > 0)
                                {
                                    drNew["SpecificationModel"] = dvInstanceInfo[0]["SpecificationModel"];
                                }
                                drNew["FixedAssetNumber"] = dtItemInfo[i]["UniversalValue3"].ToString();
                                //drNew["itemCount"] = "";
                                drNew["faultType"] = dtItemInfo[i]["ItemValue"].ToString();
                                if (dr["FinishDate"] != DBNull.Value && dr["ActionDate"] != DBNull.Value)
                                {
                                    TimeSpan ts = DateTime.Parse(dr["FinishDate"].ToString()) - DateTime.Parse(dr["ActionDate"].ToString());
                                    drNew["dealDays"] = ts.Days + 1;
                                }
                                drNew["ActionUserName"] = dr["ActionUserName"].ToString();
                                drNew["startTime"] = dr["ActionDate"].ToString();
                                drNew["endTime"] = dr["FinishDate"].ToString();
                                drNew["detail"] = "";
                                dt.Rows.Add(drNew);

                            }
                        }
                    }
                }
                else
                {
                    foreach (string pointId in portIds)
                    {
                        DataRow[] drArry = dtTaskInfo.Select("PointId=" + pointId);
                        string strTaskCodes = "";
                        string pointName = "";
                        string MissionName = "";
                        DateTime ActionDate = dtBegin;
                        DateTime FinishDate = dtEnd;
                        for (int i = 0; i < drArry.Length; i++)
                        {
                            if (drArry[i]["MissionName"] != DBNull.Value)
                            {
                                MissionName = drArry[i]["MissionName"].ToString();
                            }
                            if (drArry[i]["PointName"] != DBNull.Value)
                            {
                                pointName = drArry[i]["PointName"].ToString();
                            }
                            if (drArry[i]["TaskCode"] != DBNull.Value)
                            {
                                strTaskCodes = "'" + drArry[i]["TaskCode"].ToString() + "',";
                            }
                            if (drArry[i]["ActionDate"] != DBNull.Value)
                            {
                                if (ActionDate > DateTime.Parse(drArry[i]["ActionDate"].ToString()))
                                {
                                    ActionDate = DateTime.Parse(drArry[i]["ActionDate"].ToString());
                                }
                            }
                            if (drArry[i]["FinishDate"] != DBNull.Value)
                            {
                                if (FinishDate < DateTime.Parse(drArry[i]["FinishDate"].ToString()))
                                {
                                    FinishDate = DateTime.Parse(drArry[i]["FinishDate"].ToString());
                                }
                            }
                        }
                        if (taskItem.Length > 0)
                        {
                            if (strTaskCodes != "")
                            {
                                strTaskCodes = strTaskCodes.TrimEnd(',');
                                if (rcbTypes != null && rcbTypes.Length > 0)
                                {
                                    foreach (string itemGuid in taskItem)
                                    {
                                        foreach (string FixedAssetNumber in FixedAssetNumbers)
                                        {
                                            foreach (string fault in rcbTypes)
                                            {
                                                dtItemInfo.RowFilter = "TaskCode in(" + strTaskCodes + ") and TaskItemGuid='" + itemGuid + "' and ItemValue='" + fault + "'  and UniversalValue3='" + FixedAssetNumber + "'";
                                                if (dtItemInfo.Count > 0)
                                                {
                                                    DataRow drNew = dt.NewRow();
                                                    drNew["pointName"] = pointName;
                                                    drNew["InstanceName"] = dtItemInfo[0]["UniversalValue2"].ToString(); ;
                                                    dvInstanceInfo.RowFilter = "FixedAssetNumber='" + FixedAssetNumber + "'";
                                                    if (dvInstanceInfo.Count > 0)
                                                    {
                                                        drNew["SpecificationModel"] = dvInstanceInfo[0]["SpecificationModel"];
                                                    }
                                                    drNew["FixedAssetNumber"] = FixedAssetNumber;
                                                    drNew["faultType"] = fault;
                                                    drNew["itemCount"] = dtItemInfo.Count;
                                                    dt.Rows.Add(drNew);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (string itemGuid in taskItem)
                                    {
                                        foreach (string FixedAssetNumber in FixedAssetNumbers)
                                        {
                                            dtItemInfo.RowFilter = "TaskCode in(" + strTaskCodes + ") and TaskItemGuid='" + itemGuid + "' and UniversalValue3='" + FixedAssetNumber + "'";
                                            if (dtItemInfo.Count > 0)
                                            {
                                                DataRow drNew = dt.NewRow();
                                                drNew["pointName"] = pointName;
                                                drNew["InstanceName"] = dtItemInfo[0]["UniversalValue2"].ToString(); ;
                                                dvInstanceInfo.RowFilter = "FixedAssetNumber='" + FixedAssetNumber + "'";
                                                if (dvInstanceInfo.Count > 0)
                                                {
                                                    drNew["SpecificationModel"] = dvInstanceInfo[0]["SpecificationModel"];
                                                }
                                                drNew["FixedAssetNumber"] = FixedAssetNumber;
                                                drNew["itemCount"] = dtItemInfo.Count;
                                                dt.Rows.Add(drNew);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dt;
        }
        public DataTable GetListEvent(string[] portIds, DateTime dtBegin, DateTime dtEnd, string taskType, string[] taskItem, string userName, bool istrue)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("pointName", typeof(string));
            dt.Columns.Add("eventType", typeof(string));
            dt.Columns.Add("item", typeof(string));
            dt.Columns.Add("itemCount", typeof(string));
            dt.Columns.Add("ActionUserName", typeof(string));
            dt.Columns.Add("itemDate", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            if (portIds != null && portIds.Length > 0)
            {
                string strWhere = " 1=1 ";
                if (portIds != null)
                {
                    string strPointids = string.Join(",", portIds);
                    strWhere += string.Format(" and PointId in ({0}) ", strPointids);
                }

                if (!string.IsNullOrWhiteSpace(taskType))
                {
                    strWhere += string.Format(" and MissionID ='{0}' ", taskType);
                }
                if (dtBegin != null)
                {
                    strWhere += string.Format(" and ActionDate >='{0}' ", dtBegin);
                }
                if (dtEnd != null)
                {
                    strWhere += string.Format(" and FinishDate <='{0}' ", dtEnd);
                }
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    strWhere += string.Format(" and ActionUserName like '%{0}%' ", userName);
                }
                string taskItemGuid = "";
                if (taskItem.Length > 0)
                {
                    taskItemGuid = "'" + string.Join("','", taskItem) + "'";
                }
                DataTable dtTaskInfo = r_MaintenanceRepository.GetList(strWhere);
                DataView dtItemInfo = s_OM_TaskItemDataService.GetListData(taskItemGuid, dtBegin, dtEnd).DefaultView;
                if (!istrue)
                {
                    foreach (DataRow dr in dtTaskInfo.Rows)
                    {
                        dtItemInfo.RowFilter = "TaskCode='" + dr["TaskCode"].ToString() + "'";
                        if (dtItemInfo.Count > 0)
                        {

                            for (int i = 0; i < dtItemInfo.Count; i++)
                            {
                                DataRow drNew = dt.NewRow();
                                drNew["pointName"] = dr["PointName"].ToString();
                                drNew["eventType"] = "进站事由";
                                drNew["item"] = dtItemInfo[i]["TaskItemGuid"].ToString();
                                drNew["ActionUserName"] = dr["ActionUserName"].ToString();
                                drNew["itemDate"] = dtItemInfo[i]["ItemRecordDate"].ToString();
                                drNew["Description"] = dtItemInfo[i]["UniversalValue2"].ToString();
                                dt.Rows.Add(drNew);
                            }

                        }
                        else
                        {
                            DataRow drNew = dt.NewRow();
                            drNew["pointName"] = dr["PointName"].ToString();
                            drNew["eventType"] = "进站事由";
                            drNew["ActionUserName"] = dr["ActionUserName"].ToString();
                            dt.Rows.Add(drNew);
                        }
                    }
                }
                else
                {
                }
            }
            return dt;
        }
        /// <summary>
        /// 获取巡检任务信息
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataTable GetListInspect(string[] portIds, DateTime dtBegin, DateTime dtEnd, string taskType, string[] taskItem, string userName, bool istrue)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("pointName", typeof(string));
            dt.Columns.Add("timeRange", typeof(string));
            dt.Columns.Add("itemType", typeof(string));
            dt.Columns.Add("item", typeof(string));
            dt.Columns.Add("itemCount", typeof(string));
            dt.Columns.Add("itemDate", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("ActionUserName", typeof(string));
            if (portIds != null && portIds.Length > 0)
            {
                string strWhere = " 1=1 ";
                if (portIds != null)
                {
                    string strPointids = string.Join(",", portIds);
                    strWhere += string.Format(" and PointId in ({0}) ", strPointids);
                }

                if (!string.IsNullOrWhiteSpace(taskType))
                {
                    strWhere += string.Format(" and MissionID ='{0}' ", taskType);
                }
                if (dtBegin != null)
                {
                    strWhere += string.Format(" and ActionDate >='{0}' ", dtBegin);
                }
                if (dtEnd != null)
                {
                    strWhere += string.Format(" and FinishDate <='{0}' ", dtEnd);
                }
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    strWhere += string.Format(" and ActionUserName like '%{0}%' ", userName);
                }
                string taskItemGuid = "";
                if (taskItem.Length > 0)
                {
                    taskItemGuid = "'" + string.Join("','", taskItem) + "'";
                }
                DataTable dtTaskInfo = r_MaintenanceRepository.GetList(strWhere);
                DataView dtItemInfo = s_OM_TaskItemDataService.GetListData(taskItemGuid, dtBegin, dtEnd).DefaultView;
                if (!istrue)
                {
                    foreach (DataRow dr in dtTaskInfo.Rows)
                    {
                        dtItemInfo.RowFilter = "TaskCode='" + dr["TaskCode"].ToString() + "'";
                        if (dtItemInfo.Count > 0)
                        {
                            for (int i = 0; i < dtItemInfo.Count; i++)
                            {
                                if (dtItemInfo[i]["ItemValue"].ToString() == "0" || dtItemInfo[i]["ItemValue"].ToString() == "1")
                                {
                                    DataRow drNew = dt.NewRow();
                                    drNew["pointName"] = dr["PointName"].ToString();
                                    drNew["timeRange"] = dr["ActionDate"].ToString() + "~" + dr["FinishDate"].ToString();
                                    drNew["itemType"] = dr["MissionName"].ToString();
                                    drNew["item"] = dtItemInfo[i]["TaskItemGuid"].ToString();
                                    drNew["itemDate"] = dtItemInfo[i]["ItemRecordDate"].ToString();
                                    drNew["Description"] = dtItemInfo[i]["Remark"].ToString();
                                    drNew["ActionUserName"] = dr["ActionUserName"].ToString();
                                    dt.Rows.Add(drNew);
                                }
                            }
                        }
                        else
                        {
                            DataRow drNew = dt.NewRow();
                            drNew["pointName"] = dr["PointName"].ToString();
                            drNew["timeRange"] = dr["ActionDate"].ToString() + "~" + dr["FinishDate"].ToString();
                            drNew["itemType"] = dr["MissionName"].ToString();
                            drNew["ActionUserName"] = dr["ActionUserName"].ToString();
                            dt.Rows.Add(drNew);
                        }
                    }
                }
                else
                {
                    foreach (string pointId in portIds)
                    {
                        DataRow[] drArry = dtTaskInfo.Select("PointId=" + pointId);
                        string strTaskCodes = "";
                        string pointName = "";
                        string MissionName = "";
                        DateTime ActionDate = dtBegin;
                        DateTime FinishDate = dtEnd;
                        for (int i = 0; i < drArry.Length; i++)
                        {
                            if (drArry[i]["MissionName"] != DBNull.Value)
                            {
                                MissionName = drArry[i]["MissionName"].ToString();
                            }
                            if (drArry[i]["PointName"] != DBNull.Value)
                            {
                                pointName = drArry[i]["PointName"].ToString();
                            }
                            if (drArry[i]["TaskCode"] != DBNull.Value)
                            {
                                strTaskCodes = "'" + drArry[i]["TaskCode"].ToString() + "',";
                            }
                            if (drArry[i]["ActionDate"] != DBNull.Value)
                            {
                                if (ActionDate > DateTime.Parse(drArry[i]["ActionDate"].ToString()))
                                {
                                    ActionDate = DateTime.Parse(drArry[i]["ActionDate"].ToString());
                                }
                            }
                            if (drArry[i]["FinishDate"] != DBNull.Value)
                            {
                                if (FinishDate < DateTime.Parse(drArry[i]["FinishDate"].ToString()))
                                {
                                    FinishDate = DateTime.Parse(drArry[i]["FinishDate"].ToString());
                                }
                            }
                        }
                        if (taskItem.Length > 0)
                        {
                            if (strTaskCodes != "")
                            {
                                strTaskCodes = strTaskCodes.TrimEnd(',');
                                foreach (string itemGuid in taskItem)
                                {
                                    dtItemInfo.RowFilter = "TaskCode in(" + strTaskCodes + ") and TaskItemGuid='" + itemGuid + "' and ItemValue in (1,0)";
                                    DataRow drNew = dt.NewRow();
                                    drNew["pointName"] = pointName;
                                    drNew["timeRange"] = ActionDate.ToString() + "~" + FinishDate.ToString();
                                    drNew["itemType"] = MissionName;
                                    drNew["item"] = itemGuid;
                                    drNew["itemCount"] = dtItemInfo.Count;
                                    dt.Rows.Add(drNew);
                                }
                            }
                        }
                        else
                        {
                            DataRow drNew = dt.NewRow();
                            drNew["pointName"] = pointName;
                            drNew["timeRange"] = ActionDate.ToString() + "~" + FinishDate.ToString();
                            drNew["itemType"] = MissionName;
                            drNew["itemCount"] = drArry.Length;
                            dt.Rows.Add(drNew);
                        }

                    }
                }
            }
            return dt;
        }
        /// <summary>
        /// 仪器使用信息
        /// </summary>
        /// <param name="FixedAssetNumbers">仪器编号</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataTable GetListByInstrument(string[] FixedAssetNumbers, DateTime dtStart, DateTime dtEnd)
        {
            return r_MaintenanceRepository.GetListByInstrument(FixedAssetNumbers, dtStart, dtEnd);
        }
        public DataTable GetListRepairByInstrument(string[] FixedAssetNumbers, DateTime dtStart, DateTime dtEnd, string taskItemGuid)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("pointName", typeof(string));
            dt.Columns.Add("InstanceName", typeof(string));
            dt.Columns.Add("SpecificationModel", typeof(string));
            dt.Columns.Add("FixedAssetNumber", typeof(string));
            dt.Columns.Add("faultType", typeof(string));
            dt.Columns.Add("faultPerson", typeof(string));
            dt.Columns.Add("faultTime", typeof(string));
            dt.Columns.Add("reasonAndProcess", typeof(string));
            dt.Columns.Add("repairPerson", typeof(string));
            dt.Columns.Add("repairTime", typeof(string));
            dt.Columns.Add("fittingName", typeof(string));
            dt.Columns.Add("fittingPerson", typeof(string));
            dt.Columns.Add("fittingTime", typeof(string));
            string strFixedAssetNumbers = "";
            if (FixedAssetNumbers.Length > 0)
            {
                strFixedAssetNumbers = "'" + string.Join("','", FixedAssetNumbers) + "'";
            }
            DataView dtItemInfo = s_OM_TaskItemDataService.GetListDatabyDevice(taskItemGuid, dtStart, dtEnd, strFixedAssetNumbers, new string[] { }, true).DefaultView;
            return dt;
        }
        #endregion
    }
}
