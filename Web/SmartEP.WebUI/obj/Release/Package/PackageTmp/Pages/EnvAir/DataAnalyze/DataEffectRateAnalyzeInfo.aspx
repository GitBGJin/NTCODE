﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataEffectRateAnalyzeInfo.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.DataEffectRateAnalyzeInfo" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script language="javascript" type="text/javascript">
                function GetRadWindow() {
                    var oWindow = null;
                    if (window.radWindow)
                        oWindow = window.RadWindow; //Will work in Moz in all cases, including clasic dialog
                    else if (window.frameElement.radWindow)
                        oWindow = window.frameElement.radWindow; //IE (and Moz as well)
                    return oWindow;
                }
                function RefreshParent() {
                    this.parent.Refresh_Grid(true);
                }
            </script>
        </telerik:RadCodeBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtDescription" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadGrid ID="gridEffectRateInfo" runat="server" GridLines="None" Height="100%" Width="100%"
            AllowPaging="false" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
            AutoGenerateColumns="False" AllowMultiRowSelection="false"
            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
            OnNeedDataSource="gridEffectRateInfo_NeedDataSource" OnItemDataBound="gridEffectRateInfo_ItemDataBound"
            CssClass="RadGrid_Customer">
            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                <CommandItemTemplate>
                    <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                        runat="server" Width="50%" Visible="false" />
                </CommandItemTemplate>
                <Columns>
                    <%--                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>--%>

<%--                    <telerik:GridTemplateColumn HeaderText="城市" UniqueName="PortName" DataField="PortName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                    <telerik:GridTemplateColumn HeaderText="站点" UniqueName="PortName" DataField="PortName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                    <telerik:GridTemplateColumn HeaderText="集成商" UniqueName="PortName" DataField="PortName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                    <telerik:GridTemplateColumn HeaderText="检测项" UniqueName="PortName" DataField="PortName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                    <telerik:GridTemplateColumn HeaderText="天数" UniqueName="PortName" DataField="PortName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                    <telerik:GridTemplateColumn HeaderText="日数据条数" UniqueName="PortName" DataField="PortName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                    <telerik:GridTemplateColumn HeaderText="有效条数" UniqueName="PortName" DataField="PortName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                    <telerik:GridTemplateColumn HeaderText="实际数据条数" UniqueName="PortName" DataField="PortName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                    <telerik:GridTemplateColumn HeaderText="有效率" UniqueName="PortName" DataField="PortName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />--%>

                    <telerik:GridNumericColumn HeaderText="站点" UniqueName="PointName" DataField="PointName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                    <telerik:GridNumericColumn HeaderText="集成商" UniqueName="IntegratorName" DataField="IntegratorName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                    <telerik:GridNumericColumn HeaderText="监测项" UniqueName="PollutantName" DataField="PollutantName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--"/>
                    <telerik:GridNumericColumn HeaderText="天数" UniqueName="Days" DataField="Days" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--"/>
                    <telerik:GridNumericColumn HeaderText="实测数据条数" UniqueName="RealCount" DataField="RealCount" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                    <telerik:GridNumericColumn HeaderText="应测数据条数" UniqueName="ShouldCount" DataField="ShouldCount" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--"/>
                    <telerik:GridNumericColumn HeaderText="有效数据条数" UniqueName="EffectCount" DataField="EffectCount" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--"/>
                    <telerik:GridNumericColumn HeaderText="有效率" UniqueName="EffectRate" DataField="EffectRate" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--"/>

                </Columns>
                <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                    PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
            </MasterTableView>
            <CommandItemStyle Width="100%" />
            <ClientSettings>
                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                    SaveScrollPosition="true"></Scrolling>
            </ClientSettings>
        </telerik:RadGrid>

    </form>
</body>
</html>
