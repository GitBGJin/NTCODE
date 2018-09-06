using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance
{
    /// <summary>
    /// 名称：MonthMaintenance.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-09-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：月巡检维护
    /// 版权所有(C)：江苏远大信息股份有限公司
    public partial class MonthMaintenance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            DataTable month = new DataTable();

            month.Columns.Add("TestItems", typeof(string));
            month.Columns.Add("Testtime", typeof(DateTime));
            DataRow drmonth = month.NewRow();
            drmonth[0] = "清洗玻璃采样管。(月维护时到站立即实施)";
            drmonth[1] = "2015-8-10";
            month.Rows.Add(drmonth);

            DataRow drmonth1 = month.NewRow();
            drmonth1[0] = "清洁采样遮雨罩";
            drmonth1[1] = "2015-9-10";
            month.Rows.Add(drmonth1);
            DataRow drmonth3 = month.NewRow();
            drmonth3[0] = "清洗冷气机滤网";
            drmonth3[1] = "2015-9-10";
            month.Rows.Add(drmonth3);

            DataRow drmonth4 = month.NewRow();
            drmonth4[0] = "清洁温湿度检知器辐射罩";
            drmonth4[1] = "2015-9-10";
            month.Rows.Add(drmonth4);

            DataRow drmonth5 = month.NewRow();
            drmonth5[0] = "清洁辐射计玻璃镜面";
            drmonth5[1] = "2015-9-10";
            month.Rows.Add(drmonth5);

            DataRow drmonth6 = month.NewRow();
            drmonth6[0] = "清洗仪器散热风扇滤网。";
            drmonth6[1] = "2015-9-10";
            month.Rows.Add(drmonth6);
            DataRow drmonth7 = month.NewRow();
            drmonth7[0] = "更换辐射计干燥用分子筛";
            drmonth7[1] = "2015-9-10";
            month.Rows.Add(drmonth7);
            DataRow drmonth2 = month.NewRow();
            drmonth2[0] = "更换零值气体产生器用去除药剂。";
            drmonth2[1] = "2015-8-10";
            month.Rows.Add(drmonth2);
            DataView dvmonth = month.DefaultView;
            gridMonth.DataSource = dvmonth;
        }

        protected void gridMonth_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridMonth_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}