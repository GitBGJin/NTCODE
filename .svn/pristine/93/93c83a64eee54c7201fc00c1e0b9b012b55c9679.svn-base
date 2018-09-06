using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Chart
{
    /// <summary>
    /// 名称：PolaryChart.aspx.cs
    /// 创建人：徐阳
    /// 创建日期：2017-06-12
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 玫瑰图数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class PolaryChart : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind();
            }
        }

        public void bind()
        {
            string pointIds = PageHelper.GetQueryString("pointIds");
            string windDir = PageHelper.GetQueryString("WindDir");
            string polaryType = PageHelper.GetQueryString("PolaryType");
            string radlDataType = PageHelper.GetQueryString("radlDataType");
            string factor = PageHelper.GetQueryString("factor");
            string dtStart = PageHelper.GetQueryString("dtBegin");
            string dtEnd = PageHelper.GetQueryString("dtEnd");
            string flag = PageHelper.GetQueryString("flag");

            hddataForAjax.Value = pointIds + "," + windDir + "," + polaryType + "," + radlDataType + "," + factor + "," + dtStart + "," + dtEnd + "," + flag;

            //RegisterScript("generate();");
        }
    }
}