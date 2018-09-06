<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventProcessing.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.EventProcessing" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <script type="text/javascript">
        //保存业务表单数据 
        function SaveFormData(formID, taskCode, activityFlag, wfOpeType, formOpeType, pointGuid) {
            //formID表示即配置的业务表单的guid，已经具体到你们的页面，是否还有必要传？ 
            //taskID表示当前任务的标识，guid型；taskCode表示任务编号 
            //activityFlag表示步骤标识，传值为1、2、3，对应第一(任务填报)、二(任务初审)、三(任务复审)步 
            //wfOpeType表示工作流按钮操作标识，传值为1、2、3、4，对应保存、提交、退回、终止 
            //formOpeType表示表单展现方式，传值为1、2，对应编辑、明细 
            //pointGuid表示测点Guid

            //业务系统需要有返回值，返回1、0，表示成功、失败 
            //如果失败的话，需要记录日志到任务日志表中，平台提供接口，业务系统调用，这项不急，演示后在具体定 
            var buttonSave = document.getElementById("<%=btnSave.ClientID%>");
            buttonSave.click();
            return 1;
        }
    </script>

    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadSplitter ID="splitterWeek" runat="server" Orientation="Horizontal" Scrolling="Y" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 80%; height: 100%;" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="btnTitle">
                            <table style="width: 100%; display: block" class="Table_Customer">
                                <tr class="btnTitle">
                                    <td class="btnTitle">
                                        <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" SkinID="ImgBtnSave" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="title" style="width: 100px; text-align: center;">进站日期：
                        </td>
                        <td class="content" style="width: 200px;">
                            <telerik:RadDateTimePicker ID="dtpTime" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="200"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                        <td class="title" style="width: 180px; text-align: center;">维护/处理时间:
                        </td>
                        <td class="content" id="timeq">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定" Width="200"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            TimeView-HeaderText="小时" />
                                    </td>
                                    <td>&nbsp;&nbsp;至&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定" Width="200"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            TimeView-HeaderText="小时" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="400px" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div style="font: 20px;">一、进站事由</div>
                <telerik:RadGrid ID="gridAroundInspect" runat="server" GridLines="None" Height="90%" Width="100%"
                    AllowPaging="false" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="gridAroundInspect_NeedDataSource" OnItemDataBound="gridAroundInspect_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="RowGuid" NoMasterRecordsText="没有数据">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="RowGuid" DataField="RowGuid" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="监测项目" UniqueName="ItemName" DataField="ItemName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="是" UniqueName="yes" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBoxList ID="cblYes" runat="server">
                                        <asp:ListItem Value="1" Text=""></asp:ListItem>
                                    </asp:CheckBoxList>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="否" UniqueName="no" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBoxList ID="cblNo" runat="server">
                                        <asp:ListItem Value="0" Text=""></asp:ListItem>
                                    </asp:CheckBoxList>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="异常指标" UniqueName="Abnormal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBoxList ID="cblAbnormal" runat="server">
                                    </asp:CheckBoxList>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="维护日期" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="220px" HeaderStyle-Width="220px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <telerik:RadDateTimePicker ID="dtpGrid" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                        DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="200"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        TimeView-HeaderText="小时" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="备注" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <telerik:RadTextBox ID="rdtDescription" runat="server" Width="100%">
                                    </telerik:RadTextBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="具体异常" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <telerik:RadComboBox ID="rcbAbnormal" runat="server" Width="100%" Localization-CheckAllString="全选" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"></telerik:RadComboBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </telerik:RadPane>
            <telerik:RadPane ID="RadPane1" runat="server" Height="80px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div style="font: 20px;">二、检查结果</div>
                <table id="Table1" style="width: 100%; height: 90%;" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="btnTitle">
                            <asp:TextBox ID="beizhu" runat="server" MaxLength="500" Height="50px" Width="99%" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenMissionId" runat="server" Value="" />
        <asp:HiddenField ID="HiddenMissionName" runat="server" Value="" />
    </form>
</body>
</html>
