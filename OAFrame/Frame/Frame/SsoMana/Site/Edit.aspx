<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SsoMana.Site.Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>站点设置</title>
    <style>
        .x-docked-bottom {
            border-top-width: 10px;
        }

        x-toolbar {
            border-top-width: 0px;
        }

        .x-toolbar-default.x-toolbar {
            border-top-width: 0px;
        }

        .x-noborder-rbl {
            border-top-width: 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="5px">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnNew" Text="确定" runat="server" Icon="Accept" OnClick="Save_Click"
                            ValidateForms="Form3">
                        </f:Button>
                        <%--<f:ContentPanel runat="server" ID="tb11" ShowBorder="false" ShowHeader="false">
                            <div style="background-color: RGB(223,234,242); width: 550px; text-align: right;">
                            </div>
                        </f:ContentPanel>--%>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Form Title="Form1" BodyPadding="1px" ID="Form3"
                    ShowHeader="false" ShowBorder="false" runat="server">
                    <Rows>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Required="True" Label="站点名称" ShowRedStar="True" ID="txt_SiteName" MaxLength="30">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Required="True" Label="内网地址" ShowRedStar="True" ID="txt_SiteUrlLan" MaxLength="30">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Required="True" Label="外网地址" ShowRedStar="True" ID="txt_SiteUrlWan" MaxLength="30">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Required="True" Label="8位密钥" ShowRedStar="True" ID="txt_SecurityKey" MaxLength="30">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:NumberBox runat="server" MinValue="0" NoDecimal="true" Label="排序号" ID="txt_SortNumber"
                                    RegexMessage="大于等于0的整数" Required="true" Text="0" ShowRedStar="True" MaxLength="4">
                                </f:NumberBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextArea runat="server" Label="备注" ID="txt_Note" Height="60" MaxLength="100">
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

