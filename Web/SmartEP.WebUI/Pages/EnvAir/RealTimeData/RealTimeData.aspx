<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RealTimeData.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.RealTimeData.RealTimeData" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
        <script src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
        <script type="text/javascript">
            var isfirst = 1;//是否首次加载
            function onRequestStart(sender, args) {
                if (args.EventArgument == "")
                    return;
                if (args.EventArgument == 0 || args.EventArgument == 1 ||
                    args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                        args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                        args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                    args.set_enableAjax(false);
                }
            }
            //Chart图形切换
            function ChartTypeChanged(item) {
                try {
                    var chartIframe = document.getElementsByName('chartIframe');
                    //var item = args.get_item().get_value();
                    for (var i = 0; i < chartIframe.length; i++) {
                        document.getElementById(chartIframe[i].id).contentWindow.HighChartTypeChange(item);
                    }
                } catch (e) {
                }
            }

            //查看图表
            function TabSelected(sender, args) {
                try {
                    var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                    var text = $.trim(args._tab._element.innerText);
                    if (text == '图表') {
                        if (isfirst.value == "1") {
                            isfirst.value = 0;
                            RefreshChart();
                        }
                    }
                } catch (e) {
                    //InitTogetherChart();
                }
            }

            //刷新图表
            function RefreshChart() {
                try {
                    //var height = document.body.clientHeight - 112 - 40;
                    //togetherChart("../Chart/ChartFrame.aspx", height);

                    var hiddenData = $("#HiddenData").val().split('|');
                    var height = parseInt(parseInt($("#pvChart").css("height")) - 40);
                    groupChartByPointid(hiddenData[0], "../Chart/ChartFrame.aspx", height);//以站点分组
                } catch (e) {
                }
                //try {
                //    var chartPage = document.getElementById("pvChart");
                //    chartPage.children[0].contentWindow.InitChart();
                //} catch (e) {
                //}
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <asp:HiddenField ID="FirstLoadChart" Value="1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRT">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRT" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRT" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRT" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <%--       <telerik:AjaxUpdatedControl ControlID="dvTotala" />
                        <telerik:AjaxUpdatedControl ControlID="dvTotalb" />--%>
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvTotala" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvTotalb" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChartType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartType" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
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
                        <td class="title" style="width: 100px">监测因子:
                        </td>
                        <td class="content" style="width: 370px;">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" DefaultAllSelected="true" CbxWidth="360" DropDownWidth="420" ID="factorCbxRsm"></CbxRsm:FactorCbxRsm>
                        </td>
                        <td class="title" style="width: 80px">排序:
                        </td>
                        <td class="content" align="left" style="width: 100px;">
                            <telerik:RadComboBox runat="server" ID="TimeSort" Width="90px">
                                <Items>
                                    <telerik:RadComboBoxItem Text="时间降序" Value="时间降序" Selected="true" />
                                    <telerik:RadComboBoxItem Text="时间升序" Value="时间升序" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>

                        <td class="content" align="right" rowspan="2" style="width: 70px;">
                            <asp:CheckBox ID="IsStatistical" Checked="false" Text="统计行" runat="server" Visible="false" />
                        </td>
                        <td class="content" rowspan="2" align="left">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">数据类型:
                        </td>
                        <td class="content" style="width: 200px;">
                            <asp:RadioButtonList ID="radlDataType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>
                        <%--   <td class="title" style="width: 100px; text-align: center;">测点数据数：
                        </td>
                        <td class="content" style="width: 200px;">
                            <div id="dvTotala" runat="server">
                                <telerik:RadDropDownList runat="server" ID="ddlTotala" OnSelectedIndexChanged="ddlTotala_SelectedIndexChanged">
                                </telerik:RadDropDownList>
                            </div>
                            <div id="dvTotalb" runat="server">
                                <telerik:RadDropDownList runat="server" ID="ddlTotalb" OnSelectedIndexChanged="ddlTotalb_SelectedIndexChanged">
                                </telerik:RadDropDownList>
                            </div>
                        </td>--%>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneTab" runat="server" Scrolling="None" Width="100%" Height="26px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadTabStrip ID="tabStrip" runat="server" SelectedIndex="0" MultiPageID="multiPage"
                    CssClass="RadTabStrip_Customer" OnClientTabSelected="TabSelected">
                    <Tabs>
                        <telerik:RadTab Text="列表">
                        </telerik:RadTab>
                        <telerik:RadTab Text="图表">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pvGrid" runat="server" Visible="true">
                        <telerik:RadGrid ID="gridRT" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="true" PageSize="50" AllowCustomPaging="true" AllowSorting="false" ShowFooter="true"
                            AutoGenerateColumns="true" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="gridRT_NeedDataSource" OnItemDataBound="gridRT_ItemDataBound" OnColumnCreated="gridRT_ColumnCreated"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                <CommandItemTemplate>
                                    <div style="width: 100%; position: relative;">
                                        <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                            runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                                        <%-- <div style="position: absolute; right: 10px; top: 5px;">
                                            <table style="font-size: 12px">
                                                <tr>
                                                    <td>超上限（HSp）</td>
                                                    <td>超下限（LSp）</td>
                                                    <td>突变（T）</td>
                                                    <td>离群（Out）</td>
                                                    <td>负值（Neg）</td>
                                                    <td>重复（Rep）</td>
                                                </tr>
                                            </table>
                                        </div>--%>
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Container.DataSetIndex + 1%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                    PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                            </MasterTableView>
                            <CommandItemStyle Width="100%" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                                    SaveScrollPosition="true"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                        <div style="padding-top: 6px;">
                            <div style="float: right;">
                                <asp:RadioButtonList runat="server" ID="ChartType" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="3" OnSelectedIndexChanged="ChartType_SelectedIndexChanged">
                                    <asp:ListItem Text="折线图" Value="spline" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="柱形图" Value="column"></asp:ListItem>
                                    <asp:ListItem Text="点图" Value="scatter"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="clear: both">
                            </div>
                        </div>
                        <div id="ChartContainer">
                        </div>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/RealTimeData.ashx" />
        <asp:HiddenField ID="HiddenChartType" runat="server" Value="spline" />
    </form>
</body>
</html>
