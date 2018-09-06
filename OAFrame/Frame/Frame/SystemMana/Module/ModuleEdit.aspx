<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModuleEdit.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SystemMana.Module.ModuleEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>菜单信息</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="5px">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnNew" Text="保存" runat="server" Icon="Disk" OnClick="Save_Click"
                            ValidateForms="Form3">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Form Title="Form1" BodyPadding="5px" ID="Form3"
                    ShowHeader="false" ShowBorder="false" runat="server" LabelWidth="80">
                    <Rows>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Required="True" Label="菜单名称" ShowRedStar="True" ID="txt_ModuleName"  MaxLength="30">
                                </f:TextBox>
                                <f:Label ID="NodeSelect" runat="server" Label="&nbsp;上级菜单">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Label="浏览标识" ID="txt_ModuleCode"  MaxLength="30">
                                </f:TextBox>
                                <f:DropDownList Label="&nbsp;菜单图标" ID="ddl_MenuImg" runat="server">
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:DropDownList Label="所属站点" ID="ddl_SiteGuid" runat="server" ShowRedStar="true" Required="true">
                                </f:DropDownList>
                                <f:RadioButtonList runat="server" ID="ddl_IsBlank" Label="&nbsp;打开方式">
                                    <f:RadioItem Text="内部" Value="0" Selected="true" />
                                    <f:RadioItem Text="外部" Value="1" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Label="菜单地址" ID="txt_ModuleUrl"  MaxLength="100">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:NumberBox runat="server" MinValue="0" NoDecimal="true" Label="排序号" ID="txt_SortNumber"
                                    RegexMessage="大于等于0的整数" Required="true" Text="0" ShowRedStar="True" MaxValue="10000">
                                </f:NumberBox>
                                <f:RadioButtonList runat="server" ID="rbl_IsEnabled" Label="&nbsp;启用">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextArea ID="txt_Note" runat="server" Height="60" Label="备注" MaxLength="100">
                                </f:TextArea>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
        <f:Window ID="Window2" Title="选择人员" EnableIFrame="true" runat="server"
            Target="Self" IsModal="True" Width="550px" Height="450px" Hidden="true">
        </f:Window>
    </form>
</body>
</html>
