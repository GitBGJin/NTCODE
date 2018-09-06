<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlineInfoNew2.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Dock.OnlineInfoNew2" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
    <script src="../../../Resources/JavaScript/FrameJS.js"></script>

    <script type="text/javascript">

        //连接到实时数据页面
        function onclickData(pointNames, factorNames) {
            OpenFineUIWindow("e6137d5a-8393-45dd-bcd1-e018a6fcadd6", "Pages/EnvAir/RealTimeData/RealTimeData.aspx?pointNames=" + pointNames + "&FactorName=" + factorNames, "实时数据");
            return false;

        }

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
            margin-left: 3px;
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
                        var MasterTable = $find("<%=RadGrid1.ClientID %>").get_masterTableView();
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

                //按钮行处理
                function gridRTB_ClientButtonClicking(sender, args) {
                    var masterTable = $find("<%= RadGrid1.ClientID %>").get_masterTableView();
                var CurrentBtn = args.get_item();
                var CurrentBtnName = CurrentBtn.get_text();
                var CurrentBtnCommandName = CurrentBtn.get_commandName();
                switch (CurrentBtnCommandName) {
                    case "RebindGrid":
                        masterTable.rebind();
                        break;
                    default:
                        break;
                }
            }
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="80" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
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
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
               <telerik:RadGrid ID="RadGrid1" runat="server" GridLines="None" CssClass="RadGrid_Customer" EnableHeaderContextMenu="True" Height="100%"
                    AutoGenerateColumns="false" Width="99.8%" AllowSorting="true" AllowMultiRowSelection="true" OnNeedDataSource="grid_NeedDataSource"  OnItemCreated="grid_ItemCreated"
                    ShowStatusBar="True" AllowPaging="true" PagerStyle-AlwaysVisible="true">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False" PageSize="500"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="false" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                         <CommandItemTemplate>
                        <telerik:RadToolBar ID="gridRTB" runat="server" AutoPostBack="true" CssClass="RadToolBar_Customer" SkinID="CRUD" Width="100%" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                    </CommandItemTemplate>
                        <Columns>
                           <telerik:GridBoundColumn DataField="点位类型" HeaderText=" 点位类型" UniqueName="点位类型"
                                HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%"
                                ItemStyle-HorizontalAlign="Center" AllowSorting="true" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="点位名称" HeaderText=" 点位名称" UniqueName="点位名称"
                                HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%"
                                ItemStyle-HorizontalAlign="Center" AllowSorting="true" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="数据类型" HeaderText=" 数据类型" UniqueName="数据类型"
                                HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%"
                                ItemStyle-HorizontalAlign="Center" AllowSorting="true" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="最新数据时间" HeaderText="最新数据时间" UniqueName="最新数据时间"
                                HeaderStyle-Width="30%" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="30%"
                                ItemStyle-HorizontalAlign="Center" AllowSorting="true" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="离线时间" HeaderText="离线时间" UniqueName="离线时间"
                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"
                                ItemStyle-HorizontalAlign="Center" AllowSorting="true" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页" Visible="false"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
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
