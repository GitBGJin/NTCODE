<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirMonthReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirMonthReport" %>

<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
        <script type="text/javascript">
            function OnClientClicking(sender, args) {
                var date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
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

            function ShowWebOffice(filename) {
                //获取主机地址之后的目录
                var pathName = window.location.pathname;
                for (var i = 0; i < pathName.length; i++) {
                    if (pathName.substr(1).indexOf('/') > 0) {
                        break;
                    }
                    pathName = pathName.substr(1);
                }
                //获取带"/"的项目名
                var projectName = pathName.substring(0, pathName.substr(1).indexOf('/') + 1);
                //拼装完整路径
                var url = projectName + "/Pages/EnvAir/Report/AirMonthReportEdit.aspx?filename=" + filename;
                parent.window.showwindows("<%=Guid.NewGuid().ToString()%>", url, unescape(filename));
            }
        </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <%--<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ReportViewer1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>

        <table id="Tb" style="width: 100%;" class="Table_Customer"
            border="0">
            <tr>
                <td class="title" style="width: 80px; text-align: center;">测点:
                </td>
                <td class="content" style="width: 250px;">
                    <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="400" CbxHeight="350" MultiSelected="true" DropDownWidth="420" ID="pointCbxRsm" OnSelectedChanged="pointCbxRsm_SelectedChanged"></CbxRsm:PointCbxRsm>
                </td>
                </tr>
            <tr>
                <td class="title" style="width: 80px;">报表时间:
                </td>
                <td class="content" style="width: 140px;">
                      <telerik:RadMonthYearPicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"  AutoPostBack="true"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                </td>
            </tr>
             <tr>
                <td >
                    <asp:ImageButton ID="btnSave"  OnClientClick="return OnClientClicking()" OnClick="btnSave_Click" runat="server" CssClass="RadToolBar_Customer" SkinID="ImgBtnSave" /></td>
                  <td class="content">
                  <%--  <telerik:RadButton ID="RadButton1" runat="server" BackColor="#3A94D3" ForeColor="White" Height="20" AutoPostBack="True" OnClientClick="return OnClientClicking();" OnClick="uploadData_Click">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="Label4" ForeColor="White" Text="导出"></asp:Label>
                        </ContentTemplate>
                    </telerik:RadButton>--%>
                </td>
            </tr>
        </table>


    </form>
</body>
</html>

