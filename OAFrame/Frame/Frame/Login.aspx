<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<embed id="s_simnew31" type="application/npsyunew3-plugin" hidden="true"> </embed><!--创建firefox,chrome等插件-->
<head id="Head1" runat="server">
    <title>江苏远大信息股份有限公司</title>
    <style type="text/css">
        body {
            background: url(Content/Themes/Login/images/login_bg.jpg);
            margin: 0;
        }

        .topbg {
            background: url(Content/Themes/Login/images/main_bg.jpg) no-repeat;
            vertical-align: top;
        }

        td.field_label {
            font-size: 16px;
            line-height: 30px;
            color: #666666;
        }

        td.field_label2 {
            font-size: 14px;
            line-height: 30px;
            color: #666666;
        }

        .copyright {
            font-size: 12px;
            color: #999;
        }

        .loginTextBox {
            border-right: 1px solid #94C7E7;
            border-top: 1px solid #94C7E7;
            border-left: 1px solid #94C7E7;
            border-bottom: 1px solid #94C7E7;
            height: 26px;
            line-height: 26px;
            width: 192px;
            font-size: 16px;
            background-color: #FFFFFF;
            background-image: url(Content/Themes/Login/images/text-bg.gif);
            background-repeat: repeat-x;
            vertical-align: middle;
            margin: 0;
            padding: 1px 3px;
        }

        .loginTextBox1 {
            border-right: 1px solid #94C7E7;
            border-top: 1px solid #94C7E7;
            border-left: 1px solid #94C7E7;
            border-bottom: 1px solid #94C7E7;
            height: 26px;
            line-height: 26px;
            width: 100px;
            font-size: 16px;
            background-color: #FFFFFF;
            background-image: url(Content/Themes/Login/images/text-bg.gif);
            background-repeat: repeat-x;
            vertical-align: middle;
            margin: 0;
            padding: 1px 3px;
        }

        .cssButton1 {
            background-image: url(Content/Themes/Login/images/btn_login.jpg);
            width: 130px;
            height: 44px;
            border: 0px;
        }

        .cssButton2 {
            background-image: url(Content/Themes/Login/images/btn_reset.jpg);
            width: 130px;
            height: 44px;
            border: 0px;
        }
    </style>
    <script type="text/javascript" src="Content/js/jquery-1.10.2.min.js"></script>
