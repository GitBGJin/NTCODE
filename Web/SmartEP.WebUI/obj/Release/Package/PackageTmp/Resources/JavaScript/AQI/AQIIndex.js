
//取得最近24小时AQI变化图的X轴坐标
function getHC24HoursXAxis(obj) {
    return new Date(obj.value).Format("HH");
}

//根据AQI值取得相应颜色
function getPointColor(aqi) {
    var color = "#25A0DA";
    aqi = parseInt(aqi, 10);
    // 0～50
    if (aqi >= 0 && aqi <= 50) {
        color = "#00e400";
    }
        //51～100
    else if (aqi >= 51 && aqi <= 100) {
        color = "yellow";
    }
        //101～150
    else if (aqi >= 101 && aqi <= 150) {
        color = "#ffa500";
    }
        //151～200
    else if (aqi >= 151 && aqi <= 200) {
        color = "red";
    }
        //201～300
    else if (aqi >= 201 && aqi <= 300) {
        color = "#800080";
    }
        //＞300
    else if (aqi > 300) {
        color = "#7e0023";
    }
    return color;
}

//空气质量指数级别
function getGradeFromAQI(aqi) {
    var grade = "--";
    if (aqi == null || aqi == "" || aqi == "-99999" || aqi == "NA")
        return grade
    aqi = parseInt(aqi, 10);
    // 0～50
    if (aqi >= 0 && aqi <= 50) {
        grade = "一级";
    }
        //51～100
    else if (aqi >= 51 && aqi <= 100) {
        grade = "二级";
    }
        //101～150
    else if (aqi >= 101 && aqi <= 150) {
        grade = "三级";
    }
        //151～200
    else if (aqi >= 151 && aqi <= 200) {
        grade = "四级";
    }
        //201～300
    else if (aqi >= 201 && aqi <= 300) {
        grade = "五级";
    }
        //＞300
    else if (aqi > 300) {
        grade = "六级";
    }
    return grade;
}

