using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.Frame;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class AuditReason : SmartEP.WebUI.Common.BasePage
    {
        AuditReasonService reasonService = new AuditReasonService();
        AuditOperatorSettingService operatorService = new AuditOperatorSettingService();
        UserService userService = new UserService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        #region 初始化
        private void InitControl()
        {
            string key = Request.QueryString["operator"];
            ResonCombox.DataSource = reasonService.GetAuditReasonList(Session["applicationUID"].ToString(), Request.QueryString["operator"]);
            //ResonCombox.DataSource = operatorService.GetAuditReasonList(Session["applicationUID"].ToString(), Request.QueryString["operator"]);
            ResonCombox.DataValueField = "ReasonGuid";
            ResonCombox.DataTextField = "ReasonContent";
            ResonCombox.DataBind();

            if (ResonCombox.Items.Count > 0)
                resonText.Text = ResonCombox.SelectedItem.Text;
        }
        #endregion

        #region 事件
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (ResonCombox.Items.Where(x => x.Text.Trim().Equals(resonText.Text.Trim())).Count() <= 0)
            {
                AuditReasonEntity reason = new AuditReasonEntity();
                reason.ReasonGuid = Guid.NewGuid().ToString();
                reason.ReasonContent = resonText.Text.Trim();
                reason.ApplicationUid = Session["applicationUID"].ToString();
                reason.EnableOrNot = true;
                reason.CreatDateTime = DateTime.Now;
                reason.CreatUser = userService.GetUserByUserId(new Guid(Session["UserGuid"].ToString())).LoginName;
                reason.UpdateDateTime = DateTime.Now;
                reason.UpdateUser = userService.GetUserByUserId(new Guid(Session["UserGuid"].ToString())).LoginName;
                reasonService.Add(reason);
            }
        }

        protected void ResonCombox_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                resonText.Text = ResonCombox.SelectedItem.Text;
            }
            catch
            {
            }
        }
        #endregion
    }
}