using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Alarm;
using SmartEP.Service.Frame;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Dock
{
    public partial class Dashboard : BasePage
    {
        /// <summary>
        /// 报警通知策略
        /// </summary>
        private NotifyStrategyService g_NotifyStrategyService = Singleton<NotifyStrategyService>.GetInstance();
        /// <summary>
        /// 报警信息
        /// </summary>
        private CreateAlarmService g_CreateAlarmService = Singleton<CreateAlarmService>.GetInstance();

        /// <summary>
        /// 字典服务
        /// </summary>
        private DictionaryService g_DicionaryService = Singleton<DictionaryService>.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitAlarmNotification();
                gridAlarm.DataBind();
                if (gridAlarm.Items.Count > 0)
                {
                    AlarmNotification.Show();
                }
            }
        }

        protected void gridAlarm_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            InitAlarmNotification();
        }

        /// <summary>
        /// 报警信息弹窗处理
        /// </summary>
        private void InitAlarmNotification()
        {
            //string alarmEventUidHsp = g_DicionaryService.GetValueByText(SmartEP.Service.Core.Enums.DictionaryType.AMS, "报警类型", "超上限");
            //string alarmEventUidLsp = g_DicionaryService.GetValueByText(SmartEP.Service.Core.Enums.DictionaryType.AMS, "报警类型", "超下限");
            //IQueryable<NotifyStrategyEntity> notifyList = g_NotifyStrategyService.RetrieveAll(SmartEP.Core.Enums.EnumMapping.GetDesc(ApplicationType.Air));
            //notifyList = notifyList.Where(x => x.EnableOrNot != null && x.EnableOrNot.Value.Equals(true)
            //    && (x.AlarmEventUid.Equals(alarmEventUidHsp) || x.AlarmEventUid.Equals(alarmEventUidLsp))
            //    && x.NotifyNumberUids.Contains(this.Session["UserGuid"].ToString()));
            string strWhere = string.Empty;
            //foreach (NotifyStrategyEntity notify in notifyList)
            //{
            //    if (!string.IsNullOrEmpty(strWhere))
            //        strWhere += " OR ( ";
            //    else
            //        strWhere += " ( ";
            //    strWhere += string.Format(" AlarmEventUid='{0}' ", notify.AlarmEventUid);
            //    string[] pointGuids = notify.EffectSubject.Split(';');
            //    string strPoints = string.Empty;
            //    foreach (string pointGuid in pointGuids)
            //    {
            //        strPoints += string.Format("'{0}',", pointGuid);
            //    }
            //    strPoints = strPoints.Trim(',');
            //    if (!string.IsNullOrEmpty(strPoints))
            //        strWhere += string.Format(" AND MonitoringPointUid in ({0}) ", strPoints);
            //    strWhere += " ) ";
            //}
            DateTime dtPre = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (string.IsNullOrEmpty(strWhere))
            {
                strWhere = " 1=1 ";
            }
            strWhere += string.Format(" AND CreatDateTime>'{0}' AND ApplicationUid='{1}'", dtPre.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm:ss"), "airaaira-aira-aira-aira-airaairaaira");
            int recordTotal = 1;
            var data = g_CreateAlarmService.GetGridViewPager(999999, 1, strWhere, out recordTotal);
            gridAlarm.DataSource = data;
        }
    }
}