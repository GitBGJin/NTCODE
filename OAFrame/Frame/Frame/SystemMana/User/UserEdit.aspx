<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SystemMana.User.UserEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<embed id="s_simnew31" type="application/npsyunew3-plugin" hidden="true"> </embed><!--创建firefox,chrome等插件-->
<head id="Head1" runat="server">
    <title>用户信息</title>
    <script type="text/javascript" src="../../Content/js/jquery-1.10.2.min.js"></script>
</head>
<body onload="SetKeyID()">
    <form id="form1" runat="server">
        <script language="javascript">

            var digitArray = new Array('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f');

            function toHex(n) {

                var result = ''
                var start = true;

                for (var i = 32; i > 0;) {
                    i -= 4;
                    var digit = (n >> i) & 0xf;

                    if (!start || digit != 0) {
                        start = false;
                        result += digitArray[digit];
                    }
                }

                return (result == '' ? '0' : result);
            }

            function SetKeyID() {
                try {
                    var DevicePath, mylen, ret;
                    var s_simnew31;
                    //创建插件或控件
                    if (navigator.userAgent.indexOf("MSIE") > 0 && !navigator.userAgent.indexOf("opera") > -1) {
                        s_simnew31 = new ActiveXObject("Syunew3A.s_simnew3");
                    }
                    else {
                        s_simnew31 = document.getElementById('s_simnew31');
                    }
                    DevicePath = s_simnew31.FindPort(0);//'来查找加密锁，0是指查找默认端口的锁
                    if (s_simnew31.LastError != 0) {
                        window.alert("未发现加密锁，请插入加密锁");
                    }
                    else {
                        //获取锁的ID
                        //alert("true");
                        $("#<%=KeyID.ClientID%>").val(toHex(s_simnew31.GetID_1(DevicePath)) + toHex(s_simnew31.GetID_2(DevicePath)));
                    }
                }

                catch (e) {
                    alert(e.name + ": " + e.message + "。可能是没有安装相应的控件或插件");
                    return false;
                }

            }
        </script>
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" ShowBorder="False" ShowHeader="false"
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
                    ShowHeader="false" ShowBorder="false" runat="server" LabelWidth="75">
                    <Rows>
                        <f:FormRow>
                            <Items>
                                <f:Label ID="lbl_OrgName" runat="server" Label="&nbsp;所属单位">
                                </f:Label>
                                <f:Label ID="lbl_DeptName" runat="server" Label="&nbsp;&nbsp;所属部门">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Required="True" Label="&nbsp;登录名" ShowRedStar="True" ID="txt_LoginID" MaxLength="30">
                                </f:TextBox>
                                <f:TextBox runat="server" Required="True" Label="&nbsp;&nbsp;姓名" ShowRedStar="True" ID="txt_DisplayName" MaxLength="30">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                         <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" TextMode="Password" Required="True" Label="&nbsp;登录密码" ShowRedStar="True" ID="txt_Password1" MaxLength="30">
                                </f:TextBox>
                                <f:TextBox runat="server" TextMode="Password" Required="True" Label="&nbsp;&nbsp;确认密码" ShowRedStar="True" ID="txt_Password2" MaxLength="30">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:RadioButtonList runat="server" ID="rbl_IsEnabled" Label="&nbsp;启用">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                                <f:NumberBox runat="server" MinValue="0" NoDecimal="true" Label="&nbsp;&nbsp;排序号" ID="txt_SortNumber"
                                    RegexMessage="大于等于0的整数" Required="true" Text="0" ShowRedStar="True" MaxValue="10000">
                                </f:NumberBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow Hidden="true">
                            <Items>
                                <f:ContentPanel runat="server" ID="cp1" ShowBorder="false" ShowHeader="false" Hidden="true">
                                    <table style="display:none;">
                                        <tr>
                                            <td>
                                                <input type="text" id="KeyID" runat="server" /></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </f:ContentPanel>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Label="&nbsp;加密狗ID" ID="txt_HardwareKey" MaxLength="30">
                                </f:TextBox>
                                <f:Button runat="server" ID="btnSetKeyID" Text="获取" OnClick="btnSetKeyID_Click"></f:Button>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextArea ID="txt_Note" runat="server" Height="50" Label="&nbsp;备注">
                                </f:TextArea>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:CheckBoxList runat="server" ID="chk_RoleList" ColumnNumber="5" Label="&nbsp;角色"></f:CheckBoxList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow Hidden="true">
                            <Items>
                                <f:TextBox runat="server" Required="True" Label="&nbsp;单位标识" ShowRedStar="True" ID="txt_OrgGuid" Hidden="true">
                                </f:TextBox>
                                <f:TextBox runat="server" Required="True" Label="&nbsp;&nbsp;部门标识" ShowRedStar="True" ID="txt_DeptGuid" Hidden="true">
                                </f:TextBox>
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
