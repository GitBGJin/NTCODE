<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RealTimeAirQualityState.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Dock.RealTimeAirQualityState" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript" language="javascript">
                var myRadGrid;
                function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

                function OnClientClicking() {
                    return true;
                }

                //控制导出时按钮不会隐藏掉处理
                function onRequestStart(sender, args) {
                    if (args.EventArgument == 6 || args.EventArgument == 7 ||
                        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                        args.set_enableAjax(false);
                    }
                }

                function RefreshWindow() {
                    window.location.href = window.location.href;
                }

                var intervalObj;//定时刷新对象

                $(function () {
                    AutoRefresh();
                });

                //自动刷新
                function AutoRefresh() {
                    var curMinture = new Date().getMinutes();//当前分钟
                    var intervalValue = (60 - curMinture) * 60 * 1000; //间隔执行时间，换算成毫秒
                    intervalObj = window.setTimeout(SearchData, intervalValue); //启动计时器，1秒执行一次//向后台发送处理数据                    
                }

                //timer处理函数
                function SearchData() {
                    var curMinture = new Date().getMinutes();//当前分钟
                    var intervalValue;//间隔执行时间，换算成毫秒
                    if (curMinture <= 5 || curMinture >= 55) {
                        intervalValue = 60 * 60 * 1000;
                        document.getElementById("<%=btnSearch.ClientID%>").click();
                    }
                    else {
                        intervalValue = (60 - curMinture) * 60 * 1000;
                    }
                    window.clearTimeout(intervalObj);
                    intervalObj = window.setTimeout(SearchData, intervalValue); //启动计时器，1秒执行一次//向后台发送处理数据 
                }

                function GetData() {
                    var obj = new Object();
                    obj.CityTypeUids = document.getElementById("<%=hdCityTypeUids.ClientID%>").value;
                    return obj;
                }
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="tbOrgan" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdCityTypeUids" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneOrgan" runat="server" Height="100%" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="tbOrgan" style="width: 96%; height: 100%; font-size: 18px; text-align: left; padding-left: 5px; margin: 10px auto;" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0" runat="server">
                    <%--<tr>
                        <td class="title" style="width: 50px; text-align: center;" colspan="2">
                            <span style="font-weight: bold; float: left;">实时空气质量状况</span>
                        </td>
                        <td class="content">
                            <span style="float: right;">
                                <asp:Button ID="btnRefresh" runat="server" Text="刷新" OnClientClick="RefreshWindow();return false;" />
                                <asp:Button ID="btnDetail" runat="server" Text="详细" OnClick="btnDetail_Click" />
                            </span>
                            <span style="clear: both;"></span>
                        </td>
                    </tr>--%>
                    <tr style="height: 50px;">
                        <td class="title" style="text-align: center; min-width: 160px; max-height: 50px;">
                            <asp:Label ID="lblRegionName" runat="server" Text="南通市区" Font-Size="28px" ForeColor="#3399ff"></asp:Label>
                        </td>
                        <td class="content" style="min-width: 150px; text-align: left;" colspan="2">
                            <asp:Label ID="lblLastIssuedTime" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="min-height: 80px;">
                            <asp:Label ID="lblAQIValue" runat="server" Font-Size="100px"></asp:Label>
                        </td>
                        <td class="content" style="text-align: center; font-size: 22px;">
                            <asp:Label ID="lblClass" runat="server" Height="30px"></asp:Label>
                            <br />
                            <asp:Label ID="lblAQITitle" runat="server" Text="AQI" Height="30px"></asp:Label>
                        </td>
                        <td rowspan="4" style="vertical-align: top;">
                            <div style="word-wrap: break-word; word-break: break-all; max-width: 225px; margin: 0 auto;">
                                <asp:Label ID="lblHealthEffect" runat="server"></asp:Label>
                                <%--<asp:Image ID="imgHealthEffect" runat="server" ImageUrl="~\Resources\Images\temp\实时空气质量首页元件图片.png"
                                    Style="max-width: 140px;" />--%>
                                <div style="font-size: 14px;">
                                    <telerik:RadImageAndTextTile ID="imgHealthEffect" runat="server" ImageUrl="~\Resources\Images\temp\实时空气质量首页元件图片.png"
                                        Title-Text="对健康的影响"
                                        Shape="Wide" Width="220px" Height="148px" ForeColor="White" ImageWidth="200px" ImageHeight="128px">
                                    </telerik:RadImageAndTextTile>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                            <div style="height: 5px;"></div>
                            <div style="width: 220px; border: 1px solid orange; font-size: 14px; margin: 0 auto;">
                                <div style="word-wrap: break-word; word-break: break-all; min-height: 70px; display: table-cell; vertical-align: middle; padding: 2px 5px 2px 5px;">
                                    <asp:Label ID="lblTakeStep" runat="server"></asp:Label>
                                </div>
                                <div style="background-color: orange; color: white; height: 30px; line-height: 30px; padding: 2px 5px 2px 5px;">
                                    <div style="height: 18px;">建议采取的措施</div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr style="height: 50px;">
                        <td class="title" style="max-height: 30px; text-align: left; padding-left: 20px;">
                            <asp:Label ID="lblPrimaryPollutantTitle" runat="server" Text="首要污染物"></asp:Label>
                        </td>
                        <td class="content" style="text-align: left;">
                            <asp:Label ID="lblPrimaryPollutant" runat="server" Font-Size="22px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 50px;">
                        <td class="title" style="max-height: 30px; text-align: left; padding-left: 20px;">
                            <asp:Label ID="lblPollutantValueTitle" runat="server" Text="浓度值"></asp:Label>
                        </td>
                        <td class="content" style="text-align: left;">
                            <asp:Label ID="lblPollutantValue" runat="server" Font-Size="22px"></asp:Label>
                            &nbsp;
                            <asp:Label ID="lblUnit" runat="server" Text="毫克/立方米" Font-Size="16px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"></td>
                    </tr>
                </table>
                <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" Style="display: none;" />
                <asp:HiddenField ID="hdCityTypeUids" runat="server" />
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
