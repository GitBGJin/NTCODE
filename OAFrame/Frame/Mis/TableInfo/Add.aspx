<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfo.Add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mis表信息</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" EnableAjax="true"></f:PageManager>
        <f:Panel ID="Panel1" runat="server" ShowBorder="False" ShowHeader="false" BodyPadding="5px"
            AutoScroll="true" Layout="Fit">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnNew" Text="保存" runat="server" Icon="Disk" ValidateForms="Form3"
                            OnClick="btnNew_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Form Title="基本信息" BodyPadding="5px" ID="Form3" ShowHeader="false" ShowBorder="false"
                    runat="server" LabelWidth="80" EnableCollapse="true">
                    <Rows>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" ID="txt_ShowTableName" Label="显示名称" Required="true" ShowRedStar="true">
                                </f:TextBox>
                            </Items>
                            <Items>
                                <f:TextBox runat="server" ID="txt_SQLTableName" Label="SQL表名" Required="true" ShowRedStar="true">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:DropDownList runat="server" ID="ddl_TableType" Label="表类型">
                                    <f:ListItem Text="表" Value="1" Selected="true"></f:ListItem>
                                    <f:ListItem Text="视图" Value="2"></f:ListItem>
                                </f:DropDownList>
                                <f:DropDownList runat="server" ID="ddl_IsPrimary" Label="是否主表">
                                    <f:ListItem Text="是" Value="1"></f:ListItem>
                                    <f:ListItem Text="否" Value="0" Selected="true"></f:ListItem>
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:DropDownList runat="server" ID="ddl_ParentGuid" Label="主表标识(仅从表填写)">
                                </f:DropDownList>
                                <f:TextBox runat="server" ID="txt_ForeignKey" Label="子表外键(仅从表填写)">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:NumberBox runat="server" ID="txt_LabelWidth" Label="文本宽度" DecimalPrecision="0"
                                    MinValue="1" MaxValue="300" Text="80">
                                </f:NumberBox>
                                <f:DropDownList runat="server" ID="ddl_DeleteType" Label="删除方式">
                                    <f:ListItem Text="物理删除" Value="1" Selected="true"></f:ListItem>
                                    <f:ListItem Text="逻辑删除" Value="2"></f:ListItem>
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:NumberBox runat="server" ID="txt_SonTableHeight" Label="子窗体高度" DecimalPrecision="0"
                                    MinValue="1">
                                </f:NumberBox>
                                <f:DropDownList runat="server" ID="ddl_WindowTarget" Label="弹窗方式">
                                    <f:ListItem Text="Self" Value="Self" Selected="true"></f:ListItem>
                                    <f:ListItem Text="Parent" Value="Parent"></f:ListItem>
                                    <f:ListItem Text="Top" Value="Top"></f:ListItem>
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:DropDownList runat="server" ID="ddl_AutoScroll" Label="自动滚动条">
                                    <f:ListItem Text="启用" Value="1" Selected="true"></f:ListItem>
                                    <f:ListItem Text="禁用" Value="0"></f:ListItem>
                                </f:DropDownList>
                                <f:DropDownList runat="server" ID="ddl_FormAttach" Label="是否有附件">
                                    <f:ListItem Text="有" Value="1" Selected="true"></f:ListItem>
                                    <f:ListItem Text="无" Value="0"></f:ListItem>
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextArea runat="server" ID="txt_SortSql" Label="排序sql" Height="40">
                                </f:TextArea>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextArea runat="server" ID="txt_FilterSql" Label="过滤sql" Height="40">
                                </f:TextArea>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:ContentPanel runat="server" ID="cp1" ShowBorder="false" ShowHeader="false">
                                    <table>
                                        <tr>
                                            <td width="80"></td>
                                            <td>示例： AddUserGuid='@UserGuid'<br>
                                                目前只支持@UserGuid、@DisplayName、@DeptGuid、@DeptName
                                            </td>
                                        </tr>
                                    </table>
                                </f:ContentPanel>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:NumberBox runat="server" ID="txt_FormWidth" Label="弹窗宽度" DecimalPrecision="0"
                                    MinValue="1">
                                </f:NumberBox>
                                <f:NumberBox runat="server" ID="txt_FormHeight" Label="弹窗高度" DecimalPrecision="0"
                                    MinValue="1">
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
