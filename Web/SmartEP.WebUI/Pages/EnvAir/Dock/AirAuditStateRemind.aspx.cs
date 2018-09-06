using SmartEP.DomainModel;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using SmartEP.Service.Frame;
using SmartEP.WebUI.Common;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Dock
{
    public partial class AirAuditStateRemind : BasePage
    {
        /// <summary>
        /// 审核数据接口
        /// </summary>
        AuditDataService m_AuditDataService = new AuditDataService();
        /// <summary>
        /// 系统配置数据接口
        /// </summary>
        DictionaryService dicService = new DictionaryService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            dayBegin.SelectedDate = DateTime.Now.AddDays(-1);
            dayEnd.SelectedDate = DateTime.Now.AddDays(-1);
            dayEnd.MaxDate = DateTime.Now.AddDays(-1);
            //站点类型
            IQueryable<V_CodeMainItemEntity> siteTypeEntites = dicService.RetrieveList(DictionaryType.Air, "空气站点类型");
            ports.DataSource = siteTypeEntites;
            ports.DataTextField = "ItemText";
            ports.DataValueField = "ItemGuid";
            ports.DataBind();
        }

        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            DateTime dtBeginDay = dayBegin.SelectedDate == null ? Convert.ToDateTime("1900-01-01") : dayBegin.SelectedDate.Value;
            DateTime dtEndDay = dayEnd.SelectedDate == null ? Convert.ToDateTime("1900-01-01") : dayEnd.SelectedDate.Value;
            if (dtBeginDay == Convert.ToDateTime("1900-01-01") || dtEndDay == Convert.ToDateTime("1900-01-01") || dtBeginDay > dtEndDay)
            {
                Alert("请选择正确的查询日期");
                return;
            }
            string point = ports.SelectedValue;
            DataTable dt = m_AuditDataService.AuditFlagStatisticsByPoint(Core.Enums.ApplicationValue.Air, point, dtBeginDay, dtEndDay);
            DataTable newdt = new DataTable();
            newdt = dt.Clone(); // 克隆dt 的结构，包括所有 dt 架构和约束,并无数据； 
            foreach (DataRow dr in dt.Rows)
            {
                //if (Convert.ToString(dr["AuditStatus"]) != "已审核")
                //{
                //    newdt.Rows.Add(dr.ItemArray);
                //}
                newdt.Rows.Add(dr.ItemArray);
            }
            gridRemind.DataSource = newdt;
        }

        /// <summary>
        /// Grid数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRemind_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();

        }

        /// <summary>
        /// Grid绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRemind_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                GridTableCell cell = (GridTableCell)item["AuditStatus"];
                DataRowView drv = e.Item.DataItem as DataRowView;
                //if (cell.Text == "未审核" || cell.Text == "已审核")
                //{
                if (cell.Text == "未审核")
                {
                    cell.ForeColor = System.Drawing.Color.Red;
                }
                if (cell.Text == "已审核")
                {
                    cell.ForeColor = System.Drawing.Color.Green;
                }
                if (cell.Text == "部分审核")
                {
                    cell.ForeColor = System.Drawing.Color.BlueViolet;
                }
                GridTableCell pointCell = (GridTableCell)item["MonitoringPointName"];
                pointCell.Text = string.Format("<a href='#' onclick='RowClick(\"{0}\",\"{1}\")'>{2}</a>", drv["PointId"], drv["Date"], drv["MonitoringPointName"]);
                //}
            }
        }

        /// <summary>
        /// 查找按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridRemind.Rebind();
        }
    }
}