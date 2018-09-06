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
using SmartEP.Service.AutoMonitoring.Water;
using SmartEP.Service.AutoMonitoring.Interfaces;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using System.ComponentModel;
using System.Collections;
using SmartEP.DomainModel;
using SmartEP.Service.Frame;
using SmartEP.Core.Generic;
using SmartEP.Utilities.Web.UI;
using SmartEP.Utilities.Caching;

namespace SmartEP.WebUI.Pages.EnvWater.DataAnalyze
{
    /// <summary>
    /// 名称：OriginalData.aspx.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：地表水原始数据操作
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class OriginalData : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private IInfectantDALService g_IInfectantDALService = null;
        InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
        /// <summary>
        /// 60分钟数据服务层
        /// </summary>
        InfectantBy60Service g_InfectantBy60Service = Singleton<InfectantBy60Service>.GetInstance();
        //代码项服务层
        DictionaryService dicService = new DictionaryService();
        //获取因子上下限数据处理服务
        ExcessiveSettingService m_ExcessiveSettingService = new ExcessiveSettingService();
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
        /// 上下限
        /// </summary>
        DataTable dtExcessive = null;
        /// <summary>
        /// 选择因子
        /// </summary>
        private string[] factorCodes = new string[] { };
        /// <summary>
        /// 选择站点
        /// </summary>
        private string[] portIds = new string[] { };
        /// <summary>
        /// 国家标记位
        /// </summary>
        //IQueryable<V_CodeMainItemEntity> siteTypeEntites = null;

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
            //IsStatistical.Visible = true;

