﻿using SmartEP.DomainModel;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.Web.WebServiceHelper;
using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SmartEP.Utilities.AdoData;

namespace SmartEP.WebUI.Common
{
    public class BasePageLZ : System.Web.UI.Page
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
                    Response.Redirect(ssoErrorUrl);
                }
                else
                {
                    string userGuid = SessionHelper.Get("UserGuid");

                    string token = System.Web.HttpContext.Current.Request.QueryString["Token"];
                    string desKey = System.Configuration.ConfigurationManager.AppSettings["DesKey"].ToString();
                    string userid = Com.Sinoyd.Security.Security.DecryptDES(token, desKey);

                    SaveLog(userGuid);
                }
            }
        }

        #endregion Methods

        // save loginlog add by wangtq 2016.5,24
        //public void SaveLog(string userGuid, string loginID, string displayName, DateTime loginTime, string ip, string loginNote)
        public void SaveLog(string userGuid)
        {
            DatabaseHelper dbinsert = new DatabaseHelper();
            string loginID = string.Empty;
            string displayName = string.Empty;
            DataTable dt = null;
            dt = getInfo(userGuid);
            if (dt != null)
            {
                loginID = dt.Rows[0][0].ToString();
                displayName = dt.Rows[0][1].ToString();
            }

            HttpRequest request = HttpContext.Current.Request;
            string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
            string loginTime = DateTime.Now.ToString();
            string rowStatus = "";
            string loginNote = LoginNote();
            if (string.IsNullOrWhiteSpace(loginNote))
            {
                rowStatus = "0";
            }
            else
            {
                rowStatus = "1";
            }
            string sql = @"insert into TB_Frame_LoginLog (RowGuid,UserGuid,DisplayName,LoginID,LoginTime,LoginIP,LoginNote,RowStatus)  values(newid(),'" + userGuid + "','" + displayName + "','" + loginID + "','" + loginTime + "','" + ip + "','" + loginNote + "','" + rowStatus + "')";
            dbinsert.ExecuteNonQuery(sql, "Frame_Connection");
        }

        //根据 userGuid 获取 loginid 和 displayName
        public DataTable getInfo(string userGuid)
        {
            DatabaseHelper db = new DatabaseHelper();
            string strSql = @"select LoginID, DisplayName from TB_Frame_User where RowGuid='" + userGuid + "'";
            DataTable dt = db.ExecuteDataTable(strSql, "Frame_Connection");
            return dt;
        }

        // 获取 url,然后根据 url 获取 loginNote
        public string LoginNote()
        {
            DatabaseHelper db = new DatabaseHelper();
            string url = string.Empty;
            string url_a = string.Empty;
            string note = string.Empty;
            url_a = Request.FilePath;
            url = Request.CurrentExecutionFilePath;

            string getUrl = string.Empty;

            string[] urls = url.Split('/');
            for (int j = 2; j < urls.Length; j++)
            {
                getUrl += urls[j] + '/';
            }

            if (getUrl.EndsWith("/"))
            {
                getUrl = getUrl.Substring(0, getUrl.Length - 1);
            }

            string sql = @"select ModuleName from TB_Frame_Module where ModuleUrl like '%" + getUrl + "%' and IsEnabled='1'";
            if (db.ExecuteScalar(sql, "Frame_Connection") != null)
            {
                note = db.ExecuteScalar(sql, "Frame_Connection").ToString();
            }

            return note;

        }

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