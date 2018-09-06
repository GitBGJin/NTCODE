﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="SmartEP.WebUI.Portal.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>南通市环境质量监测监控系统</title>
        <link rel="prerender" href="http://218.91.209.251:1117///EQMSPortalNT/Pages/EnvAir/SuperStation/LadarBMPThermodynamic.aspx?Token=3F39B8DB7964E63723B489567A263463231EB91F2C2E39BC0DCBB5798A2DEA64F658521A5B3AC1AA91A59BB9E500497C" />

    <link href="../Resources/CSS/HomePage.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/JavaScript/JQuery/jquery-1.9.0.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <script type="text/javascript">
            if (top != self) {
                if (top.location != self.location)
                    top.location = self.location;
            }
            $("document").ready(function () {
                $("#copyright").css({ "top": ($("body").height() - $("#copyright").height() - 5) + "px" });
                $("#wapper").css({ "height": ($("body").height()) + "px" });
            });
            function Logout(sender, args) {
                
                if (!window.confirm("你确定要退出系统？"))
                    args.set_cancel(true);
            }
        </script>
        <div id="Container">
            <div id="wapper" class="divWapper">
                
                <div class="TitleBg">
                    <div class="Logo">
                    </div>
                    <div class="TitleRight">
                        <div style="float:right;top:10px;position:relative;background-color: transparent;">
                        <table style="background-color: transparent; width: 100%;">
                            <tr>
                                <td>
                                    <div>
                                    <img src="../Resources/Images/HomePage/img_portrait.png" />
                                    </div>
                                    
                                </td>
                                <td>
                                    <span runat="server" id="WelcomeTitle" style="color:#fff"></span>
                                </td>
                                <td>
                                    <div class="divJiCheng">
                                        <telerik:RadImageAndTextTile ID="RadImageOut" OnClientClicking=" Logout" runat="server" Width="107px" Height="26px" BorderStyle="None"
                                             ImageUrl="~/Resources/Images/HomePage/icon_out.png">
                                            <PeekTemplateSettings Animation="None" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                            <PeekTemplate>
                                                <img src="../Resources/Images/HomePage/icon_out_highlight.png" alt="" style="cursor: pointer;" />
                                            </PeekTemplate>
                                            <PeekTemplateSettings />
                                        </telerik:RadImageAndTextTile>
                                    </div>
                                </td>
                                <td>
                                    <div class="divJiCheng">
                                    <telerik:RadImageAndTextTile ID="RadImageMgr" runat="server" Shape="Wide" ToolTip="后台管理"
                                        ImageUrl="~/Resources/Images/HomePage/icon_sstting.png" Width="31px" Height="31px" Target="_self" IsSeparator="true" BorderStyle="None"  >
                                        <PeekTemplateSettings Animation="None" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                        <PeekTemplate>
                                            <img src="../Resources/Images/HomePage/icon_sstting_highlight.png" alt="" style="cursor: pointer;" />
                                        </PeekTemplate>
                                    </telerik:RadImageAndTextTile>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        </div>
                    </div>
                </div>
                
                <div style="width: 90%; margin-left:100px; padding-top: 5%;" class="divJiCheng">
                    <table style="background-color: transparent; width: 100%;">
                        <tr>
                            <td>
                                
                                <telerik:RadImageAndTextTile ID="RadImageSuperStation" runat="server" Shape="Wide"
                                    ImageUrl="~/Resources/Images/HomePage/grey/超级站管理.png?t=1" Width="166px" Height="166px" Target="_self" IsSeparator="true" BorderStyle="None" >
                                    <PeekTemplateSettings Animation="None" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                    <PeekTemplate>
                                        <img src="../Resources/Images/HomePage/green/超级站管理.png?t=1" alt="" style="cursor: pointer;" />
                                    </PeekTemplate>
                                </telerik:RadImageAndTextTile>
                                <%--<div class="images-content"  style="left:140px;top:250px;">
                                    <asp:label ID="lblSuperStation2" runat="server" Text="超级站管理" />
                                </div>--%>
                            </td>
                            <td>
                                <telerik:RadImageAndTextTile ID="RadImageGeneralPara" runat="server" Shape="Wide"
                                    ImageUrl="~/Resources/Images/HomePage/grey/常规参数.png?t=1" Width="166px" Height="166px" Target="_self" IsSeparator="true" BorderStyle="None">
                                    <PeekTemplateSettings Animation="None" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                    <PeekTemplate>
                                        <img src="../Resources/Images/HomePage/green/常规参数.png?t=1" alt="" style="cursor: pointer;" />
                                    </PeekTemplate>
                                </telerik:RadImageAndTextTile>
                            </td>
                            <td>
                                <telerik:RadImageAndTextTile ID="RadImageRelevance" runat="server" Shape="Wide"
                                    ImageUrl="~/Resources/Images/HomePage/grey/关联性分析.png" Width="166px" Height="166px" Target="_self" IsSeparator="true" BorderStyle="None">
                                    <PeekTemplateSettings Animation="None" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                    <PeekTemplate>
                                        <img src="../Resources/Images/HomePage/green/关联性分析.png" alt="" style="cursor: pointer;" />
                                    </PeekTemplate>
                                </telerik:RadImageAndTextTile>

                            </td>
                            <td rowspan="3">
                                <div id="divAQI" class="bg_AQI" runat="server">
                                    
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadImageAndTextTile ID="RadImageAudit" runat="server" Shape="Wide"
                                    ImageUrl="~/Resources/Images/HomePage/grey/数据审核.png" Width="166px" Height="166px" Target="_self" IsSeparator="true" BorderStyle="None" >
                                    <PeekTemplateSettings Animation="None" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                    <PeekTemplate>
                                        <img src="../Resources/Images/HomePage/green/数据审核.png" alt="" style="cursor: pointer;" />
                                    </PeekTemplate>
                                </telerik:RadImageAndTextTile>
                            </td>
                            <td>
                                <telerik:RadImageAndTextTile ID="RadImageSysConfig" runat="server" Shape="Wide"
                                    ImageUrl="~/Resources/Images/HomePage/grey/系统信息.png" Width="166px" Height="166px" Target="_self" IsSeparator="true" BorderStyle="None">
                                    <PeekTemplateSettings Animation="None" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                    <PeekTemplate>
                                        <img src="../Resources/Images/HomePage/green/系统信息.png" alt="" style="cursor: pointer;" />
                                    </PeekTemplate>
                                </telerik:RadImageAndTextTile>
                            </td>
                            <td>
                                <telerik:RadImageAndTextTile ID="RadImageStatistic" runat="server" Shape="Wide"
                                    ImageUrl="~/Resources/Images/HomePage/grey/统计.png" Width="166px" Height="166px" Target="_self" IsSeparator="true" BorderStyle="None">
                                    <PeekTemplateSettings Animation="None" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                    <PeekTemplate>
                                        <img src="../Resources/Images/HomePage/green/统计.png" alt="" style="cursor: pointer;" />
                                    </PeekTemplate>
                                </telerik:RadImageAndTextTile>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadImageAndTextTile ID="RadImageGIS" runat="server" Shape="Wide"
                                    ImageUrl="~/Resources/Images/HomePage/grey/GIS.png" Width="166px" Height="166px" Target="_self" IsSeparator="true" BorderStyle="None">
                                    <PeekTemplateSettings Animation="None" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                    <PeekTemplate>
                                        <img src="../Resources/Images/HomePage/green/GIS.png" alt="" style="cursor: pointer;" />
                                    </PeekTemplate>
                                </telerik:RadImageAndTextTile>
                            </td>
                            <td>
                                <telerik:RadImageAndTextTile ID="RadImageVideo" runat="server" Shape="Wide"  Visible="true"
                                    ImageUrl="~/Resources/Images/HomePage/grey/报表管理.png" Width="166px" Height="166px" Target="_self" IsSeparator="true" BorderStyle="None">
                                    <PeekTemplateSettings Animation="None" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                    <PeekTemplate>
                                        <img src="../Resources/Images/HomePage/green/报表管理.png" alt="" style="cursor: pointer;" />
                                    </PeekTemplate>
                                </telerik:RadImageAndTextTile>
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                    </table>
                    
                </div>
            </div>
            <div id="copyright" style="position: absolute; left: 0px; top: 0px; color: #666; font-family: 微软雅黑; font-size: 14px; vertical-align: middle; overflow: hidden; text-align: center; height: auto; margin: 0; padding: 0; width: 100%;">
                <ul style="list-style: none;">
                    <li>南通市环境监测中心 版权所有</li>
                    <li><a style="text-decoration: none; color: #666;" href="http://www.sinoyd.com">江苏远大信息股份有限公司</a> 技术支持</li>
                </ul>
            </div>
        </div>
    </form>
</body>
</html>

