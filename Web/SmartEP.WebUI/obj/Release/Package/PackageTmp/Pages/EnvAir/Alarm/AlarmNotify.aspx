<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmNotify.aspx.cs" MasterPageFile="~/WebMaster/MasterPage.Master" Inherits="SmartEP.WebUI.Pages.EnvAir.Alarm.AlarmNotify" %>

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

            //站点编辑按钮
            function ShowEditForm(NotifySendUid) {
                var oWnd = window.radopen("AlarmHandle.aspx?NotifySendUid=" + NotifySendUid, "PointDialog");
            }
            ////行双击事件
            //function OnRowDblClick(sender, args) {
            //    var selectIndex = myRadGrid._selectedIndexes.length > 0 ? myRadGrid._selectedIndexes[0] : -1;
            //    var selectKeyValues = selectIndex >= 0 ? myRadGrid._clientKeyValues[selectIndex] : null;
            //    if (selectKeyValues != null && selectKeyValues["MonitoringPointUid"] != null) {
            //        window.radopen("AirPointEdit.aspx?MonitoringPointUid=" + selectKeyValues["MonitoringPointUid"], "PointDialog");
            //    }
            //}
        </script>
    </telerik:RadScriptBlock>
    <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
        Width="100%">
        <telerik:RadPane ID="paneWhere" runat="server" Height="56px" Width="100%" Scrolling="None" BorderWidth="0">
            <table class="Table_Customer" width="100%">
                <tr>
                    <td class="title">通知类型：</td>
                    <td class="content" style="width: 220px;">
                        <telerik:RadComboBox runat="server" ID="comboNotifyType" Width="180px" />
                    </td>
                      <td class="title" style="width: 80px; text-align: center;">发送时间:
                        </td>
                        <td class="content" style="width: 360px;" colspan="4">
                            <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                            &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                   </tr>
                 <tr>
                       <td class="title">接收人：</td>
                    <td class="content" style="width: 100px;">
                        <telerik:RadTextBox ID="txtReceiveUserName" runat="server"></telerik:RadTextBox>
                    </td>
                      <td class="title">发送内容：</td>
                    <td class="content" style="width: 100px;">
                        <telerik:RadTextBox ID="txtContent" runat="server"></telerik:RadTextBox>
                    </td>
                    <td class="title">是否处理：</td>
                    <td class="content" style="width: 100px;">
                        <asp:RadioButtonList ID="rbtnHandleOrNot" Width="90px" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="是" Value="1"  />
                            <asp:ListItem Text="否" Value="0" Selected="True" />
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
                AllowPaging="true" PageSize="20" AllowCustomPaging="false" AllowMultiRowSelection="true" AllowMultiRowEdit="true"
                ShowHeader="true" ShowStatusBar="true"
                OnNeedDataSource="grid_NeedDataSource" OnGridExporting="grid_GridExporting" OnItemCommand="grid_ItemCommand" OnItemCreated="grid_ItemCreated">
                <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                <MasterTableView DataKeyNames="NotifySendUid" ClientDataKeyNames="NotifySendUid" GridLines="None" CommandItemDisplay="Top" EditMode="InPlace"
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
                         <telerik:GridTemplateColumn HeaderText="处理" UniqueName="NotifySendUid" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderButtonType="None" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                            <ItemTemplate>
                                  <img id="btnHandle" style="cursor: pointer;" alt="处理" title="点击处理" src="../../../../Resources/Images/icons/page_edit.png"
                                    onclick="ShowEditForm('<%# DataBinder.Eval(Container, "DataItem.NotifySendUid")%>')" />
                                 
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                       
                        <telerik:GridBoundColumn HeaderText="报警信息" UniqueName="SendContent" DataField="SendContent" HeaderStyle-Width="300px" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                        <telerik:GridBoundColumn HeaderText="发送时间" UniqueName="SendDateTime" DataField="SendDateTime" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <telerik:GridBoundColumn HeaderText="接收人" UniqueName="ReceiveUserNames" DataField="ReceiveUserNames" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />

                        <telerik:GridCheckBoxColumn HeaderText="是否处理" UniqueName="HandleFinishOrNot" DataField="HandleFinishOrNot" HeaderStyle-Width="40px" />
                       <%--  <telerik:GridTemplateColumn HeaderText="是否处理" UniqueName="HandleFinishOrNot" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                 <%#GetHandleOrNot(DataBinder.Eval(Container.DataItem, "HandleFinishOrNot").ToString())%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        
                         <telerik:GridBoundColumn HeaderText="处理人" UniqueName="UpdateUser" DataField="UpdateUser" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                       <telerik:GridBoundColumn HeaderText="处理时间" UniqueName="UpdateDateTime" DataField="UpdateDateTime" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />

                        <telerik:GridBoundColumn HeaderText="处理意见" UniqueName="Description" DataField="Description" />
                         
                    </Columns>
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
            <telerik:RadWindow ID="PointDialog" runat="server" Height="400px" Width="600px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                Title="报警处理" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
        </Windows>
        <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
            PinOn="固定" />
    </telerik:RadWindowManager>
</asp:Content>
