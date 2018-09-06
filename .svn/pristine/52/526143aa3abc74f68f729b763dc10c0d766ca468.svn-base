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
    public partial class AlarmHandle : BasePage
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
                    if (alarmEntity.DealFlag == true)
                    {
                        dtpHandle.SelectedDate = alarmEntity.DealTime;
                        txtDesc.Text = alarmEntity.DealContent;
                        alarmPLHandle.SelectedValue = alarmEntity.DealMode;
                    }
                    else
                    {
                        //dtpHandle.SelectedDate = null;
                        dtpHandle.SelectedDate = DateTime.Now;
                        txtDesc.Text = string.Empty;
                    }
                    if (alarmEntity.AuditFlag == true)
                    {
                        btnEdit.Visible = false;
                    }
                }
            }
            else
            {
                dtpHandle.SelectedDate = DateTime.Now;
                alarmContent.Visible = false;
                pLDeal.Visible = false;
            } 
        }

        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["type"] != null)
                {
                    if (ViewState["type"].ToString() == "mulite")
                    {
                        string[] alarmIds = ViewState["AlarmUid"].ToString().Split(';');
                        for(int i=0;i<alarmIds.Length;i++)
                        {
                            CreatAlarmEntity alarmEntity = g_CreateAlarmService.RetrieveFirstOrDefault(x => x.AlarmUid.Equals(alarmIds[i]));
                            if (alarmEntity != null)
                            {
                                alarmEntity.DealFlag = true;
                                alarmEntity.DealTime = dtpHandle.SelectedDate;
                                alarmEntity.DealContent = txtDesc.Text.Trim();
                                alarmEntity.DealMan = SessionHelper.Get("DisplayName");
                                alarmEntity.DealMode = alarmPLHandle.SelectedValue;
                                g_CreateAlarmService.Update(alarmEntity);
                                Alert("处理成功！");
                                //界面刷新
                                RadScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "<script>RefreshParent();</script>", false);
                            }
                        }
                    }
                    else
                    {
                        if (alarmPLHandle.SelectedValue == "0")//单个处理
                        {
                            CreatAlarmEntity alarmEntity = g_CreateAlarmService.RetrieveFirstOrDefault(x => x.AlarmUid.Equals(ViewState["AlarmUid"].ToString()));
                            if (alarmEntity != null)
                            {
                                //控制处理的最小时间
                                //DateTime MinDateTime = string.IsNullOrEmpty(Convert.ToString(alarmEntity.CreatDateTime))
                                //    ? alarmEntity.RecordDateTime.Value : alarmEntity.CreatDateTime.Value;
                                //if (dtpHandle.SelectedDate < MinDateTime)
                                //{
                                //    Alert("处理时间不能小于报警发布时间");
                                //    return;
                                //}
                                alarmEntity.DealFlag = true;
                                alarmEntity.DealTime = dtpHandle.SelectedDate;
                                alarmEntity.DealContent = txtDesc.Text.Trim();
                                alarmEntity.DealMan = SessionHelper.Get("DisplayName");
                                alarmEntity.DealMode = alarmPLHandle.SelectedValue;
                                g_CreateAlarmService.Update(alarmEntity);
                                Alert("处理成功！");
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
                                //DateTime MinDateTime = string.IsNullOrEmpty(Convert.ToString(alarmEntity.CreatDateTime))
                                //    ? alarmEntity.RecordDateTime.Value : alarmEntity.CreatDateTime.Value;
                                //if (dtpHandle.SelectedDate < MinDateTime)
                                //{
                                //    Alert("处理时间不能小于报警发布时间");
                                //    return;
                                //}
                                if (alarmEntity.RecordDateTime == null)
                                {
                                    Alert("无法进行批量处理，该条报警信息的生成时间为空");
                                    return;
                                }

                                DataView dv = new CreateAlarmService().GetPLAlarmInfo(alarmEntity.ApplicationUid, alarmEntity.MonitoringPointUid, alarmEntity.AlarmEventUid, alarmEntity.ItemName, alarmPLHandle.SelectedValue, alarmEntity.CreatDateTime);
                                if (dv != null && dv.Count > 0)
                                {
                                    if (alarmPLHandle.SelectedValue == "1")//处理当天数据
                                    {
                                        if (alarmEntity.CreatDateTime < Convert.ToDateTime(dv[0]["CreatDateTime"]))
                                        {
                                            Alert("无法进行批量处理，该条数据不是当前测点、报警类型、因子当天的最新数据");
                                            return;
                                        }
                                    }
                                    if (alarmPLHandle.SelectedValue == "2")//处理该因子该测点该报警类型所有数据
                                    {
                                        if (alarmEntity.CreatDateTime < Convert.ToDateTime(dv[0]["CreatDateTime"]))
                                        {
                                            Alert("无法进行批量处理，该条数据不是当前测点、报警类型、因子目前为止的最新数据");
                                            return;
                                        }
                                    }
                                    for (int i = 0; i < dv.Count; i++)//更新CreatAlarm表里的处理状态
                                    {
                                        CreatAlarmEntity alarmEntity1 = g_CreateAlarmService.RetrieveFirstOrDefault(x => x.AlarmUid.Equals(dv[i]["AlarmUid"].ToString()));
                                        alarmEntity1.DealFlag = true;
                                        alarmEntity1.DealTime = dtpHandle.SelectedDate;
                                        alarmEntity1.DealContent = txtDesc.Text.Trim();
                                        alarmEntity1.DealMan = SessionHelper.Get("DisplayName");
                                        alarmEntity1.DealMode = alarmPLHandle.SelectedValue;
                                        g_CreateAlarmService.Update(alarmEntity1);
                                    }
                                    Alert("处理成功！");
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
                Alert("处理失败！");
                return;
            }
        }
    }
}