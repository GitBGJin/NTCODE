using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
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
    /// 名称：InstrumentParameterSearch.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：仪器参数查询
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class InstrumentParameterSearch : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private InstrumentDataBy60Service m_InstrumentDataBy60Service;

        /// <summary>
        /// 选择测点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<PollutantCodeEntity> factors = null;

        /// <summary>
        /// 监测仪器数据列表
        /// </summary>
        private IList<MonitoringInstrumentEntity> m_MonitorInstrList = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_InstrumentDataBy60Service = new InstrumentDataBy60Service();
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            dtpBegin.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00"));
            dtpEnd.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:00"));
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
                //DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
                //MonitoringPointAirService monitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
                //IQueryable<MonitoringPointEntity> monitoringPointQueryable = monitoringPointAirService.RetrieveAirMPListByEnable();//获取所有启用的空气点位列表
                //IQueryable<V_CodeMainItemEntity> codeMainItemQueryable = dictionaryService.RetrieveList(DictionaryType.Air, "空气站点属性类型");//获取空气站点属性类型
                //string[] contrlUids = codeMainItemQueryable.Where(t => t.ItemText == "中意" || t.ItemText == "国控").Select(t => t.ItemGuid).ToArray();
                //monitoringPointQueryable = monitoringPointQueryable.Where(p => contrlUids.Contains(p.ContrlUid));
                //string[] pointNameArray = monitoringPointQueryable.Select(t => t.MonitoringPointName).ToArray();
                //string pointNames = pointNameArray.Aggregate((t1, t2) => t1 + ";" + t2);
                pointCbxRsm.SetPointValuesFromNames("监测站");
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

            DateTime dtmBegion = dtpBegin.SelectedDate.Value;
            DateTime dtmEnd = dtpEnd.SelectedDate.Value;
            int pageSize = gridInstrument.PageSize;//每页显示数据个数  
            int pageNo = gridInstrument.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数
            points = pointCbxRsm.GetPoints();
            //IPoint point = points.FirstOrDefault();
            string instrumentCode = string.Empty;
            string[] pollutantCodes = comboFactor.CheckedItems.Select(t => t.Value).ToArray();
            string orderBy = "PointId,Tstamp Desc";//OrderByNum
            //string pointId = point != null ? point.PointID : string.Empty;
            //factors = factorCbxRsm.GetFactors();
            factors = GetPollutantListByPointList(points, pollutantCodes);
            gridInstrument.ShowFooter = false;
            var samplingRateData = new DataView();
            if (points != null && points.Count > 0)//(point != null)
            {
                MonitoringInstrumentService monitoringInstrumentService = new MonitoringInstrumentService();
                m_MonitorInstrList = monitoringInstrumentService.RetrieveListByUids(comboInstrument.Items.Select(t => t.Value).ToArray()).ToList();

                //for (int i = 0; i < comboInstrument.Items.Count; i++)
                //{
                //    MonitoringInstrumentEntity monitoringInstrumentEntity = monitoringInstrumentService.RetrieveByPointUid(point.PointGuid, comboInstrument.Items[i].Value);//根据站点Uid和仪器Uid获取监测仪器
                //    m_MonitorInstrList.Add(monitoringInstrumentEntity);
                //}
                instrumentCode = (m_MonitorInstrList != null && m_MonitorInstrList.Count > 0)
                    ? m_MonitorInstrList.Where(t => t.InstrumentUid == comboInstrument.SelectedValue).Select(t => t.InstrumentNumber).FirstOrDefault() : string.Empty;
                instrumentCode = string.IsNullOrWhiteSpace(instrumentCode) && comboInstrument.Items.Count > 0 ? comboInstrument.SelectedItem.Text : instrumentCode;
                samplingRateData = m_InstrumentDataBy60Service.GetDataPager(points.Select(t => t.PointID).ToArray(), instrumentCode, pollutantCodes, dtmBegion, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
            }
            else
            {
                samplingRateData = null;
            }

            //绑定数据
            if (samplingRateData == null)
            {
                gridInstrument.DataSource = null;
            }
            else
            {
                //【给隐藏域赋值，用于显示Chart】
                SetHiddenData(points.Select(t => t.PointID).ToArray(), instrumentCode, pollutantCodes, dtmBegion, dtmEnd);
                gridInstrument.DataSource = samplingRateData;
            }

            //数据总行数
            gridInstrument.VirtualItemCount = recordTotal;
        }

        /// <summary>
        /// 页面隐藏域控件赋值，将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void SetHiddenData(string[] portIds, string instrumentCode, string[] factors, DateTime dtBegin, DateTime dtEnd)
        {
            if (factors.Length > 0 && portIds != null)
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors)
                                 + "|" + dtBegin + "|" + dtEnd + "|" + instrumentCode + "|Air";
                HiddenChartType.Value = ChartType.SelectedValue;
            }
        }
        #endregion

        #region 服务器端控件事件处理

        /// <summary>
        /// 切换图表类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenChartType.Value = ChartType.SelectedValue;
            RegisterScript("ChartTypeChanged('" + ChartType.SelectedValue + "');");
        }

        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridInstrument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridInstrument_ItemDataBound(object sender, GridItemEventArgs e)
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
                        pointCell.Text = point.PointName;
                    }
                }
                if (item["instrumentCode"] != null && m_MonitorInstrList != null && m_MonitorInstrList.Count > 0)
                {
                    GridTableCell instrumentCell = (GridTableCell)item["instrumentCode"];
                    MonitoringInstrumentEntity monitorInstr = (m_MonitorInstrList != null && m_MonitorInstrList.Count > 0)
                        ? m_MonitorInstrList.FirstOrDefault(x => x.InstrumentNumber != null && x.InstrumentNumber.Equals(drv["instrumentCode"].ToString().Trim())) : null;
                    if (monitorInstr != null)
                    {
                        instrumentCell.Text = comboInstrument.Items.Where(t => t.Value == monitorInstr.InstrumentUid)
                                                                   .Select(t => t.Text).FirstOrDefault();
                    }
                }
                for (int iRow = 0; iRow < factors.Count; iRow++)
                {
                    PollutantCodeEntity factor = factors[iRow];
                    GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                    string factorStatus = (dt.Columns.Contains(factor.PollutantCode + "_Status") && drv[factor.PollutantCode + "_Status"] != DBNull.Value)
                        ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                    string factorMark = (dt.Columns.Contains(factor.PollutantCode + "_Mark") && drv[factor.PollutantCode + "_Mark"] != DBNull.Value)
                        ? drv[factor.PollutantCode + "_Mark"].ToString() : string.Empty;
                    decimal value = 0M;
                    if (decimal.TryParse(cell.Text, out value))
                    {
                        value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.DecimalDigit.ToString())
                            ? 3 : Convert.ToInt32(factor.DecimalDigit.ToString()));
                        cell.Text = value.ToString("G0");
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
        /// Grid生成列事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridInstrument_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
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
                else if (col.DataField == "instrumentCode")
                {
                    col.HeaderText = "仪器";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Tstamp")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:yyyy-MM-dd HH:00}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(125);
                    col.ItemStyle.Width = Unit.Pixel(125);
                }
                else if (comboFactor.Items.Select(x => x.Value).Contains(col.DataField))
                {
                    RadComboBoxItem comboBoxItem = comboFactor.Items.FirstOrDefault(x => x.Value.Equals(col.DataField));
                    col.HeaderText = string.Format("{0}", comboBoxItem.Text);//少单位
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.Visible = true;
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
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridInstrument.CurrentPageIndex = 0;
            gridInstrument.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {
                RegisterScript("RefreshChart();");
            }
            else
            {
                FirstLoadChart.Value = "1";
            }
        }

        /// <summary>
        /// 测点切换事件
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
            MonitoringInstrumentService monitoringInstrumentService = new MonitoringInstrumentService();
            IList<IPoint> pointList = pointCbxRsm.GetPoints();//.FirstOrDefault();
            if (pointList != null && pointList.Count > 0)//(point != null)
            {
                string[] pointGuids = pointList.Select(t => t.PointGuid).ToArray();

                //MonitoringPointAirService monitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
                //IQueryable<MonitoringPointEntity> monitoringPointQueryable = monitoringPointAirService.RetrieveAirMPListByEnable();
                //string[] pointGuids = monitoringPointQueryable.Select(t => t.MonitoringPointUid).ToArray();

                IQueryable<InstrumentEntity> instrumentQueryable = monitoringInstrumentService.RetrieveListByPointUids(pointGuids).Distinct();//根据站点Uid数组获取监测仪器列表
                comboInstrument.DataSource = instrumentQueryable.OrderBy(t => t.OrderByNum);
                comboInstrument.DataValueField = "RowGuid";
                comboInstrument.DataTextField = "InstrumentName";
                comboInstrument.DataBind();
                if (comboInstrument.Items.Count > 0)
                {
                    comboInstrument.Items[0].Checked = true;
                }
                comboInstrument_SelectedIndexChanged(null, null);
            }
            else
            {
                comboInstrument.Items.Clear();
                //factorCbxRsm.SetFactorValuesFromNames(string.Empty);
                comboFactor.Items.Clear();
            }
        }

        /// <summary>
        /// 仪器切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void comboInstrument_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //InstrumentService instrumentService = new InstrumentService();//提供仪器库服务
            InstrumentChannelService instrumentChannelService = new InstrumentChannelService();//提供仪器通道信息服务
            //IQueryable<InstrumentChannelEntity> instrumentChannelQueryable =
            //    instrumentService.RetrieveInstrumentChannelListByInstrumentUid(comboInstrument.SelectedValue);//根据仪器Uid获取监测因子列表
            IQueryable<PollutantCodeEntity> pollutantCodeEntityQueryable =
                instrumentChannelService.RetrieveInstrumentStateListByInstrumentUid(comboInstrument.SelectedValue);//根据仪器Uid获取仪器监测的所有状态
            //string pollutantNames = instrumentChannelQueryable.ToString(t => t.PollutantName, ";");
            //factorCbxRsm.SetFactorValuesFromNames(pollutantNames);//设置默认因子（因子名称）
            comboFactor.DataSource = pollutantCodeEntityQueryable.OrderBy(t => t.OrderByNum);
            comboFactor.DataValueField = "PollutantCode";
            comboFactor.DataTextField = "PollutantName";
            comboFactor.DataBind();
            //if (comboFactor.Items.Count > 0)
            //{
            //    comboFactor.Items[0].Checked = true;
            //}
            for (int i = 0; i < comboFactor.Items.Count; i++)
            {
                comboFactor.Items[i].Checked = true;
            }
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

                DateTime dtmBegion = dtpBegin.SelectedDate.Value;
                DateTime dtmEnd = dtpEnd.SelectedDate.Value;
                points = pointCbxRsm.GetPoints();
                //IPoint point = points.FirstOrDefault();
                //string pointId = point != null ? point.PointID : string.Empty;
                if (points != null && points.Count > 0)//(point != null)
                {
                    string instrumentCode = string.Empty;
                    string[] pollutantCodes = comboFactor.CheckedItems.Select(t => t.Value).ToArray();
                    string orderBy = "PointId,Tstamp Desc";
                    //IList<IPollutant> pollutants = GetPollutantsByPointList(points, pollutantCodes);
                    MonitoringInstrumentService monitoringInstrumentService = new MonitoringInstrumentService();
                    m_MonitorInstrList = monitoringInstrumentService.RetrieveListByUids(comboInstrument.Items.Select(t => t.Value).ToArray()).ToList();
                    //for (int i = 0; i < comboInstrument.Items.Count; i++)
                    //{
                    //    MonitoringInstrumentEntity monitoringInstrumentEntity = monitoringInstrumentService.RetrieveByPointUid(point.PointGuid, comboInstrument.Items[i].Value);//根据站点Uid和仪器Uid获取监测仪器
                    //    m_MonitorInstrList.Add(monitoringInstrumentEntity);
                    //}
                    instrumentCode = (m_MonitorInstrList != null && m_MonitorInstrList.Count > 0)
                        ? m_MonitorInstrList.Where(t => t.InstrumentUid == comboInstrument.SelectedValue).Select(t => t.InstrumentNumber).FirstOrDefault() : string.Empty;
                    instrumentCode = string.IsNullOrWhiteSpace(instrumentCode) ? comboInstrument.SelectedItem.Text : instrumentCode;
                    //pollutants = pollutants.Distinct().ToArray();
                    DataView dv = m_InstrumentDataBy60Service.GetExportData(points.Select(t => t.PointID).ToArray(), instrumentCode, pollutantCodes, dtmBegion, dtmEnd, orderBy);
                    DataTable dt = UpdateExportColumnName(dv);
                    ExcelHelper.DataTableToExcel(dt, "仪器参数查询", "仪器参数查询", this.Page);
                }
                else
                {
                    Alert("请先选择测点。");
                }
            }
        }

        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataView dv)
        {
            DataTable dt = dv.ToTable();
            DataTable dtNew = new DataTable();
            MonitoringInstrumentService monitoringInstrumentService = new MonitoringInstrumentService();
            string[] pollutantCodes = comboFactor.CheckedItems.Select(t => t.Value).ToArray();
            points = pointCbxRsm.GetPoints();
            factors = GetPollutantListByPointList(points, pollutantCodes);

            foreach (DataColumn dc in dt.Columns)
            {
                string columnName = dc.ColumnName;
                if (columnName.IndexOf("(") >= 0)
                {
                    columnName = columnName.Substring(0, columnName.IndexOf("("));
                }
                if (columnName == "PointId")
                {
                    dtNew.Columns.Add("PointId", typeof(string));
                }
                else if (columnName == "Tstamp")
                {
                    dtNew.Columns.Add("Tstamp", typeof(string));
                }
                else if (columnName == "日期")
                {
                    dtNew.Columns.Add("日期", typeof(string));
                }
                else if (!dtNew.Columns.Contains(columnName))
                {
                    dtNew.Columns.Add(columnName, dc.DataType);
                }
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                DataRow drNew = dtNew.NewRow();
                MonitoringInstrumentEntity monitorInstr = (m_MonitorInstrList != null && m_MonitorInstrList.Count > 0)
                    ? m_MonitorInstrList.FirstOrDefault(x => x.InstrumentNumber != null && x.InstrumentNumber.Equals(drNew["instrumentCode"].ToString().Trim())) : null;
                foreach (DataColumn dcNew in dtNew.Columns)
                {
                    drNew[dcNew] = dr[dcNew.ColumnName];
                }
                drNew["PointId"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                    ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                    : drNew["PointId"].ToString();
                if (monitorInstr != null)
                {
                    drNew["instrumentCode"] = comboInstrument.Items.Where(t => t.Value == monitorInstr.InstrumentUid)
                                                                   .Select(t => t.Text).FirstOrDefault();
                }
                if (dtNew.Columns.Contains("Tstamp") && !string.IsNullOrWhiteSpace(dr["Tstamp"].ToString()))
                {
                    drNew["Tstamp"] = string.Format("{0:yyyy-MM-dd HH:00}", DateTime.Parse(dr["Tstamp"].ToString()));
                }
                else if (dtNew.Columns.Contains("日期") && !string.IsNullOrWhiteSpace(dr["日期"].ToString()))
                {
                    drNew["日期"] = string.Format("{0:yyyy-MM-dd HH:00}", DateTime.Parse(dr["日期"].ToString()));
                }
                dtNew.Rows.Add(drNew);
            }
            for (int i = 0; i < dtNew.Columns.Count; i++)
            {
                DataColumn dcNew = dtNew.Columns[i];
                if (dcNew.ColumnName == "PointId")
                {
                    dcNew.ColumnName = "测点";
                }
                else if (dcNew.ColumnName == "Tstamp" || dcNew.ColumnName == "日期")
                {
                    string tstcolformat = "{0:yyyy-MM-dd HH:00}";
                    dcNew.ColumnName = "日期";
                }
                else if (dcNew.ColumnName == "instrumentCode" || dcNew.ColumnName == "仪器")
                {
                    dcNew.ColumnName = "仪器";
                }
                else if (comboFactor.Items.Count(x => dcNew.ColumnName.Equals(x.Value)) > 0)
                {
                    RadComboBoxItem comboBoxItem = comboFactor.Items.FirstOrDefault(x => x.Value.Equals(dcNew.ColumnName));
                    dcNew.ColumnName = string.Format("{0}", comboBoxItem.Text);
                }
                //else if (factors.Count(x => dcNew.ColumnName.Equals(x.PollutantCode)) > 0)
                //{
                //    PollutantCodeEntity factor = factors.FirstOrDefault(x => dcNew.ColumnName.Equals(x.PollutantCode));
                //    dcNew.ColumnName = string.Format("{0}", factor.PollutantName);
                //}
                //else if (factors.Count(x => dcNew.ColumnName.Equals(x.PollutantName)) > 0)
                //{
                //    PollutantCodeEntity factor = factors.FirstOrDefault(x => dcNew.ColumnName.Equals(x.PollutantName));
                //    dcNew.ColumnName = string.Format("{0}", factor.PollutantName);
                //}
                //else if (factors.Count(x => dcNew.ColumnName.StartsWith(x.PollutantCode + "(")) > 0)
                //{
                //    PollutantCodeEntity factor = factors.FirstOrDefault(x => dcNew.ColumnName.StartsWith(x.PollutantCode + "("));
                //    dcNew.ColumnName = string.Format("{0}", factor.PollutantName);
                //}
                //else if (factors.Count(x => dcNew.ColumnName.StartsWith(x.PollutantName + "(")) > 0)
                //{
                //    PollutantCodeEntity factor = factors.FirstOrDefault(x => dcNew.ColumnName.StartsWith(x.PollutantName + "("));
                //    dcNew.ColumnName = string.Format("{0}", factor.PollutantName);
                //}
                else
                {
                    dtNew.Columns.Remove(dcNew);
                    i--;
                }
            }
            return dtNew;
        }

        /// <summary>
        /// 根据测点数据获取因子数据
        /// </summary>
        /// <param name="pointList">测点数据</param>
        /// <param name="pollutantCodes">因子Code数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantListByPointList(IList<IPoint> pointList, string[] pollutantCodes)
        {
            IList<PollutantCodeEntity> pollutantList = new List<PollutantCodeEntity>();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            foreach (IPoint point in pointList)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                    instrumentChannelService.RetrieveStatusChannelListByPointUid(point.PointGuid);//根据站点Uid获取所有监测仪器状态通道
                pollutantList = pollutantList.Union(pollutantCodeQueryable.Where(t => pollutantCodes.Contains(t.PollutantCode))).ToList();
            }
            return pollutantList;
        }

        /// <summary>
        /// 根据测点数据获取因子数据
        /// </summary>
        /// <param name="pointList">测点数据</param>
        /// <param name="pollutantCodes">因子Code数组</param>
        /// <returns></returns>
        private IList<IPollutant> GetPollutantsByPointList(IList<IPoint> pointList, string[] pollutantCodes)
        {
            IList<IPollutant> pollutantList = new List<IPollutant>();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            foreach (IPoint point in pointList)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                    instrumentChannelService.RetrieveChannelListByPointUid(point.PointGuid);//根据站点Uid获取所有监测通道
                foreach (PollutantCodeEntity pollutantCodeEntity in pollutantCodeQueryable)
                {
                    if (pollutantCodes.Contains(pollutantCodeEntity.PollutantCode))
                    {
                        pollutantList.Add(new RsmFactor(pollutantCodeEntity.PollutantName, pollutantCodeEntity.PollutantCode,
                            pollutantCodeEntity.DecimalDigit.ToString(), pollutantCodeEntity.MeasureUnitUid, pollutantCodeEntity.PollutantUid));
                    }
                }
            }
            return pollutantList;
        }
        #endregion
    }
}