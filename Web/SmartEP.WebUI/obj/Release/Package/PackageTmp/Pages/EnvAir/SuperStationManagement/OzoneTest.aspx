<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OzoneTest.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.SuperStationManagement.OzoneTest" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm1" Src="~/Controls/FactorCbxRsm1.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript" src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script type="text/javascript" src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
            <script src="../../../Resources/JavaScript/telerikControls/highcharts.js"></script>
            <script type="text/javascript">
                var myRadGrid;
                function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

                var isfirst = 1;//是否首次加载
                function TabSelected(sender, args) {

                    try {
                        var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                        var isfirsts = document.getElementById("<%=FirstLoadCharts.ClientID%>");
                        var text = $.trim(args._tab._element.innerText);
                        if (text == '图表') {
                            if (isfirst.value == "1") {
                                isfirst.value = 0;
                                InitGroupChart();
                            }
                        }
                        else if (text == '饼图') {
                            if (isfirsts.value == "1") {
                                isfirsts.value = 0;
                                CreatCharts();
                            }
                        }
                    } catch (e) {
                    }
                }



                function InitGroupChart() {
                    try {


                        //if (IsStatistical.checked == true) {

                        var pointF = $("#HiddenPointFactor").val();
                        if (pointF == "point") {                //已站点画图

                            var hiddenData = $("#HiddenData").val().split('|');
                            var height = parseInt(parseInt($("#pvChart").css("height")) - 65);
                            var hdGroupFac = $("#hdGroupFac").val();
                            var hdGroupName = $("#hdGroupName").val();
                            //HiddenData.value = "";
                            groupChartByPointid(hiddenData[0], hdGroupFac, hdGroupName, "../Chart/ChartFrame.aspx", height);
                        } else if (pointF == "factor") {        //以因子画图
                            var hiddenData = $("#HiddenData").val().split('|');
                            groupChart(hiddenData[1], "", "", "../Chart/ChartFrame.aspx", (parseInt($("#pvChart").css("height")) - 65));
                        } else if (pointF == "all") {
                            //var Time = hdDTime.value;
                            //var Data = hdDate.value;
                            var Time = eval("(" + hdDTime.value + ")");
                            var Data = eval("(" + hdDate.value + ")");
                            var ChartType = hdDChartType.value;
                            console.log(Time);
                            console.log(Data);
                            var chart = new Highcharts.Chart("ChartContainer", {
                                chart: {
                                    defaultSeriesType: ChartType
                                },
                                title: {
                                    text: '各类总值',
                                    x: -20
                                },
                                subtitle: {
                                    text: '',
                                    x: -20
                                },
                                xAxis: {
                                    categories: Time,
                                    tickInterval: Math.ceil(Time.length / 5)
                                },
                                yAxis: {
                                    title: {
                                        text: 'ppb'
                                    },
                                    plotLines: [{
                                        value: 0,
                                        width: 1,
                                        color: '#808080'
                                    }],
                                    opposite: false
                                }
                                ,
                                tooltip: {
                                    valueSuffix: 'ppb'
                                },
                                legend: {
                                    layout: 'vertical',
                                    align: 'right',
                                    verticalAlign: 'middle',
                                    borderWidth: 0
                                },
                                series: Data
                            });
                        }

                    } catch (e) {
                    }
                }
                function ClientButtonClicking(sender, args) {
                    var uri = "../DataAnalyze/AuditDataWindow.aspx";
                    var oWindow = window.radopen(encodeURI(uri), "ConfigOfflineDialog");
                }
                function CreatCharts() {
                    $('#PieChart1').html("");
                    $('#PieChart2').html("");
                    $('#PieChart3').html("");
                    $('#PieOne').html("");
                    $('#PieTwo').html("");
                    //获取隐藏域的值
                    //var BrandData = document.getElementById("hdBrandData").value;
                    var TypeNames = document.getElementById("hdTypes").value;
                    var Points = document.getElementById("hdPoints").value;
                    var Begion = document.getElementById("hdBegion").value;
                    var End = document.getElementById("hdEnd").value;
                    var OrderBy = document.getElementById("hdOrderBy").value;
                    var DataType = document.getElementById("hdDataType").value;
                    var TimeType = document.getElementById("hdTimeType").value;

                    //根据值跳转画图页面
                    var chartdiv1 = "";
                    var chartdiv2 = "";
                    var chartdiv3 = "";
                    var chartdiv4 = "";
                    var chartdiv5 = "";
                    //$.each(pointIds.split(','), function (chartNo, value) {
                    chartdiv1 = "";
                    chartdiv1 += '<div style=" width:100%; height:600px;" >';
                    chartdiv1 += '<iframe name="chartIframe1" id="frame1' + Math.random() + '" src="../Chart/OzonePrecursorChart.aspx?TypeNames=' + TypeNames + '&Points=' + Points + '&Begion=' + Begion + '&End=' + End + '&OrderBy=' + OrderBy + '&DataType=' + DataType + '&TimeType=' + TimeType + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                    chartdiv1 += '</div>';

                    chartdiv2 = "";
                    chartdiv2 += '<div style=" width:100%; height:600px;" >';
                    chartdiv2 += '<iframe name="chartIframe2" id="frame2' + Math.random() + '" src="../Chart/OzonePrecursorChart1.aspx?TypeNames=' + TypeNames + '&Points=' + Points + '&Begion=' + Begion + '&End=' + End + '&OrderBy=' + OrderBy + '&DataType=' + DataType + '&TimeType=' + TimeType + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                    chartdiv2 += '</div>';

                    chartdiv3 = "";
                    chartdiv3 += '<div style=" width:100%; height:600px;" >';
                    chartdiv3 += '<iframe name="chartIframe3" id="frame3' + Math.random() + '" src="../Chart/OzonePrecursorChart2.aspx?TypeNames=' + TypeNames + '&Points=' + Points + '&Begion=' + Begion + '&End=' + End + '&OrderBy=' + OrderBy + '&DataType=' + DataType + '&TimeType=' + TimeType + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                    chartdiv3 += '</div>';

                    chartdiv4 = "";
                    chartdiv4 += '<div style=" width:100%; height:600px;" >';
                    chartdiv4 += '<iframe name="chartIframe4" id="frame4' + Math.random() + '" src="../Chart/OzonePrecursorOneChart.aspx?TypeNames=' + TypeNames + '&Points=' + Points + '&Begion=' + Begion + '&End=' + End + '&OrderBy=' + OrderBy + '&DataType=' + DataType + '&TimeType=' + TimeType + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                    chartdiv4 += '</div>';

                    chartdiv5 = "";
                    chartdiv5 += '<div style=" width:100%; height:600px;" >';
                    chartdiv5 += '<iframe name="chartIframe5" id="frame5' + Math.random() + '" src="../Chart/OzonePrecursorTwoChart.aspx?TypeNames=' + TypeNames + '&Points=' + Points + '&Begion=' + Begion + '&End=' + End + '&OrderBy=' + OrderBy + '&DataType=' + DataType + '&TimeType=' + TimeType + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                    chartdiv5 += '</div>';

                    if (No.checked == true) {
                        $('#PieOne').append(chartdiv4);
                        $("#PieOne").css("overflow-y", "auto");
                        $("#PieOne").height("650px");//设置图表Iframe的高度
                        $("#PieOne").width("100%");//设置图表Iframe的宽度
                    }
                    else if (IsStatistical.checked == true || IsType.checked == true) {
                        $("#PieChart1").append(chartdiv1);
                        $('#PieChart2').append(chartdiv2);
                        $('#PieChart3').append(chartdiv3);

                        $("#PieChart1").css("overflow-y", "auto");
                        $("#PieChart1").height("650px");//设置图表Iframe的高度
                        $("#PieChart1").width("100%");//设置图表Iframe的宽度

                        $("#PieChart2").css("overflow-y", "auto");
                        $("#PieChart2").height("650px");//设置图表Iframe的高度
                        $("#PieChart2").width("100%");//设置图表Iframe的宽度

                        $("#PieChart3").css("overflow-y", "auto");
                        $("#PieChart3").height("650px");//设置图表Iframe的高度
                        $("#PieChart3").width("100%");//设置图表Iframe的宽度
                    }
                    //else if(IsType.checked==true)
                    //{
                    //    $('#PieTwo').append(chartdiv5);
                    //    $("#PieTwo").css("overflow-y", "auto");
                    //    $("#PieTwo").height("650px");//设置图表Iframe的高度
                    //    $("#PieTwo").width("100%");//设置图表Iframe的宽度
                    //}




                }
                //Chart图形切换
                function ChartTypeChanged(item) {
                    try {
                        var chartIframe = document.getElementsByName('chartIframe');
                        //var item = args.get_item().get_value();
                        if ($("#HiddenPointFactor").val() == "all") {
                            InitGroupChart();
                        }

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

                function InitTogetherChart() {
                    try {
                        //var bodyHeight = document.body.clientHeight;
                        //togetherChart("../Chart/ChartFrame.aspx", (parseInt(bodyHeight) - 112 - 40));

                        var hiddenData = $("#HiddenData").val().split('|');
                        var height = parseInt(parseInt($("#pvChart").css("height")) - 40);
                        groupChartByPointid(hiddenData[0], "../Chart/ChartFrame.aspx", height);//以站点分组
                    } catch (e) {
                    }
                }
                function OnClientClicking() {
                    var rbl = document.getElementsByName("radlDataType");
                    var rb2 = document.getElementsByName("RadioButtonList1");
                    for (var i = 0; i < rb2.length; i++) {
                        if (rb2[i].checked && rb2[i].value == "Min1") {
                            var date1 = new Date();
                            var date2 = new Date();
                            date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                            date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
                        else if (rb2[i].checked && rb2[i].value == "Min5") {
                            var date1 = new Date();
                            var date2 = new Date();
                            date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                            date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
                        else if (rb2[i].checked && rb2[i].value == "Min60") {
                            var date1 = new Date();
                            var date2 = new Date();
                            date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                            date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
            }
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

//tab页切换时时间检查
function OnClientSelectedIndexChanging(sender, args) {
    var rbl = document.getElementsByName("radlDataType");
    var rb2 = document.getElementsByName("radlDataTypeOri");
    for (var i = 0; i < rb2.length; i++) {
        if (rb2[i].checked && rb2[i].value == "Min1") {
            var date1 = new Date();
            var date2 = new Date();
            date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
            date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
        else if (rb2[i].checked && rb2[i].value == "Min5") {
            var date1 = new Date();
            var date2 = new Date();
            date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
            date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
        else if (rb2[i].checked && rb2[i].value == "Min60") {
            var date1 = new Date();
            var date2 = new Date();
            date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
            date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
}
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
        <asp:HiddenField ID="FirstLoadCharts" Value="1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridAudit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit"  LoadingPanelID="RadAjaxLoadingPanel1"/>
                        <%--<telerik:AjaxUpdatedControl ControlID="ChartContainer" LoadingPanelID="RadAjaxLoadingPanel1"/>--%>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="hdHeavyMetalMonitor" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadCharts" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointName" />
                        <telerik:AjaxUpdatedControl ControlID="hdGroupName" />  
                        <telerik:AjaxUpdatedControl ControlID="Pie1" />
                        <telerik:AjaxUpdatedControl ControlID="Pie2" />
                        <telerik:AjaxUpdatedControl ControlID="Pie3" />
                        <telerik:AjaxUpdatedControl ControlID="hdGroupFac" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenPointFactor" />
                        <%--<telerik:AjaxUpdatedControl ControlID="ChartContainer"  LoadingPanelID="RadAjaxLoadingPanel1"/>--%>
                        <%--<telerik:AjaxUpdatedControl ControlID="chartcontainers" LoadingPanelID="RadAjaxLoadingPanel1" />--%>
                        <telerik:AjaxUpdatedControl ControlID="hdBrandData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPoints" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdTypes" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdTimeType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdDate" />
                        <telerik:AjaxUpdatedControl ControlID="hdDTime" />
                        <telerik:AjaxUpdatedControl ControlID="hdDChartType" />
                        <telerik:AjaxUpdatedControl ControlID="hdBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdOrderBy" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="PieChart1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="PieChart2" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="PieChart3" LoadingPanelID="RadAjaxLoadingPanel1" />
                        
                        <%--<telerik:AjaxUpdatedControl ControlID="pvChart" LoadingPanelID="RadAjaxLoadingPanel1" />--%>
                        <telerik:AjaxUpdatedControl ControlID="PieOne" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="PieTwo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="No">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="No" />
                        <telerik:AjaxUpdatedControl ControlID="IsType" />
                        <telerik:AjaxUpdatedControl ControlID="IsStatistical" />
                        <telerik:AjaxUpdatedControl ControlID="ChartType" />
                        <telerik:AjaxUpdatedControl ControlID="PointFactor" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenPointFactor" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="cbFactor" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="tbFactor" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="IsType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="No" />
                        <telerik:AjaxUpdatedControl ControlID="IsType" />
                        <telerik:AjaxUpdatedControl ControlID="IsStatistical" />
                        <telerik:AjaxUpdatedControl ControlID="ChartType" />
                        <telerik:AjaxUpdatedControl ControlID="PointFactor" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenPointFactor" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="cbFactor" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="tbFactor" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="IsStatistical">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="No" />
                        <telerik:AjaxUpdatedControl ControlID="IsType" />
                        <telerik:AjaxUpdatedControl ControlID="IsStatistical" />
                        <telerik:AjaxUpdatedControl ControlID="ChartType" />
                        <telerik:AjaxUpdatedControl ControlID="PointFactor" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenPointFactor" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="cbFactor" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="tbFactor" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />

                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadioButtonList1">
                    <UpdatedControls>
                        
                        <telerik:AjaxUpdatedControl ControlID="dtpHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />

                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />

                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlDataSource">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        
                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="RadioButtonList1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpDay" LoadingPanelID="RadAjaxLoadingPanel1" />
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

                <telerik:AjaxSetting AjaxControlID="ChartType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartType" />
                        <telerik:AjaxUpdatedControl ControlID="hdDChartType" />
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
            <telerik:RadPane ID="paneWhere" runat="server" Height="80px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 150px">站点:
                        </td>
                        <td class="content" style="width: 570px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="300" CbxHeight="350" DefaultAllSelected="true" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"
                                Visible="true" ></CbxRsm:PointCbxRsm>
                            <%--<telerik:RadDropDownList ID="cbPoint" runat="server" Width="160px" Visible="false">
                            </telerik:RadDropDownList>--%>
                        </td>
                        <td class="title" style="width: 150px">监测因子:
                        </td>
                        <td class="content" style="width: 400px;">
                            <CbxRsm:FactorCbxRsm1 runat="server" ApplicationType="Air" CbxWidth="380" DropDownWidth="420" ID="factorCbxRsm1"  Visible="false" ></CbxRsm:FactorCbxRsm1>
                            <CbxRsm:FactorCbxRsm1 runat="server"  ApplicationType="Air" CbxWidth="380" DropDownWidth="420" DefaultAllSelected="true" ID="factorCbxRsm" Visible="false"></CbxRsm:FactorCbxRsm1>
                            <telerik:RadComboBox runat="server" ID="cbFactor" Width="380px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Visible="true" CheckedItemsTexts="DisplayAllInInput" Localization-CheckAllString="选择全部" EmptyMessage="请选择">
                            </telerik:RadComboBox>
                            <telerik:RadComboBox runat="server" ID="tbFactor" Width="380px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Visible="false" CheckedItemsTexts="DisplayAllInInput" Localization-CheckAllString="选择全部" EmptyMessage="请选择">
                            </telerik:RadComboBox>
                        </td>
                        
                        <%--<td class="content" align="left" style="width: 150px;" rowspan="2">
                            <table>
                                <tr>
                                    <td>--%>
                                        <asp:RadioButton ID="No" text="一级类" Visible="false" runat="server" GroupName="分类" AutoPostBack="true"  OnCheckedChanged="No_CheckedChanged"/>
                                    <%--</td>
                                </tr>
                                <tr>
                                    <td>--%>
                                        <asp:RadioButton ID="IsType" text="二级类" Visible="false" runat="server" GroupName="分类" AutoPostBack="true" OnCheckedChanged="No_CheckedChanged"/>
                                    <%--</td>
                                </tr>
                                <tr>
                                    <td>--%>
                                        <asp:RadioButton ID="IsStatistical" text="分因子" Visible="false" runat="server" GroupName="分类" AutoPostBack="true" Checked="true"   OnCheckedChanged="No_CheckedChanged"/>
                                    <%--</td>
                                </tr>
                            </table>
                            
                        </td>--%>
                        
                        
                    </tr>
                    <tr>
                        <%--<td class="title" style="width: 150px; text-align: center;">数据类型:
                        </td>--%>
                        <td id="Td1" class="content" style="width: 600px;" runat="server" visible="false">
                            <asp:RadioButtonList ID="radlDataType" runat="server" RepeatDirection="Horizontal" Visible="false" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" Visible="false"  OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 150px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 570px;">
                            <div runat="server" id="dtpHour" visible="true">
                            <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />结束时间:
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                        </div>
                            <%-- 原始日控件 --%>
                            <div runat="server" id="dtpDay" visible="false">
                                <telerik:RadDatePicker ID="dtpDayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationStep="12" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDatePicker ID="dtpDayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationStep="12" />
                            </div>
                            <%-- 审核月控件 --%>
                            <div runat="server" id="dtpMonth" visible="false">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="dtpMonthBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="105px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                            <td>&nbsp;&nbsp;至&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="dtpMonthEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" Width="105px"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div runat="server" id="dbtHour" visible="false">
                                <telerik:RadDateTimePicker ID="hourBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDateTimePicker ID="hourEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                            </div>
                            <div runat="server" id="dbtDay" visible="false">
                                <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationStep="12" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationStep="12" />
                            </div>
                           
                            <div runat="server" id="dbtWeek" visible="false">
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
                                            <td>日期范围：<asp:TextBox runat="server" BorderWidth="0" BorderColor="#ffffff" ID="txtweekF" ReadOnly="true" Width="70px"></asp:TextBox>~<asp:TextBox runat="server" ID="txtweekT" BorderWidth="0" BorderColor="#ffffff" ReadOnly="true" Width="70px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                            <div runat="server" id="dbtMonth" visible="false">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="95px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-OkButtonCaption="确定" />
                                            </td>
                                            <td>&nbsp;&nbsp;至&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" Width="95px"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-OkButtonCaption="确定" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                            <div runat="server" id="dbtSeason" visible="false">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="seasonBegin" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-OkButtonCaption="确定" />
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
                                            <td>季&nbsp;&nbsp;至</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="seasonEnd" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-OkButtonCaption="确定" />
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
                            <div runat="server" id="dbtYear" visible="false">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="yearBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="95px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-OkButtonCaption="确定" />
                                            </td>
                                            <td>&nbsp;&nbsp;至&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="yearEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="95px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-OkButtonCaption="确定" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                        <td class="title" style="width: 80px">排序:
                        </td>
                        <td class="content" align="left" style="width: 100px;">
                            <asp:CheckBox ID="IsSort" Checked="false" Text="统计行" runat="server" Visible="false" />
                            <telerik:RadComboBox runat="server" ID="TimeSort" Width="90px">
                                <Items>
                                    <telerik:RadComboBoxItem Text="时间降序" Value="时间降序" Selected="true" />
                                    <telerik:RadComboBoxItem Text="时间升序" Value="时间升序" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <%--<td class="title" style="width: 80px" runat="server">数据来源:
                                    </td>--%>
                                    <td class="content" style="width: 100px;">
                                        <telerik:RadDropDownList ID="ddlDataSource" Visible="false" runat="server" Width="100px" OnSelectedIndexChanged="ddlDataSource_SelectedIndexChanged" AutoPostBack="true">
                                            <Items>
                                                <telerik:DropDownListItem  Text="原始数据" Value="OriData" Selected="true"/>
                                                <telerik:DropDownListItem  Text="审核数据" Value="AuditData" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                    </td>
                        <td class="content" align="left" style="width: 150px;" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
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
                        <telerik:RadTab Text="饼图">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="grvGrid" runat="server" Visible="true">
                        <telerik:RadGrid ID="gridAudit" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="20" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                            AutoGenerateColumns="true" AllowMultiRowSelection="false" EnableViewState="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="gridAudit_NeedDataSource" OnItemDataBound="gridAudit_ItemDataBound" OnColumnCreated="gridAudit_ColumnCreated"
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
                                        <telerik:RadButton ID="RadButton1" runat="server" BackColor="#1984CA" AutoPostBack="false" ForeColor="White"  OnClientClicking="ClientButtonClicking" Text="标记位说明" Visible="false">
                                                <ContentTemplate>
                                                        <asp:Label runat="server" ID="Label4" ForeColor="White" Text="标记位说明"></asp:Label>
                                                </ContentTemplate>
                                        </telerik:RadButton>
                                    </div>
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <%--<telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Container.DataSetIndex + 1%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>

                                </Columns>
                                <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                    PageSizeLabelText="显示记录数:" PageSizes="20 50 100" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                            </MasterTableView>
                            <CommandItemStyle Width="100%" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="2"
                                    SaveScrollPosition="true"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                        <div style="padding-top: 6px;" id="PointChart" runat="server" visible="true"> 
                            <div style="float: left;">
                                <asp:RadioButtonList runat="server" ID="PointFactor" AutoPostBack="true" Visible="true" RepeatDirection="Vertical" RepeatColumns="2" OnSelectedIndexChanged="PointFactor_SelectedIndexChanged">
                                    <asp:ListItem Text="总值" Value="all" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="因子" Value="point" ></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="float: right;">
                                <asp:RadioButtonList runat="server" ID="ChartType" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="3" OnSelectedIndexChanged="ChartType_SelectedIndexChanged">
                                    <asp:ListItem Text="折线图" Value="spline" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="柱形图" Value="column"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            
                            <%--<div id="chartcontainers" runat="server" visible="false" style="width: 100%; height: 500px">
                            </div>--%>
                        </div>
                        <div id="ChartContainer" runat="server">
                            </div>
                        <%--<div id="container" >

                        </div>--%>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pChart" runat="server" Visible="true">
                        <%--<div style="padding-top: 6px;">--%>
                           <table id="Pie1" style="width:100%" runat="server" visible="true">
                               <tr>
                                   <td style="width:33%">
                                       <div id="PieChart1"  style=" width: 100%;  height: 500px;"></div>
                                   </td>
                                   <td style="width:33%">
                                       <div id="PieChart2" style="width: 100%; height: 500px;" ></div>
                                   </td>
                                   <td style="width:33%">
                                       <div id="PieChart3" style="width: 100%; height: 500px" ></div>
                                   </td>
                               </tr>
                           </table>
                        <table id="Pie2" style="width:100%" runat="server" visible="false">
                            <tr>
                                   <td style="width:100%">
                                       <div id="PieOne"  style=" width: 100%;  height: 500px;"></div>
                                   </td>
                            </tr>
                        </table>
                        <table id="Pie3" style="width:100%" runat="server" visible="false">
                            <tr>
                                   <td style="width:100%">
                                       <div id="PieTwo"  style=" width: 100%;  height: 500px;"></div>
                                   </td>
                            </tr>
                        </table>
                        <%--</div>--%>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="ConfigOfflineDialog" runat="server" Height="410px" Width="530px" Skin="Outlook" IconUrl="~/App_Themes/Fminine/images/RadGridHeaderBg2.png"
                    Title="标记位说明" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/AuditData.ashx" />
        <asp:HiddenField ID="HiddenChartType" runat="server" Value="spline" />
        <asp:HiddenField ID="hdHeavyMetalMonitor" runat="server" />
        <asp:HiddenField ID="hdPointName" runat="server" />
        <asp:HiddenField ID="hdGroupName" runat="server" Value="0" />
        <asp:HiddenField ID="hdDTime" runat="server" Value=""/>
        <asp:HiddenField ID="hdDate" runat="server" Value=""/>
        <asp:HiddenField ID="hdBrandData" runat="server" value="0"/>
        <asp:HiddenField ID="hdPoints" runat="server" value="0"/>
        <asp:HiddenField ID="hdTypes" runat="server" value="0"/>
        <asp:HiddenField ID="hdTimeType" runat="server" value="0"/>
        <asp:HiddenField ID="hdDataType" runat="server" value="0"/>
        <asp:HiddenField ID="hdBegion" runat="server" value="0"/>
        <asp:HiddenField ID="hdEnd" runat="server" value="0"/>
        <asp:HiddenField ID="hdOrderBy" runat="server" value="0"/>
        <asp:HiddenField ID="hdGroupFac" runat="server" Value="0" />
        <asp:HiddenField ID="HiddenPointFactor" runat="server" Value="all" />
        <asp:HiddenField ID="hdDChartType" runat="server" Value="spline" />
        <%--<script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>--%>
        <%--<script src="../../../Resources/JavaScript/HighCharts/highstock.js"></script>--%>
        <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
    </form>
</body>
</html>

