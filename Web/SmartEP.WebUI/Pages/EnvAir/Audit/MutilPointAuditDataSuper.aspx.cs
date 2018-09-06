﻿using System;
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
using System.Threading;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using log4net;
using SmartEP.Core.Generic;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    /// <summary>
    /// 名称：MutilPointAuditDataSuper.cs
    /// 创建人：
    /// 创建日期：
    /// 维护人员：
    /// 最新维护人员：吕云
    /// 最新维护日期：2017-6-20
    /// 功能摘要：
    /// 审核站点抽象类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class MutilPointAuditDataSuper : SmartEP.WebUI.Common.BasePage
    {
        #region 属性
        AuditMonitoringPointPollutantService pollutantService = new AuditMonitoringPointPollutantService();//因子接口
        AuditOperatorSettingService operatorService = new AuditOperatorSettingService();
        AuditStatusMappingService statusService = new AuditStatusMappingService();
        AuditDataService auditService = new AuditDataService();
        AuditLogService logService = new AuditLogService();
        AuditOperatorService auditDataService = new AuditOperatorService();
        UserService userService = new UserService();
        //DataDealService dataDealService = new DataDealService();
        AirAuditMonitoringPointService pointAirService = new AirAuditMonitoringPointService();//点位接口
        WaterAuditMonitoringPointService pointWaterService = new WaterAuditMonitoringPointService();//点位接口 地表水
        public static bool isfirst = true;
        MonitoringInstrumentService instrumentService = new MonitoringInstrumentService();
        BaseDataModel BaseDataModel = new BaseDataModel();

        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        /// <summary>
        /// 审核因子配置接口
        /// </summary>
        SmartEP.Service.AutoMonitoring.common.AuditMonitoringPointService g_AuditMonitoringPointService = Singleton<SmartEP.Service.AutoMonitoring.common.AuditMonitoringPointService>.GetInstance();
        /// <summary>
        /// 应用程序Uid
        /// </summary>
        string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(SmartEP.Core.Enums.ApplicationValue.Air);
        /// <summary>
        /// 超级站监站点状态类型
        /// </summary>
        int PointType = 1;

        //AuditMonitoringPointPollutantService pollutantService = new AuditMonitoringPointPollutantService();//因子接口
        //string[] portid = { "1" };
        //string[] factorCode = { "a05024" };
        //string[] factorName = { "PM10" };
        #endregion

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
                //pointType：空气（0：国控点；1：市控点位；2：超级站点位；3：省控点位；4：区控点位；5：创模点位;6：中意项目）
                //           地表水（0：水源地；1：城区河道；2：浮标站；3：人工监站点；）     
                if (Request.QueryString["pointType"] != null)
                    ViewState["pointType"] = Request.QueryString["pointType"];
                else
                    ViewState["pointType"] = "0";
                ViewState["PointID"] = Request.QueryString["PointID"] != null ? Request.QueryString["PointID"].ToString() : "";//点位ID
                InitControl();
                //timer.Enabled = true;
            }
        }

        #region 初始化控件
        private void InitControl()
        {
            InitMenu();//初始化右键菜单
            RadDatePickerBegin.SelectedDate = Request.QueryString["dtBegin"] != null ? Convert.ToDateTime(Request.QueryString["dtBegin"].ToString()) : DateTime.Now.AddDays(-1);
            RadDatePickerEnd.SelectedDate = Request.QueryString["dtEnd"] != null ? Convert.ToDateTime(Request.QueryString["dtEnd"].ToString()) : RadDatePickerBegin.SelectedDate;
            PointBind();//初始化点位
            StatusInit();//获取点位审核状态
            string Pointids = GetPointSelectedValue();
            PollutantBind();
            FactorRsmAudit.firstLoad = 2;

        }
        //protected void timer_Tick(object sender, EventArgs e)
        //{
        //    gridAuditData.CurrentPageIndex = 0;
        //    gridAuditData.Rebind();
        //    timer.Enabled = false;
        //}
        #region 因子绑定
        private void PollutantBind()
        {
            string PointID = radioPoint.SelectedValue.ToString().Split('/')[0];
            string PointUid = radioPoint.SelectedValue.ToString().Split('/')[1];

            //获取仪器
            IQueryable<InstrumentEntity> instrumentList = instrumentService.RetrieveListByPointUid(PointUid);
            //instrumentList = instrumentList.Where(p => p.RowGuid != "6cd5c158-8a79-4540-a8b1-2a062759c9a0");//剔除超级站常规参数

            radioPollutant.DataSource = instrumentList;
            radioPollutant.DataTextField = "InstrumentName";
            radioPollutant.DataValueField = "RowGuid";
            radioPollutant.DataBind();

            if (Session["InstrumentUid"] == null)
            {
            }
            else
            {
                string InstrumentUid = Session["InstrumentUid"].ToString();
                for (int i = 0; i < radioPollutant.Items.Count; i++)
                {
                    if (radioPollutant.Items[i].Value == InstrumentUid)
                        radioPollutant.Items[i].Selected = true;
                }
            }
        }
        #endregion

        private void InitMenu()
        {

            //多选单元格时显示的菜单（没有互动信息和查看因子数据）
            IQueryable<V_AuditOperatorSettingEntity> list = operatorService.GetAuditOperatorList(Session["applicationUID"].ToString(), "Manual");
            RadMenuData.DataSource = list;
            RadMenuData.DataTextField = "OperatorName";
            RadMenuData.DataValueField = "AuditOperatorUid";
            RadMenuData.DataBind();

            //单选单元格时显示的菜单（没有互动信息和查看因子数据）
            RadMenuAuditLog.DataSource = list;
            RadMenuAuditLog.DataTextField = "OperatorName";
            RadMenuAuditLog.DataValueField = "AuditOperatorUid";
            RadMenuAuditLog.DataBind();

            //图表显示的菜单
            ContextMenuChart.DataSource = list;
            ContextMenuChart.DataTextField = "OperatorName";
            ContextMenuChart.DataValueField = "AuditOperatorUid";
            ContextMenuChart.DataBind();

            #region 其他右键菜单信息
            //恢复
            RadMenuItem item = new RadMenuItem();
            item.Text = "恢复";
            item.Value = "restore";
            item.ToolTip = "恢复至原始数据";
            RadMenuData.Items.Add(item);

            item = new RadMenuItem();
            item.Text = "恢复";
            item.Value = "restore";
            item.ToolTip = "恢复至原始数据";
            RadMenuAuditLog.Items.Add(item);

            item = new RadMenuItem();
            item.Text = "恢复";
            item.Value = "restore";
            item.ToolTip = "恢复至原始数据";
            ContextMenuChart.Items.Add(item);

            //时间段恢复
            item = new RadMenuItem();
            item.Text = "时间段恢复";
            item.Value = "restore";
            item.ToolTip = "时间段恢复至原始数据";
            RadMenuData.Items.Add(item);

            item = new RadMenuItem();
            item.Text = "时间段恢复";
            item.Value = "restore";
            item.ToolTip = "时间段恢复至原始数据";
            RadMenuAuditLog.Items.Add(item);

            item = new RadMenuItem();
            item.Text = "时间段恢复";
            item.Value = "restore";
            item.ToolTip = "时间段恢复至原始数据";
            ContextMenuChart.Items.Add(item);


            #endregion

            #region 添加提示信息
            int num = 0;
            foreach (V_AuditOperatorSettingEntity itemMenu in list)
            {
                RadMenuData.Items[num].ToolTip = itemMenu.ToolTip;
                RadMenuAuditLog.Items[num].ToolTip = itemMenu.ToolTip;
                ContextMenuChart.Items[num].ToolTip = itemMenu.ToolTip;
                num++;
            }
            #endregion

        }

        #region 点位绑定
        private void PointBind()
        {
            IQueryable<MonitoringPointEntity> pointList = null;
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]))
                pointList = pointAirService.RetrieveAirByAuditType(ViewState["pointType"].ToString(), Session["UserGuid"].ToString()).Distinct();
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))
                pointList = pointWaterService.RetrieveWaterByAuditType(ViewState["pointType"].ToString(), Session["UserGuid"].ToString());
            foreach (MonitoringPointEntity PEntity in pointList)
            {
                ListItem item = new ListItem();
                item.Value = PEntity.PointId + "/" + PEntity.MonitoringPointUid;
                item.Text = PEntity.MonitoringPointName;
                ListItem lt = new ListItem(item.Text, item.Value);
                radioPoint.Items.Add(lt);
            }

            string Pointids = "";
            string PointUids = "";
            foreach (ListItem item in radioPoint.Items)
            {
                if (ViewState["PointID"].ToString().Equals(item.Value.Split('/')[0]))
                    item.Selected = true;
            }

            if (radioPoint.Items.Count > 0 && radioPoint.SelectedItem == null)
            {
                radioPoint.Items[0].Selected = true;
                Pointids = radioPoint.SelectedValue.ToString().Split('/')[0];//站点单选，故Pointids只有一个PointID
                PointUids = radioPoint.SelectedValue.ToString().Split('/')[1];
            }
            ViewState["PointID"] = Pointids;
            ViewState["PointUid"] = PointUids;
        }
        #endregion

        #region 获取点位和审核状态
        public void StatusInit()
        {
            string Pointids = GetPointSelectedValue();
            string InsId = Session["InstrumentUid"].ToString();
            //根据仪器uid获取因子类型uid
            string uid = string.Empty;
            switch (InsId)
            {
                case "56dd6e9b-4c8f-4e67-a70f-b6a277cb44d7":
                    uid = "e5b6d666-24d1-473a-b15a-33a36245d44f";
                    break;
                case "6e4aa38a-f68b-490b-9cd7-3b92c7805c2d":
                    uid = "14b38adf-d899-4362-99ff-6a9e9dd35485";
                    break;
                case "3745f768-a789-4d58-9578-9e41fde5e5f0":
                    uid = "fbc6dyta-d06c-678g-b5y0-89b12be5bda3";
                    break;
                case "1589850e-0df1-4d9d-b508-4a77def158ba":
                    uid = "5575a0e1-d948-4566-9dcd-4b4767688add";
                    break;
                case "a6b3d80c-8281-4bc6-af47-f0febf568a5c":
                    uid = "59f02681-093f-48f0-9cac-ac59acd7038f";
                    break;
                case "da4f968f-cc6e-4fec-8219-6167d100499d":
                    uid = "aabe91e0-29a4-427c-becc-0b29f1224422";
                    break;
                case "9ef57f3c-8cce-4fe3-980f-303bbcfde260":
                    uid = "da92c7c1-4932-4007-a6d5-2866aa8c63f1";
                    break;
                case "6cd5c158-8a79-4540-a8b1-2a062759c9a0":
                    uid = "339B72C4-7295-4D31-B9EB-23342CB3697E";
                    break;
            }
            List<PointPollutantInfo> PointPollutantInfos = pollutantService.RetrieveSiteMapPollutantList(204, "airaaira-aira-aira-aira-airaairaaira", Session["UserGuid"].ToString()).Where(p => p.PGuid != null && p.PGuid.Equals(uid)).ToList<PointPollutantInfo>();
            string[] factors = new string[PointPollutantInfos.Count];
            for (int i = 0; i < PointPollutantInfos.Count; i++)
            {
                factors[i] = PointPollutantInfos[i].PID;
            }
            if (factors.Length <= 0) return;
            string status = logService.RerieveAuditStateNew(Pointids.Split(';'), RadDatePickerBegin.SelectedDate.Value, RadDatePickerEnd.SelectedDate.Value, Session["applicationUID"].ToString(), ViewState["pointType"].Equals("2") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira") ? "1" : "0", factors, InsId);
            //string status = logService.RerieveAuditState(Pointids.Split(';'), RadDatePickerBegin.SelectedDate.Value, RadDatePickerEnd.SelectedDate.Value, Session["applicationUID"].ToString(), ViewState["pointType"].Equals("2") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira") ? "1" : "0");
            Color color = Color.Black;
            auditState.Text = GetStatusName(status, out color);
            auditState.ForeColor = color;
        }
        #endregion

        #endregion

        #region 事件
        /// <summary>
        /// 数据源绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

            PointIDHidden.Value = GetPointSelectedValue();
            string insGuid = radioPollutant.SelectedValue;
            InsIDHidden.Value = insGuid;
            //switch (insGuid)
            //{
            //    case "56dd6e9b-4c8f-4e67-a70f-b6a277cb44d7":
            //        InsIDHidden.Value = "e5b6d666-24d1-473a-b15a-33a36245d44f";
            //        break;
            //    case "d2747011-ff8d-4b04-a006-c32ecaad4507":
            //        InsIDHidden.Value = "14b38adf-d899-4362-99ff-6a9e9dd35485";
            //        break;
            //    case "3745f768-a789-4d58-9578-9e41fde5e5f0":
            //        InsIDHidden.Value = "fbc6dyta-d06c-678g-b5y0-89b12be5bda3";
            //        break;
            //    case "1589850e-0df1-4d9d-b508-4a77def158ba":
            //        InsIDHidden.Value = "5575a0e1-d948-4566-9dcd-4b4767688add";
            //        break;
            //    case "59f02681-093f-48f0-9cac-ac59acd7038f":
            //        InsIDHidden.Value = "59f02681-093f-48f0-9cac-ac59acd7038f";
            //        break;
            //    case "aabe91e0-29a4-427c-becc-0b29f1224422":
            //        InsIDHidden.Value = "aabe91e0-29a4-427c-becc-0b29f1224422";
            //        break;
            //    case "7b25523b-4007-41e6-8640-f1587d474893":
            //        InsIDHidden.Value = "da92c7c1-4932-4007-a6d5-2866aa8c63f1";
            //        break;
            //}
            AddColumnsOfGridAuditData();//绑定列

            gridAuditData.MasterTableView.ClearEditItems();
            string[] pointid = GetPointSelectedValue().Split(';');

            #region 从审核历史表导入审核预处理数据
            try
            {
                string AuditTransfer = ConfigurationManager.AppSettings["AuditTransfer"] != null ? ConfigurationManager.AppSettings["AuditTransfer"].ToString() : "";
                if (!AuditTransfer.Equals("") && AuditTransfer.Split(':')[0].Equals("1") && Convert.ToInt32(AuditTransfer.Split(':')[1]) > 0)
                    auditDataService.GetDataFromHis(Session["applicationUID"].ToString(), pointid, RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1), Convert.ToInt32(AuditTransfer.Split(':')[1]));
            }
            catch
            {
            }
            #endregion

            string pointType = ViewState["pointType"].Equals("2") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira") ? "1" : "0";//判断是否是超级站
            //gridAuditData.DataSource = auditService.RetrieveAuditHourData(Session["applicationUID"].ToString(), pointid, Session["FactorCodeSuper"].ToString().Split(';'), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1), true, IsShowTotal.Checked == true ? true : false, pointType);
            gridAuditData.DataSource = auditService.RetrieveAuditHourDataS(Session["applicationUID"].ToString(), pointid, InsIDHidden.Value, Session["FactorCodeSuper"].ToString().Split(';'), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1), true, IsShowTotal.Checked == true ? true : false, pointType);
            RegisterScript("ClearSelectedInfo();GridResize();");
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
                #region 着色  ToolpTip  等处理 绑定单元格 onmouseover 事件
                if (e.Item is GridDataItem && myRadGrid.MasterTableView.DataSourceCount > 0)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    foreach (GridColumn col in myRadGrid.MasterTableView.RenderColumns)
                    {
                        #region 着色
                        string CurrUName = col.UniqueName;
                        if (!CurrUName.Equals("PointId") && !CurrUName.Equals("PointName") && !CurrUName.Equals("AuditStatus") && !CurrUName.Equals("DataDateTime") && col.Visible == true)
                        {
                            DataRowView row = (DataRowView)item.DataItem;
                            string CurrFlag = row[CurrUName + "_AuditFlag"].ToString();
                            int DecimalNum = 3;
                            TableCell cell = item[CurrUName];
                            if (row[CurrUName] == DBNull.Value) cell.Text = "/";//空值用/代替
                            if (row["PointId"].ToString().Equals("0")) continue;//统计行不进行着色
                            //获取小数位数
                            DecimalNum = Session["FactorCodeSuper"] != null ? Convert.ToInt32(Session["PollutantDecimalNumSuper"].ToString().Split(';')[Array.IndexOf(Session["FactorCodeSuper"].ToString().Split(';'), CurrUName)]) : 3;

                            #region 绑定单元格 onmouseover 事件
                            //item[CurrUName].Attributes.Add("onmouseover", "MouseOver('" + item.ItemIndex + "','" + col.UniqueName + "');");
                            #endregion
                            if (!CurrFlag.Equals("") && !CurrFlag.Equals("N") && !CurrFlag.Equals("d"))
                            {
                                //获取原始数据值
                                decimal sourceValue = auditService.GetSourcePolltantValue(Session["applicationUID"].ToString(), Convert.ToDateTime(row["DataDateTime"]), Convert.ToInt32(row["PointId"]), CurrUName);
                                cell.ToolTip = (sourceValue == -99999 ? "原始值：/" : "原始值：" + DecimalExtension.GetRoundValue(sourceValue, DecimalNum)) + "   \r\n";//鼠标悬停显示原始数据
                                #region 根据ColorTag着色并把日志加载到ToolTip
                                cell.CssClass = "rgBatchChanged";
                                if (CurrFlag.Contains("RM"))//自动或人工置为无效
                                {
                                    cell.ToolTip += "置为无效";
                                }
                                else if (CurrFlag.Equals("MF"))//修改过
                                {
                                    cell.ToolTip += "修改";
                                }
                                else
                                {
                                    string status = "";
                                    IQueryable<AuditStatusMappingEntity> list = statusService.GetDataStatusMappingList(Session["applicationUID"].ToString(), CurrFlag);
                                    int num = 0;
                                    foreach (AuditStatusMappingEntity entity in list)
                                    {
                                        if (entity.StatusIdentify.ToUpper().Equals("HSP") || entity.StatusIdentify.ToUpper().Equals("LSP"))
                                        {
                                            if (num == 0)
                                            {
                                                status += ",超标异常";
                                                cell.BackColor = Color.FromName(getAuditConfigColor(0));
                                            }
                                            num++;
                                        }
                                        else if (entity.StatusIdentify.ToUpper().Equals("UPD"))
                                        {
                                            status += ",PM10、PM2.5倒挂";
                                            cell.BackColor = Color.FromName(getAuditConfigColor(1));
                                        }
                                        else
                                            status += "," + entity.StatusName;
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
                            cell.Text = radioPoint.Items.FindByValue(row["PointId"].ToString() + "/" + radioPoint.SelectedValue.ToString().Split('/')[1]).Text;
                        }
                        else if (CurrUName.Equals("AuditStatus") && col.Visible == true)//获取审核状态
                        {
                            Color color = Color.Black;
                            TableCell cell = item[CurrUName];
                            DataRowView row = (DataRowView)item.DataItem;
                            cell.Text = GetStatusName(row["AuditStatus"] != DBNull.Value ? row["AuditStatus"].ToString() : "", out color);
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
        /// 绑定Grid列
        /// </summary>
        private void AddColumnsOfGridAuditData()
        {
            string[] factorCode = Session["FactorCodeSuper"].ToString().Trim().Split(';');
            FactorCodes.Value = Session["FactorCodeSuper"].ToString().Trim();
            string[] factorName = Session["FactorNameSuper"].ToString().Trim().Split(';');
            string[] PollutantDecimalNum = Session["PollutantDecimalNumSuper"].ToString().Trim().Split(';');
            string[] PollutantUnit = Session["PollutantUnitSuper"].ToString().Trim().Split(';');
            string[] IsEdit = Session["isEditSuper"].ToString().Trim().Split(';');
            int baseNum = 11;


            gridAuditData.Columns.Clear();
            gridAuditData.Columns.Add(new GridTemplateColumn() { HeaderText = "点位名称", UniqueName = "PointName" });
            //gridAuditData.Columns.Add(new GridTemplateColumn() { HeaderText = "审核状态", UniqueName = "Status" });
            gridAuditData.Columns.Add(new GridBoundColumn() { HeaderText = "审核状态", UniqueName = "AuditStatus", DataField = "AuditStatus" });
            gridAuditData.Columns.Add(new GridBoundColumn() { DataField = "PointID", UniqueName = "PointID", HeaderText = "PointID", Visible = false });
            if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))//水
            {
                baseNum = 9;
                GridBoundColumn column = new GridBoundColumn() { DataField = "DataDateTime", UniqueName = "DataDateTime", HeaderText = "时间点", DataFormatString = "{0:yyyy-MM-dd HH:mm}" };
                column.HeaderStyle.Width = new Unit(120);
                column.ReadOnly = false;
                gridAuditData.Columns.Add(column);
            }
            else
                gridAuditData.Columns.Add(new GridTemplateColumn() { DataField = "DataDateTime", ReadOnly=false, UniqueName = "DataDateTime", HeaderText = "时间点", ItemTemplate = new MyTemplate("DataDateTime", 0, Session["applicationUID"].ToString()) });
            for (int i = 0; i < factorCode.Length; i++)
            {
                if (!factorCode[i].Equals(""))
                {
                    string unit = "(mg/m<sup>3</sup>)";
                    if (PollutantUnit[i].Equals("mg/L") && EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]) || PollutantUnit[i].Equals("mg/m3")) unit = "(mg/m<sup>3</sup>)";
                    else unit = "(" + PollutantUnit[i] + ")";
                    GridTemplateColumn col1;
                    if (PollutantUnit[i].Equals(""))
                    {
                        col1 = new GridTemplateColumn() { DataField = factorCode[i], UniqueName = factorCode[i], HeaderText = factorName[i] + "</br>", ItemTemplate = new MyTemplate(factorCode[i], Convert.ToInt32(PollutantDecimalNum[i]), Session["applicationUID"].ToString()), EditItemTemplate = new MyTemplate(Convert.ToInt32(PollutantDecimalNum[i])) };
                    }
                    else
                    {
                        col1 = new GridTemplateColumn() { DataField = factorCode[i], UniqueName = factorCode[i], HeaderText = factorName[i] + "</br>" + unit, ItemTemplate = new MyTemplate(factorCode[i], Convert.ToInt32(PollutantDecimalNum[i]), Session["applicationUID"].ToString()), EditItemTemplate = new MyTemplate(Convert.ToInt32(PollutantDecimalNum[i])) };
                    }
                    col1.ReadOnly = IsEdit[i].ToString().Equals("1") ? true : false;
                    //col1.UseNativeEditorsInMobileMode = false;
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
        /// 站点切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radioPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["PointID"] = GetPointSelectedValue();
            PollutantBind();
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
            String myUrl = "MutilPointAuditStateSuper.aspx?Token=" + Request.QueryString["Token"] + "&app=" + app + "&pointType=" + ViewState["pointType"] + "&dtBegin=" + RadDatePickerBegin.SelectedDate.Value.Date + "&dtEnd=" + RadDatePickerEnd.SelectedDate.Value.Date + "&PointID=" + ViewState["PointID"];//AuditState.aspx
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
            //if (days >= GetAuditDays())
            //    RadDatePickerEnd.SelectedDate = RadDatePickerBegin.SelectedDate.Value.Date;
            //RegisterScript("GridResize();LoadingData();");
            gridAuditData.Rebind();
            //RegisterScript("LoadingData();");
        }

        protected void RadDatePickerEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            //int days = (RadDatePickerEnd.SelectedDate.Value.Date - RadDatePickerBegin.SelectedDate.Value.Date).Days;
            //if (days >= GetAuditDays())
            //    RadDatePickerBegin.SelectedDate = RadDatePickerEnd.SelectedDate.Value.Date;
            //RegisterScript("GridResize();LoadingData();");
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
            //RegisterScript("GridResize();LoadingData();");
            gridAuditData.Rebind();
        }

        /// <summary>
        /// 审核操作后图表刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void refreshData_Click(object sender, EventArgs e)
        {
            //RegisterScript("GridResize();LoadingData();");
            gridAuditData.Rebind();
            //RegisterScript("LoadingData();");
        }

        /// <summary>
        /// 审核提交后图表刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void submitButton_Click(object sender, EventArgs e)
        {
            StatusInit();//获取点位审核状态
            //RegisterScript("GridResize();LoadingData();");
            gridAuditData.Rebind();
        }

        /// <summary>
        /// 提交审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitAudit_Click(object sender, EventArgs e)
        {
            #region 暂停使用(目前采用异步的方式)
            ////判断国控点生成DBF文件
            //bool IsCreateDBF = false;
            //if (ViewState["pointType"].Equals("0") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira")) IsCreateDBF = true;

            //string pointid = GetPointSelectedValue();
            //if (!pointid.Equals(""))
            //{
            //    DateTime beginTime = RadDatePickerBegin.SelectedDate.Value.Date;
            //    DateTime endTime = RadDatePickerEnd.SelectedDate.Value.Date;
            //    string UserIP = IPAddressExtensions.GetUserIp();
            //    string UserUid = Session["UserGuid"].ToString();
            //    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
            //    bool isSuccess = auditDataService.SubmitAudit(Session["applicationUID"].ToString(), pointid.Split(';'), beginTime, endTime, UserIP, UserUid, CreatUser, false, "", IsCreateDBF, ViewState["pointType"].Equals("2") ? "1" : "0");
            //    if (isSuccess)
            //    {
            //        RegisterScript("alert(\"提交审核成功！\");");
            //        StatusInit();
            //        gridAuditData.Rebind();
            //        ////RegisterScript("LoadingData();");
            //        //Color color = Color.Black;
            //        //auditState.Text = GetStatusName("1", out color);
            //        //auditState.ForeColor = color;
            //    }
            //    else
            //    {
            //        RegisterScript("alert(\"提交审核失败！\");");
            //        gridAuditData.Rebind();
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search_Click(object sender, EventArgs e)
        {
            string Pointids = GetPointSelectedValue();
            gridAuditData.DataSource = null;

            PointIDHidden.Value = Pointids;
            string insGuid = radioPollutant.SelectedValue;
            InsIDHidden.Value = insGuid;
            //switch (insGuid)
            //{
            //    case "56dd6e9b-4c8f-4e67-a70f-b6a277cb44d7":
            //        InsIDHidden.Value = "e5b6d666-24d1-473a-b15a-33a36245d44f";
            //        break;
            //    case "d2747011-ff8d-4b04-a006-c32ecaad4507":
            //        InsIDHidden.Value = "14b38adf-d899-4362-99ff-6a9e9dd35485";
            //        break;
            //    case "3745f768-a789-4d58-9578-9e41fde5e5f0":
            //        InsIDHidden.Value = "fbc6dyta-d06c-678g-b5y0-89b12be5bda3";
            //        break;
            //    case "1589850e-0df1-4d9d-b508-4a77def158ba":
            //        InsIDHidden.Value = "5575a0e1-d948-4566-9dcd-4b4767688add";
            //        break;
            //    case "59f02681-093f-48f0-9cac-ac59acd7038f":
            //        InsIDHidden.Value = "59f02681-093f-48f0-9cac-ac59acd7038f";
            //        break;
            //    case "aabe91e0-29a4-427c-becc-0b29f1224422":
            //        InsIDHidden.Value = "aabe91e0-29a4-427c-becc-0b29f1224422";
            //        break;
            //    case "7b25523b-4007-41e6-8640-f1587d474893":
            //        InsIDHidden.Value = "da92c7c1-4932-4007-a6d5-2866aa8c63f1";
            //        break;
            //}
            //InitChartFactors();//初始化图形单因子切换
            StatusInit();//获取点位审核状态
            //RegisterScript("GridResize();LoadingData();");
            gridAuditData.Rebind();
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void selectAll_Click(object sender, EventArgs e)
        {
            BindingFactors();//重新绑定因子
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void inverse_Click(object sender, EventArgs e)
        {
            BindingFactors();//重新绑定因子
        }

        /// <summary>
        /// 重新绑定因子以及 隐藏域Pointid（图表）
        /// </summary>
        private void BindingFactors()
        {
            string Pointids = GetPointSelectedValue();

            if (!Pointids.Equals(""))
            {
                ViewState["PointID"] = Pointids;
                //factorCbxRsm.GetFactorsSuper();//获取sitemap因子
                factorNames.Value = Session["FactorCodeSuper"].ToString() + "|" + Session["FactorNameSuper"].ToString() + "|" + Session["PollutantDecimalNumSuper"].ToString() + "|" + Session["PollutantUnitSuper"].ToString();//隐藏域赋值，用于显示Chart时AJAX传值用
                PointIDHidden.Value = Pointids;
                string insGuid = radioPollutant.SelectedValue;
                InsIDHidden.Value = insGuid;
                //switch (insGuid)
                //{
                //    case "56dd6e9b-4c8f-4e67-a70f-b6a277cb44d7":
                //        InsIDHidden.Value = "e5b6d666-24d1-473a-b15a-33a36245d44f";
                //        break;
                //    case "d2747011-ff8d-4b04-a006-c32ecaad4507":
                //        InsIDHidden.Value = "14b38adf-d899-4362-99ff-6a9e9dd35485";
                //        break;
                //    case "3745f768-a789-4d58-9578-9e41fde5e5f0":
                //        InsIDHidden.Value = "fbc6dyta-d06c-678g-b5y0-89b12be5bda3";
                //        break;
                //    case "1589850e-0df1-4d9d-b508-4a77def158ba":
                //        InsIDHidden.Value = "5575a0e1-d948-4566-9dcd-4b4767688add";
                //        break;
                //    case "59f02681-093f-48f0-9cac-ac59acd7038f":
                //        InsIDHidden.Value = "59f02681-093f-48f0-9cac-ac59acd7038f";
                //        break;
                //    case "aabe91e0-29a4-427c-becc-0b29f1224422":
                //        InsIDHidden.Value = "aabe91e0-29a4-427c-becc-0b29f1224422";
                //        break;
                //    case "7b25523b-4007-41e6-8640-f1587d474893":
                //        InsIDHidden.Value = "da92c7c1-4932-4007-a6d5-2866aa8c63f1";
                //        break;
                //}
            }
            else
            {
                Alert("请选择站点！");
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
        /// 获取选择的站点值（以;隔开）
        /// </summary>
        /// <returns></returns>
        private string GetPointSelectedValue()
        {
            string Pointids = "";
            foreach (ListItem item in radioPoint.Items)
            {
                if (item.Selected == true)
                    Pointids += Pointids.Equals("") ? item.Value.Split('/')[0] : ";" + item.Value.Split('/')[0];
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
                statusName = "无数据";
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

        protected void radioPollutant_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 修改by lvyun 2017-6-22
            string PointUid = radioPoint.SelectedValue.ToString().Split('/')[1];
            string InstrumentGuid = radioPollutant.SelectedValue;

            string PollutantName = "", PollutantCode = "", PollutantUid = "", PollutantShow = "", PollutantRead = "";

            //审核配置因子
            IQueryable<AuditMonitoringPointPollutantEntity> AuditLists = g_AuditMonitoringPointService.DetailRetrieve(x => PointUid.Equals(x.AuditMonitoringPointEntity.MonitoringPointUid) && x.AuditMonitoringPointEntity.ApplicationUid == ApplicationUid);//&& x.AuditMonitoringPointEntity.PointType == PointType
            List<AuditMonitoringPointPollutantEntity> AuditData = new List<AuditMonitoringPointPollutantEntity>();
            if (AuditLists != null)
            {
                foreach (AuditMonitoringPointPollutantEntity audit in AuditLists)
                {
                    if (PointUid == audit.AuditMonitoringPointEntity.MonitoringPointUid)
                    {
                        if (!AuditData.Contains(audit))
                        {
                            AuditData.Add(audit);
                        }
                    }
                }
            }
            if (AuditData != null)
            {

                foreach (AuditMonitoringPointPollutantEntity audit in AuditData)
                {
                    PollutantShow += audit.PollutantCode + ";";
                    if (!string.IsNullOrEmpty(Convert.ToString(audit.ReadOnly)) && (Boolean)audit.ReadOnly)
                    {
                        PollutantRead += audit.PollutantCode + ";";
                    }
                }
            }

            string[] PollutantShowList = PollutantShow.Split(';');
            string[] PollutantReadList = PollutantRead.Split(';');
            #endregion

            //string CategoryUid = "";
            string selCode = "";
            string selName = "";
            string isEdit = "";
            string PollutantDecimalNum = "";
            string PollutantUnit = "";
            //for (int i = 0; i < radioPollutant.Items.Count; i++)
            //{
            //    if (radioPollutant.Items[i].Selected)
            //        CategoryUid = radioPollutant.Items[i].Value;
            //}

            AirPollutantService airservice = new AirPollutantService();
            IQueryable<InstrumentChannelEntity> monitorInstrList = BaseDataModel.InstrumentChannelEntities;


            IQueryable<PollutantCodeEntity> pollutantQueryable = from item in airservice.RetrieveListByUseOrNot(true)
                                                                 join ic in monitorInstrList on item.PollutantCode equals ic.PollutantCode
                                                                 where (PollutantShowList.Contains(item.PollutantCode) && ic.InstrumentUid.Equals(InstrumentGuid))
                                                                 orderby item.OrderByNum descending
                                                                 select item;

            //PollutantCodeEntity[] pEntity = pollutantQueryable.ToArray();
            List<PollutantCodeEntity> pEntity = pollutantQueryable.ToList();
            if (InstrumentGuid == "3745f768-a789-4d58-9578-9e41fde5e5f0")
            {
                PollutantCodeEntity Entity40 = airservice.RetrieveEntityByCode("a51040");
                PollutantCodeEntity Entity48 = airservice.RetrieveEntityByCode("a51048");
                PollutantCodeEntity Entity52 = airservice.RetrieveEntityByCode("a51052");
                PollutantCodeEntity Entity54 = airservice.RetrieveEntityByCode("a51054");
                pEntity.Remove(Entity40);
                pEntity.Remove(Entity48);
                pEntity.Remove(Entity52);
                pEntity.Remove(Entity54);
            }
            //if (CategoryUid == "fbc6dyta-d06c-678g-b5y0-89b12be5bda3")
            //    pEntity = airservice.RetrieveListByUseOrNot(true).Where(p => p.CategoryUid == CategoryUid && p.OrderByNum != 0 && PollutantShowList.Contains(p.PollutantCode)).ToArray();
            //else
            //    pEntity = airservice.RetrieveListByUseOrNot(true).Where(p => p.CategoryUid == CategoryUid && PollutantShowList.Contains(p.PollutantCode)).ToArray();
            for (int i = 0; i < pEntity.Count; i++)
            {
                selCode += selCode == "" ? pEntity[i].PollutantCode : ";" + pEntity[i].PollutantCode;
                selName += selName == "" ? pEntity[i].PollutantName : ";" + pEntity[i].PollutantName;
                PollutantDecimalNum += PollutantDecimalNum == "" ? pEntity[i].DecimalDigit.ToString() : ";" + pEntity[i].DecimalDigit.ToString();

                DictionaryService dservice = new DictionaryService();
                string Unit = dservice.GetTextByGuid(Service.Core.Enums.DictionaryType.AMS, "计量单位", pEntity[i].MeasureUnitUid);
                PollutantUnit += Unit + ";";
                //isEdit += isEdit == "" ? "0" : ";" + "0";
                if (PollutantReadList.Contains(pEntity[i].PollutantCode))
                {
                    isEdit += "1;";
                }
                else
                {
                    isEdit += "0;";
                }
            }
            Session["InstrumentUid"] = InstrumentGuid;
            //Session["FactorCategorySuper"] = CategoryUid;
            Session["FactorCodeSuper"] = selCode;
            Session["FactorNameSuper"] = selName;
            Session["isEditSuper"] = isEdit;
            Session["PollutantDecimalNumSuper"] = PollutantDecimalNum;
            Session["PollutantUnitSuper"] = !PollutantUnit.Equals("") ? PollutantUnit.Substring(0, PollutantUnit.Length - 1) : "";
        }
    }
}