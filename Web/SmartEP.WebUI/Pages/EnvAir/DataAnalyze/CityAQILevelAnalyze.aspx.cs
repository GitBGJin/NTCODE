using SmartEP.DomainModel;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.WebControl.CbxRsm;
using SmartEP.WebUI.Common;
using System;
using System.Collections;
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
    /// 名称：CityAQILevelAnalyze.aspx.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：市区和全市各空气质量指数分级情况统计
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class CityAQILevelAnalyze : BasePage
    {
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

            //每页显示数据个数            
            int pageSize = grdAQI.PageSize;
            //当前页的序号
            int currentPageIndex = grdAQI.CurrentPageIndex + 1;
            //查询记录的开始序号
            int startRecordIndex = pageSize * currentPageIndex;

            int recordTotal = 0;

            var dataView = new DataView();// g_OfflineBiz.GetGridViewPager(pageSize, currentPageIndex, GetWhereString(), out recordTotal);
            if (dataView == null)
            {
                dataView = new DataView();
            }
            grdAQI.DataSource = new DataTable();//dataView;

            //数据分页的页数
            grdAQI.VirtualItemCount = recordTotal;
        }

        #endregion

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdAQI.Rebind();
        }

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
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdAQI.Rebind();
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
    }
}