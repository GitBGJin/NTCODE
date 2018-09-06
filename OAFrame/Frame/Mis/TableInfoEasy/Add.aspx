<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfoEasy.Add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>数据表信息</title>
</head>
<body>
    <form id="form1" runat="server">
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
            <f:Form Title="基本信息" BodyPadding="5px" ID="Form3" ShowHeader="false" ShowBorder="false" runat="server" LabelWidth="90" EnableCollapse="true">
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:TextBox runat="server" ID="txt_ShowTableName" Label="数据表名称" Required="true" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:RadioButtonList runat="server" ID="rbl_FormAttach" Label="是否有附件" Required="true" ShowRedStar="true" Width="177">
                                <f:RadioItem Text="有" Value="1" Selected="true"></f:RadioItem>
                                <f:RadioItem Text="无" Value="0"></f:RadioItem>
                            </f:RadioButtonList>
                            <f:NumberBox runat="server" ID="txt_LabelWidth" Label="文字宽度" Text="80" Required="true" ShowRedStar="true" DecimalPrecision="0">
                            </f:NumberBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:NumberBox runat="server" ID="txt_FormWidth" Label="弹窗宽度" Text="800" Required="true" ShowRedStar="true" DecimalPrecision="0">
                            </f:NumberBox>
                            <f:NumberBox runat="server" ID="txt_FormHeight" Label="弹窗高度" Text="600" Required="true" ShowRedStar="true" DecimalPrecision="0">
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
