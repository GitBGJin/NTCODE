<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PointWeekReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.PointWeekReport" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=9.0.15.324, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
        <script type="text/javascript">
            function OnClientClicking(sender, args) {
                var date1 = $find("<%= weekList.ClientID %>").get_selectedDate();
                if (date1 == null || date1 == "") {
                    args._cancel = true;
                    alert("时间不能为空！");
                    //sender.set_autoPostBack(false);
                    //return false;
                }
                else {
                    return true;
                }
            }

            $(document).ready(function () {
                SetHeigth();
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
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ReportViewer1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MonthTime">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="weekList" />
                         <telerik:AjaxUpdatedControl ControlID="Literal1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="weekList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Literal1" />
                        <telerik:AjaxUpdatedControl ControlID="weekList" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">测点:
                        </td>
                        <td class="content" style="width: 180px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="220" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="title" style="width: 80px">监测因子:
                        </td>
                        <td class="content" style="width: 370px;">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="360" DropDownWidth="420" ID="factorCbxRsm"></CbxRsm:FactorCbxRsm>
                        </td>
                        <td class="content" style="width: 80px;" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="uploadData" runat="server" OnClientClick="return OnClientClicking()" OnClick="uploadData_Click" SkinID="ImgBtnExcel" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">日期:
                        </td>
                        <td class="content" style="width: 360px;">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadMonthYearPicker ID="MonthTime" AutoPostBack="true" runat="server" Width="120px"
                                            OnSelectedDateChanged="MonthTime_SelectedDateChanged">
                                            <MonthYearNavigationSettings TodayButtonCaption="今天" CancelButtonCaption="取消" OkButtonCaption="确定" />
                                        </telerik:RadMonthYearPicker>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="weekList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="weekList_SelectedIndexChanged" SkinID="Default" Skin="Default"></telerik:RadComboBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td></td>
                        <td class="content">
                            <asp:Literal ID="Literal1" runat="server"></asp:Literal></td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div style="width: 100%; height: 500px;" id="divReportViewer" runat="server">
                    <telerik:ReportViewer ID="ReportViewer1" runat="server" ShowExportGroup="false" ShowPrintButton="false" ShowPrintPreviewButton="false"
                        BackColor="White" Width="100%" Height="100%" ParametersAreaVisible="True" ToolbarVisible="false" OnLoad="ReportViewer1_Load" />
                </div>
            </telerik:RadPane>
        </telerik:RadSplitter>

    </form>
</body>
