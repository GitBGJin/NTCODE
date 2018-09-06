using Com.Sinoyd.Security;
using log4net;
using SmartEP.Utilities.Web.NetWork;
using SmartEP.Utilities.Web.WebServiceHelper;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Portal
{
    /// <summary>
    /// 名称：AuditData.aspx.cs
    /// 创建人：吕云
    /// 创建日期：2017-5-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：中间页面
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class MidPage : SmartEP.WebUI.Common.BasePage
    {
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        protected void Page_Load(object sender, EventArgs e)
        {
            //form1.Target = "_blank";
            if (!IsPostBack)
            {
                InitUrl();
            }
        }

        private void InitUrl()
        {
            try
            {
                string userGuid = Session["UserGuid"].ToString();
                string Type = Request["Type"];
                string Token = Security.EncryptDES(userGuid, "12345678");

                #region 获取页面url
                string InnerIP = System.Configuration.ConfigurationManager.AppSettings["InnerIP"].ToString();
                string InnerPortalUrl = System.Configuration.ConfigurationManager.AppSettings["InnerPortalUrl"].ToString();
                bool IsInnerNetWork = Networks.IsInnerNetWork(InnerIP);

                string frameUrl = ConfigurationManager.AppSettings["PortalUrl"].ToString();
                if (IsInnerNetWork == true)//指定ip处于内网则切换到内网ip
                {
                    //frameUrl = InnerPortalUrl;
                }
                string homePageUrl = frameUrl + "/" + ConfigurationManager.AppSettings["PortalName"].ToString();
                frameUrl += "/" + ConfigurationManager.AppSettings["FrameworkName"].ToString();
                string gISUrl = ConfigurationManager.AppSettings["GISUrl"].ToString();
                #endregion

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

                if (strRoleGuidList == "")
                {
                    ClientScript.RegisterStartupScript(ClientScript.GetType(), "alert", "<script>alert('无权限访问，请联系管理员！');top.location ='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "'</script>");
                }
                else
                {
                    switch (Type)
                    {
                        case "Air":
                            //Response.Redirect(frameUrl + "/FrameAir.aspx?Token=" + Token);
                            Response.Redirect(frameUrl + "/FrameAir.aspx?Token=" + Token,false);
                            break;
                        case "SuperStation":
                            if ((!string.IsNullOrEmpty(SuperStationRole) & strRoleGuidList.IndexOf(SuperStationRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                                //Response.Redirect(frameUrl + "/FrameSuperStation.aspx?Token=" + Token);
                                Response.Redirect(frameUrl + "/FrameSuperStation.aspx?Token=" + Token,false);
                            else
                            {
                                ClientScript.RegisterStartupScript(ClientScript.GetType(), "alert", "<script>alert('无权限访问，请联系管理员！');top.location ='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "'</script>");
                            }
                            break;
                        case "GeneralPara":
                            if ((!string.IsNullOrEmpty(GeneralParaRole) & strRoleGuidList.IndexOf(GeneralParaRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                                //Response.Redirect(frameUrl + "/FrameGeneralPara.aspx?Token=" + Token);
                                Response.Redirect(frameUrl + "/FrameGeneralPara.aspx?Token=" + Token,false);
                            else
                            {
                                ClientScript.RegisterStartupScript(ClientScript.GetType(), "alert", "<script>alert('无权限访问，请联系管理员！');top.location ='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "'</script>");
                            }
                            break;
                        case "Relevance":
                            if ((!string.IsNullOrEmpty(RelevanceRole) & strRoleGuidList.IndexOf(RelevanceRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                                //Response.Redirect(frameUrl + "/FrameRelevance.aspx?Token=" + Token);
                                Response.Redirect(frameUrl + "/FrameRelevance.aspx?Token=" + Token,false);
                            else
                            {
                                ClientScript.RegisterStartupScript(ClientScript.GetType(), "alert", "<script>alert('无权限访问，请联系管理员！');top.location ='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "'</script>");
                            }
                            break;
                        case "Audit":
                            if ((!string.IsNullOrEmpty(AuditRole) & strRoleGuidList.IndexOf(AuditRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                                //Response.Redirect(frameUrl + "/FrameAudit.aspx?Token=" + Token);
                                Response.Redirect(frameUrl + "/FrameAudit.aspx?Token=" + Token,false);
                            else
                            {
                                ClientScript.RegisterStartupScript(ClientScript.GetType(), "alert", "<script>alert('无权限访问，请联系管理员！');top.location ='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "'</script>");
                            }
                            break;
                        case "SysConfig":
                            if ((!string.IsNullOrEmpty(SysConfigRole) & strRoleGuidList.IndexOf(SysConfigRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                                //Response.Redirect(frameUrl + "/FrameSysConfig.aspx?Token=" + Token);
                                Response.Redirect(frameUrl + "/FrameSysConfig.aspx?Token=" + Token,false);
                            else
                            {
                                ClientScript.RegisterStartupScript(ClientScript.GetType(), "alert", "<script>alert('无权限访问，请联系管理员！');top.location ='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "'</script>");
                            }
                            break;
                        case "Statistic":
                            if ((!string.IsNullOrEmpty(StatisticRole) & strRoleGuidList.IndexOf(StatisticRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                                //Response.Redirect(frameUrl + "/FrameStatistic.aspx?Token=" + Token);
                                Response.Redirect(frameUrl + "/FrameStatistic.aspx?Token=" + Token, false);
                            else
                            {
                                ClientScript.RegisterStartupScript(ClientScript.GetType(), "alert", "<script>alert('无权限访问，请联系管理员！');top.location ='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "'</script>");
                            }
                            break;
                        case "GIS":
                            if ((!string.IsNullOrEmpty(GisRole) & strRoleGuidList.IndexOf(GisRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                                //Response.Redirect(gISUrl);
                                Response.Redirect(gISUrl,false);
                                //Response.Redirect(gISUrl + "?Token=" + Token);
                                //Response.Write("<script>window.open('" + gISUrl + "','_blank');</script>");

                                //Response.Write("<script>window.open('" + gISUrl + "','_blank');self.parent.location='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "';</script>");
                            else
                                ClientScript.RegisterStartupScript(ClientScript.GetType(), "alert", "<script>alert('无权限访问，请联系管理员！');top.location ='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "'</script>");
                            break;
                        case "Video":
                            if ((!string.IsNullOrEmpty(VideoRole) & strRoleGuidList.IndexOf(VideoRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                                //Response.Write("<script>window.open('" + gISUrl + "','_blank');self.parent.location='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "';</script>");
                                //Response.Redirect(frameUrl + "/FrameExcel.aspx?Token=" + Token);
                                Response.Redirect(frameUrl + "/FrameExcel.aspx?Token=" + Token,false);
                            else
                                ClientScript.RegisterStartupScript(ClientScript.GetType(), "alert", "<script>alert('无权限访问，请联系管理员！');top.location ='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "'</script>");
                            break;
                        case "Mgr":
                            if ((!string.IsNullOrEmpty(MgrRole) & strRoleGuidList.IndexOf(MgrRole) > -1)
                                || (!string.IsNullOrEmpty(SuperRole) & strRoleGuidList.IndexOf(SuperRole) > -1))
                                //Response.Redirect(frameUrl + "/FrameMgr.aspx?Token=" + Token);
                                Response.Redirect(frameUrl + "/FrameMgr.aspx?Token=" + Token,false);
                            else
                                ClientScript.RegisterStartupScript(ClientScript.GetType(), "alert", "<script>alert('无权限访问，请联系管理员！');top.location ='" + homePageUrl + "/Portal/HomePage.aspx?Token=" + Token + "'</script>");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        
    }
}