//最近24小时AQI变化图处理
function getAQIForPre24HoursCallback(response) {
    response = eval(response);//jQuery.parseJSON(response);
    var seriesData = new Array();
    var dateStart = new Date(new Date().valueOf() - 23 * 60 * 60 * 1000);
    var dateEnd = new Date();
    var iList = 0;
    var minValue = 0;
    var maxValue = 0;
    var pointData = new Array();
    var dataExit = false;
    if (response != "undefined" && response != null) {
        if (response.length > 0) {
            dateStart = new Date(response[0].DateTime);
            dateEnd = new Date(response[response.length - 1].DateTime);
        }
        for (var iRow = 0; iRow < response.length; iRow++) {
            var point = {};
            var tempModel = response[iRow];
            point.x = new Date(tempModel.DateTime);
            if (tempModel.AQIValue == null || tempModel.AQIValue == "-99999" || tempModel.AQIValue == "" || tempModel.AQIValue == "NA") {
                point.y = null;
                point.id = null;
                //pointData.push(point);
            }
            else {
                point.y = parseInt(tempModel.AQIValue, 10);
                point.id = tempModel.PrimaryPollutant;
                point.marker = {
                    "enabled": true,
                    "fillColor": "White",
                    "lineColor": getPointColor(point.y),
                    "lineWidth": 2,
                    "radius": 3,
                    "states": {
                        "hover": {
                            "enabled": true,
                            "fillColor": "White",
                            "lineColor": getPointColor(point.y),
                            "lineWidth": 3,
                            "radius": 5
                        }
                    }
                };
                pointData.push(point);
                if (minValue > parseInt(tempModel.AQIValue, 10)) {
                    minValue = parseInt(tempModel.AQIValue, 10);
                }
                if (maxValue < parseInt(tempModel.AQIValue, 10)) {
                    maxValue = parseInt(tempModel.AQIValue, 10);
                }
                dataExit = true;
            }
        }
    }

    maxValue = (parseInt(maxValue / 50) + 1) * 50;

    var serie = {};
    serie.states = {
        "hover": {
            "enabled": true,
            "fillColor": "White",
            "lineColor": "#E48701",
            "lineWidth": 1,
            "radius": 5
        }
    };
    serie.name = "AQI：";
    serie.color = "#25A0DA";
    serie.type = "spline";
    serie.lineWidth = 1;
    serie.data = pointData;

    //优
    var serie1 = {};
    serie1.name = "status_1";
    var pointData1 = new Array();
    var point1 = {
        "x": new Date(dateStart.valueOf() - 60 * 60 * 1000),
        "y": 0,
        "color": "#00e400",
        "marker": {
            "enabled": true,
            "fillColor": "#00e400",
            "lineColor": "#00e400",
            "lineWidth": 0,
            "radius": 1
        }
    };
    var point2 = {
        "x": new Date(dateStart.valueOf() - 59 * 60 * 1000),
        "y": 50,
        "color": "#00e400",
        "marker": {
            "enabled": true,
            "fillColor": "#00e400",
            "lineColor": "#00e400",
            "lineWidth": 0,
            "radius": 1
        }
    };
    pointData1.push(point1, point2);
    serie1.data = pointData1;
    serie1.color = "#00e400";
    serie1.lineWidth = 10;
    serie1.states = {
        "hover": {
            "enabled": false
        }
    }

    //良
    var serie2 = {};
    serie2.name = "status_2";
    var pointData2 = new Array();
    var point1 = {
        "x": new Date(dateStart.valueOf() - 60 * 60 * 1000),
        "y": 0,
        "color": "yellow",
        "marker": {
            "enabled": true,
            "fillColor": "yellow",
            "lineColor": "yellow",
            "lineWidth": 0,
            "radius": 1
        }
    };
    var point2 = {
        "x": new Date(dateStart.valueOf() - 59 * 60 * 1000),
        "y": 50,
        "color": "yellow",
        "marker": {
            "enabled": true,
            "fillColor": "yellow",
            "lineColor": "yellow",
            "lineWidth": 0,
            "radius": 1
        }
    };
    pointData2.push(point1, point2);
    serie2.data = pointData2;
    serie2.color = "yellow";
    serie2.lineWidth = 10;
    serie2.states = {
        "hover": {
            "enabled": false
        }
    }

    //轻度污染
    var serie3 = {};
    serie3.name = "status_3";
    var pointData3 = new Array();
    var point1 = {
        "x": new Date(dateStart.valueOf() - 60 * 60 * 1000),
        "y": 0,
        "color": "#ffa500",
        "marker": {
            "enabled": true,
            "fillColor": "#ffa500",
            "lineColor": "#ffa500",
            "lineWidth": 0,
            "radius": 1
        }
    };
    var point2 = {
        "x": new Date(dateStart.valueOf() - 59 * 60 * 1000),
        "y": 50,
        "color": "#ffa500",
        "marker": {
            "enabled": true,
            "fillColor": "#ffa500",
            "lineColor": "#ffa500",
            "lineWidth": 0,
            "radius": 1
        }
    };
    pointData3.push(point1, point2);
    serie3.data = pointData3;
    serie3.color = "#ffa500";
    serie3.lineWidth = 10;
    serie3.states = {
        "hover": {
            "enabled": false
        }
    }

    //中度污染
    var serie4 = {};
    serie4.name = "status_4";
    var pointData4 = new Array();
    var point1 = {
        "x": new Date(dateStart.valueOf() - 60 * 60 * 1000),
        "y": 0,
        "color": "Red",
        "marker": {
            "enabled": true,
            "fillColor": "Red",
            "lineColor": "Red",
            "lineWidth": 0,
            "radius": 1
        }
    };
    var point2 = {
        "x": new Date(dateStart.valueOf() - 59 * 60 * 1000),
        "y": 50,
        "color": "Red",
        "marker": {
            "enabled": true,
            "fillColor": "Red",
            "lineColor": "Red",
            "lineWidth": 0,
            "radius": 1
        }
    };
    pointData4.push(point1, point2);
    serie4.data = pointData4;
    serie4.color = "Red";
    serie4.lineWidth = 10;
    serie4.states = {
        "hover": {
            "enabled": false
        }
    }

    //重度污染
    var serie5 = {};
    serie5.name = "status_5";
    var pointData5 = new Array();
    var point1 = {
        "x": new Date(dateStart.valueOf() - 60 * 60 * 1000),
        "y": 0,
        "color": "#800080",
        "marker": {
            "enabled": true,
            "fillColor": "#800080",
            "lineColor": "#800080",
            "lineWidth": 0,
            "radius": 1
        }
    };
    var point2 = {
        "x": new Date(dateStart.valueOf() - 59 * 60 * 1000),
        "y": 100,
        "color": "#800080",
        "marker": {
            "enabled": true,
            "fillColor": "#800080",
            "lineColor": "#800080",
            "lineWidth": 0,
            "radius": 1
        }
    };
    pointData5.push(point1, point2);
    serie5.data = pointData5;
    serie5.color = "#800080";
    serie5.lineWidth = 10;
    serie5.states = {
        "hover": {
            "enabled": false
        }
    }

    //严重污染
    var serie6 = {};
    serie6.name = "status_6";
    var pointData6 = new Array();
    var point1 = {
        "x": new Date(dateStart.valueOf() - 60 * 60 * 1000),
        "y": 0,
        "color": "#7e0023",
        "marker": {
            "enabled": true,
            "fillColor": "#7e0023",
            "lineColor": "#7e0023",
            "lineWidth": 0,
            "radius": 1
        }
    };
    var point2 = {
        "x": new Date(dateStart.valueOf() - 59 * 60 * 1000),
        "y": maxValue - 300,
        "color": "#7e0023",
        "marker": {
            "enabled": true,
            "fillColor": "#7e0023",
            "lineColor": "#7e0023",
            "lineWidth": 0,
            "radius": 1
        }
    };
    pointData6.push(point1, point2);
    serie6.data = pointData6;
    serie6.color = "#7e0023";
    serie6.lineWidth = 10;
    serie6.states = {
        "hover": {
            "enabled": false
        }
    }

    if (dataExit) {
        seriesData.push(serie);
    }

    var tickInterval = 50;
    if (maxValue > 300) {
        seriesData.push(serie6);
        tickInterval = 50;
    }
    if (maxValue > 200) {
        if (maxValue <= 300) {
            maxValue = 300;
            tickInterval = 50;
        }
        seriesData.push(serie5);
    }
    if (maxValue > 150) {
        if (maxValue <= 200) {
            maxValue = 200;
            tickInterval = 25;
        }
        seriesData.push(serie4);
    }
    if (maxValue > 100) {
        if (maxValue <= 150) {
            maxValue = 150;
            tickInterval = 25;
        }
        seriesData.push(serie3);
    }
    if (maxValue > 50) {
        if (maxValue <= 100) {
            maxValue = 100;
            tickInterval = 25;
        }
        seriesData.push(serie2);
    }
    else {
        tickInterval = 10;
    }
    seriesData.push(serie1);

    var hcAQI24Hours = new Highcharts.Chart({
        chart: {
            renderTo: "Container",
            defaultSeriesType: 'line',
            backgroundColor: 'rgba(119,152,191,0)'
        },
        title: '',
        credits: {
            enabled: false
        },
        plotOptions: {
            series: {
                dataLabels: {
                    enabled: false
                },
                stacking: "normal",
                shadow: false,
                states: {
                    hover: {
                        enabled: false
                    }
                }
            }
        },
        tooltip: {
            formatter: function (event) {
                return getTooTip24Hours(this)
            }
        },
        legend: {
            enabled: false
        },
        exporting: {
            enabled: false
        },
        xAxis: [{
            labels: {
                enabled: true,
                rotation: -30,
                step: 2,
                formatter: function (event) {
                    return getHC24HoursXAxis(this);
                },
                y: 25
            },
            type: "datetime",
            min: dateStart.valueOf() - 60 * 60 * 1000,
            max: dateEnd.valueOf(),
            tickInterval: 3600000,
            tickLength: 10
        }],
        yAxis: [{
            title: '',
            tickWidth: 1,
            labels: {
                enabled: true
            },
            lineWidth: 0,
            max: maxValue,
            min: 0,
            offset: 0,
            tickInterval: tickInterval,
            tickLength: 5
        }],

        series: seriesData
    });
}

