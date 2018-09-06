<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoringInstrumentList.aspx.cs" MasterPageFile="~/WebMaster/MasterPage.Master" Inherits="SmartEP.WebUI.Pages.EnvAir.BaseData.MonitoringInstrumentList" %>

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
                    <telerik:AjaxUpdatedControl ControlID="grid" />
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
                            var oWnd = window.radopen("MonitoringInstrumentAdd.aspx?ApplicationType=air&MonitoringPointUid=" + '<% =pointGuid %>', "PointDialog");
                            oWnd.maximize();
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

            //行编辑按钮
            function ShowEditForm(Id) {
                var oWnd = window.radopen("MonitoringInstrumentEdit.aspx?ApplicationType=air&MonitoringPointUid=" + '<% =pointGuid %>' + "&InstrumentUid=" + Id, "PointDialog");
                oWnd.maximize();
            }

            ////行双击事件
            //function OnRowDblClick(sender, args) {
            //    var selectIndex = myRadGrid._selectedIndexes.length > 0 ? myRadGrid._selectedIndexes[0] : -1;
            //    var selectKeyValues = selectIndex >= 0 ? myRadGrid._clientKeyValues[selectIndex] : null;
            //    if (selectKeyValues != null && selectKeyValues["MonitoringPointUid"] != null) {
            //        window.radopen("MonitoringInstrumentEdit.aspx?MonitoringPointUid=" + selectKeyValues["MonitoringPointUid"], "PointDialog");
            //    }
            //}
        </script>
    </telerik:RadScriptBlock>
    <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
        Width="100%">
        <telerik:RadPane ID="paneWhere" runat="server" Height="28px" Width="100%" Scrolling="None" BorderWidth="0">
            <table class="Table_Customer" width="100%">
                <tr>

                    <td class="title">仪器名称：</td>
                    <td class="content" style="width: 100px;">
                        <telerik:RadTextBox ID="txtInstrumentName" runat="server"></telerik:RadTextBox>
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
                AllowPaging="true" PageSize="20" AllowCustomPaging="false" AllowMultiRowSelection="true" AllowMultiRowEdit="true"
                ShowHeader="true" ShowStatusBar="true"
                OnNeedDataSource="grid_NeedDataSource" OnGridExporting="grid_GridExporting" OnItemCommand="grid_ItemCommand">
                <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                <MasterTableView DataKeyNames="RowGuid" ClientDataKeyNames="RowGuid" GridLines="None" CommandItemDisplay="Top" EditMode="InPlace"
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
                        <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="操作" UniqueName="RowGuid" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderButtonType="None" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                            <ItemTemplate>
                                  <img id="btnEdit" style="cursor: pointer;" alt="编辑" title="点击编辑" src="../../../../Resources/Images/icons/page_edit.png"
                                    onclick="ShowEditForm( '<%# DataBinder.Eval(Container, "DataItem.RowGuid")%>')" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="仪器名称" UniqueName="InstrumentName" DataField="InstrumentName" HeaderStyle-Width="180px" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                       
                        <telerik:GridTemplateColumn HeaderText="仪器类型" UniqueName="InstrumentTypeUid">
                            <ItemTemplate>
                                <%#GetInstrumentTypeName(DataBinder.Eval(Container.DataItem, "InstrumentTypeUid").ToString())%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                       <%-- <telerik:GridTemplateColumn HeaderText="站点类型" UniqueName="SiteTypeUid">
                            <ItemTemplate>
                                <%#GetSiteType(DataBinder.Eval(Container.DataItem, "SiteTypeUid").ToString())%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="站点状态" UniqueName="RunStatusUid">
                            <ItemTemplate>
                                <%#GetRunStatus(DataBinder.Eval(Container.DataItem, "RunStatusUid").ToString())%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>

                        <telerik:GridBoundColumn HeaderText="描述" UniqueName="Description" DataField="Description" />
                         
                    </Columns>
                </MasterTableView>
                <ClientSettings>
                    <Selecting AllowRowSelect="true" />
                    <ClientEvents OnGridCreating="RadGridCreating"/>
                    <Scrolling AllowScroll="true" EnableVirtualScrollPaging="false" UseStaticHeaders="true"
                        SaveScrollPosition="false"></Scrolling>
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="InstrumentDialog" runat="server" Height="410px" Width="530px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                Title="监测仪器" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
        </Windows>
        <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
            PinOn="固定" />
    </telerik:RadWindowManager>
</asp:Content>
