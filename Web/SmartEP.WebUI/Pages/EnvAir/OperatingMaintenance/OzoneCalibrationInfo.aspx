<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OzoneCalibrationInfo.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.OzoneCalibrationInfo" %>

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
                <table id="Tb" style="width: 700px; height: 5%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 15%; text-align: center;">仪器名称及型号：
                        </td>
                        <td style="width: 10%; text-align: center;">

                            <asp:Label ID="lblInstrumentType" runat="server" Text="S0101010"></asp:Label>
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm" Visible="false"></CbxRsm:PointCbxRsm>

                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" DropDownWidth="420" ID="factorCbxRsm" Visible="false"></CbxRsm:FactorCbxRsm>
                        </td>

                        <td class="title" style="width: 10%; text-align: center;">仪器编号：
                        </td>
                        <td>
                            <asp:Label ID="lblSiteName" runat="server" Text="A010101"></asp:Label>
                        </td>


                    </tr>
                    <tr>
                        <td class="title" style="width: 15%; text-align: center;">校准地点：
                        </td>
                        <td style="width: 10%; text-align: center;">

                            <asp:Label ID="Label10" runat="server" Text="南门"></asp:Label>
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm1" Visible="false"></CbxRsm:PointCbxRsm>

                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" DropDownWidth="420" ID="factorCbxRsm1" Visible="false"></CbxRsm:FactorCbxRsm>
                        </td>

                        <td class="title" style="width: 10%; text-align: center;">开始时间：
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="2015.02.5"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 10%; text-align: center;">室内温度：</td>
                        <td style="width: 10%; text-align: center;">
                            <asp:Label ID="Label27" runat="server" Text="30"></asp:Label>

                        </td>
                        <td class="title" style="width: 10%; text-align: center;">室内湿度：</td>
                        <td style="width: 10%; text-align: center;">
                            <asp:Label ID="Label8" runat="server" Text="20"></asp:Label>

                        </td>
                        <td class="title" style="width: 10%; text-align: center;">室内气压：</td>
                        <td style=" text-align:left;">
                            <asp:Label ID="Label9" runat="server" Text="30"></asp:Label>

                        </td>
                    </tr>
                </table>
                <div style="margin: 10px 0px 10px 10px;">参比标准：</div>
                <div style="margin: 10px 0px 10px 10px;">
                    <table style="width: 700px; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">

                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">仪器名称</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label1" runat="server" Text="SX010101"></asp:Label>
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">仪器型号及编号</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label2" runat="server" Text="01"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;" colspan="2">与NISTSRP比对日期</td>

                            <td class="title" style="width: 10%; text-align: center;" colspan="2">2015.05.05</td>

                        </tr>
                    </table>

                </div>

                <div style="margin: 10px 0px 10px 10px;">1、线性检查：单位（PPB）</div>
                <div style="margin: 10px 0px 10px 10px;">
                    <table style="width: 700px; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                            <td class="title" style="width: 10%; text-align: center;">1</td>
                            <td class="content" style="width: 5%; text-align: center;">2
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">3</td>
                            <td class="content" style="width: 5%; text-align: center;">4
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">5</td>
                            <td class="content" style="width: 5%; text-align: center;">6
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">7</td>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">序号</td>
                            <td class="content" style="width: 5%; text-align: center;">传递标准
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">被校仪器</td>
                            <td class="content" style="width: 5%; text-align: center;">被校仪器
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">被校仪器</td>
                            <td class="content" style="width: 5%; text-align: center;">被校仪器
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">被校仪器</td>
                            <td class="content" style="width: 5%; text-align: center;">被校仪器
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">被校仪器</td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">1</td>
                            <td class="content" style="width: 5%; text-align: center;">0
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">1</td>
                            <td class="content" style="width: 5%; text-align: center;">1</td>
                            <td class="title" style="width: 10%; text-align: center;">1</td>
                            <td class="content" style="width: 5%; text-align: center;">1</td>
                            <td class="title" style="width: 10%; text-align: center;">1</td>
                            <td class="content" style="width: 5%; text-align: center;">1</td>
                            <td class="title" style="width: 10%; text-align: center;">1</td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">2</td>
                            <td class="content" style="width: 5%; text-align: center;">100
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">2</td>
                            <td class="content" style="width: 5%; text-align: center;">2</td>
                            <td class="title" style="width: 10%; text-align: center;">2</td>
                            <td class="content" style="width: 5%; text-align: center;">2</td>
                            <td class="title" style="width: 10%; text-align: center;">2</td>
                            <td class="content" style="width: 5%; text-align: center;">2</td>
                            <td class="title" style="width: 10%; text-align: center;">2</td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">3</td>
                            <td class="content" style="width: 5%; text-align: center;">200
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">3</td>
                            <td class="content" style="width: 5%; text-align: center;">3</td>
                            <td class="title" style="width: 10%; text-align: center;">3</td>
                            <td class="content" style="width: 5%; text-align: center;">3</td>
                            <td class="title" style="width: 10%; text-align: center;">3</td>
                            <td class="content" style="width: 5%; text-align: center;">3</td>
                            <td class="title" style="width: 10%; text-align: center;">3</td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">4</td>
                            <td class="content" style="width: 5%; text-align: center;">300
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">4</td>
                            <td class="content" style="width: 5%; text-align: center;">4</td>
                            <td class="title" style="width: 10%; text-align: center;">4</td>
                            <td class="content" style="width: 5%; text-align: center;">4</td>
                            <td class="title" style="width: 10%; text-align: center;">4</td>
                            <td class="content" style="width: 5%; text-align: center;">4</td>
                            <td class="title" style="width: 10%; text-align: center;">4</td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">5</td>
                            <td class="content" style="width: 5%; text-align: center;">400
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">5</td>
                            <td class="content" style="width: 5%; text-align: center;">5</td>
                            <td class="title" style="width: 10%; text-align: center;">5</td>
                            <td class="content" style="width: 5%; text-align: center;">5</td>
                            <td class="title" style="width: 10%; text-align: center;">5</td>
                            <td class="content" style="width: 5%; text-align: center;">5</td>
                            <td class="title" style="width: 10%; text-align: center;">5</td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">6</td>
                            <td class="content" style="width: 5%; text-align: center;">700
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">6</td>
                            <td class="content" style="width: 5%; text-align: center;">6</td>
                            <td class="title" style="width: 10%; text-align: center;">6</td>
                            <td class="content" style="width: 5%; text-align: center;">6</td>
                            <td class="title" style="width: 10%; text-align: center;">6</td>
                            <td class="content" style="width: 5%; text-align: center;">6</td>
                            <td class="title" style="width: 10%; text-align: center;">6</td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">7</td>
                            <td class="content" style="width: 5%; text-align: center;">0
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">1</td>
                            <td class="content" style="width: 5%; text-align: center;">1</td>
                            <td class="title" style="width: 10%; text-align: center;">1</td>
                            <td class="content" style="width: 5%; text-align: center;">1</td>
                            <td class="title" style="width: 10%; text-align: center;">1</td>
                            <td class="content" style="width: 5%; text-align: center;">1</td>
                            <td class="title" style="width: 10%; text-align: center;">1</td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">1</td>
                            <td class="content" style="width: 5%; text-align: center;" colspan="2">被校仪器
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">截距</td>
                            <td class="content" style="width: 5%; text-align: center;">1</td>
                            <td class="title" style="width: 10%; text-align: center;">斜率</td>
                            <td class="content" style="width: 5%; text-align: center;">1</td>
                            <td class="title" style="width: 10%; text-align: center;">相关系数</td>
                            <td class="content" style="width: 5%; text-align: center;">1</td>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">2</td>
                            <td class="content" style="width: 5%; text-align: center;" colspan="2">被校仪器
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">截距</td>
                            <td class="content" style="width: 5%; text-align: center;">2</td>
                            <td class="title" style="width: 10%; text-align: center;">斜率</td>
                            <td class="content" style="width: 5%; text-align: center;">2</td>
                            <td class="title" style="width: 10%; text-align: center;">相关系数</td>
                            <td class="content" style="width: 5%; text-align: center;">2</td>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">3</td>
                            <td class="content" style="width: 5%; text-align: center;" colspan="2">被校仪器
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">截距</td>
                            <td class="content" style="width: 5%; text-align: center;">3</td>
                            <td class="title" style="width: 10%; text-align: center;">斜率</td>
                            <td class="content" style="width: 5%; text-align: center;">3</td>
                            <td class="title" style="width: 10%; text-align: center;">相关系数</td>
                            <td class="content" style="width: 5%; text-align: center;">3</td>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">4</td>
                            <td class="content" style="width: 5%; text-align: center;" colspan="2">被校仪器
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">截距</td>
                            <td class="content" style="width: 5%; text-align: center;">4</td>
                            <td class="title" style="width: 10%; text-align: center;">斜率</td>
                            <td class="content" style="width: 5%; text-align: center;">4</td>
                            <td class="title" style="width: 10%; text-align: center;">相关系数</td>
                            <td class="content" style="width: 5%; text-align: center;">4</td>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">5</td>
                            <td class="content" style="width: 5%; text-align: center;" colspan="2">被校仪器
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">截距</td>
                            <td class="content" style="width: 5%; text-align: center;">5</td>
                            <td class="title" style="width: 10%; text-align: center;">斜率</td>
                            <td class="content" style="width: 5%; text-align: center;">5</td>
                            <td class="title" style="width: 10%; text-align: center;">相关系数</td>
                            <td class="content" style="width: 5%; text-align: center;">5</td>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">6</td>
                            <td class="content" style="width: 5%; text-align: center;" colspan="2">被校仪器
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">截距</td>
                            <td class="content" style="width: 5%; text-align: center;">6</td>
                            <td class="title" style="width: 10%; text-align: center;">斜率</td>
                            <td class="content" style="width: 5%; text-align: center;">6</td>
                            <td class="title" style="width: 10%; text-align: center;">相关系数</td>
                            <td class="content" style="width: 5%; text-align: center;">6</td>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">7</td>
                            <td class="content" style="width: 5%; text-align: center;" colspan="2">被校仪器
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">截距</td>
                            <td class="content" style="width: 5%; text-align: center;">7</td>
                            <td class="title" style="width: 10%; text-align: center;">斜率</td>
                            <td class="content" style="width: 5%; text-align: center;">7</td>
                            <td class="title" style="width: 10%; text-align: center;">相关系数</td>
                            <td class="content" style="width: 5%; text-align: center;">7</td>

                        </tr>
                        
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;" colspan="3">平均值</td>
                            
                            <td class="title" style="width: 10%; text-align: center;">截距</td>
                            <td class="content" style="width: 5%; text-align: center;">3</td>
                            <td class="title" style="width: 10%; text-align: center;">斜率</td>
                            <td class="content" style="width: 5%; text-align: center;">3</td>
                            <td class="title" style="width: 10%; text-align: center;">相关系数</td>
                            <td class="content" style="width: 5%; text-align: center;">3</td>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;" colspan="3">被校仪器</td>
                            <td class="content" style="width: 5%; text-align: center;">4</td>
                            <td class="title" style="width: 10%; text-align: center;">标准偏差</td>
                            <td class="content" style="width: 5%; text-align: center;">4</td>
                            <td class="title" style="width: 13%; text-align: center;">相对标准偏差</td>
                            
                            <td class="title" style="width: 10%; text-align: center;" colspan="2">----</td>


                        </tr>
                    </table>
                </div>

            </telerik:RadPane>

        </telerik:RadSplitter>

    </form>
</body>
</html>
