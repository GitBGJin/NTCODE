using SmartEP.Core.Generic;
using SmartEP.Service.OperatingMaintenance.Air;
using SmartEP.Utilities.Office;
using SmartEP.Utilities.Web.UI;
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
    public partial class QueryByInstrument : SmartEP.WebUI.Common.BasePage
    {
        //服务处理
        MaintenanceService m_MaintenanceService = Singleton<MaintenanceService>.GetInstance();
        QualityControlDataSearchService dataSearchService = new QualityControlDataSearchService();//
        /// <summary>
        /// 运维任务项配置服务层
        /// </summary>
        OM_TaskItemConfigService r_OM_TaskItemConfigService = Singleton<OM_TaskItemConfigService>.GetInstance();
        public string _IsSpareParts;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _IsSpareParts = PageHelper.GetQueryString("systemType");
                BindTree();
                InitControl();//初始化控件

            }
        }
        #region 初始化控件
        private void InitControl()
        {
            string type = PageHelper.GetQueryString("systemType");//系统类型:1水2气
            if (type == "")
                type = "2";
            if (type == "2")
            {
                //数据类型
                radlDataType.Items.Add(new ListItem("使用信息", "0"));
                radlDataType.Items.Add(new ListItem("维护信息", "1"));
                radlDataType.Items.Add(new ListItem("维修信息", "2"));
                //radlDataType.Items.Add(new ListItem("仪器状态参数", "3"));
                //radlDataType.Items.Add(new ListItem("质控数据", "4"));
            }
            else if (type == "1")
            {
                radlDataType.Items.Add(new ListItem("使用信息", "0"));
                radlDataType.Items.Add(new ListItem("维修信息", "1"));
            }
            radlDataType.SelectedValue = "0";
            RadDatePickerBegin.SelectedDate = Request.QueryString["dtBegin"] != null ? Convert.ToDateTime(Request.QueryString["dtBegin"]) : new DateTime(DateTime.Now.Year, 1, 1); ;
            RadDatePickerEnd.SelectedDate = Request.QueryString["dtEnd"] != null ? Convert.ToDateTime(Request.QueryString["dtEnd"]) : DateTime.Now.Date.AddDays(-1);
        }
        #endregion

        protected void BindTree()
        {
            string type = PageHelper.GetQueryString("systemType");//系统类型:1水2气
            string IsSpareParts = PageHelper.GetQueryString("IsSpareParts");//是否备件:1是0否
            if (type == "")
                type = "2";
            DataView dvTree = dataSearchService.GetDataByObjectType(type);
            DataView dvAll = dataSearchService.GetInstanceDataByObjectType(type, IsSpareParts);

            string strParentNode = "";
            List<SiteDataItem> siteData = new List<SiteDataItem>();

            for (var i = 0; i < dvTree.Count; i++)
            {
                strParentNode = dvTree[i]["InstrumentType"].ToString();
                string guid = dvTree[i]["RowGuid"].ToString();
                //根节点
                siteData.Add(new SiteDataItem((i + 1), 0, strParentNode));

                dvAll.RowFilter = "Infoguid='" + guid + "'";
                for (var j = 0; j < dvAll.Count; j++)
                {
                    siteData.Add(new SiteDataItem(GetValue<int>(dvAll[j]["FixedAssetNumber"], 0), i + 1, dvAll[j]["InstrumentName"].ToString()));
                }
            }

            TreeView1.DataTextField = "Text";
            TreeView1.DataValueField = "ID";
            TreeView1.DataFieldID = "ID";
            TreeView1.DataFieldParentID = "ParentID";
            TreeView1.DataSource = siteData;
            TreeView1.DataBind();
            TreeView1.ExpandAllNodes();
        }
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
            private int _id;
            private int _parentId;

            public string Text
            {
                get { return _text; }
                set { _text = value; }
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

            public SiteDataItem(int id, int parentId, string text)
            {
                _id = id;
                _parentId = parentId;
                _text = text;
            }
        }

        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = PageHelper.GetQueryString("systemType");//系统类型:1水2气
            if (type == "")
                type = "2";
            if (type == "1")
            {
                if (radlDataType.SelectedIndex == 1)
                {
                    radlDataType.SelectedIndex = 0;
                    RegisterScript("MoveMaintain()");
                }
            }
        }

        protected void TreeView1_NodeCheck(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {

            ddlInstrument.Items.Clear();
            string name = "";
            foreach (RadTreeNode t in TreeView1.CheckedNodes)
            {
                if (t.ParentNode != null)
                {
                    if (t.Value != null && t.Value.Length > 0)
                    {
                        ddlInstrument.Items.Add(new RadComboBoxItem(t.Text, t.Value.ToString()));
                    }
                }
                else
                    name = t.Text;
            }
            for (int i = 0; i < ddlInstrument.Items.Count; i++)
                ddlInstrument.Items[i].Checked = true;
        }
        public void BindGrid()
        {
            DateTime dtBegin = RadDatePickerBegin.SelectedDate.Value;
            DateTime dtEnd = RadDatePickerEnd.SelectedDate.Value;
            DataTable dt = new DataTable();
            string rcbType = "";
            foreach (RadComboBoxItem item in ddlInstrument.CheckedItems)
            {
                string[] str = item.Text.ToString().Split('/');
                rcbType += str[1].ToString() + ","; ;
            }
            rcbType = rcbType.Trim(',').ToString();
            string[] name = rcbType.Split(',');
            //if(radlDataType.
            if (radlDataType.SelectedValue == "0")
            {
                dt = m_MaintenanceService.GetListByInstrument(name, dtBegin, dtEnd);
            }
            else if (radlDataType.SelectedValue == "2")
            {
                DataTable dtNew = r_OM_TaskItemConfigService.GetList("0762678A-5C7E-4C03-B8AC-95B2742ABA6C", "air", "2");
                Dictionary<string, string> itemConfig = new Dictionary<string, string>();
                List<string> taskLst = new List<string>();
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    itemConfig.Add(dtNew.Rows[i]["RowGuid"].ToString(), dtNew.Rows[i]["ItemName"].ToString());
                    taskLst.Add(dtNew.Rows[i]["RowGuid"].ToString());
                }
                string[] taskItem = taskLst.ToArray();
                dt = m_MaintenanceService.GetInstanceRepairByPoint(null, null, "0762678A-5C7E-4C03-B8AC-95B2742ABA6C", name, taskItem, dtBegin, dtEnd, itemConfig, true);
            }
            else if (radlDataType.SelectedValue == "1")
            {
                DataTable dtNew = r_OM_TaskItemConfigService.GetList("15CBC5CD-EC65-4108-8A66-E5A560605BE7", "air", "2");
                Dictionary<string, string> itemConfig = new Dictionary<string, string>();
                List<string> taskLst = new List<string>();
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    itemConfig.Add(dtNew.Rows[i]["RowGuid"].ToString(), dtNew.Rows[i]["ItemName"].ToString());
                    taskLst.Add(dtNew.Rows[i]["RowGuid"].ToString());
                }
                string[] taskItem = taskLst.ToArray();
                dt = m_MaintenanceService.GetInstancemaintainByPoint(null, null, "15CBC5CD-EC65-4108-8A66-E5A560605BE7", dtBegin, dtEnd, taskItem, name);
            }
            gridMaintenance.DataSource = dt;
            gridMaintenance.VirtualItemCount = dt.Rows.Count;
        }
        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            DateTime dtBegin = RadDatePickerBegin.SelectedDate.Value;
            DateTime dtEnd = RadDatePickerEnd.SelectedDate.Value;
            DataTable dt = new DataTable();
            string rcbType = "";
            foreach (RadComboBoxItem item in ddlInstrument.CheckedItems)
            {
                string[] str = item.Text.ToString().Split('/');
                rcbType += str[1].ToString() + ","; ;
            }
            rcbType = rcbType.Trim(',').ToString();
            string[] name = rcbType.Split(',');
            //if(radlDataType.
            if (radlDataType.SelectedValue == "0")
            {
                dt = m_MaintenanceService.GetListByInstrument(name, dtBegin, dtEnd);
                dt = UpdateExportColumnName(dt);
                ExcelHelper.DataTableToExcel(dt, "使用信息", "使用信息", this.Page);
            }
            else if (radlDataType.SelectedValue == "2")
            {
                DataTable dtNew = r_OM_TaskItemConfigService.GetList("0762678A-5C7E-4C03-B8AC-95B2742ABA6C", "air", "2");
                Dictionary<string, string> itemConfig = new Dictionary<string, string>();
                List<string> taskLst = new List<string>();
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    itemConfig.Add(dtNew.Rows[i]["RowGuid"].ToString(), dtNew.Rows[i]["ItemName"].ToString());
                    taskLst.Add(dtNew.Rows[i]["RowGuid"].ToString());
                }
                string[] taskItem = taskLst.ToArray();
                dt = m_MaintenanceService.GetInstanceRepairByPoint(null, null, "0762678A-5C7E-4C03-B8AC-95B2742ABA6C", name, taskItem, dtBegin, dtEnd, itemConfig, true);
                dt = UpdateExportColumnName(dt);
                ExcelHelper.DataTableToExcel(dt, "维修信息", "维修信息", this.Page);
            }
            else if (radlDataType.SelectedValue == "1")
            {
                DataTable dtNew = r_OM_TaskItemConfigService.GetList("15CBC5CD-EC65-4108-8A66-E5A560605BE7", "air", "2");
                Dictionary<string, string> itemConfig = new Dictionary<string, string>();
                List<string> taskLst = new List<string>();
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    itemConfig.Add(dtNew.Rows[i]["RowGuid"].ToString(), dtNew.Rows[i]["ItemName"].ToString());
                    taskLst.Add(dtNew.Rows[i]["RowGuid"].ToString());
                }
                string[] taskItem = taskLst.ToArray();
                dt = m_MaintenanceService.GetInstancemaintainByPoint(null, null, "15CBC5CD-EC65-4108-8A66-E5A560605BE7", dtBegin, dtEnd, taskItem, name);
                dt = UpdateExportColumnName(dt);
                ExcelHelper.DataTableToExcel(dt, "维护信息", "维护信息", this.Page);
            }
        }
        private DataTable UpdateExportColumnName(DataTable dt)
        {
            List<string> dcNames = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName == ("InstanceName"))
                {
                    dc.ColumnName = "仪器类型";

                }
                else if (dc.ColumnName == ("SpecificationModel"))
                {
                    dc.ColumnName = "仪器型号";

                }
                else if (dc.ColumnName == ("FixedAssetNumber"))
                {
                    dc.ColumnName = "仪器编号";

                }
                else if (dc.ColumnName == ("instance"))
                {
                    dc.ColumnName = "仪器名称/编号";

                }
                else if (dc.ColumnName == ("OperateDate"))
                {
                    dc.ColumnName = "时间";

                }
                else if (dc.ColumnName == ("State"))
                {
                    dc.ColumnName = "状态";

                }
                else if (dc.ColumnName == ("RoomName"))
                {
                    dc.ColumnName = "位置";

                }
                else if (dc.ColumnName == ("OperateUserName"))
                {
                    dc.ColumnName = "执行人";

                }
                else if (dc.ColumnName == ("OperateContent"))
                {
                    dc.ColumnName = "备注";

                }
                else if (dc.ColumnName == ("faultType"))
                {
                    dc.ColumnName = "故障现象";

                }
                else if (dc.ColumnName == ("faultPerson"))
                {
                    dc.ColumnName = "确认人";

                }
                else if (dc.ColumnName == ("faultTime"))
                {
                    dc.ColumnName = "确认时间";

                }

                else if (dc.ColumnName == ("reasonAndProcess"))
                {
                    dc.ColumnName = "故障原因和检修过程";

                }
                else if (dc.ColumnName == ("repairPerson"))
                {
                    dc.ColumnName = "检修人";

                }
                else if (dc.ColumnName == ("repairTime"))
                {
                    dc.ColumnName = "检修时间";

                }
                else if (dc.ColumnName == ("fittingName"))
                {
                    dc.ColumnName = "更换零件名称";

                }
                else if (dc.ColumnName == ("fittingPerson"))
                {
                    dc.ColumnName = "更换人";

                }
                else if (dc.ColumnName == ("fittingTime"))
                {
                    dc.ColumnName = "更换时间";

                }
                else if (dc.ColumnName == ("Tstamp"))
                {
                    dc.ColumnName = "时间";

                }
                else if (dc.ColumnName == ("AddFlow"))
                {
                    dc.ColumnName = "采样流量";

                }
                else if (dc.ColumnName == ("Pressure"))
                {
                    dc.ColumnName = "压力";

                }
                else if (dc.ColumnName == ("LampEnergy"))
                {
                    dc.ColumnName = "紫光灯能量";

                }
                else if (dc.ColumnName == ("SamplingFilm"))
                {
                    dc.ColumnName = "更换采样膜";

                }
                else if (dc.ColumnName == ("EffectiveLoad"))
                {
                    dc.ColumnName = "膜有效负荷";

                }
                else if (dc.ColumnName == ("Membrane"))
                {
                    dc.ColumnName = "更换滤镜";

                }
                else if (dc.ColumnName == ("AlarmInfo"))
                {
                    dc.ColumnName = "报警信息";

                }
                else
                {
                    dcNames.Add(dc.ColumnName);
                    //dt.Columns.Remove(dc.ColumnName);
                }
            }
            foreach (string dcName in dcNames)
            {
                dt.Columns.Remove(dcName);
            }
            return dt;
        }
        protected void gridMaintenance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridMaintenance_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }


        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridMaintenance.Rebind();
        }

        protected void gridMaintenance_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                if (col.DataField == "InstanceName")
                {
                    col.HeaderText = "仪器类型";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "SpecificationModel")
                {
                    col.HeaderText = "仪器型号";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "FixedAssetNumber")
                {
                    col.HeaderText = "仪器编号";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(160);
                    col.ItemStyle.Width = Unit.Pixel(160);
                }
                else if (col.DataField == "instance")
                {
                    col.HeaderText = "仪器名称/编号";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(160);
                    col.ItemStyle.Width = Unit.Pixel(160);
                }
                else if (col.DataField == "OperateDate")
                {
                    col.HeaderText = "时间";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "State")
                {
                    col.HeaderText = "状态";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "RoomName")
                {
                    col.HeaderText = "位置";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "OperateUserName")
                {
                    col.HeaderText = "执行人";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "OperateContent")
                {
                    col.HeaderText = "备注";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "faultType")
                {
                    col.HeaderText = "故障现象";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "faultPerson")
                {
                    col.HeaderText = "确认人";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "faultTime")
                {
                    col.HeaderText = "确认时间";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }

                else if (col.DataField == "reasonAndProcess")
                {
                    col.HeaderText = "故障原因和检修过程";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(170);
                    col.ItemStyle.Width = Unit.Pixel(170);
                }
                else if (col.DataField == "repairPerson")
                {
                    col.HeaderText = "检修人";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "repairTime")
                {
                    col.HeaderText = "检修时间";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "fittingName")
                {
                    col.HeaderText = "更换零件名称";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "fittingPerson")
                {
                    col.HeaderText = "检修人";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "fittingTime")
                {
                    col.HeaderText = "检修时间";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Tstamp")
                {
                    col.HeaderText = "时间";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "AddFlow")
                {
                    col.HeaderText = "采样流量";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Pressure")
                {
                    col.HeaderText = "压力";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "LampEnergy")
                {
                    col.HeaderText = "紫光灯能量";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "SamplingFilm")
                {
                    col.HeaderText = "更换采样膜";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "EffectiveLoad")
                {
                    col.HeaderText = "膜有效负荷";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Membrane")
                {
                    col.HeaderText = "更换滤镜";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "AlarmInfo")
                {
                    col.HeaderText = "报警信息";
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