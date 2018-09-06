using SmartEP.Core.Enums;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Alarm;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
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
    public partial class AlarmNotifySetting : BasePage
    {
        DictionaryService dicService = new DictionaryService();
        NotifyStrategyService notifyStrategyService = new NotifyStrategyService();
        NotifyNumbersService notifyNumberService = new NotifyNumbersService();
        private string applicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Air);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitControl();
            }
        }

        private void InitControl()
        {
            //报警类型
            IQueryable<V_CodeMainItemEntity> regionEntites = dicService.RetrieveList(DictionaryType.AMS, "报警类型");
            comboAlarmType.DataSource = regionEntites;
            comboAlarmType.DataTextField = "ItemText";
            comboAlarmType.DataValueField = "ItemGuid";
            comboAlarmType.DataBind();

            foreach (RadComboBoxItem item in comboAlarmType.Items)
            {
                item.Checked = true;
            }

            IQueryable<V_CodeMainItemEntity> notifyGradeEntites = dicService.RetrieveList(DictionaryType.AMS, "通知级别");
            notifyGradeList.DataSource = notifyGradeEntites;
            notifyGradeList.DataTextField = "ItemText";
            notifyGradeList.DataValueField = "ItemGuid";
            notifyGradeList.DataBind();

            foreach (RadComboBoxItem item in notifyGradeList.Items)
            {
                item.Checked = true;
            }
        }

        /// <summary>
        /// 获取报警类型
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string GetAlarmTypeName(object uid)
        {
            return dicService.GetTextByGuid(DictionaryType.AMS, "报警类型", uid.ToString());
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string GetEnableOrNot(object EnableOrNot)
        {
            if ((Boolean)EnableOrNot)
            {
                return "<font color='green'>启用</font>";
            }
            else
            {
                return "<font color='red'>停用</font>";
            }
        }

        /// <summary>
        /// 获取报警等级
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string GetGradeName(object uid)
        {
            return dicService.GetTextByGuid(DictionaryType.AMS, "通知级别", uid.ToString());
        }

        /// <summary>
        /// 获取站点名称字符串，以,隔开
        /// </summary>
        /// <param name="effectSubjects"></param>
        /// <returns></returns>
        public string GetPoints(object effectSubjects)
        {
            string pointName = "";
            MonitoringPointService pointService = new MonitoringPointService();
            pointName = pointService.RetrievePointNamesByPointUids(effectSubjects.ToString().Split(';'));
            if (pointName == "")
            {
                pointName = dicService.GetCodeDictionaryTextByValue(effectSubjects.ToString());
            }
            return pointName;
        }

        /// <summary>
        /// 获取接受人名称字符串，以,隔开
        /// </summary>
        /// <param name="notifyNumberUids"></param>
        /// <returns></returns>
        public string GetReceiveUsers(object notifyNumberUids)
        {
            return SmartEP.Utilities.DataTypes.ExtensionMethods.StringExtensions.GetArrayStrNoEmpty(notifyNumberService.RetrieveNameArrayByUids(notifyNumberUids.ToString().Split(';')).ToList(), ";");
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
            IQueryable<NotifyStrategyEntity> DataLists = notifyStrategyService.Retrieve(p => p.ApplicationUid == applicationValue && p.EnableOrNot == (rbtEnableOrNot.SelectedValue == "1" ? true : false));
            if (notifyGradeList.CheckedItems.Count > 0)
            {
                List<string> notifyGradeLists = notifyGradeList.CheckedItems.Select(x => x.Value).ToList<string>();
                DataLists = DataLists.Where(p => notifyGradeLists.Contains(p.NotifyGradeUid));
            }
            if (comboAlarmType.CheckedItems.Count > 0)
            {
                List<string> comboAlarmTypes = comboAlarmType.CheckedItems.Select(x => x.Value).ToList<string>();
                DataLists = DataLists.Where(p => comboAlarmTypes.Contains(p.AlarmEventUid));
            }
            if (!string.IsNullOrEmpty(txtStrategyName.Text.Trim()))
            {
                DataLists = DataLists.Where(p => p.NotifyStrategyName.Contains(txtStrategyName.Text.Trim()));
            }
            grid.DataSource = DataLists;
        }

        protected void grid_ItemCreated(object sender, GridItemEventArgs e)
        {
            ///初始化ToolBar
            switch (e.Item.ItemType)
            {
                case GridItemType.CommandItem:
                    GridCommandItem CmdItem = (GridCommandItem)(e.Item);
                    if (CmdItem != null)
                    {
                        RadToolBar RTB = (RadToolBar)(CmdItem.FindControl("gridRTB"));
                        InitRadToolBar(RTB);
                    }
                    break;
            }
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
                    ArrayList SelGuid = new ArrayList();
                    foreach (String item in radGrid.SelectedIndexes)
                    {
                        Index = Convert.ToInt32(item);
                        SelGuid.Add(radGrid.MasterTableView.DataKeyValues[Index]["NotifyStrategyUid"].ToString());
                    }
                    string[] SelGid = (string[])SelGuid.ToArray(typeof(string));
                    IQueryable<NotifyStrategyEntity> entities = notifyStrategyService.RetrieveListByUids(SelGid);
                    if (entities != null && entities.Count() > 0)
                    {
                        notifyStrategyService.BatchDelete(entities.ToList());
                        base.Alert("删除成功！");
                    }
                    break;
            }
        }

        /// <summary>
        /// 初始化ToolBar
        /// </summary>
        /// <param name="RTB"></param>
        private void InitRadToolBar(RadToolBar RTB)
        {
            RTB.Items[1].Visible = false;
            RTB.Items[7].Visible = false;
            RTB.Items[8].Visible = false;
            RTB.Items[11].Visible = false;
            RTB.Items[12].Visible = false;
            RTB.Items[13].Visible = false;
        }
    }
}