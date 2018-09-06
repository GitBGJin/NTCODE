using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class QualityControlDataSearch : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            rdpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            rdpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            if (!IsPostBack)
            {
                pointCbxRsm_SelectedChanged();
            }
            DataTable dt = new DataTable();
            string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            foreach (string portId in portIds)
            {
                if (portId == "1")
                {
                    dt.Columns.Add("Factor", typeof(string));
                    DataRow dr = dt.NewRow();
                    dr[0] = "二氧化氮";
                    dt.Rows.Add(dr);

                    DataRow dr1 = dt.NewRow();
                    dr1[0] = "一氧化氮";
                    dt.Rows.Add(dr1);

                    DataRow dr2 = dt.NewRow();
                    dr2[0] = "二氧化硫";
                    dt.Rows.Add(dr2);

                    DataRow dr3 = dt.NewRow();
                    dr3[0] = "臭氧";
                    dt.Rows.Add(dr3);

                    DataRow dr4 = dt.NewRow();
                    dr4[0] = "一氧化碳";
                    dt.Rows.Add(dr4);

                    DataRow dr5 = dt.NewRow();
                    dr5[0] = "氮氧化物";
                    dt.Rows.Add(dr5);
                }
                else
                {
                    dt.Columns.Add("Factor", typeof(string));
                    DataRow dr = dt.NewRow();
                    dr[0] = "二氧化氮";
                    dt.Rows.Add(dr);

                    DataRow dr1 = dt.NewRow();
                    dr1[0] = "一氧化氮";
                    dt.Rows.Add(dr1);

                    DataRow dr2 = dt.NewRow();
                    dr2[0] = "二氧化硫";
                    dt.Rows.Add(dr2);

                    DataRow dr3 = dt.NewRow();
                    dr3[0] = "臭氧";
                    dt.Rows.Add(dr3);

                    DataRow dr4 = dt.NewRow();
                    dr4[0] = "一氧化碳";
                    dt.Rows.Add(dr4);

                    DataRow dr5 = dt.NewRow();
                    dr5[0] = "氮氧化物";
                    dt.Rows.Add(dr5);
                }
            }

            DataView dv = dt.DefaultView;
            grdQualityControl.DataSource = dv;
        }

        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdQualityControl_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdQualityControl_ItemDataBound(object sender, GridItemEventArgs e)
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
            }
            //GridDataItem item = e.Item as GridDataItem;
            //if (e.Item is GridDataItem)
            //{
            //    if (item["PointId"] != null)
            //    {
            //        GridTableCell pointCell = (GridTableCell)item["PointId"];
            //        IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
            //        if (point != null)
            //            pointCell.Text = point.PointName;
            //    }
            //    if (item["PollutantCode"] != null)
            //    {
            //        GridTableCell factorCell = (GridTableCell)item["PollutantCode"];
            //        IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(factorCell.Text.Trim()));
            //        if (factor != null)
            //            factorCell.Text = factor.PollutantName;
            //    }
            //}
        }

        #endregion

        protected void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                foreach (GridItem item in grdQualityControl.Items)
                {
                    (item.FindControl("checkSingle") as CheckBox).Checked = true;
                    item.Selected = true;
                }
            }
            else
            {
                foreach (GridItem item in grdQualityControl.Items)
                {
                    (item.FindControl("checkSingle") as CheckBox).Checked = false;
                    item.Selected = false;
                }
            }
        }

        protected void checkSingle_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).Parent.Parent as GridItem).Selected = (sender as CheckBox).Checked;
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {

        }

        /// <summary>
        /// 站点因子关联
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
            points = pointCbxRsm.GetPoints();
            InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
            IList<string> list = new List<string>();
            string[] factor;
            string factors = string.Empty;
            foreach (IPoint point in points)
            {
                IQueryable<PollutantCodeEntity> p = m_InstrumentChannelService.RetrieveChannelListByPointUid(point.PointGuid);
                list = list.Union(p.Select(t => t.PollutantName)).ToList();
            }
            factor = list.ToArray();
            foreach (string f in factor)
            {
                factors += f + ";";
            }
            factorCbxRsm.SetFactorValuesFromNames(factors);
        }


    }
}