using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.DataAuditing.AuditBaseInfo;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Controls;
using SmartEP.WebControl.CbxRsm;
using SmartEP.Core.Enums;
using Telerik.Web.UI.Calendar;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System.Threading;
using SmartEP.WebUI.Pages.EnvAir.Audit;
using SmartEP.Utilities.AdoData;
using System.ComponentModel;
using SmartEP.Service.OperatingMaintenance.Air;
using System.Collections;
using SmartEP.Utilities.Office;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Core.Generic;


namespace SmartEP.WebUI.Pages.EnvAir.QualityControl
{
    public partial class QualityControlDataSearchNew : SmartEP.WebUI.Common.BasePage
    {
        #region 属性
        AirAuditMonitoringPointService pointAirService = new AirAuditMonitoringPointService();//点位接口 空气
        QualityControlDataSearchService dataSearchService = new QualityControlDataSearchService();//
        /// <summary>
        /// 站点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;
        static DateTime currentBegin = DateTime.Now;
        static DateTime currentEnd = DateTime.Now;
        string objectType = PageHelper.GetQueryString("ObjectType");
        #endregion
        protected RadTreeView columnTreeView;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!IsPostBack)
                {
                    BindTree();
                }
                InitControl();//初始化控件

            }
        }

        protected void BindTree()
        {
            DataView dvTree = dataSearchService.GetDataAreaPager();
            DataView dvAll = dataSearchService.GetDataPointPager();

            string strParentNode = "";
            List<SiteDataItem> siteData = new List<SiteDataItem>();
            if (dvAll.Count > 0)
            {
                for (var i = 0; i < dvTree.Count; i++)
                {
                    strParentNode = dvTree[i]["AreaName"].ToString();
                    string guid = dvTree[i]["RowGuid"].ToString();

                    dvAll.RowFilter = "AreaGuid='" + guid + "'";
                    //根节点
                    if (dvAll.Count > 0)
                    {
                        siteData.Add(new SiteDataItem((i + 1), 0, strParentNode));
                        for (var j = 0; j < dvAll.Count; j++)
                        {
                            siteData.Add(new SiteDataItem(GetValue<int>(dvAll[j]["AreaGuid"], 0), i + 1, dvAll[j]["ObjectName"].ToString()));
                        }
                    }
                }
            }
            TreeView1.DataTextField = "Text";
            TreeView1.DataValueField = "ID";
            TreeView1.DataFieldID = "ID";
            TreeView1.DataFieldParentID = "ParentID";
            TreeView1.DataSource = siteData;
            TreeView1.DataBind();
            TreeView1.ExpandAllNodes();
            //if (TreeView1. != null)
            TreeView1.CheckAllNodes();
        }
        #region 初始化控件
        private void InitControl()
        {
            RadDatePickerBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd 00:00:00"));
            RadDatePickerEnd.SelectedDate = DateTime.Now;
        #endregion
        }
        #region 事件
        /// <summary>
        /// 时间段选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadDatePickerBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (RadDatePickerBegin.SelectedDate == null || RadDatePickerEnd.SelectedDate == null)
            {
                Alert("时间不能为空");
                return;
            }
            else if (RadDatePickerBegin.SelectedDate.Value > RadDatePickerEnd.SelectedDate.Value)
            {
                Alert("开始时间必须小于结束时间！");
                return;
            }
            gridDataSearch.Rebind();
        }

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            DateTime dtBegin = RadDatePickerBegin.SelectedDate.Value;
            DateTime dtEnd = RadDatePickerEnd.SelectedDate.Value;
            DataTable dt = new DataTable();
            //每页显示数据个数            
            int pageSize = gridDataSearch.PageSize;
            //当前页的序号
            int currentPageIndex = gridDataSearch.CurrentPageIndex + 1;
            //查询记录的开始序号
            int startRecordIndex = pageSize * currentPageIndex;
            string factorValue = "";
            foreach (RadComboBoxItem item in factor.CheckedItems)
            {
                factorValue += item.Value.ToString() + ",";
            }
            factorValue = factorValue.Trim(',').ToString();
            string[] factors = factorValue.Split(',');

            string rcbType = "";
            foreach (RadTreeNode t in TreeView1.CheckedNodes)
            {
                if (t.ParentNode != null)
                {
                    if (t.Value != null && t.Value.Length > 0)
                    {
                        rcbType += t.Text.ToString() + ",";
                    }
                }
            }
            HiddenPoint.Value = rcbType.Trim(',').ToString();

            string[] name = hdInstrument.Value.Split(',');
            string[] names = { };
            string[] pointIds = HiddenPoint.Value.Split(',');
            pvChart.Visible = true;
            switch (ddlSearch.SelectedValue)
            {
                case "1":
                    dt = dataSearchService.GetPMSharpDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    string[] Rownames = { "输入数据" };
                    names = Rownames;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
                case "2":
                    dt = dataSearchService.GetPMTeomSharpDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    string[] Rownames2 = { "主输入读数" };
                    names = Rownames2;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
                case "3":
                    dt = dataSearchService.GetStdFlowMeterDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    string[] Rownames3 = { "待校准流量设备读数" };
                    names = Rownames3;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
                case "4":
                    dt = dataSearchService.GetO3HappenDevDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    string[] Rownames4 = { "被校仪器浓度" };
                    names = Rownames4;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
                case "5":
                    dt = dataSearchService.GetNOxDevDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    string[] Rownames5 = { "NO仪器响应", "NOx仪器响应" };
                    names = Rownames5;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
                case "6":
                    dt = dataSearchService.GetCaliDevDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    string[] Rownames6 = { "仪器读数" };
                    names = Rownames6;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
                case "7":
                    dt = dataSearchService.GetCaliDevDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    string[] Rownames7 = { "仪器读数" };
                    names = Rownames7;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
                case "8":
                    // pvChart.Visible = false;
                    dt = dataSearchService.GetZeroGasDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    string[] Rownames8 = { "当日响应平均值", "次日响应平均值" };
                    names = Rownames8;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
                case "9":
                    dt = dataSearchService.GetAnaDevPrecisionDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    dt.Columns.Remove("Code");
                    names = factors;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
                case "10":
                    dt = dataSearchService.GetZeroAndSpanDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    dt.Columns.Remove("Code");
                    names = factors;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
                case "11":
                    // pvChart.Visible = false;
                    dt = dataSearchService.GetAnaDevDriftDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    string[] Rownames11 = { "零气后响应" };
                    names = Rownames11;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
                case "12":
                    dt = dataSearchService.GetAnaDevDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    string[] Rownames12 = { "分析仪响应" };
                    names = Rownames12;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
                case "13":
                    dt = dataSearchService.GetMultiPointDataNewPager(name, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                    string[] Rownames13 = { "分析仪响应" };
                    names = Rownames13;
                    SetHiddenData(name, names, pointIds, dtBegin.ToString() + ";" + dtEnd.ToString());
                    break;
            }
            gridDataSearch.DataSource = dt;

            //数据分页的页数
            gridDataSearch.VirtualItemCount = dt.Rows.Count;
            //if (ddlSearch.SelectedValue == "8")
            //{
            //    gridDataSearch.MasterTableView.ColumnGroups[7].HeaderText = "零气仪器响应";
            //    gridDataSearch.MasterTableView.ColumnGroups[8].HeaderText = "零气参考";
            //}
            if (ddlSearch.SelectedValue == "10")
            {
                gridDataSearch.MasterTableView.ColumnGroups[2].HeaderText = "仪器零点响应";
                gridDataSearch.MasterTableView.ColumnGroups[3].HeaderText = "仪器跨度响应";
                gridDataSearch.MasterTableView.ColumnGroups[4].HeaderText = "斜率";
                gridDataSearch.MasterTableView.ColumnGroups[5].HeaderText = "截距";
                gridDataSearch.MasterTableView.ColumnGroups[6].HeaderText = "增益";
            }
            if (ddlSearch.SelectedValue == "11")
            {
                gridDataSearch.MasterTableView.ColumnGroups[0].HeaderText = "零气24小时漂移";
                gridDataSearch.MasterTableView.ColumnGroups[1].HeaderText = "跨度24小时漂移";
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


        #endregion
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            string rcbType = "";
            foreach (RadTreeNode t in TreeView1.CheckedNodes)
            {
                if (t.ParentNode != null)
                {
                    if (t.Value != null && t.Value.Length > 0)
                    {
                        rcbType += t.Text.ToString() + ",";
                    }
                }
            }
            HiddenPoint.Value = rcbType.Trim(',').ToString();
            string[] pointIds = HiddenPoint.Value.Split(',');
            if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
            {
                gridDataSearch.CurrentPageIndex = 0;
                gridDataSearch.Rebind();
                RegisterScript("RefreshChart();");
            }
            else
            {
                Alert("请选择测点");
                return;
            }

        }

        #endregion

        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            try
            {
                Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
                if (button.CommandName == "ExportToExcel")
                {
                    DateTime dtBegin = RadDatePickerBegin.SelectedDate.Value;
                    DateTime dtEnd = RadDatePickerEnd.SelectedDate.Value;
                    DataTable dt = new DataTable();
                    string[] SN = hdInstrument.Value.Split(',');
                    string rcbType = "";
                    foreach (RadTreeNode t in TreeView1.CheckedNodes)
                    {
                        if (t.ParentNode != null)
                        {
                            if (t.Value != null && t.Value.Length > 0)
                            {
                                rcbType += t.Text.ToString() + ",";
                            }
                        }
                    }
                    HiddenPoint.Value = rcbType.Trim(',').ToString();
                    string[] pointIds = HiddenPoint.Value.Split(',');

                    switch (ddlSearch.SelectedValue)
                    {
                        case "1":
                            dt = dataSearchService.GetPMSharpDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            break;
                        case "2":
                            dt = dataSearchService.GetPMTeomSharpDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            break;
                        case "3":
                            dt = dataSearchService.GetStdFlowMeterDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            break;
                        case "4":
                            dt = dataSearchService.GetO3HappenDevDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            break;
                        case "5":
                            dt = dataSearchService.GetNOxDevDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            break;
                        case "6":
                            dt = dataSearchService.GetCaliDevDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            break;
                        case "7":
                            dt = dataSearchService.GetCaliDevDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            break;
                        case "8":
                            dt = dataSearchService.GetZeroGasDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            break;
                        case "9":
                            dt = dataSearchService.GetAnaDevPrecisionDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            dt.Columns.Remove("Code");
                            break;
                        case "10":
                            dt = dataSearchService.GetZeroAndSpanDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            dt.Columns.Remove("Code");
                            break;
                        case "11":
                            dt = dataSearchService.GetAnaDevDriftDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            break;
                        case "12":
                            dt = dataSearchService.GetAnaDevDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            break;
                        case "13":
                            dt = dataSearchService.GetMultiPointDataNewPager(SN, pointIds, dtBegin, dtEnd, ddlSearch.SelectedText).Table;
                            break;
                    }
                    //dt = dataSearchService.GetPMSharpDataPager(SN, dtBegin, dtEnd).Table;
                    ExcelHelper.DataTableToExcel(dt, "质控数据查询", "质控数据查询", this.Page);
                }
            }
            catch (Exception ex)
            {
            }
        }


        protected void gridDataSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();

        }

        protected void gridDataSearch_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                //GridDataItem item = e.Item as GridDataItem;
                //DataRowView drv = e.Item.DataItem as DataRowView;
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                GridTableCell cell = (GridTableCell)item["时间"];
                if (cell.Text.ToString() != "--")
                {
                    cell.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Parse(cell.Text.ToString()));
                }
                GridTableCell taskCodeCell = (GridTableCell)item["任务编号"];
                taskCodeCell.Text = string.Format("<a href='#' onclick='RowClick(\"{0}\")'>{1}</a>", taskCodeCell.Text, taskCodeCell.Text);


                if (ddlSearch.SelectedValue == "1")
                {
                    GridTableCell Dcell = (GridTableCell)item["检定结果"];
                    string DabiaoStatus = drv["检定结果"] != DBNull.Value ? drv["检定结果"].ToString() : string.Empty;

                    if (DabiaoStatus == "True")
                    {
                        Dcell.Text = "运行";
                    }
                    else
                        Dcell.Text = "离线";
                }
                if (ddlSearch.SelectedValue == "9")
                {
                    GridTableCell Dcell = (GridTableCell)item["状态"];
                    string DabiaoStatus = drv["状态"] != DBNull.Value ? drv["状态"].ToString() : string.Empty;

                    if (DabiaoStatus == "True")
                    {
                        Dcell.Text = "开";
                    }
                    else
                        Dcell.Text = "关";
                }
                //if (ddlSearch.SelectedValue == "8")
                //{
                //    GridTableCell Dcell = (GridTableCell)item["验证结果"];
                //    string DabiaoStatus = drv["验证结果"] != DBNull.Value ? drv["验证结果"].ToString() : string.Empty;

                //    if (DabiaoStatus == "1")
                //    {
                //        Dcell.Text = "次日";
                //    }
                //    else if (DabiaoStatus == "0")
                //        Dcell.Text = "当日";
                //}
            }
        }

        protected void gridDataSearch_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                col.HeaderText = col.DataField;
                col.EmptyDataText = "--";
                col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                if (col.DataField == "测点")
                {
                    col.HeaderStyle.Width = Unit.Pixel(100);
                    col.ItemStyle.Width = Unit.Pixel(100);
                }
                else if (col.DataField == "仪器编号")
                {
                    col.HeaderStyle.Width = Unit.Pixel(130);
                    col.ItemStyle.Width = Unit.Pixel(130);
                }
                else
                {
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                if (col.DataField == "零气初始响应")
                    col.ColumnGroupName = "零气24小时漂移";
                if (col.DataField == "零气后响应")
                    col.ColumnGroupName = "零气24小时漂移";
                if (col.DataField == "零气漂移")
                    col.ColumnGroupName = "零气24小时漂移";
                if (col.DataField == "跨度初始响应")
                    col.ColumnGroupName = "跨度24小时漂移";
                if (col.DataField == "跨度后响应")
                    col.ColumnGroupName = "跨度24小时漂移";
                if (col.DataField == "跨度漂移")
                    col.ColumnGroupName = "跨度24小时漂移";
                if (col.DataField == "零气")
                    col.ColumnGroupName = "仪器零点响应";
                if (col.DataField == "零气调节前")
                    col.ColumnGroupName = "仪器零点响应";
                if (col.DataField == "零气调节后")
                    col.ColumnGroupName = "仪器零点响应";
                if (col.DataField == "跨度气")
                    col.ColumnGroupName = "仪器跨度响应";
                if (col.DataField == "跨度气调节前")
                    col.ColumnGroupName = "仪器跨度响应";
                if (col.DataField == "跨度气调节后")
                    col.ColumnGroupName = "仪器跨度响应";
                if (col.DataField == "斜率调节前")
                    col.ColumnGroupName = "斜率";
                if (col.DataField == "斜率调节后")
                    col.ColumnGroupName = "斜率";
                if (col.DataField == "截距调节前")
                    col.ColumnGroupName = "截距";
                if (col.DataField == "截距调节后")
                    col.ColumnGroupName = "截距";
                if (col.DataField == "增益调节前")
                    col.ColumnGroupName = "增益";
                if (col.DataField == "增益调节后")
                    col.ColumnGroupName = "增益";
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 页面隐藏域控件赋值
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="timeStr"></param>
        private void SetHiddenData(string[] name, string[] names, string[] pointIds, string timeStr)
        {
            HiddenData.Value = string.Join(";", name) + "|" + string.Join(";", names) + "|" + string.Join(";", pointIds) + "|"
                             + timeStr + "|" + ddlSearch.SelectedValue + "|" + ddlSearch.SelectedText + "|Air" + "|";
        }

        protected void ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //HiddenChartType.Value = ChartType.SelectedValue;
            //RegisterScript("ChartTypeChanged('" + ChartType.SelectedValue + "');");
        }

        protected void TreeView1_NodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            //ddlSearch.Items.Clear();
            string name = "";
            string rcbType = "";
            foreach (RadTreeNode t in TreeView1.CheckedNodes)
            {
                if (t.ParentNode != null)
                {
                    if (t.Value != null && t.Value.Length > 0)
                    {
                        rcbType += t.Text.ToString() + ",";
                    }
                }
                else
                    name = t.Text;
            }
            rcbType = rcbType.Trim(',').ToString();
            string[] PointNames = rcbType.Split(',');
            DataView dvPoint = dataSearchService.GetDataPointName(PointNames);
            string SiteGuid = "";
            for (int i = 0; i < dvPoint.Count; i++)
            {
                if (dvPoint[0]["RowGuid"].ToString() != "")
                    SiteGuid += dvPoint[i]["RowGuid"].ToString() + ",";
            }
            SiteGuid = SiteGuid.Trim(',');
            string[] SiteGuids = SiteGuid.Split(',');

            string objectType = "2";
            DataView dvTree = dataSearchService.GetDataBasePager();
            DataView dvAll = dataSearchService.GetDataPointPager(objectType, SiteGuids);

            if (dvAll.Count > 0)
            {
                for (var i = 0; i < dvTree.Count; i++)
                {
                    string guid = dvTree[i]["RowGuid"].ToString();

                    dvAll.RowFilter = "Infoguid='" + guid + "'";
                    //根节点
                    if (dvAll.Count > 0)
                    {
                        for (var j = 0; j < dvAll.Count; j++)
                        {
                            if (dvAll[j]["FixedAssetNumber"].ToString() != "")
                                hdInstrument.Value += dvAll[j]["FixedAssetNumber"].ToString() + ",";
                        }
                    }
                }
            }
            hdInstrument.Value = hdInstrument.Value.Trim(',').ToString();
            //if (name.Contains("PM10") || name.Contains("PM2.5"))
            //{
            //    ddlSearch.Items.Add(new DropDownListItem("Sharp5030颗粒物检查校准记录表", "1"));
            //    ddlSearch.Items.Add(new DropDownListItem("Thermo1400、1405颗粒物检查校准记录表", "2"));
            //}
            //else
            //{
            //    ddlSearch.Items.Add(new DropDownListItem("标准流量计检定核查记录表", "3"));
            //    ddlSearch.Items.Add(new DropDownListItem("臭氧校准仪校准记录表", "4"));
            //    ddlSearch.Items.Add(new DropDownListItem("氮氧化物分析仪动态校准记录表", "5"));
            //    ddlSearch.Items.Add(new DropDownListItem("动态校准仪流量（标准气）检查记录表", "6"));
            //    ddlSearch.Items.Add(new DropDownListItem("动态校准仪流量（稀释气）检查记录表", "7"));
            //    ddlSearch.Items.Add(new DropDownListItem("零气纯度检查记录表", "8"));
            //    ddlSearch.Items.Add(new DropDownListItem("气体分析仪精密度检查记录表", "9"));
            //    ddlSearch.Items.Add(new DropDownListItem("气体分析仪零点、跨度检查与调节记录表", "10"));
            //    ddlSearch.Items.Add(new DropDownListItem("气体分析仪零漂、标漂检查记录表", "11"));
            //    ddlSearch.Items.Add(new DropDownListItem("气体分析仪准确度审核记录表", "12"));
            //    ddlSearch.Items.Add(new DropDownListItem("多点线性校准记录表", "13"));
            //}
        }

        protected void ddlSearch_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            factor.Items.Clear();
            if (ddlSearch.SelectedValue == "9" || ddlSearch.SelectedValue == "10")
            {
                factor.Items.Add(new RadComboBoxItem("SO2", "SO2"));
                factor.Items.Add(new RadComboBoxItem("CO", "CO"));
                factor.Items.Add(new RadComboBoxItem("O3", "O3"));
                factor.Items.Add(new RadComboBoxItem("NO", "NO"));
                factor.Items.Add(new RadComboBoxItem("NOx", "NOx"));
            }
            for (int i = 0; i < factor.Items.Count; i++)
                factor.Items[i].Checked = true;
        }
    }
}