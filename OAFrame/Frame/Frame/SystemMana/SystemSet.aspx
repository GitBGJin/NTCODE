<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemSet.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SystemMana.SystemSet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="panel1" />
        <f:Panel runat="server" ID="panel1" Layout="VBox" BoxConfigPosition="Start" BoxConfigAlign="Stretch" ShowBorder="false" ShowHeader="false">
            <Items>
                <f:Panel runat="server" ID="form2" BoxFlex="1" Layout="HBox" BoxConfigPosition="Start" BoxConfigAlign="Stretch" ShowBorder="false" ShowHeader="false" BodyPadding="1px">
                    <Items>
                        <f:Panel ID="Panel2" runat="server" Title="用户密码管理" BoxFlex="1" BoxMargin="4px 4px 0px 4px" EnableCollapse="false" BodyPadding="4px">
                            <Items>
                                <f:Form runat="server" ID="form11" BodyPadding="4px" ShowBorder="false" ShowHeader="false">
                                    <Rows>
                                        <f:FormRow>
                                            <Items>
                                                <f:TextBox runat="server" ID="txt_PasswordOld" ShowLabel="true" Label="原密码" Required="true" ShowRedStar="true" TextMode="Password"  MaxLength="30"></f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow>
                                            <Items>
                                                <f:TextBox runat="server" ID="txt_PasswordNew1" ShowLabel="true" Label="新密码" Required="true" ShowRedStar="true" TextMode="Password"  MaxLength="30"></f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow>
                                            <Items>
                                                <f:TextBox runat="server" ID="txt_PasswordNew2" ShowLabel="true" Label="密码确认" Required="true" ShowRedStar="true" TextMode="Password"  MaxLength="30"></f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow>
                                            <Items>
                                                <f:Button runat="server" ID="btnSave" Text="保存密码" OnClick="btnSave_Click" ValidateForms="form11"></f:Button>
                                            </Items>
                                        </f:FormRow>
                                    </Rows>
                                </f:Form>
                            </Items>
                        </f:Panel>
                         <f:Panel ID="Panel5" runat="server" Title="导航方式设置" BoxFlex="1" BoxMargin="4px" EnableCollapse="false" Hidden="true">
                            <Items>
                                <f:Form runat="server" ID="form3" BodyPadding="4px" ShowBorder="false" ShowHeader="false">
                                    <Rows>
                                        <f:FormRow>
                                            <Items>
                                                <f:DropDownList runat="server" ID="ddl_NavigationType" Label="导航方式"></f:DropDownList>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow>
                                            <Items>
                                                <f:DropDownList runat="server" ID="ddl_DefaultMenuType" Label="默认菜单">
                                                    <f:ListItem Selected="true" Text="标准菜单" Value="1" />
                                                    <f:ListItem Text="个人菜单" Value="2" />
                                                </f:DropDownList>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow>
                                            <Items>
                                                <f:Button runat="server" ID="btnNavigationType" Text="保存设置" OnClick="btnNavigationType_Click"></f:Button>
                                                <f:Button runat="server" ID="btnNavigationSet" Text=" 个性导航设置 " OnClientClick="openModulePersonal();"></f:Button>
                                            </Items>
                                        </f:FormRow>
                                    </Rows>
                                </f:Form>
                            </Items>
                        </f:Panel>
                       
                    </Items>
                </f:Panel>
                <f:Panel runat="server" ID="Panel4" BoxFlex="1" Layout="HBox" BoxConfigPosition="Start" BoxConfigAlign="Stretch" ShowBorder="false" ShowHeader="false" BodyPadding="1px" Hidden="true">
                    <Items>
                        <f:Panel ID="Panel3" runat="server" Title="主题设置" BoxFlex="1" BoxMargin="4px 4px 0px 4px" EnableCollapse="false" BodyPadding="4px">
                            <Items>
                                <f:RadioButtonList runat="server" ID="rbl_Skin" Label="&nbsp;&nbsp;&nbsp;&nbsp;选择主题" Width="200" ShowLabel="true" ColumnNumber="1"></f:RadioButtonList>
                                <f:Button runat="server" ID="btnSkin" Text=" 保 存 " OnClick="btnSkin_Click"></f:Button>
                            </Items>
                        </f:Panel>
                        <f:Panel ID="Panel6" runat="server" Title="首页设置" BoxFlex="1" BoxMargin="4px" EnableCollapse="false" BodyPadding="1px">
                            <Items>
                                <f:Form runat="server" ID="form4" BodyPadding="4px" ShowBorder="false" ShowHeader="false">
                                    <Rows>
                                        <%--<f:FormRow>
                                            <Items>
                                                <f:TextBox runat="server" ID="TextBox1" Label="首页地址"></f:TextBox>
                                            </Items>
                                        </f:FormRow>--%>
                                        <f:FormRow>
                                            <Items>
                                                <f:TextBox runat="server" ID="txt_HomePageUrl" Label="首页地址"></f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow>
                                            <Items>
                                                <f:Button runat="server" ID="btnHomePageSet" Text="保存设置" OnClick="btnHomePageSet_Click"></f:Button>
                                            </Items>
                                        </f:FormRow>
                                    </Rows>
                                </f:Form>
                            </Items>
                        </f:Panel>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
        <script type="text/javascript">

            var basePath = '<%= ResolveUrl("~/") %>';
            function openModulePersonal() {
                parent.showwindows('ModulePersonalSet', basePath + 'SystemMana/ModulePersonal/ModuleFrame.aspx', '个性导航设置');
            }

        </script>
    </form>
</body>
</html>
