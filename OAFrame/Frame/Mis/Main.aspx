<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>远大Mis平台、报表平台、图表平台主页面</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href="Css/main.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
            function showwindows(a, b, c) {
                var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');
                mainTabStrip.addTab({
                    'id': a,
                    'url': b,
                    'title': c,
                    'closable': true,
                    'bodyStyle': 'padding:0px;'
                });
            }

            function showwindows_Mis(a, b, c) {
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

            function showwindows_Report(a, b, c) {
                var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');
                 mainTabStrip.addTab({
                     'id': a,
                     'url': b,
                     'title': c,
                     'closable': true,
                     'iconCls': 'report',
                     'bodyStyle': 'padding:0px;'
                 });
            }

            function showwindows_Chart(a, b, c) {
                var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');
                mainTabStrip.addTab({
                    'id': a,
                    'url': b,
                    'title': c,
                    'closable': true,
                    'iconCls': 'chart',
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
                var tab = mainTabStrip.getItem(ModuleGuid);
                if (tab) {
                    window.frames[ModuleGuid].location.reload();
                }
            }

            function delrefreshTab(pm, sm) {
                var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');
                mainTabStrip.removeTab(sm);
                var tab = mainTabStrip.getItem(pm);
                if (tab) {
                    window.frames[pm].location.reload();
                }
            }

            function showtab(a, b, c) {
                parent.window.showwindows(a, b + "?RowGuid=" + a, c);
            }

            function showtab1(a, b, c) {
                parent.window.showwindows(a, b, "" + c);
            }

        </script>
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1"
            EnablePageLoading="false" Theme="Neptune" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server" BodyPadding="3">
            <Regions>
              <f:Region ID="regionTop" ShowBorder="false" ShowHeader="false" Position="Top"
                    Layout="Fit" runat="server" EnableCollapse="false" Split="true" BoxConfigPadding="0" Height="45">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Bottom" runat="server" CssClass="topbar" Height="45">
                            <Items>                               
                                <f:ToolbarText ID="txtUser" runat="server" CssStyle="font-size:16pt;" Text="江苏省远大信息系统有限公司Mis、报表、图表管理平台">
                                </f:ToolbarText>                               
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:Region>
                <f:Region ID="regionLeft" Split="true"
                    Width="210px" Margins="0 0 0 2" ShowHeader="true" ShowBorder="true" Title="系统菜单"
                    Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Left" runat="server">
                    <Items>
                        <f:Tree runat="server" ID="Tree1" ShowHeader="false">
                            <Nodes>
                                <f:TreeNode Text="Mis平台" Expanded="true" Icon="Folder">
                                    <f:TreeNode Text="Mis列表(初级)" Icon="Page" OnClientClick="javascript:showwindows_Mis('Mis1','TableInfoEasy/List.aspx','Mis列表(初级)');">
                                    </f:TreeNode>                                
                                    <f:TreeNode Text="Mis列表(中高级)" Icon="Page" OnClientClick="javascript:showwindows_Mis('Mis2','TableInfo/List.aspx','Mis列表(中高级)');">
                                    </f:TreeNode>
                                </f:TreeNode>
                                <f:TreeNode Text="报表平台" Expanded="true" Icon="Folder">
                                    <f:TreeNode Text="报表平台" Icon="Report" OnClientClick="javascript:showwindows_Report('report','Report/List.aspx','报表平台');">
                                    </f:TreeNode>
                                </f:TreeNode>
                                <f:TreeNode Text="图表平台" Expanded="true" Icon="Folder">
                                    <f:TreeNode Text="图表平台" Icon="ChartBar" OnClientClick="javascript:showwindows_Chart('chart','Chart/List.aspx','图表平台');">
                                    </f:TreeNode>
                                </f:TreeNode>
                            </Nodes>
                        </f:Tree>
                    </Items>
                </f:Region>
                <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                    runat="server">
                    <Items>
                        <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server" ActiveTabIndex="0">
                            <%--<Tabs>
                                <f:Tab runat="server" Title="Mis列表(初级)" ID="Tab1" IFrameUrl="TableInfoEasy/List.aspx" Icon="Page" EnableIFrame="true"></f:Tab>
                                <f:Tab runat="server" Title="Mis列表(中高级)" ID="Tab2" IFrameUrl="TableInfo/List.aspx" Icon="Page" EnableIFrame="true"></f:Tab>
                                <f:Tab runat="server" Title="报表列表" ID="Tab3" IFrameUrl="TableInfo/List.aspx" Icon="Report" EnableIFrame="true"></f:Tab>
                                <f:Tab runat="server" Title="图表列表" ID="Tab4" IFrameUrl="TableInfo/List.aspx" Icon="ChartBar" EnableIFrame="true"></f:Tab>
                            </Tabs>--%>
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
            //alert(1);

            return;
            var mainTabStrip = Ext.getCmp('<%= mainTabStrip.ClientID %>');
            mainTabStrip.addTab({
                'id': 'Mis1',
                'url': 'TableInfoEasy/List.aspx',
                'title': 'Mis列表(初级)',
                'closable': false,
                'iconCls': 'page',
                'bodyStyle': 'padding:0px;'
            });

            mainTabStrip.addTab({
                'id': 'Mis2',
                'url': 'TableInfo/List.aspx',
                'title': 'Mis列表(中高级)',
                'closable': false,
                'iconCls': 'page',
                'bodyStyle': 'padding:0px;'
            });
        });
    </script>
    <style>
        .page {
            background-image: url(images/icons/Page.png) !important;
        }

        .report {
            background-image: url(images/icons/report.png) !important;
        }

        .chart {
            background-image: url(images/icons/Chart_Bar.png) !important;
        }

        .sysset {
            background-image: url(images/icons/asterisk_orange.png) !important;
        }
    </style>
</body>
</html>
