<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuperStation_lijingpu.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.SuperStationManagement.SuperStation_lijingpu" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%--<script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>--%>
    <script src="../../../Resources/jquery-1.9.0.min.js" type="text/javascript"></script>
    <script src="../../../Resources/highcharts.js"></script>
    <script src="../../../Resources/heatmap.js"></script>
    <script src="../../../Resources/ChartHeatmap.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            //tab切换事件控制数据类型的显示隐藏
            $("#tabStrip ul li").click(function () {
                var tapType = $(this).find("a").attr("rel");
                switch (tapType) {
                    case "1"://列表
                        $("#Typetitle").show();
                        $("#Type").show();
                        break;
                    case "2"://图表
                        $("#Typetitle").hide();
                        $("#Type").hide();
                        break;
                    case "3"://热力图
                        $("#Typetitle").show();
                        $("#Type").show();
                        break;
                }
            });
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
            <script type="text/javascript">
                //仪器1的绘图方法
                function InitTogetherChart() {
                    $("#container").html("");

                    var seriesOptions = [];
                    var strData = [];

                    createChart = function () {
                        Highcharts.setOptions({

                            lang: {
                                rangeSelectorZoom: ''    //隐藏Zoom
                            }
                        });
                        
                        $('#container').highcharts({

                            chart: {
                                type: 'column',
                                //inverted: true
                            },
                            title: {
                                text: ''
                            },

                            subtitle: {
                                text: ''
                            },
                            xAxis: {
                                reversed: false,
                                //type: 'logarithmic',
                                //categories: strData,
                                //tickInterval: 1
                            },
                            yAxis: { title: { text: 'dw/dlogDp' } },
                            tooltip: { valueSuffix: 'dw/dlogDp' },
                            legend: {
                                layout: 'vertical', align: 'center',
                                verticalAlign: 'bottom', borderWidth: 0
                            },
                            series: seriesOptions
                        })
                    };
                    var heavyMetalMonitor = $("#hdHeavyMetalMonitor").val();//查询的总的数据
                    var hiddenData = $("#hiddendiameter").val();//粒径
                    var allData = [];
                    var effectData = JSON.parse(heavyMetalMonitor);
                    var diameter = hiddenData.split(';');
                    console.log(diameter);
                    $.each(effectData, function (key, obj) {
                        for (var i = 0; i < diameter.length; i++) {

                            if (obj[diameter[i]] != null) {
                                allData.push([parseFloat(diameter[i]), parseFloat(obj[diameter[i]])]);
                            } else {
                                allData.push([parseFloat(diameter[i]), null]);
                            }
                        }

                    });
                    console.log(allData);
                    seriesOptions[0] = {
                        type: 'column',
                        name: "3772L(nm)",
                        data: allData,
                        pointStart: 1,
                        zIndex: 1,
                        //yAxis: 0
                    }
                    createChart();
                }

                //Splite加载事件（初始化Chart）
                function loadSplitter(sender) {
                    $(function () {
                        InitTogetherChart();
                    });
                }

                function chart() {
                    //InitTogetherChart();

                    var type = document.getElementById("hdType").value;
                    var flag = document.getElementById("hddtFlag").value;
                    var dtB = document.getElementById("hddtB").value;
                    var dtE = document.getElementById("hddtE").value;
                    var dtF = document.getElementById("hddtF").value;
                    var dtT = document.getElementById("hddtT").value;
                    var num = document.getElementById("hdNum").value;
                    var staT = document.getElementById("hddtBegion").value;
                    var endT = document.getElementById("hddtEnd").value;
                    var pointIds = document.getElementById("hdPointId").value;
                    var names = document.getElementById("hdName").value;
                    $("#container").html("");
                    var chartdiv = "";
                    $.each(pointIds.split(','), function (chartNo, value) {
                        var name = (names.split(','))[chartNo];
                        console.log(name);
                        chartdiv = "";
                        chartdiv += '<div style=" width:100%; height:600px;">';
                        if (flag == "1") {
                            chartdiv += '<iframe name="chartIframe" id="frame' + value + '" src="../Chart/lijingpuChart.aspx?name=' + name + '&pointIds=' + value + '&flag=' + flag + '&type=' + type + '&dtB=' + dtB + '&dtE=' + dtE + '&dtF=' + dtF + '&dtT=' + dtT + '&num=' + num + '" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                        } else {
                            chartdiv += '<iframe name="chartIframe" id="frame' + value + '" src="../Chart/lijingpuChart.aspx?name=' + name + '&pointIds=' + value + '&type=' + type + '&dtStart=' + staT + '&dtEnd=' + endT + '&num=' + num + '" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                        } chartdiv += '</div>';
                        $("#container").append(chartdiv);

                    });
                    $("#container").css("overflow-y", "auto");
                    $("#container").height("650px");//设置图表Iframe的高度
                    $("#container").width("100%");//设置图表Iframe的宽度
                }

                var isfirst = 1;//是否首次加载
                function TabSelected(sender, args) {
                    try {
                        var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                            var text = $.trim(args._tab._element.innerText);
                            if (text == '图表') {
                                if (isfirst.value == "1") {
                                    isfirst.value = 0;
                                    SearchData();
                                }
                            }
                        } catch (e) {
                        }
                    }

                    function SearchData() {
                        var buttonSearch = document.getElementById("<%=btnSearch.ClientID%>");
                        buttonSearch.click();
                    }

                    //根据类型获取图表显示方式
                    function GetChart() {
                        InitGroupChart();
                    }
                    function RefreshChart() {
                        try {
                            var chartPage = document.getElementById("pvChart");
                            chartPage.children[0].contentWindow.InitChart();

                            //var highchartPage = document.getElementById("HighChart");
                            //highchartPage.children[0]
                        } catch (e) {
                        }
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
            else if (rb2[i].checked && rb2[i].value == "OriDay") {
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
            else if (rb2[i].checked && rb2[i].value == "OriMonth") {
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
            else if (rb2[i].checked && rb2[i].value == "OriDay") {
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
            else if (rb2[i].checked && rb2[i].value == "OriMonth") {
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
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <asp:HiddenField ID="FirstLoadChart" Value="1" runat="server" />
        <%--<asp:HiddenField ID="SecondLoadChart" Value="1" runat="server" />--%>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtB" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtE" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtF" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtFlag" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
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
                        <telerik:AjaxUpdatedControl ControlID="hdNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtB" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtE" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtF" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtFlag" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdType" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <telerik:AjaxUpdatedControl ControlID="container" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtB" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtE" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtF" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtFlag" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdType" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <telerik:AjaxUpdatedControl ControlID="container" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtB" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtE" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtF" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtFlag" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdHeavyMetalMonitor" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hiddendiameter" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="SecondLoadChart" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dv" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dv1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="Hdv1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="Hdv2" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lbTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtB" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtE" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtF" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtFlag" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="nextSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdHeavyMetalMonitor" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hiddendiameter" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dv" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dv1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="Hdv1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="Hdv2" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lbTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvNext" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtB" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtE" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtF" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtFlag" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="preSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdHeavyMetalMonitor" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hiddendiameter" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dv" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dv1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="Hdv1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="Hdv2" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lbTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvNext" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtB" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtE" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtF" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtFlag" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridAudit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtB" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtE" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtF" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtFlag" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointId" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtB" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtE" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtF" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hddtFlag" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0" OnClientLoad="loadSplitter">
            <telerik:RadPane ID="paneWhere" runat="server" Height="80px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">测点:
                        </td>
                        <td class="content" style="width: 200px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="380" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"
                                Visible="true" DefaultIPointMode="Region" DefaultAllSelected="true"></CbxRsm:PointCbxRsm>
                            <%--<telerik:RadDropDownList ID="cbPoint" runat="server" Width="160px" Visible="false">
                            </telerik:RadDropDownList>--%>
                        </td>
                        <%--<td class="title" style="width: 80px">
                            <div id="Typetitle">数据类型:</div>
                        </td>
                        <td class="content" style="width: 200px;">
                            <div id="Type">
                                <telerik:RadDropDownList ID="rddlType" runat="server" Width="160px">
                                </telerik:RadDropDownList>
                            </div>
                        </td>--%>
                        <td class="title" style="width: 80px">
                            <div id="divDataSource">数据来源:</div>
                        </td>
                        <td class="content" style="width: 200px;">
                            <telerik:RadDropDownList ID="ddlDataSource" runat="server" Width="90px" OnSelectedIndexChanged="ddlDataSource_SelectedIndexChanged" AutoPostBack="true">
                                <Items>
                                    <telerik:DropDownListItem Text="原始数据" Value="OriData" Selected="true" />
                                    <telerik:DropDownListItem Text="审核数据" Value="AuditData" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                        <td class="content" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">数据类型:
                        </td>
                        <td class="content" style="width: 420px;">
                            <asp:RadioButtonList ID="radlDataType" runat="server" RepeatDirection="Horizontal" Visible="false" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                            <asp:RadioButtonList ID="radlDataTypeOri" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataTypeOri_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>
                        <%--<td class="title" style="width: 80px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 360px;">
                            <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="220px"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">结束时间:
                        </td>
                        <td class="content" style="width: 360px;">
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" Width="220px"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>--%>
                        <td class="title" style="width: 80px; text-align: center;">时间:
                        </td>
                        <td class="content" style="width: 500px;">
                            <%-- 原始小时控件 --%>
                            <div runat="server" id="dtpHour" visible="false">
                                <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                            </div>
                            <%-- 审核小时控件 --%>
                            <div runat="server" id="dbtHour">
                                <telerik:RadDateTimePicker ID="hourBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDateTimePicker ID="hourEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                            </div>

                            <div runat="server" id="dbtDay">
                                <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                    MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                MonthYearNavigationSettings-OkButtonCaption="确定"
                                MonthYearNavigationSettings-CancelButtonCaption="取消" />
                            </div>
                            <div runat="server" id="dbtMonth">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="105px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                            <td>&nbsp;&nbsp;至&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" Width="105px"
                                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    MonthYearNavigationSettings-CancelButtonCaption="取消" />
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
                                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    MonthYearNavigationSettings-CancelButtonCaption="取消" OnSelectedDateChanged="weekBegin_SelectedDateChanged" AutoPostBack="true" />
                                            </td>
                                            <td>&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadDropDownList ID="weekFrom" runat="server" Width="60px" OnSelectedIndexChanged="weekFrom_SelectedIndexChanged" AutoPostBack="true">
                                                </telerik:RadDropDownList>
                                            </td>
                                            <td>&nbsp;&nbsp;至</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="weekEnd" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    MonthYearNavigationSettings-CancelButtonCaption="取消" OnSelectedDateChanged="weekEnd_SelectedDateChanged" AutoPostBack="true" />
                                            </td>
                                            <td>&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadDropDownList ID="weekTo" runat="server" Width="60px" OnSelectedIndexChanged="weekTo_SelectedIndexChanged" AutoPostBack="true">
                                                </telerik:RadDropDownList>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>日期范围：<asp:TextBox runat="server" BorderWidth="0" BorderColor="#ffffff" ID="txtweekF" ReadOnly="true" Width="80px"></asp:TextBox><asp:TextBox runat="server" ID="txtweekT" BorderWidth="0" BorderColor="#ffffff" ReadOnly="true" Width="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneTab" runat="server" Scrolling="None" Width="100%" Height="26px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadTabStrip ID="tabStrip" runat="server" SelectedIndex="0" MultiPageID="multiPage" OnClientTabSelecting="OnClientSelectedIndexChanging"
                    CssClass="RadTabStrip_Customer" OnClientTabSelected="TabSelected">
                    <Tabs>
                        <telerik:RadTab Text="列表" rel="1" Value="0">
                        </telerik:RadTab>
                        <telerik:RadTab Text="图表" rel="2" Value="1">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="grvGrid" runat="server" Visible="true">
                        <telerik:RadGrid ID="gridAudit" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                            AutoGenerateColumns="true" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="gridAudit_NeedDataSource" OnItemDataBound="gridAudit_ItemDataBound" OnColumnCreated="gridAudit_ColumnCreated"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                <CommandItemTemplate>
                                    <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                        runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
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
                                    PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                            </MasterTableView>
                            <CommandItemStyle Width="100%" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                                    SaveScrollPosition="true"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvChart" runat="server">
                        <div id="dv" runat="server" style="width: 100%;">
                            <div id="container" style="width: 100%; height: 500px">
                            </div>
                        </div>

                        <%--<div id="dv1" runat="server" style="width: 50%; height: 100%; float: right">
                            <div id="dvlijingpuyiS" style="width: 100%; height: 500px">
                            </div>
                        </div>--%>
                        <%--<div>
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadButton ID="preSearch" runat="server" BackColor="#3A94D3" ForeColor="White" Height="20" OnClick="preSearch_Click">
                                            <ContentTemplate>
                                                <asp:Label runat="server" ID="Label5" ForeColor="White" Text="上一张"></asp:Label>
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                    <td>
                                        <label runat="server" id="lbTime"></label>
                                    </td>
                                    <td>
                                        <div id="dvNext" runat="server">
                                            <telerik:RadButton ID="nextSearch" runat="server" BackColor="#3A94D3" ForeColor="White" Height="20" OnClick="nextSearch_Click">
                                                <ContentTemplate>
                                                    <asp:Label runat="server" ID="Label5" ForeColor="White" Text="下一张"></asp:Label>
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>--%>
                    </telerik:RadPageView>
                    <%--<telerik:RadPageView ID="HighChart" runat="server">
                        <div id="Hdv1" runat="server" style="width: 100%">
                            <div id="container2" style="width: 95%; height: 600px; margin: 0 auto" runat="server">
                            </div>
                        </div>

                        <div id="Hdv2" runat="server" style="width: 100%; height: 100%">
                            <div id="container3" style="width: 95%; height: 600px; margin: 0 auto" runat="server">
                            </div>
                        </div>
                        <h1>你好！</h1>
                    </telerik:RadPageView>--%>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenNum" runat="server" Value="0" />
        <asp:HiddenField ID="hdHeavyMetalMonitor" runat="server" />
        <asp:HiddenField ID="hiddendiameter" runat="server" />

        <%-- 绘制图表的数据存到隐藏域中 --%>
        <asp:HiddenField ID="hdNum" runat="server" Value="0" />
        <asp:HiddenField ID="hddtBegion" runat="server" Value="0" />
        <asp:HiddenField ID="hddtEnd" runat="server" Value="0" />
        <asp:HiddenField ID="hdPointId" runat="server" Value="0" />
        <asp:HiddenField ID="hdName" runat="server" Value="0" />

        <%-- 时间抽象化 --%>
        <asp:HiddenField ID="hddtB" runat="server" Value="0" />
        <asp:HiddenField ID="hddtE" runat="server" Value="0" />
        <asp:HiddenField ID="hddtF" runat="server" Value="0" />
        <asp:HiddenField ID="hddtT" runat="server" Value="0" />
        <asp:HiddenField ID="hddtFlag" runat="server" Value="0" />
        <asp:HiddenField ID="hdType" runat="server" Value="0" />
    </form>
</body>
</html>
