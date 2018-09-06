<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GranuleSpecial.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Special.GranuleSpecial2" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="../../../Resources/CSS/pagination.css" />
    <title></title>
    <script src="../../../Resources/JavaScript/Ladar/jquery-1.9.0.min.js"></script>
    <script src="../../../Resources/JavaScript/Ladar/highcharts.js"></script>
    <script src="../../../Resources/JavaScript/Ladar/heatmap.js"></script>
    <script src="../../../Resources/JavaScript/Ladar/ChartHeatmap.js"></script>
    <script type="text/javascript" src="../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist/echarts.js"></script>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            $(function () {
                ShowECharts();
            });

            function ShowECharts() {
                /*var obj = str.parseJSON(); //由JSON字符串转换为JSON对象
                  或者 var obj = JSON.parse(str); //由JSON字符串转换为JSON对象
                */

                var strAirWeather = document.getElementById("<%=hdAirWeather.ClientID%>").value;
                var AirWeather = JSON.parse(strAirWeather);
                var Tstamp = [];
                var a01001 = [];
                var a01002 = [];
                var a01006 = [];
                var a01007 = [];
                var a01008 = [];

                $.each(AirWeather, function (key, obj) {
                    Tstamp.push(obj.Tstamp);
                    a01001.push(obj.a01001);
                    a01002.push(obj.a01002);
                    a01006.push(obj.a01006);
                    a01007.push(obj.a01007);
                    a01008.push(obj.a01008);
                });
                var List = [];
                var j = 0;
                for (var i in a01007) {
                    debugger;
                    List.push({ value: a01007[i], symbolRotate: a01008[i] })
                }
                require.config({
                    paths: {
                        echarts: '../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist'
                    }
                });
                require(
                    [
                        'echarts',
                        'echarts/chart/bar',
                        'echarts/chart/line',
                    ],
                    function (ec) {
                        //--- 饼图 ---
                        var myChart = ec.init(document.getElementById('divAirQualityLevel'));
                        option = {
                            tooltip: {
                                trigger: 'axis',
                                axisPointer: {
                                    type: 'cross',
                                    crossStyle: {
                                        color: '#999'
                                    }
                                }
                            },
                            toolbox: {
                                feature: {
                                    dataView: { show: true, readOnly: false },
                                    magicType: { show: true, type: ['line', 'bar'] },
                                    restore: { show: true },
                                    saveAsImage: { show: true }
                                }
                            },
                            legend: {
                                data: ['湿度', '温度', '气压', '风速风向']
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    axisTick: {
                                        alignWithLabel: true
                                    },
                                    data: Tstamp,
                                    axisPointer: {
                                        type: 'shadow'
                                    }
                                }
                            ],
                            yAxis: [
                                 {
                                     type: 'value',
                                     name: '气压',
                                     position: 'right',
                                     axisLabel: {
                                         formatter: '{value} hPa'
                                     }
                                 },
        {
            type: 'value',
            name: '风速',
            position: 'right',
            offset: 80,
            axisLabel: {
                formatter: '{value} m/s'
            }
        }
                            ],
                            series: [
                                {
                                    name: '湿度',
                                    type: 'bar',
                                    itemStyle: {
                                        normal: {
                                            color: '#5CACEE'
                                        }
                                    },
                                    data: a01002
                                },
                                {
                                    name: '温度',
                                    type: 'line',
                                    symbolSize: 4,
                                    smooth: true,  //这句就是让曲线变平滑的  
                                    itemStyle: {
                                        normal: {
                                            color: '#98FB98',
                                            lineStyle: {
                                                width: 1
                                            }

                                        }
                                    },
                                    data: a01001
                                }, {
                                    name: '气压',
                                    type: 'line',
                                    symbol: 'none',
                                    smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                                    itemStyle: {
                                        normal: {
                                            color: 'black',
                                            lineStyle: {
                                                width: 1,
                                                type: 'dotted'  //'dotted'虚线 'solid'实线
                                            },
                                            label: {
                                                show: true
                                            }
                                        },
                                        emphasis: {
                                            color: 'blue'
                                        }
                                    },
                                    smooth: true,  //这句就是让曲线变平滑的  
                                    data: a01006
                                },
                                {
                                    name: '风速风向',
                                    type: 'line',
                                    symbolSize: 6,
                                    symbol: 'arrow',
                                    smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                                    itemStyle: {
                                        normal: {
                                            color: 'blue',
                                            lineStyle: {
                                                width: 1,
                                                type: 'dotted'  //'dotted'虚线 'solid'实线
                                            }
                                        },
                                        emphasis: {
                                            color: 'blue'
                                        }
                                    },
                                    smooth: true,  //这句就是让曲线变平滑的  
                                    yAxisIndex: 1,
                                    data: List
                                }
                            ]
                        };

                        myChart.setOption(option);
                    }
                );
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

}
}

