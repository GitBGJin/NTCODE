using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Core.Interfaces;
using System.Configuration;
using System.Drawing;


namespace SmartEP.WebUI.Pages.EnvAir.RemoteControl
{
    public partial class DeviationScheduler : SmartEP.WebUI.Common.BasePage
    {
        CalibrationReportService _calibrationService = new CalibrationReportService();
        string[] factors = { "a21026", "a21003", "a21005", "a05024" };
        static List<IPoint> PointList = null;
        private void Page_Load(object sender, EventArgs e)
        {
            //RadToolTipManager2.TargetControls.Clear();
            if (!IsPostBack)
            {
                #region 初始化
                RadDatePickerBegin.DateInput.DisplayDateFormat = "yyyy-MM-dd";
                RadDatePickerBegin.SelectedDate = System.DateTime.ParseExact(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01"), "yyyy-MM-dd", null);
                RadDatePickerEnd.DateInput.DisplayDateFormat = "yyyy-MM-dd";
                RadDatePickerEnd.SelectedDate = System.DateTime.ParseExact(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1).ToString("yyyy-MM-dd"), "yyyy-MM-dd", null);
                #endregion

                RadSchedulerDeviation.SelectedDate = DateTime.Now.AddDays(-1);
                RadSchedulerDeviation.SelectedView = SchedulerViewType.MonthView;
                //BindData(0);
                //RadToolTipManager2.TargetControls.Add("RadScheduler1_SelectedDateCalendar_Top", "", true);
                DetalToolTip.TargetControls.Add("RadScheduler1_aa", "", true);
            }


        }

        #region 绑定数据
        private void BindData(int flag)
        {
            #region 绑定日程数据
            #region 时间判断
            DateTime begin = RadSchedulerDeviation.VisibleRangeStart;
            DateTime end = RadSchedulerDeviation.VisibleRangeEnd;
            if (flag == -1)
            {
                if (RadSchedulerDeviation.SelectedView == SchedulerViewType.MonthView)
                {
                    begin = Convert.ToDateTime(RadSchedulerDeviation.VisibleRangeStart.ToString("yyyy-MM-01 00:00")).AddMonths(-1);
                }
                else if (RadSchedulerDeviation.SelectedView == SchedulerViewType.DayView)
                {
                    begin = RadSchedulerDeviation.VisibleRangeStart.AddDays(-1);
                }
                else if (RadSchedulerDeviation.SelectedView == SchedulerViewType.WeekView)
                {
                    begin = RadSchedulerDeviation.VisibleRangeStart.AddDays(-7);
                }
            }
            else if (flag == 1)
            {
                if (RadSchedulerDeviation.SelectedView == SchedulerViewType.MonthView)
                {
                    end = RadSchedulerDeviation.VisibleRangeEnd.AddMonths(2);
                }
                else if (RadSchedulerDeviation.SelectedView == SchedulerViewType.DayView)
                {
                    end = RadSchedulerDeviation.VisibleRangeEnd.AddDays(1);
                }
                else if (RadSchedulerDeviation.SelectedView == SchedulerViewType.WeekView)
                {
                    end = RadSchedulerDeviation.VisibleRangeEnd.AddDays(7);
                }
            }
            else if (flag == 3)
            {
                begin = Convert.ToDateTime(RadDatePickerBegin.SelectedDate);
                RadSchedulerDeviation.SelectedDate = begin;
                end = Convert.ToDateTime(RadDatePickerEnd.SelectedDate);
            }
            #endregion

            #region 绑定数据
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            if (portIds != null && !portIds[0].Equals(""))
            {
                DataView dv = new DataView();
                if (RadSchedulerDeviation.SelectedView == SchedulerViewType.MonthView)
                    dv = _calibrationService.GetCalibrationDayData(begin, end, portIds, factors);
                else
                    dv = _calibrationService.GetCalibrationHourData(begin, end, portIds, factors);
                RadSchedulerDeviation.DataSource = dv;
                RadSchedulerDeviation.DataKeyField = "id";
                RadSchedulerDeviation.DataSubjectField = "Pointid";
                RadSchedulerDeviation.DataStartField = "startTime";
                RadSchedulerDeviation.DataEndField = "endTime";
                RadSchedulerDeviation.DataDescriptionField = "Description";
                RadSchedulerDeviation.HoursPanelTimeFormat = "H:mm";
            }
            #endregion
            #endregion
        }
        #endregion

        #region 事件
        /// <summary>
        /// 日历切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadSchedulerDeviation_NavigationCommand(object sender, SchedulerNavigationCommandEventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(e.SelectedDate) != Convert.ToDateTime("0001-01-01 00:00"))
                    RadSchedulerDeviation.SelectedDate = e.SelectedDate;
                int flag = 2;
                if (e.Command.ToString().Contains("Next"))
                {
                    flag = 1;
                }
                else if (e.Command.ToString().Contains("Previous"))
                {
                    flag = -1;
                }

