<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrameShupl.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.FrameShupl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>水质在线监测监控平台</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <script type="text/javascript" src="Content/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="Content/js/jquery.cookie.js"></script>
    <style type="text/css">
        #clothMenu {
            width: 170px;
            height: 48px;
            background: url(Content/images/imgs/bg_cloth.png) no-repeat;
            position: fixed;
            top: 0;
            right: 0;
            margin: 51px 90px 0px 0px;
            z-index: 10000;
        }

            #clothMenu ul {
                list-style: none;
                text-align: center;
                margin: 10px -18px 0px -33px;
            }

                #clothMenu ul li {
                    float: left;
                    color: #333;
                    cursor: pointer;
                    margin-left: 12px;
                }

                    #clothMenu ul li p {
                        margin-top: 1px;
                        font-size: 12px;
                        color: #333;
                    }

                    #clothMenu ul li img {
                        margin-top: -2px;
                        border: none;
                    }

        .selected {
            border: none;
        }
    </style>
    <style type="text/css">
        .home {
            background-image: url(Content/icon/House.png) !important;
        }

        .page {
            background-image: url(Content/icon/page.png) !important;
        }

        .sysset {
            background-image: url(Content/icon/asterisk_orange.png) !important;
        }

        .x-grid-cell-inner-treecolumn {
            padding-top: 10px;
            padding-bottom: 10px;
        }

        .x-tree-node-text {
            font-size: 14px;
        }

        .x-panel-body-default {
            color: #000;
        }

        .x-accordion-layout-ct {
            padding-left: 0px;
            padding-right: 0px;
            padding-top: 0px;
            padding-bottom: 0px;
        }

        .x-accordion-hd {
            padding-top: 0px;
            padding-bottom: 0px;
        }

        .x-accordion-hd-sibling-expanded {
            background-color: red;
        }
    </style>
    <link id="Theme" href="" rel="stylesheet" />
