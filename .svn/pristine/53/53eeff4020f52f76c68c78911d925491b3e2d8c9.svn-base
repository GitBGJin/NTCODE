using SmartEP.DomainModel;
//using SmartEP.DomainModel.Framework;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.Web.WebServiceHelper;
using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartEP.WebUI.Common
{
    public class BasePage : System.Web.UI.Page
    {
        private int scriptFlag;
        #region Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Helper.SetTelerikTheme(this, Helper.GetTheme());
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            //Session["UserGuid"] = "8f2c499d-fd0c-49de-a31e-ff577969e82a";
            Helper.SetPageTheme(this, Helper.GetTheme());
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //FEC.WebUI.Common.SetPageTheme(this);

            if (!IsPostBack)
            {
                //单点出错登录地址
                string ssoErrorUrl = System.Configuration.ConfigurationManager.AppSettings["SsoErrorUrl"].ToString();
                //本地用户的Session为空
                if (string.IsNullOrEmpty(SessionHelper.Get("UserGuid")))
                {
                    if (!string.IsNullOrEmpty(Request["Token"]))
                    {
                        if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Token"]))
                        {
                            //令牌数据，用户标识加密后的数据
                            string token = System.Web.HttpContext.Current.Request.QueryString["Token"];
                            string desKey = Helper.GetAppSetting("DesKey");
                            string userGuid = Com.Sinoyd.Security.Security.DecryptDES(token, desKey);
                            if (!UserService.RegisterUser(userGuid))
                            {
                                Response.Redirect(ssoErrorUrl);
                            }
                        }
                        else
                        {
                            //Response.Redirect(ssoLoginUrl);
                        }
                    }
                    else
                    {
                        Response.Redirect(ssoErrorUrl);
                    }
                }
                else
                {
                    //Response.Redirect(ssoErrorUrl);
                    //if (!string.IsNullOrEmpty(Request["Token"]))
                    //{
                    //    //令牌数据，用户标识加密后的数据
                    //    string token = System.Web.HttpContext.Current.Request.QueryString["Token"];
                    //    string desKey = System.Configuration.ConfigurationManager.AppSettings["DesKey"].ToString();
                    //    string userGuid = Com.Sinoyd.Security.Security.DecryptDES(token, desKey);

                    //    if (userGuid != SessionHelper.Get("UserGuid"))
                    //    {
                    //        if (!UserService.RegisterUser(userGuid))
                    //        {
                    //            Response.Redirect(ssoLoginUrl);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //}
                }
            }
        }

        #endregion Methods


        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="message"></param>
        public void Alert(string message)
        {
            this.RegisterScript("alert('" + HttpUtility.HtmlEncode(message) + "')");
        }
        /// <summary>
        /// 执行前台脚本
        /// </summary>
        /// <param name="script"></param>
        public void RegisterScript(string script)
        {
            string Text = "<script language=\"javascript\" type = \"text/javascript\">\n" + script + "\n</script>";
            scriptFlag++;
            //base.ClientScript.RegisterStartupScript(base.GetType(), scriptFlag.ToString(), Text);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", Text, false);
        }
    }
}