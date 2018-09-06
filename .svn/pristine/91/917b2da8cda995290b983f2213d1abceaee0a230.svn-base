<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AroundInspect.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.AroundInspect" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        <telerik:RadSplitter ID="splitterWeek" runat="server" Orientation="Horizontal" Height="80%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 80%; height: 100%;" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="btnTitle">
                            <table style="width: 100%; display: none" class="Table_Customer">
                                <tr class="btnTitle">
                                    <td class="btnTitle">
                                        <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" SkinID="ImgBtnSave" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="title" style="width: 100px; text-align: center;">进站日期：
                        </td>
                        <td class="content" style="width: 360px;">
                            <telerik:RadDateTimePicker ID="dtpTime" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="220"
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
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定" Width="220"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            TimeView-HeaderText="小时" />
                                    </td>
                                    <td>&nbsp;&nbsp;至&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定" Width="220"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            TimeView-HeaderText="小时" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="80%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridAroundInspect" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="false" AllowCustomPaging="false" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="gridAroundInspect_NeedDataSource" OnItemDataBound="gridAroundInspect_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="RowGuid" NoMasterRecordsText="运维任务没有配置运维项目！">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="RowGuid" DataField="RowGuid" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="项目分类" UniqueName="UpItem" DataField="UpItem" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="监测项目" UniqueName="ItemName" DataField="ItemName" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="是" HeaderStyle-Width="40px" ItemStyle-Width="40px" UniqueName="yes" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBoxList ID="cblYes" runat="server">
                                        <asp:ListItem Value="1" Text=""></asp:ListItem>
                                    </asp:CheckBoxList>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="否" UniqueName="no" HeaderStyle-Width="40px" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBoxList ID="cblNo" runat="server">
                                        <asp:ListItem Value="0" Text=""></asp:ListItem>
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
                        </Columns>
                        <%--  <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>--%>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenMissionId" runat="server" Value="" />
        <asp:HiddenField ID="HiddenMissionName" runat="server" Value="" />
    </form>
</body>
</html>
