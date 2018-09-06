using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.AutoMonitoring.common;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.Frame;
using SmartEP.Utilities.Caching;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class AuditSetting : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 系统配置数据接口
        /// </summary>
        DictionaryService g_DictionaryService = Singleton<DictionaryService>.GetInstance();
        /// <summary>
        /// 测点数据接口
        /// </summary>
        MonitoringPointService g_MonitoringPointService = Singleton<MonitoringPointService>.GetInstance();
        /// <summary>
        /// 监测因子接口
        /// </summary>
        PollutantService g_PollutantService = Singleton<PollutantService>.GetInstance();
        /// <summary>
        /// 审核因子配置接口
        /// </summary>
        AuditMonitoringPointService g_AuditMonitoringPointService = Singleton<AuditMonitoringPointService>.GetInstance();
        /// <summary>
        /// 字典类别
        /// </summary>
        SmartEP.Service.Core.Enums.DictionaryType DType = SmartEP.Service.Core.Enums.DictionaryType.Air;
        /// <summary>
        /// 应用程序Uid
        /// </summary>
        string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(SmartEP.Core.Enums.ApplicationValue.Air);
        /// <summary>
        /// 状态因子Uid
        /// </summary>
        string PollutantTupeUid = "ae39f55e-5c43-4b4a-b224-0b925b5f3c9f";
        /// <summary>
        /// 普通监测点状态类型
        /// </summary>
        int PointType = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化站点类型
                LoadSiteTypeData();
                //加载数据
                BindTable();
            }
        }

        #region 初始化站点类型
        /// <summary>
        /// 初始化站点类型
        /// 气站类型是ContrlUid，水战类型是SiteTypeUid
        /// </summary>
        public void LoadSiteTypeData()
        {
            //站点类型
            IQueryable<V_CodeMainItemEntity> ContrlUidLists = g_DictionaryService.RetrieveList(DType, "空气站点属性类型");
            ddlContrlUid.DataSource = ContrlUidLists;
            ddlContrlUid.DataTextField = "ItemText";
            ddlContrlUid.DataValueField = "ItemGuid";
            ddlContrlUid.DataBind();
        }
        #endregion

        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        private void BindTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PointUid", typeof(string));
            dt.Columns.Add("PointName", typeof(string));
            dt.Columns.Add("ContrlUid", typeof(string));
            dt.Columns.Add("PollutantName", typeof(string));
            dt.Columns.Add("PollutantCode", typeof(string));
            dt.Columns.Add("PollutantUid", typeof(string));
            dt.Columns.Add("PollutantShow", typeof(string));
            dt.Columns.Add("PollutantRead", typeof(string));

            string ContrlUid = ddlContrlUid.SelectedValue;
            IQueryable<MonitoringPointEntity> PointLists = g_MonitoringPointService.Retrieve(x => x.ApplicationUid == ApplicationUid && x.ContrlUid == ContrlUid && (Boolean)x.EnableOrNot && (Boolean)x.ShowOrNot).OrderByDescending(x => x.OrderByNum);
            List<string> PointUids = new List<string>();
            if (PointLists != null)
            {
                foreach (MonitoringPointEntity point in PointLists)
                {
                    if (!PointUids.Contains(point.MonitoringPointUid))
                    {
                        PointUids.Add(point.MonitoringPointUid);
                    }
                    DataRow dr = dt.NewRow();
                    dr["PointUid"] = point.MonitoringPointUid;
                    dr["PointName"] = point.MonitoringPointName;
                    dr["ContrlUid"] = point.ContrlUid;
                    dt.Rows.Add(dr);
                }
            }
            //测点对应状态因子
            IQueryable<V_Point_InstrumentChannelEntity> PollutantLists = g_PollutantService.Retrieve(x => PointUids.Contains(x.MonitoringPointUid) && x.TypeUid == PollutantTupeUid && x.ApplicationUid == ApplicationUid).OrderByDescending(x => x.OrderByNum);
            //审核配置因子
            IQueryable<AuditMonitoringPointPollutantEntity> AuditLists = g_AuditMonitoringPointService.DetailRetrieve(x => PointUids.Contains(x.AuditMonitoringPointEntity.MonitoringPointUid) && x.AuditMonitoringPointEntity.ApplicationUid == ApplicationUid && x.AuditMonitoringPointEntity.PointType == PointType);

            //开始循环组装数据
            foreach (DataRow dr in dt.Rows)
            {
                string PointUid = Convert.ToString(dr["PointUid"]);
                List<V_Point_InstrumentChannelEntity> PollutantData = new List<V_Point_InstrumentChannelEntity>();
                List<AuditMonitoringPointPollutantEntity> AuditData = new List<AuditMonitoringPointPollutantEntity>();
                //加载所有因子
                if (PollutantLists != null)
                {
                    foreach (V_Point_InstrumentChannelEntity pollutant in PollutantLists)
                    {
                        if (PointUid == pollutant.MonitoringPointUid)
                        {
                            if (PollutantData != null)
                            {
                                bool isContains = false;
                                foreach (V_Point_InstrumentChannelEntity item in PollutantData)
                                {
                                    if (pollutant.PollutantUid == item.PollutantUid)
                                    {
                                        isContains = true;
                                        break;
                                    }
                                }
                                if (!isContains)
                                {
                                    PollutantData.Add(pollutant);
                                }
                            }
                            else
                            {
                                PollutantData.Add(pollutant);
                            }
                        }
                    }
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
                }
                //组装数据
                string PollutantName = "", PollutantCode = "", PollutantUid = "", PollutantShow = "", PollutantRead = "";
                if (PollutantData != null)
                {
                    foreach (V_Point_InstrumentChannelEntity pollutant in PollutantData)
                    {
                        PollutantName += pollutant.PollutantName + "|";
                        PollutantCode += pollutant.PollutantCode + "|";
                        PollutantUid += pollutant.PollutantUid + "|";
                    }
                    if (AuditData != null)
                    {
                        foreach (AuditMonitoringPointPollutantEntity audit in AuditData)
                        {
                            PollutantShow += audit.PollutantUid + "|";
                            if (!string.IsNullOrEmpty(Convert.ToString(audit.ReadOnly)) && (Boolean)audit.ReadOnly)
                            {
                                PollutantRead += audit.PollutantUid + "|";
                            }
                        }
                    }
                }
                if (PollutantName.Length > 0)
                {
                    dr["PollutantName"] = PollutantName.Substring(0, PollutantName.Length - 1);
                }
                if (PollutantCode.Length > 0)
                {
                    dr["PollutantCode"] = PollutantCode.Substring(0, PollutantCode.Length - 1);
                }
                if (PollutantUid.Length > 0)
                {
                    dr["PollutantUid"] = PollutantUid.Substring(0, PollutantUid.Length - 1);
                }
                if (PollutantShow.Length > 0)
                {
                    dr["PollutantShow"] = PollutantShow.Substring(0, PollutantShow.Length - 1);
                }
                if (PollutantRead.Length > 0)
                {
                    dr["PollutantRead"] = PollutantRead.Substring(0, PollutantRead.Length - 1);
                }
            }
            //绑定数据源
            rptList.DataSource = dt;
            rptList.DataBind();
        }
        #endregion

        #region Repeater单元格绑定事件
        /// <summary>
        /// Repeater单元格绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                string Pollutant = ((HiddenField)e.Item.FindControl("hidPollutantName")).Value;
                if (Pollutant.Length > 0)
                {
                    string[] PollutantNames = ((HiddenField)e.Item.FindControl("hidPollutantName")).Value.Split('|');
                    string[] PollutantUids = ((HiddenField)e.Item.FindControl("hidPollutantUid")).Value.Split('|');
                    CheckBoxList cblPollutant_Show = (CheckBoxList)e.Item.FindControl("cblPollutant_Show");
                    CheckBoxList cblPollutant_Read = (CheckBoxList)e.Item.FindControl("cblPollutant_Read");
                    cblPollutant_Show.Items.Clear();
                    for (int i = 0; i < PollutantUids.Length; i++)
                    {
                        cblPollutant_Show.Items.Add(new ListItem(PollutantNames[i], PollutantUids[i]));
                        cblPollutant_Read.Items.Add(new ListItem(PollutantNames[i], PollutantUids[i]));
                    }
                    //显示配置
                    string PollutantShow = ((HiddenField)e.Item.FindControl("hidPollutantShow")).Value;
                    if (PollutantShow.Length > 0)
                    {
                        string[] shows = PollutantShow.Split('|');
                        for (int n = 0; n < cblPollutant_Show.Items.Count; n++)
                        {
                            for (int i = 0; i < shows.Length; i++)
                            {
                                if (cblPollutant_Show.Items[n].Value == shows[i])
                                {
                                    cblPollutant_Show.Items[n].Selected = true;
                                    break;
                                }
                            }
                        }
                    }
                    //只读配置
                    string PollutantRead = ((HiddenField)e.Item.FindControl("hidPollutantRead")).Value;
                    if (PollutantRead.Length > 0)
                    {
                        string[] reads = PollutantRead.Split('|');
                        for (int n = 0; n < cblPollutant_Read.Items.Count; n++)
                        {
                            for (int i = 0; i < reads.Length; i++)
                            {
                                if (cblPollutant_Read.Items[n].Value == reads[i])
                                {
                                    cblPollutant_Read.Items[n].Selected = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 查询按钮事件
        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindTable();
        }
        #endregion

        #region 保存按钮事件
        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            string CreatUser = SessionHelper.Get("DisplayName");
            DateTime CreatDateTime = DateTime.Now;
            List<string> PointUids = new List<string>();
            List<AuditMonitoringPointEntity> MainLists = new List<AuditMonitoringPointEntity>();
            List<AuditMonitoringPointPollutantEntity> DetailLists = new List<AuditMonitoringPointPollutantEntity>();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                string PointUid = ((HiddenField)rptList.Items[i].FindControl("hidPointUid")).Value;
                PointUids.Add(PointUid);
                string ContrlUid = ((HiddenField)rptList.Items[i].FindControl("hidContrlUid")).Value;
                string PollutantName = ((HiddenField)rptList.Items[i].FindControl("hidPollutantName")).Value;
                string PollutantCode = ((HiddenField)rptList.Items[i].FindControl("hidPollutantCode")).Value;
                string PollutantUid = ((HiddenField)rptList.Items[i].FindControl("hidPollutantUid")).Value;
                if (PollutantName.Length > 0)
                {
                    string[] names = PollutantName.Split('|');
                    string[] codes = PollutantCode.Split('|');
                    string[] uids = PollutantUid.Split('|');
                    CheckBoxList cblPollutant_Show = (CheckBoxList)rptList.Items[i].FindControl("cblPollutant_Show");
                    CheckBoxList cblPollutant_Read = (CheckBoxList)rptList.Items[i].FindControl("cblPollutant_Read");

                    //因子显示权限
                    List<string> pollutantUids_Show = new List<string>();
                    for (int n = 0; n < cblPollutant_Show.Items.Count; n++)
                    {
                        if (cblPollutant_Show.Items[n].Selected)
                        {
                            pollutantUids_Show.Add(cblPollutant_Show.Items[n].Value);
                        }
                    }
                    //因子只读权限
                    List<string> pollutantUids_Read = new List<string>();
                    for (int n = 0; n < cblPollutant_Read.Items.Count; n++)
                    {
                        if (cblPollutant_Read.Items[n].Selected)
                        {
                            pollutantUids_Read.Add(cblPollutant_Read.Items[n].Value);
                        }
                    }

                    //开始组装数据
                    if (pollutantUids_Show != null && pollutantUids_Show.Count > 0)
                    {
                        //主表数据
                        string mainUid = Guid.NewGuid().ToString();
                        AuditMonitoringPointEntity main = new AuditMonitoringPointEntity();
                        main.AuditMonitoringPointUid = mainUid;
                        main.ApplicationUid = ApplicationUid;
                        main.MonitoringPointUid = PointUid;
                        main.AuditTypeUid = ContrlUid;
                        main.PointType = PointType;
                        main.CreatUser = CreatUser;
                        main.CreatDateTime = CreatDateTime;
                        MainLists.Add(main);
                        //从表数据
                        for (int j = 0; j < uids.Length; j++)
                        {
                            if (pollutantUids_Show.Contains(uids[j]))
                            {
                                AuditMonitoringPointPollutantEntity detail = new AuditMonitoringPointPollutantEntity();
                                detail.AuditPollutantUID = Guid.NewGuid().ToString();
                                detail.ApplicationUid = ApplicationUid;
                                detail.AuditMonitoringPointUid = mainUid;
                                detail.PollutantUid = uids[j];
                                detail.PollutantCode = codes[j];
                                detail.PollutantName = names[j];
                                detail.ReadOnly = false;
                                if (pollutantUids_Read.Contains(uids[j]))
                                {
                                    detail.ReadOnly = true;
                                }
                                detail.CreatUser = CreatUser;
                                detail.CreatDateTime = CreatDateTime;
                                DetailLists.Add(detail);
                            }
                        }
                    }
                }
            }
            //操作数据
            if (PointUids != null && PointUids.Count > 0)
            {
                g_AuditMonitoringPointService.GetData(PointUids, MainLists, DetailLists, ApplicationUid, PointType);
            }

            Alert("保存成功");
        }
        #endregion
    }
}