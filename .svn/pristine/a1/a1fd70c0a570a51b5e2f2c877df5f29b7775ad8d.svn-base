using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.RemoteControl
{
    public partial class ControlSearch : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {

            startTime.SelectedDate = DateTime.Now.AddDays(-1);
            endTime.SelectedDate = DateTime.Now;
        }
        #endregion
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            DateTime dtBegion = startTime.SelectedDate.Value;
            DateTime dtEnd = endTime.SelectedDate.Value;
            DataTable myView = new DataTable();
            myView.Columns.Add("id", typeof(int));
            myView.Columns.Add("operaterTime", typeof(DateTime));
            myView.Columns.Add("monitoringPointName", typeof(string));
            myView.Columns.Add("cmdDesc", typeof(string));
            myView.Columns.Add("execState", typeof(string));
            myView.Columns.Add("feedback", typeof(string));
            RadGrid1.DataSource = myView;
        }
        #endregion

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {

        }
    }
}