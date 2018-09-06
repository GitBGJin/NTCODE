using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebControl.CbxRsm;
using SmartEP.WebUI.Common;
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
    /// 名称：DataSamplingRateAnalyzeDetail.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：环境空气数据捕获率统计详情
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class DataSamplingRateAnalyzeDetail : SmartEP.Core.Interfaces.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private AirDataSamplingRateService m_AirDataSamplingRateQueryService;

        /// <summary>
        /// 选择的因子，从查询界面传递过来
        /// </summary>
        private IList<IPollutant> Factors
        {
            get
            {
                if (Session["DataSamplingRateAnalyzeFactors"] == null)
                {
                    return new List<IPollutant>();
                }
                return Session["DataSamplingRateAnalyzeFactors"] as IList<IPollutant>;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            m_AirDataSamplingRateQueryService = new AirDataSamplingRateService();
            if (!this.IsPostBack)
            {
                InitForm();
            }
        }

        /// <summary>
        /// 界面数据初始化
        /// </summary>
        private void InitForm()
        {
            string samplingRateId = PageHelper.GetQueryString("SamplingRateId");
            if (!string.IsNullOrEmpty(samplingRateId))
            {
                this.ViewState["SamplingRateId"] = samplingRateId;
            }
        }

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid(string samplingRateId)
        {
            DateTime dtmBegin = DateTime.TryParse(PageHelper.GetQueryString("dtmBegin"), out dtmBegin)
                        ? dtmBegin : DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime dtmEnd = DateTime.TryParse(PageHelper.GetQueryString("dtmEnd"), out dtmEnd)
                        ? dtmEnd : DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            string[] portIds = { samplingRateId };
            string[] factors = Factors.Select(t => t.PollutantCode).ToArray();
            int pageSize = gridSamplingRateDetail.PageSize;//每页显示数据个数  
            int pageNo = gridSamplingRateDetail.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数

            //绑定数据
            var samplingRateData = m_AirDataSamplingRateQueryService.GetSamplingRateDetailData(portIds, factors, dtmBegin, dtmEnd, pageSize, pageNo, out recordTotal);
            if (samplingRateData == null)
            {
                gridSamplingRateDetail.DataSource = new DataTable();
            }
            else
            {
                gridSamplingRateDetail.DataSource = samplingRateData;
            }

            //数据总行数
            gridSamplingRateDetail.VirtualItemCount = recordTotal;

            //统计日期
            gridSamplingRateDetail.MasterTableView.Caption = string.Format("统计日期：{0:yyyy-MM-dd}~{1:yyyy-MM-dd}", dtmBegin, dtmEnd);
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridSamplingRateDetail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid(ViewState["SamplingRateId"].ToString());
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridSamplingRateDetail_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem item = e.Item as GridDataItem;
            //    DataRowView drv = e.Item.DataItem as DataRowView;
            //for (int iRow = 0; iRow < factors.Count; iRow++)
            //{
            //    RsmFactor factor = factors[iRow];
            //    GridTableCell cell = (GridTableCell)item[factor.FactorCode];
            //    string factorFlag = drv[factor.FactorCode + "_Status"] != DBNull.Value ? drv[factor.FactorCode + "_Status"].ToString() : string.Empty;
            //    cell.Text = cell.Text + factorFlag;
            //    //cell.Text = factorinfo.GetAlermString(drv[factorinfo.factorColumnName], drv["dataflag"], cell);
            //}
            //}
        }
        #endregion
    }
}