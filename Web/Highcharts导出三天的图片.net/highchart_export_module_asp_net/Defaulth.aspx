<%@ Page Language="C#" ValidateRequest="false"  AutoEventWireup="true" CodeBehind="Defaulth.aspx.cs" Inherits="highchart_export_module_asp_net.Defaulth" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Highchart js export module sample</title>
    <script src="Javascript/ChartHeatmap.js"></script>
    <script src="Javascript/jquery-1.8.3.min.js"></script>
    <script src="Javascript/highcharts.js"></script>
    <script src="Javascript/highcharts-more.js"></script>
    <script src="Javascript/exporting.js"></script>
    <script src="Javascript/data.js"></script>
    <script src="Javascript/heatmap.js"></script>
    <script src="Javascript/highcharts-zh_CN.js"></script>
    <script type="text/javascript">
        
    </script>
</head>
<body>
    <script type="text/javascript">
        
        var charth;
        $(function () {
            var Border = document.getElementById("hdBorder").value;
            var Time = document.getElementById("hdTime").value;
            var count = document.getElementById("hdCount").value;
            var b = eval("(" + Border + ")");
            var c = eval("(" + Time + ")");
            var chart = new Highcharts.Chart('ChartContainer', {
                title: {
                    text: '激光雷达',
                    x: -20
                },
                subtitle: {
                    text: '边界层高度',
                    x: -20
                },
                xAxis: {
                    type: Time.type,
                    tickInterval: Math.ceil(count / 5),
                    categories: c,
                    showLastLabel: true,
                    dateTimeLabelFormats: {
                        day: '%m/%d %H时',
                        hour: '%m/%d %H时',
                        month: '%m/%d',
                        week: '%m-%d',

                        second: '%d日%H点',
                        minute: '%d日%H点'
                    }
                },
                yAxis: {
                    title: {
                        text: '边界层高度 (Km)'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    valueSuffix: 'Km'
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                exporting: {type: "image/svg+xml",url: "/NTJGLDXG/highcharts_exporth.aspx",}
                    ,
                series: [{
                    name: '边界层高度',
                    data: b
                }
                ]
            });

        });
        //导出图表按钮  
        
        function ExportClickh() {
            debugger
            charth = $('#ChartContainer').highcharts();

            charth.exportChart({
                exportFormat: 'image/svg+xml'
            });
        }
        window.onload = function () {
            //页面加载完成20s后自动导出svg图片
            debugger
            setTimeout("document.form1.btnExporth.click()", 15000);//millisec,15s
        }
    </script> 
    <form id="form1" runat="server">
    <h1 align="center" style="color:red;">正在生成激光雷达文件，请不要关闭该窗口！</h1>
        <div id="ChartContainer" style="width:900px;"></div>
        <input type="button" id="btnExporth" value="导出" onclick="ExportClickh();" style="width: 100px" runat="server" /> 
        <asp:HiddenField ID="hdCount" runat="server" Value="" />
        <asp:HiddenField ID="hdTime" runat="server" Value="" />
        <asp:HiddenField ID="hdBorder" runat="server" Value="" />
        <div>
    </div>
    </form>
</body>
</html>
