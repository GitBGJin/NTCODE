﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.Update" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 269px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <div id="AuditSubmitDiv" style="display: none; height: 390px; width: 1200px; vertical-align: middle; text-align: center; background-color: white; opacity: 0.7; filter: alpha(opacity=70); z-index: 100; position: absolute;">
            <p style="text-align: center; vertical-align: middle; padding-top: 20%; font-weight: bold; font-size: 18px; color: #b4aa38;">正在同步...请勿关闭页面</p>
        </div>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="120px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">同步站点:
                        </td>
                        <td class="content">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="300" CbxHeight="350" MultiSelected="true" DropDownWidth="400" ID="pointCbxRsm"
                                DefaultIPointMode="Region"></CbxRsm:PointCbxRsm>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 100px">同步因子:
                        </td>
                        <td class="content">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="300" DropDownWidth="400" ID="factorCbxRsm"></CbxRsm:FactorCbxRsm>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">同步时间:
                        </td>
                        <td class="content">
                            <%--<telerik:RadDatePicker ID="dtpBegin" runat="server" Visible="true" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" Width="140px" />--%>
                            <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                            至
                            <%--<telerik:RadDatePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" Visible="true" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" Width="140px" />--%>
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                            &nbsp &nbsp &nbsp
                            <telerik:RadButton ID="UpdateMessage" runat="server" Text="手动同步" OnClick="UpdateMessage_Click" OnClientClicking="btnSyncClicked"></telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
            <script src="../../../Resources/JavaScript/Echarts/build/dist/echarts.js"></script>
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>

            <script type="text/javascript">
                function btnSyncClicked(sender, args) {
                    $("#AuditSubmitDiv").css("display", "");
                }
                $(document).ready(function () {
                    ResizePageDiv();//设置蒙版div的高度、宽度
                });
                function ResizePageDiv() {
                    var bodyWidth = document.body.clientWidth;
                    var bodyHeight = document.body.clientHeight;
                    $('#pagediv').css("height", bodyHeight);
                    $('#pagediv').css("width", bodyWidth);
                    $('#AuditSubmitDiv').css("height", bodyHeight);
                    $('#AuditSubmitDiv').css("width", bodyWidth);
                }
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
