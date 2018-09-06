using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.MPInfo;
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
    /// 名称：DataSamplingRateAnalyzeNew.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-12-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：环境空气数据捕获率统计
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class DataSamplingRateAnalyzeNew : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private AirDataSamplingRateNewService m_AirDataSamplingRateNewService;
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
        private DataView dvStatistical = null;

        /// <summary>
        /// 选择的因子，要传递到详情界面
        /// </summary>
        private IList<IPollutant> Factors
        {
            set
            {
                Session["DataSamplingRateAnalyzeFactors"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            m_AirDataSamplingRateNewService = new AirDataSamplingRateNewService();
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
            dtpBegin.SelectedDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            dtpEnd.SelectedDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            if (!IsPostBack)
            {
                MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
                IQueryable<MonitoringPointEntity> monitoringPointQueryable = m_MonitoringPointAirService.RetrieveAirMPListByEnable();//获取所有启用的空气点位列表
                monitoringPointQueryable = monitoringPointQueryable.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad"     //国控点
                                                                               || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca"  //对照点
                                                                               || p.ContrlUid == "bdf0837a-eb59-4c4a-a05f-c774a17f3077");//路边站
                string pointNames = monitoringPointQueryable.Select(t => t.MonitoringPointName)
                                        .Aggregate((a, b) => a + ";" + b);
                IList<PollutantCodeEntity> pollutantCodeList = GetPollutantListByCalAQI();
                string pollutantNames = pollutantCodeList.Select(t => t.PollutantName).Aggregate((a, b) => a + ";" + b);
                pointCbxRsm.SetPointValuesFromNames(pointNames);
                factorCbxRsm.SetFactorValuesFromNames(pollutantNames);

                pointCbxRsm_SelectedChanged();
            }
            if (dtpBegin.SelectedDate == null || dtpEnd.SelectedDate == null)
            {
                return;
            }
            else if (dtpBegin.SelectedDate > dtpEnd.SelectedDate)
            {
                return;
            }

            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            DateTime dtmBegion = dtpBegin.SelectedDate.Value;
            DateTime dtmEnd = dtpEnd.SelectedDate.Value;
            points = pointCbxRsm.GetPoints();
            string[] portIds = points.Select(t => t.PointID).ToArray();
            string orderBy = "PointId,Tstamp Desc";
            int pageSize = gridSamplingRate.PageSize;//每页显示数据个数  
            int pageNo = gridSamplingRate.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数
            factors = factorCbxRsm.GetFactors();
            Factors = factors;
            gridSamplingRate.ShowFooter = true;

            if (portIds == null || portIds.Length == 0)
            {
                gridSamplingRate.DataSource = null;
            }
            else
            {
                //绑定数据
                var samplingRateData = m_AirDataSamplingRateNewService.DataSamplingRateRetrieve(portIds, factors.Select(t => t.PollutantCode).ToArray(), dtmBegion, dtmEnd, pageSize, pageNo, out recordTotal, out dvStatistical, orderBy);

                if (samplingRateData == null)
                {
                    gridSamplingRate.DataSource = null;
                }
                else
                {
                    gridSamplingRate.DataSource = samplingRateData;
                }
            }

            //数据总行数
            gridSamplingRate.VirtualItemCount = recordTotal;
            Session["airFactors"] = factors.Select(t => t.PollutantCode).ToArray();
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridSamplingRate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridSamplingRate_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                DataTable dt = drv.DataView.Table;
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(drv["PointId"].ToString().Trim()));
                    if (point != null)
                    {
                        pointCell.Text = string.Format("<a href='#' onclick='ShowDetailForm(\"{0}\")'>{1}</a>", drv["PointId"], point.PointName);
                    }
                }
            }
        }

        /// <summary>
        /// Grid生成列事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridSamplingRate_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                int radGridColWidthValue = int.Parse(radGridColWidth.Value);
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                {
                    return;
                }
                if (col.DataField == "PointId")
                {
                    col.HeaderText = "测点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterText = "合计";
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
                    col.HeaderText = string.Format("{0}", factor.PollutantName);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    SetGridFooterText(col, dvStatistical);
                }
                else if (col.DataField == "TotalValue")
                {
                    col.HeaderText = "合计";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    SetGridFooterText(col, dvStatistical);
                }
                else if (col.DataField == "blankspaceColumn")
                {
                    col.HeaderText = string.Empty;
                }
                else
                {
                    col.Visible = false;
                }
            }
            catch (Exception ex) { }
        }

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
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridSamplingRate.CurrentPageIndex = 0;
            gridSamplingRate.Rebind();
        }

        /// <summary>
        /// 测点切换事件
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
            //IList<string> pollutantNameList = new List<string>();
            //string pollutantNames = string.Empty;
            //points = pointCbxRsm.GetPoints();
            //InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            //foreach (IPoint point in points)
            //{
            //    IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
            //        instrumentChannelService.RetrieveChannelListByPointUid(point.PointGuid);//根据站点Uid获取所有监测通道
            //    pollutantNameList = pollutantNameList.Union(pollutantCodeQueryable.Select(t => t.PollutantName)).ToList();
            //}
            //pollutantNames = pollutantNameList.ToString(t => t, ";");
            //factorCbxRsm.SetFactorValuesFromNames(pollutantNames);//设置默认因子（因子名称）
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
                if (dtpBegin.SelectedDate == null || dtpEnd.SelectedDate == null)
                {
                    //Alert("开始时间或者终止时间，不能为空！");
                    return;
                }
                else if (dtpBegin.SelectedDate > dtpEnd.SelectedDate)
                {
                    //Alert("开始时间不能大于终止时间！");
                    return;
                }

                string[] portIds = pointCbxRsm.GetPoints().Select(t => t.PointID).ToArray();
                string orderBy = "PointId,Tstamp Desc";
                int recordTotal = 0;
                factors = factorCbxRsm.GetFactors();
                DateTime dtmBegion = dtpBegin.SelectedDate.Value;
                DateTime dtmEnd = dtpEnd.SelectedDate.Value;
                var dv = m_AirDataSamplingRateNewService.DataSamplingRateRetrieve(portIds, factors.Select(t => t.PollutantCode).ToArray(), dtmBegion, dtmEnd, int.MaxValue, 0, out recordTotal, out dvStatistical, orderBy);
                DataTable dt = UpdateExportColumnName(dv, dvStatistical);
                ExcelHelper.DataTableToExcel(dt, "数据捕获率统计", "数据捕获率统计", this.Page);
            }
        }

        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <param name="dvStatistical">合计行数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataView dv, DataView dvStatistical)
        {
            DataTable dtNew = dv.ToTable();
            points = pointCbxRsm.GetPoints();
            factors = factorCbxRsm.GetFactors();

            if (dvStatistical != null && dvStatistical.Table.Rows.Count > 0)
            {
                DataTable dtStatistical = dvStatistical.Table;
                DataRow drAddRow = dtNew.NewRow();
                drAddRow["PointId"] = "合计";
                for (int i = 0; i < dtStatistical.Rows.Count; i++)
                {
                    DataRow drStatistical = dtStatistical.Rows[i];
                    if (dtNew.Columns.Contains(drStatistical["PollutantCode"].ToString()))
                    {
                        drAddRow[drStatistical["PollutantCode"].ToString()] = drStatistical["PollutantTotal"].ToString().Replace("<br/>", " \r\n");
                    }
                }
                dtNew.Rows.Add(drAddRow);
            }
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                drNew["PointId"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                    ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                    : drNew["PointId"].ToString();
            }
            for (int i = 0; i < dtNew.Columns.Count; i++)
            {
                DataColumn dcNew = dtNew.Columns[i];
                if (dcNew.ColumnName == "PointId")
                {
                    dcNew.ColumnName = "测点";
                }
                else if (factors.Select(x => x.PollutantCode).Contains(dcNew.ColumnName))
                {
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(dcNew.ColumnName));
                    dcNew.ColumnName = string.Format("{0}", factor.PollutantName);
                }
                else if (dcNew.ColumnName == "TotalValue")
                {
                    dcNew.ColumnName = "合计";
                }
                else
                {
                    dtNew.Columns.Remove(dcNew);
                    i--;
                }
            }
            return dtNew;
        }
        #endregion

        /// <summary>
        /// 获取参与评价AQI的常规6因子
        /// </summary>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantListByCalAQI()
        {
            SmartEP.Service.BaseData.Channel.AirPollutantService airPollutantService = new SmartEP.Service.BaseData.Channel.AirPollutantService();
            return airPollutantService.RetrieveListByCalAQI().ToList();
        }

    }
}