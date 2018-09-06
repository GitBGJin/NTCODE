<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MicrowaveRadiation.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.SuperStationManagement.MicrowaveRadiation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%--<script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>--%>
    <script src="../../../Resources/jquery-1.9.0.min.js"></script>
    <script src="../../../Resources/highcharts.js"></script>
    <script src="../../../Resources/heatmap.js"></script>
    <script src="../../../Resources/ChartHeatmap.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            //tab切换事件控制数据类型的显示隐藏
            $("#tabStrip ul li").click(function () {
                var tapType = $(this).find("a").attr("rel");
                //;
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
                        $("#Typetitle").hide();
                        $("#Type").hide();
                        break;
                }
            })
            ////第一次加载全部隐藏
            //$("#Typetitle").hide();
            //$("#Type").hide();
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
<%--        <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
            <script src="../../../Resources/JavaScript/HighCharts/highcharts.js"></script>
            <script src="../../../Resources/JavaScript/ChartOperator/ShowChart.js"></script>
            <script src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>--%>
            <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
            <script type="text/javascript" language="javascript">
                function InitTogetherChart() {
                    $("#container").html("");
                    var seriesOptions = [];
                    createChart = function () {
                        Highcharts.setOptions({
                            lang: {
                                rangeSelectorZoom: ''    //隐藏Zoom
                            }
                        });
                        $('#container').highcharts({
                            chart: {
                                type: 'spline',
                                inverted: true
                            },
                            title: {
                                text: ''
                            },
                            subtitle: {
                                text: ''
                            },
                            xAxis: {
                                reversed: false,
                                title: { text: 'Km' }
                            },
                            yAxis: { title: { text: '' }, min: 0 },
                            tooltip: { valueSuffix: 'K' },
                            legend: {
                                layout: 'vertical', align: 'center',
                                verticalAlign: 'bottom', borderWidth: 0
                            },
                            plotOptions: {
                                series: {
                                    marker: {
                                        enabled: false
                                    }
                                },
                            },
                            series: seriesOptions
                        })
                    };
                    var heavyMetalMonitor = $("#hdwenduData").val();//查询的总的数据
                    var hiddenData = $("#hiddendiameter").val();//粒径
                    var strData = [];
                    var allData = [];
                    var effectData = JSON.parse(heavyMetalMonitor);
                    var diameter = hiddenData.split(';');
                    $.each(effectData, function (key, obj) {
                        for (var i = 0; i < hiddenData.length; i++) {
                            if (obj[diameter[i]] != null) {
                                allData.push([parseFloat(diameter[i]), parseFloat(obj[diameter[i]])]);
                            } else {
                                allData.push([parseFloat(diameter[i]), null]);
                            }
                        }
                    });
                    seriesOptions[0] = {
                        type: 'spline',
                        name: "温度",
                        data: allData,
                        zIndex: 1,
                        yAxis: 0
                    }
                    createChart();
                }

                function InitTogetherChartM() {
                    $("#container1").html("");

                    var seriesOptions = [];


                    createChart = function () {
                        Highcharts.setOptions({

                            lang: {
                                rangeSelectorZoom: ''    //隐藏Zoom
                            }
                        });
                        $('#container1').highcharts({
                            chart: {
                                type: 'spline',
                                inverted: true
                            },
                            title: {
                                text: ''
                            },
                            subtitle: {
                                text: ''
                            },
                            xAxis: {
                                reversed: false,
                                title: { text: 'Km' }
                            },
                            yAxis: { title: { text: '' }, min: 0 },
                            tooltip: { valueSuffix: 'g/m³' },
                            legend: {
                                layout: 'vertical', align: 'center',
                                verticalAlign: 'bottom', borderWidth: 0
                            },
                            plotOptions: {
                                series: {
                                    marker: {
                                        enabled: false
                                    }
                                },
                            },
                            series: seriesOptions
                        })
                    };
                    var miduMonitor = $("#hdmiduData").val();//查询的总的数据
                    var hiddenData = $("#hiddendiameter").val();//粒径
                    var strData = [];
                    var allData = [];
                    var effectData = JSON.parse(miduMonitor);
                    var diameter = hiddenData.split(';');
                    $.each(effectData, function (key, obj) {
                        for (var i = 0; i < hiddenData.length; i++) {
                            if (obj[diameter[i]] != null) {
                                allData.push([parseFloat(diameter[i]), parseFloat(obj[diameter[i]])]);
                            } else {
                                allData.push([parseFloat(diameter[i]), null]);
                            }
                        }
                    });
                    seriesOptions[0] = {
                        type: 'spline',
                        name: "蒸汽密度",
                        data: allData,
                        zIndex: 1,
                        yAxis: 0
                    }
                    createChart();
                }

                function InitTogetherChartS() {
                    $("#container2").html("");

                    var seriesOptions = [];


                    createChart = function () {
                        Highcharts.setOptions({

                            lang: {
                                rangeSelectorZoom: ''    //隐藏Zoom
                            }
                        });
                        $('#container2').highcharts({
                            chart: {
                                type: 'spline',
                                inverted: true
                            },
                            title: {
                                text: ''
                            },
                            subtitle: {
                                text: ''
                            },
                            xAxis: {
                                reversed: false,
                                title: { text: 'Km' }
                            },
                            yAxis: { title: { text: '' }, min: 0 },
                            tooltip: { valueSuffix: '%' },
                            legend: {
                                layout: 'vertical', align: 'center',
                                verticalAlign: 'bottom', borderWidth: 0
                            },
                            plotOptions: {
                                series: {
                                    marker: {
                                        enabled: false
                                    }
                                },
                            },
                            series: seriesOptions
                        })
                    };
                    var shiduMonitor = $("#hdshiduData").val();//查询的总的数据
                    var hiddenData = $("#hiddendiameter").val();//粒径
                    var strData = [];
                    var allData = [];
                    var effectData = JSON.parse(shiduMonitor);
                    var diameter = hiddenData.split(';');
                    $.each(effectData, function (key, obj) {
                        for (var i = 0; i < hiddenData.length; i++) {
                            if (obj[diameter[i]] != null) {
                                allData.push([parseFloat(diameter[i]), parseFloat(obj[diameter[i]])]);
                            } else {
                                allData.push([parseFloat(diameter[i]), null]);
                            }
                        }
                    });
                    seriesOptions[0] = {
                        type: 'spline',
                        name: "相对湿度",
                        data: allData,
                        zIndex: 1,
                        yAxis: 0
                    }
                    createChart();
                    ;
                }
                var isfirst = 1;//是否首次加载
                var isSecond = 1;
                function TabSelected(sender, args) {
                    try {
                        var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                        var isSecond = document.getElementById("<%=SecondLoadChart.ClientID%>");
                        var text = $.trim(args._tab._element.innerText);
                        ;
                            if (text == '图表') {
                                if (isfirst.value == "1") {
                                    isfirst.value = 0;
                                    SearchData();
                                }
                            }
                            else if (text == '热力图') {
                                if (isSecond.value == "1") {
                                    isSecond.value = 0;
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
                        InitTogetherChart();
                        InitTogetherChartM();
                        InitTogetherChartS();
                    }
                    function RefreshChart() {
                        try {
                            var chartPage = document.getElementById("pvChart");
                            chartPage.children[0].contentWindow.InitChart();
                        } catch (e) {
                        }
                    }
                    function OnClientClicking() {
                        var date1 = new Date();
                        var date2 = new Date();
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
                    //Grid按钮行处理
                    function gridRTB_ClientButtonClicking(sender, args) {
                        args.set_cancel(!OnClientClicking());
                    }
                    //tab页切换时时间检查
                    function OnClientSelectedIndexChanging(sender, args) {
                        var date1 = new Date();
                        var date2 = new Date();
                        date1 = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                        date2 = $find("<%= dayEnd.ClientID %>").get_selectedDate();
                        if ((date1 == null) || (date2 == null)) {
                            alert("开始时间或者终止时间，不能为空！");
                            args.set_cancel(true);
                            return;
                        }
                        if (date1 > date2) {
                            alert("开始时间不能大于终止时间！");
                            args.set_cancel(true);
                            return;
                        } else {
                            return;
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
                    //控制导出时按钮不会隐藏掉处理
                    //function onRequestStart(sender, args) {
                    //    if (args.EventArgument == 6 || args.EventArgument == 7 ||
                    //        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                    //            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                    //            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                    //        args.set_enableAjax(false);
                    //    }
                    //}
                    function onRequestEnd(sender, args) {
                    }

                    function chart() {
                        //var strJSON = $("#hdHeavyMetalMonitor").val();//获取数据源
                        //ajaxHighChart("", "", strJSON, "container", "时间", "", "");//传值
                        //;
                        var strJSON1 = $("#MicrowaveRadiationTemperature").val();
                        ajaxHighChart("微波辐射温度热力图", "微波辐射温度热力图", strJSON1, "HeatChart1", "时间", "Km", "温度(K)");
                        var strJSON2 = $("#MicrowaveRadiationVaporDensity").val();
                        ajaxHighChart("微波辐射蒸汽密度热力图", "微波辐射蒸汽密度热力图", strJSON2, "HeatChart2", "时间", "Km", "蒸汽密度(g/m³)");
                        var strJSON3 = $("#MicrowaveRadiationRelativeHumidity").val();
                        ajaxHighChart("微波辐射相对湿度热力图", "微波辐射相对湿度热力图", strJSON3, "HeatChart3", "时间", "Km", "相对湿度(%)");
                    }
                    //function chart1() {
                    //    //var strJSON = $("#hdHeavyMetalMonitor").val();//获取数据源
                    //    //ajaxHighChart("", "", strJSON, "container", "时间", "", "");//传值
                    //    //;
                    //    var strJSON = $("#MicrowaveRadiationTemperature").val();
                    //    ajaxHighChart("微波辐射温度热力图", "微波辐射温度热力图", strJSON, "HeatChart1", "时间", "Km", "温度(K)");
                    //}
                    //function chart2() {
                    //    var strJSON = $("#MicrowaveRadiationVaporDensity").val();
                    //    ajaxHighChart("微波辐射蒸汽密度热力图", "微波辐射蒸汽密度热力图", strJSON, "HeatChart2", "时间", "Km", "蒸汽密度(g/m³)");
                    //}
                    //function chart3() {
                    //    var strJSON = $("#MicrowaveRadiationRelativeHumidity").val();
                    //    ajaxHighChart("微波辐射相对湿度热力图", "微波辐射相对湿度热力图", strJSON, "HeatChart3", "时间", "Km", "相对湿度(%)");
                    //}
                    //Splite加载事件（初始化Chart）
                    function loadSplitter(sender) {
                        $(function () {
                            InitTogetherChart();
                            InitTogetherChartM();
                            InitTogetherChartS();
                            var strJSON1 = $("#MicrowaveRadiationTemperature").val();
                            ajaxHighChart("微波辐射温度热力图", "微波辐射温度热力图", strJSON1, "HeatChart1", "时间", "Km", "温度(K)");
                            var strJSON2 = $("#MicrowaveRadiationVaporDensity").val();
                            ajaxHighChart("微波辐射蒸汽密度热力图", "微波辐射蒸汽密度热力图", strJSON2, "HeatChart2", "时间", "Km", "蒸汽密度(g/m³)");
                            var strJSON3 = $("#MicrowaveRadiationRelativeHumidity").val();
                            ajaxHighChart("微波辐射相对湿度热力图", "微波辐射相对湿度热力图", strJSON3, "HeatChart3", "时间", "Km", "相对湿度(%)");
                        });
                    }                        
            </script>
        </telerik:RadCodeBlock>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <asp:HiddenField ID="FirstLoadChart" Value="1" runat="server" />
        <asp:HiddenField ID="SecondLoadChart" Value="1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tabStrip">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridWeibo" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdwenduData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hiddendiameter" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdmiduData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdshiduData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="SecondLoadChart" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dv" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvmidu" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvshidu" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lbTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="MicrowaveRadiationTemperature" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="MicrowaveRadiationVaporDensity" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="MicrowaveRadiationRelativeHumidity" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridWeibo" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdwenduData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hiddendiameter" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdmiduData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdshiduData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="SecondLoadChart" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dv" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvmidu" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvshidu" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lbTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="MicrowaveRadiationTemperature" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="MicrowaveRadiationVaporDensity" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="MicrowaveRadiationRelativeHumidity" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HeatChart1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HeatChart2" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HeatChart3" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="preSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="hdwenduData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hiddendiameter" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdmiduData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdshiduData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dv" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvmidu" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvshidu" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lbTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="nextSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="hdwenduData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hiddendiameter" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdmiduData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdshiduData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dv" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvmidu" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvshidu" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lbTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenNum" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridWeibo"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0" OnClientLoad="loadSplitter">
            <telerik:RadPane ID="paneWhere" runat="server" Height="70px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">测点:
                        </td>
                        <td class="content" style="width: 200px;">
                            <telerik:RadDropDownList ID="cbPoint" runat="server" Width="160px">
                                <Items>
                                    <telerik:DropDownListItem Text="监测站" Value="9" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                        <td class="title" style="width: 80px">
                            <div id="Typetitle">监测因子:</div>
                        </td>
                        <td class="content" style="width: 200px;">
                            <div id="Type">
                                <telerik:RadComboBox runat="server" ID="cbFactor" Width="180px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" Localization-CheckAllString="选择全部" EmptyMessage="请选择">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="温度" Value="401" Checked="true" />
                                        <telerik:RadComboBoxItem Text="蒸汽密度" Value="402" />
                                        <telerik:RadComboBoxItem Text="相对湿度" Value="404" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 360px;">
                            <telerik:RadDateTimePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="220px"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">结束时间:
                        </td>
                        <td class="content" style="width: 360px;">
                            <telerik:RadDateTimePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" Width="220px"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
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
                        <telerik:RadTab Text="热力图" rel="3" Value="2">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="Y"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="grvGrid" runat="server" Visible="true">
                        <telerik:RadGrid ID="gridWeibo" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="true"
                            AutoGenerateColumns="true" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="gridWeibo_NeedDataSource" OnItemDataBound="gridWeibo_ItemDataBound" OnColumnCreated="gridWeibo_ColumnCreated"
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
                        <div id="dv" runat="server" style="width: 33%; height: 90%; float: left;">
                            <div id="container" style="width: 98%; height: 480px">
                            </div>
                        </div>
                        <div id="dvmidu" runat="server" style="width: 33%; height: 90%; float: left">
                            <div id="container1" style="width: 98%; height: 480px">
                            </div>
                        </div>
                        <div id="dvshidu" runat="server" style="width: 34%; height: 90%; float: right">
                            <div id="container2" style="width: 98%; height: 480px">
                            </div>
                        </div>
                        <div style="width: 80%; height: 10%;">
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
                                        <label runat="server" id="lbTime" style="width: 200px"></label>
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
                        </div>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="HighChart" runat="server">
                        <div id="Hdv1" runat="server" style="width: 33%; height: 100%;float:left;" >
                            <div id="HeatChart1" style="width: 98%; height: 600px;margin:0 auto;" runat="server" >
                            </div>
                        </div>
                        <div id="Hdv2" runat="server" style="width: 33%; height: 100%;float:left;">
                            <div id="HeatChart2" style="width: 98%; height: 600px;margin:0 auto;" runat="server">
                            </div>
                        </div>
                        <div id="Hdv3" runat="server" style="width: 34%; height: 100%;float:left;">
                            <div id="HeatChart3" style="width: 98%; height: 600px;margin:0 auto;" runat="server">
                            </div>
                        </div>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="hdwenduData" runat="server" />
        <asp:HiddenField ID="hdmiduData" runat="server" />
        <asp:HiddenField ID="hdshiduData" runat="server" />
        <asp:HiddenField ID="hiddendiameter" runat="server" />
        <asp:HiddenField ID="HiddenNum" runat="server" Value="0" />
        <asp:HiddenField ID="HiddenLable" runat="server" />
        <asp:HiddenField ID="MicrowaveRadiationTemperature" runat="server" />
        <asp:HiddenField ID="MicrowaveRadiationVaporDensity" runat="server" />
        <asp:HiddenField ID="MicrowaveRadiationRelativeHumidity" runat="server" />
    </form>
</body>
</html>
