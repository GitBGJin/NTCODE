<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForecastAdd.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Forecast.ForecastAdd" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content1="text/html; charset=utf-8" />
    <title1>增加发布信息</title1>
    <style type="text/css">
        /*开始：一般Table的样式*/
        table.Table_Customer1 {
            border: 1px solid #FFFFFF;
            border-collapse: collapse;
        }

            table.Table_Customer1 td.border {
                border-bottom: 1px solid #fff;
            }

            table.Table_Customer1 td {
                color: #000;
                border-left: 1px solid #FFFFFF;
                border-right: 1px solid #ffffff;
                border-top: 1px solid #ffffff;
            }

                table.Table_Customer1 td.header {
                    text-align: center;
                    background-color: #d2e5f4;
                    height: 30px;
                    font-size: 12px;
                    /*background-image: url('images/RadGridHeaderBg.png');*/
                    /*background-image: url(../Resource/Images/Portal/allbg.gif);*/
                    background-position: 0px 0pt;
                    background-color: #85b4de;
                    padding-left: 10px;
                }

                table.Table_Customer1 td.title1 {
                    background-color: #d1eff5; /*#d2e5f4*/
                    text-align: center;
                    width: 120px;
                }

                table.Table_Customer1 td.content1 {
                    text-align: center;
                    background-color: #d2e5f4;
                }

                table.Table_Customer1 td.btns {
                    text-align: center;
                    background-color: #d2e5f4;
                }
        /*结束：一般Table的样式*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="radCbxAlarmType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radCbxAlarmType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="RadGridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript" language="javascript">
                function GetRadWindow() {
                    var oWindow = null;
                    if (window.radWindow)
                        oWindow = window.RadWindow; //Will work in Moz in all cases, including clasic dialog
                    else if (window.frameElement.radWindow)
                        oWindow = window.frameElement.radWindow; //IE (and Moz as well)
                    return oWindow;
                }

                function Cls(args) {

                    this.parent.Refresh_Grid(args);
                    alert('保存完成！');
                    GetRadWindow().Close();
                    //if (args) {
                    //    lab.style.color = "green";
                    //    lab.innerHTML = "保存成功!";
                    //}
                    //else {
                    //    lab.style.color = "red";
                    //    lab.innerHTML = "保存失败!";
                    //}
                }
            </script>
        </telerik:RadScriptBlock>
        <table id="maintable" class="Table_Customer1" width="100%">
            <tr>
                <td class="header" style="width: 60px;">时段</td>
                <td class="header" style="width: 120px;">空气质量</td>
                <td class="header" style="width: 120px;">首要污染物</td>
                <td class="header">AQI</td>
            </tr>
            <tr>
                <td class="title1">
                    <telerik:RadTextBox ID="RadTxtAQITimeA" runat="server" Enabled="true" Width="60px"></telerik:RadTextBox></td>
                <td class="content1">
                    <telerik:RadComboBox ID="RadCbxAQIClassA" runat="server" Width="130px"></telerik:RadComboBox>
                </td>
                <td class="content1">
                    <telerik:RadComboBox ID="RadCbxPrimaryPollutantA" runat="server" Width="100px"></telerik:RadComboBox>
                </td>
                <td class="content1">
                    <telerik:RadTextBox ID="RadMTxtAQIA" runat="server" Width="50px"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td class="title1">
                    <telerik:RadTextBox ID="RadTxtAQITimeB" runat="server" Enabled="true" Width="60px"></telerik:RadTextBox></td>
                <td class="content1">
                    <telerik:RadComboBox ID="RadCbxAQIClassB" runat="server" Width="130px"></telerik:RadComboBox>
                </td>
                <td class="content1">
                    <telerik:RadComboBox ID="RadCbxPrimaryPollutantB" runat="server" Width="100px"></telerik:RadComboBox>
                </td>
                <td class="content1">
                    <telerik:RadTextBox ID="RadMTxtAQIB" runat="server" Width="50px"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td class="title1">
                    <telerik:RadTextBox ID="RadTxtAQITimeC" runat="server" Enabled="true" Width="60px"></telerik:RadTextBox></td>
                <td class="content1">
                    <telerik:RadComboBox ID="RadCbxAQIClassC" runat="server" Width="130px"></telerik:RadComboBox>
                </td>
                <td class="content1">
                    <telerik:RadComboBox ID="RadCbxPrimaryPollutantC" runat="server" Width="100px"></telerik:RadComboBox>
                </td>
                <td class="content1">
                    <telerik:RadTextBox ID="RadMTxtAQIC" runat="server" Width="50px"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td class="title1" style="font-size: 12px;">评价</td>
                <td class="content1" colspan="3">
                    <telerik:RadTextBox ID="RadTxtDescription" runat="server" TextMode="MultiLine" Width="100%" Height="45px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td class="title1" style="font-size: 12px;">发布单位</td>
                <td class="content1" colspan="3">
                    <telerik:RadTextBox ID="RadTxtIssuedUnit" runat="server" Width="100%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td class="title1" style="font-size: 12px;">发布时间</td>
                <td class="content1" style="text-align: left;" colspan="3">
                    <telerik:RadDateTimePicker ID="RadDT" runat="server" Width="185px" MinDate="1900-01-01 00:00"
                        DateInput-Font-Size="10" DateInput-Font-Bold="true" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                        DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" TimeView-HeaderText="小时" />
                </td>

            </tr>
            <tr>
                <td class="title1" colspan="4">
                    <asp:CheckBox ID="ChkIsIssused" runat="server" Text="是否发布" Width="80px" Font-Size="12px" />
                    <telerik:RadButton ID="RadBtnSave" runat="server" Text="增加" OnClick="RadBtnSave_Click">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" />
                    </telerik:RadButton>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
