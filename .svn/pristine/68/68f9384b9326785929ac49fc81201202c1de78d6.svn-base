using SmartEP.Core.Interfaces;
using SmartEP.Service.AutoMonitoring.Water;
using SmartEP.Service.DataAnalyze.Water;
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
    /// 名称：AddInstrumentOther.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：多点线性校准结果和零/跨漂移情况
    /// 版权所有(C)：江苏远大信息股份有限公司
    public partial class AddInstrumentOther : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        WaterDataEffectRateService waterDataEffectRate = new WaterDataEffectRateService();

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;

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

        }
        #endregion




    }
}