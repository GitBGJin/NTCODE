using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Alarm;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Alarm
{
    public partial class AlarmAuditEdit : BasePage
    {
        /// <summary>
        /// 报警服务类
        /// </summary>
        CreateAlarmService g_CreateAlarmService = Singleton<CreateAlarmService>.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ViewState["AlarmUid"] = PageHelper.GetQueryString("AlarmUid");
                ViewState["type"] = PageHelper.GetQueryString("type");
                BindUI();
            }
        }

        /// <summary>
        /// 绑定控件
        /// </summary>
        private void BindUI()
        {
            if (ViewState["type"].ToString() == "single")
            {
                CreatAlarmEntity alarmEntity = g_CreateAlarmService.RetrieveFirstOrDefault(x => x.AlarmUid.Equals(ViewState["AlarmUid"].ToString()));
                if (alarmEntity != null)
                {
                    txtAlarmCon.Text = alarmEntity.Content;
                    DateTime MinDateTime = string.IsNullOrEmpty(Convert.ToString(alarmEntity.CreatDateTime))
                            ? alarmEntity.RecordDateTime.Value : alarmEntity.CreatDateTime.Value;
                    txtAlarmCon.Text += "【报警发布时间：" + MinDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "】";
                    txtDealTime.Text = alarmEntity.DealTime != null ? alarmEntity.DealTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                    txtDealContext.Text = alarmEntity.DealContent;
                    if (alarmEntity.AuditFlag == true)
                    {
                        dateAudit.SelectedDate = alarmEntity.AuditTime;
                        txtAuditContext.Text = alarmEntity.AuditContent;
                        alarmPLAudit.SelectedValue = alarmEntity.AuditMode;
                        btnEdit.Visible = false;
                    }
                    else
                    {
                        //dateAudit.SelectedDate = null;
                        dateAudit.SelectedDate = DateTime.Now;
                        txtAuditContext.Text = string.Empty;
                    }
                }
            }
            else
            {
                dateAudit.SelectedDate = DateTime.Now;
                alarmContent.Visible = false;
                alarmTime.Visible = false;
                alarmSuggest.Visible = false;
                pLAudit.Visible = false;
            }
        }

        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {
            //try
            //{
            //    CreatAlarmEntity alarmEntity = g_CreateAlarmService.RetrieveFirstOrDefault(x => x.AlarmUid.Equals(ViewState["AlarmUid"].ToString()));
            //    if (alarmEntity != null)
            //    {
            //        DateTime MinDateTime = alarmEntity.DealTime.Value;
            //        if (dateAudit.SelectedDate < MinDateTime)
            //        {
            //            Alert("审核时间不能小于处理时间");
            //            return;
            //        }
            //        alarmEntity.AuditFlag = true;
            //        alarmEntity.AuditTime = dateAudit.SelectedDate;
            //        alarmEntity.AuditContent = txtAuditContext.Text.Trim();
            //        alarmEntity.AuditMan = SessionHelper.Get("DisplayName");
            //        g_CreateAlarmService.Update(alarmEntity);
            //        Alert("审核成功！");
            //        //界面刷新
            //        RadScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "<script>RefreshParent();</script>", false);
            //    }
            //}
            //catch
            //{
            //    Alert("处理失败！");
            //    return;
            //}
            try
            {
                if (ViewState["type"] != null)
                {
                    if (ViewState["type"].ToString() == "mulite")
                    {
                        string[] alarmIds = ViewState["AlarmUid"].ToString().Split(';');
                        for (int i = 0; i < alarmIds.Length; i++)
                        {
                            CreatAlarmEntity alarmEntity = g_CreateAlarmService.RetrieveFirstOrDefault(x => x.AlarmUid.Equals(alarmIds[i]));
                            if (alarmEntity != null)
                            {
                                alarmEntity.AuditFlag = true;
                                alarmEntity.AuditTime = dateAudit.SelectedDate;
                                alarmEntity.AuditContent = txtAuditContext.Text.Trim();
                                alarmEntity.AuditMan = SessionHelper.Get("DisplayName");
                                alarmEntity.AuditMode = alarmPLAudit.SelectedValue;
                                g_CreateAlarmService.Update(alarmEntity);
                                Alert("审核成功！");
                                //界面刷新
                                RadScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "<script>RefreshParent();</script>", false);
                            }
                        }
                    }
                    else
                    {
                        if (alarmPLAudit.SelectedValue == "0")
                        {
                            CreatAlarmEntity alarmEntity = g_CreateAlarmService.RetrieveFirstOrDefault(x => x.AlarmUid.Equals(ViewState["AlarmUid"].ToString()));
                            if (alarmEntity != null)
                            {
                                //控制处理的最小时间
                                //DateTime MinDateTime = alarmEntity.DealTime.Value;
                                //if (dateAudit.SelectedDate < MinDateTime)
                                //{
                                //    Alert("审核时间不能小于处理时间");
                                //    return;
                                //}
                                alarmEntity.AuditFlag = true;
                                alarmEntity.AuditTime = dateAudit.SelectedDate;
                                alarmEntity.AuditContent = txtAuditContext.Text.Trim();
                                alarmEntity.AuditMan = SessionHelper.Get("DisplayName");
                                alarmEntity.AuditMode = alarmPLAudit.SelectedValue;
                                g_CreateAlarmService.Update(alarmEntity);
                                Alert("审核成功！");
                                //界面刷新
                                RadScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "<script>RefreshParent();</script>", false);
                            }
                        }
                        else
                        {
                            CreatAlarmEntity alarmEntity = g_CreateAlarmService.RetrieveFirstOrDefault(x => x.AlarmUid.Equals(ViewState["AlarmUid"].ToString()));
                            if (alarmEntity != null)
                            {

                                //控制处理的最小时间
                                //DateTime MinDateTime = alarmEntity.DealTime.Value;
                                //if (dateAudit.SelectedDate < MinDateTime)
                                //{
                                //    Alert("审核时间不能小于处理时间");
                                //    return;
                                //}
                                if (alarmEntity.RecordDateTime == null)
                                {
                                    Alert("无法进行批量处理，该条报警信息的生成时间为空");
                                    return;
                                }

                                DataView dv = new CreateAlarmService().GetAuditPLAlarmInfo(alarmEntity.ApplicationUid, alarmEntity.MonitoringPointUid, alarmEntity.AlarmEventUid, alarmEntity.ItemName, alarmPLAudit.SelectedValue, alarmEntity.CreatDateTime);
                                if (dv != null && dv.Count > 0)
                                {
                                    if (alarmPLAudit.SelectedValue == "1")
                                    {
                                        if (alarmEntity.CreatDateTime < Convert.ToDateTime(dv[0]["CreatDateTime"]))
                                        {
                                            Alert("无法进行批量处理，该条数据不是当前测点、报警类型、因子当天的最新数据");
                                            return;
                                        }
                                    }
                                    if (alarmPLAudit.SelectedValue == "2")
                                    {
                                        if (alarmEntity.CreatDateTime < Convert.ToDateTime(dv[0]["CreatDateTime"]))
                                        {
                                            Alert("无法进行批量处理，该条数据不是当前测点、报警类型、因子目前为止的最新数据");
                                            return;
                                        }
                                    }
                                    for (int i = 0; i < dv.Count; i++)
                                    {
                                        CreatAlarmEntity alarmEntity1 = g_CreateAlarmService.RetrieveFirstOrDefault(x => x.AlarmUid.Equals(dv[i]["AlarmUid"].ToString()));
                                        alarmEntity1.AuditFlag = true;
                                        alarmEntity1.AuditTime = dateAudit.SelectedDate;
                                        alarmEntity1.AuditContent = txtAuditContext.Text.Trim();
                                        alarmEntity1.AuditMan = SessionHelper.Get("DisplayName");
                                        alarmEntity1.AuditMode = alarmPLAudit.SelectedValue;
                                        g_CreateAlarmService.Update(alarmEntity1);
                                    }
                                    Alert("审核成功！");
                                    //界面刷新
                                    RadScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "<script>RefreshParent();</script>", false);
                                }
                                else
                                {
                                    Alert("无可处理数据！");
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                Alert("审核失败！");
                return;
            }
        }
    }
}