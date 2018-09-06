<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetailEdit.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SystemMana.CodeDictionary.ModuleEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>数据字典明细</title>
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
                    ShowHeader="false" ShowBorder="false" runat="server" LabelWidth="95">
                    <Rows>
                        <f:FormRow>
                            <Items>
                                <f:Label ID="NodeSelect" runat="server" Label="&nbsp;上级目录">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Required="True" Label="&nbsp;明细名称" ShowRedStar="True" ID="txt_DetailName" MaxLength="30">
                                </f:TextBox>
                                <f:NumberBox runat="server" MinValue="0" NoDecimal="true" Label="&nbsp;排序号" ID="txt_SortNumber"
                                    RegexMessage="大于等于0的整数" Required="true" Text="0" ShowRedStar="True" MaxValue="10000">
                                </f:NumberBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextArea ID="txt_Note" runat="server" Height="60" Label="&nbsp;备注" MaxLength="100">
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
