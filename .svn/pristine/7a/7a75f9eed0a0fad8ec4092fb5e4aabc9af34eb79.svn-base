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
    public partial class MonitoringInstrumentAdd : BasePage
    {
        private MonitoringInstrumentService monitorInstrumentService = new MonitoringInstrumentService();
        private InstrumentService instrumentService = new InstrumentService();
        private AcquisitionInstrumentService acquisitionService = new AcquisitionInstrumentService();
        private MonitoringPointService pointService = new MonitoringPointService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string pointGuid = PageHelper.GetQueryString("MonitoringPointUid");
                string applicationType = PageHelper.GetQueryString("ApplicationType");
                if (!string.IsNullOrEmpty(pointGuid))
                {
                    this.ViewState["PointUid"] = pointGuid;
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
            txtPointName.Text = pointService.RetrieveEntityByUid(ViewState["PointUid"].ToString()).MonitoringPointName;


        }
        /// <summary>
        /// 离线配置追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
               //先获取数采仪Uid
               string acqusionInstrumentUid = acquisitionService.RetrieveEntityByMonitoringPointUid(ViewState["PointUid"].ToString()).AcquisitionUid;

               if (!monitorInstrumentService.IsExistByAcqUid(acqusionInstrumentUid, rcbInstrument.SelectedValue))
               {
                   MonitoringInstrumentEntity newMonitoringInstrument = new MonitoringInstrumentEntity();
                   newMonitoringInstrument.MonitoringinstrumentUid = Guid.NewGuid().ToString();
                   newMonitoringInstrument.AcquisitionUid = acqusionInstrumentUid;
                   newMonitoringInstrument.InstrumentUid = rcbInstrument.SelectedValue;

                   if (txtFetchDataTime.Text != "")
                       newMonitoringInstrument.FetchDataTime = int.Parse(txtFetchDataTime.Text.Trim());
                   newMonitoringInstrument.InstrumentNumber = txtInstrumentNumber.Text.Trim();
                   newMonitoringInstrument.Description = txtMemo.Text.Trim();
                   newMonitoringInstrument.SerialNum = txtSerialNum.Text.Trim();
                   if (txtSerialPort.Text != "")
                       newMonitoringInstrument.SerialPort = int.Parse(txtSerialPort.Text.Trim());
                  // newMonitoringInstrument.SerialSetting = txtSerialNum.Text.Trim();
                   newMonitoringInstrument.SocketIP = txtSocketIP.Text.Trim();
                   if (txtSocketPort.Text != "")
                       newMonitoringInstrument.SocketPort = int.Parse(txtSocketPort.Text.Trim());
                   newMonitoringInstrument.SocketProtocol = txtSocketProtocol.Text.Trim();
                   newMonitoringInstrument.SupportInterfaces = txtSupportInterfaces.Text.Trim();
                   //新增使用接口  串口通讯设置  质保期
                   newMonitoringInstrument.UseInterfaces = txtUseInterfaces.Text.Trim();
                   if (txtWarrantyPeriod.Text!= "")
                      newMonitoringInstrument.WarrantyPeriod =int.Parse(txtWarrantyPeriod.Text.Trim());
                   newMonitoringInstrument.SerialSetting = txtSerialSetting.Text.Trim();

                   monitorInstrumentService.Add(newMonitoringInstrument);
                   Alert("添加成功！");
                   ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
               }
               else
               {
                   Alert("仪器已存在！");
                   return;
               }
            }
            catch (Exception Err)
            {
                Alert("保存失败！");
            }
        }
    } 
}