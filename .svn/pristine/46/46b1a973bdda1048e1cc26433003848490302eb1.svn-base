using SmartEP.WebUI.Common;
using SmartEP.DomainModel;
using SmartEP.Utilities.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Portal
{
    public partial class Login : System.Web.UI.Page
    {
        FrameworkModel _userModel = new FrameworkModel();

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Web.HttpContext.Current.Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();
            }
            catch
            {
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            string name = RadTxtUser.Text;
            string pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(RadTxtPwd.Text, "SHA1");

            Frame_UserEntity userObj = _userModel.Frame_UserEntities.FirstOrDefault(x => x.LoginID.Equals(name) && x.Password.Equals(pwd) && x.IsEnabled != (Nullable<int>)null && x.IsEnabled.Value == 1);

            if (userObj != null)
            {
                SessionHelper.Add("UserGuid", userObj.RowGuid);
                SessionHelper.Add("DisplayName", userObj.DisplayName);
                SessionHelper.Add("LoginID", userObj.LoginID);
                System.Web.HttpContext.Current.Session.Timeout = 600;

                string desKey = System.Configuration.ConfigurationManager.AppSettings["DesKey"].ToString();
                string token = Com.Sinoyd.Security.Security.EncryptDES(userObj.RowGuid, desKey);
                Response.Redirect("HomePage.aspx?token=" + token);
                // Response.Redirect("WZFramework/FrameAll3.aspx");
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(
               this,
               this.GetType(),
               Guid.NewGuid().ToString(),
               "alert('" + HttpUtility.HtmlEncode("系统不能完成您的登录请求，请检查您的用户名和密码是否匹配!!") + "');",
               true);

                return;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.RadTxtUser.Text = string.Empty;
            this.RadTxtPwd.Text = string.Empty;
        }
    }
}