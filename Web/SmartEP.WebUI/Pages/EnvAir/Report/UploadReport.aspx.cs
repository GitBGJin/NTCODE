using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class UploadReport : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        ReportLogService ReportLogService = new ReportLogService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["id"] = PageHelper.GetQueryString("id");
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["id"].ToString() != "")
                {
                    int id = int.TryParse(ViewState["id"].ToString(), out id) ? id : 0;
                    /// '状态信息
                    System.Text.StringBuilder strMsg = new System.Text.StringBuilder();
                    strMsg.Append("上传的文件内容是：<hr color=red>");
                    ///'检查文件扩展名字
                    HttpPostedFile postedFile = fileuolp.PostedFile;
                    string fileName, fileExtension;
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    if (fileName != "")
                    {
                        var customDatum = ReportLogService.ReportLogRetrieveByid(id).FirstOrDefault();
                        string pathReport = customDatum.ReportName;
                        string pageid = customDatum.PageTypeID;
                        string[] pathReportarry = pathReport.Split('(');
                        pathReport = pathReportarry[0];
                        fileExtension = System.IO.Path.GetExtension(fileName);
                        strMsg.Append("上传的文件类型：" + postedFile.ContentType.ToString() + "<br />");
                        strMsg.Append("客户端文件地址：" + postedFile.FileName + "<br />");
                        strMsg.Append("上传文件的文件名：" + fileName + "<br />");
                        strMsg.Append("上传文件的扩展名：" + fileExtension + "<br /></hr>");
                        string path = Server.MapPath(pathReport);
                        DateTime dt = DateTime.Now;
                        string newname = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + pageid + fileExtension;
                        if (pageid == "RoutineMonthReport")
                        {
                            if (fileExtension == ".mdb")
                            {
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);//创建新路径
                                }
                                ///将上传文件保存到指定的文件夹
                                postedFile.SaveAs(path + newname);

                                customDatum.ReportName = pathReport + newname;
                                ReportLogService.ReportLogUpdate(customDatum);
                                //Alert("上传成功！");
                                RegisterScript("RefreshParent();");
                            }
                            else
                            {
                                Alert("选择文件格式不正确！");
                            }
                        }
                        else if (pageid == "RunningMonthReport" || pageid == "MonthReport" || pageid == "AQI-SECDayReportNew" || pageid == "AirQualityWeekReport")
                        {
                            if (fileExtension == ".doc" || fileExtension == ".docx")
                            {
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);//创建新路径
                                }
                                ///将上传文件保存到指定的文件夹
                                postedFile.SaveAs(path + newname);

                                customDatum.ReportName = pathReport + newname;
                                ReportLogService.ReportLogUpdate(customDatum);
                                //Alert("上传成功！");
                                RegisterScript("RefreshParent();");
                            }
                            else
                            {
                                Alert("选择文件格式不正确！");
                            }
                        }
                        else
                        {
                            if (fileExtension == ".xls" || fileExtension == ".xlsx")
                            {
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);//创建新路径
                                }
                                ///将上传文件保存到指定的文件夹
                                postedFile.SaveAs(path + newname);

                                customDatum.ReportName = pathReport + newname;
                                ReportLogService.ReportLogUpdate(customDatum);
                                //Alert("上传成功！");
                                RegisterScript("RefreshParent();");
                            }
                            else
                            {
                                Alert("选择文件格式不正确！");
                            }
                        }
                    }
                    else
                    {
                        Alert("请选择文件！");
                    }
                }
                else
                {
                    Alert("未能发现该条记录！");
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnMonthReport_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                if (ViewState["id"].ToString() != "")
                {
                    int id = int.TryParse(ViewState["id"].ToString(), out id) ? id : 0;
                    /// '状态信息
                    System.Text.StringBuilder strMsg = new System.Text.StringBuilder();
                    strMsg.Append("上传的文件内容是：<hr color=red>");
                    ///'检查文件扩展名字
                    HttpPostedFile postedFile = fileuolp.PostedFile;
                    string fileName, fileExtension;
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    if (fileName != "")
                    {
                        var customDatum = ReportLogService.ReportLogRetrieveByid(id).FirstOrDefault();
                        string pathReport = customDatum.ReportName;
                        string pageid = customDatum.PageTypeID;
                        string[] pathReportarry = pathReport.Split('(');
                        pathReport = pathReportarry[0];
                        fileExtension = System.IO.Path.GetExtension(fileName);
                        strMsg.Append("上传的文件类型：" + postedFile.ContentType.ToString() + "<br />");
                        strMsg.Append("客户端文件地址：" + postedFile.FileName + "<br />");
                        strMsg.Append("上传文件的文件名：" + fileName + "<br />");
                        strMsg.Append("上传文件的扩展名：" + fileExtension + "<br /></hr>");
                        string path = Server.MapPath(pathReport);
                        DateTime dt = DateTime.Now;
                        string newname = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + pageid + fileExtension;
                        if (pageid == "RoutineMonthReport")
                        {
                            if (fileExtension == ".mdb")
                            {
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);//创建新路径
                                }
                                ///将上传文件保存到指定的文件夹
                                postedFile.SaveAs(path + newname);

                                customDatum.ReportName = pathReport + newname;
                                ReportLogService.ReportLogUpdate(customDatum);
                                Alert("上传成功！");
                                RegisterScript("RefreshParent();");
                            }
                            else
                            {
                                Alert("选择文件格式不正确！");
                            }
                        }
                        else if (pageid == "RunningMonthReport" || pageid == "MonthReport" || pageid == "AQI-SECDayReportNew" || pageid == "AirQualityWeekReport")
                        {
                            if (fileExtension == ".doc" || fileExtension == ".docx")
                            {
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);//创建新路径
                                }
                                ///将上传文件保存到指定的文件夹
                                postedFile.SaveAs(path + newname);

                                customDatum.ReportName = pathReport + newname;
                                ReportLogService.ReportLogUpdate(customDatum);
                                Alert("上传成功！");
                                RegisterScript("RefreshParent();");
                            }
                            else
                            {
                                Alert("选择文件格式不正确！");
                            }
                        }
                        else
                        {
                            if (fileExtension == ".xls" || fileExtension == ".xlsx")
                            {
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);//创建新路径
                                }
                                ///将上传文件保存到指定的文件夹
                                postedFile.SaveAs(path + newname);

                                customDatum.ReportName = pathReport + newname;
                                ReportLogService.ReportLogUpdate(customDatum);
                                Alert("上传成功！");
                                RegisterScript("RefreshParent();");
                            }
                            else
                            {
                                Alert("选择文件格式不正确！");
                            }
                        }
                    }
                    else
                    {
                        Alert("请选择文件！");
                    }
                }
                else
                {
                    Alert("未能发现该条记录！");
                }
            }
            catch (Exception ex)
            {
            }

        }
    }
}