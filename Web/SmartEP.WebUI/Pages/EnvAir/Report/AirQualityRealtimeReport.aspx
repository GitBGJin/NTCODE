<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualityRealtimeReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirQualityRealtimeReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title></title>
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

                //控制导出时按钮不会隐藏掉处理
                function onRequestStart(sender, args) {
                    if (args.EventArgument == 6 || args.EventArgument == 7 ||
                        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                        args.set_enableAjax(false);
                    }
                }

                function OnClientClicking() {
                    var date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    var date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        //sender.set_autoPostBack(false);
                        //window.event.returnValue = false;
                        return false;
                    }
                    else if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    }
                    else {
                        return true;
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

                window.onload = function () {
                    document.getElementById("<%= dtpBegin.ClientID %>").onchange = CheckDateIsEmpty;
                    document.getElementById("<%= dtpEnd.ClientID %>").onchange = CheckDateIsEmpty;
                }

                function CheckDateIsEmpty() {
                    //var dtpBeginfind = $find("<%= dtpBegin.ClientID %>");
                    //var dtpEndfind = $find("<%= dtpEnd.ClientID %>");
                    //var dateBegin = dtpBeginfind.get_selectedDate();
                    //var dateEnd = dtpEndfind.get_selectedDate();
                    //var newDate = new Date();
                    //var isAlert = false;
                    //if (dateBegin == null) {
                    //    if (dateEnd != null && newDate > dateEnd) {
                    //        newDate = dateEnd;
                    //    }
                    //    dtpBeginfind.set_selectedDate(new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate(), newDate.getHours()));
                    //    isAlert = true;
                    //}
                    //if (dateEnd == null) {
                    //    if (dateBegin != null && newDate < dateBegin) {
                    //        newDate = dateBegin;
                    //    }
                    //    dtpEndfind.set_selectedDate(new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate(), newDate.getHours()));
                    //    isAlert = true;
                    //}
                    //if (isAlert) {
                    //    alert('开始时间或者终止时间，不能为空！');
                    //}
                }

                //按钮行处理
                function gridRTB_ClientButtonClicking(sender, args) {
                    var masterTable = $find("<%= gridRealTimeAQI.ClientID %>").get_masterTableView();
                    var CurrentBtn = args.get_item();
                    var CurrentBtnName = CurrentBtn.get_text();
                    var CurrentBtnCommandName = CurrentBtn.get_commandName();
                    switch (CurrentBtnCommandName) {
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
                    args.set_cancel(!OnClientClicking());
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
                <telerik:AjaxSetting AjaxControlID="gridRealTimeAQI">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeAQI" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <telerik:AjaxUpdatedControl ControlID="dvPoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCityProper" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCity" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCityModel" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 120px; text-align: center;">查询范围:
                        </td>
                        <td class="content" style="width: 320px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged" RepeatLayout="Flow" RepeatColumns="5">
                                <asp:ListItem Text="区域" Value="City" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="测点" Value="Port"></asp:ListItem>

                                <%--            <asp:ListItem Text="城市均值（创模点）" Value="CityModel"></asp:ListItem>--%>
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">查询区域/测点:
                        </td>
                        <td class="content" style="width: 300px;">
                            <div runat="server" id="dvCity">
                                <telerik:RadComboBox runat="server" ID="comboCity" Width="180px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                                    Localization-CheckAllString="选择全部">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="市辖区" Value="b6e983c4-4f92-4be3-bbac-d9b71c470640" Checked="true" />
                                        <telerik:RadComboBoxItem Text="崇川区" Value="be0839ef-2059-437a-aeb3-84586cdc24c6" Checked="true" />
                                        <telerik:RadComboBoxItem Text="港闸区" Value="c1008828-0413-4762-9976-87134de58453" Checked="true" />
                                        <telerik:RadComboBoxItem Text="开发区" Value="62cf6841-187e-4ec2-a139-90aef3547d06" Checked="true" />
                                        <telerik:RadComboBoxItem Text="海安县" Value="6e66a670-f866-4bb9-abe1-317702d333dc" Checked="true" />
                                        <telerik:RadComboBoxItem Text="海门市" Value="0ea68e7c-5f43-4d59-b913-d8392a8457a4" Checked="true" />
                                        <telerik:RadComboBoxItem Text="启东市" Value="6305492a-7145-4f54-b88d-031be0d62282" Checked="true" />
                                        <telerik:RadComboBoxItem Text="如东市" Value="afc66edb-0a2a-493c-b340-d84249cd9afd" Checked="true" />
                                        <telerik:RadComboBoxItem Text="如皋市" Value="52d96e0a-728a-4973-bbf5-1b33367687fd" Checked="true" />
                                        <telerik:RadComboBoxItem Text="通州区" Value="fcc5ab39-41cc-4329-ac72-248ccbcd8b29" Checked="true" />
                                        <telerik:RadComboBoxItem Text="通州湾示范区" Value="9ac74b62-25ae-4b32-9ea9-14baed98f2b2" Checked="true" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                            <div runat="server" id="dvPoint" style="display: none">
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="220" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                            </div>
                        </td>
                        <td></td>
                        <td class="title" style="width: 80px" rowspan="2">排序:
                        </td>
                        <td class="content" align="left" style="width: 100px;" rowspan="2">
                            <telerik:RadComboBox runat="server" ID="TimeSort" Width="90px">
                                <Items>
                                    <telerik:RadComboBoxItem Text="时间降序" Value="时间降序" Selected="true" />
                                    <telerik:RadComboBoxItem Text="时间升序" Value="时间升序" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
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
                        <td class="title" style="text-align: center;">结束时间:</td>
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
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                        </CommandItemTemplate>
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="污染物浓度及空气质量分指数（IAQI）" HeaderText="污染物浓度及空气质量分指数（IAQI）"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="PM2.5" HeaderText="PM<sub>2.5</sub>1小时平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="PM10" HeaderText="PM<sub>10</sub>1小时平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="NO2" HeaderText="二氧化氮(NO<sub>2</sub>)1小时平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="SO2" HeaderText="二氧化硫(SO<sub>2</sub>)1小时平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="CO" HeaderText="一氧化碳(CO)1小时平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="O31" HeaderText="臭氧(O<sub>3</sub>)1小时平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <%--<telerik:GridColumnGroup Name="O38" HeaderText="臭氧(O<sub>3</sub>)8小时滑动平均"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>--%>
                            <telerik:GridColumnGroup Name="空气质量指数类别" HeaderText="空气质量指数类别"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="监测点位名称" UniqueName="PointId" DataField="PointId" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                            <telerik:GridDateTimeColumn HeaderText="日期" UniqueName="DateTime" DataField="DateTime" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="PM25" DataField="PM25" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="PM25_IAQI" DataField="PM25_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="PM10" DataField="PM10" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="PM10_IAQI" DataField="PM10_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="NO2" DataField="NO2" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="NO2_IAQI" DataField="NO2_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="SO2" DataField="SO2" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="SO2_IAQI" DataField="SO2_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(mg/m<sup>3</sup>)" UniqueName="CO" DataField="CO" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" HeaderStyle-Width="93px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="CO_IAQI" DataField="CO_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="浓度/(μg/m<sup>3</sup>)" UniqueName="O3" DataField="O3" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O31" HeaderStyle-Width="90px" />
                            <telerik:GridBoundColumn HeaderText="分指数" UniqueName="O3_IAQI" DataField="O3_IAQI" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O31" HeaderStyle-Width="60px" />
                            <telerik:GridBoundColumn HeaderText="空气质量<br />指数(AQI)" UniqueName="AQIValue" DataField="AQIValue" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                            <telerik:GridBoundColumn HeaderText="首要污染物" UniqueName="PrimaryPollutant" DataField="PrimaryPollutant" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                            <telerik:GridBoundColumn HeaderText="空气质量<br />指数级别" UniqueName="Grade" DataField="Grade" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                            <telerik:GridBoundColumn HeaderText="类别" UniqueName="Class" DataField="Class" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="空气质量指数类别" HeaderStyle-Width="65px" />
                            <telerik:GridBoundColumn HeaderText="颜色" UniqueName="RGBValue" DataField="RGBValue" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="空气质量指数类别" HeaderStyle-Width="65px" />
                        </Columns>
                        <HeaderStyle Font-Bold="false" />
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="0"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>

