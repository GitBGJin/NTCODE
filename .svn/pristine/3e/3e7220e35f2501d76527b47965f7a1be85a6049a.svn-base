using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using SmartEP.Service.OperatingMaintenance.Air;
using SmartEP.Utilities.Office;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance
{
    public partial class MaintenanceQuery : SmartEP.WebUI.Common.BasePage
    {
        //服务处理
        MaintenanceService m_MaintenanceService = Singleton<MaintenanceService>.GetInstance();
        MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
        OM_TaskItemConfigService r_OM_TaskItemConfigService = Singleton<OM_TaskItemConfigService>.GetInstance();
        IQueryable<MonitoringPointEntity> EnableOrNotports = null;
        //代码项服务层
        DictionaryService dicService = new DictionaryService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTreeView();
                InitControl();
                //BindGrid();
                BindGridType();
            }
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //数据类型
            radlDataType.Items.Add(new ListItem("仪器检修记录", "0"));
            radlDataType.Items.Add(new ListItem("巡检维护记录", "1"));
            radlDataType.Items.Add(new ListItem("事件处理记录", "2"));
            radlDataType.SelectedValue = "1";
            dayBegin.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
            dayEnd.SelectedDate = DateTime.Now;
            IQueryable<V_CodeMainItemEntity> ItemNameEntites = dicService.RetrieveList(DictionaryType.AMS, "故障现象");
            faultTypes.DataSource = ItemNameEntites;
            faultTypes.DataTextField = "ItemText";
            faultTypes.DataValueField = "ItemValue";
            faultTypes.DataBind();
            DataTable dtTask = r_OM_TaskItemConfigService.GetListTask("", "").Select("MissionName like '%巡检%'").CopyToDataTable();
            rdlTaskType.DataValueField = "MissionID";
            rdlTaskType.DataTextField = "MissionName";
            rdlTaskType.DataSource = dtTask;
            rdlTaskType.DataBind();
            BindTaskItem();
            DataTable dtTaskItem = r_OM_TaskItemConfigService.GetList("C0EAF3A5-8DF4-479D-8CEC-EF9E342F8312", "air", "2");
            rcbWork.DataValueField = "RowGuid";
            rcbWork.DataTextField = "ItemName";
            rcbWork.DataSource = dtTaskItem;
            rcbWork.DataBind();
        }
        public void BindTaskItem()
        {
            string MissionId = rdlTaskType.SelectedValue;
            DataTable dtTaskItem = r_OM_TaskItemConfigService.GetList(MissionId, "air", "2");
            rcbCarry.DataValueField = "RowGuid";
            rcbCarry.DataTextField = "ItemName";
            rcbCarry.DataSource = dtTaskItem;
            rcbCarry.DataBind();
        }
        public void BindTreeView()
        {
            EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "bdf0837a-eb59-4c4a-a05f-c774a17f3077").Select(p => p.MonitoringPointUid).ToArray();
            //string pointId = "1BE910A1-4FC0-4D3C-B74B-2A4154E9647C";
            DataView dvInstance = m_MaintenanceService.GetInstanceByGuid(EnableOrNotportsarry, "2");
            string strParentNode = "";
            string strParentValue = "";
            List<SiteDataItem> siteData = new List<SiteDataItem>();
            for (var i = 0; i < EnableOrNotportsarry.Length; i++)
            {
                strParentNode = EnableOrNotports.Where(p => p.MonitoringPointUid == EnableOrNotportsarry[i]).Select(p => p.MonitoringPointName).FirstOrDefault();
                strParentValue = EnableOrNotports.Where(p => p.MonitoringPointUid == EnableOrNotportsarry[i]).Select(p => p.PointId).FirstOrDefault().ToString();
                //根节点
                siteData.Add(new SiteDataItem(i + 1, 0, strParentValue, strParentNode));
                dvInstance.RowFilter = "ObjectID='" + EnableOrNotportsarry[i] + "'";
                for (var j = 0; j < dvInstance.Count; j++)
                {
                    siteData.Add(new SiteDataItem(GetValue<int>(dvInstance[j]["FixedAssetNumber"], 0), i + 1, dvInstance[j]["FixedAssetNumber"].ToString(), dvInstance[j]["instance"].ToString()));
                }
                //DataTable dtV = m_MaintenanceService.GetInstrument(EnableOrNotportsarry[i]);
                //for (var j = 0; j < dtV.Rows.Count; j++)
                //{
                //    siteData.Add(new SiteDataItem(GetValue<int>(dtV.Rows[j]["FixedAssetNumber"], 0), i + 1, dtV.Rows[j]["FixedAssetNumber"].ToString(), dtV.Rows[j]["InstanceName"].ToString()));
                //}
            }
            RadTreeView1.DataTextField = "Text";
            RadTreeView1.DataValueField = "Value";
            RadTreeView1.DataFieldID = "ID";
            RadTreeView1.DataFieldParentID = "ParentID";
            RadTreeView1.CheckChildNodes = true;
            RadTreeView1.MultipleSelect = true;
            RadTreeView1.DataSource = siteData;
            RadTreeView1.DataBind();
            RadTreeView1.ExpandAllNodes();
            //RadTreeView1.CheckAllNodes();
        }

        public void BindGrid()
        {
            try
            {
                EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
                DateTime dtBegin = dayBegin.SelectedDate.Value;
                DateTime dtEnd = dayEnd.SelectedDate.Value.Date.AddDays(+1).AddMilliseconds(-1);
                List<string> numberList = new List<string>();
                List<string> pointList = new List<string>();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (RadTreeNode tr in RadTreeView1.CheckedNodes)
                {
                    if (!dict.ContainsKey(tr.Value))
                    {
                        if (tr.ParentNode == null)
                        {
                            dict.Add(tr.Value, tr.Text);
                        }
                    }
                    if (tr.ParentNode != null)
                    {
                        numberList.Add(tr.Value);
                    }
                    else
                    {
                        pointList.Add(tr.Value);
                    }
                }
                string pointguid = "";
                string pointId = "";
                foreach (string m in dict.Values)
                {
                    pointguid += EnableOrNotports.Where(p => p.MonitoringPointName == m).Select(p => p.MonitoringPointUid).FirstOrDefault() + ",";
                    pointId += EnableOrNotports.Where(p => p.MonitoringPointName == m).Select(p => p.PointId).FirstOrDefault() + ",";
                }
                string[] pointGuids = null;
                string[] pointIds = null;
                if (pointguid != "")
                {
                    pointGuids = pointguid.TrimEnd(',').Split(',');
                }
                if (pointId != "")
                {
                    pointIds = pointId.TrimEnd(',').Split(',');
                }
                string[] FixedAssetNumbers = numberList.ToArray();
                string rcbfaultType = "";
                foreach (RadComboBoxItem item in faultTypes.CheckedItems)
                {
                    rcbfaultType += (item.Text.ToString() + ",");
                }
                string[] rcbfaultTypes = { };
                if (rcbfaultType != "")
                {
                    rcbfaultTypes = rcbfaultType.Trim(',').Split(',');
                }
                string strTaskType = rdlTaskType.SelectedValue;
                string rcbType = "";
                foreach (RadComboBoxItem item in rcbCarry.CheckedItems)
                {
                    rcbType += (item.Value.ToString() + ",");
                }
                string[] rcbTypes = { };
                if (rcbType != "")
                {
                    rcbTypes = rcbType.Trim(',').Split(',');
                }
                string userName = rtbPerson.Text;
                bool istrue = IsStatistical.Checked;
                DataTable dt = new DataTable();
                if (radlDataType.SelectedValue == "0")
                {
                    dt = m_MaintenanceService.GetListDevice(pointIds, pointGuids, FixedAssetNumbers, dtBegin, dtEnd, "0762678A-5C7E-4C03-B8AC-95B2742ABA6C", new string[] { "5E6028D1-7E5F-4A36-8689-C9D8F30B3720" }, userName, istrue, rcbfaultTypes);
                }
                else if (radlDataType.SelectedValue == "1")
                {
                    dt = m_MaintenanceService.GetListInspect(pointIds, dtBegin, dtEnd, strTaskType, rcbTypes, userName, istrue);

                }
                else if (radlDataType.SelectedValue == "2")
                {
                    string rcbworkType = "";
                    foreach (RadComboBoxItem item in rcbWork.CheckedItems)
                    {
                        rcbworkType += (item.Value.ToString() + ",");
                    }
                    string[] rcbworkTypes = { };
                    if (rcbworkType != "")
                    {
                        rcbworkTypes = rcbworkType.Trim(',').Split(',');
                    }
                    dt = m_MaintenanceService.GetListEvent(pointIds, dtBegin, dtEnd, "C0EAF3A5-8DF4-479D-8CEC-EF9E342F8312", rcbworkTypes, userName, istrue);
                }

                gridMaintenance.DataSource = dt;
                gridMaintenance.VirtualItemCount = dt.Rows.Count;
            }
            catch (Exception ex)
            {
            }

        }

        #region  递归获得父节点信息
        /// <summary>
        /// 递归获得父节点信息
        /// </summary>
        /// <param name="dict">键值对</param>
        /// <param name="tr"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParentNode(Dictionary<string, string> dict, RadTreeNode tr)
        {
            if (tr.ParentNode == null)
            {
                return dict;
            }
            else
            {
                if (!dict.ContainsKey(tr.ParentNode.Value))
                {
                    dict.Add(tr.ParentNode.Value, tr.ParentNode.ParentNode == null ? "0" : tr.ParentNode.ParentNode.Text);
                }
                GetParentNode(dict, tr.ParentNode);
            }
            return dict;
        }
        #endregion
        private string GetStringValue(object source, string defaultValue)
        {
            return (source != null) ? source.ToString() : defaultValue;
        }
        private TValue Parse<TValue>(object source, TValue defaultValue)
        {
            string value = this.GetStringValue(source, null);

            TValue result = defaultValue;
            if (!string.IsNullOrEmpty(value))
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(TValue));
                try
                {
                    result = (TValue)typeConverter.ConvertFrom(value);
                }
                catch { }
            }
            return result;
        }
        private TValue GetValue<TValue>(object source, TValue defaultValue)
        {
            if (source == null)
            {
                return defaultValue;
            }
            string valueToConvert = source.ToString();
            return this.Parse(valueToConvert, defaultValue);
        }

        internal class SiteDataItem
        {
            private string _text;
            private string _value;
            private int _id;
            private int _parentId;

            public string Text
            {
                get { return _text; }
                set { _text = value; }
            }

            public string Value
            {
                get { return _value; }
                set { _value = value; }
            }
            public int ID
            {
                get { return _id; }
                set { _id = value; }
            }

            public int ParentID
            {
                get { return _parentId; }
                set { _parentId = value; }
            }

            public SiteDataItem(int id, int parentId, string value, string text)
            {
                _id = id;
                _parentId = parentId;
                _text = text;
                _value = value;
            }
        }

        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridType();
        }
        public void BindGridType()
        {
            dvDevice.Visible = false;
            dvInspect.Visible = false;
            dvEvent.Visible = false;
            if (radlDataType.SelectedValue == "0")
            {
                dvDevice.Visible = true;
                dvInspect.Visible = false;
                dvEvent.Visible = false;
            }
            else if (radlDataType.SelectedValue == "1")
            {
                dvDevice.Visible = false;
                dvInspect.Visible = true;
                dvEvent.Visible = false;
            }
            else
            {
                dvDevice.Visible = false;
                dvInspect.Visible = false;
                dvEvent.Visible = true;
            }

        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridMaintenance.Rebind();
        }

        protected void gridMaintenance_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridMaintenance_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (radlDataType.SelectedValue == "1")
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    if (item["item"] != null)
                    {
                        GridTableCell itemCell = (GridTableCell)item["item"];
                        itemCell.Text = rcbCarry.Items.Where(t => t.Value == itemCell.Text).Select(x => x.Text).FirstOrDefault();
                    }
                }
            }
        }

        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (RadTreeNode tr in RadTreeView1.CheckedNodes)
                {
                    //dict = GetParentNode(dict, tr);
                    if (!dict.ContainsKey(tr.Value))
                    {
                        if (tr.ParentNode == null)
                        {
                            dict.Add(tr.Value, tr.Text);
                        }
                    }
                }
                string pointId = "";
                foreach (string m in dict.Values)
                {
                    pointId += EnableOrNotports.Where(p => p.MonitoringPointName == m).Select(p => p.PointId).FirstOrDefault() + ",";
                }
                string[] pointIds = null;
                if (pointId != "")
                {
                    pointIds = pointId.TrimEnd(',').Split(',');
                }
                DateTime dtBegin = dayBegin.SelectedDate.Value;
                DateTime dtEnd = dayEnd.SelectedDate.Value.Date.AddDays(+1).AddMilliseconds(-1);
                string strTaskType = rdlTaskType.SelectedValue;
                string rcbType = "";
                foreach (RadComboBoxItem item in rcbCarry.CheckedItems)
                {
                    rcbType += (item.Value.ToString() + ",");
                }
                string[] rcbTypes = { };
                if (rcbType != "")
                {
                    rcbTypes = rcbType.Trim(',').Split(',');
                }
                string userName = rtbPerson.Text;
                bool istrue = IsStatistical.Checked;
                DataTable dt = new DataTable();
                if (pointIds != null)
                {
                    dt = m_MaintenanceService.GetListInspect(pointIds, dtBegin, dtEnd, strTaskType, rcbTypes, userName, istrue);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["item"] != DBNull.Value)
                    {
                        dt.Rows[i]["item"] = rcbCarry.Items.Where(t => t.Value == dt.Rows[i]["item"].ToString()).Select(x => x.Text).FirstOrDefault();
                    }
                }
                dt = UpdateExportColumnName(dt, istrue);
                ExcelHelper.DataTableToExcel(dt, "巡检维护记录", "巡检维护记录", this.Page);
            }
        }
        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <param name="dvStatistical">合计行数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataTable dt, bool istrue)
        {
            DataTable dtNew = dt;
            if (dtNew.Columns.Contains("pointName"))
            {
                dtNew.Columns["pointName"].SetOrdinal(0);
                dtNew.Columns["pointName"].ColumnName = "测点";
            }
            if (dtNew.Columns.Contains("timeRange"))
            {
                dtNew.Columns["timeRange"].SetOrdinal(1);
                dtNew.Columns["timeRange"].ColumnName = "巡检时间范围";
            }
            if (dtNew.Columns.Contains("itemType"))
            {
                dtNew.Columns["itemType"].SetOrdinal(2);
                dtNew.Columns["itemType"].ColumnName = "项目分类";
            }
            if (dtNew.Columns.Contains("item"))
            {
                dtNew.Columns["item"].SetOrdinal(3);
                dtNew.Columns["item"].ColumnName = "执行项目";
            }
            if (dtNew.Columns.Contains("itemCount"))
            {
                dtNew.Columns["itemCount"].SetOrdinal(4);
                dtNew.Columns["itemCount"].ColumnName = "执行次数";
            }
            if (dtNew.Columns.Contains("itemDate"))
            {
                dtNew.Columns["itemDate"].SetOrdinal(5);
                dtNew.Columns["itemDate"].ColumnName = "执行时间";
            }
            if (dtNew.Columns.Contains("Description"))
            {
                dtNew.Columns["Description"].SetOrdinal(6);
                dtNew.Columns["Description"].ColumnName = "备注";
            }
            if (dtNew.Columns.Contains("ActionUserName"))
            {
                dtNew.Columns["ActionUserName"].SetOrdinal(7);
                dtNew.Columns["ActionUserName"].ColumnName = "巡检人";
            }
            if (istrue)
            {
                if (dt.Columns.Contains("执行时间"))
                {
                    dt.Columns.Remove("执行时间");
                }
                if (dt.Columns.Contains("备注"))
                {
                    dt.Columns.Remove("备注");
                }
                if (dt.Columns.Contains("巡检人"))
                {
                    dt.Columns.Remove("巡检人");
                }
            }
            else
            {
                if (dt.Columns.Contains("执行次数"))
                {
                    dt.Columns.Remove("执行次数");
                }
            }

            return dtNew;
        }
        protected void rdlTaskType_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            BindTaskItem();
        }

        protected void RadTreeView1_NodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            dayEnd.SelectedDate = DateTime.Now.AddDays(-1);
        }

        protected void gridMaintenance_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                bool istrue = IsStatistical.Checked;
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                if (col.DataField == "pointName")
                {
                    col.HeaderText = "测点";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "timeRange")
                {
                    col.HeaderText = "巡检时间范围";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "itemType")
                {
                    col.HeaderText = "项目分类";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "item")
                {
                    col.HeaderText = "执行项目";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "itemCount")
                {
                    if (istrue)
                    {
                        e.Column.Visible = true;
                        col.HeaderText = "执行次数";
                        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.HeaderStyle.Width = Unit.Pixel(110);
                        col.ItemStyle.Width = Unit.Pixel(110);
                    }
                    else
                    {
                        e.Column.Visible = false;
                    }
                }
                else if (col.DataField == "itemDate")
                {
                    if (!istrue)
                    {
                        e.Column.Visible = true;
                        col.HeaderText = "执行时间";
                        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.HeaderStyle.Width = Unit.Pixel(110);
                        col.ItemStyle.Width = Unit.Pixel(110);
                    }
                    else
                    {
                        e.Column.Visible = false;
                    }
                }
                else if (col.DataField == "Description")
                {
                    if (!istrue)
                    {
                        e.Column.Visible = true;
                        col.HeaderText = "备注";
                        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.HeaderStyle.Width = Unit.Pixel(110);
                        col.ItemStyle.Width = Unit.Pixel(110);
                    }
                    else
                    {
                        e.Column.Visible = false;
                    }
                }
                else if (col.DataField == "ActionUserName")
                {
                    if (!istrue)
                    {
                        e.Column.Visible = true;
                        col.HeaderText = "处理人";
                        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.HeaderStyle.Width = Unit.Pixel(110);
                        col.ItemStyle.Width = Unit.Pixel(110);
                    }
                    else
                    {
                        e.Column.Visible = false;
                    }
                }
                else if (col.DataField == "InstanceName")
                {
                    col.HeaderText = "仪器名称";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "SpecificationModel")
                {
                    col.HeaderText = "仪器编号";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "FixedAssetNumber")
                {
                    col.HeaderText = "系统编号";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "faultType")
                {
                    e.Column.Visible = true;
                    col.HeaderText = "故障类型";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "dealDays")
                {
                    if (!istrue)
                    {
                        e.Column.Visible = true;
                        col.HeaderText = "处理天数";
                        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.HeaderStyle.Width = Unit.Pixel(110);
                        col.ItemStyle.Width = Unit.Pixel(110);
                    }
                    else
                    {
                        e.Column.Visible = false;
                    }
                }
                else if (col.DataField == "startTime")
                {
                    if (!istrue)
                    {
                        e.Column.Visible = true;
                        col.HeaderText = "维护开始时间";
                        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.HeaderStyle.Width = Unit.Pixel(110);
                        col.ItemStyle.Width = Unit.Pixel(110);
                    }
                    else
                    {
                        e.Column.Visible = false;
                    }
                }
                else if (col.DataField == "endTime")
                {
                    if (!istrue)
                    {
                        e.Column.Visible = true;
                        col.HeaderText = "维护结束时间";
                        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.HeaderStyle.Width = Unit.Pixel(110);
                        col.ItemStyle.Width = Unit.Pixel(110);
                    }
                    else
                    {
                        e.Column.Visible = false;
                    }
                }
                else if (col.DataField == "detail")
                {
                    if (!istrue)
                    {
                        e.Column.Visible = true;
                        col.HeaderText = "详情";
                        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.HeaderStyle.Width = Unit.Pixel(110);
                        col.ItemStyle.Width = Unit.Pixel(110);
                    }
                    else
                    {
                        e.Column.Visible = false;
                    }
                }
                else if (col.DataField == "eventType")
                {
                    col.HeaderText = "事件分类";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else
                {
                    e.Column.Visible = false;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}