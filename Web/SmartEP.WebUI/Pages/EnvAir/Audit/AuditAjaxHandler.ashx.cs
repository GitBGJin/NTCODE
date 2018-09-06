using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using SmartEP.Utilities.Web.ExtensionMethods;
using SmartEP.Service.Frame;
using SmartEP.WebUI.Controls;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    /// <summary>
    /// 名称：AuditAjaxHandler.cs
    /// 创建人：
    /// 创建日期：
    /// 维护人员：
    /// 最新维护人员：吕云
    /// 最新维护日期：2017-6-20
    /// 功能摘要：
    /// 审核站点抽象类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditAjaxHandler : IHttpHandler
    {
        AuditOperatorService operatoService = new AuditOperatorService();//审核操作接口

        DataDealService dataDealService = new DataDealService();
        AuditOperatorService auditDataService = new AuditOperatorService();
        UserService userService = new UserService();
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["DataType"] != null)
            {
                if (context.Request.QueryString["DataType"].Equals("MutilFactor"))//单点多因子（单站审核）
                {
                    string[] portid = context.Request["PointID"].ToString().Split(';');
                    string[] factorCode = context.Request["FactorCode"].ToString().Split(';');
                    //string[] factorName = context.Request["FactorName"].ToString().Split(';');
                    //string[] PollutantDecimalNum = context.Request["PollutantDecimalNum"].ToString().Split(';');
                    string applicationUID = context.Request["ApplicationUID"].ToString();
                    DateTime StartTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    string jsonStr = dataDealService.AuditMultiFacotr(applicationUID, portid, factorCode, StartTime.Date, StartTime.Date.AddDays(1).AddHours(-1), true);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(jsonStr);
                }
                else if (context.Request.QueryString["DataType"].Equals("MutilPointFactor"))//单点多因子(多站审核)
                {
                    string[] portid = context.Request["PointID"].ToString().Split(';');
                    string[] factorCode = context.Request["FactorCode"].ToString().Split(';');
                    //string[] factorName = context.Request["FactorName"].ToString().Split(';');
                    //string[] PollutantDecimalNum = context.Request["PollutantDecimalNum"].ToString().Split(';');
                    string applicationUID = context.Request["ApplicationUID"].ToString();
                    DateTime StartTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    DateTime EndTime = Convert.ToDateTime(context.Request["EndTime"].ToString()).Date.AddDays(1).AddHours(-1);
                    string pointType = context.Request["PointType"].ToString();
                    string jsonStr = dataDealService.AuditMultiPointFacotr(applicationUID, portid, factorCode, StartTime.Date, EndTime, true, FactorRsmAudit.pList, pointType);

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(jsonStr);
                }
                else if (context.Request.QueryString["DataType"].Equals("SingleFactor"))//多点单因子
                {
                    string[] portid = context.Request["PointID"].ToString().Split(';');
                    string[] PointName = context.Request["PointName"].ToString().Split(';');
                    string factorCode = context.Request["FactorCode"].ToString();
                    //string factorName = HttpUtility.UrlDecode(context.Request["FactorName"].ToString());
                    //int PollutantDecimalNum = context.Request["PollutantDecimalNum"] != null && !context.Request["PollutantDecimalNum"].Equals("") ? Convert.ToInt32(context.Request["PollutantDecimalNum"].ToString()) : 3;

                    string applicationUID = context.Request["ApplicationUID"].ToString();
                    DateTime StartTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    string jsonStr = dataDealService.AuditSingleFacotr(applicationUID, portid, PointName, factorCode, Convert.ToDateTime(StartTime), Convert.ToDateTime(StartTime), true);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(jsonStr);
                }
                else if (context.Request.QueryString["DataType"].Equals("ModifyAuditData"))//保存修改数据(时间段有效)
                {
                    #region 获取参数
                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    string[] FactorCode = context.Request["FactorCode"].ToString().Split(';');
                    string[] DataTime = context.Request["DataTime"].ToString().Split(';');
                    string[] NewData = context.Request["NewData"].ToString().Split(';');
                    string applicationUID = context.Request["ApplicationUID"].ToString();
                    string Reason = context.Request["Reason"].ToString();
                    string OperationTypeEnum = "修改";
                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                    string operatorFlag = context.Request.QueryString["Flag"].ToString();
                    #endregion
                    auditDataService.ModifyAuditData(applicationUID, PointID, FactorCode, DataTime, NewData, Reason, operatorFlag, OperationTypeEnum, UserIP, UserUid, CreatUser);
                }
                else if (context.Request.QueryString["DataType"].Equals("ModifyAuditDataNew"))//保存修改数据(时间段有效)
                {
                    #region 获取参数
                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    string[] FactorCode = context.Request["FactorCode"].ToString().Split(';');
                    string[] DataTime = context.Request["DataTime"].ToString().Split(';');
                    string[] NewData = context.Request["NewData"].ToString().Split(';');
                    string applicationUID = context.Request["ApplicationUID"].ToString();
                    string Reason = context.Request["Reason"].ToString();
                    string OperationTypeEnum = "修改";
                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                    string operatorFlag = context.Request.QueryString["Flag"].ToString();
                    #endregion
                    auditDataService.ModifyAuditDataNew(applicationUID, PointID, FactorCode, DataTime, NewData, Reason, operatorFlag, OperationTypeEnum, UserIP, UserUid, CreatUser);
                }
                else if (context.Request.QueryString["DataType"].Equals("InvalidRow"))//保存修改数据
                {
                    
                    #region 获取参数
                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    string[] FactorCode = context.Request["FactorCode"].ToString().Split(';');
                    string[] DataTime = context.Request["DataTime"].ToString().Split(';');
                    string[] NewData = context.Request["NewData"].ToString().Split(';');
                    string applicationUID = context.Request["ApplicationUID"].ToString();
                    string Reason = context.Request["Reason"].ToString();
                    string OperationTypeEnum = "修改";
                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                    string operatorFlag = context.Request.QueryString["Flag"].ToString();
                    #endregion
                    auditDataService.ModifyAuditDataForRow(applicationUID, PointID, FactorCode, DataTime, NewData, Reason, operatorFlag, OperationTypeEnum, UserIP, UserUid, CreatUser);
                }
                else if (context.Request.QueryString["DataType"].Equals("ModifyAuditDataSuper"))//保存修改数据
                {
                    #region 获取参数
                    string[] Pollutant = context.Request["Pollutant"].ToString().Split(';');
                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    string[] FactorCode = context.Request["FactorCode"].ToString().Split(';');
                    string[] DataTime = context.Request["DataTime"].ToString().Split(';');
                    string[] NewData = context.Request["NewData"].ToString().Split(';');
                    string Reason = context.Request["Reason"].ToString();
                    string OperationTypeEnum = "修改";
                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                    string operatorFlag = context.Request.QueryString["Flag"].ToString();

                    #endregion
                    auditDataService.ModifyAirAuditDataSuper(PointID, FactorCode, DataTime, NewData, Reason, operatorFlag, OperationTypeEnum, UserIP, UserUid, CreatUser, Pollutant);
                }
                else if (context.Request.QueryString["DataType"].Equals("ModifyAuditDataLijingpu"))//保存修改数据
                {
                    #region 获取参数
                    string[] Pollutant = context.Request["Pollutant"].ToString().Split(';');
                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    string[] FactorCode = context.Request["FactorCode"].ToString().Split(';');
                    string[] DataTime = context.Request["DataTime"].ToString().Split(';');
                    string[] NewData = context.Request["NewData"].ToString().Split(';');
                    string Reason = context.Request["Reason"].ToString();
                    string OperationTypeEnum = "修改";
                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                    string operatorFlag = context.Request.QueryString["Flag"].ToString();

                    #endregion
                    auditDataService.ModifyAirAuditDataLijingpu(PointID, FactorCode, DataTime, NewData, Reason, operatorFlag, OperationTypeEnum, UserIP, UserUid, CreatUser, Pollutant);
                }
                else if (context.Request.QueryString["DataType"].Equals("RestoreAuditData"))//恢复数据
                {
                    #region 获取参数
                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    string[] FactorCode = context.Request["FactorCode"].ToString().Split(';');
                    string[] DataTime = context.Request["DataTime"].ToString().Split(';');
                    string applicationUID = context.Request["ApplicationUID"].ToString();
                    string Reason = context.Request["Reason"].ToString();
                    string OperationTypeEnum = "还原";
                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                    #endregion
                    auditDataService.RestoreAuditData(applicationUID, PointID, FactorCode, DataTime, Reason, OperationTypeEnum, UserIP, UserUid, CreatUser);
                }
                else if (context.Request.QueryString["DataType"].Equals("SubmitAudit"))//提交审核
                {

                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    string InsId = context.Request["InsId"].ToString();
                    string ApplicationUID = context.Request["ApplicationUID"].ToString();
                    DateTime BeginTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    DateTime EndTime = Convert.ToDateTime(context.Request["EndTime"].ToString());
                    bool IsCreateDBF = false;
                    if (context.Request["pointType"].ToString().Equals("0") && ApplicationUID.Equals("airaaira-aira-aira-aira-airaairaaira")) IsCreateDBF = true;

                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                    bool isSuccess = auditDataService.SubmitAudit(ApplicationUID, PointID,InsId, BeginTime, EndTime, UserIP, UserUid, CreatUser, false, "", IsCreateDBF, context.Request["pointType"].ToString().Equals("2") ? "1" : "0");
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(isSuccess);

                    #region 获取参数【暂停使用】
                    //string[] PointID = context.Request["PointID"].ToString().Split(';');
                    //string ApplicationUID = context.Request["ApplicationUID"].ToString();
                    //DateTime DataTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    //auditDataService.SubmitAudit(ApplicationUID, PointID, DataTime, DataTime, "", "", "", false, "");
                    #endregion
                }
                else if (context.Request.QueryString["DataType"].Equals("ReAudit"))//重新审核
                {
                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    string ApplicationUID = context.Request["ApplicationUID"].ToString();
                    DateTime BeginTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    DateTime EndTime = Convert.ToDateTime(context.Request["EndTime"].ToString());
                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;

                    bool isSuccess = operatoService.DeleteAuditNew(ApplicationUID, PointID, BeginTime, EndTime, "", "", "", "重新加载", "重新加载", "");
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(isSuccess);
                }
                else if (context.Request.QueryString["DataType"].Equals("SubmitAuditNew"))//手动提交审核
                {

                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    string ApplicationUID = context.Request["ApplicationUID"].ToString();
                    DateTime BeginTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    DateTime EndTime = Convert.ToDateTime(context.Request["EndTime"].ToString());
                    bool IsCreateDBF = false;
                    if (context.Request["pointType"].ToString().Equals("0") && ApplicationUID.Equals("airaaira-aira-aira-aira-airaairaaira")) IsCreateDBF = true;

                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                    bool isSuccess = auditDataService.SubmitAuditNew(ApplicationUID, PointID, BeginTime, EndTime, "2", UserIP, UserUid, CreatUser, false, "", IsCreateDBF, context.Request["pointType"].ToString().Equals("2") ? "1" : "0");
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(isSuccess);

                    #region 获取参数【暂停使用】
                    //string[] PointID = context.Request["PointID"].ToString().Split(';');
                    //string ApplicationUID = context.Request["ApplicationUID"].ToString();
                    //DateTime DataTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    //auditDataService.SubmitAudit(ApplicationUID, PointID, DataTime, DataTime, "", "", "", false, "");
                    #endregion
                }
                else if (context.Request.QueryString["DataType"].Equals("SubmitAuditWeather"))//气象提交审核
                {

                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    string[] Factors = context.Request["FactorCode"].ToString().Split(';');
                    string ApplicationUID = context.Request["ApplicationUID"].ToString();
                    DateTime BeginTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    DateTime EndTime = Convert.ToDateTime(context.Request["EndTime"].ToString());
                    bool IsCreateDBF = false;
                    if (context.Request["pointType"].ToString().Equals("0") && ApplicationUID.Equals("airaaira-aira-aira-aira-airaairaaira")) IsCreateDBF = true;

                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                    bool isSuccess = auditDataService.SubmitAuditWeather(ApplicationUID, PointID,Factors, BeginTime, EndTime, UserIP, UserUid, CreatUser, false, "", IsCreateDBF, context.Request["pointType"].ToString().Equals("2") ? "1" : "0");
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(isSuccess);

                    #region 获取参数【暂停使用】
                    //string[] PointID = context.Request["PointID"].ToString().Split(';');
                    //string ApplicationUID = context.Request["ApplicationUID"].ToString();
                    //DateTime DataTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    //auditDataService.SubmitAudit(ApplicationUID, PointID, DataTime, DataTime, "", "", "", false, "");
                    #endregion
                }
                else if (context.Request.QueryString["DataType"].Equals("PreData"))//加载预处理数据
                {
                }
                else if (context.Request.QueryString["DataType"].Equals("MutilPointFactorSuper"))//单点多因子(多站审核)
                {
                    string[] portid = context.Request["PointID"].ToString().Split(';');
                    string[] factorCode = context.Request["FactorCode"].ToString().Split(';');
                    //string[] factorName = context.Request["FactorName"].ToString().Split(';');
                    //string[] PollutantDecimalNum = context.Request["PollutantDecimalNum"].ToString().Split(';');
                    string applicationUID = context.Request["ApplicationUID"].ToString();
                    DateTime StartTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    DateTime EndTime = Convert.ToDateTime(context.Request["EndTime"].ToString()).Date.AddDays(1).AddSeconds(-1);
                    string jsonStr = dataDealService.AuditMultiPointFacotrSuper(applicationUID, portid, factorCode, StartTime.Date, EndTime, true, FactorRsmAudit.pList);

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(jsonStr);
                }
                else if (context.Request.QueryString["DataType"].Equals("SubmitAuditSuper"))//提交审核（汞）
                {

                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    string ApplicationUID = context.Request["ApplicationUID"].ToString();
                    DateTime BeginTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    DateTime EndTime = Convert.ToDateTime(context.Request["EndTime"].ToString());
                    bool IsCreateDBF = false;
                    if (context.Request["pointType"].ToString().Equals("0") && ApplicationUID.Equals("airaaira-aira-aira-aira-airaairaaira")) IsCreateDBF = true;

                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                    bool isSuccess = auditDataService.SubmitAuditSuper(ApplicationUID, PointID, BeginTime, EndTime, UserIP, UserUid, CreatUser, false, "", IsCreateDBF, context.Request["pointType"].ToString().Equals("2") ? "1" : "0");
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(isSuccess);

                    #region 获取参数【暂停使用】
                    //string[] PointID = context.Request["PointID"].ToString().Split(';');
                    //string ApplicationUID = context.Request["ApplicationUID"].ToString();
                    //DateTime DataTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    //auditDataService.SubmitAudit(ApplicationUID, PointID, DataTime, DataTime, "", "", "", false, "");
                    #endregion
                }
                else if (context.Request.QueryString["DataType"].Equals("SubmitAuditWeibo"))//提交审核（微波辐射）
                {

                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    DateTime BeginTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    DateTime EndTime = Convert.ToDateTime(context.Request["EndTime"].ToString());

                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                    bool isSuccess = auditDataService.SubmitAuditSuper(PointID, BeginTime, EndTime, UserIP, UserUid, CreatUser, false, "");
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(isSuccess);

                    #region 获取参数【暂停使用】
                    //string[] PointID = context.Request["PointID"].ToString().Split(';');
                    //string ApplicationUID = context.Request["ApplicationUID"].ToString();
                    //DateTime DataTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    //auditDataService.SubmitAudit(ApplicationUID, PointID, DataTime, DataTime, "", "", "", false, "");
                    #endregion
                }
                else if (context.Request.QueryString["DataType"].Equals("SubmitAuditLijingpu"))//提交审核（粒径谱）
                {

                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    DateTime BeginTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    DateTime EndTime = Convert.ToDateTime(context.Request["EndTime"].ToString());

                    string UserIP = IPAddressExtensions.GetUserIp();
                    string UserUid = context.Request["UserGuid"].ToString();
                    string CreatUser = userService.GetUserByUserId(new Guid(UserUid)).LoginName;
                    bool isSuccess = auditDataService.SubmitAuditLijingpu(PointID, BeginTime, EndTime, UserIP, UserUid, CreatUser, false, "");
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(isSuccess);

                    #region 获取参数【暂停使用】
                    //string[] PointID = context.Request["PointID"].ToString().Split(';');
                    //string ApplicationUID = context.Request["ApplicationUID"].ToString();
                    //DateTime DataTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    //auditDataService.SubmitAudit(ApplicationUID, PointID, DataTime, DataTime, "", "", "", false, "");
                    #endregion
                }
            }
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}