//取得AQI变化图的Y轴色带的TooTip
function getTooTip24Hours(obj) {
    var strRet = "";
    switch (obj.series.name) {
        case "status_6":
            {
                strRet = "空气质量指数：> 300 <br/>";
                strRet += "空气质量状况：严重污染 <br/>";
                break;
            }
        case "status_5":
            {
                strRet = "空气质量指数：201~300 <br/>";
                strRet += "空气质量状况：重度污染 <br/>";
                break;
            }
        case "status_4":
            {
                strRet = "空气质量指数：151~200 <br/>";
                strRet += "空气质量状况：中度污染 <br/>";
                break;
            }
        case "status_3":
            {
                strRet = "空气质量指数：101~150 <br/>";
                strRet += "空气质量状况：轻度污染 <br/>";
                break;
            }
        case "status_2":
            {
                strRet = "空气质量指数：51~100 <br/>";
                strRet += "空气质量状况：良 <br/>";
                break;
            }
        case "status_1":
            {
                strRet = "空气质量指数：0~50 <br/>";
                strRet += "空气质量状况：优 <br/>";
                break;
            }
        default:
            {
                strRet = "空气质量指数：" + obj.y + "<br/>";
                strRet += "空气质量等级：" + getGradeFromAQI(obj.y) + "<br/>";
                if (typeof (obj.point.id) != "undefined" && obj.point.id != null) {
                    var str = obj.point.id;
                    str = str.replace("SO2", "SO<sub>2</sub>");
                    str = str.replace("NO2", "NO<sub>2</sub>");
                    str = str.replace("PM10", "PM<sub>10</sub>");
                    str = str.replace("PM2.5", "PM<sub>2.5</sub>");
                    str = str.replace("O3", "O<sub>3</sub>");
                    str = str.replace("NO2", "NO<sub>2</sub>");
                    str = str.replace("NO2", "NO<sub>2</sub>");
                    strRet += "首要污染物：" + str + "<br/>";
                }
                strRet += "时间：" + new Date(obj.x).Format("yyyy-MM-dd HH:00");
                break;
            }
    }
    return strRet;
}

