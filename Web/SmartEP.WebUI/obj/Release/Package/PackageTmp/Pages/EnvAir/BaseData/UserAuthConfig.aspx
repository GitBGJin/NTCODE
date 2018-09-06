<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAuthConfig.aspx.cs" MasterPageFile="~/WebMaster/MasterPage.Master" Inherits="SmartEP.WebUI.Pages.EnvAir.BaseData.UserAuthConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
        <AjaxSettings>

            <telerik:AjaxSetting AjaxControlID="grid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grid" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="splitter">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grid" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grid" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript" language="javascript">
            //测点因子授权
            function ShowEditAuthDetailWnd(UserUid, CName)
            {
                var oWnd = window.radopen("UserAuthConfigDetail.aspx?UserUid=" + UserUid, "PointDialog");
                oWnd.SetTitle("用户（" + CName + "）测点因子授权");
            }

        </script>
    </telerik:RadScriptBlock>
    <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
        Width="100%">
        <telerik:RadPane ID="paneWhere" runat="server" Height="28px" Width="100%" Scrolling="None" BorderWidth="0">
            <table class="Table_Customer" width="100%">
                <tr>
                    <td class="title">用户名：</td>
                    <td class="content" style="width: 100px;">
                        <telerik:RadTextBox ID="txtUserName" runat="server" />
                    </td>                    
                    <td class="btnsSearch" colspan="7">
                        <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                    </td>
                </tr>
            </table>
        </telerik:RadPane>
        <telerik:RadPane ID="paneGrid" runat="server" Height="100%" Width="100%" Scrolling="None" BorderWidth="0">
            <telerik:RadGrid ID="grid" runat="server" CssClass="RadGrid_Customer" GridLines="None" Width="99.8%" Height="100%"
                AutoGenerateColumns="false" ToolTip="点击编辑图标可为用户进行测点和因子授权。"
                AllowPaging="true" PageSize="24" AllowCustomPaging="false" AllowMultiRowSelection="true" AllowMultiRowEdit="true"
                ShowHeader="true" ShowStatusBar="true" OnNeedDataSource="grid_NeedDataSource">
                <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                <MasterTableView DataKeyNames="Id" ClientDataKeyNames="Id" GridLines="None" CommandItemDisplay="None" EditMode="InPlace"
                    NoMasterRecordsText="没有数据" NoDetailRecordsText="没有数据" ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage">
                    <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                        PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ExportToExcelText="导出到Excel"
                        ShowExportToWordButton="true" ExportToWordText="导出到Word" ShowExportToPdfButton="true" ExportToPdfText="导出到PDF" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="用户名" UniqueName="CName" DataField="CName" HeaderStyle-Width="100px" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                        <telerik:GridBoundColumn HeaderText="登录名" UniqueName="LoginName" DataField="LoginName" MaxLength="14" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                        <telerik:GridTemplateColumn HeaderText="授权" UniqueName="MonitoringPointUid" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" HeaderButtonType="None" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                            <ItemTemplate>
                                 <img id="btnEdit" style="cursor: pointer;" alt="编辑" title="点击编辑" src="../../../Resources/Images/icons/page_edit.png"
                                    onclick="ShowEditAuthDetailWnd('<%# DataBinder.Eval(Container, "DataItem.Id")%>', '<%# DataBinder.Eval(Container, "DataItem.CName")%>')" />
                                 授权
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                     
                    </Columns>
                    <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                    PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                </MasterTableView>
                <ClientSettings>
                    <Scrolling AllowScroll="true" EnableVirtualScrollPaging="false" UseStaticHeaders="true"
                        SaveScrollPosition="false" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="PointDialog" runat="server" Height="600px" Width="800px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                Title="配置" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
        </Windows>
        <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
            PinOn="固定" />
    </telerik:RadWindowManager>
</asp:Content>
