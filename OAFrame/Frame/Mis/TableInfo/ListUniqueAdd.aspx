<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListUniqueAdd.aspx.cs"
    Inherits="Com.Sinoyd.Mis.WebUI.TableInfo.ListUniqueAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mis表唯一性</title>
</head>
<body>
    <form id="form1" runat="server">
    <script>

    </script>
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" EnableAjax="true"></f:PageManager>
    <f:Panel ID="Panel1" runat="server" ShowBorder="False" ShowHeader="false" BodyPadding="5px" AutoScroll="true">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnNew" Text="保存" runat="server" Icon="Disk" ValidateForms="Form3" OnClick="btnNew_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Form Title="基本信息" BodyPadding="5px" ID="Form3" ShowHeader="false" ShowBorder="false" runat="server" LabelWidth="80" EnableCollapse="true">
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:TextBox runat="server" ID="txt_UniqueValueList" Label="唯一性">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:ContentPanel runat="server" ID="cp1" ShowBorder="false" ShowHeader="false">
                                <table>
                                    <tr>
                                        <td width="80">
                                        </td>
                                        <td>
                                            示例：Name+Tel
                                        </td>
                                    </tr>
                                </table>
                            </f:ContentPanel>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:NumberBox runat="server" ID="txt_SortNumber" Label="排序值" Text="0">
                            </f:NumberBox>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
