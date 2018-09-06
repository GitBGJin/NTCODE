/*小时数据提示框格式*/
var formatterHourData = function () {
    //return Highcharts.dateFormat("%d日%H点", this.x) + ":" + this.y;
    var series = this.series;
    var aa = this;
    return Highcharts.dateFormat("%m/%d日 %H时", this.x) + "<br/>" + series.name + this.y +" "+ series.tooltipOptions.valueSuffix;
}
/*月数据提示框格式*/
var formatterMonthData = function () {
    var series = this.series;
    return Highcharts.dateFormat("%y%m", this.x) + "<br/>" + series.name + this.y + " " + series.tooltipOptions.valueSuffix;
}
/*日数据提示框格式*/
var formatterDayData = function () {
    var series = this.series;
    return Highcharts.dateFormat("%y/%m/%d日", this.x) + "<br/>" + series.name + this.y + " " + series.tooltipOptions.valueSuffix;
}
/*分钟数据提示框格式*/
var formatterMinuteData = function () {
    var series = this.series;
    return Highcharts.dateFormat("%d日%H时%M分", this.x) + "<br/>" + series.name + this.y + " " + series.tooltipOptions.valueSuffix;
}
/*小时数据X轴格式*/
var formatterHourData_XLabel = function () {
    return Highcharts.dateFormat("%m/%d %H时", this.value);
}
/*日时数据X轴格式*/
var formatterDayData_XLabel = function () {
    return Highcharts.dateFormat("%d日", this.value);
}
/*分钟数据X轴格式*/
var formatterMinuteData_XLabel = function () {
    return Highcharts.dateFormat("%H时%M分", this.value);
}

function ajaxHighChart(urlStr, dataValue, id) {
    //var markerEnable = true;
    $('#' + id).html("<div style='padding-left:30%;padding-right:30%;padding-top:20%;'>正在加载数据....</div>");
    //var defer = $.Deferred();
    $.ajax({
        type: "POST",
        data: dataValue,
        url: urlStr+"?randnum=" + Math.random(),
        dataType: "",
        cache: false, //指令
        async: true, //取消同步
        beforeSend: function () {
        }, //发送数据之前
        success: function (msg) {
            //defer.resolve(msg);
            var tsm = eval("(" + msg + ")");
            //if (tsm.series.length > 0 && tsm.series[0].data.length == 1) markerEnable = true;
            var chartType = (tsm.chartType == null || tsm.chartType == undefined || tsm.chartType == "") ? 'spline' : tsm.chartType;
            var xType = (tsm.xType == null || tsm.xType == undefined || tsm.xType == "") ? "datetime" : tsm.xType;
            //var yTitleText = (tsm.yTitleText == null || tsm.yTitleText == undefined || tsm.yTitleText == "") ? "浓度" : tsm.yTitleText;
            var columnStacking = (tsm.columnStacking == null || tsm.columnStacking == undefined) ? 'normal' : tsm.columnStacking;
            var xAxisData = (tsm.xAxisData == null || tsm.xAxisData == undefined || tsm.xAxisData == "")
                ? {
                    type: xType,
                    dateTimeLabelFormats: {
                        day: '%m/%d %H时',
                        hour: '%m/%d %H时',
                        month: '%m/%d',
                        week: '%m-%d',

                        second: '%d日%H点',
                        minute: '%d日%H点'
                    }
                } : tsm.xAxisData;
            $('#' + id).highcharts({
                chart: {
                    //renderTo: id,
                    animation: false,
                    zoomType: 'x',
                    resetZoomButton: {
                        position: {
                            x: 0,
                            y: -40
                        }
                    }
                    //, backgroundColor: 'rgba(119,152,191,0)'
                    , type: chartType
                },
                //colors: ['rgba(0,228,0,1)', 'rgba(255,255,0,1)', 'rgba(255,126,0,1)', 'rgba(255,0,0,1)', 'rgba(153,0,76,1)', 'rgba(126,0,35,1)', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a'
                //],
                colors: ['#c0232a', '#85951d', '#f3a43b', '#b019be', '#b6c335', '#28727b', '#1c93e7', '#61c0de', '#e87c24', '#1a9dc7', '#9bca62', '#d268dc', '#ff8463', '#32b573', '#fbce0f', '#2f57a8', '#d8ac13', '#60cac4', '#eb4714', '#be23df', 'rgba(0,228,0,1)', 'rgba(255,255,0,1)', 'rgba(255,126,0,1)', 'rgba(255,0,0,1)', 'rgba(153,0,76,1)', 'rgba(126,0,35,1)', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a'],
                title: {
                    text: tsm.titleText
                },
                legend: {
                    align: 'center',
                    verticalAlign: 'bottom'
                },
                plotOptions: {
                    series: {

                    },
                    spline: {
                        marker: {
                            radius: 4,
                            symbol: "circle",
                            //, enabled: markerEnable
                            lineColor:this.color,
                            lineWidth: 2,
                            fillColor:'white'
                        }
                    }, scatter: {
                        marker: {
                            radius: 4,
                            symbol: "circle"
                             , enabled: true

                        }
                    },
                    //column: {
                    //    stacking: columnStacking
                    //},
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}: {point.percentage:.1f} %</b>'
                            //style: {
                            //    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            //}
                        }
                    }
                },
                xAxis: xAxisData,
                yAxis: (tsm.yAxis == null || tsm.yAxis == undefined) ? {
                    title: {
                        text: "浓度"
                    }
                } : tsm.yAxis,
                //yAxis: {
                //    title: {
                //        text: yTitleText
                //    }
                //},
                tooltip: tsm.tooltip,
                series: tsm.series
            });
        },
        error: function (err) {
            //alert(err);
        }
    });

    return defer.promise();

}