            //string pollutantName = System.Configuration.ConfigurationManager.AppSettings["WaterPollutant"];
            //factorCbxRsm.SetFactorValuesFromNames(pollutantName);
            factors = factorCbxRsm.GetFactors();
            //if (factors.Count() == 0)
            //{
            //    factorCbxRsm.DefaultAllSelected = true;
            //}
            //string WaterPoints = System.Configuration.ConfigurationManager.AppSettings["WaterPoints"];
            //pointCbxRsm.SetPointValuesFromNames(WaterPoints, SessionHelper.Get("UserGuid").ToString());
            dtpBegin.SelectedDate = DateTime.Now.AddDays(-1);
            dtpEnd.SelectedDate = DateTime.Now;
            dayBegin.SelectedDate = DateTime.Now.AddDays(-7);
            dayEnd.SelectedDate = DateTime.Now;
            //数据类型
            radlDataType.Items.Add(new ListItem("分钟", PollutantDataType.Min1.ToString()));
            radlDataType.Items.Add(new ListItem("小时", PollutantDataType.Min60.ToString()));
            //radlDataType.Items.Add(new ListItem("原始日均值", PollutantDataType.Day.ToString()));
            radlDataType.SelectedValue = PollutantDataType.Min60.ToString();
            if (radlDataType.SelectedValue == "Min1")
            {
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm:ss";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm:ss";
            }
            else
            {
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            }
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
            //数据类型对应接口初始化
            g_IInfectantDALService = MonitoringDataWater.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue));

            factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            DateTime dtBegion = dtpBegin.SelectedDate.Value;
            DateTime dtEnd = dtpEnd.SelectedDate.Value;
            if (radlDataType.SelectedValue == "Min1")
            {
                dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:mm:ss"));
                dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (radlDataType.SelectedValue == "Min60")
            {
                dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:mm"));
                dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:mm"));
            }
            //else if (radlDataType.SelectedValue == "Day")
            //{
            //    dtBegion = dayBegin.SelectedDate.Value;
            //    dtEnd = dayEnd.SelectedDate.Value;
            //    dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd 00:00:00"));
            //    dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd 23:59:59"));
            //}
            points = pointCbxRsm.GetPoints();
            portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            factors = factorCbxRsm.GetFactors();

            //生成RadGrid的绑定列
            dvStatistical = null;
            //是否显示统计行
            if (IsStatistical.Checked && factors.Count != 0)
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

            //【给隐藏域赋值，用于显示Chart】
            SetHiddenData(portIds, factors, dtBegion, dtEnd);

            //数据总行数
            int recordTotal = 0;
            string orderby = "PointId,Tstamp desc";
            if (portIds != null && factors.Count > 0)
            {
                if (radlDataType.SelectedValue == "Min1" || radlDataType.SelectedValue == "Min60")
                {
                    var monitorData = g_IInfectantDALService.GetDataPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderby);
                    gridOriginal.DataSource = monitorData;
                    //数据总行数
                    gridOriginal.VirtualItemCount = recordTotal;
                }
                //else if (radlDataType.SelectedValue == "Day")
                //{
                //    orderby = "PointId";
                //    //var monitorData = g_InfectantBy60Service.GetDayAvgData(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, orderby);
                //    var monitorData = g_InfectantBy60Service.GetDayAvgDataUpdate(portIds, factors, dtBegion, dtEnd, orderby);
                //    monitorData.Sort = "PointId,Tstamp desc";
                //    gridOriginal.DataSource = monitorData;
                //    //数据总行数
                //    gridOriginal.VirtualItemCount = monitorData.Count;
                //}
            }
            else
            {
                gridOriginal.DataSource = null;
            }
            ////获取上下限
            //string DataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
            //IQueryable<ExcessiveSettingInfo> Excessive = m_ExcessiveSettingService.RetrieveListByPointAndFactor(ApplicationValue.Water, factorCodes, portIds, DataTypeUid);
            //dtExcessive = ConvertToDataTable(Excessive);
            ////国家数据标记位
            //siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "国家数据标记");
        }

        /// <summary>
        /// 绑定图表
        /// </summary>
        private void BindChart()
        {
            //if (ShowType.Text.Equals("分屏"))
            //{
            //    RegisterScript("InitGroupChart();");
            //}
            //else if (ShowType.Text.Equals("合并"))
            //{
            //    RegisterScript("InitTogetherChart();");
            //}
        }
        /// <summary>
        /// 因子、测点类型图表选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PointFactor_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenPointFactor.Value = PointFactor.SelectedValue;
            RegisterScript("PointFactor('" + PointFactor.SelectedValue + "');");
        }
        /// <summary>
        /// 页面隐藏域控件赋值，将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void SetHiddenData(string[] portIds, IList<IPollutant> factors, DateTime dtBegin, DateTime dtEnd)
        {
            if (factors.Count > 0 && portIds != null)
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "|Water";
                HiddenChartType.Value = ChartType.SelectedValue;
            }
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
            //string DataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
            ////string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            ////string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            //IQueryable<ExcessiveSettingInfo> Excessive = m_ExcessiveSettingService.RetrieveListByPointAndFactor(ApplicationValue.Water, factorCodes, portIds, DataTypeUid);
            //DataTable dtExcessive = ConvertToDataTable(Excessive);
            ////国家数据标记位
            //IQueryable<V_CodeMainItemEntity> siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "国家数据标记");

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                    if (point != null)
                        pointCell.Text = point.PointName;
                }
                for (int iRow = 0; iRow < factors.Count; iRow++)
                {
                    //string siteTypeName = "--";//标记位名称
                    IPollutant factor = factors[iRow];
                    GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                    //string factorStatus = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                    //string factorMark = drv[factor.PollutantCode + "_Mark"] != DBNull.Value ? drv[factor.PollutantCode + "_Mark"].ToString() : string.Empty;
                    decimal value = 0M;
                    if (decimal.TryParse(cell.Text, out value))
                    {
                        value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                        cell.Text = value.ToString("");
                        //        if ((factorStatus != "N" && !string.IsNullOrEmpty(factorStatus)) || !string.IsNullOrEmpty(factorMark))
                        //        {
                        //            string markContent = string.Empty;
                        //            if (factorStatus != "N" && !string.IsNullOrEmpty(factorStatus))
                        //            {
                        //                markContent += factorStatus + "|";
                        //                siteTypeName = factorStatus + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorStatus))
                        //.Select(t => t.ItemText).FirstOrDefault() + ")" + "|";//标记位名称
                        //            }
                        //            if (!string.IsNullOrEmpty(factorMark))
                        //            {
                        //                markContent += factorMark;
                        //                if (siteTypeName == "--")
                        //                {
                        //                    string[] factorMarkArry = factorMark.Split(',');
                        //                    for (int m = 0; m < factorMarkArry.Length; m++)
                        //                    {
                        //                        siteTypeName = factorMark + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorMark))
                        //              .Select(t => t.ItemText).FirstOrDefault() + ")";//标记位名称
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    string[] factorMarkArry = factorMark.Split(',');
                        //                    for (int m = 0; m < factorMarkArry.Length; m++)
                        //                    {
                        //                        siteTypeName += factorMark + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorMark))
                        //              .Select(t => t.ItemText).FirstOrDefault() + ")";//标记位名称
                        //                    }
                        //                }
                        //            }
                        //            markContent = markContent.TrimEnd('|');
                        //            cell.Text = cell.Text + "(" + markContent + ")";
                        //            cell.ForeColor = System.Drawing.Color.Red;
                        //            cell.Font.Bold = true;
                        //        }
                        //if (dtExcessive.DefaultView.Count > 0)
                        //{
                        //    if (item["PointId"] != null)
                        //    {
                        //        GridTableCell pointCell = (GridTableCell)item["PointId"];
                        //        IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                        //        DataRow[] drExcessive = dtExcessive.Select("PointID='" + point.PointID + "' and PollutantCode='" + factor.PollutantCode + "'");
                        //        if (drExcessive.Count() > 0)
                        //        {
                        //            cell.ToolTip = "上限：" + drExcessive[0]["excessiveUpper"] + "\n下限：" + drExcessive[0]["excessiveLow"] + "\n标记位：" + siteTypeName.TrimEnd('|');
                        //        }
                        //        else
                        //        {
                        //            cell.ToolTip = "上限：-- \n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    cell.ToolTip = "上限：-- \n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(ExcessiveSettingInfo)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (ExcessiveSettingInfo item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(ExcessiveSettingInfo)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }
        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridOriginal.CurrentPageIndex = 0;
            points = pointCbxRsm.GetPoints();
            if (points.Count > 1 && IsStatistical.Checked == true)
            {
                Alert("只统计单工位号数据，请不要勾选统计行");
            }
            else
            {
                gridOriginal.Rebind();
            }
            if (tabStrip.SelectedTab.Text == "图表")
            {
                BindChart();
            }
            else
            {
                FirstLoadChart.Value = "1";
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
                //数据类型对应接口初始化
                g_IInfectantDALService = MonitoringDataWater.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue));
                string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                factors = factorCbxRsm.GetFactors();
                DateTime dtBegion = dtpBegin.SelectedDate.Value;
                DateTime dtEnd = dtpEnd.SelectedDate.Value;
                DataView dv = new DataView();
                if (radlDataType.SelectedValue == "Min1")
                {
                    dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:mm:ss"));
                    dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                    dv = g_IInfectantDALService.GetExportData(portIds, factors, dtBegion, dtEnd);
                }
                else if (radlDataType.SelectedValue == "Min60")
                {
                    dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:mm"));
                    dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:mm"));
                    dv = g_IInfectantDALService.GetExportData(portIds, factors, dtBegion, dtEnd);
                }
                else if (radlDataType.SelectedValue == "Day")
                {
                    dtBegion = dayBegin.SelectedDate.Value;
                    dtEnd = dayEnd.SelectedDate.Value;
                    dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd 00:00:00"));
                    dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd 23:59:59"));
                    dv = g_InfectantBy60Service.GetDayExportData(portIds, factors, dtBegion, dtEnd, "PointId");
                    dv.Sort = "PointId,日期 desc";
                }
                DataTable dtNew = dv.ToTable();
                dtNew.Columns.Add("PointName", typeof(string));
                if (IsStatistical.Checked && factors.Count != 0)
                {
                    dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
                }
                DataTable dt = UpdateExportColumnName(dtNew, dvStatistical);
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PointId"] != DBNull.Value && dr["PointId"].ToString() != "")
                    {
                        string pointid = dr["PointId"].ToString();
                        IPoint point = pointCbxRsm.GetPoints().FirstOrDefault(x => x.PointID.Equals(pointid));
                        dr["PointName"] = point.PointName;
                    }
                }
                dt.Columns["PointName"].SetOrdinal(0);
                dt.Columns["PointName"].ColumnName = "工位号";
                dt.Columns.Remove("PointId");
                if (radlDataType.SelectedValue != "Day")
                {
                    dt.Columns.Remove("序号");
                }
                ExcelHelper.DataTableToExcel(dt, "历史数据查询", "历史数据查询", this.Page);
            }
        }
        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <param name="dvStatistical">合计行数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataTable dt, DataView dvStatistical)
        {
            DataTable dtNew = dt;
            if (dvStatistical != null && dvStatistical.Table.Rows.Count > 0)
            {
                DataTable dtStatistical = dvStatistical.Table;
                DataRow drMaxRow = dtNew.NewRow();
                drMaxRow["PointName"] = "最大值";
                DataRow drMinRow = dtNew.NewRow();
                drMinRow["PointName"] = "最小值";
                DataRow drAvgRow = dtNew.NewRow();
                drAvgRow["PointName"] = "平均值";
                for (int i = 0; i < dtStatistical.Rows.Count; i++)
                {
                    DataRow drStatistical = dtStatistical.Rows[i];
                    if (drStatistical["PollutantCode"] != DBNull.Value && drStatistical["PollutantCode"].ToString() != "")
                    {
                        IPollutant factor = factorCbxRsm.GetFactors().FirstOrDefault(x => x.PollutantCode.Equals(drStatistical["PollutantCode"].ToString()));
                        int pdn = 0;
                        if (factor != null)
                        {
                            pdn = Convert.ToInt32(string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                        }
                        if (dtNew.Columns.Contains(factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"))
                        {
                            drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Max"] != DBNull.Value ? drStatistical["Value_Max"] : "--";
                            drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Min"] != DBNull.Value ? drStatistical["Value_Min"] : "--";
                            drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Avg"] != DBNull.Value ? drStatistical["Value_Avg"] : "--";

                            if (drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
                            {
                                decimal AVG = Convert.ToDecimal(drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
                                drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(AVG, pdn).ToString();
                            }
                            if (drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
                            {
                                decimal MAX = Convert.ToDecimal(drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
                                drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(MAX, pdn).ToString();
                            }
                            if (drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
                            {
                                decimal MIN = Convert.ToDecimal(drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
                                drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(MIN, pdn).ToString();
                            }
                        }
                    }
                }
                dtNew.Rows.Add(drAvgRow);
                dtNew.Rows.Add(drMaxRow);
                dtNew.Rows.Add(drMinRow);
            }
            return dtNew;
        }
        /// <summary>
        /// 图表类型选择（折线图、柱形图、点状图）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenChartType.Value = ChartType.SelectedValue;
            RegisterScript("ChartTypeChanged('" + ChartType.SelectedValue + "');");
        }
        #endregion

        /// <summary>
        /// grid 创建列事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridOriginal_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                if (col.DataField == "PointId")
                {
                    col.HeaderText = "工位号";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterText = "平均值<br>最大值<br>最小值";
                    col.HeaderStyle.Width = Unit.Pixel(100);
                    col.ItemStyle.Width = Unit.Pixel(100);
                }
                else if (col.DataField == "Tstamp")
                {
                    string tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Day)
                        tstcolformat = "{0:yyyy-MM-dd}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Min60)
                    {
                        col = (GridDateTimeColumn)e.Column;
                        tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    }
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Min5)
                    {
                        col = (GridDateTimeColumn)e.Column;
                        tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    }
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Min1)
                    {
                        col = (GridDateTimeColumn)e.Column;
                        tstcolformat = "{0:yyyy-MM-dd HH:mm:ss}";
                    }
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "DateTime")
                {
                    string tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Day)
                        tstcolformat = "{0:yyyy-MM-dd}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
                    if (factor.PollutantName == "流向")
                    {
                        col.HeaderText = string.Format("{0}", factor.PollutantName);
                    }
                    else
                    {
                        col.HeaderText = string.Format("{0}<br>({1})", factor.PollutantName, factor.PollutantMeasureUnit);
                    }
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    SetGridFooterText(col);
                }
                else
                {
                    e.Column.Visible = false;
                }

            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 统计行事件
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
                    if (factors != null)
                    {
                        foreach (IPollutant p in factors)
                        {
                            if (p.PollutantCode.Equals(col.DataField))
                            {
                                IPollutant factor = p;
                                if (avg != "--")
                                {
                                    decimal AVG = Convert.ToDecimal(avg);
                                    avg = DecimalExtension.GetPollutantValue(AVG, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum)).ToString();
                                }
                                if (max != "--")
                                {
                                    decimal MAX = Convert.ToDecimal(max);
                                    max = DecimalExtension.GetPollutantValue(MAX, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum)).ToString();
                                }
                                if (min != "--")
                                {
                                    decimal MIN = Convert.ToDecimal(min);
                                    min = DecimalExtension.GetPollutantValue(MIN, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum)).ToString();
                                }
                            }
                        }
                    }

                }
                col.FooterText = string.Format("{0}<br>{1}<br>{2}", avg, max, min);
                col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            }
        }
        /// <summary>
        /// 站点因子联动
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
            points = pointCbxRsm.GetPoints();
            InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
            IList<string> list = new List<string>();
            string[] factor;
            string factors = string.Empty;
            foreach (IPoint point in points)
            {
                IQueryable<PollutantCodeEntity> p = m_InstrumentChannelService.RetrieveChannelListByPointUid(point.PointGuid);
                list = list.Union(p.Select(t => t.PollutantName)).ToList();
            }
            factor = list.ToArray();
            foreach (string f in factor)
            {
                factors += f + ";";
            }
            factorCbxRsm.SetFactorValuesFromNames(factors);
        }

        protected void ShowType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindChart();
        }

        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbHour.Style["display"] = "none";
            tbDay.Style["display"] = "none";
            //IsStatistical.Visible = false;
            switch (radlDataType.SelectedValue)
            {
                case "Min1":
                case "Min60":
                    tbHour.Style["display"] = "normal";
                    break;
                //case "Day":
                //    tbDay.Style["display"] = "normal";
                //    IsStatistical.Visible = false;
                //    break;
            }
        }

    }
}