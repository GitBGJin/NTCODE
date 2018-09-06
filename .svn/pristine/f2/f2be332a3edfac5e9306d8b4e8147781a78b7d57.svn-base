using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.RealTimeData
{
    /// <summary>
    /// 名称：RealTimeOnlineStateNew.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-12-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：实时在线状态信息
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class RealTimeOnlineStateNew : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        AirRealTimeOnlineStateNewService airRealTimeOnlineStateNew = new AirRealTimeOnlineStateNewService();
        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 获取测点名称
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
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //从首页原件传过来的参数
            string pointIds = PageHelper.GetQueryString("pointNames");
            string factorCodes = PageHelper.GetQueryString("FactorName");
            
            if (!string.IsNullOrWhiteSpace(pointIds) && !string.IsNullOrWhiteSpace(factorCodes))
            {
                //switch (selectedPa)
                //{
                //    case "0": weekTo.Items[1].Selected = true; break;
                //    case "1": weekTo.Items[2].Selected = true; break;
                //    default: weekTo.Items[0].Selected = true; break;
                //}
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
                string pollutantNames = pollutantCodeList.Select(t => t.PollutantName).Aggregate((a, b) => a + ";" + b);

                factorCbxRsm.SetFactorValuesFromNames(pollutantNames);
            }

            //数据类型
            radlDataType.Items.Add(new ListItem("五分钟", "Min5"));
            radlDataType.Items.Add(new ListItem("一小时", "Min60"));

            radlDataType.SelectedValue = "Min60";

        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            DataView dvOnlineRate = null;
            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            points = pointCbxRsm.GetPoints();
            string[] portIds = points.Select(t => t.PointID).ToArray();
            int pageSize = gridRealTimeOnlineState.PageSize;//每页显示数据个数  
            int pageNo = gridRealTimeOnlineState.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数
            factors = factorCbxRsm.GetFactors();
            string netWorkType = weekTo.SelectedValue;
            string dataType = radlDataType.SelectedValue;
            var airRealTimeOnlineData = new DataView();
            DataTable dt = new DataTable();

            if (portIds == null || portIds.Length == 0)
            {
                airRealTimeOnlineData = null;
            }
            else
            {
                Dictionary<string, int> dicStatusCode = new Dictionary<string, int>();
                dicStatusCode.Add("OnlineCount", 1);//在线数
                dicStatusCode.Add("OfflineCount", 0);//离线
                //dicStatusCode.Add("WarnCount", 4);//报警
                //dicStatusCode.Add("FaultCount", 8);//故障
                //dicStatusCode.Add("StopCount", 16);//停运
                //dicStatusCode.Add("AlwaysOnlineCount", 32);//始终在线

                
                airRealTimeOnlineData = airRealTimeOnlineStateNew.GetRealTimeOnlineStateDataPager(portIds, factors.Select(it => it.PollutantCode).ToArray(), netWorkType, dicStatusCode, out dvOnlineRate, dataType, pageSize, pageNo, out recordTotal);
                
                //绑定数据
                //airRealTimeOnlineData = airRealTimeOnlineState.GetRealTimeOnlineStateDataPager(netWorkType, portIds, factors.Select(it => it.PollutantCode).ToArray(), pageSize, pageNo, out recordTotal, out dvOnlineRate);
                //airRealTimeOnlineData.Sort = "OrderPoint asc,Tstamp desc";
                //因子保留小数位
                
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
                gridRealTimeOnlineState.DataSource = airRealTimeOnlineData;
            }

            //数据总行数
            gridRealTimeOnlineState.VirtualItemCount = recordTotal;

            if (dvOnlineRate != null && dvOnlineRate.Count > 0)
            {
                lblNetwork.Text = dvOnlineRate[0]["OnlineRate"].ToString() + "%";
                lblAllNetwork.Text = dvOnlineRate[0]["TotalCount"].ToString();
                lblOffOnline.Text = dvOnlineRate[0]["OfflineCount"].ToString();
                lblOnline.Text = dvOnlineRate[0]["OnlineCount"].ToString();
                
                //lblWarn.Text = dvOnlineRate[0]["WarnCount"].ToString();
                //lblFault.Text = dvOnlineRate[0]["FaultCount"].ToString();
                //lblStop.Text = dvOnlineRate[0]["StopCount"].ToString();
                //lblAlwaysOnline.Text = dvOnlineRate[0]["AlwaysOnlineCount"].ToString();

            }
            else
            {
                lblNetwork.Text = "0%";
                lblAllNetwork.Text = "0";
                lblOffOnline.Text = "0";
                lblOnline.Text = "0";
                
                //lblWarn.Text = "0";
                //lblFault.Text = "0";
                //lblStop.Text = "0";
                //lblAlwaysOnline.Text = "0";
            }

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
                        pointCell.Text = string.Format("<a href='#' style='cursor:pointer;' onclick='onclickData(\"{0}\",\"{1}\")'>{2}</a>", point.PointID, factors.Select(it => it.PollutantCode).ToArray().Aggregate((a, b) => a + ";" + b), point.PointName);

                    }
                }
                if (item["NetWorking"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["NetWorking"];
                    if (drv["NetWorking"].ToString().Equals("1"))
                    {
                        pointCell.Text = "<img style=\"height: 20px; width: 20px;\"  src=\"../../../Resources/images/public/On.PNG\" />";
                    }
                    else if (drv["NetWorking"].ToString().Equals("0"))
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
                        value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
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
                                //IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                                DataRow[] drExcessive = dtExcessive.Select("PointID='" + item["PointId"] + "' and PollutantCode='" + factor.PollutantCode + "'");
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

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridRealTimeOnlineState.Rebind();
        }

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
                    col.HeaderText = "测点";
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
                    string tstcolformat = "{0:MM-dd HH:mm}";
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