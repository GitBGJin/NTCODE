using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.QualityControl
{
    public partial class NOXMovingInstrumentCalibrate : SmartEP.WebUI.Common.BasePage
    {
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
           
        }
        public void BindGrid()
        {
            var dataView = new DataView();
            if (dataView == null)
            {
                dataView = new DataView();
            }
            grdFlowCorrect.DataSource = new DataTable();
        }
        protected void grdFlowCorrect_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void grdFlowCorrect_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {

        }

        public void Bind()
        {
            var dataView = new DataView();
            if (dataView == null)
            {
                dataView = new DataView();
            }
            grdCylinderGasInfor.DataSource = new DataTable();
        }
        protected void grdCylinderGasInfor_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Bind();
        }

        protected void grdCylinderGasInfor_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }
        public void Bind1()
        {
            var dataView = new DataView();
            if (dataView == null)
            {
                dataView = new DataView();
            }
            grdNOrNOxAnalyze.DataSource = new DataTable();
        }
        protected void grdNOrNOxAnalyze_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Bind1();
        }

        protected void grdNOrNOxAnalyze_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }
    }
}