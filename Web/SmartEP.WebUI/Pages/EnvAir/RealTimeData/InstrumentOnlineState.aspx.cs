using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Service.DataAnalyze.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.RealTimeData
{
    public partial class InstrumentOnlineState : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DataOnlineService s_DataOnlineService = Singleton<DataOnlineService>.GetInstance();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //数据类型
            radlDataType.Items.Add(new ListItem("一分钟", PollutantDataType.Min1.ToString()));
            radlDataType.Items.Add(new ListItem("五分钟", PollutantDataType.Min5.ToString()));
            radlDataType.Items.Add(new ListItem("一小时", PollutantDataType.Min60.ToString()));
            radlDataType.SelectedValue = PollutantDataType.Min60.ToString();//不是从首页原件传过来的参数默认 一小时数据

            //仪器
            Instrument.DataSource = s_DataOnlineService.GetInstrumentInfo();
            Instrument.DataTextField = "ItemText";
            Instrument.DataValueField = "ItemGuid";
            Instrument.DataBind();
            foreach (RadComboBoxItem item in Instrument.Items)
                item.Checked = true;
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {

            DataView dvOnlineRate = null;
            string Instruments = string.Join(",", Instrument.CheckedItems.Select(x => x.Value).ToArray());
            string[] InstrumentUids = (Instruments).Split(',');

            int pageSize = gridRealTimeOnlineState.PageSize;//每页显示数据个数  
            int pageNo = gridRealTimeOnlineState.CurrentPageIndex;//当前页的序号
            string netWorkType = Online.SelectedValue;
            var OnlineData = new DataView();
            DataTable dt = new DataTable();

            if (InstrumentUids == null || InstrumentUids.Length == 0)
            {
                OnlineData = null;
            }
            else
            {
                PollutantDataType pollutantDataType = SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue);
                //绑定数据
                OnlineData = s_DataOnlineService.GetInstrumentOnlineInfo(InstrumentUids, pollutantDataType, netWorkType).DefaultView;

                if (OnlineData != null && OnlineData.Count > 0)
                {
                    decimal OnlineCount = 0;
                    decimal OfflineCount = 0;
                    decimal TotalCount = 0;
                    string OnlineRate = "0%";
                    TotalCount = OnlineData.Count;
                    OnlineData.RowFilter = "NetWorking=1";
                    OnlineCount = OnlineData.Count;
                    OnlineData.RowFilter = "NetWorking=0";
                    OfflineCount = OnlineData.Count;
                    OnlineRate = Math.Round(OnlineCount / TotalCount * 100, 2) + "%";
                    lblNetwork.Text = OnlineRate;
                    lblAllNetwork.Text = TotalCount.ToString();
                    lblOffOnline.Text = OfflineCount.ToString();
                    lblOnline.Text = OnlineCount.ToString();
                }
                else
                {
                    lblNetwork.Text = "0%";
                    lblAllNetwork.Text = "0";
                    lblOffOnline.Text = "0";
                    lblOnline.Text = "0";
                }
                OnlineData.RowFilter = string.Empty;
                //因子保留小数位
                dt = OnlineData.ToTable();
                if (netWorkType == "2")
                {
                    OnlineData.RowFilter = "NetWorking=1";
                    dt = OnlineData.ToTable();
                }
                if (netWorkType == "3")
                {
                    OnlineData.RowFilter = "NetWorking=0";
                    dt = OnlineData.ToTable();
                }

            }
            if (OnlineData == null)
            {
                gridRealTimeOnlineState.DataSource = null;
            }
            else
            {
                if (InstrumentUids.Length == 0)
                {
                    dt.Clear();
                    dvOnlineRate.Table.Clear();
                }
                gridRealTimeOnlineState.DataSource = dt.DefaultView;
            }

            //数据总行数
            gridRealTimeOnlineState.VirtualItemCount = dt.Rows.Count;
        }

        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {

                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["NetWorking"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["NetWorking"];
                    if (drv["NetWorking"].ToString().ToUpper() == "1")
                    {
                        pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/public/On.PNG\" />";
                    }
                    else if (drv["NetWorking"].ToString().ToUpper() == "0")
                    {
                        pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/public/Off.PNG\" />";

                    }
                    else if (string.IsNullOrWhiteSpace(drv["NetWorking"].ToString()))
                    {
                        pointCell.Text = "--";

                    }
                }
            }
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridRealTimeOnlineState.Rebind();
        }

        #endregion

        protected void gridRealTimeOnlineState_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;

                if (col.DataField == "InstrumentName")
                {
                    col.HeaderText = "仪器";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(90);
                    col.ItemStyle.Width = Unit.Pixel(90);
                }
                else if (col.DataField == "NetWorking")
                {
                    col.HeaderText = "联网状态";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(70);
                    col.ItemStyle.Width = Unit.Pixel(70);
                }
                else if (col.DataField == "NetWorkInfo")
                {
                    col.HeaderText = "联网信息";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(90);
                    col.ItemStyle.Width = Unit.Pixel(90);
                }
                else if (col.DataField == "Tstamp")
                {
                    string tstcolformat = "{0:MM-dd HH:mm}";
                    col = col as GridDateTimeColumn;
                    col.HeaderText = "最近时间";
                    col.EmptyDataText = "--";
                    col.DataFormatString = tstcolformat;
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(90);
                    col.ItemStyle.Width = Unit.Pixel(90);
                }
                else if (col.DataField == "NetWorkInfo")
                {
                    col.Visible = false;
                    col.HeaderText = "离线时长";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "blankspaceColumn")
                {
                    col.HeaderText = string.Empty;
                    col.Visible = false;
                }
                else
                {
                    col.Visible = false;
                }
            }
            catch (Exception ex) { }

        }

    }
}