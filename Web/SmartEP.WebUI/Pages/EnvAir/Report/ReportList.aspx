<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportList.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.ReportList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript" language="javascript">
                //页面刷新
                function Refresh_Grid(args) {
                    if (args) {
                        var MasterTable = $find("<%= gridList.ClientID %>").get_masterTableView();
                        MasterTable.rebind();
                    }
                }

                function ClientButtonClicking() {
                    
                    window.HTMLTitleElement = "rewew";
                    var pageID = document.getElementById("<%=hdPageId.ClientID%>").value;
                    var uri = pageID + ".aspx?DisplayPerson=" + document.getElementById("<%=hdPerson.ClientID%>").value;
                    var oWindow = window.radopen(encodeURI(uri), "ConfigOfflineDialog");
                    var width = document.documentElement.clientWidth;
                    var height = document.documentElement.clientHeight;
                    if (pageID == "AirQualityWeekReport" || pageID == "AQI-SECDayReportNew" || pageID == "VillageWeekReport"
                        || pageID == "VillageMonthReport" || pageID == "AirQualittyHalfYearReport" || pageID == "AirQualittyYearReport"
                        || pageID == "AirQualittySeasonReport" || pageID == "AirQualittyMonthReport") {
                        oWindow.SetSize(width, height);
                        oWindow.Center();
                    }
                }
                //行编辑按钮
                function ShowEditForm(id) {
                    var uri = "UploadReport.aspx?id=" + id;
                    window.radopen(encodeURI(uri), "ConfigOfflineDialog");
                }
            </script>
        </telerik:RadScriptBlock>
        <%--        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnAdd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ConfigOfflineDialog" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Width="100%" Height="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="30px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td style="width: 80px">开始时间</td>
                        <td style="width: 140px">
                            <%--  <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd" />--%>
                            <telerik:RadMonthYearPicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="120px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                        </td>
                        <td style="width: 80px">结束时间</td>
                        <td style="width: 140px">
                            <%--                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd" />--%>
                            <telerik:RadMonthYearPicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="120px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                        </td>
                        <td class="content" style="text-align: left">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridList" runat="server" GridLines="None" Width="100%" Height="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="false" AllowMultiRowSelection="true" OnItemCommand="gridList_ItemCommand" OnDeleteCommand="gridList_DeleteCommand"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="gridList_NeedDataSource" OnItemDataBound="gridList_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <MasterTableView GridLines="None" TableLayout="Fixed" DataKeyNames="id" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <div style="width: 100%; position: relative;" runat="server" id="dvTool">
                                <%-- <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                    runat="server" Width="50%" />--%>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="btnAdd" runat="server" OnClientClick="ClientButtonClicking();return false;" CssClass="RadToolBar_Customer" SkinID="ImgBtnAdd" />
                                        </td>
                                    </tr>
                                </table>


                            </div>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="报表日期" DataField="DateTimeRange" UniqueName="DateTimeRange" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="站点名称" DataField="PointNames" UniqueName="PointNames" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="监测项目" DataField="FactorsNames" UniqueName="FactorsNames" EmptyDataText="--" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="创建日期" DataField="CreatDateTime" UniqueName="CreatDateTime" HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="操作人" DataField="CreatUser" HeaderStyle-Width="100px" ItemStyle-Width="100px" UniqueName="CreatUser" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <%--<telerik:GridBoundColumn HeaderText="报表名称" DataField="ReportName" UniqueName="ReportName" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />--%>
                            <telerik:GridTemplateColumn HeaderText="上传文件" UniqueName="id" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" HeaderButtonType="None" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <img id="btnEdit" style="cursor: pointer;" alt="上传" title="点击上传" src="../../../Resources/Images/icons/folder_up.png"
                                        onclick="ShowEditForm('<%# DataBinder.Eval(Container, "DataItem.id")%>')" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="下载" UniqueName="ReportName" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" HeaderButtonType="None" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <%--<img id="btnEdit" style="cursor: pointer;" alt="上传" title="点击上传" src="../../../Resources/Images/icons/folder_up.png"
                                        onclick="ShowEditForm('<%# DataBinder.Eval(Container, "DataItem.id")%>')" />--%>
                                    <asp:ImageButton ID="imageBtn" runat="server" ToolTip="点击下载" CommandName="modify" CommandArgument='<%# Eval("id") %>' ImageUrl="../../../Resources/Images/icons/arrow_down.png" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridButtonColumn HeaderText="删除" CommandName="Delete" Text="删除" HeaderStyle-Width="60px" ItemStyle-Width="60px" ConfirmText="确定删除吗？" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridButtonColumn>
                            <%--<telerik:GridButtonColumn ButtonType="LinkButton" HeaderText="上传" Text="上传" CommandName="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridButtonColumn>--%>
                            <%--<telerik:GridButtonColumn ButtonType="LinkButton" HeaderText="下载" Text="下载" CommandName="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridButtonColumn>--%>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="ConfigOfflineDialog" runat="server" Height="410px" Width="530px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                    Title="文件操作" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" ShowOnTopWhenMaximized="true" />
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>
        <asp:HiddenField ID="hdPerson" runat="server" />
        <asp:HiddenField ID="hdPageId" runat="server" />
    </form>
</body>
</html>
