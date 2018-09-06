using Com.Sinoyd.Security;
using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Portal
{
    public partial class MidPageLZ : SmartEP.WebUI.Common.BasePageLZ
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitUrl();
            }
        }
        private void InitUrl()
        {
            string userGuid = Session["UserGuid"].ToString();
            string Type = Request["Type"];
            string Token = Security.EncryptDES(userGuid, "12345678");

            #region 获取页面url
            string frameUrl = ConfigurationManager.AppSettings["PortalUrl"].ToString();
            string homePageUrl = frameUrl + "/" + ConfigurationManager.AppSettings["PortalName"].ToString();
            frameUrl += "/" + ConfigurationManager.AppSettings["FrameworkName"].ToString();
            string gISLZUrl = ConfigurationManager.AppSettings["GISLZUrl"].ToString();
            string waterlzRole = ConfigurationManager.AppSettings["WaterLZRole"].ToString();
            #endregion

            #region 通过用户Guid获取用户的角色
            string FrameRoleWebServiceUrl = ConfigurationManager.AppSettings["FrameRoleServiceUrl"].ToString();
            string flag = ConfigurationManager.AppSettings["Flag"].ToString();
            string strRoleGuidList = WebServiceHelper.InvokeWebService(FrameRoleWebServiceUrl, "GetRoleGuidList", new object[] { userGuid, flag }).ToString();
            #endregion
            if (strRoleGuidList == "")
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "alert", "<script>alert('无权限访问，请联系管理员！');top.location ='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "'</script>");
            }
            else
            {
                switch (Type)
                {
                    case "Mgr":
                        Response.Redirect(frameUrl + "/FrameMgr.aspx?Token=" + Token);
                        break;
                    case "WaterLZ":
                        Response.Redirect(frameUrl + "/FrameWaterLZ.aspx?Token=" + Token);
                        break;
                    case "GisLZ":
                        Response.Redirect(gISLZUrl);
                        break;

                }
            }
        }
        
    }
}