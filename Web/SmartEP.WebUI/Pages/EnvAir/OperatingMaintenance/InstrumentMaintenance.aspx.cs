using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.MPInfo;
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
    /// 名称：InstrumentMaintenance.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-09-07
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：仪器维修维护保养
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class InstrumentMaintenance : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        AirDataEffectRateService airDataEffectRate = new AirDataEffectRateService();

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
            dtpBegin.SelectedDate = DateTime.Now.AddDays(-1);
            dtpEnd.SelectedDate = DateTime.Now;

        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {

            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            DateTime dtBegion = dtpBegin.SelectedDate.Value;
            DateTime dtEnd = dtpEnd.SelectedDate.Value;
            points = pointCbxRsm.GetPoints();
            string[] portIds = points.Select(t => t.PointID).ToArray();
            int pageSize = gridInstrumentMaintenance.PageSize;//每页显示数据个数  
            int pageNo = gridInstrumentMaintenance.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数
            factors = factorCbxRsm.GetFactors();

            //绑定数据
            var samplingRateData = airDataEffectRate.GetPointEffectRateDetail(portIds, factors.Select(t => t.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
            if (samplingRateData == null)
            {
                gridInstrumentMaintenance.DataSource = new DataTable();
            }
            else
            {
                gridInstrumentMaintenance.DataSource = samplingRateData;
            }

            //数据总行数
            gridInstrumentMaintenance.VirtualItemCount = recordTotal;

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
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridInstrumentMaintenance.Rebind();
        }

        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                //List<IPoint> portIds = pointCbxRsm.GetPoints();
                //factors = factorCbxRsm.GetFactors();
                //DateTime dateBegion = dtpBegin.SelectedDate.Value;
                //DateTime dateEnd = dtpEnd.SelectedDate.Value;
                //DataView dv = m_MonitorDataQueryBiz.GetExportData(EnumMapping.ParseEnum<AutoMonitorType>(radlDataType.SelectedValue), portIds, factors, dateBegion, dateEnd);
                //ExcelHelper.DataTableToExcel(dv.ToTable(), "数据查询", "数据查询", this.Page);
            }
        }

        #endregion


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
                    //ArrayList SelGuid = new ArrayList();
                    //foreach (String item in radGrid.SelectedIndexes)
                    //{
                    //    Index = Convert.ToInt32(item);
                    //    SelGuid.Add(TypeConversionExtensions.TryTo<object, Guid>(radGrid.MasterTableView.DataKeyValues[Index]["OfflineSettingUid"]));
                    //}
                    //string[] SelGid = (string[])SelGuid.ToArray(typeof(string));

                    //IQueryable<OfflineSettingEntity> entities = g_OfflineBiz.Retrieve(x => SelGid.Contains(x.OffLineSettingUid));
                    //if (entities != null && entities.Count() > 0)
                    //{
                    //    g_OfflineBiz.BatchDelete(entities.ToList<OfflineSettingEntity>());
                    //    base.Alert("删除成功！");
                    //}
                    #endregion
                    break;
            }
        }


    }
}