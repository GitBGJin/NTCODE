<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MovingInstrumentCalibrate.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.QualityControl.MovingInstrumentCalibrate" %>

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
                <td>动态校准仪校准记录表</td>
            </tr>
        </table>
        <table border="0" style="width: 80%; margin-left: 10%;border-collapse:collapse" class="tabContent">        
            <tr>
                <td colspan="3">
                    <telerik:RadGrid ID="gridJBXX" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridJBXX_NeedDataSource" OnItemDataBound="gridJBXX_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="日期" DataField="Tstamp" UniqueName="Tstamp" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="操作者" DataField="operator" UniqueName="operator" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="开始时间" DataField="dtBegin" UniqueName="dtBegin" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="结束时间" DataField="dtEnd" UniqueName="dtEnd" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr style="text-align: center; font-size: 20px">
                <td colspan="3">设备相关信息</td>
            </tr>
            <tr>
                <td colspan="3">
                    <telerik:RadGrid ID="gridLLJXX" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridLLJXX_NeedDataSource" OnItemDataBound="gridLLJXX_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="流量计信息" HeaderText="流量计信息"
                                    HeaderStyle-HorizontalAlign="Center" />
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px" ColumnGroupName="流量计信息"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%#Container.DataSetIndex + 1%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn HeaderText="量程" DataField="range" UniqueName="range" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="流量计信息" />
                                <telerik:GridBoundColumn HeaderText="流量计型号" DataField="flowmeterModel" UniqueName="flowmeterModel" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="流量计信息" />
                                <telerik:GridBoundColumn HeaderText="流量计编号" DataField="flowmeterNum" UniqueName="flowmeterNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="流量计信息" />
                                <telerik:GridBoundColumn HeaderText="检定日期" DataField="testDate" UniqueName="testDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="流量计信息" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr style="text-align: center; font-size: 20px">
                <td colspan="3">动态仪校准信息</td>
            </tr>
            <tr>
                <td colspan="3">
                    <telerik:RadGrid ID="gridHJCSXX" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridHJCSXX_NeedDataSource" OnItemDataBound="gridHJCSXX_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="环境参数信息" HeaderText="环境参数信息"
                                    HeaderStyle-HorizontalAlign="Center" />
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="温度计型号" DataField="thermometerType" UniqueName="thermometerType" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="环境参数信息" />
                                <telerik:GridBoundColumn HeaderText="设备编号" DataField="TequipmentNum" UniqueName="TequipmentNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="环境参数信息" />
                                <telerik:GridBoundColumn HeaderText="检定日期" DataField="TtestDate" UniqueName="TtestDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="环境参数信息" />
                                <telerik:GridBoundColumn HeaderText="气压计型号" DataField="barometerType" UniqueName="barometerType" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="环境参数信息" />
                                <telerik:GridBoundColumn HeaderText="设备编号" DataField="BequipmentNum" UniqueName="BequipmentNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="环境参数信息" />
                                <telerik:GridBoundColumn HeaderText="检定日期" DataField="BtestDate" UniqueName="BtestDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="环境参数信息" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr style="text-align: center; font-size: 20px">
                <td colspan="3">环境参数测量结果</td>
            </tr>
            <tr>
                <td colspan="3">
                    <telerik:RadGrid ID="gridHJCSCEJG" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridHJCSCEJG_NeedDataSource" OnItemDataBound="gridHJCSCEJG_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px" ColumnGroupName="流量计信息"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%#Container.DataSetIndex + 1%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn HeaderText="时间" DataField="Tstamp" UniqueName="Tstamp" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="温度（℃）" DataField="Temperature" UniqueName="Temperature" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="气压（hpa）" DataField="airpressure" UniqueName="airpressure" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid></td>
            </tr>
            <tr style="text-align: center; font-size: 20px">
                <td colspan="3">校准气流量控制器校准结果（量程：<label runat="server" id="labRange"></label>）</td>
            </tr>
            <tr>
                <td colspan="3">
                    <telerik:RadGrid ID="gridJZQLL" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridJZQLL_NeedDataSource" OnItemDataBound="gridJZQLL_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px" ColumnGroupName="流量计信息"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%#Container.DataSetIndex + 1%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn HeaderText="设定值<br />mv" DataField="setValues" UniqueName="setValues" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="仪器读数<br />ml/min" DataField="instrumentReading" UniqueName="instrumentReading" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="流量计读数<br />ml/min" DataField="flowmeterReading" UniqueName="flowmeterReading" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="流量计修正读数<br />ml/min（质量流量）" DataField="correctionReading" UniqueName="correctionReading" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="输入校准器值<br />ml/min（质量流量）" DataField="calibratorValues" UniqueName="calibratorValues" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid></td>
            </tr>
            <tr>
                <td>斜率：<label runat="server" id="JZQXL"></label></td>
                <td>截距：<label runat="server" id="JZQJJ"></label></td>
                <td>相关系数：<label runat="server" id="JZQXGXS"></label>
                </td>
            </tr>
            <tr>
                <td colspan="3">评价：<label runat="server" id="labJZQEval"></label></td>
            </tr>
        </table>
        <br />
        <table border="0" style="width: 80%; margin-left: 10%; border-collapse:collapse" class="tabContent">
            <tr style="text-align: center; font-size: 20px">
                <td colspan="3">稀释气流量控制器校准结果（量程：<label runat="server" id="labXSQLL"></label>）</td>
            </tr>
            <tr>
                <td colspan="3">
                    <telerik:RadGrid ID="gridXSQLL" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridXSQLL_NeedDataSource" OnItemDataBound="gridXSQLL_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px" ColumnGroupName="流量计信息"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%#Container.DataSetIndex + 1%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn HeaderText="设定值<br />mv" DataField="setValues" UniqueName="setValues" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="仪器读数<br />ml/min" DataField="instrumentReading" UniqueName="instrumentReading" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="流量计读数<br />ml/min" DataField="flowmeterReading" UniqueName="flowmeterReading" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="流量计修正读数<br />ml/min（质量流量）" DataField="correctionReading" UniqueName="correctionReading" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="输入校准器值<br />ml/min（质量流量）" DataField="calibratorValues" UniqueName="calibratorValues" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid></td>
            </tr>
            <tr>
                <td>斜率：<label runat="server" id="labXSQXL"></label></td>
                <td>截距：<label runat="server" id="labXSQJJ"></label></td>
                <td>相关系数：<label runat="server" id="labXSQXGXS"></label>
                </td>
            </tr>
            <tr>
                <td colspan="3">评价：<label runat="server" id="labXSQEval"></label></td>
            </tr>
        </table>
    </form>
</body>
</html>
