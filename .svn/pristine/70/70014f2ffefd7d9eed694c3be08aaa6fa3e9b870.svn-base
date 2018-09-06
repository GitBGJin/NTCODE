using SmartEP.Core.Interfaces;
using SmartEP.Service.AutoMonitoring.Air;
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
    /// 名称：TeomTrafficHardwareInfo.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-09-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：TEOM流量控制器校准（硬件）(Teom颗粒物监测仪流量小准记录2)
    /// 版权所有(C)：江苏远大信息股份有限公司
    public partial class TeomTrafficHardwareInfo : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        AirDataEffectRateService airDataEffectRate = new AirDataEffectRateService();

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 统计
        /// </summary>
        DataView dvStatistical = null;

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