var Pre7DaysStartDate = null;
//最近七天AQI变化图的处理
function getAQIForPre7DaysCallback(response) {
    response = eval(response)
    var seriesData = new Array();
    var iList = 0;
    var minValue = 0;
    var maxValue = 0;
    var dateEnd = new Date();
    var dataExit = false;
    if (response != "undefined" && response != null) {
        if (response.length > 0) {
            Pre7DaysStartDate = new Date(response[0].ReportDateTime);
            dateEnd = new Date(response[response.length - 1].ReportDateTime);
        }
        for (var iRow = 0; iRow < response.length; iRow++) {
            var serie = {};
            var pointData = new Array();
            var value = 0;
            //var point = {};
            //point.x = iRow + 1;

            var tempModel = response[iRow];

            if (tempModel.AQIValue != null && tempModel.AQIValue != "" && tempModel.AQIValue != "-99999" && tempModel.AQIValue != "NA") {

                value = parseInt(tempModel.AQIValue, 10);
                if (minValue > parseInt(tempModel.AQIValue, 10)) {
                    minValue = parseInt(tempModel.AQIValue, 10);
                }
                if (maxValue < parseInt(tempModel.AQIValue, 10)) {
                    maxValue = parseInt(tempModel.AQIValue, 10);
                }
                if (value != null && value != 'null') {
                    var pointY_01 = 0;
                    var pointY_02 = 0;
                    var pointY_03 = 0;
                    if (value < 20) {
                        pointY_02 = 0;
                        pointY_03 = value;
                    }
                    else {
                        pointY_02 = value - 0;
                        pointY_03 = value;
                    }

                    var point1 = { "x": iRow + 1, "y": pointY_01, "color": getPointColor(value), "marker": { "enabled": false, "fillColor": "#00e400", "lineColor": "#00e400", "lineWidth": 0, "radius": 0 } };
                    var point2 = { "x": iRow + 1, "y": pointY_02, "color": getPointColor(value), "marker": { "enabled": false, "fillColor": "#00e400", "lineColor": "#00e400", "lineWidth": 0, "radius": 0 } };
                    pointData.push(point1);
                    pointData.push(point2);
                    var serie = {};
                    serie.type = "line";
                    serie.data = pointData;
                    if (value == null || value == 'null')
                        serie.name = 'null';
                    else
                        serie.name = value;
                    serie.color = getPointColor(value);//"#99ccff";
                    serie.lineWidth = 10;
                    serie.states = { "hover": { "enabled": false } }
                    seriesData.push(serie);

                    pointData = new Array();
                    point1 = { "x": iRow + 1, "y": pointY_02, "color": getPointColor(value), "marker": { "enabled": false, "fillColor": "#00e400", "lineColor": "#00e400", "lineWidth": 0, "radius": 0 } };
                    point2 = { "x": iRow + 1, "y": pointY_03, "color": getPointColor(value), "marker": { "enabled": false, "fillColor": "#00e400", "lineColor": "#00e400", "lineWidth": 0, "radius": 0 } };
                    pointData.push(point1);
                    pointData.push(point2);
                    var serie = {};
                    serie.type = "line";
                    //serie.type = "column";
                    serie.data = pointData;
                    if (value == null || value == 'null')
                        serie.name = 'null';
                    else
                        serie.name = value;
                    serie.color = getPointColor(value);
                    serie.lineWidth = 10;
                    serie.states = { "hover": { "enabled": false } }
                    seriesData.push(serie);
                }


            }
        }
    }


    maxValue = (parseInt(maxValue / 50) + 1) * 50;
    //优
    var serie1 = {};
    serie1.name = "status_1";
    serie1.type = "line";
    var pointData1 = new Array();
    var point1 = { "x": 0, "y": 0, "color": "#00e400", "marker": { "enabled": true, "fillColor": "#00e400", "lineColor": "#00e400", "lineWidth": 0, "radius": 1 } };
    var point2 = { "x": 0, "y": 50, "color": "#00e400", "marker": { "enabled": true, "fillColor": "#00e400", "lineColor": "#00e400", "lineWidth": 0, "radius": 1 } };
    pointData1.push(point1, point2);
    serie1.data = pointData1;
    serie1.color = "#00e400";
    serie1.lineWidth = 10;
    serie1.states = { "hover": { "enabled": false } }

    //良
    var serie2 = {};
    serie2.name = "status_2";
    var pointData2 = new Array();
    var point1 = { "x": 0, "y": 50, "color": "yellow", "marker": { "enabled": true, "fillColor": "yellow", "lineColor": "yellow", "lineWidth": 0, "radius": 1 } };
    var point2 = { "x": 0, "y": 100, "color": "yellow", "marker": { "enabled": true, "fillColor": "yellow", "lineColor": "yellow", "lineWidth": 0, "radius": 1 } };
    pointData2.push(point1, point2);
    serie2.data = pointData2;
    serie2.type = "line";
    serie2.color = "yellow";
    serie2.lineWidth = 10;
    serie2.states = { "hover": { "enabled": false } }

    //轻度污染
    var serie3 = {};
    serie3.name = "status_3";
    var pointData3 = new Array();
    var point1 = { "x": 0, "y": 100, "color": "#ffa500", "marker": { "enabled": true, "fillColor": "#ffa500", "lineColor": "#ffa500", "lineWidth": 0, "radius": 1 } };
    var point2 = { "x": 0, "y": 150, "color": "#ffa500", "marker": { "enabled": true, "fillColor": "#ffa500", "lineColor": "#ffa500", "lineWidth": 0, "radius": 1 } };
    pointData3.push(point1, point2);
    serie3.data = pointData3;
    serie3.type = "line";
    serie3.color = "#ffa500";
    serie3.lineWidth = 10;
    serie3.states = { "hover": { "enabled": false } }

    //中度污染
    var serie4 = {};
    serie4.name = "status_4";
    var pointData4 = new Array();
    var point1 = { "x": 0, "y": 150, "color": "Red", "marker": { "enabled": true, "fillColor": "Red", "lineColor": "Red", "lineWidth": 0, "radius": 1 } };
    var point2 = { "x": 0, "y": 200, "color": "Red", "marker": { "enabled": true, "fillColor": "Red", "lineColor": "Red", "lineWidth": 0, "radius": 1 } };
    pointData4.push(point1, point2);
    serie4.data = pointData4;
    serie4.type = "line";
    serie4.color = "Red";
    serie4.lineWidth = 10;
    serie4.states = { "hover": { "enabled": false } }

    //重度污染
    var serie5 = {};
    serie5.name = "status_5";
    var pointData5 = new Array();
    var point1 = { "x": 0, "y": 200, "color": "#800080", "marker": { "enabled": true, "fillColor": "#800080", "lineColor": "#800080", "lineWidth": 0, "radius": 1 } };
    var point2 = { "x": 0, "y": 300, "color": "#800080", "marker": { "enabled": true, "fillColor": "#800080", "lineColor": "#800080", "lineWidth": 0, "radius": 1 } };
    pointData5.push(point1, point2);
    serie5.data = pointData5;
    serie5.type = "line";
    serie5.color = "#800080";
    serie5.lineWidth = 10;
    serie5.states = { "hover": { "enabled": false } }

    //严重污染
    var serie6 = {};
    serie6.name = "status_6";
    var pointData6 = new Array();
    var point1 = { "x": 0, "y": 300, "color": "#7e0023", "marker": { "enabled": true, "fillColor": "#7e0023", "lineColor": "#7e0023", "lineWidth": 0, "radius": 1 } };
    var point2 = { "x": 0, "y": maxValue, "color": "#7e0023", "marker": { "enabled": true, "fillColor": "#7e0023", "lineColor": "#7e0023", "lineWidth": 0, "radius": 1 } };
    pointData6.push(point1, point2);
    serie6.data = pointData6;
    serie6.type = "line";
    serie6.color = "#7e0023";
    serie6.lineWidth = 10;
    serie6.states = { "hover": { "enabled": false } }

    var tickInterval = 50;
    if (maxValue > 300) {
        seriesData.push(serie6);
        tickInterval = 50;
    }
    if (maxValue > 200) {
        if (maxValue <= 300) {
            maxValue = 300;
            tickInterval = 50;
        }
        seriesData.push(serie5);
    }
    if (maxValue > 150) {
        if (maxValue <= 200) {
            maxValue = 200;
            tickInterval = 25;
        }
        seriesData.push(serie4);
    }
    if (maxValue > 100) {
        if (maxValue <= 150) {
            maxValue = 150;
            tickInterval = 25;
        }
        seriesData.push(serie3);
    }
    if (maxValue > 50) {
        if (maxValue <= 100) {
            maxValue = 100;
            tickInterval = 25;
        }
        seriesData.push(serie2);
    }
    else {
        tickInterval = 10;
    }
    seriesData.push(serie1);

    var hcAQI7Days = new Highcharts.Chart({
        chart: { renderTo: "Container", backgroundColor: 'rgba(119,152,191,0)' },
        credits: { enabled: false },
        plotOptions: {
            column: {

                stacking: "normal"
            }
        },
        title: { "text": "" },
        legend: { "enabled": false },
        exporting: { "enabled": false },
        xAxis: [{ "labels": { "enabled": true, "rotation": -30, "formatter": function (event) { return getHC7DaysXAxis(this); }, "y": 25 }, "min": 0, "max": 32, "plotLines": [{}], "tickInterval": 2, "tickLength": 10, "title": { "text": "" } }],
        yAxis: [{ "tickWidth": 1, "labels": { "enabled": true }, "lineWidth": 0, "max": maxValue, "min": 0, "offset": 0, "tickInterval": tickInterval, "tickLength": 5, "title": { "text": "" } }],
        tooltip: { "formatter": function (event) { return getTooTip7Days(this) } },
        series: seriesData
    });
}

