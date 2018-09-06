<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListQuery.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfo.ListQuery"
    Debug="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
    </script>
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
    <f:Panel ID="Panel1" runat="server" BodyPadding="3px" ShowBorder="false" ShowHeader="false"
        Height="300px" Layout="Anchor">
        <Items>
            <f:ContentPanel ID="ContentPanel1" runat="server" BodyPadding="5px" ShowBorder="false"
                ShowHeader="false" Title="ContentPanel">
                <table>
                    <tr>
                        <td>
                            <table id="table1" runat="server">
                            </table>
                        </td>
                        <td>
                            <f:Button ID="btnImport" Text="查找" runat="server" Icon="Magnifier" OnClick="btnSearch_Click">
                            </f:Button>
                        </td>
                    </tr>
                </table>
            </f:ContentPanel>
            <f:Panel ID="Panel2" ShowBorder="True" ShowHeader="false" runat="server" AnchorValue="100% -36"
                Layout="Fit">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnNew" Text="添加" runat="server" Icon="Add" EnablePostBack="true" Hidden="True"> 
                            </f:Button>
                            <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true"
                                OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的数据吗？" Hidden="True">
                            </f:Button>
                            <f:Button ID="btnExport" Text="导出" runat="server" Icon="PageExcel" EnableAjax="false"
                                OnClick="btnExport_OnClick" Hidden="true">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Grid ID="Grid1" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False"
                        AllowSorting="true" SortDirection="ASC" AllowPaging="true" runat="server" EnableCheckBoxSelect="true"
                        DataKeyNames="RowGuid" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableTextSelection="true" SortField="ID" EnableHeaderMenu="false">
                        <Columns>
                            <f:RowNumberField Width="30"></f:RowNumberField>
                        </Columns>
                    </f:Grid>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    <f:Window ID="Window1" Title="Add" EnableIFrame="true" runat="server" EnableCollapse="false"
        EnableDrag="false" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Self"
        Width="600px" Height="400px" OnClose="Window1_OnClose" Hidden="true">
    </f:Window>
    <f:Window ID="Window2" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="false"
        EnableDrag="false" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Self"
        Width="600px" Height="400px" OnClose="Window2_OnClose" Hidden="true">
    </f:Window>
    </form>
</body>
</html>
