using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Alarm;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Alarm
{
    public partial class AlarmNotify : BasePage
    {
        //字典服务
        DictionaryService dicService = new DictionaryService();
        //通知发送服务
        NotifySendService notifySendService = new NotifySendService();
        //应用程序Uid
        public string applicationUid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                applicationUid = PageHelper.GetQueryString("ApplicationUid");
                ViewState["ApplicationUid"] = applicationUid;
                InitControl();
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //通知类型
            IQueryable<V_CodeMainItemEntity> notifyTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "通知类型");
            comboNotifyType.DataSource = notifyTypeEntites;
            comboNotifyType.DataTextField = "ItemText";
            comboNotifyType.DataValueField = "ItemGuid";
            comboNotifyType.DataBind();
            comboNotifyType.Items.Insert(0, new RadComboBoxItem("", ""));

            dtpBegin.SelectedDate = DateTime.Now.AddDays(-2);
            dtpEnd.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// 初始化ToolBar
        /// </summary>
        /// <param name="RTB"></param>
        private void InitRadToolBar(RadToolBar RTB)
        {
            RTB.Items[2].Enabled = false;//新增
            RTB.Items[2].Visible = false;//新增
            //RTB.Items[3].Enabled = true;//编辑
            //RTB.Items[3].Visible = true;//编辑
            //RTB.Items[5].Enabled = true;//保存
            //RTB.Items[5].Visible = true;//保存
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grid.Rebind();
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            grid.DataSource = notifySendService.RetrieveList(ViewState["ApplicationUid"].ToString(), comboNotifyType.SelectedValue, txtContent.Text, txtReceiveUserName.Text, dtpBegin.SelectedDate.Value, dtpEnd.SelectedDate.Value, rbtnHandleOrNot.SelectedValue == "1" ? true : false);
            //数据分页的页数
            //grid.VirtualItemCount = pointList.Count();

        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_GridExporting(object sender, GridExportingArgs e)
        {
            //DataTable dataTable = g_OfflineBiz.GetGridDataAll(GetWhereString());
            //if (e.ExportType == ExportType.Excel)
            //{
            //    ExcelHelper.DataTableToExcel(dataTable, "离线配置", "离线配置", this.Page);
            //}
            //else if (e.ExportType == ExportType.Word)
            //{
            //    WordHelper.DataTableToWord(dataTable, "离线配置", this.Page);
            //}
        }

        /// <summary>
        /// ToolBar按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            RadGrid radGrid = ((RadGrid)sender);
            int Index = -1;
            switch (e.CommandName)
            {
                case "DeleteSelected":
                    #region DeleteSelected
                    ArrayList SelGuid = new ArrayList();
                    foreach (String item in radGrid.SelectedIndexes)
                    {
                        Index = Convert.ToInt32(item);
                        SelGuid.Add(radGrid.MasterTableView.DataKeyValues[Index]["NotifySendUid"].ToString());
                    }
                    string[] SelGid = (string[])SelGuid.ToArray(typeof(string));

                    IQueryable<NotifySendEntity> entities = notifySendService.RetrieveListByUids(SelGid);
                    if (entities != null && entities.Count() > 0)
                    {
                        notifySendService.BatchDelete(entities.ToList());
                        base.Alert("删除成功！");
                    }
                    #endregion
                    break;
            }
        }

        protected void grid_ItemCreated(object sender, GridItemEventArgs e)
        {
            ///初始化ToolBar
            switch (e.Item.ItemType)
            {
                case GridItemType.CommandItem:
                    #region CommandItem 初始化ToolBar
                    GridCommandItem CmdItem = (GridCommandItem)(e.Item);
                    if (CmdItem != null)
                    {
                        RadToolBar RTB = (RadToolBar)(CmdItem.FindControl("gridRTB"));
                        InitRadToolBar(RTB);
                    }
                    break;
                    #endregion
            }
        }

        //public string GetHandleOrNot(string handleOrNot)
        //{
        //    return handleOrNot.ToLower()=="true" ? "是" : "否";
        //}
    }
}