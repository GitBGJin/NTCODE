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
using System.Threading;
using SmartEP.Service.BaseData.Channel;
using System.Collections;
using SmartEP.Utilities.AdoData;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class MutilPointAuditDataLijingpu : SmartEP.WebUI.Common.BasePage
    {
        #region 属性
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
                //           地表水（0：水源地；1：城区河道；2：浮标站；3：人工监测点；）     
                if (Request.QueryString["pointType"] != null)
                    ViewState["pointType"] = Request.QueryString["pointType"];
                else
                    ViewState["pointType"] = "0";
                ViewState["PointID"] = Request.QueryString["PointID"] != null ? Request.QueryString["PointID"].ToString() : "";//点位ID
                InitControl();
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
            PollutantBind();//初始化因子
            insertAuditTable();//同步到审核数据库
            ////国控点不能提交审核
            //if (ViewState["pointType"].Equals("0") && EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]))
            //{
            //    SubmitAuditTD.Visible = false;
            //}
        }

        private void insertAuditTable()
        {
            DatabaseHelper dbhelp = new DatabaseHelper();

            #region 大粒径APS
            if (Lijingpu.SelectedValue.Equals("ASP"))
            {
                string isExistL = "select * from [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_L] where pointid=" + radioPoint.SelectedValue + " and datetime>='" + Convert.ToDateTime(RadDatePickerBegin.SelectedDate).ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + Convert.ToDateTime(RadDatePickerEnd.SelectedDate).ToString("yyyy-MM-dd 23:59:59") + "'";
                DataTable dtL = dbhelp.ExecuteDataTable(isExistL, "AMS_MonitoringBusinessConnection");
                if (dtL.Rows.Count <= 0)
                {
                    string insertSqlL = "insert into [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_L] " +
        "([PointId],[DateTime],[0.523],[0.542],[0.583],[0.626],[0.673],[0.723],[0.777],[0.835],[0.898],[0.965],[1.037],[1.114],[1.197],[1.286],[1.382],[1.486],[1.596],[1.715],[1.843],[1.981],[2.129],[2.288],[2.458],[2.642],[2.839],[3.051],[3.278],[3.523],[3.786],[4.068],[4.371],[4.698],[5.048],[5.425],[5.829],[6.264],[6.732],[7.234],[7.774],[8.354],[8.977],[9.647],[10.37],[11.14],[11.97],[12.86],[13.82],[14.86],[15.96],[17.15],[18.43],[19.81])" +
        "(SELECT [PointId],[DateTime],[0.523],[0.542],[0.583],[0.626],[0.673],[0.723],[0.777],[0.835],[0.898],[0.965],[1.037],[1.114],[1.197],[1.286],[1.382],[1.486],[1.596],[1.715],[1.843],[1.981],[2.129],[2.288],[2.458],[2.642],[2.839],[3.051],[3.278],[3.523],[3.786],[4.068],[4.371],[4.698],[5.048],[5.425],[5.829],[6.264],[6.732],[7.234],[7.774],[8.354],[8.977],[9.647],[10.37],[11.14],[11.97],[12.86],[13.82],[14.86],[15.96],[17.15],[18.43],[19.81]" +
        "  FROM [AMS_AirAutoMonitor].[dbo].[TB_SuperStation_lijingpu_L]" +
        " where pointid=" + radioPoint.SelectedValue + " and datetime>='" + Convert.ToDateTime(RadDatePickerBegin.SelectedDate).ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + Convert.ToDateTime(RadDatePickerEnd.SelectedDate).ToString("yyyy-MM-dd 23:59:59") + "')";
                    dbhelp.ExecuteNonQuery(insertSqlL, "AMS_MonitoringBusinessConnection");
                }
            }
            #endregion

            #region 中粒径L72
            if (Lijingpu.SelectedValue.Equals("L72"))
            {
                string isExistM = "select * from [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_M] where pointid=" + radioPoint.SelectedValue + " and datetime>='" + Convert.ToDateTime(RadDatePickerBegin.SelectedDate).ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + Convert.ToDateTime(RadDatePickerEnd.SelectedDate).ToString("yyyy-MM-dd 23:59:59") + "'";
                DataTable dtM = dbhelp.ExecuteDataTable(isExistM, "AMS_MonitoringBusinessConnection");
                if (dtM.Rows.Count <= 0)
                {
                    string insertSqlM = "insert into [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_lijingpu_M] " +
        "([PointId],[DateTime],[13.6],[14.1],[14.6],[15.1],[15.7],[16.3],[16.8],[17.5],[18.1],[18.8],[19.5],[20.2],[20.9],[21.7],[22.5],[23.3],[24.1],[25.0],[25.9],[26.9],[27.9],[28.9],[30.0],[31.1],[32.2],[33.4],[34.6],[35.9],[37.2],[38.5],[40.0],[41.4],[42.9],[44.5],[46.1],[47.8],[49.6],[51.4],[53.3],[55.2],[57.3],[59.4],[61.5],[63.8],[66.1],[68.5],[71.0],[73.7],[76.4],[79.1],[82.0],[85.1],[88.2],[91.4],[94.7],[98.2],[101.8],[105.5],[109.4],[113.4],[117.6],[121.9],[126.3],[131.0],[135.8],[140.7],[145.9],[151.2],[156.8],[162.5],[168.5],[174.7],[181.1],[187.7],[194.6],[201.7],[209.1],[216.7],[224.7],[232.9],[241.4],[250.3],[259.5],[269.0],[278.8],[289.0],[299.6],[310.6],[322.0],[333.8],[346.0],[358.7],[371.8],[385.4],[399.5],[414.2],[429.4],[445.1],[461.4],[478.3],[495.8],[514.0],[532.8],[552.3],[572.5],[593.5],[615.3],[637.8],[661.2],[685.4],[710.5],[736.5])" +
        "(SELECT [PointId],[DateTime],[13.6],[14.1],[14.6],[15.1],[15.7],[16.3],[16.8],[17.5],[18.1],[18.8],[19.5],[20.2],[20.9],[21.7],[22.5],[23.3],[24.1],[25.0],[25.9],[26.9],[27.9],[28.9],[30.0],[31.1],[32.2],[33.4],[34.6],[35.9],[37.2],[38.5],[40.0],[41.4],[42.9],[44.5],[46.1],[47.8],[49.6],[51.4],[53.3],[55.2],[57.3],[59.4],[61.5],[63.8],[66.1],[68.5],[71.0],[73.7],[76.4],[79.1],[82.0],[85.1],[88.2],[91.4],[94.7],[98.2],[101.8],[105.5],[109.4],[113.4],[117.6],[121.9],[126.3],[131.0],[135.8],[140.7],[145.9],[151.2],[156.8],[162.5],[168.5],[174.7],[181.1],[187.7],[194.6],[201.7],[209.1],[216.7],[224.7],[232.9],[241.4],[250.3],[259.5],[269.0],[278.8],[289.0],[299.6],[310.6],[322.0],[333.8],[346.0],[358.7],[371.8],[385.4],[399.5],[414.2],[429.4],[445.1],[461.4],[478.3],[495.8],[514.0],[532.8],[552.3],[572.5],[593.5],[615.3],[637.8],[661.2],[685.4],[710.5],[736.5] FROM [AMS_AirAutoMonitor].[dbo].[TB_SuperStation_lijingpu_M]" +
        " where pointid=" + radioPoint.SelectedValue + " and datetime>='" + Convert.ToDateTime(RadDatePickerBegin.SelectedDate).ToString("yyyy-MM-dd 00:00:00") + "' and datetime<='" + Convert.ToDateTime(RadDatePickerEnd.SelectedDate).ToString("yyyy-MM-dd 23:59:59") + "')";
                    dbhelp.ExecuteNonQuery(insertSqlM, "AMS_MonitoringBusinessConnection");
                }
            }
            #endregion
        }

        #region 因子绑定
        private void PollutantBind()
        {
            if (ViewState["PointID"].ToString() == "204")
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("text", typeof(string));
                dt.Columns.Add("value", typeof(string));

                DataRow drlijingpu = dt.NewRow();
                drlijingpu["text"] = "粒径谱仪";
                drlijingpu["value"] = "da92c7c1-4932-4007-a6d5-2866aa8c63f1";
                dt.Rows.Add(drlijingpu);

                radioPollutant.DataSource = dt;
                radioPollutant.DataTextField = "text";
                radioPollutant.DataValueField = "value";
                radioPollutant.DataBind();


                string CategoryUid = Session["FactorCategorySuper"].ToString();
                for (int i = 0; i < radioPollutant.Items.Count; i++)
                {
                    if (radioPollutant.Items[i].Value == CategoryUid)
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

            //修改
            item = new RadMenuItem();
            item.Text = "修改";
            item.Value = "modify";
            item.ToolTip = "修改数据";
            ContextMenuChart.Items.Add(item);

            //互动信息
            item = new RadMenuItem();
            item.Text = "互动信息";
            item.Value = "log";
            item.ToolTip = "查看审核记录";
            RadMenuAuditLog.Items.Add(item);

            item = new RadMenuItem();
            item.Text = "互动信息";
            item.Value = "log";
            item.ToolTip = "查看审核记录";
            ContextMenuChart.Items.Add(item);

            //查看因子数据
            item = new RadMenuItem();
            item.Text = "查看因子数据";
            item.Value = "factor";
            item.ToolTip = "查看所有点位的因子数据";
            RadMenuAuditLog.Items.Add(item);

            item = new RadMenuItem();
            item.Text = "查看因子数据";
            item.Value = "factor";
            item.ToolTip = "查看所有点位的因子数据";
            ContextMenuChart.Items.Add(item);
            #endregion
        }
        //private void InitMenu()
        //{


        //    IQueryable<V_AuditOperatorSettingEntity> list = operatorService.GetAuditOperatorList(Session["applicationUID"].ToString(), "Manual");
        //    RadMenuData.DataSource = list;
        //    RadMenuData.DataTextField = "OperatorName";
        //    RadMenuData.DataValueField = "AuditOperatorUid";
        //    RadMenuData.DataBind();
        //    foreach (RadMenuItem itemMenu in RadMenuData.Items)
        //    {
        //        itemMenu.ToolTip = list.Where(x => x.AuditOperatorUid.Equals(itemMenu.Value)).FirstOrDefault().ToolTip;
        //    }
        //    RadMenuItem item = new RadMenuItem();
        //    item.Text = "恢复";
        //    item.Value = "Restore";
        //    item.ToolTip = "恢复至原始数据";
        //    RadMenuData.Items.Add(item);

        //}

        #region 点位绑定
        private void PointBind()
        {
            IQueryable<MonitoringPointEntity> pointList = null;
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]))
                pointList = pointAirService.RetrieveAirByAuditType(ViewState["pointType"].ToString(), Session["UserGuid"].ToString());
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))
                pointList = pointWaterService.RetrieveWaterByAuditType(ViewState["pointType"].ToString(), Session["UserGuid"].ToString());
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
            //string CategoryUid = "";
            //CategoryUid = Session["FactorCategorySuper"].ToString();
            //AirPollutantService airservice = new AirPollutantService();
            //List<PollutantCodeEntity> pEntitys = airservice.RetrieveListByUseOrNot(true).Where(p => p.CategoryUid == CategoryUid).ToList<PollutantCodeEntity>();

            //string selectValue = "";
            //if (chartFactorRadio.Items.Count > 0) selectValue = chartFactorRadio.SelectedItem.Value;
            //chartFactorRadio.DataSource = pEntitys;
            //chartFactorRadio.DataTextField = "PollutantName";
            //chartFactorRadio.DataValueField = "PollutantCode";
            //chartFactorRadio.DataBind();
            //if (pEntitys.Count > 0)
            //{
            //    if (!selectValue.Equals(""))
            //    {
            //        if (chartFactorRadio.Items.FindByValue(selectValue) != null)
            //            chartFactorRadio.Items.FindByValue(selectValue).Selected = true;
            //        else
            //            chartFactorRadio.Items[0].Selected = true;
            //    }
            //    else
            //        chartFactorRadio.Items[0].Selected = true;
            //}
            //if (chartFactorRadio.SelectedItem != null)
            //    factorNames.Value = chartFactorRadio.SelectedItem.Value + "|" + chartFactorRadio.SelectedItem.Text;//隐藏域赋值，用于显示Chart时AJAX传值用
            //else
            //    factorNames.Value = "||";//隐藏域赋值，用于显示Chart时AJAX传值用
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
            string pointType = ViewState["pointType"].Equals("2") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira") ? "1" : "0";//判断是否是超级站
            string[] pointid = GetPointSelectedValue().Split(';');
            DataView dv = auditService.RetrieveAuditHourDataSuper(Session["applicationUID"].ToString(), pointid, Lijingpu.SelectedValue.Split(';'), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddSeconds(-1), true, IsShowTotal.Checked == true ? true : false, pointType);

            PointIDHidden.Value = GetPointSelectedValue();
            AddColumnsOfGridAuditData(dv);//绑定列

            gridAuditData.MasterTableView.ClearEditItems();

            #region 从审核历史表导入审核预处理数据
            //try
            //{
            //    string AuditTransfer = ConfigurationManager.AppSettings["AuditTransfer"] != null ? ConfigurationManager.AppSettings["AuditTransfer"].ToString() : "";
            //    if (!AuditTransfer.Equals("") && AuditTransfer.Split(':')[0].Equals("1") && Convert.ToInt32(AuditTransfer.Split(':')[1]) > 0)
            //        auditDataService.GetDataFromHis(Session["applicationUID"].ToString(), pointid, RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddSeconds(-1), Convert.ToInt32(AuditTransfer.Split(':')[1]));
            //}
            //catch
            //{
            //}
            #endregion

            gridAuditData.DataSource = dv;

            RegisterScript("ClearSelectedInfo();GridResize();");
        }

        /// <summary>
        /// 表格着色，添加单元格鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditData_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            //try
            //{
            //    RadGrid myRadGrid = ((RadGrid)sender);
            //    #region 着色  ToolpTip  等处理 绑定单元格 onmouseover 事件
            //    if (e.Item is GridDataItem && myRadGrid.MasterTableView.DataSourceCount > 0)
            //    {
            //        GridDataItem item = (GridDataItem)e.Item;
            //        foreach (GridColumn col in myRadGrid.MasterTableView.RenderColumns)
            //        {
            //            #region 着色
            //            string CurrUName = col.UniqueName;
            //            if (!CurrUName.Equals("PointId") && !CurrUName.Equals("PointName") && !CurrUName.Equals("AuditStatus") && !CurrUName.Equals("DataDateTime") && col.Visible == true)
            //            {
            //                DataRowView row = (DataRowView)item.DataItem;
            //                string CurrFlag = row[CurrUName + "_AuditFlag"].ToString();
            //                int DecimalNum = 3;
            //                TableCell cell = item[CurrUName];
            //                if (row[CurrUName] == DBNull.Value) cell.Text = "/";//空值用/代替
            //                if (row["PointId"].ToString().Equals("0")) continue;//统计行不进行着色
            //                //获取小数位数
            //                DecimalNum = Session["FactorCodeSuper"] != null ? Convert.ToInt32(Session["PollutantDecimalNumSuper"].ToString().Split(';')[Array.IndexOf(Session["FactorCodeSuper"].ToString().Split(';'), CurrUName)]) : 3;

            //                #region 绑定单元格 onmouseover 事件
            //                //item[CurrUName].Attributes.Add("onmouseover", "MouseOver('" + item.ItemIndex + "','" + col.UniqueName + "');");
            //                #endregion
            //                if (!CurrFlag.Equals(""))
            //                {
            //                    //获取原始数据值
            //                    decimal sourceValue = auditService.GetSourcePolltantValue(Session["applicationUID"].ToString(), Convert.ToDateTime(row["DataDateTime"]), Convert.ToInt32(row["PointId"]), CurrUName);
            //                    cell.ToolTip = (sourceValue == -99999 ? "原始值：/" : "原始值：" + DecimalExtension.GetRoundValue(sourceValue, DecimalNum)) + "   \r\n";//鼠标悬停显示原始数据
            //                    #region 根据ColorTag着色并把日志加载到ToolTip
            //                    cell.CssClass = "rgBatchChanged";
            //                    if (CurrFlag.Contains("RM"))//自动或人工置为无效
            //                    {
            //                        cell.ToolTip += "置为无效";
            //                    }
            //                    else if (CurrFlag.Equals("C"))//修改过
            //                    {
            //                        cell.ToolTip += "修改";
            //                    }
            //                    else
            //                    {
            //                        string status = "";
            //                        IQueryable<AuditStatusMappingEntity> list = statusService.GetDataStatusMappingList(Session["applicationUID"].ToString(), CurrFlag);
            //                        int num = 0;
            //                        foreach (AuditStatusMappingEntity entity in list)
            //                        {
            //                            if (entity.StatusIdentify.ToUpper().Equals("HSP") || entity.StatusIdentify.ToUpper().Equals("LSP"))
            //                            {
            //                                if (num == 0)
            //                                {
            //                                    status += ",超标异常";
            //                                    cell.BackColor = Color.FromName(getAuditConfigColor(0));
            //                                }
            //                                num++;
            //                            }
            //                            //else if (entity.StatusIdentify.ToUpper().Equals("UPD"))
            //                            //{
            //                            //    status += ",PM10、PM2.5倒挂";
            //                            //    cell.BackColor = Color.FromName(getAuditConfigColor(1));
            //                            //}
            //                            else
            //                                status += "," + entity.StatusName;
            //                        }
            //                        if (!status.Equals(""))
            //                            cell.ToolTip += status.Substring(1, status.Length - 1);
            //                    }
            //                    #region 注释
            //                    //if (CurrFlag.Contains("RM"))//自动或人工置为无效
            //                    //{
            //                    //    cell.ToolTip += "置为无效";
            //                    //}
            //                    //else if (CurrFlag.Equals("C"))//修改过
            //                    //{
            //                    //    cell.ToolTip += "修改";
            //                    //}
            //                    //else
            //                    //{
            //                    //    string status = "";
            //                    //    if (CurrFlag.Contains("HSp") || CurrFlag.Contains("LSp") || CurrFlag.Contains("Hsp") || CurrFlag.Contains("Lsp"))//超标
            //                    //    {
            //                    //        status += ",超标异常";
            //                    //        cell.BackColor = Color.FromName(getAuditConfigColor(0));
            //                    //    }
            //                    //    if (CurrFlag.Contains("UPd"))//倒挂
            //                    //    {
            //                    //        status += ",PM10、PM2.5倒挂";
            //                    //        cell.BackColor = Color.FromName(getAuditConfigColor(1));
            //                    //    }
            //                    //    if (CurrFlag.Split(',').Contains("H"))//有效数据不足
            //                    //    {
            //                    //        status += ",有效数据不足";
            //                    //    }
            //                    //    if (CurrFlag.Contains("Rep"))//重复异常
            //                    //    {
            //                    //        status += ",重复异常";
            //                    //    }
            //                    //    if (CurrFlag.Contains("Neg"))//负值异常
            //                    //    {
            //                    //        status += ",负值异常";
            //                    //    }
            //                    //    if (CurrFlag.Contains("Out"))//离群异常
            //                    //    {
            //                    //        status += ",离群异常";
            //                    //    }
            //                    //    if (!status.Equals(""))
            //                    //        cell.ToolTip += status.Substring(1, status.Length - 1);
            //                    //}
            //                    #endregion
            //                    #endregion
            //                }
            //            }
            //            else if (CurrUName.Equals("PointName") && col.Visible == true) //获取点位名称
            //            {
            //                TableCell cell = item[CurrUName];
            //                DataRowView row = (DataRowView)item.DataItem;
            //                cell.Text = radioPoint.Items.FindByValue(row["PointId"].ToString()).Text;
            //            }
            //            else if (CurrUName.Equals("AuditStatus") && col.Visible == true)//获取审核状态
            //            {
            //                Color color = Color.Black;
            //                TableCell cell = item[CurrUName];
            //                DataRowView row = (DataRowView)item.DataItem;
            //                cell.Text = GetStatusName(row["AuditStatus"] != DBNull.Value ? row["AuditStatus"].ToString() : "", out color);
            //                cell.ForeColor = color;

            //                //TableCell cell = item[CurrUName];
            //                //DataRowView row = (DataRowView)item.DataItem;
            //                //AuditStatusForDayEntity status = logService.RerieveAuditState(Convert.ToInt32(row["PointID"]), Convert.ToDateTime(row["DataDateTime"]).Date, Convert.ToDateTime(row["DataDateTime"]).Date, Session["applicationUID"].ToString());
            //                //Color color = Color.Black;
            //                //cell.Text = GetStatusName(status == null ? "" : status.Status, out color);
            //                //cell.ForeColor = color;
            //            }
            //            #endregion
            //        }
            //    }
            //    #endregion
            //}
            //catch
            //{
            //}
        }

        /// <summary>
        /// Grid加载时添加Column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditData_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 绑定Grid列
        /// </summary>
        private void AddColumnsOfGridAuditData(DataView dv)
        {
            gridAuditData.Columns.Clear();
            GridBoundColumn columnPoint = new GridBoundColumn() { HeaderText = "点位名称", DataField = "PointName", UniqueName = "PointName" };
            columnPoint.HeaderStyle.Width = new Unit(120);
            columnPoint.ReadOnly = true;
            gridAuditData.Columns.Add(columnPoint);

            GridBoundColumn columnDateTime = new GridBoundColumn() { DataField = "DateTime", UniqueName = "DateTime", HeaderText = "时间点", DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}" };
            columnDateTime.HeaderStyle.Width = new Unit(120);
            columnDateTime.ReadOnly = true;
            gridAuditData.Columns.Add(columnDateTime);

            gridAuditData.Columns.Add(new GridBoundColumn() { DataField = "PointID", UniqueName = "PointID", HeaderText = "PointID", Visible = false });
            for (int i = 3; i < dv.Table.Columns.Count - 5; i++)
            {
                GridBoundColumn col1 = new GridBoundColumn() { DataField = dv.Table.Columns[i].ColumnName, UniqueName = dv.Table.Columns[i].ColumnName.Trim(), HeaderText = dv.Table.Columns[i].ColumnName.Trim() };
                col1.ReadOnly = false;
                gridAuditData.Columns.Add(col1);
            }
            //for (int i = 0; i < factorCode.Length; i++)
            //{
            //    if (!factorCode[i].Equals(""))
            //    {
            //        string unit = "(mg/m<sup>3</sup>)";
            //        if (PollutantUnit[i].Equals("mg/L") && EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]) || PollutantUnit[i].Equals("mg/m3")) unit = "(mg/m<sup>3</sup>)";
            //        else unit = "(" + PollutantUnit[i] + ")";
            //        GridTemplateColumn col1 = new GridTemplateColumn() { DataField = factorCode[i], UniqueName = factorCode[i], HeaderText = factorName[i] + "</br>" + unit, ItemTemplate = new MySuperTemplate(factorCode[i], Convert.ToInt32(PollutantDecimalNum[i]), Session["applicationUID"].ToString()), EditItemTemplate = new MySuperTemplate() };
            //        col1.ReadOnly = IsEdit[i].ToString().Equals("1") ? true : false;
            //        gridAuditData.Columns.Add(col1);
            //    }
            //}
            if (dv.Table.Columns.Count > 13)
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
            RegisterScript("GridResize();");
            gridAuditData.Rebind();
            //RegisterScript("LoadingData();");
        }

        protected void RadDatePickerEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            //int days = (RadDatePickerEnd.SelectedDate.Value.Date - RadDatePickerBegin.SelectedDate.Value.Date).Days;
            //if (days >= GetAuditDays())
            //    RadDatePickerBegin.SelectedDate = RadDatePickerEnd.SelectedDate.Value.Date;
            RegisterScript("GridResize();");
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
            RegisterScript("GridResize();");
            gridAuditData.Rebind();
        }

        /// <summary>
        /// 审核操作后图表刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void refreshData_Click(object sender, EventArgs e)
        {
            RegisterScript("GridResize();");
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
            RegisterScript("GridResize();");
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
            insertAuditTable();//审核预处理表数据加载
            string Pointids = GetPointSelectedValue();
            gridAuditData.DataSource = null;
            PointIDHidden.Value = Pointids;
            InitChartFactors();//初始化图形单因子切换
            StatusInit();//获取点位审核状态
            RegisterScript("GridResize();");
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
                factorNames.Value = Session["FactorCodeSuper"].ToString() + "|" + Session["FactorNameSuper"].ToString() + "|" + Session["PollutantDecimalNumSuper"].ToString() + "|" + Session["PollutantUnitSuper"].ToString();//隐藏域赋值，用于显示Chart时AJAX传值用
                PointIDHidden.Value = Pointids;
            }
            else
            {
                Alert("请选择测点！");
                return;
            }
        }

        /// <summary>
        /// 图表因子单选 初始化
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
        /// 图表因子单选切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chartFactorRadio_SelectedIndexChanged(object sender, EventArgs e)
        {
            //factorNames.Value = chartFactorRadio.SelectedItem.Value + "|" + chartFactorRadio.SelectedItem.Text;//隐藏域赋值，用于显示Chart时AJAX传值用
            //RegisterScript("GridResize();LoadingData();");
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
            string CategoryUid = "";
            string isEdit = "0";
            for (int i = 0; i < radioPollutant.Items.Count; i++)
            {
                if (radioPollutant.Items[i].Selected)
                    CategoryUid = radioPollutant.Items[i].Value;
            }

            Session["FactorCategorySuper"] = CategoryUid;
            Session["isEditSuper"] = isEdit;
        }
    }
}