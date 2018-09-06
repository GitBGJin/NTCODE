using Aspose.Words;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.OperatingMaintenance;
using SmartEP.Service.OperatingMaintenance.Air;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance
{
    public partial class AroundInspectList : System.Web.UI.Page
    {
        WaterInspectionBaseService m_WaterInspectionBaseService = new WaterInspectionBaseService();
        OM_TaskItemDataService r_OM_TaskItemDataService = Singleton<OM_TaskItemDataService>.GetInstance();
        OM_TaskItemConfigService r_OM_TaskItemConfigService = Singleton<OM_TaskItemConfigService>.GetInstance();
        MonitoringPointAirService r_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["TaskCode"] = PageHelper.GetQueryString("TaskCode");
                ViewState["MissionId"] = PageHelper.GetQueryString("MissionId");
                ViewState["username"] = "";
                ViewState["pointname"] = "";
            }

        }
        protected void Page_Prerender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }
        public void BindGrid()
        {
            DataTable taskDt = m_WaterInspectionBaseService.GetList(ViewState["TaskCode"].ToString(), "");
            if (taskDt.DefaultView.Count != 0)
            {
                dtpTime.Text = taskDt.Rows[0]["FileUpLoadDate"].ToString();
                dtpBegin.Text = taskDt.Rows[0]["ActionDate"].ToString();
                dtpEnd.Text = taskDt.Rows[0]["FinishDate"].ToString();
                ViewState["username"] = taskDt.Rows[0]["ActionUserName"].ToString();
                MonitoringPointEntity pointEntity = r_MonitoringPointAirService.RetrieveEntityByID(int.Parse(taskDt.Rows[0]["PointId"] == DBNull.Value ? "0" : taskDt.Rows[0]["PointId"].ToString()));
                ViewState["pointname"] = pointEntity == null ? "" : pointEntity.MonitoringPointName;
                DataTable TaskItemData = r_OM_TaskItemDataService.GetList(ViewState["TaskCode"].ToString(), taskDt.Rows[0]["id"].ToString());
                DataView dvTaskItem = TaskItemData.DefaultView;
                foreach (Telerik.Web.UI.GridDataItem item in gridAroundInspect.Items)
                {
                    CheckBoxList cblYes = (CheckBoxList)item.FindControl("cblYes");
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
                            if (dvTaskItem[0]["ItemRecordDate"] != DBNull.Value)
                            {
                                item["maintenancedate"].Text = dvTaskItem[0]["ItemRecordDate"].ToString();
                            }
                            else
                            {
                                item["maintenancedate"].Text = "";
                            }
                            item["remark"].Text = dvTaskItem[0]["Remark"].ToString();
                        }
                        else
                        {
                            item["maintenancedate"].Text = "";
                            item["remark"].Text = "";
                        }
                    }
                }
            }

        }
        protected void gridAroundInspect_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = r_OM_TaskItemConfigService.GetList(ViewState["MissionId"].ToString(), "air", "2");
            DataView dv = dt.DefaultView;
            dv.RowFilter = "ItemLevel='2' and UpItem<>'请选择'";
            dv.Sort = "UpItem";
            gridAroundInspect.DataSource = dv;
            gridAroundInspect.VirtualItemCount = dv.Count;

        }

        protected void gridAroundInspect_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            string TempPath = Server.MapPath("../../../Files/TempFile/Word/" + "AroundInspect" + ".doc");
            Document doc = new Document(TempPath);
            string[] MergeName = doc.MailMerge.GetFieldNames();
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToMergeField("pointname");
            builder.Write(ViewState["pointname"].ToString());
            builder.MoveToMergeField("username");
            builder.Write(ViewState["username"].ToString());
            builder.MoveToMergeField("datetime1");
            builder.Write(dtpTime.Text);
            builder.MoveToMergeField("datetime2");
            builder.Write(dtpBegin.Text);
            builder.MoveToMergeField("datetime3");
            builder.Write(dtpEnd.Text);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (Telerik.Web.UI.GridDataItem item in gridAroundInspect.Items)
            {
                if (item["UpItem"].Text != "" && !item["ItemName"].Text.Contains("周边环境"))
                {
                    if (item["ItemName"].Text.Contains("是否") || item["ItemName"].Text.Contains("有无"))
                    {
                        if (!dic.ContainsKey(item["UpItem"].Text))
                        {
                            dic.Add(item["UpItem"].Text, item["remark"].Text);

                        }
                        else
                        {
                            dic[item["UpItem"].Text] = dic[item["UpItem"].Text] + ";" + item["remark"].Text;
                        }
                    }
                }
                CheckBoxList cblYes = (CheckBoxList)item.FindControl("cblYes");
                if (item["ItemName"].Text.Contains("周边环境"))
                {
                    if (cblYes.Items[0].Selected)
                    {
                        if (MergeName.Contains("外部环境"))
                        {
                            builder.MoveToMergeField("外部环境");
                            builder.InsertComboBox("外部环境", new string[] { "有", "没有" }, 0);
                        }
                        if (MergeName.Contains("外部环境备注"))
                        {
                            builder.MoveToMergeField("外部环境备注");
                            builder.Write(item["remark"].Text);
                        }
                    }
                    else
                    {
                        if (MergeName.Contains("外部环境"))
                        {
                            builder.MoveToMergeField("外部环境");
                            builder.InsertComboBox("外部环境", new string[] { "有", "没有" }, 1);
                        }
                    }
                }
                else if (item["ItemName"].Text.Contains("是否") || item["ItemName"].Text.Contains("有无"))
                {
                    if (cblYes.Items[0].Selected)
                    {
                        if (MergeName.Contains(item["ItemName"].Text))
                        {
                            builder.MoveToMergeField(item["ItemName"].Text);
                            builder.Write("是");
                        }
                    }
                    else
                    {
                        if (MergeName.Contains(item["ItemName"].Text))
                        {
                            builder.MoveToMergeField(item["ItemName"].Text);
                            builder.Write("否");
                        }
                    }
                }
                else
                {
                    if (MergeName.Contains(item["ItemName"].Text))
                    {
                        builder.MoveToMergeField(item["ItemName"].Text);
                        builder.Write(item["remark"].Text);
                    }
                }
            }
            foreach (string item in dic.Keys)
            {
                if (MergeName.Contains(item))
                {
                    builder.MoveToMergeField(item);
                    builder.Write(dic[item]);
                }
            }
            doc.MailMerge.DeleteFields();
            doc.Save(this.Response, "日常巡检" + ".doc", ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }
    }
}