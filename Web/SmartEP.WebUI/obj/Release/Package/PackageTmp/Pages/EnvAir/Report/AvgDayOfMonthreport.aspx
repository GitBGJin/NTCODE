<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AvgDayOfMonthreport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AvgDayOfMonthreport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function RefreshParent() {
                    this.parent.Refresh_Grid(true);
                    window.close();
                }
                //行编辑按钮 pageTypeID和waterOrAirType参数名称固定pageTypeID：该页面的ID;waterOrAirType：水或气，0：表示水，1：表示气
                function ShowDetails() {
                    //var uri = "CustomDialog.aspx?pageTypeID=AutoMonitorSystemMonthReport2 &waterOrAirType=0";
                    //window.radopen(encodeURI(uri), "DialogOpen");

                }
                //关闭遮罩层
                function closeWin() {
                    var bgObj = document.getElementById("divbgObj");
                    if (bgObj !== null)
                        document.body.removeChild(bgObj);
                }
                //遮罩层
                function alertWin() {
                    var iWidth = document.body.clientWidth;
                    var iHeight = document.body.clientHeight;

                    var bgObj = document.createElement("div");
                    bgObj.setAttribute("id", "divbgObj");
                    bgObj.style.cssText = "position:absolute;left:0px;top:0px;width:" + iWidth + "px;height:" + Math.max(document.body.clientHeight, iHeight) + "px;filter:Alpha(Opacity=30);opacity:0.3;background: url('../Images/login/BgSpliter.png');background-color:#FEFEFE;z-index:101;text-align:center; vertical-align:middle;color:red;";
                    var bgimg = document.createElement("img");
                    bgimg.setAttribute("src", "/Skins/Default/Ajax/loading.gif");
                    bgObj.appendChild(bgimg);
                    document.body.appendChild(bgObj);

                }
                window.onload = function () {
                    //遍历页面所有 按钮添加loading效果，目前测试 只用一个
                    //var target = document.getElementById("btnMonthReport");
                    var target1 = document.getElementById("btnExport2");
                    var type = "click";
                    var func = alertWin;
                    if (target1.addEventListener) {
                        target1.addEventListener(type, func, false);
                    } else if (target1.attachEvent) {
                        target1.attachEvent("on" + type, func);
                    } else {
                        target1["on" + type] = func;
                    }
                }
            </script>
        </telerik:RadCodeBlock>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="cboConfig">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="143px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%" class="Table_Customer" border="0">
                    <tr>
                        <td class="title" style="width: 80px">
                            <asp:ImageButton ID="btnExport2" OnClick="btnExport2_Click" runat="server" CssClass="RadToolBar_Customer" SkinID="ImgBtnSave" />
                        </td>
                        <td class="content"></td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px">测&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 点:</td>
                        <td class="content">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="360" CbxHeight="350" MultiSelected="true" DropDownWidth="410" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                        </td>
                    </tr>
                    <%--    <tr>
                        <td class="title" style="width: 80px">监测因子:</td>
                        <td class="content">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="360" DropDownWidth="410" ID="factorCbxRsm"></CbxRsm:FactorCbxRsm>
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="title" style="width: 80px;">月报时间：</td>
                        <td class="content">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadMonthYearPicker ID="rmypMonthTime" runat="server" Width="150px" OnSelectedDateChanged="rmypMonthTime_SelectedDateChanged" AutoPostBack="true"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                    </td>
                                    <td style="width: 0px;"></td>
                                    <td style="width: 0px;"></td>
                                    <td style="width: 0px;"></td>
                                    <td style="width: 26px;"></td>
                                    <td class="content">
                                        <asp:Label runat="server" ID="lblDateRange" Visible="false" Font-Italic="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px;"></td>
                        <td class="content">
                            <asp:Label runat="server" ID="lblRange"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="content" style="width: 80px;"></td>
                        <td class="content" style="text-align: center">
                            <asp:Button runat="server" ID="btnSave" Visible="false" Text="保  存" OnClick="btnSave_Click" OnClientClick="RefreshParent" />
                            <asp:Button runat="server" ID="btnExport" Visible="false" Text="下  载" OnClick="btnExport_Click" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="radWM" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="DialogOpen" runat="server" Height="510px" Width="780px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                    Title="站点因子分组" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>
    </form>
</body>
</html>
