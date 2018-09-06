<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Defaultd.aspx.cs" Inherits="highchart_export_module_asp_net.Defaultd" %>

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
        
        var chartd;
        $(function () {
            $('#containerd').html("");
            var strJSOND = document.getElementById("HiddenDatad").value;

            HiddenDatad.value = "";

            ajaxHighChart("激光雷达", "退偏图", strJSOND, "containerd", "时间", "Km", "值");


            $("#containerd").css("overflow-y", "auto");
            $("#containerd").height("500px");//设置图表Iframe的高度
            $("#containerd").width("700px");//设置图表Iframe的宽度
            $("#containerd").css("margin", "0 auto");
        });
        //导出图表按钮
        function ExportClickd() {
            debugger
            chartd = $('#containerd').highcharts();
            chartd.exportChart({
                exportFormat: 'image/svg+xml'
            });
        }
        window.onload = function () {
            //页面加载完成20s后自动导出svg图片
            debugger
            setTimeout("document.form1.btnExportd.click()", 20000);//millisec,20s
        }
    </script> 
    <form id="form1" runat="server">
    <h1 align="center" style="color:red;">正在生成激光雷达文件，请不要关闭该窗口！</h1>
        <div id="containerd" style="width:900px;"></div>
        <input type="button" id="btnExportd" value="导出" onclick="ExportClickd();" style="width: 100px" runat="server" /> 
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="HiddenDatae" runat="server" Value="" />
        <asp:HiddenField ID="HiddenDatad" runat="server" Value="" />
        <div>
    </div>
    </form>
</body>
</html>
