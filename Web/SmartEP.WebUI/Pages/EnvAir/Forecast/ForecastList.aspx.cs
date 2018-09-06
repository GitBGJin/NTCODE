using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.DataAnalyze.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Forecast
{
    /// <summary>
    /// 名称：ForecastList.aspx.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-08-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：空气质量预报
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class ForecastList : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        RPT_AQIForecastService m_AQIForecast = Singleton<RPT_AQIForecastService>.GetInstance();

        override protected void OnPreInit(EventArgs e)
        {
            Page.Theme = "";
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region 初始化
                ViewState["RTBCommandname"] = "";
                ViewState["LoginID"] = Session["LoginID"];
                //ViewState["UserGuid"] = Session["UserGuid"].ToString();
                ViewState["DisplayName"] = Session["Displayname"];
                RadDTBegin.SelectedDate = DateTime.Now.AddDays(-8);
                RadDTEnd.SelectedDate = DateTime.Now;
                #endregion
            }
        }

        private void InitRadToolBar(RadToolBar RTB)
        {
            RTB.LoadContentFile("ToolBar.xml");
            #region
            switch (ViewState["RTBCommandname"].ToString())
            {
                case "打印":
                    RTB.Items[0].Visible = false;//打印
                    break;
                case "增加":
                    RTB.Items[0].Visible = false;//打印
                    break;
                case "编辑":
                    #region 编辑
                    RTB.Items[0].Visible = false;//打印
                    RTB.Items[0].Visible = false;//打印
                    RTB.Items[1].Enabled = false;//
                    RTB.Items[2].Enabled = true;//增加
                    RTB.Items[3].Enabled = false;//编辑
                    RTB.Items[4].Enabled = false;//删除
                    RTB.Items[5].Enabled = true;//保存
                    RTB.Items[6].Enabled = true;//取消
                    RTB.Items[7].Enabled = true;//
                    RTB.Items[8].Enabled = false;//查询
                    RTB.Items[9].Enabled = false;//刷新
                    break;
                    #endregion
                case "保存":
                case "取消":
                case "查询":
                    #region
                    RTB.Items[0].Visible = false;//打印
                    RTB.Items[0].Visible = false;//打印
                    RTB.Items[1].Enabled = false;//
                    RTB.Items[2].Enabled = true;//增加
                    RTB.Items[3].Enabled = true;//编辑
                    RTB.Items[4].Enabled = true;//删除
                    RTB.Items[5].Enabled = false;//保存
                    RTB.Items[6].Enabled = false;//取消
                    RTB.Items[7].Enabled = true;//
                    RTB.Items[8].Enabled = true;//查询
                    RTB.Items[9].Enabled = true;//刷新
                    break;
                    #endregion
                default:
                    RTB.Items[0].Visible = false;//打印
                    break;
            }
            #endregion
        }

        protected void RadGridRTB_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            RadToolBar RTB = (RadToolBar)sender;
            ViewState["RTBCommandname"] = e.Item.Text;
            switch (e.Item.Text)
            {
                case "打印":
                    RTB.Items[0].Visible = false;//打印
                    break;
                case "增加":
                    RTB.Items[0].Visible = false;//打印
                    break;
                case "编辑":
                    RTB.Items[0].Visible = false;//打印
                    break;
                case "保存":
                case "取消":
                    RTB.Items[0].Visible = false;//打印
                    break;
                case "查询":
                    RTB.Items[0].Visible = false;//打印
                    RadGrid1.Rebind();
                    break;
                default:
                    RTB.Items[0].Visible = false;//打印
                    break;
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid RG = (RadGrid)sender;
            String SqlWhere = "";

            if (RadDTBegin.SelectedDate != null) { SqlWhere += " AND (IssuedTime>='" + RadDTBegin.SelectedDate.Value.ToString() + "')"; }
            if (RadDTEnd.SelectedDate != null) { SqlWhere += " AND (IssuedTime<'" + RadDTEnd.SelectedDate.Value.ToString() + "')"; }

            DataView dv = m_AQIForecast.GetList(SqlWhere).DefaultView;
            RG.DataSource = dv;
            RG.VirtualItemCount = dv.Count;
        }

        protected void RadGrid1_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                #region
                #endregion
            }
            catch (Exception Err)
            {
                Session["SelfErrInfo"] = "因子配置->格式化表格列时发生异常！";
                Session["SysErrInfo"] = Err.Message;
            }
        }

        protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case GridItemType.AlternatingItem:
                    break;
                case GridItemType.ColGroup:
                    break;
                case GridItemType.ColItem:
                    break;
                case GridItemType.CommandItem:
                    #region CommandItem 初始化ToolBar
                    GridCommandItem CmdItem = (GridCommandItem)(e.Item);
                    if (CmdItem != null)
                    {
                        RadToolBar RTB = (RadToolBar)(CmdItem.FindControl("RadGridRTB"));
                        InitRadToolBar(RTB);
                    }
                    break;
                    #endregion
                case GridItemType.EditFormItem:
                    break;
                case GridItemType.EditItem:
                    #region 编辑状态时的宽度
                    if (e.Item.IsInEditMode)
                    {
                        Control Ctrl11 = (((GridEditableItem)e.Item).EditManager.GetColumnEditor("AQIClassA").ContainerControl).Controls[1];
                        Control Ctrl12 = (((GridEditableItem)e.Item).EditManager.GetColumnEditor("AQIClassB").ContainerControl).Controls[1];
                        Control Ctrl13 = (((GridEditableItem)e.Item).EditManager.GetColumnEditor("AQIClassC").ContainerControl).Controls[1];

                        Control Ctrl14 = (((GridEditableItem)e.Item).EditManager.GetColumnEditor("PrimaryPollutantA").ContainerControl).Controls[1];
                        Control Ctrl15 = (((GridEditableItem)e.Item).EditManager.GetColumnEditor("PrimaryPollutantB").ContainerControl).Controls[1];
                        Control Ctrl16 = (((GridEditableItem)e.Item).EditManager.GetColumnEditor("PrimaryPollutantC").ContainerControl).Controls[1];

                        Control Ctrl21 = (((GridEditableItem)e.Item).EditManager.GetColumnEditor("AQIA").ContainerControl).Controls[0];
                        Control Ctrl22 = (((GridEditableItem)e.Item).EditManager.GetColumnEditor("AQIB").ContainerControl).Controls[0];
                        Control Ctrl23 = (((GridEditableItem)e.Item).EditManager.GetColumnEditor("AQIC").ContainerControl).Controls[0];

                        Control Ctrl41 = (((GridEditableItem)e.Item).EditManager.GetColumnEditor("Description").ContainerControl).Controls[0];
                        Control Ctrl42 = (((GridEditableItem)e.Item).EditManager.GetColumnEditor("IssuedUnit").ContainerControl).Controls[0];
                        Control Ctrl43 = (((GridEditableItem)e.Item).EditManager.GetColumnEditor("IssuedTime").ContainerControl).Controls[0];

                        ((RadComboBox)Ctrl11).Width = Unit.Percentage(100);
                        ((RadComboBox)Ctrl12).Width = Unit.Percentage(100);
                        ((RadComboBox)Ctrl13).Width = Unit.Percentage(100);
                        ((RadComboBox)Ctrl14).Width = Unit.Percentage(100);
                        ((RadComboBox)Ctrl15).Width = Unit.Percentage(100);
                        ((RadComboBox)Ctrl16).Width = Unit.Percentage(100);

                        ((TextBox)Ctrl21).Width = Unit.Percentage(100);
                        ((TextBox)Ctrl22).Width = Unit.Percentage(100);
                        ((TextBox)Ctrl23).Width = Unit.Percentage(100);

                        ((TextBox)Ctrl41).Width = Unit.Percentage(100);
                        ((TextBox)Ctrl42).Width = Unit.Percentage(100);
                        ((TextBox)Ctrl43).Width = Unit.Percentage(100);
                    }
                    break;
                    #endregion
                case GridItemType.FilteringItem:
                    break;
                case GridItemType.Footer:
                    break;
                case GridItemType.GroupFooter:
                    break;
                case GridItemType.GroupHeader:
                    break;
                case GridItemType.Header:
                    break;
                case GridItemType.Item:
                    break;
                case GridItemType.NestedView:
                    break;
                case GridItemType.NoRecordsItem:
                    break;
                case GridItemType.Pager:
                    break;
                case GridItemType.SelectedItem:
                    break;
                case GridItemType.Separator:
                    break;
                case GridItemType.StatusBar:
                    break;
                case GridItemType.TFoot:
                    break;
                case GridItemType.THead:
                    break;
                case GridItemType.Unknown:
                    break;
                default:
                    break;
            }
            #region 多表头

            #endregion
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            RadGrid myRadGrid = ((RadGrid)sender);
            switch (e.Item.ItemType)
            {
                case GridItemType.AlternatingItem:
                    break;
                case GridItemType.ColGroup:
                    break;
                case GridItemType.ColItem:
                    break;
                case GridItemType.CommandItem:
                    break;
                case GridItemType.EditFormItem:
                    break;
                case GridItemType.EditItem:
                    #region
                    GridEditableItem editItem = (GridEditableItem)e.Item;

                    #region 空气质量
                    String AqiClassA = myRadGrid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["AQIClassA"].ToString();
                    RadComboBox RadCBoxClassA = (RadComboBox)editItem.FindControl("RadCbxAQIClassA");
                    bindCbx(RadCBoxClassA, 0);
                    foreach (RadComboBoxItem radItem in RadCBoxClassA.Items)
                    { if (AqiClassA.IndexOf(radItem.Value) >= 0) radItem.Selected = true; }

                    String AqiClassB = myRadGrid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["AQIClassB"].ToString();
                    RadComboBox RadCBoxClassB = (RadComboBox)editItem.FindControl("RadCbxAQIClassB");
                    bindCbx(RadCBoxClassB, 0);
                    foreach (RadComboBoxItem radItem in RadCBoxClassB.Items)
                    { if (AqiClassB.IndexOf(radItem.Value) >= 0) radItem.Selected = true; }

                    String AqiClassC = myRadGrid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["AQIClassC"].ToString();
                    RadComboBox RadCBoxClassC = (RadComboBox)editItem.FindControl("RadCbxAQIClassC");
                    bindCbx(RadCBoxClassC, 0);
                    foreach (RadComboBoxItem radItem in RadCBoxClassC.Items)
                    { if (AqiClassC.IndexOf(radItem.Value) >= 0) radItem.Selected = true; }
                    #endregion

                    #region 首要污染物
                    String AqiPPA = myRadGrid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["PrimaryPollutantA"].ToString();
                    RadComboBox RadCbxPPA = (RadComboBox)editItem.FindControl("RadCbxPrimaryPollutantA");
                    bindCbx(RadCbxPPA, 1);
                    foreach (RadComboBoxItem radItem in RadCbxPPA.Items)
                    { if (AqiPPA.IndexOf(radItem.Value) >= 0) radItem.Selected = true; }

                    String AqiPPB = myRadGrid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["PrimaryPollutantB"].ToString();
                    RadComboBox RadCbxPPB = (RadComboBox)editItem.FindControl("RadCbxPrimaryPollutantB");
                    bindCbx(RadCbxPPB, 1);
                    foreach (RadComboBoxItem radItem in RadCbxPPB.Items)
                    { if (AqiPPB.IndexOf(radItem.Value) >= 0) radItem.Selected = true; }

                    String AqiPPC = myRadGrid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["PrimaryPollutantC"].ToString();
                    RadComboBox RadCbxPPC = (RadComboBox)editItem.FindControl("RadCbxPrimaryPollutantC");
                    bindCbx(RadCbxPPC, 1);
                    foreach (RadComboBoxItem radItem in RadCbxPPC.Items)
                    { if (AqiPPC.IndexOf(radItem.Value) >= 0) radItem.Selected = true; }
                    #endregion

                    #endregion
                    break;
                case GridItemType.FilteringItem:
                    break;
                case GridItemType.Footer:
                    break;
                case GridItemType.GroupFooter:
                    break;
                case GridItemType.GroupHeader:
                    break;
                case GridItemType.Header:
                    #region 表头换行
                    #endregion
                    #region 表头居中
                    e.Item.HorizontalAlign = HorizontalAlign.Center;
                    #endregion
                    break;
                case GridItemType.Item:
                    #region
                    #endregion
                    break;
                case GridItemType.NestedView:
                    break;
                case GridItemType.NoRecordsItem:
                    break;
                case GridItemType.Pager:
                    break;
                case GridItemType.SelectedItem:
                    break;
                case GridItemType.Separator:
                    break;
                case GridItemType.StatusBar:
                    break;
                case GridItemType.TFoot:
                    break;
                case GridItemType.THead:
                    break;
                case GridItemType.Unknown:
                    break;
                default:
                    break;
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            RadGrid myRadGrid = ((RadGrid)sender);
            int Index = -1;
            switch (e.CommandName)
            {
                case "InitInsert":
                    break;
                case "PerformInsert":
                    break;
                case "Edit":
                    break;
                case "EditSelected":
                    #region 记录编辑ID
                    ViewState["EditSelID"] = "";
                    foreach (String item in myRadGrid.SelectedIndexes)
                    {
                        ViewState["EditSelID"] = ViewState["EditSelID"].ToString() + "," + item;
                    }
                    #endregion
                    break;
                case "Update":
                    break;
                case "UpdateEdited":
                    #region UpdateEdited
                    try
                    {
                        RPT_AQIForecastEntity entity = new RPT_AQIForecastEntity();
                        String DT = DateTime.Now.ToShortDateString();
                        foreach (String item in myRadGrid.EditIndexes)
                        {
                            Index = Convert.ToInt32(item);
                            entity.ID = Convert.ToInt32(myRadGrid.MasterTableView.DataKeyValues[Index]["ID"]);
                            entity.AQITimeA = ((TextBox)(myRadGrid.MasterTableView.Items[Index].Cells[3].Controls[0])).Text;
                            entity.AQIClassA = ((RadComboBox)(myRadGrid.MasterTableView.Items[Index].Cells[4].Controls[1])).SelectedValue;
                            entity.PrimaryPollutantA = ((RadComboBox)(myRadGrid.MasterTableView.Items[Index].Cells[5].Controls[1])).SelectedValue;
                            entity.AQIA = ((TextBox)(myRadGrid.MasterTableView.Items[Index].Cells[6].Controls[0])).Text;

                            entity.AQITimeB = ((TextBox)(myRadGrid.MasterTableView.Items[Index].Cells[7].Controls[0])).Text;
                            entity.AQIClassB = ((RadComboBox)(myRadGrid.MasterTableView.Items[Index].Cells[8].Controls[1])).SelectedValue;
                            entity.PrimaryPollutantB = ((RadComboBox)(myRadGrid.MasterTableView.Items[Index].Cells[9].Controls[1])).SelectedValue;
                            entity.AQIB = ((TextBox)(myRadGrid.MasterTableView.Items[Index].Cells[10].Controls[0])).Text;

                            entity.AQITimeC = ((TextBox)(myRadGrid.MasterTableView.Items[Index].Cells[11].Controls[0])).Text;
                            entity.AQIClassC = ((RadComboBox)(myRadGrid.MasterTableView.Items[Index].Cells[12].Controls[1])).SelectedValue;
                            entity.PrimaryPollutantC = ((RadComboBox)(myRadGrid.MasterTableView.Items[Index].Cells[13].Controls[1])).SelectedValue;
                            entity.AQIC = ((TextBox)(myRadGrid.MasterTableView.Items[Index].Cells[14].Controls[0])).Text;

                            entity.Description = ((TextBox)(myRadGrid.MasterTableView.Items[Index].Cells[15].Controls[0])).Text;
                            entity.IssuedUnit = ((TextBox)(myRadGrid.MasterTableView.Items[Index].Cells[16].Controls[0])).Text;
                            entity.IssuedTime = Convert.ToDateTime(((TextBox)(myRadGrid.MasterTableView.Items[Index].Cells[17].Controls[0])).Text);
                            entity.IsIssued = Convert.ToBoolean(Convert.ToInt32(((CheckBox)(myRadGrid.MasterTableView.Items[Index].Cells[18].Controls[0])).Checked));

                            m_AQIForecast.Update(entity);
                        }
                        myRadGrid.Rebind();
                    }
                    catch (Exception)
                    { }
                    #endregion
                    break;
                case "Delete":
                    break;
                case "DeleteSelected":
                    #region DeleteSelected
                    String IDs = "";
                    foreach (String item in myRadGrid.SelectedIndexes)
                    {
                        Index = Convert.ToInt32(item);
                        IDs += "," + myRadGrid.MasterTableView.DataKeyValues[Index]["ID"].ToString();
                    }
                    IDs = IDs.Trim(',');
                    if (IDs != "") m_AQIForecast.Delete(IDs);


                    #endregion
                    break;
                case "Candel":
                    break;
                case "SaveAll":
                    #region 暂停使用 SaveAll
                    try
                    {
                        #region

                        #endregion
                        this.RadGrid1.Rebind();
                    }
                    catch (Exception)
                    { }
                    break;
                    #endregion
            }

        }

        private void bindCbx(RadComboBox RadCbx, int Type)
        {
            switch (Type)
            {
                case 0:
                    RadCbx.Items.Clear();
              RadCbx.Items.Add(new RadComboBoxItem("优", "优"));
                    RadCbx.Items.Add(new RadComboBoxItem("良", "良"));
                    RadCbx.Items.Add(new RadComboBoxItem("轻度污染", "轻度污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("中度污染", "中度污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("重度污染", "重度污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("严重污染", "严重污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("优~良", "优~良"));
                    RadCbx.Items.Add(new RadComboBoxItem("良~轻度污染", "良~轻度污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("轻度污染~中度污染", "轻度污染~中度污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("中度污染~重度污染", "中度污染~重度污染"));
                    RadCbx.Items.Add(new RadComboBoxItem("重度污染~严重污染", "重度污染~严重污染"));
                    RadCbx.DataBind();
                    break;
                default:
                    RadCbx.Items.Clear();
                    RadCbx.Items.Add(new RadComboBoxItem("--", "--"));
                    RadCbx.Items.Add(new RadComboBoxItem("SO2", "SO2"));
                    RadCbx.Items.Add(new RadComboBoxItem("NO2", "NO2"));
                    RadCbx.Items.Add(new RadComboBoxItem("PM10", "PM10"));
                    RadCbx.Items.Add(new RadComboBoxItem("CO", "CO"));
                    RadCbx.Items.Add(new RadComboBoxItem("O3", "O3"));
                    RadCbx.Items.Add(new RadComboBoxItem("PM2.5", "PM2.5"));
                    RadCbx.DataBind();
                    break;
            }
        }
    }
}