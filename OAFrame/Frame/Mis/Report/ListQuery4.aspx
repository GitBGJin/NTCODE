<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListQuery4.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Report.ListQuery4"
    Debug="true" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <script>
        function showtab1(a, b, c) {
            parent.window.showwindows(a, b, c);
        }
    </script>
    <style>
        .x-grid-row-summary .x-grid-cell-inner
        {
            font-weight: bold;
            color: red;
        }
    </style>
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
                            <f:Button ID="btnImport" Text="统计" runat="server" Icon="Report" OnClick="btnSearch_Click">
                            </f:Button>
                        </td>
                    </tr>
                </table>
            </f:ContentPanel>
            <f:Panel ID="Panel2" ShowBorder="True" ShowHeader="false" runat="server" AnchorValue="100% -46"
                Layout="Fit">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnExport" Text="导出Excel" runat="server" Icon="PageExcel" OnClick="ExportExcel"
                                EnableAjax="false" DisableControlBeforePostBack="false">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Grid ID="Grid1" Title="Grid1" ShowBorder="false" ShowHeader="False" runat="server"
                        EnableCheckBoxSelect="false" IsDatabasePaging="false" EnableTextSelection="true"
                        EnableHeaderMenu="false" DataKeyNames="RowGuid">
                        <Columns>
                        </Columns>
                    </f:Grid>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
