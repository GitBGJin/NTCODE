<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListQueryView.aspx.cs" Inherits="TK.Mis.Web.TableInfoSet.ListQueryView"
    Debug="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script>
        function copyToClipboard(value)
        {
            window.clipboardData.setData('text', value);
        }

        function showtab(b, c)
        {
            parent.window.showwindows(Math.random().toString(), b, c);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
    </script>
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
    <f:Panel ID="Panel1" runat="server" BodyPadding="3px" ShowBorder="false" ShowHeader="false" Height="300px" Layout="Anchor">
        <Items>
            <f:ContentPanel ID="ContentPanel1" runat="server" BodyPadding="5px" ShowBorder="false" ShowHeader="false" Title="ContentPanel">
                <table>
                    <tr>
                        <td style="display: none;">
                            <table id="table1" runat="server">
                            </table>
                        </td>
                        <td>
                            &nbsp;&nbsp;填报单位：
                        </td>
                        <td width="200">
                            <f:DropDownList runat="server" ID="ddl_ApplyOUName" Width="200">
                            </f:DropDownList>
                        </td>
                        <td>
                            &nbsp;&nbsp;是否上报
                        </td>
                        <td width="80">
                            <f:DropDownList runat="server" ID="ddl_Sfsb" Width="80">
                                <f:ListItem Selected="true" Value="所有选项" Text="所有选项"></f:ListItem>
                                <f:ListItem Value="是" Text="是"></f:ListItem>
                                <f:ListItem Value="否" Text="否"></f:ListItem>
                            </f:DropDownList>
                        </td>
                        <td>
                            <f:Button ID="btnImport" Text="查找" runat="server" Icon="Magnifier" OnClick="btnSearch_Click">
                            </f:Button>
                        </td>
                    </tr>
                </table>
            </f:ContentPanel>
            <f:Panel ID="Panel2" ShowBorder="True" ShowHeader="false" runat="server" AnchorValue="100% -36" Layout="Fit">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server" Hidden="true">
                        <Items>
                            <f:Button ID="btnNew" Text="添加" runat="server" Icon="Add" EnablePostBack="true" Hidden="true">
                            </f:Button>
                            <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true" OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的数据吗？" Hidden="true">
                            </f:Button>
                            <f:Button ID="btnExport" Text="导出" runat="server" Icon="PageExcel" EnableAjax="false" OnClick="btnExport_OnClick" Hidden="true">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Grid ID="Grid1" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False" AllowSorting="true" SortDirection="ASC" AllowPaging="true" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange" OnSort="Grid1_Sort" EnableTextSelection="true" RowVerticalAlign="Middle">
                        <Columns><f:RowNumberField></f:RowNumberField>
                            <f:TemplateField HeaderText="填报单位" ColumnID="AddDeptName" TextAlign="Center" Width="300">
                                <ItemTemplate>
                                    <div style="text-align: left;">
                                        <%# DataBinder.Eval(Container.DataItem, "AddDeptName").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="80" HeaderText="是否上报" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: Center;">
                                        <a style="text-decoration: none; color: #000;" href="#" onclick="javascript:showtab('<%# getApplyHref(DataBinder.Eval(Container.DataItem, "AddDeptGuid").ToString()) %>','<%# DataBinder.Eval(Container.DataItem, "AddDeptName").ToString()%>');">
                                            <%# getApplyCount(DataBinder.Eval(Container.DataItem, "AddDeptGuid").ToString())%></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                        </Columns>
                    </f:Grid>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    <f:Window ID="Window1" Title="Add" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Parent" Width="600px" Height="400px" OnClose="Window1_OnClose" Hidden="true">
    </f:Window>
    <f:Window ID="Window2" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Parent" Width="600px" Height="400px" OnClose="Window2_OnClose" Hidden="true">
    </f:Window>
    </form>
</body>
</html>
