<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PMCity3YearSum.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.PMCity3YearSum" %>

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
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function OnClientClicking() {
                    var date1 = $find("<%= dateBegin.ClientID %>").get_selectedDate();
                    var date2 = $find("<%= dateEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！"); 
                        return false;
                    }

                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
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

            </script>
        </telerik:RadCodeBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>

                <telerik:AjaxSetting AjaxControlID="gridPM3Year">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridPM3Year" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridPM3Year" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridPM3Year" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="comboPort" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCityProper" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCity" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCityModel" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddl_year">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="dateBegin" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dateEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="dateEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="dateBegin" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dateEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <td class="title" style="width: 80px">基准年:
                        </td>
                        <td class="content" style="width: 400px;">
                            <telerik:RadTextBox ID="StandarYear" runat="server" ReadOnly="true" Text="2013"></telerik:RadTextBox>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">时间:
                        </td>
                        <td class="content" style="width: 500px;">
                            <asp:DropDownList ID="ddl_year" runat="server" OnSelectedIndexChanged="ddl_year_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                            &nbsp;&nbsp;
                            <telerik:RadDatePicker ID="dateBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Enabled="false"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Width="100" />
                            &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDatePicker ID="dateEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" 
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Width="100" OnSelectedDateChanged="dateEnd_SelectedDateChanged" AutoPostBack="true"/>
                        </td>
                        <td class="content" align="left" style="width: 70px;" rowspan="2">
                            <asp:CheckBox ID="IsStatistical" Checked="false" Text="统计行" runat="server" />
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 120px; text-align: center;">查询类型:
                        </td>
                        <td class="content" style="width: 400px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>

                        <td class="title" style="width: 120px; text-align: center;">查询点位（区域）:
                        </td>
                        <td class="content" style="width: 300px;">
                            <telerik:RadComboBox runat="server" ID="comboPort" Width="300px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="南门" Value="28" Checked="true" />
                                    <telerik:RadComboBoxItem Text="彩香" Value="29" Checked="true" />
                                    <telerik:RadComboBoxItem Text="轧钢厂" Value="31" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴中区" Value="34" Checked="true" />
                                    <telerik:RadComboBoxItem Text="高新区" Value="35" Checked="true" />
                                    <telerik:RadComboBoxItem Text="工业园区" Value="88" Checked="true" />
                                    <telerik:RadComboBoxItem Text="相城区" Value="101" Checked="true" />
                                    <telerik:RadComboBoxItem Text="上方山" Value="103" Checked="true" />
                                    <telerik:RadComboBoxItem Text="兴福子站" Value="110" Checked="true" />
                                    <telerik:RadComboBoxItem Text="菱塘子站" Value="109" Checked="true" />
                                    <telerik:RadComboBoxItem Text="海虞子站" Value="108" Checked="true" />
                                    <telerik:RadComboBoxItem Text="张家港市监测站" Value="116" Checked="true" />
                                    <telerik:RadComboBoxItem Text="城北小学" ToolTip="" Value="117" Checked="true" />
                                    <telerik:RadComboBoxItem Text="昆山第二中学" Value="114" Checked="true" />
                                    <telerik:RadComboBoxItem Text="震川中学" Value="115" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴江环保局" Value="105" Checked="true" />
                                    <telerik:RadComboBoxItem Text="教师进修学校" Value="106" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴江开发区" Value="107" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓监测站" Value="111" Checked="true" />
                                    <telerik:RadComboBoxItem Text="科教新城实小" Value="112" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadComboBox runat="server" ID="comboCityProper" Width="300px" CheckBoxes="true" Visible="false" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="姑苏区" Value="6a4e7093-f2c6-46b4-a11f-0f91b4adf379" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴中区" Value="e1c104f3-aaf3-4d0e-9591-36cdc83be15a" Checked="true" />
                                    <telerik:RadComboBoxItem Text="高新区" Value="f320aa73-7c55-45d4-a363-e21408e0aac3" Checked="true" />
                                    <telerik:RadComboBoxItem Text="工业园区" Value="69a993ff-78c6-459b-9322-ee77e0c8cd68" Checked="true" />
                                    <telerik:RadComboBoxItem Text="相城区" ToolTip="" Value="8756bd44-ff18-46f7-aedf-615006d7474c" Checked="true" />
                                    <telerik:RadComboBoxItem Text="市区合计" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadComboBox runat="server" ID="comboCity" Width="300px" CheckBoxes="true" Visible="false" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="苏州市区" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Checked="true" />
                                    <telerik:RadComboBoxItem Text="张家港" Value="4296ce53-78d3-4741-9eda-6306e3e5b399" Checked="true" />
                                    <telerik:RadComboBoxItem Text="常熟" Value="f7444783-a425-411c-a54b-f9fed72ec72e" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓" Value="d993d02f-fcc3-4ea6-b52b-9414fbd9b8e6" Checked="true" />
                                    <telerik:RadComboBoxItem Text="昆山" ToolTip="" Value="636775d8-091d-4754-9ed2-cd9dfef1f6ab" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴江" Value="48d749e6-d07c-4764-8d50-50f170defe0b" Checked="true" />
                                    <telerik:RadComboBoxItem Text="全市合计" Value="00000000000000000000000000000000000000000000000000" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadComboBox runat="server" ID="comboCityModel" Width="300px" CheckBoxes="true" Visible="false" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="苏州市区" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Checked="true" />
                                    <telerik:RadComboBoxItem Text="张家港" Value="4296ce53-78d3-4741-9eda-6306e3e5b399" Checked="true" />
                                    <telerik:RadComboBoxItem Text="常熟" Value="f7444783-a425-411c-a54b-f9fed72ec72e" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓" Value="d993d02f-fcc3-4ea6-b52b-9414fbd9b8e6" Checked="true" />
                                    <telerik:RadComboBoxItem Text="昆山" ToolTip="" Value="636775d8-091d-4754-9ed2-cd9dfef1f6ab" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴江" Value="48d749e6-d07c-4764-8d50-50f170defe0b" Checked="true" />
                                    <telerik:RadComboBoxItem Text="全市合计" Value="00000000000000000000000000000000000000000000000000" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pvGrid" runat="server" Visible="true">
                        <telerik:RadGrid ID="gridPM3Year" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="true"
                            AutoGenerateColumns="False" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="gridPM3Year_NeedDataSource" OnItemDataBound="gridPM3Year_ItemDataBound"
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
                                    <telerik:GridColumnGroup Name="PM10First" HeaderText="PM10 24小时平均值"
                                        HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="PM2.5First" HeaderText="PM2.5 24小时平均值"
                                        HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="PM10Second" HeaderText="PM10 24小时平均值"
                                        HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="PM2.5Second" HeaderText="PM2.5 24小时平均值"
                                        HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="PM10Thidly" HeaderText="PM10 24小时平均值"
                                        HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="PM2.5Thidly" HeaderText="PM2.5 24小时平均值"
                                        HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Container.DataSetIndex + 1%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn HeaderText="监测点位名称" UniqueName="PortName" DataField="PortName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                    <telerik:GridBoundColumn HeaderText="日期" UniqueName="Time" DataField="Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn HeaderText="2013" UniqueName="PM102013" DataField="PM102013" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10First" />
                                    <telerik:GridBoundColumn HeaderText="2013" UniqueName="PM2.52013" DataField="PM2.52013" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5First" />
                                    <telerik:GridBoundColumn HeaderText="2014" UniqueName="PM102014" DataField="PM102014" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10Second" />
                                    <telerik:GridBoundColumn HeaderText="2014" UniqueName="PM2.52014" DataField="PM2.52014" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5Second" />
                                    <telerik:GridBoundColumn HeaderText="2015" UniqueName="PM102015" DataField="PM102015" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10Thidly" />
                                    <telerik:GridBoundColumn HeaderText="2015" UniqueName="PM2.52015" DataField="PM2.52015" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5Thidly" />
                                </Columns>
                                <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                    PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                            </MasterTableView>
                            <CommandItemStyle Width="100%" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="2"
                                    SaveScrollPosition="true"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvChart" runat="server" Visible="true" ContentUrl="">
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>


