<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SsoMana.Config.Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>代码项设置</title>
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
                    ShowHeader="false" ShowBorder="false" runat="server">
                    <Rows>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Required="True" Label="配置项名称" ShowRedStar="True" ID="txt_ConfigName" Readonly="true" MaxLength="30">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Required="True" Label="配置项值" ShowRedStar="True" ID="txt_ConfigValue" MaxLength="30">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextArea runat="server" Required="True" Label="配置项值" ShowRedStar="True" ID="txt_Note">
                                </f:TextArea>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>

