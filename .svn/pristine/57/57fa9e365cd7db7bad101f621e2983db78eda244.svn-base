using SmartEP.Core.Interfaces;
using SmartEP.WebControl.CbxRsm;
using SmartEP.WebUI.Common;
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
    /// 名称：InvalidDaysYearAnalyze.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：年无效天查询
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class InvalidDaysYearAnalyze : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        //private OfflineSettingBiz g_OfflineBiz = new OfflineSettingBiz();
        //private MonitorDataQueryBiz m_MonitorDataQueryBiz;       

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;

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
            for (int i = 0; i < 20; i++)
            {
                ddlBegin.Items.Add(new DropDownListItem((DateTime.Now.Year - i).ToString(), (DateTime.Now.Year - i).ToString()));
                if (DateTime.Now.Year - i <= 2009)
                {
                    break;
                }
            }
            for (int i = 0; i < 20; i++)
            {
                ddlEnd.Items.Add(new DropDownListItem((DateTime.Now.Year - i).ToString(), (DateTime.Now.Year - i).ToString()));
                if (DateTime.Now.Year - i <= 2009)
                {
                    break;
                }
            }
            ddlEnd.SelectedText = DateTime.Now.Year.ToString();
            ddlBegin.SelectedText = (DateTime.Now.Year - 1).ToString();

            ////数据类型
            //radlDataType.Items.Add(new ListItem("一分钟", AutoMonitorType.Min1.ToString()));
            //radlDataType.Items.Add(new ListItem("五分钟", AutoMonitorType.Min5.ToString()));
            //radlDataType.Items.Add(new ListItem("一小时", AutoMonitorType.Min60.ToString()));
            //radlDataType.SelectedValue = AutoMonitorType.Min60.ToString();

            //查询类型
            //rbtnlType.Items.Add(new ListItem("点位", "点位"));
            //rbtnlType.Items.Add(new ListItem("苏州市区", "苏州市区"));
            //rbtnlType.Items.Add(new ListItem("城市均值", "城市均值"));
            //rbtnlType.Items.Add(new ListItem("城市均值（创模点）", "城市均值（创模点）"));
            rbtnlType.SelectedValue = "点位";

            //查询点位（区域）
            //ddlPoint.Items.Add(new ListItem("南门", "南门"));
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            //string[] factorCodes = factorCbxRsm.GetFactorValues(CbxRsmReturnType.Code);
            //DateTime dtBegion = dtpBegin.SelectedDate.Value;
            //DateTime dtEnd = dtpEnd.SelectedDate.Value;
            //List<IPoint> portIds = pointCbxRsm.GetPoints();
            //factors = factorCbxRsm.GetFactors();

            ////生成RadGrid的绑定列
            //DataView dvStatistical = null;
            ////是否显示统计行
            //if (IsStatistical.Checked)
            //{
            //    gridMonitor.ShowFooter = true;
            //    dvStatistical = m_MonitorDataQueryBiz.GetStatisticalData(EnumMapping.ParseEnum<AutoMonitorType>(radlDataType.SelectedValue), portIds, factors, dtBegion, dtEnd);
            //}
            //else
            //{
            //    gridMonitor.ShowFooter = false;
            //}
            //CreateRadGridColumn(factors, dvStatistical);    

            int pageSize = gridInvalidDays.PageSize;  //每页显示数据个数  
            int currentPageIndex = gridInvalidDays.CurrentPageIndex + 1;  //当前页的序号
            int startRecordIndex = pageSize * currentPageIndex;  //查询记录的开始序号
            int recordTotal = 0;  //数据总行数
            var dataView = new DataView();// g_OfflineBiz.GetGridViewPager(pageSize, currentPageIndex, GetWhereString(), out recordTotal);

            if (dataView == null)
            {
                dataView = new DataView();
            }
            gridInvalidDays.DataSource = new DataTable();//dataView;            
            gridInvalidDays.VirtualItemCount = recordTotal;
        }

        /// <summary>
        /// 生成RadGrid的绑定列
        /// </summary>
        /// <param name="factors">因子列表</param>
        /// <param name="dvStatistical">统计行数据</param>
        public void CreateRadGridColumn(IList<IPollutant> factors, DataView dvStatistical)
        {
            int radGridColWidthValue;

            for (int m = (gridInvalidDays.Columns.Count - 1); m > 0; m--)
            {
                gridInvalidDays.Columns.RemoveAt(m);
            }
            radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());

            #region << 追加日期列 >>
            //GridDateTimeColumn colTstamp = m_MonitorDataQueryBiz.CreateGridBoundColumn(EnumMapping.ParseEnum<AutoMonitorType>(radlDataType.SelectedValue), "Tstamp", "日期", "Tstamp", 120);
            //gridInvalidDays.Columns.Add(colTstamp);
            #endregion

            #region << 追加因子列 >>
            foreach (IPollutant factor in factors)
            {
                //GridBoundColumn col = m_MonitorDataQueryBiz.CreateGridBoundColumn(factor.PollutantCode, string.Format("{0}({1})<br>", factor.PollutantName, factor.PollutantMeasureUnit)
                //    , factor.PollutantCode, radGridColWidthValue, HorizontalAlign.Center, HorizontalAlign.Center, dvStatistical);
                //gridInvalidDays.Columns.Add(col);
            }
            #endregion

            #region << 空白列 >>
            //GridBoundColumn colBlankspace = m_MonitorDataQueryBiz.CreateGridBoundColumn("blankspaceColumn", "", "blankspaceColumn", 0, HorizontalAlign.Center, HorizontalAlign.Center, dvStatistical);
            //gridInvalidDays.Columns.Add(colBlankspace);
            #endregion
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
                for (int iRow = 0; iRow < factors.Count; iRow++)
                {
                    IPollutant factor = factors[iRow];
                    GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                    string factorFlag = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                    cell.Text = cell.Text + factorFlag;
                    //cell.Text = factorinfo.GetAlermString(drv[factorinfo.factorColumnName], drv["dataflag"], cell);
                }
            }
        }

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridInvalidDays.Rebind();
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

        /// <summary>
        /// 查询类型切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //暂时写成这样，等提供数据源后再修改
            comboPort.Visible = false;
            comboCityProper.Visible = false;
            comboCity.Visible = false;
            comboCityModel.Visible = false;
            switch (rbtnlType.SelectedValue)
            {
                case "Port":
                    comboPort.Visible = true;
                    break;
                case "CityProper":
                    comboCityProper.Visible = true;
                    break;
                case "City":
                    comboCity.Visible = true;
                    break;
                case "CityModel":
                    comboCityModel.Visible = true;
                    break;
            }
        }
        #endregion
    }
}