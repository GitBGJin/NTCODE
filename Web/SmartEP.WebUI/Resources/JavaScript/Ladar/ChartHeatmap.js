﻿function ajaxHighChart(title, subtitle, strJSON, id, xName, yName, zName, hdMin, hdMax) {
    $('#' + id).html("<div style='padding-left:30%;padding-right:30%;padding-top:20%;'>正在加载数据....</div>");
    if (xName == "") {
    } else {
        xName = xName + ":";
    }
    if (yName == "") {
    } else {
        yName = yName + ":";
    }
    if (zName == "") {
    } else {
        zName = zName + ":";
    }
    var dataValue = [];
    if (strJSON == "" || strJSON == "[]") {
    } else {
        dataValue = eval('(' + strJSON + ')');
    }

    var objvalue = [];
    var xcategories = [];
    //var xtimecategories = [];
    var ycategories = [];

    var i = 0;
    var j = 0;
    for (var obj1 in dataValue) {
        var tmpValue = dataValue[obj1].xValue;
        var newstr = tmpValue.replace(/-/g, '/');
        var date = new Date(newstr);
        var time_str = date.getTime().toString();
        if (xcategories.indexOf(dataValue[obj1].xValue) < 0) {
            xcategories.push(dataValue[obj1].xValue);
            //xtimecategories.push(time_str);
            var mm = dataValue[obj1].xValue;
            strJSON = strJSON.replace(/\\/g, '');
            strJSON = strJSON.replace(new RegExp('"' + dataValue[obj1].xValue + '"', 'g'), i);
            i++;
        }
        if (ycategories.indexOf(dataValue[obj1].yValue) < 0) {
            ycategories.push(dataValue[obj1].yValue);

            strJSON = strJSON.replace(new RegExp('"' + dataValue[obj1].yValue + '"', 'g'), '"' + j + '"');
            j++;
        }

    }
    var dataarray=[];
    dataarray = eval('(' + strJSON + ')');
    for (var obj1 in dataarray) {
        objvalue.push([parseFloat(dataarray[obj1].xValue), parseFloat(dataarray[obj1].yValue), parseFloat(dataarray[obj1].zValue)]);
    }
    //var stopC = null;
    ////[0.00, '#003de9'], [0.60, '#2dd466'], [1.20, '#87ff00'], [1.80, '#fefb00'], [2.40, '#ff9700'], [3.00, '#ff0000']
    //if (subtitle == "消光系数532") {
    //    stopC = "{stops: [[0.00, '#003de9'],[0.20, '#2dd466'],[0.40, '#87ff00'],[0.60, '#fefb00'],[0.80, '#ff9700'],[1.00, '#ff0000']],min: 0.00,max: 1.00,tickInterval: 0.2,startOnTick: true,endOnTick: true,labels: {format: '{value}'}}";
    //    stopC = eval('(' + stopC + ')');
    //}
    //if (subtitle == "消光系数355") {
    //    stopC = "{stops: [[0.00, '#003de9'],[0.20, '#2dd466'],[0.40, '#87ff00'],[0.60, '#fefb00'],[0.80, '#ff9700'],[1.00, '#ff0000']],min: 0.00,max: 3.00,tickInterval: 0.6,startOnTick: true,endOnTick: true,labels: {format: '{value}'}}";
    //    stopC = eval('('+stopC+')');
    //}
    //if (subtitle == "退偏图") {
    //    stopC = "{stops: [[0.00, '#003de9'],[0.20, '#2dd466'],[0.40, '#87ff00'],[0.60, '#fefb00'],[0.80, '#ff9700'],[1.00, '#ff0000']],min: 0.00,max: 0.30,tickInterval: 0.06,startOnTick: true,endOnTick: true,labels: {format: '{value}'}}";
    //    stopC = eval('('+stopC+')');
    //}
    var hmin = parseFloat(hdMin);
    var hmax = parseFloat(hdMax);
    var htick = hmax / 5;
    $.ajax({
        type: "POST",
        data: dataValue,
        dataType: "",
        cache: false, //指令
        async: true, //取消同步
        beforeSend: function () {
        }, //发送数据之前
        success: function (msg) {
            (function (H) {
                var Series = H.Series,
                    each = H.each;
                /**
                 * Create a hidden canvas to draw the graph on. The contents is later copied over
                 * to an SVG image element.
                 */
                Series.prototype.getContext = function () {
                    if (!this.canvas) {
                        this.canvas = document.createElement('canvas');
                        this.canvas.setAttribute('width', this.chart.chartWidth);
                        this.canvas.setAttribute('height', this.chart.chartHeight);
                        this.image = this.chart.renderer.image('', 0, 0, this.chart.chartWidth, this.chart.chartHeight).add(this.group);
                        this.ctx = this.canvas.getContext('2d');
                    }
                    return this.ctx;
                };
                /**
                 * Draw the canvas image inside an SVG image
                 */
                Series.prototype.canvasToSVG = function () {
                    this.image.attr({ href: this.canvas.toDataURL('image/png') });
                };
                /**
                 * Wrap the drawPoints method to draw the points in canvas instead of the slower SVG,
                 * that requires one shape each point.
                 */
                H.wrap(H.seriesTypes.heatmap.prototype, 'drawPoints', function () {
                    var ctx = this.getContext();
                    if (ctx) {
                        // draw the columns
                        each(this.points, function (point) {
                            var plotY = point.plotY,
                                shapeArgs,
                                pointAttr;
                            if (plotY !== undefined && !isNaN(plotY) && point.y !== null) {
                                shapeArgs = point.shapeArgs;
                                pointAttr = (point.pointAttr && point.pointAttr['']) || point.series.pointAttribs(point);
                                ctx.fillStyle = pointAttr.fill;
                                ctx.fillRect(shapeArgs.x, shapeArgs.y, shapeArgs.width, shapeArgs.height);
                            }
                        });
                        this.canvasToSVG();
                    } else {
                        this.chart.showLoading('Your browser doesn\'t support HTML5 canvas, <br>please use a modern browser');
                    }
                });
                H.seriesTypes.heatmap.prototype.directTouch = false; // Use k-d-tree
            }(Highcharts));
            Highcharts.setOptions({

                lang: {
                    resetZoom: '重置',
                    resetZoomTitle: '重置缩放比例',
                    rangeSelectorZoom: ''    //隐藏Zoom
                }
            });
            $('#' + id).highcharts({
                chart: {
                    type: 'heatmap',
                    margin: [60, 10, 100, 100],
                    plotBackgroundColor: '#EFEFEF'
                    //zoomType: 'x',
                    //panning: true,
                    //panKey: 'shift'
                },
                title: {
                    text: title,
                    align: 'center',
                    x: 40
                },
                subtitle: {
                    text: subtitle,
                    align: 'center',
                    x: 40
                },
                xAxis: {
                    type: 'string',
                    categories: xcategories || [],
                    labels: {
                        //formatter: function () {
                        //    return Highcharts.dateFormat('%H:%M:%S',this.value);
                        //},
                        //style:{layoutFlow: 'vertical-ideographic'},
                        //formatter: function () {
                        //    return this.value.split('').join("<br />");
                        //},
                        //formatter:function () {
                        //    return Highcharts.dateFormat("%H点%M分", this.value);
                        //},
                        format: '{value}', // long month
                        //format:function () {
                        //    return Highcharts.dateFormat('%H:%M:%S',this.value);
                        //},
                        step: Math.ceil(xcategories.length / 3)
                    },
                    showLastLabel: false,
                    showFirstLabel: true,
                    //tickInterval: xcategories ? (xcategories.length - 1) : 0,
                    //minTickInterval: 500,
                    //maxTickInterval:3600,
                    //startOnTick: true,
                    //tickPixelInterval:100,
                    tickLength: 1
                    //tickInterval: 32
                },
                //yAxis: {
                //    title: {
                //        text: yName
                //    },
                //    labels: {
                //        format: '{value}',
                //        //step:1
                //    },
                //    tickInterval: 80,
                //    //tickPositions: [3.0,2.4,1.8,1.2,0.6,0],
                //    showLastLabel: false,
                //    showFirstLabel: true,
                //    startOnTick: false,
                //    endOnTick: false,
                //    //min:1,
                //    //max:400,
                //    //tickAmount: 8,
                //    //gridLineWidth:0,
                //    categories: ycategories,
                //    minPadding: 0,
                //    maxPadding: 0,
                //    reversed: true
                //},
                yAxis: {
                    title: {
                        text: yName
                    },
                    labels: {
                        format: '{value}',
                        //step:0.5
                    },
                    //tickInterval: 80,
                    //showLastLabel: true,
                    //showFirstLabel: true,
                    startOnTick: true,
                    endOnTick: true,
                    tickLength: 1,
                    //tickAmount: 8,
                    //gridLineWidth:0,
                    tickPositions: [0, Math.ceil((ycategories.length - 1) / 4), Math.ceil((ycategories.length - 1)*2 / 4), Math.ceil((ycategories.length - 1)*3 / 4), ycategories.length - 1],
                    categories: ycategories,
                    //minPadding: ycategories.length,
                    minPadding: 0,
                    maxPadding: 0,
                    reversed: false
                },
                colorAxis:
                    {
                    stops: [
                        [0.00, '#003de9'],
                        [0.20, '#2dd466'],
                        [0.40, '#87ff00'],
                        [0.60, '#fefb00'],
                        [0.80, '#ff9700'],
                        [1.00, '#ff0000']
                    ],
                    min: hmin,
                    max: hmax,
                    tickInterval: htick,
                    startOnTick: true,
                    endOnTick: true,
                    labels: {
                        format: '{value}'
                    }
                    
                }
                ,
                tooltip: {
                    formatter: function () {
                        return '<b>' + xName + this.series.xAxis.categories[this.point.x] + '</b><br><b>' + yName +
                            this.series.yAxis.categories[this.point.y] + '</b><br><b>' + zName + this.point.value + '</b>';
                    }
                },
                series: [{
                    borderWidth: 0,
                    nullColor: '#EFEFEF',
                    data: objvalue,

                    turboThreshold: Number.MAX_VALUE // #3404, remove after 4.0.5 release
                }]
            })
        }
    });
    //return defer.promise();
}