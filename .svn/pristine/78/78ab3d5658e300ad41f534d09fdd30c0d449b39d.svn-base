using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Web.UI;

namespace SmartEP.Service.Frame
{
    public class FrameModuleService
    {
        BaseDAHelper g_DBBiz = new BaseDAHelper();
        string g_FrameCon = "Frame_Connection";

        #region << 左侧菜单处理 >>

        /// <summary>
        /// 左侧菜单初期化
        /// </summary>
        /// <param name="leftPanelBar">菜单控件</param>
        /// <param name="userGuid">用户GUID</param>
        /// <param name="parentModuleGuid">系统类型</param>
        /// <param name="token">框架token</param>
        public void InitLeftPanelBar(RadPanelBar leftPanelBar, string userGuid, string parentModuleGuid, string token)
        {
            DataView dv = GetFrameModuleByParent(userGuid, parentModuleGuid);
            dv.RowFilter = "ParentModuleGuid is NULL ";
            if (dv.Count > 0)
            {
                foreach (DataRowView drv in dv)
                {
                    RadPanelItem itemChild = new RadPanelItem();
                    string moduleGuid = drv["ModuleGuid"] != DBNull.Value ? drv["ModuleGuid"].ToString() : string.Empty;
                    string url = GetModuleUrl(drv, token);
                    itemChild.Value = string.IsNullOrEmpty(url) ? url : url + ";" + moduleGuid;
                    itemChild.Text = drv["ModuleName"] != DBNull.Value ? drv["ModuleName"].ToString() : string.Empty;
                    string imgUrl = drv["IconUrl"] != DBNull.Value ? drv["IconUrl"].ToString() : string.Empty;
                    imgUrl = "../Resources/Images/leftPanelBar/icon/" + imgUrl;
                    itemChild.ImageUrl = imgUrl;
                    //string moduleGuid = drv["ModuleGuid"] != DBNull.Value ? drv["ModuleGuid"].ToString() : string.Empty;

                    leftPanelBar.Items.Add(itemChild);
                    DataTable dvClone = dv.Table.Clone();
                    dvClone = dv.Table.Copy();
                    AddRadPanelBarItems(itemChild, moduleGuid, dvClone.DefaultView, token);
                }
            }
        }

        /// <summary>
        /// 取得指定菜单父节点的所有子节点
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="parentModuleGuid"></param>
        /// <returns></returns>
        private DataView GetFrameModuleByParent(string userGuid, string parentModuleGuid)
        {
            g_DBBiz.ClearParameters();
            SqlParameter pram1 = new SqlParameter();
            pram1 = new SqlParameter();
            pram1.SqlDbType = SqlDbType.NVarChar;
            pram1.ParameterName = "@UserGuid";
            pram1.Value = userGuid;
            g_DBBiz.SetProcedureParameters(pram1);

            SqlParameter pram2 = new SqlParameter();
            pram2 = new SqlParameter();
            pram2.SqlDbType = SqlDbType.NVarChar;
            pram2.ParameterName = "@ParentModuleGuid";
            pram2.Value = parentModuleGuid;
            g_DBBiz.SetProcedureParameters(pram2);
            return g_DBBiz.ExecuteProc("UP_GetUserModuleByParent", g_FrameCon);
        }

        /// <summary>
        /// 遍历插入菜单
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ParentModuleGuid"></param>
        /// <param name="DV"></param>
        private void AddRadPanelBarItems(RadPanelItem item, string ParentModuleGuid, DataView DV, string token)
        {
            DV.RowFilter = "ParentModuleGuid = '" + ParentModuleGuid + "'";
            if (DV.Count > 0)
            {
                //item.ChildGroupHeight = System.Web.UI.WebControls.Unit.Pixel(DV.Count * 40);
                //item.he
                foreach (DataRowView drv in DV)
                {
                    RadPanelItem itemChild = new RadPanelItem();
                    string moduleGuid = drv["ModuleGuid"] != DBNull.Value ? drv["ModuleGuid"].ToString() : string.Empty;
                    string url = GetModuleUrl(drv, token);
                    itemChild.Value = string.IsNullOrEmpty(url) ? url : url + ";" + moduleGuid;
                    //itemChild.Value = GetModuleUrl(drv, token);
                    itemChild.Text = drv["ModuleName"] != DBNull.Value ? drv["ModuleName"].ToString() : string.Empty;
                    string imgUrl = drv["IconUrl"] != DBNull.Value ? drv["IconUrl"].ToString() : string.Empty;
                    imgUrl = "../Resources/Images/leftPanelBar/icon/" + imgUrl;
                    itemChild.ImageUrl = imgUrl;
                    //itemChild.ImageUrl = "none";
                    item.Items.Add(itemChild);
                    DataTable dvClone = DV.Table.Clone();
                    dvClone = DV.Table.Copy();
                    AddRadPanelBarItems(itemChild, moduleGuid, dvClone.DefaultView, token);
                }
            }
        }

        /// <summary>
        /// 取得菜单地址
        /// </summary>
        /// <param name="drv"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private string GetModuleUrl(DataRowView drv, string token)
        {
            string moduleUrl = string.Empty;
            moduleUrl = drv["ModuleUrl"] != DBNull.Value ? drv["ModuleUrl"].ToString() : string.Empty;
            string siteUrlLan = drv["SiteUrlLan"] != DBNull.Value ? drv["SiteUrlLan"].ToString() : string.Empty;
            string siteGuid = drv["SiteGuid"] != DBNull.Value ? drv["SiteGuid"].ToString() : string.Empty;
            //是否是框架站点
            if (!siteGuid.Equals("682262A4-491F-44A6-B306-F6D0383F6A78"))
            {
                if (!string.IsNullOrEmpty(moduleUrl))
                {
                    if (moduleUrl.IndexOf('?') >= 0)
                    {
                        moduleUrl = moduleUrl + "&Token=" + token;
                    }
                    else
                    {
                        moduleUrl = moduleUrl + "?Token=" + token;
                    }
                }
                if (!string.IsNullOrEmpty(siteUrlLan) && !string.IsNullOrEmpty(moduleUrl))
                {
                    moduleUrl = "../" + siteUrlLan + moduleUrl;
                }
            }

            return moduleUrl;
        }
        #endregion
    }
}
