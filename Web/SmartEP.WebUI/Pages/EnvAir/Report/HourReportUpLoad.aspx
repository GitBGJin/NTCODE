<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HourReportUpLoad.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.HourReportUpLoad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>区、县数据上报</title>
</head>
<body>
    <form id="form1" runat="server">
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
                <telerik:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal" Height="100%" Width="100%">
            <telerik:RadPane ID="ToolBarRadPane" runat="server" Height="100px" Width="100%" Scrolling="None" BorderWidth="0">
                <table class="Table_Customer" style="height: 100%; width: 100%">
                    <tr>
                        <td class="title" style="width: 100px;">上报模板：
                        </td>
                        <td class="title" colspan="3" style="text-align: left;">
                            <a href="../../../Files/区县上报/区县上报数据导入模板.xls" style="color: red; font-weight: bolder; font-size: 16px;">区县上报数据导入模板.xls</a>
                        </td>
                        <td rowspan="3">
                            <div style="height: 100px; width: 100%; overflow: auto" runat="server" id="div1"></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 100px;">选择Excel文件：
                        </td>
                        <td class="content" style="width: 500px;" colspan="2">
                            <asp:FileUpload runat="server" ID="FileUpload1" Width="100%"></asp:FileUpload>
                        </td>
                        <td class="content" style="text-align: center;">
                            <asp:ImageButton runat="server" ID="btnUpLoad" SkinID="ImgBtnUpload" OnClick="btnUpLoad_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 100px;">监测点：
                        </td>
                        <td class="content" style="width: 400px;">
                            <telerik:RadComboBox runat="server" ID="RadPsSel" CheckBoxes="true" Width="100%"
                                EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="五局大院" Value="762" Checked="true" />
                                    <telerik:RadComboBoxItem Text="宜园" Value="764" Checked="true" />
                                    <telerik:RadComboBoxItem Text="虹桥邮政" Value="751" Checked="true" />
                                    <telerik:RadComboBoxItem Text="五星公园" Value="752" Checked="true" />
                                    <telerik:RadComboBoxItem Text="第二实验小学" Value="753" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td class="content" style="text-align: right; width: 100px">
                            <asp:ImageButton runat="server" ID="btnSearch" SkinID="ImgBtnSearch" OnClick="btnSearch_Click" />
                        </td>
                        <td class="content" style="text-align: center; width: 100px">
                            <asp:ImageButton runat="server" ID="btnSave" SkinID="ImgBtnSave" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="RadPane1" runat="server" Height="100%" Width="100%" Scrolling="None" BorderWidth="0">
                <telerik:RadGrid ID="RadGrid1" runat="server" GridLines="None" CssClass="RadGrid_Customer" Width="100%" Height="100%" BorderStyle="None"
                    AutoGenerateColumns="false" OnNeedDataSource="RadGrid1_NeedDataSource">
                    <MasterTableView GridLines="None" CommandItemDisplay="None" NoMasterRecordsText="没有数据">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row"
                                HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Container.DataSetIndex + 1%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="PointName" UniqueName="PointName" HeaderText="监测点名称"
                                HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Tstamp" UniqueName="Tstamp" HeaderText="日期" DataFormatString="{0:yyyy-MM-dd}"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Tstamp" UniqueName="Tstamp" HeaderText="小时" DataFormatString="{0:HH}"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="a21026" HeaderText="SO<sub>2</sub>" HeaderTooltip="123"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a21026"),Eval("a21026_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a21003" HeaderText="NO"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a21003"),Eval("a21003_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a21004" HeaderText="NO<sub>2</sub>"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a21004"),Eval("a21004_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a21002" HeaderText="NOx"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a21002"),Eval("a21002_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a34002" HeaderText="PM<sub>10</sub>"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a34002"),Eval("a34002_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a21005" HeaderText="CO"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a21005"),Eval("a21005_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a05024" HeaderText="O<sub>3</sub>"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a05024"),Eval("a05024_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a34004" HeaderText="PM<sub>2.5</sub>"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a34004"),Eval("a34004_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a01007" HeaderText="WS"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a01007"),Eval("a01007_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a01008" HeaderText="WD"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a01008"),Eval("a01008_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a01001" HeaderText="TEMP"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a01001"),Eval("a01001_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a01002" HeaderText="RH"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a01002"),Eval("a01002_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a01006" HeaderText="PRESS"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a01006"),Eval("a01006_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="a01020" HeaderText="VISIBILITY"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# GetValue(Eval("a01020"),Eval("a01020_Flag")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <Scrolling AllowScroll="true" EnableVirtualScrollPaging="false" UseStaticHeaders="true"
                            SaveScrollPosition="false"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
