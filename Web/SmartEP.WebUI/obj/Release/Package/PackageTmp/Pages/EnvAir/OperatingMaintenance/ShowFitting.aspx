<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowFitting.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.ShowFitting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript" language="javascript">
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
                            //var ss = $find("<%= hd.ClientID %>").value;
                             
                            var oWnd = window.radopen("AddFitting.aspx?systemType= <%=hdType.Value%>&InstrumentInstanceGuid=<%= hd.Value %>", "PointDialog");
                            oWnd.SetTitle("添加配件信息")
                            oWnd.Title = "添加配件信息";
                            oWnd.maximize();
                            args.set_cancel(true);//给用户提示正在处理中
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

            </script>
        </telerik:RadScriptBlock>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="28px" Width="100%" Scrolling="None" BorderWidth="0">
                <table class="Table_Customer" width="100%">
                    <tr>

                        <td class="title">配件名称：</td>
                        <td class="content" style="width: 100px;">
                            <asp:TextBox ID="FittingName" runat="server"></asp:TextBox>
                        </td>


                        <td class="btnsSearch">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                </table>
                `  
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Height="100%" Width="100%" Scrolling="None" BorderWidth="0">
                <telerik:RadGrid ID="grid" runat="server" CssClass="RadGrid_Customer" GridLines="None" Width="99.8%" Height="100%"
                    AutoGenerateColumns="false" ToolTip="鼠标在表格内单击或拖动选定记录后，可单个或批量删除和更新！"
                    AllowPaging="true" PageSize="20" AllowCustomPaging="false" AllowMultiRowSelection="true" AllowMultiRowEdit="true"
                    ShowHeader="true" ShowStatusBar="true"
                    OnNeedDataSource="grid_NeedDataSource" OnItemCommand="grid_ItemCommand">
                    <MasterTableView DataKeyNames="RowGuid" ClientDataKeyNames="RowGuid" GridLines="None" CommandItemDisplay="Top" EditMode="InPlace"
                        NoMasterRecordsText="没有数据" NoDetailRecordsText="没有数据" ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage">
                        <CommandItemTemplate>
                            <telerik:RadToolBar ID="gridRTB" runat="server" AutoPostBack="true" CssClass="RadToolBar_Customer" SkinID="CD" Width="100%" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                        </CommandItemTemplate>

                        <Columns>
                            <telerik:GridClientSelectColumn HeaderText="选择" UniqueName="ClientSelectColumn" Exportable="false">
                                <HeaderStyle Width="40px"></HeaderStyle>
                            </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="配件名称" UniqueName="FittingName" DataField="FittingName" HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="系统编号" UniqueName="FittingFixedAssetNumber" DataField="FittingFixedAssetNumber" HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="true" EnableVirtualScrollPaging="false" UseStaticHeaders="true"
                            SaveScrollPosition="false"></Scrolling>
                    </ClientSettings>

                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="InstrumentFault" runat="server" Height="1000px" Width="680px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                    Title="显示仪器配件" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>
        <asp:HiddenField ID="hd" runat="server" />
        <asp:HiddenField ID="hdType" runat="server" />
    </form>
</body>
</html>
