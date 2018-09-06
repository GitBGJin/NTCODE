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
    /// 名称：MaintenanceInfo.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：仪器保养详情
    /// 版权所有(C)：江苏远大信息股份有限公司
    public partial class MaintenanceInfo : SmartEP.WebUI.Common.BasePage
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


        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            DateTime dtBegion = DateTime.Now ;
            DateTime dtEnd = DateTime.Now;
            points = pointCbxRsm.GetPoints();
            string[] portIds = points.Select(t => t.PointID).ToArray();
            int pageSize = gridMaintenanceInfo.PageSize;//每页显示数据个数  
            int pageNo = gridMaintenanceInfo.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数
            factors = factorCbxRsm.GetFactors();

            dvStatistical = airDataEffectRate.GetEffectRateStatisticalData(portIds, factors.Select(t => t.PollutantCode).ToArray(), dtBegion, dtEnd);
            //绑定数据
            var airDataEffectRateData = airDataEffectRate.GetPointEffectRateDetail(portIds, factors.Select(t => t.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
            if (airDataEffectRateData == null)
            {
                gridMaintenanceInfo.DataSource = new DataTable();
            }
            else
            {
                gridMaintenanceInfo.DataSource = airDataEffectRateData;
            }

            //数据总行数
            gridMaintenanceInfo.VirtualItemCount = recordTotal;



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
            


        }
        /// <summary>
        /// Grid生成列事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDER_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            

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