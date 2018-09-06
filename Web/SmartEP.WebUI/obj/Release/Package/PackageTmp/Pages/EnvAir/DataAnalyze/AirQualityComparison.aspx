﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualityComparison.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.AirQualityComparison" %>


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
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript" language="javascript">
                var myRadGrid;
                function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

                //页面刷新
                function Refresh_Grid(args) {
                    if (args) {
                        var MasterTable = $find("<%= grdAvgDayRange.ClientID %>").get_masterTableView();
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
                function OnClicking() {
                    //var reg = /^.+(msie|trident).{0,2}(\d+).*$/i;
                    //var result = reg.exec(navigator.userAgent);
                    if ((navigator.userAgent.indexOf('MSIE') >= 0) && (navigator.userAgent.indexOf('Opera') < 0)) {
                        var tmp = window.open("about:blank", "", "fullscreen=1")
                        tmp.moveTo(0, 0);
                        tmp.resizeTo(screen.width + 20, screen.height);
                        tmp.focus();
                        tmp.location = "http://221.178.131.85:5222/EQMSPortalZ/Show/ShowInterface.aspx";
                    } else {
                        alert("请切换成IE内核，方便打开视频！");
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
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="grdAvgDayRange">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgDayRange" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRTB"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgDayRange" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgDayRange" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvPoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="80px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 120px; text-align: center;">查询类型:
                        </td>
                        <td class="content" style="width: 120px;">
                            <asp:RadioButtonList ID="rbtnlType" Visible="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged" RepeatLayout="Flow" RepeatColumns="5">
                                <asp:ListItem Text="区域" Value="CityProper" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="测点" Value="Port"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">站点:
                        </td>
                        <td class="content" style="width: 300px;">
                            <div runat="server" visible="true" id="dvPoint" style="display: normal">
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" Visible="false" CbxWidth="220" CbxHeight="350"  MultiSelected="false" DropDownWidth="520" ID="pointCbxRsm" DefaultIPointMode="Region"></CbxRsm:PointCbxRsm>
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air"  CbxWidth="220" CbxHeight="350"  MultiSelected="true" DropDownWidth="520" ID="pointCbxRsmCity" DefaultIPointMode="Region"></CbxRsm:PointCbxRsm>
                            </div>
                        </td>
                        <td class="title" style="width: 80px">比对年份：
                        </td>
                        <td class="content" style="width: 100px;">
                            <telerik:RadComboBox runat="server" Visible="true" ID="Year" Width="90px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" Localization-CheckAllString="全选">
                            </telerik:RadComboBox>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">时间范围:
                        </td>
                        <td class="content" style="width: 360px;">
                            
                            <telerik:RadMonthYearPicker ID="rdpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="105px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                            &nbsp;&nbsp;至&nbsp;&nbsp;
                            
                            <telerik:RadMonthYearPicker ID="rdpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" Width="105px"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                        </td>
                        <td style="height: 25px; background-color: white; text-align: center" rowspan="1">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                            
                        </td>
                        <%--<td>
                            <asp:ImageButton ID="ImageButton1" runat="server" OnClientClick="return OnClicking()" SkinID="ImgBtnSearch" />
                        </td>--%>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="grdAvgDayRange" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="true" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound" OnColumnCreated="grdAvgDayRange_ColumnCreated"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToWordButton="true" ShowExportToExcelButton="true"  ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <div style="position: relative;">
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="WordQ" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                            <div style="position: absolute; right: 10px; top: 10px;">
                                            <b>区域站点选择请勾选同一市区的站点</b>
                                        </div>
                                </div>
                        </CommandItemTemplate>

                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="PM25" HeaderText="PM<sub>2.5</sub>平均浓度(μg/m³)"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="PM10" HeaderText="PM<sub>10</sub>平均浓度(μg/m³)"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="NO2" HeaderText="NO<sub>2</sub>平均浓度(μg/m³)"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="SO2" HeaderText="SO<sub>2</sub>平均浓度(μg/m³)"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="CO" HeaderText="CO平均浓度(mg/m³)"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="O3" HeaderText="O<sub>3</sub>-8小时平均浓度(μg/m³)"
                                HeaderStyle-HorizontalAlign="Center" />
                        </ColumnGroups>
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