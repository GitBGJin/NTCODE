using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
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
    public partial class QueryByPoint : SmartEP.WebUI.Common.BasePage
    {
        public string type;
        public string isSpareParts;

        //服务处理
        MaintenanceService m_MaintenanceService = Singleton<MaintenanceService>.GetInstance();
        MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
        MonitoringPointWaterService m_MonitoringPointWaterService = Singleton<MonitoringPointWaterService>.GetInstance();
        /// <summary>
        /// 运维任务项配置服务层
        /// </summary>
        OM_TaskItemConfigService r_OM_TaskItemConfigService = Singleton<OM_TaskItemConfigService>.GetInstance();
        IQueryable<MonitoringPointEntity> EnableOrNotports = null;
        public string _IsSpareParts;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _IsSpareParts = PageHelper.GetQueryString("systemType");
                BindTreeView();
                InitControl();
                //BindGrid();
            }
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            type = PageHelper.GetQueryString("systemType");//系统类型:1水2气

            if (type == "")
                type = "2";
            hdType.Value = type;
            isSpareParts = PageHelper.GetQueryString("IsSpareParts");//是否备件
            if (type == "2")
            {
                //数据类型
                if (isSpareParts == "0")
                {
                    radlDataType.Items.Add(new ListItem("出入记录查询", "0"));
                    radlDataType.Items.Add(new ListItem("维护记录查询", "1"));
                    radlDataType.Items.Add(new ListItem("维修记录查询", "2"));
                    radlDataType.Items.Add(new ListItem("当前仪器清单", "3"));
                    radlDataType.SelectedValue = "0";
                }
                else if (isSpareParts == "1")
                {
                    radlDataType.Items.Add(new ListItem("出入记录查询", "0"));
                    radlDataType.Items.Add(new ListItem("维护记录查询", "1"));
                    radlDataType.Items.Add(new ListItem("维修记录查询", "2"));
                    radlDataType.SelectedValue = "0";
                }
            }
            else if (type == "1")
            {
                if (isSpareParts == "0")
                {
                    radlDataType.Items.Add(new ListItem("当前仪器清单", "3"));
                    radlDataType.Items.Add(new ListItem("出入记录查询", "0"));
                    radlDataType.Items.Add(new ListItem("维修记录查询", "1"));
                    radlDataType.SelectedValue = "3";
                }
                else if (isSpareParts == "1")
                {
                    radlDataType.Items.Add(new ListItem("出入记录查询", "0"));
                    radlDataType.Items.Add(new ListItem("维修记录查询", "1"));
                    radlDataType.SelectedValue = "0";

                }
            }

            dayBegin.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
            dayEnd.SelectedDate = DateTime.Now;
        }

        public void BindGridMaintenance()
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
                bool istrue = IsStatistical.Checked;
                if (radlDataType.SelectedValue == "0")//出入记录查询
                {
                    if (!istrue)
                    {
                        DataView InstanceSite = m_MaintenanceService.GetInstanceSite(FixedAssetNumbers, dtBegin, dtEnd);
                        gridMaintenance.DataSource = InstanceSite;
                        gridMaintenance.VirtualItemCount = InstanceSite.Count;
                    }
                    else
                    {
                        DataView InstanceSiteByPoint = m_MaintenanceService.GetInstanceSiteByPoint(pointGuids, dtBegin, dtEnd);
                        gridMaintenance.DataSource = InstanceSiteByPoint;
                        gridMaintenance.VirtualItemCount = InstanceSiteByPoint.Count;
                    }
                }
                else if (radlDataType.SelectedValue == "2")
                {
                    DataTable dt = r_OM_TaskItemConfigService.GetList("0762678A-5C7E-4C03-B8AC-95B2742ABA6C", "air", "2");
                    Dictionary<string, string> itemConfig = new Dictionary<string, string>();
                    List<string> taskLst = new List<string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        itemConfig.Add(dt.Rows[i]["RowGuid"].ToString(), dt.Rows[i]["ItemName"].ToString());
                        taskLst.Add(dt.Rows[i]["RowGuid"].ToString());
                    }
                    string[] taskItem = taskLst.ToArray();

                    DataTable dtInstance = m_MaintenanceService.GetInstanceRepairByPoint(pointIds, pointGuids, "0762678A-5C7E-4C03-B8AC-95B2742ABA6C", FixedAssetNumbers, taskItem, dtBegin, dtEnd, itemConfig, istrue);
                    gridMaintenance.DataSource = dtInstance;
                    gridMaintenance.VirtualItemCount = dtInstance.Rows.Count;
                }
                else if (radlDataType.SelectedValue == "1")
                {
                    DataTable dt = r_OM_TaskItemConfigService.GetList("15CBC5CD-EC65-4108-8A66-E5A560605BE7", "air", "2");
                    Dictionary<string, string> itemConfig = new Dictionary<string, string>();
                    List<string> taskLst = new List<string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        itemConfig.Add(dt.Rows[i]["RowGuid"].ToString(), dt.Rows[i]["ItemName"].ToString());
                        taskLst.Add(dt.Rows[i]["RowGuid"].ToString());
                    }
                    string[] taskItem = taskLst.ToArray();

                    DataTable dtmaintain = m_MaintenanceService.GetInstancemaintainByPoint(pointIds, pointGuids, "15CBC5CD-EC65-4108-8A66-E5A560605BE7", dtBegin, dtEnd, taskItem, FixedAssetNumbers);
                    gridMaintenance.DataSource = dtmaintain;
                    gridMaintenance.VirtualItemCount = dtmaintain.Rows.Count;
                }
                else if (radlDataType.SelectedValue == "3")
                {
                    if (!istrue)
                    {
                        DataView InstanceSite = m_MaintenanceService.GetInstanceList(FixedAssetNumbers, dtBegin, dtEnd);
                        gridMaintenance.DataSource = InstanceSite;
                        gridMaintenance.VirtualItemCount = InstanceSite.Count;
                    }
                    else//曾用仪器查询
                    {
                        DataView InstanceSiteByPoint = m_MaintenanceService.GetInstanceSiteByPoint(pointGuids, dtBegin, dtEnd);
                        gridMaintenance.DataSource = InstanceSiteByPoint;
                        gridMaintenance.VirtualItemCount = InstanceSiteByPoint.Count;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //绑定树
        public void BindTreeView()
        {
            type = PageHelper.GetQueryString("systemType");//系统类型:1水2气
            string IsSpareParts = PageHelper.GetQueryString("IsSpareParts");//是否备件:1是0否
            if (type == "")
                type = "2";
            if (type == "2")
            {
                EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            }
            else if (type == "1")
            {
                EnableOrNotports = m_MonitoringPointWaterService.RetrieveWaterMPListByEnable();
            }

            string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.EnableOrNot == true).OrderBy(t => t.PointId).Select(p => p.MonitoringPointUid).ToArray();
            //string pointId = "1BE910A1-4FC0-4D3C-B74B-2A4154E9647C";
            if (EnableOrNotportsarry != null && EnableOrNotportsarry.Length > 0)
            {
                DataView dvInstance = m_MaintenanceService.GetInstanceByGuid(EnableOrNotportsarry, type, IsSpareParts);
                string strParentNode = "";
                string strParentValue = "";
                List<SiteDataItem> siteData = new List<SiteDataItem>();
                for (var i = 0; i < EnableOrNotportsarry.Length; i++)
                {
                    strParentNode = EnableOrNotports.Where(p => p.MonitoringPointUid == EnableOrNotportsarry[i]).Select(p => p.MonitoringPointName).FirstOrDefault();
                    strParentValue = EnableOrNotports.Where(p => p.MonitoringPointUid == EnableOrNotportsarry[i]).Select(p => p.MonitoringPointUid).FirstOrDefault();
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
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridMaintenance.Rebind();

            type = PageHelper.GetQueryString("systemType");//系统类型:1水2气
            if (type == "")
                type = "2";
                if (radlDataType.SelectedValue == "3")
                {
                    gridMaintenance.Columns[1].Visible = true;
                }
                else
                {
                    gridMaintenance.Columns[1].Visible = false;
                }

        }

        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            type = PageHelper.GetQueryString("systemType");//系统类型:1水2气
            if (type == "")
                type = "2";
            if (type == "1")
            {
                if (radlDataType.SelectedValue == "1")
                {
                    radlDataType.SelectedValue = "0";
                    RegisterScript("MoveMaintain()");
                }
            }
        }

        protected void gridMaintenance_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGridMaintenance();
        }


        protected void gridRTB_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                DataTable dtInstance = new DataTable();
                DateTime dtBegin = dayBegin.SelectedDate.Value;
                DateTime dtEnd = dayEnd.SelectedDate.Value.Date.AddDays(+1).AddMilliseconds(-1);
                List<string> numberList = new List<string>();
                List<string> pointList = new List<string>();
                foreach (RadTreeNode tr in RadTreeView1.CheckedNodes)
                {
                    if (tr.ParentNode != null)
                    {
                        numberList.Add(tr.Value);
                    }
                    else
                    {
                        pointList.Add(tr.Value);
                    }
                }

                string[] FixedAssetNumbers = numberList.ToArray();
                string[] pointIds = pointList.ToArray();
                bool istrue = IsStatistical.Checked;
                if (radlDataType.SelectedValue == "0")
                {
                    if (!istrue)
                    {
                        if (FixedAssetNumbers != null && FixedAssetNumbers.Length > 0)
                        {
                            DataView InstanceSite = m_MaintenanceService.GetInstanceSite(FixedAssetNumbers, dtBegin, dtEnd);
                            dtInstance = InstanceSite.Table;
                        }
                    }
                    else
                    {
                        if (pointIds != null && pointIds.Length > 0)
                        {
                            DataView InstanceSiteByPoint = m_MaintenanceService.GetInstanceSiteByPoint(pointIds, dtBegin, dtEnd);
                            dtInstance = InstanceSiteByPoint.Table;
                        }
                    }
                    dtInstance = UpdateExportColumnName(dtInstance, "0");
                    ExcelHelper.DataTableToExcel(dtInstance, "出入记录查询", "出入记录查询", this.Page);
                }
                else if (radlDataType.SelectedValue == "1")
                {
                    DataTable dt = r_OM_TaskItemConfigService.GetList("15CBC5CD-EC65-4108-8A66-E5A560605BE7", "air", "2");
                    Dictionary<string, string> itemConfig = new Dictionary<string, string>();
                    List<string> taskLst = new List<string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        itemConfig.Add(dt.Rows[i]["RowGuid"].ToString(), dt.Rows[i]["ItemName"].ToString());
                        taskLst.Add(dt.Rows[i]["RowGuid"].ToString());
                    }
                    string[] taskItem = taskLst.ToArray();

                    DataTable dtmaintain = m_MaintenanceService.GetInstancemaintainByPoint(pointIds, pointIds, "15CBC5CD-EC65-4108-8A66-E5A560605BE7", dtBegin, dtEnd, taskItem, FixedAssetNumbers);
                    dtInstance = UpdateExportColumnName(dtmaintain, "1");
                    ExcelHelper.DataTableToExcel(dtInstance, "维护记录查询", "维护记录查询", this.Page);
                }
                else if (radlDataType.SelectedValue == "2")
                {
                    DataTable dt = r_OM_TaskItemConfigService.GetList("0762678A-5C7E-4C03-B8AC-95B2742ABA6C", "air", "2");
                    Dictionary<string, string> itemConfig = new Dictionary<string, string>();
                    List<string> taskLst = new List<string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        itemConfig.Add(dt.Rows[i]["RowGuid"].ToString(), dt.Rows[i]["ItemName"].ToString());
                        taskLst.Add(dt.Rows[i]["RowGuid"].ToString());
                    }
                    string[] taskItem = taskLst.ToArray();

                    dtInstance = m_MaintenanceService.GetInstanceRepairByPoint(pointIds, pointIds, "0762678A-5C7E-4C03-B8AC-95B2742ABA6C", FixedAssetNumbers, taskItem, dtBegin, dtEnd, itemConfig, istrue);
                    dtInstance = UpdateExportColumnName(dtInstance, "2");
                    ExcelHelper.DataTableToExcel(dtInstance, "维修记录查询", "维修记录查询", this.Page);
                }
                else if (radlDataType.SelectedValue == "3")
                {
                    if (!istrue)
                    {
                        dtInstance = m_MaintenanceService.GetInstanceList(FixedAssetNumbers, dtBegin, dtEnd).ToTable();
                    }
                    else//曾用仪器查询
                    {
                        dtInstance = m_MaintenanceService.GetInstanceSiteByPoint(pointIds, dtBegin, dtEnd).ToTable();
                    }
                    dtInstance = UpdateExportColumnName(dtInstance, "0");
                    ExcelHelper.DataTableToExcel(dtInstance, "当前仪器清单", "当前仪器清单", this.Page);
                }
            }
        }
        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <param name="dvStatistical">合计行数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataTable dt, string type)
        {
            if (type == "0")
            {
                DataTable dtNew = dt;
                if (dtNew.Columns.Contains("ObjectName"))
                {
                    dtNew.Columns["ObjectName"].SetOrdinal(0);
                    dtNew.Columns["ObjectName"].ColumnName = "测点";
                }
                if (dtNew.Columns.Contains("InstanceName"))
                {
                    dtNew.Columns["InstanceName"].SetOrdinal(1);
                    dtNew.Columns["InstanceName"].ColumnName = "仪器类型";
                }
                if (dtNew.Columns.Contains("SpecificationModel"))
                {
                    dtNew.Columns["SpecificationModel"].SetOrdinal(2);
                    dtNew.Columns["SpecificationModel"].ColumnName = "仪器型号";
                }
                if (dtNew.Columns.Contains("FixedAssetNumber"))
                {
                    dtNew.Columns["FixedAssetNumber"].SetOrdinal(3);
                    dtNew.Columns["FixedAssetNumber"].ColumnName = "仪器编号";
                }
                if (dtNew.Columns.Contains("OperateDate"))
                {
                    dtNew.Columns["OperateDate"].SetOrdinal(4);
                    dtNew.Columns["OperateDate"].ColumnName = "时间";
                }
                if (dtNew.Columns.Contains("Status"))
                {
                    dtNew.Columns["Status"].SetOrdinal(5);
                    dtNew.Columns["Status"].ColumnName = "状态";
                }
                if (dtNew.Columns.Contains("Note"))
                {
                    dtNew.Columns["Note"].SetOrdinal(6);
                    dtNew.Columns["Note"].ColumnName = "备注";
                }
                if (dtNew.Columns.Contains("SiteGuid"))
                {
                    dtNew.Columns.Remove("SiteGuid");
                }
                if (dtNew.Columns.Contains("InstrumentInstanceGuid"))
                {
                    dtNew.Columns.Remove("InstrumentInstanceGuid");
                }
                dt = dtNew;
            }
            else
            {
                List<string> dcNames = new List<string>();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName == ("pointName"))
                    {
                        dc.ColumnName = "测点";

                    }
                    else if (dc.ColumnName == ("InstanceName"))
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
                    else if (dc.ColumnName == ("Status"))
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
            }
            return dt;
        }

        protected void gridMaintenance_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                bool istrue = IsStatistical.Checked;
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                if (col.DataField == "ObjectName")
                {
                    col.HeaderText = "测点";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "pointName")
                {
                    col.HeaderText = "测点";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "InstanceName")
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
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
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
                else if (col.DataField == "Status")
                {
                    if (radlDataType.SelectedIndex == 1)
                    {
                        col.HeaderText = "事件";
                    }
                    else
                    {
                        col.HeaderText = "状态";
                    }
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Note")
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