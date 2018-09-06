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
    /// 名称：ZeroPointAndSpanCheck.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-09-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：气体分析仪零点/跨度检查与调节、精密度检查记录表
    /// 版权所有(C)：江苏远大信息股份有限公司
    public partial class ZeroPointAndSpanCheck : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {

        }

        protected void gridYQXX_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("instrumentType", typeof(string));
            dt.Columns.Add("instrumentModel", typeof(string));
            dt.Columns.Add("instrumentNum", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "SO<sub>2</sub>";
            dr[1] = "GB 8970－88";
            dr[2] = "T1001";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "O<sub>3</sub>";
            dr1[1] = "GB/T 15438－95";
            dr1[2] = "T1001";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "CO";
            dr2[1] = "GB 9801－88";
            dr2[2] = "T1001";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3[0] = "NO<sub>X</sub>";
            dr3[1] = "GB/T 15436－95";
            dr3[2] = "T2001";
            dt.Rows.Add(dr3);
            DataView dv = dt.DefaultView;
            gridYQXX.DataSource = dv;
        }

        protected void gridYQXX_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridGPQXX_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("instrumentType", typeof(string));
            dt.Columns.Add("steelNum", typeof(string));
            dt.Columns.Add("validityPeriod", typeof(string));
            dt.Columns.Add("steelPressure", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "SO<sub>2</sub>";
            dr[1] = "GP 10041-09";
            dr[2] = "6个月";
            dr[3] = "0.9mPa";
            dt.Rows.Add(dr);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "CO";
            dr2[1] = "GP 10042-11";
            dr2[2] = "6个月";
            dr2[3] = "0.8mPa";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3[0] = "NO";
            dr3[1] = "GP 10022-01";
            dr3[2] = "6个月";
            dr3[3] = "1.0mPa";
            dt.Rows.Add(dr3);
            DataView dv = dt.DefaultView;
            gridGPQXX.DataSource = dv;
        }

        protected void gridGPQXX_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridJZQXX_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("calibratorModel", typeof(string));
            dt.Columns.Add("equipmentNum", typeof(string));
            dt.Columns.Add("MFCDate", typeof(string));
            dt.Columns.Add("O3Date", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "HX-700S";
            dr[1] = "HX-700S-1001";
            dr[2] = "2015-09-28";
            dr[3] = "2015-09-28";
            dt.Rows.Add(dr);
            DataView dv = dt.DefaultView;
            gridJZQXX.DataSource = dv;
        }

        protected void gridJZQXX_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridWRWTJ_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("pollutant", typeof(string));
            dt.Columns.Add("zeroGas", typeof(string));
            dt.Columns.Add("zeroBeforeAdjustment", typeof(string));
            dt.Columns.Add("zeroAfterAdjustment", typeof(string));
            dt.Columns.Add("spanGas", typeof(string));
            dt.Columns.Add("spanBeforeAdjustment", typeof(string));
            dt.Columns.Add("spanAfterAdjustment", typeof(string));
            dt.Columns.Add("interceptBeforeAdjustment", typeof(string));
            dt.Columns.Add("interceptAfterAdjustment", typeof(string));
            dt.Columns.Add("slopeBeforeAdjustment", typeof(string));
            dt.Columns.Add("slopeAfterAdjustment", typeof(string));
            dt.Columns.Add("gainBeforeAdjustment", typeof(string));
            dt.Columns.Add("gainfterAdjustment", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "SO<sub>2</sub>";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "CO";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "O<sub>3</sub>";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3[0] = "NO";
            dt.Rows.Add(dr3);
            DataRow dr4 = dt.NewRow();
            dr4[0] = "NO<sub>X</sub>";
            dt.Rows.Add(dr4);
            DataView dv = dt.DefaultView;
            gridWRWTJ.DataSource = dv;
        }

        protected void gridWRWTJ_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridJMDJC_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("pollutant", typeof(string));
            dt.Columns.Add("gasconcentration", typeof(string));
            dt.Columns.Add("instrumentresponse", typeof(string));
            dt.Columns.Add("percenterror", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "SO<sub>2</sub>";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "CO";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "O<sub>3</sub>";
            dt.Rows.Add(dr2);
            DataView dv = dt.DefaultView;
            //gridJMDJC.DataSource = dv;
        }

        protected void gridJMDJC_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridJBXX_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("point", typeof(string));
            dt.Columns.Add("indoorTemp", typeof(string));
            dt.Columns.Add("operator", typeof(string));
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("dtBegin", typeof(string));
            dt.Columns.Add("dtEnd", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "南门";
            dr[1] = "28℃";
            dr[2] = "张三";
            dr[3] = "2015-9-28";
            dr[4] = "9:10:00";
            dr[5] = "10:00:00";

            dt.Rows.Add(dr);
            DataView dv = dt.DefaultView;
            gridJBXX.DataSource = dv;
        }

        protected void gridJBXX_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }
    }
}