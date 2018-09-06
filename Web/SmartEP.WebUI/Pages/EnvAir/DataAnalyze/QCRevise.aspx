<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QCRevise.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.QCRevise" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>校准数据查询</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            //控制导出时按钮不会隐藏掉处理
            function onRequestStart(sender, args) {
                if (args.EventArgument == "")
                    return;
                if (args.EventArgument == 6 || args.EventArgument == 7 ||
                    args.EventArgument == 0 || args.EventArgument == 1 ||
                    args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                        args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                        args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                    args.set_enableAjax(false);
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="80px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%;" class="Table_Customer">
                    <tr>
                        <td class="title" style="text-align: right; width: 100px">测点：
                        </td>
                        <td class="content" style="text-align: left; width: 180px;">
                            <asp:DropDownList runat="server" ID="ddlPoint" Width="180px"></asp:DropDownList>
                        </td>
                        <td class="title" style="text-align: right; width: 100px">开始时间：
                        </td>
                        <td class="content" style="text-align: left; width: 180px;">
                            <telerik:RadDateTimePicker runat="server" ID="txtStartDate" MinDate="1900-01-01 00:00" DateInput-DateFormat="yyyy-MM-dd HH:00"></telerik:RadDateTimePicker>
                        </td>
                        <td class="title" style="text-align: right; width: 100px">截止日期：
                        </td>
                        <td class="content" style="text-align: left; width: 180px">
                            <telerik:RadDateTimePicker runat="server" ID="txtEndDate" MinDate="1900-01-01 00:00" DateInput-DateFormat="yyyy-MM-dd HH:00"></telerik:RadDateTimePicker>
                        </td>
                        <td class="content" style="text-align: left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" SkinID="ImgBtnSearch" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="text-align: right; width: 100px">校准类型：
                        </td>
                        <td class="content" style="text-align: left" colspan="5">
                            <asp:CheckBoxList ID="cblLogType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="自动零校准" Value="Z" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="自动精密度校准" Value="A" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="自动跨度校准" Value="S" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="手动零校准" Value="z"></asp:ListItem>
                                <asp:ListItem Text="手动精密度校准" Value="a"></asp:ListItem>
                                <asp:ListItem Text="手动跨度校准" Value="s"></asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="RadGrid1" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="20" AllowCustomPaging="true" AllowSorting="false"
                    AutoGenerateColumns="false" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="RadGrid1_NeedDataSource" CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" ItemStyle-Width="50px"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Container.DataSetIndex + 1%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="MonitoringPointName" HeaderText="测点名称" UniqueName="MonitoringPointName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="calTime" HeaderText="校准开始时间" UniqueName="calTime">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="overTime" HeaderText="校准结束时间" UniqueName="overTime">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="calNameCode" HeaderText="校准名称" UniqueName="calNameCode">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="calTypeCode" HeaderText="校准类型" UniqueName="calTypeCode">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="calConc" HeaderText="校准浓度" UniqueName="calConc">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="calFlow" HeaderText="校准流量" UniqueName="calFlow">
                            </telerik:GridBoundColumn>
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
