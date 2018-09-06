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
    /// 名称：WeekMaintenanceSearch.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-09-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：周巡检维护
    /// 版权所有(C)：江苏远大信息股份有限公司
    public partial class WeekMaintenanceSearch : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {

            DataTable week = new DataTable();

            week.Columns.Add("TestItems", typeof(string));
            week.Columns.Add("Testtime", typeof(DateTime));
            DataRow drweek = week.NewRow();
            drweek[0] = "测站周围环境是否出现影响子站设备安全和监测采样品质的重大变迁？";
            drweek[1] = "2015-8-10";
            week.Rows.Add(drweek);

            DataRow drweek1 = week.NewRow();
            drweek1[0] = "避雷及设备系统接地是否正常（外观、线路接点、接地箱等）？";
            drweek1[1] = "2015-9-10";
            week.Rows.Add(drweek1);
            DataRow drweek3 = week.NewRow();
            drweek3[0] = "电力系统及站外配线是否正常？";
            drweek3[1] = "2015-9-10";
            week.Rows.Add(drweek3);

            DataRow drweek4 = week.NewRow();
            drweek4[0] = "站房门禁系统是否正常开启关闭？";
            drweek4[1] = "2015-9-10";
            week.Rows.Add(drweek4);

            DataRow drweek5 = week.NewRow();
            drweek5[0] = "站内照明、数采及显示器键盘和鼠标是否正常？";
            drweek5[1] = "2015-9-10";
            week.Rows.Add(drweek5);

            DataRow drweek6 = week.NewRow();
            drweek6[0] = "站房内外是否整齐清洁？";
            drweek6[1] = "2015-9-10";
            week.Rows.Add(drweek6);
            DataRow drweek7 = week.NewRow();
            drweek7[0] = "消防设备外观是否正常？";
            drweek7[1] = "2015-9-10";
            week.Rows.Add(drweek7);
            DataRow drweek2 = week.NewRow();
            drweek2[0] = "数采设备时间是否正确？（北京时间±5分钟）";
            drweek2[1] = "2015-8-10";
            week.Rows.Add(drweek2);
            DataView dvweek = week.DefaultView;
            gridWeekDevice.DataSource = dvweek;
            DataTable dt = new DataTable();

            dt.Columns.Add("TestItems", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "减压阀";
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1[0] = "温度计显示读数";
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2[0] = "站内电源电压读数";
            dt.Rows.Add(dr2);
            DataView dv = dt.DefaultView;
            gridDevice.DataSource = dv;


            DataTable dtMaintenance = new DataTable();

            dtMaintenance.Columns.Add("TestItems", typeof(string));
            dtMaintenance.Columns.Add("TestDw", typeof(string));

            DataRow drMaintenance = dtMaintenance.NewRow();
            drMaintenance[0] = "臭氧发生器参考电压是否正常？(80～600mv)";
            drMaintenance[1] = "mv";
            dtMaintenance.Rows.Add(drMaintenance);

            DataRow drMaintenance1 = dtMaintenance.NewRow();
            drMaintenance1[0] = "臭氧流量是否正常？（0.100～0.200LPM）";
            drMaintenance1[1] = "LPM";
            dtMaintenance.Rows.Add(drMaintenance1);

            DataRow drMaintenance2 = dtMaintenance.NewRow();
            drMaintenance2[0] = "臭氧驱动电压是否正常？（800mv）";
            drMaintenance2[1] = "mv";
            dtMaintenance.Rows.Add(drMaintenance2);
            DataView dvMaintenance = dtMaintenance.DefaultView;

            gridWeekFile.DataSource = dvweek;
            gridInstrument.DataSource = dvweek;
            gridMaintenance.DataSource = dvMaintenance;
        }

        protected void gridWeekDevice_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            BindGrid();
        }

        protected void gridWeekDevice_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridDevice_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridDevice_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            BindGrid();
        }

        protected void gridWeekFile_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridWeekFile_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridInstrument_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridInstrument_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridMaintenance_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridMaintenance_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}