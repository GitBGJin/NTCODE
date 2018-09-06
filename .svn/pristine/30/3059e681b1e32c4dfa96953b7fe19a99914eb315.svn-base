using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance
{
    public partial class SeasonMaintenance : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            DataTable season = new DataTable();

            season.Columns.Add("TestItems", typeof(string));
            season.Columns.Add("Testtime", typeof(DateTime));
            DataRow drseason = season.NewRow();
            drseason[0] = "清洗玻璃采样管。(月维护时到站立即实施)";
            drseason[1] = "2015-8-10";
            season.Rows.Add(drseason);

            DataRow drseason1 = season.NewRow();
            drseason1[0] = "清洁采样遮雨罩";
            drseason1[1] = "2015-9-10";
            season.Rows.Add(drseason1);
            DataRow drseason3 = season.NewRow();
            drseason3[0] = "清洗冷气机滤网";
            drseason3[1] = "2015-9-10";
            season.Rows.Add(drseason3);

            DataRow drseason4 = season.NewRow();
            drseason4[0] = "清洁温湿度检知器辐射罩";
            drseason4[1] = "2015-9-10";
            season.Rows.Add(drseason4);

            DataRow drseason5 = season.NewRow();
            drseason5[0] = "清洁辐射计玻璃镜面";
            drseason5[1] = "2015-9-10";
            season.Rows.Add(drseason5);

            DataRow drseason6 = season.NewRow();
            drseason6[0] = "清洗仪器散热风扇滤网。";
            drseason6[1] = "2015-9-10";
            season.Rows.Add(drseason6);
            DataRow drseason7 = season.NewRow();
            drseason7[0] = "更换辐射计干燥用分子筛";
            drseason7[1] = "2015-9-10";
            season.Rows.Add(drseason7);
            DataRow drseason2 = season.NewRow();
            drseason2[0] = "更换零值气体产生器用去除药剂。";
            drseason2[1] = "2015-8-10";
            season.Rows.Add(drseason2);
            DataView dvseason = season.DefaultView;
            gridSeason.DataSource = dvseason;
        }
        protected void gridSeason_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridSeason_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}