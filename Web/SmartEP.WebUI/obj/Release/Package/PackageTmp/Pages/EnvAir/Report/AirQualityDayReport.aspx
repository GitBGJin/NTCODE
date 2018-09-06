﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualityDayReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirQualityDayReport" %>

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
                        var MasterTable = $find("<%= gridDayAQI.ClientID %>").get_masterTableView();
                        var MasterTableRealTime = $find("<%= gridRealTimeAQI.ClientID %>").get_masterTableView();
                        MasterTableRealTime.rebind();
                        MasterTable.rebind();
                    }
                }

                function PointFactor(item) {
                    try {
                        CreatChart();
                    } catch (e) {
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
                    var DSType = document.getElementById("hdDSType").value;
                    var fac = document.getElementById("hdFactors").value;

                    if (chartContent == "factorIAQI") {
                        var factors;
                        if (fac == "H") {
                            factors = 'PM25_IAQI,PM10_IAQI,NO2_IAQI,SO2_IAQI,CO_IAQI,O3_IAQI';
                        }
                        if (fac == "D") {
                            factors = 'PM25_IAQI,PM10_IAQI,NO2_IAQI,SO2_IAQI,CO_IAQI,Max8HourO3_IAQI';
                        }
                        var chartdiv = "";
                        $.each(factors.split(','), function (chartNo, value) {
                            chartdiv = "";
                            chartdiv += '<div style=" width:100%; height:600px;">';
                            chartdiv += '<iframe name="chartIframe" id="frame' + chartNo + '" src="../Chart/AirQualityChart.aspx?pointIds=' + pointIds + '&factor=' + value + '&DSType=' + DSType + '&chartType=' + chartType + '&chartContent=' + chartContent + '&quality=' + quality + '&dtBegion=' + dtBegion + '&dtEnd=' + dtEnd + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                            chartdiv += '</div>';
                            $("#container").append(chartdiv);

                        });
                        $("#container").css("overflow-y", "auto");
                        $("#container").height("650px");//设置图表Iframe的高度
                        $("#container").width("100%");//设置图表Iframe的宽度
                    }

                    if (chartContent == "factorValue") {
                        var factors;
                        if (fac == "H") {
                            factors = 'PM25,PM10,NO2,SO2,CO,O3';
                        }
                        if (fac == "D") {
                            factors = 'PM25,PM10,NO2,SO2,CO,Max8HourO3';
                        }
                        var chartdiv = "";
                        $.each(factors.split(','), function (chartNo, value) {
                            chartdiv = "";
                            chartdiv += '<div style=" width:100%; height:600px;">';
                            chartdiv += '<iframe name="chartIframe" id="frame' + chartNo + '" src="../Chart/AirQualityChart.aspx?pointIds=' + pointIds + '&factor=' + value + '&DSType=' + DSType + '&chartType=' + chartType + '&chartContent=' + chartContent + '&quality=' + quality + '&dtBegion=' + dtBegion + '&dtEnd=' + dtEnd + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                            chartdiv += '</div>';
                            $("#container").append(chartdiv);

                        });
                        $("#container").css("overflow-y", "auto");
                        $("#container").height("650px");//设置图表Iframe的高度
                        $("#container").width("100%");//设置图表Iframe的宽度
                    }

                    if (chartContent == "primaryAQI" || chartContent == "primaryValue") {
                        var chartdiv = "";
                        //$.each(pointIds.split(','), function (chartNo, value) {
                        chartdiv = "";
                        chartdiv += '<div style=" width:100%; height:600px;">';
                        chartdiv += '<iframe name="chartIframe" id="frame' + Math.random() + '" src="../Chart/AirQualityChart.aspx?pointIds=' + pointIds + '&DSType=' + DSType + '&chartType=' + chartType + '&chartContent=' + chartContent + '&quality=' + quality + '&dtBegion=' + dtBegion + '&dtEnd=' + dtEnd + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                        chartdiv += '</div>';
                        $("#container").append(chartdiv);

                        //});
                        $("#container").css("overflow-y", "auto");
                        $("#container").height("650px");//设置图表Iframe的高度
                        $("#container").width("100%");//设置图表Iframe的宽度
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
                    if ($find("<%= dtpBegin.ClientID %>") != null && $find("<%= dtpEnd.ClientID %>") != null) {
                        var date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                        var date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                        if ((date1 == null) || (date2 == null)) {
                            alert("开始时间或者终止时间，不能为空！");
                            //sender.set_autoPostBack(false);
                            return false;
                        }
                        else if (date1 > date2) {
                            alert("开始时间不能大于终止时间！");
                            return false;
                        }
                        else {
                            return true;
                        }
                    }
                    if ($find("<%= hourBegin.ClientID %>") != null && $find("<%= hourEnd.ClientID %>") != null) {
                        var dateHour1 = $find("<%= hourBegin.ClientID %>").get_selectedDate();
                        var dateHour2 = $find("<%= hourEnd.ClientID %>").get_selectedDate();
                        if ((dateHour1 == null) || (dateHour2 == null)) {
                            alert("开始时间或者终止时间，不能为空！");
                            //sender.set_autoPostBack(false);
                            return false;
                        }
                        else if (dateHour1 > dateHour2) {
                            alert("开始时间不能大于终止时间！");
                            return false;
                        }
                        else {
                            return true;
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

                //小时按钮行处理
                function gridRTB_ClientButtonClickingHour(sender, args) {
                    var masterTable = $find("<%= gridRealTimeAQI.ClientID %>").get_masterTableView();
                    var CurrentBtn = args.get_item();
                    var CurrentBtnName = CurrentBtn.get_text();
                    var CurrentBtnCommandName = CurrentBtn.get_commandName();
                    switch (CurrentBtnCommandName) {
                        case "DeleteSelected":
                            try {
                                //删除
                                var selItems = masterTable.get_selectedItems();
                                if (selItems.length <= 0) { alert("请选择要删除的记录！") }
                                else
                                {
                                    args.set_cancel(!confirm('确定删除所有选中的记录？'));
                                }
                            } catch (e) { }
                            break;
                        case "RebindGrid":
                            masterTable.rebind();
                            break;
                        default:
                            break;
                    }
                    //args.set_cancel(!OnClientClicking());
                }

                //按钮行处理
                function gridRTB_ClientButtonClicking(sender, args) {
                    var masterTable = $find("<%= gridDayAQI.ClientID %>").get_masterTableView();
                    var CurrentBtn = args.get_item();
                    var CurrentBtnName = CurrentBtn.get_text();
                    var CurrentBtnCommandName = CurrentBtn.get_commandName();
                    switch (CurrentBtnCommandName) {
                        case "DeleteSelected":
                            try {
                                //删除
                                var selItems = masterTable.get_selectedItems();
                                if (selItems.length <= 0) { alert("请选择要删除的记录！") }
                                else
                                {
                                    args.set_cancel(!confirm('确定删除所有选中的记录？'));
                                }
                            } catch (e) { }
                            break;
                        case "RebindGrid":
                            masterTable.rebind();
                            break;
                        default:
                            break;
                    }
                    //args.set_cancel(!OnClientClicking());
                }
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <asp:HiddenField ID="FirstLoadChart" Value="1" runat="server" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="timer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="gridDayAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdQuality" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartContent" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdDSType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdFactors" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="container" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpBegin" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hourBegin" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hourEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divTypeContent" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdFactors" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridDayAQI">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridDayAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridDayAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridDayAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%--<telerik:AjaxSetting AjaxControlID="dtpBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                 <telerik:AjaxSetting AjaxControlID="hourEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="hourBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                 <telerik:AjaxSetting AjaxControlID="dtpEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="gridDayAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hourEnd"/>
                        <telerik:AjaxUpdatedControl ControlID="hdQuality" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartContent" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdDSType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdFactors" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="container" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="tabStrip">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="gridDayAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdQuality" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartContent" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdDSType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdFactors" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="container" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRealTimeAQI">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChartType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChartContent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartContent" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartContent" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <td class="title" style="width: 80px; text-align: center;">查询范围:
                        </td>
                        <td class="content" style="width: 200px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server"  RepeatLayout="Flow" RepeatColumns="2">
                                <asp:ListItem Text="区域" Value="CityProper"></asp:ListItem>
                                <asp:ListItem Text="测点" Value="Port" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">站点:
                        </td>
                        <td class="content" style="width: 230px;">
                            <div runat="server" id="dvPoint">
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="220" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm" DefaultIPointMode="Region"></CbxRsm:PointCbxRsm>
                            </div>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">
                            <div id="divType" runat="server" visible="false">质量类别:</div>
                        </td>
                        <td class="content" style="width: 300px;">
                            <div id="divTypeContent" runat="server" visible="false">
                                <telerik:RadComboBox runat="server" ID="rcbCityProper" Width="250px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
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
                            </div>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">
                            <div id="divDataFrom" runat="server">数据来源:</div>
                        </td>
                        <td class="content" style="width: 300px;">
                            <div>
                                <telerik:RadDropDownList ID="ddlDataFrom" runat="server" Width="90px" >
                                    <Items>
                                        <telerik:DropDownListItem Text="原始数据" Value="OriData" Selected="true" />
                                        <telerik:DropDownListItem Text="审核数据" Value="AuditData" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                        </td>
                        <td class="content" align="center" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" OnClientClick="return OnClientClicking()" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">数据类型:
                        </td>
                        <td class="content" style="width: 200px;">
                            <asp:RadioButtonList ID="radlDataType" runat="server" RepeatLayout="Flow" RepeatColumns="2" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 80px">排序:
                        </td>
                        <td class="content" align="left" style="width: 230px;">
                            <telerik:RadComboBox runat="server" ID="TimeSort" Width="180px">
                                <Items>
                                    <telerik:RadComboBoxItem Text="时间降序" Value="时间降序" Selected="true" />
                                    <telerik:RadComboBoxItem Text="时间升序" Value="时间升序" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 300px;">
                            <div>
                                <telerik:RadDatePicker ID="dtpBegin" runat="server" Visible="false"  MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Width="160px" Calendar-FastNavigationStep="12" />

                                <telerik:RadDateTimePicker ID="hourBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Width="160px" />
                            </div>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">结束时间:
                        </td>
                        <td class="content" style="width: 300px;">
                            <div>
                                <telerik:RadDatePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" Visible="false" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Width="160px" Calendar-FastNavigationStep="12" />

                                <telerik:RadDateTimePicker ID="hourEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationStep="12"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Width="160px" />
                            </div>
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
                        <telerik:RadGrid ID="gridDayAQI" runat="server" GridLines="None" Height="100%" Width="100%"
                            AutoGenerateColumns="False" AllowMultiRowSelection="false"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBoundDay" Visible="false"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                <CommandItemTemplate>
                                    <div style="position: relative;">
                                        <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                            runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClickDay" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                                        <div style="position: absolute; right: 10px; top: 10px;">
                                            <b>城市日AQI的评价项目包括：SO<sub>2</sub>、NO<sub>2</sub>、CO、PM<sub>10</sub>、PM<sub>2.5</sub>的24小时平均和O<sub>3</sub>日最大8小时滑动平均等6个指标。</b>
                                        </div>
                                    </div>
                                </CommandItemTemplate>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup Name="污染物浓度及空气质量分指数（IAQI）" HeaderText="污染物浓度及空气质量分指数（IAQI）"
                                        HeaderStyle-HorizontalAlign="Center" />
                                    <telerik:GridColumnGroup Name="PM2.5" HeaderText="PM<sub>2.5</sub>24小时平均值"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="PM10" HeaderText="PM<sub>10</sub>24小时平均值"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="NO2" HeaderText="二氧化氮(NO<sub>2</sub>)24小时平均值"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="SO2" HeaderText="二氧化硫(SO<sub>2</sub>)24小时平均值"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="CO" HeaderText="一氧化碳(CO)24小时平均值"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="O38" HeaderText="臭氧(O<sub>3</sub>)最大8小时滑动平均值"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="空气质量指数类别" HeaderText="空气质量指数类别"
                                        HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn HeaderText="监测点位名称" UniqueName="PointId" DataField="PointId" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                    <telerik:GridDateTimeColumn HeaderText="日期" UniqueName="DateTime" DataField="DateTime" DataFormatString="{0:yyyy-MM-dd}" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="PM25" DataField="PM25" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="PM25_IAQI" DataField="PM25_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" HeaderStyle-Width="60px" />
                                    <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="PM10" DataField="PM10" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="PM10_IAQI" DataField="PM10_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" HeaderStyle-Width="60px" />
                                    <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="NO2" DataField="NO2" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="NO2_IAQI" DataField="NO2_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" HeaderStyle-Width="60px" />
                                    <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="SO2" DataField="SO2" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="SO2_IAQI" DataField="SO2_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" HeaderStyle-Width="60px" />
                                    <telerik:GridBoundColumn HeaderText="浓度/(mg/m<sup>3</sup>)" UniqueName="CO" DataField="CO" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="CO_IAQI" DataField="CO_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" HeaderStyle-Width="60px" />
                                    <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="Max8HourO3" DataField="Max8HourO3" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O38" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="Max8HourO3_IAQI" DataField="Max8HourO3_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O38" HeaderStyle-Width="60px" />
                                    <telerik:GridBoundColumn HeaderText="空气质量<br />指数(AQI)" UniqueName="AQIValue" DataField="AQIValue" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                    <telerik:GridBoundColumn HeaderText="首要污染物" UniqueName="PrimaryPollutant" DataField="PrimaryPollutant" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                    <telerik:GridBoundColumn HeaderText="空气质量<br />指数级别" UniqueName="Grade" DataField="Grade" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                    <telerik:GridBoundColumn HeaderText="类别" UniqueName="Class" DataField="Class" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="空气质量指数类别" HeaderStyle-Width="65px" />
                                    <telerik:GridBoundColumn HeaderText="颜色" UniqueName="RGBValue" DataField="RGBValue" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="空气质量指数类别" HeaderStyle-Width="65px" />
                                </Columns>
                                <HeaderStyle Font-Bold="false" />
                                <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                    PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                            </MasterTableView>
                            <CommandItemStyle Width="100%" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                                    SaveScrollPosition="false" FrozenColumnsCount="3"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>

                        <telerik:RadGrid ID="gridRealTimeAQI" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                            AutoGenerateColumns="False" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBoundHour" Visible="true"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                <CommandItemTemplate>
                                    <div style="position: relative;">
                                        <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                            runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClickHour" OnClientButtonClicking="gridRTB_ClientButtonClickingHour" />
                                        <div style="position: absolute; right: 10px; top: 10px;">
                                            <b>城市小时AQI的评价项目包括：SO<sub>2</sub>、NO<sub>2</sub>、PM<sub>10</sub>、CO、O<sub>3</sub>和PM<sub>2.5</sub>的1小时平均等6项指标。</b>
                                        </div>
                                    </div>
                                </CommandItemTemplate>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup Name="污染物浓度及空气质量分指数（IAQI）" HeaderText="污染物浓度及空气质量分指数（IAQI）"
                                        HeaderStyle-HorizontalAlign="Center" />
                                    <telerik:GridColumnGroup Name="PM2.5" HeaderText="PM<sub>2.5</sub>1小时平均"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="PM10" HeaderText="PM<sub>10</sub>1小时平均"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="NO2" HeaderText="二氧化氮(NO<sub>2</sub>)1小时平均"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="SO2" HeaderText="二氧化硫(SO<sub>2</sub>)1小时平均"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="CO" HeaderText="一氧化碳(CO)1小时平均"
                                        HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="O31" HeaderText="臭氧(O<sub>3</sub>)1小时平均"
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
                                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn HeaderText="监测点位名称" UniqueName="PointId" DataField="PointId" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                    <telerik:GridDateTimeColumn HeaderText="日期" UniqueName="DateTime" DataField="DateTime" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="PM25" DataField="PM25" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="PM25_IAQI" DataField="PM25_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" HeaderStyle-Width="60px" />
                                    <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="PM10" DataField="PM10" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="PM10_IAQI" DataField="PM10_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" HeaderStyle-Width="60px" />
                                    <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="NO2" DataField="NO2" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="NO2_IAQI" DataField="NO2_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" HeaderStyle-Width="60px" />
                                    <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="SO2" DataField="SO2" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="SO2_IAQI" DataField="SO2_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" HeaderStyle-Width="60px" />
                                    <telerik:GridBoundColumn HeaderText="浓度/(mg/m<sup>3</sup>)" UniqueName="CO" DataField="CO" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" HeaderStyle-Width="93px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="CO_IAQI" DataField="CO_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" HeaderStyle-Width="60px" />
                                    <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="O3" DataField="O3" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O31" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="O3_IAQI" DataField="O3_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O31" HeaderStyle-Width="60px" />
<%--                                    <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="Recent8HoursO3NT" DataField="Recent8HoursO3NT" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O38NT" HeaderStyle-Width="90px" />
                                    <telerik:GridBoundColumn HeaderText="分指数" UniqueName="Recent8HoursO3NT_IAQI" DataField="Recent8HoursO3NT_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O38NT" HeaderStyle-Width="60px" />--%>
                                    <telerik:GridBoundColumn HeaderText="空气质量<br />指数(AQI)" UniqueName="AQIValue" DataField="AQIValue" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                    <telerik:GridBoundColumn HeaderText="首要污染物" UniqueName="PrimaryPollutant" DataField="PrimaryPollutant" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                    <telerik:GridBoundColumn HeaderText="空气质量<br />指数级别" UniqueName="Grade" DataField="Grade" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                    <telerik:GridBoundColumn HeaderText="类别" UniqueName="Class" DataField="Class" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="空气质量指数类别" HeaderStyle-Width="65px" />
                                    <telerik:GridBoundColumn HeaderText="颜色" UniqueName="RGBValue" DataField="RGBValue" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="空气质量指数类别" HeaderStyle-Width="65px" />
                                </Columns>
                                <HeaderStyle Font-Bold="false" />
                                <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                    PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                            </MasterTableView>
                            <CommandItemStyle Width="100%" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                                    SaveScrollPosition="false" FrozenColumnsCount="3"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                        <div style="padding-top: 6px;">
                            <div style="float: left;">
                                &nbsp &nbsp 图形：
                                <telerik:RadDropDownList runat="server" ID="ChartContent" Width="140px" OnSelectedIndexChanged="ChartContent_SelectedIndexChanged" AutoPostBack="true">
                                    <Items>
                                        <telerik:DropDownListItem Text="空气质量指数(AQI)" Value="primaryAQI"  Selected="true" />
                                        <telerik:DropDownListItem Text="六参数浓度值" Value="factorValue"/>
                                        <telerik:DropDownListItem Text="六参数分指数(IAQI)" Value="factorIAQI" />
                                        <telerik:DropDownListItem Text="首要污染物浓度值" Value="primaryValue" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                            <div style="float: right;">
                                <asp:RadioButtonList runat="server" ID="ChartType" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="4" OnSelectedIndexChanged="ChartType_SelectedIndexChanged">
                                    <asp:ListItem Text="折线图" Value="spline" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="柱形图" Value="column"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="clear: both">
                            </div>
                        </div>
                        <div id="container" runat="server">
                        </div>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <asp:Timer runat="server" ID="timer" Interval="1" Enabled="false" OnTick="timer_Tick"></asp:Timer>
        <asp:HiddenField ID="hdPointId" runat="server" Value="0" />
        <asp:HiddenField ID="hddtBegion" runat="server" Value="0" />
        <asp:HiddenField ID="hddtEnd" runat="server" Value="0" />
        <asp:HiddenField ID="hdQuality" runat="server" Value="0" />
        <asp:HiddenField ID="hdChartType" runat="server" Value="spline" />
        <asp:HiddenField ID="hdChartContent" runat="server" Value="primaryAQI" />
        <asp:HiddenField ID="hdDSType" runat="server" Value="0" />
        <asp:HiddenField ID="hdFactors" runat="server" Value="H" />
    </form>
</body>
</html>
