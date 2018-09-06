
/*分屏(按因子)*/
function groupChart(columns, groupfactors, specialcharts, url, iframeHeight) {
    $("#ChartContainer").html("");
    var chartdiv = "";
    var num = 0;
    var charts = "";
    //var columns = "ZAD_DATA1,ZAD_DATA2,ZAD_DATA3,ZAD_DATA4,ZAD_DATA5";//因子
    //var groupfactors = "<%=GetGroupFactors() %>";//分组因子
    //var specialcharts = "<%=GetSpecialChart() %>";//获取特殊因子
    var orignalcolumns = columns;

    if (groupfactors == "") {//没有配置分组信息
        charts = columns.split(",");
    } else {//已经配置了分组信息
        columns = columns.split(",");
        groupfactors = groupfactors.split(";");
        var groups = "";
        $.each(groupfactors, function (groupfactorNo, groupfactor) {//从分组的因子中去掉不需要搜索的因子
            var factors = groupfactor.split(",");
            if (groups != "") groups += ";"
            $.each(factors, function (factorNo, factor) {
                var i = 0;
                for (; i < columns.length; i++) {
                    if (factor.toLowerCase() == columns[i].toLowerCase()) break;
                }
                if (i < columns.length)
                    groups += factor + ",";
            });
        });
        groups = groups.replace(/,{2,}/gim, ","); //去除多余的逗号
        groups = groups.replace(/,;|;,/gim, ";");
        groups = groups.replace(/;{2,}/gim, ";"); //去掉多余的分号
        groups = groups.replace(/;$|,$/gim, ""); //去掉尾的分号或分号

        if (groups != "") {//从columns中去已经存在于groups中的因子
            groupstemp = groups.replace(";", ",").split(",");
            var columnstemp = "";
            $.each(columns, function (columnNo, column) {
                var i = 0;
                for (; i < groupstemp.length; i++) {
                    if (column.toLowerCase() == groupstemp[i].toLowerCase()) break;
                };
                if (i >= groupstemp.length) {
                    if (columnstemp != "") columnstemp += ",";
                    columnstemp += column;
                }
            });
            columns = columnstemp; //单图单因子信息,分隔符为逗
            groupfactors = groups; //单图多因子信息组,分组符为分号
            columns = columns.replace(/,{2,}/gim, ","); //去除多余的逗号
            columns = columns.replace(/,$/gim, ""); //去除尾部的逗号
            if (columns != "") charts = charts.concat(columns.split(","))
            charts = charts.concat(groups.split(";"));
        } else {
            charts = orignalcolumns.split(",");
        }
    }

    var spcharts = ""; //列殊的因子
    var cmmcharts = ""; //普通因子
    if (specialcharts != "") {
        $.each(charts, function (chartNo, chart) {
            if (HasSpeicalChart(specialcharts.split(','), chart.split(','))) {
                if (spcharts != "") spcharts += ";";
                spcharts += chart;
            } else {
                if (cmmcharts != "") cmmcharts += ";";
                cmmcharts += chart;
            }
        });
    } else {
        cmmcharts = charts.join(';');
    }

    if (spcharts != null && spcharts.length > 0)
        $.each(spcharts.split(';'), function (chartNo, value) {
            chartdiv += '<div><iframe name="chartIframe" id="' + value + '" src="' + url + '?mappingColumns=' + value + '&defalutColumn=' + value.split(',')[0] + '&merge=false" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="300px"></iframe></div>';
            chartdiv += "<br />";
            num++;
        });

    var height = cmmcharts.split(';').length == 1 ? iframeHeight : iframeHeight / 2;
    $.each(cmmcharts.split(';'), function (chartNo, value) {
        chartdiv += '<div style=" width:100%; height:' + height + 'px;margin-top:10px;">';
        chartdiv += '<iframe name="chartIframe" id="' + value + '" src="' + url + '?FactorCode=' + value + '" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="100%" scrolling="no"></iframe>';
    });
    $("#ChartContainer").css("overflow-y", "auto");
    $("#ChartContainer").height(iframeHeight + 50 + "px");//设置图表Iframe的高度
    $("#ChartContainer").width("100%");//设置图表Iframe的宽度
    $("#ChartContainer").html(chartdiv);
}

