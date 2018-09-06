<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubStationInspection.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.SubStationInspection" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .tabContent td {
            border: 1px solid #808080;
            border-collapse: collapse;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <table border="0" style="width: 80%; margin-left: 10%;">
            <tr style="text-align: center; font-size: 30px">
                <td>空气质量自动监测子站巡检维护记录</td>
            </tr>
        </table>
        <br />
        <table border="0" style="width: 80%; margin-left: 10%; border-collapse: collapse">
            <tr>
                <td class="title">子站名称
                </td>
                <td class="content">
                    <label runat="server" id="labSubStationName" style="border-bottom: 1px solid; display: inline-block; width: 150px;"></label>
                </td>
                <td class="title">日期
                </td>
                <td class="content">
                    <label runat="server" id="labDatetime" style="border-bottom: 1px solid; display: inline-block; width: 150px;"></label>
                </td>
                <td class="title">天气
                </td>
                <td class="content">
                    <label runat="server" id="labWeather" style="border-bottom: 1px solid; display: inline-block; width: 150px;"></label>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table border="0" style="width: 80%; margin-left: 10%; border-collapse: collapse">
            <tr>
                <td class="title">监测子站周边环境是否有变化或影响测定结果的作业？</td>
                <td class="content">
                    <asp:RadioButtonList runat="server" ID="rdblImpact" RepeatDirection="Horizontal" Width="150">
                        <asp:ListItem Text="有" Value="1"></asp:ListItem>
                        <asp:ListItem Text="没有" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td class="content"></td>
            </tr>
            <tr>
                <td class="title" colspan="3">如有，请说明
                    <label runat="server" id="labExplain" style="border-bottom: 1px solid; display: inline-block; width: 400px;"></label>
                </td>
            </tr>
        </table>
        <br />
        <table border="0" style="width: 80%; margin-left: 10%; border-collapse: collapse" class="tabContent">
            <tr>
                <td class="title" colspan="6">监测子站内部环境</td>
            </tr>
            <tr>
                <td class="title">站内温度</td>
                <td class="content">
                    <label runat="server" id="Label1"></label>
                </td>
                <td class="title">是否有异常噪声</td>
                <td class="content">
                    <label runat="server" id="Label2"></label>
                </td>
                <td class="title">空调工作是否正常</td>
                <td class="content">
                    <label runat="server" id="Label3"></label>
                </td>
            </tr>
            <tr>
                <td class="title">站内湿度</td>
                <td class="content">
                    <label runat="server" id="Label4"></label>
                </td>
                <td class="title">是否有异常气味</td>
                <td class="content">
                    <label runat="server" id="Label5"></label>
                </td>
                <td class="title">屋面是否有渗漏现象</td>
                <td class="content">
                    <label runat="server" id="Label6"></label>
                </td>
            </tr>
            <tr>
                <td class="title">机柜电压</td>
                <td class="content">
                    <label runat="server" id="Label7"></label>
                </td>
                <td class="title">机柜温度</td>
                <td class="content">
                    <label runat="server" id="Label8"></label>
                </td>
                <td class="title">稳压电源工作是否正常</td>
                <td class="content">
                    <label runat="server" id="Label9"></label>
                </td>
            </tr>
            <tr>
                <td class="title">地面是否清洁</td>
                <td class="content">
                    <label runat="server" id="Label10"></label>
                </td>
                <td class="title">设备是否积灰</td>
                <td class="content">
                    <label runat="server" id="Label11"></label>
                </td>
                <td class="title">本次是否做好环境卫生</td>
                <td class="content">
                    <label runat="server" id="Label12"></label>
                </td>
            </tr>
        </table>
        <table border="0" style="width: 80%; margin-left: 10%; border-collapse: collapse" class="tabContent">
            <tr>
                <td class="title">采样系统
                </td>
            </tr>
            <tr>
                <td class="title">
                    <telerik:RadGrid ID="gridSampling" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridSampling_NeedDataSource" OnItemDataBound="gridSampling_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="采样流量" DataField="Tstamp" UniqueName="Tstamp" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="采样压力" DataField="operator" UniqueName="operator" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="总管和支管<br />有无冷凝水或污物" DataField="dtBegin" UniqueName="dtBegin" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="是否清洗<br />采样头" DataField="dtEnd" UniqueName="dtEnd" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="是否清洗<br />采样管路" DataField="dtBegin" UniqueName="dtBegin" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="是否清洗<br />PM<sub>10</sub>切割头" DataField="dtEnd" UniqueName="dtEnd" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td class="title">备注：<label runat="server" id="labSamplingRemarks"></label>
                </td>
            </tr>
            <tr>
                <td class="title">数据采集系统
                </td>
            </tr>
            <tr>
                <td class="title">
                    <telerik:RadGrid ID="gridDataCollection" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridDataCollection_NeedDataSource" OnItemDataBound="gridDataCollection_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="工控机是否正常" DataField="Tstamp" UniqueName="Tstamp" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="PLC是否正常" DataField="operator" UniqueName="operator" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="数据情况是否正常" DataField="dtBegin" UniqueName="dtBegin" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td class="title">备注：<label runat="server" id="labDataCollection"></label>
                </td>
            </tr>
            <tr>
                <td class="title">仪器信息
                </td>
            </tr>
            <tr>
                <td class="title">
                    <telerik:RadGrid ID="gridInstrumentInfo" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="true" AllowMultiRowSelection="false" OnColumnCreated="gridInstrumentInfo_ColumnCreated"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridInstrumentInfo_NeedDataSource" OnItemDataBound="gridInstrumentInfo_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <%-- <Columns>
                                <telerik:GridBoundColumn HeaderText="工控机是否正常" DataField="Tstamp" UniqueName="Tstamp" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="PLC是否正常" DataField="operator" UniqueName="operator" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="数据情况是否正常" DataField="dtBegin" UniqueName="dtBegin" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>--%>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td class="title">备注：<label runat="server" id="labInstrumentInfo"></label>
                </td>
            </tr>
            <tr>
                <td class="title">校准器信息
                </td>
            </tr>
            <tr>
                <td class="title">
                    <telerik:RadGrid ID="gridCalibration" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="true" AllowMultiRowSelection="false" OnColumnCreated="gridInstrumentInfo_ColumnCreated"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridCalibration_NeedDataSource" OnItemDataBound="gridCalibration_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="校准器是否正常" DataField="Tstamp" UniqueName="Tstamp" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="零气发生器是否正常" DataField="operator" UniqueName="operator" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td class="title">备注：<label runat="server" id="labCalibration"></label>
                </td>
            </tr>
            <tr>
                <td class="title">其他需要说明的问题：<label runat="server" id="labOther"></label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
