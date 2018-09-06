using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
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
    public partial class EventProcessing : SmartEP.WebUI.Common.BasePage
    {
        private string MissionId = "";
        private string MissionName = "";
        DataView dv = new DataView();
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
        DictionaryService dicService = new DictionaryService();
        FrameworkModel _userModel = new FrameworkModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
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
            ViewState["TaskCode"] = PageHelper.GetQueryString("TaskCode");
            string strFormCode = PageHelper.GetQueryString("FormCode");
            string strFormName = "";

            object taskInfo = WebServiceHelper.InvokeWebService(m_OperationTaskWebServiceUrl, "GetTaskInfoByID", new object[] { new string[] { ViewState["TaskCode"].ToString() } });
            DataTable dtTaskInfo = taskInfo as DataTable;
            if (dtTaskInfo.DefaultView.Count != 0)
            {
                string pointGuid = dtTaskInfo.Rows[0]["ObjectID"].ToString();
                strFormName = dtTaskInfo.Rows[0]["FormName"].ToString();
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
            //ViewState["TaskCode"] = "201605077538";
            //string strFormCode = "07";
            DataTable dtTask = r_OM_TaskItemConfigService.GetListTask(strFormCode, strFormName);
            if (strFormName == "" && strFormCode == "")
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

            DataTable taskDt = m_WaterInspectionBaseService.GetList(ViewState["TaskCode"].ToString(), "");
            if (taskDt.DefaultView.Count != 0)
            {
                ViewState["BaseStatus"] = "edit";
                ViewState["BaseId"] = taskDt.Rows[0]["id"].ToString();
                dtpTime.SelectedDate = DateTime.Parse(taskDt.Rows[0]["FileUpLoadDate"].ToString());
                dtpBegin.SelectedDate = DateTime.Parse(taskDt.Rows[0]["ActionDate"].ToString());
                dtpEnd.SelectedDate = DateTime.Parse(taskDt.Rows[0]["FinishDate"].ToString());

                DataTable TaskItemData = r_OM_TaskItemDataService.GetList(ViewState["TaskCode"].ToString(), taskDt.Rows[0]["id"].ToString());
                DataView dvTaskItem = TaskItemData.DefaultView;
                foreach (Telerik.Web.UI.GridDataItem item in gridAroundInspect.Items)
                {

                    CheckBoxList cblYes = (CheckBoxList)item.FindControl("cblYes");
                    CheckBoxList cblNo = (CheckBoxList)item.FindControl("cblNo");
                    CheckBoxList cblAbnormal = (CheckBoxList)item.FindControl("cblAbnormal");
                    RadDateTimePicker rftpTime = (RadDateTimePicker)item.FindControl("dtpGrid");
                    RadTextBox rdtDescription = (RadTextBox)item.FindControl("rdtDescription");
                    RadComboBox rcbAbnormal = (RadComboBox)item.FindControl("rcbAbnormal");
                    string[] guid = item.KeyValues.Split('"');
                    dv.RowFilter = "ItemName='" + item["ItemName"].Text + "' and RowGuid='" + guid[1] + "'";
                    if (dv.Count > 0)
                    {
                        if (dv[0]["ItemFactor"] != DBNull.Value && dv[0]["ItemFactor"].ToString() != "")
                        {
                            cblAbnormal.Style["display"] = "normal";
                            cblAbnormal.Items.Add(new ListItem(dv[0]["ItemFactor"].ToString()));
                        }
                        else
                        {
                            cblAbnormal.Style["display"] = "none";
                        }
                    }
                    IQueryable<V_CodeMainItemEntity> integratorEntites = dicService.RetrieveList(DictionaryType.AMS, "具体异常");
                    rcbAbnormal.DataSource = integratorEntites;
                    rcbAbnormal.DataTextField = "ItemText";
                    rcbAbnormal.DataValueField = "ItemGuid";
                    rcbAbnormal.DataBind();
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
                            if (dvTaskItem[0]["ItemFactor"].ToString() == "1")
                            {
                                cblAbnormal.Items[0].Selected = true;
                            }
                            if (dvTaskItem[0]["ItemRecordDate"] != DBNull.Value)
                            {
                                rftpTime.SelectedDate = DateTime.Parse(dvTaskItem[0]["ItemRecordDate"].ToString());
                            }
                            else
                            {
                                rftpTime.SelectedDate = DateTime.Now;
                            }
                            if (dvTaskItem[0]["UniversalValue1"].ToString() != "")
                            {
                                rcbAbnormal.Items.Clear();
                                string[] SN = dvTaskItem[0]["UniversalValue1"].ToString().Split(',');
                                for (int i = 0; i < SN.Length; i++)
                                    rcbAbnormal.Items.Add(new RadComboBoxItem(SN[i].ToString(), SN[i].ToString()));
                                for (int i = 0; i < rcbAbnormal.Items.Count; i++)
                                {
                                    rcbAbnormal.Items[i].Checked = true;
                                }
                            }
                            rdtDescription.Text = dvTaskItem[0]["Remark"].ToString();
                            beizhu.Text = dvTaskItem[0]["UniversalValue2"].ToString();
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
                    CheckBoxList cblAbnormal = (CheckBoxList)item.FindControl("cblAbnormal");
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
                        if (dv[0]["ItemFactor"].ToString() == "1")
                        {
                            cblAbnormal.Items[0].Selected = true;
                        }
                    }
                }
            }
        }
        protected void gridAroundInspect_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = r_OM_TaskItemConfigService.GetList(HiddenMissionId.Value, "air", "2");
            dv = dt.DefaultView;
            gridAroundInspect.DataSource = dt;

            gridAroundInspect.VirtualItemCount = dt.Rows.Count;
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
                    List<OM_TaskItemDatumEntity> OM_TaskItemDatumEntityArry = new List<OM_TaskItemDatumEntity>();
                    foreach (Telerik.Web.UI.GridDataItem item in gridAroundInspect.Items)
                    {
                        OM_TaskItemDatumEntity OM_TaskItemDatumEntity = new OM_TaskItemDatumEntity();
                        CheckBoxList cblYes = (CheckBoxList)item.FindControl("cblYes");
                        CheckBoxList cblNo = (CheckBoxList)item.FindControl("cblNo");
                        CheckBoxList cblAbnormal = (CheckBoxList)item.FindControl("cblAbnormal");
                        RadDateTimePicker rftpTime = (RadDateTimePicker)item.FindControl("dtpGrid");
                        RadTextBox rdtDescription = (RadTextBox)item.FindControl("rdtDescription");
                        RadComboBox rcbAbnormal = (RadComboBox)item.FindControl("rcbAbnormal");

                        OM_TaskItemDatumEntity.TaskGuid = int.Parse(taskDt.Rows[0]["id"].ToString());
                        OM_TaskItemDatumEntity.TaskCode = ViewState["TaskCode"].ToString();
                        string[] guid = item.KeyValues.Split('"');
                        if (guid.Length > 2)
                        {
                            OM_TaskItemDatumEntity.TaskItemGuid = new Guid(guid[1]);
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
                        if (cblAbnormal.Items.Count > 0)
                        {
                            if (cblAbnormal.Items[0].Selected == true)
                            {
                                OM_TaskItemDatumEntity.ItemFactor = "1";
                            }
                        }
                        if (rftpTime.SelectedDate != null)
                        {
                            OM_TaskItemDatumEntity.ItemRecordDate = rftpTime.SelectedDate.Value;
                        }
                        OM_TaskItemDatumEntity.MaintainceUser = ViewState["UserName"].ToString();
                        OM_TaskItemDatumEntity.MaintainceDate = dtpTime.SelectedDate.Value;
                        OM_TaskItemDatumEntity.Remark = rdtDescription.Text;
                        string abnormalStr = "";
                        foreach (RadComboBoxItem t in rcbAbnormal.CheckedItems)
                        {
                            abnormalStr += (t.Text.ToString() + ",");
                        }
                        abnormalStr = abnormalStr.Trim(',');
                        OM_TaskItemDatumEntity.UniversalValue1 = abnormalStr;
                        OM_TaskItemDatumEntity.UniversalValue2 = beizhu.Text;
                        OM_TaskItemDatumEntityArry.Add(OM_TaskItemDatumEntity);
                    }
                    r_OM_TaskItemDataService.insertTable(OM_TaskItemDatumEntityArry.ToArray(), int.Parse(taskDt.Rows[0]["id"].ToString()), ViewState["TaskCode"].ToString());
                }
            }
        }
    }
}