using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.QualityControl
{
    /// <summary>
    /// 名称：StandardAirValidateReport2.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：标准气体验证报告2
    /// 版权所有(C)：江苏远大信息股份有限公司
    public partial class StandardAirValidateReport2 : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }
        public void BindGrid()
        {
        }
        protected void gridBZQT_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("gasType", typeof(string));
            dt.Columns.Add("VerificationSite", typeof(string));
            dt.Columns.Add("dtBegin", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "苏州环境监测中心";
            dr[2] = "2015-9-28";

            dt.Rows.Add(dr);
            DataView dv = dt.DefaultView;
            gridBZQT.DataSource = dv;
        }

        protected void gridBZQT_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridQTXX_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Manufacturer", typeof(string));
            dt.Columns.Add("steelNum", typeof(string));
            dt.Columns.Add("steelPressure", typeof(string));
            dt.Columns.Add("steelConcentration", typeof(string));
            dt.Columns.Add("Medium", typeof(string));
            dt.Columns.Add("ValidityPeriod", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "2015-10-28";

            dt.Rows.Add(dr);
            DataView dv = dt.DefaultView;
            gridQTXX.DataSource = dv;
        }

        protected void gridQTXX_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridJZQXX_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("equipmentModel", typeof(string));
            dt.Columns.Add("equipmentNum", typeof(string));
            dt.Columns.Add("MFCcalibrationDate", typeof(string));
            dt.Columns.Add("O3calibrationDate", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dr[2] = "2015-9-28";
            dr[3] = "2015-9-28";

            dt.Rows.Add(dr);
            DataView dv = dt.DefaultView;
            gridJZQXX.DataSource = dv;
        }

        protected void gridJZQXX_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridLQYXX_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("equipmentModel", typeof(string));
            dt.Columns.Add("equipmentNum", typeof(string));
            dt.Columns.Add("ExitPressure", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dr[2] = "435";

            dt.Rows.Add(dr);
            DataView dv = dt.DefaultView;
            gridLQYXX.DataSource = dv;
        }

        protected void gridLQYXX_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridCKBZ_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Manufacturer", typeof(string));
            dt.Columns.Add("steelNum", typeof(string));
            dt.Columns.Add("steelPressure", typeof(string));
            dt.Columns.Add("steelConcentration", typeof(string));
            dt.Columns.Add("Medium", typeof(string));
            dt.Columns.Add("ValidityPeriod", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "2015-10-28";

            dt.Rows.Add(dr);
            DataView dv = dt.DefaultView;
            gridCKBZ.DataSource = dv;
        }

        protected void gridCKBZ_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridFXY_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("analyzerModel", typeof(string));
            dt.Columns.Add("equipmentNum", typeof(string));
            dt.Columns.Add("calibrationDate", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dr[2] = "2015-10-28";

            dt.Rows.Add(dr);
            DataView dv = dt.DefaultView;
            gridFXY.DataSource = dv;
        }

        protected void gridFXY_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        //protected void gridNextDay_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        //{
        //    gridNextDay.DataSource = new DataTable();
        //}

        //protected void gridNextDay_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        //{

        //}

        //protected void gridSameDay_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        //{
        //    gridSameDay.DataSource = new DataTable();
        //}

        //protected void gridSameDay_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        //{

        //}
    }
}