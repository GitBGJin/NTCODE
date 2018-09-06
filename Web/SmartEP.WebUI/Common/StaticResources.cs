using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Common
{
    public class StaticResources
    {
        #region Methods

        /// <summary>
        /// 绑定左侧菜单
        /// </summary>
        /// <param name="RPB"></param>
        public static void BindLeftMenu(RadPanelBar RPB)
        {
            System.Data.DataView dv = GetMenuDataSource();
            RPB.DataFieldParentID = "ParentID";
            RPB.DataFieldID = "ID";
            RPB.DataValueField = "ID";
            RPB.DataTextField = "Name";
            RPB.DataNavigateUrlField = "Url";
            RPB.DataSource = dv;
            RPB.DataBind();
            if (RPB.Items.Count > 0) RPB.Items[0].Expanded = true;
        }

        public static void BindTopMenu(RadMenu rm)
        {
            rm.DataFieldID = "ID";
            rm.DataFieldParentID = "ParentID";
            rm.DataTextField = "Name";
            rm.DataValueField = "ID";
            rm.DataNavigateUrlField = "Url";
            rm.DataSource = GetMenuDataSource();
            rm.DataBind();
        }

        public static DataView CreatDataView(string strSql, string connstr)
        {
            try
            {
                SqlConnection myConn = new SqlConnection(Common.Helper.GetConnectionString(connstr));
                SqlDataAdapter myCommand = new SqlDataAdapter(strSql, myConn);
                DataTable dt = new DataTable();
                myCommand.Fill(dt);
                return dt.DefaultView;
            }
            catch
            {
                return null;
            }
        }

        public static DataView GetMenuDataSource()
        {
            String RadMenuSql = "select vrm.[ID] , vrm.[ParentID], vrm.[Code],vrm.[ParentCode], vrm.[Name],vrm.[Url],isnull(fm.SmallIconAddress,'') as SmallIconAddress ,isnull(fm.bigIconAddress,'') as bigIconAddress,isnull(fm.middleIconAddress,'') as middleIconAddress  FROM [V_RadMenu_Air] vrm  join dbo.Frame_Module fm  on vrm.ModuleGuid=fm.ModuleGuid ";
            if (HttpContext.Current.Session["UserGuid"] != null || HttpContext.Current.Session["UserGuid"].ToString().Length > 0)
            {
                RadMenuSql += String.Format(" WHERE (UserGuid = '{0}')", HttpContext.Current.Session["UserGuid"]);
                RadMenuSql += "  and BelongToSys=4 ";
            }
            else
            {
                RadMenuSql += " WHERE (1 = 2)";
            }
            RadMenuSql += " ORDER BY vrm.[OrderNumber]";
            return CreatDataView(RadMenuSql, "CommonPlatFormConnection");
        }

        public static System.Collections.Generic.List<object> Themes()
        {
            return new List<object> {
                new {theme= "Office2007",cnTheme="Office2007主题"},
                new {theme= "Black",cnTheme="黑色主题"},
                new {theme= "Sunset", cnTheme="砖红色主题"},
                new {theme= "Windows7",cnTheme="Window7主题"},
                new {theme= "Hay",cnTheme="灰色主题"},
                new {theme= "Vista",cnTheme="Vista主题"},
                new {theme= "SinoydBlue",cnTheme="蓝色主题"},
                new {theme="SinoydPink",cnTheme="粉红色"},
                new {theme="SinoydDefault",cnTheme="灰色"}
               };
        }

        #endregion Methods
    }
}