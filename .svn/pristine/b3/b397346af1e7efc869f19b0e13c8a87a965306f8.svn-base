using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance
{
    public partial class YearMaintenance : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            DataTable year = new DataTable();

            year.Columns.Add("TestItems", typeof(string));
            year.Columns.Add("Testtime", typeof(DateTime));
            DataRow dryear = year.NewRow();
            dryear[0] = "清洗玻璃采样管。";
            dryear[1] = "2015-8-10";
            year.Rows.Add(dryear);

            DataRow dryear1 = year.NewRow();
            dryear1[0] = "清洁采样遮雨罩";
            dryear1[1] = "2015-9-10";
            year.Rows.Add(dryear1);
            DataRow dryear3 = year.NewRow();
            dryear3[0] = "清洗冷气机滤网";
            dryear3[1] = "2015-9-10";
            year.Rows.Add(dryear3);

            DataRow dryear4 = year.NewRow();
            dryear4[0] = "清洁温湿度检知器辐射罩";
            dryear4[1] = "2015-9-10";
            year.Rows.Add(dryear4);

            DataRow dryear5 = year.NewRow();
            dryear5[0] = "清洁辐射计玻璃镜面";
            dryear5[1] = "2015-9-10";
            year.Rows.Add(dryear5);

            DataRow dryear6 = year.NewRow();
            dryear6[0] = "清洗仪器散热风扇滤网。";
            dryear6[1] = "2015-9-10";
            year.Rows.Add(dryear6);
            DataRow dryear7 = year.NewRow();
            dryear7[0] = "更换辐射计干燥用分子筛";
            dryear7[1] = "2015-9-10";
            year.Rows.Add(dryear7);
            DataRow dryear2 = year.NewRow();
            dryear2[0] = "更换零值气体产生器用去除药剂。";
            dryear2[1] = "2015-8-10";
            year.Rows.Add(dryear2);
            DataView dvyear = year.DefaultView;
            gridYear.DataSource = dvyear;
        }
        protected void gridYear_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridYear_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}