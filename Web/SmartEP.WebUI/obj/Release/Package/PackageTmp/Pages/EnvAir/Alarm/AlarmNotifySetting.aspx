<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmNotifySetting.aspx.cs" MasterPageFile="~/WebMaster/MasterPage.Master" Inherits="SmartEP.WebUI.Pages.EnvAir.Alarm.AlarmNotifySetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grid" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="splitter">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grid" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grid" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <ClientEvents OnRequestStart="onRequestStart" />
    </telerik:RadAjaxManager>

    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript" language="javascript">
            var myRadGrid;
            function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

            //页面刷新
            function Refresh_Grid(args) {
                if (args) {
                    var MasterTable = $find("<%= grid.ClientID %>").get_masterTableView();
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

            //按钮行处理
            function gridRTB_ClientButtonClicking(sender, args) {
                var masterTable = $find("<%= grid.ClientID %>").get_masterTableView();
                var CurrentBtn = args.get_item();
                var CurrentBtnName = CurrentBtn.get_text();
                var CurrentBtnCommandName = CurrentBtn.get_commandName();
                switch (CurrentBtnCommandName) {
                    case "InitInsert":
                        {
                            //增加
                            window.radopen("AlarmNotifyAdd.aspx", "PointDialog");
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

            //编辑按钮
            function ShowEditForm(NotifyStrategyUid) {
                var oWnd = window.radopen("AlarmNotifyEdit.aspx?NotifyStrategyUid=" + NotifyStrategyUid, "PointDialog");
            }
        </script>
    </telerik:RadScriptBlock>
    <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
        Width="100%">
        <telerik:RadPane ID="paneWhere" runat="server" Height="30px" Width="100%" Scrolling="None" BorderWidth="0">
            <table class="Table_Customer" style="height: 100%;">
                <tr>
                    <td class="title" style="width: 100px;">报警等级：</td>
                    <td class="content" style="width: 150px;">
                        <telerik:RadComboBox runat="server" ID="notifyGradeList" Width="100%" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" Localization-CheckAllString="所有报警等级" EmptyMessage="请选择" />
                    </td>
                    <td class="title" style="width: 100px;">报警类型：</td>
                    <td class="content" style="width: 150px;">
                        <telerik:RadComboBox runat="server" ID="comboAlarmType" Width="100%" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" Localization-CheckAllString="所有报警类型" EmptyMessage="请选择" />
                    </td>
                    <td class="title" style="width: 100px;">策略名称：</td>
                    <td class="content" style="width: 150px;">
                        <telerik:RadTextBox ID="txtStrategyName" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                    <td class="title" style="width: 100px;">是否启用：</td>
                    <td class="content" style="width: 100px;">
                        <asp:RadioButtonList ID="rbtEnableOrNot" Width="90px" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="是" Value="1" Selected="True" />
                            <asp:ListItem Text="否" Value="0" />
                        </asp:RadioButtonList>
                    </td>
                    <td class="btnsSearch">
                        <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                    </td>
                </tr>
            </table>
        </telerik:RadPane>
        <telerik:RadPane ID="paneGrid" runat="server" Height="100%" Width="100%" Scrolling="None" BorderWidth="0">
            <telerik:RadGrid ID="grid" runat="server" CssClass="RadGrid_Customer" GridLines="None" Width="99.8%" Height="100%"
                AutoGenerateColumns="false" ToolTip="鼠标在表格内单击或拖动选定记录后，可单个或批量删除和更新！"
                AllowPaging="true" PageSize="24" AllowCustomPaging="false" AllowMultiRowSelection="true" AllowMultiRowEdit="true"
                ShowHeader="true" ShowStatusBar="true"
                OnNeedDataSource="grid_NeedDataSource" OnItemCreated="grid_ItemCreated" OnItemCommand="grid_ItemCommand">
                <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                <MasterTableView DataKeyNames="NotifyStrategyUid" ClientDataKeyNames="NotifyStrategyUid" GridLines="None" CommandItemDisplay="Top" EditMode="InPlace"
                    NoMasterRecordsText="没有数据" NoDetailRecordsText="没有数据" ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage">
                    <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                        PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ExportToExcelText="导出到Excel"
                        ShowExportToWordButton="true" ExportToWordText="导出到Word" ShowExportToPdfButton="true" ExportToPdfText="导出到PDF" />
                    <CommandItemTemplate>
                        <telerik:RadToolBar ID="gridRTB" runat="server" AutoPostBack="true" CssClass="RadToolBar_Customer" SkinID="CRUD" Width="100%" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridClientSelectColumn HeaderText="选择" UniqueName="ClientSelectColumn" Exportable="false">
                            <HeaderStyle Width="40px"></HeaderStyle>
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="编辑" UniqueName="NotifyStrategyUid" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderButtonType="None" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                            <ItemTemplate>
                                <img id="btnHandle" style="cursor: pointer;" alt="编辑" title="点击编辑" src="../../../Resources/Images/icons/page_edit.png"
                                    onclick="ShowEditForm('<%# DataBinder.Eval(Container, "DataItem.NotifyStrategyUid")%>')" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="监测点" UniqueName="EffectSubject" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#GetPoints(DataBinder.Eval(Container.DataItem, "EffectSubject").ToString())%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="策略名称" UniqueName="NotifyStrategyName" DataField="NotifyStrategyName" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <telerik:GridTemplateColumn HeaderText="报警等级" UniqueName="NotifyGradeUid" HeaderStyle-Width="100px" HeaderStyle-Height="50px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#GetGradeName(DataBinder.Eval(Container.DataItem,"NotifyGradeUid"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="报警类型" UniqueName="AlarmEventUid" HeaderStyle-Width="100px" HeaderStyle-Height="50px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#GetAlarmTypeName(DataBinder.Eval(Container.DataItem,"AlarmEventUid"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="接收人" UniqueName="NotifyNumberUids" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#GetReceiveUsers(DataBinder.Eval(Container.DataItem, "NotifyNumberUids").ToString())%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="开始时间" UniqueName="BeginTime" DataField="BeginTime" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <telerik:GridBoundColumn HeaderText="结束时间" UniqueName="EndTime" DataField="EndTime" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <telerik:GridBoundColumn HeaderText="通知次数" UniqueName="NotifyCount" DataField="NotifyCount" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <telerik:GridBoundColumn HeaderText="通知间隔<br/>（分钟）" UniqueName="NotifySpan" DataField="NotifySpan" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <telerik:GridTemplateColumn HeaderText="是否启用" UniqueName="EnableOrNot" HeaderStyle-Width="80px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#GetEnableOrNot(DataBinder.Eval(Container.DataItem,"EnableOrNot"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle FirstPageToolTip="首页" PageSizes="24 48 96" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                </MasterTableView>
                <ClientSettings>
                    <Selecting AllowRowSelect="true" />
                    <ClientEvents OnGridCreating="RadGridCreating" />
                    <Scrolling AllowScroll="true" EnableVirtualScrollPaging="false" UseStaticHeaders="true"
                        SaveScrollPosition="false"></Scrolling>
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="PointDialog" runat="server" Height="500px" Width="800px" Skin="Outlook"
                Title="报警通知配置" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
        </Windows>
        <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
            PinOn="固定" />
    </telerik:RadWindowManager>
</asp:Content>
