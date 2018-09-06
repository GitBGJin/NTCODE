<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewTest.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.NewTest" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link type="text/css" rel="stylesheet" href="../../../App_Themes/Neptune/Telerik.Web.UI.Skins/Metro/RadSplitterAudit.css" />
    <style type="text/css">
        .RadCalendar_Neptune .rcTitlebar {
            background: #3A94D3 !important;
            background-image: none !important;
            border: 1px solid #3A94D3 !important;
        }

        #RadCalendar1_Title {
            color: #fff !important;
        }

        .RadCalendar_Neptune .rcTitlebar .RadCalendar_Neptune .rcTitlebar TABLE {
            background: #3A94D3 !important;
        }
    </style>
    <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
        <script src="../../../Resources/JavaScript/PChart/jquery-1.8.3.min.js"></script>
            <script src="../../../Resources/JavaScript/PChart/highcharts.js"></script>
            <script src="../../../Resources/JavaScript/PChart/exporting.js"></script>
            <script src="../../../Resources/JavaScript/PChart/highcharts-zh_CN.js"></script>
            <script type="text/javascript" src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
        <script type="text/javascript">
            //function onRequestStart(sender, args) {
            //    if (args.EventArgument == "")
            //        return;
            //    if (args.EventArgument == 0 || args.EventArgument == 1 ||
            //        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
            //            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
            //            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
            //        args.set_enableAjax(false);
            //    }
            //}
            var isfirst = 1;//是否首次加载
            function TabSelected(sender, args) {
                try {
                    var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                    var isfirsts = document.getElementById("<%=FirstLoadCharts.ClientID%>");
                    var text = $.trim(args._tab._element.innerText);
                    if (text == '柱形图') {
                        if (isfirst.value == "1") {
                            //isfirst.value = 1;
                            ColChart();
                        }
                    }
                    else if (text == '饼图') {
                        if (isfirsts.value == "1") {
                            //isfirsts.value = 0;
                            PieChart();
                        }
                    }
                } catch (e) {
                }
            }
            function RadCalendarClick(sender, eventArgs) {
                var day = eventArgs.get_renderDay();
                if (day.get_isSelectable()) {
                    if (day._date && day._date.length == 3) {
                        //document.getElementById("<%=selectDateTime.ClientID%>").value = day._date[0] + "-" + day._date[1] + "-" + day._date[2];
                    }
                    //document.getElementById('EnterAudit').click();
                }
            }

            function ColChart() {
                var hdcol = document.getElementById("hdCol").value;
                var hdname = document.getElementById("hdName").value;
                var hdmonths = document.getElementById("hdMonth").value;
                console.log(hdcol);
                console.log(hdmonths);
                hdcol = eval("(" + hdcol + ")");
                hdmonths = eval("(" + hdmonths + ")");
                console.log(hdcol);
                console.log(hdmonths);
                var chart = new Highcharts.Chart("ChartContainer", {
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: '各月份达标率'
                    },
                    xAxis: {
                        categories: hdmonths,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: '达标率（%）'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: hdname,
                        data: hdcol
                    }]
                });
            }
            function PieChart() {
                var hdvalue = document.getElementById("hdPie").value;
                hdvalue = eval("(" + hdvalue + ")");
                hdPie.value = "";
                console.log(hdvalue);
                var chart = new Highcharts.Chart("PieChart", {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false
                    },
                    title: {
                        text: '空气质量天数比例'
                    },
                    tooltip: {
                        headerFormat: '{series.name}<br>',
                        pointFormat: '{point.name}: <b>{point.percentage:.3f}%</b>'
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.3f} %',
                                style: {
                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            }
                        }
                    },
                    series: [{
                        type: 'pie',
                        name: '空气质量类别占比',
                        data: hdvalue
                    }]
                });
            };
            //设置容器宽度、高度
            $("document").ready(function () {
                //ResizePageDiv();
            });

            function loadSplitter(sender) {
                GridResize();
                SetCalenderHight();
            }

            ////蒙版宽度高度设置
            //function ResizePageDiv() {
            //    var bodyWidth = document.body.clientWidth;
            //    var bodyHeight = document.body.clientHeight;
            //    $('#pagediv').css("height", bodyHeight);
            //    $('#pagediv').css("width", bodyWidth);
            //}

            //隐藏站点按钮
            function HidePanel(send, args) {
                $('#RAD_SLIDING_PANE_TAB_RadSlidingPane_Point').css("display", "none");
            }

            //显示站点按钮
            function ShowPanel(send, args) {
                $('#RAD_SLIDING_PANE_TAB_RadSlidingPane_Point').css("display", "block");

            }

            ////重新设置表格高度
            //function GridResize() {
            //    var bodyHeight = document.body.clientHeight;
            //    $('#RadGridAnalyze_GridData').css("height", bodyHeight - bodyHeight * 0.58 - 28 - 12);//设置表格高度 
            //}

            ////设置日历高度
            //function SetCalenderHight() {
            //    var height = (parseFloat($('#RadPane3').css("height")) - 33 - 30 - 60 - 20) / 6;
            //    if (height > 10)
            //        $(".tableAdt").height(height);
            //    else
            //        $(".tableAdt").height(10);
            //}

            function OnClientNotificationUpdated(sender, args) {
                var newMsgs = sender.get_value();
                if (newMsgs != 0) {
                    play();
                    sender.show();
                }
            }
            function OnClientNotificationHidden(sender, eventArgs) {
            }

            function CalendarViewChanged(sender, args) {
                //SetCalenderHight();
            }
            function checkAll(button, args) {
                for (var i = 0; i < document.getElementById("radPoint").getElementsByTagName("input").length; i++) {
                    document.getElementById("radPoint_" + i).checked = true;
                }
            }
            function deleteAll(button, args) {
                for (var i = 0; i < document.getElementById("radPoint").getElementsByTagName("input").length; i++) {
                    document.getElementById("radPoint_" + i).checked = false;
                }
            }
            function ReverseAll(button, args) {
                for (var i = 0; i < document.getElementById("radPoint").getElementsByTagName("input").length; i++) {
                    var objCheck = document.getElementById("radPoint_" + i);
                    if (objCheck.checked)
                        objCheck.checked = false;
                    else
                        objCheck.checked = true;
                }
            }
