<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZeroPointAndSpanCheck.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.QualityControl.ZeroPointAndSpanCheck" %>

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
        <table style="width: 80%; margin-left: 10%">
            <tr style="text-align: center; font-size: 30px">
                <td>气体分析仪零点/跨度检查与调节、精密度检查记录表</td>
            </tr>
        </table>
        <br />
        <table style="width: 80%; margin-left: 10%; border-collapse: collapse" class="tabContent">
            <tr>
                <td colspan="6">
                    <telerik:RadGrid ID="gridJBXX" runat="server" GridLines="None" Height="100%" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridJBXX_NeedDataSource" OnItemDataBound="gridJBXX_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn DataField="point" UniqueName="point" HeaderText="站点" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn DataField="indoorTemp" UniqueName="indoorTemp" HeaderText="室内温度" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn DataField="operator" UniqueName="operator" HeaderText="操作者" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn DataField="Tstamp" UniqueName="Tstamp" HeaderText="日期" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn DataField="dtBegin" UniqueName="dtBegin" HeaderText="开始时间" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn DataField="dtEnd" UniqueName="dtEnd" HeaderText="结束时间" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>

            </tr>

        </table>
        <table border="0" style="width: 80%; margin-left: 10%; text-align: center; border-collapse: collapse" class="tabContent">
            <tr>
                <td colspan="6">
                    <telerik:RadGrid ID="gridYQXX" runat="server" GridLines="None" Height="100%" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridYQXX_NeedDataSource" OnItemDataBound="gridYQXX_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="仪器信息" HeaderText="仪器信息"
                                    HeaderStyle-HorizontalAlign="Center" />
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="仪器类型" DataField="instrumentType" UniqueName="instrumentType" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="仪器信息" />
                                <telerik:GridBoundColumn HeaderText="仪器型号" DataField="instrumentModel" UniqueName="instrumentModel" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="仪器信息" />
                                <telerik:GridBoundColumn HeaderText="仪器编号" DataField="instrumentNum" UniqueName="instrumentNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="仪器信息" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <telerik:RadGrid ID="gridGPQXX" runat="server" GridLines="None" Height="100%" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridGPQXX_NeedDataSource" OnItemDataBound="gridGPQXX_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="钢瓶气信息" HeaderText="钢瓶气信息"
                                    HeaderStyle-HorizontalAlign="Center" />
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="钢瓶气类型" DataField="instrumentType" UniqueName="instrumentType" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="钢瓶气信息" />
                                <telerik:GridBoundColumn HeaderText="钢瓶编号" DataField="steelNum" UniqueName="steelNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="钢瓶气信息" />
                                <telerik:GridBoundColumn HeaderText="有效期" DataField="validityPeriod" UniqueName="validityPeriod" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="钢瓶气信息" />
                                <telerik:GridBoundColumn HeaderText="瓶内压力" DataField="steelPressure" UniqueName="steelPressure" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="钢瓶气信息" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <telerik:RadGrid ID="gridJZQXX" runat="server" GridLines="None" Height="100%" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridJZQXX_NeedDataSource" OnItemDataBound="gridJZQXX_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="校准器信息" HeaderText="校准器信息"
                                    HeaderStyle-HorizontalAlign="Center" />
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="校准器型号" DataField="calibratorModel" UniqueName="calibratorModel" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="校准器信息" />
                                <telerik:GridBoundColumn HeaderText="设备编号" DataField="equipmentNum" UniqueName="equipmentNum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="校准器信息" />
                                <telerik:GridBoundColumn HeaderText="MFC校准日期" DataField="MFCDate" UniqueName="MFCDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="校准器信息" />
                                <telerik:GridBoundColumn HeaderText="臭氧源校准日期" DataField="O3Date" UniqueName="O3Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="校准器信息" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <telerik:RadGrid ID="gridWRWTJ" runat="server" GridLines="None" Height="100%" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridWRWTJ_NeedDataSource" OnItemDataBound="gridWRWTJ_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="仪器零点响应" HeaderText="仪器零点响应"
                                    HeaderStyle-HorizontalAlign="Center" />
                                <telerik:GridColumnGroup Name="仪器跨度响应" HeaderText="仪器零点响应"
                                    HeaderStyle-HorizontalAlign="Center" />
                                <telerik:GridColumnGroup Name="截距" HeaderText="截距"
                                    HeaderStyle-HorizontalAlign="Center" />
                                <telerik:GridColumnGroup Name="斜率" HeaderText="斜率"
                                    HeaderStyle-HorizontalAlign="Center" />
                                <telerik:GridColumnGroup Name="增益" HeaderText="增益"
                                    HeaderStyle-HorizontalAlign="Center" />
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="污染物" DataField="pollutant" UniqueName="pollutant" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <telerik:GridBoundColumn HeaderText="零气" DataField="zeroGas" UniqueName="zeroGas" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="仪器零点响应" />
                                <telerik:GridBoundColumn HeaderText="调节前" DataField="zeroBeforeAdjustment" UniqueName="zeroBeforeAdjustment" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="仪器零点响应" />
                                <telerik:GridBoundColumn HeaderText="调节后" DataField="zeroAfterAdjustment" UniqueName="zeroAfterAdjustment" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="仪器零点响应" />
                                <telerik:GridBoundColumn HeaderText="跨度气" DataField="spanGas" UniqueName="spanGas" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="仪器跨度响应" />
                                <telerik:GridBoundColumn HeaderText="调节前" DataField="spanBeforeAdjustment" UniqueName="spanBeforeAdjustment" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="仪器跨度响应" />
                                <telerik:GridBoundColumn HeaderText="调节后" DataField="spanAfterAdjustment" UniqueName="spanAfterAdjustment" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="仪器跨度响应" />
                                <telerik:GridBoundColumn HeaderText="调节前" DataField="interceptBeforeAdjustment" UniqueName="interceptBeforeAdjustment" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="截距" />
                                <telerik:GridBoundColumn HeaderText="调节后" DataField="interceptAfterAdjustment" UniqueName="interceptAfterAdjustment" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="截距" />
                                <telerik:GridBoundColumn HeaderText="调节前" DataField="slopeBeforeAdjustment" UniqueName="slopeBeforeAdjustment" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="斜率" />
                                <telerik:GridBoundColumn HeaderText="调节后" DataField="slopeAfterAdjustment" UniqueName="slopeAfterAdjustment" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="斜率" />
                                <telerik:GridBoundColumn HeaderText="调节前" DataField="gainBeforeAdjustment" UniqueName="gainBeforeAdjustment" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="增益" />
                                <telerik:GridBoundColumn HeaderText="调节后" DataField="gainfterAdjustment" UniqueName="gainfterAdjustment" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="增益" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <%--        <tr>
                <td colspan="6">
                    <telerik:RadGrid ID="gridJMDJC" runat="server" GridLines="None" Height="100%" Width="100%"
                        AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                        OnNeedDataSource="gridJMDJC_NeedDataSource" OnItemDataBound="gridJMDJC_ItemDataBound"
                        CssClass="RadGrid_Customer">
                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="精密度检查结果" HeaderText="精密度检查结果"
                                    HeaderStyle-HorizontalAlign="Center" />
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="污染物" DataField="pollutant" UniqueName="pollutant" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="精密度检查结果" />
                                <telerik:GridBoundColumn HeaderText="检查气浓度" DataField="gasconcentration" UniqueName="gasconcentration" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="精密度检查结果" />
                                <telerik:GridBoundColumn HeaderText="仪器响应" DataField="instrumentresponse" UniqueName="instrumentresponse" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="精密度检查结果" />
                                <telerik:GridBoundColumn HeaderText="百分误差" DataField="percenterror" UniqueName="percenterror" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="精密度检查结果" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>--%>
            <tr>
                <td colspan="6" style="font-weight: bold; color: #444444; background-color: #f7f3f7; border-bottom: #5d8cc9 1px solid; border-left: #5d8cc9 1px solid; border-right: #5d8cc9 1px solid; padding: 5px 7px 4px 7px">精密度检查结果</td>
            </tr>
            <tr>
                <td colspan="2" style="font-weight: bold; color: #444444; background-color: #f7f3f7; border-bottom: #5d8cc9 1px solid; border-left: #5d8cc9 1px solid; border-right: #5d8cc9 1px solid; padding: 5px 7px 4px 7px">污染物</td>
                <td style="font-weight: bold; color: #444444; background-color: #f7f3f7; border-bottom: #5d8cc9 1px solid; border-left: #5d8cc9 1px solid; border-right: #5d8cc9 1px solid; padding: 5px 7px 4px 7px">检查气密度</td>
                <td colspan="2" style="font-weight: bold; color: #444444; background-color: #f7f3f7; border-bottom: #5d8cc9 1px solid; border-left: #5d8cc9 1px solid; border-right: #5d8cc9 1px solid; padding: 5px 7px 4px 7px">仪器响应</td>
                <td style="font-weight: bold; color: #444444; background-color: #f7f3f7; border-bottom: #5d8cc9 1px solid; border-left: #5d8cc9 1px solid; border-right: #5d8cc9 1px solid; padding: 5px 7px 4px 7px">百分误差</td>
            </tr>
            <tr>
                <td colspan="2">SO<sub>2</sub></td>
                <td></td>
                <td colspan="2"></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2">CO</td>
                <td></td>
                <td colspan="2"></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2">O<sub>3</sub></td>
                <td></td>
                <td colspan="2"></td>
                <td></td>
            </tr>
            <tr>
                <td rowspan="2">NO<sub>2</sub>
                </td>
                <td>NO</td>
                <td>
                    <label runat="server" id="labJMDNO"></label>
                </td>
                <td>O<sub>3关</sub><label runat="server" id="labNOO3Close"></label></td>
                <td>O<sub>3开</sub><label runat="server" id="labNOO3Open"></label></td>
                <td></td>
            </tr>
            <tr>
                <td>NO<sub>X</sub></td>
                <td>
                    <label runat="server" id="labJMDNOX"></label>
                </td>
                <td>O<sub>3关</sub><label runat="server" id="labNOXO3Close"></label></td>
                <td>O<sub>3开</sub><label runat="server" id="labNOXO3Open"></label></td>
                <td></td>
            </tr>
        </table>
        <%--     <table border="1" style="width: 80%; margin-left: 10%; text-align: center">
            <tr>
                <td colspan="5">仪器信息</td>
            </tr>
            <tr>
                <td>仪器类型</td>
                <td>SO<sub>2</sub></td>
                <td>CO</td>
                <td>O<sub>3</sub></td>
                <td>NO<sub>X</sub></td>
            </tr>
            <tr>
                <td>仪器型号</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>仪器编号</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>
        <br />
        <table border="1" style="width: 80%; margin-left: 10%; text-align: center">
            <tr>
                <td colspan="6">钢瓶气信息</td>
            </tr>
            <tr>
                <td>SO<sub>2</sub>钢瓶编号</td>
                <td></td>
                <td>SO<sub>2</sub>有效期</td>
                <td></td>
                <td>SO<sub>2</sub>瓶内压力</td>
                <td></td>
            </tr>
            <tr>
                <td>NO钢瓶编号</td>
                <td></td>
                <td>NO有效期</td>
                <td></td>
                <td>NO瓶内压力</td>
                <td></td>
            </tr>
            <tr>
                <td>CO钢瓶编号</td>
                <td></td>
                <td>CO有效期</td>
                <td></td>
                <td>CO瓶内压力</td>
                <td></td>
            </tr>
        </table>
        <br />
        <table border="1" style="width: 80%; margin-left: 10%; text-align: center">
            <tr>
                <td colspan="4">校准器信息</td>
            </tr>
            <tr>
                <td>校准器型号</td>
                <td></td>
                <td>设备编号</td>
                <td></td>
            </tr>
            <tr>
                <td>MFC校准日期</td>
                <td></td>
                <td>臭氧源校准日期</td>
                <td></td>
            </tr>
        </table>
        <br />
        <table border="1" style="width: 80%; margin-left: 10%; text-align: center">
            <tr>
                <td rowspan="2" colspan="2">污染物</td>
                <td colspan="3">仪器零点响应</td>
                <td colspan="3">仪器跨度响应</td>
                <td colspan="2">截距</td>
                <td colspan="2">斜率</td>
                <td colspan="2">增益</td>
            </tr>
            <tr>
                <td>零气</td>
                <td>调节前</td>
                <td>调节后</td>
                <td>跨度气</td>
                <td>调节前</td>
                <td>调节后</td>
                <td>调节前</td>
                <td>调节后</td>
                <td>调节前</td>
                <td>调节后</td>
                <td>调节前</td>
                <td>调节后</td>
            </tr>
            <tr>
                <td colspan="2">SO<sub>2</sub></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2">CO</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2">O<sub>3</sub></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td rowspan="2">NO<sub>2</sub></td>
                <td>NO</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>NO<sub>X</sub></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>
        <br />
        <table border="1" style="width: 80%; margin-left: 10%; text-align: center">
            <tr>
                <td>污染物</td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>--%>
    </form>
</body>
</html>
