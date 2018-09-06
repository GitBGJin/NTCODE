using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Alarm;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Alarm
{
    public partial class AlarmNotifyEdit : SmartEP.WebUI.Common.BasePage
    {
        DictionaryService dicService = new DictionaryService();
        NotifyStrategyService notifyStrategyService = new NotifyStrategyService();
        NotifyNumbersService notifyNumberService = new NotifyNumbersService();
        NotifyMailsService notifyMailService = new NotifyMailsService();
        MonitoringPointService pointService = new MonitoringPointService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string notifyStrategyGuid = PageHelper.GetQueryString("NotifyStrategyUid");
                if (!string.IsNullOrEmpty(notifyStrategyGuid))
                {
                    this.ViewState["NotifyStrategyUid"] = notifyStrategyGuid;
                    InitControl();
                    BindUI();
                }
            }
        }
        /// <summary>
        /// 初始化下拉列表
        /// </summary>
        private void InitControl()
        {
            //通知级别
            IQueryable<V_CodeMainItemEntity> regionEntites = dicService.RetrieveList(DictionaryType.AMS, "通知级别");
            notifyGradeList.DataSource = regionEntites;
            notifyGradeList.DataTextField = "ItemText";
            notifyGradeList.DataValueField = "ItemGuid";
            notifyGradeList.DataBind();
            notifyGradeList.Items.Insert(0, new RadComboBoxItem("", ""));

            //通知号码
            IQueryable<V_NotifyNumberEntity> notifyNumberEntitys = notifyNumberService.RetrieveList();
            notifyNumberList.DataSource = notifyNumberEntitys;
            notifyNumberList.DataTextField = "Name";
            notifyNumberList.DataValueField = "RowGuid";
            notifyNumberList.DataBind();

            //通知邮箱
            IQueryable<V_NotifyMaliEntity> notifyMailEntitys = notifyMailService.RetrieveList();
            notifyMailList.DataSource = notifyMailEntitys;
            notifyMailList.DataTextField = "Name";
            notifyMailList.DataValueField = "RowGuid";
            notifyMailList.DataBind();

        }

        private void BindUI()
        {
            //绑定控件值
            NotifyStrategyEntity notifyStrategy = notifyStrategyService.RetrieveEntity(ViewState["NotifyStrategyUid"].ToString());
            if (notifyStrategy != null)
            {
                txtNotifyStrategyName.Text = notifyStrategy.NotifyStrategyName;
                notifyGradeList.SelectedValue = notifyStrategy.NotifyGradeUid;
                alarmEventList.SelectedValue = notifyStrategy.AlarmEventUid;
                txtBeginTime.SelectedTime = TimeSpan.Parse(notifyStrategy.BeginTime);
                txtEndTime.SelectedTime = TimeSpan.Parse(notifyStrategy.EndTime);
                txtNotifyCount.Text = notifyStrategy.NotifyCount.ToString();
                txtNotifySpan.Text = notifyStrategy.NotifySpan;
                if (!string.IsNullOrEmpty(notifyStrategy.BeginDate))
                {
                    beginDate.SelectedDate = Convert.ToDateTime(notifyStrategy.BeginDate);
                }
                if (!string.IsNullOrEmpty(notifyStrategy.EndDate))
                {
                    endDate.SelectedDate = Convert.ToDateTime(notifyStrategy.EndDate);
                }
                rbtnEnableOrNot.SelectedValue = notifyStrategy.EnableOrNot == true ? "1" : "0";
                tbxOrderByNum.Text = notifyStrategy.OrderByNum.ToString();
                txtDescription.Text = notifyStrategy.Description;

                foreach (RadComboBoxItem cmbItem in notifyNumberList.Items)
                {
                    if (notifyStrategy.NotifyNumberUids.Contains(cmbItem.Value))
                    {
                        cmbItem.Checked = true;
                    }
                }
                foreach (RadComboBoxItem cmbItem in notifyMailList.Items)
                {
                    if (notifyStrategy.NotifyMailUids.Contains(cmbItem.Value))
                    {
                        cmbItem.Checked = true;
                    }
                }
                if (notifyStrategy.EffectSubject.Contains("7e05b94c-bbd4-45c3-919c-42da2e63fd43"))
                {
                    rbtnlType.SelectedValue = "CityProper";
                    BindRad();
                }
                else
                {
                    rbtnlType.SelectedValue = "Port";
                    BindRad();
                    pointCbxRsm.SetPointValuesFromNames(pointService.RetrievePointNamesByPointUids(notifyStrategy.EffectSubject.Split(';')));
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //通知策略实体
                NotifyStrategyEntity notifyStrategy = notifyStrategyService.RetrieveEntity(ViewState["NotifyStrategyUid"].ToString());
                notifyStrategy.NotifyGradeUid = notifyGradeList.SelectedValue;
                if (rbtnlType.SelectedValue == "Port")
                {
                    notifyStrategy.EffectSubject = StringExtensions.GetArrayStrNoEmpty(pointCbxRsm.GetPointValues(CbxRsmReturnType.Guid).ToList(), ";"); //pGuid.TrimEnd(';');
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    notifyStrategy.EffectSubject = ddlCityProper.SelectedValue;
                }
                string numbers = "";
                string mails = "";
                foreach (RadComboBoxItem cmbItem in notifyNumberList.Items)
                {
                    if (cmbItem.Checked == true)
                    {
                        numbers += cmbItem.Value + ";";
                    }
                }
                foreach (RadComboBoxItem cmbItem in notifyMailList.Items)
                {
                    if (cmbItem.Checked == true)
                    {
                        mails += cmbItem.Value + ";";
                    }
                }
                notifyStrategy.NotifyNumberUids = numbers.TrimEnd(';');
                notifyStrategy.NotifyNumbers = notifyNumberService.RetrieveNumbersByUids(notifyStrategy.NotifyNumberUids.Split(';'));
                notifyStrategy.NotifyMailUids = mails.TrimEnd(';');
                notifyStrategy.NotifyMails = notifyMailService.RetrieveNumbersByUids(notifyStrategy.NotifyMailUids.Split(';'));
                notifyStrategy.AlarmEventUid = alarmEventList.SelectedValue;
                notifyStrategy.NotifyStrategyName = txtNotifyStrategyName.Text.Trim();
                notifyStrategy.BeginTime = txtBeginTime.SelectedTime.Value.ToString().Trim();
                notifyStrategy.EndTime = txtEndTime.SelectedTime.Value.ToString().Trim();
                notifyStrategy.NotifyCount = txtNotifyCount.Text.Trim() != "" ? Convert.ToInt32(txtNotifyCount.Text.Trim()) : 0;
                notifyStrategy.NotifySpan = txtNotifySpan.Text.Trim();
                notifyStrategy.BeginDate = beginDate.SelectedDate.ToString();
                notifyStrategy.EndDate = endDate.SelectedDate.FormatToString("yyyy-MM-dd 23:59:59");
                notifyStrategy.EnableOrNot = rbtnEnableOrNot.SelectedValue == "1" ? true : false;
                notifyStrategy.OrderByNum = tbxOrderByNum.Text.Trim() != "" ? Convert.ToInt32(tbxOrderByNum.Text.Trim()) : 0;
                notifyStrategy.Description = txtDescription.Text.Trim();
                notifyStrategy.DataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
                notifyStrategy.LastNotifyTime = (DateTime?)null;
                notifyStrategy.CreatDateTime = DateTime.Now;
                notifyStrategyService.Update(notifyStrategy);
                Alert("更新成功！");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
            }
            catch
            {
                Alert("更新失败！");
            }
        }

        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRad();
        }

        public void BindRad()
        {
            IQueryable<V_CodeMainItemEntity> siteTypeEntites = null;
            dvPoints.Style["display"] = "none";
            dvProper.Style["display"] = "none";
            ddlCityProper.Visible = false;
            switch (rbtnlType.SelectedValue)
            {
                case "CityProper":
                    siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "报警类型").Where(t => t.ItemText.Contains("重度污染") || t.ItemText.Contains("AQI"));
                    dvProper.Style["display"] = "normal";
                    ddlCityProper.Visible = true;
                    break;
                case "Port":
                    siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "报警类型").Where(t => !(t.ItemText.Contains("重度污染") || t.ItemText.Contains("AQI")));
                    //pointCbxRsm.Visible = true;
                    dvPoints.Style["display"] = "normal";
                    break;
                default:
                    siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "报警类型");
                    break;
            }
            alarmEventList.DataSource = siteTypeEntites;
            alarmEventList.DataTextField = "ItemText";
            alarmEventList.DataValueField = "ItemGuid";
            alarmEventList.DataBind();
            alarmEventList.Items.Insert(0, new RadComboBoxItem("", ""));

        }
    }
}