﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmCompsite.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Alarm.AlarmCompsite" %>

<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function SelectAll(obj) {
                    var theBox = obj;
                    xState = theBox.checked;
                    elem = theBox.form.elements;
                    for (i = 0; i < elem.length; i++)
                        if (elem[i].type == "checkbox" && elem[i].name != theBox.name && elem[i].name.split('$')[0] == theBox.name.split('$')[0]) {
                            if (elem[i].checked != xState)
                                elem[i].click();
                        }
                }

                function OnClientClicking() {
                    var date1 = new Date();
                    var date2 = new Date();
                    var date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    var date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        return false;
                    }
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                
                function Refresh_Grid(args) {
                    if (args) {
                        var alarmHandle = $find('AlarmHandle');
                        alarmHandle.set_modal(false);
                        alarmHandle.hide();

                        var MasterTable = $find("<%= gridMonitor.ClientID %>").get_masterTableView();
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

                //报警处理界面
                function ShowEditForm(id,type) {
                    var oWnd = radopen("AlarmHandle.aspx?AlarmUid=" + id+"&type="+type, "AlarmHandle");
                    return false;
                }
            </script>
        </telerik:RadCodeBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridMonitor">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridMonitor" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRTB" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%--<telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="comboAlarmType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridMonitor" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%" class="Table_Customer">
                    <tr>
                        <td class="title" style="width: 80px">站点:
                        </td>
                        <td class="content" style="width: 200px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="180" CbxHeight="350" DefaultAllSelected="true" MultiSelected="true" DropDownWidth="400" ID="pointCbxRsm" DefaultIPointMode="Region"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">报警类型:
                        </td>
                        <td class="content" style="width: 200px;">
                            <telerik:RadComboBox runat="server" ID="comboAlarmType" Width="150px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" Localization-CheckAllString="所有报警类型" EmptyMessage="请选择">
                            </telerik:RadComboBox>                           
                        </td>
                        <td class="title" style="width: 80px; text-align: center;"><input type="checkbox" id="chbAllData" runat="server" />
                        </td>
                        <td class="content" style="width: 200px;">
                            <asp:Label ID="chbAllDataLbl" runat="server" Text="显示所有报警信息"></asp:Label>
                        </td>
                        <td class="content" align="left" style="padding-left: 10px;" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 200px;">
                            <telerik:RadDateTimePicker ID="dtpBegin" runat="server" Width="180px" MinDate="1900-01-01" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">截止时间:
                        </td>
                        <td class="content" style="width: 200px;">
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" Width="180px" MinDate="1900-01-01" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">处理状态:
                        </td>
                        <td class="content" style="width: 200px;">已处理<asp:CheckBox ID="cbxHaveDeal" runat="server" Checked="true" />
                            未处理<asp:CheckBox ID="cbxNoDeal" runat="server" Checked="true" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridMonitor" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="gridMonitor_NeedDataSource" OnItemDataBound="gridMonitor_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据" ClientDataKeyNames="AlarmUid">
                        <CommandItemTemplate>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn
                                HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="selAll" Text="" runat="server" AutoPostBack="false" onclick="javascript:SelectAll(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span onclick="window.event.cancelBubble=true;">
                                        <asp:CheckBox ID="selAlarmConfig" runat="server" AutoPostBack="false" />
                                    </span>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Container.DataSetIndex + 1%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="点位名称" UniqueName="MonitoringPointName" DataField="MonitoringPointName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="100px" ReadOnly="true" Visible="True" />
                            <telerik:GridBoundColumn HeaderText="时间" UniqueName="CreatDateTime" DataField="CreatDateTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                            <telerik:GridBoundColumn HeaderText="报警类型" UniqueName="AlarmEventName" DataField="AlarmEventName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="100px" ReadOnly="true" Visible="True" />
                            <telerik:GridBoundColumn HeaderText="报警内容" UniqueName="Content" DataField="Content" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-Width="300px" ReadOnly="true" Visible="True" />
                            <telerik:GridBoundColumn HeaderText="颜色" UniqueName="AlarmGradeUid" DataField="AlarmGradeUid" HeaderStyle-Width="50px"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="是否处理" UniqueName="dealFlag" DataField="dealFlag" HeaderStyle-Width="70px"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="处理人" UniqueName="dealMan" DataField="dealMan" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70px" ReadOnly="true" Visible="True" />
                            <telerik:GridBoundColumn HeaderText="处理时间" UniqueName="dealTime" DataField="dealTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="120px" ReadOnly="true" Visible="True" />
                            <telerik:GridTemplateColumn HeaderText="处理" UniqueName="TemplateEditColumn" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <input id="Button1" type="image" value="button" src="../../../Resources/Images/telerik/common/action_edit.gif"
                                        onclick="ShowEditForm('<%#DataBinder.Eval(Container.DataItem,"AlarmUid")%>    ','single'); return false;">
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="是否审核" UniqueName="auditFlag" DataField="auditFlag" HeaderStyle-Width="70px"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="审核人" UniqueName="auditMan" DataField="auditMan" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70px" ReadOnly="true" Visible="false" />
                            <telerik:GridBoundColumn HeaderText="审核时间" UniqueName="auditTime" DataField="auditTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="120px" ReadOnly="true" Visible="false" />
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="3"
                            SaveScrollPosition="false"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close"
            EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="AlarmHandle" runat="server" Width="500px" Height="350px" ViewStateMode="Enabled"
                    ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Localization-Cancel="取消"
                    Localization-Close="关闭" Localization-Maximize="最大化" Localization-Minimize="最小化"
                    Localization-Reload="刷新" Localization-PinOff="浮动" Localization-PinOn="固定" />
            </Windows>
        </telerik:RadWindowManager>
    </form>
</body>
</html>
