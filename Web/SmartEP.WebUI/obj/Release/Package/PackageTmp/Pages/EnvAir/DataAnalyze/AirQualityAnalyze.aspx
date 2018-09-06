<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualityAnalyze.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.AirQualityAnalyze" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

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
                        var MasterTable = $find("<%= gridQualityAnalyze.ClientID %>").get_masterTableView();
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
                    var date1 = new Date();
                    var date2 = new Date();
                    // var date1 = $find("<= dtpBegin.ClientID %>").get_selectedDate();
                    //     var date2 = $find("<= dtpEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        //sender.set_autoPostBack(false);
                        return false;
                    }
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                //tab页切换时时间检查
                function OnClientSelectedIndexChanging(sender, args) {
                    var date1 = new Date();
                    var date2 = new Date();
                    // var date1 = $find("<= dtpBegin.ClientID %>").get_selectedDate();
                    //   var date2 = $find("<= dtpEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        args.set_cancel(true);
                        return;
                    }
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        args.set_cancel(true);
                        return;
                    } else {
                        return;
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
                <telerik:AjaxSetting AjaxControlID="gridQualityAnalyze">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridQualityAnalyze" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridQualityAnalyze" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridQualityAnalyze" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridQualityAnalyze" />
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
                        <td class="title" style="width: 120px; text-align: center;">开始时间:
                        </td>
                        <td class="content">
                            <telerik:RadDatePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">截止时间：
                        </td>
                        <td class="content">
                            <telerik:RadDatePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                        <td class="content" align="left" style="width: 70px;" rowspan="2">
                            <asp:CheckBox ID="IsStatistical" Checked="false" Text="国家数据" runat="server" />
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
                            <telerik:RadComboBox runat="server" ID="comboPort" Width="180px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
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
                            <telerik:RadComboBox runat="server" ID="comboCityProper" Width="180px" CheckBoxes="true" Visible="false" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="姑苏区" Value="6a4e7093-f2c6-46b4-a11f-0f91b4adf379" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴中区" Value="e1c104f3-aaf3-4d0e-9591-36cdc83be15a" Checked="true" />
                                    <telerik:RadComboBoxItem Text="高新区" Value="f320aa73-7c55-45d4-a363-e21408e0aac3" Checked="true" />
                                    <telerik:RadComboBoxItem Text="工业园区" Value="69a993ff-78c6-459b-9322-ee77e0c8cd68" Checked="true" />
                                    <telerik:RadComboBoxItem Text="相城区" ToolTip="" Value="8756bd44-ff18-46f7-aedf-615006d7474c" Checked="true" />
                                    <telerik:RadComboBoxItem Text="市区合计" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadComboBox runat="server" ID="comboCity" Width="180px" CheckBoxes="true" Visible="false" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
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
                            <telerik:RadComboBox runat="server" ID="comboCityModel" Width="180px" CheckBoxes="true" Visible="false" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
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
                <telerik:RadGrid ID="gridQualityAnalyze" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="gridQualityAnalyze_NeedDataSource" OnItemDataBound="gridQualityAnalyze_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                        </CommandItemTemplate>
                        <Columns>
                            <%--<telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridBoundColumn HeaderText="城市" UniqueName="PortName" DataField="PortName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn HeaderText="点位" UniqueName="Point" DataField="Point" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="优" UniqueName="Superior" DataField="Superior" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="比例" UniqueName="Sscale" DataField="Sscale" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="良" UniqueName="Fine" DataField="Fine" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="比例" UniqueName="Fscale" DataField="Fscale" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="轻度污染" UniqueName="MildPollute" DataField="MildPollute" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="中度污染" UniqueName="MezzoPollute" DataField="MezzoPollute" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="重度污染" UniqueName="SeverePollute" DataField="SeverePollute" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="严重污染" UniqueName="SeriousPollute" DataField="SeriousPollute" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="超标天数合计" UniqueName="OverTotal" DataField="OverTotal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
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
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>