function RefreshChart() {
    try {
        var chartPage = document.getElementById("pvChart");
        chartPage.children[0].contentWindow.InitChart();
    } catch (e) {
    }
}
        </script>
        <script type="text/javascript" src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
        <script type="text/javascript">

            //页面加载初始化页面高度
            $(document).ready(function () {
                debugger;
                GetShow();
            });

            //数据行选中事件
            function GetSelect(obj) {
                //添加选中效果
                $(obj).parent().find("tr").each(function (index) {
                    if (this.id != "tr_title") {
                        $(this).css('background-color', '#FFFFFF');
                    }
                })
                $(obj).css('background-color', '#FFA544');

                //获取图片路径
                var arr = [];
                $(obj).find("input:hidden").each(function (index) {
                    arr.push(this.value);
                });
                //清除历史图片
                document.getElementById("divImg").style.background = "#FFFFFF";
                //document.getElementById("divPT").style.background = "#FFFFFF";
                //重新加载图片

                var count = arr.length;
                if (count > 0) {
                    GetImgUrl(arr[0]);
                    if (count == 2) {
                        GetImgUrl(arr[1])
                    }
                }
            }

            //重新加载图片
            function GetImgUrl(imgurl) {
                debugger;
                var filename = imgurl.substring(imgurl.lastIndexOf('/') + 1).toLowerCase();
                var fileurl = imgurl.replace('~', '../../..');
                var rbl = document.getElementsByName("radlDataType");
                document.getElementById("imgName").innerText = "城市影像/能见度";
                document.getElementById("divImg").style.background = 'url(' + fileurl + ')';
                document.getElementById("divImg").style.backgroundSize = '100% 100%';
            }


            function GetShow() {
                debugger;
                var maxh = document.body.clientHeight;
                var maxw = document.body.clientWidth;
                document.getElementById("divImg").style.width = maxw - 260 + 'px';
                document.getElementById("divImg").style.height = maxh - 50 + 'px';
                //document.getElementById("divPT").style.width = maxw - 260 + 'px';
                //document.getElementById("divPT").style.height = maxh + 'px';
            }

            function CreatCharts() {
                debugger;
                $('#ChartContainer').html("");
                //获取隐藏域的值
                var strJSON = HiddenDataNew.value;
                debugger;
                var quality = Quality.value;
                var dtStart = DtStart.value;
                var dtEnd = DtEnd.value;
                HiddenDataNew.value = "";
                if (quality != "border") {
                    debugger;
                    ajaxHighChart("热力图", "热力图", strJSON, "ChartContainer", "时间", "Km", "值");
                }
                else {

                }
                //console.log(quality);
                //console.log(dtStart);
                //console.log(dtEnd);
                //根据值跳转画图页面
                var chartdiv = "";
                //$.each(pointIds.split(','), function (chartNo, value) {
                //chartdiv = "";
                //chartdiv += '<div style=" width:100%; height:600px;">';
                //chartdiv += '<iframe name="chartIframe" id="frame' + Math.random() + '" src="../Chart/LadarBMPThermodynamicChart.aspx?quality=' + quality + '&dtStart=' + dtStart + '&dtEnd=' + dtEnd + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                //chartdiv += '</div>';
                //console.log(chartdiv);
                //$("#ChartContainer").append(chartdiv);

                ////});
                $("#ChartContainer").css("overflow-y", "auto");
                $("#ChartContainer").height("500px");//设置图表Iframe的高度
                $("#ChartContainer").width("100%");//设置图表Iframe的宽度

            }
        </script>
        <style>
            .SpanTitle {
                font-size: 16px;
                color: #6995CA;
                vertical-align: middle;
                font-family: 'Microsoft YaHei',SimSun;
                margin-top: 8px;
                margin-left: 10px;
            }

            .fieldsetTitle {
                /*padding-left: 10px;
            padding-right: 10px;
            padding-bottom: 10px;*/
                border-color: #E6F2FE;
                background-color: white;
            }

            .divTitle {
                width: 100%;
                height: 25px;
            }

            /*表格样式*/
            .border-table {
                width: 100%;
                min-width: 198px;
                border-width: 1px;
                margin: 0;
                background: #fff;
                text-align: center;
                font-size: 14px;
            }

                .border-table th, .border-table td {
                    margin: 0;
                    padding: 2px 10px;
                    line-height: 26px;
                    height: 28px;
                    border: 1px solid #eee;
                    vertical-align: middle;
                    white-space: nowrap;
                    word-break: keep-all;
                }

                .border-table thead th {
                    color: #333;
                    font-weight: bold;
                    white-space: nowrap;
                    text-align: center;
                    background: #B1D1EC;
                }

                .border-table tbody th {
                    padding-right: 5px;
                    text-align: right;
                    color: #707070;
                    background-color: #f9f9f9;
                }
        </style>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divAirQualityLevel" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="div1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="div2" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdAirWeather" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenDataNew" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="img" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="diVisibility" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="imgName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="Quality" />
                        <telerik:AjaxUpdatedControl ControlID="DtStart" />
                        <telerik:AjaxUpdatedControl ControlID="DtEnd" />
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="imgName">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divImg" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="diVisibility">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divImg" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                <telerik:AjaxSetting AjaxControlID="start">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="start" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="stop">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="stop" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%--<telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="timer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="img" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="diVisibility" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="imgName" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0" BorderSize="0" BorderStyle="None"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None" MaxHeight="100"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" class="Table_Customer">
                    <tr>
                        <td class="title" style="width: 80px">测点:
                        </td>
                        <td class="content" style="width: 300px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="220" CbxHeight="350" MultiSelected="false" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="title" style="width: 80px">监测因子:
                        </td>
                        <td class="content" style="width: 370px;">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="360" DefaultAllSelected="true" DropDownWidth="420" ID="factorCbxRsm"></CbxRsm:FactorCbxRsm>
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
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">数据类型:
                        </td>
                        <td class="content" style="width: 400px;">
                            <asp:RadioButtonList ID="radlDataType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true" Visible="false">
                            </asp:RadioButtonList>
                            <asp:RadioButtonList ID="radlDataTypeOri" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataTypeOri_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 400px;">
                            <div runat="server" id="dtpHour" visible="false">
                                <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" />
                                结束时间;
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                            </div>
                            <div runat="server" id="dbtHour">
                                <telerik:RadDateTimePicker ID="hourBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" />
                                结束时间;
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
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                结束时间;
                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
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
                        </td>

                        <td class="title" style="width: 80px">激光雷达类型:
                        </td>
                        <td class="content" style="width: 120px;">
                            <telerik:RadDropDownList ID="ddlJiGuang" runat="server" Width="100px" AutoPostBack="true" Visible="true">
                                <Items>
                                    <telerik:DropDownListItem Text="消光系数532" Value="extin532" Selected="true" />
                                    <telerik:DropDownListItem Text="消光系数355" Value="extin355" />
                                    <telerik:DropDownListItem Text="边界层高度" Value="border" />
                                    <telerik:DropDownListItem Text="退偏振度" Value="depol" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneImage" runat="server" Width="100%"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table style="width: 99%; margin: auto;">
                    <tr>
                        <td style="width: 100%;">
                            <div class="fieldsetTitle">
                                <div class="divTitle">
                                    <div class="SpanTitle">
                                        气象参数分析图
                                    </div>
                                </div>
                                <div id="divAirQualityLevel" style="width: 99%; height: 500px; text-align: center;"></div>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;">
                            <div class="fieldsetTitle">
                                <div class="divTitle">
                                    <div class="SpanTitle">
                                        多因子综合分析图
                                    </div>
                                </div>
                                <div id="div1" style="width: 99%; height: 500px; text-align: center">
                                    <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">

                                        <telerik:RadPageView ID="pvChart" runat="server" ContentUrl="~/Pages/EnvAir/Chart/ChartFrame.aspx">
                                        </telerik:RadPageView>
                                    </telerik:RadMultiPage>
                                </div>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;">
                            <div class="fieldsetTitle">
                                <div class="divTitle">
                                    <div class="SpanTitle">
                                        激光雷达
                                    </div>
                                </div>
                                <div id="ChartContainer" style="width: 99%; height: 500px; text-align: center">
                                </div>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;">
                            <div class="fieldsetTitle">
                                <div class="divTitle">
                                    <div class="SpanTitle">
                                        城市摄影能见度
                                    </div>
                                </div>
                                <div id="div2" style="width: 99%; height: 500px; text-align: center">

                                    <table style="width: 100%; height: 98%; text-align: center;">
                                        <tr style="width: 100%; height: 100%;">
                                            <td style="width: 200px; height: 100%; vertical-align: top">
                                                <br />
                                                <br />
                                                <div style="width: 100%; max-height: 350px; overflow-x: hidden;" runat="server" id="diVisibility"></div>
                                                <br />
                                                <asp:Timer runat="server" ID="timer" Interval="3000" Enabled="false" OnTick="timer_Tick"></asp:Timer>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadNumericTextBox ID="tbSecond" runat="server" Width="20px"></telerik:RadNumericTextBox>
                                                            秒
                                                        </td>
                                                        <td>
                                                            <telerik:RadButton ID="start" runat="server" BackColor="#3A94D3" ForeColor="White" Height="20" AutoPostBack="True" OnClick="start_Click">
                                                                <ContentTemplate>
                                                                    <asp:Label runat="server" ID="Label4" ForeColor="White" Text="自动播放"></asp:Label>
                                                                </ContentTemplate>
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td>
                                                            <telerik:RadButton ID="stop" runat="server" BackColor="#3A94D3" ForeColor="White" Height="20" AutoPostBack="True" OnClick="stop_Click">
                                                                <ContentTemplate>
                                                                    <asp:Label runat="server" ID="Label4" ForeColor="White" Text="停止"></asp:Label>
                                                                </ContentTemplate>
                                                            </telerik:RadButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 2px; height: 100%; background-color: #f3f3f3"></td>
                                            <td style="width: 20px; height: 100%">
                                                <label id="imgName" runat="server"></label>
                                            </td>
                                            <td style="height: 100%">
                                                <div runat="server" id="divImg"></div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>



            </telerik:RadPane>
        </telerik:RadSplitter>
        <asp:HiddenField ID="hdAirWeather" runat="server" />
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/GranuleSpecial.ashx" />
        <asp:HiddenField ID="Quality" runat="server" />
        <asp:HiddenField ID="DtStart" runat="server" />
        <asp:HiddenField ID="DtEnd" runat="server" />
        <asp:HiddenField ID="HiddenDataNew" runat="server" Value="" />
        <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
    </form>
</body>
</html>
