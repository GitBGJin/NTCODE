﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualittyAvgStatistical.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirQualittyAvgStatistical" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript" language="javascript">
                var myRadGrid;
                function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }
               

                var isfirst = 1;//是否首次加载
                function TabSelected(sender, args) {
                    try {
                        var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                        var text = $.trim(args._tab._element.innerText);
                        if (text == '图表') {
                            if (isfirst.value == "1") {
                                isfirst.value = 0;
                                CreatChart();
                            }
                        }
                    } catch (e) {
                    }
                }
                //页面刷新
                function Refresh_Grid(args) {
                    if (args) {
                        var MasterTable = $find("<%= gridAvgStatistical.ClientID %>").get_masterTableView();
                        MasterTable.rebind();
                    }
                }
                
                //绘图
                function CreatChart() {
                    $('#container').html("");
                    var pointIds = document.getElementById("hdPointId").value;
                    var dtBegion = document.getElementById("hddtBegion").value;
                    var dtEnd = document.getElementById("hddtEnd").value;
                    var quality = document.getElementById("hdQuality").value;
                    var chartType = document.getElementById("hdChartType").value;
                    var chartContent = document.getElementById("hdChartContent").value;
                    var dsType = document.getElementById("hdDSType").value;

                    var chartdiv = "";
                    //$.each(pointIds.split(','), function (chartNo, value) {
                    chartdiv = "";
                    chartdiv += '<div style=" width:100%; height:600px;">';
                    chartdiv += '<iframe name="chartIframe" id="frame' + Math.random() + '" src="../Chart/AirQualittyAvgStatisticalChart.aspx?pointIds=' + pointIds + '&quality=' + quality + '&chartType=' + chartType + '&dsType=' + dsType + '&chartContent=' + chartContent + '&dtBegion=' + dtBegion + '&dtEnd=' + dtEnd + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                    chartdiv += '</div>';
                    $("#container").append(chartdiv);

                    //});
                    $("#container").css("overflow-y", "auto");
                    $("#container").height("650px");//设置图表Iframe的高度
                    $("#container").width("100%");//设置图表Iframe的宽度
                }

                function OnClientClicking() {
                    var rbl = document.getElementsByName("radlDataType");
                    for (var i = 0; i < rbl.length; i++) {
                        if (rbl[i].checked && rbl[i].value == "Hour") {
                            var hourB = $find("<%= hourBegin.ClientID %>").get_selectedDate();
                            var hourE = $find("<%= hourEnd.ClientID %>").get_selectedDate();
                            if ((hourB == null) || (hourE == null)) {
                                alert("开始时间或者终止时间，不能为空！");
                                return false;
                            }
                            if (hourB > hourE) {
                                alert("开始时间不能大于终止时间！");
                                return false;
                            } else {
                                return true;
                            }
                        }
                        else if (rbl[i].checked && rbl[i].value == "Day") {
                            var dayB = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                            var dayE = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                            if ((dayB == null) || (dayE == null)) {
                                alert("开始时间或者终止时间，不能为空！");
                                return false;
                            }
                            if (dayB > dayE) {
                                alert("开始时间不能大于终止时间！");
                                return false;
                            } else {
                                return true;
                            }
                        }
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
            function PointFactor(item) {
                try {
                    CreatChart();
                } catch (e) {
                }
            }
            window.onload = function () {
                //document.getElementById("<%= dtpBegin.ClientID %>").onchange = CheckDateIsEmpty;
                //    document.getElementById("<%= dtpEnd.ClientID %>").onchange = CheckDateIsEmpty;
                }

                function CheckDateIsEmpty() {
                    //var dtpBeginfind = $find("<%= dtpBegin.ClientID %>");
                    //var dtpEndfind = $find("<%= dtpEnd.ClientID %>");
                    //var dateBegin = dtpBeginfind.get_selectedDate();
                    //var dateEnd = dtpEndfind.get_selectedDate();
                    //var newDate = new Date();
                    //var isAlert = false;
                    //if (dateBegin == null) {
                    //    if (dateEnd != null && newDate > dateEnd) {
                    //        newDate = dateEnd;
                    //    }
                    //    dtpBeginfind.set_selectedDate(new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate()));
                    //    isAlert = true;
                    //}
                    //if (dateEnd == null) {
                    //    if (dateBegin != null && newDate < dateBegin) {
                    //        newDate = dateBegin;
                    //    }
                    //    dtpEndfind.set_selectedDate(new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate()));
                    //    isAlert = true;
                    //}
                    //if (isAlert) {
                    //    alert('开始时间或者终止时间，不能为空！');
                    //}
                }

               
                function onRequestEnd(sender, args) {
                }
                function ClientButtonClicking(sender, args) {
                  var uri = "AQIDataExplain.aspx";
                  var oWindow = window.radopen(uri, "AQIDataExplainDetail");
                  var ds = 1;
                }
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <asp:HiddenField ID="FirstLoadChart" Value="1" runat="server" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridAvgStatistical">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAvgStatistical" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridAvgStatisticalNew">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAvgStatisticalNew" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" />
                        <telerik:AjaxUpdatedControl ControlID="gridAvgStatistical" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAvgStatistical" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="gridAvgStatistical" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="gridAvgStatisticalNew" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <%--<telerik:AjaxUpdatedControl ControlID="hourEnd" LoadingPanelID="RadAjaxLoadingPanel1" />--%>
                        <telerik:AjaxUpdatedControl ControlID="hdQuality" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdCharType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartContent" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdDSType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="container" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="tabStrip">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="gridAvgStatistical" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="gridAvgStatisticalNew" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdQuality" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdCharType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartContent" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdDSType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="container" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" />
                        <telerik:AjaxUpdatedControl ControlID="tbHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="tbDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ddlDataFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChartContent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartContent"  LoadingPanelID="RadAjaxLoadingPanel1"/>
                        <telerik:AjaxUpdatedControl ControlID="hdChartContent" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="dvPoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvCityproper" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvCity" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvCityMode" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <td class="title" style="width: 120px; text-align: center;">查询范围:
                        </td>
                        <td class="content" style="width: 200px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged" RepeatLayout="Flow" RepeatColumns="5">
                                <asp:ListItem Text="区域" Value="CityProper" ></asp:ListItem>
                                <asp:ListItem Text="测点" Value="Port" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">站点:
                        </td>
                        <td class="content" style="width: 200px;">
                            <div runat="server" id="dvPoint" >
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="220" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm" DefaultIPointMode="Region"></CbxRsm:PointCbxRsm>
                            </div>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">质量类别:
                        </td>
                        <td class="content" style="width: 300px;">
                            <telerik:RadComboBox runat="server" ID="rcbCityProper" Width="280px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                                Localization-CheckAllString="选择全部">
                                <Items>
                                    <telerik:RadComboBoxItem Text="优" Value="1" Checked="true" />
                                    <telerik:RadComboBoxItem Text="良" Value="2" Checked="true" />
                                    <telerik:RadComboBoxItem Text="轻度污染" Value="3" Checked="true" />
                                    <telerik:RadComboBoxItem Text="中度污染" Value="4" Checked="true" />
                                    <telerik:RadComboBoxItem Text="重度污染" Value="5" Checked="true" />
                                    <telerik:RadComboBoxItem Text="严重污染" Value="6" Checked="true" />
                                    <telerik:RadComboBoxItem Text="无效天" Value="7" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlDataFrom" runat="server" Width="90px" AutoPostBack="true">
                                    <Items>
                                        <telerik:DropDownListItem Text="原始数据" Value="OriData" Selected="true" />
                                        <telerik:DropDownListItem Text="审核数据" Value="AuditData" />
                                    </Items>
                                </telerik:RadDropDownList>
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server"  OnClick="btnSearch_Click" OnClientClick="return OnClientClicking()" SkinID="ImgBtnSearch" />
<%--                            OnClientClick="return OnClientClicking()"--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 120px; text-align: center;">数据类型:
                        </td>
                        <td class="content" style="width: 200px;">
                            <asp:RadioButtonList ID="radlDataType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="时段均值" Value="Hour" Selected="True" />
                                <%--<asp:ListItem Text="日均值" Value="Day"/>--%>
                            </asp:RadioButtonList>
                        </td>
                        <td colspan="4">
                            <table runat="server" id="tbHour">
                                <tr>
                                    <td class="title" style="width: 120px; text-align: center;">开始时间:
                                    </td>
                                    <td class="content" style="width: 200px;">
                                        <telerik:RadDateTimePicker ID="hourBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                                    </td>
                                    <td class="title" style="width: 120px; text-align: center;">结束时间:</td>
                                    <td class="content" style="width: 300px;">
                                        <telerik:RadDateTimePicker ID="hourEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            TimeView-HeaderText="小时" Calendar-FastNavigationStep="12"  />
                                    </td>
                                </tr>
                            </table>
                            <table runat="server" id="tbDay" visible="false">
                                <tr>
                                    <td class="title" style="width: 120px; text-align: center;">开始时间:
                                    </td>
                                    <td class="content" style="width: 200px;">
                                        <telerik:RadDatePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                            DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12" 
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" Width="120px" />
                                    </td>
                                    <td class="title" style="width: 120px; text-align: center;">结束时间:</td>
                                    <td class="content" style="width: 300px;">
                                        <telerik:RadDatePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                            DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12" 
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" Width="120px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneTab" runat="server" Scrolling="None" Width="100%" Height="26px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadTabStrip ID="tabStrip" runat="server" SelectedIndex="0" MultiPageID="multiPage"
                    CssClass="RadTabStrip_Customer" OnClientTabSelected="TabSelected">
                    <Tabs>
                        <telerik:RadTab Text="列表">
                        </telerik:RadTab>
                        <telerik:RadTab Text="图表">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="grvGrid" runat="server" Visible="true">
                <telerik:RadGrid ID="gridAvgStatistical" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <div style="position: relative;">
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                       
                                <div style="position: absolute; right: 10px; top: 2px;">
                                            <b>城市日AQI的评价项目包括：SO<sub>2</sub>、NO<sub>2</sub>、CO、PM<sub>10</sub>、PM<sub>2.5</sub>的24小时平均和O<sub>3</sub>日最大8小时滑动平均等6个指标。<br />当且仅当时间范围为24h，并且范围是0点到23点的时候，臭氧8h按照日数据计算，其他时间段臭氧8h取24笔数据计算，并且取其中的最大值。</b>
                                        </div>
                                </div>
                        </CommandItemTemplate>
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="污染物浓度及空气质量分指数（IAQI）" HeaderText="污染物浓度及空气质量分指数（IAQI）"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="PM2.5" HeaderText=""
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="PM10" HeaderText=""
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="NO2" HeaderText=""
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="SO2" HeaderText=""
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="CO" HeaderText=""
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="O3" HeaderText=""
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <%--<telerik:GridColumnGroup Name="O38NT" HeaderText="臭氧(O<sub>3</sub>)最近8小时滑动平均值"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>--%>
                            <telerik:GridColumnGroup Name="空气质量指数类别" HeaderText="空气质量指数类别"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>
                            <%--   <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridBoundColumn HeaderText="监测点位名称" UniqueName="PointId" DataField="PointId" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="PM25" DataField="PM25" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="PM25_AQI" DataField="PM25_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="PM10" DataField="PM10" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="PM10_AQI" DataField="PM10_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="NO2" DataField="NO2" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="NO2_AQI" DataField="NO2_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="SO2" DataField="SO2" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="SO2_AQI" DataField="SO2_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(mg/m<sup>3</sup>)" UniqueName="CO" DataField="CO" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" HeaderStyle-Width="100px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="CO_AQI" DataField="CO_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="O3" DataField="O3" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O3" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="O3_AQI" DataField="O3_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O3" HeaderStyle-Width="60px" />
<%--                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="Max8HourO3NT" DataField="Max8HourO3NT" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O38NT" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="Max8HourO3NT_AQI" DataField="Max8HourO3NT_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O38NT" HeaderStyle-Width="60px" />--%>
                            <telerik:GridBoundColumn HeaderText="空气质量<br />指数(AQI)" UniqueName="Max_AQI" DataField="Max_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                            <telerik:GridBoundColumn HeaderText="首要污染物" UniqueName="PrimaryPollutant" DataField="PrimaryPollutant" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                            <telerik:GridBoundColumn HeaderText="空气质量<br />指数级别" UniqueName="Grade" DataField="Grade" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                            <telerik:GridBoundColumn HeaderText="类别" UniqueName="Class" DataField="Class" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="空气质量指数类别" HeaderStyle-Width="65px" />
                            <telerik:GridBoundColumn HeaderText="颜色" UniqueName="Color" DataField="Color" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="空气质量指数类别" HeaderStyle-Width="65px" />
                        </Columns>
                        <HeaderStyle Font-Bold="false" />
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="1"
                            SaveScrollPosition="false"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
                        <telerik:RadGrid ID="gridAvgStatisticalNew" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <div style="position: relative;">
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                                  <div style="position: absolute; right: 10px; top: 10px;">
                                            <telerik:RadButton ID="RadButton1" runat="server" BackColor="#1984CA" ForeColor="White"  AutoPostBack="false" OnClientClicking="ClientButtonClicking" Text="AQI说明">
                                <ContentTemplate>
                                                                <asp:Label runat="server" ID="Label4" ForeColor="White" Text="AQI说明"></asp:Label>
                                                            </ContentTemplate>
                            </telerik:RadButton>
                                        </div>
                                </div>
                        </CommandItemTemplate>
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="污染物浓度及空气质量分指数（IAQI）" HeaderText="污染物浓度及空气质量分指数（IAQI）"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="PM2.5" HeaderText=""
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="PM10" HeaderText=""
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="NO2" HeaderText=""
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="SO2" HeaderText=""
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="CO" HeaderText=""
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
<%--                            <telerik:GridColumnGroup Name="O3" HeaderText=""
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>--%>
                            <telerik:GridColumnGroup Name="O38" HeaderText="臭氧(O<sub>3</sub>)8小时"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="空气质量指数类别" HeaderText="空气质量指数类别"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>
                            <%--   <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridBoundColumn HeaderText="监测点位名称" UniqueName="PointId" DataField="PointId" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="PM25" DataField="PM25" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="PM25_AQI" DataField="PM25_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="PM10" DataField="PM10" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="PM10_AQI" DataField="PM10_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="NO2" DataField="NO2" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="NO2_AQI" DataField="NO2_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="SO2" DataField="SO2" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="SO2_AQI" DataField="SO2_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(mg/m<sup>3</sup>)" UniqueName="CO" DataField="CO" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" HeaderStyle-Width="100px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="CO_AQI" DataField="CO_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" HeaderStyle-Width="60px" />
                           <%-- <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="O3" DataField="O3" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O3" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="O3_AQI" DataField="O3_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O3" HeaderStyle-Width="60px" />--%>
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="Recent8HoursO3NT" DataField="Recent8HoursO3NT" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O38" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="Recent8HoursO3NT_AQI" DataField="Recent8HoursO3NT_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O38" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="空气质量<br />指数(AQI)" UniqueName="Max_AQI" DataField="Max_AQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                            <telerik:GridBoundColumn HeaderText="首要污染物" UniqueName="PrimaryPollutant" DataField="PrimaryPollutant" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                            <telerik:GridBoundColumn HeaderText="空气质量<br />指数级别" UniqueName="Grade" DataField="Grade" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                            <telerik:GridBoundColumn HeaderText="类别" UniqueName="Class" DataField="Class" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="空气质量指数类别" HeaderStyle-Width="65px" />
                            <telerik:GridBoundColumn HeaderText="颜色" UniqueName="Color" DataField="Color" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="空气质量指数类别" HeaderStyle-Width="65px" />
                        </Columns>
                        <HeaderStyle Font-Bold="false" />
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="1"
                            SaveScrollPosition="false"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
                        </telerik:RadPageView>
                <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                        <div style="padding-top: 6px;">
                            <div style="float: left;">
                                &nbsp &nbsp 图形：
                                <telerik:RadDropDownList runat="server" ID="ChartContent" Width="140px" OnSelectedIndexChanged="ChartContent_SelectedIndexChanged" AutoPostBack="true">
                                    <Items>
                                        <telerik:DropDownListItem Text="空气质量指数(AQI)" Value="primaryAQI"  Selected="true"/>
                                        <telerik:DropDownListItem Text="六参数浓度值" Value="factorValue" />
                                        <telerik:DropDownListItem Text="六参数分指数(IAQI)" Value="factorIAQI" />
                                        <telerik:DropDownListItem Text="首要污染物浓度值" Value="primaryValue" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                            <div style="float: right;">
                                <asp:RadioButtonList runat="server" ID="ChartType" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="4">
                                    <%--<asp:ListItem Text="折线图" Value="spline" Selected="True"></asp:ListItem>--%>
                                    <asp:ListItem Text="柱形图" Value="column" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="clear: both">
                            </div>
                        </div>
                        <div id="container">
                        </div>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
      <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="false">
            <Windows>
                <telerik:RadWindow ID="AQIDataExplainDetail" runat="server" Height="200px" Width="530px" 
                    Title="AQI说明" ReloadOnShow="false" ShowContentDuringLoad="false" Modal="false" />
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>
        <asp:HiddenField ID="hdPointId" runat="server" Value="0" />
        <asp:HiddenField ID="hddtBegion" runat="server" Value="0" />
        <asp:HiddenField ID="hddtEnd" runat="server" Value="0" />
        <asp:HiddenField ID="hdQuality" runat="server" Value="0" />
        <asp:HiddenField ID="hdChartType" runat="server" Value="0" />
        <asp:HiddenField ID="hdChartContent" runat="server" Value="primaryAQI" />
        <asp:HiddenField ID="hdDSType" runat="server" Value="0" />
    </form>
</body>
</html>
