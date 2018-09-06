using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance
{
    /// <summary>
    /// 名称：AddInstrumentMaintenance.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-10-6
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：空气质量自动监测子站巡检维护记录
    /// 版权所有(C)：江苏远大信息股份有限公司
    public partial class SubStationInspection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gridInEnvironment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            
        }

        protected void gridInEnvironment_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridSampling_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

        }

        protected void gridSampling_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridDataCollection_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

        }

        protected void gridDataCollection_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridInstrumentInfo_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Parameter", typeof(string));
            dt.Columns.Add("SO2", typeof(string));
            dt.Columns.Add("NOX", typeof(string));
            dt.Columns.Add("CO", typeof(string));
            dt.Columns.Add("O3", typeof(string));
            dt.Columns.Add("PM10Main", typeof(string));
            dt.Columns.Add("PM10Auxiliary", typeof(string));
            dt.Columns.Add("PM2.5Main", typeof(string));
            dt.Columns.Add("PM2.5Auxiliary", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "采样流量";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "压力";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "紫外灯能量";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3[0] = "是否更换采样膜";
            dt.Rows.Add(dr3);
            DataRow dr4 = dt.NewRow();
            dr4[0] = "膜有效负荷";
            dt.Rows.Add(dr4);
            DataRow dr5 = dt.NewRow();
            dr5[0] = "是否更换滤膜（筒）";
            dt.Rows.Add(dr5);
            DataRow dr6 = dt.NewRow();
            dr6[0] = "报警信息";
            dt.Rows.Add(dr6);
            DataView dv = dt.DefaultView;
            gridInstrumentInfo.DataSource = dv;
        }

        protected void gridInstrumentInfo_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void gridInstrumentInfo_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
               
                    col.HeaderText = col.DataField;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
              
            }
            catch (Exception ex) { }
        }

        protected void gridCalibration_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

        }

        protected void gridCalibration_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }
    }
}