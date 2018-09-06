<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualittyHalfYearReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirQualittyHalfYearReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript">
                function RefreshParent() {
                    this.parent.Refresh_Grid(true);
                }
                //行编辑按钮 pageTypeID和waterOrAirType参数名称固定pageTypeID：该页面的ID;waterOrAirType：水或气，0：表示水，1：表示气
                function ShowDetails() {

                }
            </script>
        </telerik:RadScriptBlock>
        <table>
            <tr>
                <td colspan="2">
                    <asp:ImageButton ID="btnSave" OnClick="btnSave_Click" runat="server" CssClass="RadToolBar_Customer" SkinID="ImgBtnSave" /></td>
            </tr>
            <tr>
            </tr>
            <tr>
                <td style="text-align: center; width: 80px">报表时间：</td>
                <td style="text-align: left; width: 120px">
                    <telerik:RadMonthYearPicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM" Width="100px"
                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" OnSelectedDateChanged="dtpBegin_SelectedDateChanged" AutoPostBack="true"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                </td>
                <td style="text-align: center; width: 40px">&nbsp;&nbsp;至</td>
                <td style="text-align: center; width: 120px">
                    <telerik:RadMonthYearPicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM" Width="100px"
                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" OnSelectedDateChanged="dtpEnd_SelectedDateChanged" AutoPostBack="true"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                </td>
                <td style="text-align: center; width: 80px">日期范围：</td>
                <td colspan="3">
                    <asp:TextBox runat="server" BorderWidth="0" BorderColor="#ffffff" ID="txtDateF" ReadOnly="true" Width="100px"></asp:TextBox>~<asp:TextBox runat="server" ID="txtDateT" BorderWidth="0" BorderColor="#ffffff" ReadOnly="true" Width="100px"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2">
                    <%-- <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="保存" OnClientClick="RefreshParent()" />--%>
                    <asp:Button ID="btnExport" runat="server" Text="下载" OnClick="btnExport_Click" Visible="false" /></td>
            </tr>
        </table>
        <table style="margin-left: 5%; margin-right: 10%; width: 100%; text-align: center">
            <tr>
                <td style="text-align: center; font-size: 26px; width: 100%">苏州市环境质量报告
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 24px; width: 100%">
                    <label runat="server" id="M1"></label>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 24px; width: 100%">苏州市环境监测中心
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 20px; width: 100%">
                    <label runat="server" id="M2"></label>
                </td>
            </tr>
            <%--  <tr>
                <td style="text-align: center; font-size: 24px; width: 100%">目录
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">一、环境空气质量 ......................................................................	1
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">1.细颗粒物（PM2.5）...................................................................................  2
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">2.可吸入颗粒物（PM10）.............................................................................  2
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">3.二氧化氮（NO2）......................................................................................  3
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">4.二氧化硫（SO2）......................................................................................  3
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">5.臭氧（O3）................................................................................................  4
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">6.一氧化碳（CO）........................................................................................  4
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">7.环境空气质量排名......................................................................................  5
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">8.酸雨............................................................................................................  5
                </td>
            </tr>--%>
            <tr>
                <td style="font-size: 20px; width: 100%">一、环境空气质量
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M3" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%;" />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 14px; width: 100%">图1
                    <label runat="server" id="M100"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M31" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater2" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%;" />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 14px; width: 100%">图2
                    <label runat="server" id="M99"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">1.细颗粒物（PM2.5）
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M32" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater3" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%;" />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 14px; width: 100%">图3
                 <label runat="server" id="M98"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">2.可吸入颗粒物（PM10）
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M33" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater4" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%;" />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 14px; width: 100%">图4
                    <label id="M97" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">3.二氧化氮（NO2）
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M34" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater5" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%;" />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 14px; width: 100%">图5
                    <label id="M96" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">4.二氧化硫（SO2）
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M35" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater6" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%;" />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 14px; width: 100%">图6
                    <label id="M95" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">5.臭氧（O3）
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M36" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater7" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%;" />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 14px; width: 100%">图7
                    <label id="M94" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">6.一氧化碳（CO）
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M37" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater8" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%;" />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 14px; width: 100%">图8
                    <label id="M93" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">7.环境空气质量排名
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M38" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 14px; width: 100%">图9
                    <label id="M92" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">8.酸雨
                </td>
            </tr>
            <tr>
                <td style="font-size: 18px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M39" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 14px; width: 100%">图10
                    <label id="M91" runat="server"></label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
