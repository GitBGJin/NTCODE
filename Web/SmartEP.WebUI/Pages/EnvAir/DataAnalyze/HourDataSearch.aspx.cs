using SmartEP.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.DataAuditing.AuditBaseInfo;
using SmartEP.Utilities.Office;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class HourDataSearch : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 审核小时数据接口
        /// </summary>
        DataQueryByDayService g_DataQueryByDayService = new DataQueryByDayService();
        /// <summary>
        /// 审核因子接口
        /// </summary>
        AuditMonitoringPointPollutantService g_AuditMonitoringPointPollutantService = new AuditMonitoringPointPollutantService();
        /// <summary>
        /// 系统类型
        /// </summary>
        public ApplicationType applicationType = ApplicationType.Air;
        /// <summary>
        /// 控件因子数据源
        /// </summary>
        private IList<SmartEP.Core.Interfaces.IPollutant> factors = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化查询时间段
                txtStartDate.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));
                txtEndDate.SelectedDate = DateTime.Now;
                txt_TimeDate.SelectedTime = TimeSpan.Parse("00:00");
                InitControl();
            }
        }

        #region 初始化因子选择
        /// <summary>
        /// 初始化因子选择
        /// </summary>
        private void InitControl()
        {
            string portid = pointCbxRsm.GetPointValuesStr(CbxRsmReturnType.ID);
            string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType);
            PointPollutantInfo[] list = g_AuditMonitoringPointPollutantService.RetrieveSiteMapPollutantList(int.Parse(portid), ApplicationUid, Convert.ToString(Session["UserGUID"])).ToArray();
            if (list.Length > 0)
            {
                string FactorNames = "";
                foreach (PointPollutantInfo Pollutant in list)
                {
                    FactorNames += Pollutant.PName + ";";
                }
                factorCbxRsm.SetFactorValuesFromNames(FactorNames.Substring(0, FactorNames.Length - 1));
            }

        }
        #endregion

        #region 获取数据源
        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <returns></returns>
        public void BindGrid()
        {
            DataTable dt = null;
            string portid = pointCbxRsm.GetPointValuesStr(CbxRsmReturnType.ID);
            if (string.IsNullOrEmpty(portid))
            {
                Alert("请选择测点！");
                return;
            }
            string[] factorCodes = factorCbxRsm.GetFactorValues(CbxRsmReturnType.Code);
            if (factorCodes.Length == 0)
            {
                Alert("请选择因子！");
                return;
            }
            string TimeDate = txt_TimeDate.SelectedTime == null ? "00:00" : Convert.ToString(txt_TimeDate.SelectedTime);
            if (txt_TimeDate.SelectedTime == null)
            {
                txt_TimeDate.SelectedTime = TimeSpan.Parse("00:00");
            }
            DateTime StartDate = txtStartDate.SelectedDate == null ? DateTime.Parse("1900-01-01") : DateTime.Parse(Convert.ToDateTime(txtStartDate.SelectedDate).ToString("yyyy-MM-dd") + " " + TimeDate);
            DateTime EndDate = txtEndDate.SelectedDate == null ? DateTime.Parse("1900-01-01") : DateTime.Parse(Convert.ToDateTime(txtEndDate.SelectedDate).ToString("yyyy-MM-dd") + " " + TimeDate);
            if (StartDate == DateTime.Parse("1900-01-01") || EndDate == DateTime.Parse("1900-01-01") || StartDate > EndDate)
            {
                Alert("请选择需要查看的时间段！");
                return;
            }
            dt = g_DataQueryByDayService.GetHourDate(StartDate, EndDate, Convert.ToInt32(portid), factorCodes);
            //数据总行数
            int recordTotal = dt.Rows.Count;
            //数据总行数
            grdAvgHourData.VirtualItemCount = recordTotal;
            //是否统计行
            if (chkIsFooter.Checked)
            {
                grdAvgHourData.ShowFooter = true;
            }
            else
            {
                grdAvgHourData.ShowFooter = false;
            }
            grdAvgHourData.DataSource = dt;
        }
        #endregion

        #region 查找按钮事件
        /// <summary>
        /// 查找按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdAvgHourData.Rebind();
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAvgHourData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }
        #endregion

        #region ToolBar事件
        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                string portid = pointCbxRsm.GetPointValuesStr(CbxRsmReturnType.ID);
                if (string.IsNullOrEmpty(portid))
                {
                    Alert("请选择测点！");
                    return;
                }
                string[] factorCodes = factorCbxRsm.GetFactorValues(CbxRsmReturnType.Code);
                if (factorCodes.Length == 0)
                {
                    Alert("请选择因子！");
                    return;
                }
                string TimeDate = txt_TimeDate.SelectedTime == null ? "00:00" : Convert.ToString(txt_TimeDate.SelectedTime);
                if (txt_TimeDate.SelectedTime == null)
                {
                    txt_TimeDate.SelectedTime = TimeSpan.Parse("00:00");
                }
                DateTime StartDate = txtStartDate.SelectedDate == null ? DateTime.Parse("1900-01-01") : DateTime.Parse(Convert.ToDateTime(txtStartDate.SelectedDate).ToString("yyyy-MM-dd") + " " + TimeDate);
                DateTime EndDate = txtEndDate.SelectedDate == null ? DateTime.Parse("1900-01-01") : DateTime.Parse(Convert.ToDateTime(txtEndDate.SelectedDate).ToString("yyyy-MM-dd") + " " + TimeDate);
                if (StartDate == DateTime.Parse("1900-01-01") || EndDate == DateTime.Parse("1900-01-01") || StartDate > EndDate)
                {
                    Alert("请选择需要查看的时间段！");
                    return;
                }

                factors = factorCbxRsm.GetFactors();
                DataTable dt = g_DataQueryByDayService.GetHourDate(StartDate, EndDate, Convert.ToInt32(portid), factorCodes);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    DataColumn dcNew = dt.Columns[i];
                    if (dcNew.ColumnName == "Tstamp")
                    {
                        dcNew.ColumnName = "时间";
                    }
                    else if (factors.Select(x => x.PollutantCode).Contains(dcNew.ColumnName))
                    {
                        SmartEP.Core.Interfaces.IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(dcNew.ColumnName));
                        dcNew.ColumnName = string.Format("{0}\n({1})", factor.PollutantName, factor.PollutantMeasureUnit);
                    }
                    else
                    {
                        dt.Columns.Remove(dcNew);
                        i--;
                    }
                }
                ExcelHelper.DataTableToExcel(dt, "小时数据对比分析", "小时数据对比分析", this.Page);
            }
        }
        #endregion

        #region 动态创建因子列
        /// <summary>
        /// 动态创建因子列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAvgHourData_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            try
            {
                //获取因子数据源
                factors = factorCbxRsm.GetFactors();
                //必须设置AutoGenerateColumns="true"
                if (e.Column.ColumnType.Equals("GridExpandColumn"))
                {
                    return;
                }
                //追加列数据
                GridBoundColumn col = (GridBoundColumn)e.Column;
                if (col.DataField == "Tstamp")
                {
                    col.HeaderText = "日期";
                    col.DataFormatString = "{0:yyyy-MM-dd HH:mm}";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                    col.FooterText = "平均值：";
                }
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    col = (GridBoundColumn)e.Column;
                    SmartEP.Core.Interfaces.IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
                    col.HeaderText = string.Format("{0}<br/>({1})", factor.PollutantName, factor.PollutantMeasureUnit);
                    col.DataFormatString = "{0:n3}";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(100);
                    col.ItemStyle.Width = Unit.Pixel(100);
                    col.Aggregate = GridAggregateFunction.Avg;
                }
                else
                {
                    e.Column.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 测点变更刷新因子事件
        /// <summary>
        /// 测点变更刷新因子事件
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
            InitControl();
        }
        #endregion
    }
}