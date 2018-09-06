﻿using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.Services;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Core.Generic;
using SmartEP.Utilities.Web.UI;
using SmartEP.DomainModel;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.BaseData.BusinessRule;
using System.ComponentModel;
using System.Collections;
using SmartEP.Service.Frame;
using SmartEP.Service.DataAnalyze.Common;

namespace SmartEP.WebUI.Pages.EnvAir.RealTimeData
{
    /// <summary>
    /// 名称：RealTimeOnlineState.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-08-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：实时在线状态信息
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class RealTimeOnlineState : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        AirRealTimeOnlineStateService airRealTimeOnlineState = null;

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 获取站点名称
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

        /// <summary>
        /// 获取因子小数位 channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
        /// </summary>
        SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = new SmartEP.Service.BaseData.Channel.AirPollutantService();

        /// <summary>
        /// 上下限
        /// </summary>
        DataTable dtExcessive = null;

        /// <summary>
        /// 国家标记位
        /// </summary>
        IQueryable<V_CodeMainItemEntity> siteTypeEntites = null;

        /// <summary>
        /// 获取因子上下限数据处理服务
        /// </summary>
        ExcessiveSettingService m_ExcessiveSettingService = new ExcessiveSettingService();

        /// <summary>
        /// 代码项服务层
        /// </summary>
        DictionaryService dicService = new DictionaryService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                InitControl();
            }
        }
        /// <summary>
        /// 设置站点控件选中超级站
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            string isAudit = "1";
            if (isAudit != null && isAudit != "")
            {
                factorCbxRsm.isAudit(isAudit);
                pointCbxRsm.isSuper("AirDER");
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //从首页原件传过来的参数
            string pointIds = PageHelper.GetQueryString("pointNames");
            string factorCodes = PageHelper.GetQueryString("FactorName");
            string modle = PageHelper.GetQueryString("Molde");
            string selectedPa = PageHelper.GetQueryString("online");
            switch (selectedPa)
            {
                case "0": weekTo.Items[1].Selected = true; break;
                case "1": weekTo.Items[2].Selected = true; break;
                default: weekTo.Items[0].Selected = true; break;
            }
            if (!string.IsNullOrWhiteSpace(pointIds) && !string.IsNullOrWhiteSpace(factorCodes))
            {

                string[] strAirFactors = factorCodes.Split(';');
                string[] strAirPoints = pointIds.Split(';');

                string pointName = "";
                foreach (string pointId in strAirPoints)
                {
                    MonitoringPointEntity monitoringPointEntity = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                    pointName += monitoringPointEntity.MonitoringPointName + ";";
                }
                pointCbxRsm.SetPointValuesFromNames(pointName);

                string factorName = "";
                foreach (string factorCode in strAirFactors)
                {
                    PollutantCodeEntity pollutantCodeEntity = m_AirPollutantService.RetrieveEntityByCode(factorCode);
                    factorName += pollutantCodeEntity.PollutantName + ";";
                }
                factorCbxRsm.SetFactorValuesFromNames(factorName);

            }
            else
            {

                MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
                IQueryable<MonitoringPointEntity> monitoringPointQueryable = m_MonitoringPointAirService.RetrieveAirMPListByEnable();//获取所有启用的空气点位列表
                monitoringPointQueryable = monitoringPointQueryable.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad"     //国控点
                                                                               || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca"  //对照点
                                                                               );//路边站

                IList<PollutantCodeEntity> pollutantCodeList = GetPollutantListByCalAQI();
                //string pollutantNames = pollutantCodeList.Select(t => t.PollutantName).Aggregate((a, b) => a + ";" + b);
                string pollutantName = System.Configuration.ConfigurationManager.AppSettings["AirPollutant"];
                factorCbxRsm.SetFactorValuesFromCodes(pollutantName);
            }

            //数据类型
            radlDataType.Items.Add(new ListItem("五分钟", PollutantDataType.Min5.ToString()));
            radlDataType.Items.Add(new ListItem("一小时", PollutantDataType.Min60.ToString()));

            //是否从首页原件传过来的参数
            if (!string.IsNullOrWhiteSpace(modle))
            {
                if (modle.Equals("Min60"))
                {
                    radlDataType.SelectedValue = PollutantDataType.Min60.ToString();
                }
                else
                {
                    radlDataType.SelectedValue = PollutantDataType.Min5.ToString();
                }
            }
            else
            {
                radlDataType.SelectedValue = PollutantDataType.Min60.ToString();//不是从首页原件传过来的参数默认 一小时数据
            }


        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {


            DataOnlineService s_DataOnlineService = Singleton<DataOnlineService>.GetInstance();
            //airRealTimeOnlineState = new AirRealTimeOnlineStateService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue));

            DataView dvOnlineRate = null;

            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            points = pointCbxRsm.GetPoints();
            string[] portIds = points.Select(t => t.PointID).ToArray();
            int pageSize = gridRealTimeOnlineState.PageSize;//每页显示数据个数  
            int pageNo = gridRealTimeOnlineState.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数
            factors = factorCbxRsm.GetFactors();
            string netWorkType = weekTo.SelectedValue;
            var airRealTimeOnlineData = new DataView();
            DataTable dt = new DataTable();

            if (portIds == null || portIds.Length == 0)
            {
                airRealTimeOnlineData = null;
            }
            else
            {
                PollutantDataType pollutantDataType = SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue);
                //绑定数据
                //airRealTimeOnlineData = airRealTimeOnlineState.GetRealTimeOnlineStateDataPager(netWorkType, portIds, factors.Select(it => it.PollutantCode).ToArray(), pageSize, pageNo, out recordTotal, out dvOnlineRate);
                airRealTimeOnlineData = s_DataOnlineService.GetOnlineInfo(ApplicationType.Air, portIds, factorCodes, pollutantDataType).DefaultView;
                //airRealTimeOnlineData.Sort = "OrderPoint asc,Tstamp desc";
                if (airRealTimeOnlineData != null && airRealTimeOnlineData.Count > 0)
                {
                    decimal OnlineCount = 0;
                    decimal OfflineCount = 0;
                    decimal TotalCount = 0;
                    string OnlineRate = "0%";
                    TotalCount = airRealTimeOnlineData.Count;
                    airRealTimeOnlineData.RowFilter = "NetWorking=1";
                    OnlineCount = airRealTimeOnlineData.Count;
                    airRealTimeOnlineData.RowFilter = "NetWorking=0";
                    OfflineCount = airRealTimeOnlineData.Count;
                    OnlineRate = Math.Round(OnlineCount / TotalCount * 100, 2) + "%";
                    lblNetwork.Text = OnlineRate;
                    lblAllNetwork.Text = TotalCount.ToString();
                    lblOffOnline.Text = OfflineCount.ToString();
                    lblOnline.Text = OnlineCount.ToString();
                }
                else
                {
                    lblNetwork.Text = "0%";
                    lblAllNetwork.Text = "0";
                    lblOffOnline.Text = "0";
                    lblOnline.Text = "0";
                }
                airRealTimeOnlineData.RowFilter = string.Empty;
                //因子保留小数位
                dt = airRealTimeOnlineData.ToTable();
                if (netWorkType == "2")
                {
                    airRealTimeOnlineData.RowFilter = "NetWorking=1";
                    dt = airRealTimeOnlineData.ToTable();
                }
                if (netWorkType == "3")
                {
                    airRealTimeOnlineData.RowFilter = "NetWorking=0";
                    dt = airRealTimeOnlineData.ToTable();
                }
                //string[] FactorCodes = factors.Select(t => t.PollutantCode).ToArray();
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    foreach (string factorCode in FactorCodes)
                //    {
                //        dt.Rows[i][factorCode].ToString();

                //        int DecimalNum = 3;
                //        if (m_AirPollutantService.GetPollutantInfo(factorCode) != null)
                //        {
                //            if (!string.IsNullOrWhiteSpace(m_AirPollutantService.GetPollutantInfo(factorCode).PollutantDecimalNum))
                //            {
                //                DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factorCode).PollutantDecimalNum);
                //            }
                //        }
                //        if (dt.Rows[i][factorCode] != DBNull.Value)
                //        {
                //            //value 需要进行小数位处理的数据 类型Decimal
                //            dt.Rows[i][factorCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factorCode]), DecimalNum);
                //        }
                //    }
                //}
            }
            if (airRealTimeOnlineData == null)
            {
                gridRealTimeOnlineState.DataSource = null;
            }
            else
            {
                if (portIds.Length == 0)
                {
                    dt.Clear();
                    dvOnlineRate.Table.Clear();
                }
                gridRealTimeOnlineState.DataSource = dt.DefaultView;
            }

            //数据总行数
            gridRealTimeOnlineState.VirtualItemCount = dt.Rows.Count;


            //获取上下限
            string DataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
            IQueryable<ExcessiveSettingInfo> Excessive = m_ExcessiveSettingService.RetrieveListByPointAndFactor(ApplicationValue.Air, factorCodes, portIds, DataTypeUid);
            dtExcessive = ConvertToDataTable(Excessive);
            //国家数据标记位
            siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "国家数据标记");

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
            string siteTypeName = "--";//标记位名称
            if (e.Item is GridDataItem)
            {

                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];

                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(drv["PointId"].ToString().Trim()));
                    if (points != null && point != null)
                    {
                        pointCell.Text = point.PointName;
                    }
                }
                //GridTableCell pointCellSuper = (GridTableCell)item["PointId"];
                //超级站
                if (points.FirstOrDefault(x => x.PointID.Equals(drv["PointId"].ToString().Trim())).PointID=="204")
                {
                    if (item["NetWorking"] != null)
                    {
                        GridTableCell pointCell = (GridTableCell)item["NetWorking"];
                        if (drv["NetWorking"].ToString().ToUpper() == "1")
                        {
                            pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/public/On.PNG\" />";
                        }
                        else if (drv["NetWorking"].ToString().ToUpper() == "0")
                        {
                            pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/public/Off.PNG\" />";

                        }
                        else if (string.IsNullOrWhiteSpace(drv["NetWorking"].ToString()))
                        {
                            pointCell.Text = "--";

                        }
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
                            if (value == -10000)
                            {
                                cell.Text = "/";
                            }
                            else
                            {
                                if (factor.PollutantMeasureUnit == "μg/m³")
                                {
                                    value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);

                                }
                                else
                                {
                                    value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                }
                                cell.Text = value.ToString("");
                     //           if ((factorStatus != "N" && !string.IsNullOrEmpty(factorStatus)) || !string.IsNullOrEmpty(factorMark))
                     //           {
                     //               string markContent = string.Empty;
                     //               if (factorStatus != "N" && !string.IsNullOrEmpty(factorStatus))
                     //               {
                     //                   markContent += factorStatus + "|";
                     //                   siteTypeName = factorStatus + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorStatus))
                     //.Select(t => t.ItemText).FirstOrDefault() + ")" + "|";//标记位名称

                     //               }
                     //               if (!string.IsNullOrEmpty(factorMark))
                     //               {
                     //                   markContent += factorMark;
                     //                   if (siteTypeName == "--")
                     //                   {
                     //                       siteTypeName = factorMark + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorMark))
                     //             .Select(t => t.ItemText).FirstOrDefault() + ")";//标记位名称
                     //                   }
                     //                   else
                     //                   {
                     //                       siteTypeName += factorMark + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorMark))
                     //             .Select(t => t.ItemText).FirstOrDefault() + ")";//标记位名称
                     //                   }
                     //               }
                     //               markContent = markContent.TrimEnd('|');
                     //               cell.Text = cell.Text + "(" + markContent + ")";
                     //               cell.ForeColor = System.Drawing.Color.Red;
                     //               cell.Font.Bold = true;
                     //           }
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
                                    cell.ToolTip = "上限： --\n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                                }

                            }
                        }
                    }
                }
                else
                {
                    if (item["NetWorking"] != null)
                    {
                        GridTableCell pointCell = (GridTableCell)item["NetWorking"];
                        if (drv["NetWorking"].ToString().ToUpper() == "1")
                        {
                            pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/public/On.PNG\" />";
                        }
                        else if (drv["NetWorking"].ToString().ToUpper() == "0")
                        {
                            pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/public/Off.PNG\" />";

                        }
                        else if (string.IsNullOrWhiteSpace(drv["NetWorking"].ToString()))
                        {
                            pointCell.Text = "--";

                        }
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
                            if (value == -10000)
                            {
                                cell.Text = "/";
                            }
                            else
                            {
                                if (factor.PollutantMeasureUnit == "μg/m³")
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
                                    cell.ToolTip = "上限： --\n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                                }

                            }
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
            gridRealTimeOnlineState.Rebind();
        }

        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void gridRTB_ButtonClick1(object sender, RadToolBarEventArgs e)
        //{
        //    Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
        //    if (button.CommandName == "ExportToExcel")
        //    {

        //    }
        //}
        #endregion

        protected void gridRealTimeOnlineState_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;

                if (col.DataField == "PointId")
                {
                    col.HeaderText = "站点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(90);
                    col.ItemStyle.Width = Unit.Pixel(90);
                }
                else if (col.DataField == "NetWorking")
                {
                    col.HeaderText = "联网状态";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(70);
                    col.ItemStyle.Width = Unit.Pixel(70);
                }
                else if (col.DataField == "NetWorkInfo")
                {
                    col.HeaderText = "联网信息";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(90);
                    col.ItemStyle.Width = Unit.Pixel(90);
                }
                else if (col.DataField == "Tstamp")
                {
                    string tstcolformat = "{0:MM-dd HH:00}";
                    col = col as GridDateTimeColumn;
                    col.HeaderText = "最近时间";
                    col.EmptyDataText = "--";
                    col.DataFormatString = tstcolformat;
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(90);
                    col.ItemStyle.Width = Unit.Pixel(90);
                }
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    string tstcolformat = "{0:0.000}";
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
                    col.HeaderText = string.Format("{0}<br>({1})", factor.PollutantName, factor.PollutantMeasureUnit);
                    col.EmptyDataText = "--";
                    col.DataFormatString = tstcolformat;
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                }
                else if (col.DataField == "SamplingRate")
                {
                    col.Visible = false;

                    col.HeaderText = "捕获率";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }

                else if (col.DataField == "blankspaceColumn")
                {
                    col.HeaderText = string.Empty;
                    col.Visible = false;
                }
                else
                {
                    col.Visible = false;
                }
            }
            catch (Exception ex) { }

        }

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
        /// 获取参与评价AQI的常规6因子
        /// </summary>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantListByCalAQI()
        {
            SmartEP.Service.BaseData.Channel.AirPollutantService airPollutantService = new SmartEP.Service.BaseData.Channel.AirPollutantService();
            return airPollutantService.RetrieveListByCalAQI().ToList();
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

    }
}