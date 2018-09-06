<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DynamicCalibrationInfo.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.DynamicCalibrationInfo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../../Resources/CSS/Table.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript" language="javascript">
                var myRadGrid;
                function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

                //页面刷新
                function Refresh_Grid(args) {
                    if (args) {
                        var MasterTable = $find("<%= gridStandardGas.ClientID %>").get_masterTableView();
                        MasterTable.rebind();
                    }
                }

                //控制导出时按钮不会隐藏掉处理
                function onRequestStart(sender, args) {
                    if (args.EventArgument == 6 || args.EventArgument == 7 ||
                        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                        args.set_enableAjax(false);
                    }
                }


                function OnClientClicking() {

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
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />

        <table id="Tb" style="width: 80%; height: 5%;" cellspacing="1" cellpadding="0" class="Table_Customer"
            border="0">
            <tr>
                <td class="title" style="width: 10%; text-align: center;">校验结论：
                </td>
                <td style="width: 5%;">

                    <asp:Label ID="lblInstrumentType" runat="server" Text="符合标准"></asp:Label>
                    <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm" Visible="false"></CbxRsm:PointCbxRsm>

                    <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" DropDownWidth="420" ID="factorCbxRsm" Visible="false"></CbxRsm:FactorCbxRsm>
                </td>

                <td class="title" style="width: 8%; text-align: center;">有效期至：
                </td>
                <td class="title" style="width: 5%; text-align: center;">
                    <asp:Label ID="lblSiteName" runat="server" Text="2020.6.25"></asp:Label>
                </td>
                <td class="title" style="width: 8%; text-align: center;">校验人签字:</td>
                <td style="width: 5%; text-align: center;">
                    <asp:Label ID="Label26" runat="server" Text="张三"></asp:Label>

                </td>
                <td class="title" style="width: 8%; text-align: center;">审核人签字：</td>
                <td class="title" style="width: 5%; text-align: center;">
                    <asp:Label ID="Label27" runat="server" Text="李四"></asp:Label>

                </td>
                <td class="title" style="width: 8%; text-align: center;">主管人签字：</td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="王五"></asp:Label>

                </td>
            </tr>
        </table>

        <div style="margin: 10px 0px 10px 10px;">
            <table style="width: 50%; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable">
                <tr>
                    <td class="title" style="width: 10%; text-align: center;">生产厂家</td>
                    <td class="content" style="width: 5%; text-align: center; border-right-width: 0px;">
                        <asp:Label ID="Label1" runat="server" Text="江苏苏州"></asp:Label>
                    </td>
                    <td class="content" style="border-left-width: 0px;"></td>
                    <td class="title" style="width: 10%; text-align: center;">仪器型号：</td>
                    <td class="content" style="width: 5%; text-align: center;">
                        <asp:Label ID="Label2" runat="server" Text="S010201"></asp:Label>
                    </td>
                    <td class="title" style="width: 10%; text-align: center;">出厂编号：</td>
                    <td style="width: 10%; text-align: center;">
                        <asp:Label ID="Label3" runat="server" Text="ASD010101"></asp:Label>

                    </td>

                </tr>
                <tr>
                    <td class="title" style="width: 10%; text-align: center;">环境条件</td>
                    <td class="content" style="width: 10%; border-right-width: 0px; text-align: center;">温度（℃）：
                    </td>
                    <td class="content" style="width: 5%; border-left-width: 0px; border-right-width: 0px;">
                        <asp:Label ID="Label28" runat="server" Text="30℃"></asp:Label>
                    </td>
                    <td class="title" style="width: 10%; text-align: center; border-right-width: 0px; border-left-width: 0px;">湿度（%）：</td>
                    <td class="content" style="width: 5%; border-left-width: 0px; border-right-width: 0px;">
                        <asp:Label ID="Label29" runat="server" Text="20"></asp:Label>
                    </td>
                    <td class="title" style="width: 10%; text-align: center; border-right-width: 0px; border-left-width: 0px;">其它：</td>
                    <td style="width: 10%; text-align: center; border-left-width: 0px;">
                        <asp:Label ID="Label30" runat="server" Text="SSAA"></asp:Label>

                    </td>

                </tr>
                <tr>
                    <td class="title" style="width: 10%; text-align: center;" rowspan="2">计量器具</td>
                    <td class="content" style="width: 5%; text-align: center;" colspan="3">名称、型号
                    </td>
                    <td class="title" style="width: 10%; text-align: center;" colspan="3">仪器编号</td>

                </tr>
                <tr>

                    <td class="content" style="width: 5%; text-align: center;" colspan="3">S01010111
                    </td>
                    <td class="title" style="width: 10%; text-align: center;" colspan="3">0X101010</td>

                </tr>
                <tr>
                    <td class="title" style="width: 10%; text-align: center;" colspan="2">动态校准仪流量范围</td>
                    <td class="content" style="width: 5%; text-align: center;" colspan="5">标气流量：  0 ～ 100  ml/min        稀释流量：  0 ～ 10 l/min 
                    </td>


                </tr>
            </table>
        </div>

        <table>
            <tr>
                <td class="title">标气流量多点校准</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gridStandardGas" runat="server" GridLines="None" Height="100%" Width="100%"
                        AllowPaging="false" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound" OnColumnCreated="grdDER_ColumnCreated"
                        CssClass="RadGrid_Customer">
                        <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                            <CommandItemTemplate>
                                <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                    runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                            </CommandItemTemplate>
                            <Columns>

                                <telerik:GridNumericColumn HeaderText="给定值(ml/min)" UniqueName="PointName" DataField="PointName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />

                                <telerik:GridNumericColumn HeaderText="量程百分数(%)" UniqueName="PointName" DataField="PointName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                                <telerik:GridNumericColumn HeaderText="测定结果(ml/min)" UniqueName="PointName" DataField="PointName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                                <telerik:GridNumericColumn HeaderText="说  明" UniqueName="PointName" DataField="PointName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />


                            </Columns>
                            <%--   <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>--%>
                        </MasterTableView>
                        <%-- <CommandItemStyle Width="100%" />
                        <ClientSettings>
                            <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="0"
                                SaveScrollPosition="true"></Scrolling>
                        </ClientSettings>--%>
                    </telerik:RadGrid>
                </td>
            </tr>
              <tr>
                <td class="title">稀释气流量多点校准</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gridDilution" runat="server" GridLines="None" Height="100%" Width="100%"
                        AllowPaging="false" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound" OnColumnCreated="grdDER_ColumnCreated"
                        CssClass="RadGrid_Customer">
                        <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                            <CommandItemTemplate>
                                <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                    runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                            </CommandItemTemplate>
                            <Columns>

                                <telerik:GridNumericColumn HeaderText="给定值(ml/min)" UniqueName="PointName" DataField="PointName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />

                                <telerik:GridNumericColumn HeaderText="量程百分数(%)" UniqueName="PointName" DataField="PointName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                                <telerik:GridNumericColumn HeaderText="测定结果(ml/min)" UniqueName="PointName" DataField="PointName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                                <telerik:GridNumericColumn HeaderText="说  明" UniqueName="PointName" DataField="PointName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />


                            </Columns>
                            <%--  <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>--%>
                        </MasterTableView>
                        <CommandItemStyle Width="100%" />
                        <%--   <ClientSettings>
                            <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="0"
                                SaveScrollPosition="true"></Scrolling>
                        </ClientSettings>--%>
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>


    </form>
</body>
</html>