/*分屏(按因子)*/
function groupChartByPointid(pointids, factors, names, url, iframeHeight) {

    $("#ChartContainer").html("");
    var chartdiv = "";
    var height = iframeHeight;
    //if (names.indexOf('|') != -1) {
    var Names = names.split('|')
    //}
    $.each(pointids, function (chartNo, value) {
        debugger
        $.each(factors.split('|'), function (chartNoF, valueF) {
            if (Names.length > 0) {
                var name = Names[chartNoF];
                chartdiv = "";
                chartdiv += '<div style=" width:100%; height:' + height + 'px;">';
                chartdiv += '<iframe name="chartIframe" id="frame' + value + chartNoF + '" src="' + url + '?PointID=' + value + '&fac=' + valueF + '&name=' + name + ' " frameborder="0" marginheight="0" marginwidth="0" width="100%" height="100%" scrolling="no"></iframe>';
                chartdiv += '</div>';
                $("#ChartContainer").append(chartdiv);
            }
            else {
                chartdiv = "";
                chartdiv += '<div style=" width:100%; height:' + height + 'px;">';
                chartdiv += '<iframe name="chartIframe" id="frame' + value + chartNoF + '" src="' + url + '?PointID=' + value + '&fac=' + valueF + '" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="100%" scrolling="no"></iframe>';
                chartdiv += '</div>';
                $("#ChartContainer").append(chartdiv);
            }
        });
    });
    $("#ChartContainer").css("overflow-y", "auto");
    $("#ChartContainer").height(iframeHeight + 50 + "px");//设置图表Iframe的高度
    $("#ChartContainer").width("100%");//设置图表Iframe的宽度
    //$("#ChartContainer").html(chartdiv);


    //$("#ChartContainer").html("");
    //var chartdiv = "";
    //var height = pointids.split(';').length == 1 ?iframeHeight:iframeHeight / 2;
    //if (height == 0) height = 300;
    //$.each(pointids.split(';'), function (chartNo, value) {
    //    chartdiv += '<div style=" width:100%; height:' + height + 'px;">';
    //    chartdiv += '<iframe name="chartIframe" id="pointid' + value + '" src="' + url + '?PointID=' + value + '&DataType='+(chartNo==0?1:0)+'" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="100%" scrolling="no"></iframe>';
    //    chartdiv += '</div>';
    //});
    //$("#ChartContainer").css("overflow-y", "auto");
    //$("#ChartContainer").height((iframeHeight + 50) + "px");//设置图表Iframe的高度
    //$("#ChartContainer").width("100%");//设置图表Iframe的宽度
    //$("#ChartContainer").html(chartdiv);
}

/*分屏(按因子)*/
function groupChartByPointidNew(pointids, dtBegin, dtEnd, Type, url, urlScatter, iframeHeight) {
    $("#ChartContainer").html("");
    var chartdiv = "";
    //var height = pointids.split(';').length == 1 ? iframeHeight : iframeHeight / 2;
    var height = iframeHeight;
    //setTimeout(function () {
    chartdiv = "";
    chartdiv += '<div style=" width:50%; height:' + height + 'px; float:left">';
    chartdiv += '<iframe name="chartIframe" id="frame' + Math.random() + '" src="' + url + '?PointID=' + pointids + '" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="100%" scrolling="no"></iframe>';
    chartdiv += '</div>';
    chartdiv += '<div style=" width:50%; height:' + height + 'px; float:left">';
    chartdiv += '<iframe name="ChartFrameScatter" id="frame' + Math.random() + '" src="' + urlScatter + '?PointID=' + pointids + '&dtBegin=' + dtBegin + '&dtEnd=' + dtEnd + '&Type=' + Type + '" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="100%" scrolling="no"></iframe>';
    chartdiv += '</div>';
    $("#ChartContainer").append(chartdiv);
    $("#ChartContainer").css("overflow-y", "auto");
    $("#ChartContainer").height(iframeHeight + 50 + "px");//设置图表Iframe的高度
    $("#ChartContainer").width("100%");//设置图表Iframe的宽度
}
function CreateIFrame() {

}


//function groupChartByPointid(pointids, url, iframeHeight) {
//    $("#ChartContainer").html("");
//    var chartdiv = "";
//    var num = 0;
//    var charts = "";
//    var height = 0;
//    var width = "30%";
//    var isSolid = "";

//    if (pointids != null && pointids.length > 0)
//        height = 0;
//    var total = pointids.split(';').length;
//    if (total % 3 == 0) {
//        height = iframeHeight / (total / 3);
//        width = "30%";
//    }
//    else {
//        height = iframeHeight / (parseInt(total / 3) + 1);
//    }
//    if (total <= 3) {
//        if (total % 3 == 1) { width = "100%"; isSolid = "border:1px solid #3A94D3;"; }
//        if (total % 3 == 2) width = "40%";
//    }

//    $.each(pointids.split(';'), function (chartNo, value) {
//        if ((chartNo + 1) % 3 == 1) {
//            num++;
//            chartdiv += "<div " + (chartNo >= 3 ? "style='margin-top:10px;'" : "") + ">";
//        }
//        chartdiv += '<div style=" width:' + width + '; height:' + height + 'px; float:left; margin-left:3%;"+isSolid>';
//        chartdiv += '<iframe name="chartIframe" id="pointid' + value + '" src="' + url + '?PointID=' + value + '" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="100%" scrolling="no"></iframe>';
//        chartdiv += '</div>';
//        if ((chartNo + 1) % 3 == 0) {
//            chartdiv += "<div style='clear:both'></div></div>";
//        }
//    });
//    $("#ChartContainer").height((num * height + 50) + "px");//设置图表Iframe的高度
//    $("#ChartContainer").html(chartdiv);
//}

/*合并*/
function togetherChart(url, iframeHeight) {
    $("#ChartContainer").html("");
    var chartdiv = "";
    chartdiv += '<div style=" width:100%; height:' + iframeHeight + 'px">';
    chartdiv = '<iframe name="chartIframe" id="chartall"  src="' + url + '" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="100%"></iframe>';
    chartdiv += '</div>';
    $("#ChartContainer").height(iframeHeight);//设置图表Iframe的高度
    $("#ChartContainer").width("100%");//设置图表Iframe的高度
    $("#ChartContainer").html(chartdiv);
}

/*是否有特殊因子*/
function HasSpeicalChart(spcharts, searchcharts) {
    for (var i = 0; i < searchcharts.length; i++) {
        for (var j = 0; j < spcharts.length; j++) {
            if (searchcharts[i] == spcharts[j]) return true;
        }
    }
    return false;
}

