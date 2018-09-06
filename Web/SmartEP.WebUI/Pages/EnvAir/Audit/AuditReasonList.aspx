<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditReasonList.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.AuditReasonList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/ecmascript">
            var myRadGrid;
            function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

            //页面刷新
            function Refresh_Grid(args) {
                if (args) {
                    var MasterTable = $find("<%= gridAudit.ClientID %>").get_masterTableView();
                    MasterTable.rebind();
                }
            }

            //按钮行处理
            function gridRTB_ClientButtonClicking(sender, args) {
                var masterTable = $find("<%= gridAudit.ClientID %>").get_masterTableView();
                var CurrentBtn = args.get_item();
                var CurrentBtnName = CurrentBtn.get_text();
                var CurrentBtnCommandName = CurrentBtn.get_commandName();
                switch (CurrentBtnCommandName) {
                    case "InitInsert":
                        {
                            //增加
                            window.radopen("AuditReasonAdd.aspx", "ReasonDialog");
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

            //行双击事件
            function OnRowDblClick(sender, args) {
                var selectIndex = myRadGrid._selectedIndexes.length > 0 ? myRadGrid._selectedIndexes[0] : -1;
                var selectKeyValues = selectIndex >= 0 ? myRadGrid._clientKeyValues[selectIndex] : null;
                if (selectKeyValues != null && selectKeyValues["ReasonGuid"] != null) {
                    window.radopen("AuditReasonAdd.aspx?ReasonGuid=" + selectKeyValues["ReasonGuid"], "ReasonDialog");
                }
            }

            //站点编辑按钮
            function ShowEditForm(ReasonGuid) {
                var oWnd = window.radopen("AuditReasonAdd.aspx?ReasonGuid=" + ReasonGuid, "ReasonDialog");
                oWnd.SetTitle("审核理由配置")
                oWnd.Title = "审核理由配置";
            }

            function onRequestStart(sender, args) {
                if (args.EventArgument == "")
                    return;
                if (args.EventArgument == 6|| args.EventArgument == 7 ||
                    args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                        args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                        args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                    args.set_enableAjax(false);
                }
            }
        </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridAudit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="11%" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 100px; text-align: center;">查找关键字：
                        </td>
                        <td class="content" style="width: 250px;">
                            <asp:TextBox runat="server" ID="txt_KeyWords" Width="250px"></asp:TextBox>
                        </td>
                        <td class="content" align="left">
                            <asp:ImageButton ID="btnSearch" runat="server" SkinID="ImgBtnSearch" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridAudit" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" ToolTip="鼠标在表格内单击或拖动选定记录后，可单个或批量删除和更新！"
                    AllowCustomPaging="false" AllowSorting="false" ShowFooter="true" AllowMultiRowSelection="true" AllowMultiRowEdit="true"
                    AutoGenerateColumns="false" ShowStatusBar="true" OnNeedDataSource="gridAudit_NeedDataSource" OnItemCommand="gridAudit_ItemCommand" CssClass="RadGrid_Customer">
                          <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                     <MasterTableView GridLines="None" NoMasterRecordsText="没有数据" CommandItemDisplay="Top"
                        DataKeyNames="ReasonGuid" ClientDataKeyNames="ReasonGuid">
                        <CommandItemTemplate>
                            <telerik:RadToolBar ID="gridRTB" runat="server" AutoPostBack="true" CssClass="RadToolBar_Customer" SkinID="CRUD" Width="100%"  OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridClientSelectColumn HeaderText="选择" UniqueName="ClientSelectColumn">
                                <HeaderStyle Width="50px"></HeaderStyle>
                            </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Container.DataSetIndex + 1%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="操作" UniqueName="AuditMonitoringPointUid" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <img id="btnEdit" style="cursor: pointer;" alt="编辑" title="点击编辑" src="../../../Resources/Images/icons/page_edit.png"
                                        onclick="ShowEditForm('<%# DataBinder.Eval(Container, "DataItem.ReasonGuid")%>')" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="审核理由" DataField="ReasonContent" UniqueName="ReasonContent"
                                HeaderStyle-Width="250px" HeaderStyle-Height="50px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="排序号" DataField="OrderByNum" UniqueName="OrderByNum"
                                HeaderStyle-Width="50px" HeaderStyle-Height="50px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="创建人" DataField="CreatUser" UniqueName="CreatUser"
                                HeaderStyle-Width="150px" HeaderStyle-Height="50px">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="创建日期" UniqueName="CreatDateTime" HeaderStyle-Width="150px" HeaderStyle-Height="50px">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.CreatDateTime")).ToString("yyyy-MM-dd") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <ClientSettings>
                        <Selecting AllowRowSelect="true" />
                        <ClientEvents OnGridCreating="RadGridCreating" OnRowDblClick="OnRowDblClick" />
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="ReasonDialog" runat="server" Height="450px" Width="450px" Skin="Outlook"
                    Title="审核理由配置" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>
    </form>
</body>
</html>
