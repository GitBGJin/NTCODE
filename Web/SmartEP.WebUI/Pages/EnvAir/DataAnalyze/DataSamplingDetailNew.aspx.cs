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
    /// 名称：DataSamplingDetailNew.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-1-4
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：环境空气数据捕获率统计详情
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class DataSamplingDetailNew : SmartEP.Core.Interfaces.BasePage
    {
        AirDataSamplingRateNewService m_AirDataSamplingRateNewService = new AirDataSamplingRateNewService();
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
            string effectRateInfoId = PageHelper.GetQueryString("SamplingRateId");
            if (!string.IsNullOrEmpty(effectRateInfoId) && Session["airFactors"] != null)
            {
                this.ViewState["SamplingRateId"] = effectRateInfoId;
            }
            else
            {
                this.ViewState["SamplingRateId"] = "";
            }
        }
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid(string effectRateInfoId)
        {
            DateTime dtmBegin = DateTime.TryParse(PageHelper.GetQueryString("dtmBegin"), out dtmBegin)
                        ? dtmBegin : DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime dtmEnd = DateTime.TryParse(PageHelper.GetQueryString("dtmEnd"), out dtmEnd)
                        ? dtmEnd : DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            string[] factors = Session["airFactors"] as string[];
            string[] portIds = { effectRateInfoId };
            var samplingRateData = m_AirDataSamplingRateNewService.GetSamplingDetailData(portIds, factors, dtmBegin, dtmEnd);
            if (samplingRateData == null)
            {
                gridSamplingRateDetail.DataSource = new DataTable();
            }
            else
            {
                samplingRateData.Sort = " NewTime desc,PollutantName desc";
                gridSamplingRateDetail.DataSource = samplingRateData;
            }
            //统计日期
            gridSamplingRateDetail.MasterTableView.Caption = string.Format("统计日期：{0:yyyy-MM-dd}~{1:yyyy-MM-dd}", dtmBegin, dtmEnd);
        }
        #endregion

        protected void gridSamplingRateDetail_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid(ViewState["SamplingRateId"].ToString());
        }

        protected void gridSamplingRateDetail_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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
    }
}