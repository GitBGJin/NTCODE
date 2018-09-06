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
    /// 名称：DataEffectRateNew.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-12-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：数据有效率查询
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class DataEffectRateNew : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        AirDataEffectRateService airDataEffectRate = new AirDataEffectRateService();
        AirDataEffectRateNewService airDataEffectRateNew = new AirDataEffectRateNewService();
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
            //if (!IsPostBack)
            //{
            //    pointCbxRsm_SelectedChanged();
            //}

            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            DateTime dtBegion = dtpBegin.SelectedDate.Value;
            DateTime dtEnd = dtpEnd.SelectedDate.Value;
            points = pointCbxRsm.GetPoints();
            string[] portIds = points.Select(t => t.PointID).ToArray();
            int pageSize = grdDER.PageSize;//每页显示数据个数  
            int pageNo = grdDER.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数
            factors = factorCbxRsm.GetFactors();
            string orderBy = "PointId asc,Tstamp desc";

            //绑定数据
            var airDataEffectRateData = airDataEffectRateNew.GetPointEffectRateDetail(portIds, factors.Select(t => t.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, out dvStatistical, orderBy);
            if (airDataEffectRateData == null)
            {
                grdDER.DataSource = new DataTable();
            }
            else
            {
                grdDER.DataSource = airDataEffectRateData;
            }

            //数据总行数
            grdDER.VirtualItemCount = recordTotal;
            Session["airFactors"] = factors.Select(t => t.PollutantCode).ToArray();
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
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(drv["PointId"].ToString().Trim()));
                    if (points != null)
                    {
                        pointCell.Text = string.Format("<a href='#' onclick='ShowDetails(\"{0}\")'>{1}</a>", drv["PointId"], point.PointName);
                    }
                }
            }


        }
        /// <summary>
        /// Grid生成列事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDER_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
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
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterText = "合计";
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
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
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
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
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdDER.Rebind();
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
                int recordTotal = 0;
                string orderBy = "PointId asc,Tstamp desc";
                string[] portIds = pointCbxRsm.GetPoints().Select(t => t.PointID).ToArray();
                DateTime dtBegion = dtpBegin.SelectedDate.Value;
                DateTime dtEnd = dtpEnd.SelectedDate.Value;
                factors = factorCbxRsm.GetFactors();
                DataView dv = airDataEffectRateNew.GetPointEffectRateDetail(portIds, factors.Select(t => t.PollutantCode).ToArray(), dtBegion, dtEnd, int.MaxValue, 0, out recordTotal, out dvStatistical, orderBy);

                DataTable dt = UpdateExportColumnName(dv, dvStatistical);
                ExcelHelper.DataTableToExcel(dt, "数据有效率查询", "数据有效率查询", this.Page);

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
            DataTable dtOld = dv.ToTable();
            DataTable dtNew = dtOld.Clone();
            dtNew.Columns["PointId"].DataType = typeof(string);
            //DataTable dtNew = dv.ToTable();
            points = pointCbxRsm.GetPoints();
            factors = factorCbxRsm.GetFactors();

            for (int i = 0; i < dtOld.Rows.Count; i++)
            {
                DataRow drOld = dtOld.Rows[i];
                DataRow drNew = dtNew.NewRow();
                foreach (DataColumn dcOld in dtOld.Columns)
                {
                    if (!string.IsNullOrWhiteSpace(drOld[dcOld].ToString()))
                    {
                        drNew[dcOld.ColumnName] = drOld[dcOld].ToString().Replace("<br/>", " \r\n");
                    }

                }
                dtNew.Rows.Add(drNew);
            }

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

        //protected void pointCbxRsm_SelectedChanged()
        //{
        //    points = pointCbxRsm.GetPoints();
        //    InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
        //    IList<string> list = new List<string>();
        //    string[] factor;
        //    string factors = string.Empty;
        //    foreach (IPoint point in points)
        //    {
        //        IQueryable<PollutantCodeEntity> p = m_InstrumentChannelService.RetrieveChannelListByPointUid(point.PointGuid);
        //        list = list.Union(p.Select(t => t.PollutantName)).ToList();
        //    }
        //    factor = list.ToArray();
        //    foreach (string f in factor)
        //    {
        //        factors += f + ";";
        //    }
        //    factorCbxRsm.SetFactorValuesFromNames(factors);
        //}

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