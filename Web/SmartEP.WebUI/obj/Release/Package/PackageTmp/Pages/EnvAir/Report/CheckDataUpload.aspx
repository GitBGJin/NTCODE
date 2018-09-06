<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckDataUpload.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.CheckDataUpload" %>

<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>考核基数数据导入</title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
            <script type="text/javascript">
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

                function onRequestEnd(sender, args) {
                }
            </script>
        </telerik:RadCodeBlock>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="FileUpload1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FileUpload1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal" Height="100%" Width="100%">
            <telerik:RadPane ID="ToolBarRadPane" runat="server" Height="100px" Width="100%" Scrolling="None" BorderWidth="0">
                <table class="Table_Customer" style="height: 100%; width: 100%">
                    <tr>
                        <td class="title" style="width: 100px;">上报模板：
                        </td>
                        <td class="title" colspan="4" style="text-align: left;">
                            <a href="../../../Files/考核基数数据导入/考核基数数据导入入模板.xlsx" style="color: red; font-weight: bolder; font-size: 16px;">考核基数数据导入模板.xlsx</a>
                        </td>
                        <td rowspan="3">
                            <div style="height: 100px; width: 100%; overflow: auto" runat="server" id="div1"></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 100px;">选择Excel文件：
                        </td>
                        <td class="content" style="width: 400px;">
                            <asp:FileUpload runat="server" ID="FileUpload1" Width="100%"></asp:FileUpload>
                        </td>
                        <td class="title" style="width: 100px;">考核基数类型：
                        </td>
                        <td class="title">
                            <asp:TextBox runat="server" ID="tbType"></asp:TextBox>
                        </td>
                        <td class="content" style="text-align: center; width: 100px">
                            <asp:ImageButton runat="server" ID="btnUpLoad" SkinID="ImgBtnUpload" OnClick="btnUpLoad_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 100px;">监测点：
                        </td>
                        <td class="content" style="width: 400px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" DefaultAllSelected="true" CbxWidth="400" CbxHeight="450" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="title" style="width: 100px;">考核基数类型：
                        </td>
                        <td>
                            <telerik:RadDropDownList runat="server" ID="ddlType" Width="150px"></telerik:RadDropDownList>
                        </td>
                        <td class="content" style="text-align: center; width: 100px">
                            <asp:ImageButton runat="server" ID="btnSearch" SkinID="ImgBtnSearch" OnClick="btnSearch_Click" />
                        </td>
                        <%-- <td class="content" style="text-align: center; width: 100px">--%>
                        <%-- <asp:ImageButton runat="server" ID="btnSave" SkinID="ImgBtnSave" OnClick="btnSave_Click" />--%>
                        <%--</td>--%>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="RadPane1" runat="server" Height="100%" Width="100%" Scrolling="None" BorderWidth="0">
                <telerik:RadGrid ID="RadGrid1" runat="server" GridLines="None" CssClass="RadGrid_Customer" Width="100%" Height="100%" BorderStyle="None"
                    AutoGenerateColumns="false" OnNeedDataSource="RadGrid1_NeedDataSource" AllowPaging="true" PageSize="24" OnItemDataBound="RadGrid1_ItemDataBound">
                    <MasterTableView GridLines="None" CommandItemDisplay="None" NoMasterRecordsText="没有数据">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row"
                                HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Container.DataSetIndex + 1%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="PointName" UniqueName="PointName" HeaderText="监测点名称"
                                HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DataType" UniqueName="DataType" HeaderText="考核基数类型"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DateTime" UniqueName="DateTime" HeaderText="日期"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SO2" UniqueName="SO2" HeaderText="SO<sub>2</sub>"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NO2" UniqueName="NO2" HeaderText="NO<sub>2</sub>"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PM10" UniqueName="DataType" HeaderText="PM<sub>10</sub>"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CO" UniqueName="DataType" HeaderText="CO"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MaxOneHourO3" UniqueName="MaxOneHourO3" HeaderText="O<sub>3</sub>_1h"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Max8HourO3" UniqueName="Max8HourO3" HeaderText="O<sub>3</sub>_8H"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PM25" UniqueName="PM25" HeaderText="PM<sub>2.5</sub>"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AQIValue" UniqueName="AQIValue" HeaderText="AQI"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PrimaryPollutant" UniqueName="PrimaryPollutant" HeaderText="主要污染物"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Class" UniqueName="Class" HeaderText="等级"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
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
