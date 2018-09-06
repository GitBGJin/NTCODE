<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="highcharts_export.aspx.cs" Inherits="highchart_export_module_asp_net.highcharts_export" ValidateRequest="false" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Page sans titre</title>
    <script src="Javascript/jquery-1.8.3.min.js"></script>
    <script src="Javascript/canvas2image.js"></script>
    <script src="Javascript/base64.js"></script>
    <script src="Javascript/canvg.js"></script>
</head>
<body>
    <script type="text/javascript">
        function myrefresh() {
            window.location.reload();
        }
        setTimeout('myrefresh();pageGoTo()', 60000); //1分刷新一次页面

        //跳转到激光雷达页面
        function pageGoTo() {
            debugger
            var date = new Date();
            var minuteNow = date.getMinutes();
            if (minuteNow >= 1 && minuteNow < 2)
            {
                setTimeout(window.location.href = 'http://localhost/NTJGLDXG/Default.aspx', 1000);
            }
            //setTimeout(window.location.href = 'http://localhost/NTJGLDXG/Default.aspx', 1000);
        }
    </script>
    <form id="form1" runat="server">
    <h1 align="center" style="color:red;">正在生成激光雷达文件，请不要关闭该窗口！</h1>
    </form>
</body>
</html>
