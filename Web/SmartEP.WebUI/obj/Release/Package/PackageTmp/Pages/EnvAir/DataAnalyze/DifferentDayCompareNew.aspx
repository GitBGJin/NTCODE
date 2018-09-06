﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DifferentDayCompareNew.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.DifferentDayCompareNew" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" href="../../../Resources/CSS/pagination.css" />
    <style type="text/css">
        .paneWhere {
            height: 60px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript" src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script type="text/javascript" src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
            <script type="text/javascript">
                var isfirst = 1;//是否首次加载
                function TabSelected(sender, args) {
                    
                    try {
                        var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                        var text = $.trim(args._tab._element.innerText);
                        if (text == '图表') {
                            if (isfirst.value == "1") {
                                isfirst.value = 0;
                                InitGroupChart();
                            }
                        }
                    } catch (e) {
                    }
                }
                function InitGroupChart() {
                    
                    try {
                        var tab = document.getElementById("tabStrip").control._selectedIndex;
                        var pointF = $("#HiddenPointFactor").val();
                        if (tab == 0 || tab == 2) {
                            var hiddenData = $("#HiddenData").val().split('|');
                            var hdGroupName = $("#hdGroupName").val();
                            var height = parseInt(parseInt($("#pvChart").css("height")) - 65);
                            groupChartByPointidCom(hiddenData[0], hdGroupName, "../Chart/chartFrames.aspx", height);
                        }
                        else if (tab == 1) {
                            if (pointF == "point") {
                                var hiddenData = $("#HiddenData").val().split('|');
                                var hdGroupName = $("#hdGroupName").val();
                                var height = parseInt(parseInt($("#pvChart").css("height")) - 65);
                                groupChartByPointidCom(hiddenData[0], hdGroupName, "../Chart/chartFrames.aspx", height);
                            }
                            else if (pointF == "factor") {
                                var hiddenData = $("#HiddenData").val().split('|');
                                groupChart(hiddenData[1], "", "", "../Chart/chartFrames.aspx", (parseInt($("#pvChart").css("height")) - 65));
                            }
                        }
                    } catch (e) {
                    }
                }
                //Chart图形切换
                function ChartTypeChanged(item) {
                    try {
                        var chartIframe = document.getElementsByName('chartIframe');
                        //var item = args.get_item().get_value();
                        for (var i = 0; i < chartIframe.length; i++) {
                            document.getElementById(chartIframe[i].id).contentWindow.HighChartTypeChange(item);
                        }
                    } catch (e) {
                    }
                }
                //X轴切换
                function PointFactor(item) {
                    try {
                        InitGroupChart();
                    } catch (e) {
                    }
                }
                function OnClientClicking() {
                    var rbl = document.getElementsByName("radlDataType");
                    var tab = document.getElementById("tabStrip").control._selectedIndex;
                    if (tab == 0 || tab == 1) {
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
                                var dayB = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                                var dayE = $find("<%= dayEnd.ClientID %>").get_selectedDate();
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
                            else if (rbl[i].checked && rbl[i].value == "Month") {
                                var monthB = $find("<%= monthBegin.ClientID %>").get_selectedDate();
                                var monthE = $find("<%= monthEnd.ClientID %>").get_selectedDate();
                                if ((monthB == null) || (monthE == null)) {
                                    alert("开始时间或者终止时间，不能为空！");
                                    return false;
                                }
                                if (monthB > monthE) {
                                    alert("开始时间不能大于终止时间！");
                                    return false;
                                } else {
                                    return true;
                                }
                            }
                            else if (rbl[i].checked && rbl[i].value == "Season") {
                                var seasondateB = $find("<%= seasonBegin.ClientID %>").get_selectedDate();
                                var seasondateE = $find("<%= seasonEnd.ClientID %>").get_selectedDate();
                                var seasondateF = $find("<%= seasonFrom.ClientID %>")._selectedValue;
                                var seasondateT = $find("<%= seasonTo.ClientID %>")._selectedValue;
                                if ((seasondateB == null) || (seasondateE == null)) {
                                    alert("开始时间或者终止时间，不能为空！");
                                    return false;
                                }
                                if (seasondateB > seasondateE) {
                                    alert("开始时间不能大于终止时间！");
                                    return false;
                                }
                                if (seasondateB < seasondateE) {
                                    return true;
                                } else {
                                    if (parseInt(seasondateF) > parseInt(seasondateT)) {
                                        alert("同年季开始时间不能大于终止时间！");
                                        return false;
                                    }
                                }
                            }
                            else if (rbl[i].checked && rbl[i].value == "Year") {
                                var yearB = $find("<%= yearBegin.ClientID %>").get_selectedDate();
                                var yearE = $find("<%= yearEnd.ClientID %>").get_selectedDate();
                                if ((yearB == null) || (yearE == null)) {
                                    alert("开始时间或者终止时间，不能为空！");
                                    return false;
                                }
                                if (yearB > yearE) {
                                    alert("开始时间不能大于终止时间！");
                                    return false;
                                } else {
                                    return true;
                                }
                            }
                            else if (rbl[i].checked && rbl[i].value == "Week") {
                                var weekdateB = $find("<%= weekBegin.ClientID %>").get_selectedDate();
                                var weekdateF = $find("<%= weekFrom.ClientID %>")._selectedIndex;
                                var weekdateE = $find("<%= weekEnd.ClientID %>").get_selectedDate();
                                var weekdateT = $find("<%= weekTo.ClientID %>")._selectedIndex;
                                if ((weekdateB == null) || (weekdateE == null)) {
                                    alert("开始时间或者终止时间，不能为空！");
                                    return false;
                                }
                                if (weekdateB > weekdateE) {
                                    alert("开始时间不能大于终止时间！");
                                    return false;
                                }
                                if (weekdateB < weekdateE) {
                                    return true;
                                } else {
                                    if (parseInt(weekdateF) > parseInt(weekdateT)) {
                                        alert("同年月周开始时间不能大于终止时间！");
                                        return false;
                                    }
                                }
                            }
    }
}
    if (tab == 2) {
        for (var i = 0; i < rbl.length; i++) {
            if (rbl[i].checked && rbl[i].value == "Hour") {
                var hourB = $find("<%= hourBegin.ClientID %>").get_selectedDate();
                var hourE = $find("<%= hourEnd.ClientID %>").get_selectedDate();
                var hourBC = $find("<%= rdtpHourFrom.ClientID %>").get_selectedDate();
                var hourEC = $find("<%= rdtpHourTo.ClientID %>").get_selectedDate();
                if ((hourB == null) || (hourE == null) || (hourBC == null) || (hourEC == null)) {
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
                var dayB = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                var dayE = $find("<%= dayEnd.ClientID %>").get_selectedDate();
                var dayBC = $find("<%= rdpDayFrom.ClientID %>").get_selectedDate();
                var dayEC = $find("<%= rdpDayTo.ClientID %>").get_selectedDate();
                if ((dayB == null) || (dayE == null) || (dayBC == null) || (dayEC == null)) {
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
            else if (rbl[i].checked && rbl[i].value == "Month") {
                var monthB = $find("<%= monthBegin.ClientID %>").get_selectedDate();
                var monthE = $find("<%= monthEnd.ClientID %>").get_selectedDate();
                var monthBC = $find("<%= rmypMonthFrom.ClientID %>").get_selectedDate();
                var monthEC = $find("<%= rmypMonthTo.ClientID %>").get_selectedDate();
                if ((monthB == null) || (monthE == null) || (monthBC == null) || (monthEC == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (monthB > monthE) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
            else if (rbl[i].checked && rbl[i].value == "Season") {
                var seasondateB = $find("<%= seasonBegin.ClientID %>").get_selectedDate();
                var seasondateE = $find("<%= seasonEnd.ClientID %>").get_selectedDate();
                var seasondateF = $find("<%= seasonFrom.ClientID %>")._selectedValue;
                var seasondateT = $find("<%= seasonTo.ClientID %>")._selectedValue;
                var seasondateBC = $find("<%= rmypSeasonFrom.ClientID %>").get_selectedDate();
                var seasondateEC = $find("<%= rmypSeasonTo.ClientID %>").get_selectedDate();
                var seasondateFC = $find("<%= ddlSeasonFrom.ClientID %>")._selectedValue;
                var seasondateTC = $find("<%= ddlSeasonTo.ClientID %>")._selectedValue;
                if ((seasondateB == null) || (seasondateE == null) || (seasondateBC == null) || (seasondateEC == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (seasondateB > seasondateE) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                }
                if (seasondateB < seasondateE) {
                    return true;
                } else {
                    if (parseInt(seasondateF) > parseInt(seasondateT)) {
                        alert("同年季开始时间不能大于终止时间！");
                        return false;
                    }
                }
            }
            else if (rbl[i].checked && rbl[i].value == "Year") {
                var yearB = $find("<%= yearBegin.ClientID %>").get_selectedDate();
                var yearE = $find("<%= yearEnd.ClientID %>").get_selectedDate();
                var yearBC = $find("<%= rmypYearFrom.ClientID %>").get_selectedDate();
                var yearEC = $find("<%= rmypYearTo.ClientID %>").get_selectedDate();
                if ((yearB == null) || (yearE == null) || (yearBC == null) || (yearEC == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (yearB > yearE) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
            else if (rbl[i].checked && rbl[i].value == "Week") {
                var weekdateB = $find("<%= weekBegin.ClientID %>").get_selectedDate();
                var weekdateE = $find("<%= weekEnd.ClientID %>").get_selectedDate();
                var weekdateF = $find("<%= weekFrom.ClientID %>")._selectedIndex;
                var weekdateT = $find("<%= weekTo.ClientID %>")._selectedIndex;
                var weekdateBC = $find("<%= rmypWeekFrom.ClientID %>").get_selectedDate();
                var weekdateEC = $find("<%= rmypWeekTo.ClientID %>").get_selectedDate();
                var weekdateFC = $find("<%= ddlWeekFrom.ClientID %>")._selectedIndex;
                var weekdateTC = $find("<%= ddlWeekTo.ClientID %>")._selectedIndex;
                if ((weekdateB == null) || (weekdateE == null) || (weekdateBC == null) || (weekdateEC == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (weekdateB > weekdateE) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                }
                if (weekdateB < weekdateE) {
                    return true;
                } else {
                    if (parseInt(weekdateF) > parseInt(weekdateT)) {
                        alert("同年月周开始时间不能大于终止时间！");
                        return false;
                    }
                }
            }

}
}

}
//tab页切换时时间检查
function OnClientSelectedIndexChanging(sender, args) {
    var rbl = document.getElementsByName("radlDataType");
    var tab = document.getElementById("tabStrip").control._selectedIndex;
    if (tab == 0 || tab == 1) {
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
                var dayB = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                var dayE = $find("<%= dayEnd.ClientID %>").get_selectedDate();
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
            else if (rbl[i].checked && rbl[i].value == "Month") {
                var monthB = $find("<%= monthBegin.ClientID %>").get_selectedDate();
                var monthE = $find("<%= monthEnd.ClientID %>").get_selectedDate();
                if ((monthB == null) || (monthE == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (monthB > monthE) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
            else if (rbl[i].checked && rbl[i].value == "Season") {
                var seasondateB = $find("<%= seasonBegin.ClientID %>").get_selectedDate();
                var seasondateE = $find("<%= seasonEnd.ClientID %>").get_selectedDate();
                var seasondateF = $find("<%= seasonFrom.ClientID %>")._selectedValue;
                var seasondateT = $find("<%= seasonTo.ClientID %>")._selectedValue;
                if ((seasondateB == null) || (seasondateE == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (seasondateB > seasondateE) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                }
                if (seasondateB < seasondateE) {
                    return true;
                } else {
                    if (parseInt(seasondateF) > parseInt(seasondateT)) {
                        alert("同年季开始时间不能大于终止时间！");
                        return false;
                    }
                }
            }
            else if (rbl[i].checked && rbl[i].value == "Year") {
                var yearB = $find("<%= yearBegin.ClientID %>").get_selectedDate();
                var yearE = $find("<%= yearEnd.ClientID %>").get_selectedDate();
                if ((yearB == null) || (yearE == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (yearB > yearE) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
            else if (rbl[i].checked && rbl[i].value == "Week") {
                var weekdateB = $find("<%= weekBegin.ClientID %>").get_selectedDate();
                var weekdateF = $find("<%= weekFrom.ClientID %>")._selectedValue;
                var weekdateE = $find("<%= weekEnd.ClientID %>").get_selectedDate();
                var weekdateT = $find("<%= weekTo.ClientID %>")._selectedValue;
                if ((weekdateB == null) || (weekdateE == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (weekdateB > weekdateE) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                }
                if (weekdateB < weekdateE) {
                    return true;
                } else {
                    if (parseInt(weekdateF) > parseInt(weekdateT)) {
                        alert("同年月周开始时间不能大于终止时间！");
                        return false;
                    }
                }
            }
}
}
    if (tab == 2) {
        for (var i = 0; i < rbl.length; i++) {
            if (rbl[i].checked && rbl[i].value == "Hour") {
                var hourB = $find("<%= hourBegin.ClientID %>").get_selectedDate();
                var hourE = $find("<%= hourEnd.ClientID %>").get_selectedDate();
                var hourBC = $find("<%= rdtpHourFrom.ClientID %>").get_selectedDate();
                var hourEC = $find("<%= rdtpHourTo.ClientID %>").get_selectedDate();
                if ((hourB == null) || (hourE == null) || (hourBC == null) || (hourEC == null)) {
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
                var dayB = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                var dayE = $find("<%= dayEnd.ClientID %>").get_selectedDate();
                var dayBC = $find("<%= rdpDayFrom.ClientID %>").get_selectedDate();
                var dayEC = $find("<%= rdpDayTo.ClientID %>").get_selectedDate();
                if ((dayB == null) || (dayE == null) || (dayBC == null) || (dayEC == null)) {
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
            else if (rbl[i].checked && rbl[i].value == "Month") {
                var monthB = $find("<%= monthBegin.ClientID %>").get_selectedDate();
                var monthE = $find("<%= monthEnd.ClientID %>").get_selectedDate();
                var monthBC = $find("<%= rmypMonthFrom.ClientID %>").get_selectedDate();
                var monthEC = $find("<%= rmypMonthTo.ClientID %>").get_selectedDate();
                if ((monthB == null) || (monthE == null) || (monthBC == null) || (monthEC == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (monthB > monthE) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
            else if (rbl[i].checked && rbl[i].value == "Season") {
                var seasondateB = $find("<%= seasonBegin.ClientID %>").get_selectedDate();
                var seasondateE = $find("<%= seasonEnd.ClientID %>").get_selectedDate();
                var seasondateF = $find("<%= seasonFrom.ClientID %>")._selectedValue;
                var seasondateT = $find("<%= seasonTo.ClientID %>")._selectedValue;
                var seasondateBC = $find("<%= rmypSeasonFrom.ClientID %>").get_selectedDate();
                var seasondateEC = $find("<%= rmypSeasonTo.ClientID %>").get_selectedDate();
                var seasondateFC = $find("<%= ddlSeasonFrom.ClientID %>")._selectedValue;
                var seasondateTC = $find("<%= ddlSeasonTo.ClientID %>")._selectedValue;
                if ((seasondateB == null) || (seasondateE == null) || (seasondateBC == null) || (seasondateEC == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (seasondateB > seasondateE) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                }
                if (seasondateB < seasondateE) {
                    return true;
                } else {
                    if (parseInt(seasondateF) > parseInt(seasondateT)) {
                        alert("同年季开始时间不能大于终止时间！");
                        return false;
                    }
                }
            }
            else if (rbl[i].checked && rbl[i].value == "Year") {
                var yearB = $find("<%= yearBegin.ClientID %>").get_selectedDate();
                var yearE = $find("<%= yearEnd.ClientID %>").get_selectedDate();
                var yearBC = $find("<%= rmypYearFrom.ClientID %>").get_selectedDate();
                var yearEC = $find("<%= rmypYearTo.ClientID %>").get_selectedDate();
                if ((yearB == null) || (yearE == null) || (yearBC == null) || (yearEC == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (yearB > yearE) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
            else if (rbl[i].checked && rbl[i].value == "Week") {
                var weekdateB = $find("<%= weekBegin.ClientID %>").get_selectedDate();
                var weekdateE = $find("<%= weekEnd.ClientID %>").get_selectedDate();
                var weekdateF = $find("<%= weekFrom.ClientID %>")._selectedValue;
                var weekdateT = $find("<%= weekTo.ClientID %>")._selectedValue;
                var weekdateBC = $find("<%= rmypWeekFrom.ClientID %>").get_selectedDate();
                var weekdateEC = $find("<%= rmypWeekTo.ClientID %>").get_selectedDate();
                var weekdateFC = $find("<%= ddlWeekFrom.ClientID %>")._selectedValue;
                var weekdateTC = $find("<%= ddlWeekTo.ClientID %>")._selectedValue;
                if ((weekdateB == null) || (weekdateE == null) || (weekdateBC == null) || (weekdateEC == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (weekdateB > weekdateE) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                }
                if (weekdateB < weekdateE) {
                    return true;
                } else {
                    if (parseInt(weekdateF) > parseInt(weekdateT)) {
                        alert("同年月周开始时间不能大于终止时间！");
                        return false;
                    }
                }
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

function onRequestEnd(sender, args) {

}
            </script>
        </telerik:RadCodeBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <asp:HiddenField ID="FirstLoadChart" Value="1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridDataCompare">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridDataCompare" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridDataCompare" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridDataCompare" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rgDataCompare" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" />
                        <telerik:AjaxUpdatedControl ControlID="hdtimeBetween" />
                        <telerik:AjaxUpdatedControl ControlID="hdGroupName" />  
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenPointFactor" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="tabStrip">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="tabStrip" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divSame" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divMul" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divMulti" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divMultiTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divHourT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divDayT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divMonthT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divSeasonT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divWeekT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divYearT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="PointFactor" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="radlHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factor" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="tableStrip">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="tableStrip" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divHourT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divDayT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divWeekT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divMonthT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divSeasonT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divYearT" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlHour">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divHourT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divDayT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divWeekT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divMonthT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divSeasonT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divYearT" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rdtpHourFrom">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rdtpHourFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rdtpHourTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rdtpHourTo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rdtpHourFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rdtpHourTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rdpDayFrom">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rdpDayFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rdpDayTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rdpDayTo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rdpDayFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rdpDayTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rmypMonthFrom">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rmypMonthFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rmypMonthTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rmypMonthTo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rmypMonthFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rmypMonthTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rmypSeasonFrom">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rmypSeasonFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rmypSeasonTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rmypSeasonTo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rmypSeasonFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rmypSeasonTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlSeasonFrom">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlSeasonFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ddlSeasonTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlSeasonTo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlSeasonFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ddlSeasonTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rmypYearFrom">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rmypYearFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rmypYearTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rmypYearTo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rmypYearFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rmypYearTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rmypWeekFrom">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rmypWeekFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rmypWeekTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rmypWeekTo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rmypWeekFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rmypWeekTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlWeekFrom">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlWeekFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ddlWeekTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlWeekTo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlWeekFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ddlWeekTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="weekBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="weekFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="weekEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="weekTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rmypWeekFrom">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlWeekFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rmypWeekTo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlWeekTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChartType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartType" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="PointFactor">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="PointFactor" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenPointFactor" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="85px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0" CssClass="paneWhere">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">站点:
                        </td>
                        <td class="content" style="width: 420px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="400" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm" OnSelectedChanged="pointCbxRsm_SelectedChanged"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="title" style="width: 80px" id="">监测因子:
                        </td>
                        <td class="content" style="width: 440px;">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="400" DropDownWidth="420" ID="factorCbxRsm" MultiSelected="true"></CbxRsm:FactorCbxRsm>
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="400" DropDownWidth="420" ID="factor" MultiSelected="true" Visible="false"></CbxRsm:FactorCbxRsm>
                        </td>

                        <td class="content" align="left" style="width: 100px;" rowspan="3">
                            <asp:CheckBox ID="OriginalData" Checked="false" Text="原始数据" Visible="false" runat="server" />
                        </td>
                        <td class="content" align="left" rowspan="3">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">数据类型:
                        </td>
                        <td class="content" style="width: 420px;">
                            <asp:RadioButtonList ID="radlHour" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                            <asp:RadioButtonList ID="radlDataType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true" Visible="false">
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">
                            <div runat="server" id="divSame" visible="false">开始时间:</div>
                            <div runat="server" id="divMul" visible="false">比对时间:</div>
                        </td>
                        <td class="content" style="width: 440px;">
                            <div runat="server" id="divHour" visible="false">
                                <telerik:RadDateTimePicker ID="hourBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                                结束时间:
                            <telerik:RadDateTimePicker ID="hourEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12"  />
                            </div>
                            <div runat="server" id="divDay" visible="false">
                                <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12" 
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                结束时间:
                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12" 
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                            </div>
                            <div runat="server" id="divMonth" visible="false">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="95px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                            <td>结束时间:</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" Width="95px"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div runat="server" id="divSeason" visible="false">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="seasonBegin" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                            <td>&nbsp;&nbsp;第</td>
                                            <td>
                                                <telerik:RadDropDownList ID="seasonFrom" runat="server" Width="40px">
                                                    <Items>
                                                        <telerik:DropDownListItem runat="server" Selected="True" Text="1" Value="1" />
                                                        <telerik:DropDownListItem runat="server" Value="2" Text="2" />
                                                        <telerik:DropDownListItem runat="server" Value="3" Text="3" />
                                                        <telerik:DropDownListItem runat="server" Value="4" Text="4" />
                                                    </Items>
                                                </telerik:RadDropDownList>
                                            </td>
                                            <td>季&nbsp;&nbsp;结束时间:</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="seasonEnd" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                            <td>&nbsp;&nbsp;第</td>
                                            <td>
                                                <telerik:RadDropDownList ID="seasonTo" runat="server" Width="40px">
                                                    <Items>
                                                        <telerik:DropDownListItem runat="server" Value="1" Text="1" />
                                                        <telerik:DropDownListItem runat="server" Value="2" Text="2" />
                                                        <telerik:DropDownListItem runat="server" Value="3" Text="3" />
                                                        <telerik:DropDownListItem runat="server" Value="4" Text="4" Selected="True" />
                                                    </Items>
                                                </telerik:RadDropDownList>
                                            </td>
                                            <td>季 </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div runat="server" id="divYear" visible="false">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="yearBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="95px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                            <td>结束时间:</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="yearEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="95px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div runat="server" id="divWeek" visible="false">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="weekBegin" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    OnSelectedDateChanged="weekBegin_SelectedDateChanged" AutoPostBack="true" />
                                            </td>
                                            <td>
                                                <telerik:RadDropDownList ID="weekFrom" runat="server" Width="80px">
                                                </telerik:RadDropDownList>
                                            </td>
                                            <td>结束时间:</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="weekEnd" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    OnSelectedDateChanged="weekEnd_SelectedDateChanged" AutoPostBack="true" />
                                            </td>
                                            <td>
                                                <telerik:RadDropDownList ID="weekTo" runat="server" Width="80px">
                                                </telerik:RadDropDownList>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td class="title" style="width: 80px; text-align: center;">
                            <div runat="server" id="divMulti" visible="false">基准时间:</div>
                        </td>
                        <td class="content" style="width: 440px;">
                            <div runat="server" id="divMultiTime" visible="false">
                                <div runat="server" id="divHourT" visible="false">
                                    <telerik:RadDateTimePicker ID="rdtpHourFrom" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                        DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12" 
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        TimeView-HeaderText="小时" OnSelectedDateChanged="rdtpHourFrom_SelectedDateChanged" AutoPostBackControl="Both" />
                                    结束时间:
                            <telerik:RadDateTimePicker ID="rdtpHourTo" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12" 
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" OnSelectedDateChanged="rdtpHourTo_SelectedDateChanged" AutoPostBackControl="Both" />
                                </div>
                                <div runat="server" id="divDayT" visible="false">
                                    <telerik:RadDatePicker ID="rdpDayFrom" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12" 
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        OnSelectedDateChanged="rdpDayFrom_SelectedDateChanged" AutoPostBack="true" />
                                    结束时间:
                            <telerik:RadDatePicker ID="rdpDayTo" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12" 
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                OnSelectedDateChanged="rdpDayTo_SelectedDateChanged" AutoPostBack="true" />
                                </div>
                                <div runat="server" id="divMonthT" visible="false">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <telerik:RadMonthYearPicker ID="rmypMonthFrom" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="95px"
                                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                        OnSelectedDateChanged="rmypMonthFrom_SelectedDateChanged" AutoPostBack="true" />
                                                </td>
                                                <td>结束时间:</td>
                                                <td>
                                                    <telerik:RadMonthYearPicker ID="rmypMonthTo" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" Width="95px"
                                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                        OnSelectedDateChanged="rmypMonthTo_SelectedDateChanged" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div runat="server" id="divSeasonT" visible="false">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <telerik:RadMonthYearPicker ID="rmypSeasonFrom" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy"
                                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                        OnSelectedDateChanged="rmypSeasonFrom_SelectedDateChanged" AutoPostBack="true" />
                                                </td>
                                                <td>&nbsp;&nbsp;第</td>
                                                <td>
                                                    <telerik:RadDropDownList ID="ddlSeasonFrom" runat="server" Width="40px" OnSelectedIndexChanged="ddlSeasonFrom_SelectedIndexChanged" AutoPostBack="true">
                                                        <Items>
                                                            <telerik:DropDownListItem runat="server" Selected="True" Text="1" Value="1" />
                                                            <telerik:DropDownListItem runat="server" Value="2" Text="2" />
                                                            <telerik:DropDownListItem runat="server" Value="3" Text="3" />
                                                            <telerik:DropDownListItem runat="server" Value="4" Text="4" />
                                                        </Items>
                                                    </telerik:RadDropDownList>
                                                </td>
                                                <td>季&nbsp;&nbsp;结束时间:</td>
                                                <td>
                                                    <telerik:RadMonthYearPicker ID="rmypSeasonTo" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy"
                                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                        OnSelectedDateChanged="rmypSeasonTo_SelectedDateChanged" AutoPostBack="true" />
                                                </td>
                                                <td>&nbsp;&nbsp;第</td>
                                                <td>
                                                    <telerik:RadDropDownList ID="ddlSeasonTo" runat="server" Width="40px" OnSelectedIndexChanged="ddlSeasonTo_SelectedIndexChanged" AutoPostBack="true">
                                                        <Items>
                                                            <telerik:DropDownListItem runat="server" Value="1" Text="1" />
                                                            <telerik:DropDownListItem runat="server" Value="2" Text="2" />
                                                            <telerik:DropDownListItem runat="server" Value="3" Text="3" />
                                                            <telerik:DropDownListItem runat="server" Value="4" Text="4" Selected="True" />
                                                        </Items>
                                                    </telerik:RadDropDownList>
                                                </td>
                                                <td>季</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div runat="server" id="divYearT" visible="false">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <telerik:RadMonthYearPicker ID="rmypYearFrom" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="95px"
                                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy"
                                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                        OnSelectedDateChanged="rmypYearFrom_SelectedDateChanged" AutoPostBack="true" />
                                                </td>
                                                <td>结束时间:</td>
                                                <td>
                                                    <telerik:RadMonthYearPicker ID="rmypYearTo" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="95px"
                                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy"
                                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                        OnSelectedDateChanged="rmypYearTo_SelectedDateChanged" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div runat="server" id="divWeekT" visible="false">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <telerik:RadMonthYearPicker ID="rmypWeekFrom" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                        OnSelectedDateChanged="rmypWeekFrom_SelectedDateChanged" AutoPostBack="true" />
                                                </td>
                                                <td>
                                                    <telerik:RadDropDownList ID="ddlWeekFrom" runat="server" Width="80px" SelectedText="DropDownListItem1" SelectedValue="4" OnSelectedIndexChanged="ddlWeekFrom_SelectedIndexChanged" AutoPostBack="true">
                                                    </telerik:RadDropDownList>
                                                </td>
                                                <td>&nbsp;&nbsp;结束时间:</td>
                                                <td>
                                                    <telerik:RadMonthYearPicker ID="rmypWeekTo" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                        OnSelectedDateChanged="rmypWeekTo_SelectedDateChanged" AutoPostBack="true" />
                                                </td>
                                                <td>
                                                    <telerik:RadDropDownList ID="ddlWeekTo" runat="server" Width="80px" SelectedText="4" SelectedValue="4" OnSelectedIndexChanged="ddlWeekTo_SelectedIndexChanged" AutoPostBack="true">
                                                    </telerik:RadDropDownList>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneTab" runat="server" Scrolling="None" Width="100%" Height="26px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadTabStrip ID="tabStrip" runat="server" MultiPageID="multiPage1" OnClientTabSelecting="OnClientSelectedIndexChanging" OnTabClick="tabStrip_TabClick" AutoPostBack="true"
                    CssClass="RadTabStrip_Customer">
                    <Tabs>
                        <telerik:RadTab Text="原始审核比对" Value="0">
                        </telerik:RadTab>
                        <telerik:RadTab Text="相同时段多个站点多个参数比对" Value="1" Visible="false">
                        </telerik:RadTab>
                        <telerik:RadTab Text="可变时段多个站点多个参数比对" Value="2">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneTable" runat="server" Scrolling="None" Width="100%" Height="26px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage1" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="RadPageView1" runat="server" Visible="true">
                        <telerik:RadTabStrip ID="tableStrip" runat="server" MultiPageID="multiPage" OnClientTabSelecting="OnClientSelectedIndexChanging"
                            CssClass="RadTabStrip_Customer" OnClientTabSelected="TabSelected">
                            <Tabs>
                                <telerik:RadTab Text="列表">
                                </telerik:RadTab>
                                <telerik:RadTab Text="图表">
                                </telerik:RadTab>
                            </Tabs>
                        </telerik:RadTabStrip>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pvGrid" runat="server" Visible="true">
                        <telerik:RadGrid ID="gridDataCompare" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="true" ShowFooter="true"
                            AutoGenerateColumns="true" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="gridDataCompare_NeedDataSource" OnItemDataBound="gridDataCompare_ItemDataBound" OnColumnCreated="gridDataCompare_ColumnCreated"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                <CommandItemTemplate>
                                    <telerik:RadToolBar BorderWidth="0" ID="gridRTB" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                        runat="server" Width="50%" />
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" AllowSorting="true">
                                        <ItemTemplate>
                                            <%#Container.DataSetIndex + 1%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                    PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                            </MasterTableView>
                            <CommandItemStyle Width="100%" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                                    SaveScrollPosition="false"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                        <div style="padding-top: 6px;">
                            <div style="float: left;">
                                <asp:RadioButtonList runat="server" ID="PointFactor" AutoPostBack="true" Visible="false" RepeatDirection="Vertical" RepeatColumns="2" OnSelectedIndexChanged="PointFactor_SelectedIndexChanged">
                                    <asp:ListItem Text="按站点分类" Value="point" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="按因子分类" Value="factor"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="float: right;">
                                <asp:RadioButtonList runat="server" ID="ChartType" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="3" OnSelectedIndexChanged="ChartType_SelectedIndexChanged">
                                    <asp:ListItem Text="折线图" Value="spline" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="柱形图" Value="column"></asp:ListItem>
                                    <%--<asp:ListItem Text="点图" Value="scatter"></asp:ListItem>--%>
                                </asp:RadioButtonList>
                            </div>
                            <div style="clear: both">
                            </div>
                        </div>
                        <div id="ChartContainer">
                        </div>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/DifferentDayCompare.ashx" />
        <asp:HiddenField ID="HiddenChartType" runat="server" Value="spline" />
        <asp:HiddenField ID="HiddenPointFactor" runat="server" Value="point" />
        <asp:HiddenField ID="hdGroupName" runat="server" Value="0" />
        <asp:HiddenField ID="hdtimeBetween" runat="server" Value="0" />
    </form>
</body>
</html>
