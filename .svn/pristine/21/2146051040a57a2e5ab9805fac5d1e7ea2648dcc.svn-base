<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RealTimeAirQualityState.ascx.cs" Inherits="SmartEP.WebUI.Controls.RealTimeAirQualityState" %>
<html>
    <head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <script src="../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
        <style type="text/css">
            td{
                height:30px;
                font-family:微软雅黑;
            }
        </style>
    </head>
    <body>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneOrgan" runat="server" Height="100%" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
        <table style="margin:20px 30px;text-align: left;">
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblTitle" runat="server" Text="实时环境空气（AQI）" Font-Size="24px" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRegion" runat="server" Text="南通市实时环境空气质量" Font-Size="18px" ForeColor="White"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblHour" runat="server" Font-Size="18px" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="2">
                    <img id="imgSymbol" src="~/Resources/Images/HomePage/smile/3.png" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblAQIValue" runat="server" Text="123" Font-Size="74px" ForeColor="White"></asp:Label>
                </td>
                
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblClass" runat="server"  Font-Size="18px" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPrimaryPollutantTitle" runat="server" Text="首要污染物" Font-Size="18px" ForeColor="White"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblPrimaryPollutant" runat="server" Font-Size="18px"  ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPollutantValueTitle" runat="server" Text="浓度值" Font-Size="18px" ForeColor="White"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblPollutantValue" runat="server" Font-Size="18px"  ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <HR style="FILTER: progid:DXImageTransform.Microsoft.Shadow(color:#987cb9,direction:145,strength:15)" width="100%" color=#85dcff SIZE=0.5>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblLastTitle" runat="server" Text="昨日环境空气（AQI）" Font-Size="18px" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                
                <td>
                    <asp:Label ID="lblLastAQITitle" runat="server" Text="AQI指数" Font-Size="16px" ForeColor="White"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblLastAQIValue" runat="server"  Font-Size="16px" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLastClassTitle" runat="server" Text="空气质量等级" Font-Size="16px" ForeColor="White"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblLastClassValue" runat="server"  Font-Size="16px" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLastPrimaryPollutantTitle" runat="server" Text="首要污染物" Font-Size="16px" ForeColor="White"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblLastPrimaryPollutantValue" runat="server"  Font-Size="16px" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLastPollutantTitle" runat="server" Text="污染物浓度值" Font-Size="16px" ForeColor="White"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblLastPollutantValue" runat="server"  Font-Size="16px" ForeColor="White"></asp:Label>
                </td>
            </tr>
        </table>
                </telerik:RadPane>
            </telerik:RadSplitter>
    </body>
</html>