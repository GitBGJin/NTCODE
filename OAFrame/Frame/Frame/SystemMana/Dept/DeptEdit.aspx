<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptEdit.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SystemMana.Dept.DeptEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>单位编辑</title>
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
                                <f:Label ID="NodeSelect" runat="server" Label="&nbsp;上级部门">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Required="True" Label="&nbsp;部门名称" ShowRedStar="True" ID="txt_DeptName" MaxLength="30">
                                </f:TextBox>
                                <f:NumberBox runat="server" MinValue="0" NoDecimal="true" Label="&nbsp;&nbsp;&nbsp;排序号" ID="txt_SortNumber"
                                    RegexMessage="大于等于0的整数" Required="true" Text="0" ShowRedStar="True" MaxLength="4">
                                </f:NumberBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextArea ID="txt_Note" runat="server" Height="80" Label="&nbsp;备注" MaxLength="200">
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
