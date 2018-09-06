<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SmartEP.WebUI.Portal.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="overflow: hidden;">
<head id="Head1" runat="server">
    <title>南通市环境质量自动监测监控系统</title>
    <link href="../Resources/CSS/login.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/JavaScript/JQuery/jquery-1.9.0.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnOK" style="background-color: none; overflow: hidden;">
        <script type="text/javascript">
            if (top != self) {
                if (top.location != self.location)
                    top.location = self.location;
            }
        </script>
        <script type="text/javascript">
            $("document").ready(function () {
                $("#copyright").css({ "top": ($("body").height() - $("#copyright").height() - 5) + "px" });
                $("#divBgImg").css({ "height": ($("body").height()) + "px" });
            });
        </script>
        <style type="text/css">
			    .remberMe{
            width:50%;
            margin-top:10px;
			      margin-left:-10px;
           padding:0;
           text-align:center;
           font-size:14px;
          
			  }
          #passRem
          {
            
            margin-right:5px;
            
            
          }
          #texts
          {
            margin:0;
            padding:0;
          }
		  </style>
        <telerik:radscriptmanager id="RadScriptManager1" runat="server">
        </telerik:radscriptmanager>
        <telerik:radajaxmanager id="ajaxManager" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnOK">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadTxtUser" />
                        <telerik:AjaxUpdatedControl ControlID="RadTxtPwd" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnCancel">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadTxtUser" />
                        <telerik:AjaxUpdatedControl ControlID="RadTxtPwd" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:radajaxmanager>
        <telerik:radinputmanager id="RadInputManager1" runat="server" skin="Default">
            <telerik:TextBoxSetting BehaviorID="TextBoxBehavior1" ErrorMessage="输入用户名!" Validation-IsRequired="true"
                Validation-ValidationGroup="Login">
                <TargetControls>
                    <telerik:TargetInput ControlID="RadTxtUser" />
                </TargetControls>
            </telerik:TextBoxSetting>
            <telerik:TextBoxSetting BehaviorID="TextBoxBehavior2" ErrorMessage="输入密码!" Validation-IsRequired="true"
                Validation-ValidationGroup="Login">
                <TargetControls>
                    <telerik:TargetInput ControlID="RadTxtPwd" />
                </TargetControls>
            </telerik:TextBoxSetting>
        </telerik:radinputmanager>
        <%-- <div class="divBgImg">
            <img style="position: fixed;" src="../Resources/Images/login/login_bg.jpg" width="100%" alt="" />
        </div>--%>
        <div id="divBgImg" style="background-image: url(../Resources/Images/login/login_bg2.jpg); width: 100%; background-repeat:repeat-x; background-position: center 0; position: relative;">
            <div class="LoginSHContainer">
                <div class="LoginSHContentContainer">
                    <div class="LoginSHCenterContainer">
                        <div class="LoginLogo">
                        </div>
                        <div class="LoginSH_Login">
                            <table class="LoginTB">
                                <tr>
                                    <td>
                                        <div class="m_left">
                                            <telerik:radimagegallery runat="server" id="RadImageGallery1" width="642px" height="472px" displayareamode="Image">
                                                <Items>
                                                    <telerik:ImageGalleryItem Description=""
                                                        ImageUrl="../Resources/Images/login/LoginLeftDiv.png" Title="" ThumbnailUrl="" />
                                                    <%--<telerik:ImageGalleryItem Description=""
                                                        ImageUrl="../Resources/Images/login/realAQIBg.png" Title="" ThumbnailUrl="" />
                                                    <telerik:ImageGalleryItem Description=""
                                                        ImageUrl="../Resources/Images/login/LoginLeftDiv.png" Title="" ThumbnailUrl="" />--%>
                                                </Items>
                                                <ThumbnailsAreaSettings Mode="ImageSliderPreview" />
                                                <ToolbarSettings ShowSlideshowButton="false" ShowFullScreenButton="false"
                                                    ShowItemsCounter="false" ShowThumbnailsToggleButton="false" />
                                                <PagerStyle ShowPagerText="false" />
                                                <ImageAreaSettings NextImageButtonText="下一页" PrevImageButtonText="前一页" />
                                                <ClientSettings>
                                                    <AnimationSettings SlideshowSlideDuration="2000">
                                                        <NextImagesAnimation Type="HorizontalResize" Easing="EaseOutSine" Speed="1500" />
                                                        <PrevImagesAnimation Type="HorizontalResize" Easing="EaseOutSine" Speed="1500" />
                                                    </AnimationSettings>
                                                </ClientSettings>
                                            </telerik:radimagegallery>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="m_right">
                                            <div class="itemUser">
                                                <asp:TextBox ID="RadTxtUser" runat="server" TabIndex="1" Width="205px" Height="22px" BackColor="#ffffff"
                                                    BorderStyle="Solid" BorderColor="#C1C1C1" BorderWidth="1px" CssClass="logintext" Font-Size="14px" />
                                            </div>
                                            <div class="itemPwd">
                                                <asp:TextBox ID="RadTxtPwd" runat="server" TabIndex="2" TextMode="Password" Width="205px" Height="22px"
                                                    BackColor="#ffffff" BorderStyle="Solid" BorderColor="#C1C1C1" BorderWidth="1px" Font-Size="14px"
                                                    CssClass="RadInput_Login" />
                                            </div>
                                          <div class="remberMe">
                                            <asp:CheckBox ID="passRem" runat="server" Checked="false"/><span id="texts">记住密码</span>
                                          </div>
                                            <div class="item btns">
                                                <telerik:radbutton id="btnOK" runat="server" buttontype="ToggleButton" toggletype="CustomToggle"
                                                    width="265px" height="41px" autopostback="true" forecolor="Blue" onclick="btnOK_Click"
                                                    style="top: 0px; left: 0px" validationgroup="Login">
                                                    <ToggleStates>
                                                        <telerik:RadButtonToggleState ImageUrl="../Resources/Images/login/btnLogin.png" HoveredImageUrl="../Resources/Images/login/btnLoginHovered.png"
                                                            Text="&nbsp;" Selected="true" />
                                                    </ToggleStates>
                                                </telerik:radbutton>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="copyright" style="position: absolute; left: 0px; top: 0px; color: #666; font-family: 微软雅黑; font-size: 14px; vertical-align: middle; overflow: hidden; text-align: center; height: auto; margin: 0; padding: 0; width: 100%;">
            <ul style="list-style: none;">
                <li>南通市环境监测中心 版权所有</li>
                <li><a style="text-decoration: none; color: #666;" href="http://www.sinoyd.com">江苏远大信息股份有限公司</a> 技术支持</li>
            </ul>
        </div>
    </form>
</body>
</html>
