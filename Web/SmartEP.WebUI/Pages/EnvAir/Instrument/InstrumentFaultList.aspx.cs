using SmartEP.DomainModel;
using SmartEP.DomainModel.Framework;
using SmartEP.Service.OperatingMaintenance.Air;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Instrument
{
    /// <summary>
    /// 名称：InstrumentFaultList.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-05-24
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：仪器故障表单列表
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class InstrumentFaultList : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理
        /// </summary>
        InstrumentFaultService m_InstrumentFault = new InstrumentFaultService();
        QualityControlDataSearchService dataSearchService = new QualityControlDataSearchService();
        public string objectType = "";
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
            rdpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            rdpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            objectType = PageHelper.GetQueryString("ObjectType");
            //DataView dv = dataSearchService.GetDataPager(objectType);
            //for (var j = 0; j < dv.Count; j++)
            //{
            //    if (dv[j]["RowGuid"].ToString() != "")
            //        InstrumentName.Items.Add(new RadComboBoxItem(dv[j]["InstrumentName"].ToString(), dv[j]["RowGuid"].ToString()));
            //}
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {

            //string[] Instruments = InstrumentName.CheckedItems.Select(t => t.Value).ToArray();
            string[] occurStatus = OccurStatus.CheckedItems.Select(t => t.Text).ToArray();
            //每页显示数据个数            
            int pageSize = grdInstrumentFault.PageSize;
            //当前页的序号
            int pageNo = grdInstrumentFault.CurrentPageIndex;
            int recordTotal = 0;
            DateTime dtBegin = Convert.ToDateTime(rdpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
            DateTime dtEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
            string operateContent = "维护停用";
            var dataView = new DataView();
            dataView = m_InstrumentFault.GetDataPager(occurStatus, dtBegin, dtEnd, operateContent, objectType, pageSize, pageNo, out recordTotal);

            if (dataView.Count > 0)
            {
                grdInstrumentFault.DataSource = dataView;

                //数据分页的页数
                grdInstrumentFault.VirtualItemCount = dataView.Count;
            }
            else

                grdInstrumentFault.DataSource = new DataTable();

        }

        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdInstrumentFault_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdInstrumentFault_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
            }
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //string[] Instruments = InstrumentName.CheckedItems.Select(t => t.Value).ToArray();
            //if (Instruments.Length == 0)
            //{
            //    Alert("请选择仪器！");
            //    return;
            //}
            grdInstrumentFault.Rebind();
        }

        protected void grdInstrumentFault_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                int id = Convert.ToInt32(dataItem.GetDataKeyValue("Id"));
                //InstrumentInstanceRecord2Entity entity = m_InstrumentFault.GetModelRecord2(id);
                //string rowguid = entity.RowGuid;
                m_InstrumentFault.DeleteRecord2(id);
            }
            catch (Exception ex)
            {
                grdInstrumentFault.Controls.Add(new LiteralControl("Unable to delete . Reason: " + ex.Message));
                e.Canceled = true;
            }
        }

        ///// <summary>
        ///// ToolBar事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        //{
        //    Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
        //    if (button.CommandName == "ExportToExcel")
        //    {
        //        //List<IPoint> portIds = pointCbxRsm.GetPoints();
        //        //factors = factorCbxRsm.GetFactors();
        //        //DateTime dateBegion = dtpBegin.SelectedDate.Value;
        //        //DateTime dateEnd = dtpEnd.SelectedDate.Value;
        //        //DataView dv = m_MonitorDataQueryBiz.GetExportData(EnumMapping.ParseEnum<AutoMonitorType>(radlDataType.SelectedValue), portIds, factors, dateBegion, dateEnd);
        //        //ExcelHelper.DataTableToExcel(dv.ToTable(), "数据查询", "数据查询", this.Page);
        //    }
        //}
        #endregion
    }
}