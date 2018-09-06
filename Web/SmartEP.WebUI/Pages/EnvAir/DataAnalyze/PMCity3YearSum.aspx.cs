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
    /// 名称：PMCity3YearSum.aspx.cs
    /// 创建人：刘长敏
    /// 创建日期：2015-08-19
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：三年颗粒物市区合计
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class PMCity3YearSum : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>

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
            dateBegin.SelectedDate = Convert.ToDateTime("01-01");
            dateEnd.SelectedDate = DateTime.Now;
            ddl_year.Items.Insert(0, new ListItem(Convert.ToString(DateTime.Now.Year), Convert.ToString(DateTime.Now.Year)));

            for (int j = 1; j <= DateTime.Now.Year - 2015; j++)
            {
                ddl_year.Items.Insert(j, new ListItem(Convert.ToString(DateTime.Now.AddYears(-j).Year), Convert.ToString(DateTime.Now.AddYears(-j).Year)));
            }

            //查询类型
            rbtnlType.Items.Add(new ListItem("点位", "Port"));
            rbtnlType.Items.Add(new ListItem("苏州市区", "CityProper"));
            rbtnlType.Items.Add(new ListItem("城市均值", "City"));
            rbtnlType.Items.Add(new ListItem("城市均值（创模点）", "CityModel"));
            rbtnlType.SelectedValue = "Port";

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
            ////是否显示国家数据
            //if (IsStatistical.Checked)
            //{
            //    gridPM3Year.ShowFooter = true;
            //    dvStatistical = m_MonitorDataQueryBiz.GetStatisticalData(EnumMapping.ParseEnum<AutoMonitorType>(radlDataType.SelectedValue), portIds, factors, dtBegion, dtEnd);
            //}
            //else
            //{
            //    gridPM3Year.ShowFooter = false;
            //}
            //CreateRadGridColumn(factors, dvStatistical);

            //获取小时AQI集合
            //List<FactorAQI> hourAQIList = _aqiReport.GetAQI(SmartEP.Service.AutoMonitoring.Enums.ReportType.HourAQI);

            ////获取小时报原始数据集合
            //List<AQIFactorData> hourOldAQIList = _aqiReport.GetAQIFactorData(SmartEP.Service.AutoMonitoring.Enums.ReportType.HourAQI);

            ////获取小时报首要污染物
            //List<FactorAQI> firstAQIList = _aqiReport.GetPrimaryAQI(SmartEP.Service.AutoMonitoring.Enums.ReportType.HourAQI);


            //每页显示数据个数            
            int pageSize = gridPM3Year.PageSize;
            //当前页的序号
            int currentPageIndex = gridPM3Year.CurrentPageIndex + 1;
            //查询记录的开始序号
            int startRecordIndex = pageSize * currentPageIndex;

            int recordTotal = 0;

            var dataView = new DataView();// g_OfflineBiz.GetGridViewPager(pageSize, currentPageIndex, GetWhereString(), out recordTotal);
            if (dataView == null)
            {
                dataView = new DataView();
            }
            gridPM3Year.DataSource = new DataTable();//dataView;

            //数据分页的页数
            gridPM3Year.VirtualItemCount = recordTotal;
        }

        /// <summary>
        /// 生成RadGrid的绑定列
        /// </summary>
        /// <param name="factors">因子列表</param>
        /// <param name="dvStatistical">统计行数据</param>
        public void CreateRadGridColumn(IList<IPollutant> factors, DataView dvStatistical)
        {
            for (int m = (gridPM3Year.Columns.Count - 1); m > 0; m--)
            {
                gridPM3Year.Columns.RemoveAt(m);
            }
            int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());

            #region << 追加日期列 >>
            //GridDateTimeColumn colTstamp = m_MonitorDataQueryBiz.CreateGridBoundColumn(EnumMapping.ParseEnum<AutoMonitorType>(radlDataType.SelectedValue), "Tstamp", "日期", "Tstamp", 120);
            //gridPM3Year.Columns.Add(colTstamp);
            #endregion

            //#region << 追加因子列 >>
            //foreach (IPollutant factor in factors)
            //{
            //    //GridBoundColumn col = m_MonitorDataQueryBiz.CreateGridBoundColumn(factor.PollutantCode, string.Format("{0}({1})<br>", factor.PollutantName, factor.PollutantMeasureUnit)
            //    //    , factor.PollutantCode, radGridColWidthValue, HorizontalAlign.Center, HorizontalAlign.Center, dvStatistical);
            //    //gridPM3Year.Columns.Add(col);
            //}
            //#endregion

            //#region << 空白列 >>
            //GridBoundColumn colBlankspace = m_AirAuditDataQueryService.CreateGridBoundColumn("blankspaceColumn", "", "blankspaceColumn", 0, HorizontalAlign.Center, HorizontalAlign.Center, dvStatistical);
            //gridPM3Year.Columns.Add(colBlankspace);
            //#endregion
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridPM3Year_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridPM3Year_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                
                //for (int iRow = 0; iRow < factors.Count; iRow++)
                //{
                //    IPollutant factor = factors[iRow];
                //    GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                //    string factorFlag = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                //    cell.Text = cell.Text + factorFlag;
                //    //cell.Text = factorinfo.GetAlermString(drv[factorinfo.factorColumnName], drv["dataflag"], cell);
                //}
            }
            else if (e.Item is GridHeaderItem)
            {
                //GridHeaderItem item = e.Item as GridHeaderItem;
                ////GridTableHeaderCell cell = (GridTableHeaderCell)item["PM102013"];
               
                //GridTableCell cell = (GridTableCell)item["PM102013"];
                //cell.Text = "rte";
            }
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridPM3Year.Rebind();
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

        protected void ddl_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            int year =DateTime.Now.Year- Convert.ToInt32(ddl_year.SelectedValue);

            dateBegin.SelectedDate = DateTime.Now.AddYears(-year);
            dateEnd.SelectedDate = DateTime.Now.AddYears(-year);


        }

        protected void dateEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {

            int year =  Convert.ToDateTime(dateEnd.SelectedDate).Year;
            int ddlyear =Convert.ToInt32(ddl_year.SelectedValue);
            if (ddlyear > year)
            {
                int a = ddlyear - year;
                dateEnd.SelectedDate = Convert.ToDateTime(dateEnd.SelectedDate).AddYears(+a);
            }
            else if (ddlyear <year)
            {
                int a = year-ddlyear;
                dateEnd.SelectedDate = Convert.ToDateTime(dateEnd.SelectedDate).AddYears(-a);
            }
        }
    }
}