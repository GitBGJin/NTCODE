using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.BaseData
{
    /// <summary>
    /// 名称：AcqusionInstrumentEdit.cs
    /// 创建人：季柯
    /// 创建日期：2015-09-01
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：数采仪编辑页面
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AcqusionInstrumentEdit :BasePage
    {
        //数采仪、点位服务
        AcquisitionInstrumentService acqInstrumentService = new AcquisitionInstrumentService();
        MonitoringPointService pointService = new MonitoringPointService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string pointGuid = PageHelper.GetQueryString("MonitoringPointUid");
                if (!string.IsNullOrEmpty(pointGuid))
                {
                    this.ViewState["PointUid"] = pointGuid;
                    InitForm(pointGuid);
                }
                else
                {

                }
            }
        }

        private void InitForm(string pointGuid)
        {
            AcquisitionInstrumentEntity entity = acqInstrumentService.RetrieveEntityByMonitoringPointUid(pointGuid);
            MonitoringPointEntity pointEntity = pointService.RetrieveEntityByUid(pointGuid);
            if (entity != null)
            {
                txtMonitoringPoint.Text = pointEntity.MonitoringPointName;

                txtAcqName.Text = entity.AcquisitionName;
                txtCommunicationPort.Text = entity.CommunicationPort;
                txtEncryptionKey.Text = entity.EncryptionKey;
                txtExcessiveCount.Text = entity.ExcessiveCount.ToString();
                txtIP.Text = entity.IP;
                txtIPPwd.Text = entity.IpPwd;
                txtIPUser.Text = entity.IpUser;
                txtMN.Text = entity.MN;
                txtRetransmissionNum.Text = entity.RetransmissionNum.ToString();
                //cbxIsACK.Checked = entity.ACKOrNot.Value;
                //cbxIsCRC.Checked = entity.CRCOrNot.Value;
                //cbxIsEncryption.Checked = entity.EncryptionOrNot.Value;
                cbxIsACK.Checked = entity.ACKOrNot == null ? false : entity.ACKOrNot.Value;
                cbxIsCRC.Checked = entity.CRCOrNot == null ? false : entity.CRCOrNot.Value;
                cbxIsEncryption.Checked = entity.EncryptionOrNot== null ? false : entity.EncryptionOrNot.Value;

                txtMemo.Text = entity.Description;
            }
        }

          /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ViewState["PointUid"] == null)
            {
                Alert("当前配置不存在！");
                return;
            }
            AcquisitionInstrumentEntity entity = acqInstrumentService.RetrieveEntityByMonitoringPointUid(ViewState["PointUid"].ToString());
            if (entity == null)
            {
                Alert("当前配置不存在！");
                return;
            }
            //不能重名
            if (txtAcqName.Text != entity.AcquisitionName && acqInstrumentService.IsExistByName(txtAcqName.Text))
            {
                Alert("数采仪名称已存在！");
                return;
            }
            if (txtMN.Text != entity.MN && acqInstrumentService.IsExistByMN(txtMN.Text))
            {
                Alert("MN号已存在！");
                return;
            }
            int retranNum = 0, excessiveCount = 0;
            int.TryParse(txtRetransmissionNum.Text, out retranNum);
            int.TryParse(txtExcessiveCount.Text, out excessiveCount);
            entity.AcquisitionName = txtAcqName.Text.Trim();
            entity.CommunicationPort = txtCommunicationPort.Text;
            entity.EncryptionKey = txtEncryptionKey.Text;
            entity.RetransmissionNum = retranNum;
            entity.ExcessiveCount = excessiveCount;
            entity.IP =  txtIP.Text;
            entity.IpPwd = txtIPPwd.Text;
            entity.IpUser = txtIPUser.Text;
            entity.MN = txtMN.Text.Trim();
             
            entity.ACKOrNot = cbxIsACK.Checked;
            entity.CRCOrNot = cbxIsCRC.Checked;
            entity.EncryptionOrNot = cbxIsEncryption.Checked;
            entity.Description = txtMemo.Text;

            //描述
            entity.Description = txtMemo.Text;
            entity.UpdateUser = String.IsNullOrEmpty(SessionHelper.Get("LoginID")) ? "system" : SessionHelper.Get("LoginID");
           
            entity.UpdateDateTime = DateTime.Now;

            acqInstrumentService.Update(entity);

            Alert("保存成功！");

            //界面刷新
            RadScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "<script>RefreshParent();</script>", false);

        }

    }
}