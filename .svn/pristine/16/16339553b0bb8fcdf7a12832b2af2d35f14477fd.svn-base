using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.DataAuditing.AuditBaseInfo;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Controls;
using SmartEP.Core.Enums;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class MutilPointChart : SmartEP.WebUI.Common.BasePage
    {
        AirAuditMonitoringPointService pointAirService = new AirAuditMonitoringPointService();//点位接口
        WaterAuditMonitoringPointService pointWaterService = new WaterAuditMonitoringPointService();//点位接口

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["pointType"] != null)
                {
                    ViewState["pointType"] = Request.QueryString["pointType"];
                    InitControl();//初始化控件
                }
            }
        }

        #region 初始化控件
        private void InitControl()
        {
            PointBind();
        }
        #region 点位绑定
        private void PointBind()
        {
            IQueryable<MonitoringPointEntity> pointList = null;
            if (Session["applicationUID"] != null && EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]))
                pointList = pointAirService.RetrieveAirByAuditType(ViewState["pointType"].ToString(), Session["UserGuid"].ToString());
            else
                pointList = pointWaterService.RetrieveWaterByAuditType(ViewState["pointType"].ToString(), Session["UserGuid"].ToString());

            if (pointList != null)
            {
                RadPortTree.DataSource = pointList;
                RadPortTree.DataFieldID = "MonitoringPointUid";
                RadPortTree.DataValueField = "PointId";
                RadPortTree.DataTextField = "MonitoringPointName";
                //RadPortTree.DataFieldParentID = "ContrlUid";
                RadPortTree.DataBind();

                if (RadPortTree.Nodes.Count > 0)
                {
                    foreach (RadTreeNode parentNode in RadPortTree.Nodes)
                    {
                        parentNode.Checked = true;
                    }
                }
            }
        }
        #endregion
        #endregion

        #region 事件

        #endregion
    }
}