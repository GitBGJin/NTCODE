<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrameShupl.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.FrameShupl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>环境空气在线监测监控平台</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <script type="text/javascript" src="Content/js/jquery-1.10.2.min.js"></script>
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
                Response.Redirect(PortalUrl + "/" + PortalName + "/HomePage.aspx");
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
                            <table style="background-color: #3D85D3; padding-top: 3px;" width="100%">
                                <tr>
                                    <td align="left" style="padding-left: 5px;" valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" style="background-color: #3D85D3; height: 55px;">
                                            <tr>
                                                <td valign="top">
                                                    <img src="Content/images/index/1.jpg" /></td>
                                                <td style="color: #FFF; font-size: 10pt; padding-left: 20px; padding-top: 10px; font-weight: bold;"><span runat="server" id="WelcomeTitle"></span></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right">
                                        <table cellpadding="0" cellspacing="0" border="0" height="60" style="background-color: #3D85D3;">
                                            <tr>
                                                <td width="70" align="center">
                                                    <f:Button ID="Button2" Icon="AsteriskOrange" Text="设置" runat="server" OnClientClick="OpenSetWin()">
                                                    </f:Button>
                                                </td>
                                                <td width="70" align="center">
                                                    <f:Button ID="Button3" Icon="TimeGo" Text="系统切换" runat="server" OnClick="SysChange_Click"
                                                        ConfirmText="您确认切换系统吗?">
                                                    </f:Button>
                                                </td>
                                                <td width="70" align="center">
                                                    <f:Button ID="Button1" Icon="UserRed" Text="退出" runat="server" OnClick="Logout_Click"
                                                        ConfirmText="您确认退出吗?">
                                                    </f:Button>
                                                </td>
                                                <td style="display: none;">
                                                    <f:DropDownList ID="ddlTheme" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlTheme_SelectedIndexChanged"
                                                        runat="server">
                                                        <f:ListItem Text="Blue" Selected="true" Value="blue" />
                                                        <f:ListItem Text="Gray" Value="Neptune" />
                                                        <f:ListItem Text="Access" Value="access" />
                                                    </f:DropDownList>
                                                    <f:TextBox runat="server" ID="txt_SiteKey" Text="12345678"></f:TextBox>
                                                    <f:TextBox runat="server" ID="txt_Menu1Count" Text="100"></f:TextBox>
                                                    <f:TextBox runat="server" ID="txt_LoginUrl" Text="/ELLab2_Frame_CaseImitateTeach/Login.aspx"></f:TextBox>
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
                    Width="230px" Margins="0 0 0 2" ShowHeader="true" ShowBorder="true" Title="系统菜单"
                    Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Left" runat="server">
                    <Items>
                        <f:ContentPanel runat="server" ID="abc1" ShowBorder="false" ShowHeader="false" BoxConfigAlign="Center" Title="<font style='color:#FFF;font-size:14px;font-weight:bold;'>系统菜单</font>">
                            <iframe id="frame1" height="10000px;" frameborder="0"></iframe>
                        </f:ContentPanel>
                        <f:TabStrip ID="TabStrip1" EnableTabCloseMenu="true" ShowBorder="false" runat="server" Hidden="true">
                            <Tabs>
                                <f:Tab ID="Tab1" Title="标准菜单" runat="server" Icon="Application" AutoScroll="true">
                                    <Items>
                                    </Items>
                                </f:Tab>
                                <f:Tab ID="Tab2" Title="个人菜单" runat="server" Icon="User" Hidden="true">
                                    <Items>
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
    <script language="javascript">

        // 页面第一个加载完毕后执行的函数
        F.ready(function () {
            var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');
            mainTabStrip.addTab({
                'id': 'HomePage',
                'url': '<%=ViewState["HomePage"]%>',
                'title': '首页',
                'closable': false,
                'iconCls': 'home',
                'bodyStyle': 'padding:0px;'
            });
            fullScreen();

            $("#frame1").attr("src", "Personal/Shupl/LeftTree.aspx?ParentModuleGuid=cbb3a26a-42cb-4149-afc8-18058196dd21&offsetHeight=" + window.document.body.offsetHeight.toString() + "&id=" + Math.random());
        });

        function initLeftTree() {
            //$('#<%= txt_Menu1Count.ClientID %>').val(window.document.body.offsetHeight.toString());
            $("#frame1").attr("src", "Personal/Shupl/LeftTree.aspx?ParentModuleGuid=cbb3a26a-42cb-4149-afc8-18058196dd21&offsetHeight=" + window.document.body.offsetHeight.toString() + "&id=" + Math.random());
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
                'id': 'SystemSet',
                'url': 'SystemMana/SystemSet.aspx',
                'title': '设置',
                'closable': true,
                'iconCls': 'sysset',
                'bodyStyle': 'padding:0px;'
            });
        }
    </script>
    <style>
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

        /*.x-accordion-item .x-accordion-hd-over {
            background-color: red; #6A6879
        }

        .x-grid-cell-inner-treecolumn {
            background-color: #FFF;
        }

        .x-grid-data-row {
            background-color: #000;
        }*/

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
</body>
</html>
