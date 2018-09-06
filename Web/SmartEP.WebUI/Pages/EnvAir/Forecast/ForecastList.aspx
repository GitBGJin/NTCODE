<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForecastList.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Forecast.ForecastList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>发布</title>
    <style type="text/css">
        /*开始：一般Table的样式*/
        table.Table_Customer1 {
            border: 1px solid #FFFFFF;
            border-collapse: collapse;
        }

            table.Table_Customer1 td.border {
                border-bottom: 1px solid #fff;
            }

            table.Table_Customer1 td {
                color: #000;
                border-left: 1px solid #FFFFFF;
                border-right: 1px solid #ffffff;
                border-top: 1px solid #ffffff;
            }

                table.Table_Customer1 td.header {
                    text-align: left;
                    background-color: #d2e5f4;
                    height: 30px;
                    /*background-image: url('images/RadGridHeaderBg.png');*/
                    background-image: url(Images/Portal/allbg.gif);
                    background-position: 0px 0pt;
                    background-color: #85b4de;
                    padding-left: 10px;
                }

                table.Table_Customer1 td.title {
                    background-color: #d1eff5; /*#d2e5f4*/
                    text-align: center;
                    width: 120px;
                }

                table.Table_Customer1 td.content {
                    background-color: #d2e5f4;
                }

                table.Table_Customer1 td.btns {
                    text-align: center;
                }
        /*结束：一般Table的样式*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="radCbxAlarmType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radCbxAlarmType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="RadGridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript" language="javascript">
                function Refresh_Grid(args) {
                    if (args) {
                        var MasterTable = $find("<%= RadGrid1.ClientID %>").get_masterTableView();
                        MasterTable.rebind();
                    }
                }

                function RadGridRTB_ClientButtonClicking(sender, args) {
                    var CurrentBtn = args.get_item();
                    var CurrentBtnName = CurrentBtn.get_text();
                    var CurrentBtnCommandName = CurrentBtn.get_commandName();
                    switch (CurrentBtnName) {
                        case "增加":
                            window.radopen("ForecastAdd.aspx", "ForecastAdd");
                            break;
                        case "删除":
                            try {
                                args.set_cancel(!confirm('确定删除所有选中的记录？'));
                            } catch (e) {
                                alert(e);
                            }
                            break;
                        default:
                            break;
                    }
                }

                function RadGridChk_SelectAllRecord(Cbx, CbxID) {
                    var MasterTableView = $find('<%= RadGrid1.ClientID %>').get_masterTableView().get_element();
                    var checkboxes = MasterTableView.tBodies[0].getElementsByTagName("input");
                    for (var i = 0; i < checkboxes.length; i++) {
                        var pos = checkboxes[i].id.indexOf(CbxID);
                        if (pos != -1) {
                            if (Cbx.checked) {
                                checkboxes[i].checked = true;
                            }
                            else {
                                checkboxes[i].checked = false;
                            }
                        }
                    }
                }
            </script>
        </telerik:RadScriptBlock>
        <table id="maintable" class="Table_Customer1" width="100%">
            <tr>
                <td class="title" style="width: 60px; font-size: 12px;">开始时间</td>
                <td class="content">
                    <telerik:RadDateTimePicker ID="RadDTBegin" runat="server" Width="185px" MinDate="1900-01-01 00:00:00"
                        DateInput-Font-Size="10" DateInput-Font-Bold="true" DateInput-DateFormat="yyyy-MM-dd HH:mm:ss"
                        DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" TimeView-HeaderText="小时" />
                </td>
                <td class="title" style="width: 60px; font-size: 12px;">结束时间</td>
                <td class="content">
                    <telerik:RadDateTimePicker ID="RadDTEnd" runat="server" Width="185px" MinDate="1900-01-01 00:00:00"
                        DateInput-Font-Size="10" DateInput-Font-Bold="true" DateInput-DateFormat="yyyy-MM-dd HH:mm:ss"
                        DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" TimeView-HeaderText="小时" />
                </td>
            </tr>
        </table>

        <telerik:RadGrid ID="RadGrid1" runat="server" GridLines="None" Height="100%" Width="100%"
            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
            AutoGenerateColumns="False" AllowMultiRowSelection="true"
            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false" AllowMultiRowEdit="true"
            ShowHeader="true" CssClass="RadGrid_Customer"
            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
            OnNeedDataSource="RadGrid1_NeedDataSource" OnColumnCreated="RadGrid1_ColumnCreated"
            OnItemCreated="RadGrid1_ItemCreated" OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand">

            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />

            <MasterTableView DataKeyNames="ID,AQIClassA,PrimaryPollutantA,AQIClassB,PrimaryPollutantB,AQIClassC,PrimaryPollutantC"
                ClientDataKeyNames="ID,AQIClassA,PrimaryPollutantA,AQIClassB,PrimaryPollutantB,AQIClassC,PrimaryPollutantC"
                GridLines="None" PageSize="20" CommandItemDisplay="Top" EditMode="InPlace"
                NoMasterRecordsText="没有数据" NoDetailRecordsText="没有数据" ShowHeadersWhenNoRecords="true">
                <CommandItemTemplate>
                    <telerik:RadToolBar ID="RadGridRTB" runat="server" Width="100%" OnButtonClick="RadGridRTB_ButtonClick" OnClientButtonClicking="RadGridRTB_ClientButtonClicking" />
                </CommandItemTemplate>

                <Columns>
                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="40px">
                        <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn HeaderText="时段" UniqueName="AQITimeA" DataField="AQITimeA" HeaderStyle-Width="25px" ItemStyle-Width="25px" ReadOnly="false" />

                    <telerik:GridTemplateColumn HeaderText="空气质量" UniqueName="AQIClassA" ReadOnly="false">
                        <HeaderStyle Width="70px" HorizontalAlign="Center" />
                        <ItemStyle Width="70px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="LabAQIClassA" runat="server" Text='<%#Eval("AQIClassA").ToString() %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadComboBox ID="RadCbxAQIClassA" runat="server"></telerik:RadComboBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="首要<BR/>污染物" UniqueName="PrimaryPollutantA" ReadOnly="false">
                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                        <ItemStyle Width="60px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="LabPrimaryPollutantA" runat="server" Text='<%#Eval("PrimaryPollutantA").ToString() %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadComboBox ID="RadCbxPrimaryPollutantA" runat="server"></telerik:RadComboBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="AQI" UniqueName="AQIA" DataField="AQIA">
                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <%-- ------------------------------------------------------------------------------------------------------------------------------------------------------------- --%>
                    <telerik:GridBoundColumn HeaderText="时段" UniqueName="AQITimeB" DataField="AQITimeB" HeaderStyle-Width="25px" ItemStyle-Width="25px" ReadOnly="false" />

                    <telerik:GridTemplateColumn HeaderText="空气质量" UniqueName="AQIClassB" ReadOnly="false">
                        <HeaderStyle Width="70px" HorizontalAlign="Center" />
                        <ItemStyle Width="70px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="LabAQIClassB" runat="server" Text='<%#Eval("AQIClassB").ToString() %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadComboBox ID="RadCbxAQIClassB" runat="server"></telerik:RadComboBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="首要<BR/>污染物" UniqueName="PrimaryPollutantB" ReadOnly="false">
                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                        <ItemStyle Width="60px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="LabPrimaryPollutantB" runat="server" Text='<%#Eval("PrimaryPollutantB").ToString() %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadComboBox ID="RadCbxPrimaryPollutantB" runat="server"></telerik:RadComboBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="AQI" UniqueName="AQIB" DataField="AQIB">
                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <%-- ------------------------------------------------------------------------------------------------------------------------------------------------------------- --%>
                    <telerik:GridBoundColumn HeaderText="时段" UniqueName="AQITimeC" DataField="AQITimeC" HeaderStyle-Width="25px" ItemStyle-Width="25px" ReadOnly="false" />

                    <telerik:GridTemplateColumn HeaderText="空气质量" UniqueName="AQIClassC" ReadOnly="false">
                        <HeaderStyle Width="70px" HorizontalAlign="Center" />
                        <ItemStyle Width="70px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="LabAQIClassC" runat="server" Text='<%#Eval("AQIClassC").ToString() %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadComboBox ID="RadCbxAQIClassC" runat="server"></telerik:RadComboBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="首要<BR/>污染物" UniqueName="PrimaryPollutantC" ReadOnly="false">
                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                        <ItemStyle Width="60px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="LabPrimaryPollutantC" runat="server" Text='<%#Eval("PrimaryPollutantC").ToString() %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadComboBox ID="RadCbxPrimaryPollutantC" runat="server"></telerik:RadComboBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="AQI" UniqueName="AQIC" DataField="AQIC">
                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <%-- ------------------------------------------------------------------------------------------------------------------------------------------------------------- --%>
                    <telerik:GridBoundColumn HeaderText="评价" UniqueName="Description" DataField="Description" />
                    <telerik:GridBoundColumn HeaderText="发布单位" UniqueName="IssuedUnit" DataField="IssuedUnit" HeaderStyle-Width="80px" />
                    <telerik:GridBoundColumn HeaderText="发布时间" UniqueName="IssuedTime" DataField="IssuedTime" HeaderStyle-Width="110px" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                    <telerik:GridCheckBoxColumn HeaderText="发布" UniqueName="IsIssued" DataField="IsIssued" HeaderStyle-Width="25px" ItemStyle-Width="25px" HeaderStyle-HorizontalAlign="Center" />
                </Columns>
                <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
            </MasterTableView>

            <ClientSettings>
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
        </telerik:RadGrid>

        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="ForecastAdd" runat="server" Width="580px" Height="520px" ViewStateMode="Enabled"
                    ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Localization-Cancel="取消"
                    Localization-Close="关闭" Localization-Maximize="最大化" Localization-Minimize="最小化"
                    Localization-Reload="刷新" Localization-PinOff="浮动" Localization-PinOn="固定" />
            </Windows>
        </telerik:RadWindowManager>
    </form>
</body>
</html>
