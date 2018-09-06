<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OzoneSpecialNew.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Special.OzoneSpecialNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="../../../Resources/JavaScript/FrameJS.js"></script>
    <style>
        .SpanTitle {
            font-size: 16px;
            color: #6995CA;
            vertical-align: middle;
            font-family: 'Microsoft YaHei',SimSun;
            margin-top: 8px;
            margin-left: 10px;
            border-left: 6px solid #6995CA;
            float: left;
            font-weight: 600;
            padding-left: 18px;
            background-color: #eff3f6;
        }

        .fieldsetTitle {
            /*padding-left: 10px;
            padding-right: 10px;
            padding-bottom: 10px;*/
            border-color: #E6F2FE;
            /*border: 1px solid #CECDCD;*/
            background-color: white;
        }

        .divTitle {
            width: 100%;
            height: 25px;
        }
    </style>
    <script type="text/javascript">
        function ShowEditForm(id) {
            var oWnd = radopen("../Alarm/AlarmHandle.aspx?AlarmUid=" + id, "AlarmHandle");
            return false;
        }
        function onclickS() {
            var childStatus = document.getElementById("iframe1").contentWindow;
            if (childStatus.GetData != undefined) {
                var ChlidData = childStatus.GetData();
                var pointNames = ChlidData.PointNames;
                var FactorName = ChlidData.FactorName;
                OpenFineUIWindow("e6137d5a-8393-45dd-bcd1-e018a6fcadd6", "Pages/EnvAir/RealTimeData/RealTimeData.aspx?pointNames=" + pointNames + "&FactorName=" + FactorName, "实时数据");
                return false;
            }
        }

        function onclickQ() {
            var childStatus = document.getElementById("iframe4").contentWindow;
            if (childStatus.GetData != undefined) {
                var ChlidData = childStatus.GetData();
                var regionUid = ChlidData.regionUid;
                var DateBegin = ChlidData.DateBegin;
                var DateEnd = ChlidData.DateEnd;
                OpenFineUIWindow("bc1e261c-3d83-4acd-9299-3a0b81beb7a1", "Pages/EnvAir/Report/AirQualityDayReport.aspx?regionUid=" + regionUid + "&DateBegin=" + DateBegin + "&DateEnd=" + DateEnd, "空气质量日报");
                return false;
            }
        }

        function onclickAlertInfo() {
            var childStatus = document.getElementById("iframe5").contentWindow;
            if (childStatus.GetData != undefined) {
                var ChlidData = childStatus.GetData();
                var pointId = ChlidData.pointid;
                var DateBegin = ChlidData.beginTime;
                var DateEnd = ChlidData.endTime;
                OpenFineUIWindow("bc1e261c-3d83-4acd-9299-3a0b81beb7a1", "Pages/EnvAir/Alarm/AlarmCompsite.aspx?pointId=" + pointId + "&DateBegin=" + DateBegin + "&DateEnd=" + DateEnd, "报警信息");
                return false;
            }
        }

        function onclickRealTime() {
            var childStatus = document.getElementById("iframe3").contentWindow;
            if (childStatus.GetData != undefined) {
                var ChlidData = childStatus.GetData();
                var cityTypeUids = ChlidData.CityTypeUids;
                OpenFineUIWindow("d2c46160-b6c0-4a75-a2ab-0509a44e0754", "Pages/EnvAir/RealTimeData/RealTimeAirQuality.aspx?CityTypeUids=" + cityTypeUids, "实时环境空气质量");
                return false;
            }
        }
        function onclickRealOnline() {

            var childStatus = document.getElementById("iframeOCM").contentWindow;
            if (childStatus.GetData != undefined) {
                var ChlidData = childStatus.GetData();
                var pointNames = ChlidData.PointNames;
                var FactorName = ChlidData.FactorName;

                OpenFineUIWindow("49502372-21c4-440b-9243-e71571712dba", "Pages/EnvAir/RealTimeData/RealTimeOnlineState.aspx?pointNames=" + pointNames + "&FactorName=" + FactorName, "实时在线状态信息");
                return false;
            }


        }
        function Refresh_Grid(args) {

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="ajaxManager" runat="server">
            <ajaxsettings>
                <telerik:AjaxSetting AjaxControlID="gridAlarm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAlarm" LoadingPanelID="radAjaxLoadingPanel2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </ajaxsettings>
        </telerik:RadAjaxManager>
        <table style="width: 99%; margin: auto;">
            <tr>
                <td style="width: 100%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                            <div class="SpanTitle" style="width: 98%;">
                                气象参数分析图
                            </div>
                        </div>
                        <div style="height: 550px;" id="divToday">
                            <iframe id="iframeOCM" src="SpecialWeather.aspx?Type=O3" style="width: 100%; height: 550px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
            </tr>
       <%--     <tr>
                <td style="width: 100%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                               <div class="SpanTitle" style="width: 98%;">风速风向玫瑰图</div>
                        </div>
                        <div style="height: 580px;">
                            <iframe id="iframe2" src="PolaryWindDirection.aspx" style="width: 100%; height: 580px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
                <td style="width: 50%;"></td>
            </tr>--%>
            <tr>
                <td style="width: 100%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                              <div class="SpanTitle" style="width: 98%;">
                                多因子分析
                            </div>
                        </div>
                        <div style="height: 500px;">
                            <iframe id="iframe3" src="SuperSolarRadiation.aspx?Type=O3" style="width: 100%; height: 500px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 100%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                              <div class="SpanTitle" style="width: 98%;">
                                VOC因子占比
                            </div>
                        </div>
                        <div style="height: 560px;">
                            <iframe id="iframe4" src="OzonePrecursor.aspx?Type=O3" style="width: 100%; height: 560px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
            </tr>
            <%--<tr>
                <td style="width: 100%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                             <div class="SpanTitle" style="width: 98%;">
                                城市能见度
                            </div>
                        </div>
                        <div style="height: 580px;">
                            <iframe id="iframe1" src="SpecialVisibility.aspx" style="width: 100%; height: 580px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
            </tr>--%>
        </table>
    </form>
</body>
</html>
