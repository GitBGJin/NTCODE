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
    /// 名称：MovingInstrumentCalibrate.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-09-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：动态校准仪校准记录表
    /// 版权所有(C)：江苏远大信息股份有限公司
    public partial class MovingInstrumentCalibrate : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            labJZQEval.InnerText = "合格！";
            labXSQEval.InnerText = "合格！";
        }
        protected void gridJBXX_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("operator", typeof(string));
            dt.Columns.Add("dtBegin", typeof(string));
            dt.Columns.Add("dtEnd", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "2015-9-28";
            dr[1] = "张三";
            dr[2] = "9:10:00";
            dr[3] = "10:00:00";

            dt.Rows.Add(dr);
            DataView dv = dt.DefaultView;
            gridJBXX.DataSource = dv;
        }

        protected void gridJBXX_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridLLJXX_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("range", typeof(string));
            dt.Columns.Add("flowmeterModel", typeof(string));
            dt.Columns.Add("flowmeterNum", typeof(string));
            dt.Columns.Add("testDate", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "0-500cc/min";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "2015-9-28";

            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "0-10L/min";
            dr1[1] = "";
            dr1[2] = "";
            dr1[3] = "2015-9-28";

            dt.Rows.Add(dr1);
            DataView dv = dt.DefaultView;
            gridLLJXX.DataSource = dv;
        }

        protected void gridLLJXX_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridHJCSXX_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("thermometerType", typeof(string));
            dt.Columns.Add("TequipmentNum ", typeof(string));
            dt.Columns.Add("TtestDate", typeof(string));
            dt.Columns.Add("barometerType", typeof(string));
            dt.Columns.Add("BequipmentNum ", typeof(string));
            dt.Columns.Add("BtestDate", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dr[2] = "2015-9-28";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "2015-9-28";

            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "";
            dr1[1] = "";
            dr1[2] = "2015-9-27";
            dr1[3] = "";
            dr1[4] = "";
            dr1[5] = "2015-9-27";

            dt.Rows.Add(dr1);
            DataView dv = dt.DefaultView;
            gridHJCSXX.DataSource = dv;
        }

        protected void gridHJCSXX_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridHJCSCEJG_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("Temperature ", typeof(string));
            dt.Columns.Add("airpressure", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "2015-9-28 07:30:00";
            dr[1] = "20";
            dr[2] = "1012";

            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "2015-9-28 10:30:00";
            dr1[1] = "25";
            dr1[2] = "1014";

            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "2015-9-28 12:30:00";
            dr2[1] = "28";
            dr2[2] = "1020";

            dt.Rows.Add(dr2);
            DataView dv = dt.DefaultView;
            gridHJCSCEJG.DataSource = dv;
        }

        protected void gridHJCSCEJG_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridJZQLL_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("setValues", typeof(string));
            dt.Columns.Add("instrumentReading ", typeof(string));
            dt.Columns.Add("flowmeterReading", typeof(string));
            dt.Columns.Add("correctionReading ", typeof(string));
            dt.Columns.Add("calibratorValues", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "250";
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1[0] = "500";
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2[0] = "750";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3[0] = "1000";
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4[0] = "1250";
            dt.Rows.Add(dr4);

            DataRow dr5 = dt.NewRow();
            dr5[0] = "1500";
            dt.Rows.Add(dr5);

            DataView dv = dt.DefaultView;
            gridJZQLL.DataSource = dv;
        }

        protected void gridJZQLL_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridXSQLL_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("setValues", typeof(string));
            dt.Columns.Add("instrumentReading ", typeof(string));
            dt.Columns.Add("flowmeterReading", typeof(string));
            dt.Columns.Add("correctionReading ", typeof(string));
            dt.Columns.Add("calibratorValues", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "250";
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1[0] = "500";
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2[0] = "750";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3[0] = "1000";
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4[0] = "1250";
            dt.Rows.Add(dr4);

            DataRow dr5 = dt.NewRow();
            dr5[0] = "1500";
            dt.Rows.Add(dr5);

            DataView dv = dt.DefaultView;
            gridXSQLL.DataSource = dv;
        }

        protected void gridXSQLL_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }
    }
}