using SmartEP.Utilities.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartEP.WebUI.Common
{
    public class Helper
    {
        #region Methods

        public static void ChangeTheme(string theme)
        {
            //HttpContext.Current.Session["SinoydTheme"] = theme;
            //CookieHelper.SetCookie("MyCssSkin", theme);
            SessionHelper.Add("MyCssSkin", theme);
        }

        public static string GetAppSetting(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            }
            catch
            {
                return null;
            }
        }

        public static string GetConnectionString(string name)
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            catch
            {
                return null;
            }
        }

        public static string GetTheme()
        {
            //if (HttpContext.Current.Session["SinoydTheme"] == null)
            //{
            //    System.Web.Configuration.PagesSection ps = (System.Web.Configuration.PagesSection)System.Configuration.ConfigurationManager.GetSection("system.web/pages");
            //    ChangeTheme(ps.Theme);
            //}
            //return (string)HttpContext.Current.Session["SinoydTheme"];
            string myCssCookie = CookieHelper.GetCookieValue("MyCssSkin");
            string myCssSession = SessionHelper.Get("MyCssSkin");
            if (string.IsNullOrEmpty(myCssCookie))
            {
                SessionHelper.Add("MyCssSkin", GetAppSetting("Telerik.Skin"));
            }
            else if (!string.IsNullOrEmpty(myCssCookie) && !myCssCookie.Equals(myCssSession))
            {
                SessionHelper.Add("MyCssSkin", myCssCookie);
            }
            return SessionHelper.Get("MyCssSkin");
        }

        /// <summary>
        /// 跳转到某个页面
        /// </summary>
        /// <param name="page"></param>
        /// <param name="url"></param>
        public static void GoTo(System.Web.UI.Page page, string url)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(page, typeof(BasePage), "script", string.Format("window.location.href='{0}';", url), true);
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        /// <param name="page"></param>
        public static void Refresh(System.Web.UI.Page page)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(page, typeof(BasePage), "script", "window.location.href=window.location.href;", true);
        }

        public static void SetPageTheme(System.Web.UI.Page page, string theme)
        {
            page.Theme = theme;
            ChangeTheme(theme);
        }

        public static void SetTelerikTheme(System.Web.UI.Page page, string theme)
        {
            Telerik.Web.UI.RadSkinManager rsm = Telerik.Web.UI.RadSkinManager.GetCurrent(page);
            if (rsm == null)
            {
                rsm = new Telerik.Web.UI.RadSkinManager()
                {
                    ClientIDMode = System.Web.UI.ClientIDMode.AutoID,
                    ID = "RadSkinManager1"
                };
                rsm.Skins.Add(new Telerik.Web.UI.SkinReference() { Path = string.Format(@"{0}/Skins", System.Web.HttpRuntime.AppDomainAppVirtualPath).Replace("//", "/") });
                page.Form.Controls.Add(rsm);
            }
            rsm.Skin = theme;
            ChangeTheme(theme);
        }

        #endregion Methods
    }
}