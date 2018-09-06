﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChartFrame.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Chart.ChartFrame" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/highcharts.js"></script>
        <script src="../../../Resources/JavaScript/ChartOperator/ShowChart.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
        <script src="../../../Resources/JavaScript/Polary/highcharts-zh_CN.js"></script>
        <%--    <script src="../../../Resources/JavaScript/HighCharts/defaultTheme.js" type="text/javascript"></script> --%>
        <script type="text/javascript">
            $("document").ready(function () {
                InitChart();
            });

            function InitChart() {
                try {
                    var hiddenData = $("#HiddenData", window.parent.document).val().split('|');
                    var RegionAll = $("#HiddenDataRegionValue", window.parent.document).val();
                    var Region = '<%=Request["Region"]%>' != "" ? '<%=Request["Region"]%>' : RegionAll;
                    var ajaxURL = $("#AjaxURL", window.parent.document).val();
                    var PointID = '<%=Request["PointID"]%>' != "" ? '<%=Request["PointID"]%>' : hiddenData[0];
                    var AllPointID = hiddenData[0];
                    var FactorCode = 0;
                    var facData = document.getElementById("hdfac").value;
                    if (facData == 0) {
                        FactorCode = '<%=Request["FactorCode"]%>' != "" ? '<%=Request["FactorCode"]%>' : hiddenData[1];//用于分屏显示
                    }
                    else {
                        FactorCode = '<%=Request["FactorCode"]%>' != "" ? '<%=Request["FactorCode"]%>' : facData;//用于分屏显示
                    }
                    var name = document.getElementById("hdname").value;
                    var Name = '<%=Request["Name"]%>' != "" ? '<%=Request["Name"]%>' : name;//用于分屏显示
                    //var hdIsLJPY = $("#hdIsLJPY").val();//区分是否为粒径谱仪
                    var DtBegin = hiddenData[2];
                    var DtEnd = hiddenData[3];
                    var radlDataType = hiddenData[4];
                    var pageType = hiddenData[5];
                    var tabStrip = hiddenData.length > 6 ? hiddenData[6] : "0";
                    var DataType = '<%=Request["DataType"]%>' != "" ? '<%=Request["DataType"]%>' : 0;//用于数据对比多时间段
                    var tabStrip = hiddenData.length > 6 ? hiddenData[6] : "0";
                    var ChartType = $("#HiddenChartType", window.parent.document) != undefined ? $("#HiddenChartType", window.parent.document).val() : "spline"
                    var PointFactor = $("#HiddenPointFactor", window.parent.document) != undefined ? $("#HiddenPointFactor", window.parent.document).val() : "point"
                    var PageSize = 20000;
                    var PageNo = 0;

                    var dataValue = {
                        PointID: PointID,
                        AllPointID: AllPointID,
                        FactorCode: FactorCode,
                        DtBegin: DtBegin,
                        DtEnd: DtEnd,
                        radlDataType: radlDataType,
                        pageType: pageType,
                        tabStrip: tabStrip,
                        DataType: DataType,
                        PageSize: PageSize,
                        PageNo: PageNo,
                        ChartType: ChartType,
                        PointFactor: PointFactor,
                        Name: Name,
                        Region:Region
                        //hdIsLJPY: hdIsLJPY
                    };
                    ajaxHighChart(ajaxURL, dataValue, "RealTimeChart");
                } catch (e) {
                }

            }

            //图形切换
            function HighChartTypeChange(charttype) {
                if (charttype == "" || charttype == undefined) charttype = "spline";
                var options = $("#RealTimeChart").highcharts().options;
                options.chart.type = charttype;
                var chart = new Highcharts.Chart(options);
            }


        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div id="RealTimeChart" style="height: 100%; width: 100%;"></div>
        <asp:HiddenField ID="hdfac" runat="server" value="0"/>
        <asp:HiddenField ID="hdname" runat="server" value=""/>
    </form>
</body>
</html>