//tab页切换时时间检查
function OnClientSelectedIndexChanging(sender, args) {
    var hourB = $find("<%= RadDatePickerBegin.ClientID %>").get_selectedDate();
    var hourE = $find("<%= RadDatePickerEnd.ClientID %>").get_selectedDate();
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
        </script>
    </telerik:RadCodeBlock>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <asp:HiddenField ID="FirstLoadChart" Value="1" runat="server" />
        <asp:HiddenField ID="FirstLoadCharts" Value="1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnablePageHeadUpdate="false" OnAjaxSettingCreating="RadAjaxManager1_AjaxSettingCreating">
            <AjaxSettings>
                <%--<telerik:AjaxSetting AjaxControlID="refreshData">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadCalendar1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsms"></telerik:AjaxUpdatedControl>
                        
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radPoint">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="selectAll">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="inverse">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="tabStrip">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="SecondLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="PieChart" />
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="Search">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadCalendar1"  LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="PieChart" />
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" />
                        <telerik:AjaxUpdatedControl ControlID="hdPie" />
                        <telerik:AjaxUpdatedControl ControlID="hdCol" />
                        <telerik:AjaxUpdatedControl ControlID="hdName" />
                        <telerik:AjaxUpdatedControl ControlID="hdMonth" />
                        <%--<telerik:AjaxUpdatedControl ControlID="pagediv" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadCalendar1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadCalendar1"  LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <%--<ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onResponseEnd" />--%>
        </telerik:RadAjaxManager>
        <div runat="server" id="pagediv" style="position: absolute; z-index: -1;"></div>
            <telerik:RadSplitter ID="RadSplitter2" runat="server" Orientation="Horizontal" BorderSize="0" Width="100%" Height="100%">
                <telerik:RadPane ID="RadPane2" runat="server" Height="6%" Scrolling="None">
                    <div id="paramSelect">
                            <table style="width: 100%; text-align: left">
                                <tr>
                                    <td class="title" style="width: 80px; text-align: center;">查询类型:
                        </td>
                        <td class="content" style="width: 240px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged" RepeatLayout="Flow" RepeatColumns="5">
                                <asp:ListItem Text="区域" Value="CityProper"></asp:ListItem>
                                <asp:ListItem Text="测点" Value="Port" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">站点:
                        </td>
                        <td class="content" style="width: 300px;">
                            <div runat="server" id="dvPoint">
                                <CbxRsm:PointCbxRsm runat="server" DefaultIPointMode="Region" ApplicationType="Air" CbxWidth="220"  CbxHeight="350" MultiSelected="false" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                                 <CbxRsm:PointCbxRsm runat="server" DefaultIPointMode="Region" ApplicationType="Air" CbxWidth="220" Visible="false"  CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsms"></CbxRsm:PointCbxRsm>
                            </div>
                        </td>
                                    <td style="width: 70px;">开始时间：</td>
                                    <td style="width: 140px;">
                                        <%--<telerik:RadDatePicker Width="120" runat="server" ID="RadDatePickerBegin" AutoPostBack="false"
                                            MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时" OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged">
                                        </telerik:RadDatePicker>--%>
                                        <telerik:RadMonthYearPicker ID="RadDatePickerBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="105px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                    </td>
                                    <td style="width: 70px;">结束时间：</td>
                                    <td style="width: 140px;">
                                        <%--<telerik:RadDatePicker Width="120" runat="server" ID="RadDatePickerEnd" AutoPostBack="false"
                                            MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时" OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged">
                                        </telerik:RadDatePicker>--%>
                                        <telerik:RadMonthYearPicker ID="RadDatePickerEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" Width="105px"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"  OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                    </td>
                                    <td style="width: 80px;">
                                        <telerik:RadButton ID="Search" runat="server" BackColor="#3A94D3" Visible="true" ForeColor="White" AutoPostBack="true" OnClick="Search_Click">
                                            <ContentTemplate>
                                                <asp:Label ID="Label1" runat="server" ForeColor="White" Text="查询"></asp:Label>
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                    <td></td>
                                    <td id="tooltipLoading">
                                        <telerik:RadNotification ID="RadNotification1" runat="server" LoadContentOn="TimeInterval"
                                            Width="200" Height="30" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true"
                                            OnClientUpdated="OnClientNotificationUpdated" OnClientHidden="OnClientNotificationHidden" Title="提示" ShowTitleMenu="false" ShowCloseButton="false" VisibleTitlebar="false"
                                            TitleIcon="none" AutoCloseDelay="3000" OffsetX="360" OffsetY="2" Position="TopCenter">
                                            <ContentTemplate>
                                                <asp:Label ID="Label1" runat="server" Text="加载数据中，请耐心等待..."></asp:Label>
                                            </ContentTemplate>
                                        </telerik:RadNotification>
                                    </td>
                                    <td style="width: 80px;">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </telerik:RadPane>
                <telerik:RadPane ID="paneTab" runat="server" Scrolling="None" Width="100%" Height="6%"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadTabStrip ID="tabStrip" runat="server" SelectedIndex="0" MultiPageID="multiPage" OnClientTabSelecting="OnClientSelectedIndexChanging"
                    CssClass="RadTabStrip_Customer" OnClientTabSelected="TabSelected">
                    <Tabs>
                        <telerik:RadTab Text="日历表">
                        </telerik:RadTab>
                        <telerik:RadTab Text="柱形图">
                        </telerik:RadTab>
                        <telerik:RadTab Text="饼图">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
                <telerik:RadPane ID="RadPane3" runat="server" Scrolling="None" Width="100%" Height="800px">
                    <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="800px" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="grvGrid" runat="server" Visible="true">
                        <div style="padding-top: 6px; padding-left: 6px;">
                                        <telerik:RadCalendar ID="RadCalendar1" OnDayRender="RadCalendar1_DayRender"  EnableRepeatableDaysOnClient="false" runat="server" MultiViewRows="3" MultiViewColumns="4"
                                            MonthLayout="Layout_7columns_x_6rows" OnDefaultViewChanged="RadCalendar1_DefaultViewChanged"  ClientEvents-OnDateClick="RadCalendarClick"
                                            CssClass="rcHeaderStyle" TitleFormat="yyyy 年 MM 月"
                                            AutoPostBack="True" FastNavigationSettings-TodayButtonCaption="当前年月" FastNavigationSettings-OkButtonCaption="确定"
                                            FastNavigationSettings-CancelButtonCaption="取消" ShowOtherMonthsDays="false" ShowColumnHeaders="true"
                                            ShowRowHeaders="false" CultureInfo="Chinese (People's Republic of China)" EnableMultiSelect="False"
                                            Width="100%" TitleStyle-Height="28" ClientEvents-OnCalendarViewChanged="CalendarViewChanged" DayCellToolTipFormat="yyyy年MM月dd日">
                                            <HeaderTemplate></HeaderTemplate>
                                            <CalendarDayTemplates>
                                                <telerik:DayTemplate ID="Adt" runat="server">
                                                    <Content>
                                                        <div style="text-align: center; cursor: pointer; font-size: 16px; font-weight: bold; color: Black; width: 100%; position: static;">
                                                            <table class="tableAdt" border="0" style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#getDay((DataBinder.Eval(Container, "ClientID")).ToString())%>'></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </Content>
                                                </telerik:DayTemplate>
                                                <telerik:DayTemplate ID="Adt一级" runat="server">
                                                    <Content>
                                                        <div style="background: #7DC733; text-align: center; cursor: pointer; font-size: 16px; font-weight: bold; color: Black; width: 100%; position: static;">
                                                            <table class="tableAdt" border="0" style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#getDay((DataBinder.Eval(Container, "ClientID")).ToString())%>'></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </Content>
                                                </telerik:DayTemplate>
                                                <telerik:DayTemplate ID="Adt二级" runat="server">
                                                    <Content>
                                                        <div style="background: #FFFF00; text-align: center; cursor: pointer; font-size: 16px; font-weight: bold; color: Black; width: 100%; position: static;">
                                                            <table class="tableAdt" border="0" style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#getDay((DataBinder.Eval(Container, "ClientID")).ToString())%>'></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </Content>
                                                </telerik:DayTemplate>
                                                <telerik:DayTemplate ID="Adt三级" runat="server">
                                                    <Content>
                                                        <div style="background: #FF7E00; text-align: center; cursor: pointer; font-size: 16px; font-weight: bold; color: Black; width: 100%; position: static;">
                                                            <table class="tableAdt" border="0" style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#getDay((DataBinder.Eval(Container, "ClientID")).ToString())%>'></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </Content>
                                                </telerik:DayTemplate>
                                                <telerik:DayTemplate ID="Adt四级" runat="server">
                                                    <Content>
                                                        <div style="background: #FF0000; text-align: center; cursor: pointer; font-size: 16px; font-weight: bold; color: Black; width: 100%; position: static;">
                                                            <table class="tableAdt" border="0" style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#getDay((DataBinder.Eval(Container, "ClientID")).ToString())%>'></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </Content>
                                                </telerik:DayTemplate>
                                                <telerik:DayTemplate ID="Adt五级" runat="server">
                                                    <Content>
                                                        <div style="background: #99004c; text-align: center; cursor: pointer; font-size: 16px; font-weight: bold; color: Black; width: 100%; position: static;">
                                                            <table class="tableAdt" border="0" style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#getDay((DataBinder.Eval(Container, "ClientID")).ToString())%>'></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </Content>
                                                </telerik:DayTemplate>
                                                <telerik:DayTemplate ID="Adt六级" runat="server">
                                                    <Content>
                                                        <div style="background: #7e0023; text-align: center; cursor: pointer; font-size: 16px; font-weight: bold; color: Black; width: 100%; position: static;">
                                                            <table class="tableAdt" border="0" style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#getDay((DataBinder.Eval(Container, "ClientID")).ToString())%>'></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </Content>
                                                </telerik:DayTemplate>
                                            </CalendarDayTemplates>
                                            <CalendarTableStyle Width="100%" />
                                        </telerik:RadCalendar>
                                        <div style="border-bottom: solid 1px; border-right: solid 1px; border-left: solid 1px; border-color: #3A94D3;">
                                            <table class="Table_Customer" style="width: 100%; height: 33px; color: White; font-size: 12px;">
                                                <tr>
                                                    <td class="title" style="width: 30px;">图示:</td>
                                                    <td class="content" style="width: 20px;">
                                                        <div style="width: 20px; height: 20px; background: #7DC733;"></div>
                                                    </td>
                                                    <td class="content" style="width: 30px; vertical-align: central;">优</td>
                                                    <td class="content" style="width: 20px;">
                                                        <div style="width: 20px; height: 20px; background: #FFFF00;"></div>
                                                    </td>
                                                    <td class="content" style="width: 30px; vertical-align: central;">良</td>
                                                    <td class="content" style="width: 20px;">
                                                        <div style="width: 20px; height: 20px; background: #FF7E00;"></div>
                                                    </td>
                                                    <td class="content" style="width: 30px; vertical-align: central;">轻度污染</td>
                                                    <td class="content" style="width: 20px;">
                                                        <div style="width: 20px; height: 20px; background: #FF0000;"></div>
                                                    </td>
                                                    <td class="content" style="width: 30px; vertical-align: central;">中度污染</td>
                                                    <td class="content" style="width: 20px;">
                                                        <div style="width: 20px; height: 20px; background: #99004c;"></div>
                                                    </td>
                                                    <td class="content" style="width: 30px; vertical-align: central;">重度污染</td>
                                                    <td class="content" style="width: 20px;">
                                                        <div style="width: 20px; height: 20px; background: #7e0023;"></div>
                                                    </td>
                                                    <td class="content" style="width: 30px; vertical-align: central;">严重污染</td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                        </telerik:RadPageView>
                <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                    <div id="ChartContainer" runat="server" style=" width: 100%;  height: 500px;">
                            </div>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pChart" runat="server" Visible="true">
                        <div id="PieChart" runat="server" style=" width: 100%;  height: 500px;"></div>
                        </telerik:RadPageView>
                        </telerik:RadMultiPage>
                    </telerik:RadPane>
                </telerik:RadSplitter>
        <div style="display: none;">
            <asp:HiddenField ID="selectDateTime" runat="server" Value="2016-01-01" />
            <asp:HiddenField ID="hdPie" runat="server" Value="0" />
            <asp:HiddenField ID="hdName" runat="server" Value="" />
            <asp:HiddenField ID="hdCol" runat="server" Value="" />
            <asp:HiddenField ID="hdMonth" runat="server" Value="" />
            <telerik:RadButton ID="EnterAudit" runat="server" AutoPostBack="true" OnClick="EnterAudit_Click"></telerik:RadButton>
        </div>
    </form>
</body>
</html>
