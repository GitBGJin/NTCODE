<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMaintenance.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.AddMaintenance" %>

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
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="40px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <!--年均值条件-->
                    <tr id="yearWhere">
                        <td class="title" style="width: 5%; text-align: center;">测点:
                        </td>
                        <td class="content" style="width: 15%;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Water" CbxWidth="200" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>

                        </td>

                        <td class="title" style="width: 5%; text-align: center;">保养人：</td>
                        <td class="title" style="width: 10%; text-align: center;">
                            <telerik:RadTextBox ID="RadTextBox1" runat="server"></telerik:RadTextBox>
                        </td>
                        <td class="title" style="width: 5%; text-align: center;">保养日期：</td>
                        <td class="title" style="width: 5%; text-align: center;">
                            <telerik:RadTextBox ID="RadTextBox2" runat="server"></telerik:RadTextBox>
                        </td>
                        <td class="title" style="width: 5%; text-align: center;">
                            <input id="btnSave" type="button" value="保存" />
                        </td>
                        <td>
                            <input id="btnOut" type="button" value="退出" /></td>
                    </tr>

                </table>
            </telerik:RadPane>

            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="370px" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div class="order-form" id="samplingMain">
                    <div class="profile">
                        <div style="float: left;">
                            <div style="margin-left: 10px; width: 102px;">仪器名称</div>
                            <ul class="profile-form" style="float: left; border: solid 1px grey; height: 320px; width: 300px; margin-left: 10px;">

                                <li><span class="k">

                                    <asp:RadioButtonList ID="rbtnlNoComprehensiveType" runat="server" RepeatDirection="Vertical">
                                        <asp:ListItem Selected="True" Value="hour">二氧化硫分析仪</asp:ListItem>
                                        <asp:ListItem Value="">一氧化氮分析仪</asp:ListItem>
                                        <asp:ListItem Value="">二氧化氮分析仪</asp:ListItem>
                                        <asp:ListItem Value="">氮氧化物分析仪</asp:ListItem>
                                        <asp:ListItem Value="">一氧化碳分析仪</asp:ListItem>
                                        <asp:ListItem Value="">臭氧分析仪</asp:ListItem>
                                        <asp:ListItem Value="">可吸入颗粒物分析仪</asp:ListItem>
                                        <asp:ListItem Value="">其他</asp:ListItem>
                                    </asp:RadioButtonList>
                                </span></li>

                            </ul>

                        </div>

                        <div style="float: left;">
                            <div style="margin-left: 10px;">项目名称</div>
                            <ul class="profile-form" style="float: left; height: 320px; width: 300px; margin-left: 10px;">
                                <li style="margin-top: -7px; margin-left: -40px;"><span class="k">

                                    <textarea id="TextArea1" cols="20" rows="2" style="height: 316px; width: 334px;"></textarea>
                                </span></li>

                            </ul>

                        </div>

                        <ul class="profile-form" style="float: left; height: 320px;">
                            <li><span class="v" style="width: 150px;">保养内容：</span> <span class="k">
                                <telerik:RadTextBox ID="rtbWaterName" runat="server"></telerik:RadTextBox>
                            </span></li>

                            <li><span class="v" style="width: 150px;">保养结果或技术指标：</span> <span class="k">
                                <telerik:RadTextBox ID="rtbSamplingPosition" runat="server"></telerik:RadTextBox>
                            </span></li>

                            <li style="text-align: right; margin-top: 200px;">
                                <asp:Button ID="Button2" runat="server" Text="添加" Width="100px" />
                            </li>
                        </ul>
                    </div>
                </div>
            </telerik:RadPane>

            <telerik:RadPane ID="RadPane1" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridSamplingRecord" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="false" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="false" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound" OnColumnCreated="grdDER_ColumnCreated"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridNumericColumn HeaderText="保养项目" UniqueName="PointName" DataField="PointName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                            <telerik:GridNumericColumn HeaderText="保养内容" UniqueName="PointName" DataField="PointName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                            <telerik:GridNumericColumn HeaderText="保养结果或技术指标" UniqueName="PointName" DataField="PointName" DecimalDigits="0" MaxLength="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />

                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="2"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>

            <%--<telerik:RadPane ID="RadPane2" runat="server" Width="100%" Height="200px" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div class="order-form" id="Div1">
                    <div class="profile">

                        <ul class="profile-form" style="float: left; height: 320px;">
                            <li><span class="v" style="width: 150px;">多点线性校准结果：</span> <span class="k">
                                <input id="Button3" type="button" value="新增" />

                            </span></li>

                            <li><span class="v" style="width: 150px;">零/跨飘逸情况：</span> <span class="k">
                                <input id="Button4" type="button" value="新增" />
                            </span></li>

                            <li style="text-align: right; margin-top: 200px;">
                                <asp:Button ID="Button1" runat="server" Text="保存" Width="100px" />
                            </li>
                        </ul>
                    </div>
                </div>
            </telerik:RadPane>--%>
        </telerik:RadSplitter>

    </form>
</body>
</html>
