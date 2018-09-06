<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualityWeekReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirQualityWeekReport" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=9.0.15.324, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>空气质量周报</title>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript" language="javascript">
            function OnClientClicking(sender, args) {
                var date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                var date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                if (date1 == null || date1 == "" || date2 == null || date2 == "") {
                    args._cancel = true;
                    alert("时间不能为空！");
                    //sender.set_autoPostBack(false);
                    //return false;

                }
                else {
                    return true;
                }
                if (date1 > date2) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
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
            function RefreshParent() {
                this.parent.Refresh_Grid(true);
                window.close();
            }
            //关闭遮罩层
            function closeWin() {
                var bgObj = document.getElementById("divbgObj");
                if (bgObj !== null)
                    document.body.removeChild(bgObj);
            }
            //遮罩层
            function alertWin() {
                var iWidth = document.body.clientWidth;
                var iHeight = document.body.clientHeight;

                var bgObj = document.createElement("div");
                bgObj.setAttribute("id", "divbgObj");
                bgObj.style.cssText = "position:absolute;left:0px;top:0px;width:" + iWidth + "px;height:" + Math.max(document.body.clientHeight, iHeight) + "px;filter:Alpha(Opacity=30);opacity:0.3;background: url('../Images/login/BgSpliter.png');background-color:#FEFEFE;z-index:101;text-align:center; vertical-align:middle;color:red;";
                var bgimg = document.createElement("img");
                bgimg.setAttribute("src", "/Skins/Default/Ajax/loading.gif");
                bgObj.appendChild(bgimg);
                document.body.appendChild(bgObj);

            }
            window.onload = function () {
                //遍历页面所有 按钮添加loading效果，目前测试 只用一个
                //var target = document.getElementById("btnMonthReport");
                var target1 = document.getElementById("btnExport");
                var type = "click";
                var func = alertWin;
                if (target1.addEventListener) {
                    target1.addEventListener(type, func, false);
                } else if (target1.attachEvent) {
                    target1.attachEvent("on" + type, func);
                } else {
                    target1["on" + type] = func;
                }
            }
        </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0" BorderSize="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px;">日期:
                        </td>
                        <td class="content" style="width: 200px;">
                            <div runat="server" id="dbtDay">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadDatePicker runat="server" ID="dtpBegin" AutoPostBack="true" OnSelectedDateChanged="dtpBegin_SelectedDateChanged"
                                                MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td>&nbsp;至&nbsp;
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="dtpEnd" runat="server" AutoPostBack="true" OnSelectedDateChanged="dtpEnd_SelectedDateChanged"
                                                MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td class="content" style="width: 100px;">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                        <td class="content">
                            <asp:ImageButton ID="btnExport" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnExport_Click" SkinID="ImgBtnSave" />
                        </td>
                        <td></td>
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
    </form>
</body>
</html>
