using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Chart
{
    public partial class ChartFrame : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string fac = PageHelper.GetQueryString("fac");
                string name = PageHelper.GetQueryString("name");
                hdfac.Value = fac;
                hdname.Value = name;
            }
        }
    }
}