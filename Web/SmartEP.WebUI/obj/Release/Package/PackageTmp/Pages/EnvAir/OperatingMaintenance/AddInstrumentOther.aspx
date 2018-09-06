<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddInstrumentOther.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.AddInstrumentOther" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../../Resources/CSS/common.css" rel="stylesheet" />
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
                        //var MasterTable = $find("< grdYear.ClientID %>").get_masterTableView();
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

                    //var dateBegin = $find("<dtpBegin.ClientID %>").get_selectedDate();
                    //var dateEnd = $find("< dtpEnd.ClientID %>").get_selectedDate();
                    //if (dateBegin == null) {
                    //    alert("开始时间不能为空！");
                    //    return false;
                    //}
                    //else if (dateEnd == null) {
                    //    alert("截至时间不能为空！");
                    //    return false;
                    //}
                    //else if (dateBegin > dateEnd) {
                    //    alert("开始时间不能大于截至时间！");
                    //    return false;
                    //}

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
                <telerik:AjaxSetting AjaxControlID="pointCbxRsmCh">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsmCh" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsmNo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsmNo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="grdYear">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdYear" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdYear" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdYear" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnComprehensiveSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdComprehensive" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnComprehensiveNoSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdNoComprehensive" LoadingPanelID="RadAjaxLoadingPanel1" />
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

                <telerik:AjaxSetting AjaxControlID="comprehensiveWeekBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="comprehensiveWeekFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="comprehensiveWeekEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="comprehensiveWeekTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="rbtnlNoComprehensiveType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlNoComprehensiveType" />
                        <telerik:AjaxUpdatedControl ControlID="comboPort" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCityProper" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCity" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCityModel" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="noComprehensiveWeekBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="noComprehensiveWeekFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="noComprehensiveWeekEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="noComprehensiveWeekTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%"  BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="40px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <!--年均值条件-->
                    <tr id="yearWhere">

                        <td class="title" style="width: 5%; text-align: center;">
                            <input id="btnSave" type="button" value="保存" />
                        </td>
                        <td>
                            <input id="btnOut" type="button" value="退出" /></td>
                    </tr>

                </table>
            </telerik:RadPane>

            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="370px" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0" >
                <div class="order-form" id="samplingMain">
                    <div class="profile">
                        <div style="float: left;">

                            <ul class="profile-form" style="float: left; border: solid 1px grey; height: 230px; width: 300px; margin-left: 10px;">

                                <li><span class="v" style="width: 150px;">多点线性校准结果&nbsp</span> <span class="k">
                                    <input id="Button1" type="button" value="新增" style="width: 80px;" />
                                </span></li>
                                <li><span class="v" style="width: 80px;">截距：</span> <span class="k">
                                    <telerik:RadTextBox ID="RadTextBox1" runat="server"></telerik:RadTextBox>

                                </span></li>
                                <li><span class="v" style="width: 80px;">斜率：</span> <span class="k">
                                    <telerik:RadTextBox ID="RadTextBox2" runat="server"></telerik:RadTextBox>

                                </span></li>
                                <li><span class="v" style="width: 80px;">相关系数：</span> <span class="k">
                                    <telerik:RadTextBox ID="RadTextBox4" runat="server"></telerik:RadTextBox>
                                </span></li>
                            </ul>

                        </div>

                        <div style="float: left;">

                            <ul class="profile-form" style="float: left; border: solid 1px grey; height: 230px; width: 300px; margin-left: 10px;">


                                <li><span class="v" style="width: 150px;">零/跨漂移情况&nbsp</span> <span class="k">
                                    <input id="Button3" type="button" value="新增" style="width: 80px;" />
                                </span></li>
                                <li><span class="v" style="width: 80px;">截距：</span> <span class="k">
                                    <telerik:RadTextBox ID="RadTextBox5" runat="server"></telerik:RadTextBox>

                                </span></li>
                                <li><span class="v" style="width: 80px;">斜率：</span> <span class="k">
                                    <telerik:RadTextBox ID="RadTextBox6" runat="server"></telerik:RadTextBox>

                                </span></li>
                                <li><span class="v" style="width: 80px;">相关系数：</span> <span class="k">
                                    <telerik:RadTextBox ID="RadTextBox7" runat="server"></telerik:RadTextBox>

                                </span></li>
                            </ul>
                        </div>

                    </div>
                </div>
            </telerik:RadPane>

        </telerik:RadSplitter>

    </form>
</body>
</html>
