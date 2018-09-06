<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeomTrafficHardwareInfo.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.TeomTrafficHardwareInfo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        table.gridtable {
            font-family: verdana,arial,sans-serif;
            font-size: 11px;
            color: #333333;
            border-width: 1px;
            border-color: #666666;
            border-collapse: collapse;
        }

            table.gridtable th {
                border-width: 1px;
                padding: 8px;
                border-style: solid;
                border-color: #666666;
                background-color: #dedede;
            }

            table.gridtable td {
                border-width: 1px;
                padding: 8px;
                border-style: solid;
                border-color: #666666;
                background-color: #ffffff;
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
            <telerik:RadPane ID="paneWhere" runat="server" Height="1000px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 700px; height: 5%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 10%; text-align: center;">站点：
                        </td>
                        <td style="width: 10%;text-align: center;">

                            <asp:Label ID="lblInstrumentType" runat="server" Text="上方山"></asp:Label>
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm" Visible="false"></CbxRsm:PointCbxRsm>

                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" DropDownWidth="420" ID="factorCbxRsm" Visible="false"></CbxRsm:FactorCbxRsm>
                        </td>

                        <td class="title" style="width: 10%; text-align: center;">操作人：
                        </td>
                        <td class="title" style="width: 15%; text-align: center;">
                            <asp:Label ID="lblSiteName" runat="server" Text="张三"></asp:Label>
                        </td>
                        <td class="title" style="width: 15%; text-align: center;">日期:</td>
                        <td >
                            <asp:Label ID="Label26" runat="server" Text="2015.05.05"></asp:Label>

                        </td>
                        
                    </tr>
                    <tr>
                        <td class="title" style="width: 10%; text-align: center;">签名：</td>
                        <td style="width: 10%; text-align: center;">
                            <asp:Label ID="Label27" runat="server" Text="上方山"></asp:Label>

                        </td>
                        <td class="title" style="width: 15%; text-align: center;">开始日期：</td>
                        <td style="width: 10%; text-align: center;">
                            <asp:Label ID="Label8" runat="server" Text="2015.02.02"></asp:Label>

                        </td>
                        <td class="title" style="width: 15%; text-align: center;">结束日期：</td>
                        <td >
                            <asp:Label ID="Label9" runat="server" Text="2015.08.02"></asp:Label>

                        </td>
                    </tr>
                </table>
               
                <div style="margin:10px 0px 10px 10px;">
                    <table style="width: 700px; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">
                        <tr>
                            <th colspan="4" class="title" style="width: 10%; text-align: center;">环境状况</th>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">温度</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label1" runat="server" Text="29℃"></asp:Label>
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">压力</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label2" runat="server" Text="30"></asp:Label>
                            </td>
                        </tr>

                    </table>

                </div>
                
                <div style="margin:10px 0px 10px 10px;">
                    <table style="width: 700px; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">
                        <tr>
                            <th colspan="4" class="title" style="width: 10%; text-align: center;">TEOM信息</th>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">控制单元序列号</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">模型号</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">变送器单元序列号</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">设备号</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label28" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
                
                <div style="margin:10px 0px 10px 10px;">
                    <table style="width: 700px; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">
                        <tr>
                            <th colspan="4" class="title" style="width: 10%; text-align: center;">流量计信息</th>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">模型号</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label10" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">认证日期</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label11" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">设备号</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label12" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">有效期</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label13" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">认证范围</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label14" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">校准方程</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label15" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>

                <div style="margin:10px 0px 10px 10px;">
                    <table style="width: 700px; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">
                        <tr>
                            <th colspan="4" class="title" style="width: 10%; text-align: center;">TEOM原始平均温度/压力（设定温度/流量菜单）</th>

                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">温度</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label6" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">压力</td>
                            <td class="content" style="width: 5%; text-align: center;">
                                <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
                
                <div style="margin:10px 0px 10px 10px;">
                    <table style="width: 700px; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">
                        <tr>
                            <th colspan="6" class="title" style="width: 10%; text-align: center;">流量校准（软件）</th>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;">设定点[l/min.]<br />
                                （设定温度/流量菜单）
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">流量计读数</td>
                            <td class="content" style="width: 5%; text-align: center;">实际流量<br />
                                [l/min.]
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">修正流量*<br />
                                [l/min.]</td>
                            <td class="content" style="width: 5%; text-align: center;">%误差**
                            </td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">主流量（校准前）</td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">主流量（校准后）</td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">辅流量（校准前）</td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">辅流量（校准后）</td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                        </tr>
                    </table>
                </div>
               
                <div style="margin:10px 0px 10px 10px;">
                    <table style="width: 700px; height: 10%;" cellspacing="1" cellpadding="1" class=" gridtable" border="1">
                        <tr>
                            <th colspan="6" class="title" style="width: 10%; text-align: center;">漏气检查</th>
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;">零漂（泵关）
                            </td>
                            <td class="title" style="width: 10%; text-align: center;">读数（泵开）</td>
                            <td class="content" style="width: 5%; text-align: center;">净读数***
                            </td>
                            
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">主泄露量读数[l/min.]</td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                       
                        </tr>
                        <tr>
                            <td class="title" style="width: 10%; text-align: center;">辅泄露量读数[l/min.]</td>
                            <td class="content" style="width: 5%; text-align: center;"></td>
                            <td class="title" style="width: 10%; text-align: center;"></td>
                            <td class="content" style="width: 5%; text-align: center;"></td>

                        </tr>


                    </table>
                </div>


            </telerik:RadPane>

        </telerik:RadSplitter>

    </form>
</body>
</html>
