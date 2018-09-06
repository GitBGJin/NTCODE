using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Portal
{
    public partial class LeftTreeLZ : SmartEP.WebUI.Common.BasePageLZ
    {
        FrameModuleService g_FrameModuleBiz = new FrameModuleService();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                if (this.Request.QueryString["ParentModuleGuid"] != null)
                {
                    InitPanelBar();
                }
            }
        }

        /// <summary>
        /// 菜单初期化
        /// </summary>
        private void InitPanelBar()
        {
            string desKey = System.Configuration.ConfigurationManager.AppSettings["DesKey"].ToString();
            string userGuid = this.Session["UserGuid"].ToString();
            string token = Com.Sinoyd.Security.Security.EncryptDES(userGuid, desKey);
            string parentModuleGuid = this.Request.QueryString["ParentModuleGuid"].ToString();
            //左侧菜单初始化
            g_FrameModuleBiz.InitLeftPanelBar(RadPanelBar1, userGuid, parentModuleGuid, token);
        }
    }
}