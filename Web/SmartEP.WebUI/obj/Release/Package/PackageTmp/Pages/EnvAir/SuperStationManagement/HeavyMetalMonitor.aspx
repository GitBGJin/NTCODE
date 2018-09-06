﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HeavyMetalMonitor.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.SuperStationManagement.HeavyMetalMonitor" %>

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
            <script type="text/javascript" src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script type="text/javascript" src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
            <script type="text/javascript">
                //var isfirst = 1;//是否首次加载
                function TabSelected(sender, args) {
                    try {
                        var tab = document.getElementById("tabStrip");
                        var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                        if (tab.control._selectedIndex == 1 && isfirst.value == 1) {
                            isfirst.value = 0;
                            SearchData();
                            //InitTogetherChart();
                        }
                    } catch (e) {
                    }
                }
                function SearchData() {
                    var buttonSearch = document.getElementById("<%=btnSearch.ClientID%>");
                    buttonSearch.click();
                }

                function InitTogetherChart() {
                    $("#container").html("");

                    var seriesOptions = [];
                    createChart = function () {

                        Highcharts.setOptions({
                            global: {
                                timezoneOffset: -8 * 60
                            },
                            lang: {
                                rangeSelectorZoom: '',   //隐藏Zoom
                                printChart: "打印"
                            }
                        });
                        var PointName = $("#hdPointName").val();//测点名称
                        // create the chart
                        $('#container').highcharts('StockChart', {
                            chart: {
                                alignTicks: false
                            },
                            colors: [
                       '#492970',
                       '#77a1e5',
                       '#c42525',
                       '#a6c96a',
                       '#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4', '#4572A7',

                            '#AA4643',

                            '#89A54E',

                            '#80699B',

                            '#3D96AE',

                            '#DB843D',

                            '#92A8CD',

                            '#A47D7C',

                            '#B5CA92'
                            ],
                            title: {
                                text: PointName + '重金属数据展示'
                            },
                            xAxis: {
                                type: 'datetime',
                                dateTimeLabelFormats: {
                                    second: '%Y-%m-%d<br/>%H:%M:%S',
                                    minute: '%Y-%m-%d<br/>%H:%M',
                                    hour: '%Y-%m-%d<br/>%H:%M',
                                    day: '%Y<br/>%m-%d',
                                    week: '%Y<br/>%m-%d',
                                    month: '%Y-%m',
                                    year: '%Y'
                                }
                            },
                            exporting: {
                                type: 'image/png',
                                buttons: {
                                    contextButton: {
                                        menuItems: [{
                                            text: '导出图像PNG',
                                            onclick: function () {
                                                this.exportChart();
                                            },
                                            separator: false
                                        }]
                                    }
                                }
                            },
                            tooltip: {
                                //valueDecimals: 3,
                                xDateFormat: '%Y-%m-%d %H:%M',
                                shared: true
                            },
                            yAxis: [{  //这里注意了  配置双Y轴的这里要看好了  这里的值是一个数组
                                min: 0,
                                lineWidth: 1,
                                tickAmount: 11,
                                title: {  //左边y轴的标题
                                    text: 'ng/m3'
                                }

                            }, {
                                min: 0,
                                lineWidth: 1,
                                tickAmount: 11,
                                title: {  //这是第二天Y轴在右边
                                    text: 'mg/m3'
                                },
                                opposite: false//这个属性的作用是说 是否与第一条y轴相反 当然是true咯
                            }],

                            plotOptions: {

                                column: {
                                    stacking: 'normal'
                                },
                                spline: {
                                    marker: {
                                        enabled: true
                                    }

                                }
                            },
                            //lang:{

                            //    printChart: '打印图表',

                            //    downloadPNG: '下载JPEG 图片'
                            //},
                            credits: { //这里配置的是取消右下角  Highcharts版权连接 请允许我这么说
                                enabled: false
                            },
                            rangeSelector: {
                                buttons: [{//定义一组buttons,下标从0开始
                                    type: 'day',
                                    count: 1,
                                    text: '天'
                                }, {
                                    type: 'week',
                                    count: 1,
                                    text: '星期'
                                }, {
                                    type: 'month',
                                    count: 1,
                                    text: '月'
                                }, {
                                    type: 'all',
                                    text: '全部'

                                }],

                                buttonTheme: {
                                    width: 36,
                                    height: 16,
                                    padding: 1,
                                    r: 0,
                                    stroke: '#68A',
                                    zIndex: 7
                                },
                                inputDateFormat: '%Y-%m-%d',
                                inputEditDateFormat: '%Y-%m-%d',
                                selected: 1//表示以上定义button的index,从0开始
                            },
                            navigator: {
                                xAxis: {
                                    labels: {
                                        format: '{value:%Y-%m-%d}'
                                    }
                                }
                            },
                            legend: {
                                enabled: true,
                                borderRadius: 0,
                                borderWidth: 1
                            },


                            series: seriesOptions

                        });
                    };

                    var heavyMetalMonitor = $("#hdHeavyMetalMonitor").val();//查询的总的数据
                    var hiddenData = $("#HiddenData").val();//因子的Code和Name 用“|”分割
                    var pollutantCode = hiddenData.split('|')[0];//因子Code
                    var pollutantName = hiddenData.split('|')[1];//因子名称
                    var chartType = $("#HiddenChartType").val();//选择日期类型
                    var code = pollutantCode.split(';');
                    var name = pollutantName.split(';');
                    var strData = [];

                    var effectData = JSON.parse(heavyMetalMonitor);

                    for (var i = 0; i < code.length; i++) {
                        var objCode = code[i];
                        var allData = [];

                        if (chartType == "Day") {
                            $.each(effectData, function (key, obj) {
                                if (obj[objCode] != null) {
                                    allData.push([parseInt(obj.Tstamp), parseFloat(obj[objCode])]);
                                } else {
                                    allData.push([parseInt(obj.Tstamp), null]);
                                }
                            });
                        }
                        else if (chartType == "Month" || chartType == "Week") {
                            $.each(effectData, function (key, obj) {

                                if (obj[objCode] != null) {
                                    allData.push([parseInt(obj.DateTime), parseFloat(obj[objCode])]);
                                } else {
                                    allData.push([parseInt(obj.DateTime), null]);
                                }
                            });
                        }

                        if (objCode == "a34004")//PM2.5 为折现其他为柱形图
                        {
                            seriesOptions[i] = {
                                type: 'spline',
                                name: name[i],
                                data: allData,

                                zIndex: 2,
                                yAxis: 1,
                                fillColor: {
                                    linearGradient: {
                                        x1: 0,
                                        y1: 0,
                                        x2: 0,
                                        y2: 1
                                    },
                                    stops: [
                                        [0, Highcharts.getOptions().colors[i]],
                                        [1, Highcharts.Color(Highcharts.getOptions().colors[i]).setOpacity(0).get('rgba')]
                                    ]
                                }
                            };
                        }
                        else {

                            seriesOptions[i] = {
                                type: 'column',
                                name: name[i],
                                data: allData,

                                zIndex: 1,
                                yAxis: 0,
                                fillColor: {
                                    linearGradient: {
                                        x1: 0,
                                        y1: 0,
                                        x2: 0,
                                        y2: 1
                                    },
                                    stops: [
                                        [0, Highcharts.getOptions().colors[i]],
                                        [1, Highcharts.Color(Highcharts.getOptions().colors[i]).setOpacity(0).get('rgba')]
                                    ]
                                }
                            };
                        }

                    }

                    createChart();

                }

                //Splite加载事件（初始化Chart）
                function loadSplitter(sender) {
                    //$(function () {
                    //    InitTogetherChart();
                    //});
                }


                function OnClientClicking() {
                    var rbl = document.getElementsByName("radlDataType");
                    for (var i = 0; i < rbl.length; i++) {
                        if (rbl[i].checked && rbl[i].value == "Day") {
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

        //tab页切换时时间检查
        function OnClientSelectedIndexChanging(sender, args) {
            var rbl = document.getElementsByName("radlDataType");
            for (var i = 0; i < rbl.length; i++) {
                if (rbl[i].checked && rbl[i].value == "Day") {
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
        <asp:HiddenField ID="LoadChart" Value="1" runat="server" />
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
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="hdHeavyMetalMonitor" />
                        <telerik:AjaxUpdatedControl ControlID="dv" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointName" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" />

                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />

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
                <telerik:AjaxSetting AjaxControlID="weekTo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtweekT" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="weekEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="weekTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="txtweekT" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="ChartType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
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
                                Visible="false"></CbxRsm:PointCbxRsm>
                            <%-- <telerik:RadComboBox runat="server" ID="cbPoint" Width="160px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" Localization-CheckAllString="选择全部" EmptyMessage="请选择">
                            </telerik:RadComboBox>--%>
                            <telerik:RadDropDownList ID="cbPoint" runat="server" Width="160px">
                            </telerik:RadDropDownList>
                        </td>
                        <td class="title" style="width: 80px">监测因子:
                        </td>
                        <td class="content" style="width: 380px;">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="380" DropDownWidth="420" ID="factorCbxRsm" Visible="false"></CbxRsm:FactorCbxRsm>
                            <telerik:RadComboBox runat="server" ID="cbFactor" Width="160px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" Localization-CheckAllString="选择全部" EmptyMessage="请选择">
                            </telerik:RadComboBox>
                        </td>
                        <td></td>
                        <td></td>
                        <td class="content" align="left" style="width: 10px;" rowspan="3">
                            <asp:CheckBox ID="IsStatistical" Checked="false" Text="统计行" runat="server" Visible="false" />
                        </td>
                        <td class="content" align="left" rowspan="3">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">数据类型:
                        </td>
                        <td class="content" style="width: 200px;">
                            <asp:RadioButtonList ID="radlDataType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">时间:
                        </td>
                        <td class="content" style="width: 380px;">

                            <div runat="server" id="dbtDay">
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


                            <div runat="server" id="dbtWeek">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="weekBegin" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    OnSelectedDateChanged="weekBegin_SelectedDateChanged" AutoPostBack="true" />
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
                                                    MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    OnSelectedDateChanged="weekEnd_SelectedDateChanged" AutoPostBack="true" />
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
                    <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                        <div id="dv" runat="server" style="width: 100%; height: 100%">
                            <div id="container" style="width: 100%; height: 500px">
                            </div>
                        </div>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/AuditData.ashx" />
        <asp:HiddenField ID="HiddenChartType" runat="server" Value="spline" />
        <asp:HiddenField ID="hdHeavyMetalMonitor" runat="server" />
        <asp:HiddenField ID="hdPointName" runat="server" />

        <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/highstock.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>

    </form>
</body>
</html>