//取得最近七天AQI变化图的X轴坐标
function getHC7DaysXAxis(obj) {

    if (obj.value == 0 || obj.value == 32) {
        return "";
    }
    else {
        return new Date(Pre7DaysStartDate.valueOf() + (obj.value - 1) * 24 * 60 * 60 * 1000).Format("MM-dd");
    }
}

//取得AQI变化图的Y轴色带的TooTip
function getTooTip7Days(obj) {
    var strRet = "";
    switch (obj.series.name) {
        case "status_6":
            {
                strRet = "空气质量指数：> 300 <br/>";
                strRet += "空气质量状况：严重污染 <br/>";
                break;
            }
        case "status_5":
            {
                strRet = "空气质量指数：201~300 <br/>";
                strRet += "空气质量状况：重度污染 <br/>";
                break;
            }
        case "status_4":
            {
                strRet = "空气质量指数：151~200 <br/>";
                strRet += "空气质量状况：中度污染 <br/>";
                break;
            }
        case "status_3":
            {
                strRet = "空气质量指数：101~150 <br/>";
                strRet += "空气质量状况：轻度污染 <br/>";
                break;
            }
        case "status_2":
            {
                strRet = "空气质量指数：51~100 <br/>";
                strRet += "空气质量状况：良 <br/>";
                break;
            }
        case "status_1":
            {
                strRet = "空气质量指数：0~50 <br/>";
                strRet += "空气质量状况：优 <br/>";
                break;
            }
        default:
            {
                if (obj.series.name == null || obj.series.name == 'null')
                    strRet = "AQI：--<br/>";
                else
                    strRet = "AQI：" + obj.series.name + "<br/>";
                strRet += "日期：" + new Date(Pre7DaysStartDate.valueOf() + (parseInt(obj.x) - 1) * 24 * 60 * 60 * 1000).Format("yyyy-MM-dd");;
                break;
            }
    }
    return strRet;
}

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "H+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

