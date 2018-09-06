<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualittyMonthReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirQualittyMonthReport" %>

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
                <td style="text-align: center; width: 100px">报表年份：
                </td>
                <td class="content" style="width: 100px;">
                    <telerik:RadComboBox runat="server" ID="rdcbxYear" Width="90px" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" OnSelectedIndexChanged="Year_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                </td>
                <td style="text-align: center; width: 100px">报表月份：
                </td>
                <td class="content" style="width: 100px;">
                    <telerik:RadComboBox runat="server" ID="rdcbxMonth" Width="90px" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" OnSelectedIndexChanged="Year_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                </td>
                <td class="title" style="width: 100px">考核基数：
                </td>
                <td class="content" style="width: 100px;">
                    <telerik:RadComboBox runat="server" ID="rdcbxJiYear" Width="90px" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消">
                    </telerik:RadComboBox>
                </td>
                <td style="text-align: center; width: 100px">日期范围：</td>
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
        <table id="tbReport" runat="server" style="margin-left: 5%; margin-right: 10%; width: 100%; text-align: center">
            <tr>
                <td style="text-align: center; font-size: 26px; width: 100%">环境空气质量重点信息
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="txtImportant1" runat="server" MaxLength="500" Height="70px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt; font-weight: bold; color: #365FA1;" CssClass="ExportHtml"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="txtImportant2" runat="server" MaxLength="500" Height="70px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt; font-weight: bold; color: #365FA1;" CssClass="ExportHtml"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="txtImportant3" runat="server" MaxLength="500" Height="70px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt; font-weight: bold; color: #365FA1;" CssClass="ExportHtml"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 26px; width: 100%">苏州市环境空气PM<sub>2.5</sub>浓度和达标天数比例情况
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tbBiaoImportant" runat="server" style="border-collapse: collapse; margin-left: 10%; margin-right: 10%">
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2" rowspan="3" colspan="2" style="width: 150px">地区</td>
                            <td class="td2" colspan="3">PM<sub>2.5</sub>浓度</td>
                            <td class="td1" colspan="3">达标天数比例</td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2" style="width: 100px">
                                <asp:Label ID="lblPM25Last" runat="server"></asp:Label>
                            </td>
                            <td class="td2" style="width: 100px">
                                <asp:Label ID="lblPM25Now" runat="server"></asp:Label>
                            </td>
                            <td class="td1" style="width: 100px">
                                <asp:Label ID="lblPM25Ji" runat="server"></asp:Label>
                            </td>
                            <td class="td1" style="width: 100px">
                                <asp:Label ID="lblDaBiaoJi" runat="server"></asp:Label>
                            </td>
                            <td class="td1" style="width: 100px">
                                <asp:Label ID="lblDaBiaoNow" runat="server"></asp:Label>
                            </td>
                            <td class="td1" style="width: 200px">
                                <asp:Label ID="lblDaBiaoLast" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">
                                <asp:Label ID="lblMonthRange1" runat="server"></asp:Label>
                            </td>
                            <td class="td2">
                                <asp:Label ID="lblMonthRange2" runat="server"></asp:Label>
                            </td>
                            <td class="td1">同期比较</td>
                            <td class="td1">
                                <asp:Label ID="lblMonthRange3" runat="server"></asp:Label>
                            </td>
                            <td class="td1">
                                <asp:Label ID="lblMonthRange4" runat="server"></asp:Label>
                            </td>
                            <td class="td1">同期比较(百分点)</td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td rowspan="6">市区
                            </td>
                            <td class="td1" style="width: 100px">姑苏区</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25LastGuSu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25NowGuSu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25JiGuSu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoJiGuSu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoNowGuSu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastGuSu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td1">吴中区</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25LastWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25NowWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25JiWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoJiWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoNowWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td1">高新区</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25LastGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25NowGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25JiGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoJiGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoNowGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td1">工业园区</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25LastGongYeYuan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25NowGongYeYuan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25JiGongYeYuan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoJiGongYeYuan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoNowGongYeYuan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastGongYeYuan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td1">相城区</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25LastXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25NowXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25JiXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoJiXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoNowXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td1">市区均值</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25LastShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25NowShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25JiShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoJiShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoNowShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2" colspan="2">张家港市</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25LastZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25NowZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25JiZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoJiZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoNowZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2" colspan="2">常熟市</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25LastChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25NowChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25JiChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoJiChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoNowChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2" colspan="2">太仓市</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25LastTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25NowTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25JiTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoJiTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoNowTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2" colspan="2">昆山市</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25LastKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25NowKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25JiKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoJiKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoNowKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2" colspan="2">吴江区</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25LastWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25NowWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25JiWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoJiWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoNowWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2" colspan="2">全市均值</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25LastQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25NowQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25JiQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoJiQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoNowQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 26px; width: 100%">一、苏州市环境空气质量
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="txtM101" runat="server" MaxLength="500" Height="70px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt;" CssClass="ExportHtml"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">表1
                    <asp:Label ID="lblBiao1" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tbBiao1" runat="server" style="border-collapse: collapse; margin-left: 10%; margin-right: 10%">
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2" rowspan="2">城市</td>
                            <td class="td2" rowspan="2">达标天数</td>
                            <td class="td1" rowspan="2">达标天数比例（%）</td>
                            <td class="td1" rowspan="2">
                                <asp:Label ID="lblBiao1LastMonthRate" runat="server"></asp:Label>
                            </td>
                            <td class="td1" colspan="6">超标天数</td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">PM<sub>2.5</sub></td>
                            <td class="td2">PM<sub>10</sub></td>
                            <td class="td1">NO<sub>2</sub></td>
                            <td class="td1">SO<sub>2</sub></td>
                            <td class="td2">CO</td>
                            <td class="td2">O<sub>3</sub>-8小时</td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">市区
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">张家港
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">常熟
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">太仓
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">昆山
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">吴江
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">全市
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%;">
                    <asp:TextBox ID="txtM104" runat="server" MaxLength="500" Height="30px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>

                <td style="text-align: left; font-size: 16px; width: 10%">
                    <asp:Image ID="imgTu1" runat="server" />
                    <br />
                    图1
                    <asp:Label runat="server" ID="lblTu1"></asp:Label>
                </td>
                <td style="text-align: left; font-size: 16px; width: 10%">
                    <asp:Image ID="imgTu2" runat="server" />
                    <br />
                    图2
                    <asp:Label runat="server" ID="lblTu2"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="txtM102" runat="server" MaxLength="500" Height="70px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt;" CssClass="ExportHtml"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="margin-left: 5px; width: 100%; font-family: 仿宋_GB2312; font-size: 14pt;">
                    <asp:TextBox ID="txtM103" runat="server" MaxLength="500" Height="70px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="margin-left: 5px; width: 100%; font-family: 仿宋_GB2312; font-size: 14pt;">
                    <asp:TextBox ID="txtM106" runat="server" MaxLength="500" Height="70px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">表2
                    <asp:Label ID="lblBiao2" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tbBiao2" runat="server" style="border-collapse: collapse; margin-left: 10%; margin-right: 10%">
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2" rowspan="2">城市</td>
                            <td class="td2" rowspan="2">达标天数</td>
                            <td class="td1" rowspan="2">达标天数比例（%）</td>
                            <td class="td1" rowspan="2">
                                <asp:Label ID="lblBiao2LastMonthRate" runat="server"></asp:Label>
                            </td>
                            <td class="td1" colspan="6">超标天数</td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">PM<sub>2.5</sub></td>
                            <td class="td2">PM<sub>10</sub></td>
                            <td class="td1">NO<sub>2</sub></td>
                            <td class="td1">SO<sub>2</sub></td>
                            <td class="td2">CO</td>
                            <td class="td2">O<sub>3</sub>-8小时</td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">南门
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysNanMen" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateNanMen" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateNanMen" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysNanMen" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysNanMen" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysNanMen" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysNanMen" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysNanMen" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysNanMen" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">彩香
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysCaiXiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateCaiXiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateCaiXiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysCaiXiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysCaiXiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysCaiXiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysCaiXiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysCaiXiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysCaiXiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">轧钢厂
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysZhaGangChang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateZhaGangChang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateZhaGangChang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysZhaGangChang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysZhaGangChang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysZhaGangChang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysZhaGangChang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysZhaGangChang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysZhaGangChang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">吴中区
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysWuZhong" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">高新区
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysGaoXin" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">工业园区
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysYuanQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateYuanQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateYuanQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysYuanQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysYuanQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysYuanQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysYuanQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysYuanQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysYuanQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">相城区
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoDaysXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoRateXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtDaBiaoLastRateXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChaoDaysXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChaoDaysXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChaoDaysXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChaoDaysXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChaoDaysXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChaoDaysXiangCheng" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="margin-left: 5px; width: 100%; font-family: 仿宋_GB2312; font-size: 14pt;">
                    <asp:TextBox ID="txtM105" runat="server" MaxLength="500" Height="70px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 26px; width: 100%">二、主要污染物状况
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="txtM201" runat="server" MaxLength="500" Height="70px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt;" CssClass="ExportHtml"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">表2
                    <asp:Label ID="lblBiao3" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tbBiao3" runat="server" style="border-collapse: collapse; margin-left: 15%; margin-right: 5%">
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2" rowspan="2">城市</td>
                            <td class="td1">PM<sub>2.5</sub></td>
                            <td class="td1">PM<sub>10</sub></td>
                            <td class="td1">NO<sub>2</sub></td>
                            <td class="td1">SO<sub>2</sub></td>
                            <td class="td1">CO</td>
                            <td class="td1">O<sub>3</sub>-8小时</td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td1">微克/立方米</td>
                            <td class="td1">微克/立方米</td>
                            <td class="td1">微克/立方米</td>
                            <td class="td1">微克/立方米</td>
                            <td class="td1">毫克/立方米</td>
                            <td class="td1">微克/立方米</td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">市区</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">张家港</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">常熟</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25ChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10ChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2ChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2ChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38ChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">太仓</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25TaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10TaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2TaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2TaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38TaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">昆山</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25KunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10KunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2KunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2KunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38KunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">吴江</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25WuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10WuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2WuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2WuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38WuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">全市</td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM25QuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPM10QuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtNO2QuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtSO2QuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtCOQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtO38QuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="txtM202" runat="server" MaxLength="500" Height="70px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt;" CssClass="ExportHtml"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图3
                    <asp:Label runat="server" ID="lblTu3"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图4
                    <asp:Label runat="server" ID="lblTu4"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="txtM203" runat="server" MaxLength="500" Height="70px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt;" CssClass="ExportHtml"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图5
                    <asp:Label runat="server" ID="lblTu5"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图6
                    <asp:Label runat="server" ID="lblTu6"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图7
                    <asp:Label runat="server" ID="lblTu7"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="txtM204" runat="server" MaxLength="500" Height="150px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt;" CssClass="ExportHtml"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 26px; width: 100%">三、酸雨状况
                </td>
            </tr>
            <tr>
                <td style="margin-left: 5px; width: 100%; font-family: 仿宋_GB2312; font-size: 14pt;">
                    <asp:TextBox ID="txtM301" runat="server" MaxLength="500" Height="70px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">表3
                    <asp:Label ID="lblBiao4" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tbBiao4" runat="server" style="border-collapse: collapse; margin-left: 15%; margin-right: 5%">
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">城市</td>
                            <td class="td1">样品数(个)</td>
                            <td class="td1">pH最小值</td>
                            <td class="td1">pH最大值</td>
                            <td class="td1">降水pH均值</td>
                            <td class="td1">酸雨频率(%)</td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">市区</td>
                            <td class="td1">
                                <asp:TextBox ID="txtYangPinShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMinShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMaxShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtAvgShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPinLvShiQu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">张家港</td>
                            <td class="td1">
                                <asp:TextBox ID="txtYangPinZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMinZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMaxZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtAvgZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPinLvZhangJiaGang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">常熟</td>
                            <td class="td1">
                                <asp:TextBox ID="txtYangPinChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMinChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMaxChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtAvgChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPinLvChangShu" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">太仓</td>
                            <td class="td1">
                                <asp:TextBox ID="txtYangPinTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMinTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMaxTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtAvgTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPinLvTaiCang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">昆山</td>
                            <td class="td1">
                                <asp:TextBox ID="txtYangPinKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMinKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMaxKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtAvgKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPinLvKunShan" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">吴江</td>
                            <td class="td1">
                                <asp:TextBox ID="txtYangPinWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMinWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMaxWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtAvgWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPinLvWuJiang" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="text-align: center; border: solid #000 1px;">
                            <td class="td2">全市</td>
                            <td class="td1">
                                <asp:TextBox ID="txtYangPinQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMinQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtMaxQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtAvgQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="td1">
                                <asp:TextBox ID="txtPinLvQuanShi" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图8
                    <asp:Label runat="server" ID="lblTu8"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 26px; width: 100%">四、空气质量气象条件分析
                </td>
            </tr>
            <tr>
                <td style="margin-left: 5px; width: 100%; font-family: 仿宋_GB2312; font-size: 14pt;">
                    <asp:TextBox ID="txtM401" runat="server" MaxLength="500" Height="90px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt;"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图9
                    <asp:Label runat="server" ID="lblTu9"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="margin-left: 5px; width: 100%; font-family: 仿宋_GB2312; font-size: 14pt;">
                    <asp:TextBox ID="txtM402" runat="server" MaxLength="500" Height="150px" Width="80%" TextMode="MultiLine"
                        Style="font-family: 仿宋_GB2312; font-size: 14pt;"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 16px; width: 100%">图10
                    <asp:Label runat="server" ID="lblTu10"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblDate" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
