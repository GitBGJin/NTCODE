using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Portal
{
    public partial class HomePageLZ : SmartEP.WebUI.Common.BasePageLZ
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitNavigateUrl();
            }
        }
        private void InitNavigateUrl()
        {
            RadImageWaterLZ.NavigateUrl = "MidPageLZ.aspx?Type=WaterLZ";
            RadImageMgr.NavigateUrl = "MidPageLZ.aspx?Type=Mgr";
            RadImageGis.NavigateUrl = "MidPageLZ.aspx?Type=GisLZ";
        }

    }
}