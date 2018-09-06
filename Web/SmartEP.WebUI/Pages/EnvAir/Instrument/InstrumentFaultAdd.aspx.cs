using SmartEP.DomainModel;
using SmartEP.DomainModel.Framework;
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

namespace SmartEP.WebUI.Pages.EnvAir.Instrument
{
    /// <summary>
    /// 名称：InstrumentFaultAdd.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-05-24
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：仪器故障表单新增编辑
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class InstrumentFaultAdd : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理
        /// </summary>
        InstrumentFaultService m_InstrumentFault = new InstrumentFaultService();
        QualityControlDataSearchService dataSearchService = new QualityControlDataSearchService();
        private string m_OperationSubmitWebServiceResourceInfoUrl = System.Configuration.ConfigurationManager.AppSettings["OperationSubmitWebServiceResourceInfoUrl"].ToString();
        private string m_OperationOMMPGetDataWebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["OperationOMMPGetDataWebServiceUrl"].ToString();
        string[] portIds = null;
        string portId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string Id = PageHelper.GetQueryString("RowGuid");
                this.ViewState["ObjectType"] = PageHelper.GetQueryString("ObjectType");
                if (this.ViewState["ObjectType"].ToString() == "2")
                {
                    pointCbxRsm.Visible = true;
                }
                else if (this.ViewState["ObjectType"].ToString() == "1")
                {
                    pointCbxRsm1.Visible = true;
                }
                if (!string.IsNullOrEmpty(Id))
                {
                    InstrumentInstanceRecord2Entity entity = m_InstrumentFault.GetModelRecord2(Convert.ToInt32(Id));
                    FrameworkModel userModel = new FrameworkModel();
                    IList<Frame_UserEntity> userObjList = userModel.Frame_UserEntities
                                    .Where(x => x.IsEnabled != (Nullable<int>)null && x.IsEnabled.Value == 1).ToList();
                    UsedUser.DataSource = userObjList;
                    UsedUser.DataTextField = "DisplayName";
                    UsedUser.DataValueField = "RowGuid";
                    UsedUser.DataBind();
                    if (UsedUser.Items.Count > 0)
                    {
                        RadComboBoxItem radItem = UsedUser.FindItemByText(entity.OperateUserName);
                        if (radItem != null)
                        {
                            radItem.Selected = true;
                        }
                    }
                    RadComboBoxItem occurStatusItem = occurStatus.FindItemByValue(entity.OccurStatus);
                    if (occurStatusItem != null)
                    {
                        occurStatusItem.Selected = true;
                    }
                    RadComboBoxItem InstrumentName = occurStatus.FindItemByValue(entity.InstrumentInstanceGuid);
                    if (InstrumentName != null)
                    {
                        InstrumentName.Selected = true;
                    }
                    OccurTime.SelectedDate = entity.OperateDate;
                    note.Text = entity.Note;
                    OperateResult.Text = entity.OperateResult;
                    //this.ViewState["InstanceGuid"] = entity.InstanceGuid;
                    this.ViewState["RowGuid"] = entity.RowGuid;
                  //  InstrumentName.SelectedItem.Text = entity.InstrumentInstanceGuid;
                    //InstrumentInstanceRecord2Entity entityRecord = m_InstrumentFault.GetModelRecord2(entity.RowGuid);
                    //this.ViewState["Id"] = entityRecord.ID.ToString();
                    pointCbxRsm_SelectedChanged();
                }
                else
                {
                    InitControl();
                }
            }
        }
        protected void InitControl()
        {
            OccurTime.SelectedDate = DateTime.Now;
            FrameworkModel userModel = new FrameworkModel();
            IList<Frame_UserEntity> userObjList = userModel.Frame_UserEntities
                            .Where(x => x.IsEnabled != (Nullable<int>)null && x.IsEnabled.Value == 1).ToList();
            UsedUser.DataSource = userObjList;
            UsedUser.DataTextField = "DisplayName";
            UsedUser.DataValueField = "RowGuid";
            UsedUser.DataBind();
            if (UsedUser.Items.Count > 0)
            {
                UsedUser.SelectedValue = SessionHelper.Get("UserGuid");
            }
            pointCbxRsm_SelectedChanged();
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {

            string Id = PageHelper.GetQueryString("RowGuid");
            if (!string.IsNullOrEmpty(Id))
            {
                try
                {
                    //InstrumentInstanceTimeRecordEntity entity = new InstrumentInstanceTimeRecordEntity();
                    //entity.ID = Convert.ToInt32(Id);
                    //entity.RowGuid = this.ViewState["RowGuid"].ToString();
                    //entity.InstanceGuid = ViewState["InstanceGuid"].ToString();
                    //entity.OccurStatus = occurStatus.SelectedItem.Text;
                    //entity.OccurTime = Convert.ToDateTime(OccurTime.SelectedDate);
                    //entity.OperateUserName = UsedUser.SelectedItem.Text;
                    //entity.OperateUserGuid = UsedUser.SelectedItem.Value;
                    //entity.OperateContent = "维护停用";
                    //entity.TypeGuid = "a63fd9a4-cd7f-4807-9cf2-8ce31995e5e5";
                    //entity.Note = note.Text;
                    //entity.OperateResault = OperateResult.Text;
                    //m_InstrumentFault.Update(entity);
                    InstrumentInstanceRecord2Entity Recordentity = new InstrumentInstanceRecord2Entity();
                    Recordentity.ID = Convert.ToInt32(ViewState["Id"]);
                    Recordentity.RowGuid = this.ViewState["RowGuid"].ToString();
                    Recordentity.InstrumentInstanceGuid = InstrumentName.SelectedItem.Text;
                    Recordentity.OperateUserGuid = UsedUser.SelectedItem.Value;
                    Recordentity.OperateDate = Convert.ToDateTime(OccurTime.SelectedDate);
                    Recordentity.OperateUserName = UsedUser.SelectedItem.Text;
                    Recordentity.OperateContent = "维护停用";
                    Recordentity.Note = note.Text;
                    Recordentity.OccurStatus = occurStatus.SelectedItem.Text;
                    Recordentity.OperateResult = OperateResult.Text;
                    Recordentity.ObjectType = this.ViewState["ObjectType"].ToString();
                    m_InstrumentFault.UpdateRecord2(Recordentity);

                    Alert("更新成功！");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
                }
                catch
                {
                    Alert("更新失败！");
                }
            }
            else
            {
                try
                {
                    InstrumentInstanceTimeRecordEntity entity = new InstrumentInstanceTimeRecordEntity();
                    string guid = Guid.NewGuid().ToString();
                    //entity.RowGuid = guid;
                    //entity.InstanceGuid = InstrumentName.SelectedValue;
                    //entity.OccurStatus = occurStatus.SelectedItem.Text;
                    //entity.OccurTime = Convert.ToDateTime(OccurTime.SelectedDate);
                    //entity.OperateUserName = UsedUser.SelectedItem.Text;
                    //entity.OperateUserGuid = UsedUser.SelectedItem.Value;
                    //entity.OperateContent = "维护停用";
                    //entity.TypeGuid = "a63fd9a4-cd7f-4807-9cf2-8ce31995e5e5";
                    //entity.Note = note.Text;
                    //entity.OperateResault = OperateResult.Text;
                    //entity.PointId = Convert.ToInt32(pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID)[0]);
                    //m_InstrumentFault.Add(entity);
                    InstrumentInstanceRecord2Entity Recordentity = new InstrumentInstanceRecord2Entity();
                    Recordentity.RowGuid = guid;
                    Recordentity.InstrumentInstanceGuid = InstrumentName.SelectedItem.Text;
                    Recordentity.OperateUserGuid = UsedUser.SelectedItem.Value;
                    Recordentity.OperateDate = Convert.ToDateTime(OccurTime.SelectedDate);
                    Recordentity.OperateUserName = UsedUser.SelectedItem.Text;
                    Recordentity.OperateContent = "维护停用";
                    Recordentity.Note = note.Text;
                    Recordentity.OccurStatus = occurStatus.SelectedItem.Text;
                    Recordentity.OperateResult = OperateResult.Text;
                    Recordentity.ObjectType = this.ViewState["ObjectType"].ToString();
                    m_InstrumentFault.AddRecord2(Recordentity);
                    Alert("保存成功！");
                    string formUrl = System.Configuration.ConfigurationManager.AppSettings["FormUrl"].ToString(); ;

                    string JSONstring = "[";
                    JSONstring += "{";
                    JSONstring += "\"" + "FixedAssetNumber" + "\":\"" + InstrumentName.SelectedValue + "\",";
                    JSONstring += "\"" + "Date" + "\":\"" + Convert.ToDateTime(OccurTime.SelectedDate) + "\",";
                    JSONstring += "\"" + "statusguid" + "\":\"" + "ACC74DA5-0E8A-4056-BCBD-FCC983901365" + "\",";//停用
                    JSONstring += "\"" + "timetypeguid" + "\":\"" + "a63fd9a4-cd7f-4807-9cf2-8ce31995e5e5" + "\",";//维护停用
                    JSONstring += "\"" + "UserGuid" + "\":\"" + SessionHelper.Get("UserGuid") + "\",";
                    JSONstring += "\"" + "UserName" + "\":\"" + UsedUser.SelectedItem.Text + "\",";
                    JSONstring += "\"" + "OperateContent" + "\":\"" + "维护停用" + "\",";
                    JSONstring += "\"" + "Note" + "\":\"" + "" + "\",";
                    JSONstring += "\"" + "FormUrl" + "\":\"" + formUrl + "ScrappedInstrumentEdit.aspx?Type=view&ScrappedInstrumentUid=" + guid + "\",";
                    JSONstring += "\"" + "changetype" + "\":\"" + "0" + "\",";
                    JSONstring += "\"" + "deviceguid" + "\":\"" + "" + "\",";
                    JSONstring += "\"" + "OrgDeviceGuid" + "\":\"" + "" + "\",";
                    JSONstring += "\"" + "OperateResault" + "\":\"" + "" + "\",";
                    JSONstring += "\"" + "IsReagent" + "\":\"" + "0" + "\",";
                    JSONstring += "\"" + "ReagentGuid" + "\":\"" + "" + "\"";
                    JSONstring += "}";
                    JSONstring += "]";
                    object taskInfo = WebServiceHelper.InvokeWebService(m_OperationSubmitWebServiceResourceInfoUrl, "SunmitWebServiceResourceInfo", "SubmitOperateRecord", new object[] { JSONstring });
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
                }
                catch
                {
                    Alert("保存失败！");
                }
            }
        }
        //protected void pointCbxRsm_SelectedChanged()
        //{
        //    InstrumentName.Items.Clear();
        //    portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.Guid);
        //    for (int i = 0; i < portIds.Length; i++)
        //    {
        //        portId = portId + portIds[i].ToString();
        //    }
        //    object taskInfo = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl, "TempGetDataWebService", "GetInsTypeByObjectID", new object[] { portId });
        //    DataTable dtTaskInfo = taskInfo as DataTable;
        //    if (dtTaskInfo.DefaultView.Count != 0)
        //    {
        //        InstrumentName.DataSource = dtTaskInfo;
        //        InstrumentName.DataTextField = dtTaskInfo.Columns["InstanceName"].ToString();
        //        InstrumentName.DataValueField = dtTaskInfo.Columns["FixedAssetNumber"].ToString();
        //        InstrumentName.DataBind();
        //    }
        //}
        private void LoadInstrument(string id)
        {
            object taskInfo = WebServiceHelper.InvokeWebService(m_OperationOMMPGetDataWebServiceUrl, "TempGetDataWebService", "GetInsTypeByObjectID", new object[] { portId });
            DataTable dtTaskInfo = taskInfo as DataTable;
            // DataTable dtTaskInfo = scrInstrumentService.GetInstrument(id);
            if (dtTaskInfo.DefaultView.Count != 0)
            {
                InstrumentName.DataSource = dtTaskInfo;
                InstrumentName.DataTextField = dtTaskInfo.Columns["InstanceName"].ToString();
                InstrumentName.DataValueField = dtTaskInfo.Columns["FixedAssetNumber"].ToString();

                InstrumentName.DataBind();
            }
            InstrumentName.Items.Insert(0, new RadComboBoxItem("", ""));

        }

        protected void pointCbxRsm_SelectedChanged()
        {
            if (this.ViewState["ObjectType"].ToString() == "1")
            {
                portIds = pointCbxRsm1.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.Guid);
            }
            else
            {
                portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.Guid);
            }
            for (int i = 0; i < portIds.Length; i++)
            {
                portId = portId + portIds[i].ToString();
            }
            LoadInstrument(portId);

        }
    }
}