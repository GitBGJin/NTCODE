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
    public partial class AlarmNotifyAdd : SmartEP.WebUI.Common.BasePage
    {
        DictionaryService dicService = new DictionaryService();
        NotifyStrategyService notifyStrategyService = new NotifyStrategyService();
        NotifyNumbersService notifyNumberService = new NotifyNumbersService();
        NotifyMailsService notifyMailService = new NotifyMailsService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitControl();
                BindRad();
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

            //报警类型
            //IQueryable<V_CodeMainItemEntity> siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "报警类型");
            //alarmEventList.DataSource = siteTypeEntites;
            //alarmEventList.DataTextField = "ItemText";
            //alarmEventList.DataValueField = "ItemGuid";
            //alarmEventList.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //通知策略实体
                NotifyStrategyEntity notifyStrategy = new NotifyStrategyEntity();
                notifyStrategy.NotifyStrategyUid = Guid.NewGuid().ToString();
                notifyStrategy.NotifyGradeUid = notifyGradeList.SelectedValue;
                notifyStrategy.ApplicationUid = "airaaira-aira-aira-aira-airaairaaira";
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
                notifyStrategy.CurrNotifyCount = 0;
                notifyStrategy.CreatDateTime = DateTime.Now;
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
                notifyStrategyService.Add(notifyStrategy);
                Alert("添加成功！");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
            }
            catch
            {
                Alert("添加失败！");
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