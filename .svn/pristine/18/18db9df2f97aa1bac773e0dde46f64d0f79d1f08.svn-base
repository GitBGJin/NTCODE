<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeekMaintenanceSearch.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.WeekMaintenanceSearch" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <table style="width: 100%" class="Table_Customer">
            <tr class="btnTitle">
                <td class="btnTitle">
                    <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" SkinID="ImgBtnSave" />
                </td>
            </tr>

        </table>
        <table id="Tb" style="width: 100%; height: 60px" cellspacing="1" cellpadding="0" class="Table_Customer"
            border="0">
            <tr>
                <td class="title" style="width: 80px">站点
                </td>
                <td class="content" style="width: 180px;">
                    <CbxRsm:PointCbxRsm runat="server" ApplicationType="Water" CbxWidth="220" CbxHeight="350" MultiSelected="false" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                </td>
                <td class="title" style="width: 80px; text-align: center;">测定日期
                </td>
                <td class="content" style="width: 180px;">
                    <telerik:RadDateTimePicker ID="dtpTime" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm:ss"
                        DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="220"
                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                        TimeView-HeaderText="小时" />
                </td>
                <td class="title" style="width: 80px; text-align: center;">操作人员
                </td>
                <td class="content" style="width: 180px;">
                    <telerik:RadTextBox runat="server" ID="rtxPerson" Width="220px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <telerik:RadSplitter ID="splitterWeek" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWeekTab" runat="server" Scrolling="None" Width="100%" Height="26px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadTabStrip ID="rtsWeek" runat="server" SelectedIndex="0" MultiPageID="multiPageDevice"
                    CssClass="RadTabStrip_Customer">
                    <Tabs>
                        <telerik:RadTab Text="站房设备">
                        </telerik:RadTab>
                        <telerik:RadTab Text="站内文件">
                        </telerik:RadTab>
                        <telerik:RadTab Text="监测仪器设备表">
                        </telerik:RadTab>
                        <telerik:RadTab Text="分析仪器点检及维护">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneWeek" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPageDevice" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pvDevice" runat="server" Visible="true">
                        <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
                            BorderWidth="0" BorderStyle="None" BorderSize="0">
                            <telerik:RadPane ID="RadPane1" runat="server" Width="100%" Scrolling="None"
                                BorderWidth="0" BorderStyle="None" BorderSize="0">
                                <telerik:RadGrid ID="gridWeekDevice" runat="server" GridLines="None" Height="100%" Width="100%"
                                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                                    OnNeedDataSource="gridWeekDevice_NeedDataSource" OnItemDataBound="gridWeekDevice_ItemDataBound"
                                    CssClass="RadGrid_Customer">
                                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="监测项目" UniqueName="TestItems" DataField="TestItems" HeaderStyle-Width="60%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn HeaderText="是" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%--<input type="checkbox" runat="server" id="jcxmYes" value="1" />--%>
                                                    <asp:CheckBoxList runat="server" ID="jcxmYes">
                                                        <asp:ListItem Value="1" Text=""></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="否" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBoxList runat="server" ID="jcxmNo">
                                                        <asp:ListItem Value="0" Text=""></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="备注" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <telerik:RadTextBox ID="RadTextBox1" runat="server">
                                                    </telerik:RadTextBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </telerik:RadPane>
                            <telerik:RadPane ID="RadPane2" runat="server" Width="100%" Scrolling="None"
                                BorderWidth="0" BorderStyle="None" BorderSize="0">
                                <telerik:RadGrid ID="gridDevice" runat="server" GridLines="None" Height="100%" Width="100%"
                                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                                    OnNeedDataSource="gridDevice_NeedDataSource" OnItemDataBound="gridDevice_ItemDataBound"
                                    CssClass="RadGrid_Customer">
                                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="监测项目" UniqueName="TestItems" DataField="TestItems" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60%" ItemStyle-HorizontalAlign="Center">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn HeaderText="监测结果" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <telerik:RadTextBox ID="jcjg" runat="server">
                                                    </telerik:RadTextBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </telerik:RadPane>
                        </telerik:RadSplitter>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvFile" runat="server" Visible="true">
                        <telerik:RadGrid ID="gridWeekFile" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                            AutoGenerateColumns="False" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="gridWeekFile_NeedDataSource" OnItemDataBound="gridWeekFile_ItemDataBound"
                            CssClass="RadGrid_Customer">
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn HeaderText="监测项目" DataField="TestItems" UniqueName="TestItems" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60%" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn HeaderText="是" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <input type="checkbox" value="1" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="否" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <input type="checkbox" value="0" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="备注" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <telerik:RadTextBox ID="RadTextBox2" runat="server">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvInstrument" runat="server" Visible="true">
                        <telerik:RadGrid ID="gridInstrument" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                            AutoGenerateColumns="False" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="gridInstrument_NeedDataSource" OnItemDataBound="gridInstrument_ItemDataBound"
                            CssClass="RadGrid_Customer">
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn HeaderText="监测因子" DataField="TestItems" UniqueName="TestItems" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn HeaderText="实际监测因子" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <input type="checkbox" value="1" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="仪器型号" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <telerik:RadDropDownList ID="rddlYQXH" runat="server">
                                            </telerik:RadDropDownList>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="仪器器号" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <telerik:RadDropDownList ID="rddlYQQH" runat="server">
                                            </telerik:RadDropDownList>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn HeaderText="上次手工校准日期" DataField="Testtime" UniqueName="Testtime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn HeaderText="数据质量是否正常" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <telerik:RadTextBox ID="rtbSJZL" runat="server">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvMaintenance" runat="server" Visible="true">
                        <table>
                            <tr>
                                <td>仪器类型</td>
                                <td>
                                    <telerik:RadDropDownList ID="rddlYQXH" runat="server">
                                    </telerik:RadDropDownList>
                                </td>
                            </tr>
                        </table>
                        <telerik:RadGrid ID="gridMaintenance" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                            AutoGenerateColumns="False" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="gridMaintenance_NeedDataSource" OnItemDataBound="gridMaintenance_ItemDataBound"
                            CssClass="RadGrid_Customer">
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn HeaderText="项目" DataField="TestItems" UniqueName="TestItems" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn HeaderText="是" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <input type="checkbox" value="1" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="否" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <input type="checkbox" value="0" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="指标读数" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30%">
                                        <ItemTemplate>
                                            <telerik:RadTextBox ID="zbds" runat="server">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn HeaderText="单位" DataField="TestDw" UniqueName="TestDw" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%"></telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