</head>
<body>
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
                        //window.alert("未发现加密锁，请插入加密锁");
                    }
                    else {

                        //获取锁的ID
                        // alert(toHex(s_simnew31.GetID_1(DevicePath)) + toHex(s_simnew31.GetID_2(DevicePath)));
                        $("#KeyID").val(toHex(s_simnew31.GetID_1(DevicePath)) + toHex(s_simnew31.GetID_2(DevicePath)));
                        //frmlogin.submit ();
                    }
                }

                catch (e) {
                    //alert(e.name + ": " + e.message + "。可能是没有安装相应的控件或插件");
                    return false;
                }

            }
        </script>
        <script type="text/javascript" language="javascript">

            function Clear() {
                $("#txt_LoginID").val("");
                $("#txt_Password").val("");
                $("#txt_LoginID").focus();
                return false;
            }
            //function check() {
            //    if ($("#txt_LoginID").val() == "") {
            //        alert("用户名不能为空！");
            //        $("#txt_LoginID").focus();
            //        return false;
            //    }
            //    if ($("#txt_Password").val() == "") {
            //        alert("密码不能为空！");
            //        $("#txt_Password").focus();
            //        return false;
            //    }
            //    return true;
            //}

            //function showMsg(msg) {
            //    document.getElementById("msg").innerHTML = msg || "&nbsp;";
            //}

            function Login() {
                if ($("#txt_LoginID").val() == "") {
                    alert("请输入用户名！");
                    $("#txt_LoginID").focus();
                    return false;
                }
                if ($("#txt_Password").val() == "") {
                    alert("请输入密码！");
                    $("#txt_Password").focus();
                    return false;
                }
                jQuery.ajax({
                    type: "post",
                    url: "Handle/Login.ashx?LoginID=" + escape($("#txt_LoginID").val()) + "&Password=" + escape($("#txt_Password").val()) + "&HardwareKey=" + $("#KeyID").val() + "&Captcha=" + escape($("#txt_Captcha").val()) + "&id=" + Math.random(),
                    dataType: "json",
                    success: function (data) {
                        if (data[0].Status == "1") {
                            //alert('登录成功！');
                            window.location = "FrameAll2.aspx?DetailModuleGuid=ca7966ae-0e80-4b22-8191-edc1906a753a";
                        }
                        else {
                            alert(data[0].Message);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(errorThrown);
                    }
                });
            }

            function GetNewCaptcha() {
                //alert(13);
                jQuery.ajax({
                    type: "post",
                    url: "Handle/GetNewCaptcha.ashx?w=80&h=30&id=" + Math.random(),
                    dataType: "json",
                    success: function (data) {
                        alert(data);
                        //if (data[0].Status == "1") {
                        //    //alert('登录成功！');
                        //    window.location = "FrameAll.aspx";
                        //}
                        //else {
                        //    alert(data[0].Message);
                        //}
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(errorThrown);
                    }
                });
            }

            $(document).ready(function () {
                $("#txt_LoginID").focus();
                SetKeyID();

                alert($("#KeyID").val());
            });

            $(function () {
                document.onkeydown = function (e) {
                    var ev = document.all ? window.event : e;
                    if (ev.keyCode == 13) {
                        if ($("#txt_LoginID").val() == "") {
                            $("#txt_LoginID").focus();
                            return;
                        }
                        if ($("#txt_Password").val() == "") {
                            $("#txt_Password").focus();
                            return;
                        }

                        Login();
                    }
                }
            });

            //$('#txt_LoginID').keydown(function (e) {
            //    alert(1);
            //    if (e.keyCode == 13) {
            //        alert(2);
            //        if ($("#txt_Password").val() == "") {
            //            $("#txt_Password").focus();
            //        }
            //        else
            //        {
            //            Login();
            //        }
            //    }
            //});
        </script>
        <input name="KeyID" type="hidden" id="KeyID" runat="server">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="1000">
            <tbody>
                <tr>
                    <td class="topbg" height="600">
                        <table style="margin-top: 159px;" align="center" border="0" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td>
                                        <img alt="" src="Content/Themes/Login/images/login.jpg" height="58" width="386" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table style="margin-top: 5px;" align="center" border="0" cellpadding="0" cellspacing="0"
                            width="300">
                            <tbody>
                                <tr>
                                    <td width="80">&nbsp;&nbsp;
                                    </td>
                                    <td style="font-size: 12px; color: red;">
                                        <div id="msg">
                                            &nbsp;
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field_label" height="50" width="80">用户名
                                    </td>
                                    <td colspan="2">
                                        <div id="name_box">
                                            <asp:TextBox runat="server" ID="txt_LoginID" CssClass="loginTextBox" MaxLength="30"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field_label" height="50" width="80">密<span style="padding-left: 15px;">码</span>
                                    </td>
                                    <td colspan="2">
                                        <div id="pwd_box">
                                            <asp:TextBox runat="server" ID="txt_Password" CssClass="loginTextBox" TextMode="Password" MaxLength="30"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="TrCaptcha" runat="server">
                                    <td class="field_label" height="50" width="80">验证码
                                    </td>
                                    <td width="100">
                                        <asp:TextBox runat="server" ID="txt_Captcha" Width="100" MaxLength="30" CssClass="loginTextBox1"></asp:TextBox></td>
                                    <td width="120">
                                        <img runat="server" id="imgCaptcha" />
                                        <%-- <asp:ImageButton runat="server" ID="img_Captcha" ImageUrl="" Enabled="false" />--%>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="padding-top: 20px; padding-left: 20px;">
                                        <input type="button" class="cssButton1" onclick="Login()" />
                                        <input type="button" class="cssButton2" onclick="Clear()" />
                                        <%--<asp:ImageButton ID="btnLogin" runat="server" ImageUrl="Content/Themes/Login/images/btn_login.jpg"
                                            OnClientClick="Login()" />
                                        <asp:ImageButton ID="btnReset" runat="server" ImageUrl="Content/Themes/Login/images/btn_reset.jpg"
                                            OnClientClick="Clear()" />--%>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table style="margin-top: 120px;" align="center" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="copyright" style="line-height: 1.8em; color: #23238e;" valign="top">版权所有：江苏远大信息股份有限公司<%-- 技术支持： <a href="http://www.sinoyd.com/" target="_blank">www.sinoyd.com</a>--%>
                                </td>
                                <td class="copyright" style="padding-left: 50px; line-height: 1.8em; color: #23238e; font-weight: bold;"
                                    valign="top"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>
