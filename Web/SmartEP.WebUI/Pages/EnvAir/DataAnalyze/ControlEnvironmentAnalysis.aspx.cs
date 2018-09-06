﻿using SmartEP.Utilities.Office;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.AutoMonitoring.Interfaces;
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
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.DomainModel.BaseData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.BaseData.BusinessRule;
using System.Collections;
using System.ComponentModel;
using SmartEP.Core.Generic;
using SmartEP.Service.Frame;
using SmartEP.DomainModel;
using SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest;
using SmartEP.Utilities.Web.UI;
using Newtonsoft.Json;


namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：ControlEnvironmentAnalysis.aspx.cs
    /// 创建人：
    /// 创建日期：2016-07-08
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：对照环境分析
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class ControlEnvironmentAnalysis : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private IInfectantDALService g_IInfectantDALService = null;
        //MonitoringPointService m_MonitoringPointService = new MonitoringPointService();
        InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
        //获取因子上下限数据处理服务
        ExcessiveSettingService m_ExcessiveSettingService = new ExcessiveSettingService();
        //代码项服务层
        DictionaryService dicService = new DictionaryService();
        //当天日期
        private DateTime _currentDate = System.DateTime.Now.Date;
        //private DateTime _currentDate = Convert.ToDateTime("2016-05-09");

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 上下限
        /// </summary>
        DataTable dtExcessive = null;
        /// <summary>
        /// 国家标记位
        /// </summary>
        IQueryable<V_CodeMainItemEntity> siteTypeEntites = null;
        /// <summary>
        /// 统计
        /// </summary>
        DataView dvStatistical = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
                txt_a.Text = "0.5";
                txt_b.Text = "0.2";
            }
        }

        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            string PointName = PageHelper.GetQueryString("PointName");
            string factors = PageHelper.GetQueryString("Factors");
            if (PointName != "")
            {
                pointCbxRsm.SetPointValuesFromNames(PointName);
                factorCbxRsm.SetFactorValuesFromNames(factors);
            }
            else
            {

                //国控点，对照点，路边站
                MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
                string strpointName = "";
                IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
                string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad" || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca").Select(p => p.MonitoringPointName).ToArray();
                foreach (string point in EnableOrNotportsarry)
                {
                    strpointName += point + ";";
                }
                pointCbxRsm.SetPointValuesFromNames(strpointName);
                string pollutantName = System.Configuration.ConfigurationManager.AppSettings["AirPollutant"];
                factorCbxRsm.SetFactorValuesFromNames(pollutantName);
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
            g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(PollutantDataType.Min60.ToString()));

            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            DateTime dtBegion = _currentDate.AddDays(-6);
            DateTime dtEnd = _currentDate.AddDays(1).AddSeconds(-1);

            dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:mm"));
            dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:mm"));

            points = pointCbxRsm.GetPoints();
            string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
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

            decimal aRatio = Convert.ToDecimal(txt_a.Text); //计算用的系数
            decimal bRatio = Convert.ToDecimal(txt_b.Text);//计算用的系数
            //【给隐藏域赋值，用于显示Chart】
            SetHiddenData(portIds, factors, _currentDate, aRatio, bRatio);


            if (portIds != null && factors != null)
            {
                //IQueryable<MonitoringPointEntity> pointList = m_MonitoringPointService.RetrieveListByPointIds(portIds);
                //var monitorData = g_IInfectantDALService.GetDataPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderby);
                DataView monitorDv = GetDataSource(portIds, factors, dtBegion, dtEnd);
                monitorDv.Sort = "PointId,Tstamp";
                gridOriginal.DataSource = monitorDv;
            }
            else
            {
                gridOriginal.DataSource = null;
            }

            //获取上下限
            string DataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
            IQueryable<ExcessiveSettingInfo> Excessive = m_ExcessiveSettingService.RetrieveListByPointAndFactor(ApplicationValue.Air, factorCodes, portIds, DataTypeUid);
            dtExcessive = ConvertToDataTable(Excessive);
            //国家数据标记位
            siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "国家数据标记");
        }

        /// <summary>
        /// 取得数据源
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegion"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        private DataView GetDataSource(string[] portIds, IList<IPollutant> factors, DateTime dtBegion, DateTime dtEnd)
        {
            SmartEP.Service.AutoMonitoring.Air.InfectantBy60Service InfectantBy60BLL = new InfectantBy60Service();
            DataView currentYearDv = new DataView(); // 存储今年当前日期,各个测点7天的日均值数据
            DataView lastYearStatisticsDv = new DataView(); // 存储去年当前日期,前后各七天, 合计15天的统计数据
            DataView currentYearStatisticsDv = new DataView(); // 存储今年当前日期,各个测点7天的统计数据
            currentYearDv = InfectantBy60BLL.GetDayAvgData(portIds, factors, dtBegion, dtEnd);

            currentYearStatisticsDv = InfectantBy60BLL.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
            lastYearStatisticsDv = InfectantBy60BLL.GetStatisticalData(portIds, factors, dtBegion.AddYears(-1).AddDays(-1), dtEnd.AddYears(-1).AddDays(7));
            //开始计算各站点, 各因子的明天预测值的上限和下限
            //2016-7-8下限 = （同比数据*a+环比数据*（1-a））*（1-b）
            //2016-7-8上限 = （同比数据*a+环比数据*（1-a））*（1+b）

            //最终结果是一个范围，如：6.23~6.89
            //同比数据计算方式=2015-6-30~2015-7-14天的小时数据的均值（同比的前后7天+当天数据）
            //环比数据计算方式=2016-7-1~2016-7-7的小时数据的均值（前7天数据）

            //a默认0.5
            //b默认0.2
            DataTable currentYearDt = currentYearDv.Table;
            decimal aRatio = Convert.ToDecimal(txt_a.Text); //计算用的系数
            decimal bRatio = Convert.ToDecimal(txt_b.Text);//计算用的系数
            foreach (string strPortId in portIds)
            {
                int intPortId = Convert.ToInt32(strPortId);
                DataRow dr = currentYearDt.NewRow();
                DataRow dr2 = currentYearDt.NewRow();
                dr["PointId"] = intPortId;
                dr2["PointId"] = intPortId;
                dr["Tstamp"] = _currentDate.AddDays(1);
                dr2["Tstamp"] = _currentDate.AddDays(1);
                decimal? lastYearAvg = null; //同比数据
                decimal? currentYearAvg = null; //环比数据
                decimal? upperLimit = null; //明天预测值的上限
                decimal? lowwerLimit = null;//明天预测值的下限
                foreach (IPollutant iPollutant in factors)
                {
                    lastYearAvg = null;
                    currentYearAvg = null;
                    upperLimit = null;
                    lowwerLimit = null;
                    lastYearStatisticsDv.RowFilter = string.Format("PointId={0} AND PollutantCode='{1}'", intPortId, iPollutant.PollutantCode);
                    if (lastYearStatisticsDv.Count > 0 && lastYearStatisticsDv[0]["Value_Avg"] != DBNull.Value)
                    {
                        lastYearAvg = Convert.ToDecimal(lastYearStatisticsDv[0]["Value_Avg"].ToString());
                    }

                    currentYearStatisticsDv.RowFilter = string.Format("PointId={0} AND PollutantCode='{1}'", intPortId, iPollutant.PollutantCode);
                    if (currentYearStatisticsDv.Count > 0 && currentYearStatisticsDv[0]["Value_Avg"] != DBNull.Value)
                    {
                        currentYearAvg = Convert.ToDecimal(lastYearStatisticsDv[0]["Value_Avg"].ToString());
                    }
                    if (lastYearAvg != null)
                    {
                        //2016-7-8下限 = （同比数据*a+环比数据*（1-a））*（1-b）
                        //2016-7-8上限 = （同比数据*a+环比数据*（1-a））*（1+b）
                        upperLimit = (lastYearAvg * aRatio + currentYearAvg * (1 - aRatio)) * (1 - bRatio);
                        lowwerLimit = (lastYearAvg * aRatio + currentYearAvg * (1 - aRatio)) * (1 + bRatio);
                    }
                    if (upperLimit != null)
                        dr[iPollutant.PollutantCode] = upperLimit;
                    if (lowwerLimit != null)
                        dr2[iPollutant.PollutantCode] = lowwerLimit;
                }
                currentYearDt.Rows.Add(dr);
                currentYearDt.Rows.Add(dr2);
            }
            DataView myDv = currentYearDt.DefaultView;
            myDv.Sort = "PointId,Tstamp";
            return myDv;
        }

        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        //public void BindGrid2()
        //{
        //    //if (!IsPostBack)
        //    //{
        //    //    pointCbxRsm_SelectedChanged();
        //    //}
        //    //数据类型对应接口初始化
        //    g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(PollutantDataType.Min60.ToString()));

        //    string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
        //    DateTime dtBegion = _currentDate.AddDays(-6);
        //    DateTime dtEnd = _currentDate.AddDays(1).AddSeconds(-1);

        //    dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:mm"));
        //    dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:mm"));

        //    points = pointCbxRsm.GetPoints();
        //    string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
        //    factors = factorCbxRsm.GetFactors();

        //    //生成RadGrid的绑定列
        //    dvStatistical = null;
        //    //是否显示统计行
        //    if (IsStatistical.Checked && factors.Count != 0)
        //    {
        //        gridOriginal.ShowFooter = true;
        //        dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
        //    }
        //    else
        //    {
        //        gridOriginal.ShowFooter = false;
        //    }

        //    //每页显示数据个数            
        //    int pageSize = gridOriginal.PageSize;
        //    //当前页的序号
        //    int pageNo = gridOriginal.CurrentPageIndex;

        //    //【给隐藏域赋值，用于显示Chart】
        //    SetHiddenData(portIds, factors, dtBegion, dtEnd);

        //    //数据总行数
        //    int recordTotal = 0;
        //    string orderby = "PointId,Tstamp desc";
        //    if (TimeSort.SelectedValue == "时间升序")
        //        orderby = "PointId,Tstamp asc";
        //    if (portIds != null && factors != null)
        //    {
        //        //IQueryable<MonitoringPointEntity> pointList = m_MonitoringPointService.RetrieveListByPointIds(portIds);
        //        var monitorData = g_IInfectantDALService.GetDataPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderby);
        //        //DataTable dt = monitorData.Table;
        //        //dt.Columns.Add("OrderByNum", typeof(int));
        //        //foreach (string portId in portIds)
        //        //{
        //        //    int portid = int.TryParse(portId, out portid) ? portid : 0;
        //        //    int?[] pointlist = pointList.Where(t => t.PointId == portid).Select(t => t.OrderByNum).ToArray();
        //        //    DataRow[] dtRow = dt.Select("PointId=" + portId);
        //        //    if (pointlist.IsNotNullAndNotEmpty())
        //        //    {
        //        //        foreach (DataRow dtrow in dtRow)
        //        //        {
        //        //            dtrow["OrderByNum"] = pointlist[0];
        //        //        }
        //        //    }
        //        //}
        //        //DataView dv = dt.DefaultView;
        //        //dv.Sort = "OrderByNum";
        //        gridOriginal.DataSource = monitorData;
        //    }
        //    else
        //    {
        //        gridOriginal.DataSource = null;
        //    }
        //    //数据总行数
        //    gridOriginal.VirtualItemCount = recordTotal;
        //    //获取上下限
        //    string DataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
        //    IQueryable<ExcessiveSettingInfo> Excessive = m_ExcessiveSettingService.RetrieveListByPointAndFactor(ApplicationValue.Air, factorCodes, portIds, DataTypeUid);
        //    dtExcessive = ConvertToDataTable(Excessive);
        //    //国家数据标记位
        //    siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "国家数据标记");
        //}
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

        private void BindChart()
        {
            if (ShowType.Text.Equals("分屏"))
            {
                RegisterScript("InitGroupChart();");
            }
            else if (ShowType.Text.Equals("合并"))
            {
                RegisterScript("InitTogetherChart();");
            }
        }

        /// <summary>
        /// 页面隐藏域控件赋值，将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void SetHiddenData(string[] portIds, IList<IPollutant> factors, DateTime currentDate, decimal aRatio, decimal bRatio)
        {
            if (factors.Count > 0 && portIds != null)
            {
                string strJsonFactors = Newtonsoft.Json.JsonConvert.SerializeObject(factors);
                //HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                //                 + "|" + dtBegin + "|" + dtEnd + "|" + PollutantDataType.Min60.ToString() + "|Air";
                HiddenData.Value = string.Join(";", portIds) + "|" + strJsonFactors
                            + "|" + PollutantDataType.Min60.ToString() + "|Air|" + currentDate + "|" + aRatio + "|" + bRatio;
                HiddenChartType.Value = ChartType.SelectedValue;
            }
        }


        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridOriginal_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //string DataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
            //string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            //string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            //IQueryable<ExcessiveSettingInfo> Excessive = m_ExcessiveSettingService.RetrieveListByPointAndFactor(ApplicationValue.Air, factorCodes, portIds, DataTypeUid);
            //DataTable dtExcessive = ConvertToDataTable(Excessive);
            ////国家数据标记位
            //IQueryable<V_CodeMainItemEntity> siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "国家数据标记");

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["tstamp"].Text == _currentDate.AddDays(1).ToString("yyyy-MM-dd"))
                    item.Style.Add("background-color", "#a9b4f0");
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                    if (points != null)
                        pointCell.Text = point.PointName;
                }
                for (int iRow = 0; iRow < factors.Count; iRow++)
                {
                    string siteTypeName = "--";//标记位名称
                    IPollutant factor = factors[iRow];
                    GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                    string factorStatus = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                    string factorMark = drv[factor.PollutantCode + "_Mark"] != DBNull.Value ? drv[factor.PollutantCode + "_Mark"].ToString() : string.Empty;
                    decimal value = 0M;
                    if (decimal.TryParse(cell.Text, out value))
                    {
                        if (factor.PollutantMeasureUnit == "μg/m3")
                        {
                            value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);

                        }
                        else
                        {
                            value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                        }
                        cell.Text = value.ToString("");
                        if ((factorStatus != "N" && !string.IsNullOrEmpty(factorStatus)) || !string.IsNullOrEmpty(factorMark))
                        {
                            string markContent = string.Empty;
                            if (factorStatus != "N" && !string.IsNullOrEmpty(factorStatus))
                            {
                                markContent += factorStatus + "|";
                                siteTypeName = factorStatus + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorStatus))
                .Select(t => t.ItemText).FirstOrDefault() + ")" + "|";//标记位名称
                            }
                            if (!string.IsNullOrEmpty(factorMark))
                            {
                                markContent += factorMark;
                                if (siteTypeName == "--")
                                {
                                    siteTypeName = factorMark + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorMark))
                          .Select(t => t.ItemText).FirstOrDefault() + ")";//标记位名称
                                }
                                else
                                {
                                    siteTypeName += factorMark + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorMark))
                          .Select(t => t.ItemText).FirstOrDefault() + ")";//标记位名称
                                }
                            }
                            markContent = markContent.TrimEnd('|');
                            cell.Text = cell.Text + "(" + markContent + ")";
                            cell.ForeColor = System.Drawing.Color.Red;
                            cell.Font.Bold = true;
                        }
                        if (dtExcessive.DefaultView.Count > 0)
                        {
                            if (item["PointId"] != null)
                            {
                                GridTableCell pointCell = (GridTableCell)item["PointId"];
                                IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                                DataRow[] drExcessive = dtExcessive.Select("PointID='" + point.PointID + "' and PollutantCode='" + factor.PollutantCode + "'");
                                if (drExcessive.Count() > 0)
                                {
                                    cell.ToolTip = "上限：" + drExcessive[0]["excessiveUpper"] + "\n下限：" + drExcessive[0]["excessiveLow"] + "\n标记位：" + siteTypeName.TrimEnd('|');
                                }
                                else
                                {
                                    cell.ToolTip = "上限：-- \n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                                }
                            }
                        }
                        else
                        {
                            cell.ToolTip = "上限：-- \n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                        }
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
            gridOriginal.Rebind();
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
                //g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(PollutantDataType.Min60.ToString()));
                string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
                factors = factorCbxRsm.GetFactors();
                DateTime dtBegion = _currentDate.AddDays(-6);
                DateTime dtEnd = _currentDate.AddDays(1).AddSeconds(-1);

                dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:mm"));
                dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:mm"));

                //DataView dv = g_IInfectantDALService.GetExportData(portIds, factors, dtBegion, dtEnd);
                //DataTable dtNew = dv.ToTable();
                DataTable dtNew = GetDataSource(portIds, factors, dtBegion, dtEnd).ToTable();
                dtNew.Columns.Add("PointName", typeof(string));
                //删除状态列, 并将因子列改名
                foreach (IPollutant item in factors)
                {
                    dtNew.Columns[item.PollutantCode].ColumnName = item.PollutantName + "(" + item.PollutantMeasureUnit + ")";
                    dtNew.Columns.Remove(item.PollutantCode + "_Status");
                    dtNew.Columns.Remove(item.PollutantCode + "_Mark");
                }
               
                if (IsStatistical.Checked && factors.Count != 0)
                {
                    dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
                }
                DataTable dt = UpdateExportColumnName(dtNew, dvStatistical);
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PointId"] != DBNull.Value)
                    {
                        string pointid = dr["PointId"].ToString();
                        IPoint point = pointCbxRsm.GetPoints().FirstOrDefault(x => x.PointID.Equals(pointid));
                        dr["PointName"] = point.PointName;
                    }

                    foreach (IPollutant item in factors)
                    {
                        if (item.PollutantMeasureUnit == "μg/m3")
                        {
                            if (!string.IsNullOrWhiteSpace(dr[item.PollutantName + "(" + item.PollutantMeasureUnit + ")"].ToString()))
                            {
                                dr[item.PollutantName + "(" + item.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(decimal.Parse(dr[item.PollutantName + "(" + item.PollutantMeasureUnit + ")"].ToString()) * 1000, string.IsNullOrEmpty(item.PollutantDecimalNum) ? 0 : Convert.ToInt32(item.PollutantDecimalNum) - 3);
                            }
                        }
                    }
                }
                dt.Columns["PointName"].SetOrdinal(0);
                dt.Columns["PointName"].ColumnName = "测点名称";
                dt.Columns["Tstamp"].ColumnName = "日期";
                dt.Columns.Remove("PointId");
                //dt.Columns.Remove("序号");
                ExcelHelper.DataTableToExcel(dt, "趋势分析", "趋势分析", this.Page);
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
                    col.HeaderText = "测点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterText = "平均值<br>最大值<br>最小值";
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Tstamp")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:yyyy-MM-dd}";
                    //if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(PollutantDataType.Min60.ToString()) == PollutantDataType.Min60)
                    //    tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    //if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(PollutantDataType.Min60.ToString()) == PollutantDataType.Min5)
                    //    tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    //if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(PollutantDataType.Min60.ToString()) == PollutantDataType.Min1)
                    //    tstcolformat = "{0:yyyy-MM-dd HH:mm:ss}";
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
                    string unit = factor.PollutantMeasureUnit;
                    string strName = factor.PollutantName;
                    if (strName == "PM2.5")
                    {
                        strName = "PM<sub>2.5</sub>";
                    }
                    if (strName == "PM10")
                    {
                        strName = "PM<sub>10</sub>";
                    }
                    if (unit == "mg/m3")
                    {
                        unit = "mg/m<sup>3</sup>";
                    }
                    col.HeaderText = string.Format("{0}<br>({1})", strName, unit);
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
                        IPollutant factor = factors[0];
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

        /// <summary>
        /// 日期控件格式 
        /// </summary>
        //protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (radlDataType.SelectedValue == "Min1")
        //    {
        //        dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm:ss";
        //        dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm:ss";
        //    }
        //    else if (radlDataType.SelectedValue == "Min5")
        //    {
        //        dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
        //        dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
        //    }
        //    else
        //    {
        //        dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
        //        dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
        //    }
        //}

        protected void ShowType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindChart();
        }
    }
}