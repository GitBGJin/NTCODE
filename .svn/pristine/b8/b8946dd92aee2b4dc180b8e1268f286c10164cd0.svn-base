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
using SmartEP.Utilities.AdoData;

namespace SmartEP.WebUI.Portal
{
    public partial class LoginLZ : System.Web.UI.Page
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
            {}
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

                // 记录登录成功日志 add by wangtq 2016.5.24
                DatabaseHelper dbinsert = new DatabaseHelper();
                string loginID = userObj.LoginID;
                string displayName = userObj.DisplayName;
                string loginNote = "登录成功";
                HttpRequest request = HttpContext.Current.Request;
                string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                string loginTime = DateTime.Now.ToString();
                string sql = @"insert into TB_Frame_LoginLog (RowGuid,UserGuid,DisplayName,LoginID,LoginTime,LoginIP,LoginNote,RowStatus)  values(newid(),'" + userObj.RowGuid + "','" + displayName + "','" + loginID + "','" + loginTime + "','" + ip + "','" + loginNote + "','1')";
                dbinsert.ExecuteNonQuery(sql, "Frame_Connection");

                Response.Redirect("HomePageLZ.aspx?token=" + token);

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

                // 记录登录失败日志 add by wangtq 2016.5.25
                DatabaseHelper dbinsert = new DatabaseHelper();
                System.Guid guid = new Guid();
                guid = Guid.NewGuid();
                string userid = guid.ToString(); ;
                string loginID = this.RadTxtUser.Text;
                string displayName = "";
                string loginNote = "登录失败";
                HttpRequest request = HttpContext.Current.Request;
                string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                string loginTime = DateTime.Now.ToString();
                string sql = @"insert into TB_Frame_LoginLog (RowGuid,UserGuid,DisplayName,LoginID,LoginTime,LoginIP,LoginNote,RowStatus)  values(newid(),'" + userid + "','" + displayName + "','" + loginID + "','" + loginTime + "','" + ip + "','" + loginNote + "','0')";
                dbinsert.ExecuteNonQuery(sql, "Frame_Connection");

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