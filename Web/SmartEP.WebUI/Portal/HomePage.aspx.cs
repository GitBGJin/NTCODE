using log4net;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.Web.WebServiceHelper;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Portal
{
    /// <summary>
    /// 名称：HomePage.aspx.cs
    /// 创建人：吕云
    /// 创建日期：2017-5-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：模块切换页面
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class HomePage : BasePageLZ
    {
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                LoadAQI();
                InitNavigateUrl();
                if (!string.IsNullOrEmpty(SessionHelper.Get("DisplayName")))
                {
                    WelcomeTitle.InnerText = "欢迎，" + SessionHelper.Get("DisplayName").ToString()+"！";
                }
            }
        }
        private void LoadAQI()
        {
            UserControl u = (UserControl)LoadControl("../Controls/RealTimeAirQualityState.ascx");
            Type t = u.GetType();
            PropertyInfo p = t.GetProperty("ID");
            p.SetValue(u, "a", null);
            divAQI.Controls.Clear();
            divAQI.Controls.Add(u);

        }
        private void InitNavigateUrl()
        {
            try
            {
                string userGuid = Session["UserGuid"].ToString();

                #region 获取角色Guid
                string SuperStationRole = ConfigurationManager.AppSettings["SuperStationRole"].ToString();
                string GeneralParaRole = ConfigurationManager.AppSettings["GeneralParaRole"].ToString();
                string RelevanceRole = ConfigurationManager.AppSettings["RelevanceRole"].ToString();
                string AuditRole = ConfigurationManager.AppSettings["AuditRole"].ToString();
                string SysConfigRole = ConfigurationManager.AppSettings["SysConfigRole"].ToString();
                string StatisticRole = ConfigurationManager.AppSettings["StatisticRole"].ToString();
                string GisRole = ConfigurationManager.AppSettings["GisRole"].ToString();
                string VideoRole = ConfigurationManager.AppSettings["VideoRole"].ToString();
                string MgrRole = ConfigurationManager.AppSettings["MgrRole"].ToString();
                string SuperRole = ConfigurationManager.AppSettings["SuperRole"].ToString();
                #endregion

                #region 通过用户Guid获取用户的角色
                string FrameRoleWebServiceUrl = ConfigurationManager.AppSettings["FrameRoleServiceUrl"].ToString();
                string flag = ConfigurationManager.AppSettings["Flag"].ToString();
                string strRoleGuidList = WebServiceHelper.InvokeWebService(FrameRoleWebServiceUrl, "GetRoleGuidList", new object[] { userGuid, flag }).ToString();
                #endregion

                if ((!string.IsNullOrEmpty(SuperStationRole) & strRoleGuidList.IndexOf(SuperStationRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                {
                    RadImageSuperStation.ImageUrl = "~/Resources/Images/HomePage/blue/超级站管理.png?t=1";
                }
                if ((!string.IsNullOrEmpty(GeneralParaRole) & strRoleGuidList.IndexOf(GeneralParaRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                {
                    RadImageGeneralPara.ImageUrl = "~/Resources/Images/HomePage/blue/常规参数.png?t=1";
                }
                if ((!string.IsNullOrEmpty(RelevanceRole) & strRoleGuidList.IndexOf(RelevanceRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                {
                    RadImageRelevance.ImageUrl = "~/Resources/Images/HomePage/blue/关联性分析.png";
                }
                if ((!string.IsNullOrEmpty(AuditRole) & strRoleGuidList.IndexOf(AuditRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                {
                    RadImageAudit.ImageUrl = "~/Resources/Images/HomePage/blue/数据审核.png";
                }
                if ((!string.IsNullOrEmpty(SysConfigRole) & strRoleGuidList.IndexOf(SysConfigRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                {
                    RadImageSysConfig.ImageUrl = "~/Resources/Images/HomePage/blue/系统信息.png";
                }
                if ((!string.IsNullOrEmpty(StatisticRole) & strRoleGuidList.IndexOf(StatisticRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                {
                    RadImageStatistic.ImageUrl = "~/Resources/Images/HomePage/blue/统计.png";
                }
                if ((!string.IsNullOrEmpty(GisRole) & strRoleGuidList.IndexOf(GisRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                {
                    RadImageGIS.ImageUrl = "~/Resources/Images/HomePage/blue/GIS.png";
                }
                if ((!string.IsNullOrEmpty(VideoRole) & strRoleGuidList.IndexOf(VideoRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                {
                    RadImageVideo.ImageUrl = "~/Resources/Images/HomePage/blue/报表管理.png";
                }

                RadImageSuperStation.NavigateUrl = "MidPage.aspx?Type=SuperStation";
                RadImageGeneralPara.NavigateUrl = "MidPage.aspx?Type=GeneralPara";
                RadImageRelevance.NavigateUrl = "MidPage.aspx?Type=Relevance";
                RadImageAudit.NavigateUrl = "MidPage.aspx?Type=Audit";
                RadImageSysConfig.NavigateUrl = "MidPage.aspx?Type=SysConfig";
                RadImageStatistic.NavigateUrl = "MidPage.aspx?Type=Statistic";
                RadImageMgr.NavigateUrl = "MidPage.aspx?Type=Mgr";
                RadImageOut.NavigateUrl = "Login.aspx";
                RadImageGIS.NavigateUrl = "MidPage.aspx?Type=GIS";
                RadImageVideo.NavigateUrl = "MidPage.aspx?Type=Video";
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
    }
}