<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="24HSinglePollutantDataAnalyzeNew.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Dock._24HSinglePollutantDataAnalyzeNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
            <script type="text/javascript">
                //图表刷新
                function RefreshChart() {
                    try {
                        var chartPage = document.getElementById("pvChart");
                        chartPage.children[0].contentWindow.InitChart();
                    } catch (e) {
                    }
                }
            </script>
            <script type="text/javascript">
                function GetData() {
                    var obj = new Object();
                    obj.PointNames = document.getElementById("<%=hdPointNames.ClientID%>").value;
                    obj.FactorName = document.getElementById("<%=hdFactorName.ClientID%>").value;
                    return obj;
                }
            </script>
        </telerik:RadScriptBlock>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rcbPoint">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rcbPoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rcbFactors" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointNames" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rcbFactors">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rcbFactors" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdFactorName" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdFactorName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointNames" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="multiPage" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0" BorderSize="0" BorderStyle="None"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="35px" Width="100%" Scrolling="None" MaxHeight="100"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%; width: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 50px; text-align: center;">测点:
                        </td>
                        <td class="content" style="width: 30%; text-align: left">
                            <%--<CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="250" CbxHeight="350" MultiSelected="true" DropDownWidth="400" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>--%>
                            <telerik:RadComboBox runat="server" OnSelectedIndexChanged="rcbPoint_SelectedIndexChanged" ID="rcbPoint" Localization-CheckAllString="全选" Width="250px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                            </telerik:RadComboBox>
                        </td>
                        <td class="title" style="width: 90px; text-align: center;">监测因子:
                        </td>
                        <td class="content" style="width: 30%; text-align: left">
                            <telerik:RadDropDownList runat="server" OnSelectedIndexChanged="rcbFactors_SelectedIndexChanged" ID="rcbFactors" Width="250px">
                            </telerik:RadDropDownList>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                        <%--  <td class="content" >
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>--%>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneImage" runat="server" Width="100%" Height="80%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pvChart" runat="server" ContentUrl="~/Pages/EnvAir/Chart/ChartFrame.aspx">
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--<asp:HiddenField ID="hdpointiddata" runat="server" />--%>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/24HSinglePollutantDataAnalyzeNew.ashx" />
        <asp:HiddenField ID="hdPointNames" runat="server" />
        <asp:HiddenField ID="hdFactorName" runat="server" />
        <asp:HiddenField ID="hdMaxValue" runat="server" />
    </form>
</body>
</html>
