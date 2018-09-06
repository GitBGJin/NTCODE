<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddFitting.aspx.cs"  Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.AddFitting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript" language="javascript">

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
                    <td class="btnAdd">
                        <asp:ImageButton ID="ImageAdd" runat="server" OnClick="btnAdd_Click" SkinID="ImgBtnSave" />
                    </td>
                </tr>
            </table>
    `   </telerik:RadPane>
        <telerik:RadPane ID="paneGrid" runat="server" Height="100%" Width="100%" Scrolling="None" BorderWidth="0">
            <telerik:RadGrid ID="grid" runat="server" CssClass="RadGrid_Customer" GridLines="None" Width="99.8%" Height="100%"
                AutoGenerateColumns="true" ToolTip="鼠标在表格内单击或拖动选定记录后，可单个或批量删除和更新！"
                AllowPaging="true" PageSize="20" AllowCustomPaging="false" AllowMultiRowSelection="true" AllowMultiRowEdit="true"
                ShowHeader="true" ShowStatusBar="true"
                OnNeedDataSource="grid_NeedDataSource" OnColumnCreated="grid_ColumnCreated">
             <MasterTableView DataKeyNames="RowGuid" ClientDataKeyNames="RowGuid" GridLines="None" CommandItemDisplay="None" EditMode="InPlace"
                    NoMasterRecordsText="没有数据" NoDetailRecordsText="没有数据" ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage">
<%--                <CommandItemTemplate>
                        <telerik:RadToolBar ID="gridRTB" runat="server" AutoPostBack="true" CssClass="RadToolBar_Customer" SkinID="CRUD" Width="100%" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                </CommandItemTemplate>--%>

                <Columns>
                   <telerik:GridClientSelectColumn HeaderText="选择" UniqueName="ClientSelectColumn" Exportable="false">
                            <HeaderStyle Width="40px"></HeaderStyle>
                   </telerik:GridClientSelectColumn>
                   <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                   </telerik:GridTemplateColumn>
<%--                   <telerik:GridBoundColumn HeaderText="配件名称" UniqueName="InstanceName" DataField="InstanceName" HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                   <telerik:GridBoundColumn HeaderText="系统编号" UniqueName="FixedAssetNumber" DataField="FixedAssetNumber" HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />--%>
               </Columns>
             </MasterTableView>
                 <ClientSettings>
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" EnableVirtualScrollPaging="false" UseStaticHeaders="true"
                        SaveScrollPosition="true"></Scrolling>
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadPane>
        </telerik:RadSplitter>
           <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="InstrumentFault" runat="server" Height="1000px" Width="680px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                    Title="仪器添加配件" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>

    </form>
</body>
</html>
