namespace SmartEP.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public abstract class BasePage : Page
    {
        #region Fields

        private string accessLog;
        private string baseUrl;
        private string description = "描述";
        private bool isAccessAuthorization;
        private bool isRegister;
        private string keywords = "关键字";
        private string operationLog;
        private int scriptFlag;
        private string theme;
        private string title = "标题";
        private string userGuid;

        #endregion Fields

        #region Methods

        public static bool CheckPermission(string s_admin, int a)
        {
            string s_temp = "";
            s_temp = s_admin.Substring(a - 1, 1);   //s_admin为全局变量
            if (s_temp == "" || s_temp == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string switch_encrypt(string s_ch)
        {
            string s_out, s_temp, temp;
            int i_len = 64;
            if (i_len == 0 || s_ch == "")
            {
                s_out = "0000";
            }
            temp = "";
            s_temp = "";
            s_out = "";
            for (int i = 0; i <= i_len - 1; i = i + 4)
            {
                temp = s_ch.Substring(i, 4);
                switch (temp)
                {
                    case "1010": s_temp = "a";
                        break;
                    case "1011": s_temp = "b";
                        break;
                    case "1100": s_temp = "c";
                        break;
                    case "1101": s_temp = "d";
                        break;
                    case "1110": s_temp = "e";
                        break;
                    case "1111": s_temp = "f";
                        break;
                    case "0000": s_temp = "0";
                        break;
                    case "0001": s_temp = "1";
                        break;
                    case "0010": s_temp = "2";
                        break;
                    case "0011": s_temp = "3";
                        break;
                    case "0100": s_temp = "4";
                        break;
                    case "0101": s_temp = "5";
                        break;
                    case "0110": s_temp = "6";
                        break;
                    case "0111": s_temp = "7";
                        break;
                    case "1000": s_temp = "8";
                        break;
                    case "1001": s_temp = "9";
                        break;
                    default: s_temp = "0";
                        break;
                }
                s_out = s_out + s_temp;
                s_temp = "";
            }
            return s_out;
        }

        //解密
        public static string switch_riddle(string s_ch)
        {
            string s_out, s_temp, temp;
            int i_len = s_ch.Length;
            if (i_len == 0 || s_ch == "")
            {
                s_out = "0";
            }
            temp = "";
            s_temp = "";
            s_out = "";
            for (int i = 0; i <= i_len - 1; i++)
            {
                temp = s_ch.Substring(i, 1);
                switch (temp)
                {
                    case "a": s_temp = "1010";
                        break;
                    case "b": s_temp = "1011";
                        break;
                    case "c": s_temp = "1100";
                        break;
                    case "d": s_temp = "1101";
                        break;
                    case "e": s_temp = "1110";
                        break;
                    case "f": s_temp = "1111";
                        break;
                    case "0": s_temp = "0000";
                        break;
                    case "1": s_temp = "0001";
                        break;
                    case "2": s_temp = "0010";
                        break;
                    case "3": s_temp = "0011";
                        break;
                    case "4": s_temp = "0100";
                        break;
                    case "5": s_temp = "0101";
                        break;
                    case "6": s_temp = "0110";
                        break;
                    case "7": s_temp = "0111";
                        break;
                    case "8": s_temp = "1000";
                        break;
                    case "9": s_temp = "1001";
                        break;
                    default: s_temp = "0000";
                        break;
                }
                s_out = s_out + s_temp;
                s_temp = "";
            }
            return s_out;
        }

        public bool hasRegister()
        {
            return true;
        }

        /// <summary>
        /// 保存浏览日志
        /// </summary>
        /// <param name="accessLog"></param>
        //public abstract void SaveAccessLog(string accessLog);

        /// <summary>
        /// 保存操作日志（CRUD等操作）
        /// </summary>
        /// <param name="operationLog"></param>
        //public abstract void SaveOperationLog(string operationLog);

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //if (!isAccessAuthorization)
            //{
            //    //重定向到无权访问页面
            //    Response.Redirect("haveNoRights.aspx");
            //    return;
            //}
        }

        //public static bool hasAccessAuthorization()
        //{
        //}
        //public static List<UsersInOperationPermissions> operationPermissions()
        //{
        //}
        protected override void OnPreInit(EventArgs e)
        {
            //if (!isRegister)
            //{
            //    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.ApplicationPath + "/Pages/Register.aspx?ReturnUrl=" + Request.RawUrl);
            //}

            base.OnPreInit(e);
        }

        #endregion Methods

        #region Other

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

        ///// <summary>
        ///// 向客户端弹出Ajax对话框。
        ///// 在使用了ScriptManager的页面如果需要弹出窗口，则需要调用此方法
        ///// 邵晓军  2012-6-21
        ///// </summary>
        ///// <param name="UpdatePanel1">页面控件的UpdatePanel名称</param>
        ///// <param name="scriptValue">需要弹出的话语</param>
        //public void AlertInAjaxPanel(System.Web.UI.UpdatePanel UpdatePanel, string script)
        //{
        //    System.Web.UI.ScriptManager.RegisterStartupScript(
        //       UpdatePanel,
        //       this.GetType(),
        //       Guid.NewGuid().ToString(),
        //       "alert('" + HttpUtility.HtmlEncode(script) + "');",
        //       true);
        //}
        ///// <summary>
        ///// 向客户端弹出Ajax对话框。
        ///// 在使用了ScriptManager的页面如果需要弹出窗口，则需要调用此方法
        ///// 邵晓军  2012-6-21
        ///// </summary>
        ///// <param name="scriptValue">The script value.</param>
        //public void AlertInAjax(string script)
        //{
        //    System.Web.UI.ScriptManager.RegisterStartupScript(
        //       this,
        //       this.GetType(),
        //       Guid.NewGuid().ToString(),
        //       "alert('" + HttpUtility.HtmlEncode(script) + "');",
        //       true);
        //}
        ///// <summary>
        ///// 向客户端写入Ajax脚本语言
        ///// 在使用了ScriptManager的页面如果需要执行脚本方法，则需要调用此方法
        ///// 邵晓军  2012-6-21
        ///// </summary>
        ///// <param name="UpdatePanel1">页面控件的UpdatePanel名称</param>
        ///// <param name="scriptValue">需要执行脚本方法</param>
        //public void RegisterScriptInAjaxPanel(System.Web.UI.UpdatePanel UpdatePanel, string script)
        //{
        //    System.Web.UI.ScriptManager.RegisterStartupScript(
        //       UpdatePanel,
        //       this.GetType(),
        //       Guid.NewGuid().ToString(),
        //       script,
        //       true);
        //}
        ///// <summary>
        ///// 向客户端写入Ajax脚本语言
        ///// 在使用了ScriptManager的页面如果需要执行脚本方法，则需要调用此方法
        ///// 邵晓军  2012-6-21
        ///// </summary>
        ///// <param name="scriptValue">The script value.</param>
        //public void RegisterScriptInAjax(string script)
        //{
        //    System.Web.UI.ScriptManager.RegisterStartupScript(
        //       this,
        //       this.GetType(),
        //       Guid.NewGuid().ToString(),
        //       script,
        //       true);
        //}

        #endregion Other
    }
}