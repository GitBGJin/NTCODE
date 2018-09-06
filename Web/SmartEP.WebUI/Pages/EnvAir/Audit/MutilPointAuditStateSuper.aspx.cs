using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.DataAuditing.AuditBaseInfo;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Controls;
using SmartEP.WebControl.CbxRsm;
using SmartEP.Core.Enums;
using Telerik.Web.UI.Calendar;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System.Threading;
using System.Configuration;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.Frame;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.AutoMonitoring.common;
using SmartEP.Core.Generic;
using log4net;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    /// <summary>
    /// 名称：MutilPointAuditStateSuper.aspx.cs
    /// 创建人：
    /// 创建日期：
    /// 维护人员：
    /// 最新维护人员：吕云
    /// 最新维护日期：2017-6-20
    /// 功能摘要：超级站审核
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class MutilPointAuditStateSuper : SmartEP.WebUI.Common.BasePage
    {
        #region 属性
        AirAuditMonitoringPointService pointAirService = new AirAuditMonitoringPointService();//点位接口 空气
        WaterAuditMonitoringPointService pointWaterService = new WaterAuditMonitoringPointService();//点位接口 地表水
        AuditMonitoringPointPollutantService pollutantService = new AuditMonitoringPointPollutantService();//因子接口
        AuditDataService auditDataService = new AuditDataService();//审核数据接口
        AuditLogService auditLogService = new AuditLogService();//审核日志接口
        AuditOperatorService operatorService = new AuditOperatorService();//审核操作接口
        public static IQueryable<PointPollutantInfo> pollutantList = null;
        //CbxRsmControl myRSM = new CbxRsmControl();
        static DateTime currentBegin = DateTime.Now;
        static DateTime currentEnd = DateTime.Now;
        MonitoringInstrumentService instrumentService = new MonitoringInstrumentService();
        MonitoringBusinessModel MonitoringBusinessModel = new MonitoringBusinessModel();
        BaseDataModel BaseDataModel = new BaseDataModel();
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        /// <summary>
        /// 审核因子配置接口
        /// </summary>
        SmartEP.Service.AutoMonitoring.common.AuditMonitoringPointService g_AuditMonitoringPointService = Singleton<SmartEP.Service.AutoMonitoring.common.AuditMonitoringPointService>.GetInstance();
        /// <summary>
        /// 应用程序Uid
        /// </summary>
        string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(SmartEP.Core.Enums.ApplicationValue.Air);
        /// <summary>
        /// 超级站监站点状态类型
        /// </summary>
        int PointType = 1;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["pointType"] != null)
                    ViewState["pointType"] = Request.QueryString["pointType"];
                else
                    ViewState["pointType"] = "0";
                if (Request.QueryString["app"] != null && Request.QueryString["app"].Equals("1"))
                    Session["applicationUID"] = "watrwatr-watr-watr-watr-watrwatrwatr";
                else
                    Session["applicationUID"] = "airaaira-aira-aira-aira-airaairaaira";
                if (Session["UserGuid"] == null)
                    Session["UserGuid"] = "4ce5bed9-78bd-489f-8b3f-a830098759c4";
                ViewState["PointID"] = Request.QueryString["PointID"] != null ? Request.QueryString["PointID"].ToString() : "";
                InitControl();//初始化控件
            }
        }

        #region 初始化控件
        private void InitControl()
        {
            if (Request.QueryString["dtBegin"] == null) Session["FactorCode"] = null;//首次加载界面情况因子session
            RadDatePickerBegin.SelectedDate = Request.QueryString["dtBegin"] != null ? Convert.ToDateTime(Request.QueryString["dtBegin"]) : DateTime.Now.Date.AddDays(-7);
            RadDatePickerEnd.SelectedDate = Request.QueryString["dtEnd"] != null ? Convert.ToDateTime(Request.QueryString["dtEnd"]) : DateTime.Now.Date.AddDays(-1);
            PointBind();//初始化点位
            PollutantBind();//初始化因子
            RadCalendar1.FocusedDate = Request.QueryString["dtBegin"] != null ? Convert.ToDateTime(Request.QueryString["dtBegin"]) : DateTime.Now;
            radioPollutant.SelectedIndex = 0;
            string InstrumentGuid = radioPollutant.Items[0].Value;
            string PointUid = radioPoint.SelectedValue.ToString().Split('/')[1];
            string PollutantName = "", PollutantCode = "", PollutantUid = "", PollutantShow = "", PollutantRead = "";

            //审核配置因子
            IQueryable<AuditMonitoringPointPollutantEntity> AuditLists = g_AuditMonitoringPointService.DetailRetrieve(x => PointUid.Equals(x.AuditMonitoringPointEntity.MonitoringPointUid) && x.AuditMonitoringPointEntity.ApplicationUid == ApplicationUid && x.AuditMonitoringPointEntity.PointType == PointType);
            List<AuditMonitoringPointPollutantEntity> AuditData = new List<AuditMonitoringPointPollutantEntity>();
            if (AuditLists != null)
            {
                foreach (AuditMonitoringPointPollutantEntity audit in AuditLists)
                {
                    if (PointUid == audit.AuditMonitoringPointEntity.MonitoringPointUid)
                    {
                        if (!AuditData.Contains(audit))
                        {
                            AuditData.Add(audit);
                        }
                    }
                }
            }
            if (AuditData != null)
            {
                foreach (AuditMonitoringPointPollutantEntity audit in AuditData)
                {
                    PollutantShow += audit.PollutantCode + ";";
                    if (!string.IsNullOrEmpty(Convert.ToString(audit.ReadOnly)) && (Boolean)audit.ReadOnly)
                    {
                        PollutantRead += audit.PollutantCode + ";";
                    }
                }
            }
            string[] PollutantShowList = PollutantShow.Split(';');
            string[] PollutantReadList = PollutantRead.Split(';');
            string selCode = "";
            string selName = "";
            string isEdit = "";
            string PollutantDecimalNum = "";
            string PollutantUnit = "";
            AirPollutantService airservice = new AirPollutantService();
            IQueryable<InstrumentChannelEntity> InstrCList = BaseDataModel.InstrumentChannelEntities;


            IQueryable<PollutantCodeEntity> pollutantQueryable = from item in airservice.RetrieveListByUseOrNot(true)
                                                                 join ic in InstrCList on item.PollutantCode equals ic.PollutantCode
                                                                 where (PollutantShowList.Contains(item.PollutantCode) && ic.InstrumentUid.Equals(InstrumentGuid))
                                                                 select item;
            PollutantCodeEntity[] pEntity = pollutantQueryable.ToArray();
            for (int i = 0; i < pEntity.Length; i++)
            {
                selCode += selCode == "" ? pEntity[i].PollutantCode : ";" + pEntity[i].PollutantCode;
                selName += selName == "" ? pEntity[i].PollutantName : ";" + pEntity[i].PollutantName;
                PollutantDecimalNum += PollutantDecimalNum == "" ? pEntity[i].DecimalDigit.ToString() : ";" + pEntity[i].DecimalDigit.ToString();

                DictionaryService dservice = new DictionaryService();
                string Unit = dservice.GetTextByGuid(Service.Core.Enums.DictionaryType.AMS, "计量单位", pEntity[i].MeasureUnitUid);
                PollutantUnit += Unit + ";";
                if (PollutantReadList.Contains(pEntity[i].PollutantCode))
                {
                    isEdit += "1;";
                }
                else
                {
                    isEdit += "0;";
                }
            }
            Session["InstrumentUid"] = InstrumentGuid;
            Session["FactorCodeSuper"] = selCode;
            Session["FactorNameSuper"] = selName;
            Session["isEditSuper"] = isEdit;
            Session["PollutantDecimalNumSuper"] = PollutantDecimalNum;
            Session["PollutantUnitSuper"] = !PollutantUnit.Equals("") ? PollutantUnit.Substring(0, PollutantUnit.Length - 1) : "";


            InitRadScheduler(true);//初始化日历控件
            string pointids = GetPointSelectedValue();
            #region 清空Session
            if (ViewState["PointID"].ToString().Equals(""))
            {
                Session["FactorCodeSuper"] = "";
                Session["FactorNameSuper"] = "";
                Session["isEditSuper"] = "";
                Session["PollutantDecimalNumSuper"] = "";
                Session["PollutantUnitSuper"] = "";
            }
            #endregion
        }

        #region 点位绑定
        private void PointBind()
        {
            IQueryable<MonitoringPointEntity> pointList = null;
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]))
                pointList = pointAirService.RetrieveAirByAuditType(ViewState["pointType"].ToString(), Session["UserGuid"].ToString()).Distinct();
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))
                pointList = pointWaterService.RetrieveWaterByAuditType(ViewState["pointType"].ToString(), Session["UserGuid"].ToString());

            foreach (MonitoringPointEntity PEntity in pointList)
            {
                ListItem item = new ListItem();
                item.Value = PEntity.PointId + "/" + PEntity.MonitoringPointUid;
                item.Text = PEntity.MonitoringPointName;
                ListItem lt = new ListItem(item.Text, item.Value);
                radioPoint.Items.Add(lt);
            }

            string Pointids = "";
            string PointUids = "";
            foreach (ListItem item in radioPoint.Items)
            {
                if (ViewState["PointID"].ToString().Equals(item.Value.Split('/')[0]))
                    item.Selected = true;
            }

            if (radioPoint.Items.Count > 0 && radioPoint.SelectedItem == null)
            {
                radioPoint.Items[0].Selected = true;
                Pointids = radioPoint.SelectedValue.ToString().Split('/')[0];//站点单选，故Pointids只有一个PointID
                PointUids = radioPoint.SelectedValue.ToString().Split('/')[1];
            }
            ViewState["PointID"] = Pointids;
            ViewState["PointUid"] = PointUids;
        }
        #endregion

        #region 因子绑定
        private void PollutantBind()
        {
            string PointID = radioPoint.SelectedValue.ToString().Split('/')[0];
            string PointUid = radioPoint.SelectedValue.ToString().Split('/')[1];
            //获取仪器
            IQueryable<InstrumentEntity> instrumentList = instrumentService.RetrieveListByPointUid(PointUid);
            //instrumentList = instrumentList.Where(p => p.RowGuid != "6cd5c158-8a79-4540-a8b1-2a062759c9a0");//剔除超级站常规参数

            //foreach (InstrumentEntity instrument in instrumentList)
            //{
            //    if (instrument.RowGuid != "6cd5c158-8a79-4540-a8b1-2a062759c9a0")//剔除超级站常规参数
            //    {
            //        ListItem item = new ListItem();
            //        item.Value = instrument.RowGuid;
            //        item.Text = instrument.InstrumentName;
            //        ListItem lt = new ListItem(item.Text, item.Value);
            //        radioPollutant.Items.Add(lt);
            //    }
            //}
            radioPollutant.DataSource = instrumentList;
            radioPollutant.DataTextField = "InstrumentName";
            radioPollutant.DataValueField = "RowGuid";
            radioPollutant.DataBind();

            //string SuperCategoryConfig = ConfigurationManager.AppSettings["SuperCategory"].ToString();
            //string[] SuperCategory = SuperCategoryConfig.Split(';');
            //DataTable dt = new DataTable();
            //dt.Columns.Add("text", typeof(string));
            //dt.Columns.Add("value", typeof(string));
            //foreach (string SuperCategoryItem in SuperCategory)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["text"] = SuperCategoryItem.Split(':')[0];
            //    dr["value"] = SuperCategoryItem.Split(':')[1];
            //    dt.Rows.Add(dr);
            //}
            //radioPollutant.DataSource = dt;
            //radioPollutant.DataTextField = "text";
            //radioPollutant.DataValueField = "value";
            //radioPollutant.DataBind();

            if (Session["InstrumentUid"] == null)
            {
            }
            else
            {
                string InstrumentUid = Session["InstrumentUid"].ToString();
                for (int i = 0; i < radioPollutant.Items.Count; i++)
                {
                    if (radioPollutant.Items[i].Value == InstrumentUid)
                        radioPollutant.Items[i].Selected = true;
                }
            }
        }
        #endregion

        #region 日历控件绑定
        private void InitRadScheduler(Boolean IsReset)
        {
            DateTime beginDate, endDate;
            beginDate = RadCalendar1.CalendarView.ViewStartDate;
            endDate = beginDate.AddDays(41);

            if (IsReset)
            {
                this.RadCalendar1.SpecialDays.Clear();
            }
            string InsId = Session["InstrumentUid"].ToString();
            //根据仪器uid获取因子类型uid
            string uid = string.Empty;
            switch (InsId)
            {
                case "56dd6e9b-4c8f-4e67-a70f-b6a277cb44d7":
                    uid = "e5b6d666-24d1-473a-b15a-33a36245d44f";
                    break;
                case "6e4aa38a-f68b-490b-9cd7-3b92c7805c2d":
                    uid = "14b38adf-d899-4362-99ff-6a9e9dd35485";
                    break;
                case "3745f768-a789-4d58-9578-9e41fde5e5f0":
                    uid = "fbc6dyta-d06c-678g-b5y0-89b12be5bda3";
                    break;
                case "1589850e-0df1-4d9d-b508-4a77def158ba":
                    uid = "5575a0e1-d948-4566-9dcd-4b4767688add";
                    break;
                case "a6b3d80c-8281-4bc6-af47-f0febf568a5c":
                    uid = "59f02681-093f-48f0-9cac-ac59acd7038f";
                    break;
                case "da4f968f-cc6e-4fec-8219-6167d100499d":
                    uid = "aabe91e0-29a4-427c-becc-0b29f1224422";
                    break;
                case "9ef57f3c-8cce-4fe3-980f-303bbcfde260":
                    uid = "da92c7c1-4932-4007-a6d5-2866aa8c63f1";
                    break;
                case "6cd5c158-8a79-4540-a8b1-2a062759c9a0":
                    uid = "339B72C4-7295-4D31-B9EB-23342CB3697E";
                    break;
            }
            List<PointPollutantInfo> PointPollutantInfos = pollutantService.RetrieveSiteMapPollutantList(204, "airaaira-aira-aira-aira-airaairaaira", Session["UserGuid"].ToString()).Where(p => p.PGuid != null && p.PGuid.Equals(uid)).ToList<PointPollutantInfo>();
            string[] factors = new string[PointPollutantInfos.Count];
            for (int i = 0; i < PointPollutantInfos.Count; i++)
            {
                factors[i] = PointPollutantInfos[i].PID;
            }
            if (factors.Length <= 0) return;

            for (DateTime myDateTime = beginDate; myDateTime <= endDate; myDateTime = myDateTime.AddDays(1))  //循环日历控件日期
            {
                string status = "";
                string Pointids = GetPointSelectedValue();
                if (!Pointids.Equals(""))
                {
                    status = auditLogService.RerieveAuditStateNew(Pointids.Split(';'), myDateTime, myDateTime, Session["applicationUID"].ToString(), ViewState["pointType"].Equals("2") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira") ? "1" : "0", factors, InsId);
                }
                #region 填充数据
                RadCalendarDay myDay = new RadCalendarDay();
                myDay.Date = myDateTime;
                myDay.TemplateID = "Adt" + status;
                //myDay.Repeatable = RecurringEvents.DayInMonth;
                //myDay.ToolTip = "fdfd";
                this.RadCalendar1.SpecialDays.Add(myDay);
                #endregion
            }

        }
        #endregion
        #endregion

        #region 事件
        /// <summary>
        /// 日历切换日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadCalendar1_DefaultViewChanged(object sender, Telerik.Web.UI.Calendar.DefaultViewChangedEventArgs e)
        {
            InitRadScheduler(false);//日历绑定
        }

        /// <summary>
        /// 绑定审核日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditLog_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string Pointids = GetPointSelectedValue();

            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
                gridAuditLog.DataSource = auditLogService.RerieveAirLog(Pointids.Split(';'), Session["applicationUID"].ToString(), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1));
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"].ToString()))//地表水
                gridAuditLog.DataSource = auditLogService.RerieveWaterLog(Pointids.Split(';'), Session["applicationUID"].ToString(), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1));

        }

        /// <summary>
        /// 删除审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteAudit_Click(object sender, EventArgs e)
        {
            string Pointids = GetPointSelectedValue();
            if (!Pointids.Equals(""))
            {

                bool result = false;
                if (ViewState["pointType"].Equals("0") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira"))//国控点数据重载
                    result = operatorService.DeleteAudit(Pointids.Replace(";", ","), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1), "", "", "", "重新加载", "重新加载", "");
                else if (ViewState["pointType"].Equals("2") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira"))//超级站数据重载
                    result = operatorService.DeleteAuditSuper(Session["applicationUID"].ToString(), Pointids.Split(';'), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1), "", "", "", "重新加载", "重新加载", "");
                else
                    result = operatorService.DeleteAudit(Session["applicationUID"].ToString(), Pointids.Split(';'), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1), "", "", "", "重新加载", "重新加载", "");
                if (result)
                {
                    RadNotification1.Show();
                    InitRadScheduler(true);
                    RadNotification1.Value = "1";//更新通知窗口值
                    //Alert("重新加载审核成功！");
                }
                else Alert("重新加载审核失败！");
            }
            else
            {
                Alert("请选择站点！");
            }
        }

        /// <summary>
        /// 站点切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radioPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["PointID"] = GetPointSelectedValue();
            BindingFactors();//重新绑定因子
        }

        /// <summary>
        /// 重新绑定因子
        /// </summary>
        private void BindingFactors()
        {
            string Pointids = GetPointSelectedValue();
            if (!Pointids.Equals(""))
            {
                ViewState["PointID"] = GetPointSelectedValue();
                PollutantBind();
            }
            else
            {
                Alert("请选择站点！");
                return;
            }
        }

        /// <summary>
        /// 时间段选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadDatePickerBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (RadDatePickerBegin.SelectedDate == null || RadDatePickerEnd.SelectedDate == null)
            {
                Alert("时间不能为空");
                return;
            }
            else if (RadDatePickerBegin.SelectedDate.Value > RadDatePickerEnd.SelectedDate.Value)
            {
                Alert("开始时间必须小于结束时间！");
                return;
            }
            gridAuditLog.Rebind();
        }

        /// <summary>
        /// 绑定日志信息点位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditLog_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem && gridAuditLog.MasterTableView.DataSourceCount > 0)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    if (item.DataItem != null)
                    {
                        if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
                        {
                            AuditAirLogEntity row = (AuditAirLogEntity)item.DataItem;
                            //TableCell cell = item["PointName"];
                            //cell.Text = radioPoint.Items.FindByValue(auditLogService.GetPointID(row.AuditStatusUid.ToString(), Session["applicationUID"].ToString()).ToString()).Text;

                            TableCell cell = item["Tstamp"];
                            cell.Text = Convert.ToDateTime(row.Tstamp).AddHours(1).ToString("MM-dd HH:mm");
                        }
                        else
                        {
                            AuditWaterLogEntity row = (AuditWaterLogEntity)item.DataItem;
                            TableCell cell = item["PointName"];
                            cell.Text = radioPoint.Items.FindByValue(auditLogService.GetPointID(row.AuditStatusUid.ToString(), Session["applicationUID"].ToString()).ToString()).Text;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 进入审核页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EnterAudit_Click(object sender, EventArgs e)
        {
            try
            {
                //string category = Session["FactorCategorySuper"].ToString();
                MutilPointAuditDataLijingpu.isfirst = true;
                DateTime DTBegin = DateTime.Parse(selectDateTime.Value);
                DateTime DTEnd = DateTime.Parse(selectDateTime.Value);

                String myUrl = "MutilPointAuditDataSuper.aspx?dtBegin=" + DTBegin + "&dtEnd=" + DTEnd + "&pointType=2&PointID=" + ViewState["PointID"] + "&Token=" + Request.QueryString["Token"];//AuditData.aspx
                if (DTBegin.Date <= DateTime.Now.Date && !GetPointSelectedValue().Equals(""))
                    Server.Transfer(myUrl, true);
                else
                    Alert("没有数据！");
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search_Click(object sender, EventArgs e)
        {
            InitRadScheduler(true);
            gridAuditLog.Rebind();
        }

        #region 方法
        /// <summary>
        /// 日历日期处理
        /// </summary>
        /// <param name="dayStr"></param>
        /// <returns></returns>
        public String getDay(String dayStr)
        {
            String[] arrStr = dayStr.Split('_');
            String day = "xx";
            if (arrStr.Length >= 4) day = arrStr[3].ToString();
            return day;
        }

        /// <summary>
        /// 获取选择的站点值（以;隔开）
        /// </summary>
        /// <returns></returns>
        private string GetPointSelectedValue()
        {
            string Pointids = "";
            foreach (ListItem item in radioPoint.Items)
            {
                if (item.Selected == true)
                    Pointids += Pointids.Equals("") ? item.Value.Split('/')[0] : ";" + item.Value.Split('/')[0];
            }
            return Pointids;
        }
        #endregion

        protected void radioPollutant_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 修改by lvyun
            string PointUid = radioPoint.SelectedValue.ToString().Split('/')[1];
            string InstrumentGuid = radioPollutant.SelectedValue;
            string PollutantName = "", PollutantCode = "", PollutantUid = "", PollutantShow = "", PollutantRead = "";

            //审核配置因子
            //IQueryable<AuditMonitoringPointPollutantEntity> AuditLists = g_AuditMonitoringPointService.DetailRetrieve(x => PointUid.Equals(x.AuditMonitoringPointEntity.MonitoringPointUid) && x.AuditMonitoringPointEntity.ApplicationUid == ApplicationUid && x.AuditMonitoringPointEntity.PointType == PointType);
            IQueryable<AuditMonitoringPointPollutantEntity> AuditLists = g_AuditMonitoringPointService.DetailRetrieve(x => PointUid.Equals(x.AuditMonitoringPointEntity.MonitoringPointUid) && x.AuditMonitoringPointEntity.ApplicationUid == ApplicationUid);
            List<AuditMonitoringPointPollutantEntity> AuditData = new List<AuditMonitoringPointPollutantEntity>();
            if (AuditLists != null)
            {
                foreach (AuditMonitoringPointPollutantEntity audit in AuditLists)
                {
                    if (PointUid == audit.AuditMonitoringPointEntity.MonitoringPointUid)
                    {
                        if (!AuditData.Contains(audit))
                        {
                            AuditData.Add(audit);
                        }
                    }
                }
            }
            if (AuditData != null)
            {

                foreach (AuditMonitoringPointPollutantEntity audit in AuditData)
                {
                    PollutantShow += audit.PollutantCode + ";";
                    if (!string.IsNullOrEmpty(Convert.ToString(audit.ReadOnly)) && (Boolean)audit.ReadOnly)
                    {
                        PollutantRead += audit.PollutantCode + ";";
                    }
                }
            }


            string[] PollutantShowList = PollutantShow.Split(';');
            string[] PollutantReadList = PollutantRead.Split(';');
            #endregion
            //string CategoryUid = "";
            string selCode = "";
            string selName = "";
            string isEdit = "";
            string PollutantDecimalNum = "";
            string PollutantUnit = "";
            //for (int i = 0; i < radioPollutant.Items.Count; i++)
            //{
            //    if (radioPollutant.Items[i].Selected)
            //        CategoryUid = radioPollutant.Items[i].Value;
            //}
            AirPollutantService airservice = new AirPollutantService();
            IQueryable<InstrumentChannelEntity> InstrCList = BaseDataModel.InstrumentChannelEntities;


            IQueryable<PollutantCodeEntity> pollutantQueryable = from item in airservice.RetrieveListByUseOrNot(true)
                                                                 join ic in InstrCList on item.PollutantCode equals ic.PollutantCode
                                                                 where (PollutantShowList.Contains(item.PollutantCode) && ic.InstrumentUid.Equals(InstrumentGuid))
                                                                 orderby item.OrderByNum descending
                                                                 select item;
            //PollutantCodeEntity[] pEntity = pollutantQueryable.ToArray();
            List<PollutantCodeEntity> pEntity = pollutantQueryable.ToList();
            //if (InstrumentGuid == "da4f968f-cc6e-4fec-8219-6167d100499d")
            //{
            //    PollutantCodeEntity O3Entity = airservice.RetrieveEntityByCode("a05024");
            //    pEntity.Add(O3Entity);
            //}
            if (InstrumentGuid == "3745f768-a789-4d58-9578-9e41fde5e5f0")
            {
                PollutantCodeEntity Entity40 = airservice.RetrieveEntityByCode("a51040");
                PollutantCodeEntity Entity48 = airservice.RetrieveEntityByCode("a51048");
                PollutantCodeEntity Entity52 = airservice.RetrieveEntityByCode("a51052");
                PollutantCodeEntity Entity54 = airservice.RetrieveEntityByCode("a51054");
                pEntity.Remove(Entity40);
                pEntity.Remove(Entity48);
                pEntity.Remove(Entity52);
                pEntity.Remove(Entity54);
            }
            //if (CategoryUid == "fbc6dyta-d06c-678g-b5y0-89b12be5bda3")//VOCS
            //    pEntity = airservice.RetrieveListByUseOrNot(true).Where(p => p.CategoryUid == CategoryUid && p.OrderByNum != 0 && PollutantShowList.Contains(p.PollutantCode)).ToArray();
            //else
            //    pEntity = airservice.RetrieveListByUseOrNot(true).Where(p => p.CategoryUid == CategoryUid && PollutantShowList.Contains(p.PollutantCode)).ToArray();
            //for (int i = 0; i < pEntity.Length; i++)
            for (int i = 0; i < pEntity.Count; i++)
            {
                selCode += selCode == "" ? pEntity[i].PollutantCode : ";" + pEntity[i].PollutantCode;
                selName += selName == "" ? pEntity[i].PollutantName : ";" + pEntity[i].PollutantName;
                PollutantDecimalNum += PollutantDecimalNum == "" ? pEntity[i].DecimalDigit.ToString() : ";" + pEntity[i].DecimalDigit.ToString();

                DictionaryService dservice = new DictionaryService();
                string Unit = dservice.GetTextByGuid(Service.Core.Enums.DictionaryType.AMS, "计量单位", pEntity[i].MeasureUnitUid);
                PollutantUnit += Unit + ";";
                //isEdit += isEdit == "" ? "0" : ";" + "0";
                if (PollutantReadList.Contains(pEntity[i].PollutantCode))
                {
                    isEdit += "1;";
                }
                else
                {
                    isEdit += "0;";
                }
            }
            Session["InstrumentUid"] = InstrumentGuid;
            //Session["FactorCategorySuper"] = CategoryUid;
            Session["FactorCodeSuper"] = selCode;
            Session["FactorNameSuper"] = selName;
            Session["isEditSuper"] = isEdit;
            Session["PollutantDecimalNumSuper"] = PollutantDecimalNum;
            Session["PollutantUnitSuper"] = !PollutantUnit.Equals("") ? PollutantUnit.Substring(0, PollutantUnit.Length - 1) : "";
        }

        #endregion
    }
}