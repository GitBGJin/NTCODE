using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.BaseData.MonitoringPoint
{
    public partial class MonitoringInstrumentEdit : BasePage
    {
        private MonitoringInstrumentService monitorInstrumentService = new MonitoringInstrumentService();
        private InstrumentService instrumentService = new InstrumentService();
        private AcquisitionInstrumentService acquisitionService = new AcquisitionInstrumentService();
        private MonitoringPointService pointService = new MonitoringPointService();
        public string pointUid,instrumentUid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                pointUid = PageHelper.GetQueryString("MonitoringPointUid");
                this.ViewState["PointUid"] = pointUid;
                string applicationType = PageHelper.GetQueryString("ApplicationType");
                instrumentUid = PageHelper.GetQueryString("InstrumentUid");
                this.ViewState["instrumentUid"] = instrumentUid;
                if (!string.IsNullOrEmpty(pointUid))
                {
                    InitControl(applicationType);
                }

            }
        }

        /// <summary>
        /// 初始化仪器下拉列表
        /// </summary>
        private void InitControl(string applicationType)
        {
            if (applicationType == "air")
                rcbInstrument.DataSource = instrumentService.RetrieveAirInstrumentList();
            else
                rcbInstrument.DataSource = instrumentService.RetrieveWaterInstrumentList();
            rcbInstrument.DataTextField = "InstrumentName";
            rcbInstrument.DataValueField = "RowGuid";
            rcbInstrument.DataBind();
            rcbInstrument.Items.Insert(0, new RadComboBoxItem("", ""));
            //站点
            txtPointName.Text = pointService.RetrieveEntityByUid(pointUid).MonitoringPointName;
            rcbInstrument.SelectedValue = instrumentUid;
            rcbInstrument.Enabled = false;

            //绑定控件的值
            MonitoringInstrumentEntity newMonitoringInstrument = monitorInstrumentService.RetrieveByPointUid(pointUid, instrumentUid);
            if (newMonitoringInstrument != null)
            {
                txtFetchDataTime.Text = newMonitoringInstrument.FetchDataTime.ToString();
                txtInstrumentNumber.Text = newMonitoringInstrument.InstrumentNumber;
                txtMemo.Text = newMonitoringInstrument.Description;
                txtSerialNum.Text = newMonitoringInstrument.SerialNum;
                txtSerialPort.Text = newMonitoringInstrument.SerialPort.ToString();
                txtSocketIP.Text = newMonitoringInstrument.SocketIP;
                txtSocketProtocol.Text = newMonitoringInstrument.SocketProtocol;
                txtSupportInterfaces.Text = newMonitoringInstrument.SupportInterfaces;
                //新增使用接口  串口通讯设置  质保期
                txtUseInterfaces.Text = newMonitoringInstrument.UseInterfaces;
                txtWarrantyPeriod.Text = Convert.ToString(newMonitoringInstrument.WarrantyPeriod);
                txtSerialSetting.Text = newMonitoringInstrument.SerialSetting;
                txtSocketPort.Text = Convert.ToString(newMonitoringInstrument.SocketPort);

            }
        }

        /// <summary>
        /// 修改监测仪器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {

                MonitoringInstrumentEntity newMonitoringInstrument = monitorInstrumentService.RetrieveByPointUid(ViewState["PointUid"].ToString(), this.ViewState["instrumentUid"].ToString());

                if (newMonitoringInstrument != null)
                {
                    if (txtFetchDataTime.Text != "")
                        newMonitoringInstrument.FetchDataTime = int.Parse(txtFetchDataTime.Text.Trim());
                    newMonitoringInstrument.InstrumentNumber = txtInstrumentNumber.Text.Trim();
                    newMonitoringInstrument.Description = txtMemo.Text.Trim();
                    newMonitoringInstrument.SerialNum = txtSerialNum.Text.Trim();
                    if (txtSerialPort.Text != "")
                        newMonitoringInstrument.SerialPort = int.Parse(txtSerialPort.Text.Trim());
                    //newMonitoringInstrument.SerialSetting = txtSerialNum.Text.Trim();
                    newMonitoringInstrument.SocketIP = txtSocketIP.Text.Trim();
                    if (txtSocketPort.Text != "")
                        newMonitoringInstrument.SocketPort = int.Parse(txtSocketPort.Text.Trim());
                    newMonitoringInstrument.SocketProtocol = txtSocketProtocol.Text.Trim();
                    newMonitoringInstrument.SupportInterfaces = txtSupportInterfaces.Text.Trim();

                    //新增使用接口  串口通讯设置  质保期
                    newMonitoringInstrument.UseInterfaces = txtUseInterfaces.Text.Trim();
                    if (txtWarrantyPeriod.Text != "")
                       newMonitoringInstrument.WarrantyPeriod = int.Parse(txtWarrantyPeriod.Text.Trim());
                    newMonitoringInstrument.SerialSetting = txtSerialSetting.Text.Trim();


                    monitorInstrumentService.Update(newMonitoringInstrument);

                    Alert("更新成功！");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
                }
            }

            catch (Exception Err)
            {
                Alert("更新失败！");
            }
        }
    }
}