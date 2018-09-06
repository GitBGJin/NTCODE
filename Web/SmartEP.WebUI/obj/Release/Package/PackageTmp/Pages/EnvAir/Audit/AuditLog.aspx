<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditLog.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.AuditLog" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function OnClientClicking() {
                    var date1 = new Date();
                    var date2 = new Date();
                    var date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    var date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        //sender.set_autoPostBack(false);
                        return false;
                    }
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                //tab页切换时时间检查
                function OnClientSelectedIndexChanging(sender, args) {
                    var date1 = new Date();
                    var date2 = new Date();
                    var date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    var date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        args.set_cancel(true);
                        return;
                    }
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        args.set_cancel(true);
                        return;
                    } else {
                        return;
                    }
                }
                function onRequestStart(sender, args) {
                    if (args.EventArgument == "")
                        return;
                    if (args.EventArgument == 0 || args.EventArgument == 1 ||
                        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                        args.set_enableAjax(false);
                    }
                }
            </script>
        </telerik:RadCodeBlock>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="auditLogGrid">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="auditLogGrid" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="auditLogGrid" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="btnSearch" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="auditLogGrid" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCom" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="factorCom">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">统计类型
                        </td>
                        <td class="content" style="width: 360px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" AutoPostBack="true"  RepeatLayout="Flow" RepeatColumns="1" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged">
                                <%--<asp:ListItem Text="常规站" Value="General" ></asp:ListItem>
                                <asp:ListItem Text="超级站" Value="Super" ></asp:ListItem>--%>
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 80px">统计因子:
                        </td>
                        <td class="content" style="width: 360px;">
                            <CbxRsm:PointCbxRsm   ID="pointCbxRsm" runat="server" ApplicationType="Air" CbxWidth="360" CbxHeight="350" DefaultAllSelected="true" Visible="false" MultiSelected="true" DropDownWidth="520"></CbxRsm:PointCbxRsm>
                            <telerik:RadComboBox ID="factorCom" runat="server" AutoPostBack="true" Width="200" SkinID="Default" Skin="Default" Visible="true"  CheckBoxes="true" Localization-CheckAllString="全选" EnableCheckAllItemsCheckBox="true" OnSelectedIndexChanged="factorCom_SelectedIndexChanged" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="黑碳分析仪" Value="e5b6d666-24d1-473a-b15a-33a36245d44f" />
                                    <telerik:RadComboBoxItem runat="server" Text="离子色谱仪" Value="1589850e-0df1-4d9d-b508-4a77def158ba" />
                                    <telerik:RadComboBoxItem runat="server" Text="太阳辐射仪" Value="da4f968f-cc6e-4fec-8219-6167d100499d" />
                                    <telerik:RadComboBoxItem runat="server" Text="粒径谱仪" Value="9ef57f3c-8cce-4fe3-980f-303bbcfde260" />
                                    <telerik:RadComboBoxItem runat="server" Text="常规参数" Value="9ef57f3c-8cce-4fe3-980f-303bbcfde260" />
                                    <telerik:RadComboBoxItem runat="server" Text="EC/OC有机碳元素碳" Value="6e4aa38a-f68b-490b-9cd7-3b92c7805c2d" />
                                    <telerik:RadComboBoxItem runat="server" Text="VOCs" Value="3745f768-a789-4d58-9578-9e41fde5e5f0" />
                                    <telerik:RadComboBoxItem runat="server" Text="三波长浊度仪" Value="a6b3d80c-8281-4bc6-af47-f0febf568a5c" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        
                        <td class="content" align="right" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">时间:
                        </td>
                        <td class="content" style="width: 360px;">
                            <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                            &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12"  />
                        </td>
                        <td class="title" style="width: 80px">监测因子:
                        </td>
                        <td class="content" style="width: 360px;">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="360" DefaultAllSelected="true" DropDownWidth="420" ID="factorCbxRsm"></CbxRsm:FactorCbxRsm>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pvGrid" runat="server" Visible="true">
                        <telerik:RadGrid ID="auditLogGrid" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="false" AllowSorting="false" ShowFooter="true"
                            AutoGenerateColumns="False" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="auditLogGrid_NeedDataSource" OnGridExporting="auditLogGrid_GridExporting" OnItemDataBound="auditLogGrid_ItemDataBound"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                <CommandItemTemplate>
                                    <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                        runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Container.DataSetIndex + 1%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridBoundColumn HeaderText="审核时间" UniqueName="审核时间" DataField="UpdateTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                                    <telerik:GridBoundColumn HeaderText="站点" UniqueName="站点" DataField="PointName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                                    <telerik:GridBoundColumn HeaderText="审核人" UniqueName="审核人" DataField="CreatUser" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                                    <telerik:GridBoundColumn HeaderText="类型" UniqueName="类型" DataField="AuditType" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                                    <telerik:GridBoundColumn HeaderText="仪器" UniqueName="仪器" DataField="PollutantName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                                    <telerik:GridBoundColumn HeaderText="数据时间" UniqueName="数据时间" DataField="tstamp" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                                    <telerik:GridBoundColumn HeaderText="因子" UniqueName="因子" DataField="AuditPollutantDataValue" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                                    <%--<telerik:GridBoundColumn HeaderText="修改值" UniqueName="修改值" DataField="AuditPollutantDataValue" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />--%>
                                    <telerik:GridBoundColumn HeaderText="审核理由" UniqueName="审核理由" DataField="AuditReason" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                                    <telerik:GridBoundColumn HeaderText="审核结果" UniqueName="审核结果" DataField="Description" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                                </Columns>
                                <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                    PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                            </MasterTableView>
                            <CommandItemStyle Width="100%" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="2"
                                    SaveScrollPosition="false"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
