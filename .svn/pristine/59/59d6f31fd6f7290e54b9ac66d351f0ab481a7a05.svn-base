using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using SmartEP.Service.DataAuditing.AuditBaseInfo;
using SmartEP.DomainModel.MonitoringBusiness;
using Telerik.Web.UI;
using System.Drawing;
using SmartEP.DomainModel.BaseData;
using System.Data;
using System.Configuration;
using SmartEP.WebUI.Controls;
using SmartEP.Utilities.Web.UI;
using SmartEP.Core.Enums;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.ExtensionMethods;
using SmartEP.Service.Frame;
using SmartEP.DomainModel;
//using SmartEP.Service.Core.Enums;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class MutilPointAuditDataWX : SmartEP.WebUI.Common.BasePage
    {
        #region 属性
        AuditStatusMappingService statusService = new AuditStatusMappingService();
        AuditDataService auditService = new AuditDataService();
        AuditLogService logService = new AuditLogService();
        AuditOperatorService auditDataService = new AuditOperatorService();
        UserService userService = new UserService();
        //DataDealService dataDealService = new DataDealService();
        AirAuditMonitoringPointService pointAirService = new AirAuditMonitoringPointService();//点位接口
        WaterAuditMonitoringPointService pointWaterService = new WaterAuditMonitoringPointService();//点位接口 地表水
        //AuditMonitoringPointPollutantService pollutantService = new AuditMonitoringPointPollutantService();//因子接口
        //string[] portid = { "1" };
        //string[] factorCode = { "a05024" };
        //string[] factorName = { "PM10" };
        #endregion

        #region 方法
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["applicationUID"] == null)
                {
                    if (Request.QueryString["app"] != null && Request.QueryString["app"].Equals("1"))
                        Session["applicationUID"] = "watrwatr-watr-watr-watr-watrwatrwatr";
                    else
                        Session["applicationUID"] = "airaaira-aira-aira-aira-airaairaaira";
                }
                //Session["UserGuid"] = "4ce5bed9-78bd-489f-8b3f-a830098759c4";
                //AuditType：空气（0：国控点；1：市控点位；2：超级站点位；3：省控点位；4：区控点位；5：创模点位;6：中意项目）
                //           地表水（0：水源地；1：城区河道；2：浮标站；3：人工监测点；）     
                if (Request.QueryString["AuditType"] != null)
                    ViewState["AuditType"] = Request.QueryString["AuditType"];
                ViewState["PointID"] = Request.QueryString["PointID"] != null ? Request.QueryString["PointID"].ToString() : "";//点位ID
                InitControl();
            }
        }

        #region 初始化控件
        private void InitControl()
        {
            RadDatePickerBegin.SelectedDate = Request.QueryString["dtBegin"] != null ? Convert.ToDateTime(Request.QueryString["dtBegin"].ToString()) : DateTime.Now.AddDays(-1);
            RadDatePickerEnd.SelectedDate = Request.QueryString["dtEnd"] != null ? Convert.ToDateTime(Request.QueryString["dtEnd"].ToString()) : RadDatePickerBegin.SelectedDate;
            InitAuditType();//初始化审核类型
            PointBind();//初始化点位
            StatusInit();//获取点位审核状态         
            //factorCbxRsm.FactorBind(Convert.ToInt32(RadPortTree.SelectedNode.Value));//绑定SiteMap因子
            //string Pointids = GetPointSelectedValue();
            factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
            //FactorRsmAudit.firstLoad = 2;
            ////国控点不能提交审核
            //if (ViewState["AuditType"].Equals("0") && EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]))
            //{
            //    SubmitAuditTD.Visible = false;
            //}

            #region 地表水添加停电、质控菜单
            //if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))
            //{
            //    try
            //    {
            //        RadMenuData.Items.FindItemByText("停电").Visible = true;
            //        RadMenuData.Items.FindItemByText("质控").Visible = true;
            //        RadMenuAuditLog.Items.FindItemByText("停电").Visible = true;
            //        RadMenuAuditLog.Items.FindItemByText("质控").Visible = true;
            //        ContextMenuChart.Items.FindItemByText("停电").Visible = true;
            //        ContextMenuChart.Items.FindItemByText("质控").Visible = true;
            //    }
            //    catch
            //    {
            //    }
            //}
            #endregion
        }

        #region 点位绑定
        private void PointBind()
        {
            IQueryable<MonitoringPointEntity> pointList = null;
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]))
                pointList = pointAirService.RetrieveAirPointListBySiteType(AuditType.SelectedItem.Value, Session["UserGuid"].ToString());
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))
                pointList = pointWaterService.RetrieveWaterPointList(AuditType.SelectedItem.Value, Session["UserGuid"].ToString());
            radioPoint.DataSource = pointList;
            radioPoint.DataValueField = "PointId";
            radioPoint.DataTextField = "MonitoringPointName";
            radioPoint.DataBind();
            string Pointids = "";
            foreach (ListItem item in radioPoint.Items)
            {

                if (ViewState["PointID"].ToString().Split(';').Contains(item.Value))
                    item.Selected = true;
                if (item.Selected)
                    Pointids += Pointids.Equals("") ? item.Value : ";" + item.Value;
            }
            if (radioPoint.Items.Count > 0 && radioPoint.SelectedItem == null)
            {
                radioPoint.Items[0].Selected = true;
                Pointids = radioPoint.SelectedValue;
            }
            ViewState["PointID"] = Pointids;
        }
        #endregion

        #region 获取点位和审核状态
        public void StatusInit()
        {
            string Pointids = GetPointSelectedValue();
            string status = logService.RerieveAuditState(Pointids.Split(';'), RadDatePickerBegin.SelectedDate.Value, RadDatePickerEnd.SelectedDate.Value, Session["applicationUID"].ToString(), ViewState["pointType"].Equals("2") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira") ? "1" : "0");
            Color color = Color.Black;
            auditState.Text = GetStatusName(status, out color);
            auditState.ForeColor = color;
        }
        #endregion

        /// <summary>
        /// 初始化图表单因子
        /// </summary>
        public void InitChartFactors()
        {
            string selectValue = "";
            if (chartFactorRadio.Items.Count > 0) selectValue = chartFactorRadio.SelectedItem.Value;
            List<PointPollutantInfo> factorList = factorCbxRsm.GetFactors();
            chartFactorRadio.DataSource = factorList;
            chartFactorRadio.DataTextField = "PName";
            chartFactorRadio.DataValueField = "PID";
            chartFactorRadio.DataBind();
            if (factorList.Count > 0)
            {
                if (!selectValue.Equals(""))
                {
                    if (chartFactorRadio.Items.FindByValue(selectValue) != null)
                        chartFactorRadio.Items.FindByValue(selectValue).Selected = true;
                    else
                        chartFactorRadio.Items[0].Selected = true;
                }
                else
                    chartFactorRadio.Items[0].Selected = true;
            }
            if (chartFactorRadio.SelectedItem != null)
                factorNames.Value = chartFactorRadio.SelectedItem.Value + "|" + chartFactorRadio.SelectedItem.Text;//隐藏域赋值，用于显示Chart时AJAX传值用
            else
                factorNames.Value = "||";//隐藏域赋值，用于显示Chart时AJAX传值用
        }

        /// <summary>
        ///  初始化审核类型
        /// </summary>
        private void InitAuditType()
        {
            DictionaryService dicService = new DictionaryService();
            IQueryable<V_CodeMainItemEntity> siteTypeEntites = null;
            List<V_CodeMainItemEntity> list = new List<V_CodeMainItemEntity>();
            if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))
            {
                siteTypeEntites = dicService.RetrieveList(SmartEP.Service.Core.Enums.DictionaryType.Water, "地表水站点类型");
                foreach (V_CodeMainItemEntity item in siteTypeEntites)
                {
                    if (pointWaterService.RetrieveWaterPointList(item.ItemGuid, Session["UserGuid"].ToString()).Count() > 0)
                        list.Add(item);
                }
            }
            else
            {
                siteTypeEntites = dicService.RetrieveList(SmartEP.Service.Core.Enums.DictionaryType.Air, "空气站点类型");
                foreach (V_CodeMainItemEntity item in siteTypeEntites)
                {
                    if (pointAirService.RetrieveAirPointListBySiteType(item.ItemGuid, Session["UserGuid"].ToString()).Count() > 0)
                        list.Add(item);
                }
            }
            AuditType.DataSource = list;
            AuditType.DataTextField = "ItemText";
            AuditType.DataValueField = "ItemGuid";
            AuditType.DataBind();
            if (AuditType.Items.Count > 0)
            {
                if (ViewState["AuditType"] != null)
                    AuditType.Items.FindByValue(ViewState["AuditType"].ToString()).Selected = true;
                else
                    AuditType.Items[0].Selected = true;
            }
            ViewState["AuditType"] = AuditType.SelectedItem.Value;
        }
        #endregion

        #region 事件
        /// <summary>
        /// 数据源绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            gridAuditData.MasterTableView.ClearEditItems();
            //string[] pointid = RadPortTree.SelectedNode.Value.Split(';');
            string[] pointid = GetPointSelectedValue().Split(';');
            //RegisterScript("LoadingData();");
            DataView dv = auditService.RetrieveAuditHourDataMutilPoint(Session["applicationUID"].ToString(), pointid, Session["FactorCode"].ToString().Split(';'), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1), true, IsShowTotal.Checked == true ? true : false);
            gridAuditData.DataSource = dv;
            RegisterScript("ClearSelectedInfo();GridResize();");
            if (dv.Count <= 0)
                gridAuditData.MasterTableView.NoMasterRecordsText = "没有数据";
        }

        /// <summary>
        /// 表格着色，添加单元格鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditData_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                RadGrid myRadGrid = ((RadGrid)sender);
                #region 分组名称
                try
                {
                    if (e.Item is GridGroupHeaderItem)
                    {
                        GridGroupHeaderItem item = e.Item as GridGroupHeaderItem;
                        string pointid = item.DataCell.Text.Replace("PointID:", "").Trim().ToString();
                        if (pointid.Equals("0"))
                            item.DataCell.Text = "统计信息";
                        else
                            item.DataCell.Text = radioPoint.Items.FindByValue(pointid).Text;
                    }
                }
                catch
                {
                }
                #endregion

                #region 着色  ToolpTip  等处理 绑定单元格 onmouseover 事件
                if (e.Item is GridDataItem && myRadGrid.MasterTableView.DataSourceCount > 0)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    foreach (GridColumn col in myRadGrid.MasterTableView.RenderColumns)
                    {
                        #region 着色
                        string CurrUName = col.UniqueName;
                        if (!CurrUName.Equals("column") && !CurrUName.Equals("PointId") && !CurrUName.Equals("PointName") && !CurrUName.Equals("Status") && !CurrUName.Equals("DataDateTime") && col.Visible == true)
                        {
                            DataRowView row = (DataRowView)item.DataItem;
                            string CurrFlag = row[CurrUName + "_AuditFlag"].ToString();
                            int DecimalNum = 3;
                            TableCell cell = item[CurrUName];
                            if (row[CurrUName] == DBNull.Value) cell.Text = "/";//空值用/代替
                            if (row["PointId"].ToString().Equals("0")) continue;//统计行不进行着色
                            //获取小数位数
                            DecimalNum = Session["FactorCode"] != null ? Convert.ToInt32(Session["PollutantDecimalNum"].ToString().Split(';')[Array.IndexOf(Session["FactorCode"].ToString().Split(';'), CurrUName)]) : 3;

                            //获取原始数据值
                            decimal sourceValue = auditService.GetSourcePolltantValue(Session["applicationUID"].ToString(), Convert.ToDateTime(row["DataDateTime"]), Convert.ToInt32(row["PointId"]), CurrUName);
                            cell.ToolTip = sourceValue == -99999 ? "原始值：/" : "原始值：" + DecimalExtension.GetRoundValue(sourceValue, DecimalNum) + "   \r\n";//鼠标悬停显示原始数据
                            #region 绑定单元格 onmouseover 事件
                            item[CurrUName].Attributes.Add("onmouseover", "MouseOver('" + item.ItemIndex + "','" + col.UniqueName + "');");
                            #endregion
                            if (cell.Text.ToString() != "/" && !CurrFlag.Equals(""))
                            {
                                #region 根据ColorTag着色并把日志加载到ToolTip
                                cell.CssClass = "rgBatchChanged";
                                if (CurrFlag.Contains("RM"))//自动或人工置为无效
                                {
                                    cell.ToolTip += "置为无效";
                                }
                                else if (CurrFlag.Contains("PF"))//置为停电
                                {
                                    cell.ToolTip += "置为停电";
                                }
                                else if (CurrFlag.Contains("QC"))//置为质控
                                {
                                    cell.ToolTip += "置为质控";
                                }
                                else if (CurrFlag.Equals("C"))//修改过
                                {
                                    cell.ToolTip += "修改";
                                }
                                else
                                {
                                    string status = "";
                                    if (CurrFlag.Contains("HSp") || CurrFlag.Contains("LSp"))//超标
                                    {
                                        status += ",超标异常";
                                        cell.BackColor = Color.FromName(getAuditConfigColor(0));
                                    }
                                    if (CurrFlag.Contains("UPd"))//倒挂
                                    {
                                        status += ",PM10、PM2.5倒挂";
                                        cell.BackColor = Color.FromName(getAuditConfigColor(1));
                                    }
                                    if (CurrFlag.Split(',').Contains("H"))//有效数据不足
                                    {
                                        status += ",有效数据不足";
                                    }
                                    if (CurrFlag.Contains("Rep"))//重复异常
                                    {
                                        status += ",重复异常";
                                    }
                                    if (CurrFlag.Contains("Neg"))//负值异常
                                    {
                                        status += ",负值异常";
                                    }
                                    if (CurrFlag.Contains("Out"))//离群异常
                                    {
                                        status += ",离群异常";
                                    }
                                    if (!status.Equals(""))
                                        cell.ToolTip += status.Substring(1, status.Length - 1);
                                }
                                #endregion
                            }
                        }
                        else if (CurrUName.Equals("PointName") && col.Visible == true) //获取点位名称
                        {
                            TableCell cell = item[CurrUName];
                            DataRowView row = (DataRowView)item.DataItem;
                            cell.Text = radioPoint.Items.FindByValue(row["PointId"].ToString()).Text;
                        }
                        else if (CurrUName.Equals("Status") && col.Visible == true)//获取审核状态
                        {
                            TableCell cell = item[CurrUName];
                            DataRowView row = (DataRowView)item.DataItem;
                            AuditStatusForDayEntity status = logService.RerieveAuditState(Convert.ToInt32(row["PointID"]), Convert.ToDateTime(row["DataDateTime"]).Date, Convert.ToDateTime(row["DataDateTime"]).Date, Session["applicationUID"].ToString());
                            Color color = Color.Black;
                            cell.Text = GetStatusName(status == null ? "" : status.Status, out color);
                            cell.ForeColor = color;
                        }
                        #endregion
                    }
                }
                #endregion
            }
            catch
            {
            }
        }

        /// <summary>
        /// Grid加载时添加Column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditData_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    StatusInit();//获取点位审核状态
            //}
            factorCbxRsm.GetFactors();
            PointIDHidden.Value = GetPointSelectedValue();
            AddColumnsOfGridAuditData();//绑定列

        }

        /// <summary>
        /// 绑定Grid列
        /// </summary>
        private void AddColumnsOfGridAuditData()
        {
            string[] factorCode = Session["FactorCode"].ToString().Trim().Split(';');
            string[] factorName = Session["FactorName"].ToString().Trim().Split(';');
            string[] PollutantDecimalNum = Session["PollutantDecimalNum"].ToString().Trim().Split(';');
            string[] PollutantUnit = Session["PollutantUnit"].ToString().Trim().Split(';');
            string[] IsEdit = Session["IsEdit"].ToString().Trim().Split(';');
            int baseNum = 11;
            #region 按点位分组
            #endregion


            gridAuditData.Columns.Clear();
            gridAuditData.Columns.Add(new GridTemplateColumn() { HeaderText = "点位名称", UniqueName = "PointName" });
            gridAuditData.Columns.Add(new GridTemplateColumn() { HeaderText = "审核状态", UniqueName = "Status" });
            gridAuditData.Columns.Add(new GridBoundColumn() { DataField = "PointID", UniqueName = "PointID", HeaderText = "PointID", Visible = false });
            if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))
            {
                baseNum = 9;
                GridTemplateColumn column = new GridTemplateColumn() { DataField = "DataDateTime", UniqueName = "DataDateTime", HeaderText = "时间点", ItemTemplate = new MyTemplate("DataDateTime", 0, Session["applicationUID"].ToString()) };
                column.HeaderStyle.Width = new Unit(120);
                column.ReadOnly = true;
                gridAuditData.Columns.Add(column);
            }
            else
                gridAuditData.Columns.Add(new GridTemplateColumn() { DataField = "DataDateTime", UniqueName = "DataDateTime", HeaderText = "时间点", ItemTemplate = new MyTemplate("DataDateTime", 0, Session["applicationUID"].ToString()) });
            for (int i = 0; i < factorCode.Length; i++)
            {
                if (!factorCode[i].Equals(""))
                {
                    string unit = "(mg/m<sup>3</sup>)";
                    if (PollutantUnit[i].Equals("mg/L")) unit = "(mg/m<sup>3</sup>)";
                    else unit = "(" + PollutantUnit[i] + ")";
                    GridTemplateColumn col1 = new GridTemplateColumn() { DataField = factorCode[i], UniqueName = factorCode[i], HeaderText = factorName[i] + "</br>" + unit, ItemTemplate = new MyTemplate(factorCode[i], Convert.ToInt32(PollutantDecimalNum[i]), Session["applicationUID"].ToString()), EditItemTemplate = new MyTemplate() };
                    col1.ReadOnly = IsEdit[i].ToString().Equals("1") ? true : false;
                    gridAuditData.Columns.Add(col1);
                }
            }
            if (factorCode.Length > baseNum)
                gridAuditData.MasterTableView.HeaderStyle.Width = new Unit(80);//超过13个因子显示横向滚动条
        }

        /// <summary>
        /// 显示统计信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void IsShowTotal_CheckedChanged(object sender, EventArgs e)
        {
            //gridAuditData.MasterTableView.ClearEditItems();
            gridAuditData.Rebind();
        }

        /// <summary>
        /// 测点切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radioPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Pointids = GetPointSelectedValue();
            if (!Pointids.Equals(""))
            {
                ViewState["PointID"] = Pointids;
                factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
                //InitChartFactors();//初始化图形单因子切换
                //factorCbxRsm.GetFactors();//获取sitemap因子
                //factorNames.Value = Session["FactorCode"].ToString() + "|" + Session["FactorName"].ToString() + "|" + Session["PollutantDecimalNum"].ToString() + "|" + Session["PollutantUnit"].ToString();//隐藏域赋值，用于显示Chart时AJAX传值用
                PointIDHidden.Value = Pointids;
                //RegisterScript("GridResize();LoadingData();");
                //gridAuditData.Rebind();
                //StatusInit();//获取点位审核状态
            }
            else
            {
                Alert("请选择测点！");
                return;
            }
        }

        /// <summary>
        /// 返回日历控件页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back_Click(object sender, EventArgs e)
        {
            int app = 0;
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]))
                app = 0;
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))
                app = 1;
            factorCbxRsm.GetFactors();//获取sitemap因子，放入Session中保存
            String myUrl = "MutilPointAuditStateWX.aspx?Token=" + Request.QueryString["Token"] + "&app=" + app + "&AuditType=" + ViewState["AuditType"] + "&dtBegin=" + RadDatePickerBegin.SelectedDate.Value.Date + "&dtEnd=" + RadDatePickerEnd.SelectedDate.Value.Date + "&PointID=" + ViewState["PointID"];//AuditState.aspx
            Response.Redirect(myUrl, true);
        }

        /// <summary>
        /// 日期重选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadDatePickerBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            //int days = (RadDatePickerEnd.SelectedDate.Value.Date - RadDatePickerBegin.SelectedDate.Value.Date).Days;
            //if (days >= 2) { Alert("时间范围不能超过2天"); return; }
            //{
            //    RadDatePickerBegin.SelectedDate= e.OldDate;
            //    Alert("审核时间超出范围！");
            //    return;
            //}              
            //else
            RegisterScript("GridResize();LoadingData();");
            gridAuditData.Rebind();
            //RegisterScript("LoadingData();");
        }

        /// <summary>
        /// 绑定状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StatusGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                var status = statusService.GetDataStatusMappingList(Session["applicationUID"].ToString());
                StatusGrid.DataSource = status;
                if (status.Count() <= 0)
                    StatusGrid.Visible = false;
                else
                    StatusGrid.Visible = true;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 因子切换
        /// </summary>
        protected void factorCbxRsm_SelectedChanged()
        {
            InitChartFactors();//初始化图形单因子切换
            RegisterScript("GridResize();LoadingData();");
            gridAuditData.Rebind();
        }

        /// <summary>
        /// 审核操作后图表刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void refreshData_Click(object sender, EventArgs e)
        {
            RegisterScript("GridResize();LoadingData();");
            gridAuditData.Rebind();
            //RegisterScript("LoadingData();");
        }

        /// <summary>
        /// 单因子切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chartFactorRadio_SelectedIndexChanged(object sender, EventArgs e)
        {
            factorNames.Value = chartFactorRadio.SelectedItem.Value + "|" + chartFactorRadio.SelectedItem.Text;//隐藏域赋值，用于显示Chart时AJAX传值用
            RegisterScript("GridResize();LoadingData();");
        }

        /// <summary>
        /// 初始化图表单因子切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chartFactorRadio_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitChartFactors();//初始化图形单因子切换
            }
        }

        /// <summary>
        /// 审核类型切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AuditType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PointBind();
            string Pointids = GetPointSelectedValue();
            PointIDHidden.Value = Pointids;
            factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
            //InitChartFactors();//初始化图形单因子切换
            //RegisterScript("GridResize();LoadingData();");
            //gridAuditData.Rebind();
            ViewState["AuditType"] = AuditType.SelectedItem.Value;
            if (Pointids.Equals(""))
            {
                Alert("此类型未配置站点！");
                return;
            }
        }


        /// <summary>
        ///  审核提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitAudit_Click(object sender, EventArgs e)
        {
            //bool hasdata = true;
            //string[] pointid = GetPointSelectedValue().Split(';');
            //if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]))
            //    hasdata = auditService.ExistAirAuditHourData(pointid, Session["FactorCode"].ToString().Split(';'), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1));
            //else
            //    hasdata = gridAuditData.MasterTableView.DataSourceCount > 0 ? true : false;
            //Session["applicationUID"].ToString(), pointid, Session["FactorCode"].ToString().Split(';'),

            //判断国控点生成DBF文件
            bool IsCreateDBF = false;
            if (ViewState["pointType"].Equals("0") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira")) IsCreateDBF = true;
            
            string pointid = GetPointSelectedValue();
            if (!pointid.Equals(""))
            {
                DateTime beginTime = RadDatePickerBegin.SelectedDate.Value.Date;
                DateTime endTime = RadDatePickerEnd.SelectedDate.Value.Date;
                string UserIP = IPAddressExtensions.GetUserIp();
                string UserUid = Session["UserGuid"].ToString();
                string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                bool isSuccess = auditDataService.SubmitAudit(Session["applicationUID"].ToString(), pointid.Split(';'), beginTime, endTime, UserIP, UserUid, CreatUser, false, "", IsCreateDBF, ViewState["pointType"].Equals("2") ?"1" :"0");
                if (isSuccess)
                {
                    RegisterScript("alert(\"提交审核成功！\");");
                    gridAuditData.Rebind();
                    //RegisterScript("LoadingData();");
                    Color color = Color.Black;
                    auditState.Text = GetStatusName("1", out color);
                    auditState.ForeColor = color;
                }
                else
                    RegisterScript("alert(\"提交审核失败！\");");
            }
        }
        #endregion

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search_Click(object sender, EventArgs e)
        {
            //PointBind();
            string Pointids = GetPointSelectedValue();
            //if (Pointids.Equals(""))
            //{
            //    Alert("请选择测点！");
            //    return;
            //}
            PointIDHidden.Value = Pointids;
            //factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
            InitChartFactors();//初始化图形单因子切换
            StatusInit();//获取点位审核状态
            RegisterScript("GridResize();LoadingData();");
            gridAuditData.Rebind();
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void selectAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in radioPoint.Items)
            {
                item.Selected = true;
            }
            string Pointids = GetPointSelectedValue();
            if (!Pointids.Equals(""))
            {
                ViewState["PointID"] = Pointids;
                factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
                InitChartFactors();//初始化图形单因子切换
                PointIDHidden.Value = Pointids;
                RegisterScript("GridResize();LoadingData();");
                gridAuditData.Rebind();
                StatusInit();//获取点位审核状态
            }
            else
            {
                Alert("请选择测点！");
                return;
            }
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void inverse_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in radioPoint.Items)
            {
                if (item.Selected)
                    item.Selected = false;
                else
                    item.Selected = true;
            }
            string Pointids = GetPointSelectedValue();
            if (!Pointids.Equals(""))
            {
                ViewState["PointID"] = Pointids;
                factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
                InitChartFactors();//初始化图形单因子切换
                PointIDHidden.Value = Pointids;
                RegisterScript("GridResize();LoadingData();");
                gridAuditData.Rebind();
                StatusInit();//获取点位审核状态
            }
            else
            {
                Alert("请选择测点！");
                return;
            }
        }

        /// <summary>
        /// 不选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void unselect_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in radioPoint.Items)
            {
                item.Selected = false;
            }
            string Pointids = GetPointSelectedValue();
            if (!Pointids.Equals(""))
            {
                ViewState["PointID"] = Pointids;
                factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
                InitChartFactors();//初始化图形单因子切换
                PointIDHidden.Value = Pointids;
                RegisterScript("GridResize();LoadingData();");
                gridAuditData.Rebind();
                StatusInit();//获取点位审核状态
            }
            else
            {
                Alert("请选择测点！");
                return;
            }
        }
        #endregion

        #region 方法
        #region 获取异常数据颜色
        public String getAuditConfigColor(int ColorTag)
        {
            try
            {
                return ConfigurationManager.AppSettings["AuditGridColors"].ToString().Split(',')[ColorTag];
            }
            catch
            {
                return "#000000";
            }
        }
        #endregion

        /// <summary>
        /// 获取选择的测点值（以;隔开）
        /// </summary>
        /// <returns></returns>
        private string GetPointSelectedValue()
        {
            string Pointids = "";
            foreach (ListItem item in radioPoint.Items)
            {
                if (item.Selected == true)
                    Pointids += Pointids.Equals("") ? item.Value : ";" + item.Value;
            }
            return Pointids;
        }

        /// <summary>
        /// 获取审核状态名称以及颜色
        /// </summary>
        /// <returns></returns>
        private string GetStatusName(string status, out Color color)
        {
            string statusName = "无数据";
            color = Color.Black;
            try
            {
                if (status.Equals("0"))
                {
                    statusName = "未审核";
                    color = Color.FromName("#F2514E");
                }
                else if (status.Equals("1"))
                {
                    statusName = "已审核";
                    color = Color.FromName("#7DC733");
                }
                else if (status.Equals("2"))
                {
                    statusName = "部分审核";
                    color = Color.FromName("#87CEFA");
                }
                else
                {
                    statusName = "无数据";
                }
            }
            catch (Exception e)
            {
                statusName = "";
            }
            return statusName;

        }

        public int GetAuditDays()
        {
            int days = -1;
            try
            {
                days = ConfigurationManager.AppSettings["AuditDays"] != null && !ConfigurationManager.AppSettings["AuditDays"].ToString().Equals("") ? Convert.ToInt32(ConfigurationManager.AppSettings["AuditDays"].ToString()) : -1;
            }
            catch
            {
                days = -1;
            }
            return days;
        }
        #endregion
    }
}