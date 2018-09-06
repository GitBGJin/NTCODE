<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AQIof24Hours.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Dock.AQIof24Hours" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../../Resources/JavaScript/HighCharts/highcharts.js" type="text/javascript"></script>
    <script src="../../../Resources/JavaScript/AQI/AQIIndex.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            SetHeigth();
            AjaxLoadingData();
        });

        function SetHeigth() {
            var divHeight = 0;
            // 获取窗口高度
            if (window.innerHeight)
                divHeight = window.innerHeight;
            else if ((document.body) && (document.body.clientHeight))
                divHeight = document.body.clientHeight;
            // 通过深入Document内部对body进行检测，获取窗口大小
            if (document.documentElement && document.documentElement.clientHeight) {
                divHeight = document.documentElement.clientHeight;
            }
            if (divHeight > 340) {
                divHeight = 340;
            }
            document.getElementById("Container").style.height = divHeight + "px";
            document.getElementById("Container").style.width = "100%";
        }

        function AjaxLoadingData() {
            $.ajax({
                type: 'post', //用POST方式传输
                url: '../ChartAjaxRequest/HourAQI.ashx?r=' + Math.random(),
                dataType: 'json',
                data: {
                    action: 'hours',
                    MonitoringRegionUid: '7e05b94c-bbd4-45c3-919c-42da2e63fd43',
                    hours: '24'
                },
                //async: false,
                success: function (data) {
                    getAQIForPre24HoursCallback(data);
                },
                error: function (res) {
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div runat="server" id="Container">
        </div>
    </form>
</body>
</html>
