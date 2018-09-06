using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System;

namespace SmartEP.Utilities.Web.UI
{
    /// <summary>
    /// 客户端脚本输出
    /// </summary>
    public class JavascriptHelper
    {
        #region HttpContext.Current.Response.Write
        /// <summary>
        /// 弹出信息,并跳转指定页面。
        /// </summary>
        public static void AlertAndRedirect(string message, string toURL)
        {
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, message, toURL));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出信息,并返回历史页面
        /// </summary>
        public static void AlertAndGoHistory(string message, int value)
        {
            string js = @"<Script language='JavaScript'>alert('{0}');history.go({1});</Script>";
            HttpContext.Current.Response.Write(string.Format(js, message, value));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 直接跳转到指定的页面
        /// </summary>
        public static void Redirect(string toUrl)
        {
            string js = @"<script language=javascript>window.location.replace('{0}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, toUrl));
        }

        /// <summary>
        /// 弹出信息 并指定到父窗口
        /// </summary>
        public static void AlertAndParentUrl(string message, string toURL)
        {
            string js = "<script language=javascript>alert('{0}');window.top.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, message, toURL));
        }

        /// <summary>
        /// 返回到父窗口
        /// </summary>
        public static void ParentRedirect(string ToUrl)
        {
            string js = "<script language=javascript>window.top.location.replace('{0}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, ToUrl));
        }

        /// <summary>
        /// 返回历史页面
        /// </summary>
        public static void BackHistory(int value)
        {
            string js = @"<Script language='JavaScript'>history.go({0});</Script>";
            HttpContext.Current.Response.Write(string.Format(js, value));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出信息
        /// </summary>
        public static void Alert(string message)
        {
            string js = "<script language=javascript>alert('{0}');</script>";
            HttpContext.Current.Response.Write(string.Format(js, message));
        }
        #endregion

        #region RegisterStartupScript
        #region 信息显示

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="message"></param>
        public static void ShowMG(string message)
        {
            WriteAjaxScript("alert('" + message + "');");
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="message">提示信息</param>
        public static void ShowMessage(string message)
        {
            ShowMessage("系统提示", 180, 120, message);
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="message">提示信息</param>
        public static void ShowMessage_link(string message, string linkurl)
        {
            ShowMessage_link("系统提示", 180, 120, message, linkurl, 8000, -1);
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="message">提示信息</param>
        private static void ShowMessage(string title, int width, int height, string message)
        {
            ShowMessage(title, width, height, message, 3000, -1);
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="message"></param>
        /// <param name="delayms"></param>
        /// <param name="leftSpace"></param>
        private static void ShowMessage(string title, int width, int height, string message, int delayms, int leftSpace)
        {
            WriteAjaxScript(string.Format("popMessage({0},{1},'{2}','{3}',{4},{5});", width, height, title, message, delayms, leftSpace == -1 ? "null" : leftSpace.ToString()));
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="message"></param>
        /// <param name="delayms"></param>
        /// <param name="leftSpace"></param>
        private static void ShowMessage_link(string title, int width, int height, string message, string linkurl, int delayms, int leftSpace)
        {
            WriteAjaxScript(string.Format("popMessage2({0},{1},'{2}','{3}','{4}',{5},{6});", width, height, title, message, linkurl, delayms, leftSpace == -1 ? "null" : leftSpace.ToString()));
        }

        #endregion

        #region 显示消息提示对话框
        /// <summary>
        /// 显示消息提示对话框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void Show(System.Web.UI.Page page, string msg)
        {
            // page.RegisterStartupScript("message", "<script language='javascript' defer>alert('" + msg.ToString() + "');</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>alert('" + msg.ToString() + "');</script>");
        }

        #endregion

        #region 控件点击 消息确认提示框

        /// <summary>
        /// 控件点击 消息确认提示框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void ShowConfirm(System.Web.UI.WebControls.WebControl Control, string msg)
        {
            Control.Attributes.Add("onclick", "return confirm('" + msg + "');");
        }
        #endregion

        #region 显示消息提示对话框，并进行页面跳转
        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowAndRedirect(System.Web.UI.Page page, string msg, string url)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("alert('{0}');", msg);
            Builder.AppendFormat("location.href='{0}'", url);
            Builder.Append("</script>");
            //page.RegisterStartupScript("message", Builder.ToString());
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());

        }

        public static void ShowAndRedirect(System.Web.UI.Page page, string msg, string url, bool top)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("alert('{0}');", msg);
            if (top == true)
            {
                Builder.AppendFormat("top.location.href='{0}'", url);
            }
            else
            {
                Builder.AppendFormat("location.href='{0}'", url);
            }
            Builder.Append("</script>");
            // page.RegisterStartupScript("message", Builder.ToString());
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());

        }

        #endregion

        #region 显示异常信息

        /// <summary>
        /// 显示异常信息
        /// </summary>
        /// <param name="ex"></param>
        public static void ShowExceptionMessage(Exception ex)
        {
            ShowExceptionMessage(ex.Message);
        }

        /// <summary>
        /// 显示异常信息
        /// </summary>
        /// <param name="message"></param>
        public static void ShowExceptionMessage(string message)
        {
            WriteAjaxScript("alert('" + message + "');");
            //PageHelper.ShowExceptionMessage("错误提示", 210, 125, message);
        }

        /// <summary>
        /// 显示异常信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="message"></param>
        private static void ShowExceptionMessage(string title, int width, int height, string message)
        {
            WriteAjaxScript(string.Format("setTimeout(\"showAlert('{0}',{1},{2},'{3}')\",100);", title, width, height, message));
        }
        #endregion

        #region 显示模态窗口

        /// <summary>
        /// 返回把指定链接地址显示模态窗口的脚本
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="url"></param>
        public static string GetShowModalWindowScript(string wid, string title, int width, int height, string url)
        {
            return string.Format("setTimeout(\"showModalWindow('{0}','{1}',{2},{3},'{4}')\",100);", wid, title, width, height, url);
        }

        /// <summary>
        /// 把指定链接地址显示模态窗口
        /// </summary>
        /// <param name="wid">窗口ID</param>
        /// <param name="title">标题</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="url">链接地址</param>
        public static void ShowModalWindow(string wid, string title, int width, int height, string url)
        {
            WriteAjaxScript(GetShowModalWindowScript(wid, title, width, height, url));
        }

        /// <summary>
        /// 为指定控件绑定前台脚本：显示模态窗口
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventName"></param>
        /// <param name="wid"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="url"></param>
        /// <param name="isScriptEnd"></param>
        public static void ShowCilentModalWindow(string wid, WebControl control, string eventName, string title, int width, int height, string url, bool isScriptEnd)
        {
            string script = isScriptEnd ? "return false;" : "";
            control.Attributes[eventName] = string.Format("showModalWindow('{0}','{1}',{2},{3},'{4}');" + script, wid, title, width, height, url);
        }

        /// <summary>
        /// 为指定控件绑定前台脚本：显示模态窗口
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="eventName"></param>
        /// <param name="wid"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="url"></param>
        /// <param name="isScriptEnd"></param>
        public static void ShowCilentModalWindow(string wid, TableCell cell, string eventName, string title, int width, int height, string url, bool isScriptEnd)
        {
            string script = isScriptEnd ? "return false;" : "";
            cell.Attributes[eventName] = string.Format("showModalWindow('{0}','{1}',{2},{3},'{4}');" + script, wid, title, width, height, url);
        }
        #endregion

        #region 显示客户端确认窗口
        /// <summary>
        /// 显示客户端确认窗口
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        public static void ShowCilentConfirm(WebControl control, string eventName, string message)
        {
            ShowCilentConfirm(control, eventName, "系统提示", 210, 125, message);
        }

        /// <summary>
        /// 显示客户端确认窗口
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventName"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="message"></param>
        public static void ShowCilentConfirm(WebControl control, string eventName, string title, int width, int height, string message)
        {
            control.Attributes[eventName] = string.Format("return showConfirm('{0}',{1},{2},'{3}','{4}');", title, width, height, message, control.ClientID);
        }

        #endregion

        #region 输出自定义脚本信息
        /// <summary>
        /// 输出自定义脚本信息,写javascript脚本
        /// </summary>
        /// <param name="script">脚本内容</param>
        public static void WriteAjaxScript(string script)
        {
            Page page = GetCurrentPage();
            page.ClientScript.RegisterStartupScript(page.GetType(), System.Guid.NewGuid().ToString(), script, true);
        }
        #endregion

        #region 得到当前页对象实例
        /// <summary>
        /// 得到当前页对象实例
        /// </summary>
        /// <returns></returns>
        public static Page GetCurrentPage()
        {
            return (Page)HttpContext.Current.Handler;
        }
        #endregion

        #endregion

        #region JavaScriptPlus

        #region 回到历史页面
        /// <summary>
        /// 回到历史页面
        /// </summary>
        /// <param name="value">-1/1</param>
        public static void GoHistory(int value)
        {
            #region
            string js = @"<Script language='JavaScript'>
                    history.go({0});  
                  </Script>";
            HttpContext.Current.Response.Write(string.Format(js, value));
            #endregion
        }
        #endregion

        #region 关闭当前窗口
        /// <summary>
        /// 关闭当前窗口
        /// </summary>
        public static void CloseWindow()
        {
            #region
            string js = @"<Script language='JavaScript'>
                    parent.opener=null;window.close();  
                  </Script>";
            HttpContext.Current.Response.Write(js);
            HttpContext.Current.Response.End();
            #endregion
        }
        #endregion

        #region 刷新父窗口
        /// <summary>
        /// 刷新父窗口
        /// </summary>
        public static void RefreshParent(string url)
        {
            #region
            string js = @"<script>try{top.location=""" + url + @"""}catch(e){location=""" + url + @"""}</script>";
            HttpContext.Current.Response.Write(js);
            #endregion
        }
        #endregion

        #region 刷新打开窗口
        /// <summary>
        /// 刷新打开窗口
        /// </summary>
        public static void RefreshOpener()
        {
            #region
            string js = @"<Script language='JavaScript'>
                    opener.location.reload();
                  </Script>";
            HttpContext.Current.Response.Write(js);
            #endregion
        }
        #endregion

        #region 转向Url指定的页面
        /// <summary>
        /// 转向Url指定的页面
        /// </summary>
        /// <param name="url">连接地址</param>
        public static void JavaScriptLocationHref(string url)
        {
            #region
            string js = @"<Script language='JavaScript'>
                    window.location.replace('{0}');
                  </Script>";
            js = string.Format(js, url);
            HttpContext.Current.Response.Write(js);
            #endregion
        }
        #endregion

        #region 打开指定大小位置的模式对话框
        /// <summary>
        /// 打开指定大小位置的模式对话框
        /// </summary>
        /// <param name="webFormUrl">连接地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="top">距离上位置</param>
        /// <param name="left">距离左位置</param>
        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left)
        {
            #region
            string features = "dialogWidth:" + width.ToString() + "px"
                + ";dialogHeight:" + height.ToString() + "px"
                + ";dialogLeft:" + left.ToString() + "px"
                + ";dialogTop:" + top.ToString() + "px"
                + ";center:yes;help=no;resizable:no;status:no;scroll=yes";
            ShowModalDialogWindow(webFormUrl, features);
            #endregion
        }
        #endregion

        #region 打开模式对话框
        /// <summary>
        /// 打开模式对话框
        /// </summary>
        /// <param name="webFormUrl">链接地址</param>
        /// <param name="features"></param>
        public static void ShowModalDialogWindow(string webFormUrl, string features)
        {
            string js = ShowModalDialogJavascript(webFormUrl, features);
            HttpContext.Current.Response.Write(js);
        }

        /// <summary>
        /// 打开模式对话框
        /// </summary>
        /// <param name="webFormUrl"></param>
        /// <param name="features"></param>
        /// <returns></returns>
        public static string ShowModalDialogJavascript(string webFormUrl, string features)
        {
            #region
            string js = @"<script language=javascript>							
							showModalDialog('" + webFormUrl + "','','" + features + "');</script>";
            return js;
            #endregion
        }
        #endregion

        #region 打开指定大小的新窗体
        /// <summary>
        /// 打开指定大小的新窗体
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="width">宽</param>
        /// <param name="heigth">高</param>
        /// <param name="top">头位置</param>
        /// <param name="left">左位置</param>
        public static void OpenWebFormSize(string url, int width, int heigth, int top, int left)
        {
            #region
            string js = @"<Script language='JavaScript'>window.open('" + url + @"','','height=" + heigth + ",width=" + width + ",top=" + top + ",left=" + left + ",location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');</Script>";

            HttpContext.Current.Response.Write(js);
            #endregion
        }
        #endregion

        #region 页面跳转（跳出框架）
        /// <summary>
        /// 页面跳转（跳出框架）
        /// </summary>
        /// <param name="url"></param>
        public static void JavaScriptExitIfream(string url)
        {
            string js = @"<Script language='JavaScript'>
                    parent.window.location.replace('{0}');
                  </Script>";
            js = string.Format(js, url);
            HttpContext.Current.Response.Write(js);
        }
        #endregion

        #endregion
    }
}