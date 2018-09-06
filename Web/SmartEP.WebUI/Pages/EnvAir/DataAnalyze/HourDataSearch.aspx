<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HourDataSearch.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.HourDataSearch" %>

<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript" language="javascript">
            //控制导出时按钮不会隐藏掉处理
            function onRequestStart(sender, args) {
                if (args.EventArgument == "")
                    return;
                if (args.EventArgument == 6 || args.EventArgument == 7 ||
                    args.EventArgument == 0 || args.EventArgument == 1 ||
                    args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                        args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                        args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                    args.set_enableAjax(false);
                }
            }
        </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
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
                <telerik:AjaxSetting AjaxControlID="factorCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="grdAvgHourData">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgHourData" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgHourData" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="80px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%" class="Table_Customer">
                    <tr>
                        <td class="title" style="text-align: right; width: 100px">测点：
                        </td>
                        <td class="content" style="width: 180px;">
                            <CbxRsm:PointCbxRsm runat="server" ID="pointCbxRsm" ApplicationType="Air" CbxWidth="180" CbxHeight="350" DropDownWidth="520" MultiSelected="false" DefaultAllSelected="false" OnSelectedChanged="pointCbxRsm_SelectedChanged"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="title" style="text-align: right; width: 100px">监测因子：
                        </td>
                        <td class="content" style="text-align: left" colspan="5">
                            <CbxRsm:FactorCbxRsm runat="server" ID="factorCbxRsm" ApplicationType="Air" CbxWidth="550" CbxHeight="350" DropDownWidth="550" MultiSelected="true"></CbxRsm:FactorCbxRsm>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="text-align: right;">开始时间：
                        </td>
                        <td class="content" style="text-align: left">
                            <telerik:RadDatePicker runat="server" ID="txtStartDate" MinDate="1900-01-01" DateInput-DateFormat="yyyy-MM-dd"></telerik:RadDatePicker>
                        </td>
                        <td class="title" style="text-align: right;">截止日期：
                        </td>
                        <td class="content" style="text-align: left;width:150px">
                            <telerik:RadDatePicker runat="server" ID="txtEndDate" MinDate="1900-01-01" DateInput-DateFormat="yyyy-MM-dd"></telerik:RadDatePicker>
                        </td>
                        <td class="title" style="text-align: right;width:60px">时间：
                        </td>
                        <td class="content" style="text-align: left;width:180px">
                            <telerik:RadTimePicker runat="server" ID="txt_TimeDate" DateInput-DateFormat="HH:00"></telerik:RadTimePicker>
                        </td>
                        <td class="content" style="text-align: left; width: 100px">
                            <asp:CheckBox runat="server" ID="chkIsFooter" Text="统计行" Checked="false" />
                        </td>
                        <td class="content" style="text-align: left">
                            <asp:ImageButton ID="btnSearch" runat="server" SkinID="ImgBtnSearch" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="grdAvgHourData" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="20" AllowCustomPaging="true" AllowSorting="false"
                    AutoGenerateColumns="true" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grdAvgHourData_NeedDataSource" CssClass="RadGrid_Customer" OnColumnCreated="grdAvgHourData_ColumnCreated">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" ItemStyle-Width="50px"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Container.DataSetIndex + 1%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PageSizes="10 50 100" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
