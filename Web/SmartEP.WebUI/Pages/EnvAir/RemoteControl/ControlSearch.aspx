<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ControlSearch.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.RemoteControl.ControlSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="FormMainRight" runat="server" enctype="multipart/form-data">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <div class="RightContentContainer">
            <fieldset>
                <legend style="color: Blue; font-weight: bold;">反控历史日志▼</legend>
                <table id="tbBase" cellspacing="1" width="100%" cellpadding="0" border="0" class="Table_Customer">
                    <tr>
                        <td class="title">开始时间
                        </td>
                        <td class="content">
                            <telerik:RadDateTimePicker ID="startTime" runat="server" MinDate="1900-01-01">
                            </telerik:RadDateTimePicker>
                        </td>
                        <td class="title">结束时间
                        </td>
                        <td class="content">
                            <telerik:RadDateTimePicker ID="endTime" runat="server" MinDate="1900-01-01">
                            </telerik:RadDateTimePicker>
                        </td>
                        <td class="btns" align="center" style="width: 10%;">
                            <telerik:RadButton ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click">
                                <Icon PrimaryIconCssClass="btnsSearch" PrimaryIconLeft="4" PrimaryIconTop="4" />
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadGrid ID="RadGrid1" runat="server" GridLines="None" AllowPaging="True"
                    CssClass="RadGrid_Customer" AllowMultiRowSelection="True" AllowSorting="True"
                    AutoGenerateColumns="False" EnableHeaderContextMenu="True" EnableHeaderContextFilterMenu="True"
                    OnNeedDataSource="RadGrid1_NeedDataSource" OnItemCommand="RadGrid1_ItemCommand"
                    ShowStatusBar="True">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True"
                        ExportOnlyData="true">
                    </ExportSettings>
                    <MasterTableView GridLines="None" PageSize="15" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        ClientDataKeyNames="id" InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <Columns>
                            <telerik:GridClientSelectColumn HeaderText="选择" UniqueName="ClientSelectColumn">
                                <HeaderStyle Width="50px"></HeaderStyle>
                            </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row">
                                <ItemTemplate>
                                    <%#Container.DataSetIndex + 1%>
                                </ItemTemplate>
                                <HeaderStyle Width="50px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="id" HeaderText="id" UniqueName="id" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="operaterTime" HeaderText="操作时间" UniqueName="operaterTime">
                            </telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn DataField="mn" HeaderText="设备编号" UniqueName="mn">
                        </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="monitoringPointName" HeaderText="测点" UniqueName="monitoringPointName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="cmdDesc" HeaderText="命令描述" UniqueName="cmdDesc">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="execState" HeaderText="执行状态" UniqueName="execState">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="feedback" HeaderText="命令反馈" UniqueName="feedback">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;[第 &lt;strong&gt;{0}&lt;/strong&gt; 页共 &lt;strong&gt;{1}&lt;/strong&gt;页]&amp;nbsp;&amp;nbsp;[第 &lt;strong&gt;{2}&lt;/strong&gt; 到 &lt;strong&gt;{3}&lt;/strong&gt;,共 &lt;strong&gt;{5}&lt;/strong&gt;条信息]"></PagerStyle>
                        <CommandItemTemplate>
                            <div style="margin: 0px; float: left">
                                <asp:LinkButton ID="LinkButton2" OnClientClick="javascript:return confirm('确定要删除选择的行?')"
                                    runat="server" CommandName="Delete"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Resources/Images/telerik/common/Delete.gif" />删除</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton3" runat="server" CommandName="RebindGrid"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Resources/Images/telerik/common/Refresh.gif" />刷新</asp:LinkButton>
                            </div>
                        </CommandItemTemplate>
                        <CommandItemSettings ExportToPdfText="Export to Pdf" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="True" AllowColumnHide="True" AllowColumnsReorder="True"
                        AllowDragToGroup="True" AllowRowHide="True" AllowRowsDragDrop="True">
                        <Selecting AllowRowSelect="True"></Selecting>
                        <Resizing AllowColumnResize="True" ResizeGridOnColumnResize="True" EnableRealTimeResize="True"></Resizing>
                    </ClientSettings>
                    <CommandItemStyle Width="100%" />
                </telerik:RadGrid>
            </fieldset>
        </div>
    </form>
</body>
</html>
