﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpecialWeather.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Special.SpecialWeather" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="../../../Resources/CSS/pagination.css" />
    <title></title>
    <script type="text/javascript" src="../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist/echarts.min.js"></script>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">

            function ShowECharts() {
                /*var obj = str.parseJSON(); //由JSON字符串转换为JSON对象
                  或者 var obj = JSON.parse(str); //由JSON字符串转换为JSON对象
                */
                
                var strAirWeather = document.getElementById("<%=hdAirWeather.ClientID%>").value;
                var strAirCode = document.getElementById("<%=hdAirCode.ClientID%>").value;
                var strAirName = document.getElementById("<%=hdAirName.ClientID%>").value;
                
                //hdAirWeather.value = "";
                //hdAirCode.value = "";
                //hdAirName.value = "";
                var minS = 9999;
                var maxS = 0;
                //因子Name
                var AirName = strAirName.split(",");

                var AirWeather = JSON.parse(strAirWeather);
                var Tstamp = [];
                var a01001 = [];
                var a01002 = [];
                var a01006 = [];
                var a01007 = [];
                var a01008 = [];
                var Wind = [];
                var a34004 = [];
                var a34002 = [];
                var a21026 = [];
                var a21004 = [];
                var a21005 = [];
                var a05024 = [];
                $.each(AirWeather, function (key, obj) {
                    Tstamp.push(obj.Tstamp);
                    a01001.push(obj.a01001);
                    a01002.push(obj.a01002);
                    a01006.push(obj.a01006);
                    a01007.push(obj.a01007);
                    a01008.push(-obj.a01008 + 180);
                    Wind.push(obj.Wind);
                    if (strAirCode.indexOf("a34004") >= 0)
                        a34004.push(obj.a34004);
                    if (strAirCode.indexOf("a34002") >= 0)
                        a34002.push(obj.a34002);
                    if (strAirCode.indexOf("a21026") >= 0)
                        a21026.push(obj.a21026);
                    if (strAirCode.indexOf("a21004") >= 0)
                        a21004.push(obj.a21004);
                    if (strAirCode.indexOf("a21005") >= 0)
                        a21005.push(obj.a21005);
                    if (strAirCode.indexOf("a05024") >= 0)
                        a05024.push(obj.a05024);
                });
                var List = [];
                for (var i in a01007) {
                    List.push({ value: a01007[i], symbolRotate: a01008[i] })
                }
                ;
                for (var i in a01006) {
                    if (a01006[i] < minS)
                        minS = a01006[i];
                    if (a01006[i] > maxS)
                        maxS = a01006[i];
                }
                ;
                minS =parseInt(minS)-5;
                maxS =parseInt(maxS)+5;
                var ListyAxis = [];
                var Listseries = [];
                if ((strAirCode.indexOf("a34004") != -1 || strAirCode.indexOf("a34002") != -1 || strAirCode.indexOf("a21026") != -1 || strAirCode.indexOf("a21004") != -1 || strAirCode.indexOf("a05024") != -1)) {
                    ListyAxis.push(
                        {
                            //yAxisIndex: 4,
                            type: 'value',
                            name: 'μg/m³',
                            offset: 110,
                            position: 'left',
                            axisLine: {
                                lineStyle: {
                                    color: '#1E90FF'
                                }
                            },
                            axisLabel: {
                                formatter: '{value}'
                            }
                        }
                    );
                }
                if (strAirCode.indexOf("a21005") !=-1) {
                    ListyAxis.push(
                        {
                            //yAxisIndex: 5,
                            type: 'value',
                            name: 'mg/m³',
                            offset: 130,
                            position: 'right',
                            axisLine: {
                                lineStyle: {
                                    color: '#1E90FF'
                                }
                            },
                            axisLabel: {
                                formatter: '{value}'
                            }
                        }
                    );
                    if (ListyAxis.length==2) {
                        Listseries.push(
                        {
                            name: '一氧化碳',
                            type: 'bar',
                            barMaxWidth: '15',
                            yAxisIndex: 1,
                            data: a21005
                        }
                    )
                    }
                    else {
                        Listseries.push(
                        {
                            name: '一氧化碳',
                            type: 'bar',
                            barMaxWidth: '15',
                            yAxisIndex: 0,
                            data: a21005
                        }
                    )
                    }
                }
                if (strAirCode.indexOf("a34004") != -1) {
                    Listseries.push(
                        {
                               name: 'PM2.5',
                               type: 'bar',
                               barMaxWidth: '15',
                               yAxisIndex: 0,
                               data: a34004
                        }

                    );
                }
                if (strAirCode.indexOf("a34002") != -1) {
                    Listseries.push(
                           {
                               name: 'PM10',
                               type: 'bar',
                               barMaxWidth: '15',
                               yAxisIndex: 0,
                               data: a34002
                           }
                    );
                }
                if (strAirCode.indexOf("a21026") != -1) {
                    Listseries.push(
                           {
                               name: '二氧化硫',
                               type: 'bar',
                               barMaxWidth: '15',
                               yAxisIndex: 0,
                               data: a21026
                           }
                           
                    );
                }
                if (strAirCode.indexOf("a21004") != -1) {
                    Listseries.push(
                           {
                               name: '二氧化氮',
                               type: 'bar',
                               barMaxWidth: '15',
                               yAxisIndex: 0,
                               data: a21004
                           }
                    );
                }
                if (strAirCode.indexOf("a05024") != -1) {
                    Listseries.push(
                           {
                               name: '臭氧',
                               type: 'bar',
                               barMaxWidth: '15',
                               yAxisIndex: 0,
                               data: a05024
                           }
                    );
                }
                
                if (strAirCode.indexOf("a01001") != -1) {
                    ListyAxis.push(
                          {
                              //yAxisIndex: 6,
                              type: 'value',
                              name: '温度',
                              min: 0,
                              max: 50,
                              position: 'right',
                              axisLine: {
                                  lineStyle: {
                                      color: '#28C728'
                                  }
                              },
                              axisLabel: {
                                  formatter: '{value} °C'
                              }
                          }
                    );
                    if (ListyAxis.length == 1) {
                        Listseries.push({
                            name: '温度',
                            type: 'line',
                            symbolSize: 4,
                            smooth: true,  //这句就是让曲线变平滑的  
                            itemStyle: {
                                normal: {
                                    color: '#28C728',
                                    lineStyle: {
                                        width: 1.5
                                    }

                                }
                            },
                            yAxisIndex: 0,
                            data: a01001
                        }
                    )
                    }
                    else if (ListyAxis.length == 2) {
                        Listseries.push({
                            name: '温度',
                            type: 'line',
                            symbolSize: 4,
                            smooth: true,  //这句就是让曲线变平滑的  
                            itemStyle: {
                                normal: {
                                    color: '#28C728',
                                    lineStyle: {
                                        width: 1.5
                                    }

                                }
                            },
                            yAxisIndex: 1,
                            data: a01001
                        }
                    )
                    }
                    else if (ListyAxis.length == 3) {
                        Listseries.push({
                            name: '温度',
                            type: 'line',
                            symbolSize: 4,
                            smooth: true,  //这句就是让曲线变平滑的  
                            itemStyle: {
                                normal: {
                                    color: '#28C728',
                                    lineStyle: {
                                        width: 1.5
                                    }

                                }
                            },
                            yAxisIndex: 2,
                            data: a01001
                        }
                    )
                    }
                }
                if (strAirCode.indexOf("a01002") != -1) {
                    ListyAxis.push(
                          {
                              //yAxisIndex: 3,
                              type: 'value',
                              name: '湿度',
                              min: 0,
                              max: 100,
                              position: 'left',
                              axisLine: {
                                  lineStyle: {
                                      color: '#1E90FF'
                                  }
                              },
                              axisLabel: {
                                  formatter: '{value} %'
                              }
                          }
                    );
                    if (ListyAxis.length == 1) {
                        Listseries.push({
                            name: '湿度',
                            type: 'line',
                            smooth: true,  //这句就是让曲线变平滑的  
                            itemStyle: {
                                normal: {
                                    color: '#1E90FF',
                                    lineStyle: {
                                        width: 1.5
                                    }
                                }
                            },
                            yAxisIndex: 0,
                            data: a01002
                        }
                    )
                    }
                    else if (ListyAxis.length == 2) {
                        Listseries.push({
                            name: '湿度',
                            type: 'line',
                            smooth: true,  //这句就是让曲线变平滑的  
                            itemStyle: {
                                normal: {
                                    color: '#1E90FF',
                                    lineStyle: {
                                        width: 1.5
                                    }
                                }
                            },
                            yAxisIndex: 1,
                            data: a01002
                        }
                    )
                    }
                    else if (ListyAxis.length == 3) {
                        Listseries.push({
                            name: '湿度',
                            type: 'line',
                            smooth: true,  //这句就是让曲线变平滑的  
                            itemStyle: {
                                normal: {
                                    color: '#1E90FF',
                                    lineStyle: {
                                        width: 1.5
                                    }
                                }
                            },
                            yAxisIndex: 2,
                            data: a01002
                        }
                    )
                    }
                    else if (ListyAxis.length == 4) {
                        Listseries.push({
                            name: '湿度',
                            type: 'line',
                            smooth: true,  //这句就是让曲线变平滑的  
                            itemStyle: {
                                normal: {
                                    color: '#1E90FF',
                                    lineStyle: {
                                        width: 1.5
                                    }
                                }
                            },
                            yAxisIndex: 3,
                            data: a01002
                        }
                    )
                    }
                }
                if (strAirCode.indexOf("a01006") != -1) {
                    ListyAxis.push(
                          {
                              //yAxisIndex: 2,
                              type: 'value',
                              name: '气压',
                              min: minS,
                              max: maxS,
                              position: 'right',
                              offset: 60,
                              axisLine: {
                                  lineStyle: {
                                      color: 'black'
                                  }
                              },
                              axisLabel: {
                                  formatter: '{value} hPa'
                              }
                          }
                    );
                    if (ListyAxis.length == 1) {
                        Listseries.push({
                            name: '气压',
                            type: 'line',
                            symbol: 'none',
                            smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                            itemStyle: {
                                normal: {
                                    color: 'black',
                                    lineStyle: {
                                        width: 1.5,
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
                            yAxisIndex: 0,
                            data: a01006
                        }
                    )
                    }
                    else if (ListyAxis.length == 2) {
                        Listseries.push({
                            name: '气压',
                            type: 'line',
                            symbol: 'none',
                            smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                            itemStyle: {
                                normal: {
                                    color: 'black',
                                    lineStyle: {
                                        width: 1.5,
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
                            yAxisIndex: 1,
                            data: a01006
                        }
                    )
                    }
                    else if (ListyAxis.length == 3) {
                        Listseries.push({
                            name: '气压',
                            type: 'line',
                            symbol: 'none',
                            smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                            itemStyle: {
                                normal: {
                                    color: 'black',
                                    lineStyle: {
                                        width: 1.5,
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
                            yAxisIndex: 2,
                            data: a01006
                        }
                    )
                    }
                    else if (ListyAxis.length == 4) {
                        Listseries.push({
                            name: '气压',
                            type: 'line',
                            symbol: 'none',
                            smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                            itemStyle: {
                                normal: {
                                    color: 'black',
                                    lineStyle: {
                                        width: 1.5,
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
                            yAxisIndex: 3,
                            data: a01006
                        }
                    )
                    }
                    else if (ListyAxis.length == 5) {
                        Listseries.push({
                            name: '气压',
                            type: 'line',
                            symbol: 'none',
                            smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                            itemStyle: {
                                normal: {
                                    color: 'black',
                                    lineStyle: {
                                        width: 1.5,
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
                            yAxisIndex: 4,
                            data: a01006
                        }
                    )
                    }
                }
                if (strAirCode.indexOf("a01007") != -1) {
                    ListyAxis.push(
                          {
                              //yAxisIndex: 1,
                              type: 'value',
                              name: '风速',
                              position: 'left',
                              offset: 60,
                              axisLine: {
                                  lineStyle: {
                                      color: 'blue'
                                  }
                              },
                              axisLabel: {
                                  formatter: '{value} m/s'
                              }
                          }
                    );
                    if (ListyAxis.length == 1) {
                        Listseries.push({
                            name: '风速风向',
                            type: 'line',
                            symbolSize: [10, 22],
                            symbol: 'arrow',
                            //symbolSize: [10, 20],
                            //symbol: 'image://http://218.91.209.251:1117/CSYC/2001-01-01/N_2001-01-01%2004-00-00_4.6715.svg',
                            showAllSymbol:true,
                            smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                            itemStyle: {
                                normal: {
                                    color: 'blue',
                                    lineStyle: {
                                        width: 1.5,
                                        type: 'dotted'  //'dotted'虚线 'solid'实线
                                    }
                                },
                                emphasis: {
                                    color: 'blue'
                                }
                            },
                            smooth: true,  //这句就是让曲线变平滑的  
                            yAxisIndex: 0,
                            data: List
                        }
                    )
                    }
                    else if (ListyAxis.length == 2) {
                        Listseries.push({
                            name: '风速风向',
                            type: 'line',
                            symbolSize: [10, 22],
                            symbol: 'arrow',
                            //symbolSize: [10, 20],
                            //symbol: 'image://http://218.91.209.251:1117/CSYC/2001-01-01/N_2001-01-01%2004-00-00_4.6715.svg',
                            showAllSymbol:true,
                            smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                            itemStyle: {
                                normal: {
                                    color: 'blue',
                                    lineStyle: {
                                        width: 1.5,
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
                    )
                    }
                    else if (ListyAxis.length == 3) {
                        Listseries.push({
                            name: '风速风向',
                            type: 'line',
                            symbolSize: [10, 22],
                            symbol: 'arrow',
                            //symbolSize: [10, 20],
                            //symbol: 'image://http://218.91.209.251:1117/CSYC/2001-01-01/N_2001-01-01%2004-00-00_4.6715.svg',
                            showAllSymbol: true,
                            smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                            itemStyle: {
                                normal: {
                                    color: 'blue',
                                    lineStyle: {
                                        width: 1.5,
                                        type: 'dotted'  //'dotted'虚线 'solid'实线
                                    }
                                },
                                emphasis: {
                                    color: 'blue'
                                }
                            },
                            smooth: true,  //这句就是让曲线变平滑的  
                            yAxisIndex: 2,
                            data: List
                        }
                    )
                    }
                    else if (ListyAxis.length == 4) {
                        Listseries.push({
                            name: '风速风向',
                            type: 'line',
                            symbolSize: [10, 22],
                            symbol: 'arrow',
                            //symbolSize: [10, 20],
                            //symbol: 'image://http://218.91.209.251:1117/CSYC/2001-01-01/N_2001-01-01%2004-00-00_4.6715.svg',
                            showAllSymbol: true,
                            smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                            itemStyle: {
                                normal: {
                                    color: 'blue',
                                    lineStyle: {
                                        width: 1.5,
                                        type: 'dotted'  //'dotted'虚线 'solid'实线
                                    }
                                },
                                emphasis: {
                                    color: 'blue'
                                }
                            },
                            smooth: true,  //这句就是让曲线变平滑的  
                            yAxisIndex: 3,
                            data: List
                        }
                    )
                    }
                    else if (ListyAxis.length == 5) {
                        Listseries.push({
                            name: '风速风向',
                            type: 'line',
                            symbolSize: [10, 22],
                            symbol: 'arrow',
                            //symbolSize: [10, 20],
                            //symbol: 'image://http://218.91.209.251:1117/CSYC/2001-01-01/N_2001-01-01%2004-00-00_4.6715.svg',
                            showAllSymbol: true,
                            smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                            itemStyle: {
                                normal: {
                                    color: 'blue',
                                    lineStyle: {
                                        width: 1.5,
                                        type: 'dotted'  //'dotted'虚线 'solid'实线
                                    }
                                },
                                emphasis: {
                                    color: 'blue'
                                }
                            },
                            smooth: true,  //这句就是让曲线变平滑的  
                            yAxisIndex: 4,
                            data: List
                        }
                    )
                    }
                    else if (ListyAxis.length == 6) {
                        Listseries.push({
                            name: '风速风向',
                            type: 'line',
                            symbolSize: [10, 22],
                            symbol: 'arrow',
                            //symbolSize: [10,20],
                            //symbol: 'image://http://218.91.209.251:1117/CSYC/2001-01-01/N_2001-01-01%2004-00-00_4.6715.svg',
                            showAllSymbol: true,
                            smooth: false,   //关键点，为true是不支持虚线的，实线就用true
                            itemStyle: {
                                normal: {
                                    color: 'blue',
                                    lineStyle: {
                                        width: 1.5,
                                        type: 'dotted'  //'dotted'虚线 'solid'实线
                                    }
                                },
                                emphasis: {
                                    color: 'blue'
                                }
                            },
                            smooth: true,  //这句就是让曲线变平滑的  
                            yAxisIndex: 5,
                            data: List
                        }
                    )
                    }
                }
                
                console.log(ListyAxis);
                console.log(Listseries);
                var myChart = echarts.init(document.getElementById('divAirQualityLevel'));
                option = {
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'cross'
                        },
                        formatter: function (params, ticket, callback) {
                            var ist = "false";
                            var keyNum = 3;
                            var strFor="";
                            for (var m = 0; m < params.length; m++) {
                                
                                if (params[m].seriesName == "温度") {
                                    strFor += params[m].seriesName + "：" + params[m].value + "<br />";
                                }
                                else if (params[m].seriesName == "湿度") {
                                    strFor += params[m].seriesName + "：" + params[m].value + "<br />";
                                }
                                else if (params[m].seriesName == "气压") {
                                    strFor += params[m].seriesName + "：" + params[m].value + "<br />";
                                }
                                else if (params[m].seriesName == "PM2.5") {
                                    strFor += params[m].seriesName + "：" + params[m].value + "<br />";
                                }
                                else if (params[m].seriesName == "PM10") {
                                    strFor += params[m].seriesName + "：" + params[m].value + "<br />";
                                }
                                else if (params[m].seriesName == "二氧化硫") {
                                    strFor += params[m].seriesName + "：" + params[m].value + "<br />";
                                }
                                else if (params[m].seriesName == "二氧化氮") {
                                    strFor += params[m].seriesName + "：" + params[m].value + "<br />";
                                }
                                else if (params[m].seriesName == "一氧化碳") {
                                    strFor += params[m].seriesName + "：" + params[m].value + "<br />";
                                }
                                else if (params[m].seriesName == "臭氧") {
                                    strFor += params[m].seriesName + "：" + params[m].value + "<br />";
                                }
                                else if (params[m].seriesName == "风速风向") {
                                    ist = "true";
                                    keyNum = m;
                                    strFor += "风速" + "：" + params[m].value + "<br />" + "风向：" + Wind[params[m].dataIndex] + "<br />";
                                }
                            }
                            return strFor;
                        },
                        //formatter: '{b}</br>{a}:{c} %</br>{a1}:{c1} °C</br>{a2}:{c2} hPa</br>风速:{c3} m/s</br>风向:{c4}度',//悬浮框显示
                        textStyle: {
                            align: 'left'
                        }
                    },
                    grid: {
                        right: '15%',
                        left: '15%',
                        layoutCenter: ['50%', '60%'],
                    },
                    toolbox: {
                        feature: {
                            saveAsImage: { show: true }
                        }
                    },
                    
                    legend: {

                        data: AirName
                    },
                    xAxis: [
                        {
                            type: 'category',
                            axisTick: {
                                alignWithLabel: true
                            },
                            data: Tstamp
                        }
                    ],
                    yAxis: ListyAxis,
                    series: Listseries
                };
                myChart.setOption(option);
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
        </script>

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
                        <telerik:AjaxUpdatedControl ControlID="hdAirWeather"  />
                        <telerik:AjaxUpdatedControl ControlID="hdAirCode"  />
                        <telerik:AjaxUpdatedControl ControlID="hdAirName"  />
                        <telerik:AjaxUpdatedControl ControlID="ddlDataSource"  />
                        <telerik:AjaxUpdatedControl ControlID="radlDataTypeOri"  />
                        <telerik:AjaxUpdatedControl ControlID="radlDataType"  />
                        <telerik:AjaxUpdatedControl ControlID="dtpHour"  />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour"  />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay"  />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth"  />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason"  />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear"  />
                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" />
                        <telerik:AjaxUpdatedControl ControlID="divAirQualityLevel" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                <telerik:AjaxSetting AjaxControlID="pointForWind">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointForWind"/>       
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
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0" BorderSize="0" BorderStyle="None"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="100px" Width="1200px" Scrolling="None" MaxHeight="100"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" class="Table_Customer">
                    <tr>
                        <td class="title" style="width: 80px">因子站点:
                        </td>
                        <td class="content" style="width: 400px;">
                            <table>
                                <tr>
                                    <td>
                                        <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="160" CbxHeight="350"  DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                                    </td>
                                    <td class="title" style="width: 80px">气象站点:
                                    </td>
                                    <td>
                                        <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="160" CbxHeight="350" MultiSelected="false" DropDownWidth="520" ID="pointForWind"></CbxRsm:PointCbxRsm>
                                    </td>
                                </tr>
                            </table>
                            
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
                        <td class="title" style="width: 80px">常规因子:
                        </td>
                        <td class="content" align="left" style="width: 220px;">
                            <telerik:RadComboBox ID="factorCom" runat="server" Width="200" SkinID="Default" Skin="Default" CheckBoxes="true" Localization-CheckAllString="全选" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="PM2.5" Value="a34004" />
                                    <telerik:RadComboBoxItem runat="server" Text="PM10" Value="a34002" />
                                    <telerik:RadComboBoxItem runat="server" Text="二氧化硫" Value="a21026" />
                                    <telerik:RadComboBoxItem runat="server" Text="二氧化氮" Value="a21004" />
                                    <telerik:RadComboBoxItem runat="server" Text="一氧化碳" Value="a21005" />
                                    <telerik:RadComboBoxItem runat="server" Text="臭氧" Value="a05024" />
                                    <telerik:RadComboBoxItem runat="server" Text="温度" Value="a01001" />
                                    <telerik:RadComboBoxItem runat="server" Text="湿度" Value="a01002" />
                                    <telerik:RadComboBoxItem runat="server" Text="风速" Value="a01007" />
                                    <telerik:RadComboBoxItem runat="server" Text="风向" Value="a01008" />
                                    <telerik:RadComboBoxItem runat="server" Text="气压" Value="a01006" />
                                </Items>
                            </telerik:RadComboBox>
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
                        <td class="content" style="width: 400px;" colspan="3">
                            <div runat="server" id="dtpHour" visible="false">
                                <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Calendar-FastNavigationStep="12"  />
                                结束时间:
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12"  />
                            </div>
                            <div runat="server" id="dbtHour">
                                <telerik:RadDateTimePicker ID="hourBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Calendar-FastNavigationStep="12"  />
                                结束时间:
                            <telerik:RadDateTimePicker ID="hourEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12"  />
                            </div>
                            <div runat="server" id="dbtDay">
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
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table style="width: 99%; margin: auto;">
                    <tr>
                        <td style="width: 100%;">
                            <div runat="server" id="divAirQualityLevel" style="height: 480px; width: 1100px;margin:auto; text-align: center;"></div>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <asp:HiddenField ID="hdAirWeather" runat="server" />
        <asp:HiddenField ID="hdAirCode" runat="server" />
        <asp:HiddenField ID="hdAirName" runat="server" />
    </form>
</body>
</html>
