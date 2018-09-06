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
    public partial class DeviceRepair : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// MissionId：表单编号
        /// MissionName：表单名称
        /// </summary>
        private string MissionId = "06";
        private string MissionName = "";
        private string pointGuid = "";
        private string InstrumentInstanceGuid = "";
        private int pointId = -1;

        /// <summary>
        /// 用户实体
        /// </summary>
        FrameworkModel _userModel = new FrameworkModel();
        /// <summary>
        /// 运维任务项配置服务层
        /// </summary>
        OM_TaskItemConfigService r_OM_TaskItemConfigService = Singleton<OM_TaskItemConfigService>.GetInstance();
        /// <summary>
        /// 运维平台任务处理WebService路径
        /// </summary>
        private string m_OperationTaskWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationTaskWebServiceUrl"].ToString();
        private string m_OperationOMMPGetDataWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationOMMPGetDataWebServiceUrl"].ToString();
        /// <summary>
        /// 水站点服务
        /// </summary>
        MonitoringPointWaterService m_MonitoringPointWaterService = Singleton<MonitoringPointWaterService>.GetInstance();
        /// <summary>
        /// 任务主表服务
        /// </summary>
        WaterInspectionBaseService m_WaterInspectionBaseService = new WaterInspectionBaseService();
        //服务处理
        MaintenanceService m_MaintenanceService = Singleton<MaintenanceService>.GetInstance();
        /// <summary>
        /// 任务主表实体
        /// </summary>
        WaterInspectionBaseEntity WaterInspectionBaseEntity = new WaterInspectionBaseEntity();
        //代码项服务层
        DictionaryService dicService = new DictionaryService();
        /// <summary>
        /// 任务保存服务
        /// </summary>
        OM_TaskItemDataService r_OM_TaskItemDataService = Singleton<OM_TaskItemDataService>.GetInstance();
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
        /// <summary>
        /// 值初始化
        /// </summary>
        private void InitControl()
        {
            ViewState["TaskCode"] = PageHelper.GetQueryString("TaskCode");
            object taskInfo = WebServiceHelper.InvokeWebService(m_OperationTaskWebServiceUrl, "GetTaskInfoByID", new object[] { new string[] { ViewState["TaskCode"].ToString() } });
            DataTable dtTaskInfo = taskInfo as DataTable;
            string FormName = "";
            if (dtTaskInfo.DefaultView.Count != 0)
            {
                FormName = dtTaskInfo.Rows[0]["FormName"].ToString();
                InstrumentInstanceGuid = dtTaskInfo.Rows[0]["InstrumentInstanceGuid"].ToString();
                pointGuid = dtTaskInfo.Rows[0]["ObjectID"].ToString();
                MonitoringPointEntity monitoringPointEntity = m_MonitoringPointWaterService.RetrieveEntityByUid(pointGuid);
                if (monitoringPointEntity == null)
                {
                    Alert("该站点不存在！请重新下发任务！");
                    return;
                }
                pointId = monitoringPointEntity.PointId;
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
            if (dtTask.Rows.Count > 0)
            {
                MissionId = dtTask.Rows[0]["MissionID"].ToString();
                MissionName = dtTask.Rows[0]["MissionName"].ToString();
                HiddenMissionId.Value = MissionId;
                HiddenMissionName.Value = MissionName;
            }
        }
        /// <summary>
        /// 绑定控件值
        /// </summary>
        public void BindGrid()
        {
            ViewState["BaseStatus"] = "";
            ViewState["BaseId"] = "";
            DataTable taskDt = m_WaterInspectionBaseService.GetList(ViewState["TaskCode"].ToString(), "");
            DataView dvInstance = m_MaintenanceService.GetInstanceByGuid(new string[] { pointGuid }, "2");
            ddlDecive.DataSource = dvInstance;
            ddlDecive.DataTextField = "instance";
            ddlDecive.DataValueField = "RowGuid";
            ddlDecive.DataBind();
            if (InstrumentInstanceGuid != "")
            {
                ddlDecive.SelectedValue = InstrumentInstanceGuid;
            }
            DataTable dt = r_OM_TaskItemConfigService.GetList(HiddenMissionId.Value, "air", "2");
            string[] ItemNames = dt.AsEnumerable().Select(d => d.Field<string>("ItemName")).ToList().ToArray();
            IQueryable<V_CodeMainItemEntity> ItemNameEntites = dicService.RetrieveListByCodeNames(DictionaryType.AMS, ItemNames);
            DataView dtFitting = m_MaintenanceService.GetFitting(ddlDecive.SelectedValue.ToString());
            if (taskDt.DefaultView.Count != 0)
            {
                ViewState["BaseStatus"] = "edit";
                ViewState["BaseId"] = taskDt.Rows[0]["id"].ToString();
                dtpTime.SelectedDate = DateTime.Parse(taskDt.Rows[0]["FileUpLoadDate"].ToString());
                dtpBegin.SelectedDate = DateTime.Parse(taskDt.Rows[0]["ActionDate"].ToString());
                dtpEnd.SelectedDate = DateTime.Parse(taskDt.Rows[0]["FinishDate"].ToString());
                DataTable TaskItemData = r_OM_TaskItemDataService.GetList(ViewState["TaskCode"].ToString(), taskDt.Rows[0]["id"].ToString());
                DataView dvTaskItem = TaskItemData.DefaultView;
                string deviceGuid = "";
                foreach (Telerik.Web.UI.GridDataItem item in gridAroundInspect.Items)
                {
                    OM_TaskItemDatumEntity OM_TaskItemDatumEntity = new OM_TaskItemDatumEntity();
                    RadDateTimePicker rftpTime = (RadDateTimePicker)item.FindControl("dtpGrid");
                    RadTextBox rdtDescription = (RadTextBox)item.FindControl("rdtDescription");
                    RadDropDownList ddlGrid = (RadDropDownList)item.FindControl("ddlGrid");
                    if (item["ItemName"].Text != "更换零件名称")
                    {
                        IQueryable<V_CodeMainItemEntity> ItemNameEntitesNew = ItemNameEntites.Where(t => t.CodeName == item["ItemName"].Text);
                        ddlGrid.DataSource = ItemNameEntitesNew;
                        ddlGrid.DataTextField = "ItemText";
                        ddlGrid.DataValueField = "ItemValue";
                        ddlGrid.DataBind();
                        ddlGrid.Items.Insert(0, new DropDownListItem("", "00"));
                    }
                    else
                    {
                        ddlGrid.DataSource = dtFitting;
                        ddlGrid.DataTextField = "FittingName";
                        ddlGrid.DataValueField = "RowGuid";
                        ddlGrid.DataBind();
                        ddlGrid.Items.Insert(0, new DropDownListItem("", "00"));
                    }
                    string[] guid = item.KeyValues.Split('"');
                    if (guid.Length > 2)
                    {
                        dvTaskItem.RowFilter = "TaskItemGuid='" + guid[1] + "'";
                        if (dvTaskItem.Count > 0)
                        {
                            string DefaultValue = dvTaskItem[0]["ItemValue"].ToString();
                            if (DefaultValue != "")
                            {
                                ddlGrid.SelectedText = DefaultValue;
                            }
                            rdtDescription.Text = dvTaskItem[0]["MaintainceUser"].ToString();
                            if (dvTaskItem[0]["ItemRecordDate"] != DBNull.Value)
                            {
                                rftpTime.SelectedDate = DateTime.Parse(dvTaskItem[0]["ItemRecordDate"].ToString());
                            }
                            else
                            {
                                rftpTime.SelectedDate = DateTime.Now;
                            }
                            if (dvTaskItem[0]["UniversalValue1"] != DBNull.Value)
                            {
                                deviceGuid = dvTaskItem[0]["UniversalValue1"].ToString();
                            }
                        }
                    }
                }
                if (deviceGuid != "")
                {
                    ddlDecive.SelectedValue = deviceGuid;
                }
            }
            else
            {
                dtpTime.SelectedDate = DateTime.Now;
                dtpBegin.SelectedDate = DateTime.Now;
                dtpEnd.SelectedDate = DateTime.Now;
                DataView dv = dt.DefaultView;
                if (dt.Rows.Count > 0)
                {
                    foreach (Telerik.Web.UI.GridDataItem item in gridAroundInspect.Items)
                    {
                        RadDateTimePicker rftpTime = (RadDateTimePicker)item.FindControl("dtpGrid");
                        rftpTime.SelectedDate = DateTime.Now;
                        RadTextBox rdtDescription = (RadTextBox)item.FindControl("rdtDescription");
                        rdtDescription.Text = ViewState["UserName"].ToString();
                        RadDropDownList ddlGrid = (RadDropDownList)item.FindControl("ddlGrid");
                        IQueryable<V_CodeMainItemEntity> ItemNameEntitesNew = ItemNameEntites.Where(t => t.CodeName == item["ItemName"].Text);
                        ddlGrid.DataSource = ItemNameEntitesNew;
                        ddlGrid.DataTextField = "ItemText";
                        ddlGrid.DataValueField = "ItemValue";
                        ddlGrid.DataBind();
                        ddlGrid.Items.Insert(0, new DropDownListItem("", "00"));
                    }
                }
            }
            BindLabel();
        }
        /// <summary>
        /// 绑定仪器编号
        /// </summary>
        public void BindLabel()
        {
            string[] device = ddlDecive.SelectedText.Split('/');
            if (device.Length > 1)
            {
                deciveNumber.Text = device[device.Length - 1];
            }
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
                DataTable taskDt = m_WaterInspectionBaseService.GetList(ViewState["TaskCode"].ToString(), "");
                if (taskDt.Rows.Count > 0)
                {
                    List<OM_TaskItemDatumEntity> OM_TaskItemDatumEntityArry = new List<OM_TaskItemDatumEntity>();
                    foreach (Telerik.Web.UI.GridDataItem item in gridAroundInspect.Items)
                    {
                        OM_TaskItemDatumEntity OM_TaskItemDatumEntity = new OM_TaskItemDatumEntity();
                        RadDateTimePicker rftpTime = (RadDateTimePicker)item.FindControl("dtpGrid");
                        RadTextBox rdtDescription = (RadTextBox)item.FindControl("rdtDescription");
                        RadDropDownList ddlGrid = (RadDropDownList)item.FindControl("ddlGrid");
                        OM_TaskItemDatumEntity.TaskGuid = int.Parse(taskDt.Rows[0]["id"].ToString());//任务id
                        OM_TaskItemDatumEntity.TaskCode = ViewState["TaskCode"].ToString();//任务编号
                        string[] guid = item.KeyValues.Split('"');
                        if (guid.Length > 2)
                        {
                            OM_TaskItemDatumEntity.TaskItemGuid = new Guid(guid[1]);//检测项目GUID
                        }
                        OM_TaskItemDatumEntity.ItemValue = ddlGrid.SelectedText;//内容
                        if (rftpTime.SelectedDate != null)
                        {
                            OM_TaskItemDatumEntity.ItemRecordDate = rftpTime.SelectedDate.Value;//任务项记录时间
                        }
                        else
                        {
                            OM_TaskItemDatumEntity.ItemRecordDate = DateTime.Now;
                        }
                        OM_TaskItemDatumEntity.MaintainceUser = rdtDescription.Text;//确认人
                        OM_TaskItemDatumEntity.UniversalValue1 = ddlDecive.SelectedValue;//仪器GUID
                        string[] device = ddlDecive.SelectedText.Split('/');
                        if (device.Length > 1)
                        {
                            string deciveName = "";
                            for (int i = 0; i < device.Length - 1; i++)
                            {
                                deciveName += device[i] + "/";
                            }
                            deciveName = deciveName.TrimEnd('/');
                            OM_TaskItemDatumEntity.UniversalValue2 = deciveName;//仪器名称
                            OM_TaskItemDatumEntity.UniversalValue3 = device[device.Length - 1];//仪器编号
                        }
                        OM_TaskItemDatumEntityArry.Add(OM_TaskItemDatumEntity);
                    }
                    r_OM_TaskItemDataService.insertTable(OM_TaskItemDatumEntityArry.ToArray(), int.Parse(taskDt.Rows[0]["id"].ToString()), ViewState["TaskCode"].ToString());
                }
            }
        }

        protected void gridAroundInspect_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = r_OM_TaskItemConfigService.GetList(HiddenMissionId.Value, "air", "2");
            gridAroundInspect.DataSource = dt;
            gridAroundInspect.VirtualItemCount = dt.Rows.Count;
        }

        protected void gridAroundInspect_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void ddlDecive_ItemSelected(object sender, DropDownListEventArgs e)
        {
            BindLabel();
            gridAroundInspect.Rebind();
            BindGrid();
        }
    }
}