using SmartEP.Core.Enums;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class AuditReasonAdd : SmartEP.WebUI.Common.BasePage
    {
        AuditReasonService g_AuditReasonService = new AuditReasonService();
        //设备类型
        private ApplicationType applicationType = ApplicationType.Air;
        //理由主键
        private string ReasonGuid = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ReasonGuid = Convert.ToString(Request["ReasonGuid"]);
            if (!IsPostBack)
            {
                InitControl(this.ReasonGuid);
            }
        }

        #region 初始化界面数据
        /// <summary>
        /// 初始化界面数据
        /// </summary>
        private void InitControl(string ReasonGuid)
        {
            if (string.IsNullOrEmpty(ReasonGuid))
            {
                return;
            }
            AuditReasonEntity[] reason = g_AuditReasonService.GetAuditReason(ReasonGuid).ToArray();
            if (reason.Length != 1)
            {
                Alert("加载数据失败！");
                return;
            }
            txtReasonContent.Text = reason[0].ReasonContent;
            txtOrderByNum.Text = reason[0].OrderByNum.ToString();
            txtDescription.Value = reason[0].Description;
            btnAdd.Visible = false;
            btnSave.Visible = true;
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (txtReasonContent.Text.Trim() == "")
            {
                Alert("请填写审核理由！");
                return;
            }
            AuditReasonEntity reason = new AuditReasonEntity();
            reason.ReasonGuid = Guid.NewGuid().ToString();
            reason.ReasonContent = txtReasonContent.Text.Trim();
            reason.OrderByNum = txtOrderByNum.Text.Trim() == "" ? 0 : Convert.ToInt32(txtOrderByNum.Text.Trim());
            reason.Description = txtDescription.Value;
            reason.CreatUser = Convert.ToString(Session["DisplayName"]);
            reason.CreatDateTime = DateTime.Now;
            reason.EnableOrNot = true;
            reason.ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType);
            g_AuditReasonService.Add(reason);
            Alert("添加成功！");
            RadScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "<script>RefreshParent();</script>", false);
        }
        #endregion

        #region 修改保存
        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (txtReasonContent.Text.Trim() == "")
            {
                Alert("请填写审核理由！");
                return;
            }
            AuditReasonEntity[] reason = g_AuditReasonService.GetAuditReason(this.ReasonGuid).ToArray();
            if (reason.Length != 1)
            {
                Alert("保存失败！");
                return;
            }
            AuditReasonEntity newreason = reason[0];
            newreason.ReasonContent = txtReasonContent.Text.Trim();
            newreason.OrderByNum = txtOrderByNum.Text.Trim() == "" ? 0 : Convert.ToInt32(txtOrderByNum.Text.Trim());
            newreason.Description = txtDescription.Value;
            newreason.UpdateUser = Convert.ToString(Session["DisplayName"]);
            newreason.UpdateDateTime = DateTime.Now;
            g_AuditReasonService.Update(newreason);
            Alert("保存成功！");
            RadScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "<script>RefreshParent();</script>", false);
        }
        #endregion
    }
}