function DealAjaxData(msg) {
    var tsm = eval("(" + msg + ")");
    //if (tsm.series.length > 0 && tsm.series[0].data.length == 1) markerEnable = true;
    var chartType = (tsm.chartType == null || tsm.chartType == undefined || tsm.chartType == "") ? 'spline' : tsm.chartType;
    var xType = (tsm.xType == null || tsm.xType == undefined || tsm.xType == "") ? "datetime" : tsm.xType;
    //var yTitleText = (tsm.yTitleText == null || tsm.yTitleText == undefined || tsm.yTitleText == "") ? "浓度" : tsm.yTitleText;
    var columnStacking = (tsm.columnStacking == null || tsm.columnStacking == undefined) ? 'normal' : tsm.columnStacking;
    var xAxisData = (tsm.xAxisData == null || tsm.xAxisData == undefined || tsm.xAxisData == "")
        ? {
            type: xType,
            dateTimeLabelFormats: {
                day: '%d日%H点',
                hour: '%d日%H点',
                month: '%m-%d',
                week: '%m-%d',

                second: '%d日%H点',
                minute: '%d日%H点'
            }
        } : tsm.xAxisData;
    $('#' + id).highcharts({
        chart: {
            //renderTo: id,
            animation: false,
            zoomType: 'x',
            resetZoomButton: {
                position: {
                    x: 0,
                    y: -40
                }
            }
            //, backgroundColor: 'rgba(119,152,191,0)'
            , type: chartType
        },
        //colors: ['rgba(0,228,0,1)', 'rgba(255,255,0,1)', 'rgba(255,126,0,1)', 'rgba(255,0,0,1)', 'rgba(153,0,76,1)', 'rgba(126,0,35,1)', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a'
        //],
        colors: ['#c0232a', '#85951d', '#f3a43b', '#b019be', '#b6c335', '#28727b', '#1c93e7', '#61c0de', '#e87c24', '#1a9dc7', '#9bca62', '#d268dc', '#ff8463', '#32b573', '#fbce0f', '#2f57a8', '#d8ac13', '#60cac4', '#eb4714', '#be23df', 'rgba(0,228,0,1)', 'rgba(255,255,0,1)', 'rgba(255,126,0,1)', 'rgba(255,0,0,1)', 'rgba(153,0,76,1)', 'rgba(126,0,35,1)', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a'],
        title: {
            text: tsm.titleText
        },
        legend: {
            align: 'center',
            verticalAlign: 'bottom'
        },
        plotOptions: {
            series: {

            },
            spline: {
                marker: {
                    radius: 4,
                    symbol: "circle",
                    //, enabled: markerEnable
                    lineColor:this.color,
                    lineWidth: 2,
                    fillColor:'white'
                }
            }, scatter: {
                marker: {
                    radius: 4,
                    symbol: "circle"
                     , enabled: true

                }
            },
            //column: {
            //    stacking: columnStacking
            //},
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}: {point.percentage:.1f} %</b>'
                    //style: {
                    //    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    //}
                }
            }
        },
        xAxis: xAxisData,
        yAxis: (tsm.yAxis == null || tsm.yAxis == undefined) ? {
            title: {
                text: "浓度"
            }
        } : tsm.yAxis,
        //yAxis: {
        //    title: {
        //        text: yTitleText
        //    }
        //},
        tooltip: tsm.tooltip,
        series: tsm.series
    });
}

//$.when(getData3()).done(function (data) {
//    $(".loadingicon").hide();
//    alert(data);
//});
