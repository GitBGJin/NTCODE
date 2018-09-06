<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GranuleChart.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Chart.GranuleChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="../../../Resources/CSS/pagination.css" />
    <title></title>
    
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
        <script type="text/javascript" src="../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist/echarts.js"></script>
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
                                    axisLabel: {
                                        formatter: ' hPa'
                                    }
                                },
                                {
                                    type: 'value',
                                    name: '温度',
                                    axisLabel: {
                                        formatter: '°C'
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
        </script>
        <script type="text/javascript">
        </script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <table style="width: 99%; margin: auto;">
                    <tr>
                        <td colspan="2" style="width: 100%;">
                            <div id="divAirQualityLevel" style="width: 99%; height: 520px; text-align: center"></div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdAirWeather" runat="server" />
                <asp:HiddenField ID="hdregionUid" runat="server" />
                <asp:HiddenField ID="hdDateBegin" runat="server" />
                <asp:HiddenField ID="hdDateEnd" runat="server" />
    </form>
</body>
</html>
