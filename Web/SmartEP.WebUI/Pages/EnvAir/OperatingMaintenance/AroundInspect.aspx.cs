﻿using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.OperatingMaintenance;
using SmartEP.Service.OperatingMaintenance.Air;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.Web.UI;
using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance
{
    public partial class AroundInspect : SmartEP.WebUI.Common.BasePage
    {
        private string MissionId = "";
        private string MissionName = "";
        /// <summary>
        /// 运维平台任务处理WebService路径
        /// </summary>
        private string m_OperationTaskWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationTaskWebServiceUrl"].ToString();
        /// <summary>
        /// 运维任务项配置服务层
        /// </summary>
        OM_TaskItemConfigService r_OM_TaskItemConfigService = Singleton<OM_TaskItemConfigService>.GetInstance();
        MonitoringPointWaterService m_MonitoringPointWaterService = Singleton<MonitoringPointWaterService>.GetInstance();
        WaterInspectionBaseEntity WaterInspectionBaseEntity = new WaterInspectionBaseEntity();
        WaterInspectionBaseService m_WaterInspectionBaseService = new WaterInspectionBaseService();
        OM_TaskItemDataService r_OM_TaskItemDataService = Singleton<OM_TaskItemDataService>.GetInstance();
        MaintenanceService m_MaintenanceService = Singleton<MaintenanceService>.GetInstance();
        FrameworkModel _userModel = new FrameworkModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
                if (PageHelper.GetQueryString("FormOpeType") == "2")
                {
                    Response.Redirect(string.Format("AroundInspectList.aspx?TaskCode={0}&&MissionId={1}", ViewState["TaskCode"], HiddenMissionId.Value));
                }
            }
        }
        protected void Page_Prerender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }
        private void InitControl()
        {
            ViewState["TaskCode"] = PageHelper.GetQueryString("TaskCode");
            object taskInfo = WebServiceHelper.InvokeWebService(m_OperationTaskWebServiceUrl, "GetTaskInfoByID", new object[] { new string[] { ViewState["TaskCode"].ToString() } });
            DataTable dtTaskInfo = taskInfo as DataTable;
            string FormName = "";
            if (dtTaskInfo.DefaultView.Count != 0)
            {
                FormName = dtTaskInfo.Rows[0]["FormName"].ToString();
                string pointGuid = dtTaskInfo.Rows[0]["ObjectID"].ToString();
                MonitoringPointEntity monitoringPointEntity = m_MonitoringPointWaterService.RetrieveEntityByUid(pointGuid);
                if (monitoringPointEntity == null)
                {
                    Alert("该站点不存在！请重新下发任务！");
                    return;
                }
            }
            else
            {
                Alert("任务不存在！请重新下发任务！");
                return;
            }

            string userGuid = PageHelper.GetQueryString("UserGuid");
            if (!string.IsNullOrWhiteSpace(userGuid))
            {
                Frame_UserEntity userObj = _userModel.Frame_UserEntities.FirstOrDefault(x => x.RowGuid.Equals(userGuid) && x.IsEnabled != (Nullable<int>)null && x.IsEnabled.Value == 1);
                ViewState["UserName"] = userObj.DisplayName;
            }
            else
            {
                ViewState["UserName"] = SessionHelper.Get("DisplayName").ToString();
            }
            string strFormCode = PageHelper.GetQueryString("FormCode");
            DataTable dtTask = r_OM_TaskItemConfigService.GetListTask(strFormCode, FormName);
            if (FormName == "" && strFormCode == "")
            {
                HiddenMissionId.Value = "";
                HiddenMissionName.Value = "";
            }
            else
            {
                if (dtTask.Rows.Count > 0)
                {
                    MissionId = dtTask.Rows[0]["MissionID"].ToString();
                    MissionName = dtTask.Rows[0]["MissionName"].ToString();
                    HiddenMissionId.Value = MissionId;
                    HiddenMissionName.Value = MissionName;
                }
            }
        }
        public void BindGrid()
        {
            ViewState["BaseStatus"] = "";
            ViewState["BaseId"] = "";
            ViewState["TaskItemData"] = "";
            DataTable taskDt = m_WaterInspectionBaseService.GetList(ViewState["TaskCode"].ToString(), "");
            if (taskDt.DefaultView.Count != 0)
            {
                ViewState["BaseStatus"] = "edit";
                ViewState["BaseId"] = taskDt.Rows[0]["id"].ToString();
                dtpTime.SelectedDate = DateTime.Parse(taskDt.Rows[0]["FileUpLoadDate"].ToString());
                dtpBegin.SelectedDate = DateTime.Parse(taskDt.Rows[0]["ActionDate"].ToString());
                dtpEnd.SelectedDate = DateTime.Parse(taskDt.Rows[0]["FinishDate"].ToString());

                DataTable TaskItemData = r_OM_TaskItemDataService.GetList(ViewState["TaskCode"].ToString(), taskDt.Rows[0]["id"].ToString());
                if (TaskItemData.Rows.Count > 0)
                {
                    ViewState["TaskItemData"] = "edit";
                }
                DataView dvTaskItem = TaskItemData.DefaultView;
                foreach (Telerik.Web.UI.GridDataItem item in gridAroundInspect.Items)
                {

                    CheckBoxList cblYes = (CheckBoxList)item.FindControl("cblYes");
                    CheckBoxList cblNo = (CheckBoxList)item.FindControl("cblNo");
                    RadDateTimePicker rftpTime = (RadDateTimePicker)item.FindControl("dtpGrid");
                    RadTextBox rdtDescription = (RadTextBox)item.FindControl("rdtDescription");
                    string[] guid = item.KeyValues.Split('"');
                    if (guid.Length > 2)
                    {
                        dvTaskItem.RowFilter = "TaskItemGuid='" + guid[1] + "'";
                        if (dvTaskItem.Count > 0)
                        {
                            string DefaultValue = dvTaskItem[0]["ItemValue"].ToString();
                            if (DefaultValue == "1")
                            {
                                cblYes.Items[0].Selected = true;
                            }
                            else if (DefaultValue == "0")
                            {
                                cblNo.Items[0].Selected = true;
                            }
                            if (dvTaskItem[0]["ItemRecordDate"] != DBNull.Value)
                            {
                                rftpTime.SelectedDate = DateTime.Parse(dvTaskItem[0]["ItemRecordDate"].ToString());
                            }
                            else
                            {
                                rftpTime.SelectedDate = DateTime.Now;
                            }
                            rdtDescription.Text = dvTaskItem[0]["Remark"].ToString();
                        }
                    }
                }
            }
            else
            {
                dtpTime.SelectedDate = DateTime.Now;
                dtpBegin.SelectedDate = DateTime.Now;
                dtpEnd.SelectedDate = DateTime.Now;
                DataTable dt = r_OM_TaskItemConfigService.GetList(HiddenMissionId.Value, "air", "2");
                DataView dv = dt.DefaultView;
                foreach (Telerik.Web.UI.GridDataItem item in gridAroundInspect.Items)
                {

                    CheckBoxList cblYes = (CheckBoxList)item.FindControl("cblYes");
                    CheckBoxList cblNo = (CheckBoxList)item.FindControl("cblNo");
                    RadDateTimePicker rftpTime = (RadDateTimePicker)item.FindControl("dtpGrid");
                    rftpTime.SelectedDate = DateTime.Now;
                    dv.RowFilter = "ItemName='" + item["ItemName"].Text + "'";
                    if (dv.Count > 0)
                    {
                        string DefaultValue = dv[0]["DefaultValue"].ToString();
                        if (DefaultValue == "1")
                        {
                            cblYes.Items[0].Selected = true;
                        }
                        else if (DefaultValue == "0")
                        {
                            cblNo.Items[0].Selected = true;
                        }
                    }
                }
            }
        }
        protected void gridAroundInspect_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = r_OM_TaskItemConfigService.GetList(HiddenMissionId.Value, "air", "2");
            DataView dv = dt.DefaultView;
            dv.RowFilter = "ItemLevel='2' and UpItem<>'请选择'";
            dv.Sort = "UpItem";
            gridAroundInspect.DataSource = dv;
            gridAroundInspect.VirtualItemCount = dv.Count;
        }

        protected void gridAroundInspect_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            object taskInfo = WebServiceHelper.InvokeWebService(m_OperationTaskWebServiceUrl, "GetTaskInfoByID", new object[] { new string[] { ViewState["TaskCode"].ToString() } });
            DataTable dtTaskInfo = taskInfo as DataTable;
            if (dtTaskInfo.Rows.Count > 0)
            {
                string pointGuid = dtTaskInfo.Rows[0]["ObjectID"].ToString();
                MonitoringPointEntity monitoringPointEntity = m_MonitoringPointWaterService.RetrieveEntityByUid(pointGuid);
                DateTime acStartTinme = DateTime.TryParse(dtpBegin.SelectedDate.Value.ToString(), out acStartTinme) ? acStartTinme : DateTime.Now;
                DateTime EndTinme = DateTime.Now;
                int ID = int.TryParse(ViewState["BaseId"].ToString(), out ID) ? ID : 0;
                if (ViewState["BaseStatus"].ToString() == "Edit")
                {
                    if (ID != 0)
                    {
                        WaterInspectionBaseEntity.id = ID;
                    }
                }
                WaterInspectionBaseEntity.MissionID = new Guid(HiddenMissionId.Value != "" ? HiddenMissionId.Value : Guid.NewGuid().ToString());
                WaterInspectionBaseEntity.MissionName = HiddenMissionName.Value;
                WaterInspectionBaseEntity.ActionDate = acStartTinme;
                WaterInspectionBaseEntity.FinishDate = EndTinme;
                DateTime BeginDatePlan = DateTime.TryParse(dtTaskInfo.Rows[0]["BeginDatePlan"].ToString(), out BeginDatePlan) ? BeginDatePlan : DateTime.Now;
                WaterInspectionBaseEntity.StartDate = BeginDatePlan;
                DateTime EndDatePlan = DateTime.TryParse(dtTaskInfo.Rows[0]["EndDatePlan"].ToString(), out EndDatePlan) ? EndDatePlan : DateTime.Now;
                WaterInspectionBaseEntity.EndDate = EndDatePlan;
                WaterInspectionBaseEntity.PointId = monitoringPointEntity.PointId.ToString();
                WaterInspectionBaseEntity.PointName = monitoringPointEntity.MonitoringPointName;
                WaterInspectionBaseEntity.TaskCode = ViewState["TaskCode"].ToString();
                WaterInspectionBaseEntity.ActionUserName = ViewState["UserName"].ToString();
                WaterInspectionBaseEntity.FileUpLoadDate = dtpTime.SelectedDate.Value;//进站时间
                WaterInspectionBaseEntity[] WaterInspectionBaseEntityarry = { WaterInspectionBaseEntity };
                if (ViewState["BaseStatus"].ToString() == "edit")
                {
                    bool flag = m_WaterInspectionBaseService.Update(WaterInspectionBaseEntity);
                }
                else
                {
                    int flag = m_WaterInspectionBaseService.Add(WaterInspectionBaseEntityarry);
                }
                //if (flag > 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('保存成功！');", true);
                //}
                DataTable taskDt = m_WaterInspectionBaseService.GetList(ViewState["TaskCode"].ToString(), "");
                if (taskDt.Rows.Count > 0)
                {
                    DataView dvInstance = m_MaintenanceService.GetInstanceByGuid(new string[] { monitoringPointEntity.MonitoringPointUid }, "2");
                    List<OM_TaskItemDatumEntity> OM_TaskItemDatumEntityArry = new List<OM_TaskItemDatumEntity>();
                    foreach (Telerik.Web.UI.GridDataItem item in gridAroundInspect.Items)
                    {
                        OM_TaskItemDatumEntity OM_TaskItemDatumEntity = new OM_TaskItemDatumEntity();
                        CheckBoxList cblYes = (CheckBoxList)item.FindControl("cblYes");
                        CheckBoxList cblNo = (CheckBoxList)item.FindControl("cblNo");
                        RadDateTimePicker rftpTime = (RadDateTimePicker)item.FindControl("dtpGrid");
                        RadTextBox rdtDescription = (RadTextBox)item.FindControl("rdtDescription");
                        OM_TaskItemDatumEntity.TaskGuid = int.Parse(taskDt.Rows[0]["id"].ToString()); //任务主表id
                        OM_TaskItemDatumEntity.TaskCode = ViewState["TaskCode"].ToString();//任务编号
                        string[] guid = item.KeyValues.Split('"');
                        if (guid.Length > 2)
                        {
                            OM_TaskItemDatumEntity.TaskItemGuid = new Guid(guid[1]);//监测项目GUID
                        }
                        if (cblYes.Items.Count > 0)
                        {
                            if (cblYes.Items[0].Selected == true)
                            {
                                OM_TaskItemDatumEntity.ItemValue = "1";
                            }
                        }
                        if (cblNo.Items.Count > 0)
                        {
                            if (cblNo.Items[0].Selected == true)
                            {
                                OM_TaskItemDatumEntity.ItemValue = "0";
                            }
                        }
                        if (rftpTime.SelectedDate != null)
                        {
                            OM_TaskItemDatumEntity.ItemRecordDate = rftpTime.SelectedDate.Value;  //监测项目维护时间
                        }
                        else
                        {
                            OM_TaskItemDatumEntity.ItemRecordDate = DateTime.Now;
                        }
                        OM_TaskItemDatumEntity.UniversalValue4 = item["UpItem"].Text; //项目分类
                        OM_TaskItemDatumEntity.MaintainceUser = ViewState["UserName"].ToString();//维护人
                        OM_TaskItemDatumEntity.MaintainceDate = dtpTime.SelectedDate.Value; //巡检日期
                        OM_TaskItemDatumEntity.Remark = rdtDescription.Text;// 备注
                        string instrumentName = "";
                        string instrumentNum = "";
                        if (item["ItemName"].Text.Contains("SO2"))
                        {
                            dvInstance.RowFilter = "InstanceName like '%SO2%'";
                            if (dvInstance.Count > 0)
                            {
                                instrumentName = dvInstance[0]["InstanceName"].ToString();
                                instrumentNum = dvInstance[0]["FixedAssetNumber"].ToString();
                            }
                        }
                        else if (item["ItemName"].Text.Contains("NOx"))
                        {
                            dvInstance.RowFilter = "InstanceName like '%NOx%'";
                            if (dvInstance.Count > 0)
                            {
                                instrumentName = dvInstance[0]["InstanceName"].ToString();
                                instrumentNum = dvInstance[0]["FixedAssetNumber"].ToString();
                            }
                        }
                        else if (item["ItemName"].Text.Contains("CO"))
                        {
                            dvInstance.RowFilter = "InstanceName like '%CO%'";
                            if (dvInstance.Count > 0)
                            {
                                instrumentName = dvInstance[0]["InstanceName"].ToString();
                                instrumentNum = dvInstance[0]["FixedAssetNumber"].ToString();
                            }
                        }
                        else if (item["ItemName"].Text.Contains("O3"))
                        {
                            dvInstance.RowFilter = "InstanceName like '%O3%'";
                            if (dvInstance.Count > 0)
                            {
                                instrumentName = dvInstance[0]["InstanceName"].ToString();
                                instrumentNum = dvInstance[0]["FixedAssetNumber"].ToString();
                            }
                        }
                        else if (item["ItemName"].Text.Contains("PM10"))
                        {
                            dvInstance.RowFilter = "InstanceName like '%PM10%'";
                            if (dvInstance.Count > 0)
                            {
                                instrumentName = dvInstance[0]["InstanceName"].ToString();
                                instrumentNum = dvInstance[0]["FixedAssetNumber"].ToString();
                            }
                        }
                        else if (item["ItemName"].Text.Contains("PM2.5"))
                        {
                            dvInstance.RowFilter = "InstanceName like '%PM2.5%'";
                            if (dvInstance.Count > 0)
                            {
                                instrumentName = dvInstance[0]["InstanceName"].ToString();
                                instrumentNum = dvInstance[0]["FixedAssetNumber"].ToString();
                            }
                        };
                        OM_TaskItemDatumEntity.UniversalValue2 = instrumentName;//仪器名称
                        OM_TaskItemDatumEntity.UniversalValue3 = instrumentNum;//仪器编号
                        OM_TaskItemDatumEntity.UniversalValue4 = item["ItemName"].Text;
                        if (item["ItemName"].Text.Contains("主"))
                        {
                            OM_TaskItemDatumEntity.Remark = "主";
                        }
                        if (item["ItemName"].Text.Contains("辅"))
                        {
                            OM_TaskItemDatumEntity.Remark = "辅";
                        }
                        if (item["ItemName"].Text.Contains("样气"))
                        {
                            OM_TaskItemDatumEntity.Remark = "样气";
                        }
                        if (item["ItemName"].Text.Contains("池压"))
                        {
                            OM_TaskItemDatumEntity.Remark = "池压";
                        }
                    }
                    if (ViewState["TaskItemData"].ToString() == "edit")
                    {
                        r_OM_TaskItemDataService.UpdateTable(OM_TaskItemDatumEntityArry.ToArray(), int.Parse(taskDt.Rows[0]["id"].ToString()), ViewState["TaskCode"].ToString());
                    }
                    else
                    {
                        r_OM_TaskItemDataService.insertTable(OM_TaskItemDatumEntityArry.ToArray(), int.Parse(taskDt.Rows[0]["id"].ToString()), ViewState["TaskCode"].ToString());
                    }
                }
            }
        }
    }
}