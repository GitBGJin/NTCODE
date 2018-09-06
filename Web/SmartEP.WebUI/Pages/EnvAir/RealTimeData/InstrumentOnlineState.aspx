<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstrumentOnlineState.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.RealTimeData.InstrumentOnlineState" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
    <script type="text/javascript">

        $(function () {

            Refresh();
            function Refresh() {
                var InterValObj; //timer变量，控制时间
                var count = 300; //间隔函数，1秒执行 换算成秒（5分钟）
                var curCount; //当前剩余秒数

                curCount = count;

                $("#timer").html(Math.floor(curCount / 60) + "分" + Math.floor(curCount % 60) + "秒");
                InterValObj = window.setInterval(SetRemainTime, 1000); //启动计时器，1秒执行一次//向后台发送处理数据

                //timer处理函数
                function SetRemainTime() {
                    if (curCount == 0) {
                        $("#btnSearch").click();//触发事件
                        Refresh();//重新开始时间
                        window.clearInterval(InterValObj); //停止计时器
                    }
                    else {
                        curCount--;
                        var minutes = Math.floor(curCount / 60);
                        var seconds = Math.floor(curCount % 60);
                        $("#timer").html(minutes + "分" + seconds + "秒");
                    }
                }

            }
        });

    </script>
    <style>
        .btn-success {
            background-color: #5cb85c;
            border-color: #4cae4c;
            color: #fff;
        }

        a {
            /*color: #0088CC;*/
            color: #000;
            text-decoration: none;
            cursor: default;
        }

        [class^="icon-"], [class*=" icon-"] {
            display: inline-block;
            width: 14px;
            height: 14px;
            margin-top: 2px;
            line-height: 14px;
            vertical-align: text-top;
            background-image: url("/Resources/images/public/glyphicons-halflings.png");
            background-position: 14px 14px;
            background-repeat: no-repeat;
        }

        .btn {
            -moz-user-select: none;
            background-image: none;
            border: 1px solid #ddd;
            border-radius: 4px;
            /*cursor: pointer;*/
            display: inline-block;
            font-size: 12px;
            font-weight: normal;
            line-height: 1.42857;
            margin-bottom: 0;
            padding: 6px 12px;
            text-align: center;
            vertical-align: middle;
            white-space: nowrap;
        }

        .netState {
            display: inline-block;
            margin-bottom: 0;
            padding-top: 0;
            vertical-align: text-top;
        }

        .margin20-l {
            margin-left: 15px;
        }

        .icon-refresh {
            background-position: -240px -24px;
        }

        .icon-white {
            background-image: url("../../../Resources/images/public/glyphicons-halflings-white.png");
        }
    </style>
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
                        var MasterTable = $find("<%=gridRealTimeOnlineState.ClientID %>").get_masterTableView();
                        MasterTable.rebind();
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
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="80" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gridRealTimeOnlineState">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeOnlineState" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeOnlineState" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeOnlineState" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lblNetwork" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lblAllNetwork" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lblOnline" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lblOffOnline" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="getNoBtn">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRealTimeOnlineState" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="comboPort" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCityProper" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCity" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCityModel" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td style="width: 8%; text-align: center;">仪器：</td>
                        <td style="width: 22%; text-align: center;">
                            <telerik:RadComboBox ID="Instrument" runat="server" Width="280" SkinID="Default" Skin="Default" CheckBoxes="true" Localization-CheckAllString="全选" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                            </telerik:RadComboBox>
                        </td>
                        <td style="width: 8%; text-align: center;">联网状态：</td>
                        <td style="width: 22%; text-align: left;">
                            <telerik:RadDropDownList ID="Online" runat="server" Width="100px">
                                <Items>
                                    <telerik:DropDownListItem runat="server" Value="1" Text="全部" />
                                    <telerik:DropDownListItem runat="server" Value="2" Text="在线" />
                                    <telerik:DropDownListItem runat="server" Value="3" Text="离线" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" id="ti">&nbsp;&nbsp;
                            <span class="margin20-l"><a id="TotalNetwork" class="netState" href="javascript:void(0)" title="显示全部点位">在线率：<asp:Label ID="lblNetwork" runat="server" Text="0%"></asp:Label></a>&nbsp;&nbsp;&nbsp;
                                <a id="TotalButton" class="netState" href="javascript:void(0)" title="显示全部点位">联网总数<asp:Label ID="lblAllNetwork" runat="server" Text="0"></asp:Label>个</a>
                                <img style="height: 20px; width: 20px;" class="netState margin20-l" src="../../../Resources/images/public/On.PNG" />&nbsp;&nbsp;
                                <a id="OnLineButton" class="netState" href="javascript:void(0)" title="只显示在线点位">在线<asp:Label ID="lblOnline" runat="server" Text="0"></asp:Label>个</a>
                                <img style="height: 20px; width: 20px;" class="netState margin20-l" src="../../../Resources/images/public/Off.PNG" />&nbsp;&nbsp;
                                <a id="OffLineButton" class="netState" href="javascript:void(0)" title="只显示离线点位">离线<asp:Label ID="lblOffOnline" runat="server" Text="0"></asp:Label>个</a>
                                <asp:RadioButtonList ID="radlDataType" runat="server" RepeatDirection="Horizontal" class="radio inline netState" Style="margin-top: -6px;">
                                </asp:RadioButtonList>
                                <a id="GetStatusButton" href="javascript:void(0)" class="btn btn-success btn-sm  margin20-l"><i class="icon-refresh icon-white"></i>刷新 <span id="timer"></span></a>
                            </span>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridRealTimeOnlineState" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="true" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound" OnColumnCreated="gridRealTimeOnlineState_ColumnCreated"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="false" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" CssClass="RadToolBar_Customer" runat="server" Visible="false" />
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="0"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>

    </form>
</body>
</html>
