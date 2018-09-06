<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuperStationInterface.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.SuperStationInterface" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script>
        function openWindow()
        {
            window.radopen(null, "ConfigOfflineDialog");
            return true;
        }
        function changeStatus()
        {
            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
             <%--   <telerik:AjaxSetting AjaxControlID="instrumentName">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="InstrumentSN" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="modifyPwd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="modifyPwd" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridInstrument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInstrument" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="splitter" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane style="text-align:right" ID="paneWhere" runat="server" Height="35px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table >
                    <tr >
                        <td style="width:99%"></td>
                        <td><telerik:RadButton runat="server" ID="modifyPwd" OnClientClicked="openWindow" Text="修改密码"></telerik:RadButton></td>
                    </tr>
                </table>            
                </telerik:RadPane>
                <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="grvGrid" runat="server" Visible="true">
                <telerik:RadGrid ID="gridInstrument" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="7" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0" OnItemDataBound="gridInstrument_ItemDataBound" OnNeedDataSource="gridInstrument_NeedDataSource"
                    CssClass="RadGrid_Customer" OnItemCommand="gridInstrument_ItemCommand">
                    <MasterTableView GridLines="None" CommandItemDisplay="None" TableLayout="Fixed" >
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                                    </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="接口ID" UniqueName="InterfaceName" DataField="InterfaceName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="30%" />
                            <telerik:GridBoundColumn HeaderText="接口名" UniqueName="ChineseName" DataField="ChineseName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="30%"  />
<%--                        <telerik:GridBoundColumn HeaderText="通信" UniqueName="CommunicateStatus" DataField="CommunicateStatus" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />--%>
                            <%--<telerik:GridTemplateColumn HeaderText="通信" UniqueName="CommunicateStatus" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="220px" >
                                <ItemTemplate>
                                        <telerik:RadButton ID="btn1" UniqueName="CommunicateStatus" runat="server" Width="220px" Height="220px" Text='<%#Eval("CommunicateStatus") %>' 
                                        Image-ImageUrl='<%# Eval("CommunicateStatus").ToString() == "0" ?        
                                    ("imagesPic/Green.png"): (Eval("CommunicateStatus").ToString() == "1"?("imagesPic/red.png"):("imagesPic/Green.png")) %>'>
                                    </telerik:RadButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridBoundColumn HeaderText="通信" UniqueName="CommunicateStatus" DataField="CommunicateStatus" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="最新读取时间" UniqueName="LatestReadTime" DataField="LatestReadTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25%"  />
<%--                        <telerik:GridBoundColumn HeaderText="操作" UniqueName="OperateStatus" DataField="OperateStatus" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />--%>
                            <telerik:GridTemplateColumn HeaderText="接口开关" UniqueName="OperateStatus" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                                <ItemTemplate>
                                    <telerik:RadButton ID="btn2" runat="server" Width="76px" Height="24px" Text='<%#Eval("OperateStatus") %>' 
                                        Image-ImageUrl='<%# Eval("OperateStatus").ToString() == "0" ? ("imagesPic/2.png"): (Eval("OperateStatus").ToString() == "1"?("imagesPic/1.png"):("imagesPic/1.png")) %>'>
                                    </telerik:RadButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="2"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
              </telerik:RadPageView>
            </telerik:RadMultiPage>
          </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="ConfigOfflineDialog" runat="server" Height="200px" Width="330px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                    Title="修改密码" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" ShowOnTopWhenMaximized="true" >
                    <ContentTemplate>
                        <br />
                        <asp:Label ID="Label2" runat="server" Text="Label" Width="80px">新 密 码：</asp:Label>
                        <telerik:RadTextBox ID="newPwd" runat="server" TextMode="Password"></telerik:RadTextBox>
                        <br />
                        <br />
                        <asp:Label ID="Label3" runat="server" Text="Label" Width="80px">重复密码：</asp:Label>
                        <telerik:RadTextBox ID="rePwd" runat="server" TextMode="Password"></telerik:RadTextBox>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="newPwd" ControlToValidate="rePwd" Display="Dynamic" ErrorMessage="*输入密码不一致" ForeColor="Red"></asp:CompareValidator>
                        <br />
                        <br />
                        <telerik:RadButton ID="Modify" runat="server" Text="确认" OnClick="Modify_Click" CommandName="commit" ></telerik:RadButton>
                        &nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="Cancel" runat="server" Text="取消" OnClick="Cancel_Click" CausesValidation="False" OnClientClick="GetRadWindow()" CommandName="cancel"></telerik:RadButton>
                        &nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="Reset" runat="server" Text="重置" OnClick="Reset_Click" CausesValidation="False" CommandName="reset"></telerik:RadButton>
                    </ContentTemplate>
                </telerik:RadWindow>
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>
    </form>
</body>
</html>
