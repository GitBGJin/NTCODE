using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Utilities.Web.UI;
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
    /// <summary>
    /// 名称：DataEffectRateInfoNew.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-08-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：数据有效率详情
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class DataEffectRateInfoNew : System.Web.UI.Page
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        AirDataEffectRateNewService airDataEffectRateNew = new AirDataEffectRateNewService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitForm();
            }
        }

        /// <summary>
        /// 界面数据初始化
        /// </summary>
        private void InitForm()
        {
            string effectRateInfoId = PageHelper.GetQueryString("effectRateInfoId");


            if (!string.IsNullOrWhiteSpace(effectRateInfoId) && Session["airFactors"] != null)
            {
                this.ViewState["effectRateInfoId"] = effectRateInfoId;
            }
        }
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid(string effectRateInfoId)
        {
            string[] factors = Session["airFactors"] as string[];
            DateTime dtmBegin = DateTime.TryParse(PageHelper.GetQueryString("dtmBegin"), out dtmBegin)
                        ? dtmBegin : DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime dtmEnd = DateTime.TryParse(PageHelper.GetQueryString("dtmEnd"), out dtmEnd)
                        ? dtmEnd : DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            string[] portIds = { effectRateInfoId };
            int pageSize = gridEffectRateInfo.PageSize;//每页显示数据个数  
            int pageNo = gridEffectRateInfo.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数

            //绑定数据                               
            var samplingRateData = airDataEffectRateNew.GetEffectRateDetailData(portIds, factors, dtmBegin, dtmEnd, pageSize, pageNo, out recordTotal);
            if (samplingRateData == null)
            {
                gridEffectRateInfo.DataSource = new DataTable();
            }
            else
            {
                samplingRateData.Sort = " NewTime desc,PollutantName desc";
                gridEffectRateInfo.DataSource = samplingRateData;
            }

            //数据总行数
            gridEffectRateInfo.VirtualItemCount = recordTotal;

            //统计日期
            gridEffectRateInfo.MasterTableView.Caption = string.Format("统计日期：{0:yyyy-MM-dd}~{1:yyyy-MM-dd}", dtmBegin, dtmEnd);
        }


        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridEffectRateInfo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid(ViewState["effectRateInfoId"].ToString());
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridEffectRateInfo_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["ReportDateTime"] != null)
                {
                    GridTableCell Cell = (GridTableCell)item["ReportDateTime"];
                    DateTime dtNow = DateTime.Now.AddYears(1);
                    DateTime dt = DateTime.TryParse(Cell.Text, out dt) ? dt : dtNow;
                    if (dt != dtNow)
                    {
                        Cell.Text = string.Format("{0:yyyy-MM-dd}", dt);
                    }
                }
            }
        }

        #endregion

    }
}