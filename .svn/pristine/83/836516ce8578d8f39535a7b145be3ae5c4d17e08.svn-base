<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePageLZ.aspx.cs" Inherits="SmartEP.WebUI.Portal.HomePageLZ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>苏州太湖饮用水源地蓝藻自动监测预警平台</title>
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
        </script>
        <script type="text/javascript">
            $("document").ready(function () {
                $("#copyright").css({ "top": ($("body").height() - $("#copyright").height() - 5) + "px" });
                $("#wapper").css({ "height": ($("body").height()) + "px" });
            });
        </script>
                <div id="Container">
            <div id="wapper" class="divWapper">
                <div class="LogoLZ">
                </div>
                <div style="width: 850px; margin: auto; padding-top: 5%;" class="divJiCheng">
                    <table style="background-color: transparent; width: 100%;">
                        <tr>
                            <td>
                                <telerik:RadImageAndTextTile ID="RadImageWaterLZ" runat="server" Shape="Wide"
                                    ImageUrl="~/Resources/Images/HomePage/WaterLZImage.png" Width="132px" Height="174px" Target="_self" IsSeparator="true" BorderStyle="None">
                                    <PeekTemplateSettings Animation="Slide" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                    <PeekTemplate>
                                        <img src="../Resources/Images/HomePage/WaterLZImage.png" alt="" style="cursor: pointer;" />
                                    </PeekTemplate>
                                </telerik:RadImageAndTextTile>
                            </td>
                            <td style="width: 10%"></td>
                            <td>
                                <telerik:RadImageAndTextTile ID="RadImageGis" runat="server" Shape="Wide"
                                    ImageUrl="~/Resources/Images/HomePage/GISImage.png" Width="132px" Height="174px" Target="_blank" IsSeparator="true" BorderStyle="None">
                                    <PeekTemplateSettings Animation="Slide" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                    <PeekTemplate>
                                        <img src="../Resources/Images/HomePage/GISImage.png" alt="" style="cursor: pointer;" />
                                    </PeekTemplate>
                                </telerik:RadImageAndTextTile>
                            </td>
                            <td style="width: 10%"></td>
                            <td>
                                <telerik:RadImageAndTextTile ID="RadImageMgr" runat="server" Shape="Wide"
                                    ImageUrl="~/Resources/Images/HomePage/MgrImage.png" Width="132px" Height="174px" Target="_self" IsSeparator="true" BorderStyle="None">
                                    <PeekTemplateSettings Animation="Slide" ShowInterval="50000000" CloseDelay="5000" AnimationDuration="2000"
                                        ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                    <PeekTemplate>
                                        <img src="../Resources/Images/HomePage/MgrImage.png" alt="" style="cursor: pointer;" />
                                    </PeekTemplate>
                                </telerik:RadImageAndTextTile>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="copyright" style="position: absolute; left: 0px; top: 0px; color: #666; font-family: 微软雅黑; font-size: 14px; vertical-align: middle; overflow: hidden; text-align: center; height: auto; margin: 0; padding: 0; width: 100%;">
                <ul style="list-style: none;">
                    <li>苏州市环境监测中心 版权所有</li>
                    <li><a style="text-decoration: none; color: #666;" href="http://www.sinoyd.com">江苏远大信息股份有限公司</a> 技术支持</li>
                </ul>
            </div>
        </div>

    </form>
</body>
</html>
