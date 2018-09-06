using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using Aspose.Cells;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Collections.Generic;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using SmartEP.Service.BaseData.Exchange;
using SmartEP.Core.Generic;
using SmartEP.Utilities.Transfer;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.Caching;

namespace SmartEP.WebUI.Pages.EnvAir.Exchange
{
    public partial class DayReportDBFUpload : SmartEP.WebUI.Common.BasePage
    {
      
        protected void Page_Load(object sender, System.EventArgs e)
        {
          InitialControl();

        }
        /// <summary>
        /// 设置站点控件选中超级站
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            string isAudit = "1";
            if (isAudit != null && isAudit != "")
            {
                factorCbxRsm.isAudit(isAudit);
            }
        }
      /// <summary>
      /// 初始化控件
      /// </summary>
        private void InitialControl()
        {
          DateTime StartDate = DateTime.Now.AddDays(-1);
          DateTime EndDate = DateTime.Now;
          dtpBegin.SelectedDate = StartDate;
          dtpEnd.SelectedDate = EndDate;
        }


        
    }
}

