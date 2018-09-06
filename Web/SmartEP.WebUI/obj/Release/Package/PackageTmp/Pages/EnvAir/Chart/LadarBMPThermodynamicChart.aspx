<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LadarBMPThermodynamicChart.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Chart.LadarBMPThermodynamicChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../../Resources/jquery-1.9.0.min.js"></script>
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
        <script src="../../../Resources/highcharts.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/data.js"></script>
        <script src="../../../Resources/heatmap.js"></script>
        <script src="../../../Resources/JavaScript/Polary/highcharts-zh_CN.js"></script>
        <script type="text/javascript">
            $(function () {
                /**
                 * This plugin extends Highcharts in two ways:
                 * - Use HTML5 canvas instead of SVG for rendering of the heatmap squares. Canvas
                 *   outperforms SVG when it comes to thousands of single shapes.
                 * - Add a K-D-tree to find the nearest point on mouse move. Since we no longer have SVG shapes
                 *   to capture mouseovers, we need another way of detecting hover points for the tooltip.
                 */
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
                            // Uncomment this to provide low-level (slow) support in oldIE. It will cause script errors on
                            // charts with more than a few thousand points.
                            // arguments[0].call(this);
                        }
                    });
                    H.seriesTypes.heatmap.prototype.directTouch = false; // Use k-d-tree
                }(Highcharts));
                var start;
                Highcharts.chart('container', {
                    data: {
                        csv: document.getElementById('hdjsonData').value,
                        parsed: function () {
                            start = +new Date();
                        }
                    },
                    chart: {
                        type: 'heatmap',
                        margin: [60, 10, 80, 50]
                    },
                    title: {
                        text: '大型热力图',
                        align: 'center',
                        x: 40
                    },
                    subtitle: {
                        text: '2013每天每小时的热力变化',
                        align: 'center',
                        x: 40
                    },
                    xAxis: {
                        type: 'datetime',
                        //min: Date.UTC(2013, 0, 1),
                        //max: Date.UTC(2014, 0, 1),
                        min: hdDtStart.value,
                        max: hdDtEnd.value,
                        labels: {
                            align: 'left',
                            x: 5,
                            y: 14,
                            format: '{value:%B}' // long month
                        },
                        showLastLabel: false,
                        tickLength: 16
                    },
                    yAxis: {
                        title: {
                            text: '高度(km)'
                        },
                        labels: {
                            format: '{value}'
                        },
                        minPadding: 0,
                        maxPadding: 0,
                        startOnTick: true,
                        endOnTick: true,
                        tickInterval: 0.4,
                        tickPositions: [0.8, 1.6, 2.4, 3.2, 4.0, 4.8, 5.0],
                        tickWidth: 1.0,
                        min: 0.2,
                        max: 5.0
                    },
                    colorAxis: {
                        stops: [
                            [0.00, '#003de9'],
                            [0.20, '#2dd466'],
                            [0.40, '#87ff00'],
                            [0.60, '#fefb00'],
                            [0.80, '#ff9700'],
                            [1.00, '#ff0000']
                        ],
                        min: 0.00,
                        max: 1.00,
                        tickInterval: 0.2,
                        startOnTick: false,
                        endOnTick: false,
                        labels: {
                            format: '{value}'
                        }
                    },
                    series: [{
                        borderWidth: 0,
                        nullColor: '#EFEFEF',
                        colsize: 24 * 36e5, // one day
                        tooltip: {
                            headerFormat: 'Temperature<br/>',
                            pointFormat: '{point.x:%e %b, %Y} {point.y}:00: <b>{point.value} ℃</b>'
                        },
                        turboThreshold: Number.MAX_VALUE // #3404, remove after 4.0.5 release
                    }]
                });
                console.log('Rendered in ' + (new Date() - start) + ' ms'); // eslint-disable-line no-console
            });

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        </div>
        
        <div id="container" style="min-width: 310px; max-width: 600px; height: 400px; margin: 0 auto"></div>
        <asp:HiddenField ID="hdjsonData" runat="server" value=""/>
        <asp:HiddenField ID="hdDtStart" runat="server" value=""/>
        <asp:HiddenField ID="hdDtEnd" runat="server" value=""/>
        <asp:HiddenField ID="hdtitle" runat="server" value=""/>
        <asp:HiddenField ID="hdunit" runat="server" value=""/>
    </form>
</body>
</html>
