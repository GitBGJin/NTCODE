<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RealTimeAirQuality.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.RealTimeData.RealTimeAirQuality" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title></title>
    <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript" language="javascript">
                var myRadGrid;
                function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

                //页面刷新
                function Refresh_Grid(args) {
                    if (args) {
                        var MasterTable = $find("<%= gridRealTimeAQI.ClientID %>").get_masterTableView();
                        MasterTable.rebind();
                    }
                }

                function OnClientClicking() {
                    return true;
                }

                //控制导出时按钮不会隐藏掉处理
                function onRequestStart(sender, args) {
                    if (args.EventArgument == 6 || args.EventArgument == 7 ||
                        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                        args.set_enableAjax(false);
                    }
                }

                //按钮行处理
                function gridRTB_ClientButtonClicking(sender, args) {
                    var masterTable = $find("<%= gridRealTimeAQI.ClientID %>").get_masterTableView();
                    var CurrentBtn = args.get_item();
                    var CurrentBtnName = CurrentBtn.get_text();
                    var CurrentBtnCommandName = CurrentBtn.get_commandName();
                    switch (CurrentBtnCommandName) {
                        case "InitInsert":
                            {
                                //增加
                                window.radopen("ConfigOfflineAdd.aspx", "ConfigOfflineDialog");
                                args.set_cancel(true);
                                break;
                            }
                        case "DeleteSelected":
                            try {
                                //删除
                                var selItems = masterTable.get_selectedItems();
                                if (selItems.length <= 0) { alert("请选择要删除的记录！") }
                                else
                                {
                                    args.set_cancel(!confirm('确定删除所有选中的记录？'));
                                }
                            } catch (e) { }
                            break;
                        case "RebindGrid":
                            masterTable.rebind();
                            break;
                        default:
                            break;
                    }
                }

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
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="comboRegion">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="comboRegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboPoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="comboPoint">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="comboPoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRealTimeAQI">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeAQI" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeAQI" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeAQI" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="RadCbxPort" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="RadCbxCityProper" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="RadCbxCity" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="RadCbxCityModel" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="30px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <%--<td class="title" style="width: 60px; text-align: center;">区域:
                        </td>
                        <td class="content" style="width: 180px;">
                            <telerik:RadComboBox runat="server" ID="comboRegion" Width="180px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                CheckedItemsTexts="DisplayAllInInput" AutoPostBack="true" OnSelectedIndexChanged="comboRegion_SelectedIndexChanged"
                                Localization-CheckAllString="选择全部">
                            </telerik:RadComboBox>
                        </td>--%>
                        <td class="title" style="width: 60px; text-align: center;">测点:
                        </td>
                        <td class="content" style="width: 300px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="300" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                            <%--<telerik:RadComboBox runat="server" ID="comboPoint" Width="280px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                CheckedItemsTexts="DisplayAllInInput" AutoPostBack="true" Localization-CheckAllString="选择全部">
                            </telerik:RadComboBox>--%>
                        </td>
                        <td class="content" align="center">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="title" style="width: 120px; text-align: center;">开始时间:
                        </td>
                        <td class="content">
                            <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                        <td class="title" style="text-align: center;">截止时间:</td>
                        <td class="content">
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridRealTimeAQI" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                        </CommandItemTemplate>
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="污染物浓度及空气质量分指数（IAQI）" HeaderText="污染物浓度及空气质量分指数（IAQI）"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="SO2" HeaderText="二氧化硫<br />(SO<sub>2</sub>)<br />24小时滑动平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="NO2" HeaderText="二氧化氮<br />(NO<sub>2</sub>)<br />24小时滑动平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="PM10" HeaderText="PM<sub>10</sub><br />24小时滑动平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="CO" HeaderText="一氧化碳<br />(CO)<br />24小时滑动平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="O31" HeaderText="臭氧(O<sub>3</sub>)<br />1小时平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="O38" HeaderText="臭氧(O<sub>3</sub>)<br />8小时滑动平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="PM2.5" HeaderText="PM<sub>2.5</sub><br />24小时滑动平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="空气质量指数类别" HeaderText="空气<br />质量<br />指数<br />类别"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="监测点位名称" UniqueName="PointId" DataField="PointId" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridDateTimeColumn HeaderText="日期" UniqueName="DateTime" DataField="DateTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="PM<sub>2.5</sub><br />(μg/m<sup>3</sup>)" UniqueName="PM25" DataField="PM25" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="PM<sub>10</sub><br />(μg/m<sup>3</sup>)" UniqueName="PM10" DataField="PM10" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="二氧化氮<br />(μg/m<sup>3</sup>)" UniqueName="NO2" DataField="NO2" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="二氧化硫<br />(μg/m<sup>3</sup>)" UniqueName="SO2" DataField="SO2" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="一氧化碳<br />(mg/m<sup>3</sup>)" UniqueName="CO" DataField="CO" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="臭氧<br />(μg/m<sup>3</sup>)" UniqueName="O3" DataField="O3" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="AQI" UniqueName="AQIValue" DataField="AQIValue" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="首要污染物" UniqueName="PrimaryPollutant" DataField="PrimaryPollutant" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="类别" UniqueName="Class" DataField="Class" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
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
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
