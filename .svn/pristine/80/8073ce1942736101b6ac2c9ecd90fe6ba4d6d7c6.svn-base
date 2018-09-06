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
    /// 名称：StandardAirValidateReport1.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：标准气体验证报告1
    /// 版权所有(C)：江苏远大信息股份有限公司
    public partial class StandardAirValidateReport1 : SmartEP.WebUI.Common.BasePage
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
        protected void gridJBXX_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("reportTime", typeof(string));
            dt.Columns.Add("verificationTime", typeof(string));
            dt.Columns.Add("operator", typeof(string));
            dt.Columns.Add("effectiveTime", typeof(string));
            dt.Columns.Add("judge", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "2015-8-28";
            dr[1] = "2015-9-28";
            dr[2] = "张三";
            dr[3] = "2015-10-28";
            dr[4] = "1";

            dt.Rows.Add(dr);
            foreach (DataRow drNew in dt.Rows)
                if (drNew["judge"].ToString() == "1")
                {
                    drNew["judge"] = "是";
                }
                else
                {
                    drNew["judge"] = "否";
                }
            DataView dv = dt.DefaultView;
            gridJBXX.DataSource = dv;
        }

        protected void gridJBXX_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridYSQT_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("parameter", typeof(string));
            dt.Columns.Add("beforeConcentration", typeof(string));
            dt.Columns.Add("impure", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "CO";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "SO<sub>2</sub>";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "NO/NO<sub>X</sub>";
            dt.Rows.Add(dr2);
            foreach (DataRow drNew in dt.Rows)
            {
                if (drNew["parameter"].ToString() == "NO/NO<sub>X</sub>")
                {

                }
                else
                {
                    drNew["impure"] = "--";
                }
            }
            DataView dv = dt.DefaultView;
            gridYSQT.DataSource = dv;
        }

        protected void gridYSQT_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridPHQT_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("equilibriumGas", typeof(string));
            dt.Columns.Add("steelNum", typeof(string));
            dt.Columns.Add("steelPressure", typeof(string));
            dt.Columns.Add("Department", typeof(string));
            dt.Columns.Add("place", typeof(string));
            dt.Columns.Add("floor", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "氮气";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "苏州环境监测中心";
            dr[5] = "四楼";

            dt.Rows.Add(dr);
            DataView dv = dt.DefaultView;
            gridPHQT.DataSource = dv;
        }

        protected void gridPHQT_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }


        protected void gridCKBZ_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("parameter", typeof(string));
            dt.Columns.Add("referenceNum", typeof(string));
            dt.Columns.Add("steelNum", typeof(string));
            dt.Columns.Add("steelPressure", typeof(string));
            dt.Columns.Add("identification", typeof(string));
            dt.Columns.Add("effectiveDate", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "CO";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "2015-9-28";

            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "SO<sub>2</sub>";
            dr1[1] = "";
            dr1[2] = "";
            dr1[3] = "";
            dr1[4] = "";
            dr1[5] = "2015-9-28";

            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "NO/NO<sub>X</sub>";
            dr2[1] = "";
            dr2[2] = "";
            dr2[3] = "";
            dr2[4] = "";
            dr2[5] = "2015-9-28";

            dt.Rows.Add(dr2);
            DataView dv = dt.DefaultView;
            gridCKBZ.DataSource = dv;
        }

        protected void gridCKBZ_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridFXY_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("parameter", typeof(string));
            dt.Columns.Add("Manufacturer", typeof(string));
            dt.Columns.Add("Model", typeof(string));
            dt.Columns.Add("sequenceNum", typeof(string));
            dt.Columns.Add("analyzerNum", typeof(string));
            dt.Columns.Add("calibrationDate", typeof(string));
            dt.Columns.Add("calibrationReading", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "CO";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "2015-9-28";
            dr[6] = "";

            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "SO<sub>2</sub>";
            dr1[1] = "";
            dr1[2] = "";
            dr1[3] = "";
            dr1[4] = "";
            dr1[5] = "2015-9-28";
            dr1[6] = "";

            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "NO/NO<sub>X</sub>";
            dr2[1] = "";
            dr2[2] = "";
            dr2[3] = "";
            dr2[4] = "";
            dr2[5] = "2015-9-28";
            dr2[6] = "";

            dt.Rows.Add(dr2);
            DataView dv = dt.DefaultView;
            gridFXY.DataSource = dv;
        }

        protected void gridFXY_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridJZQ_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Manufacturer", typeof(string));
            dt.Columns.Add("Model", typeof(string));
            dt.Columns.Add("sequenceNum", typeof(string));
            dt.Columns.Add("analyzerNum", typeof(string));
            dt.Columns.Add("MFCcalibrationDate", typeof(string));
            dt.Columns.Add("MFCValidityPeriod", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "2015-9-28";
            dr[5] = "2015-10-28";

            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "";
            dr1[1] = "";
            dr1[2] = "";
            dr1[3] = "";
            dr1[4] = "2015-9-28";
            dr1[5] = "2015-10-28";

            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "";
            dr2[1] = "";
            dr2[2] = "";
            dr2[3] = "";
            dr2[4] = "2015-9-28";
            dr2[5] = "2015-10-28";

            dt.Rows.Add(dr2);
            DataView dv = dt.DefaultView;
            gridJZQ.DataSource = dv;
        }

        protected void gridJZQ_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridJDJG_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("parameter", typeof(string));
            dt.Columns.Add("beforeConcentration", typeof(string));
            dt.Columns.Add("impure", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "CO";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "SO<sub>2</sub>";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "NO/NO<sub>X</sub>";
            dt.Rows.Add(dr2);
            foreach (DataRow drNew in dt.Rows)
            {
                if (drNew["parameter"].ToString() == "NO/NO<sub>X</sub>")
                {

                }
                else
                {
                    drNew["impure"] = "--";
                }
            }
            DataView dv = dt.DefaultView;
            gridJDJG.DataSource = dv;
        }

        protected void gridJDJG_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }
    }
}