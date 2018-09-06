using SmartEP.Core.Enums;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Utilities.Office;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class AuditReasonList : SmartEP.WebUI.Common.BasePage
    {

        AuditReasonService g_AuditReasonService = new AuditReasonService();
        //设备类型
        private ApplicationType applicationType = ApplicationType.Air;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        #region 绑定RadGrid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            string KeyWords = txt_KeyWords.Text.Trim();
            DataTable dt = g_AuditReasonService.GetAuditReasonData(applicationType, KeyWords);
            //数据总行数
            int recordTotal = dt.Rows.Count;
            //数据总行数
            gridAudit.VirtualItemCount = recordTotal;
            //绑定数据源
            gridAudit.DataSource = dt;
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAudit_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }
        #endregion

        #region 查找按钮事件
        /// <summary>
        /// 查找按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridAudit.Rebind();
        }
        #endregion

        #region ToolBar按钮事件
        /// <summary>
        /// ToolBar按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAudit_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            RadGrid radGrid = ((RadGrid)sender);
            int Index = -1;
            switch (e.CommandName)
            {
                case "DeleteSelected":
                    List<string> ReasonGuids = new List<string>();
                    foreach (String item in radGrid.SelectedIndexes)
                    {
                        Index = Convert.ToInt32(item);
                        string Uid = radGrid.MasterTableView.DataKeyValues[Index]["ReasonGuid"].ToString();
                        if (!ReasonGuids.Contains(Uid))
                        {
                            ReasonGuids.Add(Uid);
                        }
                    }
                    if (ReasonGuids.Count > 0)
                    {
                        if (Delete(ReasonGuids))
                        {
                            Alert("删除成功！");
                        }
                    }
                    break;
            }
        }
        #endregion

        #region 删除按钮调用方法
        /// <summary>
        /// 删除按钮调用方法
        /// </summary>
        /// <param name="ReasonGuids">审核理由主键数组</param>
        public bool Delete(List<string> ReasonGuids)
        {
            try
            {
                g_AuditReasonService.Delete(ReasonGuids);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion

        //protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        //{
        //    Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
        //    if (button.CommandName == "ExportToExcel")
        //    {
        //        string KeyWords = txt_KeyWords.Text.Trim();
        //        DataTable dt = g_AuditReasonService.GetAuditReasonData(applicationType, KeyWords);
        //        ExcelHelper.DataTableToExcel(dt, "审核理由", "审核理由", this.Page);
        //    }
        //}
    }
}