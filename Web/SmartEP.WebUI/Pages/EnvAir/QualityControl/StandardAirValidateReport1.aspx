<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StandardAirValidateReport1.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.QualityControl.StandardAirValidateReport1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .tabContent  td {
            border: 1px solid #808080;
            border-collapse: collapse;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <table style="width: 80%; margin-left: 10%;">
            <tr style="text-align: center; font-size: 30px">
                <td>标准气体验证报告</td>
            </tr>
        </table>
        <table border="0" style="width: 80%; margin-left: 10%; border-collapse: collapse" class="tabContent">

            <tr>
                <td>
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
                                <telerik:GridBoundColumn HeaderText="报告日期" DataField="reportTime" UniqueName="reportTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="验证日期" DataField="verificationTime" UniqueName="verificationTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="操作人员" DataField="operator" UniqueName="operator" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="验证有效日期" DataField="effectiveTime" UniqueName="effectiveTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <%--<telerik:GridTemplateColumn HeaderText="压缩气体压力是否达到500 psig" DataField="judge" UniqueName="judge" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBoxList runat="server" ID="cblYSQTYL">
                                            <asp:ListItem Value="1" Text="是"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="否"></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>--%>
                                <telerik:GridBoundColumn HeaderText="压缩气体压力是否达到500 psig" DataField="judge" UniqueName="judge" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr style="text-align: center; font-size: 20px">
                <td>压缩气体</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gridYSQT" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridYSQT_NeedDataSource" OnItemDataBound="gridYSQT_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="参数" DataField="parameter" UniqueName="parameter" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="之前鉴定浓度,ppm" DataField="beforeConcentration" UniqueName="beforeConcentration" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="NO2不纯度,ppm/%" DataField="impure" UniqueName="impure" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gridPHQT" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridPHQT_NeedDataSource" OnItemDataBound="gridPHQT_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="QA实验室" HeaderText="QA实验室"
                                    HeaderStyle-HorizontalAlign="Center" />
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="平衡气体" DataField="equilibriumGas" UniqueName="equilibriumGas" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="钢瓶编号" DataField="steelNum" UniqueName="steelNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="钢瓶压力<br />psi" DataField="steelPressure" UniqueName="steelPressure" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="部门/工作组" DataField="Department" UniqueName="Department" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="地点" DataField="place" UniqueName="place" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="QA实验室" />
                                <telerik:GridBoundColumn HeaderText="楼层" DataField="floor" UniqueName="floor" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="QA实验室" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gridCKBZ" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridCKBZ_NeedDataSource" OnItemDataBound="gridCKBZ_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="参考标准" HeaderText="参考标准"
                                    HeaderStyle-HorizontalAlign="Center" />
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="参数" DataField="parameter" UniqueName="parameter" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="标准参考物质编号" DataField="referenceNum" UniqueName="referenceNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="钢瓶编号" DataField="steelNum" UniqueName="steelNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="钢瓶压力，psi" DataField="steelPressure" UniqueName="steelPressure" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="鉴定浓度，ppm/ppb" DataField="identification" UniqueName="identification" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="鉴定有效日期" DataField="effectiveDate" UniqueName="effectiveDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr style="text-align: center; font-size: 20px">
                <td>参考仪器</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gridFXY" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridFXY_NeedDataSource" OnItemDataBound="gridFXY_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="分析仪" HeaderText="分析仪"
                                    HeaderStyle-HorizontalAlign="Center" />
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="参数" DataField="parameter" UniqueName="parameter" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="分析仪" />
                                <telerik:GridBoundColumn HeaderText="生产厂商" DataField="Manufacturer" UniqueName="Manufacturer" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="分析仪" />
                                <telerik:GridBoundColumn HeaderText="型号" DataField="Model" UniqueName="Model" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="分析仪" />
                                <telerik:GridBoundColumn HeaderText="序列号" DataField="sequenceNum" UniqueName="sequenceNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="分析仪" />
                                <telerik:GridBoundColumn HeaderText="分析仪编号" DataField="analyzerNum" UniqueName="analyzerNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="分析仪" />
                                <telerik:GridBoundColumn HeaderText="最近校准日期" DataField="calibrationDate" UniqueName="calibrationDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="分析仪" />
                                <telerik:GridBoundColumn HeaderText="读数（Ppm/ppb）" DataField="calibrationReading" UniqueName="calibrationReading" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="分析仪" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gridJZQ" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridJZQ_NeedDataSource" OnItemDataBound="gridJZQ_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="校准器" HeaderText="校准器"
                                    HeaderStyle-HorizontalAlign="Center" />
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="生产厂商" DataField="Manufacturer" UniqueName="Manufacturer" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="校准器" />
                                <telerik:GridBoundColumn HeaderText="型号" DataField="Model" UniqueName="Model" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="校准器" />
                                <telerik:GridBoundColumn HeaderText="序列号" DataField="sequenceNum" UniqueName="sequenceNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="校准器" />
                                <telerik:GridBoundColumn HeaderText="校准器编号" DataField="analyzerNum" UniqueName="analyzerNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="校准器" />
                                <telerik:GridBoundColumn HeaderText="MFC鉴定日期" DataField="MFCcalibrationDate" UniqueName="MFCcalibrationDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="校准器" />
                                <telerik:GridBoundColumn HeaderText="MFC鉴定有效期" DataField="MFCValidityPeriod" UniqueName="MFCValidityPeriod" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="校准器" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr style="text-align: center; font-size: 20px">
                <td>鉴定结果</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gridJDJG" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridJDJG_NeedDataSource" OnItemDataBound="gridJDJG_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="参数" DataField="parameter" UniqueName="parameter" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="之前鉴定浓度,ppm" DataField="beforeConcentration" UniqueName="beforeConcentration" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="NO2不纯度,ppm/%" DataField="impure" UniqueName="impure" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
