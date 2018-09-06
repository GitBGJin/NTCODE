<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NOXMovingInstrumentCalibrate.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.QualityControl.NOXMovingInstrumentCalibrate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .td {
            border: solid black 1px;
            background-color: #D9D9D9;
            height: 19px;
        }

        table {
            border: solid #000 1px;
            border-collapse: collapse;
            width: 99%;
        }

        .Lable {
            border: solid black 1px;
            height: 19px;
            text-align: center;
        }

        .text {
            border-style: none;
            text-align: center;
            width: 99%;
            height: 99%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <div>
            <p>
                <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" SkinID="ImgBtnSave" />
            </p>
            <table>
                <tbody>
                    <tr>
                        <td class="td" style="width: 10%">站点：</td>
                        <td class="Lable" style="width: 23%">
                            <label id="lbl1" runat="server" />
                        </td>
                        <td class="td" style="width: 10%">标准物质：</td>
                        <td class="Lable" style="width: 23%">
                            <label id="Label1" runat="server" />
                        </td>
                        <td class="td" style="width: 10%">室内温度：</td>
                        <td class="Lable" style="width: 23%">
                            <label id="Label2" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 10%">开始时间：</td>
                        <td class="Lable" style="width: 23%">
                            <label id="Label3" runat="server" />
                        </td>
                        <td class="td" style="width: 10%">结束时间：</td>
                        <td class="Lable" style="width: 23%">
                            <label id="Label4" runat="server" />
                        </td>
                        <td class="td" style="width: 10%">操作人：</td>
                        <td class="Lable" style="width: 23%">
                            <label id="Label5" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <tr>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="6">仪器信息</td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 10%">分析仪型</td>
                        <td class="Lable" style="width: 23%">
                            <label id="Label6" runat="server" />
                        </td>
                        <td class="td" style="width: 10%">设备编号</td>
                        <td class="Lable" style="width: 23%">
                            <label id="Label7" runat="server" />
                        </td>
                        <td class="td" style="width: 10%">上次校准日期</td>
                        <td class="Lable" style="width: 23%">
                            <label id="Label8" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <tr>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="8">校准器信息</td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 10%">校准器型号</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label9" runat="server" />
                        </td>
                        <td class="td" style="width: 10%">设备编号</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label10" runat="server" />
                        </td>
                        <td class="td" style="width: 10%">MFC校准日期</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label11" runat="server" />
                        </td>
                        <td class="td" style="width: 10%">臭氧发生器校准日期</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label12" runat="server" />
                        </td>
                    </tr>
                   <tr>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="8">流量修正</td>
                    </tr>
                    <tr>
                        <telerik:RadSplitter ID="RadSplitter2" runat="server" Orientation="Horizontal" Height="50px"  BorderWidth="0"
                            Width="99%">
                            <telerik:RadPane ID="RadPane2" runat="server" Width="100%" Height="10%" Scrolling="None"
                                BorderWidth="0" BorderStyle="None" BorderSize="0">
                                <telerik:RadGrid ID="grdFlowCorrect" runat="server" GridLines="None" Height="100%" Width="100%"
                                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                                    OnNeedDataSource="grdFlowCorrect_NeedDataSource" OnItemDataBound="grdFlowCorrect_ItemDataBound"
                                    CssClass="RadGrid_Customer">
                                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                        <Columns>
                                            <telerik:GridBoundColumn HeaderText="名称" UniqueName="ComProject" DataField="ComProject" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <telerik:GridBoundColumn HeaderText="斜率" UniqueName="RengentName" DataField="RengentName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <telerik:GridBoundColumn HeaderText="截距" UniqueName="RengentName" DataField="RengentName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </telerik:RadPane>
                        </telerik:RadSplitter>
                    </tr>
                </tbody>
            </table>

            <table>
                <tbody>
                    <tr>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="4">零气源信息</td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 25%">设备编号</td>
                        <td class="Lable" style="width: 25%">
                            <label id="Label13" runat="server" />
                        </td>
                        <td class="td" style="width: 25%">出口压力</td>
                        <td class="Lable" style="width: 25%">
                            <label id="Label14" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <tr>
                        <td class="td" style="text-align: center; font-size: 18px">钢瓶气信息</td>
                    </tr>
                    <tr>
                        <telerik:RadSplitter ID="SplitterRg" runat="server" Orientation="Horizontal" Height="10%" BorderWidth="0"
                            Width="99%">
                            <telerik:RadPane ID="paneRg" runat="server" Width="100%" Height="10%" Scrolling="None"
                                BorderWidth="0" BorderStyle="None" BorderSize="0">
                                <telerik:RadGrid ID="grdCylinderGasInfor" runat="server" GridLines="None" Height="100%" Width="100%"
                                    AllowPaging="True" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                                    OnNeedDataSource="grdCylinderGasInfor_NeedDataSource" OnItemDataBound="grdCylinderGasInfor_ItemDataBound"
                                    CssClass="RadGrid_Customer">
                                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                        <Columns>
                                            <telerik:GridBoundColumn HeaderText="钢瓶编号" UniqueName="ComProject" DataField="ComProject" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <telerik:GridBoundColumn HeaderText="瓶内压力" UniqueName="RengentName" DataField="RengentName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <telerik:GridBoundColumn HeaderText="填装效期" UniqueName="RengentName" DataField="RengentName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <telerik:GridBoundColumn HeaderText="有效日期" UniqueName="ComProject" DataField="ComProject" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <telerik:GridBoundColumn HeaderText="NO" UniqueName="RengentName" DataField="RengentName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <telerik:GridBoundColumn HeaderText="NOx" UniqueName="RengentName" DataField="RengentName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </telerik:RadPane>
                        </telerik:RadSplitter>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <tr>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="4">NO及NOx校准结果的线性相关分析</td>
                    </tr>
                    <tr>
                        <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal" Height="10%" BorderWidth="0"
                            Width="99%">
                            <telerik:RadPane ID="RadPane1" runat="server" Width="100%" Height="100%" Scrolling="None"
                                BorderWidth="0" BorderStyle="None" BorderSize="0">
                                <telerik:RadGrid ID="grdNOrNOxAnalyze" runat="server" GridLines="None" Height="100%" Width="100%"
                                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                                    OnNeedDataSource="grdNOrNOxAnalyze_NeedDataSource" OnItemDataBound="grdNOrNOxAnalyze_ItemDataBound"
                                    CssClass="RadGrid_Customer">
                                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">

                                        <ColumnGroups>
                                            <telerik:GridColumnGroup Name="we" HeaderText="we" HeaderStyle-HorizontalAlign="Center"/>
                                            <telerik:GridColumnGroup Name="NO" HeaderText="NO" HeaderStyle-HorizontalAlign="Center" ParentGroupName="we"/>
                                            <telerik:GridColumnGroup Name="NOx" HeaderText="NOx" HeaderStyle-HorizontalAlign="Center" ParentGroupName="we"/>
                                        </ColumnGroups>
                                        <Columns>
                                            <telerik:GridBoundColumn HeaderText="实际浓度" UniqueName="ComProject" DataField="ComProject" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO" />
                                            <telerik:GridBoundColumn HeaderText="仪器响应" UniqueName="RengentName" DataField="RengentName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO" />
                                            <telerik:GridBoundColumn HeaderText="实际浓度" UniqueName="RengentName" DataField="RengentName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NOx" />
                                            <telerik:GridBoundColumn HeaderText="仪器响应" UniqueName="ComProject" DataField="ComProject" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NOx" />
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </telerik:RadPane>
                        </telerik:RadSplitter>
                    </tr>

                </tbody>
            </table>
            <table>
                <tbody>
                       <tr>
                        <td class="td" style="width: 25%">斜率</td>
                        <td class="Lable" style="width: 25%">
                            <label id="Label21" runat="server" />
                        </td>
                        <td class="td" style="width: 25%">斜率</td>
                        <td class="Lable" style="width: 25%">
                            <label id="Label22" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 25%">截距</td>
                        <td class="Lable" style="width: 25%">
                            <label id="Label23" runat="server" />
                        </td>
                        <td class="td" style="width: 25%">截距</td>
                        <td class="Lable" style="width: 25%">
                            <label id="Label24" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 25%">相关系数</td>
                        <td class="Lable" style="width: 25%">
                            <label id="Label25" runat="server" />
                        </td>
                        <td class="td" style="width: 25%">相关系数</td>
                        <td class="Lable" style="width: 25%">
                            <label id="Label26" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                      <tr>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="6">钼炉转化率</td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 15%">NO设置浓度</td>
                        <td class="td" style="width: 15%">O3  开/关</td>
                        <td class="td" style="width: 15%">氮氧化物分析仪 resp/adj</td>
                        <td class="td" style="text-align: center;width: 55% " colspan="3">O3设置浓度</td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 15%" rowspan="11">满量程90%</td>
                        <td class="td" style="width: 15%" rowspan="4">关</td>
                          <td class="td" style="width: 15%" >[NO]resp</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label18" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label33" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label34" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 15%">[NOx]resp</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label20" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label35" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label36" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td class="td" style="width: 15%">[NOx]adj</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label15" runat="server" />
                        </td>
                          <td class="Lable" style="width: 15%">
                            <label id="Label37" runat="server" />
                        </td>
                          <td class="Lable" style="width: 15%">
                            <label id="Label38" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td class="td" style="width: 15%">[NOx]adj</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label16" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label39" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label40" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td class="td" style="width: 15%" rowspan="4">开</td>
                          <td class="td" style="width: 15%" >[NO]resp</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label17" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label41" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label42" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 15%">[NOx]resp</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label19" runat="server" />
                        </td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label43" runat="server" />
                        </td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label44" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td class="td" style="width: 15%">[NOx]adj</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label27" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label45" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label46" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td class="td" style="width: 15%">[NOx]adj</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label28" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label47" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label48" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td class="td" style="width: 15%" rowspan="3"></td>
                          <td class="td" style="width: 15%" >Delta[NO]</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label29" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label49" runat="server" />
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label50" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 15%">Delta[NOx]</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label30" runat="server" />
                        </td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label51" runat="server" />
                        </td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label52" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td class="td" style="width: 15%">转化率</td>
                        <td class="Lable" style="width: 15%">
                            <label id="Label31" runat="server" />X
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label53" runat="server" />Y
                        </td>
                         <td class="Lable" style="width: 15%">
                            <label id="Label54" runat="server" />Z
                        </td>
                    </tr>
                     <tr>
                        <td class="td" style="text-align: center;width: 15%" colspan="3">平均转化效率</td>
                        <td class="Lable" style="text-align: center;width: 15%" colspan="3">
                            <label id="Label32" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
