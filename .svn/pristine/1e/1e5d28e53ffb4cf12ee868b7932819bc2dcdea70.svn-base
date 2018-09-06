<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PollutantSituationReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.PollutantSituationReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
                        var MasterTable = $find("<%= grdSituation.ClientID %>").get_masterTableView();
                        MasterTable.rebind();
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
                    var date1 = new Date();
                    var date2 = new Date();
                    date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        //sender.set_autoPostBack(false);
                        return false;
                    }
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                //Grid按钮行处理
                function gridRTB_ClientButtonClicking(sender, args) {
                    args.set_cancel(!OnClientClicking());
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
                <telerik:AjaxSetting AjaxControlID="grdSituation">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdSituation" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdSituation" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
               <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="dvPoints" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvProper" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdSituation" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
              <telerik:AjaxSetting AjaxControlID="rdlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rdlType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lastText" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="lastingDays" LoadingPanelID="RadAjaxLoadingPanel1"/>
                      <telerik:AjaxUpdatedControl ControlID="textClass" LoadingPanelID="RadAjaxLoadingPanel1" />
                      <telerik:AjaxUpdatedControl ControlID="textFactor" LoadingPanelID="RadAjaxLoadingPanel1" />
                      <telerik:AjaxUpdatedControl ControlID="dvClass" LoadingPanelID="RadAjaxLoadingPanel1" />
                      <telerik:AjaxUpdatedControl ControlID="dvFactor" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr style="height:12px;">
                       <td class="title" style="width: 80px; text-align: center;">查询范围：
                        </td>
                        <td class="content">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged" RepeatLayout="Flow" RepeatColumns="5">
                                <asp:ListItem Text="区域" Value="CityProper" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="测点" Value="Port"></asp:ListItem>

                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 97px; text-align:center"><div style="margin-left:7px;">查询区域/测点：</div>
                        </td>
                        <td class="content" style="width: 380px;" colspan="3">
                            <div runat="server" id="dvProper">
                                <%--<telerik:RadDropDownList runat="server" ID="ddlCityProper" Width="350px" Visible="false"></telerik:RadDropDownList>--%>
                                <telerik:RadComboBox runat="server" ID="rcbCityProper" Localization-CheckAllString="全选" Width="380px" CheckBoxes="true" Visible="false" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="姑苏区" Value="6a4e7093-f2c6-46b4-a11f-0f91b4adf379" />
                                        <telerik:RadComboBoxItem Text="吴中区" Value="e1c104f3-aaf3-4d0e-9591-36cdc83be15a" />
                                        <telerik:RadComboBoxItem Text="高新区" Value="f320aa73-7c55-45d4-a363-e21408e0aac3" />
                                        <telerik:RadComboBoxItem Text="工业园区" Value="69a993ff-78c6-459b-9322-ee77e0c8cd68" />
                                        <telerik:RadComboBoxItem Text="相城区" Value="8756bd44-ff18-46f7-aedf-615006d7474c" />
                                        <telerik:RadComboBoxItem Text="苏州市区" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Checked="true" />
                                        <telerik:RadComboBoxItem Text="张家港" Value="66d2abd1-ca39-4e39-909f-da872704fbfd" Checked="true" />
                                        <telerik:RadComboBoxItem Text="常熟市" Value="d7d7a1fe-493a-4b3f-8504-b1850f6d9eff" Checked="true" />
                                        <telerik:RadComboBoxItem Text="太仓市" Value="57b196ed-5038-4ad0-a035-76faee2d7a98" Checked="true" />
                                        <telerik:RadComboBoxItem Text="昆山市" Value="2e2950cd-dbab-43b3-811d-61bd7569565a" Checked="true" />
                                        <telerik:RadComboBoxItem Text="吴江区" Value="2fea3cb2-8b95-45e6-8a71-471562c4c89c" Checked="true" />
                                        <telerik:RadComboBoxItem Text="全市" Value="5a566145-4884-453c-93ad-16e4344c85c9" Checked="true" />
                                    </Items> 
                                </telerik:RadComboBox>
                            </div>
                            <div runat="server" id="dvPoints">
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="380" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                            </div>
                          </td>
                      <td class="title" style="width: 65px; text-align: center;" >
                        <div runat="server" id="textClass">类别：</div>
                        <div runat="server" id="textFactor">监测因子:</div>
                      </td>
                      <td class="content" colspan="2">
                        <div runat="server" id="dvClass">
                          <telerik:RadComboBox runat="server" ID="rcbFactors" Localization-CheckAllString="全选" Width="250px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                  <telerik:RadComboBoxItem Text="优" Value="0"  />
                                   <telerik:RadComboBoxItem Text="良" Value="1"/>
                                    <telerik:RadComboBoxItem Text="轻度污染" Value="2" Checked="true" />
                                    <telerik:RadComboBoxItem Text="中度污染" Value="3" Checked="true" />
                                    <telerik:RadComboBoxItem Text="重度污染" Value="4" Checked="true" />
                                   
                                     <telerik:RadComboBoxItem Text="严重污染" Value="5" Checked="true" />
                                    <telerik:RadComboBoxItem Text="无效" Value="6"  />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                        <div runat="server" id="dvFactor">
                          <telerik:RadComboBox runat="server" ID="rcbFactor" Localization-CheckAllString="全选" Width="250px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="PM2.5" Value="PM2.5" Checked="true" />
                                    <telerik:RadComboBoxItem Text="PM10" Value="PM10" Checked="true" />
                                    <telerik:RadComboBoxItem Text="二氧化氮" Value="NO2" Checked="true" />
                                    <telerik:RadComboBoxItem Text="二氧化硫" Value="SO2" Checked="true" />
                                    <telerik:RadComboBoxItem Text="一氧化碳" Value="CO" Checked="true" />
                                    
                                    <telerik:RadComboBoxItem Text="臭氧8小时" Value="O3-8小时" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                      </td>
                      <td></td>
                      </tr>
                      <tr style="height:8px;">
                        <td class="title">
                          统计类型:
                        </td>
                        <td class="content">
                          <telerik:RadDropDownList ID="rdlType" runat="server" Width="95px" AutoPostBack="true" OnSelectedIndexChanged="rdlType_SelectedIndexChanged" >
                                            <Items>
                                                <telerik:DropDownListItem runat="server" Selected="True" Value="1" Text="类别"/>
                                                <telerik:DropDownListItem runat="server" Text="污染持续" Value="2" />
                                                <telerik:DropDownListItem runat="server" Text="首要污染物" value="3" />
                                            </Items>
                                        </telerik:RadDropDownList>
                        </td>
                        <td class="title" style="width: 95px; text-align: center;">开始时间：
                        </td>
                        <td class="content">
                            <telerik:RadDatePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy年MM月dd日"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                MonthYearNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                MonthYearNavigationSettings-OkButtonCaption="确定" Width="150" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                MonthYearNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" />
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">结束时间：
                        </td>
                        <td class="content" style="width: 200px">
                            <telerik:RadDatePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy年MM月dd日"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                MonthYearNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                MonthYearNavigationSettings-OkButtonCaption="确定" Width="150" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                MonthYearNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" />
                        </td>
                        <td class="title">
                          <div id="lastText" runat="server" style="margin-left:15px;">持续天数>=</div>
                        </td>
                        <td class="content" style="width:60px;">
                          <asp:TextBox ID="lastingDays" runat="server" Width="30px" Text="2"></asp:TextBox>
                        </td>
                        <td class="content" align="left">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" ImageAlign="Middle"/>
                        </td>
                    </tr>

                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="grdSituation" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="true" AllowMultiRowSelection="false"
                     EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound "
                    CssClass="RadGrid_Customer" OnColumnCreated="grdSituation_ColumnCreated">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%-- <telerik:GridBoundColumn HeaderText="区域/测点" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn HeaderText="日期" DataField="DateTime" UniqueName="DateTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="持续天数" DataField="ContinuousDays" UniqueName="ContinuousDays" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="轻度污染" DataField="LightPollution" UniqueName="LightPollution" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="中度污染" DataField="ModeratePollution" UniqueName="ModeratePollution" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="重度污染" DataField="HighPollution" UniqueName="HighPollution" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="严重污染" DataField="SeriousPollution" UniqueName="SeriousPollution" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />--%>
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
