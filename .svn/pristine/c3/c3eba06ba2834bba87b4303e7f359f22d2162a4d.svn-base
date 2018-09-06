<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoringComparisonInfo.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.MonitoringComparisonInfo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../../Resources/CSS/Table.css" rel="stylesheet" />
    <title></title>
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

               
                    <table id="Tb" style="width: 700px; height: 5%" cellspacing="1" cellpadding="0" class="Table_Customer"
                        border="0">
                        <tr>
                            <td class="title" style="width: 15%; text-align: right;">送检仪器名称：
                            </td>
                            <td style="width: 8%; text-align: center;">

                                <asp:Label ID="lblInstrumentType" runat="server" Text="南门"></asp:Label>
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm" Visible="false"></CbxRsm:PointCbxRsm>

                                <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" DropDownWidth="420" ID="factorCbxRsm" Visible="false"></CbxRsm:FactorCbxRsm>
                            </td>

                            <td class="title" style="width: 20%; text-align: right;">送检仪器型号及编号：
                            </td>
                            <td>
                                <asp:Label ID="lblSiteName" runat="server" Text="S1212121"></asp:Label>
                            </td>


                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: right;">送检单位：
                            </td>
                            <td style="width: 8%; text-align: center;">

                                <asp:Label ID="Label6" runat="server" Text="南门"></asp:Label>
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm1" Visible="false"></CbxRsm:PointCbxRsm>

                                <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" DropDownWidth="420" ID="factorCbxRsm1" Visible="false"></CbxRsm:FactorCbxRsm>
                            </td>

                            <td class="title" style="width: 8%; text-align: right;">校准地点：
                            </td>
                            <td  style="width: 12%; text-align: left;">
                                <asp:Label ID="Label7" runat="server" Text="苏州市政府"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 8%; text-align: right;">温度：</td>
                            <td style="width: 8%; text-align: center;">
                                <asp:Label ID="Label27" runat="server" Text="29℃"></asp:Label>

                            </td>
                            <td class="title" style="width: 8%; text-align: right;">湿度：</td>
                            <td style="width: 8%; text-align: left;">
                                <asp:Label ID="Label12" runat="server" Text="10"></asp:Label>

                            </td>
                            <td class="title" style="width: 8%; text-align: center;">气压：</td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="29atm / mmHg"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div style="clear: both;"></div>
                    <div style="margin: 10px 0px 10px 10px;">参比标准</div>
                    <div style="margin: 10px 0px 10px 10px;">
                        <table style="width: 700px; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">

                            <tr>
                                <td class="title" style="width: 10%; text-align: center;">仪器名称</td>
                                <td class="content" style="width: 5%; text-align: center;">
                                    <asp:Label ID="Label3" runat="server" Text="VOC分析仪"></asp:Label>
                                </td>
                                <td class="title" style="width: 10%; text-align: center;">仪器型号及编号</td>
                                <td class="content" style="width: 10%; text-align: center;">
                                    <asp:Label ID="Label4" runat="server" Text="S0101"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="title" style="width: 10%; text-align: center;">上次与NISTSRP34比对日期</td>
                                <td class="content" style="width: 5%; text-align: center;">
                                    <asp:Label ID="Label5" runat="server" Text="2015.05.03"></asp:Label>
                                </td>
                                <td class="title" style="width: 10%; text-align: center;" colspan="2"></td>
                            </tr>

                        </table>
                    </div>
                    <div style="margin: 10px 0px 10px 10px;">校准结果</div>
                    <div style="margin: 10px 0px 10px 10px;">
                        <table style="width: 700px; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">

                            <tr>
                                <td class="title" style="width: 10%; text-align: center;" colspan="2">校准以前</td>
                                <td class="content" style="width: 5%; text-align: center;" colspan="2">校准以后</td>

                            </tr>
                            <tr>
                                <td class="title" style="width: 10%; text-align: center;">相关系数</td>
                                <td class="content" style="width: 5%; text-align: center;">0.2</td>
                                <td class="title" style="width: 10%; text-align: center;">相关系数</td>
                                <td class="content" style="width: 5%; text-align: center;">0.5</td>
                            </tr>
                            <tr>
                                <td class="title" style="width: 10%; text-align: center;">斜率</td>
                                <td class="content" style="width: 5%; text-align: center;">0.4</td>
                                <td class="title" style="width: 10%; text-align: center;">斜率</td>
                                <td class="title" style="width: 10%; text-align: center;">1.5</td>
                            </tr>
                            <tr>

                                <td class="content" style="width: 5%; text-align: center;">截距(bbp)
                                </td>
                                <td class="title" style="width: 10%; text-align: center;">2</td>
                                <td class="content" style="width: 5%; text-align: center;">截距(bbp)
                                </td>
                                <td class="title" style="width: 10%; text-align: center;">5</td>
                            </tr>
                            <tr>

                                <td class="content" style="width: 5%; text-align: center;" rowspan="2">零点漂移
                                </td>
                                <td class="title" style="width: 10%; text-align: center;" rowspan="2">6</td>
                                <td class="content" style="width: 5%; text-align: center;">24小时零点漂移
                                </td>
                                <td class="title" style="width: 10%; text-align: center;">5</td>

                            </tr>

                            <tr>
                                

                            <td class="content" style="width: 5%; text-align: center;">24小时跨度漂移
                            </td>
                                <td class="content" style="width: 5%; text-align: center;"5></td>
                            </tr>
                            <tr>
                                <td class="content" style="width: 5%; text-align: center;" rowspan="2">跨度漂移
                                </td>
                                <td class="content" style="width: 5%; text-align: center;" rowspan="2">8</td>

                                <td class="content" style="width: 5%; text-align: center;">7天零漂移
                                </td>
                                <td class="content" style="width: 5%; text-align: center;">8</td>
                            </tr>
                            <tr>

                                <td class="content" style="width: 5%; text-align: center;">7天跨度漂移
                                </td>
                                <td class="content" style="width: 5%; text-align: center;">8</td>
                            </tr>

                        </table>
                    </div>
               


            </telerik:RadPane>
        </telerik:RadSplitter>

    </form>
</body>
</html>
