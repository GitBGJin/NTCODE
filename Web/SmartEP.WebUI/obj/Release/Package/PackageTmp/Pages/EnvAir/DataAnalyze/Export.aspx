<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Export.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.Export" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                
                //tab页切换时时间检查
                function OnClientSelectedIndexChanging(sender, args) {
                    
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
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="auditLogGrid">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="auditLogGrid" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%--<telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="auditLogGrid" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="btnSearch" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="auditLogGrid" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <td class="title" style="width: 80px; text-align: center;">
                        </td>
                        <td class="content" style="width: 120px;">
                            </td>
                        <td class="title" style="width: 80px; text-align: center;">
                        </td>
                        <td class="content" style="width: 120px;">
                            </td>
                        <td class="title" style="width: 80px; text-align: center;">
                        </td>
                        <td class="content" style="width: 120px;">
                            </td>
                        <td class="title" style="width: 80px; text-align: center;">
                        </td>
                        <td class="content" style="width: 120px;">
                            </td>
                        <td class="title" style="width: 80px; text-align: center;">
                        </td>
                        <td class="content" style="width: 120px;">
                            </td>
                        <td class="title" style="width: 80px; text-align: center;">
                        </td>
                        <td class="content" style="width: 120px;">
                            </td>
                        <td class="content" align="right">
                            <asp:ImageButton ID="btnSearch" runat="server"  OnClick="btnSearch_Click" SkinID="Creatclick" />
                        </td>
                        <td class="content" align="right">
                            <asp:ImageButton ID="btnSearch1" runat="server"  OnClick="btnSearch1_Click" SkinID="Exportclick" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pvGrid" runat="server" Visible="true">
                        <telerik:RadGrid ID="auditLogGrid" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="20" AllowCustomPaging="false" AllowSorting="false" ShowFooter="true"
                            AutoGenerateColumns="False" AllowMultiRowSelection="True" ClientSettings-Selecting-AllowRowSelect="true"  AllowMultiRowEdit="true"
                            EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="auditLogGrid_NeedDataSource" OnGridExporting="auditLogGrid_GridExporting" OnItemDataBound="auditLogGrid_ItemDataBound"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" RefreshText="" ShowExportToExcelButton="false" ShowExportToWordButton="false" ShowExportToPdfButton="false" />
                                <Columns>
                                    <telerik:GridClientSelectColumn HeaderText="选择"  UniqueName="ClientSelectColumn" Exportable="false">
                                        <HeaderStyle Width="10px"></HeaderStyle>
                                    </telerik:GridClientSelectColumn>
                                    <telerik:GridTemplateColumn FilterControlAltText="Filter rowNum column" HeaderText="序号" meta:resourcekey="GridTemplateColumnResource1" UniqueName="rowNum" HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <%# Container.DataSetIndex + 1 %>
                                        </ItemTemplate>
                                        <HeaderStyle Height="40px" HorizontalAlign="Center" Width="40px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn HeaderText="表单名称" UniqueName="表单名称" DataField="ImpName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                                    <telerik:GridBoundColumn HeaderText="状态" UniqueName="状态"  DataField="State" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" >
                                        <%--<ItemTemplate>
                                             <asp:Label ID="Label2" runat="server" Text='<%#eval_r("uState") %>' Visible="false"></asp:Label>
                                            <asp:ImageButton ID="imuncre" ImageUrl="" runat="server" CommandName="UnFreeze" CommandArgument='<%#eval_r("uID") %>' Visible="false"/>
                                            <asp:ImageButton ID="imcre" ImageUrl="" runat="server" CommandName="UnFreeze" CommandArgument='<%#eval_r("uID") %>' Visible="false"/>
                                            <asp:ImageButton ID="imcring" ImageUrl="" runat="server" CommandName="Freeze" CommandArgument='<%#eval_r("uID") %>' Visible="false"/>
                                        </ItemTemplate>--%>
                                        </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="最新生成时间" UniqueName="最新生成时间" DataField="CreatTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                                    <telerik:GridTemplateColumn HeaderText="下载" UniqueName="download" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn+<%# Container.DataSetIndex + 1 %>" runat="server"  OnClick="btnSearch2_Click" SkinID="Havedown" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle FirstPageToolTip="首页" Visible="false" AlwaysVisible="false" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                    PageSizeLabelText="显示记录数:" PageSizes="20 50 100" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                            </MasterTableView>
                            <CommandItemStyle Width="100%" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="2"
                                    SaveScrollPosition="true"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>

