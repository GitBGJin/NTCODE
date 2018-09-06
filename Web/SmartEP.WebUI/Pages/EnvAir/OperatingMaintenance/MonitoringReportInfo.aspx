<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoringReportInfo.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.MonitoringReportInfo" %>

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
                        //var MasterTable = $find("<gridMaintenanceInfo.ClientID %>").get_masterTableView();
                        //MasterTable.rebind();
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
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridMaintenanceInfo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridMaintenanceInfo" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridMaintenanceInfo" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridMaintenanceInfo" />
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
            <telerik:RadPane ID="paneWhere" runat="server" Height="100%" Width="100%" Scrolling="Y"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div style="float: left; width: 25%;">
                    <div style="margin: 10px 0px 10px 10px;">被传递臭氧传递仪器</div>
                    <div style="margin: 10px 0px 10px 10px;">
                        <table style="width: 100%; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">

                            <tr>
                                <td class="title" style="width: 10%; text-align: center;">仪器名称</td>
                                <td class="content" style="width: 5%; text-align: center;">
                                    <asp:Label ID="Label1" runat="server" Text="普通"></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td class="title" style="width: 10%; text-align: center;">仪器编号</td>
                                <td class="content" style="width: 5%; text-align: center;">
                                    <asp:Label ID="Label2" runat="server" Text="S01X01"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="title" style="width: 10%; text-align: center;">检测原理</td>
                                <td class="content" style="width: 5%; text-align: center;">
                                    <asp:Label ID="Label8" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="title" style="width: 10%; text-align: center;">标准类别</td>
                                <td class="content" style="width: 5%; text-align: center;">基本标准/传递标准/工作标准
                                </td>
                            </tr>

                        </table>

                    </div>
                </div>
                <div style="float: left; width: 25%;">
                    <div style="margin: 10px 0px 10px 10px;">检测时间、位置</div>
                    <div style="margin: 10px 0px 10px 10px;">
                        <table style="width: 100%; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">

                            <tr>
                                <td class="title" style="width: 10%; text-align: center;">时间</td>
                                <td class="content" style="width: 5%; text-align: center;">
                                    <asp:Label ID="Label9" runat="server" Text="2015.05.05"></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td class="title" style="width: 10%; text-align: center;">地点</td>
                                <td class="content" style="width: 5%; text-align: center;">
                                    <asp:Label ID="Label10" runat="server" Text="园区"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="title" style="width: 10%; text-align: center;">位置</td>
                                <td class="content" style="width: 5%; text-align: center;">
                                    <asp:Label ID="Label11" runat="server" Text="娄葑"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="title" style="width: 10%; text-align: center;">室温：25℃</td>
                                <td class="content" style="width: 5%; text-align: center;">气压：36atm / mmHg
                                </td>
                            </tr>

                        </table>

                    </div>
                </div>
                <div style="clear: both;"></div>
                <div style="margin: 10px 0px 10px 10px;">传递标准</div>
                <div style="margin: 10px 0px 10px 10px;">
                    <table style="width: 50%; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">

                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">仪器名称</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label3" runat="server" Text="普通"></asp:Label>
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">仪器型号及编号</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label4" runat="server" Text="S10A01"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">上次与NISTSRP34比对日期</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label5" runat="server" Text="2015.02.02"></asp:Label>
                            </td>
                            <td class="title" style="width: 10%; text-align: center;" colspan="2"></td>
                            
                        </tr>

                    </table>
                </div>
                <div style="margin: 10px 0px 10px 10px;">结果</div>
                <div style="margin: 10px 0px 10px 10px;">
                    <table style="width: 50%; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">

                        <tr>
                            <td class="title" style="width: 10%; text-align: center;" colspan="2">气体类别</td>
                            <td class="content" style="width: 5%; text-align: center;" colspan="2">普通</td>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;" colspan="2">被传递仪器测量范围</td>
                            <td class="content" style="width: 5%; text-align: center;">0.5—0.86</td>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;" rowspan="3">线性关系</td>
                            <td class="content" style="width: 5%; text-align: center;">相关系数
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">0.2</td>

                        </tr>
                        <tr>

                            <td class="content" style="width: 5%; text-align: center;">斜率
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">0.5</td>

                        </tr>
                        <tr>

                            <td class="content" style="width: 5%; text-align: center;">截距
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">2</td>

                        </tr>

                        <tr>
                            <td class="title" style="width: 10%; text-align: center;" rowspan="2">漂移(24小时/7天)</td>
                            <td class="content" style="width: 5%; text-align: center;">零点
                            </td>
                            <td class="content" style="width: 5%; text-align: center;">3</td>
                        </tr>
                        <tr>
                            <td class="content" style="width: 5%; text-align: center;">跨点
                            </td>
                            <td class="content" style="width: 5%; text-align: center;">5</td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;" rowspan="2">漂移(24小时/7天)</td>
                            <td class="content" style="width: 5%; text-align: center;">零点
                            </td>
                            <td class="content" style="width: 5%; text-align: center;">4</td>
                        </tr>
                        <tr>
                            <td class="content" style="width: 5%; text-align: center;">跨点
                            </td>
                            <td class="content" style="width: 5%; text-align: center;">5</td>
                        </tr>
                    </table>
                </div>
                <div style="margin: 10px 0px 10px 10px;">与上次比对结果比较</div>
                <div style="margin: 10px 0px 10px 10px;">
                    <table style="width: 50%; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">

                        <tr>
                            <td class="title" style="width: 10%; text-align: center;" colspan="2">上次比对日期</td>
                            <td class="content" style="width: 5%; text-align: center;" colspan="2">2015.02.05</td>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">上次的斜率值</td>
                            <td class="content" style="width: 5%; text-align: center;">0.5</td>
                            <td class="title" style="width: 10%; text-align: center;">上次的截距值(ppb)</td>
                            <td class="content" style="width: 5%; text-align: center;">0.8</td>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">变化(%)</td>
                            <td class="content" style="width: 5%; text-align: center;">32</td>
                            <td class="title" style="width: 10%; text-align: center;">变化(ppb)</td>
                            <td class="content" style="width: 5%; text-align: center;">25</td>

                        </tr>

                    </table>
                </div>


            </telerik:RadPane>

        </telerik:RadSplitter>

    </form>
</body>
</html>
