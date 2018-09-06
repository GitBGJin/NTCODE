<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WebMaster/MasterPage2.Master"
    CodeBehind="RemoteControlMana2.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.RemoteControl.RemoteControlMana2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            //刷新远程控制命令
            function RefreshRemoteControl(url) {

                var pane = $find("<%= topPane.ClientID %>");
                if (!pane) return;

                pane.set_contentUrl(url);
            }
            //刷新远程控制历史日志
            function RefreshLogs(url) {

                var pane = $find("<%= bottomPane.ClientID %>");
                if (!pane) return;

                pane.set_contentUrl(url);
            }

            //父页面调用子页面方法
            function Test() {
                var pane = $find("<%= topPane.ClientID %>");
                var iframe = pane.getExtContentElement();
                var contentWindow = iframe.contentWindow;
                contentWindow.AlertTest();
            }

            function GetRadBottomPane() {
                var pane = $find("<%= bottomPane.ClientID %>")

                if (!pane) return;

                if (pane.get_collapsed()) {
                    pane.expand();
                }
                else {
                    pane.collapse();
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" LiveResize="true" ResizeWithParentPane="true" Height="100%">
        <telerik:RadPane ID="navigationPane" runat="server" Width="20%" ContentUrl="ProtocolCommandTree.aspx"
            ShowContentDuringLoad="false">
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="contentPane" runat="server" Scrolling="none">
            <telerik:RadSplitter ID="RadSplitter2" runat="server" Orientation="Horizontal" LiveResize="true" ResizeWithParentPane="true">
                <telerik:RadPane ID="topPane" runat="server" Height="60%" ContentUrl="RemoteControl2.aspx"
                    ShowContentDuringLoad="false">
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar2" runat="server" CollapseMode="Both">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="bottomPane" runat="server" ContentUrl="RemoteControlLog.aspx"
                    ShowContentDuringLoad="false">
                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID='ContentPlaceHolder3' runat="server">
    远程控制</asp:Content>
