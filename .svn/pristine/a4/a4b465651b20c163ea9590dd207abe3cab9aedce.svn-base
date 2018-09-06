<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HeavyPollutantAnalyze.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.HeavyPollutantAnalyze" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .RadGrid_Customer {
            float: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function OnClientClicking() {
                    var date1 = new Date();
                    var date2 = new Date();
                    date1 = $find("<%= dtpFirstBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= dtpFirstEnd.ClientID %>").get_selectedDate();
                    str1 = (date1 + "").split(" ");
                    str2 = (date2 + "").split(" ");
                    yearBegin = str1[5];
                    yearEnd = str2[5];
                    var date3 = $find("<%= dtpThirdBegin.ClientID %>").get_selectedDate();
                    var date4 = $find("<%= dtpThirdEnd.ClientID %>").get_selectedDate();
                    var str3 = (date3 + "").split(" ");
                    var str4 = (date4 + "").split(" ");
                    yearBegin3 = str3[5];
                    yearEnd4 = str4[5];

                    if ((date1 == null) || (date2 == null) || (date3 == null) || (date4 == null)) {
                        alert("开始时间或结束时间不能为空！");
                        //sender.set_autoPostBack(false);
                        return false;
                    }
                    if (date1 > date2 || date3 > date4) {
                        alert("开始时间不能大于结束时间！");
                        return false;
                    }
                    if (yearBegin != yearEnd || yearBegin3 != yearEnd4) {
                        alert("应选同一年月度范围！");
                        return false;
                    } else {
                        return true;
                    }
                }
                //Grid按钮行处理
                function gridRTB_ClientButtonClicking(sender, args) {
                    args.set_cancel(!OnClientClicking());
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
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gridHeavyPollutantAnalyze">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridHeavyPollutantAnalyze" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridHeavyPollutantAnalyze" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridHeavyPollutantAnalyze" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridHeavyPollutantAnalyze" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="dvPoints" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvProper" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <%--<td class="title" style="width: 120px; text-align: center;">查询类型
                        </td>--%>
                        <td class="content" style="width: 120px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged" RepeatLayout="Flow" RepeatColumns="5">
                                <asp:ListItem Text="测点" Value="Port" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="区域" Value="CityProper"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <%-- <td class="title" style="width: 120px; text-align: center;">查询测点（区域）
                        </td>--%>
                        <td class="content" style="width: auto;">
                            <div runat="server" id="dvPoints">
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="240" CbxHeight="350" MultiSelected="false" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                            </div>
                            <div runat="server" id="dvProper">
                                <telerik:RadDropDownList runat="server" ID="ddlCityProper" Width="240px" Visible="false"></telerik:RadDropDownList>
                            </div>

                        </td>
                        <td class="title" style="width: 80px">污染程度
                        </td>
                        <td class="content">
                            <%--                            <telerik:RadComboBox runat="server" ID="rcbPollution" Width="240px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="false" CheckedItemsTexts="DisplayAllInInput">
                            </telerik:RadComboBox>--%>
                            <telerik:RadDropDownList runat="server" ID="rcbPollution" Width="240px">
                            </telerik:RadDropDownList>
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">基准时间
                        </td>
                        <td class="content" style="width: 280px;">
                            <div id="dvFirst" runat="server">
                                <telerik:RadMonthYearPicker ID="dtpFirstBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy年MM月"
                                    DatePopupButton-ToolTip="打开日历选择" Width="100px"
                                    MonthYearNavigationSettings-CancelButtonCaption="取消"
                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                                <telerik:RadMonthYearPicker ID="dtpFirstEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy年MM月"
                                    DatePopupButton-ToolTip="打开日历选择" Width="100px"
                                    MonthYearNavigationSettings-CancelButtonCaption="取消"
                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月" />
                            </div>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">
                            <%--            <div id="dvSecond" runat="server">
                                <telerik:RadMonthYearPicker ID="dtpSecondBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy年MM月"
                                    DatePopupButton-ToolTip="打开日历选择" Width="100px"
                                    MonthYearNavigationSettings-CancelButtonCaption="取消"
                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                                <telerik:RadMonthYearPicker ID="dtpSecondEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy年MM月"
                                    DatePopupButton-ToolTip="打开日历选择" Width="100px"
                                    MonthYearNavigationSettings-CancelButtonCaption="取消"
                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月" />
                            </div>--%>
                            对比时间
                        </td>
                        <td class="content" style="width: 280px;">
                            <div id="dvThird" runat="server">
                                <telerik:RadMonthYearPicker ID="dtpThirdBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy年MM月"
                                    DatePopupButton-ToolTip="打开日历选择" Width="100px"
                                    MonthYearNavigationSettings-CancelButtonCaption="取消"
                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                                <telerik:RadMonthYearPicker ID="dtpThirdEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy年MM月"
                                    DatePopupButton-ToolTip="打开日历选择" Width="100px"
                                    MonthYearNavigationSettings-CancelButtonCaption="取消"
                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月" />
                            </div>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridHeavyPollutantAnalyze" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="false" AllowMultiRowSelection="false"
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
                        <Columns>
                            <telerik:GridBoundColumn HeaderText="" DataField="timeA" UniqueName="timeA" HeaderStyle-HorizontalAlign="Center" SortExpression="desc" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="AQI(基准)" DataField="AQIValue(0)" UniqueName="AQIValue(0)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="污染物(基准)" DataField="PrimaryPollutant(0)" UniqueName="PrimaryPollutant(0)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="污染等级(基准)" DataField="Grade(0)" UniqueName="Grade(0)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="" DataField="timeB" UniqueName="timeB" HeaderStyle-HorizontalAlign="Center" SortExpression="desc" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="AQI(对比)" DataField="AQIValue(1)" UniqueName="AQIValue(1)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="污染物(对比)" DataField="PrimaryPollutant(1)" UniqueName="PrimaryPollutant(1)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="污染等级(对比)" DataField="Grade(1)" UniqueName="Grade(1)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <%-- <telerik:GridBoundColumn HeaderText="" DataField="timeC" UniqueName="timeC" HeaderStyle-HorizontalAlign="Center" SortExpression="desc" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="AQI" DataField="AQIValue(2)" UniqueName="AQIValue(2)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="污染物" DataField="PrimaryPollutant(2)" UniqueName="PrimaryPollutant(2)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="污染等级" DataField="Grade(2)" UniqueName="Grade(2)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />--%>
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
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
