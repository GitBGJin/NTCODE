<%@ Page Language="C#" MasterPageFile="~/Pages/EnvAir/Forecast/MainFrame.Master" AutoEventWireup="true" CodeBehind="AQIForecast.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Forecast.AQIForecast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>空气质量预报</title>
    <style type="text/css">
        /*开始：一般Table的样式*/
        table.Table_Customer7 {
            border: 0px solid #FFFFFF;
            border-collapse: collapse;
        }

            table.Table_Customer7 td.border {
                border-bottom: 1px solid #fff;
            }

            table.Table_Customer7 td {
                color: #000;
                border-left: 1px solid #FFFFFF;
                border-right: 1px solid #ffffff;
                border-top: 1px solid #ffffff;
            }

                table.Table_Customer7 td.header {
                    text-align: center;
                    font-weight: 700;
                    border-top: 0px solid #ffffff;
                    background-color: #d2e5f4;
                    height: 30px;
                    font-size: 28px;
                    /*background-image: url('images/RadGridHeaderBg.png');*/
                    /*background-image: url(../Resource/Images/Portal/allbg.gif);*/
                    background-position: 0px 0pt;
                    background-color: #85b4de;
                    padding-left: 10px;
                }

                table.Table_Customer7 td.title {
                    background-color: #d1eff5; /*#d2e5f4*/
                    text-align: center;
                    width: 120px;
                }

                table.Table_Customer7 td.content {
                    font-size: 25px;
                    text-align: center;
                    background-color: #d2e5f4;
                }

                table.Table_Customer7 td.btns {
                    text-align: center;
                    background-color: #d2e5f4;
                }
        /*结束：一般Table的样式*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
    <table id="maintable" class="Table_Customer7" width="100%">
        <tr>
            <td style="border: 0px;" rowspan="8"></td>
            <td colspan="4" style="width: 640px; text-align: center; border: 0px; font-size: 30px; font-weight: 800; padding-top: 50px; padding-bottom: 5px;">空气质量预报</td>
            <td style="border: 0px;" rowspan="8"></td>
        </tr>
        <tr>
            <td class="header" style="width: 120px;">时段</td>
            <td class="header" style="width: 200px;">空气质量</td>
            <td class="header" style="width: 200px;">首要污染物</td>
            <td class="header" style="width: 120px;">AQI</td>
        </tr>
        <tr>
            <td class="content"><span id="SpanAQITimeA" runat="server" style="width: 60px;">&nbsp;</span></td>
            <td class="content"><span id="SpanAQIClassA" runat="server" style="width: 100px">&nbsp;</span></td>
            <td class="content"><span id="SpanPrimaryPollutantA" runat="server" style="width: 100px">&nbsp;</span></td>
            <td class="content"><span id="SpanAQIA" runat="server" style="width: 60px">&nbsp;</span></td>
        </tr>
        <tr>
            <td class="content"><span id="SpanAQITimeB" runat="server" style="width: 60px;">&nbsp;</span></td>
            <td class="content"><span id="SpanAQIClassB" runat="server" style="width: 100px">&nbsp;</span></td>
            <td class="content"><span id="SpanPrimaryPollutantB" runat="server" style="width: 100px">&nbsp;</span></td>
            <td class="content"><span id="SpanAQIB" runat="server" style="width: 60px">&nbsp;</span></td>
        </tr>
        <tr>
            <td class="content"><span id="SpanAQITimeC" runat="server" style="width: 60px;">&nbsp;</span></td>
            <td class="content"><span id="SpanAQIClassC" runat="server" style="width: 100px">&nbsp;</span></td>
            <td class="content"><span id="SpanPrimaryPollutantC" runat="server" style="width: 100px">&nbsp;</span></td>
            <td class="content"><span id="SpanAQIC" runat="server" style="width: 60px">&nbsp;</span></td>
        </tr>
        <tr>
            <td class="content" colspan="4" style="text-align: left; font-size: 20px; color: #5B5B5B;">
                <span id="SpanDescription" runat="server">&nbsp;</span>
            </td>
        </tr>
        <tr>
            <td class="content" colspan="4" style="text-align: right; font-size: 20px; color: #5B5B5B;">
                <span id="SpanIssuedUnit" runat="server">&nbsp;</span>
            </td>
        </tr>
        <tr>
            <td class="content" style="text-align: left; text-align: right; font-size: 20px; color: #5B5B5B;" colspan="4">
                <span id="SpanDT" runat="server">&nbsp;</span>
            </td>
        </tr>
    </table>
</asp:Content>
