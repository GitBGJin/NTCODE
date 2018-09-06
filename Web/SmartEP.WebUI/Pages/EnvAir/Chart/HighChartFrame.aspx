<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HighChartFrame.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Chart.HighChartFrame" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%--<script src="../../../Resources/jquery-1.9.0.min.js"></script>--%>
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
        <script src="../../../Resources/highcharts.js"></script>
        <script src="../../../Resources/heatmap.js"></script>
        <script src="../../../Resources/ChartHeatmap.js"></script>
        <script type="text/javascript">
            //function HeatmapChart() {
            //    ;
            //    var pointids = $("#HiddenDataR").val();
            //    var jsonData = $("#HiddenDataS").val();
            //    ;
            //    var chartdiv = "";
            //    chartdiv = "";
            //    chartdiv += "<div id=\"" + pointids + "\" style=\"height: 50px; width: 100%; float: left; display: block\">";
            //    //chartdiv += "<h1>111111</h1>";
            //    chartdiv += "</div>";
            //    $("#container").append(chartdiv);
            //    var c = $("#container");
            //    ajaxHighChart("南门", "南门", jsonData, pointids, "时间", "监测因子", "数值");//传值 

            //};

            $("document").ready(function () {
                var pointName = $("#HiddenDataR").val();
                var strJSON = $("#HiddenDataS").val();//获取数据源
                ajaxHighChart(pointName, pointName, strJSON, "container", "时间", "监测因子", "数值");//传值 
            });
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div id="container" style="height: 100%; width: 100%;" runat="server"></div>
        <asp:HiddenField ID="HiddenDataS" runat="server" Value="" />
        <asp:HiddenField ID="HiddenDataR" runat="server" Value="" />
    </form>
</body>
</html>
