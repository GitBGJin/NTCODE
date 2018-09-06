<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GranuleSpecialNew.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Special.GranuleSpecialNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
   
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="ajaxManager" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gridAlarm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAlarm" LoadingPanelID="radAjaxLoadingPanel2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
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
                            <iframe id="iframeOCM" src="SpecialWeather.aspx?Type=PM" style="width: 100%; height: 550px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
            </tr>
          <%--  <tr style="display:none">
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
                        <div style="height: 480px;">
                            <iframe id="iframe3" src="FactorSpecial.aspx?Type=PM" style="width: 100%; height: 480px;"
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
                                城市能见度
                            </div>
                        </div>
                        <div style="height: 580px;">
                            <iframe id="iframe1" src="SpecialVisibility.aspx" style="width: 100%; height: 580px;"
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
                                激光雷达
                            </div>
                        </div>
                        <div style="height: 560px;">
                            <iframe id="iframe4" src="LadarBMPThermodynamic.aspx" style="width: 100%; height: 580px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
            </tr>

        </table>
    </form>
</body>
</html>
