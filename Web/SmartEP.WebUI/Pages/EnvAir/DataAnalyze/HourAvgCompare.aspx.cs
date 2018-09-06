using SmartEP.Core.Enums;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
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
    public partial class HourAvgCompare : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 小时审核数据接口
        /// </summary>
        DataQueryByDayService g_DataQueryByDayService = new DataQueryByDayService();
        /// <summary>
        /// 气因子接口
        /// </summary>
        AirPollutantService g_AirPollutantService = new AirPollutantService();
        /// <summary>
        /// 暂时固定因子PM2.5（PollutantCode1），PM10（PollutantCode2）
        /// </summary>
        private string[] FactorCodes = new string[] { "a34004", "a34002" };
        /// <summary>
        /// 因子实体
        /// </summary>
        IList<SmartEP.Core.Interfaces.IPollutant> FactorList = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //因子
            FactorList = g_AirPollutantService.GetDefaultFactors(FactorCodes);
            if (!IsPostBack)
            {
                //初始化查询时间段
                txtStartDate.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01 01:00"));
                txtEndDate.SelectedDate = DateTime.Now;
            }
        }

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
            DateTime StartDate = Convert.ToDateTime(txtStartDate.SelectedDate == null ? DateTime.Parse("1900-01-01") : txtStartDate.SelectedDate);
            DateTime EndDate = Convert.ToDateTime(txtEndDate.SelectedDate == null ? DateTime.Parse("1900-01-01") : txtEndDate.SelectedDate);
            if (StartDate == DateTime.Parse("1900-01-01") || EndDate == DateTime.Parse("1900-01-01") || StartDate > EndDate)
            {
                Alert("请选择需要查看的时间段！");
                return;
            }
            dt = g_DataQueryByDayService.GetHourAvgCompareData(StartDate, EndDate, Convert.ToInt32(portid), FactorCodes);
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
                DateTime StartDate = Convert.ToDateTime(txtStartDate.SelectedDate);
                DateTime EndDate = Convert.ToDateTime(txtEndDate.SelectedDate);
                if (StartDate == DateTime.Parse("1900-01-01") || EndDate == DateTime.Parse("1900-01-01") || StartDate > EndDate)
                {
                    Alert("请选择需要查看的时间段！");
                    return;
                }

                DataTable dt = g_DataQueryByDayService.GetHourAvgCompareData(StartDate, EndDate, Convert.ToInt32(portid), FactorCodes);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    DataColumn dcNew = dt.Columns[i];
                    if (dcNew.ColumnName == "Tstamp")
                    {
                        dcNew.ColumnName = "时间";
                    }
                    else if (FactorList.Select(x => x.PollutantCode).Contains(dcNew.ColumnName))
                    {
                        SmartEP.Core.Interfaces.IPollutant factor = FactorList.FirstOrDefault(x => x.PollutantCode.Equals(dcNew.ColumnName));
                        dcNew.ColumnName = string.Format("{0}\n({1})", factor.PollutantName, factor.PollutantMeasureUnit);
                    }
                    else if (dcNew.ColumnName == "ratio")
                    {
                        dcNew.ColumnName = "比值";
                    }
                    else
                    {
                        dt.Columns.Remove(dcNew);
                        i--;
                    }
                }
                ExcelHelper.DataTableToExcel(dt, "颗粒物数据对比", "颗粒物数据对比", this.Page);
            }
        }
        #endregion

        #region 动态创建因子列
        /// <summary>
        /// 动态创建因子列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAvgHourData_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
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
                else if (FactorList.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    col = (GridBoundColumn)e.Column;
                    SmartEP.Core.Interfaces.IPollutant factor = FactorList.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
                    col.HeaderText = string.Format("{0}<br/>({1})", factor.PollutantName, factor.PollutantMeasureUnit);
                    col.DataFormatString = "{0:n3}";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                    col.Aggregate = GridAggregateFunction.Avg;
                }
                else if (col.DataField == "ratio")
                {
                    col = (GridBoundColumn)e.Column;
                    col.HeaderText = "比值";
                    col.DataFormatString = "{0:n3}";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
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
    }
}