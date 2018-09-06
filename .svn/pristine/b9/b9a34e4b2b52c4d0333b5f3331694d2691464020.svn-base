using SmartEP.Utilities.Office;
using SmartEP.Service.Core.Enums;
using SmartEP.WebControl.CbxRsm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Service.AutoMonitoring.Interfaces;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Utilities.DataTypes.ExtensionMethods;


namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：OriginalData.aspx.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：环境空气原始数据操作
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class OriginalDataTest : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private IInfectantDALService g_IInfectantDALService = null;

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

        /// <summary>
        /// 应用程序
        /// </summary>
        ApplicationType appType = ApplicationType.Air;
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
            //数据类型
            radlDataType.Items.Add(new ListItem("一分钟", PollutantDataType.Min1.ToString()));
            radlDataType.Items.Add(new ListItem("五分钟", PollutantDataType.Min5.ToString()));
            radlDataType.Items.Add(new ListItem("一小时", PollutantDataType.Min60.ToString()));
            radlDataType.SelectedValue = PollutantDataType.Min60.ToString();
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            try
            {
                //数据类型对应接口初始化
                g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue));

                string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                DateTime dtBegion = dtpBegin.SelectedDate.Value;
                DateTime dtEnd = dtpEnd.SelectedDate.Value;
                points = pointCbxRsm.GetPoints();
                string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
                factors = factorCbxRsm.GetFactors();

                //生成RadGrid的绑定列
                dvStatistical = null;
                //是否显示统计行
                if (IsStatistical.Checked)
                {
                    gridOriginal.ShowFooter = true;
                    dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
                }
                else
                {
                    gridOriginal.ShowFooter = false;
                }

                //每页显示数据个数            
                int pageSize = gridOriginal.PageSize;
                //当前页的序号
                int pageNo = gridOriginal.CurrentPageIndex;

                //数据总行数
                int recordTotal = 0;

                var monitorData = g_IInfectantDALService.GetDataPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
                gridOriginal.DataSource = monitorData;

                //数据总行数
                gridOriginal.VirtualItemCount = recordTotal;
            }
            catch (Exception ex) { }
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridOriginal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridOriginal_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                    if (points != null)
                        pointCell.Text = point.PointName;
                }
                for (int iRow = 0; iRow < factors.Count; iRow++)
                {
                    IPollutant factor = factors[iRow];
                    GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                    string factorStatus = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                    string factorMark = drv[factor.PollutantCode + "_Mark"] != DBNull.Value ? drv[factor.PollutantCode + "_Mark"].ToString() : string.Empty;
                    decimal value = 0M;
                    if (decimal.TryParse(cell.Text, out value))
                    {
                        value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                        cell.Text = value.ToString();
                        if (!string.IsNullOrEmpty(factorMark))
                        {
                            cell.Text = cell.Text + "(" + factorMark + ")";
                            cell.BackColor = System.Drawing.Color.Red;
                            cell.ForeColor = System.Drawing.Color.White;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridOriginal.Rebind();
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
                //数据类型对应接口初始化
                g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue));
                string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
                factors = factorCbxRsm.GetFactors();
                DateTime dateBegion = dtpBegin.SelectedDate.Value;
                DateTime dateEnd = dtpEnd.SelectedDate.Value;
                DataView dv = g_IInfectantDALService.GetExportData(portIds, factors, dateBegion, dateEnd);
                ExcelHelper.DataTableToExcel(dv.ToTable(), "数据查询", "数据查询", this.Page);
            }
        }

        #endregion

        protected void gridOriginal_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = (GridBoundColumn)e.Column;
                if (col.DataField == "PointId")
                {
                    col.HeaderText = "测点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterText = "平均值<br>最大值<br>最小值";
                    col.HeaderStyle.Width = Unit.Pixel(115);
                    col.ItemStyle.Width = Unit.Pixel(115);
                }
                else if (col.DataField == "Tstamp")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Min60)
                        tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(125);
                    col.ItemStyle.Width = Unit.Pixel(125);
                }
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(e.Column.UniqueName));
                    col.HeaderText = string.Format("{0}<br>({1})", factor.PollutantName, factor.PollutantMeasureUnit);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    SetGridFooterText(col);
                }
                else if (col.DataField == "blankspaceColumn")
                {
                    col.HeaderText = "";
                }
                else
                {
                    e.Column.Visible = false;
                }

            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 设置统计行
        /// </summary>
        /// <param name="col"></param>
        public void SetGridFooterText(GridBoundColumn col)
        {
            //统计行
            if (dvStatistical != null)
            {
                string avg = string.Empty;
                string max = string.Empty;
                string min = string.Empty;
                dvStatistical.RowFilter = string.Format("PollutantCode='{0}'", col.DataField);
                if (dvStatistical.Count > 0)
                {
                    avg = dvStatistical[0]["Value_Avg"] != DBNull.Value ? dvStatistical[0]["Value_Avg"].ToString() : "--";
                    max = dvStatistical[0]["Value_Max"] != DBNull.Value ? dvStatistical[0]["Value_Max"].ToString() : "--";
                    min = dvStatistical[0]["Value_Min"] != DBNull.Value ? dvStatistical[0]["Value_Min"].ToString() : "--";
                }
                col.FooterText = string.Format("{0}<br>{1}<br>{2}", avg, max, min);
                col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }
}