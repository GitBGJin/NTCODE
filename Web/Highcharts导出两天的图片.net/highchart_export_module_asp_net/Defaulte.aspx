<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Defaulte.aspx.cs" Inherits="highchart_export_module_asp_net.Defaulte" %>

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
        var charte;
        $(function () {
            $('#containere').html("");
            var strJSONE = document.getElementById("HiddenDatae").value;

            HiddenDatae.value = "";

            ajaxHighChart("激光雷达", "消光系数355", strJSONE, "containere", "时间", "Km", "值");

            $("#containere").css("overflow-y", "auto");
            $("#containere").height("500px");//设置图表Iframe的高度
            $("#containere").width("700px");//设置图表Iframe的宽度
            $("#containere").css("margin", "0 auto");
        });
        function ExportClicke() {
            debugger
            charte = $('#containere').highcharts();
            charte.exportChart({
                exportFormat: 'image/svg+xml'
            });
        }
        window.onload = function () {
            //页面加载完成20s后自动导出svg图片
            debugger
            setTimeout("document.form1.btnExporte.click()", 20000);//millisec,20s
        }
    </script> 
    <form id="form1" runat="server">
    <h1 align="center" style="color:red;">正在生成激光雷达文件，请不要关闭该窗口！</h1>
        <div id="containere" style="width:900px;"></div>
        <input type="button" id="btnExporte" value="导出" onclick="ExportClicke();" style="width: 100px" runat="server" />
        <asp:HiddenField ID="HiddenDatae" runat="server" Value="" />
        <div>
    </div>
    </form>
</body>
</html>
