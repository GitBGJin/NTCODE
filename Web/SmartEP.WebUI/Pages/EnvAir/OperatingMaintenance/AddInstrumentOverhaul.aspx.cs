using SmartEP.Core.Interfaces;
using SmartEP.Service.AutoMonitoring.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance
{
    /// <summary>
    /// 名称：AddOverhaul.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：仪器检修
    /// 版权所有(C)：江苏远大信息股份有限公司
    public partial class AddInstrumentOverhaul : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        WaterDataEffectRateService waterDataEffectRate = new WaterDataEffectRateService();

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 统计
        /// </summary>
        DataView dvStatistical = null;

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

        }
        #endregion



        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            //string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            //DateTime dtBegion = dtpBegin.SelectedDate.Value;
            //DateTime dtEnd = dtpBegin.SelectedDate.Value;
            //points = pointCbxRsm.GetPoints();
            //string[] portIds = points.Select(t => t.PointID).ToArray();
            //int pageSize = gridSamplingRecord.PageSize;//每页显示数据个数  
            //int pageNo = gridSamplingRecord.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数
            //factors = factorCbxRsm.GetFactors();

            //dvStatistical = waterDataEffectRate.GetEffectRateStatisticalData(portIds, factors.Select(t => t.PollutantCode).ToArray(), dtBegion, dtEnd);
            ////绑定数据
            //var airDataEffectRateData = waterDataEffectRate.GetPointEffectRateDetail(portIds, factors.Select(t => t.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
            //if (airDataEffectRateData == null)
            //{
            //    gridSamplingRecord.DataSource = new DataTable();
            //}
            //else
            //{
            //    gridSamplingRecord.DataSource = airDataEffectRateData;
            //}

            DataTable dtt = new DataTable();
            dtt.Rows.Add(dtt.NewRow());
            dtt.Rows.Add(dtt.NewRow());
            dtt.Rows.Add(dtt.NewRow());
            gridSamplingRecord.DataSource = dtt;

            //数据总行数
            gridSamplingRecord.VirtualItemCount = recordTotal;




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
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem item = e.Item as GridDataItem;
            //    DataRowView drv = e.Item.DataItem as DataRowView;
            //    if (item["PointId"] != null)
            //    {
            //        GridTableCell pointCell = (GridTableCell)item["PointId"];
            //        IPoint point = points.FirstOrDefault(x => x.PointID.Equals(drv["PointId"].ToString().Trim()));
            //        if (points != null)
            //        {
            //            pointCell.Text = string.Format("<a href='#' onclick='ShowDetails(\"{0}\")'>{1}</a>", drv["PointId"], point.PointName);
            //        }
            //    }
            //}
        }
        /// <summary>
        /// Grid生成列事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDER_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {

            //try
            //{
            //    GridBoundColumn col = e.Column as GridBoundColumn;
            //    if (col == null)
            //        return;

            //    if (col.DataField == "PointId")
            //    {
            //        col.HeaderText = "测点";
            //        col.EmptyDataText = "--";
            //        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            //        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            //        col.FooterText = "合计";
            //        col.HeaderStyle.Width = Unit.Pixel(110);
            //        col.ItemStyle.Width = Unit.Pixel(110);
            //    }
            //    else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
            //    {
            //        int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
            //        IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
            //        col.HeaderText = string.Format("{0}", factor.PollutantName);
            //        col.EmptyDataText = "--";
            //        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            //        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            //        col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
            //        col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
            //        SetGridFooterText(col, dvStatistical);
            //    }
            //    else if (col.DataField == "TotalValue")
            //    {
            //        int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
            //        col.HeaderText = "合计";
            //        col.EmptyDataText = "--";
            //        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            //        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            //        col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
            //        col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
            //        SetGridFooterText(col, dvStatistical);
            //    }
            //    else if (col.DataField == "blankspaceColumn")
            //    {
            //        col.HeaderText = string.Empty;
            //    }
            //    else
            //    {
            //        col.Visible = false;
            //    }
            //}
            //catch (Exception ex) { }
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridSamplingRecord.Rebind();
        }

        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        //{
        //    Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
        //    if (button.CommandName == "ExportToExcel")
        //    {
        //        List<RsmPoint> portIds = pointCbxRsm.GetPoints();
        //        factors = factorCbxRsm.GetFactors();
        //        DateTime dateBegion = dtpBegin.SelectedDate.Value;
        //        DateTime dateEnd = dtpEnd.SelectedDate.Value;
        //        DataView dv = m_MonitorDataQueryBiz.GetExportData(EnumMapping.ParseEnum<AutoMonitorType>(radlDataType.SelectedValue), portIds, factors, dateBegion, dateEnd);
        //        ExcelHelper.DataTableToExcel(dv.ToTable(), "数据查询", "数据查询", this.Page);
        //    }
        //}

        /// <summary>
        /// 设置列的Footer信息
        /// </summary>
        /// <param name="col">列</param>
        /// <param name="dvStatistical">要绑定的数据</param>
        private void SetGridFooterText(GridBoundColumn col, DataView dvStatistical)
        {
            //统计行
            if (dvStatistical != null)
            {
                string total = string.Empty;
                dvStatistical.RowFilter = string.Format("PollutantCode='{0}'", col.DataField);
                if (dvStatistical.Count > 0)
                {
                    total = dvStatistical[0]["PollutantTotal"] != DBNull.Value ? dvStatistical[0]["PollutantTotal"].ToString() : "--";
                }
                col.FooterText = string.Format("{0}", total);
                col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            }
        }
        #endregion
    }
}