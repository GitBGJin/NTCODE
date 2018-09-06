<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualittyYearReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirQualittyYearReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .td1 {
            border: solid #000 1px;
            width: 100px;
        }

        .td2 {
            border: solid #000 1px;
            width: 150px;
        }
    </style>
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
                <td>
                    <asp:ImageButton ID="btnSave" OnClick="btnSave_Click" runat="server" CssClass="RadToolBar_Customer" SkinID="ImgBtnSave" /></td>
            </tr>
            <tr>
                <td style="text-align: center; width: 80px">报表年份：
                </td>
                <td class="content" style="width: 100px;">
                    <telerik:RadComboBox runat="server" ID="Year" Width="90px" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" OnSelectedIndexChanged="Year_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                </td>
                <td class="title" style="width: 80px">考核基数：
                </td>
                <td class="content" style="width: 100px;">
                    <telerik:RadComboBox runat="server" ID="YearBegin" Width="90px" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消">
                    </telerik:RadComboBox>
                </td>
                <td style="text-align: center; width: 80px">日期范围：</td>
                <td style="width: 400px">
                    <asp:TextBox runat="server" BorderWidth="0" BorderColor="#ffffff" ID="txtDateF" ReadOnly="true" Width="100px"></asp:TextBox>~<asp:TextBox runat="server" ID="txtDateT" BorderWidth="0" BorderColor="#ffffff" ReadOnly="true" Width="100px"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <%-- <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="保存" OnClientClick="RefreshParent()" />--%>
                    <asp:Button ID="btnExport" runat="server" Text="下载" OnClick="btnExport_Click" Visible="false" /></td>
            </tr>
        </table>
        <table style="margin-left: 5%; margin-right: 10%; width: 100%; text-align: center">
            <tr>
                <td style="text-align: center; font-size: 26px; width: 100%">一、环境质量状况
                </td>
            </tr>
            <tr>
                <td style="font-size: 22px; width: 100%">1. 环境空气质量
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M1" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%; " />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图1
                    <label runat="server" id="M100"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M2" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater2" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%; " />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图2
                    <label runat="server" id="M99"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">（1）细颗粒物（PM2.5）
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M3" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater3" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%; " />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图3
                 <label runat="server" id="M98"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">（2）可吸入颗粒物（PM10）
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M4" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater4" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%; " />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图4
                    <label id="M97" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">（3）二氧化氮（NO2）
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M5" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater5" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%; " />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图5
                    <label id="M96" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">（4）二氧化硫（SO2）
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M6" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater6" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%; " />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图6
                    <label id="M95" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">（5）臭氧（O3）
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M7" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater7" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%; " />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图7
                    <label id="M94" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">（6）一氧化碳（CO）
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M8" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">
                    <asp:Repeater ID="Repeater8" runat="server">
                        <ItemTemplate>
                            <img src="<%#string.Format("{0}",GetDataItem())%>" style="width: 60%; " />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图8
                    <label id="M93" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">（7）环境空气质量排名
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M11" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图9
                    <label id="M91" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">（8）酸雨
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M12" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图10
                    <label id="M90" runat="server"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 22px; width: 100%">2. 现代化考核环境质量
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; width: 100%">（1）现代化空气质量
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M9" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">表1
                    <label id="M92" runat="server"></label>
                </td>
            </tr>
        </table>
        <table style="border-collapse: collapse; margin-left: 15%; margin-right: 5%">
            <tr style="text-align: center; border: solid #000 1px;">
                <td class="td2">时间</td>
                <td class="td2">地区名称</td>
                <td class="td1">全市</td>
                <td class="td1">张家港</td>
                <td class="td1">常熟</td>
                <td class="td1">太仓</td>
                <td class="td1">昆山</td>
                <td class="td1">吴江</td>
                <td class="td1">市区</td>
            </tr>
            <tr style="text-align: center; border: solid #000 1px;">
                <td class="td2" rowspan="2">
                    <label id="M30" runat="server" style="width: 150px"></label>
                </td>
                <td class="td2">达标天数(天)</td>
                <td class="td1">
                    <asp:TextBox ID="M31" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M32" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M33" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M34" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M35" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M36" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M37" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr style="text-align: center; border: solid #000 1px;">
                <td class="td2">达标天数比例</td>
                <td class="td1">
                    <asp:TextBox ID="M41" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M42" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M43" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M44" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M45" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M46" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M47" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr style="text-align: center; border: solid #000 1px;">
                <td class="td2" rowspan="2">
                    <label id="M50" runat="server" style="width: 150px"></label>
                </td>
                <td class="td2">达标天数(天)</td>
                <td class="td1">
                    <asp:TextBox ID="M51" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M52" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M53" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M54" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M55" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M56" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M57" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr style="text-align: center; border: solid #000 1px;">
                <td class="td2">达标天数比例</td>
                <td class="td1">
                    <asp:TextBox ID="M61" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M62" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M63" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M64" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M65" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M66" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="td1">
                    <asp:TextBox ID="M67" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table style="margin-left: 5%; margin-right: 10%; width: 100%; text-align: center">
            <tr>
                <td style="text-align: center; font-size: 26px; width: 100%">二、存在的主要问题
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M13" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 26px; width: 100%">三、对策与建议
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M14" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>

                </td>
            </tr>
        </table>
    </form>
</body>
</html>
