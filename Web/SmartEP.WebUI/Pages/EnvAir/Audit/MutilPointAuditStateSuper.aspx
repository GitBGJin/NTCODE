﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MutilPointAuditStateSuper.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.MutilPointAuditStateSuper" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link type="text/css" rel="stylesheet" href="../../../App_Themes/Neptune/Telerik.Web.UI.Skins/Metro/RadSplitterAudit.css" />
    <style type="text/css">
        .RadCalendar_Neptune .rcTitlebar {
            background: #3A94D3 !important;
            background-image: none !important;
            border: 1px solid #3A94D3 !important;
        }

        #RadCalendar1_Title {
            color: #fff !important;
        }

        .RadCalendar_Neptune .rcTitlebar .RadCalendar_Neptune .rcTitlebar TABLE {
            background: #3A94D3 !important;
        }
    </style>
    <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
        <script type="text/javascript">
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

            function RadCalendarClick(sender, eventArgs) {
                
                var day = eventArgs.get_renderDay();
                if (day.get_isSelectable()) {
                    if (day._date && day._date.length == 3) {
                        document.getElementById("<%=selectDateTime.ClientID%>").value = day._date[0] + "-" + day._date[1] + "-" + day._date[2];
                    }
                    document.getElementById('EnterAudit').click();
                }
            }

            function onResponseEnd(sender, args) {
                GridResize();
                SetCalenderHight();
                ResizePageDiv();
            }

            //设置容器宽度、高度
            $("document").ready(function () {
                ResizePageDiv();
            });

            function loadSplitter(sender) {
                GridResize();
                SetCalenderHight();
            }

            //蒙版宽度高度设置
            function ResizePageDiv() {
                var bodyWidth = document.body.clientWidth;
                var bodyHeight = document.body.clientHeight;
                $('#pagediv').css("height", bodyHeight);
                $('#pagediv').css("width", bodyWidth);
            }

            //隐藏站点按钮
            function HidePanel(send, args) {
                $('#RAD_SLIDING_PANE_TAB_RadSlidingPane_Point').css("display", "none");
            }

            //显示站点按钮
            function ShowPanel(send, args) {
                $('#RAD_SLIDING_PANE_TAB_RadSlidingPane_Point').css("display", "block");

            }

            //重新设置表格高度
            function GridResize() {
                var bodyHeight = document.body.clientHeight;
                $('#RadGridAnalyze_GridData').css("height", bodyHeight - bodyHeight * 0.58 - 28 - 12);//设置表格高度 
            }

            //设置日历高度
            function SetCalenderHight() {
                var height = (parseFloat($('#RadPane3').css("height")) - 33 - 30 - 60 - 20) / 6;
                if (height > 10)
                    $(".tableAdt").height(height);
                else
                    $(".tableAdt").height(10);
            }

            function OnClientNotificationUpdated(sender, args) {
                var newMsgs = sender.get_value();
                if (newMsgs != 0) {
                    play();
                    sender.show();
                }
            }
            function OnClientNotificationHidden(sender, eventArgs) {
            }

            function CalendarViewChanged(sender, args) {
                //SetCalenderHight();
            }
            function checkAll(button, args) {
                for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                    document.getElementById("radioPoint_" + i).checked = true;
                }
            }
            function deleteAll(button, args) {
                for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                    document.getElementById("radioPoint_" + i).checked = false;
                }
            }
            function ReverseAll(button, args) {
                for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                    var objCheck = document.getElementById("radioPoint_" + i);
                    if (objCheck.checked)
                        objCheck.checked = false;
                    else
                        objCheck.checked = true;
                }
            }
            function CheckSelect(objectName) {
                
                var tb = document.getElementByIdx(objectName);

                for (var i = 0; i < tb.cells.length; i++) {
                    var chk = tb.cells[i].firstChild;
                    if (chk != event.srcElement) {
                        chk.checked = false;
                    }
                }
            }

            function CheckSelect1() {
                var tb = document.getElementById("radioPoint");
                for (var i = 0; i < tb.rows.length; i++) {
                    for (var j = 0; j < tb.rows[i].cells.length; j++) {
                        var chk = tb.rows[i].cells[j].firstChild;
                        if (chk != null && chk != event.srcElement) {
                            chk.checked = false;
                        }
                    }
                }
            };

            function CheckSelect() {
                var tb = document.getElementById("radioPollutant");
                for (var i = 0; i < tb.rows.length; i++) {
                    for (var j = 0; j < tb.rows[i].cells.length; j++) {
                        var chk = tb.rows[i].cells[j].firstChild;
                        if (chk != null && chk != event.srcElement) {
                            chk.checked = false;
                        }
                    }
                }
            };
        </script>
    </telerik:RadCodeBlock>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnablePageHeadUpdate="false">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="refreshData">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditLog"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadCalendar1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radioPoint">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radioPollutant"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="selectAll">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radioPollutant"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%--<telerik:AjaxSetting AjaxControlID="EnterAudit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditLog"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadCalendar1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="inverse">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radioPollutant"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="Search">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditLog"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadCalendar1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="pagediv" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadCalendar1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadCalendar1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="DeleteAudit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditLog"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadCalendar1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="pagediv" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onResponseEnd" />
        </telerik:RadAjaxManager>

        <div runat="server" id="pagediv" style="position: absolute; z-index: -1;"></div>
        <telerik:RadSplitter ID="RadSplitter1" runat="server" BorderSize="0" Width="100%" Height="100%" OnClientLoad="loadSplitter">
            <!-- 左侧站点、因子-->
            <telerik:RadPane runat="server" ID="LeftPane" Width="20%">
                <telerik:RadSlidingZone ID="RadSlidingZone1" runat="server" DockedPaneId="RadSlidingPane_Point" BorderSize="0">
                    <telerik:RadSlidingPane ID="RadSlidingPane_Point" Width="200" runat="server" Title="站点" UndockText="收缩" DockText="固定" CollapseText="关闭"
                        OnClientExpanding="HidePanel" OnClientBeforeDock="HidePanel" OnClientBeforeUndock="ShowPanel" OnClientCollapsed="ShowPanel"
                        EnableDock="true">
                        <div>
                            <asp:CheckBoxList runat="server" ID="radioPoint" onclick="CheckSelect1()" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="2" OnSelectedIndexChanged="radioPoint_SelectedIndexChanged"></asp:CheckBoxList>
                        </div>
                        <div style="padding-top: 10px;">
                            <div style="width: 100%; height: 28px; color: #fff; background-color: #3A94D3;">
                                <div style="padding-top: 3px; padding-left: 5px;">仪器</div>
                            </div>
                            <asp:CheckBoxList runat="server" ID="radioPollutant" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="1" Font-Bold="true" Font-Size="11" OnSelectedIndexChanged="radioPollutant_SelectedIndexChanged" onclick="CheckSelect()"></asp:CheckBoxList>
                        </div>
                    </telerik:RadSlidingPane>
                </telerik:RadSlidingZone>
            </telerik:RadPane>
            <!--中间-->
            <telerik:RadPane ID="MiddlePane" runat="server" Scrolling="None" Width="78%" Height="100%">
                <telerik:RadSplitter ID="RadSplitter2" runat="server" Orientation="Horizontal" BorderSize="0" Width="100%" Height="100%">
                    <!--头部日历绑定-->
                    <telerik:RadPane ID="RadPane2" runat="server" Height="58%" Scrolling="None">
                        <div id="paramSelect">
                            <table style="width: 100%; text-align: left">
                                <tr>
                                    <td style="width: 70px;">开始时间：</td>
                                    <td style="width: 140px;">
                                        <telerik:RadDatePicker Width="120" runat="server" ID="RadDatePickerBegin" AutoPostBack="false"  Calendar-FastNavigationStep="12"
                                            MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时" OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged">
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td style="width: 70px;">结束时间：</td>
                                    <td style="width: 140px;">
                                        <telerik:RadDatePicker Width="120" runat="server" ID="RadDatePickerEnd" AutoPostBack="false"  Calendar-FastNavigationStep="12"
                                            MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时" OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged">
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td style="width: 80px;">
                                        <telerik:RadButton ID="Search" runat="server" BackColor="#3A94D3" Visible="true" ForeColor="White" AutoPostBack="true" OnClick="Search_Click">
                                            <ContentTemplate>
                                                <asp:Label ID="Label1" runat="server" ForeColor="White" Text="查询"></asp:Label>
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                    <td></td>
                                    <td id="tooltipLoading">
                                        <telerik:RadNotification ID="RadNotification1" runat="server" LoadContentOn="TimeInterval"
                                            Width="200" Height="30" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true"
                                            OnClientUpdated="OnClientNotificationUpdated" OnClientHidden="OnClientNotificationHidden" Title="提示" ShowTitleMenu="false" ShowCloseButton="false" VisibleTitlebar="false"
                                            TitleIcon="none" AutoCloseDelay="3000" OffsetX="360" OffsetY="2" Position="TopCenter">
                                            <ContentTemplate>
                                                <asp:Label ID="Label1" runat="server" Text="加载数据中，请耐心等待..."></asp:Label>
                                            </ContentTemplate>
                                        </telerik:RadNotification>
                                    </td>
                                    <td style="width: 80px;display:none">
                                        <telerik:RadButton ID="DeleteAudit" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="true" OnClick="DeleteAudit_Click">
                                            <ContentTemplate>
                                                <asp:Label runat="server" ID="Label4" ForeColor="White" Text="重新审核"></asp:Label>
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>

                                </tr>
                            </table>
                        </div>
                        <div>
                            <telerik:RadSplitter ID="RadSplitter3" runat="server" BorderSize="0" Width="100%" Height="100%">
                                <telerik:RadPane ID="RadPane3" runat="server" Scrolling="None" Width="35%" Height="100%">
                                    <div style="padding-top: 6px; padding-left: 6px;">
                                        <telerik:RadCalendar ID="RadCalendar1" runat="server" MultiViewRows="1" MultiViewColumns="1"
                                            MonthLayout="Layout_7columns_x_6rows" OnDefaultViewChanged="RadCalendar1_DefaultViewChanged" ClientEvents-OnDateClick="RadCalendarClick"
                                            CssClass="rcHeaderStyle" TitleFormat="yyyy 年 MM 月"
                                            AutoPostBack="True" FastNavigationSettings-TodayButtonCaption="当前年月" FastNavigationSettings-OkButtonCaption="确定"
                                            FastNavigationSettings-CancelButtonCaption="取消" ShowOtherMonthsDays="false" ShowColumnHeaders="true"
                                            ShowRowHeaders="false" CultureInfo="Chinese (People's Republic of China)" EnableMultiSelect="False"
                                            Width="100%" TitleStyle-Height="28" ClientEvents-OnCalendarViewChanged="CalendarViewChanged" DayCellToolTipFormat="yyyy年MM月dd日">
                                            <HeaderTemplate></HeaderTemplate>
                                            <CalendarDayTemplates>
                                                <telerik:DayTemplate ID="Adt" runat="server">
                                                    <Content>
                                                        <div style="text-align: center; cursor: pointer; font-size: 16px; font-weight: bold; color: Black; width: 100%; position: static;">
                                                            <table class="tableAdt" border="0" style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#getDay((DataBinder.Eval(Container, "ClientID")).ToString())%>'></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </Content>
                                                </telerik:DayTemplate>
                                                <telerik:DayTemplate ID="Adt0" runat="server">
                                                    <Content>
                                                        <div style="background: #F2514E; text-align: center; cursor: pointer; font-size: 16px; font-weight: bold; color: Black; width: 100%; position: static;">
                                                            <table class="tableAdt" border="0" style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#getDay((DataBinder.Eval(Container, "ClientID")).ToString())%>'></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </Content>
                                                </telerik:DayTemplate>
                                                <telerik:DayTemplate ID="Adt1" runat="server">
                                                    <Content>
                                                        <div style="background: #7DC733; text-align: center; cursor: pointer; font-size: 16px; font-weight: bold; color: Black; width: 100%; position: static;">
                                                            <table class="tableAdt" border="0" style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#getDay((DataBinder.Eval(Container, "ClientID")).ToString())%>'></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </Content>
                                                </telerik:DayTemplate>
                                                <telerik:DayTemplate ID="Adt2" runat="server">
                                                    <Content>
                                                        <div style="background: #87CEFA; text-align: center; cursor: pointer; font-size: 16px; font-weight: bold; color: Black; width: 100%; position: static;">
                                                            <table class="tableAdt" border="0" style="width: 100%;">
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#getDay((DataBinder.Eval(Container, "ClientID")).ToString())%>'></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </Content>
                                                </telerik:DayTemplate>
                                            </CalendarDayTemplates>
                                            <CalendarTableStyle Width="100%" />
                                        </telerik:RadCalendar>
                                        <div style="border-bottom: solid 1px; border-right: solid 1px; border-left: solid 1px; border-color: #3A94D3;">
                                            <table class="Table_Customer" style="width: 100%; height: 33px; color: White; font-size: 12px;">
                                                <tr>
                                                    <td class="title" style="width: 60px;">图示:</td>
                                                    <td class="content" style="width: 20px;">
                                                        <div style="width: 20px; height: 20px; background: #F2514E;"></div>
                                                    </td>
                                                    <td class="content" style="width: 60px; vertical-align: central;">&nbsp;未审核&nbsp;&nbsp;</td>
                                                    <td class="content" style="width: 20px;">
                                                        <div style="width: 20px; height: 20px; background: #7DC733;"></div>
                                                    </td>
                                                    <td class="content" style="width: 60px; vertical-align: central;">已审核</td>
                                                    <td class="content" style="width: 20px;">
                                                        <div style="width: 20px; height: 20px; background: #87CEFA;"></div>
                                                    </td>
                                                    <td class="content" style="width: 60px; vertical-align: central;">部分审核</td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </telerik:RadPane>
                                <telerik:RadPane ID="RadPane4" runat="server" Scrolling="None" Width="65%" Height="100%">
                                    <div style="padding: 6px;">
                                        <div style="width: 100%; height: 28px; color: #fff; background-color: #3A94D3;">
                                            <div style="padding-top: 6px; padding-left: 6px;">日志信息</div>
                                        </div>
                                        <telerik:RadGrid ID="gridAuditLog" runat="server" GridLines="None"
                                            AllowPaging="false" PageSize="24" AllowSorting="false" Height="550px"
                                            AutoGenerateColumns="false" AllowMultiRowSelection="false"
                                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                                            CssClass="RadGrid_Customer" OnNeedDataSource="gridAuditLog_NeedDataSource" OnItemDataBound="gridAuditLog_ItemDataBound" ShowHeader="true">
                                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据" ShowHeader="true" ShowHeadersWhenNoRecords="true">
                                                <Columns>
                                                    <telerik:GridBoundColumn HeaderText="数据时间" UniqueName="Tstamp" DataField="Tstamp" DataFormatString="{0:MM-dd HH:mm}" />
                                                    <%--<telerik:GridTemplateColumn HeaderText="站点名称" UniqueName="PointName" DataField="PointName" />--%>
                                                    <telerik:GridBoundColumn HeaderText="仪器" UniqueName="PollutantName" DataField="PollutantName" />
                                                    <%--<telerik:GridBoundColumn HeaderText="原始值" UniqueName="SourcePollutantDataValue" DataField="SourcePollutantDataValue" />--%>
                                                    <telerik:GridBoundColumn HeaderText="因子" UniqueName="AuditPollutantDataValue" DataField="AuditPollutantDataValue" />
                                                    <telerik:GridBoundColumn HeaderText="审核人" UniqueName="UpdateUser" DataField="UpdateUser" />
                                                    <telerik:GridBoundColumn HeaderText="审核日期" UniqueName="AuditTime" DataField="AuditTime" DataFormatString="{0:MM-dd HH:mm}" />
                                                </Columns>
                                            </MasterTableView>
                                            <CommandItemStyle Width="100%" />
                                            <ClientSettings>
                                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="2"
                                                    SaveScrollPosition="true"></Scrolling>
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </div>
                                </telerik:RadPane>
                            </telerik:RadSplitter>
                        </div>
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Width="1000" Height="1000">
        </telerik:RadAjaxLoadingPanel>
        <input type="button" id="refreshData" style="display: none;" />
        <asp:HiddenField ID="factorNames" runat="server" Value="" />
        <div style="display: none;">
            <asp:HiddenField ID="selectDateTime" runat="server" Value="2016-01-01" />
            <telerik:RadButton ID="EnterAudit" runat="server" AutoPostBack="true" OnClick="EnterAudit_Click"></telerik:RadButton>
        </div>
    </form>
</body>
</html>