</head>
<body onresize="javascript:initLeftTree();">
    <form id="form1" runat="server">
        <script type="text/javascript">
            function showwindows(a, b, c) {
                var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');

                mainTabStrip.addTab({
                    'id': a,
                    'url': b,
                    'title': c,
                    'closable': true,
                    'iconCls': 'page',
                    'bodyStyle': 'padding:0px;'
                });
            }

            function showwindowsnoclose(a, b, c) {
                var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');

                mainTabStrip.addTab({
                    'id': a,
                    'url': b,
                    'title': c,
                    'closable': false,
                    'bodyStyle': 'padding:0px;'

                });
            }

            function deltab(a) {
                var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');
                mainTabStrip.removeTab(a);
            }

            function refreshTab(ModuleGuid) {
                var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');
                var tab = mainTabStrip.getTab(ModuleGuid);
                if (tab) {
                    window.frames[ModuleGuid].location = window.frames[ModuleGuid].location;
                }
            }

            function delrefreshTab(pm, sm) {
                var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');
                mainTabStrip.removeTab(sm);
                var tab = mainTabStrip.getTab(pm);
                if (tab) {
                    window.frames[pm].location.reload();
                }
            }

            function showtab(a, b, c) {
                parent.window.showwindows(a, b + "?RowGuid=" + a, c);
            }

            function showtab1(a, b, c) {
                parent.window.showwindows(a, b, "考核-" + c);
                //window.document.getElementById("a").onselect();
            }
        </script>
        <script runat="server" language="C#">
            protected void SysChange_Click(object sender, EventArgs e)
            {
                string PortalUrl = System.Configuration.ConfigurationManager.AppSettings["PortalUrl"].ToString();
                string PortalName = System.Configuration.ConfigurationManager.AppSettings["PortalName"].ToString();
                Response.Redirect(PortalUrl + "/" + PortalName + "/Portal/HomePage.aspx");
            }
            protected void SysLogout_Click(object sender, EventArgs e)
            {
                string PortalUrl = System.Configuration.ConfigurationManager.AppSettings["PortalUrl"].ToString();
                string PortalName = System.Configuration.ConfigurationManager.AppSettings["PortalName"].ToString();
                Response.Redirect(PortalUrl + "/" + PortalName + "/Portal/Login.aspx");
            }
        </script>
        <script type="text/javascript">
            $(function () {
                <%-- 系统切换js --%>
                $("#dropDown").click(function () {
                    $("#leftMenu").toggle("fast");
                });

                var tabLi = $("#leftMenu").find("li");
                for (var i = 0; i < tabLi.length; i++) {
                    tabLi[i].index = i;
                    tabLi[i].onmouseover = function () {
                        if (this.index == 0) {
                            $("#iconImg").css({ "background-position": "-273px -60px" });
                        }
                        if (this.index == 1) {
                            $("#iconImg").css({ "background-position": "-29px -56px" });
                        }
                        if (this.index == 2) {
                            $("#iconImg").css({ "background-position": "-62px -60px" });
                        }
                        if (this.index == 3) {
                            $("#iconImg").css({ "background-position": "-120px -61px" });
                        }
                        if (this.index == 4) {
                            $("#iconImg").css({ "background-position": "2px -61px" });
                        }
                        if (this.index == 5) {
                            $("#iconImg").css({ "background-position": "-91px -61px" });
                        }
                    };
                }


                <%-- 换肤js --%>
                $("#Button6").click(function () {
                    $("#clothMenu").toggle("fast");
                });
            });

            //网站换肤
            $(function () {
                var $li = $("#clothMenu li");  //查找到元素
                $li.click(function () {   //给元素添加事件
                    switchSkin(this.id);//调用函数
                    window.location.reload(true);
                });
                //保存Cookie完毕以后就可以通过Cookie来获取当前的皮肤了
                var cookie_skin = $.cookie("MyCssSkin");     //获取Cookie的值
                if (!cookie_skin) {                          //如果确实存在Cookie
                    cookie_skin = "Neptune";
                }
                switchSkin(cookie_skin);     //执行

                //系统切换
                var $li = $("#leftMenu li");  //查找到元素
                $li.click(function () {   //给元素添加事件
                    SysChange(this.id);//调用函数
                });
            });
            function switchSkin(skinName) {
                //$("#clothname").attr("title", skinName);
                $("#frame1").attr("title", skinName);
                $("#" + skinName).addClass("selected")                //当前<li>元素选中frame1
                .siblings().removeClass("selected");  //去掉其他同辈<li>元素的选中
                $("#Theme").attr("href", "App_Themes/" + skinName + "/" + skinName + ".css"); //设置不同皮肤
                $.cookie("MyCssSkin", skinName, { path: '/', expires: 10 });  //保存Cookie
            }

            var PortalUrl = '<%=System.Configuration.ConfigurationManager.AppSettings["PortalUrl"].ToString()%>';
            var PortalName = '<%=System.Configuration.ConfigurationManager.AppSettings["PortalName"].ToString()%>';
            var V02PortalSZ = PortalUrl + "/" + PortalName;
            var Token = '<%=Request["Token"].ToString()%>';
            var GISUrl = '<%=System.Configuration.ConfigurationManager.AppSettings["GISUrl"].ToString()%>';
            var NoiseUrl = '<%=System.Configuration.ConfigurationManager.AppSettings["NoiseUrl"].ToString()%>';

            function Logout() {
                if (window.confirm("你确定要退出系统？"))
                    top.location = V02PortalSZ + "/Portal/Login.aspx";
            }

            function SysChange(sysName) {
                if (window.confirm("你确定要切换系统？")) {
                    if (sysName == 'FrameGIS')
                        //top.location = GISUrl;
                        //window.open(GISUrl);
                        window.open(V02PortalSZ + "/Portal/MidPage.aspx?Type=Gis");
                    else if (sysName == 'FrameNoise')
                        window.open(V02PortalSZ + "/Portal/MidPage.aspx?Type=Noise");
                    else if (sysName == 'FrameAir')
                        top.location = V02PortalSZ + "/Portal/MidPage.aspx?Type=Air";
                    else if (sysName == 'FrameWater')
                        top.location = V02PortalSZ + "/Portal/MidPage.aspx?Type=Water";
                    else if (sysName == 'FrameMgr')
                        top.location = V02PortalSZ + "/Portal/MidPage.aspx?Type=Mgr";
                }
            }
        </script>

        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1"
            EnablePageLoading="false" Theme="Neptune" runat="server"></f:PageManager>
        <f:Timer ID="Timer1" Interval="60" Enabled="true" OnTick="Timer1_Tick" EnableAjaxLoading="false" runat="server">
        </f:Timer>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server" BodyPadding="3">
            <Regions>
                <f:Region ID="Region1" Margins="0 0 0 0" Height="60px" ShowBorder="false" ShowHeader="false"
                    Position="Top" runat="server" EnableCollapse="false" Collapsed="false" Split="false">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Bottom" runat="server" Hidden="true">
                            <Items>
                                <f:ToolbarText ID="ToolbarText1" Text="&nbsp;" runat="server">
                                </f:ToolbarText>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                </f:ToolbarSeparator>
                                <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                </f:ToolbarSeparator>
                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </f:ToolbarSeparator>
                                <f:ToolbarText ID="ToolbarText3" Text="主题: " runat="server">
                                </f:ToolbarText>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:ContentPanel ShowBorder="false" ShowHeader="false"
                            ID="ContentPanel1" runat="server" Height="60">
                            <input type="text" id="clothname" style="display: none; position: fixed; color: red" title="" />
                            <div id="leftMenu" style="color: #333; font-size: 13px; width: 73px; display: none; position: fixed; top: 0; right: 0; z-index: 9999; margin-top: 47px;">
                                <table>
                                    <tr>
                                        <td>
                                            <img src="Content/images/imgs/bg_leftMenuUp.png" alt="" style="margin: 0px 0px -3px -1px;" /></td>
                                    </tr>
                                    <tr>
                                        <td style="background: url(Content/images/imgs/bg_leftMenurepeat.png) repeat-y; width: 73px;">
                                            <ul>
                                                <li id="FrameAir">
                                                    <a href="#">
                                                        <img src="Content/images/imgs/icon_blockAir.png" alt="" />
                                                        <span>环境空气</span>
                                                    </a>
                                                </li>
                                                <li id="FrameWater">
                                                    <a href="#">
                                                        <img src="Content/images/imgs/icon_blockWater.png" alt="" />
                                                        <span>地表水</span>
                                                    </a>
                                                </li>
                                                <li id="FrameNoise">
                                                    <a href="#">
                                                        <img src="Content/images/imgs/icon_blockVoice.png" alt="" />
                                                        <span>噪声</span>
                                                    </a>
                                                </li>
                                                <li id="FrameGIS">
                                                    <a href="#">
                                                        <img src="Content/images/imgs/icon_blockGIS.png" alt="" />
                                                        <span>GIS</span>
                                                    </a>
                                                </li>
                                               <%-- <li id="FrameQC">
                                                    <a href="#">
                                                        <img src="Content/images/imgs/icon_blockZhikong.png" alt="" />
                                                        <span>质控</span>
                                                    </a>
                                                </li>--%>
                                                <li id="FrameMgr">
                                                    <a href="#">
                                                        <img src="Content/images/imgs/icon_blockHoutai.png" alt="" />
                                                        <span>后台</span>
                                                    </a>
                                                </li>
                                            </ul>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <img src="Content/images/imgs/bg_leftMenudown.png" alt="" style="margin-top: -3px; margin-left: -1px;" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="clothMenu" style="display: none;">
                                <ul>
                                    <li id="Neptune">
                                        <div>
                                            <img src="Content/images/imgs/icon_Blue.png" alt="海王星" />
                                            <p>海王星</p>
                                        </div>
                                    </li>
                                    <li id="Fresh">
                                        <div>
                                            <img src="Content/images/imgs/icon_Fresh.png" alt="清新版" />
                                            <p>清新版</p>
                                        </div>
                                    </li>
                                    <li id="Feminine">
                                        <div>
                                            <img src="Content/images/imgs/icon_Pink.png" alt="女性化" />
                                            <p>女性化</p>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <div class="DivLogoWater"></div>
                            <table id="Container_table" width="100%">
                                <tr>
                                    <td id="Td_left" align="left" style="padding-left: 0px;" valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td valign="top" style="width: 300px;">
                                                    <%--<div style="background: url(Content/images/index/WaterTop.png) no-repeat;"></div>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>

                                    <td id="Td_right" align="right">
                                        <table>
                                            <tr>
                                                <td id="Td_right_user">
                                                    <div>
                                                        <img src="Content/images/imgs/icon_picEx.png" alt="" />
                                                    </div>
                                                    <span runat="server" id="WelcomeTitle"></span>
                                                </td>
                                                <td>
                                                    <table style="margin-top: -18px;">
                                                        <tr>
                                                            <td class="weather">
                                                                <p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>
                                                                <%--<p><span>苏州市</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;晴&nbsp;&nbsp;&nbsp;&nbsp;12℃/23℃</p>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table id="lineButton" style="height: 29px">
                                                                    <tr>
                                                                        <td>
                                                                            <%--<img src="Content/images/imgs/bg_iconLeft.png" alt="" style="margin: -6px -3px 0px 0px" />--%>
                                                                            <div class="lineButton_left"></div>
                                                                        </td>
                                                                        <td class="lineButton_center">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <input type="button" class="iconText" onclick="OpenSetWin()" title="个人菜单设置" id="Button7" style="border: none; cursor: pointer;" />
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div>
                                                                                            <input type="button" class="iconSetting" onclick="OpenSetWin1()" title="重置密码" id="Button8" style="border: none; cursor: pointer;" />
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div>
                                                                                            <input type="button" class="iconBack" title="退出" onclick="Logout()" id="Button2" style="border: none; cursor: pointer;" />
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div>
                                                                                            <input type="button" class="iconCloth" title="换肤" id="Button6" style="border: none; cursor: pointer;" />
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            <div class="lineButton_right"></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td id="Td_right_menu">
                                                    <div>
                                                        <%--<img id="iconImg" src="Content/images/imgs/icon_Air.png" alt="" style="float: left; margin: 1px 10px 0 14px;" />--%>
                                                        <%--<img id="dropDown" src="Content/images/imgs/icon_downarrow2.png" alt="" style="float: right; margin: 10px 11px 0px 0px; cursor: pointer;" />--%>
                                                        <div id="iconImg"></div>
                                                        <div id="dropDown"></div>
                                                    </div>
                                                </td>
                                                <%-- <td width="70" align="center"></td>
                                                <td width="70" align="center"></td>
                                                <td width="70" align="center"></td>
                                                <td style="display: none;"></td>--%>
                                                <td style="display: none;">
                                                    <f:Button ID="Button3" Icon="TimeGo" Text="系统切换" runat="server" OnClick="SysChange_Click"
                                                        ConfirmText="您确认切换系统吗?">
                                                    </f:Button>
                                                </td>
                                                <td>
                                                    <div style="display: none;">
                                                        <f:DropDownList ID="ddlTheme" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlTheme_SelectedIndexChanged"
                                                            runat="server">
                                                            <f:ListItem Text="Blue" Selected="true" Value="blue" />
                                                            <f:ListItem Text="Gray" Value="Neptune" />
                                                            <f:ListItem Text="Access" Value="access" />
                                                        </f:DropDownList>
                                                        <f:TextBox runat="server" ID="txt_SiteKey" Text="12345678"></f:TextBox>
                                                        <f:TextBox runat="server" ID="txt_Menu1Count" Text="100"></f:TextBox>
                                                        <f:TextBox runat="server" ID="txt_LoginUrl" Text="/ELLab2_Frame_CaseImitateTeach/Login.aspx"></f:TextBox>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </f:ContentPanel>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Split="true"
                    Width="210px" Margins="0 0 0 2" ShowHeader="true" ShowBorder="true" Title="系统菜单"
                    Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Left" runat="server">
                    <Items>
                        <f:TabStrip ID="TabStrip1" EnableTabCloseMenu="true" ShowBorder="true" runat="server" Hidden="false">
                            <Tabs>
                                <f:Tab ID="Tab1" Title="标准菜单" runat="server" Icon="Application">
                                    <Items>
                                        <f:ContentPanel runat="server" ID="abc1" Width="205%" Height="650%" ShowBorder="false" ShowHeader="false" BoxConfigAlign="Center" Title="<font style='color:#FFF;font-size:14px;font-weight:bold;'>系统菜单</font>">
                                            <iframe id="frame1" name="leftMenuFrame" width="100%" height="650%" frameborder="0" title="Neptune" scrolling="auto"></iframe>
                                        </f:ContentPanel>
                                    </Items>
                                </f:Tab>
                                <f:Tab ID="Tab2" Title="个人菜单" runat="server" Icon="User">
                                    <Items>
                                        <f:ContentPanel runat="server" ID="abc2" Width="205%" Height="650%" ShowBorder="false" ShowHeader="false" BoxConfigAlign="Center" Title="<font style='color:#FFF;font-size:14px;font-weight:bold;'>系统菜单</font>">
                                            <iframe id="frame2" name="leftPersonalMenuFrame" width="100%" height="650%" frameborder="0" title="Neptune" scrolling="auto"></iframe>
                                        </f:ContentPanel>
                                    </Items>
                                </f:Tab>
                            </Tabs>
                        </f:TabStrip>
                    </Items>
                </f:Region>
                <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                    runat="server">
                    <Items>
                        <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                        </f:TabStrip>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server"
            EnableConfirmOnClose="false" IFrameUrl="about:blank" Target="Parent" IsModal="True"
            Width="400px" Height="200px" Hidden="true">
        </f:Window>
    </form>
    <script language="javascript" type="text/javascript">
        var PortalUrl = '<%=System.Configuration.ConfigurationManager.AppSettings["PortalUrl"].ToString()%>';
        var PortalName = '<%=System.Configuration.ConfigurationManager.AppSettings["PortalName"].ToString()%>';
        var V02PortalSZ = PortalUrl + "/" + PortalName;
        var Token = '<%=Request["Token"].ToString()%>';
        var homePage = V02PortalSZ + "/Pages/EnvWater/Dock/DashboardWhite.aspx";

        // 页面第一个加载完毕后执行的函数
        F.ready(function () {
            var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');
            mainTabStrip.addTab({
                'id': 'HomePage',
                'url': homePage,
                'title': '首页',
                'closable': false,
                'iconCls': 'home',
                'bodyStyle': 'padding:0px;'
            });
            fullScreen();
            $("#frame1").attr("height", GetBodyHeight());
            $("#frame1").attr("src", V02PortalSZ + "/Portal/LeftTree.aspx?ParentModuleGuid=fd2d3899-7047-4452-9a70-8fd329160a73&Token=" + Token + "&id=" + Math.random());
            $("#frame2").attr("height", GetBodyHeight());
            $("#frame2").attr("src", V02PortalSZ + "/Portal/PersonalLeftTree.aspx?ParentModuleGuid=fd2d3899-7047-4452-9a70-8fd329160a73&Token=" + Token + "&id=" + Math.random());
        });

        function initLeftTree() {
            $("#frame1").attr("height", GetBodyHeight());
            $("#frame2").attr("height", GetBodyHeight());
            //$("#frame1").attr("src", V02PortalSZ + "/Portal/LeftTree.aspx?ParentModuleGuid=fd2d3899-7047-4452-9a70-8fd329160a73&Token=" + Token + "&id=" + Math.random());
        }

        function fullScreen() {
            var xc = 0;
            var yc = 0;
            if (window.screen) {
                var ah = screen.availHeight - 30;
                var aw = screen.availWidth - 10;
                xc = (aw - 810) / 2;
                yc = (ah - 582) / 2;
                if (xc < 0) xc = -7;
                if (yc < 0) yc = -5;
            }

            window.resizeTo(screen.availWidth, screen.availHeight)
            window.moveTo(0, 0);
        }

        function OpenSetWin() {
            var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');
            mainTabStrip.addTab({
                'id': 'ModuleFrame',
                'url': 'SystemMana/ModulePersonal/ModuleFrame.aspx',
                'title': '设置',
                'closable': true,
                'iconCls': 'sysset',
                'bodyStyle': 'padding:0px;'
            });
        }

        function OpenSetWin1() {
            var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');
                    mainTabStrip.addTab({
                        'id': 'SystemSet',
                        'url': 'SystemMana/SystemSet.aspx',
                        'title': '重置密码',
                        'closable': true,
                        'iconCls': 'sysset',
                        'bodyStyle': 'padding:0px;'
                    });
                }

        function GetBodyHeight() {
            var myHeight;
            if (typeof (window.innerWidth) == 'number') {
                //Non-IE   
                myHeight = window.innerHeight;
            } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                //IE 6+ in 'standards compliant mode'   
                myHeight = document.documentElement.clientHeight;
            } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                //IE 4 compatible   
                myHeight = document.body.clientHeight;
            }
            myHeight = myHeight - 150;
            if (myHeight < 0)
                myHeight = 768;
            return myHeight + "px";
        }
    </script>
</body>
</html>
