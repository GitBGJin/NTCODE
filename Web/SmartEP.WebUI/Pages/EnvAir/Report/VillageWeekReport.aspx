﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VillageWeekReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.VillageWeekReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=9.0.15.324, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script src='<%=ResolveClientUrl("~/Resources/JavaScript/jquery/jquery-1.7.1.min.js") %>' type="text/javascript"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>市区、区县周报</title>
    <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
        <script type="text/javascript" language="javascript">
            //$(document).ready(function () {
            //    SetHeigth();
            //});
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
                if (divHeight > 300) {
                    divHeight = divHeight - 60;
                }
                document.getElementById("<%=divReportViewer.ClientID%>").style.height = divHeight + "px";
                document.getElementById("<%=ReportViewer1.ClientID%>").style.height = divHeight + "px";
            }
        </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
        <%--     <asp:ScriptManager ID="ss" runat="server" />
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">--%>
        <%--<script src='<%= ResolveClientUrl("../../../Resources/JavaScript/telerikControls/RadComboxIncludeSiteMap.js") %>' type="text/javascript"></script>--%>
        <%--         <script type="text/javascript">
                function OnClientClicked(button, args) {
                    var BtnTxt = button._text;
                    var BtnCmdName = button._commandName;
                    executeCheck('all', BtnCmdName, '', 'multi');
                }
            </script>
        </telerik:RadScriptBlock>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">--%>
        <%--   <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadButton4">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="iFrameRepDay" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0" BorderSize="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="tbBase" cellspacing="1" width="100%" cellpadding="0" class="Table_Customer" border="0">
                    <tr>
                        <td class="title" style="width: 60px;">时间
                        </td>
                        <td>
                            <table>
                                <tr>

                                    <td class="content" style="width: 120px;">
                                        <%--                                        <telerik:RadDatePicker ID="RadMonthYearPicker1" AutoPostBack="true" runat="server" EnableTyping="false" DateInput-DateFormat="yyyy-MM-dd"
                                            OnSelectedDateChanged="RadMonthYearPicker1_SelectedDateChanged" OnViewCellCreated="RadMonthYearPicker1_ViewCellCreated"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                        </telerik:RadDatePicker>--%>
                                        <telerik:RadDatePicker runat="server" ID="RadMonthYearPicker1" AutoPostBack="true"
                                            OnSelectedDateChanged="RadMonthYearPicker1_SelectedDateChanged" OnViewCellCreated="RadMonthYearPicker1_ViewCellCreated"
                                            MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                        </telerik:RadDatePicker>
                                    </td>
                                    <%--  <td style="width: 90px;">
                                        <telerik:RadComboBox ID="cmboWeeks" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmboWeeks_SelectedIndexChanged" Width="80px"></telerik:RadComboBox>
                                    </td>--%>
                                    <td class="title" style="width: 50px;">因子
                                    </td>
                                    <td class="content" style="width: 100px;">
                                        <telerik:RadComboBox runat="server" ID="cbxFactor" Width="100px">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="PM2.5" Value="PM25" Selected="true" />
                                                <telerik:RadComboBoxItem Text="PM10" Value="PM10" />
                                                <telerik:RadComboBoxItem Text="二氧化硫" Value="SO2" />
                                                <telerik:RadComboBoxItem Text="二氧化氮" Value="NO2" />
                                                <telerik:RadComboBoxItem Text="一氧化碳" Value="CO" />
                                                <telerik:RadComboBoxItem Text="臭氧" Value="Max8HourO3" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="title" style="width: 80px">考核基数：
                                    </td>
                                    <td class="content" style="width: 100px;">
                                        <telerik:RadComboBox runat="server" ID="YearBegin" Width="90px" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="content" style="width: 100px;">
                                        <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                                    </td>
                                    <td class="content">
                                        <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" SkinID="ImgBtnSave" />
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="btnExport" runat="server" Text="下载" OnClick="btnExport_Click" Visible="false" /></td>
                                    <%-- <td valign="middle" style="text-align: left">
                                        <telerik:RadButton ID="RadButton4" runat="server" Width="60px" Text=" 查  询 " OnClick="RadBtnSearch_Click"
                                            Image-IsBackgroundImage="true" />
                                    </td>--%>
                                </tr>
                            </table>
                        </td>

                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div style="width: 100%; height: 500px;" id="divReportViewer" runat="server">
                    <telerik:ReportViewer ID="ReportViewer1" runat="server"
                        BackColor="White" Width="100%" Height="100%" ParametersAreaVisible="True" />
                </div>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--</telerik:RadAjaxPanel>--%>

        <%--     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                $(document).ready(function () {
                    var IFrame = document.getElementById("iFrameRepDay");
                    IFrame.style.visibility = 'visible';

                });


                function IFrameReHeigth(url) {
                    try {
                        var IFrame = document.getElementById("iFrameRepDay");
                        IFrame.height = (IFrame.parentElement.offsetParent.clientHeight - 78) + 'px';
                        document.getElementById("iFrameRepDay").src = url;

                    } catch (e) {
                    }
                }


            </script>
        </telerik:RadCodeBlock>

        <iframe id="iFrameRepDay" name="iFrameRepDay" frameborder="0" style="margin: 0; padding: 0; z-index: 7; border: 0px solid Gray; visibility: hidden;"
            width="100%" height="500px" marginwidth="0" scrolling="no"></iframe>--%>
    </form>
</body>
</html>

