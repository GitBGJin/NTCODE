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
//using SmartEP.Service.DataAuditing.AuditBaseInfo;
//using SmartEP.Service.DataAuditing.AuditInterfaces;
using System.Data;
using System.Configuration;
using SmartEP.WebUI.Controls;
using SmartEP.Utilities.Web.UI;
using SmartEP.Core.Enums;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class AuditData : SmartEP.WebUI.Common.BasePage
    {
        #region 属性
        AuditStatusMappingService statusService = new AuditStatusMappingService();
        AuditDataService auditService = new AuditDataService();
        AuditLogService logService = new AuditLogService();
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
                //pointType：空气（0：国控点；1：市控点位；2：超级站点位；3：省控点位；4：区控点位；5：创模点位;6：中意项目）
                //           地表水（0：水源地；1：城区河道；2：浮标站；3：人工监测点；）     
                if (Request.QueryString["pointType"] != null)
                    ViewState["pointType"] = Request.QueryString["pointType"];
                else
                    ViewState["pointType"] = "0";
                InitControl();
            }
        }

        #region 初始化控件
        private void InitControl()
        {
            RadDatePickerBegin.SelectedDate = Request.QueryString["DTBegin"] != null ? Convert.ToDateTime(Request.QueryString["DTBegin"].ToString()) : DateTime.Now;
            //lastPort.Attributes["OnClick"] = "return false;";//使LinkButton不提交
            //nextPort.Attributes["OnClick"] = "return false;";
            PointBind();//初始化点位
            StatusInit();//获取点位审核状态
            //factorCbxRsm.FactorBind(Convert.ToInt32(RadPortTree.SelectedNode.Value));//绑定SiteMap因子
            factorCbxRsm.FactorBind(Convert.ToInt32(radioPoint.SelectedValue));//绑定SiteMap因子

            //国控点不能提交审核
            if (ViewState["pointType"].Equals("0") && EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]))
            {
                SubmitAuditTD.Visible = false;
            }
        }

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
            if (radioPoint.Items.Count > 0)
            {
                if (Session["PointID"] != null || Request.QueryString["PointID"] != null)
                {
                    string pointID = Request.QueryString["PointID"] != null ? Request.QueryString["PointID"].ToString() : "";

                    ListItem item = radioPoint.Items.FindByValue(pointID.Equals("") ? Session["PointID"].ToString() : pointID);
                    if (item == null)
                        radioPoint.Items[0].Selected = true;
                    else
                        item.Selected = true;
                }
                else
                    radioPoint.Items[0].Selected = true;
            }
        }
        #endregion

        #region 获取点位和审核状态
        public void StatusInit()
        {
            portName.Text = radioPoint.SelectedItem.Text + "：";
            AuditStatusForDayEntity status = logService.RerieveAuditState(Convert.ToInt32(radioPoint.SelectedValue), RadDatePickerBegin.SelectedDate.Value, RadDatePickerBegin.SelectedDate.Value, Session["applicationUID"].ToString());
            if (status == null)
                auditState.Text = "无数据";
            else if (status.Status.Equals("0"))
                auditState.Text = "未审核";
            else if (status.Status.Equals("1"))
                auditState.Text = "已审核";
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
            gridAuditData.MasterTableView.ClearEditItems();
            //string[] pointid = RadPortTree.SelectedNode.Value.Split(';');
            string[] pointid = radioPoint.SelectedValue.Split(';');
            gridAuditData.DataSource = auditService.RetrieveAuditHourData(Session["applicationUID"].ToString(), pointid, Session["FactorCode"].ToString().Split(';'), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerBegin.SelectedDate.Value.Date.AddDays(1).AddHours(-1), true, IsShowTotal.Checked == true ? true : false);
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
                        if (!CurrUName.Equals("PointId") && !CurrUName.Equals("DataDateTime") && col.Visible == true)
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
                                if (CurrFlag.Contains(",RM"))//自动或人工置为无效
                                {
                                    cell.ToolTip += "置为无效";
                                }
                                else if (CurrFlag.Equals("C"))//修改过
                                {
                                    cell.ToolTip += "修改";
                                }
                                else
                                {
                                    string status = "";
                                    if (CurrFlag.Contains(",HSp") || CurrFlag.Contains(",LSp"))//超标
                                    {
                                        status += ",超标异常";
                                        cell.BackColor = Color.FromName(getAuditConfigColor(0));
                                    }
                                    if (CurrFlag.Contains(",UPd"))//倒挂
                                    {
                                        status += ",PM10、PM2.5倒挂";
                                        cell.BackColor = Color.FromName(getAuditConfigColor(1));
                                    }
                                    if (CurrFlag.Contains(",H"))//有效数据不足
                                    {
                                        status += ",有效数据不足";
                                    }
                                    if (CurrFlag.Contains(",Rep"))//重复异常
                                    {
                                        status += ",重复异常";
                                    }
                                    if (CurrFlag.Contains(",Neg"))//负值异常
                                    {
                                        status += ",负值异常";
                                    }
                                    if (CurrFlag.Contains(",Out"))//离群异常
                                    {
                                        status += ",离群异常";
                                    }
                                    if (!status.Equals(""))
                                        cell.ToolTip += status.Substring(1, status.Length - 1);
                                }
                                #endregion
                            }
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
            //factorCbxRsm.FactorBind(Convert.ToInt32(radioPoint.SelectedValue));//绑定SiteMap因子
            factorCbxRsm.GetFactors();//获取sitemap因子
            StatusInit();//获取点位审核状态
            factorNames.Value = Session["FactorCode"].ToString() + "|" + Session["FactorName"].ToString() + "|" + Session["PollutantDecimalNum"].ToString() + "|" + Session["PollutantUnit"].ToString();//隐藏域赋值，用于显示Chart时AJAX传值用
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
            gridAuditData.Columns.Clear();
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
            if (factorCode.Length > 13)
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
            Session["PointID"] = radioPoint.SelectedValue;
            factorCbxRsm.FactorBind(Convert.ToInt32(radioPoint.SelectedValue));//绑定SiteMap因子
            factorCbxRsm.GetFactors();//获取sitemap因子
            factorNames.Value = Session["FactorCode"].ToString() + "|" + Session["FactorName"].ToString() + "|" + Session["PollutantDecimalNum"].ToString() + "|" + Session["PollutantUnit"].ToString();//隐藏域赋值，用于显示Chart时AJAX传值用
            gridAuditData.Rebind();
            StatusInit();//获取点位审核状态
            RegisterScript("LoadingData();");
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
            String myUrl = "AuditState.aspx?Token=" + Request.QueryString["Token"] + "&app=" + app + "&pointType=" + ViewState["pointType"] + "&dtBegin=" + RadDatePickerBegin.SelectedDate.Value.Date;//AuditState.aspx
            Response.Redirect(myUrl, true);
        }

        /// <summary>
        /// 日期重选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadDatePickerBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            //if (RadDatePickerBegin.SelectedDate.Value.Date > DateTime.Now.Date)
            //{
            //    RadDatePickerBegin.SelectedDate= e.OldDate;
            //    Alert("审核时间超出范围！");
            //    return;
            //}              
            //else
            gridAuditData.Rebind();
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
        #endregion

        #region 自定义方法
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

        protected void SubmitAudit_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #endregion
    }


}