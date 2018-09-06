<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.Main" %>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
    <title>FineUI 在线示例 - 基于 ExtJS 的开源 ASP.NET 控件库</title>
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <meta name="Title" content="基于 ExtJS 的开源 ASP.NET 控件库(ExtJS based open source ASP.NET Controls)" />
    <meta name="Description" content="FineUI 的使命是创建 No JavaScript，No CSS，No UpdatePanel，No ViewState，No WebServices 的网站应用程序" />
    <meta name="Keywords" content="开源,ASP.NET,控件库,ExtJS,AJAX,Web2.0" />
    <link href="css/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <Sino:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></Sino:PageManager>
        <Sino:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <Sino:Region ID="Region1" Margins="0 0 0 0" ShowBorder="false" Height="50px" ShowHeader="false"
                    Position="Top" Layout="Fit" runat="server">
                    <Items>
                        <Sino:ContentPanel ShowBorder="false" CssClass="jumbotron" ShowHeader="false" ID="ContentPanel1"
                            runat="server" Expanded="true" EnableCollapse="true">
                            <table width="100%" cellpadding="0" cellspacing="0" style="background-color:#005999;">
                                <tr>
                                    <td height="50" style="padding-left:5px;"><img src="images/index/1.png" height="41" width="641" /></td>
                                    <td></td>
                                    <td></td>
                                </tr>

                            </table>
                        </Sino:ContentPanel>
                    </Items>
                </Sino:Region>
                <Sino:Region ID="leftRegion" Split="true" Width="200px" ShowHeader="true" Title="示例菜单"
                    EnableCollapse="true" Layout="Fit" Position="Left" runat="server">
                </Sino:Region>
                <Sino:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                    runat="server">
                    <Items>
                        <Sino:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" EnableFrame="false" ShowBorder="false" runat="server">
                            <Tabs>
                                <Sino:Tab ID="Tab1" Title="首页" Layout="Fit" Icon="House" CssClass="maincontent" runat="server">
                                    <Toolbars>
                                        <Sino:Toolbar ID="Toolbar1" runat="server">
                                            <Items>
                                                <Sino:ToolbarFill ID="ToolbarFill2" runat="server">
                                                </Sino:ToolbarFill>
                                                <Sino:Button ID="btnGotoOpenSourceSite" Icon="DiskDownload" Text="退出" OnClick="btnGotoOpenSourceSite_Click"
                                                    EnablePostBack="true" runat="server" ConfirmText="您确定退出吗？">
                                                </Sino:Button>
                                                <Sino:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                </Sino:ToolbarSeparator>
                                                <Sino:Button ID="Button1" Icon="PageGo" Text="官网首页" EnablePostBack="false" OnClientClick="window.open('http://fineui.com/', '_blank');"
                                                    runat="server">
                                                </Sino:Button>
                                                <Sino:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                                </Sino:ToolbarSeparator>
                                                <Sino:Button ID="Button2" Icon="PageGo" Text="论坛交流" OnClientClick="window.open('http://fineui.com/bbs/', '_blank');"
                                                    EnablePostBack="false" runat="server">
                                                </Sino:Button>
                                            </Items>
                                        </Sino:Toolbar>
                                    </Toolbars>
                                    <Items>
                                        <Sino:ContentPanel ID="ContentPanel2" ShowBorder="false" BodyPadding="10px" ShowHeader="false" AutoScroll="true"
                                            runat="server">
                                            <h2>关于FineUI</h2>
                                            基于 ExtJS 的开源 ASP.NET 控件库
                                        
                                            <br />
                                            <h2>FineUI的使命</h2>
                                            创建 No JavaScript，No CSS，No UpdatePanel，No ViewState，No WebServices 的网站应用程序
                                        
                                            <br />
                                            <h2>支持的浏览器</h2>
                                            IE 8.0+、Chrome、Firefox、Opera、Safari
                                        
                                            <br />
                                            <h2>授权协议</h2>
                                            Apache License v2.0（ExtJS 库在 <a target="_blank" href="http://www.sencha.com/license">GPL v3</a> 协议下发布）
                                            
                                            <br />
                                            <h2>相关链接</h2>
                                            首页：<a target="_blank" href="http://fineui.com/">http://fineui.com/</a>
                                            <br />
                                            论坛：<a target="_blank" href="http://fineui.com/bbs/">http://fineui.com/bbs/</a>
                                            <br />
                                            示例：<a target="_blank" href="http://fineui.com/demo/">http://fineui.com/demo/</a>
                                            <br />
                                            文档：<a target="_blank" href="http://fineui.com/doc/">http://fineui.com/doc/</a>
                                            <br />
                                            下载：<a target="_blank" href="http://fineui.codeplex.com/">http://fineui.codeplex.com/</a>
                                            <br />
                                            <br />
                                            <br />
                                            注：FineUI 不再内置 ExtJS 库，请手工添加 ExtJS 库：<a target="_blank" href="http://fineui.com/bbs/forum.php?mod=viewthread&tid=3218">http://fineui.com/bbs/forum.php?mod=viewthread&tid=3218</a>

                                        </Sino:ContentPanel>
                                    </Items>
                                </Sino:Tab>
                                <%--<Sino:Tab ID="Tab2" Title="标签1" EnableClose="false" BodyPadding="5px" runat="server" EnableIFrame="true" IFrameUrl="http://localhost/FrameDemo/Mis/TableInfo/List.aspx">
                                </Sino:Tab>
                                <Sino:Tab ID="Tab3" Title="标签2" EnableClose="false" BodyPadding="5px" runat="server" EnableIFrame="true" IFrameUrl="http://192.168.1.134/FrameTestFineUI/Default.aspx">
                                </Sino:Tab>
                                <Sino:Tab ID="Tab4" Title="标签3" EnableClose="false" BodyPadding="5px" runat="server" EnableIFrame="true" IFrameUrl="http://192.168.1.134/FrameTestRad/Default.aspx">
                                </Sino:Tab>
                                <Sino:Tab ID="Tab5" Title="标签5" EnableClose="false" BodyPadding="5px" runat="server">
                                    <Items>
                                        <Sino:ContentPanel runat="server" ID="cp1">
                                            <iframe src="http://www.fineui.com/demo/basic/login.aspx"></iframe>
                                        </Sino:ContentPanel>
                                    </Items>
                                </Sino:Tab>--%>
                            </Tabs>
                        </Sino:TabStrip>
                    </Items>
                </Sino:Region>
            </Regions>
        </Sino:RegionPanel>
        <Sino:Window ID="windowSourceCode" Icon="PageWhiteCode" Title="源代码" Hidden="true" EnableIFrame="true"
            runat="server" IsModal="true" Width="950px" Height="550px" EnableClose="true"
            EnableMaximize="true" EnableResize="true">
        </Sino:Window>
        <Sino:Menu ID="menuSettings" runat="server">
            <Sino:MenuButton ID="btnExpandAll" IconUrl="~/images/expand-all.gif" Text="展开菜单" EnablePostBack="false"
                runat="server">
            </Sino:MenuButton>
            <Sino:MenuButton ID="btnCollapseAll" IconUrl="~/images/collapse-all.gif" Text="折叠菜单"
                EnablePostBack="false" runat="server">
            </Sino:MenuButton>
            <Sino:MenuSeparator ID="MenuSeparator1" runat="server">
            </Sino:MenuSeparator>
            <Sino:MenuButton EnablePostBack="false" Text="菜单样式" ID="MenuStyle" runat="server">
                <Menu ID="Menu3" runat="server">
                    <Sino:MenuCheckBox Text="树菜单" ID="MenuStyleTree" Checked="true" GroupName="MenuStyle"
                        AutoPostBack="true" OnCheckedChanged="MenuStyle_CheckedChanged" runat="server">
                    </Sino:MenuCheckBox>
                    <Sino:MenuCheckBox Text="手风琴+树菜单" ID="MenuStyleAccordion" GroupName="MenuStyle" AutoPostBack="true"
                        OnCheckedChanged="MenuStyle_CheckedChanged" runat="server">
                    </Sino:MenuCheckBox>
                </Menu>
            </Sino:MenuButton>
            <Sino:MenuButton EnablePostBack="false" Text="语言" ID="MenuLang" runat="server">
                <Menu ID="Menu2" runat="server">
                    <Sino:MenuCheckBox Text="简体中文" ID="MenuLangZHCN" Checked="true" GroupName="MenuLang"
                        AutoPostBack="true" OnCheckedChanged="MenuLang_CheckedChanged" runat="server">
                    </Sino:MenuCheckBox>
                    <Sino:MenuCheckBox Text="繁體中文" ID="MenuLangZHTW" GroupName="MenuLang" AutoPostBack="true"
                        OnCheckedChanged="MenuLang_CheckedChanged" runat="server">
                    </Sino:MenuCheckBox>
                    <Sino:MenuCheckBox Text="English" ID="MenuLangEN" GroupName="MenuLang" AutoPostBack="true"
                        OnCheckedChanged="MenuLang_CheckedChanged" runat="server">
                    </Sino:MenuCheckBox>
                </Menu>
            </Sino:MenuButton>
            <Sino:MenuButton ID="MenuTheme" EnablePostBack="false" Text="主题" runat="server">
                <Menu ID="Menu4" runat="server">
                    <Sino:MenuCheckBox Text="Neptune" ID="MenuThemeNeptune" Checked="true" GroupName="MenuTheme"
                        AutoPostBack="true" OnCheckedChanged="MenuTheme_CheckedChanged" runat="server">
                    </Sino:MenuCheckBox>
                    <Sino:MenuCheckBox Text="Blue" ID="MenuThemeBlue" GroupName="MenuTheme"
                        AutoPostBack="true" OnCheckedChanged="MenuTheme_CheckedChanged" runat="server">
                    </Sino:MenuCheckBox>
                    <Sino:MenuCheckBox Text="Gray" ID="MenuThemeGray" GroupName="MenuTheme" AutoPostBack="true"
                        OnCheckedChanged="MenuTheme_CheckedChanged" runat="server">
                    </Sino:MenuCheckBox>
                    <Sino:MenuCheckBox Text="Access" ID="MenuThemeAccess" GroupName="MenuTheme" AutoPostBack="true"
                        OnCheckedChanged="MenuTheme_CheckedChanged" runat="server">
                    </Sino:MenuCheckBox>
                </Menu>
            </Sino:MenuButton>
            <Sino:MenuSeparator ID="MenuSeparator2" runat="server">
            </Sino:MenuSeparator>
            <Sino:MenuHyperLink ID="MenuHyperLink2" runat="server" Text="转到 v3.x 中文示例" NavigateUrl="http://fineui.com/demo_v3/" Target="_blank">
            </Sino:MenuHyperLink>
            <Sino:MenuHyperLink ID="MenuHyperLink1" runat="server" Text="转到 v3.x 英文示例" NavigateUrl="http://fineui.com/demo_en/" Target="_blank">
            </Sino:MenuHyperLink>
        </Sino:Menu>
        <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/images/menu.xml"></asp:XmlDataSource>
    </form>
    <img src="images/logo/logo3.png" alt="FineUI 图标" id="logo" />
    <script src="js/default.js" type="text/javascript"></script>
</body>
</html>
