using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using SmartEP.Core.Enums;
using Telerik.Web.UI;
using System.Data;
using SmartEP.DomainModel.MonitoringBusiness;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class AuditFactorLogInfo : System.Web.UI.Page
    {
        #region 属性
        AuditLogService auditLogService = new AuditLogService();//审核数据接口
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region 参数初始化
                ViewState["PointID"] = Request.QueryString["PointID"];
                ViewState["DataTime"] = Request.QueryString["DataTime"];
                ViewState["factorCode"] = Request.QueryString["factorCode"];
                #endregion
            }
        }


        /// <summary>
        /// 绑定审核日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditLog_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
                gridAuditLog.DataSource = auditLogService.RerieveAirLog(Convert.ToInt32(ViewState["PointID"]), Session["applicationUID"].ToString(), ViewState["factorCode"].ToString(), Convert.ToDateTime(ViewState["DataTime"]), Convert.ToDateTime(ViewState["DataTime"]));
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"].ToString()))//地表水
                gridAuditLog.DataSource = auditLogService.RerieveWaterLog(Convert.ToInt32(ViewState["PointID"]), Session["applicationUID"].ToString(), ViewState["factorCode"].ToString(), Convert.ToDateTime(ViewState["DataTime"]), Convert.ToDateTime(ViewState["DataTime"]));
        }

        protected void gridAuditLog_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))
            {
                try
                {
                    RadGrid myRadGrid = ((RadGrid)sender);
                    if (e.Item is GridDataItem && myRadGrid.MasterTableView.DataSourceCount > 0)
                    {
                        GridDataItem item = (GridDataItem)e.Item;
                        foreach (GridColumn col in myRadGrid.MasterTableView.RenderColumns)
                        {
                            string CurrUName = col.UniqueName;
                            if (CurrUName.Equals("tstamp") && col.Visible == true)
                            {
                                AuditAirLogEntity log = (AuditAirLogEntity)item.DataItem;
                                TableCell cell = item[CurrUName];
                                cell.Text = Convert.ToDateTime(log.Tstamp).AddHours(1).ToString("dd日HH点");

                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }
    }
}