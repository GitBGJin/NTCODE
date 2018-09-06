using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.Office;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：DataEffectiveRate.aspx.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-05-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：系统正常运行率
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class DataEffectiveRate : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;
        string ddlSel = "0";
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
            rdpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            rdpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));

            //国控点，对照点，路边站
            MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            string strpointName = "";
            IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad" || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca").Select(p => p.MonitoringPointName).ToArray();
            foreach (string point in EnableOrNotportsarry)
            {
                strpointName += point + ";";
            }
            pointCbxRsm.SetPointValuesFromNames(strpointName);
            string pollutantName = System.Configuration.ConfigurationManager.AppSettings["AirPollutant"];
            factorCbxRsm.SetFactorValuesFromCodes(pollutantName);
        }
        #endregion
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            string[] factors = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);

            DateTime dtBegion = Convert.ToDateTime(rdpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
            DateTime dtEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));

            //生成RadGrid的绑定列
            //每页显示数据个数            
            int pageSize = grdEffectiveRate.PageSize;
            //当前页的序号
            int pageNo = grdEffectiveRate.CurrentPageIndex;
            var AvgDayData = new DataView();

            //点位
            if (portIds != null && factors != null)
            {
                AvgDayData = m_HourData.GetEffectiveData(portIds, factors, dtBegion, dtEnd);
                grdEffectiveRate.DataSource = AvgDayData;
                grdEffectiveRate.VirtualItemCount = AvgDayData.Count;
            }
            else
            {
                grdEffectiveRate.DataSource = new DataTable();
            }
        }

        #endregion
        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdEffectiveRate_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }
        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdEffectiveRate_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            GridDataItem item = e.Item as GridDataItem;
            DataRowView drv = e.Item.DataItem as DataRowView;
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            string[] factors = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Name);
            string factor = "";
            foreach (string strName in factors)
            {
                factor += strName + ";";
            }
            DateTime dtBegion = Convert.ToDateTime(rdpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
            DateTime dtEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
            if (e.Item is GridDataItem)
            {
                for (int i = 0; i < portIds.Length; i++)
                {
                    string portName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;
                    if (item[portName] != null)
                    {
                        GridTableCell pointCell = (GridTableCell)item[portName];
                        pointCell.Text = string.Format("<a href='#' onclick='RowClick(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\")'>{5}</a>", portName, dtBegion, dtEnd, factor,ddlSel, pointCell.Text);
                    }
                }
            }
        }
        /// <summary>
        /// 生成RadGrid的绑定列
        /// </summary>
        /// <param sender></param>
        /// <param e></param>
        protected void grdEffectiveRate_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
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
                col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                col.HeaderStyle.Width = Unit.Pixel(110);
                col.ItemStyle.Width = Unit.Pixel(110);

            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            try
            {
                Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
                if (button.CommandName == "ExportToExcel")
                {
                    string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                    string[] factors = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                    DateTime dtBegion = Convert.ToDateTime(rdpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                    DateTime dtEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                    DataTable dt = m_HourData.GetEffectiveData(portIds, factors, dtBegion, dtEnd, true).Table;
                    ExcelHelper.DataTableToExcel(dt, "系统正常运行率", "系统正常运行率", this.Page);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdEffectiveRate.CurrentPageIndex = 0;
            grdEffectiveRate.Rebind();
        }
    }
}