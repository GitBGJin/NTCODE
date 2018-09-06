﻿<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeBehind="DefaultThree.aspx.cs" Inherits="highchart_export_module_asp_net._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
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
        var chart;
        var charte;
        var chartd;
        var charth;
        $(function () {
            $('#container').html("");
            var strJSON = document.getElementById("HiddenData").value;
            var strJSONE = document.getElementById("HiddenDatae").value;
            var strJSOND = document.getElementById("HiddenDatad").value;

            var hdMin = document.getElementById("hdMin").value;
            var hdMax = document.getElementById("hdMax").value;

            var hdMine = document.getElementById("hdMine").value;
            var hdMaxe = document.getElementById("hdMaxe").value;

            var hdMind = document.getElementById("hdMind").value;
            var hdMaxd = document.getElementById("hdMaxd").value;


            var strJSONE = document.getElementById("HiddenDatae").value;
            var strJSOND = document.getElementById("HiddenDatad").value;


            HiddenData.value = "";

            HiddenDatae.value = "";
            HiddenDatad.value = "";

            ajaxHighChart("激光雷达", "消光系数532", strJSON, "container", "时间", "Km", "值", hdMin, hdMax);
            ajaxHighChart("激光雷达", "消光系数355", strJSONE, "containere", "时间", "Km", "值", hdMine, hdMaxe);
            ajaxHighChart("激光雷达", "退偏图", strJSOND, "containerd", "时间", "Km", "值", hdMind, hdMaxd);

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
                    tickInterval: Math.ceil(count / 3),
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
                //exporting: {type: "image/svg+xml",url: "/NTJGLDXG/highcharts_exporth.aspx",}
                //    ,
                series: [{
                    name: '边界层高度',
                    data: b
                }
                ]
            });


            

            $("#container").css("overflow-y", "auto");
            $("#container").height("500px");//设置图表Iframe的高度
            $("#container").width("700px");//设置图表Iframe的宽度
            $("#container").css("margin", "0 auto");

            $("#containere").css("overflow-y", "auto");
            $("#containere").height("500px");//设置图表Iframe的高度
            $("#containere").width("700px");//设置图表Iframe的宽度
            $("#containere").css("margin", "0 auto");

            $("#containerd").css("overflow-y", "auto");
            $("#containerd").height("500px");//设置图表Iframe的高度
            $("#containerd").width("700px");//设置图表Iframe的宽度
            $("#containerd").css("margin", "0 auto");

            $("#ChartContainer").css("overflow-y", "auto");
            $("#ChartContainer").height("500px");//设置图表Iframe的高度
            $("#ChartContainer").width("700px");//设置图表Iframe的宽度
            $("#ChartContainer").css("margin", "0 auto");
        });
        //导出图表按钮  
        function ExportClick() {
            debugger

            
            chart = $('#container').highcharts();
            charte = $('#containere').highcharts();
            chartd = $('#containerd').highcharts();
            charth = $('#ChartContainer').highcharts();
            var Svg532 = chart.getSVG();
            var Svg355 = charte.getSVG();
            var Svgtuipian = chartd.getSVG();
            var Svghight = charth.getSVG();
            $.ajax({
                type: "post",
                ContentType: "application/json; charset=utf-8",
                url: "",
                dataType: "json",
                data: {
                    svg532: Svg532,
                    svg355: Svg355,
                    svgtuipian: Svgtuipian,
                    svghight: Svghight
                },
                success: function () {
                    //这里是想实现传入数据后页面跳转
                    document.location = "";
                }
            });
            //chart.exportChart({
            //    exportFormat: 'image/svg+xml'
            //});
            //charte.exportChart({
            //    exportFormat: 'image/svg+xml'
            //});
            //chartd.exportChart({
            //    exportFormat: 'image/svg+xml'
            //});
            //charth.exportChart({
            //    exportFormat: 'image/svg+xml'
            //});
        }
        window.onload = function () {
            //页面加载完成20s后自动导出svg图片
            debugger
            setTimeout("document.form1.btnExport.click()", 50000);//millisec,50s
        }
    </script> 

    <form id="form1" runat="server">
        <h1 align="center" style="color:red;">正在生成激光雷达文件，请不要关闭该窗口！</h1>
        <input type="button" id="btnExport" value="导出" onclick="ExportClick();" style="width: 100px" runat="server" /> 

        <div id="container" style="width:900px;"></div>
        <div id="containere" style="width:900px;"></div>
        <div id="containerd" style="width:900px;"></div>
        <div id="ChartContainer" style="width:700px;"></div>
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />

        <asp:HiddenField ID="HiddenDatae" runat="server" Value="" />
        <asp:HiddenField ID="HiddenDatad" runat="server" Value="" />
        <asp:HiddenField ID="hdMin" runat="server" Value="" />
        <asp:HiddenField ID="hdMine" runat="server" Value="" />
        <asp:HiddenField ID="hdMind" runat="server" Value="" />
        <asp:HiddenField ID="hdMax" runat="server" Value="" />
        <asp:HiddenField ID="hdMaxe" runat="server" Value="" />
        <asp:HiddenField ID="hdMaxd" runat="server" Value="" />
        <asp:HiddenField ID="hdCount" runat="server" Value="" />
        <asp:HiddenField ID="hdTime" runat="server" Value="" />
        <asp:HiddenField ID="hdBorder" runat="server" Value="" />
        <div>
    </div>
    </form>

</body>
</html>
