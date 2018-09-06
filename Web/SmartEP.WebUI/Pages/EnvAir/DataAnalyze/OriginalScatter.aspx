﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OriginalScatter.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.OriginalScatter" ValidateRequest="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="../../../Resources/CSS/pagination.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
            <script type="text/javascript">

                $(function () {
                    //第一次加载全部隐藏
                    $("#dbtHour,#dbtDay,#dbtMonth,#dbtSeason,#dbtYear,#dbtWeek").hide(); $("#dtpHour").show();//初始化隐藏
                });

                var isfirst = 1;//是否首次加载
                function TabSelected(sender, args) {
                    try {
                        var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                        var isfirst2 = document.getElementById("<%=FirstLoadChart2.ClientID%>");
                        var text = $.trim(args._tab._element.innerText);
                        if (text == '日变化' || text == '日变化趋势')   //隐藏控件
                        {
                            $('#px').hide();
                            $('#TimeSort').hide();
                            $("#dtpHour,#dbtHour,#dbtMonth,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dbtDay").show();
                            $('#lx').hide();
                            $('#radlDataType').hide();
                            $('#radlDataTypeOri').hide();
                            //var rbl = document.getElementsByName("radlDataType");
                            //var rb2 = document.getElementsByName("radlDataTypeOri");
                            //for (var i = 0; i < rbl.length; i++) {
                            //    if (rbl[i].value == "Day") {
                            //        rbl[i].checked = true;
                            //    }
                            //}
                            //for (var i = 0; i < rb2.length; i++) {
                            //    if (rb2[i].value == "OriDay") {
                            //        rb2[i].checked = true;
                            //    }
                            //}
                        }
                        else if (text == '列表' || text == '图表')   //隐藏控件
                        {
                            $('#px').show();
                            $('#TimeSort').show();
                            $('#lx').show();
                            var sele = $find("<%=ddlDataSource.ClientID%>");
                            var index = sele._selectedIndex;
                            if (index == 0) {
                                $('#radlDataTypeOri').show();
                                $('#radlDataType').hide();
                                $("#dbtDay,#dbtHour,#dbtMonth,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dtpHour").show();
                            } else {
                                $('#radlDataTypeOri').hide();
                                $('#radlDataType').show();
                                var rbl = document.getElementsByName("radlDataType");
                                for (var i = 0; i < rbl.length; i++) {
                                    if (rbl[i].checked)
                                        switch (rbl[i].value) {
                                            case "Hour": $("#dtpHour,#dbtDay,#dbtMonth,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dbtHour").show(); break;
                                            case "Day": $("#dtpHour,#dbtHour,#dbtMonth,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dbtDay").show(); break;
                                            case "Week": $("#dtpHour,#dbtDay,#dbtMonth,#dbtYear,#dbtHour,#dbtSeason").hide(); $("#dbtWeek").show(); break;
                                            case "Month": $("#dtpHour,#dbtDay,#dbtHour,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dbtMonth").show(); break;
                                            case "Season": $("#dtpHour,#dbtDay,#dbtMonth,#dbtYear,#dbtWeek,#dbtHour").hide(); $("#dbtSeason").show(); break;
                                            case "Year": $("#dtpHour,#dbtDay,#dbtMonth,#dbtHour,#dbtWeek,#dbtSeason").hide(); $("#dbtYear").show(); break;
                                        }
                                }
                            }
                            
                            
                        }
                        if (text == '图表') {
                            if (isfirst.value == "1") {
                                isfirst.value = 0;
                                GetChart();
                            }
                        }
                        if (text == '日变化趋势') {
                            if (isfirst2.value == "1") {
                                isfirst2.value = 0;
                                createDayDev();
                            }
                        }
                    } catch (e) {
                        //InitTogetherChart();
                    }
                }
                function DivSelecteds() {
                    try {
                        $('#radlDataTypeOri').hide();
                        $('#radlDataType').show();
                        var rbl = document.getElementsByName("radlDataType");
                        for (var i = 0; i < rbl.length; i++) {
                            if (rbl[i].checked)
                                switch (rbl[i].value) {
                                    case "Hour": $("#dtpHour,#dbtDay,#dbtMonth,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dbtHour").show(); break;
                                    case "Day": $("#dtpHour,#dbtHour,#dbtMonth,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dbtDay").show(); break;
                                    case "Week": $("#dtpHour,#dbtDay,#dbtMonth,#dbtYear,#dbtHour,#dbtSeason").hide(); $("#dbtWeek").show(); break;
                                    case "Month": $("#dtpHour,#dbtDay,#dbtHour,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dbtMonth").show(); break;
                                    case "Season": $("#dtpHour,#dbtDay,#dbtMonth,#dbtYear,#dbtWeek,#dbtHour").hide(); $("#dbtSeason").show(); break;
                                    case "Year": $("#dtpHour,#dbtDay,#dbtMonth,#dbtHour,#dbtWeek,#dbtSeason").hide(); $("#dbtYear").show(); break;
                                }
                        }
                    } catch (e) {
                    }
                }
                function DivSelectedOri() {
                    try {
                        var rbl = document.getElementsByName("radlDataTypeOri");
                        for (var i = 0; i < rbl.length; i++) {
                            if (rbl[i].checked)
                                switch (rbl[i].value) {
                                    case "Min1": $("#dbtSeason,#dbtDay,#dbtMonth,#dbtYear,#dbtWeek,#dbtHour").hide(); $("#dtpHour").show(); break;
                                    case "Min5": $("#dbtYear,#dbtDay,#dbtMonth,#dbtHour,#dbtWeek,#dbtSeason").hide(); $("#dtpHour").show(); break;
                                    case "Min60": $("#dbtHour,#dbtDay,#dbtMonth,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dtpHour").show(); break;
                                    case "OriDay": $("#dbtDay,#dbtHour,#dbtMonth,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dtpHour").show(); break;
                                    case "OriMonth": $("#dbtMonth,#dbtDay,#dbtHour,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dtpHour").show(); break;
                                }
                        }

                    } catch (e) {
                    }
                }
                function DivSelected() {
                    try {
                        $("#dbtHour,#dbtDay,#dbtMonth,#dbtSeason,#dbtYear,#dbtWeek").hide(); $("#dtpHour").show();
                        $('#lx').show();
                        $('#px').show();
                        $('#TimeSort').show();

                        var sele = $find("<%=ddlDataSource.ClientID%>");
                        var index = sele._selectedIndex;
                        if (index == 0) {
                            $('#radlDataTypeOri').show();
                            $('#radlDataType').hide();
                            $("#dbtDay,#dbtHour,#dbtMonth,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dtpHour").show();
                        } else {
                            $('#radlDataTypeOri').hide();
                            $('#radlDataType').show();
                            var rbl = document.getElementsByName("radlDataType");
                            for (var i = 0; i < rbl.length; i++) {
                                if (rbl[i].checked)
                                    switch (rbl[i].value) {
                                        case "Hour": $("#dtpHour,#dbtDay,#dbtMonth,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dbtHour").show(); break;
                                        case "Day": $("#dtpHour,#dbtHour,#dbtMonth,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dbtDay").show(); break;
                                        case "Week": $("#dtpHour,#dbtDay,#dbtMonth,#dbtYear,#dbtHour,#dbtSeason").hide(); $("#dbtWeek").show(); break;
                                        case "Month": $("#dtpHour,#dbtDay,#dbtHour,#dbtYear,#dbtWeek,#dbtSeason").hide(); $("#dbtMonth").show(); break;
                                        case "Season": $("#dtpHour,#dbtDay,#dbtMonth,#dbtYear,#dbtWeek,#dbtHour").hide(); $("#dbtSeason").show(); break;
                                        case "Year": $("#dtpHour,#dbtDay,#dbtMonth,#dbtHour,#dbtWeek,#dbtSeason").hide(); $("#dbtYear").show(); break;
                                    }
                            }
                        }

                    } catch (e) {
                        //InitTogetherChart();
                    }
                }
                function DivSelectedNew() {
                    try {
                        $("#dbtHour,#dtpHour,#dbtMonth,#dbtSeason,#dbtYear,#dbtWeek").hide(); $("#dbtDay").show();
                        $('#px').hide();
                        $('#TimeSort').hide();
                        $('#lx').hide();
                        $('#radlDataType').hide();
                        $('#radlDataTypeOri').hide();

                    } catch (e) {
                        //InitTogetherChart();
                    }
                }
                //根据类型获取图表显示方式
                function GetChart() {
                    var showType = $("#ShowType").find("[checked]")[0].defaultValue;
                    if (showType == '合并')

                        InitTogetherChart();
                    else
                        InitGroupChart();
                }

                function InitGroupChart() {
                    try {
                        var hiddenData = $("#HiddenData").val().split('|');
                        var height = document.body.clientHeight - 112 - 40;
                        groupChart(hiddenData[1], "", "", "../Chart/ChartFrame.aspx", height);
                    } catch (e) {
                    }
                }

                function InitTogetherChart() {
                    try {
                        var hiddenData = $("#HiddenData").val().split('|');
                        var height = parseInt(parseInt($("#pvChart").css("height")) - 40);
                        groupChartByPointidNew(hiddenData[0], hiddenData[2], hiddenData[3], hiddenData[4], "../Chart/ChartFrame.aspx", "../Chart/ChartFrameScatter.aspx", height);//以站点分组
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

                //绘制日变化图表
                function createDayDev() {
                    $('#container').html("");
                    var hiddenData = $("#hddev").val().split('|');
                    var chartdiv = "";
                    chartdiv = "";
                    chartdiv += '<div style=" width:100%; height:600px;">';
                    chartdiv += '<iframe name="chartIframe" id="frame' + Math.random() + '" src="../Chart/DayDevelementChart.aspx?pointIds=' + hiddenData[0] + '&factorIds=' + hiddenData[1] + '&dtbegin=' + hiddenData[2] + '&dtend=' + hiddenData[3] + '&flag=' + hiddenData[4] + '&CorL=' + hiddenData[5] + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                    chartdiv += '</div>';
                    $("#container").append(chartdiv);
                    $("#container").css("overflow-y", "auto");
                    $("#container").height("650px");//设置图表Iframe的高度
                    $("#container").width("100%");//设置图表Iframe的宽度
                }

                function OnClientClicking() {
                    var rbl = document.getElementsByName("radlDataType");
                    var rb2 = document.getElementsByName("radlDataTypeOri");
                    for (var i = 0; i < rb2.length; i++) {
                        if (rb2[i].checked && rb2[i].value == "Min1") {
                            var date1 = new Date();
                            var date2 = new Date();
                            if ($find("<%= dtpBegin.ClientID %>") != null && $find("<%= dtpEnd.ClientID %>") != null) {
                                date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                                date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                                if ((date1 == null) || (date2 == null)) {
                                    alert("开始时间或者终止时间，不能为空！");
                                    return false;
                                }
                                if (date1 > date2) {
                                    alert("开始时间不能大于终止时间！");
                                    return false;
                                } else {
                                    return true;
                                }
                            }
                        }
                        else if (rb2[i].checked && rb2[i].value == "Min5") {
                            var date1 = new Date();
                            var date2 = new Date();
                            if ($find("<%= dtpBegin.ClientID %>") != null && $find("<%= dtpEnd.ClientID %>") != null) {
                                date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                                date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                                if ((date1 == null) || (date2 == null)) {
                                    alert("开始时间或者终止时间，不能为空！");
                                    return false;
                                }
                                if (date1 > date2) {
                                    alert("开始时间不能大于终止时间！");
                                    return false;
                                } else {
                                    return true;
                                }
                            }
                        }
                        else if (rb2[i].checked && rb2[i].value == "Min60") {
                            var date1 = new Date();
                            var date2 = new Date();
                            if ($find("<%= hourBegin.ClientID %>") != null && $find("<%= hourEnd.ClientID %>") != null) {
                                date1 = $find("<%= hourBegin.ClientID %>").get_selectedDate();
                                date2 = $find("<%= hourEnd.ClientID %>").get_selectedDate();
                                if ((date1 == null) || (date2 == null)) {
                                    alert("开始时间或者终止时间，不能为空！");
                                    return false;
                                }
                                if (date1 > date2) {
                                    alert("开始时间不能大于终止时间！");
                                    return false;
                                } else {
                                    return true;
                                }
                            }
                        }
                        else if (rb2[i].checked && rb2[i].value == "OriDay") {
                            var date1 = new Date();
                            var date2 = new Date();
                            if ($find("<%= dayBegin.ClientID %>") != null && $find("<%= dayEnd.ClientID %>") != null) {
                                date1 = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                                date2 = $find("<%= dayEnd.ClientID %>").get_selectedDate();
                                if ((date1 == null) || (date2 == null)) {
                                    alert("开始时间或者终止时间，不能为空！");
                                    return false;
                                }
                                if (date1 > date2) {
                                    alert("开始时间不能大于终止时间！");
                                    return false;
                                } else {
                                    return true;
                                }
                            }
                        }
                        else if (rb2[i].checked && rb2[i].value == "OriMonth") {
                            var date1 = new Date();
                            var date2 = new Date();
                            if ($find("<%= monthBegin.ClientID %>") != null && $find("<%= monthEnd.ClientID %>") != null) {
                                date1 = $find("<%= monthBegin.ClientID %>").get_selectedDate();
                                date2 = $find("<%= monthEnd.ClientID %>").get_selectedDate();
                                if ((date1 == null) || (date2 == null)) {
                                    alert("开始时间或者终止时间，不能为空！");
                                    return false;
                                }
                                if (date1 > date2) {
                                    alert("开始时间不能大于终止时间！");
                                    return false;
                                } else {
                                    return true;
                                }
                            }
                        }
    }
    for (var i = 0; i < rbl.length; i++) {
        if (rbl[i].checked && rbl[i].value == "Hour") {
            if ($find("<%= hourBegin.ClientID %>") != null && $find("<%= hourEnd.ClientID %>") != null) {
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
        }
        else if (rbl[i].checked && rbl[i].value == "Day") {
            if ($find("<%= dayBegin.ClientID %>") != null && $find("<%= dayEnd.ClientID %>") != null) {
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
        }
        else if (rbl[i].checked && rbl[i].value == "Month") {
            if ($find("<%= monthBegin.ClientID %>") != null && $find("<%= monthEnd.ClientID %>") != null) {
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
        }
        else if (rbl[i].checked && rbl[i].value == "Season") {
            if ($find("<%= seasonBegin.ClientID %>") != null && $find("<%= seasonEnd.ClientID %>") != null && $find("<%= seasonFrom.ClientID %>") != null && $find("<%= seasonTo.ClientID %>") != null) {
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
        }
        else if (rbl[i].checked && rbl[i].value == "Year") {
            if ($find("<%= yearBegin.ClientID %>") != null && $find("<%= yearEnd.ClientID %>") != null) {
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
        }
        else if (rbl[i].checked && rbl[i].value == "Week") {
            if ($find("<%= weekBegin.ClientID %>") != null && $find("<%= weekFrom.ClientID %>") != null && $find("<%= weekEnd.ClientID %>") != null && $find("<%= weekTo.ClientID %>") != null) {
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
}
//Grid按钮行处理
function gridRTB_ClientButtonClicking(sender, args) {
    args.set_cancel(!OnClientClicking());
}
//tab页切换时时间检查
function OnClientSelectedIndexChanging(sender, args) {
    var rbl = document.getElementsByName("radlDataType");
    var rb2 = document.getElementsByName("radlDataTypeOri");
    for (var i = 0; i < rb2.length; i++) {
        if (rb2[i].checked && rb2[i].value == "Min1") {
            var date1 = new Date();
            var date2 = new Date();
            if ($find("<%= dtpBegin.ClientID %>") != null && $find("<%= dtpEnd.ClientID %>") != null) {
                date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                if ((date1 == null) || (date2 == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (date1 > date2) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
        }
        else if (rb2[i].checked && rb2[i].value == "Min5") {
            var date1 = new Date();
            var date2 = new Date();
            if ($find("<%= dtpBegin.ClientID %>") != null && $find("<%= dtpEnd.ClientID %>") != null) {
                date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                if ((date1 == null) || (date2 == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (date1 > date2) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
        }
        else if (rb2[i].checked && rb2[i].value == "Min60") {
            var date1 = new Date();
            var date2 = new Date();
            if ($find("<%= hourBegin.ClientID %>") != null && $find("<%= hourEnd.ClientID %>") != null) {
                date1 = $find("<%= hourBegin.ClientID %>").get_selectedDate();
                date2 = $find("<%= hourEnd.ClientID %>").get_selectedDate();
                if ((date1 == null) || (date2 == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (date1 > date2) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
        }
        else if (rb2[i].checked && rb2[i].value == "OriDay") {
            var date1 = new Date();
            var date2 = new Date();
            if ($find("<%= dayBegin.ClientID %>") != null && $find("<%= dayEnd.ClientID %>") != null) {
                date1 = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                date2 = $find("<%= dayEnd.ClientID %>").get_selectedDate();
                if ((date1 == null) || (date2 == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (date1 > date2) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
        }
        else if (rb2[i].checked && rb2[i].value == "OriMonth") {
            var date1 = new Date();
            var date2 = new Date();
            if ($find("<%= monthBegin.ClientID %>") != null && $find("<%= monthEnd.ClientID %>") != null) {
                date1 = $find("<%= monthBegin.ClientID %>").get_selectedDate();
                date2 = $find("<%= monthEnd.ClientID %>").get_selectedDate();
                if ((date1 == null) || (date2 == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    return false;
                }
                if (date1 > date2) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
        }
}
    for (var i = 0; i < rbl.length; i++) {
        if (rbl[i].checked && rbl[i].value == "Hour") {
            if ($find("<%= hourBegin.ClientID %>") != null && $find("<%= hourEnd.ClientID %>") != null) {
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
        }
        else if (rbl[i].checked && rbl[i].value == "Day") {
            if ($find("<%= dayBegin.ClientID %>") != null && $find("<%= dayEnd.ClientID %>") != null) {
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
        }
        else if (rbl[i].checked && rbl[i].value == "Month") {
            if ($find("<%= monthBegin.ClientID %>") != null && $find("<%= monthEnd.ClientID %>") != null) {
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
        }
        else if (rbl[i].checked && rbl[i].value == "Season") {
            if ($find("<%= seasonBegin.ClientID %>") != null && $find("<%= seasonEnd.ClientID %>") != null && $find("<%= seasonFrom.ClientID %>") != null && $find("<%= seasonTo.ClientID %>") != null) {
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
        }
        else if (rbl[i].checked && rbl[i].value == "Year") {
            if ($find("<%= yearBegin.ClientID %>") != null && $find("<%= yearEnd.ClientID %>") != null) {
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
        }
        else if (rbl[i].checked && rbl[i].value == "Week") {
            if ($find("<%= weekBegin.ClientID %>") != null && $find("<%= weekFrom.ClientID %>") != null && $find("<%= weekEnd.ClientID %>") != null && $find("<%= weekTo.ClientID %>") != null) {
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
        <asp:HiddenField ID="FirstLoadChart2" Value="1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ddlDataSource">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataTypeOri" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridOriginal">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridOriginal" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridOriginal" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridOriginal" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart2" />
                        <telerik:AjaxUpdatedControl ControlID="hddev" />
                        <telerik:AjaxUpdatedControl ControlID="RadioButtonList1" />

                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" />
                        <telerik:AjaxUpdatedControl ControlID="dtpHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="radlDataTypeOri">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataTypeOri" />
                        <telerik:AjaxUpdatedControl ControlID="dtpHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="weekBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="weekFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="txtweekF" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="weekFrom">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtweekF" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="weekEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="weekTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="txtweekT" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="weekTo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtweekT" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ShowType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" />
                        <telerik:AjaxUpdatedControl ControlID="ShowType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChartType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartType" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadioButtonList1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadioButtonList1" />
                        <telerik:AjaxUpdatedControl ControlID="hddev" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="tabStrip">
                    <UpdatedControls>
                        <%--      <telerik:AjaxUpdatedControl ControlID="radlDataType" />
                        <telerik:AjaxUpdatedControl ControlID="radlDataTypeOri" />--%>
                        <telerik:AjaxUpdatedControl ControlID="dtpHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="80px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%;" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">站点:
                        </td>
                        <td class="content" style="width: 300px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="220" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="title" style="width: 80px">监测因子:
                        </td>
                        <td class="content" style="width: 370px;">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="360" DefaultAllSelected="true" DropDownWidth="420" ID="factorCbxRsm" OnSelectedChanged="factorCbxRsm_SelectedChanged"></CbxRsm:FactorCbxRsm>
                        </td>
                        <td class="title" style="width: 80px">
                            <div id="px">排序:</div>
                        </td>
                        <td class="content" align="left" style="width: 100px;">
                            <asp:CheckBox ID="IsStatistical" Checked="false" Text="统计行" runat="server" Visible="false" />
                            <telerik:RadComboBox runat="server" ID="TimeSort" Width="90px">
                                <Items>
                                    <telerik:RadComboBoxItem Text="时间降序" Value="时间降序" Selected="true" />
                                    <telerik:RadComboBoxItem Text="时间升序" Value="时间升序" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;" ><div id="lx">数据类型:</div>
                        </td>
                        <td class="content" style="width: 400px;">
                            <asp:RadioButtonList ID="radlDataType" runat="server" style="display:none;" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                            <asp:RadioButtonList ID="radlDataTypeOri" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataTypeOri_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 400px;">
                            <%-- 原始小时控件 --%>
                            <div runat="server" id="dtpHour">
                                <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                                结束时间:
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                            </div>
                            <%-- 审核小时控件 --%>
                            <div runat="server" id="dbtHour">
                                <telerik:RadDateTimePicker ID="hourBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                                结束时间;
                            <telerik:RadDateTimePicker ID="hourEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                            </div>
                            <div runat="server" id="dbtDay">
                                <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                结束时间：
                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                            </div>
                            <div runat="server" id="dbtMonth">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="105px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                            <td>&nbsp;&nbsp;至&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" Width="105px"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div runat="server" id="dbtSeason">
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
                                            <td>季 &nbsp;&nbsp;至</td>
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
                            <div runat="server" id="dbtYear">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="yearBegin" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                            <td>&nbsp;&nbsp;至&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="yearEnd" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div runat="server" id="dbtWeek">
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
                                                <telerik:RadDropDownList ID="weekFrom" runat="server" Width="80px" OnSelectedIndexChanged="weekFrom_SelectedIndexChanged" AutoPostBack="true">
                                                </telerik:RadDropDownList>
                                            </td>
                                            <td>&nbsp;&nbsp;至</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="weekEnd" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    OnSelectedDateChanged="weekEnd_SelectedDateChanged" AutoPostBack="true" />
                                            </td>
                                            <td>
                                                <telerik:RadDropDownList ID="weekTo" runat="server" Width="80px" OnSelectedIndexChanged="weekTo_SelectedIndexChanged" AutoPostBack="true">
                                                </telerik:RadDropDownList>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>日期范围：<asp:TextBox runat="server" BorderWidth="0" BorderColor="#ffffff" ID="txtweekF" ReadOnly="true" Width="90px"></asp:TextBox><asp:TextBox runat="server" ID="txtweekT" BorderWidth="0" BorderColor="#ffffff" ReadOnly="true" Width="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                        <td class="title" style="width: 80px">数据来源:
                        </td>
                        <td class="content" align="left" style="width: 100px;">
                            <telerik:RadDropDownList ID="ddlDataSource" runat="server" Width="90px" OnSelectedIndexChanged="ddlDataSource_SelectedIndexChanged" AutoPostBack="true">
                                <Items>
                                    <telerik:DropDownListItem Text="原始数据" Value="OriData" Selected="true" />
                                    <telerik:DropDownListItem Text="审核数据" Value="AuditData" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneTab" runat="server" Scrolling="None" Width="100%" Height="26px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadTabStrip ID="tabStrip" runat="server" SelectedIndex="0" MultiPageID="multiPage" OnClientTabSelecting="OnClientSelectedIndexChanging"
                    CssClass="RadTabStrip_Customer" OnClientTabSelected="TabSelected">
                    <Tabs>
                        <telerik:RadTab Text="列表">
                        </telerik:RadTab>
                        <telerik:RadTab Text="图表">
                        </telerik:RadTab>
                        <telerik:RadTab Text="日变化" PageViewID="pvGrid">
                        </telerik:RadTab>
                        <telerik:RadTab Text="日变化趋势">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pvGrid" runat="server" Visible="true">
                        <telerik:RadGrid ID="gridOriginal" runat="server" GridLines="None" Height="100%" Width="100%" ClientSettings-Scrolling-FrozenColumnsCount="3"
                            AllowPaging="True" PageSize="20" PagerStyle-ShowPagerText="false" AllowCustomPaging="true" AllowSorting="false" ShowFooter="true"
                            AutoGenerateColumns="true" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="gridOriginal_NeedDataSource" OnItemDataBound="gridOriginal_ItemDataBound" OnColumnCreated="gridOriginal_ColumnCreated"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                <CommandItemTemplate>
                                    <div style="width: 100%; position: relative;">
                                        <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                            runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                                        <%--OnClientButtonClicking="gridRTB_ClientButtonClicking"--%>
                                        <%--  <div style="position: absolute; right: 10px; top: 5px;">
                                            <table style="font-size: 12px">
                                                <tr>
                                                    <td>超上限（HSp）</td>
                                                    <td>超下限（LSp）</td>
                                                    <td>突变（T）</td>
                                                    <td>离群（Out）</td>
                                                    <td>负值（Neg）</td>
                                                    <td>重复（Rep）</td>
                                                </tr>
                                            </table>
                                        </div>--%>
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Container.DataSetIndex + 1%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                    PageSizeLabelText="显示记录数:" PageSizes="20 50 100" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                            </MasterTableView>
                            <CommandItemStyle Width="100%" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                                    SaveScrollPosition="true"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                        <div style="padding-top: 6px;">
                            <div style="float: left; display: none;">
                                <asp:RadioButtonList ID="ShowType" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="ShowType_SelectedIndexChanged">
                                    <asp:ListItem Text="合并" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="分屏"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="float: right; display: none">
                                <asp:RadioButtonList runat="server" ID="ChartType" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="3" OnSelectedIndexChanged="ChartType_SelectedIndexChanged">
                                    <asp:ListItem Text="折线图" Value="spline" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="柱形图" Value="column"></asp:ListItem>
                                    <asp:ListItem Text="点图" Value="scatter"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="clear: both">
                            </div>
                        </div>
                        <div id="ChartContainer">
                        </div>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="RadPageView1" runat="server" Visible="true">
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="RadPageView2" runat="server" Visible="true">
                        <div style="padding-top: 6px;">
                            <div style="float: right;">
                                <asp:RadioButtonList runat="server" ID="RadioButtonList1" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="4" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
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
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/OriginalScatter.ashx" />
        <asp:HiddenField ID="HiddenChartType" runat="server" Value="spline" />
        <asp:HiddenField ID="hddev" runat="server" />
    </form>
</body>
</html>
