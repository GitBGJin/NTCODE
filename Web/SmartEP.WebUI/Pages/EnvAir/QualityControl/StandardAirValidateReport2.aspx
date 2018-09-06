<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StandardAirValidateReport2.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.QualityControl.StandardAirValidateReport2" %>

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
        <table style="width: 80%; margin-left: 10%;">
            <tr style="text-align: center; font-size: 30px">
                <td>标准气体验证报告</td>
            </tr>
        </table>
        <table style="width: 80%; margin-left: 10%; border-collapse: collapse" class="tabContent">
            <tr>
                <td>
                    <telerik:RadGrid ID="gridBZQT" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridBZQT_NeedDataSource" OnItemDataBound="gridBZQT_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="待验证气体种类" DataField="gasType" UniqueName="gasType" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="验证地点" DataField="VerificationSite" UniqueName="VerificationSite" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="验证开始日期" DataField="dtBegin" UniqueName="dtBegin" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr style="text-align: center;">
                <td>待验证气体信息</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gridQTXX" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridQTXX_NeedDataSource" OnItemDataBound="gridQTXX_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="生产商" DataField="Manufacturer" UniqueName="Manufacturer" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="钢瓶编号" DataField="steelNum" UniqueName="steelNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="钢瓶压力" DataField="steelPressure" UniqueName="steelPressure" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="钢瓶浓度" DataField="steelConcentration" UniqueName="steelConcentration" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="介质" DataField="Medium" UniqueName="Medium" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="有效日期" DataField="ValidityPeriod" UniqueName="ValidityPeriod" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr style="text-align: center;">
                <td>分析仪器信息</td>
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
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="分析仪型号" DataField="analyzerModel" UniqueName="analyzerModel" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="设备编号" DataField="equipmentNum" UniqueName="equipmentNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="多点校准日期" DataField="calibrationDate" UniqueName="calibrationDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr style="text-align: center;">
                <td>校准器信息</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gridJZQXX" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridJZQXX_NeedDataSource" OnItemDataBound="gridJZQXX_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="设备型号" DataField="equipmentModel" UniqueName="equipmentModel" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="设备编号" DataField="equipmentNum" UniqueName="equipmentNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="MFC校准时间" DataField="MFCcalibrationDate" UniqueName="MFCcalibrationDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="臭氧发生器校准时间" DataField="O3calibrationDate" UniqueName="O3calibrationDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr style="text-align: center;">
                <td>零气源信息</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gridLQYXX" runat="server" GridLines="None" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridLQYXX_NeedDataSource" OnItemDataBound="gridLQYXX_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="设备型号" DataField="equipmentModel" UniqueName="equipmentModel" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="设备编号" DataField="equipmentNum" UniqueName="equipmentNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="出口压力（psi）" DataField="ExitPressure" UniqueName="ExitPressure" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr style="text-align: center;">
                <td>参考标准信息</td>
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
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="生产商" DataField="Manufacturer" UniqueName="Manufacturer" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="钢瓶编号" DataField="steelNum" UniqueName="steelNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="钢瓶压力" DataField="steelPressure" UniqueName="steelPressure" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="钢瓶浓度" DataField="steelConcentration" UniqueName="steelConcentration" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="介质" DataField="Medium" UniqueName="Medium" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="有效日期" DataField="ValidityPeriod" UniqueName="ValidityPeriod" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 80%; margin-left: 10%; text-align: center; border-collapse: collapse" class="tabContent">
            <tr>
                <td colspan="11">验证结果</td>
            </tr>
            <tr>
                <td>流量</td>
                <td colspan="2">稀释气</td>
                <td colspan="3">
                    <label runat="server" id="labXSQ"></label>
                </td>
                <td colspan="2">源气</td>
                <td colspan="3">
                    <label runat="server" id="labYQ"></label>
                </td>
            </tr>
            <tr>
                <td rowspan="2">验证结果</td>
                <td colspan="5">当日验证结果</td>
                <td colspan="5">次日验证结果</td>
            </tr>
            <tr>

                <%--  <td>
                      <telerik:RadGrid ID="gridSameDay" runat="server" GridLines="None"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridSameDay_NeedDataSource" OnItemDataBound="gridSameDay_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <HeaderStyle Width="40" />
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="均值" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="偏差" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>--%>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>均值</td>
                <td>偏差</td>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>均值</td>
                <td>偏差</td>
                <%--  <td colspan="2" rowspan="5">
                    <telerik:RadGrid ID="gridNextDay" runat="server" GridLines="None"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridNextDay_NeedDataSource" OnItemDataBound="gridNextDay_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <HeaderStyle Width="40" />
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="均值" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="偏差" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid></td>--%>
            </tr>
            <tr>
                <td>零气</td>
                <td>
                    <label runat="server" id="Label1"></label>
                </td>
                <td>
                    <label runat="server" id="Label2"></label>
                </td>
                <td>
                    <label runat="server" id="Label3"></label>
                </td>
                <td>
                    <label runat="server" id="Label4"></label>
                </td>
                <td>
                    <label runat="server" id="Label5"></label>
                </td>
                <td>
                    <label runat="server" id="Label6"></label>
                </td>
                <td>
                    <label runat="server" id="Label7"></label>
                </td>
                <td>
                    <label runat="server" id="Label8"></label>
                </td>
                <td>
                    <label runat="server" id="Label9"></label>
                </td>
                <td>
                    <label runat="server" id="Label10"></label>
                </td>
            </tr>
            <tr>
                <td>参考标准响应</td>
                <td>
                    <label runat="server" id="Label11"></label>
                </td>
                <td>
                    <label runat="server" id="Label12"></label>
                </td>
                <td>
                    <label runat="server" id="Label13"></label>
                </td>
                <td>
                    <label runat="server" id="Label14"></label>
                </td>
                <td>
                    <label runat="server" id="Label15"></label>
                </td>
                <td>
                    <label runat="server" id="Label16"></label>
                </td>
                <td>
                    <label runat="server" id="Label17"></label>
                </td>
                <td>
                    <label runat="server" id="Label18"></label>
                </td>
                <td>
                    <label runat="server" id="Label19"></label>
                </td>
                <td>
                    <label runat="server" id="Label20"></label>
                </td>
            </tr>
            <tr>
                <td>待测标准响应</td>
                <td>
                    <label runat="server" id="Label21"></label>
                </td>
                <td>
                    <label runat="server" id="Label22"></label>
                </td>
                <td>
                    <label runat="server" id="Label23"></label>
                </td>
                <td>
                    <label runat="server" id="Label24"></label>
                </td>
                <td>
                    <label runat="server" id="Label25"></label>
                </td>
                <td>
                    <label runat="server" id="Label26"></label>
                </td>
                <td>
                    <label runat="server" id="Label27"></label>
                </td>
                <td>
                    <label runat="server" id="Label28"></label>
                </td>
                <td>
                    <label runat="server" id="Label29"></label>
                </td>
                <td>
                    <label runat="server" id="Label30"></label>
                </td>
            </tr>
            <tr>
                <td>零验证浓度</td>
                <td>
                    <label runat="server" id="Label31"></label>
                </td>
                <td>
                    <label runat="server" id="Label32"></label>
                </td>
                <td>
                    <label runat="server" id="Label33"></label>
                </td>
                <td>
                    <label runat="server" id="Label34"></label>
                </td>
                <td>
                    <label runat="server" id="Label35"></label>
                </td>
                <td>
                    <label runat="server" id="Label36"></label>
                </td>
                <td>
                    <label runat="server" id="Label37"></label>
                </td>
                <td>
                    <label runat="server" id="Label38"></label>
                </td>
                <td>
                    <label runat="server" id="Label39"></label>
                </td>
                <td>
                    <label runat="server" id="Label40"></label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
