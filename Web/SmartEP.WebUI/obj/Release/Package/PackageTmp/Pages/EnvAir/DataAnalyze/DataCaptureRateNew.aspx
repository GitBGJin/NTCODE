﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataCaptureRateNew.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.DataCaptureRateNew" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../../../Resources/JavaScript/FrameJS.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript" language="javascript">
                var myRadGrid;
                function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

                //页面刷新
                function Refresh_Grid(args) {
                    if (args) {
                        var MasterTable = $find("<%= grdDataCaptureRateNew.ClientID %>").get_masterTableView();
                        MasterTable.rebind();
                    }
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

                function OnClientClicking() {
                    date1 = $find("<%= rdpBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= rdpEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        //sender.set_autoPostBack(false);
                        return false;
                    }
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                function onRequestStart(sender, args) {
                    if (args.EventArgument == "")
                        return;
                    if (args.EventArgument == 0 || args.EventArgument == 1 ||
                        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                        args.set_enableAjax(false);
                    }
                }

                function RowClick(pointName, dtStart, dtEnd, factorCodes, ddlSel) {

                    var moduleGuide = "BF583993-F766-4F98-900D-E200A03F0291";
                    OpenFineUIWindow(moduleGuide, "Pages/EnvAir/DataAnalyze/AuditData.aspx?PointName=" + pointName + "&DTBegin=" + dtStart + "&DTEnd=" + dtEnd + "&Factors=" + factorCodes + "&ddlSel=" + ddlSel, "审核数据")
                }
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="grdDataCaptureRateNew">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDataCaptureRateNew" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDataCaptureRateNew" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%--<telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDataCaptureRateNew" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdDataCaptureRateNew" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsmSuper"  LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm"  LoadingPanelID="RadAjaxLoadingPanel1" />
                         <telerik:AjaxUpdatedControl ControlID="rbtnlType"  LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm"  LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCom"  LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        
                        <td class="title" style="width: 120px; text-align: center;">统计类型
                        </td>
                        <td class="content" style="width: 260px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" AutoPostBack="true"  RepeatLayout="Flow" RepeatColumns="2" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged">
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">站点:
                        </td>
                        <td class="content" style="width: 220px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="200" CbxHeight="350" MultiSelected="true" Visible="false" DefaultAllSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="200" CbxHeight="350" MultiSelected="true" Visible="true" DefaultAllSelected="true" DropDownWidth="520" ID="pointCbxRsmSuper"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="title" style="width: 150px; text-align: center;">监测因子:
                        </td>
                        <td class="content" style="width: 270px;">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="270" DropDownWidth="420"  ID="factorCbxRsm"></CbxRsm:FactorCbxRsm>
                            <telerik:RadComboBox ID="factorCom" runat="server" Visible="false" Width="270" SkinID="Default" Skin="Default" CheckBoxes="true" Localization-CheckAllString="全选" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="PM2.5" Value="a34004" />
                                    <telerik:RadComboBoxItem runat="server" Text="PM10" Value="a34002" />
                                    <telerik:RadComboBoxItem runat="server" Text="二氧化硫" Value="a21026" />
                                    <telerik:RadComboBoxItem runat="server" Text="二氧化氮" Value="a21004" />
                                    <telerik:RadComboBoxItem runat="server" Text="一氧化碳" Value="a21005" />
                                    <telerik:RadComboBoxItem runat="server" Text="臭氧" Value="a05024" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    <%--</tr>
                    <tr>--%>
                        <td class="title" style="width: 150px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 120px;">
                            <telerik:RadDatePicker ID="rdpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="110px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationStep="12" />
                        </td>
                        <td class="title" style="width: 150px; text-align: center;">结束时间:
                        </td>
                        <td class="content" style="width: 120px;">
                            <telerik:RadDatePicker ID="rdpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="110px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationStep="12" />
                        </td>
                        <td style="height: 25px; background-color: white; text-align: center">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="grdDataCaptureRateNew" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="true" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grdDataCaptureRateNew_NeedDataSource" OnItemDataBound="grdDataCaptureRateNew_ItemDataBound" OnColumnCreated="grdDataCaptureRateNew_ColumnCreated"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <div style="width: 100%; position: relative;">
                                <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                    runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                                <div style="position: absolute; right: 10px; top: 2px;">
                                    <table style="font-size: 12px">
                                        <tr>
                                            <td style="text-align: center">有效数据捕获率<br />
                                                有效运行时数：运行考核总时数
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </CommandItemTemplate>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" 
                            SaveScrollPosition="false"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <asp:HiddenField ID="hdValue" runat="server" />
    </form>
</body>
</html>
