<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualityHourOrDayReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirQualityHourOrDayReport" %>

<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="div_hour" />
                        <telerik:AjaxUpdatedControl ControlID="div_day" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%" class="Table_Customer">
                    <tr>
                        <td class="title" style="width: 120px;">查询点位：
                        </td>
                        <td class="content" style="width: 690px;" colspan="2">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="690" CbxHeight="350" MultiSelected="true" DropDownWidth="690" ID="pointCbxRsm" DefaultAllSelected="false"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="content" rowspan="3">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 120px">数据类型：</td>
                        <td class="content" style="width: 150px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" RepeatLayout="Flow" RepeatColumns="2" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="小时数据" Value="HourData" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="日数据" Value="DayData"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <div id="div_hour" runat="server" visible="true">
                                <table>
                                    <tr>
                                        <td class="title" style="width: 120px;">开始时间：
                                        </td>
                                        <td class="content" style="width: 150px;">
                                            <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01 01:00" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                TimeView-HeaderText="小时" />
                                        </td>
                                        <td class="title" style="width: 120px;">截止时间：</td>
                                        <td class="content" style="width: 150px;">
                                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01 01:00" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                TimeView-HeaderText="小时" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="div_day" runat="server" visible="false">
                                <table>
                                    <tr>
                                        <td class="title" style="width: 120px;">开始日期：
                                        </td>
                                        <td class="content" style="width: 150px;">
                                            <telerik:RadDatePicker ID="BeginDate" runat="server" MinDate="1900-01-01" DateInput-DateFormat="yyyy-MM-dd"
                                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                        </td>
                                        <td class="title" style="width: 120px;">截止日期：</td>
                                        <td class="content" style="width: 150px;">
                                            <telerik:RadDatePicker ID="EndDate" runat="server" MinDate="1900-01-01" DateInput-DateFormat="yyyy-MM-dd"
                                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="RadGrid1" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="20" AllowCustomPaging="true" AllowSorting="false"
                    AutoGenerateColumns="true" AllowMultiRowSelection="false" MasterTableView-ShowFooter="true"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
                    OnColumnCreated="RadGrid1_ColumnCreated" CssClass="RadGrid_Customer">
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" AutoPostBack="true" />
                        </CommandItemTemplate>
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="污染物浓度及空气质量分指数（IAQI）" HeaderText="污染物浓度及空气质量分指数（IAQI）"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="SO2" HeaderText="二氧化硫(SO<sub>2</sub>)"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="NO2" HeaderText="二氧化氮(NO<sub>2</sub>)"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="PM10" HeaderText="PM<sub>10</sub>"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="CO" HeaderText="一氧化碳(CO)"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="O31" HeaderText="臭氧(O<sub>3</sub>)1"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="O38" HeaderText="臭氧(O<sub>3</sub>)8"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="PM2.5" HeaderText="PM<sub>2.5</sub>"
                                HeaderStyle-HorizontalAlign="Center" ParentGroupName="污染物浓度及空气质量分指数（IAQI）">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="空气质量指数类别" HeaderText="空气质量指数类别"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PageSizes="10 50 100" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
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