                if (e.Command.ToString().Contains("Day"))
                    RadSchedulerDeviation.SelectedView = SchedulerViewType.DayView;
                else if (e.Command.ToString().Contains("Week"))
                    RadSchedulerDeviation.SelectedView = SchedulerViewType.WeekView;
                else if (e.Command.ToString().Contains("Month"))
                    RadSchedulerDeviation.SelectedView = SchedulerViewType.MonthView;
                else if (e.Command.ToString().Contains("Multi"))
                    RadSchedulerDeviation.SelectedView = SchedulerViewType.MultiDayView;
                BindData(flag);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 日历加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadSchedulerDeviation_Load(object sender, EventArgs e)
        {
            BindData(0);
        }

        /// <summary>
        /// 日历节点绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadSchedulerDeviation_AppointmentDataBound(object sender, SchedulerEventArgs e)
        {
            if (PointList == null)
                PointList = pointCbxRsm.GetPoints();
            //object aa = e.Appointment.Resources.GetResource("Point", 1);
            string Subject = PointList.Where(x => x.PointID == e.Appointment.Subject).FirstOrDefault().PointName;
            try
            {
                e.Appointment.ForeColor = GetPointColor(Convert.ToInt32(e.Appointment.Subject));
            }
            catch
            {
            }
            e.Appointment.Subject = Subject;
        }

        /// <summary>
        /// 测点选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadCBoxPoint_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //if (RadCBoxPoint.CheckedItems.Count <= 0)
            //    RunJScript("alert('请选择测点！');");
            //else
            BindData(2);
        }

        /// <summary>
        /// 时间选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadDatePickerBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindData(3);
        }

        /// <summary>
        /// 站点因子关联
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
            PointList = pointCbxRsm.GetPoints();
            //points = pointCbxRsm.GetPoints();
            //InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
            //IList<string> list = new List<string>();
            //string[] factor;
            //string factors = string.Empty;
            //foreach (IPoint point in points)
            //{
            //    IQueryable<PollutantCodeEntity> p = m_InstrumentChannelService.RetrieveChannelListByPointUid(point.PointGuid);
            //    list = list.Union(p.Select(t => t.PollutantName)).ToList();
            //}
            //factor = list.ToArray();
            //foreach (string f in factor)
            //{
            //    factors += f + ";";
            //}
            //factorCbxRsm.SetFactorValuesFromNames(factors);
        }
        #endregion

        #region 方法
        #region 获取日历控件数据颜色
        public Color GetPointColor(int PointID)
        {
            Color color = new System.Drawing.Color();
            try
            {
                List<IPoint> PointList = pointCbxRsm.GetPoints();
                int index = PointList.Select(x => x.PointID).ToList().IndexOf(PointID.ToString());
                if (index < 0)
                    color = Color.FromName("#000000");
                else
                    color = Color.FromName(ConfigurationManager.AppSettings["SchedulerColors"].ToString().Split(',')[index]);
            }
            catch
            {
                color = Color.FromName("#000000");
            }

            return color;
        }


        #endregion

        #region 提示框刷新
        protected void RadSchedulerDeviation_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
        {
            if (e.Appointment.Visible && !IsAppointmentRegisteredForTooltip(e.Appointment))
            {
                string id = e.Appointment.ID.ToString();

                foreach (string domElementID in e.Appointment.DomElements)
                {
                    DetalToolTip.TargetControls.Add(domElementID, id, true);
                }
            }
        }
        private bool IsAppointmentRegisteredForTooltip(Appointment apt)
        {
            foreach (ToolTipTargetControl targetControl in DetalToolTip.TargetControls)
            {
                if (apt.DomElements.Contains(targetControl.TargetControlID))
                {
                    return true;
                }
            }

            return false;
        }
        protected void DetalToolTip_AjaxUpdate(object sender, ToolTipUpdateEventArgs e)
        {

            if (e.TargetControlID != null)
            {
                try
                {
                    int aptId;
                    Appointment apt;
                    string[] targetIdArray = e.TargetControlID.Split('_');
                    aptId = int.Parse(targetIdArray[targetIdArray.Length - 2]);
                    apt = RadSchedulerDeviation.Appointments[aptId];

                    AppointmentToolTip toolTip = (AppointmentToolTip)LoadControl("AppointmentToolTip.ascx");
                    toolTip.ID = "UcAppointmentTooltip1";
                    toolTip.TargetAppointment = apt;
                    e.UpdatePanel.ContentTemplateContainer.Controls.Add(toolTip);
                }
                catch
                {

                }
            }
            else
            {
                return;
            }

        }
        #endregion
        #endregion
    }
}